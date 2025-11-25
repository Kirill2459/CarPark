using System;
using System.Collections.Generic;
using Model.Entities;

namespace Shared
{
    public interface ICommonView
    {
        // Общие методы отображения
        void DisplayCars(List<Car> cars);
        void DisplayOwners(List<Owner> owners);
        void DisplayFreeCars(List<Car> freeCars);
        void DisplayOwnerCars(List<Car> ownerCars);
        void ShowMessage(string message);
        void ShowError(string error);

        // Общие события
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
        event Action ShowOwnersRequested;
    }
}