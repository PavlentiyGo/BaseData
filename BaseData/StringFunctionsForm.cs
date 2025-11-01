using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BaseData
{
    public partial class StringFunctionsForm : Form
    {
        private ComboBox cmbTables;
        private ComboBox cmbColumns;
        private ComboBox cmbFunctions;
        private TextBox txtParameter1;
        private TextBox txtParameter2;
        private Button btnExecute;
        private DataGridView dataGridView;
        private TextBox txtResult;
        private Button btnApplyToColumn;
        private Label lblParam1;
        private Label lblParam2;

        public StringFunctionsForm()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                // Применяем стили ко всем контролам
                if (cmbTables != null) Styles.ApplyComboBoxStyle(cmbTables);
                if (cmbColumns != null) Styles.ApplyComboBoxStyle(cmbColumns);
                if (cmbFunctions != null) Styles.ApplyComboBoxStyle(cmbFunctions);
                if (txtParameter1 != null) Styles.ApplyTextBoxStyle(txtParameter1);
                if (txtParameter2 != null) Styles.ApplyTextBoxStyle(txtParameter2);
                if (txtResult != null) Styles.ApplyTextBoxStyle(txtResult);

                if (btnExecute != null) Styles.ApplyButtonStyle(btnExecute);
                if (btnApplyToColumn != null) Styles.ApplySecondaryButtonStyle(btnApplyToColumn);

                // Стили для меток
                var labels = this.Controls.OfType<Label>().ToArray();
                foreach (var label in labels)
                {
                    Styles.ApplyLabelStyle(label, false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Функции работы со строками";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Заголовок
            var titleLabel = new Label
            {
                Text = "Функции работы со строками",
                Location = new Point(10, 10),
                Size = new Size(880, 30),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Выбор таблицы
            var lblTable = new Label { Text = "Таблица:", Location = new Point(10, 50), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            cmbTables = new ComboBox { Location = new Point(100, 50), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            // Выбор столбца
            var lblColumn = new Label { Text = "Столбец:", Location = new Point(320, 50), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            cmbColumns = new ComboBox { Location = new Point(410, 50), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            // Выбор функции
            var lblFunction = new Label { Text = "Функция:", Location = new Point(10, 90), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            cmbFunctions = new ComboBox { Location = new Point(100, 90), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            // Параметры функций
            lblParam1 = new Label { Text = "Параметр 1:", Location = new Point(370, 90), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtParameter1 = new TextBox { Location = new Point(460, 90), Width = 120 };

            lblParam2 = new Label { Text = "Параметр 2:", Location = new Point(590, 90), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtParameter2 = new TextBox { Location = new Point(680, 90), Width = 120 };

            // Кнопки
            btnExecute = new Button { Text = "Выполнить", Location = new Point(810, 50), Width = 80, Height = 30 };
            btnApplyToColumn = new Button { Text = "Применить", Location = new Point(810, 90), Width = 80, Height = 30 };

            // DataGridView для результатов
            dataGridView = new DataGridView
            {
                Location = new Point(10, 130),
                Size = new Size(880, 350),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Поле для результата
            var lblResult = new Label { Text = "Результат:", Location = new Point(10, 490), Width = 80, TextAlign = ContentAlignment.MiddleRight };
            txtResult = new TextBox
            {
                Location = new Point(100, 490),
                Size = new Size(790, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Кнопка закрытия
            var btnClose = new Button { Text = "Закрыть", Location = new Point(810, 580), Width = 80, Height = 30 };

            // Добавление контролов на форму
            this.Controls.AddRange(new Control[] {
                titleLabel, lblTable, cmbTables, lblColumn, cmbColumns,
                lblFunction, cmbFunctions, lblParam1, txtParameter1, lblParam2, txtParameter2,
                btnExecute, btnApplyToColumn, dataGridView, lblResult, txtResult, btnClose
            });

            // Заполняем комбобоксы
            cmbFunctions.Items.AddRange(new string[] {
                "UPPER - В верхний регистр",
                "LOWER - В нижний регистр",
                "SUBSTRING - Подстрока",
                "TRIM - Удалить пробелы",
                "LTRIM - Удалить пробелы слева",
                "RTRIM - Удалить пробелы справа",
                "LPAD - Дополнить слева",
                "RPAD - Дополнить справа",
                "CONCAT - Объединить строки",
                "REPLACE - Заменить подстроку",
                "LENGTH - Длина строки",
                "REVERSE - Обратная строка"
            });

            // События
            cmbTables.SelectedIndexChanged += LoadColumns;
            cmbFunctions.SelectedIndexChanged += ToggleParameters;
            btnExecute.Click += ExecuteFunction;
            btnApplyToColumn.Click += ApplyToColumn;
            btnClose.Click += (s, e) => this.Close();

            LoadTables();
            ToggleParameters(null, EventArgs.Empty); // Инициализация видимости параметров
        }

        private void LoadTables()
        {
            try
            {
                cmbTables.Items.Clear();
                // Используем таблицы из вашей базы данных
                cmbTables.Items.AddRange(new string[] { "clients", "goods", "orders" });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблиц: {ex.Message}");
            }
        }

        private void LoadColumns(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null) return;

            cmbColumns.Items.Clear();
            string tableName = cmbTables.SelectedItem.ToString();

            // Столбцы из вашей структуры базы данных
            switch (tableName)
            {
                case "clients":
                    cmbColumns.Items.AddRange(new string[] { "surname", "name", "middlename", "location", "phone", "email" });
                    break;
                case "goods":
                    cmbColumns.Items.AddRange(new string[] { "name", "unit", "currency" });
                    break;
                case "orders":
                    cmbColumns.Items.AddRange(new string[] { "notes" }); // Добавьте текстовые поля если есть
                    break;
            }

            if (cmbColumns.Items.Count > 0)
                cmbColumns.SelectedIndex = 0;
        }

        private void ToggleParameters(object sender, EventArgs e)
        {
            if (cmbFunctions.SelectedItem == null) return;

            string selectedFunction = cmbFunctions.SelectedItem.ToString();

            // Настройка видимости параметров в зависимости от функции
            bool showParam1 = false;
            bool showParam2 = false;

            switch (selectedFunction.Split(' ')[0])
            {
                case "UPPER":
                case "LOWER":
                case "TRIM":
                case "LTRIM":
                case "RTRIM":
                case "LENGTH":
                case "REVERSE":
                    showParam1 = false;
                    showParam2 = false;
                    break;
                case "SUBSTRING":
                    showParam1 = true;
                    showParam2 = true;
                    lblParam1.Text = "Начало:";
                    lblParam2.Text = "Длина:";
                    break;
                case "LPAD":
                case "RPAD":
                    showParam1 = true;
                    showParam2 = true;
                    lblParam1.Text = "Длина:";
                    lblParam2.Text = "Заполнитель:";
                    break;
                case "CONCAT":
                    showParam1 = true;
                    showParam2 = true;
                    lblParam1.Text = "Строка 2:";
                    lblParam2.Text = "Разделитель:";
                    break;
                case "REPLACE":
                    showParam1 = true;
                    showParam2 = true;
                    lblParam1.Text = "Искать:";
                    lblParam2.Text = "Заменить:";
                    break;
            }

            lblParam1.Visible = showParam1;
            txtParameter1.Visible = showParam1;
            lblParam2.Visible = showParam2;
            txtParameter2.Visible = showParam2;
        }

        private void ExecuteFunction(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null || cmbColumns.SelectedItem == null ||
                cmbFunctions.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу, столбец и функцию");
                return;
            }

            if (!AppSettings.connect)
            {
                MessageBox.Show("Сначала подключитесь к базе данных");
                return;
            }

            try
            {
                string tableName = cmbTables.SelectedItem.ToString();
                string columnName = cmbColumns.SelectedItem.ToString();
                string functionName = cmbFunctions.SelectedItem.ToString().Split(' ')[0];

                string sql = BuildSqlQuery(tableName, columnName, functionName);

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        AddParameters(cmd, functionName);

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridView.DataSource = dt;

                            // Показываем результат в текстовом поле
                            if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                            {
                                txtResult.Text = dt.Rows[0][0]?.ToString() ?? "NULL";
                            }
                            else
                            {
                                txtResult.Text = "Нет данных";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения: {ex.Message}");
            }
        }

        private string BuildSqlQuery(string tableName, string columnName, string functionName)
        {
            string baseColumn = $"\"{columnName}\"";
            string baseQuery = $"SELECT {baseColumn} as original, ";

            string functionCall = functionName switch
            {
                "UPPER" => $"UPPER({baseColumn})",
                "LOWER" => $"LOWER({baseColumn})",
                "SUBSTRING" => $"SUBSTRING({baseColumn} FROM @param1 FOR @param2)",
                "TRIM" => $"TRIM({baseColumn})",
                "LTRIM" => $"LTRIM({baseColumn})",
                "RTRIM" => $"RTRIM({baseColumn})",
                "LPAD" => $"LPAD({baseColumn}, @param1, @param2)",
                "RPAD" => $"RPAD({baseColumn}, @param1, @param2)",
                "CONCAT" => $"CONCAT({baseColumn}, @param2, @param1)",
                "REPLACE" => $"REPLACE({baseColumn}, @param1, @param2)",
                "LENGTH" => $"LENGTH({baseColumn})",
                "REVERSE" => $"REVERSE({baseColumn})",
                _ => baseColumn
            };

            return $"{baseQuery} {functionCall} as result FROM \"{tableName}\" WHERE {baseColumn} IS NOT NULL LIMIT 20";
        }

        private void AddParameters(NpgsqlCommand cmd, string functionName)
        {
            switch (functionName)
            {
                case "SUBSTRING":
                    if (int.TryParse(txtParameter1.Text, out int start) &&
                        int.TryParse(txtParameter2.Text, out int length))
                    {
                        cmd.Parameters.AddWithValue("@param1", start);
                        cmd.Parameters.AddWithValue("@param2", length);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@param1", 1);
                        cmd.Parameters.AddWithValue("@param2", 10);
                    }
                    break;
                case "LPAD":
                case "RPAD":
                    if (int.TryParse(txtParameter1.Text, out int totalLength))
                    {
                        cmd.Parameters.AddWithValue("@param1", totalLength);
                        cmd.Parameters.AddWithValue("@param2", txtParameter2.Text ?? " ");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@param1", 20);
                        cmd.Parameters.AddWithValue("@param2", txtParameter2.Text ?? " ");
                    }
                    break;
                case "CONCAT":
                    cmd.Parameters.AddWithValue("@param1", txtParameter1.Text ?? "");
                    cmd.Parameters.AddWithValue("@param2", txtParameter2.Text ?? " - ");
                    break;
                case "REPLACE":
                    cmd.Parameters.AddWithValue("@param1", txtParameter1.Text ?? "");
                    cmd.Parameters.AddWithValue("@param2", txtParameter2.Text ?? "");
                    break;
            }
        }

        private void ApplyToColumn(object sender, EventArgs e)
        {
            MessageBox.Show("Функция массового применения к столбцу находится в разработке", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}