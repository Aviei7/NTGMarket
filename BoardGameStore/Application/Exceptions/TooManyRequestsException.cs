using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class TooManyRequestsException : Exception
    {
        public string Status { get; }
        public int StatusCode { get; }
        public string UserMessage { get; }
        public TimeSpan? RetryAfter { get; }



        public TooManyRequestsException(string? userMessage = null, TimeSpan? retryAfter = null) : base("Rate limit exceeded")
        {
            StatusCode = 429;
            Status = "rate_limit";
            UserMessage = userMessage ?? "Занадто багато запитів. Спробуйте пізніше.";
            RetryAfter = retryAfter;
        }
    }
}
