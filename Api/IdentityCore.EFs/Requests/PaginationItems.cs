namespace IdentityCore.EFs.Requests
{
    public class PaginationItems<T> where T : class
    {
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public List<T> Items { get; set; }

        public PaginationItems() 
        {

        }

        public PaginationItems(int pageSize,int pageNum,int totalCount,List<T> items)
        {
            Items = items;
            PageSize = pageSize;
            PageNum = pageNum;
            TotalCount = totalCount;
            TotalPage = PageSize != 0 ? (int)Math.Ceiling((decimal)totalCount / PageSize) : 1;
        }
    }
}
