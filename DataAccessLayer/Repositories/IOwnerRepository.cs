using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataAccessLayer
{
    public interface IOwnerRepository : IRepository<OwnerRep>
    {
        void AddCarToOwner(int ownerId, int carId);
        void RemoveCarFromOwner(int ownerId, int carId);
        IEnumerable<CarRep> GetOwnerCars(int ownerId);
    }
}


