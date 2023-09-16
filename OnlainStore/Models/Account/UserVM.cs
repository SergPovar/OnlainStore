using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.CompilerServices;
using OnlainStore.Data;

namespace OnlainStore.Models.Account;

public class UserVM
{
    public int User_Id { get; set; }
    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    [DisplayName("Email Adres")]
    public string EmailAdres { get; set; }
    [Required]
    [DisplayName("User Name")]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; set; }
    public int Role_Id { get; set; }

    public UserVM()
    {
        
    }
    public UserVM(UserDTO row)
    {
        User_Id = row.User_Id;
        FirstName = row.FirstName;
        LastName = row.LastName;
        EmailAdres = row.EmailAdres;
        Username = row.Username;
        Password = row.Password;
        Role_Id = row.Role_Id;
    }
}