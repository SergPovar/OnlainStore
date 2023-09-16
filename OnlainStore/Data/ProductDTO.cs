using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlainStore.Models.Data;

[Table("Products")]
public class ProductDTO
{
    [Key]
    public int Id_Product { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
    public int Id_Category { get; set; }
    public string ImageName { get; set; }
    
    // [ForeignKey("Id_category")]
    // public virtual CategoryDTO Category { get; set; }
}