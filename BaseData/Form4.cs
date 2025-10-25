using System;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public partial class Form4 : Form
    {
        Log rch = new Log();

        public Form4(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
                rch.LogError($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Настройки формы
            this.Text = "Добавление данных";
            this.Size = new System.Drawing.Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Главный контейнер
            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 4;
            mainPanel.ColumnCount = 1;

            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            mainPanel.Padding = new Padding(20);
            mainPanel.BackColor = Color.Transparent;

            // Заголовок
            Label titleLabel = new Label()
            {
                Text = "Добавление данных",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Styles.DarkColor, // Используем Styles вместо Form1
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };

            // Кнопка добавления клиента
            Button btnAddClient = new Button()
            {
                Text = "Добавить клиента",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Margin = new Padding(10, 15, 10, 15),
                Height = 60
            };
            btnAddClient.Click += (s, e) =>
            {
                if (AppSettings.IsConnectionStringSet)
                {
                    AddClientForm form = new AddClientForm(rch);
                    form.ShowDialog();
                    form.Dispose();
                }
                else
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rch.LogWarning("Сначала подключитесь к базе данных");
                }
            };

            // Кнопка добавления товара
            Button btnAddProduct = new Button()
            {
                Text = "Добавить товар",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Margin = new Padding(10, 15, 10, 15),
                Height = 60
            };
            btnAddProduct.Click += (s, e) =>
            {
                if (AppSettings.IsConnectionStringSet)
                {
                    AddProductForm form = new AddProductForm(rch);
                    form.ShowDialog();
                    form.Dispose();
                }
                else
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rch.LogWarning("Сначала подключитесь к базе данных");
                }
            };

            // Кнопка добавления продажи
            Button btnAddSale = new Button()
            {
                Text = "Оформить продажу",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Margin = new Padding(10, 15, 10, 15),
                Height = 60
            };
            btnAddSale.Click += (s, e) =>
            {
                if (AppSettings.IsConnectionStringSet)
                {
                    AddSaleForm form = new AddSaleForm(rch);
                    form.ShowDialog();
                    form.Dispose();
                }
                else
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rch.LogWarning("Сначала подключитесь к базе данных");
                }
            };

            // Добавляем элементы на панель
            mainPanel.Controls.Add(titleLabel, 0, 0);
            mainPanel.Controls.Add(btnAddClient, 0, 1);
            mainPanel.Controls.Add(btnAddProduct, 0, 2);
            mainPanel.Controls.Add(btnAddSale, 0, 3);

            // Добавляем панель на форму
            this.Controls.Add(mainPanel);

            this.ResumeLayout(false);

            // Применяем стили к кнопкам через Styles вместо Form1
            Styles.ApplyPrimaryButtonStyle(btnAddClient);
            Styles.ApplyPrimaryButtonStyle(btnAddProduct);
            Styles.ApplyPrimaryButtonStyle(btnAddSale);
        }
    }
}