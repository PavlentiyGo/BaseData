namespace BaseData
{
    partial class DeleteConstraints
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
            DeleteComboBox = new ComboBox();
            DelBtn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(251, 9);
            label1.Name = "label1";
            label1.Size = new Size(282, 34);
            label1.TabIndex = 1;
            label1.Text = "Удалить ограничения";
            // 
            // DeleteComboBox
            // 
            DeleteComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DeleteComboBox.FormattingEnabled = true;
            DeleteComboBox.Location = new Point(251, 73);
            DeleteComboBox.Name = "DeleteComboBox";
            DeleteComboBox.Size = new Size(282, 23);
            DeleteComboBox.TabIndex = 2;
            // 
            // DelBtn
            // 
            DelBtn.Location = new Point(329, 125);
            DelBtn.Name = "DelBtn";
            DelBtn.Size = new Size(128, 55);
            DelBtn.TabIndex = 3;
            DelBtn.Text = "Удалить";
            DelBtn.UseVisualStyleBackColor = true;
            DelBtn.Click += DelBtn_Click;
            // 
            // DeleteConstraints
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DelBtn);
            Controls.Add(DeleteComboBox);
            Controls.Add(label1);
            Name = "DeleteConstraints";
            Text = "DeleteConstraints";
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private ComboBox DeleteComboBox;
        private Button DelBtn;
    }
}