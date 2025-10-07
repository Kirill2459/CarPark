using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataAccessLayer.EntityFrameWork
{
    public class EntityCarRepository : ICarRepository
    {
        private readonly DBContext _context;

        public EntityCarRepository(DBContext context)
        {
            _context = context;
        }

        public void Add(CarRep item)
        {
            _context.Set<CarRep>().Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            CarRep carRep = _context.Cars.Where(o => o.ID_car == id).FirstOrDefault();
            _context.Set<CarRep>().Remove(carRep);
            _context.SaveChanges();
        }

        public IEnumerable<CarRep> ReadAll()
        {
            return new List<CarRep>(_context.Cars);
        }

        public CarRep ReadById(int id)
        {
            return _context.Cars.Where(o => o.ID_car == id).FirstOrDefault();
        }

        public void Update(CarRep item)
        {
            // Находим существующую машину
            var existingCar = _context.Cars.FirstOrDefault(c => c.ID_car == item.ID_car);

            if (existingCar == null)
            {
                throw new ArgumentException($"Машина с ID {item.ID_car} не найдена");
            }

            // Обновляем свойства существующей машины
            existingCar.Brand = item.Brand;
            existingCar.Model = item.Model;
            existingCar.Year = item.Year;
            existingCar.Price = item.Price;
            existingCar.IdOwner = item.IdOwner;

            _context.SaveChanges();
        }

        public IEnumerable<OwnerRep> GetCarOwners(int carId)
        {
            OwnerCar ownerCar = _context.OwnerCars.Where(o => o.ID_car == carId).FirstOrDefault();
            OwnerRep ownerRep = _context.Owners.Where(o => o.ID_owner == ownerCar.ID_owner).FirstOrDefault();

            IEnumerable <OwnerRep> ownersRep = new List<OwnerRep>() { ownerRep };

            return ownersRep;
        }
    }
}
