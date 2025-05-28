using ApiAvaliacao.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Porta onde o servidor Node.js está rodando
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AvaliacaoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowOrigin"); // Aplicar a política de CORS
app.UseAuthorization();
app.MapControllers();

app.Run();
