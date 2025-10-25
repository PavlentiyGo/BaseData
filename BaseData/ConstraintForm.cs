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
        private Log rch = new Log();

        public ConstraintForm(int num, Log log)
        {
            rch = log;
            TableNum = num;
            InitializeComponent();
            ApplyStyles();
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

            rch.LogInfo($"Форма ограничений для таблицы {MetaInformation.tables[TableNum]} инициализирована");
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                label1.ForeColor = Styles.DarkColor;

                if (comboBox1 != null) Styles.ApplyComboBoxStyle(comboBox1);
                if (TableComboBox != null) Styles.ApplyComboBoxStyle(TableComboBox);
                if (ColumnComboBox != null) Styles.ApplyComboBoxStyle(ColumnComboBox);
                if (CheckTextBox != null) Styles.ApplyTextBoxStyle(CheckTextBox);

                if (NotNullCheckBox != null)
                {
                    NotNullCheckBox.ForeColor = Styles.DarkColor;
                    NotNullCheckBox.Font = Styles.MainFont;
                }
                if (UniqueCheckBox != null)
                {
                    UniqueCheckBox.ForeColor = Styles.DarkColor;
                    UniqueCheckBox.Font = Styles.MainFont;
                }
                if (CheckCheckBox != null)
                {
                    CheckCheckBox.ForeColor = Styles.DarkColor;
                    CheckCheckBox.Font = Styles.MainFont;
                }
                if (ForeignKeyCheckBox != null)
                {
                    ForeignKeyCheckBox.ForeColor = Styles.DarkColor;
                    ForeignKeyCheckBox.Font = Styles.MainFont;
                }

                if (TableLabel != null) Styles.ApplyLabelStyle(TableLabel);
                if (ColumnLabel != null) Styles.ApplyLabelStyle(ColumnLabel);

                if (SaveBtn != null)
                {
                    Styles.ApplyPrimaryButtonStyle(SaveBtn);
                    SaveBtn.Font = new Font(Styles.MainFont.FontFamily, 11F, FontStyle.Bold);
                    SaveBtn.Size = new Size(220, 60);
                }

                rch.LogInfo("Стили формы ограничений применены успешно");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка применения стилей формы ограничений: {ex.Message}");
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                rch.LogInfo("Начало сохранения ограничений");

                string column = comboBox1.Text;
                if (column == "")
                {
                    rch.LogWarning("Попытка сохранения ограничений без выбора колонки");
                    MessageBox.Show("Необходимо выбрать колонку на которую задаются ограничения");
                    return;
                }

                bool changesMade = false;

                if (CheckCheckBox.Checked)
                {
                    if (CheckTextBox.Text == "")
                    {
                        rch.LogWarning("Попытка сохранения CHECK ограничения без условия");
                        MessageBox.Show("Необходимо заполнить check, если он выбран");
                        return;
                    }
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT check_{MetaInformation.tables[TableNum]}_{column} CHECK ({CheckTextBox.Text});");
                    rch.LogInfo($"CHECK ограничение установлено для колонки {column}");
                    changesMade = true;
                }

                if (ForeignKeyCheckBox.Checked)
                {
                    if (TableComboBox.Text == "" || ColumnComboBox.Text == "")
                    {
                        rch.LogWarning("Попытка сохранения FOREIGN KEY без выбора таблицы или колонки");
                        MessageBox.Show("Необходимо заполнить Таблица и Столбец, если Foreign Key выбран");
                        return;
                    }
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT fk_{MetaInformation.tables[TableNum]}_{column} FOREIGN KEY ({column}) REFERENCES {TableComboBox.Text}({ColumnComboBox.Text})");
                    rch.LogInfo($"FOREIGN KEY установлен для колонки {column}");
                    changesMade = true;
                }

                if (NotNullCheckBox.Checked)
                {
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ALTER COLUMN {column} SET NOT NULL");
                    rch.LogInfo($"NOT NULL установлен для колонки {column}");
                    changesMade = true;
                }

                if (UniqueCheckBox.Checked)
                {
                    Request($"ALTER TABLE {MetaInformation.tables[TableNum]} ADD CONSTRAINT unique_{MetaInformation.tables[TableNum]}_{column} UNIQUE ({column})");
                    rch.LogInfo($"UNIQUE установлен для колонки {column}");
                    changesMade = true;
                }

                if (changesMade)
                {
                    MessageBox.Show("Все выбранные ограничения успешно установлены");
                    rch.LogInfo("Ограничения успешно сохранены");
                }
                else
                {
                    MessageBox.Show("Не выбрано ни одного ограничения");
                    rch.LogWarning("Попытка сохранения без выбора ограничений");
                }

                this.Close();
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка сохранения ограничений: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }

        private void Request(string request)
        {
            using (NpgsqlConnection connect = new NpgsqlConnection(AppSettings.SqlConnection))
            {
                connect.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(request, connect))
                {
                    command.ExecuteNonQuery();
                }
            }
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
            label1 = new Label();
            comboBox1 = new ComboBox();
            NotNullCheckBox = new CheckBox();
            UniqueCheckBox = new CheckBox();
            CheckCheckBox = new CheckBox();
            ForeignKeyCheckBox = new CheckBox();
            CheckTextBox = new TextBox();
            TableLabel = new Label();
            TableComboBox = new ComboBox();
            ColumnLabel = new Label();
            ColumnComboBox = new ComboBox();
            SaveBtn = new Button();
            SuspendLayout();

            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(341, 34);
            label1.TabIndex = 0;
            label1.Text = "Ограничения для колонки: ";

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(359, 20);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(197, 23);
            comboBox1.TabIndex = 1;

            NotNullCheckBox.AutoSize = true;
            NotNullCheckBox.Location = new Point(27, 76);
            NotNullCheckBox.Name = "NotNullCheckBox";
            NotNullCheckBox.Size = new Size(81, 19);
            NotNullCheckBox.TabIndex = 2;
            NotNullCheckBox.Text = "NOT NULL";
            NotNullCheckBox.UseVisualStyleBackColor = true;

            UniqueCheckBox.AutoSize = true;
            UniqueCheckBox.Location = new Point(27, 112);
            UniqueCheckBox.Name = "UniqueCheckBox";
            UniqueCheckBox.Size = new Size(69, 19);
            UniqueCheckBox.TabIndex = 3;
            UniqueCheckBox.Text = "UNIQUE";
            UniqueCheckBox.UseVisualStyleBackColor = true;

            CheckCheckBox.AutoSize = true;
            CheckCheckBox.Location = new Point(27, 147);
            CheckCheckBox.Name = "CheckCheckBox";
            CheckCheckBox.Size = new Size(64, 19);
            CheckCheckBox.TabIndex = 4;
            CheckCheckBox.Text = "CHECK";
            CheckCheckBox.UseVisualStyleBackColor = true;

            ForeignKeyCheckBox.AutoSize = true;
            ForeignKeyCheckBox.Location = new Point(27, 186);
            ForeignKeyCheckBox.Name = "ForeignKeyCheckBox";
            ForeignKeyCheckBox.Size = new Size(97, 19);
            ForeignKeyCheckBox.TabIndex = 5;
            ForeignKeyCheckBox.Text = "FOREIGN KEY";
            ForeignKeyCheckBox.UseVisualStyleBackColor = true;

            CheckTextBox.Location = new Point(126, 145);
            CheckTextBox.Name = "CheckTextBox";
            CheckTextBox.Size = new Size(261, 23);
            CheckTextBox.TabIndex = 6;
            CheckTextBox.Visible = false;

            TableLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            TableLabel.Location = new Point(130, 186);
            TableLabel.Name = "TableLabel";
            TableLabel.Size = new Size(61, 19);
            TableLabel.TabIndex = 7;
            TableLabel.Text = "Таблица";
            TableLabel.Visible = false;

            TableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TableComboBox.FormattingEnabled = true;
            TableComboBox.Location = new Point(197, 182);
            TableComboBox.Name = "TableComboBox";
            TableComboBox.Size = new Size(98, 23);
            TableComboBox.TabIndex = 8;
            TableComboBox.Visible = false;
            TableComboBox.SelectedIndexChanged += TableComboBox_SelectedIndexChanged;

            ColumnLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            ColumnLabel.Location = new Point(313, 185);
            ColumnLabel.Name = "ColumnLabel";
            ColumnLabel.Size = new Size(61, 19);
            ColumnLabel.TabIndex = 9;
            ColumnLabel.Text = "Столбец";
            ColumnLabel.Visible = false;

            ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ColumnComboBox.FormattingEnabled = true;
            ColumnComboBox.Location = new Point(380, 182);
            ColumnComboBox.Name = "ColumnComboBox";
            ColumnComboBox.Size = new Size(100, 23);
            ColumnComboBox.TabIndex = 10;
            ColumnComboBox.Visible = false;

            SaveBtn.Location = new Point(242, 231);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(192, 85);
            SaveBtn.TabIndex = 11;
            SaveBtn.Text = "Сохранить изменения";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(657, 340);
            Controls.Add(SaveBtn);
            Controls.Add(ColumnComboBox);
            Controls.Add(ColumnLabel);
            Controls.Add(TableComboBox);
            Controls.Add(TableLabel);
            Controls.Add(CheckTextBox);
            Controls.Add(ForeignKeyCheckBox);
            Controls.Add(CheckCheckBox);
            Controls.Add(UniqueCheckBox);
            Controls.Add(NotNullCheckBox);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Name = "ConstraintForm";
            Text = "ConstraintForm";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private ComboBox comboBox1;
        private CheckBox NotNullCheckBox;
        private CheckBox UniqueCheckBox;
        private CheckBox CheckCheckBox;
        private CheckBox ForeignKeyCheckBox;
        private TextBox CheckTextBox;
        private Label TableLabel;
        private ComboBox TableComboBox;
        private Label ColumnLabel;
        private ComboBox ColumnComboBox;
        private Button SaveBtn;
    }
}