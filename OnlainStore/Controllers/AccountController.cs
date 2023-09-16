using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlainStore.Data;
using OnlainStore.Models.Account;
using OnlainStore.Models.Data;
using OnlainStore.Models.ViewModel.Pages;

namespace OnlainStore.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult CreateAccount()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        var username = User.Identity.Name;
        if (!string.IsNullOrEmpty(username))
        {
            return RedirectToAction("user-profile");
        }

        return View();
    }

    [HttpPost]
    public IActionResult CreateAccount(UserVM model)
    {
        if (model.FirstName == null || model.LastName == null || model.EmailAdres == null)
        {
            return View(model);
        }

        if (!model.Password.Equals(model.ConfirmPassword))
        {
            ModelState.AddModelError("", "Password doesn't match");
            return View(model);
        }

        using (var db = new Db())
        {
            if (db.Users.Any(x => x.Username.Equals(model.Username)))
            {
                ModelState.AddModelError("", $"Username {model.Username} is taken");
                model.Username = "";
                return View(model);
            }

            var userDTO = new UserDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAdres = model.EmailAdres,
                Username = model.Username,
                Password = model.Password,
                Role_Id = 2
            };
            db.Users.Add(userDTO);
            db.SaveChanges();
        }

        TempData["SM"] = "You are now registred and can login";
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginUserVM model)
    {
        if (model.Username == null || model.Password == null)
        {
            return View(model);
        }

        var isValid = false;
        using (var db = new Db())
        {
            if (db.Users.Any(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
            {
                isValid = true;
            }

            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            var user = db.Users.Where(x => x.Username == model.Username).FirstOrDefault();
            var role = db.Roles.FirstOrDefault(x => x.Role_Id == user.Role_Id);
          
            // var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username) };
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, model.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
            };
        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            return Redirect("/");
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<RedirectResult> Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/account/login");
    }

    [HttpGet]
    [ActionName("user-profile")]
    public IActionResult UserProfile()
    {
        var username = User.Identity.Name;
        UserProfileVM model;
        using (var db = new Db())
        {
            var dto = db.Users.FirstOrDefault(x => x.Username == username);
            model = new UserProfileVM(dto);
        }

        return View("UserProfile", model);
    }

    [HttpPost]
    [ActionName("user-profile")]
    [Authorize]
    public IActionResult UserProfile(UserProfileVM model)
    {
        var usernameIsChanged = false;
        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.FirstName) ||
            string.IsNullOrWhiteSpace(model.LastName))
        {
            return View("UserProfile",model);
        }

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password doesn't match");
                return View("UserProfile",model);
            }
        }

        using (var db = new Db())
        {
            var username = User.Identity.Name;
            if (username != model.Username)
            {
                username = model.Username;
                usernameIsChanged = true;
            }

            if (db.Users.Where(x => x.User_Id != model.User_Id).Any(x => x.Username == username))
            {
                ModelState.AddModelError("", $"Username {model.Username} has already existed");
                model.Username = "";
                return View("UserProfile", model);
            }

            var dto = db.Users.Find(model.User_Id);
            dto.FirstName = model.FirstName;
            dto.LastName = model.LastName;
            dto.EmailAdres = model.EmailAdres;
            dto.Username = model.Username;
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                dto.Password = model.Password;
            }

            db.SaveChanges();
        }

        TempData["SM"] = "You have edited you profile";
        if (!usernameIsChanged)
        {
            return View("UserProfile", model);
        }

        return RedirectToAction("Logout");
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public IActionResult Orders()
    {
        var ordedsForUser = new List<OrdersForUserVM>();
        using (var db = new Db())
        {
            var user = db.Users.FirstOrDefault(x => x.Username == User.Identity.Name);
            var userId = user.User_Id;
            var orders = db.Orders.Where(x => x.User_Id == userId).ToArray()
                .Select(x => new OrderVM(x)).ToList();

            foreach (var order in orders)
            {
                var productsAndQty = new Dictionary<string, int>();
                var total = 0m;
                var orderDetailsDTO = db.OrderDetails.Where(x => x.Order_Id == order.Order_Id).ToList();
                foreach (var orderDetails in orderDetailsDTO)
                {
                    var product = db.Products.FirstOrDefault(x => x.Id_Product == orderDetails.Id_Product);
                    var price = product.Price;
                    var productName = product.Name;
                    var quantity = orderDetails.Quantity;
                    productsAndQty.Add(productName,quantity);
                    total += quantity * price;
                }
                ordedsForUser.Add(new OrdersForUserVM()
                {
                    OrderNumber = order.Order_Id,
                    Total = total,
                    ProductsAndQuty = productsAndQty,
                    CreatedAdd = order.CreatedAdd
                });
            }
        }

        return View(ordedsForUser);
    }
    
}