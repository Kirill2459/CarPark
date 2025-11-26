using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public class OwnerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int ExperienceYear { get; set; }
        public List<int> IdCarsOwner { get; set; } = new List<int>();

        /// <summary>
        /// Конструктор для создания
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">имя</param>
        /// <param name="year">год рождения</param>
        /// <param name="experienceYear">стаж</param>
        /// <param name="idCarsOwner">список id машин владельца</param>
        public OwnerDTO(int id, string name, int year, int experienceYear, List<int> idCarsOwner)
        {
            Id = id;
            Name = name;
            Year = year;
            ExperienceYear = experienceYear;
            IdCarsOwner = idCarsOwner;
        }

        /// <summary>
        /// Конструктор для поиска по Id
        /// </summary>
        /// <param name="id">id</param>
        public OwnerDTO(int id)
        {
            Id = id;
        }

        public OwnerDTO() { }
    }
}
