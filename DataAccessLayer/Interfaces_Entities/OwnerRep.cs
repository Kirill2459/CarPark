using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OwnerRep : IDomainObject
    {
        public int ID_owner { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int ExperienceYear { get; set; }

        // Явная реализация интерфейса
        int IDomainObject.Id
        {
            get => ID_owner;
            set => ID_owner = value;
        }
    }
}
