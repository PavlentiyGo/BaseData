using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseData
{
    public partial class AdvancedSearchForm : Form
    {
        private ComboBox? tableComboBox;
        private CheckedListBox? columnsListBox;
        private TextBox? whereTextBox;
        private TextBox? orderByTextBox;
        private TextBox? groupByTextBox;
        private TextBox? havingTextBox;
        private ComboBox? aggregateComboBox;
        private ComboBox? aggregateColumnComboBox;
        private Button? executeButton;
        private Button? clearButton;
        private Button? selectAllButton;
        private Button? deselectAllButton;
        private DataGridView? resultsDataGridView;
        private TextBox? queryPreviewTextBox;
        private Log rch;
        private Label? resultsCountLabel;
        private Label? queryTimeLabel;

        public AdvancedSearchForm(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyStyles();
            LoadTableList();
            rch.LogInfo("Форма расширенного поиска инициализирована");
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

                rch.LogInfo("Стили формы расширенного поиска применены успешно");
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Расширенный поиск - конструктор SQL запросов";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            // Главный контейнер
            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 13;
            mainPanel.ColumnCount = 3;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.Transparent;

            // Заголовок
            Label titleLabel = new Label()
            {
                Text = "Конструктор SQL запросов SELECT",
                Font = new Font(Styles.MainFont.FontFamily, 16F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // 1. Выбор таблицы
            Label tableLabel = new Label() { Text = "Таблица:*", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(tableLabel, true);
            tableComboBox = new ComboBox();
            tableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tableComboBox.SelectedIndexChanged += TableComboBox_SelectedIndexChanged!;

            // 2. Выбор столбцов
            Label columnsLabel = new Label() { Text = "Столбцы:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(columnsLabel, true);
            columnsListBox = new CheckedListBox();
            columnsListBox.CheckOnClick = true;
            columnsListBox.ItemCheck += (s, e) => UpdateQueryPreview();

            // Кнопки для выбора столбцов
            Panel columnButtonsPanel = new Panel();
            columnButtonsPanel.Height = 40;
            columnButtonsPanel.BackColor = Color.Transparent;
            selectAllButton = new Button() { Text = "Выбрать все", Size = new Size(100, 30) };
            deselectAllButton = new Button() { Text = "Снять все", Size = new Size(100, 30) };
            selectAllButton.Click += (s, e) => SelectAllColumns(true);
            deselectAllButton.Click += (s, e) => SelectAllColumns(false);

            columnButtonsPanel.Controls.Add(selectAllButton);
            columnButtonsPanel.Controls.Add(deselectAllButton);
            deselectAllButton.Location = new Point(selectAllButton.Right + 10, 0);

            // 3. Агрегатные функции
            Label aggregateLabel = new Label() { Text = "Агрегатная функция:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(aggregateLabel);
            aggregateComboBox = new ComboBox();
            aggregateComboBox.Items.AddRange(new string[] { "Без агрегации", "COUNT", "SUM", "AVG", "MAX", "MIN" });
            aggregateComboBox.SelectedIndex = 0;
            aggregateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            aggregateComboBox.SelectedIndexChanged += AggregateComboBox_SelectedIndexChanged!;

            aggregateColumnComboBox = new ComboBox();
            aggregateColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            aggregateColumnComboBox.SelectedIndexChanged += (s, e) => UpdateQueryPreview();

            // 4. WHERE условие
            Label whereLabel = new Label() { Text = "WHERE (условие фильтрации):", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(whereLabel);
            whereTextBox = new TextBox();
            whereTextBox.Font = new Font("Consolas", 9F);
            whereTextBox.PlaceholderText = "например: price > 100 AND name LIKE '%apple%'";
            whereTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // 5. GROUP BY
            Label groupByLabel = new Label() { Text = "GROUP BY (группировка):", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(groupByLabel);
            groupByTextBox = new TextBox();
            groupByTextBox.Font = new Font("Consolas", 9F);
            groupByTextBox.PlaceholderText = "например: category, supplier";
            groupByTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // 6. HAVING
            Label havingLabel = new Label() { Text = "HAVING (фильтрация групп):", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(havingLabel);
            havingTextBox = new TextBox();
            havingTextBox.Font = new Font("Consolas", 9F);
            havingTextBox.PlaceholderText = "например: COUNT(*) > 5";
            havingTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // 7. ORDER BY
            Label orderByLabel = new Label() { Text = "ORDER BY (сортировка):", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(orderByLabel);
            orderByTextBox = new TextBox();
            orderByTextBox.Font = new Font("Consolas", 9F);
            orderByTextBox.PlaceholderText = "например: name ASC, price DESC";
            orderByTextBox.TextChanged += (s, e) => UpdateQueryPreview();

            // Предварительный просмотр запроса
            Label previewLabel = new Label()
            {
                Text = "Предварительный просмотр SQL запроса:",
                Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = 30
            };

            queryPreviewTextBox = new TextBox()
            {
                Multiline = true,
                Height = 80,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Кнопки выполнения
            Panel buttonPanel = new Panel();
            buttonPanel.Height = 50;
            buttonPanel.BackColor = Color.Transparent;
            executeButton = new Button() { Text = "Выполнить запрос", Size = new Size(150, 40) };
            clearButton = new Button() { Text = "Очистить", Size = new Size(100, 40) };
            executeButton.Click += ExecuteButton_Click!;
            clearButton.Click += ClearButton_Click!;

            buttonPanel.Controls.Add(executeButton);
            buttonPanel.Controls.Add(clearButton);
            clearButton.Location = new Point(executeButton.Right + 20, 0);

            // Label для отображения количества результатов
            resultsCountLabel = new Label()
            {
                Text = "Результаты: 0 записей",
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Label для отображения времени выполнения
            queryTimeLabel = new Label()
            {
                Text = "Время выполнения: -",
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // DataGridView для результатов
            resultsDataGridView = new DataGridView();
            resultsDataGridView.Dock = DockStyle.Fill;
            resultsDataGridView.ReadOnly = true;
            resultsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            resultsDataGridView.AllowUserToAddRows = false;
            resultsDataGridView.AllowUserToDeleteRows = false;
            resultsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            resultsDataGridView.RowHeadersVisible = false;

            // Настройка layout
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Таблица
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); // Столбцы
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Кнопки столбцов
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Агрегатные функции
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // WHERE
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // GROUP BY
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // HAVING
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // ORDER BY
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); // Предпросмотр
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Кнопки выполнения
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F)); // Счетчик результатов
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); // Время выполнения
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Результаты

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

            // Добавление элементов на панель
            mainPanel.Controls.Add(tableLabel, 0, 0);
            mainPanel.Controls.Add(tableComboBox!, 1, 0);
            mainPanel.SetColumnSpan(tableComboBox!, 2);

            mainPanel.Controls.Add(columnsLabel, 0, 1);
            mainPanel.Controls.Add(columnsListBox!, 1, 1);
            mainPanel.SetColumnSpan(columnsListBox!, 2);

            mainPanel.Controls.Add(columnButtonsPanel, 0, 2);
            mainPanel.SetColumnSpan(columnButtonsPanel, 3);

            mainPanel.Controls.Add(aggregateLabel, 0, 3);
            mainPanel.Controls.Add(aggregateComboBox!, 1, 3);
            mainPanel.Controls.Add(aggregateColumnComboBox!, 2, 3);

            mainPanel.Controls.Add(whereLabel, 0, 4);
            mainPanel.Controls.Add(whereTextBox!, 1, 4);
            mainPanel.SetColumnSpan(whereTextBox!, 2);

            mainPanel.Controls.Add(groupByLabel, 0, 5);
            mainPanel.Controls.Add(groupByTextBox!, 1, 5);
            mainPanel.SetColumnSpan(groupByTextBox!, 2);

            mainPanel.Controls.Add(havingLabel, 0, 6);
            mainPanel.Controls.Add(havingTextBox!, 1, 6);
            mainPanel.SetColumnSpan(havingTextBox!, 2);

            mainPanel.Controls.Add(orderByLabel, 0, 7);
            mainPanel.Controls.Add(orderByTextBox!, 1, 7);
            mainPanel.SetColumnSpan(orderByTextBox!, 2);

            mainPanel.Controls.Add(previewLabel, 0, 8);
            mainPanel.Controls.Add(queryPreviewTextBox!, 1, 8);
            mainPanel.SetColumnSpan(queryPreviewTextBox!, 2);

            mainPanel.Controls.Add(buttonPanel, 0, 9);
            mainPanel.SetColumnSpan(buttonPanel, 3);

            mainPanel.Controls.Add(resultsCountLabel!, 0, 10);
            mainPanel.SetColumnSpan(resultsCountLabel!, 3);

            mainPanel.Controls.Add(queryTimeLabel!, 0, 11);
            mainPanel.SetColumnSpan(queryTimeLabel!, 3);

            mainPanel.Controls.Add(resultsDataGridView!, 0, 12);
            mainPanel.SetColumnSpan(resultsDataGridView!, 3);

            this.Controls.Add(mainPanel);
            this.Controls.Add(titleLabel);

            this.ResumeLayout(false);
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

        private void TableComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadColumnsForSelectedTable();
            UpdateAggregateColumns();
            UpdateQueryPreview();
        }

        private void LoadTableList()
        {
            try
            {
                tableComboBox?.Items.Clear();
                tableComboBox?.Items.AddRange(new string[] {
                    MetaInformation.tables[0], // clients
                    MetaInformation.tables[1], // goods
                    MetaInformation.tables[3]  // orders
                });

                if (tableComboBox != null && tableComboBox.Items.Count > 0)
                    tableComboBox.SelectedIndex = 0;

                rch.LogInfo("Список таблиц загружен в конструктор запросов");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки списка таблиц: {ex.Message}");
            }
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
                    aggregateColumnComboBox.Items.Add(item.ToString()!);
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

                // Базовая валидация SQL
                if (!ValidateSQLQuery(query))
                {
                    MessageBox.Show("SQL запрос содержит потенциально опасные конструкции", "Ошибка безопасности",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Валидация условий WHERE/HAVING
                string validationError = ValidateConditions();
                if (!string.IsNullOrEmpty(validationError))
                {
                    MessageBox.Show(validationError, "Ошибка валидации",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        if (queryTimeLabel != null)
                            queryTimeLabel.Text = $"Время выполнения: {executionTime.TotalMilliseconds:F0} мс";

                        rch.LogInfo($"Запрос выполнен успешно. Найдено записей: {dataTable.Rows.Count}, время: {executionTime.TotalMilliseconds:F0} мс");

                        // Автоматическое форматирование числовых столбцов
                        FormatNumericColumns();

                        MessageBox.Show($"Найдено записей: {dataTable.Rows.Count}\nВремя выполнения: {executionTime.TotalMilliseconds:F0} мс", "Результат",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                rch.LogError($"Ошибка выполнения SQL запроса: {ex.Message}");
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}\n\nПроверьте синтаксис условий WHERE/HAVING.", "Ошибка SQL",
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

        private string ValidateConditions()
        {
            // Проверка базового синтаксиса условий WHERE
            if (!string.IsNullOrEmpty(whereTextBox?.Text))
            {
                string whereCondition = whereTextBox.Text.Trim();
                if (whereCondition.StartsWith("AND") || whereCondition.StartsWith("OR") ||
                    whereCondition.EndsWith("AND") || whereCondition.EndsWith("OR"))
                {
                    return "Условие WHERE содержит некорректное использование AND/OR операторов";
                }

                // Проверка на наличие SQL-инъекций в WHERE
                if (ContainsDangerousPatterns(whereCondition))
                {
                    return "Условие WHERE содержит потенциально опасные конструкции";
                }
            }

            // Проверка базового синтаксиса условий HAVING
            if (!string.IsNullOrEmpty(havingTextBox?.Text))
            {
                string havingCondition = havingTextBox.Text.Trim();
                if (havingCondition.StartsWith("AND") || havingCondition.StartsWith("OR") ||
                    havingCondition.EndsWith("AND") || havingCondition.EndsWith("OR"))
                {
                    return "Условие HAVING содержит некорректное использование AND/OR операторов";
                }

                // Проверка на наличие SQL-инъекций в HAVING
                if (ContainsDangerousPatterns(havingCondition))
                {
                    return "Условие HAVING содержит потенциально опасные конструкции";
                }
            }

            // Проверка GROUP BY и ORDER BY
            if (!string.IsNullOrEmpty(groupByTextBox?.Text) && ContainsDangerousPatterns(groupByTextBox.Text))
            {
                return "Условие GROUP BY содержит потенциально опасные конструкции";
            }

            if (!string.IsNullOrEmpty(orderByTextBox?.Text) && ContainsDangerousPatterns(orderByTextBox.Text))
            {
                return "Условие ORDER BY содержит потенциально опасные конструкции";
            }

            return string.Empty;
        }

        private bool ContainsDangerousPatterns(string text)
        {
            string[] dangerousPatterns = {
                "DROP", "DELETE", "UPDATE", "INSERT", "CREATE", "ALTER",
                "EXEC", "EXECUTE", "TRUNCATE", "GRANT", "REVOKE"
            };

            foreach (var pattern in dangerousPatterns)
            {
                if (text.ToUpper().Contains(pattern))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidateSQLQuery(string query)
        {
            // Базовая защита от SQL-инъекций - проверяем опасные конструкции
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
            if (whereTextBox != null) whereTextBox.Text = string.Empty;
            if (groupByTextBox != null) groupByTextBox.Text = string.Empty;
            if (havingTextBox != null) havingTextBox.Text = string.Empty;
            if (orderByTextBox != null) orderByTextBox.Text = string.Empty;
            if (aggregateComboBox != null) aggregateComboBox.SelectedIndex = 0;
            if (resultsDataGridView != null) resultsDataGridView.DataSource = null;
            if (queryPreviewTextBox != null) queryPreviewTextBox.Text = string.Empty;
            if (resultsCountLabel != null) resultsCountLabel.Text = "Результаты: 0 записей";
            if (queryTimeLabel != null) queryTimeLabel.Text = "Время выполнения: -";

            SelectAllColumns(true);
            rch.LogInfo("Форма конструктора запросов очищена");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            rch.LogInfo("Форма расширенного поиска закрыта");
            base.OnFormClosed(e);
        }
    }
}