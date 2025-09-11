using Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Logic
    {
        // Поднимаемся на 3 уровня вверх от исполняемого файла
        static string basePath = Path.GetFullPath(Path.Combine(
            Environment.CurrentDirectory,
            @"..\..\..\DataFiles"));


        //==============JSON=================
        public static List<T> ReadAll<T>()
        {
            

            string path = Path.Combine(basePath, typeof(T).Name + ".json");
            string jsonString = File.ReadAllText(path);
            List<T> entities = JsonConvert.DeserializeObject<List<T>>(jsonString);
            return entities;
        }

        public static T Read<T>(int id)
        {
            List<T> entities = ReadAll<T>();

            var propertyInfo = typeof(T).GetProperty("Id");
            T entity = entities.FirstOrDefault(item =>
            {
                var Id = propertyInfo.GetValue(item);
                return Id != null && Id.Equals(id);
            });

            return entity;
        }

        public static void Add<T>(T entity)
        {
            List<T> entities = ReadAll<T>();
            entities.Add(entity);
            string pathOut = Path.Combine(basePath, typeof(T).Name + ".json");
            string jsonOut = JsonConvert.SerializeObject(entities);
            File.WriteAllText(pathOut, jsonOut);
        }

        public static void Delete<T>(int id)
        {
            List<T> entities = ReadAll<T>();

            var propertyInfo = typeof(T).GetProperty("Id");
            T entity = entities.FirstOrDefault(item =>
            {
                var Id = propertyInfo.GetValue(item);
                return Id != null && Id.Equals(id);
            });

            if (entity != null)
            {
                entities.Remove(entity);
                string pathOut = Path.Combine(basePath, typeof(T).Name + ".json");
                string jsonOut = JsonConvert.SerializeObject(entities);
                File.WriteAllText(pathOut, jsonOut);
            }
        }

        public static void Update<T>(T updateEntity)
        {
            var propertyInfo = typeof(T).GetProperty("Id");
            if (propertyInfo == null)
                throw new ArgumentException("Класс должен иметь свойство Id");

            // Правильное получение значения
            int id = (int)propertyInfo.GetValue(updateEntity);

            Delete<T>(id);
            Add(updateEntity);
        }
        //===================================


        //генератор Id
        public static int GeneratorId()
        {
            int id = int.Parse(File.ReadAllText(Path.Combine(basePath, "id.txt")));
            ++id;
            File.WriteAllText(Path.Combine(basePath, "id.txt"), id.ToString());
            return id;
        }



        // Создание машины
        public static Car CreateCar(string brand, string model, int year, decimal price)
        {
            var car = new Car
            {
                Id = GeneratorId(),
                Brand = brand,
                Model = model,
                Year = year,
                Price = price
            };
            return car;
        }







        //_________БИЗНЕС ФУНКЦИИ___________

        // Чтение только винтажных машин
        public static List<Car> GetVintageCars()
        {
            List<Car> cars = ReadAll<Car>();
            return cars.Where(car => car.Year <= 1990).ToList();
        }

        // Сортировка по бренду
        public static List<Car> GetCarsByBrand(string brand)
        {
            List<Car> cars = ReadAll<Car>();
            return cars.Where(car => car.Brand == brand).ToList();
        }

        // Общая стоимость автопарка
        public static decimal AllPrice()
        {
            List<Car> cars = ReadAll<Car>();
            return cars.Sum(c => c.Price);
        }






        

        // Создание владельца
        public static Owner CreateOwner(string name, int year, int experienceYear)
        {
            var owner = new Owner
            {
                Id = GeneratorId(),
                Name = name,
                Year = year,
                ExperienceYear = experienceYear
            };
            return owner;
        }


        //// Добавление машины владельцу
        //public Owner AddCarOwner(int idOwner, int idCar)
        //{
        //    var owner = _owners.FirstOrDefault(c => c.Id == idOwner);
        //    if (owner == null)
        //        throw new ArgumentException($"Владелец с ID {idOwner} не найден");

        //    var car = _cars.FirstOrDefault(c => c.Id == idCar);
        //    if (car == null)
        //        throw new ArgumentException($"Машина с ID {idCar} не найдена");

        //    if (!car.NoOwner)
        //        throw new InvalidOperationException($"Машина с ID {idCar} уже занята");

        //    // Проверяем, нет ли уже такой машины у владельца
        //    if (owner.CarsOwner.Any(c => c.Id == idCar))
        //        throw new InvalidOperationException($"Машина с ID {idCar} уже принадлежит этому владельцу");

        //    // Добавляем машину и обновляем флаг
        //    owner.CarsOwner.Add(car);
        //    car.NoOwner = false;

        //    return owner;
        //}

        //// Вывод машин владельца 
        //public List<Car> ShowCarsOwner(int idOwner)
        //{
        //    var owner = _owners.FirstOrDefault(c => c.Id == idOwner);
        //    if (owner == null)
        //        throw new ArgumentException($"Владелец с ID {idOwner} не найден");

        //    return owner.CarsOwner;
        //}

        //// Удаление машины у владельца
        //public Owner DeleteCarOwner(int idCar, int idOwner)
        //{
        //    var owner = _owners.FirstOrDefault(c => c.Id == idOwner);
        //    if (owner == null)
        //        throw new ArgumentException($"Владелец с ID {idOwner} не найден");

        //    var car = _cars.FirstOrDefault(c => c.Id == idCar);
        //    if (car == null)
        //        throw new ArgumentException($"Машина с ID {idCar} не найдена");

        //    if (car == null)
        //        throw new ArgumentException($"Машина с ID {idCar} не найдена");

        //    if (car.NoOwner)
        //        throw new InvalidOperationException($"Машина с ID {idCar} свободна");

        //    owner.CarsOwner.Remove(car);
        //    car.NoOwner = true;

        //    return owner;
        //}
    }
}
