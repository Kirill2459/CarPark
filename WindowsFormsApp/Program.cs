using System;
using System.Windows.Forms;
using Presenter;
using Model;
using Ninject;

namespace WindowsFormsApp
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IKernel kernel = new StandardKernel(new SimpleConfigModule());

            var view = new Form1();
            var logic = kernel.Get<ILogicService>();  // Получаем по интерфейсу

            var presenter = new FormPresenter(view, logic);

            Application.Run(view);
        }
    }
}
