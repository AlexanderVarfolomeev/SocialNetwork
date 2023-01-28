using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;
using SocialNetwork.WebAPI;
using SocialNetwork.WebAPI.Configuration;

var settings = new AppSettings(new SettingSource());
var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var services = builder.Services;
// Add services to the container.
builder.AddAppSerilog();

services.AddAppDbContext(settings);
services.AddAppServices();
services.AddControllers();
services.AddAppAuth(settings);
services.AddAppVersioning();

services.AddAppSwagger(settings);
services.AddAppAutomapper();
services.AddAppCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAppMiddlewares();
app.UseAppAuth();
app.UseAppCors();

app.UseAppSerilog();
app.UseAppSwagger();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();