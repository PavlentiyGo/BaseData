
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaseData
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            
        }
        public bool CurrencyTypeExists(string connectionString)
        {
            string query = @"
        SELECT EXISTS (
            SELECT 1 
            FROM pg_type 
            WHERE typname = 'currency'
        );
    ";

            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand(query, connection))
            {
                connection.Open();
                bool exists = (bool)command.ExecuteScalar();
                return exists;
            }
        }
        private void SqlConnectionReader(string connectionString)
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            try
            {
                sqlConnection = new NpgsqlConnection(connectionString);
                sqlConnection.Open();
                if (sqlConnection.State == ConnectionState.Open)
                {
                    MessageBox.Show("Подключение успешно установлено, таблицы установлены!");
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = sqlConnection;
                    command.CommandType = CommandType.Text;
                    if (CurrencyTypeExists(connectionString))
                    {
                        command.CommandText = "CREATE TABLE IF NOT EXISTS clients(\r\n\tid SERIAL PRIMARY KEY,\r\n\tsurname VARCHAR(255),\r\n\tname VARCHAR(255),\r\n\tmiddleName VARCHAR(255),\r\n\tlocation VARCHAR(255),\r\n\tnumbers TEXT[],\r\n\temail VARCHAR(255) UNIQUE,\r\n\tconstClient boolean\r\n);\r\nCREATE TABLE IF NOT EXISTS goods(\r\n\tid SERIAL PRIMARY KEY,\r\n\tname VARCHAR(255),\r\n\tprice INTEGER CHECK (price>0),\r\n\tmeasure currency\t\r\n);\r\nCREATE TABLE IF NOT EXISTS sells(\r\n\tgoodId INTEGER NOT NULL,\r\n\tsellId  SERIAL PRIMARY KEY,\r\n\tclientId INTEGER NOT NULL,\r\n\tsellDate DATE,\r\n\tdeliveryDate DATE,\r\n\tcount INTEGER,\r\n\tFOREIGN KEY (goodId)\r\n\t\tREFERENCES goods (id)\r\n\t\tON DELETE CASCADE\r\n\t\tON UPDATE CASCADE,\r\n\tFOREIGN KEY (clientId)\r\n\t\tREFERENCES clients (id)\r\n\t\tON DELETE CASCADE\r\n\t\tON UPDATE CASCADE\r\n);";
                    }
                    else
                    {
                        command.CommandText = "CREATE TYPE currency AS ENUM ('rub', 'eu', 'usd');\r\nCREATE TABLE IF NOT EXISTS clients(\r\n\tid SERIAL PRIMARY KEY,\r\n\tsurname VARCHAR(255),\r\n\tname VARCHAR(255),\r\n\tmiddleName VARCHAR(255),\r\n\tlocation VARCHAR(255),\r\n\tnumbers TEXT[],\r\n\temail VARCHAR(255) UNIQUE,\r\n\tconstClient boolean\r\n);\r\nCREATE TABLE IF NOT EXISTS goods(\r\n\tid SERIAL PRIMARY KEY,\r\n\tname VARCHAR(255),\r\n\tprice INTEGER CHECK (price>0),\r\n\tmeasure currency\t\r\n);\r\nCREATE TABLE IF NOT EXISTS sells(\r\n\tgoodId INTEGER NOT NULL,\r\n\tsellId  SERIAL PRIMARY KEY,\r\n\tclientId INTEGER NOT NULL,\r\n\tsellDate DATE,\r\n\tdeliveryDate DATE,\r\n\tcount INTEGER,\r\n\tFOREIGN KEY (goodId)\r\n\t\tREFERENCES goods (id)\r\n\t\tON DELETE CASCADE\r\n\t\tON UPDATE CASCADE,\r\n\tFOREIGN KEY (clientId)\r\n\t\tREFERENCES clients (id)\r\n\t\tON DELETE CASCADE\r\n\t\tON UPDATE CASCADE\r\n);";
                    }
                    NpgsqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(dataReader);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка PostgreSQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Общая ошибка: {ex.Message}");
            }
            finally
            {
                sqlConnection?.Close();
                sqlConnection?.Dispose();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EntryButton_Click(object sender, EventArgs e)
        {
            AppSettings.sqlConnection = "Server=localhost;Port=";
            AppSettings.sqlConnection += PortText.Text;
            AppSettings.sqlConnection += ";Database=";
            AppSettings.sqlConnection += BdText.Text;
            AppSettings.sqlConnection += "; User Id = ";
            AppSettings.sqlConnection += IdText.Text;
            AppSettings.sqlConnection += "; Password=";
            AppSettings.sqlConnection += PasswordText.Text;
            SqlConnectionReader(AppSettings.sqlConnection);
        }
        private void Auto_Click(object sender, EventArgs e)
        {
            PortText.Text = "5432";
            BdText.Text = "Internet-shop(Project)";
            IdText.Text = "postgres";
            PasswordText.Text = "WE<3ANGELINA";
        }
    }
    public static class AppSettings
    {
        public static string sqlConnection { get; set; }
    }
}
