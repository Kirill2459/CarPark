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
    public partial class AddCarForm : Form
    {
        private static Logic Logic;


        public AddCarForm()
        {
            InitializeComponent();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            Logic = ninjectKernel.Get<Logic>();
        }

        //добавить
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string brand = textBox1.Text;
                string model = textBox2.Text;
                int year = int.Parse(textBox3.Text);
                decimal price = decimal.Parse(textBox4.Text);

                Car car = Logic.CreateCar(brand, model, year, price);
                Logic.Add<Car>(car);
                MessageBox.Show($"Успешно добавлена: {car.Brand} {car.Model}, {car.Year} года, - {car.Price} руб");
            }
            catch
            {
                MessageBox.Show($"Ошибка");
            }
            this.Close();
        }

        //отмена
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
