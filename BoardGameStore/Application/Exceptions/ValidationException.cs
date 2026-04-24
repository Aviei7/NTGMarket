using Microsoft.AspNetCore.Http;

namespace Application.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public int StatusCode { get; } = StatusCodes.Status400BadRequest;
        public string Status { get; } = "validation_error";
        public string UserMessage { get; }

        public ValidationException(string userMessage) : base(userMessage)
        {
            UserMessage = userMessage;
        }
    }
}
