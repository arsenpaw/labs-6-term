using Lab2.Abstractions;
using Lab2.Data;
using Lab2.Infrastructure;
using Lab2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab2 API", Version = "v1" }));

builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameSessionService, GameSessionService>();
builder.Services.AddScoped<ISessionLogService, SessionLogService>();
builder.Services.AddDbContext<AppDbContext>((sp, opt) =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
       .AddInterceptors(sp.GetRequiredService<AuditInterceptor>()));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
using var appContext = services.GetRequiredService<AppDbContext>();
try
{
    appContext.Database.Migrate();
}
catch (Exception ex)
{
    var writer = Console.Error;
    writer.WriteLine(ex);
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab2 API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<Lab2.Middleware.ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
