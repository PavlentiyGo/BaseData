using Npgsql;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaseData
{
    public partial class DeleteConstraints : Form
    {
        private int TabNum;
        private Log rch = new Log();

        public DeleteConstraints(int tabNum)
        {
            TabNum = tabNum;
            InitializeComponent();
            ApplyStyles();
            System.Windows.Forms.ComboBox deleteComboBox = this.DeleteComboBox;
            deleteComboBox.Items.AddRange(MetaInformation.GetConstraintNames(MetaInformation.tables[tabNum]));
            rch.LogInfo($"Форма удаления ограничений для таблицы {MetaInformation.tables[tabNum]} инициализирована");
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (label1 != null)
                {
                    label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                    label1.ForeColor = Styles.DarkColor;
                }

                if (DeleteComboBox != null) Styles.ApplyComboBoxStyle(DeleteComboBox);

                if (DelBtn != null)
                {
                    Styles.ApplyDangerButtonStyle(DelBtn);
                    DelBtn.Font = new Font(Styles.MainFont.FontFamily, 11F, FontStyle.Bold);
                    DelBtn.Size = new Size(150, 50);
                }

                rch.LogInfo("Стили формы удаления ограничений применены успешно");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка применения стилей формы удаления ограничений: {ex.Message}");
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(DeleteComboBox.Text))
                {
                    MessageBox.Show("Выберите ограничение для удаления");
                    rch.LogWarning("Попытка удаления ограничения без выбора");
                    return;
                }

                rch.LogInfo($"Начало удаления ограничения: {DeleteComboBox.Text}");

                using (NpgsqlConnection connect = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connect.Open();
                    string sql;
                    string constraintName = DeleteComboBox.Text;

                    if (constraintName.Contains("not_null"))
                    {
                        string columns = GetCheckClauseByConstraintName(AppSettings.SqlConnection, constraintName);
                        string[] parts = columns.Split(' ');
                        string column = parts[0];
                        sql = $"ALTER TABLE {MetaInformation.tables[TabNum]} ALTER COLUMN {column} DROP NOT NULL";
                        rch.LogInfo($"Удаление NOT NULL ограничения для колонки {column}");
                    }
                    else
                    {
                        sql = $"ALTER TABLE {MetaInformation.tables[TabNum]} DROP CONSTRAINT \"{constraintName}\"";
                        rch.LogInfo($"Удаление ограничения: {constraintName}");
                    }

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connect))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Ограничение удалено");
                rch.LogInfo($"Ограничение {DeleteComboBox.Text} успешно удалено");
                this.Close();
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка удаления ограничения: {ex.Message}");
                MessageBox.Show($"Ошибка удаления ограничения: {ex.Message}");
            }
        }

        private string GetCheckClauseByConstraintName(
            string connectionString,
            string constraintName,
            string schema = "public")
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = @"
                SELECT ch.check_clause
                FROM information_schema.check_constraints ch
                WHERE ch.constraint_schema = @schema
                  AND ch.constraint_name = @constraintName;";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("schema", schema);
            cmd.Parameters.AddWithValue("constraintName", constraintName);

            return cmd.ExecuteScalar() as string;
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.DeleteComboBox = new System.Windows.Forms.ComboBox();
            this.DelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // label1
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            this.label1.Location = new System.Drawing.Point(200, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Удалить ограничения";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // DeleteComboBox
            this.DeleteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeleteComboBox.FormattingEnabled = true;
            this.DeleteComboBox.Location = new System.Drawing.Point(150, 120);
            this.DeleteComboBox.Name = "DeleteComboBox";
            this.DeleteComboBox.Size = new System.Drawing.Size(400, 23);
            this.DeleteComboBox.TabIndex = 2;

            // DelBtn
            this.DelBtn.Location = new System.Drawing.Point(275, 180);
            this.DelBtn.Name = "DelBtn";
            this.DelBtn.Size = new System.Drawing.Size(150, 50);
            this.DelBtn.TabIndex = 3;
            this.DelBtn.Text = "Удалить";
            this.DelBtn.UseVisualStyleBackColor = true;
            this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);

            // DeleteConstraints
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 300);
            this.Controls.Add(this.DelBtn);
            this.Controls.Add(this.DeleteComboBox);
            this.Controls.Add(this.label1);
            this.Name = "DeleteConstraints";
            this.Text = "Удаление ограничений";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DeleteComboBox;
        private System.Windows.Forms.Button DelBtn;
    }
}