using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eumel.Dj.WebServer
{
    public class SwaggerUserTokenHeader : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Constants.UserToken,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
        }
    }
}