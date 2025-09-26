using System;
using System.Windows.Forms;
using System.Drawing;
using Npgsql;

namespace BaseData
{
    public partial class AddProductForm : Form
    {
        private TextBox? txtName;
        private TextBox? txtPrice;
        private TextBox? txtStock;
        private ComboBox? cmbUnit;
        private ComboBox? cmbCurrency;
        private Button? btnAdd;
        private Button? btnCancel;
        private int? _productId;

        public AddProductForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        public AddProductForm(int productId) : this()
        {
            _productId = productId;
            if (btnAdd != null) btnAdd.Text = "Сохранить";
            this.Text = "Редактировать товар";
            LoadProductData(productId);
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (txtName != null) Styles.ApplyTextBoxStyle(txtName);
                if (txtPrice != null) Styles.ApplyTextBoxStyle(txtPrice);
                if (txtStock != null) Styles.ApplyTextBoxStyle(txtStock);
                if (cmbUnit != null) Styles.ApplyComboBoxStyle(cmbUnit);
                if (cmbCurrency != null) Styles.ApplyComboBoxStyle(cmbCurrency);

                if (btnAdd != null) Styles.ApplyButtonStyle(btnAdd);
                if (btnCancel != null) Styles.ApplySecondaryButtonStyle(btnCancel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавить товар";
            this.Size = new System.Drawing.Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 7;
            mainPanel.ColumnCount = 2;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.Transparent;

            Label titleLabel = new Label()
            {
                Text = "Добавление товара",
                Font = new Font(Styles.MainFont, 12F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };

            Label lblName = new Label() { Text = "Название:*", TextAlign = ContentAlignment.MiddleRight };
            txtName = new TextBox();

            Label lblPrice = new Label() { Text = "Цена:*", TextAlign = ContentAlignment.MiddleRight };
            txtPrice = new TextBox();

            Label lblCurrency = new Label() { Text = "Валюта:", TextAlign = ContentAlignment.MiddleRight };
            cmbCurrency = new ComboBox();
            cmbCurrency.Items.AddRange(new string[] { "RUB", "USD", "EUR", "KZT" });
            cmbCurrency.SelectedIndex = 0;

            Label lblUnit = new Label() { Text = "Единица:*", TextAlign = ContentAlignment.MiddleRight };
            cmbUnit = new ComboBox();
            cmbUnit.Items.AddRange(new string[] { "шт", "кг", "л", "м", "уп" });
            cmbUnit.SelectedIndex = 0;

            Label lblStock = new Label() { Text = "Количество:", TextAlign = ContentAlignment.MiddleRight };
            txtStock = new TextBox() { Text = "0" };

            btnAdd = new Button() { Text = "Добавить", Size = new Size(100, 45) };
            btnAdd.Click += BtnAdd_Click;

            btnCancel = new Button() { Text = "Отмена", Size = new Size(100, 45) };
            btnCancel.Click += (s, e) => this.Close();

            Styles.ApplyLabelStyle(lblName, true);
            Styles.ApplyLabelStyle(lblPrice, true);
            Styles.ApplyLabelStyle(lblCurrency);
            Styles.ApplyLabelStyle(lblUnit, true);
            Styles.ApplyLabelStyle(lblStock);

            mainPanel.Controls.Add(lblName, 0, 0);
            mainPanel.Controls.Add(txtName!, 1, 0);
            mainPanel.Controls.Add(lblPrice, 0, 1);
            mainPanel.Controls.Add(txtPrice!, 1, 1);
            mainPanel.Controls.Add(lblCurrency, 0, 2);
            mainPanel.Controls.Add(cmbCurrency!, 1, 2);
            mainPanel.Controls.Add(lblUnit, 0, 3);
            mainPanel.Controls.Add(cmbUnit!, 1, 3);
            mainPanel.Controls.Add(lblStock, 0, 4);
            mainPanel.Controls.Add(txtStock!, 1, 4);

            Panel buttonsPanel = new Panel();
            buttonsPanel.Dock = DockStyle.Fill;
            buttonsPanel.BackColor = Color.Transparent;
            mainPanel.SetColumnSpan(buttonsPanel, 2);
            mainPanel.Controls.Add(buttonsPanel, 0, 6);

            buttonsPanel.Controls.Add(btnCancel);
            buttonsPanel.Controls.Add(btnAdd);

            btnAdd.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

            btnAdd.Top = buttonsPanel.Height - btnAdd.Height - 5;
            btnAdd.Left = buttonsPanel.Width - btnAdd.Width - 5;

            btnCancel.Top = buttonsPanel.Height - btnCancel.Height - 5;
            btnCancel.Left = btnAdd.Left - btnCancel.Width - 10;

            for (int i = 0; i < 5; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            }
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            this.Controls.Add(mainPanel);
            this.Controls.Add(titleLabel);

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
                        "SELECT name, price, unit, stock_quantity, currency FROM goods WHERE id = @id",
                        connection);
                    command.Parameters.AddWithValue("id", productId);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtName!.Text = reader.GetString(0);
                        txtPrice!.Text = reader.GetDecimal(1).ToString("0.00");
                        cmbUnit!.SelectedItem = reader.GetString(2);
                        txtStock!.Text = reader.GetInt32(3).ToString();
                        if (!reader.IsDBNull(4))
                            cmbCurrency!.SelectedItem = reader.GetString(4);
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
                    cmbUnit!.SelectedItem?.ToString() ?? "шт", txtStock!.Text,
                    cmbCurrency!.SelectedItem?.ToString() ?? "RUB");
            }
            else
            {
                AddProduct(txtName!.Text, txtPrice!.Text, cmbUnit!.SelectedItem?.ToString() ?? "шт",
                    txtStock!.Text, cmbCurrency!.SelectedItem?.ToString() ?? "RUB");
            }
        }

        private void AddProduct(string name, string priceText, string unit, string stockText, string currency)
        {
            if (!ValidateInput(name, priceText, stockText))
                return;

            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(@"
                        INSERT INTO goods (name, price, unit, stock_quantity, currency) 
                        VALUES (@name, @price, @unit, @stock, @currency)",
                        connection);

                    command.Parameters.AddWithValue("name", name.Trim());
                    command.Parameters.AddWithValue("price", decimal.Parse(priceText));
                    command.Parameters.AddWithValue("unit", unit);
                    command.Parameters.AddWithValue("stock", int.Parse(stockText));
                    command.Parameters.AddWithValue("currency", currency);

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

        private void UpdateProduct(int productId, string name, string priceText, string unit, string stockText, string currency)
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
                    SET name = @name, price = @price, unit = @unit, stock_quantity = @stock, currency = @currency
                    WHERE id = @id", connection);

                    command.Parameters.AddWithValue("id", productId);
                    command.Parameters.AddWithValue("name", name.Trim());
                    command.Parameters.AddWithValue("price", decimal.Parse(priceText));
                    command.Parameters.AddWithValue("unit", unit);
                    command.Parameters.AddWithValue("stock", int.Parse(stockText));
                    command.Parameters.AddWithValue("currency", currency);

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
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название товара");
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