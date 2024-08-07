using Core.Entities;
using Entities.Concrete;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Scan> Scans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("VB_USERS");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.username).HasMaxLength(50).HasColumnName("username");
                entity.Property(e => e.MACADDRESS).HasMaxLength(64).HasColumnName("MACADDRESS");
                entity.Property(e => e.role).HasMaxLength(64).HasColumnName("role");
                entity.Property(e => e.LOGICALREF).HasColumnName("LOGICALREF");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("VB_ORDERS");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.siparisNumarasi).HasMaxLength(64).HasColumnName("siparisNumarasi");
                entity.Property(e => e.status).HasColumnName("status");
                entity.Property(e => e.synchronized).HasColumnName("synchronized");

                entity.HasOne(e => e.User)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("VB_ORDERDETAILS");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.siparisId).HasColumnName("siparisId");
                entity.Property(e => e.siparisNumarasi).HasMaxLength(64).HasColumnName("siparisNumarasi");
                entity.Property(e => e.malzemeKodu).HasMaxLength(64).HasColumnName("malzemeKodu");

                entity.HasOne(e => e.Order)
                .WithMany(e => e.OrderDetails)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Scan>(entity =>
            {
                entity.ToTable("VB_SCANS");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.scanId).HasMaxLength(48).HasColumnName("scanId");
                entity.Property(e => e.result).HasMaxLength(32).HasColumnName("result");

                entity.HasOne(e => e.OrderDetail)
                .WithMany(e => e.Scans)
                .HasForeignKey(e => e.orderDetailId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Configuration.ConnectionString);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            //ChangeTracker : Entityler üzerinden yapılan değişikliklerin ya da yeni eklenen verinin yakalanmasını sağlayan property. Update operasyonlarında Track edilen verileri yakalar.

            var datas = ChangeTracker
                .Entries<BaseEntity>();
            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreateDate = DateTime.Now,
                    EntityState.Modified => data.Entity.UpdateDate = DateTime.Now,
                    _ => DateTime.Now,
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
