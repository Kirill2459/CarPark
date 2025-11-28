using ConsoleApp;
using DataTransferObject;
using Model;
using Ninject;
using Presenter.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Presenter.ViewModel
{
    public class VMManager
    {
        /// <summary>
        /// ViewModel главного окна
        /// </summary>
        private MainVM mainVM;

        /// <summary>
        /// ViewModel добавления машины
        /// </summary>
        private AddCarVM addCarVM;

        /// <summary>
        /// ViewModel обновления машины
        /// </summary>
        private UpdateCarVM updateCarVM;

        /// <summary>
        /// ViewModel добавления владельца
        /// </summary>
        private AddOwnerVM addOwnerVM;




        /// <summary>
        /// Событие обновления MainVM
        /// </summary>
        public event Action<MainVM> MainVMChanged;

        /// <summary>
        /// Событие обновления AddCarVM
        /// </summary>
        public event Action<AddCarVM> AddCarVMChanged;

        /// <summary>
        /// Событие обновления AddCarVM
        /// </summary>
        public event Action<UpdateCarVM> UpdateCarVMChanged;

        /// <summary>
        /// Событие обновления AddCarVM
        /// </summary>
        public event Action<AddOwnerVM> AddOwnerVMChanged;





        // Настройка DI
        static IKernel kernel = new StandardKernel(new SimpleConfigModule());
        private ILogicService logic = kernel.Get<ILogicService>();

        /// <summary>
        /// Конструктор ViewModelManager
        /// </summary>
        public VMManager()
        {
            mainVM = new MainVM(logic);

            mainVM.SwitchToAddCarViewEvent += GetAddCarVM;
            mainVM.SwitchToUpdateCarViewEvent += GetUpdateCarVM;
            mainVM.SwitchToAddOwnerViewEvent += GetAddOwnerVM;
        }

        /// <summary>
        /// Метод обновления MainVM
        /// </summary>
        public void GetMainVM()
        {
            MainVMChanged.Invoke(mainVM);
        }

        /// <summary>
        /// Метод обновления UpdateCarVM
        /// </summary>
        public void GetUpdateCarVM()
        {
            updateCarVM = new UpdateCarVM(logic, new CarDTO()
            {
                Id = mainVM.SelectedCar.Id,
                Brand = mainVM.SelectedCar.Brand,
                Model = mainVM.SelectedCar.Model,
                Year = mainVM.SelectedCar.Year,
                Price = mainVM.SelectedCar.Price,
                IdOwner = mainVM.SelectedCar.IdOwner
            });

            UpdateCarVMChanged.Invoke(updateCarVM);
        }

        /// <summary>
        /// Метод обновления AddCarVM
        /// </summary>
        public void GetAddCarVM()
        {
            addCarVM = new AddCarVM(logic);
            AddCarVMChanged.Invoke(addCarVM);
        }

        /// <summary>
        /// Метод обновления AddOwnerVM
        /// </summary>
        public void GetAddOwnerVM()
        {
            addOwnerVM = new AddOwnerVM(logic);
            AddOwnerVMChanged.Invoke(addOwnerVM);
        }
    }
}
