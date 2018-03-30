using System;
using Microsoft.EntityFrameworkCore;

namespace DetailPage.Models
{
    public class DetailPageContext : DbContext
    {
        public DetailPageContext(DbContextOptions<DetailPageContext> options):base(options)
        {}
        public DbSet<DetailPageModel> DetailPages { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailPageModel>().ToTable("DetailPage");
        }
    }
}