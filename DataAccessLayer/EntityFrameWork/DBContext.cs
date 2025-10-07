using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.EntityFrameWork
{
    public class DBContext : DbContext
    {
        public DbSet<CarRep> Cars { get; set; }
        public DbSet<OwnerRep> Owners { get; set; }
        public DbSet<OwnerCar> OwnerCars { get; set; }

        public DBContext(string stringConection) : base(stringConection)
        { }

    }
}
