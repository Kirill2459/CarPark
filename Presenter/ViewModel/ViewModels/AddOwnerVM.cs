using Model;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presenter.ViewModel.ViewModels
{
    public class AddOwnerVM : ViewModel, INotifyPropertyChanged
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
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
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
        private int _experienceYear;
        public int ExperienceYear
        {
            get => _experienceYear;
            set
            {
                if (_experienceYear != value)
                {
                    _experienceYear = value;
                    OnPropertyChanged(nameof(ExperienceYear));
                }
            }
        }
        private List<int> _idCarsOwner = new List<int>();
        public List<int> IdCarsOwner
        {
            get => _idCarsOwner;
            set
            {
                if (_idCarsOwner != value)
                {
                    _idCarsOwner = value;
                    OnPropertyChanged(nameof(IdCarsOwner));
                }
            }
        }

        private readonly ILogicService _logic;
        public AddOwnerVM(ILogicService logic)
        {
            _logic = logic;

            AddOwnerCommand = new RelayCommand(AddOwner);
            CancelCommand = new RelayCommand(Cancel);
        }

        public RelayCommand AddOwnerCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private void AddOwner()
        {
            Owner owner = new Owner()
            {
                Name = Name,
                Year = Year,
                ExperienceYear = ExperienceYear
            };

            _logic.Add(owner);
            MessageBox.Show($"Добавлен: {owner.Name}, возраст:{owner.Year}, стаж:{owner.ExperienceYear}");

            Cancel();
        }


        public event Action CloseEvent;
        private void Cancel()
        {
            CloseEvent.Invoke();
        }
    }
}
