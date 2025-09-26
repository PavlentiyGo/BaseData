using System;
using System.Windows.Forms;
using Npgsql;

namespace BaseData
{
    public partial class AddClientForm : Form
    {
        public AddClientForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавить клиента";
            this.Size = new System.Drawing.Size(350, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblSurname = new Label() { Text = "Фамилия:", Location = new System.Drawing.Point(10, 15), Size = new System.Drawing.Size(100, 20) };
            TextBox txtSurname = new TextBox() { Location = new System.Drawing.Point(120, 10), Size = new System.Drawing.Size(200, 20) };

            Label lblName = new Label() { Text = "Имя:", Location = new System.Drawing.Point(10, 45), Size = new System.Drawing.Size(100, 20) };
            TextBox txtName = new TextBox() { Location = new System.Drawing.Point(120, 40), Size = new System.Drawing.Size(200, 20) };

            Label lblMiddleName = new Label() { Text = "Отчество:", Location = new System.Drawing.Point(10, 75), Size = new System.Drawing.Size(100, 20) };
            TextBox txtMiddleName = new TextBox() { Location = new System.Drawing.Point(120, 70), Size = new System.Drawing.Size(200, 20) };

            Label lblLocation = new Label() { Text = "Адрес:", Location = new System.Drawing.Point(10, 105), Size = new System.Drawing.Size(100, 20) };
            TextBox txtLocation = new TextBox() { Location = new System.Drawing.Point(120, 100), Size = new System.Drawing.Size(200, 20) };

            Label lblPhone = new Label() { Text = "Телефон:", Location = new System.Drawing.Point(10, 135), Size = new System.Drawing.Size(100, 20) };
            TextBox txtPhone = new TextBox() { Location = new System.Drawing.Point(120, 130), Size = new System.Drawing.Size(200, 20) };

            Label lblEmail = new Label() { Text = "Email:", Location = new System.Drawing.Point(10, 165), Size = new System.Drawing.Size(100, 20) };
            TextBox txtEmail = new TextBox() { Location = new System.Drawing.Point(120, 160), Size = new System.Drawing.Size(200, 20) };

            CheckBox chkConstClient = new CheckBox() { Text = "Постоянный клиент", Location = new System.Drawing.Point(120, 185) };

            Button btnAdd = new Button() { Text = "Добавить", Location = new System.Drawing.Point(120, 220), Size = new System.Drawing.Size(100, 30) };
            btnAdd.Click += (s, e) => AddClient(txtSurname.Text, txtName.Text, txtMiddleName.Text,
                txtLocation.Text, txtPhone.Text, txtEmail.Text, chkConstClient.Checked);

            this.Controls.AddRange(new Control[] { lblSurname, txtSurname, lblName, txtName, lblMiddleName, txtMiddleName,
                lblLocation, txtLocation, lblPhone, txtPhone, lblEmail, txtEmail, chkConstClient, btnAdd });

            this.ResumeLayout(false);
        }

        private void AddClient(string surname, string name, string middlename, string location, string phone, string email, bool constClient)
        {
            if (string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Заполните обязательные поля (Фамилия, Имя, Email)");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(AppSettings.SqlConnection))
                {
                    connection.Open();
                    var command = new NpgsqlCommand(@"
                INSERT INTO clients (surname, name, middlename, location, phone, email, constclient) 
                VALUES (@surname, @name, @middlename, @location, @phone, @email, @constclient)",
                        connection);

                    command.Parameters.AddWithValue("surname", surname);
                    command.Parameters.AddWithValue("name", name);
                    command.Parameters.AddWithValue("middlename", middlename ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("location", location ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("phone", phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("constclient", constClient);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Клиент успешно добавлен");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления клиента: {ex.Message}");
            }
        }
    }
}