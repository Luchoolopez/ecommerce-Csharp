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

            // 1. Empezamos asumiendo que es un error del servidor (500)
            var statusCode = (int)HttpStatusCode.InternalServerError;

            // 2. Evaluamos EL TIPO de excepción para asignar el código HTTP correcto
            switch (exception)
            {
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // 404
                    break;

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized; // 401
                    break;

                case ArgumentException:
                case InvalidOperationException:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
            }

            var mensajeMinuscula = exception.Message.ToLower();
            if (statusCode == 500 && mensajeMinuscula.Contains("invalido"))
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                exito = false,
                mensaje = exception.Message,
                data = (object)null
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}

