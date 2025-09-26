using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace BaseData
{
    public class Form3 : Form
    {
        private TabControl mainTabControl = null!;
        private TabPage clientsTabPage = null!;
        private TabPage goodsTabPage = null!;
        private TabPage sellsTabPage = null!;
        private IContainer components = null!;

        public Form3()
        {
            InitializeComponent();
            ApplyStyles();
            this.Load += Form3_Load;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.mainTabControl = new TabControl();
            this.clientsTabPage = new TabPage();
            this.goodsTabPage = new TabPage();
            this.sellsTabPage = new TabPage();

            // mainTabControl
            this.mainTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.mainTabControl.Controls.Add(this.clientsTabPage);
            this.mainTabControl.Controls.Add(this.goodsTabPage);
            this.mainTabControl.Controls.Add(this.sellsTabPage);
            this.mainTabControl.Location = new Point(10, 10);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new Size(780, 430);
            this.mainTabControl.TabIndex = 0;

            // clientsTabPage
            this.clientsTabPage.Location = new Point(4, 24);
            this.clientsTabPage.Name = "clientsTabPage";
            this.clientsTabPage.Padding = new Padding(3);
            this.clientsTabPage.Size = new Size(772, 402);
            this.clientsTabPage.TabIndex = 0;
            this.clientsTabPage.Text = "Клиенты";
            this.clientsTabPage.UseVisualStyleBackColor = true;

            // goodsTabPage
            this.goodsTabPage.Location = new Point(4, 24);
            this.goodsTabPage.Name = "goodsTabPage";
            this.goodsTabPage.Padding = new Padding(3);
            this.goodsTabPage.Size = new Size(772, 402);
            this.goodsTabPage.TabIndex = 1;
            this.goodsTabPage.Text = "Товары";
            this.goodsTabPage.UseVisualStyleBackColor = true;

            // sellsTabPage
            this.sellsTabPage.Location = new Point(4, 24);
            this.sellsTabPage.Name = "sellsTabPage";
            this.sellsTabPage.Padding = new Padding(3);
            this.sellsTabPage.Size = new Size(772, 402);
            this.sellsTabPage.TabIndex = 2;
            this.sellsTabPage.Text = "Продажи";
            this.sellsTabPage.UseVisualStyleBackColor = true;

            // Form3
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.mainTabControl);
            this.Name = "Form3";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Просмотр данных";
        }

        private void ApplyStyles()
        {
            try
            {
                Styles.ApplyFormStyle(this);

                if (this.mainTabControl != null)
                {
                    Styles.ApplyTabControlStyle(this.mainTabControl);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
            }
        }

        private void Form3_Load(object? sender, EventArgs e)
        {
            CreateTabs();
        }

        private void CreateTabs()
        {
            try
            {
                LoadClientsTab();
                LoadGoodsTab();
                LoadSellsTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании вкладок: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClientsTab()
        {
            if (this.clientsTabPage == null) return;

            if (AppSettings.IsConnectionStringSet)
            {
                try
                {
                    Clients clients = new Clients();
                    clients.Dock = DockStyle.Fill;
                    this.clientsTabPage.Controls.Clear();
                    this.clientsTabPage.Controls.Add(clients);
                    clients.Show(AppSettings.SqlConnection);
                }
                catch (Exception ex)
                {
                    AddErrorLabelToTab(this.clientsTabPage, $"Ошибка загрузки клиентов: {ex.Message}");
                }
            }
            else
            {
                AddInfoLabelToTab(this.clientsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
            }
        }

        private void LoadGoodsTab()
        {
            if (this.goodsTabPage == null) return;

            if (AppSettings.IsConnectionStringSet)
            {
                try
                {
                    GoodsUC goods = new GoodsUC();
                    goods.Dock = DockStyle.Fill;
                    this.goodsTabPage.Controls.Clear();
                    this.goodsTabPage.Controls.Add(goods);
                    goods.Show(AppSettings.SqlConnection);
                }
                catch (Exception ex)
                {
                    AddErrorLabelToTab(this.goodsTabPage, $"Ошибка загрузки товаров: {ex.Message}");
                }
            }
            else
            {
                AddInfoLabelToTab(this.goodsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
            }
        }

        private void LoadSellsTab()
        {
            if (this.sellsTabPage == null) return;

            if (AppSettings.IsConnectionStringSet)
            {
                try
                {
                    SellsUC sells = new SellsUC();
                    sells.Dock = DockStyle.Fill;
                    this.sellsTabPage.Controls.Clear();
                    this.sellsTabPage.Controls.Add(sells);
                    sells.LoadData(AppSettings.SqlConnection);
                }
                catch (Exception ex)
                {
                    AddErrorLabelToTab(this.sellsTabPage, $"Ошибка загрузки продаж: {ex.Message}");
                }
            }
            else
            {
                AddInfoLabelToTab(this.sellsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
            }
        }

        private void AddErrorLabelToTab(TabPage tabPage, string message)
        {
            Label label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 10F)
            };
            tabPage.Controls.Clear();
            tabPage.Controls.Add(label);
        }

        private void AddInfoLabelToTab(TabPage tabPage, string message)
        {
            Label label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F)
            };
            tabPage.Controls.Clear();
            tabPage.Controls.Add(label);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}