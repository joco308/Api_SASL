using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using Api_SASL.Servicios.InterfazServicios;
using System.Text.Json;


namespace Api_SASL.Servicios;


public class WebSocketGestor
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>> _grupos = new();

    private readonly SemaphoreSlim _semaforoEnvio = new(1, 1);


    // REGISTRAR USUARIO
    public IResultadoServicio AgregarConexion(string usuarioId, string rol, WebSocket socket)
    {
        var grupo = _grupos.GetOrAdd(rol, _ => new ConcurrentDictionary<string, WebSocket>());

        grupo.AddOrUpdate(usuarioId, socket, (key, oldSocket) => socket);
        return new Success();
    }


    // ELIMINAR USUARIO (Cuando se va del sistema)
    public async Task<IResultadoServicio> EliminarConexionAsync(string usuarioId, string rol)
    {
        if(!_grupos.TryGetValue(rol, out var grupo))
        {
            return new NotFound("No se encontro el grupo por ese rol");
        }

        if (grupo.TryRemove(usuarioId, out var socket))
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Desconectado por el gestor", CancellationToken.None);
            }
            socket.Dispose(); // Liberamos la memoria RAM que usaba ese socket
            return new Success();
        }
        return new ValidationError("No se pudo eliminar algo salio mal no se encontro al usuario");
    }

    // ENVIAR MENSAJE PRIVADO 
    public async Task<IResultadoServicio> EnviarMensajeGrupoAsync(string rol, object mensaje)
    {
        // Buscamos si el usuario está actualmente online
        if (!_grupos.TryGetValue(rol, out var grupo)) return new NotFound("No se encontro el grupo"); 


        foreach(var user in grupo)
        {
            var socket = user.Value;
            if (socket.State != WebSocketState.Open){continue;}

            string jsonTexto = JsonSerializer.Serialize(mensaje);
            var bytes = Encoding.UTF8.GetBytes(jsonTexto);
            var buffer = new ArraySegment<byte>(bytes, 0, bytes.Length);

            await _semaforoEnvio.WaitAsync();
            try
            {
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                return new NotFound($"algo salio mal {ex}");
            }
            finally
            {
                _semaforoEnvio.Release();
            }
        }

        return new Success();
    }
}