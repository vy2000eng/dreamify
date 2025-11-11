using dreamify.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dreamify.Infrastructure;

public class ApplicationDbContext:IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
        
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
           // .Property(u => u.UserName).HasMaxLength(256);

           builder.Entity<User>()
               .HasIndex(u => u.Email)
               .IsUnique();




    }
    
    
}