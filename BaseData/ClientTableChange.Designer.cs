namespace BaseData
{
    partial class ClientTableChange
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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
            textBox1 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            comboBox3 = new ComboBox();
            label5 = new Label();
            comboBox4 = new ComboBox();
            label6 = new Label();
            AddColumnTypeBox = new ComboBox();
            AddColumnText = new TextBox();
            SuspendLayout();
            // 
            // AddColumn
            // 
            AddColumn.Location = new Point(378, 27);
            AddColumn.Name = "AddColumn";
            AddColumn.Size = new Size(151, 83);
            AddColumn.TabIndex = 0;
            AddColumn.Text = "Добавить столбец";
            AddColumn.UseVisualStyleBackColor = true;
            AddColumn.Click += AddColumn_Click;
            // 
            // DeleteColumn
            // 
            DeleteColumn.Location = new Point(378, 116);
            DeleteColumn.Name = "DeleteColumn";
            DeleteColumn.Size = new Size(151, 80);
            DeleteColumn.TabIndex = 1;
            DeleteColumn.Text = "Удалить столбец";
            DeleteColumn.UseVisualStyleBackColor = true;
            DeleteColumn.Click += DeleteColumn_Click;
            DeleteBox.DataSource = MetaInformation.columnsClients;
            // 
            // ChangeTableData
            // 
            ChangeTableData.Location = new Point(378, 202);
            ChangeTableData.Name = "ChangeTableData";
            ChangeTableData.Size = new Size(151, 97);
            ChangeTableData.TabIndex = 2;
            ChangeTableData.Text = "Изменить данные столбца";
            ChangeTableData.UseVisualStyleBackColor = true;
            ChangeTableData.Click += ChangeTableData_Click;
            // 
            // RenameTable
            // 
            RenameTable.Location = new Point(378, 305);
            RenameTable.Name = "RenameTable";
            RenameTable.Size = new Size(151, 77);
            RenameTable.TabIndex = 3;
            RenameTable.Text = "Переименовать таблицу";
            RenameTable.UseVisualStyleBackColor = true;
            RenameTable.Click += RenameTable_Click;
            // 
            // SetConstraint
            // 
            SetConstraint.Location = new Point(378, 388);
            SetConstraint.Name = "SetConstraint";
            SetConstraint.Size = new Size(151, 64);
            SetConstraint.TabIndex = 4;
            SetConstraint.Text = "Задать ограничения";
            SetConstraint.UseVisualStyleBackColor = true;
            SetConstraint.Click += SetConstraint_Click;
            // 
            // label1
            // 
            label1.Location = new Point(76, 27);
            label1.Name = "label1";
            label1.Size = new Size(100, 41);
            label1.TabIndex = 6;
            label1.Text = "Название столбца";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // DeleteBox
            // 
            DeleteBox.FormattingEnabled = true;
            DeleteBox.Location = new Point(138, 173);
            DeleteBox.Name = "DeleteBox";
            DeleteBox.Size = new Size(121, 23);
            DeleteBox.TabIndex = 8;
            // 
            // label2
            // 
            label2.Location = new Point(147, 129);
            label2.Name = "label2";
            label2.Size = new Size(100, 41);
            label2.TabIndex = 9;
            label2.Text = "Название столбца";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(138, 359);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(121, 23);
            textBox1.TabIndex = 10;
            // 
            // label3
            // 
            label3.Location = new Point(147, 315);
            label3.Name = "label3";
            label3.Size = new Size(100, 41);
            label3.TabIndex = 11;
            label3.Text = "Название столбца";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.Location = new Point(66, 388);
            label4.Name = "label4";
            label4.Size = new Size(110, 23);
            label4.TabIndex = 12;
            label4.Text = "Название столбца";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(182, 388);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(121, 23);
            comboBox3.TabIndex = 13;
            // 
            // label5
            // 
            label5.Location = new Point(66, 417);
            label5.Name = "label5";
            label5.Size = new Size(110, 23);
            label5.TabIndex = 14;
            label5.Text = "Ограничение";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(182, 417);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(121, 23);
            comboBox4.TabIndex = 15;
            // 
            // label6
            // 
            label6.Location = new Point(239, 27);
            label6.Name = "label6";
            label6.Size = new Size(100, 41);
            label6.TabIndex = 16;
            label6.Text = "Тип столбца";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AddColumnTypeBox
            // 
            AddColumnTypeBox.FormattingEnabled = true;
            AddColumnTypeBox.Items.AddRange(new object[] { "boolean", "smallint", "integer", "bigint", "serial", "bigserial", "numeric", "decimal", "real", "double precision", "text", "varchar", "char", "date", "time", "timestamp", "timestamptz", "interval", "json", "jsonb", "uuid", "bytea", "inet" });
            AddColumnTypeBox.Location = new Point(230, 71);
            AddColumnTypeBox.Name = "AddColumnTypeBox";
            AddColumnTypeBox.Size = new Size(121, 23);
            AddColumnTypeBox.TabIndex = 17;
            // 
            // AddColumnText
            // 
            AddColumnText.Location = new Point(66, 71);
            AddColumnText.Name = "AddColumnText";
            AddColumnText.Size = new Size(120, 23);
            AddColumnText.TabIndex = 18;
            // 
            // ClientTableChange
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 561);
            Controls.Add(AddColumnText);
            Controls.Add(AddColumnTypeBox);
            Controls.Add(label6);
            Controls.Add(comboBox4);
            Controls.Add(label5);
            Controls.Add(comboBox3);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox1);
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
            Text = "ClientTableChange";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button AddColumn;
        private Button DeleteColumn;
        private Button ChangeTableData;
        private Button RenameTable;
        private Button SetConstraint;
        private Label label1;
        private ComboBox DeleteBox;
        private Label label2;
        private TextBox textBox1;
        private Label label3;
        private Label label4;
        private ComboBox comboBox3;
        private Label label5;
        private ComboBox comboBox4;
        private Label label6;
        private ComboBox AddColumnTypeBox;
        private TextBox AddColumnText;
    }
}