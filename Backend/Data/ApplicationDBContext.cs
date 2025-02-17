    using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Backend.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        //Constructor
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            //Database.EnsureCreated();
        }
        //DbSets : Represent the tables in the database
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        //Fluent API: Configure the relationship between the tables
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); //This is required to configure the identity tables and columns

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId })); //Composite key

            builder.Entity<Portfolio>() //Configure the relationship between the Portfolio and AppUser
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);
            
            builder.Entity<Portfolio>() //Configure the relationship between the Portfolio and Stock
                .HasOne(p => p.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);

            List<IdentityRole> role = new List<IdentityRole>  //Seed the roles : Admin and User
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            };
            
            builder.Entity<IdentityRole>().HasData(role); //Seed the roles
        }
    }
}
