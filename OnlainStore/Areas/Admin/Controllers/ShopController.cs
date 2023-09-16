using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;
using X.PagedList;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OnlainStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ShopController : Controller
{
    private readonly IHostingEnvironment _hostingEnvironment;

    public ShopController(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet]
    public IActionResult Categories()
    {
        var list = new List<CategoryVM>();
        using (var db = new Db())
        {
            list = db.Categories.ToArray()
                .OrderBy(x => x.Sorting)
                .Select(x => new CategoryVM(x))
                .ToList();
        }

        return View(list);
    }

    [HttpPost]
    public string AddNewCategory(string catName)
    {
        string id;

        using (var db = new Db())
        {
            if (db.Categories.Any(x => x.Name == catName))
            {
                return "titletaken";
            }

            var dto = new CategoryDTO();
            dto.Name = catName;
            dto.Slug = catName.Replace(" ", "-").ToLower();
            dto.Sorting = 100;
            db.Categories.Add(dto);

            db.SaveChanges();

            id = dto.Id_category.ToString();
        }

        return id;
    }

    [HttpPost]
    public void ReorderCategories(int[] id)
    {
        using var db = new Db();
        var count = 1;
        var dto = new CategoryDTO();
        foreach (var catId in id)
        {
            dto = db.Categories.Find(catId);
            dto.Sorting = count;
            db.SaveChanges();
            count++;
        }
    }

    [HttpGet]
    public IActionResult DeleteCategory(int id)
    {
        using (var db = new Db())
        {
            CategoryDTO dto = db.Categories.Find(id);
            db.Categories.Remove(dto);
            db.SaveChanges();
        }

        TempData["SM"] = "You have deleted category";
        return RedirectToAction("Categories");
    }

    [HttpPost]
    public string RenameCategory(string newCatName, int id)
    {
        using (var db = new Db())
        {
            if (db.Categories.Any(x => x.Name == newCatName))
            {
                return "titletaken";
            }

            var dto = db.Categories.Find(id);

            dto.Name = newCatName;
            dto.Slug = newCatName.Replace(" ", "-").ToLower();
            db.SaveChanges();
        }

        return "ok";
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        var model = new ProductVM();
        using (var db = new Db())
        {
            model.Categories = new SelectList(db.Categories.ToList(), "Id_category", "Name");
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult AddProduct(ProductVM model, IFormFile file)
    {
        if (model.Name == null || model.Description == null)
        {
            using (var db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id_category", "Name");
                return View(model);
            }
        }

        int Id_product;
        using (var db = new Db())
        {
            if (db.Products.Any(x => x.Name == model.Name))
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id_category", "Name");
                ModelState.AddModelError("", "That product name is taken!");
                return View(model);
            }

            var productDTO = new ProductDTO();
            productDTO.Name = model.Name;
            productDTO.Slug = model.Name.Replace(" ", "-").ToLower();
            productDTO.Description = model.Description;
            productDTO.Price = model.Price;
            productDTO.Id_Category = model.Id_category;
            if (file != null)
            {
                productDTO.ImageName = file.FileName;
            }
            else
            {
                productDTO.ImageName = "No image";
            }

            var catDTO = db.Categories.FirstOrDefault(x => x.Id_category == model.Id_category);
            productDTO.CategoryName = catDTO.Name;
            db.Products.Add(productDTO);
            db.SaveChanges();
            Id_product = productDTO.Id_Product;
        }

        TempData["SM"] = "You have added a product!";

        #region Upload Image

        var originalDirectory = new DirectoryInfo("wwwroot/IMG/Uploads/");
        var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
        var pathString2 = Path.Combine(originalDirectory.ToString(), "Products/" + Id_product);
        var pathString3 = Path.Combine(originalDirectory.ToString(), "Products/" + Id_product + "/Gallery");
        var pathString4 = Path.Combine(originalDirectory.ToString(), "Products/" + Id_product + "/Avatar");

        if (!Directory.Exists(pathString1))
        {
            Directory.CreateDirectory(pathString1);
        }

        if (!Directory.Exists(pathString2))
        {
            Directory.CreateDirectory(pathString2);
        }

        if (!Directory.Exists(pathString3))
        {
            Directory.CreateDirectory(pathString3);
        }

        if (!Directory.Exists(pathString4))
        {
            Directory.CreateDirectory(pathString4);
        }

        if (file != null && file.Length > 0)
        {
            var ext = file.ContentType.ToLower();
            if (ext != "image/jpg" &&
                ext != "image/jpeg" &&
                ext != "image/pjpeg" &&
                ext != "image/gif" &&
                ext != "image/webp" &&
                ext != "image/png")
                using (var db = new Db())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id_category", "Name");
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension");
                    return View(model);
                }

            var path1 = string.Format($"{pathString4}/{file.FileName}");
            //  var path2 = string.Format($"{pathString3}/{file.FileName}");

            using (FileStream fs = new FileStream(path1, FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }

        #endregion

        return RedirectToAction("AddProduct");
    }

    [HttpGet]
    public IActionResult Products(int? page, int? catId)
    {
        var listProductVM = new List<ProductVM>();
        var pageNumber = page ?? 1;

        using (var db = new Db())
        {
            listProductVM = db.Products.ToArray()
                .Where(x => catId == null || catId == 0 || x.Id_Category == catId)
                .Select(x => new ProductVM(x))
                .ToList();
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id_category", "Name");
            ViewBag.SelectedCat = catId.ToString();
        }

        var onePageOfProducts = listProductVM.ToPagedList(pageNumber, 3);
        ViewBag.onePageOfProducts = onePageOfProducts;

        return View(listProductVM);
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        ProductVM model;
        using (var db = new Db())
        {
            var dto = db.Products.Find(id);
            if (dto == null)
            {
                return Content("That product doesn't exist");
            }

            model = new ProductVM(dto);
            model.Categories = new SelectList(db.Categories.ToList(), "Id_category", "Name");
            model.GalleryImages = Directory
                .EnumerateFiles("wwwroot/IMG/Uploads/Products/" + id + "/Gallery")
                .Select(fn => Path.GetFileName(fn));
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult EditProduct(ProductVM model, IFormFile file)
    {
        var id = model.Id_product;
        using (var db = new Db())
        {
            model.Categories = new SelectList(db.Categories.ToList(), "Id_category", "Name");
        }

        if (model.Name == null || model.Description == null)
        {
            return View(model);
        }

        using (var db = new Db())
        {
            if (db.Products.Where(x => x.Id_Product != id)
                .Any(x => x.Name == model.Name))
            {
                ModelState.AddModelError("", "That product name is taken");
                return View(model);
            }
        }

        using (var db = new Db())
        {
            var dto = db.Products.Find(id);
            dto.Name = model.Name;
            dto.Slug = model.Name.Replace(" ", "-").ToLower();
            dto.Description = model.Description;
            dto.Price = model.Price;
            dto.Id_Category = model.Id_category;
            dto.ImageName = model.ImageName;

            var catDTO = db.Categories.FirstOrDefault(x => x.Id_category == model.Id_category);
            dto.CategoryName = catDTO.Name;
            db.SaveChanges();
        }

        TempData["SM"] = "You have edited the product";

        #region UploadImage

        if (file != null && file.Length > 0)
        {
            var ext = file.ContentType.ToLower();
            if (ext != "image/jpg" &&
                ext != "image/jpeg" &&
                ext != "image/pjpeg" &&
                ext != "image/gif" &&
                ext != "image/x-png" &&
                ext != "image/png")
                using (var db = new Db())
                {
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension");
                    return View(model);
                }

            var originalDirectory = new DirectoryInfo("wwwroot/IMG/Uploads/Products/");
            var pathString1 = Path.Combine(originalDirectory.ToString(), id + "/Avatar");
            var pathString2 = Path.Combine(originalDirectory.ToString(), id + "/Gallery");

            var di1 = new DirectoryInfo(pathString1);
            var di2 = new DirectoryInfo(pathString2);
            foreach (var file2 in di1.GetFiles())
            {
                file2.Delete();
            }

            foreach (var file3 in di2.GetFiles())
            {
                file3.Delete();
            }

            var imgName = file.FileName;
            using (var db = new Db())
            {
                var productDTO = db.Products.Find(id);
                productDTO.ImageName = imgName;
                db.SaveChanges();
            }

            var path1 = string.Format($"{pathString1}/{file.FileName}");
            using (var fs = new FileStream(path1, FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }

        #endregion

        return RedirectToAction("EditProduct");
    }

    [HttpGet]
    public IActionResult DeleteProduct(int id)
    {
        using (var db = new Db())
        {
            var dto = db.Products.Find(id);
            db.Products.Remove(dto);
            db.SaveChanges();
        }

        var originalDirectory = new DirectoryInfo("wwwroot/IMG/Uploads/Products/");
        var pathString = Path.Combine(originalDirectory.ToString(), id.ToString());
        if (Directory.Exists(pathString))
        {
            Directory.Delete(pathString, recursive: true);
        }

        return RedirectToAction("Products");
    }

    [HttpPost]
    public void SaveGalleryImages(int id)
    {
        foreach (var fileName in Request.Form.Files)
        {
            var file = fileName;
            if (file != null && file.Length > 0)
            {
                var originalDirectory = new DirectoryInfo("wwwroot/IMG/Uploads/Products/");
                var pathString1 = Path.Combine(originalDirectory.ToString(), id + "/Gallery");
                var path = string.Format($"{pathString1}/{file.FileName}");
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fs);
                }
            }
        }
    }

    [HttpPost]
    public void DeleteImage(int id, string imageName)
    {
        var fullPath1 = "wwwroot/IMG/Uploads/Products/" + id + "/Gallery/" + imageName;

        if (System.IO.File.Exists(fullPath1))
        {
            System.IO.File.Delete(fullPath1);
        }
    }

    [HttpGet]
    public IActionResult Orders()
    {
        var ordersForAdmin = new List<OrdersForAdminVM>();
        using (var db = new Db())
        {
            var orders = db.Orders.ToArray().Select(x => new OrderVM(x)).ToList();

            foreach (var order in orders)
            {
                var productAndQty = new Dictionary<string, int>();
                var total = 0m;
                var orderDetailsList = db.OrderDetails.Where(x => x.Order_Id == order.Order_Id).ToList();
                var user = db.Users.FirstOrDefault(x=>x.User_Id==order.User_Id);
                var username = user.Username;
                foreach (var orderDetails in orderDetailsList)
                {
                    var productDTO = db.Products.FirstOrDefault(x => x.Id_Product == orderDetails.Id_Product);
                    var price = productDTO.Price;
                    var productName = productDTO.Name;
                    var qty = orderDetails.Quantity;
                    productAndQty.Add(productName,qty);
                    total += qty * price;
                }
                ordersForAdmin.Add(new OrdersForAdminVM()
                {
                    OrderNumber = order.Order_Id,
                    Username = username,
                    Total = total,
                    ProductsAndQuty = productAndQty,
                    CreatedAdd = order.CreatedAdd
                });
            }
        }

        return View(ordersForAdmin);
    }
}