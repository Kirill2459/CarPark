using DataAccessLayer;
using Ninject.Modules;
using DataAccessLayer.EntityFrameWork;
using DataAccessLayer.Dapper;


namespace Model
{
    public class SimpleConfigModule : NinjectModule
    {
        //Изменяйте только эту часть (HOME-PC или HONORPC) во избежание ошибок
        private static string _connectionString = "Data Source = HOME-PC\\SQLEXPRESS;Initial Catalog = CarPark; Integrated Security = True; MultipleActiveResultSets=True";

        public override void Load()
        {
            // Регистрируем Logic как реализацию ILogicService
            Bind<ILogicService>().To<Logic>().InSingletonScope();

            // Для Dapper
            Bind<IOwnerRepository>().To<DapperOwnerRepository>().InSingletonScope()
                .WithConstructorArgument("connectionString", _connectionString);
            Bind<ICarRepository>().To<DapperCarRepository>().InSingletonScope()
                .WithConstructorArgument("connectionString", _connectionString);

            // ИЛИ для Entity Framework
            //Bind<string>().ToConstant(_connectionString).WhenInjectedInto<DBContext>();
            //Bind<IOwnerRepository>().To<EntityOwnerRepository>().InSingletonScope();
            //Bind<ICarRepository>().To<EntityCarRepository>().InSingletonScope();
        }
    }
}
