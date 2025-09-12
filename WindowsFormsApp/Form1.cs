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

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public List<Car> cars { get; set; } = new List<Car>();
        public List<Owner> owners { get; set; } = new List<Owner>();
        public List<Car> carsOwnedByOwners { get; set; } = new List<Car>(); //машины принадлежащие владельцам
        public List<Car> carsFree { get; set; } = new List<Car>(); //свободные машины




        public Form1()
        {
            InitializeComponent();

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
            cars = Logic.GetVintageCars();
            dataGridView_Cars.DataSource = cars;
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

        //узнать полную стоимость автопарка
        private void button6_Click(object sender, EventArgs e)
        {
            decimal allPrice = Logic.AllPrice();
            MessageBox.Show($"Полная стоимость автопарка составляет {allPrice} рублей.");
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
                    MessageBox.Show("Владелей с таким Id не зарегистрирован.");
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
            //....
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
            //...
        }
    }
}
