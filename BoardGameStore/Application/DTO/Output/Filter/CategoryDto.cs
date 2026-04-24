namespace Application.DTO.Output.FilterDTO
{
    public record CategoryDto { 
        public int CategoryID { get; init; } 
        public string? Name { get; init; } = null; 
        public string? Slug { get; init; } = null; 
    };
}
