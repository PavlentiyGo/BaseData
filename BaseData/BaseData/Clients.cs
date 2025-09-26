using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class Clients : UserControl
    {
        private string? currentConnectionString;
        private Button? refreshButton;
        private Button? deleteButton;
        private Button? editButton;
        private Button? addButton;
        private DataGridView? dataGrid;

        public Clients()
        {
            InitializeComponent();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.refreshButton = new Button();
            this.deleteButton = new Button();
            this.editButton = new Button();
            this.addButton = new Button();
            this.dataGrid = new DataGridView();

            SuspendLayout();

            // Панель для кнопок
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 50;
            buttonPanel.Padding = new Padding(10);
            buttonPanel.BackColor = Color.Transparent;

            // addButton
            this.addButton.Text = "Добавить клиента";
            this.addButton.Size = new Size(140, 35);
            this.addButton.Location = new Point(10, 7);
            this.addButton.Click += (s, e) =>
            {
                if (AppSettings.IsConnectionStringSet)
                {
                    var form = new AddClientForm();
                    form.FormClosed += (sender, e) => RefreshData();
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // refreshButton
            this.refreshButton.Text = "Обновить";
            this.refreshButton.Size = new Size(100, 35);
            this.refreshButton.Location = new Point(160, 7);
            this.refreshButton.Click += (s, e) => RefreshData();

            // editButton
            this.editButton.Text = "Редактировать";
            this.editButton.Size = new Size(120, 35);
            this.editButton.Location = new Point(270, 7);
            this.editButton.Click += EditSelectedClient;

            // deleteButton
            this.deleteButton.Text = "Удалить";
            this.deleteButton.Size = new Size(100, 35);
            this.deleteButton.Location = new Point(400, 7);
            this.deleteButton.Click += DeleteSelectedClient;

            // dataGrid
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.Dock = DockStyle.Fill;
            this.dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.BackgroundColor = Color.White;
            this.dataGrid.BorderStyle = BorderStyle.FixedSingle;
            this.dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new Point(0, 50);
            this.dataGrid.Margin = new Padding(10);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.TabIndex = 4;
            this.dataGrid.CellDoubleClick += DataGrid_CellDoubleClick;
            this.dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.addButton);
            buttonPanel.Controls.Add(this.refreshButton);
            buttonPanel.Controls.Add(this.editButton);
            buttonPanel.Controls.Add(this.deleteButton);

            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(buttonPanel);
            this.Name = "Clients";
            this.Size = new Size(800, 500);

            ResumeLayout(false);
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (this.dataGrid != null)
                {
                    Styles.ApplyDataGridViewStyle(this.dataGrid);
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
            if (refreshButton != null) Styles.ApplySecondaryButtonStyle(refreshButton);
            if (editButton != null) Styles.ApplySecondaryButtonStyle(editButton);
            if (addButton != null) Styles.ApplyButtonStyle(addButton);
            if (deleteButton != null) Styles.ApplyDangerButtonStyle(deleteButton);
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

            using (var form = new AddClientForm(clientId))
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
                        var command = new NpgsqlCommand("DELETE FROM clients WHERE id = @id", connection);
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

        private void RefreshData()
        {
            if (this.dataGrid == null) return;

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
                        string query = "SELECT id, surname, name, middlename, location, phone, email, constclient FROM clients ORDER BY id";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, sqlConnection))
                        {
                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable data = new DataTable();
                                adapter.Fill(data);

                                this.dataGrid.DataSource = data;
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

        private void SafeConfigureDataGridViewColumns()
        {
            try
            {
                if (this.dataGrid?.Columns == null || this.dataGrid.Columns.Count == 0)
                    return;

                foreach (DataGridViewColumn column in this.dataGrid.Columns)
                {
                    if (column?.Name == null) continue;

                    string columnName = column.Name.ToLower();

                    if (columnName == "id")
                    {
                        column.HeaderText = "ID";
                        column.Width = 50;
                    }
                    else if (columnName == "surname") column.HeaderText = "Фамилия";
                    else if (columnName == "name") column.HeaderText = "Имя";
                    else if (columnName == "middlename") column.HeaderText = "Отчество";
                    else if (columnName == "location") column.HeaderText = "Адрес";
                    else if (columnName == "phone") column.HeaderText = "Телефон";
                    else if (columnName == "email") column.HeaderText = "Email";
                    else if (columnName == "constclient")
                    {
                        column.HeaderText = "Постоянный";
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch
            {
                // Игнорируем ошибки настройки колонок
            }
        }
    }
}