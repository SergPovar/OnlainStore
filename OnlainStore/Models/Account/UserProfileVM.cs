using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OnlainStore.Data;

namespace OnlainStore.Models.Account;

public class UserProfileVM
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
    public string Password { get; set; }
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; set; }

    public UserProfileVM()
    {
        
    }
    public UserProfileVM(UserDTO row)
    {
        User_Id = row.User_Id;
        FirstName = row.FirstName;
        LastName = row.LastName;
        EmailAdres = row.EmailAdres;
        Username = row.Username;
        Password = row.Password;
    }
}