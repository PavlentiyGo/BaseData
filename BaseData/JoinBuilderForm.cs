using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;

namespace BaseData
{
    public partial class JoinBuilderForm : Form
    {
        private ComboBox cmbFirstTable;
        private ComboBox cmbSecondTable;
        private ComboBox cmbFirstColumn;
        private ComboBox cmbSecondColumn;
        private ComboBox cmbJoinType;
        private Button btnAddJoin;
        private Button btnExecute;
        private Button btnClear;
        private DataGridView dgvResults;
        private ListBox lstJoins;
        private Log rch;

        private List<JoinCondition> joinConditions;

        public JoinBuilderForm(Log log)
        {
            rch = log;
            joinConditions = new List<JoinCondition>();
            InitializeComponent();
            ApplyStyles();
            LoadTables();
            rch.LogInfo("Мастер соединений инициализирован");
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Мастер соединений (JOIN Builder)";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            // Главный контейнер
            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 6;
            mainPanel.ColumnCount = 2;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.Transparent;

            // Заголовок
            Label titleLabel = new Label()
            {
                Text = "Мастер соединений таблиц",
                Font = new Font(Styles.MainFont.FontFamily, 16F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };
            mainPanel.SetColumnSpan(titleLabel, 2);
            mainPanel.Controls.Add(titleLabel, 0, 0);

            // ������ ������ ������
            GroupBox tableSelectionGroup = new GroupBox()
            {
                Text = "Выбор таблиц для соединения",
                Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            mainPanel.SetColumnSpan(tableSelectionGroup, 2);
            mainPanel.Controls.Add(tableSelectionGroup, 0, 1);

            TableLayoutPanel tablePanel = new TableLayoutPanel();
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.RowCount = 2;
            tablePanel.ColumnCount = 4;
            tablePanel.Padding = new Padding(5);

            // Первая таблица
            Label lblFirstTable = new Label() { Text = "Первая таблица:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblFirstTable, true);
            cmbFirstTable = new ComboBox();
            cmbFirstTable.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFirstTable.SelectedIndexChanged += CmbFirstTable_SelectedIndexChanged;

            // Вторая таблица
            Label lblSecondTable = new Label() { Text = "Вторая таблица:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblSecondTable, true);
            cmbSecondTable = new ComboBox();
            cmbSecondTable.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSecondTable.SelectedIndexChanged += CmbSecondTable_SelectedIndexChanged;

            tablePanel.Controls.Add(lblFirstTable, 0, 0);
            tablePanel.Controls.Add(cmbFirstTable, 1, 0);
            tablePanel.Controls.Add(lblSecondTable, 2, 0);
            tablePanel.Controls.Add(cmbSecondTable, 3, 0);

            // Колонки для соединения
            Label lblFirstColumn = new Label() { Text = "Колонка из первой таблицы:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblFirstColumn, true);
            cmbFirstColumn = new ComboBox();
            cmbFirstColumn.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblSecondColumn = new Label() { Text = "Колонка из второй таблицы:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblSecondColumn, true);
            cmbSecondColumn = new ComboBox();
            cmbSecondColumn.DropDownStyle = ComboBoxStyle.DropDownList;



            tablePanel.Controls.Add(lblFirstColumn, 0, 1);
            tablePanel.Controls.Add(cmbFirstColumn, 1, 1);
            tablePanel.Controls.Add(lblSecondColumn, 2, 1);
            tablePanel.Controls.Add(cmbSecondColumn, 3, 1);

            tableSelectionGroup.Controls.Add(tablePanel);

            // Панель типа соединения и кнопок
            GroupBox joinControlGroup = new GroupBox()
            {
                Text = "Управление соединением",
                Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                Height = 50
            };
            mainPanel.SetColumnSpan(joinControlGroup, 2);
            mainPanel.Controls.Add(joinControlGroup, 0, 2);

            TableLayoutPanel controlPanel = new TableLayoutPanel();
            controlPanel.Dock = DockStyle.Fill;
            controlPanel.RowCount = 1;
            controlPanel.ColumnCount = 5;
            controlPanel.Padding = new Padding(10);
            controlPanel.Height = 60;

            Label lblJoinType = new Label() { Text = "Тип соединения:", TextAlign = ContentAlignment.MiddleRight };
            Styles.ApplyLabelStyle(lblJoinType, true);
            cmbJoinType = new ComboBox();
            cmbJoinType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbJoinType.Items.AddRange(new string[] { "INNER JOIN", "LEFT JOIN", "RIGHT JOIN", "FULL JOIN", "CROSS JOIN" });
            cmbJoinType.SelectedIndex = 0;

            btnAddJoin = new Button() { Text = "Добавить условие", Size = new Size(150, 70) };
            btnAddJoin.Click += BtnAddJoin_Click;

            btnExecute = new Button() { Text = "Выполнить запрос", Size = new Size(150, 70) };
            btnExecute.Click += BtnExecute_Click;

            btnClear = new Button() { Text = "Очистить все", Size = new Size(150, 70) };
            btnClear.Click += BtnClear_Click;

            // После создания кнопок добавьте:
            btnAddJoin.Margin = new Padding(10, 5, 10, 5);
            btnExecute.Margin = new Padding(10, 5, 10, 5);
            btnClear.Margin = new Padding(10, 5, 10, 5);

            controlPanel.Controls.Add(lblJoinType, 0, 0);
            controlPanel.Controls.Add(cmbJoinType, 1, 0);
            controlPanel.Controls.Add(btnAddJoin, 2, 0);
            controlPanel.Controls.Add(btnExecute, 3, 0);
            controlPanel.Controls.Add(btnClear, 4, 0);

            // Добавьте выравнивание для элементов в ячейках
            controlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Тип соединения
            controlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // ComboBox
            controlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Добавить условие
            controlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Выполнить
            controlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Очистить

            joinControlGroup.Controls.Add(controlPanel);

            // Список добавленных соединений
            GroupBox joinsListGroup = new GroupBox()
            {
                Text = "Добавленные условия соединения",
                Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            mainPanel.Controls.Add(joinsListGroup, 0, 3);

            lstJoins = new ListBox();
            lstJoins.Dock = DockStyle.Fill;
            lstJoins.Font = new Font(Styles.MainFont.FontFamily, 9F);
            lstJoins.Height = 120;
            joinsListGroup.Controls.Add(lstJoins);

            // Результаты
            GroupBox resultsGroup = new GroupBox()
            {
                Text = "Результаты запроса",
                Font = new Font(Styles.MainFont.FontFamily, 10F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            mainPanel.SetColumnSpan(resultsGroup, 2);
            mainPanel.Controls.Add(resultsGroup, 0, 4);

            dgvResults = new DataGridView();
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.ReadOnly = true;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvResults.Height = 300;
            resultsGroup.Controls.Add(dgvResults);

            // Кнопка закрытия
            Panel bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Fill;
            bottomPanel.Height = 50;
            bottomPanel.BackColor = Color.Transparent;
            mainPanel.SetColumnSpan(bottomPanel, 2);
            mainPanel.Controls.Add(bottomPanel, 0, 5);

            Button btnClose = new Button() { Text = "Закрыть", Size = new Size(100, 35) };
            btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnClose.Click += (s, e) => this.Close();

            bottomPanel.Controls.Add(btnClose);

            // Настройка строк и колонок главной панели
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // // Заголовок
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); // Выбор таблиц
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));  // Управление
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F)); // Список соединений
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Результаты
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));  // Кнопка закрытия

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                // Применяем стили к элементам управления
                if (cmbFirstTable != null) Styles.ApplyComboBoxStyle(cmbFirstTable);
                if (cmbSecondTable != null) Styles.ApplyComboBoxStyle(cmbSecondTable);
                if (cmbFirstColumn != null) Styles.ApplyComboBoxStyle(cmbFirstColumn);
                if (cmbSecondColumn != null) Styles.ApplyComboBoxStyle(cmbSecondColumn);
                if (cmbJoinType != null) Styles.ApplyComboBoxStyle(cmbJoinType);

                if (btnAddJoin != null) Styles.ApplySecondaryButtonStyle(btnAddJoin);
                if (btnExecute != null) Styles.ApplyButtonStyle(btnExecute);
                if (btnClear != null) Styles.ApplyDangerButtonStyle(btnClear);

                if (dgvResults != null)
                {
                    Styles.ApplyDataGridViewStyle(dgvResults);
                }

                rch.LogInfo("Стили мастера соединений применены успешно");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка применения стилей мастера соединений: {ex.Message}");
            }
        }

        private void LoadTables()
        {
            try
            {
                cmbFirstTable.Items.Clear();
                cmbSecondTable.Items.Clear();

                // Загружаем названия таблиц из MetaInformation
                foreach (string table in MetaInformation.tables)
                {
                    if (!string.IsNullOrEmpty(table))
                    {
                        cmbFirstTable.Items.Add(table);
                        cmbSecondTable.Items.Add(table);
                    }
                }

                if (cmbFirstTable.Items.Count > 0) cmbFirstTable.SelectedIndex = 0;
                if (cmbSecondTable.Items.Count > 1) cmbSecondTable.SelectedIndex = 1;

                rch.LogInfo("Списки таблиц загружены успешно");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки таблиц: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки таблиц: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbFirstTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadColumnsForTable(cmbFirstTable.SelectedItem?.ToString(), cmbFirstColumn);
        }

        private void CmbSecondTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadColumnsForTable(cmbSecondTable.SelectedItem?.ToString(), cmbSecondColumn);
        }

        private void LoadColumnsForTable(string tableName, ComboBox comboBox)
        {
            if (string.IsNullOrEmpty(tableName) || comboBox == null) return;

            try
            {
                comboBox.Items.Clear();

                string[] columns = GetTableColumns(tableName);
                if (columns != null)
                {
                    comboBox.Items.AddRange(columns);
                    if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0;
                }

                rch.LogInfo($"Колонки для таблицы {tableName} загружены: {columns?.Length ?? 0} колонок");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки колонок для таблицы {tableName}: {ex.Message}");
            }
        }

        private string[] GetTableColumns(string tableName)
        {
            if (tableName == null) return null;

            try
            {
                // Сначала пробуем получить из MetaInformation
                if (tableName.Equals(MetaInformation.tables[0], StringComparison.OrdinalIgnoreCase))
                    return MetaInformation.columnsClients;
                else if (tableName.Equals(MetaInformation.tables[1], StringComparison.OrdinalIgnoreCase))
                    return MetaInformation.columnsGoods;
                else if (tableName.Equals(MetaInformation.tables[3], StringComparison.OrdinalIgnoreCase))
                    return MetaInformation.columnsOrders;
                else
                    return GetColumnsFromDatabase(tableName);
            }
            catch
            {
                return GetColumnsFromDatabase(tableName);
            }
        }

        private string[] GetColumnsFromDatabase(string tableName)
        {
            var columns = new List<string>();

            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(
                        "SELECT column_name FROM information_schema.columns " +
                        "WHERE table_name = @tableName AND table_schema = 'public' " +
                        "ORDER BY ordinal_position",
                        connection);
                    command.Parameters.AddWithValue("tableName", tableName.ToLower());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка получения колонок из БД для таблицы {tableName}: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки колонок для таблицы {tableName}: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return columns.ToArray();
        }

        private void BtnAddJoin_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedJoinType = cmbJoinType.SelectedItem?.ToString();
                bool isCrossJoin = selectedJoinType == "CROSS JOIN";

                // Для CROSS JOIN не требуются колонки для соединения
                if (!isCrossJoin && (cmbFirstTable.SelectedItem == null || cmbSecondTable.SelectedItem == null ||
                    cmbFirstColumn.SelectedItem == null || cmbSecondColumn.SelectedItem == null))
                {
                    MessageBox.Show("Заполните все поля для добавления условия соединения", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Для обычных JOIN требуются все поля
                if (isCrossJoin && (cmbFirstTable.SelectedItem == null || cmbSecondTable.SelectedItem == null))
                {
                    MessageBox.Show("Выберите таблицы для CROSS JOIN", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var joinCondition = new JoinCondition
                {
                    FirstTable = cmbFirstTable.SelectedItem.ToString(),
                    SecondTable = cmbSecondTable.SelectedItem.ToString(),
                    FirstColumn = isCrossJoin ? null : cmbFirstColumn.SelectedItem?.ToString(),
                    SecondColumn = isCrossJoin ? null : cmbSecondColumn.SelectedItem?.ToString(),
                    JoinType = selectedJoinType
                };

                joinConditions.Add(joinCondition);
                UpdateJoinsList();

                rch.LogInfo($"Добавлено условие соединения: {joinCondition}");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка добавления условия соединения: {ex.Message}");
                MessageBox.Show($"Ошибка добавления условия соединения: {ex.Message}", "ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (joinConditions.Count == 0)
                {
                    MessageBox.Show("Добавьте хотя бы одно условие соединения", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ДОБАВЛЕННАЯ ЛОГИКА ВЫПОЛНЕНИЯ ЗАПРОСА:
                string query = BuildJoinQuery();
                rch.LogInfo($"Выполнение JOIN запроса: {query}");

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    using (var adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvResults.DataSource = dataTable;
                        ApplyResultGridStyles();

                        rch.LogInfo($"Запрос выполнен успешно. Получено строк: {dataTable.Rows.Count}");
                        MessageBox.Show($"Запрос выполнен успешно. Найдено записей: {dataTable.Rows.Count}",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка выполнения JOIN запроса: {ex.Message}");
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private string BuildJoinQuery()
        {
            if (joinConditions.Count == 0) return string.Empty;

            var firstCondition = joinConditions[0];
            string query;

            if (firstCondition.JoinType == "CROSS JOIN")
            {
                query = $"SELECT * FROM {firstCondition.FirstTable} CROSS JOIN {firstCondition.SecondTable}";
            }
            else
            {
                query = $"SELECT * FROM {firstCondition.FirstTable} " + $"{firstCondition.JoinType} {firstCondition.SecondTable} " + $"ON {firstCondition.FirstTable}.{firstCondition.FirstColumn} = " + $"{firstCondition.SecondTable}.{firstCondition.SecondColumn}";
            }

            for (int i = 1; i < joinConditions.Count; i++)
            {
                var condition = joinConditions[i];

                if (condition.JoinType == "CROSS JOIN")
                {
                    query += $" CROSS JOIN {condition.SecondTable}";
                }
                else
                {
                    query += $" {condition.JoinType} {condition.SecondTable} " + $"ON {condition.FirstTable}.{condition.FirstColumn} = " + $"{condition.SecondTable}.{condition.SecondColumn}";
                }
            }

            return query;
        }


        private void BtnClear_Click(object sender, EventArgs e)
        {
            joinConditions.Clear();
            UpdateJoinsList();
            dgvResults.DataSource = null;
            rch.LogInfo("Все условия соединения очищены");
        }

        private void UpdateJoinsList()
        {
            lstJoins.Items.Clear();
            foreach (var condition in joinConditions)
            {
                lstJoins.Items.Add(condition.ToString());
            }
        }


        private void ApplyResultGridStyles()
        {
            try
            {
                foreach (DataGridViewColumn column in dgvResults.Columns)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.Padding = new Padding(3);
                }

                dgvResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка применения стилей к результатам: {ex.Message}");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            rch.LogInfo($"Мастер соединений закрыт. Причина: {e.CloseReason}");
            base.OnFormClosed(e);
        }
    }

}