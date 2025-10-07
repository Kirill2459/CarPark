using DataAccessLayer;
using DataAccessLayer.Dapper;
using DataAccessLayer.EntityFrameWork;
using Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;


namespace Model
{
    public class Logic
    {
        private static IOwnerRepository _ownerRepository;
        private static ICarRepository _carRepository;
        private static string _connectionString = "Data Source = HonorPC\\SQLEXPRESS;Initial Catalog = CarPark; Integrated Security = True; MultipleActiveResultSets=True";


        //"Data Source = HonorPC\\SQLEXPRESS;Initial Catalog = CarPark; Integrated Security = True; Encrypt=True"

        static Logic()
        {
            //_ownerRepository = new DapperOwnerRepository(_connectionString);
            //_carRepository = new DapperCarRepository(_connectionString);


            DBContext dbContext = new DBContext(_connectionString);
            _ownerRepository = new EntityOwnerRepository(dbContext);
            _carRepository = new EntityCarRepository(dbContext);
        }

        public static List<T> ReadAll<T>()
        {
            if (typeof(T) == typeof(Owner))
                return ReadAllOwners().Cast<T>().ToList();
            else if (typeof(T) == typeof(Car))
                return ReadAllCars().Cast<T>().ToList();

            return new List<T>();
        }

        public static T Read<T>(int id)
        {
            if (typeof(T) == typeof(Owner))
                return (T)(object)ReadOwner(id);
            else if (typeof(T) == typeof(Car))
                return (T)(object)ReadCar(id);

            return default(T);
        }

        public static void Add<T>(T entity)
        {
            if (typeof(T) == typeof(Owner))
                AddOwner((Owner)(object)entity);
            else if (typeof(T) == typeof(Car))
                AddCar((Car)(object)entity);
        }

        public static void Delete<T>(int id)
        {
            if (typeof(T) == typeof(Owner))
                DeleteOwner(id);
            else if (typeof(T) == typeof(Car))
                DeleteCar(id);
        }

        public static void Update<T>(T updateEntity)
        {
            if (typeof(T) == typeof(Owner))
                UpdateOwner((Owner)(object)updateEntity);
            else if (typeof(T) == typeof(Car))
                UpdateCar((Car)(object)updateEntity);
        }

        // ============ OWNER METHODS ============

        public static List<Owner> ReadAllOwners()
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

        public static Owner ReadOwner(int id)
        {
            var ownerRep = _ownerRepository.ReadById(id);
            if (ownerRep == null) return null;

            var owner = ConvertToOwner(ownerRep);
            owner.IdCarsOwner = _ownerRepository.GetOwnerCars(owner.Id)
                .Select(c => c.ID_car).ToList();
            return owner;
        }

        public static void AddOwner(Owner owner)
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

        public static void UpdateOwner(Owner owner)
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

        public static void DeleteOwner(int id)
        {
            _ownerRepository.Delete(id);
        }

        // ============ CAR METHODS ============

        public static List<Car> ReadAllCars()
        {
            return _carRepository.ReadAll().Select(ConvertToCar).ToList();
        }

        public static Car ReadCar(int id)
        {
            var carRep = _carRepository.ReadById(id);
            return carRep == null ? null : ConvertToCar(carRep);
        }

        public static void AddCar(Car car)
        {
            var carRep = ConvertToCarRep(car);
            _carRepository.Add(carRep);
            car.Id = carRep.ID_car;
        }

        public static void UpdateCar(Car car)
        {
            var carRep = ConvertToCarRep(car);
            _carRepository.Update(carRep);
        }

        public static void DeleteCar(int id)
        {
            _carRepository.Delete(id);
        }

        // ============ RELATIONSHIP METHODS ============

        public static void AddCarToOwner(int ownerId, int carId)
        {
            _ownerRepository.AddCarToOwner(ownerId, carId);
        }

        public static void RemoveCarFromOwner(int ownerId, int carId)
        {
            _ownerRepository.RemoveCarFromOwner(ownerId, carId);
        }

        public static List<Car> GetOwnerCars(int ownerId)
        {
            return _ownerRepository.GetOwnerCars(ownerId).Select(ConvertToCar).ToList();
        }

        public static List<Owner> GetCarOwners(int carId)
        {
            return _carRepository.GetCarOwners(carId).Select(ConvertToOwner).ToList();
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
                Id = carRep.ID_car,  // ← Маппим ID_car → Id,
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

        // ============ BUSINESS METHODS ============

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

        public static List<Car> SortByYear(int minYear)
        {
            var cars = ReadAllCars();
            return cars.Where(car => car.Year >= minYear).ToList();
        }

        public static List<Car> GetCarsByBrand(string brand)
        {
            var cars = ReadAllCars();
            return cars.Where(car => car.Brand == brand).ToList();
        }

        public static decimal GetCarsPrice(List<Car> cars)
        {
            return cars.Sum(c => c.Price);
        }
    }
}
