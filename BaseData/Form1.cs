using System;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form1 : Form
    {
        Log rch = new Log();
        public Form1()
        {
            rch = InitializeComponent();
            ApplyStyles();

            // �������� ������� � �������
            CreateButton.Click += CreateButton_Click;
            AddButton.Click += AddButton_Click;
            GetButton.Click += GetButton_Click;
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"������ ���������� ������: {ex.Message}");
                rch.LogError($"������ ���������� ������: {ex.Message}");
            }
        }

        /// <summary>
        /// ������� ����� � ������� (Form2)
        /// </summary>
        private void CreateButton_Click(object? sender, EventArgs e)
        {
            using (Form2 form2 = new Form2(rch))
            {
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("����� � ������� ������� �����������",
                        "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rch.LogInfo("����� � ������� ������� �����������");
                }
            }
        }

        /// <summary>
        /// ������ ������ (Form4)
        /// </summary>
        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("������� ������������ � ���� ������", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rch.LogError("������� ������������ � ���� ������");
                return;
            }

            using (Form4 form4 = new Form4(rch))
            {
                form4.ShowDialog();
            }
        }

        /// <summary>
        /// �������� ������ (Form3) - ����������
        /// </summary>
        private void GetButton_Click(object? sender, EventArgs e)
        {
            if (!AppSettings.IsConnectionStringSet)
            {
                MessageBox.Show("������� ������������ � ���� ������", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rch.LogError("������� ������������ � ���� ������");
                return;
            }

            Form3 form3 = new Form3(rch);
            form3.ShowDialog();
            form3.Dispose();
        }
    }
}