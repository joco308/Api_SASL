using System.ComponentModel.DataAnnotations;

namespace Api_SASL.DTOs;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Mensaje { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public bool Exito { get; set; }
}

public class Verificar2FARequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Length(6, 6, ErrorMessage = "El código debe tener exactamente 6 dígitos")]
    public string Codigo { get; set; } = string.Empty;
}

public class Verificar2FAResponse
{
    public string Mensaje { get; set; } = string.Empty;
    public bool Valido { get; set; }
    public string? Token { get; set; }
}