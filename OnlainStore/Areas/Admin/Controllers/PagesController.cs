using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;

namespace OnlainStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PagesController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        List<PageVM> pageList = new();
        using (var db = new Db())
        {
            pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
        }
        
        return View(pageList);
    }

    [HttpGet]
    public IActionResult AddPage()
    {
        return View("AddPage");
    }

    [HttpPost]
    public IActionResult AddPage(PageVM model)
    {
        if (model.Title == null || model.Body == null)
        {
            TempData["SM"] = "Model incorrect";
            return View(model);
        }

        using (var db = new Db())
        {
            string slug;

            var pageDTO = new PagesDTO
            {
                Title = model.Title.ToUpper()
            };

            if (model.Slug != "home" && string.IsNullOrWhiteSpace(model.Slug))
            {
                slug = model.Title.Replace(" ", "-").ToLower();
            }
            else
            {
                slug = model.Slug.Replace(" ", "-").ToLower();
            }

            if (db.Pages.Any(x => x.Title == model.Title))
            {
                ModelState.AddModelError("", "That title is already exist");
                return View(model);
            }

            if (db.Pages.Any(x => x.Slug == model.Slug))
            {
                ModelState.AddModelError("", "That slug is already exist");
                return View(model);
            }

            pageDTO.Body = model.Body;
            pageDTO.Slug = slug;
            pageDTO.HasSidebar = model.HasSidebar;
            pageDTO.Sorting = 100;
            db.Pages.Add(pageDTO);
            db.SaveChanges();
        }

        TempData["SM"] = "You have added new page";

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditPage(int id)
    {
        PageVM model;
        using (var db = new Db())
        {
            model = new PageVM(db.Pages.Find(id));
            if (model == null)
            {
                return Content("Page doesn't exist");
            }
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult EditPage(PageVM model)
    {
        if (model.Title == null || model.Body == null)
        {
            TempData["SM"] = "Model incorrect";
            return View(model);
        }

        using (var db = new Db())
        {
            var id = model.Id_page;
            var slug = "home";
            var dto = db.Pages.Find(id);
            dto.Title = model.Title;
            if (model.Slug != "home")
            {
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
            }

            if (db.Pages.Where(x => x.Id_page != id).Any(x => x.Title == model.Title))
            {
                ModelState.AddModelError("", "That title is already exist");
                return View(model);
            }

            if (db.Pages.Where(x => x.Id_page != id).Any(x => x.Slug == model.Slug))
            {
                ModelState.AddModelError("", "That slug is already exist");
                return View(model);
            }

            dto.Slug = slug;
            dto.Body = model.Body;
            dto.HasSidebar = model.HasSidebar;
            db.SaveChanges();
        }

        TempData["SM"] = "You have edited page";
        return RedirectToAction("EditPage");
    }

    [HttpGet]
    public IActionResult DetailsPage(int id)
    {
        PagesDTO pageDto = new();
        using (var db = new Db())
        {
            pageDto = db.Pages.Find(id);
        }

        if (pageDto == null)
        {
            return Content("The page doesn't exist");
        }

        var pageVM = new PageVM(pageDto);
        return View(pageVM);
    }

    [HttpGet]
    public IActionResult DeletePage(int id)
    {
        using (var db = new Db())
        {
            PagesDTO dto = db.Pages.Find(id);
            db.Pages.Remove(dto);
            db.SaveChanges();
        }

        TempData["SM"] = "You have deleted page";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public void ReorderPages(int[] id)
    {
        using var db = new Db();
        var count = 1;
        var dto = new PagesDTO();
        foreach (var pageId in id)
        {
            dto = db.Pages.Find(pageId);
            dto.Sorting = count;
            db.SaveChanges();
            count++;
        }
    }

    [HttpGet]
    public IActionResult EditSidebar()
    {
        SidebarVM model;
        using (var db = new Db())
        {
            var dto = db.Sidebars.Find(1);
            model = new SidebarVM(dto);
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult EditSidebar(SidebarVM model)
    {
        if (model.Body == null)
        {
            TempData["SM"] = "Model incorrect";
            return View(model);
        }
        using (var db = new Db())
        {
            var dto = db.Sidebars.Find(1);
            dto.Body = model.Body;
            db.SaveChanges();
        }

        TempData["SM"] = "You have edited Sidebar";
        return RedirectToAction("EditSidebar");
    }
}