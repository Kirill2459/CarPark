using DataAccessLayer;
//using DataAccessLayer.Dapper;
//using DataAccessLayer.EntityFrameWork;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Remoting.Contexts;


namespace Model
{
    public class Logic
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICarRepository _carRepository;

        public Logic(IOwnerRepository ownerRepository, ICarRepository carRepository)
        {
            _ownerRepository = ownerRepository;
            _carRepository = carRepository;
        }


        /// <summary>
        /// Обобщающий метод для чтения всех сущностей
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <returns>Список сущностей</returns>
        public List<T> ReadAll<T>()
        {
            if (typeof(T) == typeof(Owner))
                return ReadAllOwners().Cast<T>().ToList();
            else if (typeof(T) == typeof(Car))
                return ReadAllCars().Cast<T>().ToList();

            return new List<T>();
        }


        /// <summary>
        /// Обобщающий метод для чтения одной сущности
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <typeparam name="id">id сущности</typeparam>
        /// <returns>Сущность</returns>
        public T Read<T>(int id)
        {
            if (typeof(T) == typeof(Owner))
                return (T)(object)ReadOwner(id);
            else if (typeof(T) == typeof(Car))
                return (T)(object)ReadCar(id);

            return default(T);
        }


        /// <summary>
        /// Обобщающий метод для добавления одной сущности
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entity">сущность</param>
        public void Add<T>(T entity)
        {
            if (typeof(T) == typeof(Owner))
                AddOwner((Owner)(object)entity);
            else if (typeof(T) == typeof(Car))
                AddCar((Car)(object)entity);
        }

        /// <summary>
        /// Обобщающий метод для удаления одной сущности
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="id">id сущности</param>
        public void Delete<T>(int id)
        {
            if (typeof(T) == typeof(Owner))
                DeleteOwner(id);
            else if (typeof(T) == typeof(Car))
                DeleteCar(id);
        }

        /// <summary>
        /// Обобщающий метод для обновления одной сущности
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="updateEntity">сущность</param>
        public void Update<T>(T updateEntity)
        {
            if (typeof(T) == typeof(Owner))
                UpdateOwner((Owner)(object)updateEntity);
            else if (typeof(T) == typeof(Car))
                UpdateCar((Car)(object)updateEntity);
        }

        // ============ OWNER METHODS ============

        /// <summary>
        /// Метод для  чтения всех владельцев
        /// </summary>
        /// <returns>Список владельцев</returns>
        public List<Owner> ReadAllOwners()
        {
            var ownersRep = _ownerRepository.ReadAll();
            var result = new List<Owner>();

            foreach (var ownerRep in ownersRep)
            {
                var owner = ConvertToOwner(ownerRep);
                // Загружаем список машин владельца
                owner.IdCarsOwner = _ownerRepository.GetOwnerCars(owner.Id)
                    .Select(c => c.ID_car).ToList();
                result.Add(owner);
            }
            return result;
        }

        /// <summary>
        /// Метод для чтения одного владельца
        /// </summary>
        /// <param name="id">id владельца</param>
        /// <returns>Владелец</returns>
        public Owner ReadOwner(int id)
        {
            var ownerRep = _ownerRepository.ReadById(id);
            if (ownerRep == null) return null;

            var owner = ConvertToOwner(ownerRep);
            owner.IdCarsOwner = _ownerRepository.GetOwnerCars(owner.Id)
                .Select(c => c.ID_car).ToList();
            return owner;
        }

        /// <summary>
        /// Метод для добавления владельца
        /// </summary>
        /// <param name="owner">Владелец</param>
        public void AddOwner(Owner owner)
        {
            var ownerRep = ConvertToOwnerRep(owner);
            _ownerRepository.Add(ownerRep);
            owner.Id = ownerRep.ID_owner; // Обновляем ID

            // Добавляем связи с машинами
            foreach (var carId in owner.IdCarsOwner)
            {
                _ownerRepository.AddCarToOwner(owner.Id, carId);
            }
        }

        /// <summary>
        /// Метод для обновления владельца
        /// </summary>
        /// <param name="owner">владелец</param>
        public void UpdateOwner(Owner owner)
        {
            var ownerRep = ConvertToOwnerRep(owner);
            _ownerRepository.Update(ownerRep);

            // Обновляем связи с машинами
            var currentCars = _ownerRepository.GetOwnerCars(owner.Id).Select(c => c.ID_car).ToList();

            // Добавляем новые связи
            foreach (var carId in owner.IdCarsOwner.Except(currentCars))
            {
                _ownerRepository.AddCarToOwner(owner.Id, carId);
            }

            // Удаляем старые связи
            foreach (var carId in currentCars.Except(owner.IdCarsOwner))
            {
                _ownerRepository.RemoveCarFromOwner(owner.Id, carId);
            }
        }

        /// <summary>
        /// Удаление владельца
        /// </summary>
        /// <param name="id">id владельца</param>
        public void DeleteOwner(int id)
        {
            _ownerRepository.Delete(id);
        }

        // ============ CAR METHODS ============

        /// <summary>
        /// Метод для  чтения всех машин
        /// </summary>
        /// <returns>Список машин</returns>
        public List<Car> ReadAllCars()
        {
            return _carRepository.ReadAll().Select(ConvertToCar).ToList();
        }

        /// <summary>
        /// Метод для чтения одной машины
        /// </summary>
        /// <param name="id">id машины</param>
        /// <returns>Машина</returns>
        public Car ReadCar(int id)
        {
            var carRep = _carRepository.ReadById(id);
            return carRep == null ? null : ConvertToCar(carRep);
        }

        /// <summary>
        /// Метод для добавления одной машины
        /// </summary>
        /// <param name="car">машина</param>
        public void AddCar(Car car)
        {
            var carRep = ConvertToCarRep(car);
            _carRepository.Add(carRep);
            car.Id = carRep.ID_car;
        }

        /// <summary>
        /// Метод для обновления машины
        /// </summary>
        /// <param name="car">машина</param>
        public void UpdateCar(Car car)
        {
            var carRep = ConvertToCarRep(car);
            _carRepository.Update(carRep);
        }

        /// <summary>
        /// Удаление машины
        /// </summary>
        /// <param name="id">id машины</param>
        public void DeleteCar(int id)
        {
            _carRepository.Delete(id);
        }

        // ============ RELATIONSHIP METHODS ============

        /// <summary>
        /// добавление машины владельцу
        /// </summary>
        /// <param name="ownerId">id владельца</param>
        /// <param name="carId">id машины</param>
        public void AddCarToOwner(int ownerId, int carId)
        {
            _ownerRepository.AddCarToOwner(ownerId, carId);
        }

        /// <summary>
        /// удаление машины у владельца
        /// </summary>
        /// <param name="ownerId">id владельца</param>
        /// <param name="carId">id машины</param>
        public void RemoveCarFromOwner(int ownerId, int carId)
        {
            _ownerRepository.RemoveCarFromOwner(ownerId, carId);
        }

        /// <summary>
        /// получение машин владельца
        /// </summary>
        /// <param name="ownerId">id владельца</param>
        public List<Car> GetOwnerCars(int ownerId)
        {
            return _ownerRepository.GetOwnerCars(ownerId).Select(ConvertToCar).ToList();
        }

        /// <summary>
        /// получение владельца машины
        /// </summary>
        /// <param name="carId">id машины</param>
        public List<Owner> GetCarOwners(int carId)
        {
            return _carRepository.GetCarOwners(carId).Select(ConvertToOwner).ToList();
        }

        // ============ CONVERSION METHODS ============

        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="ownerRep">интерфейс владельца</param>
        /// <returns>объект Owner</returns>
        private static Owner ConvertToOwner(OwnerRep ownerRep)
        {
            return new Owner
            {
                Id = ownerRep.ID_owner,
                Name = ownerRep.Name,
                Year = ownerRep.Year,
                ExperienceYear = ownerRep.ExperienceYear
            };
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="owner">владелец</param>
        /// <returns>объект OwnerRep</returns>
        private static OwnerRep ConvertToOwnerRep(Owner owner)
        {
            return new OwnerRep
            {
                ID_owner = owner.Id,
                Name = owner.Name,
                Year = owner.Year,
                ExperienceYear = owner.ExperienceYear
            };
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="carRep">интерфейс машины</param>
        /// <returns>объект Car</returns>
        private static Car ConvertToCar(CarRep carRep)
        {
            return new Car
            {
                Id = carRep.ID_car,  // ← Маппим ID_car → Id,
                Brand = carRep.Brand,
                Model = carRep.Model,
                Year = carRep.Year,
                IdOwner = carRep.IdOwner,
                Price = carRep.Price
            };
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="car">машина</param>
        /// <returns>объект CarRep</returns>
        private static CarRep ConvertToCarRep(Car car)
        {
            return new CarRep
            {
                ID_car = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                IdOwner = car.IdOwner,
                Price = car.Price
            };
        }

        // ============ BUSINESS METHODS ============

        /// <summary>
        /// Создание владельца
        /// </summary>
        /// <param name="name">имя владельца</param>
        /// <param name="year">возраст</param>
        /// <param name="experienceYear">стаж</param>
        /// <returns>Владелец</returns>
        public static Owner CreateOwner(string name, int year, int experienceYear)
        {
            var owner = new Owner
            {
                Name = name,
                Year = year,
                ExperienceYear = experienceYear
            };
            return owner;
        }

        /// <summary>
        /// Создание машины
        /// </summary>
        /// <param name="brand">бранд авто</param>
        /// <param name="model">марка авто</param>
        /// <param name="year">год выпуска</param>
        /// <param name="price">цена</param>
        /// <returns>Машина</returns>
        public static Car CreateCar(string brand, string model, int year, decimal price)
        {
            var car = new Car
            {
                Brand = brand,
                Model = model,
                Year = year,
                Price = price
            };
            return car;
        }

        /// <summary>
        /// Сортировка машин по году выпуска
        /// </summary>
        /// <param name="minYear">год начиная скоторого будут выбираться машины</param>
        /// <returns>Список машин, отсортированный по году выпуска авто</returns>
        public List<Car> SortByYear(int minYear)
        {
            var cars = ReadAllCars();
            return cars.Where(car => car.Year >= minYear).ToList();
        }

        /// <summary>
        /// Сортировка машин по бренду
        /// </summary>
        /// <param name="brand">бранд авто</param>
        /// <returns>Список машин, отсортированный по бренду авто</returns>
        public List<Car> GetCarsByBrand(string brand)
        {
            var cars = ReadAllCars();
            return cars.Where(car => car.Brand == brand).ToList();
        }

        /// <summary>
        /// Расчет стоимости всех машин
        /// </summary>
        /// <param name="cars">список машин</param>
        /// <returns>Стоимость всех машин</returns>
        public static decimal GetCarsPrice(List<Car> cars)
        {
            return cars.Sum(c => c.Price);
        }
    }
}
