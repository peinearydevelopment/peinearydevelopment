namespace Contracts
{
    public class ResultSet
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
        public int TotalPageCount => (TotalResults % PageSize == 0) ? TotalResults / PageSize : (TotalResults / PageSize) + 1;
        public bool IsOnFirstPage => PageIndex == 0;
        public bool IsOnLastPage => TotalPageCount == 0 || TotalPageCount == PageIndex + 1;
        public bool HasPreviousPage => !IsOnFirstPage;
        public bool HasNextPage => !IsOnLastPage;
    }

    public class ResultSet<T> : ResultSet
    {
        public T[] Results { get; set; }
    }
}
