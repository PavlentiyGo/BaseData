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
    public partial class ConstraintForm : Form
    {
        private int TableNum;
        public ConstraintForm(int num)
        {
            TableNum = num;
            InitializeComponent();
            AddInfo(TableNum);
            CheckCheckBox.CheckedChanged += (s, e) =>
            {
                CheckTextBox.Visible = CheckCheckBox.Checked;
            };

            ForeignKeyCheckBox.CheckedChanged += (s, e) =>
            {
                bool show = ForeignKeyCheckBox.Checked;
                TableLabel.Visible = show;
                TableComboBox.Visible = show;
                ColumnLabel.Visible = show;
                ColumnComboBox.Visible = show;
            };
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            string column = comboBox1.Text;
            if (column == "")
            {
                MessageBox.Show("Необходимо выбрать колонку на которую задаются ограничения");
                return;
            }
            if (NotNullCheckBox.Checked)
            {
                Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ALTER COLUMN {column} SET NOT NULL");
                MessageBox.Show("NotNull установлено");
            }
            if (UniqueCheckBox.Checked)
            {
                Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT UNIQUE {column}");
            }
            if (CheckCheckBox.Checked)
            {

            }
            if (ForeignKeyCheckBox.Checked)
            {
            }
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
        private void AddInfo(int tableNum)
        {

            if (tableNum == 0)
            {
                string[] columns = MetaInformation.columnsClients;
                comboBox1.Items.AddRange(columns);
                string[] table = { MetaInformation.tables[1], MetaInformation.tables[3] };

                TableComboBox.Items.AddRange(table);
            }
            else if (tableNum == 1)
            {
                string[] table = { MetaInformation.tables[1], MetaInformation.tables[3] };
                //string[] columns = 5{;
                //comboBox1.Items.AddRange(columns);
                //TableComboBox.Items.AddRange(columns);
                //ColumnComboBox.Items.AddRange(columns);
            }
            else if (tableNum == 3)
            {
                string[] columns = MetaInformation.columnsOrders;
                comboBox1.Items.AddRange(columns);
                TableComboBox.Items.AddRange(columns);
                ColumnComboBox.Items.AddRange(columns);
            }
        }
        private string[] RemoveAt(string[] array, int index)
        {
            if (index < 0 || index >= array.Length)
                return array; // или бросить исключение

            var list = new List<string>(array);
            list.RemoveAt(index);
            return list.ToArray();
        }
        private void TableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColumnComboBox.Items.Clear();

            string? selectedTable = TableComboBox.SelectedItem as string;
            if (selectedTable == null) return;

            string[] columnsToLoad = new string[0];

            if (selectedTable == MetaInformation.tables[1])
                columnsToLoad = MetaInformation.columnsGoods;
            else if (selectedTable == MetaInformation.tables[3])
                columnsToLoad = MetaInformation.columnsOrders;

            ColumnComboBox.Items.AddRange(columnsToLoad);
        }
    }
}
