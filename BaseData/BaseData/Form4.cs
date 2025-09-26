using System;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Добавление данных";
            this.Size = new System.Drawing.Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;

            Button btnAddClient = new Button() { Text = "Добавить клиента", Location = new System.Drawing.Point(50, 20), Size = new System.Drawing.Size(200, 30) };
            btnAddClient.Click += (s, e) => { new AddClientForm().ShowDialog(); };

            Button btnAddProduct = new Button() { Text = "Добавить товар", Location = new System.Drawing.Point(50, 60), Size = new System.Drawing.Size(200, 30) };
            btnAddProduct.Click += (s, e) => { new AddProductForm().ShowDialog(); };

            Button btnAddSale = new Button() { Text = "Добавить продажу", Location = new System.Drawing.Point(50, 100), Size = new System.Drawing.Size(200, 30) };
            btnAddSale.Click += (s, e) => { new AddSaleForm().ShowDialog(); };

            Button btnClose = new Button() { Text = "Закрыть", Location = new System.Drawing.Point(50, 140), Size = new System.Drawing.Size(200, 30) };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { btnAddClient, btnAddProduct, btnAddSale, btnClose });
            this.ResumeLayout(false);
        }
    }
}