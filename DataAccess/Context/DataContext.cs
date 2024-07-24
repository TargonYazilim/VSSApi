using Core.Entities;
using Entities.Concrete;
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
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.username).HasMaxLength(50).HasColumnName("username");
                entity.Property(e => e.MACADDRESS).HasMaxLength(64).HasColumnName("MACADDRESS");
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
