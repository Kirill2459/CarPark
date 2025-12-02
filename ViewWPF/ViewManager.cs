using Presenter.ViewModel.ViewModels;
using Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;

namespace ViewWPF
{
    public class ViewManager
    {
        /// <summary>
        /// ViewModel для главного окна
        /// </summary>
        private static MainVM MainVM { get; set; }

        ///// <summary>
        ///// ViewModel для окна с добавлением машины
        ///// </summary>
        //private static AddCarVM AddCarVM { get; set; }

        ///// <summary>
        ///// ViewModel для окна с добавлением владельцев
        ///// </summary>
        //private static AddOwnerVM AddOwnerVM { get; set; }

        ///// <summary>
        ///// ViewModel для окна с изменением машины
        ///// </summary>
        //private static UpdateCarVM UpdateCarVM { get; set; }

        /// <summary>
        /// ViewModelManager для организации работы нескольких ViewModel многооконного приложения
        /// </summary>
        private static VMManager VMManager { get; set; }

        
        private static Dictionary<ViewModel, Window> dictWindow = new Dictionary<ViewModel, Window>();



        [STAThread] // Нужен для запуска WPF
        static void Main(string[] args)
        {
            VMManager = new VMManager();

            VMManager.AddCarVMChanged += CreateAddCarView;

            VMManager.AddOwnerVMChanged += CreateAddOwnerView;

            VMManager.UpdateCarVMChanged += CreateUpdateCarView;

            VMManager.MainVMChanged += CreateMainView;

            Application app = new Application();

            // Вызываем событие обновления MainVM для запуска начального окна
            VMManager.GetMainVM();

            app.Run();
        }



        /// <summary>
        /// Метод создания главного окна и связывания его DataContext с MainVM
        /// </summary>
        /// <param name="mainVM">ViewModel для главной формы</param>
        private static void CreateMainView(MainVM mainVM)
        {
            //mainVM.SwitchToAddCarViewEvent += VMManager.GetAddCarVM;
            //mainVM.SwitchToUpdateCarViewEvent += VMManager.GetUpdateCarVM;
            //mainVM.SwitchToAddOwnerViewEvent += VMManager.GetAddOwnerVM;
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainVM;

            MainVM = mainVM;

            dictWindow.Add(mainVM, mainWindow);

            dictWindow[mainVM].ShowDialog();

            //mainWindow.Show();
        }

        /// <summary>
        /// Метод создания окна изменения машин и связывания его DataContext с UpdateCarVM
        /// </summary>
        /// <param name="updateCarVN">ViewModel для изменения машин</param>
        private static void CreateUpdateCarView(UpdateCarVM updateCarVN)
        {
            var updateCarWindow = new UpdateCarWindow();
            //UpdateCarVM = updateCarVN;
            updateCarWindow.DataContext = updateCarVN;

            updateCarVN.CloseEvent += () => CloseWindow(updateCarWindow);

            //обновление MainVM
            updateCarVN.CloseEvent += MainVM.RefreshAllData;


            dictWindow.Add(updateCarVN, updateCarWindow);

            dictWindow[updateCarVN].ShowDialog();
            //updateCarWindow.ShowDialog();
        }

        /// <summary>
        /// Метод создания окна добавления машин и связывания его DataContext с AddCarVM
        /// </summary>
        /// <param name="addCarVM">ViewModel для добавления машин</param>
        private static void CreateAddCarView(AddCarVM addCarVM)
        {
            var addCarWindow = new AddCarWindow();
            //AddCarVM = addCarVM;
            addCarWindow.DataContext = addCarVM;

            addCarVM.CloseEvent += () => CloseWindow(addCarWindow);

            //обновление MainVM
            addCarVM.CloseEvent += MainVM.RefreshAllData;


            dictWindow.Add(addCarVM, addCarWindow);

            dictWindow[addCarVM].ShowDialog();
            //addCarWindow.ShowDialog();
        }

        /// <summary>
        /// Метод создания окна добавления владельца и связывания его DataContext с AddOwnerVM
        /// </summary>
        /// <param name="addOwnerVM">ViewModel для добавления владельца</param>
        private static void CreateAddOwnerView(AddOwnerVM addOwnerVM)
        {
            var addOwnerWindow = new AddOwnerWindow();
            //AddOwnerVM = addOwnerVM;
            addOwnerWindow.DataContext = addOwnerVM;

            addOwnerVM.CloseEvent += () => CloseWindow(addOwnerWindow);

            //обновление MainVM
            addOwnerVM.CloseEvent += MainVM.RefreshAllData;


            dictWindow.Add(addOwnerVM, addOwnerWindow);

            dictWindow[addOwnerVM].ShowDialog();
            //addOwnerWindow.ShowDialog();
        }

        /// <summary>
        /// Метод для закрытия окна
        /// </summary>
        /// <param name="window">окно</param>
        private static void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
