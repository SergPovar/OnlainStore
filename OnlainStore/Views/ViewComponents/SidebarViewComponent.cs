using Microsoft.AspNetCore.Mvc;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;

namespace OnlainStore.Views.ViewComponents;

public class SidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        SidebarVM model;
        using (var db = new Db())
        {
            var dto = db.Sidebars.Find(2);
            model = new SidebarVM(dto);
        }
        return View("_SidebarMenuPartial",model);
    }
}