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
            CarRep carRep = _context.Cars.Where(o => o.ID_car == item.ID_car).FirstOrDefault();
            Delete(item.ID_car);
            Add(item);
            _context.SaveChanges();
        }

        public IEnumerable<OwnerRep> GetCarOwners(int carId)
        {
            OwnerCar ownerCar = _context.OwnerCars.Where(o => o.CarId == carId).FirstOrDefault();
            OwnerRep ownerRep = _context.Owners.Where(o => o.ID_owner == ownerCar.OwnerId).FirstOrDefault();

            IEnumerable <OwnerRep> ownersRep = new List<OwnerRep>() { ownerRep };

            return ownersRep;
        }
    }
}
