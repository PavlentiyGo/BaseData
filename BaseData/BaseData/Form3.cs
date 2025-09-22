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
            TabPage tabPage3 = new TabPage("Продажи");
            tabControl1.TabPages.Add(tabPage3);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(787, 420);
            tabPage2.TabIndex = 1;
            tabPage2.UseVisualStyleBackColor = true;
        }
    }
}
