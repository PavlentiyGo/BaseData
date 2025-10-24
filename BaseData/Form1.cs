using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form1 : Form
    {
        private Button CreateButton;
        private Button AddButton;
        private Button GetButton;
        private Log rch = new Log();

        public Form1()
        {
            rch = InitializeComponent();
            ApplyStyles();

            // Привязка событий к кнопкам
            CreateButton.Click += CreateButton_Click;
            AddButton.Click += AddButton_Click;
            GetButton.Click += GetButton_Click;
        }

        private void ApplyStyles()
        {
            try
            {
                ApplyFormStyle(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
                rch.LogError($"Ошибка применения стилей: {ex.Message}");
            }
        }

        /// <summary>
        /// Создать схему и таблицы (Form2)
        /// </summary>
        private void CreateButton_Click(object? sender, EventArgs e)
        {
            using (Form2 form2 = new Form2(rch))
            {
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Схема и таблицы успешно пересозданы",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rch.LogInfo("Схема и таблицы успешно пересозданы");
                }
            }
        }

        /// <summary>
        /// Внести данные (Form4)
        /// </summary>
        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rch.LogError("Сначала подключитесь к базе данных");
                return;
            }

            using (Form4 form4 = new Form4(rch))
            {
                form4.ShowDialog();
            }
        }

        /// <summary>
        /// Просмотр данных (Form3) - ИСПРАВЛЕНО
        /// </summary>
        private void GetButton_Click(object? sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("Сначала подключитесь к базе данных", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rch.LogError("Сначала подключитесь к базе данных");
                return;
            }

            Form3 form3 = new Form3(rch);
            form3.ShowDialog();
            form3.Dispose();
        }

        #region Стили (оригинальные)

        // Цветовая схема из оригинального Styles
        public static Color LightColor => Color.FromArgb(255, 248, 240);
        public static Color DarkColor => Color.FromArgb(50, 50, 50);
        public static Color DangerColor => Color.FromArgb(231, 76, 60);

        // Шрифты
        public static Font MainFont => new Font("Segoe UI", 9F, FontStyle.Regular);

        // Методы применения стилей (оригинальные)
        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = LightColor;
            form.Font = MainFont;
        }

        public static void ApplyButtonStyle(Button button)
        {
            // Оригинальный стиль кнопок из Form1.Designer.cs
            button.BackColor = Color.White;
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderSize = 1;
            button.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
        }

        public static void ApplySecondaryButtonStyle(Button button)
        {
            button.BackColor = Color.FromArgb(240, 240, 240);
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderSize = 1;
            button.Font = MainFont;
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyDangerButtonStyle(Button button)
        {
            button.BackColor = DangerColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderSize = 1;
            button.Font = MainFont;
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BackColor = Color.White;
            textBox.ForeColor = DarkColor;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = MainFont;
        }

        public static void ApplyComboBoxStyle(ComboBox comboBox)
        {
            comboBox.BackColor = Color.White;
            comboBox.ForeColor = DarkColor;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = MainFont;
        }

        public static void ApplyDataGridViewStyle(DataGridView dataGridView)
        {
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.FixedSingle;
            dataGridView.Font = MainFont;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = DarkColor;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(MainFont, FontStyle.Bold);
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView.ColumnHeadersHeight = 35;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.DefaultCellStyle.Padding = new Padding(3);
            dataGridView.RowTemplate.Height = 30;
        }

        public static void ApplyTabControlStyle(TabControl tabControl)
        {
            // Оригинальный стиль из Form3
            tabControl.Appearance = TabAppearance.Normal;
            tabControl.ItemSize = new Size(120, 35);
            tabControl.SizeMode = TabSizeMode.Fixed;
        }

        public static void ApplyLabelStyle(Label label, bool bold = false)
        {
            label.ForeColor = DarkColor;
            label.Font = bold ? new Font(MainFont, FontStyle.Bold) : MainFont;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        public static void ApplyDateTimePickerStyle(DateTimePicker dateTimePicker)
        {
            dateTimePicker.BackColor = Color.White;
            dateTimePicker.ForeColor = DarkColor;
            dateTimePicker.Format = DateTimePickerFormat.Short;
            dateTimePicker.Font = MainFont;
        }

        #endregion

        #region Initialize Component (оригинальный)

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Log InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Text = "Управление базой данных магазина";
            this.BackColor = System.Drawing.Color.FromArgb(255, 248, 240);
            this.Padding = new System.Windows.Forms.Padding(40);

            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.Transparent;
            mainPanel.Padding = new Padding(30);

            Label titleLabel = new Label();
            titleLabel.Text = "Управление базой данных магазина";
            titleLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            titleLabel.ForeColor = DarkColor;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 100;
            titleLabel.BackColor = Color.Transparent;

            TableLayoutPanel buttonPanel = new TableLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 400;
            buttonPanel.RowCount = 3; // Изменено с 4 на 3 (убрана кнопка Выход)
            buttonPanel.ColumnCount = 1;
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            buttonPanel.Padding = new Padding(100, 30, 100, 30);
            buttonPanel.BackColor = Color.Transparent;

            RichTextBox infoTextBox = new RichTextBox();
            Log InfoTextBox = new Log();
            InfoTextBox.rtb = infoTextBox;
            InfoTextBox.rtb.Name = "InfoTextBox";
            InfoTextBox.rtb.Dock = DockStyle.Fill;
            InfoTextBox.rtb.Font = new Font("Consolas", 10F);
            InfoTextBox.rtb.BackColor = Color.White;
            InfoTextBox.rtb.ForeColor = Color.FromArgb(50, 50, 50);
            InfoTextBox.rtb.BorderStyle = BorderStyle.FixedSingle;
            InfoTextBox.rtb.Padding = new Padding(10);
            InfoTextBox.rtb.Margin = new Padding(20, 10, 20, 20);
            InfoTextBox.rtb.ReadOnly = true;
            InfoTextBox.rtb.ScrollBars = RichTextBoxScrollBars.Vertical;

            CreateButton = new Button();
            AddButton = new Button();
            GetButton = new Button();

            CreateButton.Dock = DockStyle.Fill;
            CreateButton.Text = "Создать схему и таблицы";
            CreateButton.Margin = new Padding(20, 8, 20, 8);
            CreateButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            CreateButton.Height = 80;

            AddButton.Dock = DockStyle.Fill;
            AddButton.Text = "Внести данные";
            AddButton.Margin = new Padding(20, 8, 20, 8);
            AddButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            AddButton.Height = 80;

            GetButton.Dock = DockStyle.Fill;
            GetButton.Text = "Просмотр данных";
            GetButton.Margin = new Padding(20, 8, 20, 8);
            GetButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            GetButton.Height = 80;

            buttonPanel.Controls.Add(CreateButton, 0, 0);
            buttonPanel.Controls.Add(AddButton, 0, 1);
            buttonPanel.Controls.Add(GetButton, 0, 2);

            mainPanel.Controls.Add(infoTextBox);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(titleLabel);

            this.Controls.Add(mainPanel);

            ApplyButtonStyle(CreateButton);
            ApplyButtonStyle(AddButton);
            ApplyButtonStyle(GetButton);

            return InfoTextBox;
        }

        #endregion
    }
}