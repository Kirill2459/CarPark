using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Dapper;
using DataAccessLayer;
using Ninject.Modules;
using DataAccessLayer.EntityFrameWork;
using Ninject;

namespace Model
{
    public class SimpleConfigModule : NinjectModule
    {
        //Изменяйте только эту часть (HOME-PC) во избежание ошибок
        private static string _connectionString = "Data Source = HOME-PC\\SQLEXPRESS;Initial Catalog = CarPark; Integrated Security = True; MultipleActiveResultSets=True";
        
        public override void Load()
        {
            // Для Dapper
            //Bind<IOwnerRepository>().To<DapperOwnerRepository>().InSingletonScope()
            //    .WithConstructorArgument("connectionString", _connectionString);
            //Bind<ICarRepository>().To<DapperCarRepository>().InSingletonScope()
            //    .WithConstructorArgument("connectionString", _connectionString);

            // ИЛИ для Entity Framework
            Bind<string>().ToConstant(_connectionString).WhenInjectedInto<DBContext>();
            Bind<IOwnerRepository>().To<EntityOwnerRepository>().InSingletonScope();
            Bind<ICarRepository>().To<EntityCarRepository>().InSingletonScope();
        }
    }
}
