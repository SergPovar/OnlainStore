using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlainStore.Models.Data;

namespace OnlainStore.Data;
[Table("Order_Details")]
public class OrderDetailsDTO
{
    [Key]
    public int Id { get; set; }
    public int Order_Id { get; set; }
    public int User_Id { get; set; }
    public int Id_Product { get; set; }
    public int Quantity { get; set; }
    [ForeignKey("Order_Id")]
    public virtual OrderDTO Orders { get; set; }
    [ForeignKey("User_Id")]
    public virtual UserDTO Users { get; set; }
    [ForeignKey("Id_Product")]
    public virtual ProductDTO Products { get; set; }
    
}