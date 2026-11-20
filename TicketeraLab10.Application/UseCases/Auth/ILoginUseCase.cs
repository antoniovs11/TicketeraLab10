using TicketeraLab10.Application.DTOs;

namespace TicketeraLab10.Application.UseCases.Auth;

public interface ILoginUseCase
{
    Task<AuthResponseDto> ExecuteAsync(LoginDto dto);
}