using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OnlainStore.Models.Data;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace OnlainStore.Models.ViewModel.Pages;

public class ProductVM
{
    public int Id_product { get; set; }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string Name { get; set; }

   // [StringLength(int.MaxValue, MinimumLength = 3)]
    public string Slug { get; set; }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string Description { get; set; }
    public decimal Price { get; set; }
    
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string CategoryName { get; set; }
    
    [DisplayName("Category")]
    [Required]
    public int Id_category { get; set; }
    
    [DisplayName("Image")]
    public string ImageName { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; }

    public IEnumerable<string?> GalleryImages { get; set; }

    public ProductVM()
    {
    }

    public ProductVM(ProductDTO row)
    {
        Id_product = row.Id_Product;
        Name = row.Name;
        Slug = row.Slug;
        Description = row.Description;
        Price = row.Price;
        CategoryName = row.CategoryName;
        Id_category = row.Id_Category;
        ImageName = row.ImageName;
    }
}