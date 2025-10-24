using System.Drawing;
using System.Windows.Forms;

namespace BaseData
{
    public static class Styles
    {
        public static Color LightColor => Color.FromArgb(255, 248, 240);
        public static Color DarkColor => Color.FromArgb(50, 50, 50);
        public static Color DangerColor => Color.FromArgb(231, 76, 60);
        public static Font MainFont => new Font("Segoe UI", 9F, FontStyle.Regular);

        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = LightColor;
            form.Font = MainFont;
        }

        public static void ApplyPrimaryButtonStyle(Button button)
        {
            button.BackColor = Color.White;
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderSize = 1;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyButtonStyle(Button button)
        {
            ApplyPrimaryButtonStyle(button);
        }

        public static void ApplySecondaryButtonStyle(Button button)
        {
            button.BackColor = Color.FromArgb(240, 240, 240);
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderSize = 1;
            button.Font = MainFont;
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyDangerButtonStyle(Button button)
        {
            button.BackColor = DangerColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderSize = 1;
            button.Font = MainFont;
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BackColor = Color.White;
            textBox.ForeColor = DarkColor;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = MainFont;
        }

        public static void ApplyComboBoxStyle(ComboBox comboBox)
        {
            comboBox.BackColor = Color.White;
            comboBox.ForeColor = DarkColor;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = MainFont;
        }

        public static void ApplyDataGridViewStyle(DataGridView dataGridView)
        {
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.FixedSingle;
            dataGridView.Font = MainFont;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = DarkColor;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(MainFont.FontFamily, MainFont.Size, FontStyle.Bold);
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView.ColumnHeadersHeight = 35;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.DefaultCellStyle.Padding = new Padding(3);
            dataGridView.RowTemplate.Height = 30;
        }

        public static void ApplyTabControlStyle(TabControl tabControl)
        {
            tabControl.Appearance = TabAppearance.Normal;
            tabControl.ItemSize = new Size(120, 35);
            tabControl.SizeMode = TabSizeMode.Fixed;
        }

        public static void ApplyLabelStyle(Label label, bool bold = false)
        {
            label.ForeColor = DarkColor;
            label.Font = bold ? new Font(MainFont.FontFamily, MainFont.Size, FontStyle.Bold) : MainFont;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        public static void ApplyDateTimePickerStyle(DateTimePicker dateTimePicker)
        {
            dateTimePicker.BackColor = Color.White;
            dateTimePicker.ForeColor = DarkColor;
            dateTimePicker.Format = DateTimePickerFormat.Short;
            dateTimePicker.Font = MainFont;
        }

        public static void ApplyPanelStyle(Panel panel)
        {
            panel.BackColor = Color.Transparent;
        }
    }
}