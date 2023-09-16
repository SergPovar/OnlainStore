using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace OnlainStore.Models.Data;

[Table("Pages")]
public class PagesDTO 
{
    [Key]
    public int Id_page { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public int Sorting { get; set; }
    public bool HasSidebar { get; set; }
}