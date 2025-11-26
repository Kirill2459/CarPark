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

            // Проверка на существование машины
            if (car == null)
            {
                MessageBox.Show($"Машины с ID {form1.idForUpdateCar} не существует!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Закрываем форму сразу
                return;
            }

            
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
                // Получаем существующую машину
                Car existingCar = Logic.Read<Car>(form1.idForUpdateCar);

                string brand = textBox1.Text;
                string model = textBox2.Text;
                int year = int.Parse(textBox3.Text);
                decimal price = decimal.Parse(textBox4.Text);

                if (!string.IsNullOrWhiteSpace(brand) && !string.IsNullOrWhiteSpace(model))
                {
                    // Обновляем существующую машину, сохраняя владельца
                    existingCar.Brand = brand;
                    existingCar.Model = model;
                    existingCar.Year = year;
                    existingCar.Price = price;
                    // IdOwner сохраняется автоматически - мы его не трогаем!

                    Logic.Update(existingCar);
                    MessageBox.Show($"Автомобиль успешно изменен: {existingCar.Brand} {existingCar.Model}, {existingCar.Year} года, - {existingCar.Price} руб");
                }
                else
                {
                    MessageBox.Show($"Есть пустые поля");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
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
