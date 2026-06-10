namespace Application.DTO;

public class OpportunitySortOrderUpdate
{
    public List<OpportunitySortOrderItem> Items { get; set; }
}

public class OpportunitySortOrderItem
{
    public string Id { get; set; }
    public int Stage { get; set; }
    public int SortOrder { get; set; }
}
