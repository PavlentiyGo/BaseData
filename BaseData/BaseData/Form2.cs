
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
        //string connectionString = "Server=localhost;Port=5432;Database=Internet-shop(Project); User Id = postgres; Password=WE<3ANGELINA";
        public Form2()
        {
            InitializeComponent();
            string enteredText = PortText.Text;
            Console.WriteLine(enteredText);

        }

        private void SqlConnectionReader(string connectionString)
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM clients";

            NpgsqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows)
            {
                DataTable data = new DataTable();
                data.Load(dataReader);

                dataGridView1.DataSource = data;

            }
            command.Dispose();
            sqlConnection.Close();
        }









        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void PortText_TextChanged(object sender, EventArgs e)
        {

        }

        private void BdText_TextChanged(object sender, EventArgs e)
        {

        }

        private void IdText_TextChanged(object sender, EventArgs e)
        {

        }

        private void EntryButton_Click(object sender, EventArgs e)
        {
            string Connect = "Server=localhost;Port=";
            Connect += PortText.Text;
            Connect += ";Database=";
            Connect += BdText.Text;
            Connect += "; User Id = ";
            Connect += IdText.Text;
            Connect += "; Password=";
            Connect += PasswordText.Text;
            MessageBox.Show(Connect);
            SqlConnectionReader(Connect);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            PortText.Text = "5432";
            BdText.Text = "Internet-shop(Project)";
            IdText.Text = "postgres";
            PasswordText.Text = "WE<3ANGELINA";
        }
    }
}
