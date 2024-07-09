using FormulaOne.Entities.DBSet;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.DataService
{
    public class AppDbContext: DbContext
    {
        // Define the DB entties
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Achievement> Achievements{ get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //specified the relationship between the entities
            modelBuilder.Entity<Achievement>(entity =>
                {
                    entity.HasOne(d => d.Driver)
                        .WithMany(d => d.Achievements)
                        .HasForeignKey(d => d.DriverId)
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Achievements_Driver");
                }
            );
        }

    }
}
