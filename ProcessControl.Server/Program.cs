using Microsoft.EntityFrameworkCore;
using ProcessControl.API.Middleware;
using ProcessControl.Application.Interfaces;
using ProcessControl.Application.Services;
using ProcessControl.Infrastructure.Persistence;
using ProcessControl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProcessoService, ProcessoService>();
builder.Services.AddScoped<IHistoricoProcessoService, HistoricoProcessoService>();

// Configura o AutoMapper para escanear os perfis de mapeamento no assembly da camada de Aplicação
builder.Services.AddAutoMapper(typeof(ProcessoService).Assembly);

// Add services to the container.
builder.Services.AddDbContext<ProcessControl.Infrastructure.Persistence.ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
