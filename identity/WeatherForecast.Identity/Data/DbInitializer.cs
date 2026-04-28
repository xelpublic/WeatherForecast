using Microsoft.AspNetCore.Identity;
using WeatherForecast.Identity.Models;

namespace WeatherForecast.Identity.Data;

/// <summary>
/// Инициализатор базы данных
/// </summary>
public class DbInitializer
{
    /// <summary>
    /// Инициализирует базу данных
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="userManager">Менеджер пользователей</param>
    /// <param name="roleManager">Менеджер ролей</param>
    public static async Task InitializeAsync(
        AuthDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        context.Database.EnsureCreated();
            
        string[] roles = { "Admin", "User" };
        foreach (var roleName in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
            
        var testUser = await userManager.FindByNameAsync("test");
        if (testUser == null)
        {
            var user = new AppUser
            {
                UserName = "test",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "test");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                throw new Exception($"Ошибка при создании тестового пользователя: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}