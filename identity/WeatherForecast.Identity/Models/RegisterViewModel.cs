using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Identity.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Требуется имя пользователя")]
    [Display(Name = "Имя пользователя")]
    public string Username { get; set; }
        
    [Required(ErrorMessage = "Требуется пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
        
    [Required(ErrorMessage = "Требуется подтверждение пароля")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [Display(Name = "Подтверждение пароля")]
    public string ConfirmPassword { get; set; }
        
    public string ReturnUrl { get; set; }
}