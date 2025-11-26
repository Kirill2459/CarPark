using System;
using System.Collections.Generic;
using Model.Entities;
using DataTransferObject;

namespace Shared
{
    public interface IConsoleView : ICommonView
    {
        // Консоль-специфичные методы
        event Action ExitRequested;

        // Методы для ввода данных
        string ReadString(string prompt);
        int ReadInt(string prompt);
        decimal ReadDecimal(string prompt);
        int ReadCarId();
        int ReadOwnerId();
        CarDTO ReadCarData();
        OwnerDTO ReadOwnerData();

        // Методы для меню
        void ShowMainMenu();
        void ShowCarMenu();
        void ShowOwnerMenu();
        void Start();
    }
}
