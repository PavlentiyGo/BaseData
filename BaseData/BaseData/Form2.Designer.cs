namespace BaseData
{
    partial class Form2
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
            CloseButton = new Button();
            PortText = new TextBox();
            BdText = new TextBox();
            IdText = new TextBox();
            PasswordText = new TextBox();
            PortLabel = new Label();
            BdLabel = new Label();
            IdLabel = new Label();
            PasswordLabel = new Label();
            EntryButton = new Button();
            AutoButton = new Button();
            SuspendLayout();
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CloseButton.Location = new Point(350, 350);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(100, 35);
            CloseButton.TabIndex = 6;
            CloseButton.Text = "Закрыть";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += button1_Click;
            // 
            // PortText
            // 
            PortText.Location = new Point(300, 50);
            PortText.Name = "PortText";
            PortText.Size = new Size(150, 23);
            PortText.TabIndex = 1;
            // 
            // BdText
            // 
            BdText.Location = new Point(300, 90);
            BdText.Name = "BdText";
            BdText.Size = new Size(150, 23);
            BdText.TabIndex = 2;
            // 
            // IdText
            // 
            IdText.Location = new Point(300, 130);
            IdText.Name = "IdText";
            IdText.Size = new Size(150, 23);
            IdText.TabIndex = 3;
            // 
            // PasswordText
            // 
            PasswordText.Location = new Point(300, 170);
            PasswordText.Name = "PasswordText";
            PasswordText.PasswordChar = '*';
            PasswordText.Size = new Size(150, 23);
            PasswordText.TabIndex = 4;
            // 
            // PortLabel
            // 
            PortLabel.Location = new Point(200, 50);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(80, 23);
            PortLabel.TabIndex = 6;
            PortLabel.Text = "Порт";
            PortLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // BdLabel
            // 
            BdLabel.Location = new Point(200, 90);
            BdLabel.Name = "BdLabel";
            BdLabel.Size = new Size(80, 23);
            BdLabel.TabIndex = 7;
            BdLabel.Text = "База данных";
            BdLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // IdLabel
            // 
            IdLabel.Location = new Point(200, 130);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(80, 23);
            IdLabel.TabIndex = 8;
            IdLabel.Text = "Пользователь";
            IdLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // PasswordLabel
            // 
            PasswordLabel.Location = new Point(200, 170);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(80, 23);
            PasswordLabel.TabIndex = 9;
            PasswordLabel.Text = "Пароль";
            PasswordLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // EntryButton
            // 
            EntryButton.Location = new Point(350, 220);
            EntryButton.Name = "EntryButton";
            EntryButton.Size = new Size(100, 35);
            EntryButton.TabIndex = 5;
            EntryButton.Text = "Подключиться";
            EntryButton.UseVisualStyleBackColor = true;
            EntryButton.Click += EntryButton_Click;
            // 
            // AutoButton
            // 
            AutoButton.Location = new Point(200, 220);
            AutoButton.Name = "AutoButton";
            AutoButton.Size = new Size(120, 35);
            AutoButton.TabIndex = 0;
            AutoButton.Text = "Автозаполнение";
            AutoButton.UseVisualStyleBackColor = true;
            AutoButton.Click += Auto_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(500, 400);
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
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Подключение к базе данных";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Button CloseButton;
        private TextBox PortText;
        private TextBox BdText;
        private TextBox IdText;
        private TextBox PasswordText;
        private Label PortLabel;
        private Label BdLabel;
        private Label IdLabel;
        private Label PasswordLabel;
        private Button EntryButton;
        private Button AutoButton;
    }
}