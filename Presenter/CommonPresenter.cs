using System;
using System.Diagnostics;
using System.Linq;
using Model;
using Model.Entities;
using Shared;
using DataTransferObject;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Presenter
{
    public class CommonPresenter
    {
        private readonly ICommonView _view;
        private readonly ILogicService _logic;

        public CommonPresenter(ICommonView view, ILogicService logic)
        {
            _view = view;
            _logic = logic;

            SubscribeToViewEvents();

            // Инициализация только для FormView (консоль сама управляет меню)
            if (view is IFormView)
            {
                InitializeView();
            }
        }

        private void SubscribeToViewEvents()
        {
            _view.ShowAllCarsRequested += OnShowAllCarsRequested;
            _view.FindOldCarsRequested += OnFindOldCarsRequested;
            _view.FindCarsByBrandRequested += OnFindCarsByBrandRequested;
            _view.DeleteCarRequested += OnDeleteCarRequested;
            _view.CalculateCarsPriceRequested += OnCalculateCarsPriceRequested;
            _view.DeleteOwnerRequested += OnDeleteOwnerRequested;
            _view.ShowOwnerCarsRequested += OnShowOwnerCarsRequested;
            _view.AddCarToOwnerRequested += OnAddCarToOwnerRequested;
            _view.UpdateCarRequested += OnUpdateCarRequested;
            _view.AddCarRequested += OnAddCarRequested;
            _view.AddOwnerRequested += OnAddOwnerRequested;
            _view.ShowOwnersRequested += OnShowOwnersRequested;

            // Консоль-специфичные события
            if (_view is IConsoleView consoleView)
            {
                consoleView.ExitRequested += OnExitRequested;
            }
        }

        private void InitializeView()
        {
            try
            {
                var owners = ConvertInOwnersDTO(_logic.ReadAll<Owner>());
                var freeCars = ConvertInCarsDTO(_logic.ReadAll<Car>().Where(c => c.IdOwner == null).ToList());

                _view.DisplayOwners(owners);
                _view.DisplayFreeCars(freeCars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка инициализации: {ex.Message}");
            }
        }

        // === ОБЩИЕ МЕТОДЫ ОБРАБОТКИ СОБЫТИЙ ===

        private void OnShowAllCarsRequested()
        {
            try
            {
                var cars = ConvertInCarsDTO(_logic.ReadAll<Car>());
                _view.DisplayCars(cars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка загрузки машин: {ex.Message}");
            }
        }

        private void OnFindOldCarsRequested(int minYear)
        {
            try
            {
                var cars = ConvertInCarsDTO(_logic.SortByYear(minYear));
                _view.DisplayCars(cars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка поиска машин: {ex.Message}");
            }
        }

        private void OnFindCarsByBrandRequested(string brand)
        {
            try
            {
                var cars = ConvertInCarsDTO(_logic.GetCarsByBrand(brand));
                _view.DisplayCars(cars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка поиска по бренду: {ex.Message}");
            }
        }

        private void OnDeleteCarRequested(int carId)
        {
            try
            {
                var car = _logic.Read<Car>(carId);

                if (car != null)
                {
                    // Удаляем связи с владельцами
                    var owners = _logic.ReadAll<Owner>();
                    foreach (var owner in owners.Where(o => o.IdCarsOwner.Contains(carId)))
                    {
                        owner.IdCarsOwner.Remove(carId);
                        _logic.Update(owner);

                        if (_view is IConsoleView)
                        {
                            _view.ShowMessage($"Машина ID:{carId} удалена у владельца {owner.Name}");
                        }
                    }

                    _logic.Delete<Car>(carId);
                    _view.ShowMessage("Автомобиль удален.");
                    RefreshAllData();
                }
                else
                {
                    _view.ShowError("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка удаления: {ex.Message}");
            }
        }

        private void OnCalculateCarsPriceRequested()
        {
            try
            {
                var cars = _logic.ReadAll<Car>();
                decimal totalPrice = _logic.GetCarsPrice(cars);

                if (_view is IConsoleView)
                {
                    _view.ShowMessage($"Стоимость всего автопарка составляет {totalPrice} руб.");
                }
                else
                {
                    _view.ShowMessage($"Стоимость текущих машин составляет {totalPrice} рублей.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка расчета стоимости: {ex.Message}");
            }
        }

        private void OnDeleteOwnerRequested(int ownerId)
        {
            try
            {
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
                    RefreshAllData();
                }
                else
                {
                    _view.ShowError("Нет владельца с таким Id.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка удаления владельца: {ex.Message}");
            }
        }

        private void OnShowOwnersRequested()
        {
            try
            {
                var owners = ConvertInOwnersDTO(_logic.ReadAll<Owner>());
                _view.DisplayOwners(owners);

            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при загрузке владельцев: {ex.Message}");
            }
        }

        private void OnShowOwnerCarsRequested(int ownerId)
        {
            try
            {
                var owner = _logic.Read<Owner>(ownerId);
                var ownerCars = ConvertInCarsDTO(_logic.GetOwnerCars(ownerId));

                if (owner != null)
                {
                    _view.DisplayOwnerCars(ownerCars);

                    if (!ownerCars.Any() && _view is IConsoleView)
                    {
                        _view.ShowMessage("У этого владельца нет машин.");
                    }
                }
                else
                {
                    _view.ShowError("Владелец с таким Id не зарегистрирован.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка загрузки машин владельца: {ex.Message}");
            }
        }

        private void OnAddCarToOwnerRequested(int ownerId, int carId)
        {
            try
            {
                var owner = _logic.Read<Owner>(ownerId);
                var car = _logic.Read<Car>(carId);

                if (owner == null)
                {
                    _view.ShowError("Владелец не найден");
                    return;
                }

                if (car == null || car.IdOwner != null)
                {
                    _view.ShowError("Этой машины нет или она занята");
                    return;
                }

                // Добавляем связь
                owner.IdCarsOwner.Add(carId);
                car.IdOwner = ownerId;

                _logic.Update(owner);
                _logic.Update(car);

                _view.ShowMessage($"Успешно! Машина {car.Model} добавлена владельцу {owner.Name}");
                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка добавления машины владельцу: {ex.Message}");
            }
        }

        private void OnUpdateCarRequested(int carId)
        {
            try
            {
                var car = _logic.Read<Car>(carId);
                if (car != null)
                {
                    // Для консоли - запросить данные и обновить
                    if (_view is IConsoleView consoleView)
                    {
                        _view.ShowMessage("Введите новые свойства для машины:");
                        var newCarDTO = consoleView.ReadCarData();

                        Car newCar = new Car()
                        {
                            Brand = newCarDTO.Brand,
                            Model = newCarDTO.Model,
                            Year = newCarDTO.Year,
                            Price = newCarDTO.Price
                        };

                        newCar.Id = carId;
                        newCar.IdOwner = car.IdOwner;

                        _logic.Update(newCar);
                        _view.ShowMessage($"Автомобиль изменен: {newCar.Brand} {newCar.Model}, {newCar.Year} года, - {newCar.Price} руб");
                    }

                    RefreshAllData();
                }
                else
                {
                    _view.ShowError("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка обновления автомобиля: {ex.Message}");
            }
        }

        private void OnAddCarRequested()
        {
            try
            {
                // Для консоли - запросить данные и добавить
                if (_view is IConsoleView consoleView)
                {
                    var carDTO = consoleView.ReadCarData();

                    Car car = new Car() {
                        Brand = carDTO.Brand,
                        Model = carDTO.Model,
                        Year = carDTO.Year,
                        Price = carDTO.Price
                    };

                    _logic.Add(car);
                    _view.ShowMessage($"Добавлена: {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
                }

                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка добавления автомобиля: {ex.Message}");
            }
        }

        private void OnAddOwnerRequested()
        {
            try
            {
                // Для консоли - запросить данные и добавить
                if (_view is IConsoleView consoleView)
                {
                    var owner = consoleView.ReadOwnerData();
                    _logic.Add(owner);
                    _view.ShowMessage($"Добавлен: {owner.Name}, возраст:{owner.Year}, стаж:{owner.ExperienceYear}");
                }

                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка добавления владельца: {ex.Message}");
            }
        }

        private void OnExitRequested()
        {
            _view.ShowMessage("Выход из приложения...");
            Environment.Exit(0);
        }

        private void RefreshAllData()
        {
            try
            {
                var owners = ConvertInOwnersDTO(_logic.ReadAll<Owner>());
                var cars = ConvertInCarsDTO(_logic.ReadAll<Car>());
                var freeCars = cars.Where(c => c.IdOwner == null).ToList();

                _view.DisplayOwners(owners);
                _view.DisplayCars(cars);
                _view.DisplayFreeCars(freeCars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка обновления данных: {ex.Message}");
            }
        }


        /// <summary>
        /// Проебразуем список объектов Car в список объектов CarDTO
        /// </summary>
        /// <param name="cars">список объектов Car</param>
        /// <returns>список объектов CarDTO</returns>
        public List<CarDTO> ConvertInCarsDTO(List<Car> cars)
        {
            List<CarDTO> carsDTO = new List<CarDTO>();
            foreach(Car car in cars)
            {
                CarDTO carDTO = new CarDTO(car.Id, car.Brand, car.Model, car.Year, car.Price, car.IdOwner);
                carsDTO.Add(carDTO);
            }

            return carsDTO;
        }

        /// <summary>
        /// Проебразуем список объектов Owner в список объектов OwnerDTO
        /// </summary>
        /// <param name="owners">список объектов Owner</param>
        /// <returns>список объектов OwnerDTO</returns>
        public List<OwnerDTO> ConvertInOwnersDTO(List<Owner> owners)
        {
            List<OwnerDTO> ownersDTO = new List<OwnerDTO>();
            foreach (Owner owner in owners)
            {
                OwnerDTO ownerDTO = new OwnerDTO(owner.Id, owner.Name, owner.Year, owner.ExperienceYear, owner.IdCarsOwner);
                ownersDTO.Add(ownerDTO);
            }

            return ownersDTO;
        }
    }
}
