using System;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public partial class Form3 : Form
    {
        private TabControl? mainTabControl;
        private TabPage? clientsTabPage;
        private TabPage? goodsTabPage;
        private TabPage? sellsTabPage;

        public Form3()
        {
            InitializeComponent();
            ApplyStyles();
        }

        private void InitializeComponent()
        {
            this.mainTabControl = new TabControl();
            this.clientsTabPage = new TabPage();
            this.goodsTabPage = new TabPage();
            this.sellsTabPage = new TabPage();

            this.mainTabControl.Dock = DockStyle.Fill;
            this.mainTabControl.Controls.Add(this.clientsTabPage);
            this.mainTabControl.Controls.Add(this.goodsTabPage);
            this.mainTabControl.Controls.Add(this.sellsTabPage);
            this.mainTabControl.Location = new Point(0, 0);
            this.mainTabControl.Size = new Size(1000, 700);

            this.clientsTabPage.Text = "Клиенты";
            this.clientsTabPage.Padding = new Padding(10);
            this.clientsTabPage.BackColor = Styles.LightColor;

            this.goodsTabPage.Text = "Товары";
            this.goodsTabPage.Padding = new Padding(10);
            this.goodsTabPage.BackColor = Styles.LightColor;

            this.sellsTabPage.Text = "Продажи";
            this.sellsTabPage.Padding = new Padding(10);
            this.sellsTabPage.BackColor = Styles.LightColor;

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 700);
            this.Controls.Add(this.mainTabControl);
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

                    this.mainTabControl.ItemSize = new Size(120, 35);
                    this.mainTabControl.SizeMode = TabSizeMode.Fixed;

                    foreach (TabPage tabPage in this.mainTabControl.TabPages)
                    {
                        tabPage.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadTabs();
        }

        private void LoadTabs()
        {
            try
            {
                LoadClientsTab();
                LoadGoodsTab();
                LoadSellsTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClientsTab()
        {
            if (this.clientsTabPage == null) return;

            try
            {
                Clients clients = new Clients();
                clients.Dock = DockStyle.Fill;
                this.clientsTabPage.Controls.Clear();
                this.clientsTabPage.Controls.Add(clients);

                if (AppSettings.IsConnectionStringSet)
                {
                    clients.Show(AppSettings.SqlConnection);
                }
                else
                {
                    AddInfoLabelToTab(this.clientsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                }
            }
            catch (Exception ex)
            {
                AddErrorLabelToTab(this.clientsTabPage, $"Ошибка загрузки клиентов: {ex.Message}");
            }
        }

        private void LoadGoodsTab()
        {
            if (this.goodsTabPage == null) return;

            try
            {
                GoodsUC goods = new GoodsUC();
                goods.Dock = DockStyle.Fill;
                this.goodsTabPage.Controls.Clear();
                this.goodsTabPage.Controls.Add(goods);

                if (AppSettings.IsConnectionStringSet)
                {
                    goods.Show(AppSettings.SqlConnection);
                }
                else
                {
                    AddInfoLabelToTab(this.goodsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                }
            }
            catch (Exception ex)
            {
                AddErrorLabelToTab(this.goodsTabPage, $"Ошибка загрузки товаров: {ex.Message}");
            }
        }

        private void LoadSellsTab()
        {
            if (this.sellsTabPage == null) return;

            try
            {
                SellsUC sells = new SellsUC();
                sells.Dock = DockStyle.Fill;
                this.sellsTabPage.Controls.Clear();
                this.sellsTabPage.Controls.Add(sells);

                if (AppSettings.IsConnectionStringSet)
                {
                    sells.LoadData(AppSettings.SqlConnection);
                }
                else
                {
                    AddInfoLabelToTab(this.sellsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                }
            }
            catch (Exception ex)
            {
                AddErrorLabelToTab(this.sellsTabPage, $"Ошибка загрузки продаж: {ex.Message}");
            }
        }

        private void AddErrorLabelToTab(TabPage tabPage, string message)
        {
            Label label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Styles.DangerColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Styles.LightColor
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
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Styles.DarkColor,
                BackColor = Styles.LightColor
            };
            tabPage.Controls.Clear();
            tabPage.Controls.Add(label);
        }
    }
}