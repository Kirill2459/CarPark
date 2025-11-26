using Model.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;



namespace WindowsFormsApp
{
    public partial class Form1 : Form, IFormView
    {
        // Свойства для хранения данных
        public List<Car> cars { get; set; } = new List<Car>();
        public List<Owner> owners { get; set; } = new List<Owner>();
        public List<Car> carsOwnedByOwners { get; set; } = new List<Car>();
        public List<Car> carsFree { get; set; } = new List<Car>();
        public int idForUpdateCar { get; set; }

        // События IView
        public event Action ShowAllCarsRequested;
        public event Action<int> FindOldCarsRequested;
        public event Action<string> FindCarsByBrandRequested;
        public event Action<int> DeleteCarRequested;
        public event Action CalculateCarsPriceRequested;
        public event Action<int> DeleteOwnerRequested;
        public event Action<int> ShowOwnerCarsRequested;
        public event Action<int, int> AddCarToOwnerRequested;
        public event Action<int> UpdateCarRequested;
        public event Action AddCarRequested;
        public event Action AddOwnerRequested;
        public event Action ShowOwnersRequested;

        public Form1()
        {
            InitializeComponent();
        }

        // === СОХРАНЯЕМ ВСЕ ОРИГИНАЛЬНЫЕ МЕТОДЫ, НО МЕНЯЕМ ИХ РЕАЛИЗАЦИЮ ===

        //показать все машины
        private void button1_Click(object sender, EventArgs e)
        {
            ShowAllCarsRequested?.Invoke();
        }

        //найти старые машины
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int year = int.Parse(textBox8.Text);
                FindOldCarsRequested?.Invoke(year);
            }
            catch
            {
                ShowError("Введены неверные данные.");
            }
        }

        //сортировать по бренду
        private void button3_Click(object sender, EventArgs e)
        {
            string brand = textBox1.Text;
            FindCarsByBrandRequested?.Invoke(brand);
        }

        //удалить автомобиль
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox2.Text);
                DeleteCarRequested?.Invoke(id);
            }
            catch
            {
                ShowError("Ошибка ввода ID.");
            }
        }

        //узнать стоимость текущих машин
        private void button6_Click(object sender, EventArgs e)
        {
            CalculateCarsPriceRequested?.Invoke();
        }

        //удалить владельца
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                int idOwner = int.Parse(textBox4.Text);
                DeleteOwnerRequested?.Invoke(idOwner);
            }
            catch
            {
                ShowError("Ошибка ввода ID владельца.");
            }
        }

        //показать машины владельца
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                int ownerID = int.Parse(textBox5.Text);
                ShowOwnerCarsRequested?.Invoke(ownerID);
            }
            catch
            {
                ShowError("Ошибка ввода ID владельца.");
            }
        }

        //добавить машину владельцу
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int ownerID = int.Parse(textBox6.Text);
                int carID = int.Parse(textBox7.Text);
                AddCarToOwnerRequested?.Invoke(ownerID, carID);
            }
            catch
            {
                ShowError("Ошибка ввода ID владельца или автомобиля.");
            }
        }

        //обновить информацию об автомобиле
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                idForUpdateCar = int.Parse(textBox3.Text);
                // Передаем this (текущий Form1) в конструктор
                UpdateCarForm updateCarForm = new UpdateCarForm(this);
                updateCarForm.ShowDialog();
                //UpdateCarRequested?.Invoke(idForUpdateCar);

                ShowAllCarsRequested?.Invoke();

            }
            catch
            {
                ShowError("Ошибка ввода ID автомобиля.");
            }
        }

        //добавить автомобиль в автопарк
        private void button7_Click(object sender, EventArgs e)
        {
            AddCarForm addCarForm = new AddCarForm();
            addCarForm.ShowDialog();

            AddCarRequested?.Invoke();
        }

        //добавить владельца
        private void button8_Click(object sender, EventArgs e)
        {
            AddOwnerForm addOwnerForm = new AddOwnerForm();
            addOwnerForm.ShowDialog();

            AddOwnerRequested?.Invoke();
        }

        // Реализация методов IView
        public void DisplayCars(List<Car> cars)
        {
            this.cars = cars;
            dataGridView_Cars.DataSource = null;
            dataGridView_Cars.DataSource = cars;
        }

        public void DisplayOwners(List<Owner> owners)
        {
            this.owners = owners;
            dataGridView_Owners.DataSource = null;
            dataGridView_Owners.DataSource = owners;
        }

        public void DisplayFreeCars(List<Car> freeCars)
        {
            this.carsFree = freeCars;
            dataGridView_CarsFree.DataSource = null;
            dataGridView_CarsFree.DataSource = freeCars;
        }

        public void DisplayOwnerCars(List<Car> ownerCars)
        {
            this.carsOwnedByOwners = ownerCars;
            dataGridView_CarsOwnedByOwners.DataSource = null;
            dataGridView_CarsOwnedByOwners.DataSource = ownerCars;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowError(string error)
        {
            MessageBox.Show(error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public int GetSelectedCarId()
        {
            return idForUpdateCar;
        }

        public int GetSelectedOwnerId()
        {
            if (int.TryParse(textBox4.Text, out int id))
                return id;
            return 0;
        }

        // Методы для работы с дополнительными формами
        public void ShowUpdateCarForm()
        {
            UpdateCarForm updateCarForm = new UpdateCarForm(this);
            updateCarForm.ShowDialog();
        }

        public void ShowAddCarForm()
        {
            AddCarForm addCarForm = new AddCarForm();
            addCarForm.ShowDialog();
        }

        public void ShowAddOwnerForm()
        {
            AddOwnerForm addOwnerForm = new AddOwnerForm();
            addOwnerForm.ShowDialog();
        }

        // Обработчики двойного клика (остаются без изменений)
        private void dataGridView_Owners_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView_Owners.Rows.Count ||
                e.ColumnIndex < 0 || e.ColumnIndex >= dataGridView_Owners.Columns.Count)
            {
                MessageBox.Show("Выберете ячейку с Id");
                return;
            }

            if (dataGridView_Owners.Columns[e.ColumnIndex].Name != "Id")
            {
                MessageBox.Show("Выберете ячейку с Id");
                return;
            }

            var value = dataGridView_Owners.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            textBox4.Text = value != null ? value.ToString() : string.Empty;
            textBox5.Text = value != null ? value.ToString() : string.Empty;
            textBox6.Text = value != null ? value.ToString() : string.Empty;
        }

        private void dataGridView_CarsFree_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView_CarsFree.Rows.Count ||
                e.ColumnIndex < 0 || e.ColumnIndex >= dataGridView_CarsFree.Columns.Count)
            {
                MessageBox.Show("Выберете ячейку с Id");
                return;
            }

            if (dataGridView_CarsFree.Columns[e.ColumnIndex].Name != "Id")
            {
                MessageBox.Show("Выберете ячейку с Id");
                return;
            }

            var value = dataGridView_CarsFree.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            textBox7.Text = value != null ? value.ToString() : string.Empty;
        }

        private void dataGridView_Cars_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView_Cars.Rows.Count ||
                e.ColumnIndex < 0 || e.ColumnIndex >= dataGridView_Cars.Columns.Count)
            {
                MessageBox.Show("Выберете ячейку с Id");
                return;
            }

            if (dataGridView_Cars.Columns[e.ColumnIndex].Name != "Id")
            {
                MessageBox.Show("Выберете ячейку с Id");
                return;
            }

            var value = dataGridView_Cars.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            textBox2.Text = value != null ? value.ToString() : string.Empty;
            textBox3.Text = value != null ? value.ToString() : string.Empty;
        }
    }
}
