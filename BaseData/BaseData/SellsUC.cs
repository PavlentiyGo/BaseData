using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public class SellsUC : UserControl
    {
        private string? currentConnectionString;
        private Button btnRefresh = null!;
        private DataGridView dataGridView1 = null!;

        public SellsUC()
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
            this.Name = "SellsUC";
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

        public void LoadData(string? connectionString)
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

                using (var connection = new NpgsqlConnection(currentConnectionString))
                {
                    connection.Open();
                    var query = @"
                        SELECT o.id as ""Номер заказа"", 
                               c.surname || ' ' || c.name as ""Клиент"",
                               o.order_date as ""Дата заказа"",
                               o.delivery_date as ""Дата доставки"",
                               o.total_amount as ""Сумма"",
                               o.discount as ""Скидка %""
                        FROM orders o
                        JOIN clients c ON o.client_id = c.id
                        ORDER BY o.order_date DESC";

                    var adapter = new NpgsqlDataAdapter(query, connection);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    this.dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}