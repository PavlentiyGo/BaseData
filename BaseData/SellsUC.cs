using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace BaseData
{
    public class SellsUC : UserControl
    {
        private static string currentConnectionString;
        private Button? btnDelete;
        private Button? btnViewDetails;
        private Button? sqlBuilderButton;
        private static DataGridView dataGridView1;
        private Button? changeTableButton;
        private TextBox? searchTextBox;
        private ComboBox? searchTypeComboBox;
        private Button? btnResetSearch;
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
            this.btnDelete = new Button();
            this.btnViewDetails = new Button();
            this.sqlBuilderButton = new Button();
            dataGridView1 = new DataGridView();
            this.changeTableButton = new Button();
            this.searchTextBox = new TextBox();
            this.searchTypeComboBox = new ComboBox();
            this.btnResetSearch = new Button();

            SuspendLayout();

            // Панель поиска
            Panel searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Height = 50;
            searchPanel.Padding = new Padding(10, 5, 10, 5);
            searchPanel.BackColor = Color.FromArgb(240, 240, 240);

            // Элементы поиска
            searchTextBox.Location = new Point(10, 12);
            searchTextBox.Size = new Size(200, 25);
            searchTextBox.PlaceholderText = "Введите текст для поиска...";
            searchTextBox.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch();
                }
            };

            searchTypeComboBox.Location = new Point(220, 12);
            searchTypeComboBox.Size = new Size(120, 25);
            searchTypeComboBox.Items.AddRange(new string[] { "ID заказа", "Клиент", "ID клиента", "Дата заказа" });
            searchTypeComboBox.SelectedIndex = 0;
            searchTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            var btnSearch = new Button();
            btnSearch.Text = "Найти";
            btnSearch.Location = new Point(350, 12);
            btnSearch.Size = new Size(80, 25);
            btnSearch.Click += (s, e) => PerformSearch();

            btnResetSearch.Text = "Сброс";
            btnResetSearch.Location = new Point(440, 12);
            btnResetSearch.Size = new Size(80, 25);
            btnResetSearch.Click += (s, e) =>
            {
                searchTextBox.Text = "";
                RefreshData();
            };

            searchPanel.Controls.Add(searchTextBox);
            searchPanel.Controls.Add(searchTypeComboBox);
            searchPanel.Controls.Add(btnSearch);
            searchPanel.Controls.Add(btnResetSearch);

            // Панель для кнопок
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 80;
            buttonPanel.Padding = new Padding(10, 12, 10, 12);
            buttonPanel.BackColor = Color.Transparent;

            // Общие настройки для кнопок
            Size buttonSize = new Size(140, 60);
            int buttonSpacing = 15;

            // btnViewDetails
            this.btnViewDetails.Text = "Детали заказа";
            this.btnViewDetails.Size = buttonSize;
            this.btnViewDetails.Location = new Point(buttonSpacing, 10);
            this.btnViewDetails.Click += ViewOrderDetails;

            this.changeTableButton.Text = "Изменить\nтаблицу";
            this.changeTableButton.Size = buttonSize;
            this.changeTableButton.Location = new Point(btnViewDetails.Right + buttonSpacing, 10);
            this.changeTableButton.Click += ChangeTableClick;

            this.sqlBuilderButton.Text = "Конструктор\nSQL";
            this.sqlBuilderButton.Size = buttonSize;
            this.sqlBuilderButton.Location = new Point(changeTableButton.Right + buttonSpacing, 10);
            this.sqlBuilderButton.Click += OpenSqlBuilder;
            this.sqlBuilderButton.Font = new Font(sqlBuilderButton.Font.FontFamily, 9F, FontStyle.Regular);

            // btnDelete
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Size = buttonSize;
            this.btnDelete.Location = new Point(sqlBuilderButton.Right + buttonSpacing, 10);
            this.btnDelete.Click += DeleteSelectedOrder;

            // dataGridView1
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.FixedSingle;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 130);
            dataGridView1.Margin = new Padding(10);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.TabIndex = 3;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 35;
            dataGridView1.RowHeadersVisible = false;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.btnViewDetails);
            buttonPanel.Controls.Add(this.changeTableButton);
            buttonPanel.Controls.Add(this.sqlBuilderButton);
            buttonPanel.Controls.Add(this.btnDelete);

            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(dataGridView1);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(searchPanel);
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

                    // Устанавливаем серый цвет заголовков
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
                    dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

                    // Устанавливаем серый цвет выделения для ячеек и строк
                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
                    dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;

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
            if (sqlBuilderButton != null)
            {
                Styles.ApplySecondaryButtonStyle(sqlBuilderButton);
                sqlBuilderButton.Font = new Font(sqlBuilderButton.Font.FontFamily, 9F, FontStyle.Regular);
            }
            if (searchTextBox != null)
            {
                Styles.ApplyTextBoxStyle(searchTextBox);
            }
            if (searchTypeComboBox != null)
            {
                Styles.ApplyComboBoxStyle(searchTypeComboBox);
            }
            if (btnResetSearch != null)
            {
                Styles.ApplySecondaryButtonStyle(btnResetSearch);
                btnResetSearch.Font = new Font(btnResetSearch.Font.FontFamily, 9F, FontStyle.Regular);
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
                    int orderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

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

            int orderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
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

        private void OpenSqlBuilder(object? sender, EventArgs e)
        {
            try
            {
                if (!AppSettings.IsConnectionStringSet)
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rch.LogWarning("Попытка открыть конструктор SQL без подключения к БД");
                    return;
                }

                using (AdvancedSearchForm sqlBuilderForm = new AdvancedSearchForm(rch))
                {
                    rch.LogInfo("Открытие конструктора SQL запросов из формы продаж");
                    sqlBuilderForm.ShowDialog();
                    rch.LogInfo("Конструктор SQL запросов закрыт");
                }
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка при открытии конструктора SQL: {ex.Message}");
                MessageBox.Show($"Ошибка при открытии конструктора SQL: {ex.Message}", "Ошибка",
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

        private static void FormatDataGridViewColumns()
        {
            var dgv = dataGridView1;
            if (dgv == null) return;

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

            if (dgv.Columns.Contains("total_amount"))
            {
                dgv.Columns["total_amount"].DefaultCellStyle.Format = "N2";
                dgv.Columns["total_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgv.Columns.Contains("discount"))
            {
                dgv.Columns["discount"].DefaultCellStyle.Format = "N2";
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

        private void PerformSearch()
        {
            if (string.IsNullOrEmpty(searchTextBox?.Text) || dataGridView1 == null)
            {
                RefreshData();
                return;
            }

            try
            {
                string searchTerm = searchTextBox.Text.Trim();
                string searchType = searchTypeComboBox?.SelectedItem?.ToString() ?? "ID заказа";

                using (var connection = new NpgsqlConnection(currentConnectionString))
                {
                    connection.Open();

                    var orderColumnsForSelect = MetaInformation.columnsOrders
                        .Where(col => col != "client_id")
                        .Select(col => $"o.{col}")
                        .ToArray();

                    var selectList = string.Join(", ", orderColumnsForSelect) +
                                     ", c.surname || ' ' || c.name AS \"client\"";

                    string query = $@"
                        SELECT {selectList}
                        FROM {MetaInformation.tables[3]} o
                        JOIN {MetaInformation.tables[0]} c ON o.client_id = c.id
                        WHERE ";

                    string whereClause = "";
                    NpgsqlParameter parameter = new NpgsqlParameter();

                    switch (searchType)
                    {
                        case "ID заказа":
                            if (int.TryParse(searchTerm, out int orderId))
                            {
                                whereClause = "o.id = @searchTerm";
                                parameter = new NpgsqlParameter("@searchTerm", orderId);
                            }
                            else
                            {
                                MessageBox.Show("Введите корректный числовой ID заказа", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            break;
                        case "Клиент":
                            whereClause = "(c.surname ILIKE @searchTerm OR c.name ILIKE @searchTerm)";
                            parameter = new NpgsqlParameter("@searchTerm", $"%{searchTerm}%");
                            break;
                        case "ID клиента":
                            if (int.TryParse(searchTerm, out int clientId))
                            {
                                whereClause = "o.client_id = @searchTerm";
                                parameter = new NpgsqlParameter("@searchTerm", clientId);
                            }
                            else
                            {
                                MessageBox.Show("Введите корректный числовой ID клиента", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            break;
                        case "Дата заказа":
                            whereClause = "o.order_date::text ILIKE @searchTerm";
                            parameter = new NpgsqlParameter("@searchTerm", $"%{searchTerm}%");
                            break;
                    }

                    query += whereClause + " ORDER BY o.order_date DESC";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.Add(parameter);

                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable data = new DataTable();
                            adapter.Fill(data);

                            dataGridView1.DataSource = data;
                            FormatDataGridViewColumns();
                            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

                            rch.LogInfo($"Выполнен поиск заказов по {searchType}: '{searchTerm}'. Найдено: {data.Rows.Count} записей");

                            if (data.Rows.Count == 0)
                            {
                                MessageBox.Show("Заказы по заданным критериям не найдены", "Результат поиска",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rch.LogError($"Ошибка поиска заказов: {ex.Message}");
            }
        }
    }
}