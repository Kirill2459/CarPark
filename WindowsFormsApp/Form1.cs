using Model.Entities;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject;


namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public List<Car> cars { get; set; } = new List<Car>();
        public List<Owner> owners { get; set; } = new List<Owner>();
        public List<Car> carsOwnedByOwners { get; set; } = new List<Car>(); //машины принадлежащие владельцам
        public List<Car> carsFree { get; set; } = new List<Car>(); //свободные машины

        public int idForUpdateCar { get; set; } //Id который будет передаваться в форму UpdateCarForm


        private static Logic Logic;


        public Form1()
        {
            InitializeComponent();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            Logic = ninjectKernel.Get<Logic>();

            owners = Logic.ReadAll<Owner>();
            dataGridView_Owners.DataSource = owners;

            carsFree = Logic.ReadAll<Car>().Where(car => car.IdOwner == null).ToList();
            dataGridView_CarsFree.DataSource = carsFree;
        }

        //показать все машины
        private void button1_Click(object sender, EventArgs e)
        {
            cars = Logic.ReadAll<Car>();
            dataGridView_Cars.DataSource = cars;
        }

        //найти старые машины
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                cars = Logic.SortByYear(int.Parse(textBox8.Text));
                dataGridView_Cars.DataSource = cars;
            }
            catch
            {
                MessageBox.Show("Введены неверные данные.");
            }
        }

        //сортировать по бренду
        private void button3_Click(object sender, EventArgs e)
        {
            string brand = textBox1.Text;
            cars = Logic.GetCarsByBrand(brand);
            dataGridView_Cars.DataSource = cars;
        }

        //удалить автомобиль
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox2.Text);

                List<Owner> owners = Logic.ReadAll<Owner>();
                Car car = Logic.Read<Car>(id);

                if (car != null)
                {
                    Logic.Delete<Car>(id);
                    foreach (Owner owner in owners)
                    {
                        // Проверяем содержит ли владелец эту машину
                        if (owner.IdCarsOwner.Contains(id))
                        {
                            // Удаляем ID машины из списка владельца
                            owner.IdCarsOwner.Remove(id);
                            Logic.Update(owner); // Обновляем владельца в json

                            MessageBox.Show($"Машина ID:{id} удалена у владельца {owner.Name}");
                        }
                    }
                    MessageBox.Show($"Автомобиль удален.");
                }
                else
                {
                    MessageBox.Show("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка.");
            }
            cars = Logic.ReadAll<Car>();
            dataGridView_Cars.DataSource = cars;
        }

        //узнать стоимость текущих машин
        private void button6_Click(object sender, EventArgs e)
        {
            List<Car> currentCars = (List<Car>)dataGridView_Cars.DataSource;
            decimal allPrice = Logic.GetCarsPrice(currentCars);
            MessageBox.Show($"Стоимость текущих машин составляет {allPrice} рублей.");
        }

        //удалить владельца
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                int idOwner = int.Parse(textBox4.Text);

                Owner owner = Logic.Read<Owner>(idOwner);
                List<Car> cars = Logic.ReadAll<Car>();

                if (owner != null)
                {
                    Logic.Delete<Owner>(idOwner); // удаление владельца

                    foreach (Car car in cars)
                    {
                        if (car.IdOwner == idOwner)
                        {
                            car.IdOwner = null; // делаем машину владельца свободной
                            Logic.Update(car); // сохраняем изменения в json
                        }
                    }
                    MessageBox.Show($"Владелец удален.");
                }
                else
                {
                    MessageBox.Show("Нет владельца с таким Id.");
                }
            }
            catch
            {
                MessageBox.Show($"Ошибка");
            }
            carsFree = Logic.ReadAll<Car>();
            dataGridView_CarsFree.DataSource = carsFree;

            owners = Logic.ReadAll<Owner>();
            dataGridView_Owners.DataSource = owners;
        }

        //показать машины владельца
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                int ownerID = int.Parse(textBox5.Text);

                Owner owner = Logic.Read<Owner>(ownerID);
                List<Car> cars = Logic.ReadAll<Car>();

                bool carFound = false;

                carsOwnedByOwners = new List<Car>();
                if (owner != null)
                {
                    foreach (Car car in cars)
                    {
                        if (car.IdOwner == ownerID)
                        {
                            carsOwnedByOwners.Add(car);
                            carFound = true;
                        }
                    }
                    if (!carFound)
                    {
                        carsOwnedByOwners = new List<Car>();
                        MessageBox.Show("У этого владельца нет машин.");
                    }
                }
                else
                {
                    MessageBox.Show("Владелец с таким Id не зарегистрирован.");
                }
            }
            catch
            {
                MessageBox.Show($"Ошибка");
            }
            dataGridView_CarsOwnedByOwners.DataSource = carsOwnedByOwners;
        }

        //добавить машину владельцу
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int ownerID = int.Parse(textBox6.Text);

                Owner owner = Logic.Read<Owner>(ownerID);

                if (owner != null)
                {
                    if (carsFree.Count != 0)
                    {
                        int carID = int.Parse(textBox7.Text);

                        bool carFound = false; // Флаг для отслеживания найденной машины

                        foreach (Car freeCar in carsFree)
                        {
                            if (freeCar.Id == carID)
                            {
                                owner.IdCarsOwner.Add(carID); // добавляем id машины владельцу
                                Logic.Update(owner);
                                freeCar.IdOwner = ownerID; // добавляем id владельца машине
                                Logic.Update(freeCar);


                                MessageBox.Show($"Успешно! Машина {freeCar.Model} добавлена владельцу {owner.Name}");
                                carFound = true;
                                break;

                            }
                        }
                        if (!carFound)
                        {
                            MessageBox.Show("Этой машины нет или она занята");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Свободных машин нет");
                    }
                }
                else
                {
                    MessageBox.Show("Владелец с таким Id не зарегистрирован.");
                }
            }
            catch
            {
                MessageBox.Show($"Ошибка");
            }
            carsFree = Logic.ReadAll<Car>().Where(car => car.IdOwner == null).ToList();
            dataGridView_CarsFree.DataSource = carsFree;
        }

        //обновить информацию об автомобиле
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                idForUpdateCar = int.Parse(textBox3.Text);

                Car car = Logic.Read<Car>(idForUpdateCar);

                if (car != null)
                {
                    UpdateCarForm updateCarForm = new UpdateCarForm(this);
                    updateCarForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("В автопарке нет автомобиля с таким Id.");
                }
            }
            catch
            {
                MessageBox.Show($"Ошибка");
            }

            cars = Logic.ReadAll<Car>();
            dataGridView_Cars.DataSource = cars;
        }

        //добавить автомобиль в автопарк
        private void button7_Click(object sender, EventArgs e)
        {
            AddCarForm addCarForm = new AddCarForm();
            addCarForm.ShowDialog();

            cars = Logic.ReadAll<Car>();
            dataGridView_Cars.DataSource = cars;
        }

        //добавить владельца
        private void button8_Click(object sender, EventArgs e)
        {
            AddOwnerForm addOwnerForm = new AddOwnerForm();
            addOwnerForm.ShowDialog();

            owners = Logic.ReadAll<Owner>();
            dataGridView_Owners.DataSource = owners;
        }

        private void dataGridView_Owners_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView_Owners.Rows.Count ||
        e.ColumnIndex < 0 || e.ColumnIndex >= dataGridView_Owners.Columns.Count)
            {
                MessageBox.Show("Выберете ячейку с Id");
                return;
            }

            // Проверяем, что кликнули НЕ на столбец Id
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

            // ЗАМЕНИТЬ dataGridView_Owners на dataGridView_CarsFree
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

            // ЗАМЕНИТЬ dataGridView_Owners на dataGridView_Cars
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
