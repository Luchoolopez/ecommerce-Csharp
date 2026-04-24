namespace EcommerceStore.DTOs
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)Limit);
    }
}
