using Npgsql;
using System;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Styles.ApplyFormStyle(this);
            Styles.ApplyButtonStyle(CreateButton);
            Styles.ApplyButtonStyle(AddButton);
            Styles.ApplyButtonStyle(GetButton);
        }

        private void CreateDataBase_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("База данных успешно создана и настроена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void AddData_Click(object sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("Сначала подключитесь к базе данных", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        private void GetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettings.IsConnectionStringSet)
                {
                    MessageBox.Show("Сначала подключитесь к базе данных", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Form3 form3 = new Form3();
                form3.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
    }
}