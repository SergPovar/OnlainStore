
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace OnlainStore.Controllers;

public class PageController : Controller
{
    [HttpGet]
    public IActionResult Index(string page ="")
    {
        if (page=="")
        {
            page = "home";
        }

        PageVM model;
        PagesDTO dto;
        using (var db = new Db())
        {
            if (!db.Pages.Any(x => x.Slug.Equals(page)))
            {
                return RedirectToAction("Index",new{page=""});
            }
        }
        using (var db = new Db())
        {
            dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
        }
        ViewBag.PageTitle = dto.Title;
        if (dto.HasSidebar==true)
        {
            ViewBag.Sidebar = "Yes";
        }
        else
        {
            ViewBag.Sidebar = "No";
        }
        model = new PageVM(dto);
        return View(model);
    }
}