using System;
using System.Windows.Forms;
using Npgsql;
using System.Data;
using System.Drawing;

namespace BaseData
{
    public partial class AddSaleForm : Form
    {
        private DataGridView? dgvOrderItems;
        private ComboBox? cmbClients;
        private ComboBox? cmbGoods;
        private TextBox? txtQuantity;
        private DateTimePicker? dtOrderDate;
        private DateTimePicker? dtDeliveryDate;
        private ComboBox? cmbCurrency;
        private Button? btnAddItem;
        private Button? btnCreateOrder;
        private Button? btnCancel;

        // Курсы валют (можно вынести в настройки)
        private readonly decimal usdRate = 90.0m;
        private readonly decimal eurRate = 98.0m;

        public AddSaleForm()
        {
            InitializeComponent();
            ApplyStyles();
            LoadClients();
            LoadGoods();
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (dgvOrderItems != null) Styles.ApplyDataGridViewStyle(dgvOrderItems);
                if (cmbClients != null) Styles.ApplyComboBoxStyle(cmbClients);
                if (cmbGoods != null) Styles.ApplyComboBoxStyle(cmbGoods);
                if (cmbCurrency != null) Styles.ApplyComboBoxStyle(cmbCurrency);
                if (txtQuantity != null) Styles.ApplyTextBoxStyle(txtQuantity);
                if (dtOrderDate != null) Styles.ApplyDateTimePickerStyle(dtOrderDate);
                if (dtDeliveryDate != null) Styles.ApplyDateTimePickerStyle(dtDeliveryDate);
                if (btnAddItem != null) Styles.ApplySecondaryButtonStyle(btnAddItem);
                if (btnCreateOrder != null) Styles.ApplyButtonStyle(btnCreateOrder);
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
            this.Text = "Добавить заказ";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            // Главный контейнер
            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 8;
            mainPanel.ColumnCount = 4;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.Transparent;

            // Заголовок
            Label titleLabel = new Label()
            {
                Text = "Оформление продажи",
                Font = new Font(Styles.MainFont, 14F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Инициализация всех полей
            this.dgvOrderItems = new DataGridView();
            this.cmbClients = new ComboBox();
            this.cmbGoods = new ComboBox();
            this.txtQuantity = new TextBox();
            this.dtOrderDate = new DateTimePicker();
            this.dtDeliveryDate = new DateTimePicker();
            this.cmbCurrency = new ComboBox();
            this.btnAddItem = new Button();
            this.btnCreateOrder = new Button();
            this.btnCancel = new Button();

            // Настройка размеров строк и колонок
            for (int i = 0; i < 8; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            }
            mainPanel.RowStyles[7] = new RowStyle(SizeType.Percent, 100F);
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            // Выбор клиента
            Label lblClient = new Label() { Text = "Клиент:*", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblClient, true);
            this.cmbClients.DropDownStyle = ComboBoxStyle.DropDownList;

            // Дата заказа
            Label lblDate = new Label() { Text = "Дата заказа:*", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblDate, true);
            this.dtOrderDate.Value = DateTime.Now;

            // Дата доставки
            Label lblDeliveryDate = new Label() { Text = "Дата доставки:*", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblDeliveryDate, true);
            this.dtDeliveryDate.Value = DateTime.Now.AddDays(3);

            // Валюта
            Label lblCurrency = new Label() { Text = "Валюта:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblCurrency);
            this.cmbCurrency.Items.AddRange(new string[] { "RUB - Российский рубль", "USD - Доллар США", "EUR - Евро" });
            this.cmbCurrency.SelectedIndex = 0;
            this.cmbCurrency.DropDownStyle = ComboBoxStyle.DropDownList;

            // Выбор товара
            Label lblGood = new Label() { Text = "Товар:*", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblGood, true);
            this.cmbGoods.DropDownStyle = ComboBoxStyle.DropDownList;

            // Количество
            Label lblQuantity = new Label() { Text = "Количество:*", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblQuantity, true);
            this.txtQuantity.Text = "1";

            // Кнопка добавления товара в заказ
            this.btnAddItem.Text = "Добавить товар";
            this.btnAddItem.Click += this.AddItemToOrder;

            // Пустая ячейка для выравнивания
            Label emptyLabel = new Label() { Text = "" };

            // Таблица товаров в заказе
            this.dgvOrderItems.AllowUserToAddRows = false;
            this.dgvOrderItems.Dock = DockStyle.Fill;
            this.dgvOrderItems.Columns.Add("GoodId", "ID товара");
            this.dgvOrderItems.Columns.Add("GoodName", "Товар");
            this.dgvOrderItems.Columns.Add("Quantity", "Количество");
            this.dgvOrderItems.Columns.Add("Price", "Цена");
            this.dgvOrderItems.Columns.Add("Stock", "В наличии");
            this.dgvOrderItems.Columns.Add("Currency", "Валюта");
            this.dgvOrderItems.Columns["GoodId"].Visible = false;

            // Кнопки
            this.btnCreateOrder.Text = "Создать заказ";
            this.btnCreateOrder.Click += this.CreateOrder;

            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += (s, e) => this.Close();

            // Добавляем элементы на панель
            mainPanel.Controls.Add(lblClient, 0, 0);
            mainPanel.Controls.Add(this.cmbClients, 1, 0);
            mainPanel.Controls.Add(lblDate, 2, 0);
            mainPanel.Controls.Add(this.dtOrderDate, 3, 0);

            mainPanel.Controls.Add(lblDeliveryDate, 0, 1);
            mainPanel.Controls.Add(this.dtDeliveryDate, 1, 1);
            mainPanel.Controls.Add(lblCurrency, 2, 1);
            mainPanel.Controls.Add(this.cmbCurrency, 3, 1);

            mainPanel.Controls.Add(lblGood, 0, 2);
            mainPanel.Controls.Add(this.cmbGoods, 1, 2);
            mainPanel.Controls.Add(lblQuantity, 2, 2);
            mainPanel.Controls.Add(this.txtQuantity, 3, 2);

            mainPanel.Controls.Add(this.btnAddItem, 0, 3);
            mainPanel.SetColumnSpan(this.btnAddItem, 4);

            mainPanel.Controls.Add(this.dgvOrderItems, 0, 4);
            mainPanel.SetColumnSpan(this.dgvOrderItems, 4);
            mainPanel.SetRowSpan(this.dgvOrderItems, 3);

            // Панель для кнопок внизу
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 50;
            buttonPanel.Padding = new Padding(10);

            buttonPanel.Controls.Add(this.btnCancel);
            buttonPanel.Controls.Add(this.btnCreateOrder);
            this.btnCreateOrder.Left = buttonPanel.Width - this.btnCreateOrder.Width - 10;
            this.btnCancel.Left = this.btnCreateOrder.Left - this.btnCancel.Width - 10;

            // Компоновка формы
            this.Controls.Add(mainPanel);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(titleLabel);

            this.ResumeLayout(false);
        }

        private void LoadClients()
        {
            try
            {
                if (this.cmbClients == null) return;

                this.cmbClients.Items.Clear();

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("SELECT id, surname, name, middlename FROM clients ORDER BY surname, name", connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        this.cmbClients.Items.Add(new ClientItem
                        {
                            Id = reader.GetInt32(0),
                            Display = $"{reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)}"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}");
            }
        }

        private void LoadGoods()
        {
            try
            {
                if (this.cmbGoods == null) return;

                this.cmbGoods.Items.Clear();

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("SELECT id, name, price, stock_quantity FROM goods WHERE stock_quantity > 0 ORDER BY name", connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        this.cmbGoods.Items.Add(new GoodItem
                        {
                            Id = reader.GetInt32(0),
                            Display = $"{reader.GetString(1)} - {reader.GetDecimal(2)} руб. (в наличии: {reader.GetInt32(3)})"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}");
            }
        }

        private void AddItemToOrder(object? sender, EventArgs e)
        {
            if (this.cmbGoods?.SelectedItem == null ||
                this.txtQuantity == null ||
                !int.TryParse(this.txtQuantity.Text, out int quantity) ||
                quantity <= 0)
            {
                MessageBox.Show("Выберите товар и укажите количество");
                return;
            }

            var good = (GoodItem)this.cmbGoods.SelectedItem;
            var goodInfo = GetGoodInfo(good.Id);

            if (goodInfo.Stock < quantity)
            {
                MessageBox.Show($"Недостаточно товара на складе. В наличии: {goodInfo.Stock}");
                return;
            }

            if (this.dgvOrderItems != null)
            {
                string currency = this.cmbCurrency?.SelectedItem?.ToString()?.Substring(0, 3) ?? "RUB";
                decimal priceInCurrency = ConvertToCurrency(goodInfo.Price, currency);

                this.dgvOrderItems.Rows.Add(good.Id, good.Display.Split('-')[0].Trim(), quantity,
                    priceInCurrency * quantity, goodInfo.Stock, currency);
            }
        }

        private decimal ConvertToCurrency(decimal priceInRubles, string currency)
        {
            return currency switch
            {
                "USD" => priceInRubles / usdRate,
                "EUR" => priceInRubles / eurRate,
                _ => priceInRubles // RUB
            };
        }

        private decimal ConvertToRubles(decimal price, string currency)
        {
            return currency switch
            {
                "USD" => price * usdRate,
                "EUR" => price * eurRate,
                _ => price // RUB
            };
        }

        private (decimal Price, int Stock) GetGoodInfo(int goodId)
        {
            using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT price, stock_quantity FROM goods WHERE id = @id", connection);
                command.Parameters.AddWithValue("id", goodId);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (reader.GetDecimal(0), reader.GetInt32(1));
                }
                return (0, 0);
            }
        }

        private void CreateOrder(object? sender, EventArgs e)
        {
            if (this.cmbClients?.SelectedItem == null ||
                this.dgvOrderItems == null ||
                this.dgvOrderItems.Rows.Count == 0 ||
                this.dtOrderDate == null ||
                this.dtDeliveryDate == null)
            {
                MessageBox.Show("Выберите клиента и добавьте товары в заказ");
                return;
            }

            var client = (ClientItem)this.cmbClients.SelectedItem;
            decimal totalAmount = 0;

            // Проверяем остатки и подсчитываем сумму в рублях
            foreach (DataGridViewRow row in this.dgvOrderItems.Rows)
            {
                if (row.Cells["GoodId"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    int goodId = Convert.ToInt32(row.Cells["GoodId"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    int stock = Convert.ToInt32(row.Cells["Stock"].Value);
                    string currency = row.Cells["Currency"].Value?.ToString() ?? "RUB";
                    decimal priceInCurrency = Convert.ToDecimal(row.Cells["Price"].Value);

                    if (quantity > stock)
                    {
                        MessageBox.Show($"Недостаточно товара '{row.Cells["GoodName"].Value}' на складе");
                        return;
                    }

                    // Конвертируем в рубли для расчета скидки
                    decimal priceInRubles = ConvertToRubles(priceInCurrency, currency);
                    totalAmount += priceInRubles;
                }
            }

            // Применяем скидку 2% для заказов свыше 5000 руб
            decimal discount = totalAmount > 5000 ? 2.0m : 0.0m;
            decimal finalAmount = totalAmount * (1 - discount / 100);

            // Если сумма больше 5000, делаем клиента постоянным
            bool makeConstant = totalAmount > 5000;

            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();

                    // Создаем заказ
                    var orderCommand = new NpgsqlCommand(@"
                        INSERT INTO orders (client_id, order_date, delivery_date, total_amount, discount) 
                        VALUES (@client_id, @order_date, @delivery_date, @total_amount, @discount) RETURNING id",
                        connection);

                    orderCommand.Parameters.AddWithValue("client_id", client.Id);
                    orderCommand.Parameters.AddWithValue("order_date", this.dtOrderDate.Value);
                    orderCommand.Parameters.AddWithValue("delivery_date", this.dtDeliveryDate.Value);
                    orderCommand.Parameters.AddWithValue("total_amount", finalAmount);
                    orderCommand.Parameters.AddWithValue("discount", discount);

                    int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());

                    // Добавляем товары в заказ и обновляем остатки
                    foreach (DataGridViewRow row in this.dgvOrderItems.Rows)
                    {
                        if (row.Cells["GoodId"].Value != null)
                        {
                            int goodId = Convert.ToInt32(row.Cells["GoodId"].Value);
                            int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                            string currency = row.Cells["Currency"].Value?.ToString() ?? "RUB";
                            decimal priceInCurrency = Convert.ToDecimal(row.Cells["Price"].Value);

                            var itemCommand = new NpgsqlCommand(@"
                                INSERT INTO order_items (order_id, good_id, quantity, price, currency) 
                                VALUES (@order_id, @good_id, @quantity, @price, @currency)",
                                connection);

                            itemCommand.Parameters.AddWithValue("order_id", orderId);
                            itemCommand.Parameters.AddWithValue("good_id", goodId);
                            itemCommand.Parameters.AddWithValue("quantity", quantity);
                            itemCommand.Parameters.AddWithValue("price", priceInCurrency);
                            itemCommand.Parameters.AddWithValue("currency", currency);

                            itemCommand.ExecuteNonQuery();

                            // Обновляем остатки
                            var updateStockCommand = new NpgsqlCommand(@"
                                UPDATE goods SET stock_quantity = stock_quantity - @quantity 
                                WHERE id = @good_id", connection);
                            updateStockCommand.Parameters.AddWithValue("quantity", quantity);
                            updateStockCommand.Parameters.AddWithValue("good_id", goodId);
                            updateStockCommand.ExecuteNonQuery();
                        }
                    }

                    // Делаем клиента постоянным если нужно
                    if (makeConstant)
                    {
                        var updateClientCommand = new NpgsqlCommand(@"
                            UPDATE clients SET constclient = true WHERE id = @client_id", connection);
                        updateClientCommand.Parameters.AddWithValue("client_id", client.Id);
                        updateClientCommand.ExecuteNonQuery();
                    }

                    string currencySymbol = this.cmbCurrency?.SelectedItem?.ToString()?.Contains("USD") == true ? "$" :
                                          this.cmbCurrency?.SelectedItem?.ToString()?.Contains("EUR") == true ? "€" : "₽";

                    MessageBox.Show($"Заказ успешно создан!\nОбщая сумма: {finalAmount:0.00} руб.\nСкидка: {discount}%");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания заказа: {ex.Message}");
            }
        }
    }

    internal class ClientItem
    {
        public int Id { get; set; }
        public string Display { get; set; } = string.Empty;
        public override string ToString() => Display;
    }

    internal class GoodItem
    {
        public int Id { get; set; }
        public string Display { get; set; } = string.Empty;
        public override string ToString() => Display;
    }
}