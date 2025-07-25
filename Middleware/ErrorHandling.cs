using System.Net;
using System.Text.Json;

namespace LibrarySystem.Middleware
{
    public class ErrorHandling
    {
        private readonly RequestDelegate next;

        public ErrorHandling(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = "";

            switch (exception)
            {
                case ArgumentNullException ex:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { message = ex.Message });
                    break;

                case ArgumentException ex:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { message = ex.Message });
                    break;
                case KeyNotFoundException ex:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { message = ex.Message });
                    break;

                default:
                    result = JsonSerializer.Serialize(new { message = "An unexpected error occurred." });
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
