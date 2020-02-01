using ereferee.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ereferee.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(GetConfigurations.GetNpgsqlConnectionString());
        }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}