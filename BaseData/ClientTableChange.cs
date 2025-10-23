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
        private int TableNum;
        public ClientTableChange(Log rch, int tableNum)
        {
            TableNum = tableNum;
            log = rch;
            InitializeComponent();
            AddInfo(tableNum);
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
                Request($"ALTER TABLE {MetaInformation.tables[TableNum]} RENAME TO {newTableName}");
            }
            if (!string.IsNullOrEmpty(oldColumnName) && !string.IsNullOrEmpty(newColumnName))
            {
                Request($"ALTER TABLE {MetaInformation.tables[TableNum]} RENAME COLUMN {oldColumnName} TO {newColumnName}");

            }
            MetaInformation.RefreshData();
            if (string.IsNullOrEmpty(oldColumnName) ^ string.IsNullOrEmpty(newColumnName))
            {
                MessageBox.Show("Необходимо полностью заполнить новые имена таблиц или столбцов");
                log.LogWarning("Необходимо полностью заполнить новые имена таблиц или столбцов");
                return;
            }
            RefreshTables();
            this.Close();
        }

        private void ChangeTableData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ChangeDataColumn.Text) || string.IsNullOrEmpty(ChangeTypeData.Text)){
                MessageBox.Show("Введите столбец для изменения и новый тип данных для него");
                log.LogWarning("Введите столбец для изменения и новый тип данных для него");
                return;
            }
            Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ALTER COLUMN {ChangeDataColumn.Text} TYPE {ChangeTypeData.Text}");
            MetaInformation.RefreshData();
            log.LogInfo($"Тип в столбеце {ChangeDataColumn.Text} в таблице {MetaInformation.tables[TableNum]} был изменён на {ChangeTypeData.Text}");
            RefreshTables();
            this.Close();
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
            if (deleteColumn == "id"|| deleteColumn == "client_id")
            {
                MessageBox.Show("Для стабильной работы программы выбранный столбец нельзя удалять");
                log.LogWarning("Для стабильной работы программы выбранный столбец нельзя удалять");
                return;
            }
            Request($"ALTER TABLE {MetaInformation.tables[TableNum]} DROP COLUMN {deleteColumn} CASCADE");
            log.LogInfo($"Столбец {deleteColumn} был удалён из таблицы {MetaInformation.tables[TableNum]}");
            MetaInformation.RefreshData();
            RefreshTables();
            this.Close();
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
            log.LogInfo($"Добавлен столбец {column} с типом {type} в таблицу {MetaInformation.tables[TableNum]}");
            Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD COLUMN {column} {type}");
            MetaInformation.RefreshData();
            RefreshTables();
            this.Close();
        }
        private void Request(string request)
        {
            NpgsqlConnection connect = new NpgsqlConnection(AppSettings.SqlConnection);
            connect.Open();
            NpgsqlCommand command = new NpgsqlCommand(request, connect);
            command.ExecuteNonQuery();
            connect.Close();
        }

        public void AddInfo(int tableNum)
        {
            if (tableNum == 0)
            {
                string[] columns = MetaInformation.columnsClients;
                DeleteBox.Items.AddRange(columns);
                ChangeDataColumn.Items.AddRange(columns);
                OldColumnName.Items.AddRange(columns);
            }
            else if (tableNum == 1)
            {
                string[] columns = MetaInformation.columnsGoods;
                DeleteBox.Items.AddRange(columns);
                ChangeDataColumn.Items.AddRange(columns);
                OldColumnName.Items.AddRange(columns);
            }
            else if (tableNum == 3)
            {
                string[] columns = MetaInformation.columnsOrders;
                DeleteBox.Items.AddRange(columns);
                ChangeDataColumn.Items.AddRange(columns);
                OldColumnName.Items.AddRange(columns);
            }
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
        private void RefreshTables()
        {
            if (TableNum == 0)
            {
                Clients.RefreshData();
            }
            else if (TableNum == 1)
            {
                GoodsUC.RefreshData();
            }
            else if (TableNum == 3)
            {
                SellsUC.RefreshData();
            }
        }
    }
}
