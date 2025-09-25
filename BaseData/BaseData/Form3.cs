using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseData
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            CreateTabs();
        }
        private void CreateTabs()
        {
            tabPage1.Text = "Клиенты";
            tabPage2.Text = "Товары";
            tabPage3.Text = "Продажи";
            Clients clients = new Clients();
            clients.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(clients);
            clients.Show(AppSettings.sqlConnection);
        }
    }
}
