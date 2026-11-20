using TicketeraLab10.Api.Configuration;
using TicketeraLab10.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using TicketeraLab10.Domain.Entities;
using TicketeraLab10.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();
if (app.Configuration.GetValue<bool>("Database:Initialize"))
{
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<TicketeraDbContext>();

    await db.Database.EnsureCreatedAsync();

    if (!await db.Roles.AnyAsync(role => role.RoleName == "Usuario"))
    {
        db.Roles.Add(new Role
        {
            RoleId = Guid.NewGuid(),
            RoleName = "Usuario"
        });

        await db.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();