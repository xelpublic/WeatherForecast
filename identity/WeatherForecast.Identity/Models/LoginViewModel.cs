using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Identity.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Требуется имя пользователя")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Требуется пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}