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
        private Button? changeTableButton;
        Log rch;

        public SellsUC(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyDataGridViewStyle();
            this.rch = rch;
        }

        private void InitializeComponent()
        {
            this.btnRefresh = new Button();
            this.btnDelete = new Button();
            this.btnViewDetails = new Button();
            this.dataGridView1 = new DataGridView();
            this.changeTableButton = new Button();

            SuspendLayout();

            // Панель для кнопок
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 80; // Увеличена высота панели
            buttonPanel.Padding = new Padding(10, 12, 10, 12); // Увеличены отступы
            buttonPanel.BackColor = Color.Transparent;

            // Общие настройки для кнопок
            Size buttonSize = new Size(140, 60); // Увеличена высота кнопок до 40px
            int buttonSpacing = 15; // Расстояние между кнопками

            // btnRefresh
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Size = buttonSize;
            this.btnRefresh.Location = new Point(10, 10);
            this.btnRefresh.Click += (s, e) => RefreshData();

            // btnViewDetails
            this.btnViewDetails.Text = "Детали заказа";
            this.btnViewDetails.Size = buttonSize;
            this.btnViewDetails.Location = new Point(btnRefresh.Right + buttonSpacing, 10);
            this.btnViewDetails.Click += ViewOrderDetails;

            // btnDelete
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Size = buttonSize;
            this.btnDelete.Location = new Point(btnViewDetails.Right + buttonSpacing, 10);
            this.btnDelete.Click += DeleteSelectedOrder;

            this.changeTableButton.Text = "Изменить\nтаблицу";
            this.changeTableButton.Size = buttonSize;
            this.changeTableButton.Location = new Point(btnDelete.Right + buttonSpacing, 10);
            this.changeTableButton.Click += ChangeTableClick;

            // dataGridView1
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.BorderStyle = BorderStyle.FixedSingle;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(0, 60);
            this.dataGridView1.Margin = new Padding(10);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.dataGridView1.ColumnHeadersHeight = 35;
            this.dataGridView1.RowHeadersVisible = false;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.btnRefresh);
            buttonPanel.Controls.Add(this.btnViewDetails);
            buttonPanel.Controls.Add(this.btnDelete);
            buttonPanel.Controls.Add(this.changeTableButton);
            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(buttonPanel);
            this.Name = "SellsUC";
            this.Size = new Size(800, 600);

            ResumeLayout(false);
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (this.dataGridView1 != null)
                {
                    Styles.ApplyDataGridViewStyle(this.dataGridView1);

                    // Дополнительные стили для улучшения внешнего вида
                    this.dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F);
                    this.dataGridView1.DefaultCellStyle.Padding = new Padding(3);
                    this.dataGridView1.RowTemplate.Height = 30;
                    this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

            // Применяем стили к кнопкам с увеличенной высотой
            if (btnRefresh != null)
            {
                Styles.ApplySecondaryButtonStyle(btnRefresh);
                btnRefresh.Font = new Font(btnRefresh.Font.FontFamily, 9F, FontStyle.Regular);
            }
            if (btnViewDetails != null)
            {
                Styles.ApplySecondaryButtonStyle(btnViewDetails);
                btnViewDetails.Font = new Font(btnViewDetails.Font.FontFamily, 9F, FontStyle.Regular);
            }
            if (btnDelete != null)
            {
                Styles.ApplyDangerButtonStyle(btnDelete);
                btnDelete.Font = new Font(btnDelete.Font.FontFamily, 9F, FontStyle.Regular);
            }
            if (changeTableButton != null)
            {
                Styles.ApplySecondaryButtonStyle(changeTableButton);
                changeTableButton.Font = new Font(changeTableButton.Font.FontFamily, 9F, FontStyle.Regular);
            }
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
                    var query = @$"
                        SELECT o.id, 
                               c.surname || ' ' || c.name as ""client"",
                               o.order_date,
                               o.delivery_date,
                               o.total_amount,
                               o.discount
                        FROM orders o
                        JOIN {MetaInformation.tables[0]} c ON o.client_id = c.id
                        ORDER BY o.order_date DESC";

                    var adapter = new NpgsqlDataAdapter(query, connection);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    this.dataGridView1.DataSource = dataTable;

                    // Настраиваем формат отображения
                    if (dataGridView1.Columns.Contains("Дата заказа"))
                    {
                        dataGridView1.Columns["Дата заказа"]!.DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridView1.Columns["Дата заказа"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dataGridView1.Columns.Contains("Дата доставки"))
                    {
                        dataGridView1.Columns["Дата доставки"]!.DefaultCellStyle.Format = "dd.MM.yyyy";
                        dataGridView1.Columns["Дата доставки"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dataGridView1.Columns.Contains("Сумма"))
                    {
                        dataGridView1.Columns["Сумма"]!.DefaultCellStyle.Format = "N2";
                        dataGridView1.Columns["Сумма"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dataGridView1.Columns.Contains("Скидка %"))
                    {
                        dataGridView1.Columns["Скидка %"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    // Автоматическое изменение размера колонок после загрузки данных
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ChangeTableClick(object? sender, EventArgs e)
        {
            ClientTableChange table = new ClientTableChange(rch, 3);
            table.ShowDialog();
        }
    }
}