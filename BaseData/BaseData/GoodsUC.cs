using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class GoodsUC : UserControl
    {
        private string? currentConnectionString;
        private Button? btnRefresh;
        private Button? btnDelete;
        private Button? btnEdit;
        private Button? btnAdd;
        private DataGridView? dataGridView1;

        public GoodsUC()
        {
            InitializeComponent();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.btnRefresh = new Button();
            this.btnDelete = new Button();
            this.btnEdit = new Button();
            this.btnAdd = new Button();
            this.dataGridView1 = new DataGridView();

            SuspendLayout();

            // Панель для кнопок
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 50;
            buttonPanel.Padding = new Padding(10);
            buttonPanel.BackColor = Color.Transparent;

            // btnAdd
            this.btnAdd.Text = "Добавить товар";
            this.btnAdd.Size = new Size(140, 35);
            this.btnAdd.Location = new Point(10, 7);
            this.btnAdd.Click += (s, e) =>
            {
                if (AppSettings.IsConnectionStringSet)
                {
                    var form = new AddProductForm();
                    form.FormClosed += (sender, e) => RefreshData();
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // btnRefresh
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Size = new Size(100, 35);
            this.btnRefresh.Location = new Point(160, 7);
            this.btnRefresh.Click += (s, e) => RefreshData();

            // btnEdit
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Size = new Size(120, 35);
            this.btnEdit.Location = new Point(270, 7);
            this.btnEdit.Click += EditSelectedGood;

            // btnDelete
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Size = new Size(100, 35);
            this.btnDelete.Location = new Point(400, 7);
            this.btnDelete.Click += DeleteSelectedGood;

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
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(this.btnAdd);
            buttonPanel.Controls.Add(this.btnRefresh);
            buttonPanel.Controls.Add(this.btnEdit);
            buttonPanel.Controls.Add(this.btnDelete);

            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(buttonPanel);
            this.Name = "GoodsUC";
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
            if (btnEdit != null) Styles.ApplySecondaryButtonStyle(btnEdit);
            if (btnAdd != null) Styles.ApplyButtonStyle(btnAdd);
            if (btnDelete != null) Styles.ApplyDangerButtonStyle(btnDelete);
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

            using (var form = new AddProductForm(goodId))
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
                        var command = new NpgsqlCommand("DELETE FROM goods WHERE id = @id", connection);
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

        public void Show(string? connectionString)
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

                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(currentConnectionString))
                {
                    sqlConnection.Open();

                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        string query = "SELECT id, name, price, unit, stock_quantity FROM goods ORDER BY id";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, sqlConnection))
                        {
                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable data = new DataTable();
                                adapter.Fill(data);

                                this.dataGridView1.DataSource = data;
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

        
        
    }
}