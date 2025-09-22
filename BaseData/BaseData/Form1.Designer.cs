namespace BaseData
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            CreateButton = new Button();
            AddButton = new Button();
            GetButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(788, 438);
            dataGridView1.TabIndex = 0;
            // 
            // CreateButton
            // 
            CreateButton.Location = new Point(255, 23);
            CreateButton.Name = "CreateButton";
            CreateButton.Size = new Size(286, 41);
            CreateButton.TabIndex = 1;
            CreateButton.Text = "Создать схему и таблицы";
            CreateButton.UseVisualStyleBackColor = true;
            CreateButton.Click += CreateDataBase_Click;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(255, 79);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(286, 36);
            AddButton.TabIndex = 2;
            AddButton.Text = "Внести данные";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddData_Click;
            // 
            // GetButton
            // 
            GetButton.Location = new Point(255, 136);
            GetButton.Name = "GetButton";
            GetButton.Size = new Size(286, 37);
            GetButton.TabIndex = 3;
            GetButton.Text = "Вывести данные";
            GetButton.UseVisualStyleBackColor = true;
            GetButton.Click += GetData_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(GetButton);
            Controls.Add(AddButton);
            Controls.Add(CreateButton);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button CreateButton;
        private Button AddButton;
        private Button GetButton;
    }
}
