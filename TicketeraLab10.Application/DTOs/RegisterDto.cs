using System.ComponentModel.DataAnnotations;

namespace TicketeraLab10.Application.DTOs;

public class RegisterDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
}