using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int ExperienceYear { get; set; }
        public List<Car> CarsOwner { get; set; } = new List<Car>();
    }
}
