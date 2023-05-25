using Web.API.Configuration;

var builder = WebApplication.CreateBuilder();

builder.ConfigureAppServices();

var app = builder.Build();

app.ConfigureRequestPipeline();

app.Run();
