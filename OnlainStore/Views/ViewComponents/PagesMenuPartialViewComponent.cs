using Microsoft.AspNetCore.Mvc;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;

namespace OnlainStore.Views.ViewComponents;

public class PagesMenuPartialViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var pageVmList = new List<PageVM>();
        using (var db = new Db())
        {
            pageVmList = db.Pages.ToArray()
                .OrderBy(x => x.Sorting)
                .Where(x => x.Slug != "home")
                .Select(x => new PageVM(x))
                .ToList();
        }
        
        return View("_PagesMenuPartial",pageVmList);
    }
}