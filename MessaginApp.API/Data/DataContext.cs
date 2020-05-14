using MessaginApp.API.Models;
using Microsoft.EntityFrameworkCore;
namespace MessaginApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Value> Values{get; set;}
    }

    
}