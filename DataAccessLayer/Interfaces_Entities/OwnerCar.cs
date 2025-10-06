using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    [Table("OwnerCars")]
    public class OwnerCar : IDomainObject
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ID_owner")]
        public int OwnerId { get; set; }
        [Column("ID_car")]
        public int CarId { get; set; }
    }
}
