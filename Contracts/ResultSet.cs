namespace Contracts
{
    public class ResultSet<T>
    {
        public T[] Results { get; set; }
        public int TotalResults { get; set; }
    }
}
