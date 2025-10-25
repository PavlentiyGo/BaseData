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
            ApplyStyles();
            AddInfo(tableNum);
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this); // Вместо Form1.ApplyFormStyle

                if (AddColumn != null) Styles.ApplyPrimaryButtonStyle(AddColumn);
                if (DeleteColumn != null) Styles.ApplyDangerButtonStyle(DeleteColumn);
                if (ChangeTableData != null) Styles.ApplySecondaryButtonStyle(ChangeTableData);
                if (RenameTable != null) Styles.ApplySecondaryButtonStyle(RenameTable);
                if (SetConstraint != null) Styles.ApplySecondaryButtonStyle(SetConstraint);
                if (DeleteConstraintBtn != null) Styles.ApplyDangerButtonStyle(DeleteConstraintBtn);

                if (DeleteBox != null) Styles.ApplyComboBoxStyle(DeleteBox);
                if (AddColumnTypeBox != null) Styles.ApplyComboBoxStyle(AddColumnTypeBox);
                if (OldColumnName != null) Styles.ApplyComboBoxStyle(OldColumnName);
                if (ChangeDataColumn != null) Styles.ApplyComboBoxStyle(ChangeDataColumn);
                if (ChangeTypeData != null) Styles.ApplyComboBoxStyle(ChangeTypeData);

                if (NewTableName != null) Styles.ApplyTextBoxStyle(NewTableName);
                if (AddColumnText != null) Styles.ApplyTextBoxStyle(AddColumnText);
                if (NewColumnName != null) Styles.ApplyTextBoxStyle(NewColumnName);

                if (label1 != null) Styles.ApplyLabelStyle(label1);
                if (label2 != null) Styles.ApplyLabelStyle(label2);
                if (label3 != null) Styles.ApplyLabelStyle(label3);
                if (label6 != null) Styles.ApplyLabelStyle(label6);
                if (label7 != null) Styles.ApplyLabelStyle(label7);
                if (label8 != null) Styles.ApplyLabelStyle(label8);
                if (label9 != null) Styles.ApplyLabelStyle(label9);
                if (label10 != null) Styles.ApplyLabelStyle(label10);

                log.LogInfo("Стили формы изменения таблицы применены успешно");
            }
            catch (Exception ex)
            {
                log.LogError($"Ошибка применения стилей формы изменения таблицы: {ex.Message}");
            }
        }

        private void SetConstraint_Click(object sender, EventArgs e)
        {
            ConstraintForm constraintForm = new ConstraintForm(TableNum,log);
            constraintForm.ShowDialog();
            this.Close();
        }

        private void RenameTable_Click(object sender, EventArgs e)
        {
            string newTableName = NewTableName.Text;
            string oldColumnName = OldColumnName.Text;
            string newColumnName = NewColumnName.Text;
            if (string.IsNullOrEmpty(oldColumnName) ^ string.IsNullOrEmpty(newColumnName))
            {
                MessageBox.Show("Необходимо полностью заполнить новые имена таблиц или столбцов");
                log.LogWarning("Необходимо полностью заполнить новые имена таблиц или столбцов");
                return;
            }
            if (!string.IsNullOrEmpty(newTableName))
            {
                Request($"ALTER TABLE {MetaInformation.tables[TableNum]} RENAME TO {newTableName}");
                MetaInformation.tables[TableNum] = newTableName.ToLower();
                Form3.RefreshTags();
            }
            if (!string.IsNullOrEmpty(oldColumnName) && !string.IsNullOrEmpty(newColumnName))
            {
                Request($"ALTER TABLE {MetaInformation.tables[TableNum]} RENAME COLUMN {oldColumnName} TO {newColumnName}");
            }
            MetaInformation.RefreshData();
            RefreshTables();
            this.Close();
        }

        private void ChangeTableData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ChangeDataColumn.Text) || string.IsNullOrEmpty(ChangeTypeData.Text))
            {
                MessageBox.Show("Введите столбец для изменения и новый тип данных для него");
                log.LogWarning("Введите столбец для изменения и новый тип данных для него");
                return;
            }
            if (CanChangeColumnType(ChangeDataColumn.Text, ChangeTypeData.Text))
            {
                Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ALTER COLUMN {ChangeDataColumn.Text} TYPE {ChangeTypeData.Text}");
                MetaInformation.RefreshData();
                log.LogInfo($"Тип в столбеце {ChangeDataColumn.Text} в таблице {MetaInformation.tables[TableNum]} был изменён на {ChangeTypeData.Text}");
                RefreshTables();
                this.Close();
            }
            else
            {
                MessageBox.Show("Данный тип нельзя применить к этому столбцу, попробуйте другой");
                log.LogWarning("Применён неверный тип");
                return;
            }
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
                string[] columns = FilterOutIdAndNames(MetaInformation.columnsClients);
                DeleteBox.Items.AddRange(columns);
                ChangeDataColumn.Items.AddRange(columns);
                OldColumnName.Items.AddRange(columns);
            }
            else if (tableNum == 1)
            {
                string[] columns = FilterOutIdAndNames(MetaInformation.columnsGoods);
                DeleteBox.Items.AddRange(columns);
                ChangeDataColumn.Items.AddRange(columns);
                OldColumnName.Items.AddRange(columns);
            }
            else if (tableNum == 3)
            {
                string[] columns = FilterOutIdAndNames(MetaInformation.columnsOrders);
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
        public static string[] FilterOutIdAndNames(string[] input)
        {
            if (input == null)
                return new string[0];

            var result = new List<string>();

            for (int i = 0; i < input.Length; i++)
            {
                string item = input[i];
                if (item != "id" && item != "surname" && item != "name" && item != "client_id")
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }
        private bool CanChangeColumnType(
    string columnName,
    string newType)
        {
            string tableName = MetaInformation.tables[TableNum];
            string connectionString = AppSettings.SqlConnection;
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                string sql = $@"
            ALTER TABLE {tableName}
            ALTER COLUMN {columnName} TYPE {newType};";
                using var cmd = new NpgsqlCommand(sql, conn, transaction);
                cmd.ExecuteNonQuery();
                transaction.Rollback();
                return true;
            }
            catch (PostgresException)
            {
                transaction.Rollback();
                return false;
            }
        }

        private void DeleteConstraintBtn_Click(object sender, EventArgs e)
        {
            DeleteConstraints del = new DeleteConstraints(TableNum,log);
            del.ShowDialog();
            this.Close();
        }

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            AddColumn = new Button();
            DeleteColumn = new Button();
            ChangeTableData = new Button();
            RenameTable = new Button();
            SetConstraint = new Button();
            label1 = new Label();
            DeleteBox = new ComboBox();
            label2 = new Label();
            NewTableName = new TextBox();
            label3 = new Label();
            label6 = new Label();
            AddColumnTypeBox = new ComboBox();
            AddColumnText = new TextBox();
            OldColumnName = new ComboBox();
            label7 = new Label();
            label8 = new Label();
            NewColumnName = new TextBox();
            ChangeDataColumn = new ComboBox();
            label9 = new Label();
            label10 = new Label();
            ChangeTypeData = new ComboBox();
            DeleteConstraintBtn = new Button();
            SuspendLayout();

            AddColumn.Location = new Point(450, 40);
            AddColumn.Name = "AddColumn";
            AddColumn.Size = new Size(180, 60);
            AddColumn.TabIndex = 0;
            AddColumn.Text = "Добавить столбец";
            AddColumn.UseVisualStyleBackColor = true;
            AddColumn.Click += AddColumn_Click;

            DeleteColumn.Location = new Point(450, 120);
            DeleteColumn.Name = "DeleteColumn";
            DeleteColumn.Size = new Size(180, 60);
            DeleteColumn.TabIndex = 1;
            DeleteColumn.Text = "Удалить столбец";
            DeleteColumn.UseVisualStyleBackColor = true;
            DeleteColumn.Click += DeleteColumn_Click;

            ChangeTableData.Location = new Point(450, 200);
            ChangeTableData.Name = "ChangeTableData";
            ChangeTableData.Size = new Size(180, 60);
            ChangeTableData.TabIndex = 2;
            ChangeTableData.Text = "Изменить тип данных столбца";
            ChangeTableData.UseVisualStyleBackColor = true;
            ChangeTableData.Click += ChangeTableData_Click;

            RenameTable.Location = new Point(450, 280);
            RenameTable.Name = "RenameTable";
            RenameTable.Size = new Size(180, 60);
            RenameTable.TabIndex = 3;
            RenameTable.Text = "Переименовать таблицу или столбец";
            RenameTable.UseVisualStyleBackColor = true;
            RenameTable.Click += RenameTable_Click;

            SetConstraint.Location = new Point(450, 360);
            SetConstraint.Name = "SetConstraint";
            SetConstraint.Size = new Size(180, 60);
            SetConstraint.TabIndex = 4;
            SetConstraint.Text = "Задать ограничения";
            SetConstraint.UseVisualStyleBackColor = true;
            SetConstraint.Click += SetConstraint_Click;

            DeleteConstraintBtn.Location = new Point(450, 440);
            DeleteConstraintBtn.Name = "DeleteConstraintBtn";
            DeleteConstraintBtn.Size = new Size(180, 60);
            DeleteConstraintBtn.TabIndex = 27;
            DeleteConstraintBtn.Text = "Удалить ограничение";
            DeleteConstraintBtn.UseVisualStyleBackColor = true;
            DeleteConstraintBtn.Click += DeleteConstraintBtn_Click;

            label1.Location = new Point(40, 40);
            label1.Name = "label1";
            label1.Size = new Size(150, 25);
            label1.TabIndex = 6;
            label1.Text = "Название столбца:";
            label1.TextAlign = ContentAlignment.MiddleLeft;

            AddColumnText.Location = new Point(200, 40);
            AddColumnText.Name = "AddColumnText";
            AddColumnText.Size = new Size(150, 23);
            AddColumnText.TabIndex = 18;

            label6.Location = new Point(40, 80);
            label6.Name = "label6";
            label6.Size = new Size(150, 25);
            label6.TabIndex = 16;
            label6.Text = "Тип столбца:";
            label6.TextAlign = ContentAlignment.MiddleLeft;

            AddColumnTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
            AddColumnTypeBox.FormattingEnabled = true;
            AddColumnTypeBox.Items.AddRange(new object[] { "boolean", "smallint", "integer", "bigint", "serial", "bigserial", "decimal", "text", "varchar", "char", "timestamp", "uuid" });
            AddColumnTypeBox.Location = new Point(200, 80);
            AddColumnTypeBox.Name = "AddColumnTypeBox";
            AddColumnTypeBox.Size = new Size(150, 23);
            AddColumnTypeBox.TabIndex = 17;

            label2.Location = new Point(40, 140);
            label2.Name = "label2";
            label2.Size = new Size(150, 25);
            label2.TabIndex = 9;
            label2.Text = "Столбец для удаления:";
            label2.TextAlign = ContentAlignment.MiddleLeft;

            DeleteBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DeleteBox.FormattingEnabled = true;
            DeleteBox.Location = new Point(200, 140);
            DeleteBox.Name = "DeleteBox";
            DeleteBox.Size = new Size(150, 23);
            DeleteBox.TabIndex = 8;

            label9.Location = new Point(40, 200);
            label9.Name = "label9";
            label9.Size = new Size(150, 25);
            label9.TabIndex = 24;
            label9.Text = "Столбец для изменения:";
            label9.TextAlign = ContentAlignment.MiddleLeft;

            ChangeDataColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            ChangeDataColumn.FormattingEnabled = true;
            ChangeDataColumn.Location = new Point(200, 200);
            ChangeDataColumn.Name = "ChangeDataColumn";
            ChangeDataColumn.Size = new Size(150, 23);
            ChangeDataColumn.TabIndex = 23;

            label10.Location = new Point(40, 240);
            label10.Name = "label10";
            label10.Size = new Size(150, 25);
            label10.TabIndex = 25;
            label10.Text = "Новый тип данных:";
            label10.TextAlign = ContentAlignment.MiddleLeft;

            ChangeTypeData.DropDownStyle = ComboBoxStyle.DropDownList;
            ChangeTypeData.FormattingEnabled = true;
            ChangeTypeData.Items.AddRange(new object[] { "boolean", "smallint", "integer", "bigint", "serial", "bigserial", "decimal", "text", "varchar", "char", "timestamp", "uuid" });
            ChangeTypeData.Location = new Point(200, 240);
            ChangeTypeData.Name = "ChangeTypeData";
            ChangeTypeData.Size = new Size(150, 23);
            ChangeTypeData.TabIndex = 26;

            label3.Location = new Point(40, 300);
            label3.Name = "label3";
            label3.Size = new Size(150, 25);
            label3.TabIndex = 11;
            label3.Text = "Новое имя таблицы:";
            label3.TextAlign = ContentAlignment.MiddleLeft;

            NewTableName.Location = new Point(200, 300);
            NewTableName.Name = "NewTableName";
            NewTableName.Size = new Size(150, 23);
            NewTableName.TabIndex = 10;

            label8.Location = new Point(40, 340);
            label8.Name = "label8";
            label8.Size = new Size(150, 25);
            label8.TabIndex = 21;
            label8.Text = "Старое имя столбца:";
            label8.TextAlign = ContentAlignment.MiddleLeft;

            OldColumnName.DropDownStyle = ComboBoxStyle.DropDownList;
            OldColumnName.FormattingEnabled = true;
            OldColumnName.Location = new Point(200, 340);
            OldColumnName.Name = "OldColumnName";
            OldColumnName.Size = new Size(150, 23);
            OldColumnName.TabIndex = 19;

            label7.Location = new Point(40, 380);
            label7.Name = "label7";
            label7.Size = new Size(150, 25);
            label7.TabIndex = 20;
            label7.Text = "Новое имя столбца:";
            label7.TextAlign = ContentAlignment.MiddleLeft;

            NewColumnName.Location = new Point(200, 380);
            NewColumnName.Name = "NewColumnName";
            NewColumnName.Size = new Size(150, 23);
            NewColumnName.TabIndex = 22;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 540);
            Controls.Add(DeleteConstraintBtn);
            Controls.Add(ChangeTypeData);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(ChangeDataColumn);
            Controls.Add(NewColumnName);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(OldColumnName);
            Controls.Add(AddColumnText);
            Controls.Add(AddColumnTypeBox);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(NewTableName);
            Controls.Add(label2);
            Controls.Add(DeleteBox);
            Controls.Add(label1);
            Controls.Add(SetConstraint);
            Controls.Add(RenameTable);
            Controls.Add(ChangeTableData);
            Controls.Add(DeleteColumn);
            Controls.Add(AddColumn);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ClientTableChange";
            Text = "Изменение структуры таблицы";
            ResumeLayout(false);
            PerformLayout();
        }

        private Button AddColumn;
        private Button DeleteColumn;
        private Button ChangeTableData;
        private Button RenameTable;
        private Button SetConstraint;
        private Label label1;
        private ComboBox DeleteBox;
        private Label label2;
        private TextBox NewTableName;
        private Label label3;
        private Label label6;
        private ComboBox AddColumnTypeBox;
        private TextBox AddColumnText;
        private ComboBox OldColumnName;
        private Label label7;
        private Label label8;
        private TextBox NewColumnName;
        private ComboBox ChangeDataColumn;
        private Label label9;
        private Label label10;
        private ComboBox ChangeTypeData;
        private Button DeleteConstraintBtn;
    }
}