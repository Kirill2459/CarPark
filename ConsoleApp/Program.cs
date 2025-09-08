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
        static void Main(string[] args)
        {
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
                Console.WriteLine("6. Узнать стоимость автопарка.");
                Console.WriteLine("7. Отсортировать машины по бренду.");
                Console.WriteLine("0. Назад в главное меню");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        List<Car> cars = Logic.ReadAll<Car>();
                        if (cars.Count == 0)
                        {
                            Console.WriteLine("Машин нет");
                        }
                        else
                        {
                            foreach (Car car in cars)
                            {
                                Console.WriteLine($"Id: {car.Id}. {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                            }
                        }
                        break;
                    case "2":
                        try
                        {
                            Console.Write("Марка: ");
                            string brand = Console.ReadLine() ?? "";
                            Console.Write("Модель: ");
                            string model = Console.ReadLine() ?? "";
                            Console.Write("Год: ");
                            int year = int.Parse(Console.ReadLine() ?? "0");
                            Console.Write("Цена(руб): ");
                            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

                            Car car = Logic.CreateCar(brand, model, year, price);
                            Logic.Add<Car>(car);
                            Console.WriteLine($"Добавлена: {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;
                    case "3":
                        try
                        {
                            Console.Write("Введите Id машины которую вы хотели бы изменить: ");
                            int id = int.Parse(Console.ReadLine() ?? "0");

                            Car car = Logic.Read<Car>(id);

                            if (car != null)
                            {
                                Console.WriteLine("Введите новые свойства для машины: ");
                                Console.Write("Марка: ");
                                string brand = Console.ReadLine() ?? "";
                                Console.Write("Модель: ");
                                string model = Console.ReadLine() ?? "";
                                Console.Write("Год: ");
                                int year = int.Parse(Console.ReadLine() ?? "0");
                                Console.Write("Цена(руб): ");
                                decimal price = decimal.Parse(Console.ReadLine() ?? "0");

                                Car newCar = Logic.CreateCar(brand, model, year, price);
                                newCar.Id = id;
                                Logic.Update<Car>(newCar);
                                Console.WriteLine($"Автомобиль изменен: {newCar.Brand} {newCar.Model}, {newCar.Year} года, - {newCar.Price} руб");
                            }
                            else
                            {
                                Console.Write("В автопарке нет автомобиля с таким Id.");

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;
                    case "4":
                        List<Car> vintageCars= Logic.GetVintageCars();
                        Console.WriteLine("Винтажные машины:");
                        if (vintageCars.Count == 0)
                        {
                            Console.WriteLine("Машин нет");
                        }
                        else
                        {
                            foreach (Car car in vintageCars)
                            {
                                Console.WriteLine($"Id: {car.Id}. {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                            }
                        }
                        break;
                    case "5":
                        try
                        {
                            Console.Write("Введите Id машины которую вы хотели бы удалить: ");
                            int id = int.Parse(Console.ReadLine() ?? "0");

                            Car car = Logic.Read<Car>(id);

                            if (car != null)
                            {
                                Logic.Delete<Car>(id);
                                Console.WriteLine($"Автомобиль удален.");
                            }
                            else
                            {
                                Console.Write("В автопарке нет автомобиля с таким Id.");

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;
                    case "6":
                        decimal costCarPark = Logic.AllPrice();
                        Console.WriteLine($"Стоимость всего автопарка составляет {costCarPark} руб.");
                        break;
                    case "7":
                        Console.Write("Введите автомобильный бренд: ");
                        string userBrand = Console.ReadLine();

                        List<Car> carsBrand = Logic.GetCarsByBrand(userBrand);

                        if (carsBrand.Count() == 0)
                        {
                            Console.Write("В автопарке нет машин с таким брендом.");
                        }
                        else
                        {
                            foreach (Car car in carsBrand)
                            {
                                Console.WriteLine($"Id: {car.Id}. {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                            }
                        }
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
