using LibraryManagementApi.Exceptions;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace LibraryManagementApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Path = context.Request.Path
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    response.StatusCode = validationEx.StatusCode;
                    errorResponse.StatusCode = validationEx.StatusCode;
                    errorResponse.ErrorCode = validationEx.ErrorCode;
                    errorResponse.Message = validationEx.Message;
                    errorResponse.ValidationErrors = validationEx.ValidationErrors;
                    break;

                case DatabaseConnectionException dbEx:
                    response.StatusCode = dbEx.StatusCode;
                    errorResponse.StatusCode = dbEx.StatusCode;
                    errorResponse.ErrorCode = dbEx.ErrorCode;
                    errorResponse.Message = _env.IsDevelopment()
                        ? dbEx.Message
                        : "Database is temporarily unavailable. Please try again later.";
                    break;

                case BaseException baseEx:
                    response.StatusCode = baseEx.StatusCode;
                    errorResponse.StatusCode = baseEx.StatusCode;
                    errorResponse.ErrorCode = baseEx.ErrorCode;
                    errorResponse.Message = baseEx.Message;
                    break;

                case SqlException sqlEx when sqlEx.Message.Contains("network-related") ||
                                             sqlEx.Message.Contains("Local Database Runtime") ||
                                             sqlEx.Message.Contains("server was not found"):
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorCode = "DATABASE_CONNECTION_ERROR";
                    errorResponse.Message = _env.IsDevelopment()
                        ? $"Database connection failed: {sqlEx.Message}"
                        : "Unable to connect to the database. Please try again later.";
                    break;

                case SqlException sqlEx:
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorCode = "DATABASE_ERROR";
                    errorResponse.Message = _env.IsDevelopment()
                        ? $"Database error: {sqlEx.Message}"
                        : "A database error occurred. Please contact support.";
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorCode = "NOT_FOUND";
                    errorResponse.Message = exception.Message;
                    break;

                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorCode = "INVALID_ARGUMENT";
                    errorResponse.Message = argEx.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.StatusCode = response.StatusCode;
                    errorResponse.ErrorCode = "INTERNAL_SERVER_ERROR";
                    errorResponse.Message = _env.IsDevelopment()
                        ? exception.Message
                        : "An unexpected error occurred.";
                    break;
            }

            if (_env.IsDevelopment())
            {
                errorResponse.StackTrace = exception.StackTrace;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(errorResponse, options);
            await response.WriteAsync(json);
        }
    }
}
