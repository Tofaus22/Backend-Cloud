using Microsoft.EntityFrameworkCore;
using WebApiCloud3.Models;

namespace WebApiCloud3.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Asegúrate de definir claves primarias si no usas anotaciones [Key]
            modelBuilder.Entity<Categoria>().HasKey(c => c.Id_Categoria);
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id_Usuario);
            modelBuilder.Entity<Producto>().HasKey(p => p.id_Producto);
        }
    }
}
