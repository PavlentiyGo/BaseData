using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace BaseData
{
    public class GoodsUC : UserControl
    {
        static private string? currentConnectionString;
        private Button? btnDelete;
        private Button? btnEdit;
        private Button? sqlBuilderButton;
        private static DataGridView? dataGridView1;
        private Button? changeTableButton;
        private TextBox? searchTextBox;
        private ComboBox? searchTypeComboBox;
        private Button? btnResetSearch;
        private Button? joinBuilderButton;
        Log rch = new Log();

        public GoodsUC(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.btnDelete = new Button();
            this.btnEdit = new Button();
            this.sqlBuilderButton = new Button();
            this.changeTableButton = new Button();
            this.searchTextBox = new TextBox();
            this.searchTypeComboBox = new ComboBox();
            this.btnResetSearch = new Button();
            this.joinBuilderButton = new Button();
            dataGridView1 = new DataGridView();

            SuspendLayout();

            // Панель поиска
            Panel searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Height = 50;
            searchPanel.Padding = new Padding(10, 5, 10, 5);
            searchPanel.BackColor = Styles.LightColor;

            // Элементы поиска
            searchTextBox!.Location = new Point(10, 12);
            searchTextBox.Size = new Size(200, 25);
            searchTextBox.PlaceholderText = "Введите текст для поиска...";
            searchTextBox.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    PerformSearch();
                }
            };

            searchTypeComboBox!.Location = new Point(220, 12);
            searchTypeComboBox.Size = new Size(120, 25);
            searchTypeComboBox.Items.AddRange(new string[] { "Название", "ID", "Цена от", "В наличии" });
            searchTypeComboBox.SelectedIndex = 0;
            searchTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            var btnSearch = new Button();
            btnSearch.Text = "Найти";
            btnSearch.Location = new Point(350, 12);
            btnSearch.Size = new Size(80, 25);
            btnSearch.Click += (s, e) => PerformSearch();

            btnResetSearch!.Text = "Сброс";
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

            // Панель кнопок действий
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 80;
            buttonPanel.Padding = new Padding(10, 12, 10, 12);
            buttonPanel.BackColor = Color.Transparent;

            Size buttonSize = new Size(140, 60);
            int buttonSpacing = 15;

            this.btnEdit!.Text = "Редактировать";
            this.btnEdit.Size = buttonSize;
            this.btnEdit.Location = new Point(buttonSpacing, 10);
            this.btnEdit.Click += EditSelectedGood;

            this.changeTableButton!.Text = "Изменить\nтаблицу";
            this.changeTableButton.Size = buttonSize;
            this.changeTableButton.Location = new Point(btnEdit.Right + buttonSpacing, 10);
            this.changeTableButton.Click += ChangeTableClick;

            this.sqlBuilderButton!.Text = "Конструктор\nSQL";
            this.sqlBuilderButton.Size = buttonSize;
            this.sqlBuilderButton.Location = new Point(changeTableButton.Right + buttonSpacing, 10);
            this.sqlBuilderButton.Click += OpenSqlBuilder;
            this.sqlBuilderButton.Font = new Font(sqlBuilderButton.Font.FontFamily, 9F, FontStyle.Regular);

            this.btnDelete!.Text = "Удалить";
            this.btnDelete.Size = buttonSize;
            this.btnDelete.Location = new Point(sqlBuilderButton.Right + buttonSpacing, 10);
            this.btnDelete.Click += DeleteSelectedGood;

            // joinBuilderButton
            this.joinBuilderButton!.Text = "Мастер\nсоединений";
            this.joinBuilderButton.Size = buttonSize;
            this.joinBuilderButton.Location = new Point(btnDelete.Right + buttonSpacing, 10);
            this.joinBuilderButton.Click += OpenJoinBuilder;
            this.joinBuilderButton.Font = new Font(joinBuilderButton.Font.FontFamily, 9F, FontStyle.Regular);

            dataGridView1!.AllowUserToAddRows = false;
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
            dataGridView1.TabIndex = 4;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 35;
            dataGridView1.RowHeadersVisible = false;

            buttonPanel.Controls.Add(this.btnEdit);
            buttonPanel.Controls.Add(this.changeTableButton);
            buttonPanel.Controls.Add(this.sqlBuilderButton);
            buttonPanel.Controls.Add(this.btnDelete);
            buttonPanel.Controls.Add(this.joinBuilderButton);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(dataGridView1);
            this.Controls.Add(buttonPanel);
            this.Controls.Add(searchPanel);
            this.Name = MetaInformation.tables[1];
            this.Size = new Size(800, 500);

            ResumeLayout(false);
        }

        private void OpenJoinBuilder(object? sender, EventArgs e)
        {
            try
            {
                if (!AppSettings.IsConnectionStringSet)
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rch.LogWarning("Попытка открыть мастер соединений без подключения к БД");
                    return;
                }

                using (JoinBuilderForm joinBuilderForm = new JoinBuilderForm(rch))
                {
                    rch.LogInfo("Открытие мастера соединений из формы товаров");
                    joinBuilderForm.ShowDialog();
                    rch.LogInfo("Мастер соединений закрыт");
                }
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка при открытии мастера соединений: {ex.Message}");
                MessageBox.Show($"Ошибка при открытии мастера соединений: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (dataGridView1 != null)
                {
                    Styles.ApplyDataGridViewStyle(dataGridView1);

                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Styles.AccentColor;
                    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Styles.AccentColor;
                    dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

                    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 240, 235);
                    dataGridView1.DefaultCellStyle.SelectionForeColor = Styles.DarkColor;
                    dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 240, 235);

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

            // Применяем стили к кнопке поиска
            var btnSearch = searchPanel?.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Найти");
            if (btnSearch != null)
            {
                Styles.ApplySecondaryButtonStyle(btnSearch);
                btnSearch.Font = new Font(btnSearch.Font.FontFamily, 9F, FontStyle.Regular);
            }

            if (btnEdit != null)
            {
                Styles.ApplySecondaryButtonStyle(btnEdit);
                btnEdit.Font = new Font(btnEdit.Font.FontFamily, 9F, FontStyle.Regular);
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
            if (joinBuilderButton != null)
            {
                Styles.ApplySecondaryButtonStyle(joinBuilderButton);
                joinBuilderButton.Font = new Font(joinBuilderButton.Font.FontFamily, 9F, FontStyle.Regular);
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

        private Panel? searchPanel => this.Controls.OfType<Panel>().FirstOrDefault();

        private void DataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OpenEditForCurrentRow();
        }

        private void EditSelectedGood(object? sender, EventArgs e)
        {
            OpenEditForCurrentRow();
        }

        private void OpenEditForCurrentRow()
        {
            if (dataGridView1?.CurrentRow == null)
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(dataGridView1.CurrentRow.Cells["id"].Value?.ToString(), out int goodId))
            {
                MessageBox.Show("Не удалось определить id товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var form = new AddProductForm(goodId, rch))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshData();
                }
            }
        }

        private void DeleteSelectedGood(object? sender, EventArgs e)
        {
            if (dataGridView1?.CurrentRow == null)
            {
                MessageBox.Show("Выберите товар для удаления", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный товар?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int goodId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                    using (var connection = new NpgsqlConnection(currentConnectionString))
                    {
                        connection.Open();
                        var command = new NpgsqlCommand($"DELETE FROM {MetaInformation.tables[1]} WHERE id = @id", connection);
                        command.Parameters.AddWithValue("id", goodId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Товар успешно удален", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshData();
                        }
                    }
                }
                catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
                {
                    MessageBox.Show("Невозможно удалить товар: существуют связанные заказы", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления товара: {ex.Message}", "Ошибка",
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
                    rch.LogInfo("Открытие конструктора SQL запросов из формы товаров");
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
            if (dataGridView1 == null) return;

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
                        string columnList = string.Join(", ", MetaInformation.columnsGoods);
                        string query = $"SELECT {columnList} FROM {MetaInformation.tables[1]} ORDER BY id";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, sqlConnection))
                        {
                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable data = new DataTable();
                                adapter.Fill(data);

                                dataGridView1.DataSource = data;

                                if (dataGridView1.Columns.Contains("price"))
                                {
                                    dataGridView1.Columns["price"]!.DefaultCellStyle.Format = "N2";
                                    dataGridView1.Columns["price"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                }
                                if (dataGridView1.Columns.Contains("stock_quantity"))
                                {
                                    dataGridView1.Columns["stock_quantity"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }

                                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
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

        private void ChangeTableClick(object? sender, EventArgs e)
        {
            ClientTableChange table = new ClientTableChange(rch, 1);
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
                string searchType = searchTypeComboBox?.SelectedItem?.ToString() ?? "Название";

                using (var connection = new NpgsqlConnection(currentConnectionString))
                {
                    connection.Open();

                    string columnList = string.Join(", ", MetaInformation.columnsGoods);
                    string query = $"SELECT {columnList} FROM {MetaInformation.tables[1]} WHERE ";
                    string whereClause = "";
                    NpgsqlParameter parameter = new NpgsqlParameter();

                    switch (searchType)
                    {
                        case "Название":
                            whereClause = "name ILIKE @searchTerm";
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
                        case "Цена от":
                            if (decimal.TryParse(searchTerm, out decimal minPrice))
                            {
                                whereClause = "price >= @searchTerm";
                                parameter = new NpgsqlParameter("@searchTerm", minPrice);
                            }
                            else
                            {
                                MessageBox.Show("Введите корректную цену", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            break;
                        case "В наличии":
                            if (int.TryParse(searchTerm, out int minStock))
                            {
                                whereClause = "stock_quantity >= @searchTerm";
                                parameter = new NpgsqlParameter("@searchTerm", minStock);
                            }
                            else
                            {
                                MessageBox.Show("Введите корректное количество", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            break;
                    }

                    query += whereClause + " ORDER BY id";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.Add(parameter);

                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable data = new DataTable();
                            adapter.Fill(data);

                            dataGridView1.DataSource = data;

                            if (dataGridView1.Columns.Contains("price"))
                            {
                                dataGridView1.Columns["price"]!.DefaultCellStyle.Format = "N2";
                                dataGridView1.Columns["price"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            }
                            if (dataGridView1.Columns.Contains("stock_quantity"))
                            {
                                dataGridView1.Columns["stock_quantity"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }

                            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

                            rch.LogInfo($"Выполнен поиск товаров по {searchType}: '{searchTerm}'. Найдено: {data.Rows.Count} записей");

                            if (data.Rows.Count == 0)
                            {
                                MessageBox.Show("Товары по заданным критериям не найдены", "Результат поиска",
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
                rch.LogError($"Ошибка поиска товаров: {ex.Message}");
            }
        }
    }
}