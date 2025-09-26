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

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Text = "Управление базой данных магазина";
            this.BackColor = System.Drawing.Color.FromArgb(255, 248, 240);
            this.Padding = new System.Windows.Forms.Padding(40);

            // Создание главной панели
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.Transparent;
            mainPanel.Padding = new Padding(30);

            // Заголовок
            Label titleLabel = new Label();
            titleLabel.Text = "Управление базой данных магазина";
            titleLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            titleLabel.ForeColor = Styles.DarkColor;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 120;
            titleLabel.BackColor = Color.Transparent;

            // Панель для кнопок
            TableLayoutPanel buttonPanel = new TableLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.RowCount = 4;
            buttonPanel.ColumnCount = 1;
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            buttonPanel.Padding = new Padding(100, 60, 100, 60);
            buttonPanel.BackColor = Color.Transparent;

            // Создание кнопок
            CreateButton = new Button();
            AddButton = new Button();
            GetButton = new Button();
            Button exitButton = new Button();

            // CreateButton
            CreateButton.Dock = DockStyle.Fill;
            CreateButton.Text = "Создать схему и таблицы";
            CreateButton.Margin = new Padding(20);
            CreateButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            // AddButton
            AddButton.Dock = DockStyle.Fill;
            AddButton.Text = "Внести данные";
            AddButton.Margin = new Padding(20);
            AddButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            // GetButton
            GetButton.Dock = DockStyle.Fill;
            GetButton.Text = "Просмотр данных";
            GetButton.Margin = new Padding(20);
            GetButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            // ExitButton
            exitButton.Dock = DockStyle.Fill;
            exitButton.Text = "Выход";
            exitButton.Margin = new Padding(20);
            exitButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            exitButton.Click += (s, e) => Application.Exit();

            // Добавляем кнопки на панель
            buttonPanel.Controls.Add(CreateButton, 0, 0);
            buttonPanel.Controls.Add(AddButton, 0, 1);
            buttonPanel.Controls.Add(GetButton, 0, 2);
            buttonPanel.Controls.Add(exitButton, 0, 3);

            // Компоновка
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(titleLabel);

            this.Controls.Add(mainPanel);

            // Применяем стили
            Styles.ApplyButtonStyle(CreateButton);
            Styles.ApplyButtonStyle(AddButton);
            Styles.ApplyButtonStyle(GetButton);
            Styles.ApplySecondaryButtonStyle(exitButton);
        }
        #endregion

        private Button CreateButton;
        private Button AddButton;
        private Button GetButton;
    }
}