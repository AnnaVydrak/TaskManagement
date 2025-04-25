using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Core.Common.Exceptions
{
    public static class ExceptionHandlingConfiguration
    {
        public static void ConfigureExceptionHandling(WebApplication app)
        {
            app.UseExceptionHandler(builder =>
                builder.Run(async context => {
                    var ex = context.Features.Get<IExceptionHandlerFeature>().Error;
                    var problem = ex switch
                    {
                        InvalidStatusTransitionException _ => new ProblemDetails { Status = 400, Title = ex.Message },
                        _ => new ProblemDetails { Status = 500, Title = "An unexpected error occurred." }
                    };
                    context.Response.StatusCode = problem.Status.Value;
                    await context.Response.WriteAsJsonAsync(problem);
                })
            );
        }
    }
}
