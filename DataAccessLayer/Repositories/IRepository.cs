using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// добавление сущности
        /// </summary>
        /// <param name="item">сущность</param>
        void Add(T item);

        /// <summary>
        /// удаление сущности по id
        /// </summary>
        /// <param name="id">id сущности</param>
        void Delete(int id);

        /// <summary>
        /// чтение всех сущностей
        /// </summary>
        /// <returns>Список сущностей</returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// чтение сущности по id
        /// </summary>
        /// <param name="id">id сущности</param>
        /// <returns>Сущность</returns>
        T ReadById(int id);

        /// <summary>
        /// обновление сущности
        /// </summary>
        /// <param name="item">сущность</param>
        void Update(T item);
    }
}
