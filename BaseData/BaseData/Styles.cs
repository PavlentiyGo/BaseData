using System.Drawing;
using System.Windows.Forms;

namespace BaseData
{
    public static class Styles
    {
        public static FontFamily Fonts => FontFamily.GenericSansSerif;

        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = Color.WhiteSmoke;
            form.Font = new Font(Fonts, 9F);
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        public static void ApplyButtonStyle(Button button)
        {
            button.BackColor = Color.SteelBlue;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font(Fonts, 9F, FontStyle.Bold);
        }

        public static void ApplyDataGridViewStyle(DataGridView dataGrid)
        {
            dataGrid.BackgroundColor = Color.White;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.DefaultCellStyle.Font = new Font(Fonts, 9F);
            dataGrid.ColumnHeadersDefaultCellStyle.Font = new Font(Fonts, 9F, FontStyle.Bold);
            dataGrid.EnableHeadersVisualStyles = false;
            dataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static void ApplyTabControlStyle(TabControl tabControl)
        {
            tabControl.Font = new Font(Fonts, 9F);
        }
    }
}