using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public partial class Form2 : Form
    {
        private Button? CloseButton;
        private TextBox? PortText;
        private TextBox? BdText;
        private TextBox? IdText;
        private TextBox? PasswordText;
        private Label? PortLabel;
        private Label? BdLabel;
        private Label? IdLabel;
        private Label? PasswordLabel;
        private Button? EntryButton;
        private Button? AutoButton;
        private Label? TitleLabel;

        public Form2()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (TitleLabel != null)
                {
                    TitleLabel.Font = new Font(Styles.MainFont, 14F, FontStyle.Bold);
                    TitleLabel.ForeColor = Styles.DarkColor;
                    TitleLabel.TextAlign = ContentAlignment.MiddleCenter;
                }

                // Применяем стили к элементам управления
                if (PortText != null) Styles.ApplyTextBoxStyle(PortText);
                if (BdText != null) Styles.ApplyTextBoxStyle(BdText);
                if (IdText != null) Styles.ApplyTextBoxStyle(IdText);
                if (PasswordText != null) Styles.ApplyTextBoxStyle(PasswordText);

                if (PortLabel != null) Styles.ApplyLabelStyle(PortLabel, true);
                if (BdLabel != null) Styles.ApplyLabelStyle(BdLabel, true);
                if (IdLabel != null) Styles.ApplyLabelStyle(IdLabel, true);
                if (PasswordLabel != null) Styles.ApplyLabelStyle(PasswordLabel, true);

                if (EntryButton != null) Styles.ApplyButtonStyle(EntryButton);
                if (AutoButton != null) Styles.ApplySecondaryButtonStyle(AutoButton);
                if (CloseButton != null) Styles.ApplySecondaryButtonStyle(CloseButton);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private string GetTableCreationScript()
        {
            return @"
        CREATE TABLE IF NOT EXISTS clients(
            id SERIAL PRIMARY KEY,
            surname VARCHAR(255) NOT NULL,
            name VARCHAR(255) NOT NULL,
            middlename VARCHAR(255),
            location VARCHAR(255),
            phone VARCHAR(20),
            email VARCHAR(255) UNIQUE,
            constclient BOOLEAN DEFAULT false
        );

        CREATE TABLE IF NOT EXISTS goods(
            id SERIAL PRIMARY KEY,
            name VARCHAR(255) NOT NULL,
            price DECIMAL(10,2) CHECK (price > 0),
            unit VARCHAR(20) DEFAULT 'шт',
            stock_quantity INTEGER DEFAULT 0 CHECK (stock_quantity >= 0)
        );

        CREATE TABLE IF NOT EXISTS orders(
            id SERIAL PRIMARY KEY,
            client_id INTEGER REFERENCES clients(id) ON DELETE CASCADE,
            order_date DATE NOT NULL DEFAULT CURRENT_DATE,
            delivery_date DATE,
            total_amount DECIMAL(10,2) DEFAULT 0,
            discount DECIMAL(5,2) DEFAULT 0
        );

        CREATE TABLE IF NOT EXISTS order_items(
            id SERIAL PRIMARY KEY,
            order_id INTEGER REFERENCES orders(id) ON DELETE CASCADE,
            good_id INTEGER REFERENCES goods(id) ON DELETE CASCADE,
            quantity INTEGER CHECK (quantity > 0),
            price DECIMAL(10,2) CHECK (price >= 0)
        );";
        }

        private void RecreateDatabaseStructure()
        {
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(AppSettings.SqlConnection))
            {
                try
                {
                    sqlConnection.Open();

                    // Удаляем существующие таблицы (если они есть)
                    string dropScript = @"
                DROP TABLE IF EXISTS order_items CASCADE;
                DROP TABLE IF EXISTS orders CASCADE;
                DROP TABLE IF EXISTS goods CASCADE;
                DROP TABLE IF EXISTS clients CASCADE;";

                    using (NpgsqlCommand dropCommand = new NpgsqlCommand(dropScript, sqlConnection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }

                    // Создаем таблицы заново
                    using (NpgsqlCommand createCommand = new NpgsqlCommand(GetTableCreationScript(), sqlConnection))
                    {
                        createCommand.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка пересоздания БД: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EntryButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PortText?.Text) || string.IsNullOrEmpty(BdText?.Text) ||
                string.IsNullOrEmpty(IdText?.Text) || string.IsNullOrEmpty(PasswordText?.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppSettings.SqlConnection = $"Server=localhost;Port={PortText.Text};Database={BdText.Text};User Id={IdText.Text};Password={PasswordText.Text};";

            if (AppSettings.TestConnection())
            {
                // Пересоздаем структуру базы данных
                RecreateDatabaseStructure();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к БД", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object? sender, EventArgs e) => this.Close();

        private void Auto_Click(object? sender, EventArgs e)
        {
            if (PortText != null) PortText.Text = "5432";
            if (BdText != null) BdText.Text = "newdata";
            if (IdText != null) IdText.Text = "postgres";
            if (PasswordText != null) PasswordText.Text = "Al7xsemenov@";
        }

        private void InitializeComponent()
        {
            this.CloseButton = new Button();
            this.PortText = new TextBox();
            this.BdText = new TextBox();
            this.IdText = new TextBox();
            this.PasswordText = new TextBox();
            this.PortLabel = new Label();
            this.BdLabel = new Label();
            this.IdLabel = new Label();
            this.PasswordLabel = new Label();
            this.EntryButton = new Button();
            this.AutoButton = new Button();
            this.TitleLabel = new Label();

            SuspendLayout();

            // TitleLabel
            this.TitleLabel.Text = "Подключение к базе данных";
            this.TitleLabel.Dock = DockStyle.Top;
            this.TitleLabel.Height = 60;
            this.TitleLabel.TextAlign = ContentAlignment.MiddleCenter;

            // CloseButton
            this.CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.CloseButton.Location = new Point(350, 350);
            this.CloseButton.Size = new Size(100, 35);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "Закрыть";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += button1_Click;

            // PortText
            this.PortText.Location = new Point(300, 80);
            this.PortText.Size = new Size(150, 23);

            // BdText
            this.BdText.Location = new Point(300, 120);
            this.BdText.Size = new Size(150, 23);

            // IdText
            this.IdText.Location = new Point(300, 160);
            this.IdText.Size = new Size(150, 23);

            // PasswordText
            this.PasswordText.Location = new Point(300, 200);
            this.PasswordText.PasswordChar = '*';
            this.PasswordText.Size = new Size(150, 23);

            // PortLabel
            this.PortLabel.Location = new Point(100, 80);
            this.PortLabel.Size = new Size(80, 23);
            this.PortLabel.Text = "Порт";
            this.PortLabel.TextAlign = ContentAlignment.MiddleRight;

            // BdLabel
            this.BdLabel.Location = new Point(100, 120);
            this.BdLabel.Size = new Size(80, 23);
            this.BdLabel.Text = "База данных";
            this.BdLabel.TextAlign = ContentAlignment.MiddleRight;

            // IdLabel
            this.IdLabel.Location = new Point(100, 160);
            this.IdLabel.Size = new Size(80, 23);
            this.IdLabel.Text = "Пользователь";
            this.IdLabel.TextAlign = ContentAlignment.MiddleRight;

            // PasswordLabel
            this.PasswordLabel.Location = new Point(100, 200);
            this.PasswordLabel.Size = new Size(80, 23);
            this.PasswordLabel.Text = "Пароль";
            this.PasswordLabel.TextAlign = ContentAlignment.MiddleRight;

            // EntryButton
            this.EntryButton.Location = new Point(300, 250);
            this.EntryButton.Size = new Size(150, 30);
            this.EntryButton.Text = "Подключиться";
            this.EntryButton.UseVisualStyleBackColor = true;
            this.EntryButton.Click += EntryButton_Click;

            // AutoButton
            this.AutoButton.Location = new Point(100, 250);
            this.AutoButton.Size = new Size(120, 35);
            this.AutoButton.Text = "Автозаполнение";
            this.AutoButton.UseVisualStyleBackColor = true;
            this.AutoButton.Click += Auto_Click;

            // Form2
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 400);
            this.Controls.Add(TitleLabel!);
            this.Controls.Add(AutoButton!);
            this.Controls.Add(EntryButton!);
            this.Controls.Add(PasswordLabel!);
            this.Controls.Add(IdLabel!);
            this.Controls.Add(BdLabel!);
            this.Controls.Add(PortLabel!);
            this.Controls.Add(PasswordText!);
            this.Controls.Add(IdText!);
            this.Controls.Add(BdText!);
            this.Controls.Add(PortText!);
            this.Controls.Add(CloseButton!);
            this.Name = "Form2";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Подключение к базе данных";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}