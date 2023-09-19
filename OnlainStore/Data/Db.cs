using Microsoft.EntityFrameworkCore;
using System.Web;
using OnlainStore.Data;

namespace OnlainStore.Models.Data;
public class Db : DbContext
    {
        public DbSet<PagesDTO> Pages { get; set; }
        public DbSet<SidebarDTO> Sidebars { get; set; }
        public DbSet<CategoryDTO> Categories { get; set; }
        public DbSet<ProductDTO> Products { get; set; }
        public DbSet<UserDTO> Users { get; set; }
        public DbSet<RoleDTO> Roles { get; set; }
        public DbSet<OrderDTO> Orders { get; set; }
        public DbSet<OrderDetailsDTO> OrderDetails { get; set; }
        public Db()
        {
        }
        public Db(DbContextOptions<Db> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          optionsBuilder.UseMySql(
              "server=localhost; user = serg; password = 12345; database= shop_onlain_db;"
              ,
              new MySqlServerVersion(new Version(8, 0, 34))
          );
        }
    }
  
