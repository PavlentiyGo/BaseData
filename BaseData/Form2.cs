using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form2 : Form
    {
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
        private Button? ResetButton;
        Log rch = new Log();

        public Form2(Log log)
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

                if (TitleLabel != null)
                {
                    TitleLabel.Font = new Font(Styles.MainFont, 14F, FontStyle.Bold);
                    TitleLabel.ForeColor = Styles.DarkColor;
                    TitleLabel.TextAlign = ContentAlignment.MiddleCenter;
                }

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
                if (ResetButton != null) Styles.ApplyButtonStyle(ResetButton);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
                rch.LogError($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private string GetTableCreationScript()
        {
            return @"
        CREATE TABLE IF NOT EXISTS clients(
            id SERIAL PRIMARY KEY,
            surname VARCHAR(20) NOT NULL,
            name VARCHAR(15) NOT NULL,
            middlename VARCHAR(20),
            location VARCHAR(255),
            phone VARCHAR(11),
            email VARCHAR(255) UNIQUE,
            constclient BOOLEAN DEFAULT false
        );

        CREATE TABLE IF NOT EXISTS goods(
            id SERIAL PRIMARY KEY,
            name VARCHAR(20) UNIQUE,
            price DECIMAL(10,2) CHECK (price > 0),
            unit VARCHAR(20) DEFAULT 'шт',
            stock_quantity INTEGER DEFAULT 0 CHECK (stock_quantity >= 0),
            currency VARCHAR(3) DEFAULT 'RUB'
            
        );

        CREATE TABLE IF NOT EXISTS orders(
            id SERIAL PRIMARY KEY,
            client_id INTEGER REFERENCES clients(id) ON DELETE RESTRICT,
            order_date DATE NOT NULL DEFAULT CURRENT_DATE,
            delivery_date DATE,
            total_amount DECIMAL(10,2) DEFAULT 0,
            discount DECIMAL(5,2) DEFAULT 0
        );

        CREATE TABLE IF NOT EXISTS order_items(
            id SERIAL PRIMARY KEY,
            order_id INTEGER REFERENCES orders(id) ON DELETE CASCADE,
            good_id INTEGER REFERENCES goods(id) ON DELETE RESTRICT,
            quantity INTEGER CHECK (quantity > 0),
            price DECIMAL(10,2) CHECK (price >= 0),
            currency VARCHAR(3) DEFAULT 'RUB'
        );";
        }
        private void RecreateDatabaseStructure_Click(object? sender, EventArgs e)
        {
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(AppSettings.SqlConnection))
            {
                Encoding utf8Encoding = Encoding.UTF8;
                if (!AppSettings.connect)
                {
                    MessageBox.Show("Сначала подключитесь к Базе данных");
                    rch.LogInfo("Сначала подключитесь к Базе данных");
                    return;
                }
                try
                {
                    sqlConnection.Open();

                    string dropScript = @"
                DROP TABLE IF EXISTS order_items CASCADE;
                DROP TABLE IF EXISTS orders CASCADE;
                DROP TABLE IF EXISTS goods CASCADE;
                DROP TABLE IF EXISTS clients CASCADE;";

                    using (NpgsqlCommand dropCommand = new NpgsqlCommand(dropScript, sqlConnection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }

                    using (NpgsqlCommand createCommand = new NpgsqlCommand(GetTableCreationScript(), sqlConnection))
                    {
                        createCommand.ExecuteNonQuery();
                    }
                    rch.LogInfo($"Бд пересоздана");
                    MessageBox.Show("Бд была пересоздана");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка пересоздания БД: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rch.LogError($"Ошибка пересоздания БД: {ex.Message}");
                }
            }
        }

        private void EntryButton_Click(object? sender, EventArgs e)
        {
            if (AppSettings.SqlConnection != "")
            {
                MessageBox.Show("Вы уже подключены к базе данных");
                return;
            }
            if (string.IsNullOrEmpty(PortText?.Text) || string.IsNullOrEmpty(BdText?.Text) ||
                string.IsNullOrEmpty(IdText?.Text) || string.IsNullOrEmpty(PasswordText?.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rch.LogWarning("Пожалуйста, заполните все поля");
                return;
            }

            AppSettings.SqlConnection = $"Server=localhost;Port={PortText.Text};Database={BdText.Text};User Id={IdText.Text};Password={PasswordText.Text};";

            if (AppSettings.TestConnection())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                AppSettings.connect = true;

            }
            else
            {
                MessageBox.Show("Не удалось подключиться к БД", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rch.LogError("Не удалось подключиться к БД");
            }
        }

        private void Auto_Click(object? sender, EventArgs e)
        {
            if (PortText != null) PortText.Text = "5432";
            if (BdText != null) BdText.Text = "Internet-shop(Project)";
            if (IdText != null) IdText.Text = "postgres";
            if (PasswordText != null) PasswordText.Text = "WE<3ANGELINA";
        }
       

        private void InitializeComponent()
        {
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
            this.ResetButton = new Button();

            SuspendLayout();

            this.TitleLabel.Text = "Подключение к базе данных";
            this.TitleLabel.Dock = DockStyle.Top;
            this.TitleLabel.Height = 60;
            this.TitleLabel.TextAlign = ContentAlignment.MiddleCenter;

            this.PortText.Location = new Point(300, 80);
            this.PortText.Size = new Size(150, 23);

            this.BdText.Location = new Point(300, 120);
            this.BdText.Size = new Size(150, 23);

            this.IdText.Location = new Point(300, 160);
            this.IdText.Size = new Size(150, 23);

            this.PasswordText.Location = new Point(300, 200);
            this.PasswordText.PasswordChar = '*';
            this.PasswordText.Size = new Size(150, 23);

            this.PortLabel.Location = new Point(100, 80);
            this.PortLabel.Size = new Size(80, 23);
            this.PortLabel.Text = "Порт";
            this.PortLabel.TextAlign = ContentAlignment.MiddleRight;

            this.BdLabel.Location = new Point(100, 120);
            this.BdLabel.Size = new Size(80, 23);
            this.BdLabel.Text = "База данных";
            this.BdLabel.TextAlign = ContentAlignment.MiddleRight;

            this.IdLabel.Location = new Point(100, 160);
            this.IdLabel.Size = new Size(80, 23);
            this.IdLabel.Text = "Пользователь";
            this.IdLabel.TextAlign = ContentAlignment.MiddleRight;

            this.PasswordLabel.Location = new Point(100, 200);
            this.PasswordLabel.Size = new Size(80, 23);
            this.PasswordLabel.Text = "Пароль";
            this.PasswordLabel.TextAlign = ContentAlignment.MiddleRight;

            this.EntryButton.Location = new Point(300, 250);
            this.EntryButton.Size = new Size(150, 50);
            this.EntryButton.Text = "Подключиться";
            this.EntryButton.UseVisualStyleBackColor = true;
            this.EntryButton.Click += EntryButton_Click;

            this.AutoButton.Location = new Point(100, 250);
            this.AutoButton.Size = new Size(150, 50);
            this.AutoButton.Text = "Автозаполнение";
            this.AutoButton.UseVisualStyleBackColor = true;
            this.AutoButton.Click += Auto_Click;

            this.ResetButton.Location = new Point(190, 330);
            this.ResetButton.Size = new Size(150, 50);
            this.ResetButton.Text = "Пересоздать";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += RecreateDatabaseStructure_Click;

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
            this.Controls.Add(ResetButton!);
            this.Name = "Form2";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Подключение к базе данных";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}