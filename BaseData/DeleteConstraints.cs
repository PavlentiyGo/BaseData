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
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaseData
{
    public partial class DeleteConstraints : Form
    {
        private int TabNum;
        public DeleteConstraints(int tabNum)
        {
            TabNum = tabNum;
            InitializeComponent();
            DeleteComboBox.Items.AddRange(MetaInformation.GetConstraintNames(MetaInformation.tables[tabNum]));
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            try
            {
                NpgsqlConnection connect = new NpgsqlConnection(AppSettings.SqlConnection);
                connect.Open();
                string sql;
                if ((DeleteComboBox.Text).Contains("not_null"))
                {
                    string columns = GetCheckClauseByConstraintName(AppSettings.SqlConnection, DeleteComboBox.Text);
                    string[] parts = columns.Split(' ');
                    string column = parts[0];
                    sql = $"ALTER TABLE {MetaInformation.tables[TabNum]} ALTER COLUMN {column} DROP NOT NULL";
                }
                else
                {
                    sql = ($"ALTER TABLE {MetaInformation.tables[TabNum]} DROP CONSTRAINT \"{DeleteComboBox.Text}\"");
                }
                NpgsqlCommand command = new NpgsqlCommand(sql, connect);
                command.ExecuteNonQuery();
                connect.Close();
                MessageBox.Show("Ограничение удалено");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private string GetCheckClauseByConstraintName(
        string connectionString,
        string constraintName,
        string schema = "public")
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = @"
        SELECT ch.check_clause
        FROM information_schema.check_constraints ch
        WHERE ch.constraint_schema = @schema
          AND ch.constraint_name = @constraintName;";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("schema", schema);
            cmd.Parameters.AddWithValue("constraintName", constraintName);

            return cmd.ExecuteScalar() as string;
        }
    }
}
