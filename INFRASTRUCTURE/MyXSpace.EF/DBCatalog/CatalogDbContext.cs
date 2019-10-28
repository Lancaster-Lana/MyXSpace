using Microsoft.EntityFrameworkCore;
using MyXSpace.Core.Entities;

namespace MyXSpace.EF
{
    /// <summary>
    /// Host DB 
    /// </summary>
    public partial class CatalogDbContext : DbContext
    {
        public virtual DbSet<Tenant> Tenants { get; set; }

        //public virtual DbSet<Customer> Customers { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) :
          base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Tenant = customer (in specific location) 
            builder.Entity<Tenant>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.HasOne(d => d.Admin)
                      .WithOne()
                      .HasForeignKey<Tenant>(d => d.AdminId);

                //entity.Property(e => e.TenantName)
                //    .IsRequired()
                //    .HasMaxLength(50);

                //entity.HasOne(d => d.Customer)
                //     .WithOne()//p => p.Tenant)
                //     .HasForeignKey<Tenant>(d => d.CustomerId)
                //     .OnDelete(DeleteBehavior.ClientSetNull); //TODO:

                //entity.HasOne(d => d.SuperAdmin)
                //    .WithMany(p => p.Tenants)
                //    .HasForeignKey(d => d.SuperAdminId);
            });

            builder.Entity<AppUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppUser>().HasMany(u => u.UserRoles).WithOne().HasForeignKey(r => r.UserId)//.IsRequired()
                .OnDelete(DeleteBehavior.SetNull);//.OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppUser>().ToTable("Users");

            builder.Entity<AppRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId)//IsRequired()
                .OnDelete(DeleteBehavior.SetNull);//.OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppRole>().ToTable("Roles");
        }
    }
}