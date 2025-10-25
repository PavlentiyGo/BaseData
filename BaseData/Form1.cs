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
                Styles.ApplyFormStyle(this);
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
            this.BackColor = Styles.LightColor; // Используем Styles
            this.Padding = new System.Windows.Forms.Padding(40);

            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.Transparent;
            mainPanel.Padding = new Padding(30);

            Label titleLabel = new Label();
            titleLabel.Text = "Управление базой данных магазина";
            titleLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            titleLabel.ForeColor = Styles.DarkColor; // Используем Styles
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 100;
            titleLabel.BackColor = Color.Transparent;

            TableLayoutPanel buttonPanel = new TableLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 400;
            buttonPanel.RowCount = 3;
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
            InfoTextBox.rtb.ForeColor = Styles.DarkColor; // Используем Styles
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

            // Применяем стили через Styles
            Styles.ApplyPrimaryButtonStyle(CreateButton);
            Styles.ApplyPrimaryButtonStyle(AddButton);
            Styles.ApplyPrimaryButtonStyle(GetButton);

            return InfoTextBox;
        }

        #endregion
    }
}