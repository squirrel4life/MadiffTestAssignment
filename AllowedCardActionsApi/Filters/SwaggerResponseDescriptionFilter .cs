using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MadiffTestAssignment.Filters
{
    public class SwaggerResponseDescriptionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.TryGetValue("400", out var badRequest))
                badRequest.Description = "Brak dozwolonych akcji";

            if (operation.Responses.TryGetValue("404", out var notFound))
                notFound.Description = "Nie znaleziono użytkownika lub karty";

            if (operation.Responses.TryGetValue("422", out var unprocessable))
                unprocessable.Description = "Wprowadzono błędne dane";

            if (operation.Responses.TryGetValue("500", out var serverError))
                serverError.Description = "Błąd serwera";
        }
    }
}
