using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    [Table("Owners")]
    public class OwnerRep : IDomainObject
    {
        [Key]
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
