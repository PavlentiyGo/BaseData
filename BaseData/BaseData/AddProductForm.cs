using System;
using System.Windows.Forms;
using Npgsql;

namespace BaseData
{
    public partial class AddProductForm : Form
    {
        private TextBox? txtName;
        private TextBox? txtPrice;
        private TextBox? txtStock;
        private ComboBox? cmbUnit;
        private Button? btnAdd;
        private int? _productId;

        public AddProductForm()
        {
            InitializeComponent();
        }

        public AddProductForm(int productId) : this()
        {
            _productId = productId;
            if (btnAdd != null) btnAdd.Text = "Сохранить";
            this.Text = "Редактировать товар";
            LoadProductData(productId);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавить товар";
            this.Size = new System.Drawing.Size(300, 250);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblName = new Label() { Text = "Название:", Location = new System.Drawing.Point(10, 15), Size = new System.Drawing.Size(80, 20) };
            txtName = new TextBox() { Location = new System.Drawing.Point(100, 10), Size = new System.Drawing.Size(150, 20) };

            Label lblPrice = new Label() { Text = "Цена:", Location = new System.Drawing.Point(10, 45), Size = new System.Drawing.Size(80, 20) };
            txtPrice = new TextBox() { Location = new System.Drawing.Point(100, 40), Size = new System.Drawing.Size(150, 20) };

            Label lblUnit = new Label() { Text = "Единица:", Location = new System.Drawing.Point(10, 75), Size = new System.Drawing.Size(80, 20) };
            cmbUnit = new ComboBox() { Location = new System.Drawing.Point(100, 70), Size = new System.Drawing.Size(150, 20) };
            cmbUnit.Items.AddRange(new string[] { "шт", "кг", "л", "м" });
            cmbUnit.SelectedIndex = 0;

            Label lblStock = new Label() { Text = "Количество:", Location = new System.Drawing.Point(10, 105), Size = new System.Drawing.Size(80, 20) };
            txtStock = new TextBox() { Location = new System.Drawing.Point(100, 100), Size = new System.Drawing.Size(150, 20), Text = "0" };

            btnAdd = new Button() { Text = "Добавить", Location = new System.Drawing.Point(100, 140), Size = new System.Drawing.Size(100, 30) };
            btnAdd.Click += BtnAdd_Click;

            this.Controls.AddRange(new Control[] { lblName, txtName!, lblPrice, txtPrice!, lblUnit, cmbUnit!, lblStock, txtStock!, btnAdd! });
            this.ResumeLayout(false);
        }

        private void LoadProductData(int productId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(
                        "SELECT name, price, unit, stock_quantity FROM goods WHERE id = @id",
                        connection);
                    command.Parameters.AddWithValue("id", productId);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtName!.Text = reader.GetString(0);
                        txtPrice!.Text = reader.GetDecimal(1).ToString("0.00");
                        cmbUnit!.SelectedItem = reader.GetString(2);
                        txtStock!.Text = reader.GetInt32(3).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных товара: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (_productId.HasValue)
            {
                UpdateProduct(_productId.Value, txtName!.Text, txtPrice!.Text,
                    cmbUnit!.SelectedItem?.ToString() ?? "шт", txtStock!.Text);
            }
            else
            {
                AddProduct(txtName!.Text, txtPrice!.Text, cmbUnit!.SelectedItem?.ToString() ?? "шт", txtStock!.Text);
            }
        }

        private void AddProduct(string name, string priceText, string unit, string stockText)
        {
            if (!ValidateInput(name, priceText, stockText))
                return;

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
                    command.Parameters.AddWithValue("price", decimal.Parse(priceText));
                    command.Parameters.AddWithValue("unit", unit);
                    command.Parameters.AddWithValue("stock", int.Parse(stockText));

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

        private void UpdateProduct(int productId, string name, string priceText, string unit, string stockText)
        {
            if (!ValidateInput(name, priceText, stockText))
                return;

            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(@"
                    UPDATE goods 
                    SET name = @name, price = @price, unit = @unit, stock_quantity = @stock
                    WHERE id = @id", connection);

                    command.Parameters.AddWithValue("id", productId);
                    command.Parameters.AddWithValue("name", name);
                    command.Parameters.AddWithValue("price", decimal.Parse(priceText));
                    command.Parameters.AddWithValue("unit", unit);
                    command.Parameters.AddWithValue("stock", int.Parse(stockText));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные товара успешно обновлены");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}");
            }
        }

        private bool ValidateInput(string name, string priceText, string stockText)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Заполните название и цену товара");
                return false;
            }

            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену");
                return false;
            }

            if (!int.TryParse(stockText, out int stock) || stock < 0)
            {
                MessageBox.Show("Введите корректное количество");
                return false;
            }

            return true;
        }
    }
}