using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OwnerCar : IDomainObject
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int CarId { get; set; }
    }
}
