
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
    public static class AppSettings
    {
        public static string sqlConnection { get; set; }
    }
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            
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
                    MessageBox.Show("Подключение успешно установлено!");
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = sqlConnection;
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
}
