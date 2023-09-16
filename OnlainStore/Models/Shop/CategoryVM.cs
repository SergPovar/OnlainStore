
using System.ComponentModel.DataAnnotations;
using OnlainStore.Models.Data;

namespace OnlainStore.Models.ViewModel.Pages;

public class CategoryVM 
{
    public int Id_category { get; set; }
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string Name { get; set; }
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string Slug { get; set; }
    public int Sorting { get; set; }

    public CategoryVM()
    {
        
    }
    public CategoryVM(CategoryDTO row)
    {
        Id_category = row.Id_category;
        Name = row.Name;
        Slug = row.Slug;
        Sorting = row.Sorting;
    }
    
}