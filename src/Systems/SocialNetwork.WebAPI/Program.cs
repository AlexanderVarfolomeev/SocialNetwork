using Microsoft.IdentityModel.Logging;
using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;
using SocialNetwork.WebAPI;
using SocialNetwork.WebAPI.Configuration;
using SocialNetwork.WebAPI.Configuration.HealthChecks;
using SocialNetwork.WebAPI.Hubs.MessengerHub;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var services = builder.Services;
var settings = new AppSettings(new SettingSource());
IdentityModelEventSource.ShowPII = true;

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

services.AddSignalR();

var app = builder.Build();

app.UseAppDbContext();

app.UseAppMiddlewares();

app.UseAppAuth();

app.UseAppCors();

app.UseAppHealthChecks();

app.UseAppSerilog();

app.UseAppSwagger();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MessengerHub>("/Chat");

app.Run();