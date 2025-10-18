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
        private DataGridView? dataGridView1;
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
            this.btnRefresh = new Button();
            this.btnDelete = new Button();
            this.btnEdit = new Button();
            this.changeTableButton = new Button();
            this.dataGridView1 = new DataGridView();

            SuspendLayout();

            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 80;
            buttonPanel.Padding = new Padding(10, 12, 10, 12);
            buttonPanel.BackColor = Color.Transparent;

            Size buttonSize = new Size(140, 60);
            int buttonSpacing = 15;

            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Size = buttonSize;
            this.btnRefresh.Location = new Point(buttonSpacing, 10);
            this.btnRefresh.Click += (s, e) => RefreshData();

            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Size = buttonSize;
            this.btnEdit.Location = new Point(btnRefresh.Right + buttonSpacing, 10);
            this.btnEdit.Click += EditSelectedGood;

            this.btnDelete.Text = "Удалить";
            this.btnDelete.Size = buttonSize;
            this.btnDelete.Location = new Point(btnEdit.Right + buttonSpacing, 10);
            this.btnDelete.Click += DeleteSelectedGood;

            this.changeTableButton.Text = "Изменить\nтаблицу";
            this.changeTableButton.Size = buttonSize;
            this.changeTableButton.Location = new Point(btnDelete.Right + buttonSpacing, 10);
            this.changeTableButton.Click += ChangeTableClick;

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
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.dataGridView1.ColumnHeadersHeight = 35;
            this.dataGridView1.RowHeadersVisible = false;

            buttonPanel.Controls.Add(this.btnRefresh);
            buttonPanel.Controls.Add(this.btnEdit);
            buttonPanel.Controls.Add(this.btnDelete);
            buttonPanel.Controls.Add(this.changeTableButton);

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

            if (btnRefresh != null)
            {
                Styles.ApplySecondaryButtonStyle(btnRefresh);
                btnRefresh.Font = new Font(btnRefresh.Font.FontFamily, 9F, FontStyle.Regular);
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
    }

}
