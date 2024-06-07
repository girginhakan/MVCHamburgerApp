using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCHamburgerApp.Data.Entities;
using System.Collections.Generic;

namespace MVCHamburgerApp.Data
{
    public class HamburgerDbContext:IdentityDbContext<AppUser,IdentityRole<int>,int>
    {
        public HamburgerDbContext(DbContextOptions<HamburgerDbContext> options):base(options) { }

        public DbSet<ExtraTopping> ExtraToppings { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>()
                .HasData(new AppUser
                {
                    Id = 1,
                    Name = "adminName",
                    Surname = "adminSurname",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@mail.com",
                    NormalizedEmail = "ADMIN@MAIL.COM",
                  
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber = "-",
                    PasswordHash = hasher.HashPassword(null, "Az*123456"),
                    SecurityStamp = Guid.NewGuid().ToString()
                });


            builder.Entity<IdentityRole<int>>()
                .HasData(new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                });


            builder.Entity<IdentityRole<int>>()
                .HasData(new IdentityRole<int>
                {
                    Id= 2,
                    Name="Musteri",
                    NormalizedName="MUSTERI"
                });

            builder.Entity<IdentityUserRole<int>>()
                .HasData(new IdentityUserRole<int>
                {
                    UserId = 1,
                    RoleId = 1
                });
        }
    }
}
