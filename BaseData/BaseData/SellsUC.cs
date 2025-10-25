using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class SellsUC : UserControl
    {
        private static string currentConnectionString;
        private Button? btnSearch;
        private Button? btnDelete;
        private Button? btnViewDetails;
        private Button? btnJoinMaster;
        private static DataGridView dataGridView1;
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
            this.btnSearch = new Button();
            this.btnDelete = new Button();
            this.btnViewDetails = new Button();
            dataGridView1 = new DataGridView();
            this.changeTableButton = new Button();
            this.btnJoinMaster = new Button();

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
            this.btnSearch.Text = "Поиск";
            this.btnSearch.Size = buttonSize;
            this.btnSearch.Location = new Point(10, 10);
            this.btnSearch.Click += BtnSearch_Click;

            // btnViewDetails
            this.btnViewDetails.Text = "Детали заказа";
            this.btnViewDetails.Size = buttonSize;
            this.btnViewDetails.Location = new Point(btnSearch.Right + buttonSpacing, 10);
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

            // join
            this.btnJoinMaster.Text = "Мастер\nсоединений";
            this.btnJoinMaster.Size = buttonSize;
            this.btnJoinMaster.Location = new Point(changeTableButton.Right + buttonSpacing, 10);
            this.btnJoinMaster.Click += BtnJoinMaster_Click;

            // dataGridView1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.FixedSingle;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 60);
            dataGridView1.Margin = new Padding(10);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.TabIndex = 3;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 35;
            dataGridView1.RowHeadersVisible = false;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.btnSearch);
            buttonPanel.Controls.Add(this.btnViewDetails);
            buttonPanel.Controls.Add(this.btnDelete);
            buttonPanel.Controls.Add(this.changeTableButton);
            buttonPanel.Controls.Add(this.btnJoinMaster);
            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(dataGridView1);
            this.Controls.Add(buttonPanel);
            this.Name = "SellsUC";
            this.Size = new Size(800, 600);

            ResumeLayout(false);
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (dataGridView1 != null)
                {
                    Styles.ApplyDataGridViewStyle(dataGridView1);

                    // Дополнительные стили для улучшения внешнего вида
                    dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F);
                    dataGridView1.DefaultCellStyle.Padding = new Padding(3);
                    dataGridView1.RowTemplate.Height = 30;
                    dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            if (btnSearch != null)
            {
                Styles.ApplySecondaryButtonStyle(btnSearch);
                btnSearch.Font = new Font(btnSearch.Font.FontFamily, 9F, FontStyle.Regular);
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
            if (btnJoinMaster != null)
            {
                Styles.ApplySecondaryButtonStyle(btnJoinMaster);
                btnJoinMaster.Font = new Font(btnJoinMaster.Font.FontFamily, 9F, FontStyle.Regular);
            }
        }

        // обработчик
        private void BtnJoinMaster_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!AppSettings.IsConnectionStringSet)
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rch.LogError("Попытка открыть мастер JOIN без подключения к БД");
                    return;
                }

                JoinBuilderForm joinForm = new JoinBuilderForm(rch);
                joinForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия мастера JOIN: {ex.Message}", "Ошибка");
                rch.LogError($"Ошибка открытия мастера JOIN: {ex.Message}");
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

static public void RefreshData()
{
    if (dataGridView1 == null) return;


    try
    {
        if (string.IsNullOrEmpty(currentConnectionString))
            return;
                

        var orderColumnsForSelect = MetaInformation.columnsOrders
            .Where(col => col != "client_id")
            .Select(col => $"o.{col}")
            .ToArray();

        var selectList = string.Join(", ", orderColumnsForSelect) + 
                         ", c.surname || ' ' || c.name AS \"client\"";

        var query = $@"
            SELECT {selectList}
            FROM {MetaInformation.tables[3]} o
            JOIN {MetaInformation.tables[0]} c ON o.client_id = c.id";

        using (var connection = new NpgsqlConnection(currentConnectionString))
        {
            connection.Open();
            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;

            // Настройка форматов по фактическим именам столбцов (как в БД)
            FormatDataGridViewColumns();
            
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

// Выносим форматирование в отдельный метод для читаемости
private static void FormatDataGridViewColumns()
{
    var dgv = dataGridView1;
    if (dgv == null) return;

    // Даты
    if (dgv.Columns.Contains("order_date"))
    {
        dgv.Columns["order_date"].DefaultCellStyle.Format = "dd.MM.yyyy";
        dgv.Columns["order_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }
    if (dgv.Columns.Contains("delivery_date"))
    {
        dgv.Columns["delivery_date"].DefaultCellStyle.Format = "dd.MM.yyyy";
        dgv.Columns["delivery_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    // Сумма
    if (dgv.Columns.Contains("total_amount"))
    {
        dgv.Columns["total_amount"].DefaultCellStyle.Format = "N2";
        dgv.Columns["total_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    // Скидка
    if (dgv.Columns.Contains("discount"))
    {
        dgv.Columns["discount"].DefaultCellStyle.Format = "N2"; // или "P" если хранится как доля (0.1 = 10%)
        dgv.Columns["discount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }
}
        static private void ShowColumnNames(string[] columnNames)
        {
            if (columnNames == null || columnNames.Length == 0)
            {
                MessageBox.Show("Список столбцов пуст.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string message = string.Join(Environment.NewLine, columnNames);
            MessageBox.Show(message, "Названия столбцов",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ChangeTableClick(object? sender, EventArgs e)
        {
            ClientTableChange table = new ClientTableChange(rch, 3);
            table.ShowDialog();
        }
        private void BtnSearch_Click(object? sender, EventArgs e)
        {

        }
    }
}