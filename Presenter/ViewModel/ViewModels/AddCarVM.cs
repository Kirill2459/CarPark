using DataTransferObject;
using Model;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presenter.ViewModel.ViewModels
{
    public class AddCarVM : ViewModel, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        private string _brand;
        public string Brand
        {
            get => _brand;
            set
            {
                if (_brand != value)
                {
                    _brand = value;
                    OnPropertyChanged(nameof(Brand));
                }
            }
        }
        private string _model;
        public string Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged(nameof(Model));
                }
            }
        }
        private int _year;
        public int Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged(nameof(Year));
                }
            }
        }
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }
        private int? _idOwner;
        public int? IdOwner
        {
            get => _idOwner;
            set
            {
                if (_idOwner != value)
                {
                    _idOwner = value;
                    OnPropertyChanged(nameof(IdOwner));
                }
            }
        }


        private readonly ILogicService _logic;
        public AddCarVM(ILogicService logic)
        {
            _logic = logic;

            AddCarCommand = new RelayCommand(AddCar);
            CancelCommand = new RelayCommand(Cancel);
        }

        public RelayCommand AddCarCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private void AddCar()
        {
            Car car = new Car()
            {
                Brand = Brand,
                Model = Model,
                Year = Year,
                Price = Price
            };

            _logic.Add(car);
            MessageBox.Show($"Добавлена: {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");

            Cancel();
        }

        public event Action CloseEvent;
        private void Cancel()
        {
            CloseEvent.Invoke();
        }
    }
}
