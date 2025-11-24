using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using Shared;

namespace Presenter
{
    public class FormPresenter
    {
        private readonly IFormView _view;
        private readonly ILogicService _logic;

        public FormPresenter(IFormView view, ILogicService logic)
        {
            _view = view;
            _logic = logic;

            SubscribeToViewEvents();
            InitializeView();
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
        }

        private void InitializeView()
        {
            try
            {
                var owners = _logic.ReadAll<Owner>();
                var freeCars = _logic.ReadAll<Car>().Where(c => c.IdOwner == null).ToList();

                _view.DisplayOwners(owners);
                _view.DisplayFreeCars(freeCars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка инициализации: {ex.Message}");
            }
        }

        private void OnShowAllCarsRequested()
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

        private void OnFindOldCarsRequested(int minYear)
        {
            try
            {
                var cars = _logic.SortByYear(minYear);
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
                var cars = _logic.GetCarsByBrand(brand);
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
                List<Owner> owners = _logic.ReadAll<Owner>();
                Car car = _logic.Read<Car>(carId);

                if (car != null)
                {
                    _logic.Delete<Car>(carId);
                    foreach (Owner owner in owners)
                    {
                        // Проверяем содержит ли владелец эту машину
                        if (owner.IdCarsOwner.Contains(carId))
                        {
                            // Удаляем ID машины из списка владельца
                            owner.IdCarsOwner.Remove(carId);
                            _logic.Update(owner);

                            _view.ShowMessage($"Машина ID:{carId} удалена у владельца {owner.Name}");
                        }
                    }
                    _view.ShowMessage($"Автомобиль удален.");
                }
                else
                {
                    _view.ShowError("В автопарке нет автомобиля с таким Id.");
                }

                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnCalculateCarsPriceRequested()
        {
            try
            {
                var currentCars = _logic.ReadAll<Car>();
                decimal allPrice = _logic.GetCarsPrice(currentCars);
                _view.ShowMessage($"Стоимость текущих машин составляет {allPrice} рублей.");
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
                Owner owner = _logic.Read<Owner>(ownerId);
                List<Car> cars = _logic.ReadAll<Car>();

                if (owner != null)
                {
                    _logic.Delete<Owner>(ownerId);

                    foreach (Car car in cars)
                    {
                        if (car.IdOwner == ownerId)
                        {
                            car.IdOwner = null;
                            _logic.Update(car);
                        }
                    }
                    _view.ShowMessage($"Владелец удален.");
                }
                else
                {
                    _view.ShowError("Нет владельца с таким Id.");
                }

                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnShowOwnerCarsRequested(int ownerId)
        {
            try
            {
                Owner owner = _logic.Read<Owner>(ownerId);
                List<Car> cars = _logic.ReadAll<Car>();

                bool carFound = false;
                List<Car> carsOwnedByOwners = new List<Car>();

                if (owner != null)
                {
                    foreach (Car car in cars)
                    {
                        if (car.IdOwner == ownerId)
                        {
                            carsOwnedByOwners.Add(car);
                            carFound = true;
                        }
                    }

                    _view.DisplayOwnerCars(carsOwnedByOwners);

                    if (!carFound)
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
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnAddCarToOwnerRequested(int ownerId, int carId)
        {
            try
            {
                Owner owner = _logic.Read<Owner>(ownerId);
                List<Car> freeCars = _logic.ReadAll<Car>().Where(car => car.IdOwner == null).ToList();

                if (owner != null)
                {
                    if (freeCars.Count != 0)
                    {
                        bool carFound = false;

                        foreach (Car freeCar in freeCars)
                        {
                            if (freeCar.Id == carId)
                            {
                                owner.IdCarsOwner.Add(carId);
                                _logic.Update(owner);
                                freeCar.IdOwner = ownerId;
                                _logic.Update(freeCar);

                                _view.ShowMessage($"Успешно! Машина {freeCar.Model} добавлена владельцу {owner.Name}");
                                carFound = true;
                                break;
                            }
                        }

                        if (!carFound)
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

                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnUpdateCarRequested(int carId)
        {
            try
            {
                Car car = _logic.Read<Car>(carId);

                if (car != null)
                {
                    RefreshAllData();
                }
                else
                {
                    _view.ShowError("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnAddCarRequested()
        {
            try
            {
                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void OnAddOwnerRequested()
        {
            try
            {
                RefreshAllData();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка: {ex.Message}");
            }
        }

        private void RefreshAllData()
        {
            try
            {
                var owners = _logic.ReadAll<Owner>();
                var cars = _logic.ReadAll<Car>();
                var freeCars = cars.Where(car => car.IdOwner == null).ToList();

                _view.DisplayOwners(owners);
                _view.DisplayCars(cars);
                _view.DisplayFreeCars(freeCars);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка обновления данных: {ex.Message}");
            }
        }
    }
}
