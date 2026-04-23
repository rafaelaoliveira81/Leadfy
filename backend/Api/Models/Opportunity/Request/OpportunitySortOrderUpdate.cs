namespace Models.Request;

public class OpportunitySortOrderUpdate
{
    public List<OpportunitySortOrderItem> Items { get; set; }
}

public class OpportunitySortOrderItem
{
    public int Id { get; set; }
    public int Stage { get; set; }
    public int SortOrder { get; set; }
}
