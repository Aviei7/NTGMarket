using Application.DTO.Output;

namespace Application.Factories
{
    public static class Api
    {
        public static ApiResponseDto<T> Ok<T>(T data, string status = "OK", string? message = null) => new ApiResponseDto<T>
        {
            Data = data,
            Success = true,
            Status = status,
            Message = message
        };
        public static ApiResponseDto<T> Fail<T>(string message, string status = "Error")
        => new ApiResponseDto<T>
        {
            Data = default!,
            Success = false,
            Status = status,
            Message = message
        };
    }
}
