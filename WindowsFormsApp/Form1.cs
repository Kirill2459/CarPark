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




        public Form1()
        {
            InitializeComponent();
            owners = Logic.ReadAll<Owner>();
            dataGridView_Owners.DataSource = owners;
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

        //обновить информацию об автомобиле
        private void button5_Click(object sender, EventArgs e)
        {
            //....
        }

        //добавить автомобиль в автопарк
        private void button7_Click(object sender, EventArgs e)
        {
            //...
        }
    }
}
