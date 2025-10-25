using System.Reflection.Metadata.Ecma335;

namespace BaseData
{
    partial class Form1
    {
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
            titleLabel.ForeColor = Styles.DarkColor;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 100;
            titleLabel.BackColor = Color.Transparent;

            TableLayoutPanel buttonPanel = new TableLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 400;
            buttonPanel.RowCount = 4;
            buttonPanel.ColumnCount = 1;
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
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
            Button exitButton = new Button();

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

            exitButton.Dock = DockStyle.Fill;
            exitButton.Text = "Выход";
            exitButton.Margin = new Padding(20, 8, 20, 8);
            exitButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            exitButton.Height = 80;
            exitButton.Click += (s, e) => Application.Exit();

            buttonPanel.Controls.Add(CreateButton, 0, 0);
            buttonPanel.Controls.Add(AddButton, 0, 1);
            buttonPanel.Controls.Add(GetButton, 0, 2);
            buttonPanel.Controls.Add(exitButton, 0, 3);

            mainPanel.Controls.Add(infoTextBox);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(titleLabel);

            this.Controls.Add(mainPanel);

            Styles.ApplyButtonStyle(CreateButton);
            Styles.ApplyButtonStyle(AddButton);
            Styles.ApplyButtonStyle(GetButton);
            Styles.ApplyButtonStyle(exitButton);
            return InfoTextBox;
        }

        private Button CreateButton;
        private Button AddButton;
        private Button GetButton;
    }
}