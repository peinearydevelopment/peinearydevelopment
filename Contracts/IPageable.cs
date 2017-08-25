namespace Contracts
{
    public interface IPageable
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int TotalResults { get; set; }
    }
}
