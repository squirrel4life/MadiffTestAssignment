using MadiffTestAssignment.Config;
using MadiffTestAssignment.Exceptions;
using MadiffTestAssignment.Filters;
using MadiffTestAssignment.Middlewares;
using MadiffTestAssignment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

public partial class Program
{
    public static void Main(string[] args)
    {
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
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errorMessages = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new ValidationException(
                    errorMessages.Count != 0
                        ? string.Join("; ", errorMessages)
                        : "U³omne zapytanie");
            };
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
            c.OperationFilter<SwaggerResponseDescriptionFilter>();
        });

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Card Actions API v1");
        });

        app.UseRouting();
        app.MapControllers();
        app.Run();
    }
}