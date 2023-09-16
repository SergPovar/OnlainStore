using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlainStore.Models.Data;
[Table("Categories")]
public class CategoryDTO
{
    [Key]
    public int Id_category { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public int Sorting { get; set; }
}