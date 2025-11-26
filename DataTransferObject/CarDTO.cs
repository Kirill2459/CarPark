using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataTransferObject
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int? IdOwner { get; set; }

        /// <summary>
        /// Конструктор для создания
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="brand">брэнд</param>
        /// <param name="model">модель</param>
        /// <param name="year">год выпуска</param>
        /// <param name="price">цена</param>
        /// <param name="idOwner">id владельца</param>
        public CarDTO(int id, string brand, string model, int year, decimal price, int? idOwner)
        {
            Id = id;
            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
            IdOwner = idOwner;
        }

        /// <summary>
        /// Конструктор для поиска по Id
        /// </summary>
        /// <param name="id"></param>
        public CarDTO(int id)
        {
            Id = id;
        }

        public CarDTO() { }
    }
}
