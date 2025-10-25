using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class GoodsUC : UserControl
    {
        static private string currentConnectionString;
        private Button? btnSearch;
        private Button? btnDelete;
        private Button? btnEdit;
        private Button? btnJoinMaster;
        private static DataGridView dataGridView1;
        private Button? changeTableButton;
        Log rch = new Log();
        public GoodsUC(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.btnSearch = new Button();
            this.btnDelete = new Button();
            this.btnEdit = new Button();
            this.changeTableButton = new Button();
            this.btnJoinMaster = new Button();
            dataGridView1 = new DataGridView();


            SuspendLayout();

            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 80;
            buttonPanel.Padding = new Padding(10, 12, 10, 12);
            buttonPanel.BackColor = Color.Transparent;

            Size buttonSize = new Size(140, 60);
            int buttonSpacing = 15;

            this.btnSearch.Text = "Поиск";
            this.btnSearch.Size = buttonSize;
            this.btnSearch.Location = new Point(buttonSpacing, 10);
            this.btnSearch.Click += BtnSearch_Click;

            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Size = buttonSize;
            this.btnEdit.Location = new Point(btnSearch.Right + buttonSpacing, 10);
            this.btnEdit.Click += EditSelectedGood;

            this.btnDelete.Text = "Удалить";
            this.btnDelete.Size = buttonSize;
            this.btnDelete.Location = new Point(btnEdit.Right + buttonSpacing, 10);
            this.btnDelete.Click += DeleteSelectedGood;

            this.changeTableButton.Text = "Изменить\nтаблицу";
            this.changeTableButton.Size = buttonSize;
            this.changeTableButton.Location = new Point(btnDelete.Right + buttonSpacing, 10);
            this.changeTableButton.Click += ChangeTableClick;

            btnJoinMaster.Text = "Мастер\nсоединений";
            btnJoinMaster.Size = buttonSize;
            btnJoinMaster.Location = new Point(changeTableButton.Right + buttonSpacing, 10);
            btnJoinMaster.Click += BtnJoinMaster_Click;

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
            dataGridView1.TabIndex = 4;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView1.ColumnHeadersHeight = 35;
            dataGridView1.RowHeadersVisible = false;

            buttonPanel.Controls.Add(this.btnSearch);
            buttonPanel.Controls.Add(this.btnEdit);
            buttonPanel.Controls.Add(this.btnDelete);
            buttonPanel.Controls.Add(this.changeTableButton);
            buttonPanel.Controls.Add(this.btnJoinMaster);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(dataGridView1);
            this.Controls.Add(buttonPanel);
            this.Name = MetaInformation.tables[1];
            this.Size = new Size(800, 500);

            ResumeLayout(false);
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (dataGridView1 != null)
                {
                    Styles.ApplyDataGridViewStyle(dataGridView1);

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
            if (btnJoinMaster != null)
            {
                //стиль для кнопки
                Styles.ApplySecondaryButtonStyle(btnJoinMaster);
                btnJoinMaster.Font = new Font(btnJoinMaster.Font.FontFamily, 9F, FontStyle.Regular);
            }
        }

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

        // Обработчик для кнопки "Мастер соединений"
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
        private void BtnSearch_Click(object? sender, EventArgs e)
        {

        }
    }

}
