namespace BaseData
{
    partial class ConstraintForm
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
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(375, 34);
            label1.TabIndex = 0;
            label1.Text = "Ограничения для колонки: ";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(359, 20);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(197, 23);
            comboBox1.TabIndex = 1;
            // 
            // NotNullCheckBox
            // 
            NotNullCheckBox.AutoSize = true;
            NotNullCheckBox.Location = new Point(27, 76);
            NotNullCheckBox.Name = "NotNullCheckBox";
            NotNullCheckBox.Size = new Size(81, 19);
            NotNullCheckBox.TabIndex = 2;
            NotNullCheckBox.Text = "NOT NULL";
            NotNullCheckBox.UseVisualStyleBackColor = true;
            // 
            // UniqueCheckBox
            // 
            UniqueCheckBox.AutoSize = true;
            UniqueCheckBox.Location = new Point(27, 112);
            UniqueCheckBox.Name = "UniqueCheckBox";
            UniqueCheckBox.Size = new Size(69, 19);
            UniqueCheckBox.TabIndex = 3;
            UniqueCheckBox.Text = "UNIQUE";
            UniqueCheckBox.UseVisualStyleBackColor = true;
            // 
            // CheckCheckBox
            // 
            CheckCheckBox.AutoSize = true;
            CheckCheckBox.Location = new Point(27, 147);
            CheckCheckBox.Name = "CheckCheckBox";
            CheckCheckBox.Size = new Size(64, 19);
            CheckCheckBox.TabIndex = 4;
            CheckCheckBox.Text = "CHECK";
            CheckCheckBox.UseVisualStyleBackColor = true;
            // 
            // ForeignKeyCheckBox
            // 
            ForeignKeyCheckBox.AutoSize = true;
            ForeignKeyCheckBox.Location = new Point(27, 186);
            ForeignKeyCheckBox.Name = "ForeignKeyCheckBox";
            ForeignKeyCheckBox.Size = new Size(97, 19);
            ForeignKeyCheckBox.TabIndex = 5;
            ForeignKeyCheckBox.Text = "FOREIGN KEY";
            ForeignKeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // CheckTextBox
            // 
            CheckTextBox.Location = new Point(126, 145);
            CheckTextBox.Name = "CheckTextBox";
            CheckTextBox.Size = new Size(261, 23);
            CheckTextBox.TabIndex = 6;
            CheckTextBox.Visible = false;
            // 
            // TableLabel
            // 
            TableLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            TableLabel.Location = new Point(130, 186);
            TableLabel.Name = "TableLabel";
            TableLabel.Size = new Size(61, 19);
            TableLabel.TabIndex = 7;
            TableLabel.Text = "Таблица";
            TableLabel.Visible = false;
            // 
            // TableComboBox
            // 
            TableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TableComboBox.FormattingEnabled = true;
            TableComboBox.Location = new Point(197, 182);
            TableComboBox.Name = "TableComboBox";
            TableComboBox.Size = new Size(98, 23);
            TableComboBox.TabIndex = 8;
            TableComboBox.Visible = false;
            TableComboBox.SelectedIndexChanged += TableComboBox_SelectedIndexChanged;
            // 
            // ColumnLabel
            // 
            ColumnLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            ColumnLabel.Location = new Point(313, 185);
            ColumnLabel.Name = "ColumnLabel";
            ColumnLabel.Size = new Size(61, 19);
            ColumnLabel.TabIndex = 9;
            ColumnLabel.Text = "Столбец";
            ColumnLabel.Visible = false;
            // 
            // ColumnComboBox
            // 
            ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ColumnComboBox.FormattingEnabled = true;
            ColumnComboBox.Location = new Point(380, 182);
            ColumnComboBox.Name = "ColumnComboBox";
            ColumnComboBox.Size = new Size(100, 23);
            ColumnComboBox.TabIndex = 10;
            ColumnComboBox.Visible = false;
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(242, 231);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(192, 85);
            SaveBtn.TabIndex = 11;
            SaveBtn.Text = "Сохранить изменения";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // ConstraintForm
            // 
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

        #endregion

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