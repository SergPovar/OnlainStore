using OnlainStore.Data;

namespace OnlainStore.Models.ViewModel.Pages;

public class OrderVM
{
    public int Order_Id { get; set; }
    public int User_Id { get; set; }
    public DateTime CreatedAdd { get; set; }

    public OrderVM()
    {

    }
    public OrderVM(OrderDTO row)
    {
        Order_Id = row.Order_Id;
        User_Id = row.User_Id;
        CreatedAdd = row.Created_Add;
    }
}