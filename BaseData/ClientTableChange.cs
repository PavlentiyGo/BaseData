using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
            string newTableName = NewTableName.Text;
            string oldColumnName = OldColumnName.Text;
            string newColumnName = NewColumnName.Text;
            if (!string.IsNullOrEmpty(newTableName))
            {
                Request($"ALTER TABLE {MetaInformation.tables[0]} RENAME TO {newTableName}");
            }
            if (!string.IsNullOrEmpty(oldColumnName) && !string.IsNullOrEmpty(newColumnName))
            {
                Request($"ALTER TABLE {MetaInformation.tables[0]} RENAME COLUMN {oldColumnName} TO {newColumnName}");
             
            }
            MetaInformation.RefreshData();
            MessageBox.Show(MetaInformation.tables[0]);
            if (string.IsNullOrEmpty(oldColumnName) ^ string.IsNullOrEmpty(newColumnName))
            {
                MessageBox.Show("Необходимо полностью заполнить новые имена таблиц или столбцов");
                log.LogWarning("Необходимо полностью заполнить новые имена таблиц или столбцов");
                return;
            }
        }

        private void ChangeTableData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ChangeDataColumn.Text) || string.IsNullOrEmpty(ChangeTypeData.Text)){
                MessageBox.Show("Введите столбец для изменения и новый тип данных для него");
                log.LogWarning("Введите столбец для изменения и новый тип данных для него");
                return;
            }
            Request($"ALTER TABLE {MetaInformation.tables[0]} ALTER COLUMN {ChangeDataColumn.Text} TYPE {ChangeTypeData.Text}");
            MetaInformation.RefreshData();
            log.LogInfo($"Тип в столбеце {ChangeDataColumn.Text} в таблице {MetaInformation.tables[0]} был изменён на {ChangeTypeData.Text}");
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
