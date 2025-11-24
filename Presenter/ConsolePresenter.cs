using System;
using System.Linq;
using Model;
using Model.Entities;
using Shared;

namespace Presenter
{
    public class ConsolePresenter
    {
        private readonly IConsoleView _view;
        private readonly ILogicService _logic;

        public ConsolePresenter(IConsoleView view, ILogicService logic)
        {
            _view = view;
            _logic = logic;

            SubscribeToViewEvents();
        }

        private void SubscribeToViewEvents()
        {
            _view.ShowCarsRequested += OnShowCarsRequested;
            _view.ShowOwnersRequested += OnShowOwnersRequested;
            _view.AddCarRequested += OnAddCarRequested;
            _view.UpdateCarRequested += OnUpdateCarRequested;
            _view.DeleteCarRequested += OnDeleteCarRequested;
            _view.SortCarsByYearRequested += OnSortCarsByYearRequested;
            _view.SortCarsByBrandRequested += OnSortCarsByBrandRequested;
            _view.CalculateCarsPriceRequested += OnCalculateCarsPriceRequested;
            _view.AddOwnerRequested += OnAddOwnerRequested;
            _view.AddCarToOwnerRequested += OnAddCarToOwnerRequested;
            _view.ShowOwnerCarsRequested += OnShowOwnerCarsRequested;
            _view.DeleteOwnerRequested += OnDeleteOwnerRequested;
            _view.ExitRequested += OnExitRequested;
        }

        // Все методы обработчики событий (как в предыдущем ответе)
        private void OnShowCarsRequested()
        {
            try
            {
                var cars = _logic.ReadAll<Car>();
                _view.DisplayCars(cars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка загрузки машин: {ex.Message}");
            }
        }

        private void OnShowOwnersRequested()
        {
            try
            {
                var owners = _logic.ReadAll<Owner>();
                _view.DisplayOwners(owners);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка загрузки владельцев: {ex.Message}");
            }
        }

        private void OnAddCarRequested()
        {
            try
            {
                var car = _view.ReadCarData();
                _logic.Add(car);
                _view.ShowMessage($"Добавлена: {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка добавления машины: {ex.Message}");
            }
        }

        private void OnUpdateCarRequested()
        {
            try
            {
                int id = _view.ReadCarId();
                var car = _logic.Read<Car>(id);

                if (car != null)
                {
                    _view.ShowMessage("Введите новые свойства для машины:");
                    var newCar = _view.ReadCarData();
                    newCar.Id = id;
                    _logic.Update(newCar);
                    _view.ShowMessage($"Автомобиль изменен: {newCar.Brand} {newCar.Model}, {newCar.Year} года, - {newCar.Price} руб");
                }
                else
                {
                    _view.ShowError("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка обновления машины: {ex.Message}");
            }
        }

        private void OnDeleteCarRequested()
        {
            try
            {
                int id = _view.ReadCarId();
                var car = _logic.Read<Car>(id);

                if (car != null)
                {
                    // Удаляем связи с владельцами
                    var owners = _logic.ReadAll<Owner>();
                    foreach (var owner in owners.Where(o => o.IdCarsOwner.Contains(id)))
                    {
                        owner.IdCarsOwner.Remove(id);
                        _logic.Update(owner);
                        _view.ShowMessage($"Машина ID:{id} удалена у владельца {owner.Name}");
                    }

                    _logic.Delete<Car>(id);
                    _view.ShowMessage("Автомобиль удален.");
                }
                else
                {
                    _view.ShowError("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка удаления машины: {ex.Message}");
            }
        }

        private void OnSortCarsByYearRequested()
        {
            try
            {
                int year = _view.ReadInt("Введите год: ");
                var cars = _logic.SortByYear(year);
                _view.ShowMessage($"Машины произведенные после {year} года:");
                _view.DisplayCars(cars);

                if (cars.Any())
                {
                    decimal totalPrice = _logic.GetCarsPrice(cars);
                    _view.ShowMessage($"Суммарная стоимость: {totalPrice} руб");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка сортировки: {ex.Message}");
            }
        }

        private void OnSortCarsByBrandRequested()
        {
            try
            {
                string brand = _view.ReadString("Введите марку автомобиля: ");
                var cars = _logic.GetCarsByBrand(brand);

                if (cars.Any())
                {
                    _view.DisplayCars(cars);
                }
                else
                {
                    _view.ShowMessage("В автопарке нет машин с таким брендом.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка сортировки: {ex.Message}");
            }
        }

        private void OnCalculateCarsPriceRequested()
        {
            try
            {
                var cars = _logic.ReadAll<Car>();
                decimal totalPrice = _logic.GetCarsPrice(cars);
                _view.ShowMessage($"Стоимость всего автопарка составляет {totalPrice} руб.");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка расчета стоимости: {ex.Message}");
            }
        }

        private void OnAddOwnerRequested()
        {
            try
            {
                var owner = _view.ReadOwnerData();
                _logic.Add(owner);
                _view.ShowMessage($"Добавлен: {owner.Name}, возраст:{owner.Year}, стаж:{owner.ExperienceYear}");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка добавления владельца: {ex.Message}");
            }
        }

        private void OnAddCarToOwnerRequested()
        {
            try
            {
                int ownerId = _view.ReadOwnerId();
                var owner = _logic.Read<Owner>(ownerId);

                if (owner != null)
                {
                    _view.ShowMessage($"Вы выбрали владельца: {owner.Name}.");

                    // Показываем свободные машины
                    var freeCars = _logic.ReadAll<Car>().Where(c => c.IdOwner == null).ToList();
                    _view.DisplayFreeCars(freeCars);

                    if (freeCars.Any())
                    {
                        int carId = _view.ReadCarId();
                        var car = freeCars.FirstOrDefault(c => c.Id == carId);

                        if (car != null)
                        {
                            owner.IdCarsOwner.Add(carId);
                            _logic.Update(owner);

                            car.IdOwner = ownerId;
                            _logic.Update(car);

                            _view.ShowMessage($"Успешно! Машина {car.Model} добавлена владельцу {owner.Name}");
                        }
                        else
                        {
                            _view.ShowError("Этой машины нет или она занята");
                        }
                    }
                    else
                    {
                        _view.ShowError("Свободных машин нет");
                    }
                }
                else
                {
                    _view.ShowError("Владелец с таким Id не зарегистрирован.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnShowOwnerCarsRequested()
        {
            try
            {
                int ownerId = _view.ReadOwnerId();
                var owner = _logic.Read<Owner>(ownerId);
                var ownerCars = _logic.GetOwnerCars(ownerId);

                _view.DisplayOwnerCars(ownerCars, owner);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnDeleteOwnerRequested()
        {
            try
            {
                int ownerId = _view.ReadOwnerId();
                var owner = _logic.Read<Owner>(ownerId);

                if (owner != null)
                {
                    // Освобождаем машины владельца
                    var cars = _logic.ReadAll<Car>();
                    foreach (var car in cars.Where(c => c.IdOwner == ownerId))
                    {
                        car.IdOwner = null;
                        _logic.Update(car);
                    }

                    _logic.Delete<Owner>(ownerId);
                    _view.ShowMessage("Владелец удален.");
                }
                else
                {
                    _view.ShowError("Нет владельца с таким Id.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnExitRequested()
        {
            _view.ShowMessage("Выход из приложения...");
            Environment.Exit(0);
        }
    }
}
