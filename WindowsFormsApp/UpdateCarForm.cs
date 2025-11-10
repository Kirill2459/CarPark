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
    public partial class UpdateCarForm : Form
    {
        Form1 form1 { get; set; }

        private static Logic Logic;

        public UpdateCarForm()
        {
            InitializeComponent();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            Logic = ninjectKernel.Get<Logic>();
        }

        public UpdateCarForm(Form1 form)
        {
            InitializeComponent();

            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            Logic = ninjectKernel.Get<Logic>();

            form1 = form;

            Car car = Logic.Read<Car>(form1.idForUpdateCar);
            textBox1.Text = car.Brand;
            textBox2.Text = car.Model;
            textBox3.Text = car.Year.ToString();
            textBox4.Text = car.Price.ToString();

        }

        //Обновить
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Car car = Logic.Read<Car>(form1.idForUpdateCar);

                string brand = textBox1.Text;
                string model = textBox2.Text;
                int year = int.Parse(textBox3.Text);
                decimal price = decimal.Parse(textBox4.Text);

                if (!string.IsNullOrWhiteSpace(brand) && !string.IsNullOrWhiteSpace(model))
                {
                    Car newCar = Logic.CreateCar(brand, model, year, price);
                    newCar.Id = form1.idForUpdateCar;
                    Logic.Update(newCar);
                    MessageBox.Show($"Автомобиль успешно изменен: {newCar.Brand} {newCar.Model}, {newCar.Year} года, - {newCar.Price} руб");
                }
                else
                {
                    MessageBox.Show($"Есть пустые поля");
                }
                
            }
            catch
            {
                MessageBox.Show($"Ошибка");
            }

            this.Close();
        }

        //Отмена
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
