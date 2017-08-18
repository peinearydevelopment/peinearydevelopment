namespace DataAccess.Contracts
{
    public class ResultSetDto<T>
    {
        public T[] Results { get; set; }
        public int TotalResults { get; set; }
    }
}
