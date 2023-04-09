using SocialNetwork.Settings.Settings;
using SocialNetwork.Settings.Source;
using SocialNetwork.Worker;
using SocialNetwork.Worker.Configuration;
using SocialNetwork.Worker.TaskExecutor;

var builder = WebApplication.CreateBuilder(args);
var settings = new AppSettings(new SettingSource());
// Add services to the container.

builder.AddAppLogger();

// Configure services
var services = builder.Services;

services.AddHttpContextAccessor();

services.AddAppDbContext(settings);
    
services.AddAppHealthChecks();

services.RegisterAppServices();


// Configure the HTTP request pipeline.

var app = builder.Build();

app.UseAppHealthChecks();


// Start task executor

app.Services.GetRequiredService<ITaskExecutor>().Start();


// Run app

app.Run();