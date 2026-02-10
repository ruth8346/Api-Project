using Microsoft.EntityFrameworkCore;
using TrickyTrayAPI.Models;
using static TrickyTrayAPI.Models.SystemState;


namespace TrickyTrayAPI.Data
{
    public class TrickyTrayDbContext : DbContext
    {
        public TrickyTrayDbContext(DbContextOptions<TrickyTrayDbContext> options) : base(options) { }

        public DbSet<Buyer> Buyers => Set<Buyer>();

        public DbSet<Donor> Donors => Set<Donor>();

        public DbSet<Gift> Gifts => Set<Gift>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderGift> OrderGift => Set<OrderGift>();
        public DbSet<SystemState> SystemState { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Donor -> Gifts (one-to-many)
            // Donor -> Gifts (one-to-many)
            modelBuilder.Entity<Gift>()
                .HasOne(g => g.Donor)
                .WithMany(d => d.Gifts)
                .HasForeignKey(g => g.DonorId)
                .OnDelete(DeleteBehavior.Cascade); // <--- זה השורה שחסרה לך!
            // Buyer -> Orders (one-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerId);

            // Order <-> Gift via OrderGift (many-to-many עם טבלת ביניים)
            modelBuilder.Entity<OrderGift>()
            .HasOne(og => og.Gift)
            .WithMany() // או WithMany(g => g.OrderGifts) אם קיים
            .HasForeignKey(og => og.GiftId); // זה ימנע את יצירת GiftId1

            modelBuilder.Entity<OrderGift>()
       .HasOne(og => og.Gift)
       .WithMany(g => g.OrderGifts) // ודאי שהשדה הזה קיים במודל Gift
       .HasForeignKey(og => og.GiftId)
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SystemState>()
                .HasData(new SystemState
                {
                    Id = 1,
                    Status = SaleStatus.Draft
                });

        }


    }
}
