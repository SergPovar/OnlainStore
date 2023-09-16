using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OnlainStore.Data;

namespace OnlainStore.Models.Account;

public class LoginUserVM 
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [DisplayName("Remember Me")]
    public bool RememberMe { get; set; }
}