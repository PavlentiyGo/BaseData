using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class Clients : UserControl
    {
        static private string currentConnectionString;
        private Button? searchButton;
        private Button? deleteButton;
        private Button? editButton;
        private Button? addButton;
        private Button? btnJoinMaster;
        private Button? changeTableButton;
        private static DataGridView dataGrid;
        Log rch = new Log();

        public Clients(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.searchButton = new Button();
            this.deleteButton = new Button();
            this.editButton = new Button();
            this.changeTableButton = new Button();
            this.btnJoinMaster = new Button();
            dataGrid = new DataGridView();

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

            // refreshButton
            this.searchButton.Text = "Поиск";
            this.searchButton.Size = buttonSize;
            this.searchButton.Location = new Point(buttonSpacing, 10);
            this.searchButton.Click += BtnSearch_Click;

            // editButton
            this.editButton.Text = "Редактировать";
            this.editButton.Size = buttonSize;
            this.editButton.Location = new Point(searchButton.Right + buttonSpacing, 10);
            this.editButton.Click += EditSelectedClient;

            // deleteButton
            this.deleteButton.Text = "Удалить";
            this.deleteButton.Size = buttonSize;
            this.deleteButton.Location = new Point(editButton.Right + buttonSpacing, 10);
            this.deleteButton.Click += DeleteSelectedClient;

            this.changeTableButton.Text = "Изменить\nтаблицу";
            this.changeTableButton.Size = buttonSize;
            this.changeTableButton.Location = new Point(deleteButton.Right + buttonSpacing, 10);
            this.changeTableButton.Click += ChangeTableClick;

            // joinmaster
            this.btnJoinMaster.Text = "Мастер\nсоединений";
            this.btnJoinMaster.Size = buttonSize;
            this.btnJoinMaster.Location = new Point(changeTableButton.Right + buttonSpacing, 10);
            this.btnJoinMaster.Click += BtnJoinMaster_Click;

            // dataGrid
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.BackgroundColor = Color.White;
            dataGrid.BorderStyle = BorderStyle.FixedSingle;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Location = new Point(0, 60);
            dataGrid.Margin = new Padding(10);
            dataGrid.Name = "dataGrid";
            dataGrid.ReadOnly = true;
            dataGrid.TabIndex = 4;
            dataGrid.CellDoubleClick += DataGrid_CellDoubleClick;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGrid.ColumnHeadersHeight = 35;
            dataGrid.RowHeadersVisible = false;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.searchButton);
            buttonPanel.Controls.Add(this.editButton);
            buttonPanel.Controls.Add(this.deleteButton);
            buttonPanel.Controls.Add(this.changeTableButton);
            buttonPanel.Controls.Add(this.btnJoinMaster);

            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(dataGrid);
            this.Controls.Add(buttonPanel);
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

                    // Дополнительные стили для улучшения внешнего вида
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

            // Применяем стили к кнопкам с увеличенной высотой
            if (searchButton != null)
            {
                Styles.ApplySecondaryButtonStyle(searchButton);
                searchButton.Font = new Font(searchButton.Font.FontFamily, 9F, FontStyle.Regular);
            }
            if (editButton != null)
            {
                Styles.ApplySecondaryButtonStyle(editButton);
                editButton.Font = new Font(editButton.Font.FontFamily, 9F, FontStyle.Regular);
            }
            if (addButton != null)
            {
                Styles.ApplyButtonStyle(addButton);
                addButton.Font = new Font(addButton.Font.FontFamily, 9F, FontStyle.Regular);
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
            if (btnJoinMaster != null)
            {
                Styles.ApplySecondaryButtonStyle(btnJoinMaster);
                btnJoinMaster.Font = new Font(btnJoinMaster.Font.FontFamily, 9F, FontStyle.Regular);
            }
        }

        // обработчик join
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
                        string query = $"SELECT {columnList} FROM {MetaInformation.tables[0]} ORDER BY id";

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

                    // Улучшаем внешний вид колонок
                    column.DefaultCellStyle.Padding = new Padding(5, 3, 5, 3);
                }

                // Автоматическое изменение размера колонок после загрузки данных
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
        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm();
            searchForm.ShowDialog();
        }
    }
}
