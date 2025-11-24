using System.Collections.Generic;
using Model.Entities;

namespace Model
{
    public interface ILogicService
    {
        List<T> ReadAll<T>();
        T Read<T>(int id);
        void Add<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(int id);

        // Специфичные методы
        List<Car> SortByYear(int minYear);
        List<Car> GetCarsByBrand(string brand);
        decimal GetCarsPrice(List<Car> cars);
        void AddCarToOwner(int ownerId, int carId);
        void RemoveCarFromOwner(int ownerId, int carId);
        List<Car> GetOwnerCars(int ownerId);
        List<Owner> GetCarOwners(int carId);
    }
}
