using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    public class OwnerCar
    {
        
        public int ID { get; set; }
        public int ID_owner { get; set; }
        public int ID_car { get; set; }
    }
}
