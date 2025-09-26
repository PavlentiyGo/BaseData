using System;
using System.Windows.Forms;
using Npgsql;
using System.Data;

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

        public AddSaleForm()
        {
            InitializeComponent();
            LoadClients();
            LoadGoods();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавить заказ";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            // Инициализация всех полей
            this.dgvOrderItems = new DataGridView();
            this.cmbClients = new ComboBox();
            this.cmbGoods = new ComboBox();
            this.txtQuantity = new TextBox();
            this.dtOrderDate = new DateTimePicker();
            this.dtDeliveryDate = new DateTimePicker();

            // Выбор клиента
            Label lblClient = new Label() { Text = "Клиент:", Location = new System.Drawing.Point(10, 15), Size = new System.Drawing.Size(80, 20) };
            this.cmbClients.Location = new System.Drawing.Point(100, 10);
            this.cmbClients.Size = new System.Drawing.Size(200, 20);

            // Дата заказа
            Label lblDate = new Label() { Text = "Дата заказа:", Location = new System.Drawing.Point(320, 15), Size = new System.Drawing.Size(80, 20) };
            this.dtOrderDate.Location = new System.Drawing.Point(410, 10);
            this.dtOrderDate.Size = new System.Drawing.Size(150, 20);
            this.dtOrderDate.Value = DateTime.Now;

            // Дата доставки
            Label lblDeliveryDate = new Label() { Text = "Дата доставки:", Location = new System.Drawing.Point(10, 45), Size = new System.Drawing.Size(80, 20) };
            this.dtDeliveryDate.Location = new System.Drawing.Point(100, 40);
            this.dtDeliveryDate.Size = new System.Drawing.Size(150, 20);
            this.dtDeliveryDate.Value = DateTime.Now.AddDays(3);

            // Выбор товара
            Label lblGood = new Label() { Text = "Товар:", Location = new System.Drawing.Point(320, 45), Size = new System.Drawing.Size(80, 20) };
            this.cmbGoods.Location = new System.Drawing.Point(410, 40);
            this.cmbGoods.Size = new System.Drawing.Size(150, 20);

            // Количество
            Label lblQuantity = new Label() { Text = "Количество:", Location = new System.Drawing.Point(10, 75), Size = new System.Drawing.Size(80, 20) };
            this.txtQuantity.Location = new System.Drawing.Point(100, 70);
            this.txtQuantity.Size = new System.Drawing.Size(80, 20);
            this.txtQuantity.Text = "1";

            // Кнопка добавления товара в заказ
            Button btnAddItem = new Button() { Text = "Добавить товар", Location = new System.Drawing.Point(200, 68), Size = new System.Drawing.Size(100, 25) };
            btnAddItem.Click += this.AddItemToOrder;

            // Таблица товаров в заказе
            this.dgvOrderItems.Location = new System.Drawing.Point(10, 100);
            this.dgvOrderItems.Size = new System.Drawing.Size(570, 200);
            this.dgvOrderItems.AllowUserToAddRows = false;
            this.dgvOrderItems.Columns.Add("GoodId", "ID товара");
            this.dgvOrderItems.Columns.Add("GoodName", "Товар");
            this.dgvOrderItems.Columns.Add("Quantity", "Количество");
            this.dgvOrderItems.Columns.Add("Price", "Цена");
            this.dgvOrderItems.Columns.Add("Stock", "В наличии");
            this.dgvOrderItems.Columns["GoodId"].Visible = false;

            // Кнопки
            Button btnCreateOrder = new Button() { Text = "Создать заказ", Location = new System.Drawing.Point(400, 320), Size = new System.Drawing.Size(100, 30) };
            btnCreateOrder.Click += this.CreateOrder;

            Button btnCancel = new Button() { Text = "Отмена", Location = new System.Drawing.Point(510, 320), Size = new System.Drawing.Size(70, 30) };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblClient, this.cmbClients, lblDate, this.dtOrderDate,
                lblDeliveryDate, this.dtDeliveryDate, lblGood, this.cmbGoods,
                lblQuantity, this.txtQuantity, btnAddItem,
                this.dgvOrderItems, btnCreateOrder, btnCancel
            });

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
                this.dgvOrderItems.Rows.Add(good.Id, good.Display.Split('-')[0].Trim(), quantity, goodInfo.Price * quantity, goodInfo.Stock);
            }
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

            // Проверяем остатки и подсчитываем сумму
            foreach (DataGridViewRow row in this.dgvOrderItems.Rows)
            {
                if (row.Cells["GoodId"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    int goodId = Convert.ToInt32(row.Cells["GoodId"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    int stock = Convert.ToInt32(row.Cells["Stock"].Value);

                    if (quantity > stock)
                    {
                        MessageBox.Show($"Недостаточно товара '{row.Cells["GoodName"].Value}' на складе");
                        return;
                    }

                    if (row.Cells["Price"].Value != null)
                        totalAmount += Convert.ToDecimal(row.Cells["Price"].Value);
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

                            var itemCommand = new NpgsqlCommand(@"
                                INSERT INTO order_items (order_id, good_id, quantity, price) 
                                VALUES (@order_id, @good_id, @quantity, @price)",
                                connection);

                            itemCommand.Parameters.AddWithValue("order_id", orderId);
                            itemCommand.Parameters.AddWithValue("good_id", goodId);
                            itemCommand.Parameters.AddWithValue("quantity", quantity);
                            itemCommand.Parameters.AddWithValue("price", Convert.ToDecimal(row.Cells["Price"].Value));

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