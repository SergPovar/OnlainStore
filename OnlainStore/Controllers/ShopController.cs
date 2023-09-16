using Microsoft.AspNetCore.Mvc;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;


namespace OnlainStore.Controllers;

public class ShopController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index","Page");
    }
    
    [HttpGet]
    public IActionResult Category(string name)
    {
        var listProductVM = new List<ProductVM>();
        using (var db = new Db())
        {
            var categoryDTO = db.Categories.FirstOrDefault(x => x.Slug == name);
            var catID = categoryDTO.Id_category;
            listProductVM = db.Products.ToArray()
                .Where(x => x.Id_Category == catID)
                .Select(x => new ProductVM(x))
                .ToList();
            var productCat = db.Products
                .FirstOrDefault(x => x.Id_Category == catID);
            if (productCat==null)
            {
                var catName = db.Categories
                    .Where(x => x.Slug == name)
                    .Select(x => x.Name)
                    .FirstOrDefault();
                ViewBag.CategoryName = catName;
            }
            else
            {
                ViewBag.CategoryName = productCat.CategoryName;
            }

        }
        return View(listProductVM);
    }

    [HttpGet]
    public IActionResult ProductDetails(string name)
    {
        ProductDTO dto;
        ProductVM model;
        var id = 0;

        using (var db = new Db())
        {
            if (!db.Products.Any(x => x.Slug.Equals(name)))
            {
                return RedirectToAction("Index","Shop");
            }

            dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();
            id = dto.Id_Product;
            model = new ProductVM(dto);
        }

        model.GalleryImages = Directory.EnumerateFiles("wwwroot/IMG/Uploads/Products/" + id + "/Gallery")
            .Select(fn => Path.GetFileName(fn));
        
        return View("ProductDetails",model);
    }
}