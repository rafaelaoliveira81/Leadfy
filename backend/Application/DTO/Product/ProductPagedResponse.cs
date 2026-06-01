namespace Application.DTO;

public class ProductPagedResponse
{
    public int TotalRegistros { get; set; }
    public List<ProductResponse> Dados { get; set; } = new();
}
