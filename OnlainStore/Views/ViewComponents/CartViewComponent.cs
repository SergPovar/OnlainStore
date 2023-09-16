using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using OnlainStore.Models.Cart;

namespace OnlainStore.Views.ViewComponents;

public class CartViewComponent  : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new CartVM();
        var qty = 0;
        var price = 0m;
        if (ViewData["cart"] !=null)
        {
            var list = (List<CartVM>)ViewData["cart"];
            foreach (var item in list)
            {
                qty += item.Quantity;
                price += item.Price * item.Quantity;
            }
            model.Quantity = qty;
            model.Price = price;
        }
        else
        {
            model.Quantity = 0;
            model.Price = 0m;
        }
        
        return View("_CartPartial",model);
    }
}