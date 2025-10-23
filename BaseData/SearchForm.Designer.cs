namespace BaseData
{
    partial class SearchForm
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
            dataGridView1 = new DataGridView();
            ButtonPannel = new Panel();
            ChoiceBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ButtonPannel.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 121);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(776, 317);
            dataGridView1.TabIndex = 0;
            // 
            // ButtonPannel
            // 
            ButtonPannel.Controls.Add(ChoiceBox);
            ButtonPannel.Location = new Point(12, 12);
            ButtonPannel.Name = "ButtonPannel";
            ButtonPannel.Size = new Size(776, 103);
            ButtonPannel.TabIndex = 2;
            // 
            // ChoiceBox
            // 
            ChoiceBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ChoiceBox.FormattingEnabled = true;
            ChoiceBox.Items.AddRange(new object[] { "LIKE", "~", "~*", "!~", "!~*" });
            ChoiceBox.Location = new Point(3, 38);
            ChoiceBox.Name = "ChoiceBox";
            ChoiceBox.Size = new Size(121, 23);
            ChoiceBox.TabIndex = 0;
            // 
            // SearchForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ButtonPannel);
            Controls.Add(dataGridView1);
            ButtonPannel.Controls.Add(ChoiceBox);
            Name = "SearchForm";
            Text = "SearchForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ButtonPannel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Panel ButtonPannel;
        private ComboBox ChoiceBox;
    }
}