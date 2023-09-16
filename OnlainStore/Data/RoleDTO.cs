using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlainStore.Data;

[Table("Roles")]
public class RoleDTO
{
    [Key]
    public int Role_Id { get; set; }
    public string Name { get; set; }
}