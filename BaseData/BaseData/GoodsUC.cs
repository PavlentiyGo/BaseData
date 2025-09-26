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
        private Button btnRefresh = null!;
        private DataGridView dataGridView1 = null!;

        public GoodsUC()
        {
            InitializeComponent();
            CreateRefreshButton();
            ApplyDataGridViewStyle();
        }

        private void InitializeComponent()
        {
            this.btnRefresh = new Button();
            this.dataGridView1 = new DataGridView();

            SuspendLayout();

            // btnRefresh
            this.btnRefresh.Location = new Point(10, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(80, 30);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;

            // dataGridView1
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = Color.White;
            this.dataGridView1.BorderStyle = BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(10, 50);
            this.dataGridView1.Margin = new Padding(10);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new Size(760, 340);
            this.dataGridView1.TabIndex = 1;

            // UserControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnRefresh);
            this.Name = "GoodsUC";
            this.Size = new Size(780, 400);

            ResumeLayout(false);
        }

        private void ApplyDataGridViewStyle()
        {
            try
            {
                if (this.dataGridView1 != null)
                {
                    var method = typeof(Styles).GetMethod("ApplyDataGridViewStyle");
                    if (method != null)
                    {
                        method.Invoke(null, new object[] { this.dataGridView1 });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стиля: {ex.Message}");
            }
        }

        private void CreateRefreshButton()
        {
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Size = new Size(80, 30);
            this.btnRefresh.Location = new Point(10, 10);
            this.btnRefresh.BackColor = Color.SteelBlue;
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Click += (s, e) => RefreshData();
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
                    ShowErrorMessage("Строка подключения не задана");
                    return;
                }

                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(currentConnectionString))
                {
                    sqlConnection.Open();

                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        string query = "SELECT * FROM goods ORDER BY id";
                        using (NpgsqlCommand command = new NpgsqlCommand(query, sqlConnection))
                        {
                            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                            {
                                DataTable data = new DataTable();
                                adapter.Fill(data);

                                this.dataGridView1.DataSource = data;
                                SafeConfigureDataGridViewColumns();

                                if (data.Rows.Count == 0)
                                {
                                    ShowInfoMessage("Таблица товаров пуста");
                                }
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                ShowErrorMessage($"Ошибка PostgreSQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void SafeConfigureDataGridViewColumns()
        {
            try
            {
                if (this.dataGridView1?.Columns == null || this.dataGridView1.Columns.Count == 0)
                    return;

                foreach (DataGridViewColumn column in this.dataGridView1.Columns)
                {
                    if (column?.Name == null) continue;

                    string columnName = column.Name.ToLower();

                    if (columnName == "id") column.HeaderText = "ID";
                    else if (columnName == "name") column.HeaderText = "Название товара";
                    else if (columnName == "price") column.HeaderText = "Цена";
                    else if (columnName == "measure") column.HeaderText = "Валюта";
                }

                this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка настройки колонок: {ex.Message}");
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}