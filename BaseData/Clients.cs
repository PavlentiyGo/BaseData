using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class Clients : UserControl
    {
        static private string? currentConnectionString;
        private Button? deleteButton;
        private Button? editButton;
        private Button? changeTableButton;
        private Button? sqlBuilderButton;
        private static DataGridView? dataGrid;
        private TextBox? searchTextBox;
        private ComboBox? searchTypeComboBox;
        private Button? btnResetSearch;
        Log rch = new Log();

        public Clients(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.deleteButton = new Button();
            this.editButton = new Button();
            this.changeTableButton = new Button();
            this.sqlBuilderButton = new Button();
            this.searchTextBox = new TextBox();
            this.searchTypeComboBox = new ComboBox();
            this.btnResetSearch = new Button();
            dataGrid = new DataGridView();

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
            searchTypeComboBox.Items.AddRange(new string[] { "ФИО", "Email", "Телефон", "ID", "Постоянный" });
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

            // editButton
            this.editButton.Text = "Редактировать";
            this.editButton.Size = buttonSize;
            this.editButton.Location = new Point(buttonSpacing, 10);
            this.editButton.Click += EditSelectedClient;

            this.changeTableButton.Text = "Изменить\nтаблицу";
            this.changeTableButton.Size = buttonSize;
            this.changeTableButton.Location = new Point(editButton.Right + buttonSpacing, 10);
            this.changeTableButton.Click += ChangeTableClick;

            this.sqlBuilderButton.Text = "Конструктор\nSQL";
            this.sqlBuilderButton.Size = buttonSize;
            this.sqlBuilderButton.Location = new Point(changeTableButton.Right + buttonSpacing, 10);
            this.sqlBuilderButton.Click += OpenSqlBuilder;
            this.sqlBuilderButton.Font = new Font(sqlBuilderButton.Font.FontFamily, 9F, FontStyle.Regular);

            // deleteButton
            this.deleteButton.Text = "Удалить";
            this.deleteButton.Size = buttonSize;
            this.deleteButton.Location = new Point(sqlBuilderButton.Right + buttonSpacing, 10);
            this.deleteButton.Click += DeleteSelectedClient;

            // dataGrid
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.BackgroundColor = Color.White;
            dataGrid.BorderStyle = BorderStyle.FixedSingle;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Location = new Point(0, 130);
            dataGrid.Margin = new Padding(10);
            dataGrid.Name = "dataGrid";
            dataGrid.ReadOnly = true;
            dataGrid.TabIndex = 4;
            dataGrid.CellDoubleClick += DataGrid_CellDoubleClick;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGrid.ColumnHeadersHeight = 35;
            dataGrid.RowHeadersVisible = false;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.editButton);
            buttonPanel.Controls.Add(this.changeTableButton);
            buttonPanel.Controls.Add(this.sqlBuilderButton);
            buttonPanel.Controls.Add(this.deleteButton);

            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(dataGrid);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(searchPanel);
            this.Name = MetaInformation.tables[0];
            this.Size = new Size(800, 500);

            ResumeLayout(false);
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (dataGrid != null)
                {
                    Styles.ApplyDataGridViewStyle(dataGrid);

                    // Устанавливаем серый цвет заголовков
                    dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
                    dataGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

                    // Устанавливаем серый цвет выделения для ячеек и строк
                    dataGrid.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    dataGrid.DefaultCellStyle.SelectionForeColor = Color.Black;
                    dataGrid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;

                    dataGrid.DefaultCellStyle.Font = new Font("Segoe UI", 8.5F);
                    dataGrid.DefaultCellStyle.Padding = new Padding(3);
                    dataGrid.RowTemplate.Height = 30;
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

            if (editButton != null)
            {
                Styles.ApplySecondaryButtonStyle(editButton);
                editButton.Font = new Font(editButton.Font.FontFamily, 9F, FontStyle.Regular);
            }
            if (deleteButton != null)
            {
                Styles.ApplyDangerButtonStyle(deleteButton);
                deleteButton.Font = new Font(deleteButton.Font.FontFamily, 9F, FontStyle.Regular);
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

        private void DataGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OpenEditForCurrentRow();
        }

        private void EditSelectedClient(object? sender, EventArgs e)
        {
            OpenEditForCurrentRow();
        }

        private void OpenEditForCurrentRow()
        {
            if (dataGrid?.CurrentRow == null)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(dataGrid.CurrentRow.Cells["id"].Value?.ToString(), out int clientId))
            {
                MessageBox.Show("Не удалось определить id клиента", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var form = new AddClientForm(clientId, rch))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshData();
                }
            }
        }

        private void DeleteSelectedClient(object? sender, EventArgs e)
        {
            if (dataGrid?.CurrentRow == null)
            {
                MessageBox.Show("Выберите клиента для удаления", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранного клиента?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int clientId = Convert.ToInt32(dataGrid.CurrentRow.Cells["id"].Value);

                    using (var connection = new NpgsqlConnection(currentConnectionString))
                    {
                        connection.Open();
                        var command = new NpgsqlCommand($"DELETE FROM {MetaInformation.tables[0]} WHERE id = @id", connection);
                        command.Parameters.AddWithValue("id", clientId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Клиент успешно удален", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshData();
                        }
                    }
                }
                catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
                {
                    MessageBox.Show("Невозможно удалить клиента: существуют связанные заказы", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления клиента: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                    rch.LogInfo("Открытие конструктора SQL запросов из формы клиентов");
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

        public void Show(string? connectionString)
        {
            currentConnectionString = connectionString;
            RefreshData();
        }

        public static void RefreshData()
        {
            if (dataGrid == null) return;

            try
            {
                if (string.IsNullOrEmpty(currentConnectionString))
                {
                    return;
                }

                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(currentConnectionString))
                {
                    sqlConnection.Open();

                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        string columnList = string.Join(", ", MetaInformation.columnsClients);
                        string query = $"SELECT {columnList} FROM {MetaInformation.tables[0]} ORDER BY surname, name";

                        using (NpgsqlCommand command = new NpgsqlCommand(query, sqlConnection))
                        {
                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable data = new DataTable();
                                adapter.Fill(data);

                                dataGrid.DataSource = data;
                                SafeConfigureDataGridViewColumns();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static private void SafeConfigureDataGridViewColumns()
        {
            try
            {
                if (dataGrid?.Columns == null || dataGrid.Columns.Count == 0)
                    return;

                foreach (DataGridViewColumn column in dataGrid.Columns)
                {
                    if (column?.Name == null) continue;

                    string columnName = column.Name.ToLower();
                    column.HeaderText = columnName;

                    column.DefaultCellStyle.Padding = new Padding(5, 3, 5, 3);
                }

                dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch
            {
                // Игнорируем ошибки настройки колонок
            }
        }

        private void ChangeTableClick(object? sender, EventArgs e)
        {
            ClientTableChange table = new ClientTableChange(rch, 0);
            table.ShowDialog();
        }

        private void PerformSearch()
        {
            if (string.IsNullOrEmpty(searchTextBox?.Text) || dataGrid == null)
            {
                RefreshData();
                return;
            }

            try
            {
                string searchTerm = searchTextBox.Text.Trim();
                string searchType = searchTypeComboBox?.SelectedItem?.ToString() ?? "ФИО";

                using (var connection = new NpgsqlConnection(currentConnectionString))
                {
                    connection.Open();

                    string columnList = string.Join(", ", MetaInformation.columnsClients);
                    string query = $"SELECT {columnList} FROM {MetaInformation.tables[0]} WHERE ";
                    string whereClause = "";
                    NpgsqlParameter parameter = new NpgsqlParameter();

                    switch (searchType)
                    {
                        case "ФИО":
                            whereClause = "(surname ILIKE @searchTerm OR name ILIKE @searchTerm OR middlename ILIKE @searchTerm)";
                            parameter = new NpgsqlParameter("@searchTerm", $"%{searchTerm}%");
                            break;
                        case "Email":
                            whereClause = "email ILIKE @searchTerm";
                            parameter = new NpgsqlParameter("@searchTerm", $"%{searchTerm}%");
                            break;
                        case "Телефон":
                            whereClause = "phone ILIKE @searchTerm";
                            parameter = new NpgsqlParameter("@searchTerm", $"%{searchTerm}%");
                            break;
                        case "ID":
                            if (int.TryParse(searchTerm, out int id))
                            {
                                whereClause = "id = @searchTerm";
                                parameter = new NpgsqlParameter("@searchTerm", id);
                            }
                            else
                            {
                                MessageBox.Show("Введите корректный числовой ID", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            break;
                        case "Постоянный":
                            if (searchTerm.ToLower() == "да" || searchTerm.ToLower() == "true" || searchTerm == "1")
                            {
                                whereClause = "constclient = true";
                                parameter = new NpgsqlParameter("@searchTerm", true);
                            }
                            else if (searchTerm.ToLower() == "нет" || searchTerm.ToLower() == "false" || searchTerm == "0")
                            {
                                whereClause = "constclient = false";
                                parameter = new NpgsqlParameter("@searchTerm", false);
                            }
                            else
                            {
                                MessageBox.Show("Введите 'да' или 'нет' для поиска постоянных клиентов", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            break;
                    }

                    query += whereClause + " ORDER BY surname, name";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.Add(parameter);

                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable data = new DataTable();
                            adapter.Fill(data);

                            dataGrid.DataSource = data;
                            SafeConfigureDataGridViewColumns();

                            rch.LogInfo($"Выполнен поиск клиентов по {searchType}: '{searchTerm}'. Найдено: {data.Rows.Count} записей");

                            if (data.Rows.Count == 0)
                            {
                                MessageBox.Show("Клиенты по заданным критериям не найдены", "Результат поиска",
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
                rch.LogError($"Ошибка поиска клиентов: {ex.Message}");
            }
        }
    }
}