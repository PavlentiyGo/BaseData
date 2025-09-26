using System;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ApplyStyles();

            // Привязка событий к кнопкам
            CreateButton.Click += CreateButton_Click;
            AddButton.Click += AddButton_Click;
            GetButton.Click += GetButton_Click;
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
            }
        }

        /// <summary>
        /// Создать схему и таблицы (Form2)
        /// </summary>
        private void CreateButton_Click(object? sender, EventArgs e)
        {
            using (Form2 form2 = new Form2())
            {
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Схема и таблицы успешно пересозданы",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Внести данные (Form4)
        /// </summary>
        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form4 form4 = new Form4())
            {
                form4.ShowDialog();
            }
        }

        /// <summary>
        /// Просмотр данных (Form3) - ИСПРАВЛЕНО
        /// </summary>
        private void GetButton_Click(object? sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form3 form3 = new Form3();
            form3.ShowDialog();
            form3.Dispose();
        }
    }
}