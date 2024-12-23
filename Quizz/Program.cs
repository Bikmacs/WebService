using Microsoft.EntityFrameworkCore;
using Quizz;
using Quizz.App.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);

var serviceScope = app.Services.CreateScope();
var dbcontext = serviceScope.ServiceProvider.GetService<ApplicationContext>();
dbcontext?.Database.Migrate();


app.Run();