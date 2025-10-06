using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Linq;
using System.Reflection.Emit;


namespace DataAccessLayer.EntityFrameWork
{
    public class DBContext : DbContext
    {
        public DbSet<CarRep> Cars { get; set; }
        public DbSet<OwnerRep> Owners { get; set; }
        public DbSet<OwnerCar> OwnerCars { get; set; }

        public DBContext(string stringConection) : base(stringConection)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OwnerCar>()
                .Property(e => e.OwnerId)
                .HasColumnName("ID_owner");
            modelBuilder.Entity<OwnerCar>()
                .Property(e => e.CarId)
                .HasColumnName("ID_car");
        }
    }
}
