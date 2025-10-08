using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataAccessLayer
{
    public interface IOwnerRepository : IRepository<OwnerRep>
    {

        /// <summary>
        /// добавление машины владельцу
        /// </summary>
        /// <param name="ownerId">id владельца</param>
        /// <param name="carId">id машины</param>
        void AddCarToOwner(int ownerId, int carId);

        /// <summary>
        /// удаление машины у владельца
        /// </summary>
        /// <param name="ownerId">id владельца</param>
        /// <param name="carId">id машины</param>
        void RemoveCarFromOwner(int ownerId, int carId);

        /// <summary>
        /// получение машин владельца
        /// </summary>
        /// <param name="ownerId">id владельца</param>
        /// <returns>Список машин владельца</returns>
        IEnumerable<CarRep> GetOwnerCars(int ownerId);
    }
}


