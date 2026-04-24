using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public sealed class UnauthorizedException : Exception
    {
        public int StatusCode { get; } = StatusCodes.Status401Unauthorized;
        public string Status { get; } = "unauthorized";
        public string UserMessage { get; }

        public UnauthorizedException(string? userMessage = null): base(userMessage ?? "Введено некоректні дані для входу.")
        {
            UserMessage = userMessage ?? "Введено некоректні дані для входу.";
        }
    }
}
