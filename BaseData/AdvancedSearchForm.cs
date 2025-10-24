using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BaseData
{
    public partial class AdvancedSearchForm : Form
    {
        private ComboBox tableComboBox;
        private CheckedListBox columnsListBox;
        private TextBox whereTextBox;
        private TextBox orderByTextBox;
        private TextBox groupByTextBox;
        private TextBox havingTextBox;
        private ComboBox aggregateComboBox;
        private ComboBox aggregateColumnComboBox;
        private Button executeButton;
        private Button clearButton;
        private Button selectAllButton;
        private Button deselectAllButton;
        private DataGridView resultsDataGridView;
        private TextBox queryPreviewTextBox;
        private Log rch;
        private Label resultsCountLabel;
        private Label queryTimeLabel;

        public AdvancedSearchForm(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyStyles();
            LoadTableList();
            rch.LogInfo("Конструктор SQL запросов инициализирован");
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (tableComboBox != null) Styles.ApplyComboBoxStyle(tableComboBox);
                if (columnsListBox != null) ApplyCheckedListBoxStyle(columnsListBox);
                if (whereTextBox != null) Styles.ApplyTextBoxStyle(whereTextBox);
                if (orderByTextBox != null) Styles.ApplyTextBoxStyle(orderByTextBox);
                if (groupByTextBox != null) Styles.ApplyTextBoxStyle(groupByTextBox);
                if (havingTextBox != null) Styles.ApplyTextBoxStyle(havingTextBox);
                if (aggregateComboBox != null) Styles.ApplyComboBoxStyle(aggregateComboBox);
                if (aggregateColumnComboBox != null) Styles.ApplyComboBoxStyle(aggregateColumnComboBox);
                if (executeButton != null) Styles.ApplyButtonStyle(executeButton);
                if (clearButton != null) Styles.ApplySecondaryButtonStyle(clearButton);
                if (selectAllButton != null) Styles.ApplySecondaryButtonStyle(selectAllButton);
                if (deselectAllButton != null) Styles.ApplySecondaryButtonStyle(deselectAllButton);
                if (resultsDataGridView != null) Styles.ApplyDataGridViewStyle(resultsDataGridView);
                if (queryPreviewTextBox != null) ApplyQueryPreviewStyle(queryPreviewTextBox);
                if (resultsCountLabel != null) ApplyResultsCountLabelStyle(resultsCountLabel);
                if (queryTimeLabel != null) ApplyQueryTimeLabelStyle(queryTimeLabel);

                rch.LogInfo("Стили применены успешно");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private void ApplyCheckedListBoxStyle(CheckedListBox listBox)
        {
            listBox.BackColor = Color.White;
            listBox.ForeColor = Color.FromArgb(64, 64, 64);
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.Font = new Font("Segoe UI", 9F);
            listBox.CheckOnClick = true;
            listBox.Height = 120;
        }

        private void ApplyQueryPreviewStyle(TextBox textBox)
        {
            textBox.BackColor = Color.LightYellow;
            textBox.ForeColor = Color.DarkBlue;
            textBox.Font = new Font("Consolas", 9F);
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void ApplyResultsCountLabelStyle(Label label)
        {
            label.Font = new Font(Styles.MainFont.FontFamily, 9F, FontStyle.Bold);
            label.ForeColor = Styles.DarkColor;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void ApplyQueryTimeLabelStyle(Label label)
        {
            label.Font = new Font(Styles.MainFont.FontFamily, 8F, FontStyle.Italic);
            label.ForeColor = Color.Gray;
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void LoadTableList()
        {
            try
            {
                tableComboBox.Items.Clear();
                tableComboBox.Items.AddRange(new string[] {
                    MetaInformation.tables[0], // clients
                    MetaInformation.tables[1], // goods
                    MetaInformation.tables[3]  // orders
                });

                if (tableComboBox.Items.Count > 0)
                    tableComboBox.SelectedIndex = 0;

                rch.LogInfo("Список таблиц загружен");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки списка таблиц: {ex.Message}");
            }
        }

        private void UpdateQueryPreview()
        {
            if (queryPreviewTextBox != null)
            {
                queryPreviewTextBox.Text = BuildQuery();
            }
        }

        private string BuildQuery()
        {
            if (tableComboBox?.SelectedItem == null) return string.Empty;

            var sb = new StringBuilder("SELECT ");
            string tableName = tableComboBox.SelectedItem.ToString() ?? "";

            // Обработка агрегатных функций
            string aggregateFunction = aggregateComboBox?.SelectedItem?.ToString() ?? "";
            string aggregateColumn = aggregateColumnComboBox?.SelectedItem?.ToString() ?? "";

            bool hasAggregation = aggregateFunction != "Без агрегации" && !string.IsNullOrEmpty(aggregateColumn);

            if (hasAggregation)
            {
                if (aggregateColumn == "*" && aggregateFunction == "COUNT")
                {
                    sb.Append("COUNT(*)");
                }
                else
                {
                    sb.Append($"{aggregateFunction}({aggregateColumn})");
                }

                // Добавляем обычные столбцы только если они выбраны
                if (columnsListBox != null && columnsListBox.CheckedItems.Count > 0)
                {
                    sb.Append(", ");
                    sb.Append(string.Join(", ", columnsListBox.CheckedItems.Cast<string>()));
                }
            }
            else
            {
                // SELECT columns
                if (columnsListBox == null || columnsListBox.CheckedItems.Count == 0)
                    sb.Append("*");
                else
                    sb.Append(string.Join(", ", columnsListBox.CheckedItems.Cast<string>()));
            }

            // FROM table
            sb.Append($" FROM {tableName}");

            // WHERE clause
            if (!string.IsNullOrEmpty(whereTextBox?.Text))
                sb.Append($" WHERE {whereTextBox.Text}");

            // GROUP BY
            if (!string.IsNullOrEmpty(groupByTextBox?.Text))
                sb.Append($" GROUP BY {groupByTextBox.Text}");

            // HAVING
            if (!string.IsNullOrEmpty(havingTextBox?.Text))
                sb.Append($" HAVING {havingTextBox.Text}");

            // ORDER BY
            if (!string.IsNullOrEmpty(orderByTextBox?.Text))
                sb.Append($" ORDER BY {orderByTextBox.Text}");

            return sb.ToString();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Конструктор SQL запросов";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(15);

            // Создаем и инициализируем все элементы управления
            InitializeControls();

            this.ResumeLayout(false);
        }

        private void InitializeControls()
        {
            // Инициализация всех полей
            tableComboBox = new ComboBox();
            columnsListBox = new CheckedListBox();
            whereTextBox = new TextBox();
            orderByTextBox = new TextBox();
            groupByTextBox = new TextBox();
            havingTextBox = new TextBox();
            aggregateComboBox = new ComboBox();
            aggregateColumnComboBox = new ComboBox();
            executeButton = new Button();
            clearButton = new Button();
            selectAllButton = new Button();
            deselectAllButton = new Button();
            resultsDataGridView = new DataGridView();
            queryPreviewTextBox = new TextBox();
            resultsCountLabel = new Label();
            queryTimeLabel = new Label();

            // Главный контейнер
            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 8;
            mainPanel.ColumnCount = 3;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.Transparent;

            // Заголовок
            Label titleLabel = new Label()
            {
                Text = "Конструктор SQL запросов",
                Font = new Font(Styles.MainFont.FontFamily, 16F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };

            // 1. Выбор таблицы
            Label tableLabel = new Label() { Text = "Таблица:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(tableLabel, true);
            tableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tableComboBox.SelectedIndexChanged += TableComboBox_SelectedIndexChanged;

            // 2. Выбор столбцов
            Label columnsLabel = new Label() { Text = "Столбцы:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(columnsLabel, true);
            columnsListBox.CheckOnClick = true;
            columnsListBox.ItemCheck += (s, e) => UpdateQueryPreview();

            // Кнопки для выбора столбцов
            Panel columnButtonsPanel = new Panel();
            columnButtonsPanel.Height = 35;
            columnButtonsPanel.BackColor = Color.Transparent;
            selectAllButton = new Button() { Text = "Выбрать все", Size = new Size(95, 25) };
            deselectAllButton = new Button() { Text = "Снять все", Size = new Size(95, 25) };
            selectAllButton.Click += (s, e) => SelectAllColumns(true);
            deselectAllButton.Click += (s, e) => SelectAllColumns(false);

            columnButtonsPanel.Controls.Add(selectAllButton);
            columnButtonsPanel.Controls.Add(deselectAllButton);
            deselectAllButton.Location = new Point(selectAllButton.Right + 10, 0);

            // 3. Агрегатные функции
            Label aggregateLabel = new Label() { Text = "Агрегатная функция:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(aggregateLabel);
            aggregateComboBox.Items.AddRange(new string[] { "Без агрегации", "COUNT", "SUM", "AVG", "MAX", "MIN" });
            aggregateComboBox.SelectedIndex = 0;
            aggregateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            aggregateComboBox.SelectedIndexChanged += AggregateComboBox_SelectedIndexChanged;

            aggregateColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            aggregateColumnComboBox.SelectedIndexChanged += (s, e) => UpdateQueryPreview();

            // Условия WHERE
            Label whereLabel = new Label() { Text = "WHERE условие:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(whereLabel);
            whereTextBox.PlaceholderText = "например: price > 100 AND name LIKE '%apple%'";
            whereTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // GROUP BY
            Label groupByLabel = new Label() { Text = "GROUP BY:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(groupByLabel);
            groupByTextBox.PlaceholderText = "столбец1, столбец2...";
            groupByTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // HAVING
            Label havingLabel = new Label() { Text = "HAVING:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(havingLabel);
            havingTextBox.PlaceholderText = "условие для сгруппированных данных";
            havingTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // ORDER BY
            Label orderByLabel = new Label() { Text = "ORDER BY:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(orderByLabel);
            orderByTextBox.PlaceholderText = "столбец1 ASC, столбец2 DESC...";
            orderByTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // Предварительный просмотр запроса
            Label previewLabel = new Label()
            {
                Text = "SQL запрос:",
                Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = 25
            };

            queryPreviewTextBox.Multiline = true;
            queryPreviewTextBox.Height = 60;
            queryPreviewTextBox.ReadOnly = true;
            queryPreviewTextBox.ScrollBars = ScrollBars.Vertical;

            // Кнопки выполнения
            Panel buttonPanel = new Panel();
            buttonPanel.Height = 45;
            buttonPanel.BackColor = Color.Transparent;
            executeButton.Text = "Выполнить";
            executeButton.Size = new Size(100, 35);
            clearButton.Text = "Очистить";
            clearButton.Size = new Size(80, 35);
            executeButton.Click += ExecuteButton_Click;
            clearButton.Click += ClearButton_Click;

            buttonPanel.Controls.Add(executeButton);
            buttonPanel.Controls.Add(clearButton);
            clearButton.Location = new Point(executeButton.Right + 15, 0);

            // Label для отображения количества результатов
            resultsCountLabel.Text = "Результаты: 0 записей";
            resultsCountLabel.Height = 20;
            resultsCountLabel.TextAlign = ContentAlignment.MiddleLeft;

            // Label для отображения времени выполнения
            queryTimeLabel.Text = "Время выполнения: -";
            queryTimeLabel.Height = 18;
            queryTimeLabel.TextAlign = ContentAlignment.MiddleLeft;

            // DataGridView для результатов
            resultsDataGridView.Dock = DockStyle.Fill;
            resultsDataGridView.ReadOnly = true;
            resultsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            resultsDataGridView.AllowUserToAddRows = false;
            resultsDataGridView.AllowUserToDeleteRows = false;
            resultsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            resultsDataGridView.RowHeadersVisible = false;

            // Настройка layout
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));    // Таблица
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));   // Столбцы
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));    // Кнопки столбцов
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));    // Агрегатные функции
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));    // Условия
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));    // Предпросмотр + кнопки
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));    // Статистика
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));    // Результаты

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

            // Добавление элементов на панель
            mainPanel.Controls.Add(tableLabel, 0, 0);
            mainPanel.Controls.Add(tableComboBox, 1, 0);
            mainPanel.SetColumnSpan(tableComboBox, 2);

            mainPanel.Controls.Add(columnsLabel, 0, 1);
            mainPanel.Controls.Add(columnsListBox, 1, 1);
            mainPanel.SetColumnSpan(columnsListBox, 2);

            mainPanel.Controls.Add(columnButtonsPanel, 0, 2);
            mainPanel.SetColumnSpan(columnButtonsPanel, 3);

            mainPanel.Controls.Add(aggregateLabel, 0, 3);
            mainPanel.Controls.Add(aggregateComboBox, 1, 3);
            mainPanel.Controls.Add(aggregateColumnComboBox, 2, 3);

            // Условия в одной строке для компактности
            Panel conditionsPanel = new Panel();
            conditionsPanel.Dock = DockStyle.Fill;
            conditionsPanel.BackColor = Color.Transparent;

            int conditionWidth = 180;
            int conditionSpacing = 10;

            whereLabel.Location = new Point(0, 10);
            whereLabel.Size = new Size(80, 20);
            whereTextBox.Location = new Point(85, 8);
            whereTextBox.Size = new Size(conditionWidth, 25);

            groupByLabel.Location = new Point(whereTextBox.Right + conditionSpacing, 10);
            groupByLabel.Size = new Size(65, 20);
            groupByTextBox.Location = new Point(groupByLabel.Right, 8);
            groupByTextBox.Size = new Size(conditionWidth, 25);

            havingLabel.Location = new Point(groupByTextBox.Right + conditionSpacing, 10);
            havingLabel.Size = new Size(55, 20);
            havingTextBox.Location = new Point(havingLabel.Right, 8);
            havingTextBox.Size = new Size(conditionWidth, 25);

            orderByLabel.Location = new Point(havingTextBox.Right + conditionSpacing, 10);
            orderByLabel.Size = new Size(65, 20);
            orderByTextBox.Location = new Point(orderByLabel.Right, 8);
            orderByTextBox.Size = new Size(conditionWidth, 25);

            conditionsPanel.Controls.AddRange(new Control[] { whereLabel, whereTextBox, groupByLabel, groupByTextBox,
                havingLabel, havingTextBox, orderByLabel, orderByTextBox });

            mainPanel.Controls.Add(conditionsPanel, 0, 4);
            mainPanel.SetColumnSpan(conditionsPanel, 3);

            // Предпросмотр и кнопки
            Panel previewButtonPanel = new Panel();
            previewButtonPanel.Dock = DockStyle.Fill;
            previewButtonPanel.BackColor = Color.Transparent;

            previewLabel.Location = new Point(0, 5);
            previewLabel.Size = new Size(80, 20);
            queryPreviewTextBox.Location = new Point(85, 5);
            queryPreviewTextBox.Size = new Size(400, 50);

            buttonPanel.Location = new Point(queryPreviewTextBox.Right + 15, 10);

            previewButtonPanel.Controls.AddRange(new Control[] { previewLabel, queryPreviewTextBox, buttonPanel });
            mainPanel.Controls.Add(previewButtonPanel, 0, 5);
            mainPanel.SetColumnSpan(previewButtonPanel, 3);

            // Статистика
            Panel statsPanel = new Panel();
            statsPanel.Dock = DockStyle.Fill;
            statsPanel.BackColor = Color.Transparent;

            resultsCountLabel.Location = new Point(0, 0);
            resultsCountLabel.Size = new Size(200, 20);
            queryTimeLabel.Location = new Point(210, 0);
            queryTimeLabel.Size = new Size(200, 20);

            statsPanel.Controls.AddRange(new Control[] { resultsCountLabel, queryTimeLabel });
            mainPanel.Controls.Add(statsPanel, 0, 6);
            mainPanel.SetColumnSpan(statsPanel, 3);

            // Результаты
            mainPanel.Controls.Add(resultsDataGridView, 0, 7);
            mainPanel.SetColumnSpan(resultsDataGridView, 3);

            this.Controls.Add(mainPanel);
            this.Controls.Add(titleLabel);
        }

        private void TableComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadColumnsForSelectedTable();
            UpdateAggregateColumns();
            UpdateQueryPreview();
        }

        private void AggregateComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (aggregateColumnComboBox != null)
            {
                aggregateColumnComboBox.Enabled = aggregateComboBox?.SelectedItem?.ToString() != "Без агрегации";
                if (aggregateColumnComboBox.Enabled && aggregateColumnComboBox.Items.Count > 0)
                {
                    aggregateColumnComboBox.SelectedIndex = 0;
                }
            }
            UpdateQueryPreview();
        }

        private void LoadColumnsForSelectedTable()
        {
            if (tableComboBox?.SelectedItem == null || columnsListBox == null) return;

            try
            {
                columnsListBox.BeginUpdate();
                columnsListBox.Items.Clear();
                string tableName = tableComboBox.SelectedItem.ToString() ?? "";

                string[] columns = tableName switch
                {
                    _ when tableName == MetaInformation.tables[0] => MetaInformation.columnsClients,
                    _ when tableName == MetaInformation.tables[1] => MetaInformation.columnsGoods,
                    _ when tableName == MetaInformation.tables[3] => MetaInformation.columnsOrders,
                    _ => new string[0]
                };

                columnsListBox.Items.AddRange(columns);
                SelectAllColumns(true);
                columnsListBox.EndUpdate();

                rch.LogInfo($"Загружены столбцы для таблицы {tableName}: {columns.Length} столбцов");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки столбцов: {ex.Message}");
            }
        }

        private void UpdateAggregateColumns()
        {
            if (aggregateColumnComboBox == null || columnsListBox == null) return;

            aggregateColumnComboBox.Items.Clear();
            aggregateColumnComboBox.Items.Add("*"); // Для COUNT(*)

            foreach (var item in columnsListBox.Items)
            {
                if (item != null)
                    aggregateColumnComboBox.Items.Add(item.ToString());
            }

            if (aggregateColumnComboBox.Items.Count > 0)
                aggregateColumnComboBox.SelectedIndex = 0;
        }

        private void SelectAllColumns(bool select)
        {
            if (columnsListBox == null) return;

            columnsListBox.BeginUpdate();
            for (int i = 0; i < columnsListBox.Items.Count; i++)
            {
                columnsListBox.SetItemChecked(i, select);
            }
            columnsListBox.EndUpdate();
            UpdateQueryPreview();
        }

        private async void ExecuteButton_Click(object? sender, EventArgs e)
        {
            await ExecuteQueryAsync();
        }

        private async Task ExecuteQueryAsync()
        {
            if (resultsDataGridView == null || resultsCountLabel == null || executeButton == null) return;

            try
            {
                string query = BuildQuery();

                if (string.IsNullOrEmpty(query) || query == "SELECT  FROM ")
                {
                    MessageBox.Show("Сформируйте корректный SQL запрос", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateSQLQuery(query))
                {
                    MessageBox.Show("SQL запрос содержит потенциально опасные конструкции", "Ошибка безопасности",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                rch.LogInfo($"Выполнение SQL запроса: {query}");

                executeButton.Enabled = false;
                executeButton.Text = "Выполняется...";

                var startTime = DateTime.Now;

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));

                        var endTime = DateTime.Now;
                        var executionTime = endTime - startTime;

                        resultsDataGridView.DataSource = dataTable;
                        resultsCountLabel.Text = $"Результаты: {dataTable.Rows.Count} записей";
                        queryTimeLabel.Text = $"Время выполнения: {executionTime.TotalMilliseconds:F0} мс";

                        rch.LogInfo($"Запрос выполнен успешно. Найдено записей: {dataTable.Rows.Count}, время: {executionTime.TotalMilliseconds:F0} мс");

                        FormatNumericColumns();

                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Запрос выполнен успешно, но данные не найдены", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                rch.LogError($"Ошибка выполнения SQL запроса: {ex.Message}");
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка SQL",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка выполнения SQL запроса: {ex.Message}");
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                executeButton.Enabled = true;
                executeButton.Text = "Выполнить запрос";
            }
        }

        private void FormatNumericColumns()
        {
            if (resultsDataGridView?.DataSource is DataTable dataTable)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.DataType == typeof(decimal) || column.DataType == typeof(double) || column.DataType == typeof(float))
                    {
                        if (resultsDataGridView.Columns.Contains(column.ColumnName))
                        {
                            resultsDataGridView.Columns[column.ColumnName].DefaultCellStyle.Format = "N2";
                            resultsDataGridView.Columns[column.ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                    else if (column.DataType == typeof(int) || column.DataType == typeof(long))
                    {
                        if (resultsDataGridView.Columns.Contains(column.ColumnName))
                        {
                            resultsDataGridView.Columns[column.ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                    else if (column.DataType == typeof(DateTime))
                    {
                        if (resultsDataGridView.Columns.Contains(column.ColumnName))
                        {
                            resultsDataGridView.Columns[column.ColumnName].DefaultCellStyle.Format = "dd.MM.yyyy";
                            resultsDataGridView.Columns[column.ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
        }

        private bool ValidateSQLQuery(string query)
        {
            string[] dangerousPatterns = {
                "DROP TABLE", "DELETE FROM", "UPDATE ", "INSERT INTO",
                "CREATE TABLE", "ALTER TABLE", "EXEC ", "EXECUTE ",
                "TRUNCATE TABLE", "CREATE INDEX", "DROP INDEX"
            };

            foreach (var pattern in dangerousPatterns)
            {
                if (query.ToUpper().Contains(pattern))
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            whereTextBox.Text = string.Empty;
            groupByTextBox.Text = string.Empty;
            havingTextBox.Text = string.Empty;
            orderByTextBox.Text = string.Empty;
            aggregateComboBox.SelectedIndex = 0;
            resultsDataGridView.DataSource = null;
            queryPreviewTextBox.Text = string.Empty;
            resultsCountLabel.Text = "Результаты: 0 записей";
            queryTimeLabel.Text = "Время выполнения: -";

            SelectAllColumns(true);
            rch.LogInfo("Форма конструктора запросов очищена");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            rch.LogInfo("Конструктор SQL запросов закрыт");
            base.OnFormClosed(e);
        }
    }
}