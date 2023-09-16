namespace OnlainStore.Models.ViewModel.Pages;

public class OrdersForUserVM
{
    public int OrderNumber { get; set; }
    public decimal Total { get; set; }
    public Dictionary<string,int> ProductsAndQuty { get; set; }
    public DateTime CreatedAdd { get; set; }
}