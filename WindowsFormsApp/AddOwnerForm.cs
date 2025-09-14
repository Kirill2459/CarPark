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
    public partial class AddOwnerForm : Form
    {
        public AddOwnerForm()
        {
            InitializeComponent();
        }

        //добавить
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string name;
                if (string.IsNullOrWhiteSpace(name = textBox1.Text) || name.Any(char.IsDigit))
                {
                    MessageBox.Show("Имя не может быть пустым или содержать цифры. Введите имя.");
                    return;
                }

                int year;
                while (true)
                {
                    if (int.TryParse(textBox2.Text, out year))
                    {
                        if (year > 105)
                        {
                            MessageBox.Show("Возраст не может быть таким большим. Повторите ввод.");
                            return;
                        }
                        else
                        {
                            break; // Ввод корректен
                        }
                    }
                    else
                    {
                        MessageBox.Show("Некорректный ввод. Введите число для возраста.");
                        return;
                    }
                }

                int expYear;
                while (true)
                {
                    if (int.TryParse(textBox3.Text, out expYear))
                    {
                        if (expYear < 0)
                        {
                            MessageBox.Show("Стаж не может быть отрицательным. Повторите ввод.");
                            return;
                        }
                        else if (expYear > 80)
                        {
                            MessageBox.Show("Стаж не может быть больше 80 лет. Повторите ввод.");
                            return;
                        }
                        else if (expYear > year - 18) // Проверка логики
                        {
                            MessageBox.Show($"Стаж не может быть больше чем возраст минус 18 лет ({year - 18}). Повторите ввод: ");
                            return;
                        }
                        else
                        {
                            break; // Ввод корректен
                        }
                    }
                    else
                    {
                        MessageBox.Show("Некорректный ввод. Введите число для стажа: ");
                        return;
                    }
                }

                Owner owner = Logic.CreateOwner(name, year, expYear);
                Logic.Add<Owner>(owner);
                MessageBox.Show($"Добавлен: {owner.Id}. {owner.Name}, возраст:{owner.Year}, стаж:{owner.ExperienceYear}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
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
