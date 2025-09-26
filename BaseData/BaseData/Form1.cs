// Form1.cs (исправленный)
using Npgsql;
using System;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Styles.ApplyFormStyle(this);
            Styles.ApplyButtonStyle(CreateButton);
            Styles.ApplyButtonStyle(AddButton);
            Styles.ApplyButtonStyle(GetButton);
        }

        private void CreateDataBase_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("База данных успешно создана и настроена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void AddData_Click(object sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("Сначала подключитесь к базе данных", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Открываем форму для добавления данных
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        // Form1.cs - улучшим обработку нажатия кнопки "Вывести данные"
        // Form1.cs - убедимся, что обработчик корректен
        private void GetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettings.IsConnectionStringSet)
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем строку подключения
                MessageBox.Show($"Подключение: {AppSettings.SqlConnection}", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form3 form3 = new Form3();
                form3.ShowDialog(); // Используем ShowDialog вместо Show для диагностики
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия Form3: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}