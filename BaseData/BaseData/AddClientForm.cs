using System;
using System.Windows.Forms;
using System.Drawing;
using Npgsql;

namespace BaseData
{
    public partial class AddClientForm : Form
    {
        private TextBox? txtSurname;
        private TextBox? txtName;
        private TextBox? txtMiddleName;
        private TextBox? txtLocation;
        private TextBox? txtPhone;
        private TextBox? txtEmail;
        private CheckBox? chkConstClient;
        private Button? btnAdd;
        private Button? btnCancel;
        private int? _clientId;
        Log rch = new Log();

        public AddClientForm(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyStyles();
            rch.LogInfo("Форма добавления клиента инициализирована");
        }

        public AddClientForm(int clientId, Log log)
        {
            _clientId = clientId;
            rch = log;
            InitializeComponent();
            if (btnAdd != null) btnAdd.Text = "Сохранить";
            this.Text = "Редактировать клиента";
            ApplyStyles();
            LoadClientData(clientId);
            rch.LogInfo($"Форма редактирования клиента ID {clientId} инициализирована");
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (txtSurname != null) Styles.ApplyTextBoxStyle(txtSurname);
                if (txtName != null) Styles.ApplyTextBoxStyle(txtName);
                if (txtMiddleName != null) Styles.ApplyTextBoxStyle(txtMiddleName);
                if (txtLocation != null) Styles.ApplyTextBoxStyle(txtLocation);
                if (txtPhone != null) Styles.ApplyTextBoxStyle(txtPhone);
                if (txtEmail != null) Styles.ApplyTextBoxStyle(txtEmail);

                if (btnAdd != null) Styles.ApplyButtonStyle(btnAdd);
                if (btnCancel != null) Styles.ApplySecondaryButtonStyle(btnCancel);

                if (chkConstClient != null)
                {
                    chkConstClient.ForeColor = Styles.DarkColor;
                    chkConstClient.Font = new Font(Styles.MainFont, 9F);
                }
                rch.LogInfo("Стили применены успешно");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка применения стилей: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавить клиента";
            this.Size = new System.Drawing.Size(450, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.RowCount = 10;
            mainPanel.ColumnCount = 2;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = Color.Transparent;

            Label titleLabel = new Label()
            {
                Text = "Добавление клиента",
                Font = new Font(Styles.MainFont, 12F, FontStyle.Bold),
                ForeColor = Styles.DarkColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };

            Label lblSurname = new Label() { Text = "Фамилия:", TextAlign = ContentAlignment.MiddleRight };
            txtSurname = new TextBox();

            Label lblName = new Label() { Text = "Имя:*", TextAlign = ContentAlignment.MiddleRight };
            txtName = new TextBox();

            Label lblMiddleName = new Label() { Text = "Отчество:", TextAlign = ContentAlignment.MiddleRight };
            txtMiddleName = new TextBox();

            Label lblLocation = new Label() { Text = "Адрес:", TextAlign = ContentAlignment.MiddleRight };
            txtLocation = new TextBox();

            Label lblPhone = new Label() { Text = "Телефон:", TextAlign = ContentAlignment.MiddleRight };
            txtPhone = new TextBox();

            Label lblEmail = new Label() { Text = "Email:*", TextAlign = ContentAlignment.MiddleRight };
            txtEmail = new TextBox();

            chkConstClient = new CheckBox() { Text = "Постоянный клиент" };

            btnAdd = new Button() { Text = "Добавить", Size = new Size(100, 45) };
            btnAdd.Click += BtnAdd_Click;

            btnCancel = new Button() { Text = "Отмена", Size = new Size(100, 45) };
            btnCancel.Click += (s, e) =>
            {
                rch.LogInfo("Форма добавления клиента закрыта по отмене");
                this.Close();
            };

            Styles.ApplyLabelStyle(lblSurname);
            Styles.ApplyLabelStyle(lblName, true);
            Styles.ApplyLabelStyle(lblMiddleName);
            Styles.ApplyLabelStyle(lblLocation);
            Styles.ApplyLabelStyle(lblPhone);
            Styles.ApplyLabelStyle(lblEmail, true);

            mainPanel.Controls.Add(lblSurname, 0, 0);
            mainPanel.Controls.Add(txtSurname!, 1, 0);
            mainPanel.Controls.Add(lblName, 0, 1);
            mainPanel.Controls.Add(txtName!, 1, 1);
            mainPanel.Controls.Add(lblMiddleName, 0, 2);
            mainPanel.Controls.Add(txtMiddleName!, 1, 2);
            mainPanel.Controls.Add(lblLocation, 0, 3);
            mainPanel.Controls.Add(txtLocation!, 1, 3);
            mainPanel.Controls.Add(lblPhone, 0, 4);
            mainPanel.Controls.Add(txtPhone!, 1, 4);
            mainPanel.Controls.Add(lblEmail, 0, 5);
            mainPanel.Controls.Add(txtEmail!, 1, 5);
            mainPanel.SetColumnSpan(chkConstClient!, 2);
            mainPanel.Controls.Add(chkConstClient!, 0, 6);

            Panel buttonsPanel = new Panel();
            buttonsPanel.Dock = DockStyle.Fill;
            buttonsPanel.BackColor = Color.Transparent;
            mainPanel.SetColumnSpan(buttonsPanel, 2);
            mainPanel.Controls.Add(buttonsPanel, 0, 8);

            buttonsPanel.Controls.Add(btnCancel);
            buttonsPanel.Controls.Add(btnAdd);

            btnAdd.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

            btnAdd.Top = buttonsPanel.Height - btnAdd.Height - 5;
            btnAdd.Left = buttonsPanel.Width - btnAdd.Width - 5;

            btnCancel.Top = buttonsPanel.Height - btnCancel.Height - 5;
            btnCancel.Left = btnAdd.Left - btnCancel.Width - 10;

            for (int i = 0; i < 7; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            }
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            this.Controls.Add(mainPanel);
            this.Controls.Add(titleLabel);

            this.ResumeLayout(false);
            rch.LogInfo("Компоненты формы инициализированы");
        }

        private void LoadClientData(int clientId)
        {
            try
            {
                rch.LogInfo($"Начало загрузки данных клиента ID: {clientId}");
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(
                        "SELECT surname, name, middlename, location, phone, email, constclient FROM clients WHERE id = @id",
                        connection);
                    command.Parameters.AddWithValue("id", clientId);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtSurname!.Text = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        txtName!.Text = reader.GetString(1);
                        txtMiddleName!.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        txtLocation!.Text = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        txtPhone!.Text = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        txtEmail!.Text = reader.GetString(5);
                        chkConstClient!.Checked = reader.GetBoolean(6);
                        rch.LogInfo($"Данные клиента ID {clientId} успешно загружены");
                    }
                    else
                    {
                        rch.LogError($"Клиент с ID {clientId} не найден в базе данных");
                        MessageBox.Show("Клиент не найден");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка загрузки данных клиента ID {clientId}: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки данных клиента: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (_clientId.HasValue)
            {
                rch.LogInfo($"Режим редактирования клиента ID: {_clientId.Value}");
                UpdateClient(_clientId.Value, txtSurname!.Text, txtName!.Text, txtMiddleName!.Text,
                    txtLocation!.Text, txtPhone!.Text, txtEmail!.Text, chkConstClient!.Checked);
            }
            else
            {
                rch.LogInfo("Режим добавления нового клиента");
                AddClient(txtSurname!.Text, txtName!.Text, txtMiddleName!.Text,
                    txtLocation!.Text, txtPhone!.Text, txtEmail!.Text, chkConstClient!.Checked);
            }
        }

        private void AddClient(string surname, string name, string middlename, string location, string phone, string email, bool constClient)
        {
            rch.LogInfo("Начало процедуры добавления клиента");

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                rch.LogWarning("Попытка добавления клиента без обязательных полей (Имя, Email)");
                MessageBox.Show("Заполните обязательные поля (Имя, Email)");
                return;
            }

            if (!IsValidName(name))
            {
                MessageBox.Show("Имя не должно содержать цифр");
                return;
            }

            if (!IsValidEmail(email))
            {
                rch.LogWarning($"Некорректный email адрес: {email}");
                MessageBox.Show("Введите корректный email адрес");
                return;
            }

            if (!string.IsNullOrEmpty(phone) && !IsValidPhone(phone))
            {
                rch.LogWarning($"Некорректный номер телефона: {phone}");
                MessageBox.Show("Введите корректный номер телефона");
                return;
            }

            // Если фамилия пустая, используем пустую строку вместо NULL
            if (string.IsNullOrEmpty(surname))
            {
                surname = "";
            }

            try
            {
                rch.LogInfo($"Добавление клиента: {surname} {name} {middlename}, email: {email}");

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(@"
                INSERT INTO clients (surname, name, middlename, location, phone, email, constclient) 
                VALUES (@surname, @name, @middlename, @location, @phone, @email, @constclient)",
                            connection);

                    // Всегда передаем фамилию как строку (даже пустую)
                    command.Parameters.AddWithValue("surname", surname.Trim());
                    command.Parameters.AddWithValue("name", name.Trim());
                    command.Parameters.AddWithValue("middlename", string.IsNullOrEmpty(middlename) ? (object)DBNull.Value : middlename.Trim());
                    command.Parameters.AddWithValue("location", string.IsNullOrEmpty(location) ? (object)DBNull.Value : location.Trim());
                    command.Parameters.AddWithValue("phone", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone.Trim());
                    command.Parameters.AddWithValue("email", email.Trim().ToLower());
                    command.Parameters.AddWithValue("constclient", constClient);

                    int result = command.ExecuteNonQuery();
                    rch.LogInfo($"Клиент успешно добавлен. Затронуто строк: {result}");
                    MessageBox.Show("Клиент успешно добавлен");
                    rch.LogInfo($"Клиент {name} {surname} (email: {email}) добавлен в базу данных");
                    this.Close();
                }
            }
            catch (NpgsqlException ex) when (ex.SqlState == "23505")
            {
                rch.LogError($"Попытка добавить клиента с существующим email: {email}. Ошибка: {ex.Message}");
                MessageBox.Show("Клиент с таким email уже существует");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка добавления клиента: {ex.Message}");
                MessageBox.Show($"Ошибка добавления клиента: {ex.Message}");
            }
        }

        private void UpdateClient(int clientId, string surname, string name, string middlename,
            string location, string phone, string email, bool constClient)
        {
            rch.LogInfo($"Начало обновления клиента ID: {clientId}");

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                rch.LogWarning($"Попытка обновления клиента ID {clientId} без обязательных полей");
                MessageBox.Show("Заполните обязательные поля (Имя, Email)");
                return;
            }

            if (!IsValidName(name))
            {
                MessageBox.Show("Имя не должно содержать цифр");
                return;
            }

            if (!IsValidEmail(email))
            {
                rch.LogWarning($"Некорректный email адрес при обновлении: {email}");
                MessageBox.Show("Введите корректный email адрес");
                return;
            }

            if (!string.IsNullOrEmpty(phone) && !IsValidPhone(phone))
            {
                rch.LogWarning($"Некорректный номер телефона при обновлении: {phone}");
                MessageBox.Show("Введите корректный номер телефона");
                return;
            }

            // Если фамилия пустая, используем пустую строку вместо NULL
            if (string.IsNullOrEmpty(surname))
            {
                surname = "";
            }

            try
            {
                rch.LogInfo($"Обновление клиента ID {clientId}: {surname} {name} {middlename}, email: {email}");

                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(@"
                UPDATE clients 
                SET surname = @surname, name = @name, middlename = @middlename, 
                    location = @location, phone = @phone, email = @email, constclient = @constclient
                WHERE id = @id", connection);

                    command.Parameters.AddWithValue("id", clientId);
                    // Всегда передаем фамилию как строку (даже пустую)
                    command.Parameters.AddWithValue("surname", surname.Trim());
                    command.Parameters.AddWithValue("name", name.Trim());
                    command.Parameters.AddWithValue("middlename", string.IsNullOrEmpty(middlename) ? (object)DBNull.Value : middlename.Trim());
                    command.Parameters.AddWithValue("location", string.IsNullOrEmpty(location) ? (object)DBNull.Value : location.Trim());
                    command.Parameters.AddWithValue("phone", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone.Trim());
                    command.Parameters.AddWithValue("email", email.Trim().ToLower());
                    command.Parameters.AddWithValue("constclient", constClient);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        rch.LogInfo($"Данные клиента ID {clientId} успешно обновлены. Затронуто строк: {result}");
                        MessageBox.Show("Данные клиента успешно обновлены");
                    }
                    else
                    {
                        rch.LogWarning($"Клиент ID {clientId} не найден для обновления");
                        MessageBox.Show("Клиент не найден");
                    }
                    this.Close();
                }
            }
            catch (NpgsqlException ex) when (ex.SqlState == "23505")
            {
                rch.LogError($"Конфликт email при обновлении клиента ID {clientId}: {email}. Ошибка: {ex.Message}");
                MessageBox.Show("Клиент с таким email уже существует");
            }
            catch (Exception ex)
            {
                rch.LogError($"Ошибка обновления клиента ID {clientId}: {ex.Message}");
                MessageBox.Show($"Ошибка обновления клиента: {ex.Message}");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                bool isValid = addr.Address == email;
                if (!isValid)
                {
                    rch.LogWarning($"Email validation failed for: {email}");
                }
                return isValid;
            }
            catch (Exception ex)
            {
                rch.LogWarning($"Email validation error for {email}: {ex.Message}");
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return true;

                string cleaned = System.Text.RegularExpressions.Regex.Replace(phone, @"[^\d+]", "");
                bool isValid = System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^(\+?\d{10,15})$");

                if (!isValid)
                {
                    rch.LogWarning($"Phone validation failed for: {phone} (cleaned: {cleaned})");
                }
                return isValid;
            }
            catch (Exception ex)
            {
                rch.LogError($"Phone validation error for {phone}: {ex.Message}");
                return false;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            rch.LogInfo($"Форма добавления/редактирования клиента закрыта. Причина: {e.CloseReason}");
            base.OnFormClosed(e);
        }

        // Новый метод проверки имени на цифры
        private bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name)) return true;
            return !System.Text.RegularExpressions.Regex.IsMatch(name, @"\d");
        }
    }
}