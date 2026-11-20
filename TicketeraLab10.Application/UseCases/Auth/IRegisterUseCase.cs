using TicketeraLab10.Application.DTOs;

namespace TicketeraLab10.Application.UseCases.Auth;

public interface IRegisterUseCase
{
    Task<AuthResponseDto> ExecuteAsync(RegisterDto dto);
}