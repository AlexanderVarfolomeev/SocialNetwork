using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;
using SocialNetwork.WebAPI;
using SocialNetwork.WebAPI.Configuration;
using SocialNetwork.WebAPI.Configuration.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var services = builder.Services;
var settings = new AppSettings(new SettingSource());

builder.AddAppSerilog();

services.AddAppDbContext(settings);
services.AddHttpContextAccessor();
services.AddAppServices();
services.AddControllers().AddAppValidator();
services.AddAppAuth(settings);
services.AddAppHealthChecks();
services.AddAppVersioning();

services.AddAppSwagger(settings);
services.AddAppAutomapper();
services.AddAppCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAppDbContext();
app.UseAppMiddlewares();
app.UseAppAuth();
app.UseAppCors();
app.UseAppHealthChecks();


app.UseAppSerilog();
app.UseAppSwagger();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();