using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OnlainStore.Models.Data;

namespace OnlainStore.Models.ViewModel.Pages;

public class SidebarVM
{
    public int Id_sidebar { get; set; }
   
    [StringLength(int.MaxValue, MinimumLength = 3)]
    //[AllowHtml]
    public string Body { get; set; }

    public SidebarVM()
    {
        
    }
    public SidebarVM(SidebarDTO row)
    {
        Id_sidebar = row.Id_sidebar;
        Body = row.Body;
    }
}