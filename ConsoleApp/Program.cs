using Model.Entities;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {

        private static Logic.ICarService _carService;
        private static Logic.IOwnerService _ownerService;
        static void Main(string[] args)
        {
            _carService = new Logic.CarService();
            _ownerService = new Logic.OwnerService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ АВТОПАРКОМ ===");
                Console.WriteLine("1. Действия с машинами");
                Console.WriteLine("2. Действия с владельцами");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CarMenu();
                        break;
                    case "2":
                        OwnerMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void CarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ДЕЙСТВИЯ С МАШИНАМИ ===");
                Console.WriteLine("1. Показать все машины");
                Console.WriteLine("2. Добавить машину");
                Console.WriteLine("3. Изменить машину");
                Console.WriteLine("4. Найти старые машины");
                Console.WriteLine("5. Удалить машину");
                Console.WriteLine("0. Назад в главное меню");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var cars = _carService.GetAllCars();
                        if (cars.Count == 0)
                        {
                            Console.WriteLine("Машин нет");
                        }
                        else
                        {
                            foreach (var car in cars)
                            {
                                Console.WriteLine($"{car.Id}){car.Brand},{car.Model} {car.Year} года, - {car.Price} руб");
                            }
                        }
                        break;
                    case "2":
                        try
                        {
                            Console.Write("Марка: ");
                            var brand = Console.ReadLine() ?? "";
                            Console.Write("Модель: ");
                            var model = Console.ReadLine() ?? "";
                            Console.Write("Год: ");
                            var year = int.Parse(Console.ReadLine() ?? "0");
                            Console.Write("Цена(руб): ");
                            var price = decimal.Parse(Console.ReadLine() ?? "0");

                            var car = _carService.CreateCar(brand, model, year, price);
                            Console.WriteLine($"Добавлена: {car.Brand},{car.Model} {car.Year} года, - {car.Price} руб");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;
                    case "3":
                        
                        break;
                    case "4":
                        
                        break;
                    case "5":
                        
                        break;
                    case "0":
                        return; // Возврат в главное меню
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void OwnerMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ДЕЙСТВИЯ С ВЛАДЕЛЬЦАМИ ===");
                Console.WriteLine("1. Показать всех владельцев");
                Console.WriteLine("2. Добавить владельца");
                Console.WriteLine("3. Добавить машину владельцу");
                Console.WriteLine("4. Показать машины владельца");
                Console.WriteLine("5. Удалить владельца");
                Console.WriteLine("0. Назад в главное меню");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        
                        break;
                    case "2":
                        
                        break;
                    case "3":
                        
                        break;
                    case "4":
                        
                        break;
                    case "5":
                        
                        break;
                    case "0":
                        return; // Возврат в главное меню
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}
