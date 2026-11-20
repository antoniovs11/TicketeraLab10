namespace TicketeraLab10.Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public List<String> Roles { get; set; } = new();
}