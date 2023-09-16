using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlainStore.Models.Data;
[Table("Sidebar")]
public class SidebarDTO
{
    [Key]
    public int Id_sidebar { get; set; }
    
    public string Body { get; set; }
}