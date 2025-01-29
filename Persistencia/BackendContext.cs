using Microsoft.AspNetCore.Identity;
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
        var adminRoleId = "51df7aae-a506-46ff-8e34-9f2f0c661885";
        var clientRoleId = "368cb24e-03d3-4a01-b558-dbde9b33272c";


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

        modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = CustomRoles.ADMIN,
                    NormalizedName = CustomRoles.ADMIN
                },
                new IdentityRole
                {
                    Id = clientRoleId,
                    Name = CustomRoles.CLIENT,
                    NormalizedName = CustomRoles.CLIENT
                }
            );


            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(
                new IdentityRoleClaim<string> { Id = 1, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.CATEGORIA_READ },
                new IdentityRoleClaim<string> { Id = 2, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.CATEGORIA_UPDATE },
                new IdentityRoleClaim<string> { Id = 3, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.CATEGORIA_CREATE },
                new IdentityRoleClaim<string> { Id = 4, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.PRODUCTOS_READ },
                new IdentityRoleClaim<string> { Id = 5, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.PRODUCTOS_UPDATE },
                new IdentityRoleClaim<string> { Id = 6, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.PRODUCTOS_CREATE },
                new IdentityRoleClaim<string> { Id = 7, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.IMAGEN_READ },
                new IdentityRoleClaim<string> { Id = 8, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.IMAGEN_UPDATE },
                new IdentityRoleClaim<string> { Id = 9, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.IMAGEN_CREATE },
                new IdentityRoleClaim<string> { Id = 10, RoleId = adminRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.USUARIO_CREATE },

                new IdentityRoleClaim<string> { Id = 11, RoleId = clientRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.CATEGORIA_READ },
                new IdentityRoleClaim<string> { Id = 12, RoleId = clientRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.PRODUCTOS_READ },
                new IdentityRoleClaim<string> { Id = 13, RoleId = clientRoleId, ClaimType = "POLICIES", ClaimValue = PolicyMaster.IMAGEN_READ }
            );

    }
}
