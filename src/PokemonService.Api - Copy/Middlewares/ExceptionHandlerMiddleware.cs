using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PokemonService.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine(JsonConvert.SerializeObject(SimplifyException(ex)));
            }
        }

        private object SimplifyException(Exception ex) => new
        {
            ExceptionType = ex.GetType().Name,
            ex.Message,
            ex.StackTrace,
            InnerException = ex.InnerException == null ? null : SimplifyException(ex.InnerException)
        };
    }

    public static class ExceptionHandlerExtension
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            return app;
        }
    }
}
