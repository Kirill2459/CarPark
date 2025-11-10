using Model.Entities;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Ninject;

using System.Web.UI.WebControls;

namespace ConsoleApp
{
    internal class Program
    {
        private static Logic Logic;
        
        static void Main(string[] args)
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            Logic = ninjectKernel.Get<Logic>();

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
                Console.WriteLine("4. Сортировать машины по году");
                Console.WriteLine("5. Удалить машину");
                Console.WriteLine("6. Узнать стоимость автопарка.");
                Console.WriteLine("7. Отсортировать машины по бренду.");
                Console.WriteLine("0. Назад в главное меню");
                Console.Write("Выберите действие: ");
                

                var choice = Console.ReadLine();
                Console.WriteLine("");

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
                            string brand = Console.ReadLine();
                            Console.Write("Модель: ");
                            string model = Console.ReadLine();
                            Console.Write("Год: ");
                            int year = int.Parse(Console.ReadLine());
                            Console.Write("Цена(руб): ");
                            decimal price = decimal.Parse(Console.ReadLine());

                            Car car = Logic.CreateCar(brand, model, year, price);
                            Logic.Add(car);
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
                                string brand = Console.ReadLine();
                                Console.Write("Модель: ");
                                string model = Console.ReadLine();
                                Console.Write("Год: ");
                                int year = int.Parse(Console.ReadLine());
                                Console.Write("Цена(руб): ");
                                decimal price = decimal.Parse(Console.ReadLine());

                                Car newCar = Logic.CreateCar(brand, model, year, price);
                                newCar.Id = id;
                                Logic.Update(newCar);
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
                        Console.Write("Введите год: ");
                        int yearForSort = int.Parse(Console.ReadLine());
                        List<Car> carsSortedByYear= Logic.SortByYear(yearForSort);
                        Console.WriteLine($"Машины произведеный после {yearForSort} года:");
                        if (carsSortedByYear.Count == 0)
                        {
                            Console.WriteLine("Машин нет");
                        }
                        else
                        {
                            foreach (Car car in carsSortedByYear)
                            {
                                Console.WriteLine($"Id: {car.Id}. {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                            }
                            Console.WriteLine();
                            Console.WriteLine($"Суммарная стоимость: {Logic.GetCarsPrice(carsSortedByYear)}");
                        }
                        break;
                    case "5":
                        try
                        {
                            Console.Write("Введите Id машины которую вы хотели бы удалить: ");
                            int id = int.Parse(Console.ReadLine() ?? "0");

                            List<Owner> owners = Logic.ReadAll<Owner>();
                            Car car = Logic.Read<Car>(id);

                            if (car != null)
                            {
                                Logic.Delete<Car>(id);
                                foreach (Owner owner in owners)
                                {
                                    // Проверяем содержит ли владелец эту машину
                                    if (owner.IdCarsOwner.Contains(id))
                                    {
                                        // Удаляем ID машины из списка владельца
                                        owner.IdCarsOwner.Remove(id);
                                        Logic.Update(owner); // Обновляем владельца в json

                                        Console.WriteLine($"Машина ID:{id} удалена у владельца {owner.Name}");
                                    }
                                }
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
                        List<Car> carsForGetCarsPrice = Logic.ReadAll<Car>();
                        decimal costCarPark = Logic.GetCarsPrice(carsForGetCarsPrice);
                        Console.WriteLine($"Стоимость всего автопарка составляет {costCarPark} руб.");
                        break;
                    case "7":
                        Console.Write("Введите марку автомобиля: ");
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
                Console.WriteLine("");

                switch (choice)
                {
                    case "1":
                        List<Owner> owners = Logic.ReadAll<Owner>();
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
                        break;
                    case "2":
                        try
                        {
                            Console.Write("Имя: ");
                            string name;

                            while (string.IsNullOrWhiteSpace(name = Console.ReadLine()) || name.Any(char.IsDigit))
                            {
                                Console.Write("Имя не может быть пустым или содержать цифры. Введите имя: ");
                            }

                            Console.Write("Возраст: ");
                            int year;
                            while (true)
                            {
                                if (int.TryParse(Console.ReadLine(), out year))
                                {
                                    if (year > 105)
                                    {
                                        Console.Write("Возраст не может быть таким большим. Повторите ввод: ");
                                    }
                                    else
                                    {
                                        break; // Ввод корректен
                                    }
                                }
                                else
                                {
                                    Console.Write("Некорректный ввод. Введите число для возраста: ");
                                }
                            }
                            

                            Console.Write("Стаж вождения: ");
                            int expYear;
                            while (true)
                            {
                                if (int.TryParse(Console.ReadLine(), out expYear))
                                {
                                    if (expYear < 0)
                                    {
                                        Console.Write("Стаж не может быть отрицательным. Повторите ввод: ");
                                    }
                                    else if (expYear > 80)
                                    {
                                        Console.Write("Стаж не может быть больше 80 лет. Повторите ввод: ");
                                    }
                                    else if (expYear > year - 18) // Проверка логики
                                    {
                                        Console.Write($"Стаж не может быть больше чем возраст минус 18 лет ({year - 18}). Повторите ввод: ");
                                    }
                                    else
                                    {
                                        break; // Ввод корректен
                                    }
                                }
                                else
                                {
                                    Console.Write("Некорректный ввод. Введите число для стажа: ");
                                }
                            }

                            Owner owner = Logic.CreateOwner(name, year, expYear);
                            Logic.Add(owner);
                            Console.WriteLine($"Добавлен: {owner.Id}. {owner.Name}, возраст:{owner.Year}, стаж:{owner.ExperienceYear}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }

                        break;
                    case "3":
                        try
                        { 
                            Console.Write("Введите номер ID владельца: ");
                            int ownerID = int.Parse(Console.ReadLine());

                            Owner owner = Logic.Read<Owner>(ownerID);

                            if (owner != null)
                            {
                                Console.WriteLine($"Вы выбрали владельца: {owner.Name}.");

                                Console.WriteLine("Список свободных машин:");
                                List<Car> cars = Logic.ReadAll<Car>();
                                List<Car> freeCars = cars.Where(car => car.IdOwner == null).ToList();

                                if (freeCars.Count != 0)
                                {
                                    foreach (Car freeCar in freeCars)
                                    {
                                        Console.WriteLine($"Id: {freeCar.Id}. {freeCar.Brand} {freeCar.Model}, {freeCar.Year} года, - {freeCar.Price} руб");
                                    }

                                    Console.Write("Введите ID машины, которую хотите добавить: ");
                                    int carID = int.Parse(Console.ReadLine());

                                    bool carFound = false; // Флаг для отслеживания найденной машины

                                    foreach (Car freeCar in freeCars)
                                    {
                                        if(freeCar.Id == carID)
                                        {
                                            owner.IdCarsOwner.Add(carID); // добавляем id машины владельцу
                                            Logic.Update(owner);
                                            freeCar.IdOwner = ownerID; // добавляем id владельца машине
                                            Logic.Update(freeCar);


                                            Console.WriteLine($"Успешно! Машина {freeCar.Model} добавлена владельцу {owner.Name}");
                                            carFound = true;
                                            break;

                                        }                                                                                                                              
                                    }
                                    if (!carFound)
                                    {
                                        Console.WriteLine("Этой машины нет или она занята");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Свободных машин нет");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Владелей с таким Id не зарегистрирован.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;
                    case "4":
                        try
                        {
                            Console.Write("Введите номер ID владельца: ");
                            int ownerID = int.Parse(Console.ReadLine());

                            Owner owner = Logic.Read<Owner>(ownerID);
                            List<Car> cars = Logic.ReadAll<Car>();

                            bool carFound = false;

                            if (owner != null)
                            {
                                Console.WriteLine($"Владельцу: {owner.Name} принадлежат машины:");
                                foreach (Car car in cars)
                                {
                                    if (car.IdOwner == ownerID)
                                    {
                                        Console.WriteLine($"Id: {car.Id}. {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                                        carFound = true;                                       
                                    }
                                }
                                if (!carFound)
                                {
                                    Console.WriteLine("У этого владельца нет машин.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Владелей с таким Id не зарегистрирован.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }

                        break;
                    case "5":
                        try
                        {
                            Console.Write("Введите Id владельца, которого вы хотели бы удалить: ");
                            int idOwner = int.Parse(Console.ReadLine() ?? "0");

                            Owner owner = Logic.Read<Owner>(idOwner);
                            List<Car> cars = Logic.ReadAll<Car>();

                            if (owner != null)
                            {
                                Logic.Delete<Owner>(idOwner); // удаление владельца

                                foreach (Car car in cars)
                                {
                                    if (car.IdOwner == idOwner)
                                    {
                                        car.IdOwner = null; // делаем машину владельца свободной
                                        Logic.Update(car); // сохраняем изменения в json
                                    }
                                }
                                Console.WriteLine($"Владелец удален.");
                            }
                            else
                            {
                                Console.Write("Нет владельца с таким Id.");

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
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
    }
}
