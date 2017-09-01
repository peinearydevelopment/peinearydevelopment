namespace Contracts
{
    public class ResultSet
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
    }

    public class ResultSet<T> : ResultSet
    {
        public T[] Results { get; set; }
    }
}
