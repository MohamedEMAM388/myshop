using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Models;

namespace myshop.DataAccess
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {         
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(
            
                X => {

                    X.Property(x => x.Name)
                     .IsRequired()
                     .HasMaxLength(100);
                
                }
                
                
            );
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        
    }
}
