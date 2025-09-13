using Npgsql;
using System.Data;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=localhost;Port=5432;Database=Internet-shop(Project); User Id = postgres; Password=WE<3ANGELINA";
        public Form1()
        {
            InitializeComponent();
            SqlConnectionReader();
        }
        
        private void SqlConnectionReader()
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
    }
}
