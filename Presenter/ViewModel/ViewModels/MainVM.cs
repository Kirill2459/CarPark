using DataTransferObject;
using Model;
using Model.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presenter.ViewModel.ViewModels
{
    public class MainVM : ViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<CarDTO> _cars = new ObservableCollection<CarDTO>();
        public ObservableCollection<CarDTO> cars
        {
            get => _cars;
            set
            {
                if (_cars != value)
                {
                    _cars = value;
                    OnPropertyChanged(nameof(cars));
                }
            }
        }

        private ObservableCollection<CarDTO> _ownerCars = new ObservableCollection<CarDTO>();
        public ObservableCollection<CarDTO> ownerCars
        {
            get => _ownerCars;
            set
            {
                if (_ownerCars != value)
                {
                    _ownerCars = value;
                    OnPropertyChanged(nameof(ownerCars));
                }
            }
        }
        private ObservableCollection<CarDTO> _freeCars = new ObservableCollection<CarDTO>();
        public ObservableCollection<CarDTO> freeCars
        {
            get => _freeCars;
            set
            {
                if (_freeCars != value)
                {
                    _freeCars = value;
                    OnPropertyChanged(nameof(freeCars));
                }
            }
        }
        private ObservableCollection<OwnerDTO> _owners = new ObservableCollection<OwnerDTO>();
        public ObservableCollection<OwnerDTO> owners
        {
            get => _owners;
            set
            {
                if (_owners != value)
                {
                    _owners = value;
                    OnPropertyChanged(nameof(owners));
                }
            }
        }

        //выбранная в таблице машина
        private CarDTO _selectedCar;
        public CarDTO SelectedCar
        {
            get => _selectedCar;
            set
            {
                _selectedCar = value;

                if(SelectedCar?.Id != null)
                {
                    IdCarForDelete = SelectedCar.Id.ToString();
                    IdCarForUpdate = SelectedCar.Id.ToString();
                    IdCarForAddForOwner = SelectedCar.Id.ToString();
                }

                OnPropertyChanged(nameof(SelectedCar));
            }
        }

        //выбранный в таблице владелец
        private OwnerDTO _selectedOwner;
        public OwnerDTO SelectedOwner
        {
            get => _selectedOwner;
            set
            {
                _selectedOwner = value;

                if (SelectedOwner?.Id != null)
                {
                    IdOwnerForDelete = SelectedOwner.Id.ToString();
                    IdOwnerForShow = SelectedOwner.Id.ToString();
                    IdOwnerForAddCar = SelectedOwner.Id.ToString();
                }

                OnPropertyChanged(nameof(SelectedOwner));
            }
        }

        //TextBox с id владельца для удаления
        private string _idOwnerForDelete;
        public string IdOwnerForDelete
        {
            get => _idOwnerForDelete;
            set
            {
                if (_idOwnerForDelete != value)
                {
                    _idOwnerForDelete = value;
                    OnPropertyChanged(nameof(IdOwnerForDelete));
                }
            }
        }
        //TextBox с id владельца для просмотра
        private string _idOwnerForShow;
        public string IdOwnerForShow
        {
            get => _idOwnerForShow;
            set
            {
                if (_idOwnerForShow != value)
                {
                    _idOwnerForShow = value;
                    OnPropertyChanged(nameof(IdOwnerForShow));
                }
            }
        }
        //TextBox с id владельца для добавления ему машины
        private string _idOwnerForAddCar;
        public string IdOwnerForAddCar
        {
            get => _idOwnerForAddCar;
            set
            {
                if (_idOwnerForAddCar != value)
                {
                    _idOwnerForAddCar = value;
                    OnPropertyChanged(nameof(IdOwnerForAddCar));
                }
            }
        }
        //TextBox с id машины для добавления её владельцу
        private string _idCarForAddForOwner;
        public string IdCarForAddForOwner
        {
            get => _idCarForAddForOwner;
            set
            {
                if (_idCarForAddForOwner != value)
                {
                    _idCarForAddForOwner = value;
                    OnPropertyChanged(nameof(IdCarForAddForOwner));
                }
            }
        }
        //TextBox с id машины для удаления
        private string _idCarForDelete;
        public string IdCarForDelete
        {
            get => _idCarForDelete;
            set
            {
                if (_idCarForDelete != value)
                {
                    _idCarForDelete = value;
                    OnPropertyChanged(nameof(IdCarForDelete));
                }
            }
        }
        //TextBox с id машины для обновления информации о ней
        private string _idCarForUpdate;
        public string IdCarForUpdate
        {
            get => _idCarForUpdate;
            set
            {
                if (_idCarForUpdate != value)
                {
                    _idCarForUpdate = value;
                    OnPropertyChanged(nameof(IdCarForUpdate));
                }
            }
        }
        //TextBox с годом для сортировки
        private string _yearForSort;
        public string YearForSort
        {
            get => _yearForSort;
            set
            {
                if (_yearForSort != value)
                {
                    _yearForSort = value;
                    OnPropertyChanged(nameof(YearForSort));
                }
            }
        }
        //TextBox с брендом автомобиля для сортировки
        private string _brandForSort;
        public string BrandForSort
        {
            get => _brandForSort;
            set
            {
                if (_brandForSort != value)
                {
                    _brandForSort = value;
                    OnPropertyChanged(nameof(BrandForSort));
                }
            }
        }



        private readonly ILogicService _logic;
        public MainVM(ILogicService logic)
        {
            _logic = logic;
            cars = ConvertInCarsDTO(_logic.ReadAll<Car>());
            owners = ConvertInOwnersDTO(_logic.ReadAll<Owner>());
            freeCars = new ObservableCollection<CarDTO>(cars.Where(c => c.IdOwner == null).ToList());

            //SubscribeToViewEvents();
            InitializationCommand();
        }

        //private void SubscribeToViewEvents()
        //{
        //    ShowAllCarEvent += ShowAllCar;
        //    SortCarForYearEvent += SortCarForYear;
        //    SortCarForBrandEvent += SortCarForBrand;
        //    CostAllCarsEvent += CostAllCars;
        //    DeleteCarEvent += DeleteCar;
        //    DeleteOwnerEvent += DeleteOwner;
        //    ShowOwnerCarsEvent += ShowOwnerCars;
        //    AddCarForOwnerEvent += AddCarForOwner;
        //}

        public void InitializationCommand()
        {
            SwitchToAddCarViewCommand = new RelayCommand(AddCar);
            SwitchToUpdateCarViewCommand = new RelayCommand<string>(UpdateCar);
            SwitchToAddOwnerViewCommand = new RelayCommand(AddOwner);

            ShowAllCarCommand = new RelayCommand(ShowAllCar);
            SortCarForYearCommand = new RelayCommand<string>(SortCarForYear);
            SortCarForBrandCommand = new RelayCommand<string>(SortCarForBrand);
            CostAllCarsCommand = new RelayCommand(CostAllCars);
            DeleteCarCommand = new RelayCommand<string>(DeleteCar);
            DeleteOwnerCommand = new RelayCommand<string>(DeleteOwner);
            ShowOwnerCarsCommand = new RelayCommand<string>(ShowOwnerCars);
            AddCarForOwnerCommand = new RelayCommand(AddCarForOwner);
        }


        //=======================COMMAND===========================

        /// <summary>
        /// Команда для создания окна добавления машин
        /// </summary>
        public RelayCommand SwitchToAddCarViewCommand { get; set; }

        /// <summary>
        /// Команда для создания окна изменения машин
        /// </summary>
        public RelayCommand<string> SwitchToUpdateCarViewCommand { get; set; }

        /// <summary>
        /// Команда для создания окна добавления владельцев
        /// </summary>
        public RelayCommand SwitchToAddOwnerViewCommand { get; set; }


        // Команда для просмотра всех машин
        public RelayCommand ShowAllCarCommand { get; set; }
        // Команда для сортировки всех машин по году
        public RelayCommand<string> SortCarForYearCommand { get; set; }
        // Команда для сортировки всех машин по бренду
        public RelayCommand<string> SortCarForBrandCommand { get; set; }
        // Команда для вычисления стоимости всех машин
        public RelayCommand CostAllCarsCommand { get; set; }
        // Команда для удаления машины
        public RelayCommand<string> DeleteCarCommand { get; set; }
        // Команда для удаления владельца
        public RelayCommand<string> DeleteOwnerCommand { get; set; }
        // Команда для просмотра машин владельца
        public RelayCommand<string> ShowOwnerCarsCommand { get; set; }
        // Команда для добавления машины владельцу
        public RelayCommand AddCarForOwnerCommand { get; set; }



        //=======================EVENT=========================

        /// <summary>
        /// Событие создания окна добавления машин
        /// </summary>
        public event Action SwitchToAddCarViewEvent;

        /// <summary>
        /// Событие создания окна изменения машин
        /// </summary>
        public event Action SwitchToUpdateCarViewEvent;

        /// <summary>
        /// Событие создания окна добавления владельцев
        /// </summary>
        public event Action SwitchToAddOwnerViewEvent;


        //// Событие для просмотра всех машин
        //public event Action ShowAllCarEvent;
        //// Событие для сортировки всех машин по году
        //public event Action<string> SortCarForYearEvent;
        //// Событие для сортировки всех машин по бренду
        //public event Action<string> SortCarForBrandEvent;
        //// Событие для вычисления стоимости всех машин
        //public event Action CostAllCarsEvent;
        //// Событие для удаления машины
        //public event Action<int> DeleteCarEvent;
        //// Событие для удаления владельца
        //public event Action<int> DeleteOwnerEvent;
        //// Событие для просмотра машин владельца
        //public event Action<int> ShowOwnerCarsEvent;
        //// Событие для добавления машины владельцу
        //public event Action<int, int> AddCarForOwnerEvent;










        private void ShowAllCar()
        {
            try
            {
                cars = ConvertInCarsDTO(_logic.ReadAll<Car>());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки машин: {ex.Message}");
            }
        }

        private void SortCarForYear(string minYear)
        {
            try
            {
                cars = ConvertInCarsDTO(_logic.SortByYear(int.Parse(minYear)));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска машин: {ex.Message}");
            }
        }

        private void SortCarForBrand(string brand)
        {
            try
            {
                cars = ConvertInCarsDTO(_logic.GetCarsByBrand(brand));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска по бренду: {ex.Message}");
            }
        }

        private void DeleteCar(string carId)
        {
            try
            {
                int CarId = int.Parse(carId);
                var car = _logic.Read<Car>(CarId);

                if (car != null)
                {
                    // Удаляем связи с владельцами
                    var owners = _logic.ReadAll<Owner>();
                    foreach (var owner in owners.Where(o => o.IdCarsOwner.Contains(CarId)))
                    {
                        owner.IdCarsOwner.Remove(CarId);
                        _logic.Update(owner);

                        MessageBox.Show($"Машина ID:{carId} удалена у владельца {owner.Name}");
                    }

                    _logic.Delete<Car>(CarId);
                    MessageBox.Show("Автомобиль удален.");
                    RefreshAllData();
                }
                else
                {
                    MessageBox.Show("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        private void CostAllCars()
        {
            try
            {
                var cars = _logic.ReadAll<Car>();
                decimal totalPrice = _logic.GetCarsPrice(cars);

                MessageBox.Show($"Стоимость всего автопарка составляет {totalPrice} руб.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета стоимости: {ex.Message}");
            }
        }

        private void DeleteOwner(string ownerId)
        {
            try
            {
                int OwnerId = int.Parse(ownerId);
                var owner = _logic.Read<Owner>(OwnerId);

                if (owner != null)
                {
                    // Освобождаем машины владельца
                    var cars = _logic.ReadAll<Car>();
                    foreach (var car in cars.Where(c => c.IdOwner == OwnerId))
                    {
                        car.IdOwner = null;
                        _logic.Update(car);
                    }

                    _logic.Delete<Owner>(OwnerId);
                    MessageBox.Show("Владелец удален.");
                    RefreshAllData();
                }
                else
                {
                    MessageBox.Show("Нет владельца с таким Id.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления владельца: {ex.Message}");
            }
        }

        private void ShowOwnerCars(string ownerId)
        {
            try
            {
                int OwnerId = int.Parse(ownerId);
                var owner = _logic.Read<Owner>(OwnerId);
                ownerCars = ConvertInCarsDTO(_logic.GetOwnerCars(OwnerId));

                if (owner != null)
                {
                    if (!ownerCars.Any())
                    {
                        MessageBox.Show("У этого владельца нет машин.");
                    }
                }
                else
                {
                    MessageBox.Show("Владелец с таким Id не зарегистрирован.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки машин владельца: {ex.Message}");
            }
        }

        private void AddCarForOwner()
        {
            try
            {
                int ownerId = int.Parse(IdOwnerForAddCar);
                int carId = int.Parse(IdCarForAddForOwner);

                var owner = _logic.Read<Owner>(ownerId);
                var car = _logic.Read<Car>(carId);

                if (owner == null)
                {
                    MessageBox.Show("Владелец не найден");
                    return;
                }

                if (car == null || car.IdOwner != null)
                {
                    MessageBox.Show("Этой машины нет или она занята");
                    return;
                }

                // Добавляем связь
                owner.IdCarsOwner.Add(carId);
                car.IdOwner = ownerId;

                _logic.Update(owner);
                _logic.Update(car);

                MessageBox.Show($"Успешно! Машина {car.Model} добавлена владельцу {owner.Name}");
                RefreshAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления машины владельцу: {ex.Message}");
            }
        }

        private void UpdateCar(string carId)
        {
            try
            {
                var car = _logic.Read<Car>(int.Parse(carId));
                if (car != null)
                {
                    SwitchToUpdateCarViewEvent.Invoke();
                }
                else
                {
                    MessageBox.Show("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления автомобиля: {ex.Message}");
            }
        }

        private void AddCar()
        {
            try
            {
                SwitchToAddCarViewEvent.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления автомобиля: {ex.Message}");
            }
        }

        private void AddOwner()
        {
            try
            {
                SwitchToAddOwnerViewEvent.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления владельца: {ex.Message}");
            }
        }

        
        public void RefreshAllData()
        {
            try
            {
                owners = ConvertInOwnersDTO(_logic.ReadAll<Owner>());
                cars = ConvertInCarsDTO(_logic.ReadAll<Car>());
                freeCars = new ObservableCollection<CarDTO>(cars.Where(c => c.IdOwner == null).ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления данных: {ex.Message}");
            }
        }












        /// <summary>
        /// Проебразуем список объектов Car в список объектов CarDTO
        /// </summary>
        /// <param name="cars">список объектов Car</param>
        /// <returns>список объектов CarDTO</returns>
        public ObservableCollection<CarDTO> ConvertInCarsDTO(List<Car> cars)
        {
            ObservableCollection<CarDTO> carsDTO = new ObservableCollection<CarDTO>();
            foreach (Car car in cars)
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
        public ObservableCollection<OwnerDTO> ConvertInOwnersDTO(List<Owner> owners)
        {
            ObservableCollection<OwnerDTO> ownersDTO = new ObservableCollection<OwnerDTO>();
            foreach (Owner owner in owners)
            {
                OwnerDTO ownerDTO = new OwnerDTO(owner.Id, owner.Name, owner.Year, owner.ExperienceYear, owner.IdCarsOwner);
                ownersDTO.Add(ownerDTO);
            }

            return ownersDTO;
        }
    }
}
