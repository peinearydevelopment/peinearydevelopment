namespace Contracts
{
    public class ResultSet<T> : IPageable
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public T[] Results { get; set; }
        public int TotalResults { get; set; }
    }
}
