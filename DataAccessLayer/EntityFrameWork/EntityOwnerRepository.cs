using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer.EntityFrameWork
{
    public class EntityOwnerRepository : IOwnerRepository
    {
        private readonly DBContext _context;

        public EntityOwnerRepository(DBContext context)
        {
            _context = context;
        }

        public void Add(OwnerRep item)
        {
            _context.Set<OwnerRep>().Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            OwnerRep ownerRep = _context.Owners.Where(o => o.ID_owner == id).FirstOrDefault();

            //Удаление связей между владельцем и его машинами
            IEnumerable<CarRep> carsRep = GetOwnerCars(ownerRep.ID_owner);
            foreach (CarRep car in carsRep)
            {
                RemoveCarFromOwner(ownerRep.ID_owner, car.ID_car);
            }

            _context.Set<OwnerRep>().Remove(ownerRep);
            _context.SaveChanges();
        }

        public void Update(OwnerRep item)
        {
            OwnerRep ownerRep = _context.Owners.Where(o => o.ID_owner == item.ID_owner).FirstOrDefault();
            Delete(item.ID_owner);
            Add(item);
            _context.SaveChanges();
        }

        public IEnumerable<OwnerRep> ReadAll()
        {
            return new List<OwnerRep>(_context.Set<OwnerRep>());
        }

        public OwnerRep ReadById(int id)
        {
            return _context.Owners.Where(o => o.ID_owner == id).FirstOrDefault();
        }



        public void AddCarToOwner(int ownerId, int carId)
        {
            OwnerCar ownerCar = new OwnerCar() { OwnerId = ownerId, CarId = carId };
            _context.Set<OwnerCar>().Add(ownerCar);
            _context.SaveChanges();
        }

        public IEnumerable<CarRep> GetOwnerCars(int ownerId)
        {
            var carsID = _context.OwnerCars.Where(o => o.OwnerId == ownerId).Select(o => o.CarId);
            List<CarRep> carsRep = new List<CarRep>();
            foreach(var carID in carsID)
            {
                carsRep.Add(_context.Cars.Where(o => o.ID_car == carID).FirstOrDefault());
            }
            return carsRep;
        }

        public void RemoveCarFromOwner(int ownerId, int carId)
        {
            OwnerCar ownerCar =  _context.OwnerCars.Where(o => o.OwnerId == ownerId && o.CarId == carId).FirstOrDefault();
            if (ownerCar != null)
            {
                _context.Set<OwnerCar>().Remove(ownerCar);
                _context.SaveChanges();
            }
        }


    }
}
