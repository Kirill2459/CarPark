using System;
using System.Collections.Generic;
using Model.Entities;

namespace Shared
{
    public interface IConsoleView
    {
        // События для пользовательских действий
        event Action ShowCarsRequested;
        event Action ShowOwnersRequested;
        event Action AddCarRequested;
        event Action UpdateCarRequested;
        event Action DeleteCarRequested;
        event Action SortCarsByYearRequested;
        event Action SortCarsByBrandRequested;
        event Action CalculateCarsPriceRequested;
        event Action AddOwnerRequested;
        event Action AddCarToOwnerRequested;
        event Action ShowOwnerCarsRequested;
        event Action DeleteOwnerRequested;
        event Action ExitRequested;

        // Методы для отображения данных
        void DisplayCars(List<Car> cars);
        void DisplayOwners(List<Owner> owners);
        void DisplayOwnerCars(List<Car> cars, Owner owner);
        void DisplayFreeCars(List<Car> freeCars);
        void ShowMessage(string message);
        void ShowError(string error);

        // Методы для ввода данных
        string ReadString(string prompt);
        int ReadInt(string prompt);
        decimal ReadDecimal(string prompt);
        int ReadCarId();
        int ReadOwnerId();
        Car ReadCarData();
        Owner ReadOwnerData();

        // Методы для меню
        void ShowMainMenu();
        void ShowCarMenu();
        void ShowOwnerMenu();
    }
}
