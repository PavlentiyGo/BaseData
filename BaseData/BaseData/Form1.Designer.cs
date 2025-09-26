namespace BaseData
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            CreateButton = new Button();
            AddButton = new Button();
            GetButton = new Button();
            SuspendLayout();
            // 
            // CreateButton
            // 
            CreateButton.BackColor = Color.SteelBlue;
            CreateButton.FlatStyle = FlatStyle.Flat;
            CreateButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            CreateButton.ForeColor = Color.White;
            CreateButton.Location = new Point(250, 50);
            CreateButton.Margin = new Padding(3, 4, 3, 4);
            CreateButton.Name = "CreateButton";
            CreateButton.Size = new Size(400, 60);
            CreateButton.TabIndex = 1;
            CreateButton.Text = "Создать схему и таблицы";
            CreateButton.UseVisualStyleBackColor = false;
            CreateButton.Click += CreateDataBase_Click;
            // 
            // AddButton
            // 
            AddButton.BackColor = Color.SteelBlue;
            AddButton.FlatStyle = FlatStyle.Flat;
            AddButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            AddButton.ForeColor = Color.White;
            AddButton.Location = new Point(250, 130);
            AddButton.Margin = new Padding(3, 4, 3, 4);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(400, 60);
            AddButton.TabIndex = 2;
            AddButton.Text = "Внести данные";
            AddButton.UseVisualStyleBackColor = false;
            AddButton.Click += AddData_Click;
            // 
            // GetButton
            // 
            GetButton.BackColor = Color.SteelBlue;
            GetButton.FlatStyle = FlatStyle.Flat;
            GetButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            GetButton.ForeColor = Color.White;
            GetButton.Location = new Point(250, 210);
            GetButton.Margin = new Padding(3, 4, 3, 4);
            GetButton.Name = "GetButton";
            GetButton.Size = new Size(400, 60);
            GetButton.TabIndex = 3;
            GetButton.Text = "Вывести данные";
            GetButton.UseVisualStyleBackColor = false;
            GetButton.Click += GetData_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(900, 400);
            Controls.Add(GetButton);
            Controls.Add(AddButton);
            Controls.Add(CreateButton);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "База данных: Управление";
            Load += Form1_Load;
            ResumeLayout(false);
        }
        #endregion

        private Button CreateButton;
        private Button AddButton;
        private Button GetButton;
    }
}