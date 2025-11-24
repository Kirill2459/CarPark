using Ninject;
using Model;
using Shared;
using Presenter;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Настройка Dependency Injection
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());

            // Создание компонентов MVP
            var view = new ConsoleView();
            var logic = ninjectKernel.Get<ILogicService>();
            var presenter = new ConsolePresenter(view, logic);

            // Запуск приложения
            view.Start();
        }
    }
}