using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WeatherForecast.Identity.Models
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "Имя")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [Display(Name = "Фамилия")]
        [MaxLength(100)]
        public string LastName { get; set; }
    }
}