using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                var error = new {
                    Id = errorId,
                    Message = "Something went wrong!"
                    };
                // Log the exception
                _logger.LogError(ex, $"{errorId}: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(error);
        }
        }
    }
}
