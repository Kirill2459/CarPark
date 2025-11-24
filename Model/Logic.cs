using DataAccessLayer;
//using DataAccessLayer.Dapper;
//using DataAccessLayer.EntityFrameWork;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Remoting.Contexts;


namespace Model
{
    public class Logic : ILogicService  // Добавляем реализацию интерфейса
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICarRepository _carRepository;

        public Logic(IOwnerRepository ownerRepository, ICarRepository carRepository)
        {
            _ownerRepository = ownerRepository;
            _carRepository = carRepository;
        }

        // ============ ОБОБЩЕННЫЕ МЕТОДЫ ИНТЕРФЕЙСА ============

        /// <summary>
        /// Обобщающий метод для чтения всех сущностей
        /// </summary>
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
        public void Update<T>(T updateEntity)
        {
            if (typeof(T) == typeof(Owner))
                UpdateOwner((Owner)(object)updateEntity);
            else if (typeof(T) == typeof(Car))
                UpdateCar((Car)(object)updateEntity);
        }

        // ============ СПЕЦИФИЧНЫЕ МЕТОДЫ ИНТЕРФЕЙСА ============

        /// <summary>
        /// Сортировка машин по году выпуска
        /// </summary>
        public List<Car> SortByYear(int minYear)
        {
            var cars = ReadAllCars();
            return cars.Where(car => car.Year >= minYear).ToList();
        }

        /// <summary>
        /// Сортировка машин по бренду
        /// </summary>
        public List<Car> GetCarsByBrand(string brand)
        {
            var cars = ReadAllCars();
            return cars.Where(car => car.Brand == brand).ToList();
        }

        /// <summary>
        /// Расчет стоимости всех машин
        /// </summary>
        public decimal GetCarsPrice(List<Car> cars)
        {
            return cars.Sum(c => c.Price);
        }

        /// <summary>
        /// добавление машины владельцу
        /// </summary>
        public void AddCarToOwner(int ownerId, int carId)
        {
            _ownerRepository.AddCarToOwner(ownerId, carId);
        }

        /// <summary>
        /// удаление машины у владельца
        /// </summary>
        public void RemoveCarFromOwner(int ownerId, int carId)
        {
            _ownerRepository.RemoveCarFromOwner(ownerId, carId);
        }

        /// <summary>
        /// получение машин владельца
        /// </summary>
        public List<Car> GetOwnerCars(int ownerId)
        {
            return _ownerRepository.GetOwnerCars(ownerId).Select(ConvertToCar).ToList();
        }

        /// <summary>
        /// получение владельца машины
        /// </summary>
        public List<Owner> GetCarOwners(int carId)
        {
            return _carRepository.GetCarOwners(carId).Select(ConvertToOwner).ToList();
        }

        // ============ OWNER METHODS ============

        /// <summary>
        /// Метод для чтения всех владельцев
        /// </summary>
        public List<Owner> ReadAllOwners()
        {
            var ownersRep = _ownerRepository.ReadAll();
            var result = new List<Owner>();

            foreach (var ownerRep in ownersRep)
            {
                var owner = ConvertToOwner(ownerRep);
                owner.IdCarsOwner = _ownerRepository.GetOwnerCars(owner.Id)
                    .Select(c => c.ID_car).ToList();
                result.Add(owner);
            }
            return result;
        }

        /// <summary>
        /// Метод для чтения одного владельца
        /// </summary>
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
        public void AddOwner(Owner owner)
        {
            var ownerRep = ConvertToOwnerRep(owner);
            _ownerRepository.Add(ownerRep);
            owner.Id = ownerRep.ID_owner;

            foreach (var carId in owner.IdCarsOwner)
            {
                _ownerRepository.AddCarToOwner(owner.Id, carId);
            }
        }

        /// <summary>
        /// Метод для обновления владельца
        /// </summary>
        public void UpdateOwner(Owner owner)
        {
            var ownerRep = ConvertToOwnerRep(owner);
            _ownerRepository.Update(ownerRep);

            var currentCars = _ownerRepository.GetOwnerCars(owner.Id).Select(c => c.ID_car).ToList();

            foreach (var carId in owner.IdCarsOwner.Except(currentCars))
            {
                _ownerRepository.AddCarToOwner(owner.Id, carId);
            }

            foreach (var carId in currentCars.Except(owner.IdCarsOwner))
            {
                _ownerRepository.RemoveCarFromOwner(owner.Id, carId);
            }
        }

        /// <summary>
        /// Удаление владельца
        /// </summary>
        public void DeleteOwner(int id)
        {
            _ownerRepository.Delete(id);
        }

        // ============ CAR METHODS ============

        /// <summary>
        /// Метод для чтения всех машин
        /// </summary>
        public List<Car> ReadAllCars()
        {
            return _carRepository.ReadAll().Select(ConvertToCar).ToList();
        }

        /// <summary>
        /// Метод для чтения одной машины
        /// </summary>
        public Car ReadCar(int id)
        {
            var carRep = _carRepository.ReadById(id);
            return carRep == null ? null : ConvertToCar(carRep);
        }

        /// <summary>
        /// Метод для добавления одной машины
        /// </summary>
        public void AddCar(Car car)
        {
            var carRep = ConvertToCarRep(car);
            _carRepository.Add(carRep);
            car.Id = carRep.ID_car;
        }

        /// <summary>
        /// Метод для обновления машины
        /// </summary>
        public void UpdateCar(Car car)
        {
            var carRep = ConvertToCarRep(car);
            _carRepository.Update(carRep);
        }

        /// <summary>
        /// Удаление машины
        /// </summary>
        public void DeleteCar(int id)
        {
            _carRepository.Delete(id);
        }

        // ============ CONVERSION METHODS ============

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

        private static Car ConvertToCar(CarRep carRep)
        {
            return new Car
            {
                Id = carRep.ID_car,
                Brand = carRep.Brand,
                Model = carRep.Model,
                Year = carRep.Year,
                IdOwner = carRep.IdOwner,
                Price = carRep.Price
            };
        }

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

        // ============ STATIC BUSINESS METHODS ============

        /// <summary>
        /// Создание владельца
        /// </summary>
        public static Owner CreateOwner(string name, int year, int experienceYear)
        {
            return new Owner
            {
                Name = name,
                Year = year,
                ExperienceYear = experienceYear
            };
        }

        /// <summary>
        /// Создание машины
        /// </summary>
        public static Car CreateCar(string brand, string model, int year, decimal price)
        {
            return new Car
            {
                Brand = brand,
                Model = model,
                Year = year,
                Price = price
            };
        }
    }
}