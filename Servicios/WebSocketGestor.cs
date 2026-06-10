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

        if (grupo.TryRemove(usuarioId, out var oldSocket))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    if (oldSocket.State == WebSocketState.Open)
                    {
                        // Le avisamos al cliente viejo por qué lo estamos echando
                        await oldSocket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure, 
                            "Se inició sesión en otro dispositivo.", 
                            CancellationToken.None
                        );
                    }
                }
                catch (Exception) { /* Ignorar si ya estaba roto */ }
                finally
                {
                    oldSocket.Dispose(); // Liberamos la RAM del socket viejo obligatoriamente
                }
            });
        }

        grupo.TryAdd(usuarioId, socket);

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
                return new NotFound($"algo salio mal {ex.Message}");
            }
            finally
            {
                _semaforoEnvio.Release();
            }
        }

        return new Success();
    }

    // EENVIAR MENSAJE ESPESIFICO GRUPO 
    public async Task<IResultadoServicio> EnviarMensajeUserEspesificoAsync(string rol, string idUsuario, object mensaje)
    {
        // Buscamos si el usuario está actualmente online
        if (!_grupos.TryGetValue(rol, out var grupo)) return new NotFound("No se encontro el grupo"); 
        if (!grupo.TryGetValue(idUsuario, out var socket)) return new NotFound("No se encontro el grupo"); 


        if (socket.State != WebSocketState.Open) return new NotFound("No esta abierto el socket");

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
            return new NotFound($"algo salio mal {ex.Message}");
        }
        finally
        {
            _semaforoEnvio.Release();
        }
        

        return new Success();
    }
}