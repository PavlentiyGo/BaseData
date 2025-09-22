namespace BaseData
{
    partial class Form2
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
            CloseButton = new Button();
            PortText = new TextBox();
            BdText = new TextBox();
            IdText = new TextBox();
            PasswordText = new TextBox();
            dataGridView1 = new DataGridView();
            PortLabel = new Label();
            BdLabel = new Label();
            IdLabel = new Label();
            PasswordLabel = new Label();
            EntryButton = new Button();
            AutoButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // CloseButton
            // 
            CloseButton.Location = new Point(352, 403);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 1;
            CloseButton.Text = "Закрыть";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += button1_Click;
            // 
            // PortText
            // 
            PortText.Location = new Point(341, 57);
            PortText.Name = "PortText";
            PortText.Size = new Size(100, 23);
            PortText.TabIndex = 2;
            // 
            // BdText
            // 
            BdText.Location = new Point(341, 100);
            BdText.Name = "BdText";
            BdText.Size = new Size(100, 23);
            BdText.TabIndex = 3;
            // 
            // IdText
            // 
            IdText.Location = new Point(341, 141);
            IdText.Name = "IdText";
            IdText.Size = new Size(100, 23);
            IdText.TabIndex = 4;
            // 
            // PasswordText
            // 
            PasswordText.Location = new Point(341, 180);
            PasswordText.Name = "PasswordText";
            PasswordText.Size = new Size(100, 23);
            PasswordText.TabIndex = 5;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(-2, -5);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(805, 452);
            dataGridView1.TabIndex = 0;
            // 
            // PortLabel
            // 
            PortLabel.Location = new Point(195, 56);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(100, 23);
            PortLabel.TabIndex = 6;
            PortLabel.Text = "Порт";
            PortLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BdLabel
            // 
            BdLabel.Location = new Point(195, 100);
            BdLabel.Name = "BdLabel";
            BdLabel.Size = new Size(100, 23);
            BdLabel.TabIndex = 7;
            BdLabel.Text = "Название БД";
            BdLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // IdLabel
            // 
            IdLabel.Location = new Point(195, 141);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(100, 23);
            IdLabel.TabIndex = 8;
            IdLabel.Text = "Айди";
            IdLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PasswordLabel
            // 
            PasswordLabel.Location = new Point(195, 180);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(100, 23);
            PasswordLabel.TabIndex = 9;
            PasswordLabel.Text = "Пароль";
            PasswordLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // EntryButton
            // 
            EntryButton.Location = new Point(352, 267);
            EntryButton.Name = "EntryButton";
            EntryButton.Size = new Size(75, 23);
            EntryButton.TabIndex = 10;
            EntryButton.Text = "Войти";
            EntryButton.UseVisualStyleBackColor = true;
            EntryButton.Click += EntryButton_Click;
            // 
            // AutoButton
            // 
            AutoButton.Location = new Point(195, 267);
            AutoButton.Name = "AutoButton";
            AutoButton.Size = new Size(100, 42);
            AutoButton.TabIndex = 11;
            AutoButton.Text = "Поставить автоматически";
            AutoButton.UseVisualStyleBackColor = true;
            AutoButton.Click += Auto_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(AutoButton);
            Controls.Add(EntryButton);
            Controls.Add(PasswordLabel);
            Controls.Add(IdLabel);
            Controls.Add(BdLabel);
            Controls.Add(PortLabel);
            Controls.Add(PasswordText);
            Controls.Add(IdText);
            Controls.Add(BdText);
            Controls.Add(PortText);
            Controls.Add(CloseButton);
            Controls.Add(dataGridView1);
            Name = "Form2";
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button CloseButton;
        private TextBox PortText;
        private TextBox BdText;
        private TextBox IdText;
        private TextBox PasswordText;
        private DataGridView dataGridView1;
        private Label PortLabel;
        private Label BdLabel;
        private Label IdLabel;
        private Label PasswordLabel;
        private Button EntryButton;
        private Button AutoButton;
    }
}