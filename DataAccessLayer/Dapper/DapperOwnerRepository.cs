using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccessLayer.Dapper
{
    public class DapperOwnerRepository : IOwnerRepository
    {
        private readonly string _connectionString;

        public DapperOwnerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(OwnerRep item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Owners (Name, Year, ExperienceYear) VALUES (@Name, @Year, @ExperienceYear); SELECT CAST(SCOPE_IDENTITY() as int)";
                item.ID_owner = db.QuerySingle<int>(sqlQuery, item);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute("DELETE FROM OwnerCars WHERE ID_owner = @id", new { id });
                db.Execute("DELETE FROM Owners WHERE ID_owner = @id", new { id });
            }
        }

        public IEnumerable<OwnerRep> ReadAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<OwnerRep>("SELECT * FROM Owners");
            }
        }

        public OwnerRep ReadById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.QueryFirstOrDefault<OwnerRep>("SELECT * FROM Owners WHERE ID_owner = @id", new { id });
            }
        }

        public void Update(OwnerRep item)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Owners SET Name = @Name, Year = @Year, ExperienceYear = @ExperienceYear WHERE ID_owner = @ID_owner";
                db.Execute(sqlQuery, item);
            }
        }

        public void AddCarToOwner(int ownerId, int carId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO OwnerCars (ID_owner, ID_car) VALUES (@ownerId, @carId)";
                db.Execute(sqlQuery, new { ownerId, carId });
            }
        }

        public void RemoveCarFromOwner(int ownerId, int carId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute("DELETE FROM OwnerCars WHERE ID_owner = @ownerId AND ID_car = @carId", new { ownerId, carId });
            }
        }

        public IEnumerable<CarRep> GetOwnerCars(int ownerId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = @"
                    SELECT c.* FROM Cars c
                    INNER JOIN OwnerCars oc ON c.ID_car = oc.ID_car
                    WHERE oc.ID_owner = @ownerId";

                return db.Query<CarRep>(sqlQuery, new { ownerId });
            }
        }

    }
}
