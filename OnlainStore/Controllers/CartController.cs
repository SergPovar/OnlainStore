using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using OnlainStore.Data;
using OnlainStore.Models.Cart;
using OnlainStore.Models.Data;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace OnlainStore.Controllers;

public class CartController : Controller
{
    private List<CartVM> _listCart = new List<CartVM>();
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult Index()
    {
       var listCart = ViewData["cart"] as List<CartVM> ?? new List<CartVM>();
        if (listCart.Count == 0 || ViewBag.Cart == null)
        {
            ViewBag.Message = "Your cart is empty";
            return View();
        }

        decimal total = 0m;
        foreach (var item in  listCart)
        {
            total += item.Total;
        }

        ViewData["cart"] = total;
        // TempData["cart"] = listCart;
        return View( listCart);
    }

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult AddToCart(int id)
    {
        var cart = ViewData["cart"] as List<CartVM> ?? new List<CartVM>();
        var model = new CartVM();
        using (var db = new Db())
        {
            var product = db.Products.Find(id);
            var productInCart = cart.FirstOrDefault(x => x.ProductId == id);
            if (productInCart == null)
            {
                cart.Add(new CartVM()
                {
                    ProductId = product.Id_Product,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    Image = product.ImageName
                });
            }
            else
            {
                productInCart.Quantity++;
            }
        }

        var qty = 0;
        var price = 0m;
        foreach (var item in cart)
        {
            qty += item.Quantity;
            price += item.Price * item.Quantity;
        }

        model.Quantity = qty;
        model.Price = price;
        ViewData["cart"] = cart;
        return View("AddToCartPartial", model);
    }

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public JsonResult IncrementProduct(int productId)
    {
        var cart = TempData["cart"] as List<CartVM>;
        using (var db = new Db())
        {
            var model = cart.FirstOrDefault(x => x.ProductId == productId);
            model.Quantity++;
            var result = new { qty = model.Quantity, price = model.Price };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult DecrementProduct(int productId)
    {
        var cart = TempData["cart"] as List<CartVM>;
        using (var db = new Db())
        {
            var model = cart.FirstOrDefault(x => x.ProductId == productId);
            if (model.Quantity > 1)
            {
                model.Quantity--;
            }
            else
            {
                model.Quantity = 0;
                cart.Remove(model);
            }

            var result = new { qty = model.Quantity, price = model.Price };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public void RemoveProduct(int productId)
    {
        var cart = TempData["cart"] as List<CartVM>;
        using (var db = new Db())
        {
            var model = cart.FirstOrDefault(x => x.ProductId == productId);
            cart.Remove(model);
        }
    }

    [Microsoft.AspNetCore.Mvc.HttpGet]
    public IActionResult PayPalPartial()
    {
        var cart = TempData["cart"] as List<CartVM> ?? new List<CartVM>();

        return View(cart);
    }

    [Microsoft.AspNetCore.Mvc.HttpPost]
    public void PlaceOrder()
    {
        var cart = TempData["cart"] as List<CartVM> ?? new List<CartVM>();
        var username = User.Identity.Name;
        var orderId = 0;
        using (var db = new Db())
        {
            var orderDTO = new OrderDTO();
            var q = db.Users.FirstOrDefault(x => x.Username == username);
            var userId = q.User_Id;
            orderDTO.User_Id = userId;
            orderDTO.Created_Add = DateTime.Now;
            db.Orders.Add(orderDTO);
            db.SaveChanges();
            orderId = orderDTO.Order_Id;
            var orderDetailsDTO = new OrderDetailsDTO();
            foreach (var item in cart)
            {
                orderDetailsDTO.Order_Id = orderId;
                orderDetailsDTO.User_Id = userId;
                orderDetailsDTO.Id_Product = item.ProductId;
                orderDetailsDTO.Quantity = item.Quantity;
                db.OrderDetails.Add(orderDetailsDTO);
                db.SaveChanges();
            }
            
        }
        var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("8ce31b6260fef9", "********09e3"),
            EnableSsl = true
        };
        client.Send("from@example.com", "to@example.com", "New Order", $"You have a new order. Order number {orderId}");
        TempData["cart"] = null;
    }
}