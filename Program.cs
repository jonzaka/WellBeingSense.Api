using Microsoft.EntityFrameworkCore;
using WellBeingSense.Api.Data;
using WellBeingSense.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<RiskEvaluationService>();

var app = builder.Build();

// Cria o banco automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

//  Swagger ATIVADO independentemente do ambiente (DEV ou PROD)
app.UseSwagger();
app.UseSwaggerUI();

//  deixe HTTPS redirection desativado para rodar no VS Code
// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
