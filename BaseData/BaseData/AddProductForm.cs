using System;
using System.Windows.Forms;
using Npgsql;

namespace BaseData
{
    public partial class AddProductForm : Form
    {
        public AddProductForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавить товар";
            this.Size = new System.Drawing.Size(300, 250);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblName = new Label() { Text = "Название:", Location = new System.Drawing.Point(10, 15), Size = new System.Drawing.Size(80, 20) };
            TextBox txtName = new TextBox() { Location = new System.Drawing.Point(100, 10), Size = new System.Drawing.Size(150, 20) };

            Label lblPrice = new Label() { Text = "Цена:", Location = new System.Drawing.Point(10, 45), Size = new System.Drawing.Size(80, 20) };
            TextBox txtPrice = new TextBox() { Location = new System.Drawing.Point(100, 40), Size = new System.Drawing.Size(150, 20) };

            Label lblUnit = new Label() { Text = "Единица:", Location = new System.Drawing.Point(10, 75), Size = new System.Drawing.Size(80, 20) };
            ComboBox cmbUnit = new ComboBox() { Location = new System.Drawing.Point(100, 70), Size = new System.Drawing.Size(150, 20) };
            cmbUnit.Items.AddRange(new string[] { "шт", "кг", "л", "м" });
            cmbUnit.SelectedIndex = 0;

            Label lblStock = new Label() { Text = "Количество:", Location = new System.Drawing.Point(10, 105), Size = new System.Drawing.Size(80, 20) };
            TextBox txtStock = new TextBox() { Location = new System.Drawing.Point(100, 100), Size = new System.Drawing.Size(150, 20), Text = "0" };

            Button btnAdd = new Button() { Text = "Добавить", Location = new System.Drawing.Point(100, 140), Size = new System.Drawing.Size(100, 30) };
            btnAdd.Click += (s, e) => AddProduct(txtName.Text, txtPrice.Text, cmbUnit.SelectedItem?.ToString() ?? "шт", txtStock.Text);

            this.Controls.AddRange(new Control[] { lblName, txtName, lblPrice, txtPrice, lblUnit, cmbUnit, lblStock, txtStock, btnAdd });
            this.ResumeLayout(false);
        }

        private void AddProduct(string name, string priceText, string unit, string stockText)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Заполните название и цену товара");
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }

            if (!int.TryParse(stockText, out int stock) || stock < 0)
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(@"
                        INSERT INTO goods (name, price, unit, stock_quantity) 
                        VALUES (@name, @price, @unit, @stock)",
                        connection);

                    command.Parameters.AddWithValue("name", name);
                    command.Parameters.AddWithValue("price", price);
                    command.Parameters.AddWithValue("unit", unit);
                    command.Parameters.AddWithValue("stock", stock);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Товар успешно добавлен");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}");
            }
        }
    }
}