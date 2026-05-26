public class PagedResult<T>
{
    public int TotalRegistros { get; set; }
    public List<T> Dados { get; set; } = new();
}