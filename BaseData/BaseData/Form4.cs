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
            this.Size = new System.Drawing.Size(500, 500); // Увеличил высоту формы
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Главный контейнер
            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 5;
            mainPanel.ColumnCount = 1;

            // Увеличил высоту строк для кнопок
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); // Заголовок
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));  // Кнопка 1
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));  // Кнопка 2
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));  // Кнопка 3
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));  // Кнопка 4

            mainPanel.Padding = new Padding(20);
            mainPanel.BackColor = Color.Transparent;

            // Заголовок
            Label titleLabel = new Label()
            {
                Text = "Добавление данных",
                Font = new Font(Styles.MainFont, 16F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };

            // Кнопка добавления клиента - УВЕЛИЧЕНА
            Button btnAddClient = new Button()
            {
                Text = "Добавить клиента",
                Dock = DockStyle.Fill,
                Font = new Font(Styles.MainFont, 12F, FontStyle.Bold), // Увеличил размер шрифта
                Margin = new Padding(10, 15, 10, 15), // Увеличил вертикальные отступы
                Height = 60 // Явно задал высоту
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

            // Кнопка добавления товара - УВЕЛИЧЕНА
            Button btnAddProduct = new Button()
            {
                Text = "Добавить товар",
                Dock = DockStyle.Fill,
                Font = new Font(Styles.MainFont, 12F, FontStyle.Bold), // Увеличил размер шрифта
                Margin = new Padding(10, 15, 10, 15), // Увеличил вертикальные отступы
                Height = 60 // Явно задал высоту
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

            // Кнопка добавления продажи - УВЕЛИЧЕНА
            Button btnAddSale = new Button()
            {
                Text = "Оформить продажу",
                Dock = DockStyle.Fill,
                Font = new Font(Styles.MainFont, 12F, FontStyle.Bold), // Увеличил размер шрифта
                Margin = new Padding(10, 15, 10, 15), // Увеличил вертикальные отступы
                Height = 60 // Явно задал высоту
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

            // Кнопка закрытия - УВЕЛИЧЕНА
            Button btnClose = new Button()
            {
                Text = "Закрыть",
                Dock = DockStyle.Fill,
                Font = new Font(Styles.MainFont, 11F, FontStyle.Regular), // Увеличил размер шрифта
                Margin = new Padding(10, 15, 10, 15), // Увеличил вертикальные отступы
                Height = 50 // Явно задал высоту
            };
            btnClose.Click += (s, e) => this.Close();

            // Добавляем элементы на панель
            mainPanel.Controls.Add(titleLabel, 0, 0);
            mainPanel.Controls.Add(btnAddClient, 0, 1);
            mainPanel.Controls.Add(btnAddProduct, 0, 2);
            mainPanel.Controls.Add(btnAddSale, 0, 3);
            mainPanel.Controls.Add(btnClose, 0, 4);

            // Добавляем панель на форму
            this.Controls.Add(mainPanel);

            this.ResumeLayout(false);

            // Применяем стили к кнопкам после инициализации
            Styles.ApplyButtonStyle(btnAddClient);
            Styles.ApplyButtonStyle(btnAddProduct);
            Styles.ApplyButtonStyle(btnAddSale);
            Styles.ApplySecondaryButtonStyle(btnClose);
        }
    }
}