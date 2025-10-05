using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CarRep : IDomainObject
    {
        public int ID_car { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int? IdOwner { get; set; }

        // Явная реализация интерфейса
        int IDomainObject.Id
        {
            get => ID_car;
            set => ID_car = value;
        }
    }
}
