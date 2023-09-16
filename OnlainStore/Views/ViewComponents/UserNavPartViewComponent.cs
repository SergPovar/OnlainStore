using Microsoft.AspNetCore.Mvc;
using OnlainStore.Models.Account;
using OnlainStore.Models.Data;

namespace OnlainStore.Views.ViewComponents;

public class UserNavPartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var username = User.Identity.Name;
        var model = new UserNavPartialVM();
        using (var db = new Db())
        {
            var dto = db.Users.FirstOrDefault(x => x.Username == username);
            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
        }
        return View("_UserNavPart",model);
    }
}