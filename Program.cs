using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi;
using TodoApi.Models;
using TodoApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Регистрация UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 2. JWT авторизация
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

// 3. Подключение к PostgreSQL
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Контроллеры
builder.Services.AddControllers();

// 5. OpenAPI
builder.Services.AddOpenApi();

// 6. ➕ CORS (разрешаем запросы из браузера)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 7. ➕ Статические файлы (чтобы открывать index.html)
app.UseDefaultFiles();   // позволяет открывать index.html по умолчанию
app.UseStaticFiles();    // отдаёт файлы из папки wwwroot

// 8. ➕ CORS (должен быть между UseRouting и UseAuthentication)
app.UseCors("AllowAll");

// 9. Аутентификация и авторизация
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();