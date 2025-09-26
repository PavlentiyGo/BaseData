using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class SellsUC : UserControl
    {
        private string? currentConnectionString;
        private Button? btnRefresh;
        private Button? btnDelete;
        private Button? btnViewDetails;
        private DataGridView? dataGridView1;

        public SellsUC()
        {
            InitializeComponent();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.btnRefresh = new Button();
            this.btnDelete = new Button();
            this.btnViewDetails = new Button();
            this.dataGridView1 = new DataGridView();

            SuspendLayout();

            // Панель для кнопок
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 50;
            buttonPanel.Padding = new Padding(10);
            buttonPanel.BackColor = Color.Transparent;

            // btnRefresh
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Size = new Size(100, 35);
            this.btnRefresh.Location = new Point(10, 7);
            this.btnRefresh.Click += (s, e) => RefreshData();

            // btnViewDetails
            this.btnViewDetails.Text = "Детали заказа";
            this.btnViewDetails.Size = new Size(120, 35);
            this.btnViewDetails.Location = new Point(120, 7);
            this.btnViewDetails.Click += ViewOrderDetails;

            // btnDelete
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Size = new Size(100, 35);
            this.btnDelete.Location = new Point(250, 7);
            this.btnDelete.Click += DeleteSelectedOrder;

            // dataGridView1
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.BorderStyle = BorderStyle.FixedSingle;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(0, 50);
            this.dataGridView1.Margin = new Padding(10);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.btnRefresh);
            buttonPanel.Controls.Add(this.btnViewDetails);
            buttonPanel.Controls.Add(this.btnDelete);

            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(buttonPanel);
            this.Name = "SellsUC";
            this.Size = new Size(800, 500);

            ResumeLayout(false);
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (this.dataGridView1 != null)
                {
                    Styles.ApplyDataGridViewStyle(this.dataGridView1);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стиля: {ex.Message}");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (btnRefresh != null) Styles.ApplySecondaryButtonStyle(btnRefresh);
            if (btnViewDetails != null) Styles.ApplySecondaryButtonStyle(btnViewDetails);
            if (btnDelete != null) Styles.ApplyDangerButtonStyle(btnDelete);
        }

        private void DeleteSelectedOrder(object? sender, EventArgs e)
        {
            if (dataGridView1?.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ для удаления", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ? Это действие нельзя отменить.",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int orderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Номер заказа"].Value);

                    using (var connection = new NpgsqlConnection(currentConnectionString))
                    {
                        connection.Open();

                        // Восстанавливаем остатки товаров
                        var restoreStockCommand = new NpgsqlCommand(@"
                            UPDATE goods 
                            SET stock_quantity = stock_quantity + oi.quantity
                            FROM order_items oi 
                            WHERE goods.id = oi.good_id AND oi.order_id = @orderId",
                            connection);
                        restoreStockCommand.Parameters.AddWithValue("orderId", orderId);
                        restoreStockCommand.ExecuteNonQuery();

                        // Удаляем элементы заказа
                        var deleteItemsCommand = new NpgsqlCommand(
                            "DELETE FROM order_items WHERE order_id = @orderId", connection);
                        deleteItemsCommand.Parameters.AddWithValue("orderId", orderId);
                        deleteItemsCommand.ExecuteNonQuery();

                        // Удаляем сам заказ
                        var deleteOrderCommand = new NpgsqlCommand(
                            "DELETE FROM orders WHERE id = @orderId", connection);
                        deleteOrderCommand.Parameters.AddWithValue("orderId", orderId);
                        deleteOrderCommand.ExecuteNonQuery();

                        MessageBox.Show("Заказ успешно удален", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления заказа: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ViewOrderDetails(object? sender, EventArgs e)
        {
            if (dataGridView1?.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ для просмотра деталей", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int orderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Номер заказа"].Value);
            ShowOrderDetails(orderId);
        }

        private void ShowOrderDetails(int orderId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(currentConnectionString))
                {
                    connection.Open();
                    var query = @"
                        SELECT g.name as ""Товар"", 
                               oi.quantity as ""Количество"", 
                               oi.price as ""Цена за единицу"",
                               (oi.quantity * oi.price) as ""Сумма"",
                               g.unit as ""Единица""
                        FROM order_items oi
                        JOIN goods g ON oi.good_id = g.id
                        WHERE oi.order_id = @orderId";

                    var adapter = new NpgsqlDataAdapter(query, connection);
                    adapter.SelectCommand!.Parameters.AddWithValue("orderId", orderId);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Детали заказа не найдены", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string details = $"Детали заказа №{orderId}:\n\n";
                    decimal orderTotal = 0;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string productName = row["Товар"].ToString() ?? "";
                        int quantity = Convert.ToInt32(row["Количество"]);
                        decimal price = Convert.ToDecimal(row["Цена за единицу"]);
                        decimal total = Convert.ToDecimal(row["Сумма"]);
                        string unit = row["Единица"].ToString() ?? "";

                        details += $"{productName} - {quantity} {unit} × {price:0.00} руб. = {total:0.00} руб.\n";
                        orderTotal += total;
                    }

                    // Получаем информацию о скидке
                    var discountCommand = new NpgsqlCommand(
                        "SELECT discount, total_amount FROM orders WHERE id = @orderId", connection);
                    discountCommand.Parameters.AddWithValue("orderId", orderId);
                    var reader = discountCommand.ExecuteReader();

                    decimal discount = 0;
                    decimal finalAmount = orderTotal;

                    if (reader.Read())
                    {
                        discount = reader.GetDecimal(0);
                        finalAmount = reader.GetDecimal(1);
                    }
                    reader.Close();

                    details += $"\nИтого: {orderTotal:0.00} руб.";

                    if (discount > 0)
                    {
                        details += $"\nСкидка: {discount}%";
                        details += $"\nК оплате: {finalAmount:0.00} руб.";
                    }

                    MessageBox.Show(details, "Детали заказа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки деталей заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadData(string? connectionString)
        {
            currentConnectionString = connectionString;
            RefreshData();
        }

        private void RefreshData()
        {
            if (this.dataGridView1 == null) return;

            try
            {
                if (string.IsNullOrEmpty(currentConnectionString))
                {
                    return;
                }

                using (var connection = new NpgsqlConnection(currentConnectionString))
                {
                    connection.Open();
                    var query = @"
                        SELECT o.id as ""Номер заказа"", 
                               c.surname || ' ' || c.name as ""Клиент"",
                               o.order_date as ""Дата заказа"",
                               o.delivery_date as ""Дата доставки"",
                               o.total_amount as ""Сумма"",
                               o.discount as ""Скидка %""
                        FROM orders o
                        JOIN clients c ON o.client_id = c.id
                        ORDER BY o.order_date DESC";

                    var adapter = new NpgsqlDataAdapter(query, connection);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    this.dataGridView1.DataSource = dataTable;

                    // Настраиваем формат отображения
                    if (dataTable.Columns.Contains("Дата заказа"))
                    {
                        this.dataGridView1.Columns["Дата заказа"]!.DefaultCellStyle.Format = "dd.MM.yyyy";
                    }
                    if (dataTable.Columns.Contains("Дата доставки"))
                    {
                        this.dataGridView1.Columns["Дата доставки"]!.DefaultCellStyle.Format = "dd.MM.yyyy";
                    }
                    if (dataTable.Columns.Contains("Сумма"))
                    {
                        this.dataGridView1.Columns["Сумма"]!.DefaultCellStyle.Format = "N2";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}