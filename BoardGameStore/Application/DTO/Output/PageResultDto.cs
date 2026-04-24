namespace Application.DTO.Output
{
    public class PageResultDto<T>
    {
        public IReadOnlyList<T> Items { get; init; } = new List<T>();
        public int TotalCount { get; init; }       // всего элементов в базе
        public int Page { get; init; }        // текущая страница
        public int PageSize { get; init; }    // сколько на странице
        public bool HasMoreItems { get; init; } // есть ли ещё товары 
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
