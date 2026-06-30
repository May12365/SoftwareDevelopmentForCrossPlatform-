using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<RepairRequest> RepairRequests => Set<RepairRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // คอนฟิกูเรชันตาราง Products (ของเดิม)
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).HasMaxLength(1000);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            // คอนฟิกูเรชันตาราง RepairRequest (ปรับปรุงใหม่)
            modelBuilder.Entity<RepairRequest>(entity =>
            {
                entity.ToTable("repair_request");

                // ระบุ Key และตั้งค่าให้ฐานข้อมูลเจเนอเรตค่าให้อัตโนมัติเมื่อกดเพิ่มข้อมูล
                entity.HasKey(r => r.request_id);
                entity.Property(r => r.request_id).ValueGeneratedOnAdd();

                entity.Property(r => r.request_no).IsRequired().HasMaxLength(50);
                entity.Property(r => r.title).IsRequired().HasMaxLength(50);
                entity.Property(r => r.decription).HasMaxLength(-1); // nvarchar(MAX)
                entity.Property(r => r.location).HasMaxLength(50);
                entity.Property(r => r.category).HasMaxLength(50);
                entity.Property(r => r.status).HasMaxLength(50);
            });
        }
    }
}