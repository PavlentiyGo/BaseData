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
        private Log rch;

        // Курсы валют
        private readonly decimal usdRate = 90.0m;
        private readonly decimal eurRate = 98.0m;

        public AddSaleForm(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyStyles();
            LoadClients();
            LoadGoods();
            rch.LogInfo("Форма оформления продажи инициализирована");
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (dgvOrderItems != null)
                {
                    Styles.ApplyDataGridViewStyle(dgvOrderItems);
                    dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    dgvOrderItems.ColumnHeadersHeight = 50;
                    dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold);
                    dgvOrderItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                    // Добавить эти строки для серого выделения
                    dgvOrderItems.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.Black;
                    dgvOrderItems.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
                    dgvOrderItems.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
                    dgvOrderItems.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
                }
                if (cmbClients != null) Styles.ApplyComboBoxStyle(cmbClients);
                if (cmbGoods != null) Styles.ApplyComboBoxStyle(cmbGoods);
                if (cmbCurrency != null) Styles.ApplyComboBoxStyle(cmbCurrency);
                if (txtQuantity != null) Styles.ApplyTextBoxStyle(txtQuantity);
                if (dtOrderDate != null) Styles.ApplyDateTimePickerStyle(dtOrderDate);
                if (dtDeliveryDate != null) Styles.ApplyDateTimePickerStyle(dtDeliveryDate);
                if (btnAddItem != null) Styles.ApplySecondaryButtonStyle(btnAddItem);
                if (btnCreateOrder != null) Styles.ApplyButtonStyle(btnCreateOrder);
                if (btnCancel != null) Styles.ApplySecondaryButtonStyle(btnCancel);

                rch.LogInfo("Стили формы продажи применены успешно");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка применения стилей формы продажи: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавить заказ";
            this.Size = new System.Drawing.Size(1000, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            Label titleLabel = new Label()
            {
                Text = "Оформление продажи",
                Font = new Font(Styles.MainFont.FontFamily, 16F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 50
            };

            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 9;
            mainPanel.ColumnCount = 4;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.Transparent;

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

            for (int i = 0; i < 4; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            }
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));

            for (int i = 0; i < 4; i++)
            {
                mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }

            Label lblClient = new Label() { Text = "Клиент:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblClient, true);
            this.cmbClients.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbClients.Height = 35;

            Label lblDate = new Label() { Text = "Дата заказа:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblDate, true);
            this.dtOrderDate.Value = DateTime.Now;
            this.dtOrderDate.Height = 35;

            Label lblDeliveryDate = new Label() { Text = "Дата доставки:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblDeliveryDate, true);
            this.dtDeliveryDate.Value = DateTime.Now.AddDays(3);
            this.dtDeliveryDate.Height = 35;

            Label lblCurrency = new Label() { Text = "Валюта:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblCurrency);
            this.cmbCurrency.Items.AddRange(new string[] { "RUB - Российский рубль", "USD - Доллар США", "EUR - Евро" });
            this.cmbCurrency.SelectedIndex = 0;
            this.cmbCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCurrency.Height = 35;

            Label lblGood = new Label() { Text = "Товар:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblGood, true);
            this.cmbGoods.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGoods.Height = 35;

            Label lblQuantity = new Label() { Text = "Количество:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblQuantity, true);
            this.txtQuantity.Text = "1";
            this.txtQuantity.Height = 35;

            this.btnAddItem.Text = "Добавить товар";
            this.btnAddItem.Size = new Size(180, 70);
            this.btnAddItem.Font = new Font(Styles.MainFont.FontFamily, 14F, FontStyle.Bold);
            this.btnAddItem.Click += this.AddItemToOrder!;

            this.btnCancel.Text = "Отмена";
            this.btnCancel.Size = new Size(160, 45);
            this.btnCancel.Font = new Font(Styles.MainFont.FontFamily, 10F);
            this.btnCancel.Click += (s, e) =>
            {
                rch.LogInfo("Форма оформления продажи закрыта по отмене");
                this.Close();
            };

            this.dgvOrderItems.Dock = DockStyle.Fill;
            this.dgvOrderItems.AllowUserToAddRows = false;
            this.dgvOrderItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrderItems.Height = 300;
            this.dgvOrderItems.MinimumSize = new Size(0, 300);
            this.dgvOrderItems.Font = new Font(Styles.MainFont.FontFamily, 10F);

            this.dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvOrderItems.ColumnHeadersHeight = 50;
            this.dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold);
            this.dgvOrderItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            this.dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            this.dgvOrderItems.RowTemplate.Height = 35;

            this.dgvOrderItems.Columns.Add("GoodId", "ID товара");
            this.dgvOrderItems.Columns.Add("GoodName", "Товар");
            this.dgvOrderItems.Columns.Add("Quantity", "Количество");
            this.dgvOrderItems.Columns.Add("Price", "Цена");
            this.dgvOrderItems.Columns.Add("Stock", "В наличии");
            this.dgvOrderItems.Columns.Add("Currency", "Валюта");
            this.dgvOrderItems.Columns["GoodId"].Visible = false;

            foreach (DataGridViewColumn column in this.dgvOrderItems.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                column.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            }

            this.dgvOrderItems.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvOrderItems.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgvOrderItems.Columns["Price"].DefaultCellStyle.Format = "N2";
            this.dgvOrderItems.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvOrderItems.Columns["Currency"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.btnCreateOrder.Text = "Создать заказ";
            this.btnCreateOrder.Size = new Size(160, 45);
            this.btnCreateOrder.Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold);
            this.btnCreateOrder.Click += this.CreateOrder!;

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

            mainPanel.Controls.Add(new Panel() { BackColor = Color.Transparent }, 0, 3);
            mainPanel.Controls.Add(new Panel() { BackColor = Color.Transparent }, 1, 3);
            mainPanel.Controls.Add(new Panel() { BackColor = Color.Transparent }, 2, 3);
            mainPanel.Controls.Add(new Panel() { BackColor = Color.Transparent }, 3, 3);

            mainPanel.Controls.Add(this.btnAddItem, 0, 4);
            mainPanel.SetColumnSpan(this.btnAddItem, 4);

            mainPanel.Controls.Add(this.dgvOrderItems, 0, 5);
            mainPanel.SetColumnSpan(this.dgvOrderItems, 4);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Height = 60;
            buttonPanel.Padding = new Padding(0, 10, 0, 10);
            buttonPanel.BackColor = Color.Transparent;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Controls.Add(this.btnCreateOrder);
            buttonPanel.Controls.Add(this.btnCancel);

            mainPanel.Controls.Add(buttonPanel, 0, 6);
            mainPanel.SetColumnSpan(buttonPanel, 4);

            this.Controls.Add(mainPanel);
            this.Controls.Add(titleLabel);

            titleLabel.Dock = DockStyle.Top;
            mainPanel.Dock = DockStyle.Fill;

            this.ResumeLayout(false);
            rch.LogInfo("Компоненты формы продажи инициализированы");
        }

        private void LoadClients()
        {
            try
            {
                rch.LogInfo("Начало загрузки списка клиентов");
                if (this.cmbClients == null) return;

                this.cmbClients.Items.Clear();
                this.cmbClients.Items.Add(new ClientItem { Id = 0, Display = "Выберите клиента" });

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("SELECT id, surname, name, middlename FROM clients ORDER BY surname, name", connection);
                    var reader = command.ExecuteReader();

                    int count = 0;
                    while (reader.Read())
                    {
                        string middlename = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        this.cmbClients.Items.Add(new ClientItem
                        {
                            Id = reader.GetInt32(0),
                            Display = $"{reader.GetString(1)} {reader.GetString(2)} {middlename}".Trim()
                        });
                        count++;
                    }
                    rch.LogInfo($"Загружено клиентов: {count}");
                }
                this.cmbClients.SelectedIndex = 0;
                rch.LogInfo("Список клиентов успешно загружен");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки клиентов: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}");
            }
        }

        private void LoadGoods()
        {
            try
            {
                rch.LogInfo("Начало загрузки списка товаров");
                if (this.cmbGoods == null) return;

                this.cmbGoods.Items.Clear();
                this.cmbGoods.Items.Add(new GoodItem { Id = 0, Display = "Выберите товар" });

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("SELECT id, name, price, stock_quantity, currency FROM goods WHERE stock_quantity > 0 ORDER BY name", connection);
                    var reader = command.ExecuteReader();

                    int count = 0;
                    while (reader.Read())
                    {
                        this.cmbGoods.Items.Add(new GoodItem
                        {
                            Id = reader.GetInt32(0),
                            Display = $"{reader.GetString(1)} - {reader.GetDecimal(2):0.00} {reader.GetString(4)} . (в наличии: {reader.GetInt32(3)})"
                        });
                        count++;
                    }
                    rch.LogInfo($"Загружено товаров: {count}");
                }
                this.cmbGoods.SelectedIndex = 0;
                rch.LogInfo("Список товаров успешно загружен");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки товаров: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}");
            }
        }

        private void AddItemToOrder(object? sender, EventArgs e)
        {
            rch.LogInfo("Попытка добавления товара в заказ");

            if (this.cmbGoods?.SelectedItem is not GoodItem good || good.Id == 0)
            {
                rch.LogWarning("Не выбран товар для добавления в заказ");
                MessageBox.Show("Выберите товар");
                return;
            }

            if (this.txtQuantity == null || !int.TryParse(this.txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                rch.LogWarning($"Некорректное количество товара: {txtQuantity?.Text}");
                MessageBox.Show("Укажите корректное количество");
                return;
            }

            var goodInfo = GetGoodInfo(good.Id);
            if (goodInfo.Stock < quantity)
            {
                rch.LogWarning($"Недостаточно товара ID {good.Id}. Запрошено: {quantity}, в наличии: {goodInfo.Stock}");
                MessageBox.Show($"Недостаточно товара на складе. В наличии: {goodInfo.Stock}");
                return;
            }

            if (this.dgvOrderItems != null)
            {
                string currency = this.cmbCurrency?.SelectedItem?.ToString()?.Split(' ')[0] ?? "RUB";
                decimal priceInCurrency = ConvertToCurrency(goodInfo.Price, currency);
                decimal totalLine = priceInCurrency * quantity;

                // Проверка на дубликат
                bool exists = false;
                foreach (DataGridViewRow row in this.dgvOrderItems.Rows)
                {
                    if (row.Cells["GoodId"].Value?.ToString() == good.Id.ToString())
                    {
                        int currentQty = Convert.ToInt32(row.Cells["Quantity"].Value);
                        row.Cells["Quantity"].Value = currentQty + quantity;
                        row.Cells["Price"].Value = priceInCurrency * (currentQty + quantity);
                        exists = true;
                        rch.LogInfo($"Товар ID {good.Id} обновлен в заказе. Новое количество: {currentQty + quantity}");
                        break;
                    }
                }

                if (!exists)
                {
                    this.dgvOrderItems.Rows.Add(
                        good.Id,
                        good.Display.Split('-')[0].Trim(),
                        quantity,
                        totalLine,
                        goodInfo.Stock,
                        currency
                    );
                    rch.LogInfo($"Товар ID {good.Id} добавлен в заказ. Количество: {quantity}, Цена: {totalLine} {currency}");
                }
            }

            this.txtQuantity!.Text = "1";
            rch.LogInfo("Товар успешно добавлен в заказ");
        }

        private decimal ConvertToCurrency(decimal priceInRubles, string currency)
        {
            decimal result = currency switch
            {
                "USD" => priceInRubles / usdRate,
                "EUR" => priceInRubles / eurRate,
                _ => priceInRubles
            };
            rch.LogInfo($"Конвертация цены: {priceInRubles} RUB -> {result} {currency}");
            return result;
        }

        private decimal ConvertToRubles(decimal price, string currency)
        {
            decimal result = currency switch
            {
                "USD" => price * usdRate,
                "EUR" => price * eurRate,
                _ => price
            };
            rch.LogInfo($"Конвертация цены: {price} {currency} -> {result} RUB");
            return result;
        }

        private (decimal Price, int Stock) GetGoodInfo(int goodId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("SELECT price, stock_quantity FROM goods WHERE id = @id", connection);
                    command.Parameters.AddWithValue("id", goodId);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var result = (reader.GetDecimal(0), reader.GetInt32(1));
                        rch.LogInfo($"Получена информация о товаре ID {goodId}: цена {result.Item1}, остаток {result.Item2}");
                        return result;
                    }
                    rch.LogWarning($"Товар ID {goodId} не найден в базе данных");
                    return (0, 0);
                }
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка получения информации о товаре ID {goodId}: {ex.Message}");
                return (0, 0);
            }
        }

        private void CreateOrder(object? sender, EventArgs e)
        {
            rch.LogInfo("Начало создания заказа");

            if (this.cmbClients?.SelectedItem is not ClientItem client || client.Id == 0)
            {
                rch.LogWarning("Не выбран клиент для создания заказа");
                MessageBox.Show("Выберите клиента");
                return;
            }

            if (this.dgvOrderItems == null || this.dgvOrderItems.Rows.Count == 0)
            {
                rch.LogWarning("Попытка создания заказа без товаров");
                MessageBox.Show("Добавьте хотя бы один товар в заказ");
                return;
            }

            if (this.dtDeliveryDate!.Value < this.dtOrderDate!.Value)
            {
                rch.LogWarning("Дата доставки раньше даты заказа");
                MessageBox.Show("Дата доставки не может быть раньше даты заказа");
                return;
            }

            decimal totalAmountRub = 0;
            int itemCount = 0;

            foreach (DataGridViewRow row in this.dgvOrderItems.Rows)
            {
                if (row.Cells["GoodId"].Value == null) continue;

                int goodId = Convert.ToInt32(row.Cells["GoodId"].Value);
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                int stock = Convert.ToInt32(row.Cells["Stock"].Value);
                string currency = row.Cells["Currency"].Value?.ToString() ?? "RUB";
                decimal totalLine = Convert.ToDecimal(row.Cells["Price"].Value);

                if (quantity > stock)
                {
                    rch.LogError($"Недостаточно товара ID {goodId} для заказа. Запрошено: {quantity}, в наличии: {stock}");
                    MessageBox.Show($"Недостаточно товара '{row.Cells["GoodName"].Value}' на складе");
                    return;
                }

                decimal totalInRub = ConvertToRubles(totalLine, currency);
                totalAmountRub += totalInRub;
                itemCount++;
            }

            decimal discount = totalAmountRub > 5000 ? 2.0m : 0.0m;
            decimal finalAmount = totalAmountRub * (1 - discount / 100);
            bool makeConstant = totalAmountRub > 5000;

            rch.LogInfo($"Расчет заказа: товаров {itemCount}, сумма {totalAmountRub:RUB}, скидка {discount}%, итого {finalAmount:RUB}");

            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        rch.LogInfo("Начало транзакции создания заказа");

                        // Создание заказа
                        var orderCommand = new NpgsqlCommand(@"
                            INSERT INTO orders (client_id, order_date, delivery_date, total_amount, discount) 
                            VALUES (@client_id, @order_date, @delivery_date, @total_amount, @discount) RETURNING id",
                            connection, transaction);

                        orderCommand.Parameters.AddWithValue("client_id", client.Id);
                        orderCommand.Parameters.AddWithValue("order_date", this.dtOrderDate.Value);
                        orderCommand.Parameters.AddWithValue("delivery_date", this.dtDeliveryDate.Value);
                        orderCommand.Parameters.AddWithValue("total_amount", finalAmount);
                        orderCommand.Parameters.AddWithValue("discount", discount);

                        int orderId = Convert.ToInt32(orderCommand.ExecuteScalar());
                        rch.LogInfo($"Создан заказ ID: {orderId} для клиента ID: {client.Id}");

                        // Добавление позиций и обновление остатков
                        int itemsAdded = 0;
                        foreach (DataGridViewRow row in this.dgvOrderItems.Rows)
                        {
                            if (row.Cells["GoodId"].Value == null) continue;

                            int goodId = Convert.ToInt32(row.Cells["GoodId"].Value);
                            int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                            string currency = row.Cells["Currency"].Value?.ToString() ?? "RUB";
                            decimal totalLine = Convert.ToDecimal(row.Cells["Price"].Value);
                            decimal pricePerUnit = totalLine / quantity;

                            var itemCommand = new NpgsqlCommand(@"
                                INSERT INTO order_items (order_id, good_id, quantity, price, currency) 
                                VALUES (@order_id, @good_id, @quantity, @price, @currency)",
                                connection, transaction);

                            itemCommand.Parameters.AddWithValue("order_id", orderId);
                            itemCommand.Parameters.AddWithValue("good_id", goodId);
                            itemCommand.Parameters.AddWithValue("quantity", quantity);
                            itemCommand.Parameters.AddWithValue("price", pricePerUnit);
                            itemCommand.Parameters.AddWithValue("currency", currency);
                            itemCommand.ExecuteNonQuery();

                            var updateStockCommand = new NpgsqlCommand(@"
                                UPDATE goods SET stock_quantity = stock_quantity - @quantity 
                                WHERE id = @good_id", connection, transaction);
                            updateStockCommand.Parameters.AddWithValue("quantity", quantity);
                            updateStockCommand.Parameters.AddWithValue("good_id", goodId);
                            updateStockCommand.ExecuteNonQuery();

                            itemsAdded++;
                            rch.LogInfo($"Добавлена позиция заказа: товар ID {goodId}, количество {quantity}, цена {pricePerUnit} {currency}");
                        }

                        // Обновление статуса клиента
                        if (makeConstant)
                        {
                            var updateClientCommand = new NpgsqlCommand(@"
                                UPDATE clients SET constclient = true WHERE id = @client_id", connection, transaction);
                            updateClientCommand.Parameters.AddWithValue("client_id", client.Id);
                            updateClientCommand.ExecuteNonQuery();
                            rch.LogInfo($"Клиент ID {client.Id} установлен как постоянный");
                        }

                        transaction.Commit();
                        rch.LogInfo($"Заказ №{orderId} успешно создан. Позиций: {itemsAdded}, Итоговая сумма: {finalAmount:0.00} руб.");
                        MessageBox.Show($"Заказ №{orderId} успешно создан!\nОбщая сумма: {finalAmount:0.00} руб.\nСкидка: {discount}%");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка создания заказа: {ex.Message}");
                MessageBox.Show($"Ошибка создания заказа: {ex.Message}");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            rch.LogInfo($"Форма оформления продажи закрыта. Причина: {e.CloseReason}");
            base.OnFormClosed(e);
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