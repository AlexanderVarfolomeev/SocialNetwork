using Duende.IdentityServer.Services;
using SocialNetwork.IdentityServer.Configuration;
using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var settings = new AppSettings(new SettingSource());
builder.AddAppSerilog();
//services.AddRepository();

//services.AddTransient<IProfileService, ProfileService>();

services.AddAppDbContext(settings.Db);
// Add services to the container.
services.AddTwitterCors();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddAppIdentity();
services.AddSwaggerGen();

var app = builder.Build();
app.UseAppSerilog();

app.UseTwitterCors();

app.UseHttpsRedirection();

app.UseAppIdentity();

app.MapControllers();

app.Run();