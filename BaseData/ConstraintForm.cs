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
            try
            {
                string column = comboBox1.Text;
                if (column == "")
                {
                    MessageBox.Show("Необходимо выбрать колонку на которую задаются ограничения");
                    return;
                }
                if (CheckCheckBox.Checked)
                {
                    if (CheckTextBox.Text == "")
                    {
                        MessageBox.Show("Необходимо заполнить check, если он выбран");
                        return;
                    }
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT check_{MetaInformation.tables[TableNum]}_{column} CHECK ({CheckTextBox.Text});");
                    MessageBox.Show("Check установлен");
                }
                if (ForeignKeyCheckBox.Checked)
                {
                    if (TableComboBox.Text == "" || ColumnComboBox.Text == "")
                    {
                        MessageBox.Show("Необходимо заполнить Таблица и Столбец, если Foreign Key выбран");
                        return;
                    }
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT fk_{MetaInformation.tables[TableNum]}_{column} FOREIGN KEY ({column}) REFERENCES {TableComboBox.Text}({ColumnComboBox.Text})");
                    MessageBox.Show("Foreign key установлен");
                }
                if (NotNullCheckBox.Checked)
                {
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ALTER COLUMN {column} SET NOT NULL");
                    MessageBox.Show("Not Null установлен");
                }
                MessageBox.Show($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT unique_{MetaInformation.tables[TableNum]}_{column} UNIQUE {column}");
                if (UniqueCheckBox.Checked)
                {
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT unique_{MetaInformation.tables[TableNum]}_{column} UNIQUE ({column})");
                    MessageBox.Show("Unique установлен");
                }
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
                string[] columns = MetaInformation.columnsGoods;
                comboBox1.Items.AddRange(columns);
                string[] table = { MetaInformation.tables[0], MetaInformation.tables[3] };
                TableComboBox.Items.AddRange(table);
            }
            else if (tableNum == 3)
            {
                string[] columns = MetaInformation.columnsOrders;
                comboBox1.Items.AddRange(columns);
                string[] table = { MetaInformation.tables[0], MetaInformation.tables[1] };
                TableComboBox.Items.AddRange(table);
            }
        }
        private void TableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColumnComboBox.Items.Clear();

            string? selectedTable = TableComboBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedTable))
                return;

            string[]? columnsToLoad = selectedTable switch
            {
                _ when selectedTable == MetaInformation.tables[0] => MetaInformation.columnsClients,
                _ when selectedTable == MetaInformation.tables[1] => MetaInformation.columnsGoods,
                _ when selectedTable == MetaInformation.tables[3] => MetaInformation.columnsOrders,
                _ => null 
            };

            if (columnsToLoad != null)
            {
                ColumnComboBox.Items.AddRange(columnsToLoad);
                if (ColumnComboBox.Items.Count > 0)
                    ColumnComboBox.SelectedIndex = 0;
            }
        }
    }
}
