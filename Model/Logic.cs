using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Logic
    {
        public static List<Car> _cars = new List<Car>();
        public static List<Owner> _owners = new List<Owner>();

        // Создаем интерфейс для связки представления с методами в логике
        public interface ICarService
        {
            Car CreateCar(string brand, string model, int year, decimal price);
            List<Car> GetAllCars();
            bool DeleteCar(int id);
            List<Car> GetVintageCars();
            Car ChangeCar(int id, string new_brand, string new_model, int new_year, decimal new_price);
            List<Car> GetCarsByBrand(string brand);
            decimal AllPrice(string brand);
        }


        public class CarService : ICarService
        {
            private int _nextIdCar = 1;

            // Создание машины
            public Car CreateCar(string brand, string model, int year, decimal price)
            {
                var car = new Car
                {
                    Id = _nextIdCar++,
                    Brand = brand,
                    Model = model,
                    Year = year,
                    Price = price
                };

                _cars.Add(car);
                return car;
            }

            // Удаление машины по ID
            public bool DeleteCar(int id)
            {
                var car = _cars.FirstOrDefault(c => c.Id == id);
                if (car != null)
                {
                    _cars.Remove(car);
                    return true;
                }
                return false;
            }

            // Чтение всех машин
            public List<Car> GetAllCars()
            {
                return _cars;
            }

            // Чтение только винтажных машин
            public List<Car> GetVintageCars()
            {
                return _cars
                    .Where(car => car.Year <= 1990)
                    .ToList();
            }

            // Изменение машины по ID
            public Car ChangeCar(int id, string new_brand, string new_model, int new_year, decimal new_price)
            {
                var car = _cars.FirstOrDefault(c => c.Id == id);

                if (car == null)
                    return null; //("Машина не найдена")

                car.Brand = new_brand;
                car.Model = new_model;
                car.Year = new_year;
                car.Price = new_price;

                return car;
            }

            //_________БИЗНЕС ФУНКЦИИ___________


            // Сортировка по бренду
            public List<Car> GetCarsByBrand(string brand)
            {
                return _cars
                    .Where(car => car.Brand == brand)
                    .ToList();
            }

            // Общая стоимость автопарка
            public decimal AllPrice(string brand)
            {
                return _cars
                    .Sum(c => c.Price);
            }

        }










        // Создаем интерфейс для связки представления с методами в логике
        public interface IOwnerService
        {
            Owner CreateOwner(string name, int year, int experienceYear);
            bool DeleteOwner(int id);
            List<Owner> GetAllOwners();
            Owner AddCarOwner(int idOwner, int idCar);
            List<Car> ShowCarsOwner(int idOwner);
            Owner DeleteCarOwner(int idCar, int idOwner);

        }

        public class OwnerService : IOwnerService
        {
            private int _nextIdOwner = 1;

            // Создание владельца
            public Owner CreateOwner(string name, int year, int experienceYear)
            {
                var owner = new Owner
                {
                    Id = _nextIdOwner++,
                    Name = name,
                    Year = year,
                    ExperienceYear = experienceYear,
                };

                _owners.Add(owner);
                return owner;
            }

            // Удаление владельца по ID
            public bool DeleteOwner(int id)
            {
                var owner = _owners.FirstOrDefault(c => c.Id == id);
                if (owner != null)
                {
                    _owners.Remove(owner);
                    return true;
                }
                return false;
            }

            // Чтение всех владельцев
            public List<Owner> GetAllOwners()
            {
                return _owners;
            }

            // Добавление машины владельцу
            public Owner AddCarOwner(int idOwner, int idCar)
            {
                var owner = _owners.FirstOrDefault(c => c.Id == idOwner);
                if (owner == null)
                    throw new ArgumentException($"Владелец с ID {idOwner} не найден");

                var car = _cars.FirstOrDefault(c => c.Id == idCar);
                if (car == null)
                    throw new ArgumentException($"Машина с ID {idCar} не найдена");

                if (!car.NoOwner)
                    throw new InvalidOperationException($"Машина с ID {idCar} уже занята");

                // Проверяем, нет ли уже такой машины у владельца
                if (owner.CarsOwner.Any(c => c.Id == idCar))
                    throw new InvalidOperationException($"Машина с ID {idCar} уже принадлежит этому владельцу");

                // Добавляем машину и обновляем флаг
                owner.CarsOwner.Add(car);
                car.NoOwner = false;

                return owner;
            }

            // Вывод машин владельца 
            public List<Car> ShowCarsOwner(int idOwner)
            {
                var owner = _owners.FirstOrDefault(c => c.Id == idOwner);
                if (owner == null)
                    throw new ArgumentException($"Владелец с ID {idOwner} не найден");

                return owner.CarsOwner;
            }

            // Удаление машины у владельца
            public Owner DeleteCarOwner(int idCar, int idOwner)
            {
                var owner = _owners.FirstOrDefault(c => c.Id == idOwner);
                if (owner == null)
                    throw new ArgumentException($"Владелец с ID {idOwner} не найден");

                var car = _cars.FirstOrDefault(c => c.Id == idCar);
                if (car == null)
                    throw new ArgumentException($"Машина с ID {idCar} не найдена");

                if (car == null)
                    throw new ArgumentException($"Машина с ID {idCar} не найдена");

                if (car.NoOwner)
                    throw new InvalidOperationException($"Машина с ID {idCar} свободна");

                owner.CarsOwner.Remove(car);
                car.NoOwner = true;

                return owner;
            }

        }

        








    }
}
