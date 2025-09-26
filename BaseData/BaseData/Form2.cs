using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
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
                DROP TABLE IF EXISTS order_items;
                DROP TABLE IF EXISTS orders;
                DROP TABLE IF EXISTS goods;
                DROP TABLE IF EXISTS clients;";

                    using (NpgsqlCommand dropCommand = new NpgsqlCommand(dropScript, sqlConnection))
                    {
                        dropCommand.ExecuteNonQuery();
                    }

                    // Создаем таблицы заново
                    using (NpgsqlCommand createCommand = new NpgsqlCommand(GetTableCreationScript(), sqlConnection))
                    {
                        createCommand.ExecuteNonQuery();
                        AddTestData(sqlConnection);
                    }

                    MessageBox.Show("База данных успешно пересоздана!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка пересоздания БД: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddTestData(NpgsqlConnection connection)
        {
            try
            {
                // Тестовые клиенты
                var clientCmd = new NpgsqlCommand(@"
                    INSERT INTO clients (surname, name, middlename, location, phone, email, constclient) 
                    VALUES 
                    ('Иванов', 'Иван', 'Иванович', 'Москва', '+79161234567', 'ivanov@mail.ru', true),
                    ('Петров', 'Петр', 'Петрович', 'Санкт-Петербург', '+79167654321', 'petrov@mail.ru', false)
                    ON CONFLICT (email) DO NOTHING", connection);
                clientCmd.ExecuteNonQuery();

                // Тестовые товары с остатками
                var goodsCmd = new NpgsqlCommand(@"
                    INSERT INTO goods (name, price, unit, stock_quantity) 
                    VALUES 
                    ('Ноутбук', 50000.00, 'шт', 10),
                    ('Мышь', 1500.50, 'шт', 25),
                    ('Монитор', 25000.00, 'шт', 5)
                    ON CONFLICT (id) DO NOTHING", connection);
                goodsCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка добавления тестовых данных: {ex.Message}");
            }
        }

        private void EntryButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PortText.Text) || string.IsNullOrEmpty(BdText.Text) ||
                string.IsNullOrEmpty(IdText.Text) || string.IsNullOrEmpty(PasswordText.Text))
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

        private void CreateDatabaseStructure()
        {
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(AppSettings.SqlConnection))
            {
                try
                {
                    sqlConnection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = sqlConnection;
                        command.CommandText = GetTableCreationScript();
                        command.ExecuteNonQuery();
                        AddTestData(sqlConnection);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания структуры БД: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) => this.Close();

        private void Auto_Click(object sender, EventArgs e)
        {
            PortText.Text = "5432";
            BdText.Text = "newdata";
            IdText.Text = "postgres";
            PasswordText.Text = "Al7xsemenov@";
        }
    }
}