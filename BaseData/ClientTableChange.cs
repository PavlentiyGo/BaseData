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
    public partial class ClientTableChange : Form
    {
        Log log;
        public ClientTableChange(Log rch)
        {
            log = rch;
            InitializeComponent();
            Styles.ApplyFormStyle(this);
        }

        private void SetConstraint_Click(object sender, EventArgs e)
        {

        }

        private void RenameTable_Click(object sender, EventArgs e)
        {

        }

        private void ChangeTableData_Click(object sender, EventArgs e)
        {

        }

        private void DeleteColumn_Click(object sender, EventArgs e)
        {
            string deleteColumn = DeleteBox.Text;
            if (string.IsNullOrEmpty(deleteColumn))
            {
                MessageBox.Show("Выберите удаляемый столбец");
                log.LogWarning("Выберите удаляемый столбец");
                return;
            }
            Request($"ALTER TABLE {MetaInformation.tables[0]} DROP COLUMN {deleteColumn}");
            log.LogInfo($"Столбец {deleteColumn} был удалён из таблицы {MetaInformation.tables[0]}");
            MetaInformation.RefreshData();
        }

        private void AddColumn_Click(object sender, EventArgs e)
        {
            string column = AddColumnText.Text;
            string type = AddColumnTypeBox.Text;
            if (column == "")
            {
                MessageBox.Show("Введите название столбца");
                log.LogWarning("Введите название столбца");
                return;
            }
            if (column == "" || !ContainsOnlyEnglishLetters(column))
            {
                MessageBox.Show("Столбец должен быть назван английскими буквами");
                log.LogWarning("Столбец должен быть назван английскими буквами");
                return;
            }
            log.LogInfo($"Добавлен столбец {column} с типом {type} в таблицу {MetaInformation.tables[0]}");
            Request($"ALTER TABLE {MetaInformation.tables[0]} ADD COLUMN {column} {type}");
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            MetaInformation.RefreshData();
        }
        private void Request(string request)
        {
            NpgsqlConnection connect = new NpgsqlConnection(AppSettings.SqlConnection);
            connect.Open();
            NpgsqlCommand command = new NpgsqlCommand(request, connect);
            command.ExecuteNonQuery();
            connect.Close();
        }
    public static bool ContainsOnlyEnglishLetters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            foreach (char c in input)
            {
                if (c < 'A' || (c > 'Z' && c < 'a') || c > 'z')
                    return false;
            }

            return true;
        }
    }
}
