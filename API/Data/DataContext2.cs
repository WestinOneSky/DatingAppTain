using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    
    public class DataContext2 : DbContext
    {
        public DataContext2(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
