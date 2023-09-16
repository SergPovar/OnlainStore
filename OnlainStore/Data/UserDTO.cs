using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlainStore.Data;

[Table("Users")]
public class UserDTO
{
    [Key] 
    public int User_Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAdres { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    // [ForeignKey("Role_Id")]
    public int Role_Id { get; set; }
    
}