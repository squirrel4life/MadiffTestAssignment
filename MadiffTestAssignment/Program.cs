using MadiffTestAssignment.Config;
using MadiffTestAssignment.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ActionRulesConfig>()
    .BindConfiguration("ActionRules")
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSingleton<ICardService, CardService>();
builder.Services.AddSingleton<ICardActionRegistry, CardActionRegistry>();
builder.Services.AddSingleton<IAllowedActionsGenerator, AllowedActionsGenerator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Card Actions API",
        Version = "v1"
    });
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Card Actions API v1");
});

app.UseRouting();
app.MapControllers();
app.Run();
