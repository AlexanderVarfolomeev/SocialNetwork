using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;
using SocialNetwork.WebAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
builder.AddAppSerilog();

services.AddAppDbContext(new AppSettings(new SettingSource()));

services.AddControllers();

services.AddAppVersioning();

services.AddAppSwagger();
services.AddAppCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAppSerilog();
app.UseAppSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();