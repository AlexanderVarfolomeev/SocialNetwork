using Duende.IdentityServer.Services;
using Microsoft.IdentityModel.Logging;
using SocialNetwork.IdentityServer.Configuration;
using SocialNetwork.IdentityServer.Configuration.HealthChecks;
using SocialNetwork.Repository;
using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var settings = new AppSettings(new SettingSource());
IdentityModelEventSource.ShowPII = true;

builder.AddAppSerilog();
services.AddRepository();

services.AddTransient<IProfileService, ProfileService>();

services.AddAppDbContext(settings.Db);
// Add services to the container.
services.AddAppCors();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddAppIdentity();
services.AddSwaggerGen();
services.AddAppHealthChecks();
var app = builder.Build();
app.UseAppSerilog();

app.UseAppCors();

app.UseHttpsRedirection();

app.UseAppHealthChecks();

app.UseAppIdentity();

app.MapControllers();

app.Run();