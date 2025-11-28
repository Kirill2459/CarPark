using ConsoleApp;
using Microsoft.SqlServer.Server;
using Model;
using Ninject;
using Shared;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsFormsApp;


namespace Presenter
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ АВТОПАРКОМ ===");
            Console.WriteLine("Выберите тип интерфейса:");
            Console.WriteLine("1 - Windows Forms (графический)");
            Console.WriteLine("2 - Console (консольный)");


            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RunWindowsFormsApp();
                    break;
                case "2":
                    RunConsoleApp();
                    break;
            }
        }

        static void RunWindowsFormsApp()
        {
            Console.WriteLine("Запуск Windows Forms приложения...");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Настройка DI
                IKernel kernel = new StandardKernel(new SimpleConfigModule());

                // Создание View (Form1)
                var view = new Form1();

                // Создание Model
                var logic = kernel.Get<ILogicService>();

                // Создание и запуск Presenter'а
                var presenter = new CommonPresenter(view, logic);

                Console.WriteLine("Приложение запущено. Окно откроется через 2 секунды...");
                System.Threading.Thread.Sleep(2000);

                // Запуск Windows Forms
                Application.Run(view);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска Windows Forms: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        static void RunConsoleApp()
        {
            Console.WriteLine("Запуск консольного приложения...");

            try
            {
                // Настройка DI
                IKernel kernel = new StandardKernel(new SimpleConfigModule());

                // Создание Console View
                var consoleView = new ConsoleView();

                // Создание Model
                var logic = kernel.Get<ILogicService>();

                // Создание и запуск Presenter'а
                var presenter = new CommonPresenter(consoleView, logic);

                Console.WriteLine("Приложение готово к работе...\n");

                // Запуск консольного приложения
                consoleView.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска консольного приложения: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}