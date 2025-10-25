using System.Drawing;
using System.Windows.Forms;

namespace BaseData
{
    public static class Styles
    {
        public static FontFamily MainFont => new FontFamily("Segoe UI");

        // Бежево-коричневая цветовая схема
        public static Color PrimaryColor => Color.FromArgb(139, 69, 19);  // Коричневый
        public static Color SecondaryColor => Color.FromArgb(245, 222, 179); // Бежевый
        public static Color LightColor => Color.FromArgb(255, 248, 240); // Светло-бежевый
        public static Color DarkColor => Color.FromArgb(101, 67, 33); // Темно-коричневый
        public static Color AccentColor => Color.FromArgb(160, 120, 80); // Средний коричневый

        // Статусные цвета в той же гамме
        public static Color SuccessColor => Color.FromArgb(85, 107, 47);  // Оливковый
        public static Color DangerColor => Color.FromArgb(139, 0, 0);     // Темно-красный

        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = LightColor;
            form.Font = new Font(MainFont, 10F);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Padding = new Padding(20);
        }

        public static void ApplyButtonStyle(Button button)
        {
            button.BackColor = PrimaryColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font(MainFont, 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(15, 10, 15, 10);
            button.MinimumSize = new Size(120, 40);

            // Эффекты при наведении
            button.FlatAppearance.MouseOverBackColor = AccentColor;
            button.FlatAppearance.MouseDownBackColor = DarkColor;
        }

        public static void ApplySecondaryButtonStyle(Button button)
        {
            button.BackColor = SecondaryColor;
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = AccentColor;
            button.Font = new Font(MainFont, 9F, FontStyle.Regular);
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(12, 8, 12, 8);
            button.MinimumSize = new Size(100, 35);
        }

        public static void ApplyDangerButtonStyle(Button button)
        {
            button.BackColor = DangerColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font(MainFont, 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(12, 8, 12, 8);
            button.MinimumSize = new Size(100, 35);
        }

        public static void ApplyDataGridViewStyle(DataGridView dataGrid)
        {
            dataGrid.BackgroundColor = LightColor;
            dataGrid.BorderStyle = BorderStyle.FixedSingle;
            dataGrid.DefaultCellStyle.Font = new Font(MainFont, 9F);
            dataGrid.DefaultCellStyle.BackColor = LightColor;
            dataGrid.DefaultCellStyle.ForeColor = DarkColor;
            dataGrid.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dataGrid.DefaultCellStyle.SelectionForeColor = DarkColor;
            dataGrid.DefaultCellStyle.Padding = new Padding(5);

            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(MainFont, 10F, FontStyle.Bold);
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.RowHeadersVisible = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.GridColor = AccentColor;

            // Alternating row colors
            dataGrid.AlternatingRowsDefaultCellStyle.BackColor = SecondaryColor;
            dataGrid.AlternatingRowsDefaultCellStyle.ForeColor = DarkColor;
        }

        public static void ApplyTabControlStyle(TabControl tabControl)
        {
            tabControl.Font = new Font(MainFont, 10F);
            tabControl.ItemSize = new Size(120, 30);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.Appearance = TabAppearance.Normal;

            // Устанавливаем цвета в соответствии с нашей цветовой схемой
            tabControl.BackColor = LightColor;
            tabControl.ForeColor = DarkColor;

            // Настраиваем стиль вкладок
            foreach (TabPage page in tabControl.TabPages)
            {
                page.BackColor = LightColor;
                page.ForeColor = DarkColor;
                page.Font = new Font(MainFont, 9F);
                page.Padding = new Padding(10);
            }
        }

        public static void ApplyLabelStyle(Label label, bool bold = false)
        {
            label.ForeColor = DarkColor;
            label.Font = new Font(MainFont, 10F, bold ? FontStyle.Bold : FontStyle.Regular);
            label.BackColor = Color.Transparent;
            label.AutoSize = true;
        }

        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BackColor = LightColor;
            textBox.ForeColor = DarkColor;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font(MainFont, 9F);
            textBox.MinimumSize = new Size(150, 25);
        }

        public static void ApplyComboBoxStyle(ComboBox comboBox)
        {
            comboBox.BackColor = LightColor;
            comboBox.ForeColor = DarkColor;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = new Font(MainFont, 9F);
            comboBox.MinimumSize = new Size(150, 25);
        }

        public static void ApplyDateTimePickerStyle(DateTimePicker dateTimePicker)
        {
            dateTimePicker.BackColor = LightColor;
            dateTimePicker.ForeColor = DarkColor;
            dateTimePicker.Font = new Font(MainFont, 9F);
            dateTimePicker.MinimumSize = new Size(150, 25);
        }

        public static void ApplyCheckBoxStyle(CheckBox checkBox)
        {
            checkBox.ForeColor = DarkColor;
            checkBox.Font = new Font(MainFont, 9F);
            checkBox.BackColor = Color.Transparent;
        }
    }
}