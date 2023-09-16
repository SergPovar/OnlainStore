using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using OnlainStore.Models.Data;

namespace OnlainStore.Models.ViewModel.Pages;

public class PageVM
{
    public int Id_page { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Title { get; set; }
    
    public string Slug { get; set; }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 3)]
    [AllowHtml]
    public string Body { get; set; }
    
    public int Sorting { get; set; }
    
    [Display(Name = "Side Bar")]
    public bool HasSidebar { get; set; }
    
    public PageVM()
    {
    }
    public PageVM(PagesDTO row)
    {
        Id_page = row.Id_page;
        Title = row.Title;
        Slug = row.Slug;
        Body = row.Body;
        Sorting = row.Sorting;
        HasSidebar = row.HasSidebar;
    }
}