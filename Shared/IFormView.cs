using System;
using Model.Entities;
using System.Collections.Generic;


namespace Shared
{
    public interface IFormView
    {
        // Методы для обновления данных на форме
        void DisplayCars(List<Car> cars);
        void DisplayOwners(List<Owner> owners);
        void DisplayFreeCars(List<Car> freeCars);
        void DisplayOwnerCars(List<Car> ownerCars);

        // События для пользовательских действий
        event Action ShowAllCarsRequested;
        event Action<int> FindOldCarsRequested;
        event Action<string> FindCarsByBrandRequested;
        event Action<int> DeleteCarRequested;
        event Action CalculateCarsPriceRequested;
        event Action<int> DeleteOwnerRequested;
        event Action<int> ShowOwnerCarsRequested;
        event Action<int, int> AddCarToOwnerRequested;
        event Action<int> UpdateCarRequested;
        event Action AddCarRequested;
        event Action AddOwnerRequested;

        // Методы для показа сообщений
        void ShowMessage(string message);
        void ShowError(string error);

        // Методы для получения данных форм
        int GetSelectedCarId();
        int GetSelectedOwnerId();
    }
}
