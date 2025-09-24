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

namespace BaseData
{
    public partial class Clients : UserControl
    {
        public Clients()
        {
            InitializeComponent();
        }

        private void Clients_Load(object sender, EventArgs e)
        {

        }
        public void Show(string connectionString)
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
                sqlConnection = new NpgsqlConnection(connectionString);
                sqlConnection.Open();
                if (sqlConnection.State == ConnectionState.Open)
                {
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
        }
    }
}
