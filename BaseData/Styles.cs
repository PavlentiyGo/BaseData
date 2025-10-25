using System.Drawing;
using System.Windows.Forms;

namespace BaseData
{
    public static class Styles
    {
<<<<<<< HEAD
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
=======
        // Коричнево-белая цветовая схема
        public static Color LightColor => Color.FromArgb(255, 253, 250); // Очень светлый бежевый
        public static Color DarkColor => Color.FromArgb(101, 67, 33);    // Темно-коричневый
        public static Color AccentColor => Color.FromArgb(160, 120, 80); // Средний коричневый для акцентов
        public static Color DangerColor => Color.FromArgb(192, 57, 43);  // Приглушенный красный

        public static Font MainFont => new Font("Segoe UI", 9F, FontStyle.Regular);
>>>>>>> adb761fe54cc026d213160796dd6ff83edca6e21

        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = LightColor;
<<<<<<< HEAD
            form.Font = new Font(MainFont, 10F);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Padding = new Padding(20);
=======
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
>>>>>>> adb761fe54cc026d213160796dd6ff83edca6e21
        }

        public static void ApplyButtonStyle(Button button)
        {
            button.BackColor = PrimaryColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font(MainFont, 10F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(15, 10, 15, 10);
            button.MinimumSize = new Size(120, 40);

            // Эффекты при наведении
            button.FlatAppearance.MouseOverBackColor = AccentColor;
            button.FlatAppearance.MouseDownBackColor = DarkColor;
        }

        public static void ApplySecondaryButtonStyle(Button button)
        {
<<<<<<< HEAD
            button.BackColor = SecondaryColor;
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
=======
            button.BackColor = Color.FromArgb(245, 240, 235); // Светлый бежевый
            button.ForeColor = DarkColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = AccentColor;
>>>>>>> adb761fe54cc026d213160796dd6ff83edca6e21
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
<<<<<<< HEAD
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font(MainFont, 9F, FontStyle.Bold);
=======
            button.FlatAppearance.BorderColor = Color.FromArgb(150, 40, 30);
            button.FlatAppearance.BorderSize = 1;
            button.Font = MainFont;
>>>>>>> adb761fe54cc026d213160796dd6ff83edca6e21
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

<<<<<<< HEAD
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.RowHeadersVisible = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.GridColor = AccentColor;

            // Alternating row colors
            dataGrid.AlternatingRowsDefaultCellStyle.BackColor = SecondaryColor;
            dataGrid.AlternatingRowsDefaultCellStyle.ForeColor = DarkColor;
=======
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
>>>>>>> adb761fe54cc026d213160796dd6ff83edca6e21
        }

        public static void ApplyTabControlStyle(TabControl tabControl)
        {
            tabControl.Font = new Font(MainFont, 10F);
            tabControl.ItemSize = new Size(120, 30);
            tabControl.SizeMode = TabSizeMode.Fixed;
<<<<<<< HEAD
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
=======

            // Используем стандартную отрисовку вместо OwnerDraw
            tabControl.DrawMode = TabDrawMode.Normal;

            // Стили для вкладок в коричневых тонах
            tabControl.BackColor = LightColor;

            // Устанавливаем цвета для разных состояний вкладок
            tabControl.ForeColor = DarkColor;
>>>>>>> adb761fe54cc026d213160796dd6ff83edca6e21
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