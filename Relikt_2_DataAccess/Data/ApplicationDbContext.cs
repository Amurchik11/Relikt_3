using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Relikt_2_DataAccess.Configurations;
using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; } 
        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SizeType> SizeType { get; set; } 
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<InquiryHeader> InquiryHeader { get; set; }
        public DbSet<InquiryDetail> InquiryDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new InquiryDetailConfiguration());

            builder.ApplyConfiguration(new OrderDetailConfiguration());
            //builder.Entity<ApplicationUser>()
            //    .Property(p => p.Id)
            //    .ValueGeneratedOnAdd();

            base.OnModelCreating(builder);
        }



        public DbSet<RightLeft> RightLeft { get; set; }
        public DbSet<ColourOfGlass> ColourOfGlass { get; set; }

        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
}
