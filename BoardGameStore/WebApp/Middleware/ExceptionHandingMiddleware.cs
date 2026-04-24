using Application.DTO.Output;
using Application.Exceptions;
using Microsoft.Data.SqlClient;

namespace WebApp.Middleware
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
        {
            try
            {
                await next(ctx);
            }
            catch (OperationCanceledException) when (ctx.RequestAborted.IsCancellationRequested)
            {
                await Write(ctx, 400, new ApiResponseDto<object>
                {
                    Success = false,
                    Status = "canceled",
                    Message = "Запит скасовано."
                });
            }
            catch (SqlException se)
            {
                var (code, status, errorMsg, message) = se.Number switch
                {
                    208 => (503, "db_unavailable", "DB.Invalid object name", "Об'єкт не знайдено"),       //Invalid object name
                    -2 => (503, "timeout", "DB.TIMEOUT", "Перевищено час очікування при підключенні до БД"),           //SQL timeout
                    4060 => (503, "db_unavailable", "DB.Connection error", "Помилка з'єднання з БД"),     //Cannot open database
                    18456 => (503, "db_unavailable", "DB.Login failed", "Помилка авторизації у БД"),    //Login failed
                    _ => (503, "db_unavailable", $"DB.{se.Number}", "Неочікувана помилка з БД")         //Other
                };

                await Write(ctx, code, new ApiResponseDto<object>
                {
                    Success = false,
                    Status = status,
                    Message = message,
                    Data = se.Message
                });
            }
            catch (KeyNotFoundException ex) //not found
            {
                await Write(ctx, 404, new ApiResponseDto<object>
                {
                    Success = false,
                    Status = "not_found",
                    Message = ex.Message
                });
            }
            catch (TooManyRequestsException ex) //Too many requests
            {
                if (ex.RetryAfter.HasValue)
                {
                    ctx.Response.Headers["Retry-After"] = ((int)ex.RetryAfter.Value.TotalSeconds).ToString();
                }

                await Write(ctx, ex.StatusCode, new ApiResponseDto<object>()
                {
                    Success = false,
                    Status = ex.Status,
                    Message = ex.UserMessage
                });
            }
            catch (UnauthorizedException ex) //Unauthorized
            {
                await Write(ctx, ex.StatusCode, new ApiResponseDto<object>()
                {
                    Success = false,
                    Status = ex.Status,
                    Message = ex.UserMessage
                });
            }
            catch (ValidationException ex)
            {
                await Write(ctx, ex.StatusCode, new ApiResponseDto<object>()
                {
                    Success = false,
                    Status = ex.Status,
                    Message = ex.UserMessage
                });
            }
            catch (Exception ex) // Other exceptions
            {
                await Write(ctx, 500, new ApiResponseDto<object>
                {
                    Success = false,
                    Status = "unexpected",
                    Message = ex.Message
                });
            }
        }

        private static Task Write(HttpContext ctx, int statusCode, ApiResponseDto<object> body)
        {
            if (!ctx.Response.HasStarted)
            {
                ctx.Response.ContentType = "application/json; charset=utf-8";
                ctx.Response.StatusCode = statusCode;
            }
            return ctx.Response.WriteAsJsonAsync(body);
        }
    }
}
