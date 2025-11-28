using DataTransferObject;
using Model;
using Model.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Presenter.ViewModel.ViewModels
{
    public class UpdateCarVM : ViewModel, INotifyPropertyChanged
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
        public UpdateCarVM(ILogicService logic, CarDTO car)
        {
            Id = car.Id;
            Price = car.Price;
            Brand = car.Brand;
            Model = car.Model;
            Year = car.Year;
            IdOwner = car.IdOwner;

            _logic = logic;

            UpdateCarCommand = new RelayCommand(UpdateCar);
            CancelCommand = new RelayCommand(Cancel);
        }

        public RelayCommand UpdateCarCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private void UpdateCar()
        {
            Car newCar = new Car()
            {
                Brand = Brand,
                Model = Model,
                Year = Year,
                Price = Price
            };

            newCar.Id = Id;
            newCar.IdOwner = IdOwner;

            _logic.Update(newCar);
            MessageBox.Show($"Автомобиль изменен: {newCar.Brand} {newCar.Model}, {newCar.Year} года, - {newCar.Price} руб");

            Cancel();
        }


        public event Action CloseEvent;
        private void Cancel()
        {
            CloseEvent.Invoke();
        }
    }
}
