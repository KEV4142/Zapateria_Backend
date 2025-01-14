using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modelo.Entidades;
using Persistencia.Models;

namespace Persistencia;

public partial class BackendContext : IdentityDbContext<AppUser>
{
    public BackendContext()
    {
    }

    public BackendContext(DbContextOptions<BackendContext> options) : base(options)
    {
    }

    public virtual DbSet<Categoria>? categorias { get; set; }

    public virtual DbSet<Imagen>? imagenes { get; set; }

    public virtual DbSet<Producto>? productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.categoriaid).HasName("pkcategoriaid");

            entity.Property(e => e.descripcion).HasMaxLength(100);
            entity.Property(e => e.estado)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'::character varying");
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.imagenid).HasName("pkimagenid");

            entity.ToTable("imagenes");

            entity.Property(e => e.publicid).HasMaxLength(100);
            entity.Property(e => e.url).HasMaxLength(100);
            entity.Property(e => e.estado)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'::character varying");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.productoid).HasName("pkproductosid");

            entity.Property(e => e.descripcion).HasMaxLength(100);
            entity.Property(e => e.estado)
                .HasMaxLength(1)
                .HasDefaultValueSql("'A'::character varying");
            entity.Property(e => e.precio).HasPrecision(10, 2);

            entity.HasOne(d => d.categoria).WithMany(p => p.productos)
                .HasForeignKey(d => d.categoriaid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkproductocategoriaid");

            entity.HasOne(d => d.imagen).WithMany(p => p.productos)
                .HasForeignKey(d => d.imagenid)
                .HasConstraintName("fkproductoimagenid");
        });
    }
}
