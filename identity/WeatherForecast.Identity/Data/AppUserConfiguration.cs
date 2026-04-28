using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherForecast.Identity.Models;

namespace WeatherForecast.Identity.Data;

/// <summary>
/// Конфигурация AppUser
/// </summary>
public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(x => x.Id);
            
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired(false);
            
        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}