using Microsoft.EntityFrameworkCore;
using Quizz.App.Infrastructure.Context;
using Microsoft.OpenApi.Models;
using Quizz.App.Domain.Models.Services;
using Quizz.App.Domain.Models.Services.AuthService;
using Quizz.App.Domain.Models.Services.BookService;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000"); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});



builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 25))));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAddBooksService, AddBooksService>();

var app = builder.Build();

// Применение миграций------------------------------------------------------
using (var serviceScope = app.Services.CreateScope())
{
    var dbcontext = serviceScope.ServiceProvider.GetService<ApplicationContext>();
    dbcontext?.Database.Migrate();
}
//---------------------------------------------------------------------------

// Настройка конвейера HTTP
app.UseCors("AllowAllOrigins");

//swager---------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
//---------------------------------------------------------------------------

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();