using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ICarRepository : IRepository<CarRep>
    {
        /// <summary>
        /// получение владельца машины
        /// </summary>
        /// <param name="carId">id машины</param>
        /// <returns>Список владельцев</returns>
        IEnumerable<OwnerRep> GetCarOwners(int carId);
    }
}
