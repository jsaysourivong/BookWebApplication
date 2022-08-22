using BookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWeb.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //Create a category table with the name categories
        public DbSet<Category> Categories { get; set; }
    }
}
