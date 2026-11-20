using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketeraLab10.Application.Interfaces;
using TicketeraLab10.Application.UseCases.Auth;
using TicketeraLab10.Domain.Interfaces;
using TicketeraLab10.Infrastructure.Data;
using TicketeraLab10.Infrastructure.Repositories;
using TicketeraLab10.Infrastructure.Services;
using TicketeraLab10.Infrastructure.UseCases.Auth;

namespace TicketeraLab10.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Conexión a la base de datos
        services.AddDbContext<TicketeraDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Registro de servicios
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRegisterUseCase, RegisterUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();

        return services;
    }
}