using Model.Entities;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;


namespace ConsoleApp
{
    public class ConsoleView : IConsoleView
    {
        // События из ICommonView
        public event Action ShowAllCarsRequested;
        public event Action<int> FindOldCarsRequested;
        public event Action<string> FindCarsByBrandRequested;
        public event Action<int> DeleteCarRequested;
        public event Action CalculateCarsPriceRequested;
        public event Action<int> DeleteOwnerRequested;
        public event Action<int> ShowOwnerCarsRequested;
        public event Action<int, int> AddCarToOwnerRequested;
        public event Action<int> UpdateCarRequested;
        public event Action AddCarRequested;
        public event Action AddOwnerRequested;

        // События из IConsoleView
        public event Action ExitRequested;

        // Старые события (нужно заменить на новые из интерфейса)
        public event Action ShowCarsRequested
        {
            add { ShowAllCarsRequested += value; }
            remove { ShowAllCarsRequested -= value; }
        }

        public event Action ShowOwnersRequested;
        public event Action SortCarsByYearRequested;
        public event Action SortCarsByBrandRequested;

        public void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
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
                        ShowCarMenu();
                        break;
                    case "2":
                        ShowOwnerMenu();
                        break;
                    case "0":
                        ExitRequested?.Invoke();
                        return;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }
            }
        }

        public void ShowCarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ДЕЙСТВИЯ С МАШИНАМИ ===");
                Console.WriteLine("1. Показать все машины");
                Console.WriteLine("2. Добавить машину");
                Console.WriteLine("3. Изменить машину");
                Console.WriteLine("4. Найти старые машины");
                Console.WriteLine("5. Найти машины по марке");
                Console.WriteLine("6. Удалить машину");
                Console.WriteLine("7. Узнать стоимость автопарка");
                Console.WriteLine("0. Назад в главное меню");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllCarsRequested?.Invoke();
                        break;
                    case "2":
                        AddCarRequested?.Invoke();
                        break;
                    case "3":
                        var carId = ReadCarId();
                        UpdateCarRequested?.Invoke(carId);
                        break;
                    case "4":
                        var year = ReadInt("Введите год (машины старше этого года): ");
                        FindOldCarsRequested?.Invoke(year);
                        break;
                    case "5":
                        var brand = ReadString("Введите марку машины: ");
                        FindCarsByBrandRequested?.Invoke(brand);
                        break;
                    case "6":
                        var deleteCarId = ReadCarId();
                        DeleteCarRequested?.Invoke(deleteCarId);
                        break;
                    case "7":
                        CalculateCarsPriceRequested?.Invoke();
                        break;
                    case "0":
                        return;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        public void ShowOwnerMenu()
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
                        ShowOwnersRequested?.Invoke();
                        break;
                    case "2":
                        AddOwnerRequested?.Invoke();
                        break;
                    case "3":
                        var ownerId = ReadOwnerId();
                        var carId = ReadCarId();
                        AddCarToOwnerRequested?.Invoke(ownerId, carId);
                        break;
                    case "4":
                        var showOwnerId = ReadOwnerId();
                        ShowOwnerCarsRequested?.Invoke(showOwnerId);
                        break;
                    case "5":
                        var deleteOwnerId = ReadOwnerId();
                        DeleteOwnerRequested?.Invoke(deleteOwnerId);
                        break;
                    case "0":
                        return;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        // Методы для отображения данных из ICommonView
        public void DisplayCars(List<Car> cars)
        {
            Console.WriteLine();
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
        }

        public void DisplayOwners(List<Owner> owners)
        {
            Console.WriteLine();
            if (owners.Count == 0)
            {
                Console.WriteLine("Владельцев нет");
            }
            else
            {
                foreach (Owner owner in owners)
                {
                    Console.WriteLine($"Id: {owner.Id}. {owner.Name}, возраст:{owner.Year}, стаж:{owner.ExperienceYear}");
                }
            }
        }

        public void DisplayOwnerCars(List<Car> ownerCars)
        {
            Console.WriteLine();
            if (ownerCars.Count == 0)
            {
                Console.WriteLine("У владельца нет машин.");
            }
            else
            {
                foreach (Car car in ownerCars)
                {
                    Console.WriteLine($"Id: {car.Id}. {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                }
            }
        }

        // Перегрузка метода для обратной совместимости
        public void DisplayOwnerCars(List<Car> cars, Owner owner)
        {
            if (owner != null)
            {
                Console.WriteLine($"\nВладельцу: {owner.Name} принадлежат машины:");
            }
            DisplayOwnerCars(cars);
        }

        public void DisplayFreeCars(List<Car> freeCars)
        {
            Console.WriteLine();
            if (freeCars.Count == 0)
            {
                Console.WriteLine("Свободных машин нет");
            }
            else
            {
                Console.WriteLine("Свободные машины:");
                foreach (Car freeCar in freeCars)
                {
                    Console.WriteLine($"Id: {freeCar.Id}. {freeCar.Brand} {freeCar.Model}, {freeCar.Year} года, - {freeCar.Price} руб");
                }
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine($"\n{message}");
        }

        public void ShowError(string error)
        {
            Console.WriteLine($"\nОшибка: {error}");
        }

        // Методы для ввода данных из IConsoleView
        public string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;
                ShowError("Некорректный ввод. Введите целое число.");
            }
        }

        public decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal result))
                    return result;
                ShowError("Некорректный ввод. Введите число.");
            }
        }

        public int ReadCarId()
        {
            return ReadInt("Введите ID машины: ");
        }

        public int ReadOwnerId()
        {
            return ReadInt("Введите ID владельца: ");
        }

        public Car ReadCarData()
        {
            string brand = ReadString("Марка: ");
            string model = ReadString("Модель: ");
            int year = ReadInt("Год: ");
            decimal price = ReadDecimal("Цена(руб): ");

            return Logic.CreateCar(brand, model, year, price);
        }

        public Owner ReadOwnerData()
        {
            string name;
            while (string.IsNullOrWhiteSpace(name = ReadString("Имя: ")) || name.Any(char.IsDigit))
            {
                ShowError("Имя не может быть пустым или содержать цифры.");
            }

            int year;
            while (true)
            {
                year = ReadInt("Возраст: ");
                if (year > 105)
                {
                    ShowError("Возраст не может быть таким большим.");
                }
                else
                {
                    break;
                }
            }

            int expYear;
            while (true)
            {
                expYear = ReadInt("Стаж вождения: ");
                if (expYear < 0)
                {
                    ShowError("Стаж не может быть отрицательным.");
                }
                else if (expYear > 80)
                {
                    ShowError("Стаж не может быть больше 80 лет.");
                }
                else if (expYear > year - 18)
                {
                    ShowError($"Стаж не может быть больше чем возраст минус 18 лет ({year - 18}).");
                }
                else
                {
                    break;
                }
            }

            return Logic.CreateOwner(name, year, expYear);
        }
    }
}
