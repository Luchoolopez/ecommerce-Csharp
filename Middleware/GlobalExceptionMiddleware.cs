using System.Net;
using System.Text.Json;

namespace EcommerceStore.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;


        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;//500

            var mensajeMinuscula = exception.Message.ToLower();

            if (mensajeMinuscula.Contains("no encontrado"))
            {
                statusCode = (int)HttpStatusCode.NotFound;//404

            }
            else if (mensajeMinuscula.Contains("invalido"))
            {
                statusCode = (int)HttpStatusCode.BadRequest;//400
            }
            context.Response.StatusCode = statusCode;

            var response = new
            {
                exito = false,
                mensaje = exception.Message,
                data = (Object)null
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
