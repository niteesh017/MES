using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Models;

namespace RadialGraderMES.Data
{
    public class MESDbContext : DbContext
    {
        public MESDbContext(DbContextOptions<MESDbContext> options) : base(options) 
        {
        }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineData> MachineData { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionBatch> ProductionBatch { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Machine>()
                .HasIndex(x => x.MachineCode)
                .IsUnique();

            modelBuilder.Entity<ProductionOrder>().
                HasIndex(x => x.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<ProductionBatch>()
                .HasIndex(x => x.BatchNumber)
                .IsUnique();

            modelBuilder.Entity<Machine>()
                .Property(x => x.MachineName)
                .HasMaxLength(150);

            modelBuilder.Entity<MachineData>()
                .Property(x => x.MachineSpeed)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MachineData>()
                .Property(x => x.InputWeight)
                .HasPrecision(12, 3);

            modelBuilder.Entity<MachineData>()
                .Property(x => x.OutputWeight)
                .HasPrecision(12, 3);

            modelBuilder.Entity<ProductionOrder>()
                .HasOne(x => x.Machine)
                .WithMany()
                .HasForeignKey(x => x.MachineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionBatch>()
                .HasOne(x => x.ProductionOrder)
                .WithMany(x => x.Batches)
                .HasForeignKey(x => x.ProductionOrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
