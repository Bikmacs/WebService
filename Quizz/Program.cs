using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quizz.App.Infrastructure.Context;
using Quizz.App.Domain.Models.Services;
using Quizz.App.Domain.Models.Services.AuthService;
using Quizz.App.Domain.Models.Services.BookService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Загружаем настройки JWT из `appsettings.json`
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing"));

// Добавляем сервисы CORS (чтобы API можно было использовать из браузера)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Настраиваем аутентификацию с JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // В проде лучше включить
        options.SaveToken = true; // Разрешаем сохранение токена в HTTP-контексте
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing"))
            ),
            ValidateIssuerSigningKey = true,
        };
    });

// Добавляем контроллеры
builder.Services.AddControllers();

// Добавляем поддержку Swagger и указываем, что API использует JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    // Добавляем схему безопасности (JWT в заголовке Authorization)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите токен в формате: Bearer {your_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Применяем схему безопасности ко всем API
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Подключаем базу данных MySQL
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 25))));

// Регистрируем сервисы аутентификации и работы с книгами
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAddBooksService, AddBooksService>();

var app = builder.Build();

// Применяем миграции при старте приложения
using (var serviceScope = app.Services.CreateScope())
{
    var dbcontext = serviceScope.ServiceProvider.GetService<ApplicationContext>();
    dbcontext?.Database.Migrate();
}

// Настройка middleware
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseRouting();

// Добавляем поддержку аутентификации и авторизации
app.UseAuthentication(); // Добавляем проверку JWT
app.UseAuthorization(); 


// Настройка Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Доступен по корню `http://localhost:5000/`
    });
}

app.MapControllers(); // Подключаем контроллеры

app.Run();
