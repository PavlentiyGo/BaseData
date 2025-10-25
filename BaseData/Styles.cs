using System.Drawing;
using System.Windows.Forms;

namespace BaseData
{
    public static class Styles
    {
        // Коричнево-белая цветовая схема
        public static Color LightColor => Color.FromArgb(255, 253, 250); // Очень светлый бежевый
        public static Color DarkColor => Color.FromArgb(101, 67, 33);    // Темно-коричневый
        public static Color AccentColor => Color.FromArgb(160, 120, 80); // Средний коричневый для акцентов
        public static Color DangerColor => Color.FromArgb(192, 57, 43);  // Приглушенный красный

        public static Font MainFont => new Font("Segoe UI", 9F, FontStyle.Regular);

        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = LightColor;
            form.Font = MainFont;
        }

        public static void ApplyPrimaryButtonStyle(Button button)
        {
            button.BackColor = DarkColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = AccentColor;
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
            button.BackColor = Color.FromArgb(245, 240, 235); // Светлый бежевый
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = AccentColor;
            button.FlatAppearance.BorderSize = 1;
            button.Font = MainFont;
            button.Cursor = Cursors.Hand;
        }

        public static void ApplyDangerButtonStyle(Button button)
        {
            button.BackColor = DangerColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(150, 40, 30);
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

            // Заголовки в коричневых тонах
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(210, 180, 140); // Светло-коричневый
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

            // Альтернативные строки с легким коричневым оттенком
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 245, 240);
        }

        public static void ApplyTabControlStyle(TabControl tabControl)
        {
            tabControl.Appearance = TabAppearance.Normal;
            tabControl.ItemSize = new Size(120, 35);
            tabControl.SizeMode = TabSizeMode.Fixed;

            // Используем стандартную отрисовку вместо OwnerDraw
            tabControl.DrawMode = TabDrawMode.Normal;

            // Стили для вкладок в коричневых тонах
            tabControl.BackColor = LightColor;

            // Устанавливаем цвета для разных состояний вкладок
            tabControl.ForeColor = DarkColor;
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

        public static void ApplyGroupBoxStyle(GroupBox groupBox)
        {
            groupBox.ForeColor = DarkColor;
            groupBox.Font = new Font(MainFont.FontFamily, MainFont.Size, FontStyle.Bold);
        }

        public static void ApplyCheckBoxStyle(CheckBox checkBox)
        {
            checkBox.ForeColor = DarkColor;
            checkBox.Font = MainFont;
        }

        // ДОБАВЛЕННЫЕ МЕТОДЫ:

        public static void ApplyCheckedListBoxStyle(CheckedListBox listBox)
        {
            listBox.BackColor = Color.White;
            listBox.ForeColor = DarkColor;
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.Font = MainFont;
            listBox.CheckOnClick = true;
        }

        public static void ApplyListBoxStyle(ListBox listBox)
        {
            listBox.BackColor = Color.White;
            listBox.ForeColor = DarkColor;
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.Font = MainFont;
            listBox.SelectionMode = SelectionMode.One;
        }

        public static void ApplyRichTextBoxStyle(RichTextBox richTextBox)
        {
            richTextBox.BackColor = Color.White;
            richTextBox.ForeColor = DarkColor;
            richTextBox.BorderStyle = BorderStyle.FixedSingle;
            richTextBox.Font = MainFont;
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
        }

        public static void ApplyNumericUpDownStyle(NumericUpDown numericUpDown)
        {
            numericUpDown.BackColor = Color.White;
            numericUpDown.ForeColor = DarkColor;
            numericUpDown.BorderStyle = BorderStyle.FixedSingle;
            numericUpDown.Font = MainFont;
        }

        public static void ApplyProgressBarStyle(ProgressBar progressBar)
        {
            progressBar.BackColor = LightColor;
            progressBar.ForeColor = AccentColor;
        }

        public static void ApplyMenuStripStyle(MenuStrip menuStrip)
        {
            menuStrip.BackColor = LightColor;
            menuStrip.ForeColor = DarkColor;
            menuStrip.Font = MainFont;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new MenuColorTable());
        }

        public static void ApplyToolStripStyle(ToolStrip toolStrip)
        {
            toolStrip.BackColor = LightColor;
            toolStrip.ForeColor = DarkColor;
            toolStrip.Font = MainFont;
            toolStrip.Renderer = new ToolStripProfessionalRenderer(new MenuColorTable());
        }

        public static void ApplyStatusStripStyle(StatusStrip statusStrip)
        {
            statusStrip.BackColor = LightColor;
            statusStrip.ForeColor = DarkColor;
            statusStrip.Font = MainFont;
        }

        public static void ApplyLinkLabelStyle(LinkLabel linkLabel)
        {
            linkLabel.ForeColor = AccentColor;
            linkLabel.Font = MainFont;
            linkLabel.LinkColor = AccentColor;
            linkLabel.VisitedLinkColor = DarkColor;
            linkLabel.ActiveLinkColor = DangerColor;
        }

        public static void ApplyTreeViewStyle(TreeView treeView)
        {
            treeView.BackColor = Color.White;
            treeView.ForeColor = DarkColor;
            treeView.BorderStyle = BorderStyle.FixedSingle;
            treeView.Font = MainFont;
            treeView.LineColor = AccentColor;
        }

        public static void ApplyListViewStyle(ListView listView)
        {
            listView.BackColor = Color.White;
            listView.ForeColor = DarkColor;
            listView.BorderStyle = BorderStyle.FixedSingle;
            listView.Font = MainFont;
        }

        public static void ApplySplitContainerStyle(SplitContainer splitContainer)
        {
            splitContainer.BackColor = LightColor;
            splitContainer.Panel1.BackColor = LightColor;
            splitContainer.Panel2.BackColor = LightColor;
        }

        public static void ApplyTrackBarStyle(TrackBar trackBar)
        {
            trackBar.BackColor = LightColor;
            trackBar.ForeColor = DarkColor;
        }

        public static void ApplyPictureBoxStyle(PictureBox pictureBox)
        {
            pictureBox.BackColor = Color.Transparent;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void ApplyMonthCalendarStyle(MonthCalendar monthCalendar)
        {
            monthCalendar.BackColor = Color.White;
            monthCalendar.ForeColor = DarkColor;
            monthCalendar.TitleBackColor = AccentColor;
            monthCalendar.TitleForeColor = Color.White;
            monthCalendar.TrailingForeColor = Color.Gray;
        }

        public static void ApplyDomainUpDownStyle(DomainUpDown domainUpDown)
        {
            domainUpDown.BackColor = Color.White;
            domainUpDown.ForeColor = DarkColor;
            domainUpDown.BorderStyle = BorderStyle.FixedSingle;
            domainUpDown.Font = MainFont;
        }

        public static void ApplyPropertyGridStyle(PropertyGrid propertyGrid)
        {
            propertyGrid.BackColor = Color.White;
            propertyGrid.ForeColor = DarkColor;
            propertyGrid.ViewBackColor = Color.White;
            propertyGrid.ViewForeColor = DarkColor;
            propertyGrid.HelpBackColor = LightColor;
            propertyGrid.HelpForeColor = DarkColor;
        }

        // Класс для кастомных цветов меню
        private class MenuColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => Styles.LightColor;
            public override Color MenuStripGradientEnd => Styles.LightColor;
            public override Color MenuItemSelected => Styles.AccentColor;
            public override Color MenuItemBorder => Styles.AccentColor;
            public override Color MenuBorder => Styles.DarkColor;
            public override Color MenuItemSelectedGradientBegin => Styles.AccentColor;
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(180, 140, 100);
            public override Color MenuItemPressedGradientBegin => Styles.DarkColor;
            public override Color MenuItemPressedGradientEnd => Styles.DarkColor;
            public override Color ImageMarginGradientBegin => Styles.LightColor;
            public override Color ImageMarginGradientEnd => Styles.LightColor;
            public override Color ImageMarginGradientMiddle => Styles.LightColor;
            public override Color ToolStripDropDownBackground => Styles.LightColor;
            public override Color SeparatorDark => Styles.AccentColor;
            public override Color SeparatorLight => Color.FromArgb(220, 200, 180);
        }
    }
}