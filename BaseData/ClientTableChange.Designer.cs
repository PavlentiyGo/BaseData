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
            NewTableName = new TextBox();
            label3 = new Label();
            label4 = new Label();
            comboBox3 = new ComboBox();
            label5 = new Label();
            comboBox4 = new ComboBox();
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
            RenameTable.Text = "Переименовать таблицу или столбец";
            RenameTable.UseVisualStyleBackColor = true;
            RenameTable.Click += RenameTable_Click;
            // 
            // SetConstraint
            // 
            SetConstraint.Location = new Point(378, 428);
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
            DeleteBox.DropDownStyle = ComboBoxStyle.DropDownList;
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
            // NewTableName
            // 
            NewTableName.Location = new Point(45, 359);
            NewTableName.Name = "NewTableName";
            NewTableName.Size = new Size(100, 23);
            NewTableName.TabIndex = 10;
            // 
            // label3
            // 
            label3.Location = new Point(45, 305);
            label3.Name = "label3";
            label3.Size = new Size(100, 41);
            label3.TabIndex = 11;
            label3.Text = "Новое имя таблицы";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.Location = new Point(56, 428);
            label4.Name = "label4";
            label4.Size = new Size(110, 23);
            label4.TabIndex = 12;
            label4.Text = "Название столбца";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox3
            // 
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(182, 428);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(121, 23);
            comboBox3.TabIndex = 13;
            // 
            // label5
            // 
            label5.Location = new Point(56, 468);
            label5.Name = "label5";
            label5.Size = new Size(110, 23);
            label5.TabIndex = 14;
            label5.Text = "Ограничение";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox4
            // 
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(182, 469);
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
            AddColumnTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
            AddColumnTypeBox.FormattingEnabled = true;
            AddColumnTypeBox.Items.AddRange(new object[] { "boolean", "smallint", "integer", "bigint", "serial", "bigserial", "decimal", "text", "varchar", "char", "timestamp", "uuid" });
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
            // OldColumnName
            // 
            OldColumnName.DropDownStyle = ComboBoxStyle.DropDownList;
            OldColumnName.FormattingEnabled = true;
            OldColumnName.Location = new Point(265, 318);
            OldColumnName.Name = "OldColumnName";
            OldColumnName.Size = new Size(100, 23);
            OldColumnName.TabIndex = 19;
            // 
            // label7
            // 
            label7.Location = new Point(159, 341);
            label7.Name = "label7";
            label7.Size = new Size(100, 41);
            label7.TabIndex = 20;
            label7.Text = "Новое имя столбца";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.Location = new Point(159, 300);
            label8.Name = "label8";
            label8.Size = new Size(100, 41);
            label8.TabIndex = 21;
            label8.Text = "Старое имя столбца";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // NewColumnName
            // 
            NewColumnName.Location = new Point(265, 359);
            NewColumnName.Name = "NewColumnName";
            NewColumnName.Size = new Size(100, 23);
            NewColumnName.TabIndex = 22;
            // 
            // ChangeDataColumn
            // 
            ChangeDataColumn.DropDownStyle = ComboBoxStyle.DropDownList;
            ChangeDataColumn.FormattingEnabled = true;
            ChangeDataColumn.Location = new Point(36, 246);
            ChangeDataColumn.Name = "ChangeDataColumn";
            ChangeDataColumn.Size = new Size(109, 23);
            ChangeDataColumn.TabIndex = 23;
            // 
            // label9
            // 
            label9.Location = new Point(36, 202);
            label9.Name = "label9";
            label9.Size = new Size(109, 41);
            label9.TabIndex = 24;
            label9.Text = "Название столбца";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            label10.Location = new Point(203, 202);
            label10.Name = "label10";
            label10.Size = new Size(109, 41);
            label10.TabIndex = 25;
            label10.Text = "Изменить тип данных";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ChangeTypeData
            // 
            ChangeTypeData.DropDownStyle = ComboBoxStyle.DropDownList;
            ChangeTypeData.FormattingEnabled = true;
            ChangeTypeData.Items.AddRange(new object[] { "boolean", "smallint", "integer", "bigint", "serial", "bigserial", "decimal", "text", "varchar", "char", "timestamp", "uuid" });
            ChangeTypeData.Location = new Point(203, 246);
            ChangeTypeData.Name = "ChangeTypeData";
            ChangeTypeData.Size = new Size(109, 23);
            ChangeTypeData.TabIndex = 26;
            // 
            // ClientTableChange
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 561);
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
            Controls.Add(comboBox4);
            Controls.Add(label5);
            Controls.Add(comboBox3);
            Controls.Add(label4);
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
        private TextBox NewTableName;
        private Label label3;
        private Label label4;
        private ComboBox comboBox3;
        private Label label5;
        private ComboBox comboBox4;
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
    }
}