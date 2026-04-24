namespace Application.DTO.Output
{
    public record class ApiResponseDto<TData>
    {
        public bool Success { get; init; }
        public string? Status { get; init; }
        public string? Message { get; init; } = null;
        public TData? Data { get; init; }
    }
}
