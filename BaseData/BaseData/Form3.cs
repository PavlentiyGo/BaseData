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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            SqlGetDate();
        }
        private void SqlGetDate()
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(AppSettings.sqlConnection);
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
    }
}
