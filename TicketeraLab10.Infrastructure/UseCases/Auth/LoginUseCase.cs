using TicketeraLab10.Application.DTOs;
using TicketeraLab10.Application.Interfaces;
using TicketeraLab10.Application.UseCases.Auth;
using TicketeraLab10.Domain.Entities;
using TicketeraLab10.Domain.Interfaces;

namespace TicketeraLab10.Infrastructure.UseCases.Auth;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwt;

    public LoginUseCase(IUnitOfWork unitOfWork, IJwtTokenGenerator jwt)
    {
        _unitOfWork = unitOfWork;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> ExecuteAsync(LoginDto dto)
    {
        var user = (await _unitOfWork.Repository<User>()
            .FindAsync(u => u.Username == dto.Username)).FirstOrDefault();

        if (user is null || user.PasswordHash != dto.Password)
            throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");

        var userRoles = await _unitOfWork.Repository<UserRole>()
            .FindAsync(ur => ur.UserId == user.UserId);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        var roles = (await _unitOfWork.Repository<Role>()
                .FindAsync(r => roleIds.Contains(r.RoleId)))
            .Select(r => r.RoleName)
            .ToList();

        return new AuthResponseDto
        {
            Token = _jwt.GenerateToken(user.UserId, user.Username, roles),
            Username = user.Username,
            Roles = roles
        };
    }
}