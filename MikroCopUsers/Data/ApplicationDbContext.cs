using Microsoft.EntityFrameworkCore;
using MikroCopUsers.Models;

namespace MikroCopUsers.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}