using TicketeraLab10.Application.DTOs;
using TicketeraLab10.Application.Interfaces;
using TicketeraLab10.Application.UseCases.Auth;
using TicketeraLab10.Domain.Entities;
using TicketeraLab10.Domain.Interfaces;

namespace TicketeraLab10.Infrastructure.UseCases.Auth;

public class RegisterUseCase : IRegisterUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwt;

    public RegisterUseCase(IUnitOfWork unitOfWork, IJwtTokenGenerator jwt)
    {
        _unitOfWork = unitOfWork;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> ExecuteAsync(RegisterDto dto)
    {
        var users = _unitOfWork.Repository<User>();

        var existing = await users.FindAsync(u => u.Username == dto.Username);
        if (existing.Any())
            throw new InvalidOperationException("El nombre de usuario ya existe.");

        var role = (await _unitOfWork.Repository<Role>()
                       .FindAsync(r => r.RoleName == "Usuario")).FirstOrDefault()
                   ?? throw new InvalidOperationException("No existe el rol 'Usuario'. Ejecuta el script de roles.");

        // CORRECCIÓN: fecha sin Kind=UTC para que Npgsql la acepte en 'timestamp without time zone'
        var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = dto.Username,
            PasswordHash = dto.Password,
            Email = dto.Email,
            CreatedAt = now // CORRECCIÓN
        };

        await users.AddAsync(user);
        await _unitOfWork.Repository<UserRole>().AddAsync(new UserRole
        {
            UserId = user.UserId,
            RoleId = role.RoleId,
            AssignedAt = now // CORRECCIÓN
        });

        await _unitOfWork.SaveChangesAsync();

        var roles = new List<string> { role.RoleName };
        return new AuthResponseDto
        {
            Token = _jwt.GenerateToken(user.UserId, user.Username, roles),
            Username = user.Username,
            Roles = roles
        };
    }
}