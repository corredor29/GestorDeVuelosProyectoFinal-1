using GestorDeVuelosProyectoFinal.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Auth.Infrastructure;

// Este contexto se usa solo para la autenticación simple basada en app_users.
// Se mantiene aparte del AppDbContext principal para no mezclar responsabilidades.
public sealed class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<AppUserEntity> AppUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Solo hay una entidad aquí, pero igual dejamos la configuración separada
        // para mantener el mismo patrón del resto del proyecto.
        modelBuilder.ApplyConfiguration(new AppUserEntityConfiguration());
    }
}
