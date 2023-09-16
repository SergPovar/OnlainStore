using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;

namespace OnlainStore.Views.ViewComponents;

public class CategoryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        List<CategoryVM> categoryVMList;
        using (var db = new Db())
        {
            categoryVMList = db.Categories.ToArray()
                .OrderBy(x => x.Sorting)
                .Select(x => new CategoryVM(x))
                .ToList();
        }
        return View("_CategoryMenuPartial",categoryVMList);
    }
   
}