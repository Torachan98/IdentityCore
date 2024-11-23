namespace IdentityCore.EFs.Requests
{
    public class PaginationItems<T> where T : class
    {
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}
