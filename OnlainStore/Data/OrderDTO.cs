using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlainStore.Data;
[Table("Orders")]
public class OrderDTO
{
    [Key]
    public int Order_Id { get; set; }
    public int User_Id { get; set; }
    public DateTime Created_Add { get; set; }
    
    [ForeignKey("User_Id")]
    public virtual UserDTO Users { get; set; }
}