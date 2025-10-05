using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dapper
{
    public class DapperCarRepository : ICarRepository
    {
        private readonly string _connectionString;

        public DapperCarRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(CarRep item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Cars (Brand, Model, Year, Price, IdOwner) VALUES (@Brand, @Model, @Year, @Price, @IdOwner); SELECT CAST(SCOPE_IDENTITY() as int)";
                item.ID_car = db.QuerySingle<int>(sqlQuery, item);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute("DELETE FROM OwnerCars WHERE ID_car = @id", new { id });
                db.Execute("DELETE FROM Cars WHERE ID_car = @id", new { id });
            }
        }

        public IEnumerable<CarRep> ReadAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<CarRep>("SELECT * FROM Cars");
            }
        }

        public CarRep ReadById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.QueryFirstOrDefault<CarRep>("SELECT * FROM Cars WHERE ID_car = @id", new { id });
            }
        }

        public void Update(CarRep item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Cars SET Brand = @Brand, Model = @Model, Year = @Year, Price = @Price, IdOwner = @IdOwner WHERE ID_car = @ID_car";
                db.Execute(sqlQuery, item);
            }
        }

        public IEnumerable<OwnerRep> GetCarOwners(int carId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = @"
                    SELECT o.* FROM Owners o
                    INNER JOIN OwnerCars oc ON o.Id = oc.ID_owner
                    WHERE oc.ID_car = @carId";

                return db.Query<OwnerRep>(sqlQuery, new { carId });
            }
        }



    }
}
