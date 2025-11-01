using System;
using System.Windows.Forms;
using System.Drawing;

namespace BaseData
{
    public partial class Form3 : Form
    {
        private TabControl? mainTabControl;
        private static TabPage? clientsTabPage;
        private static TabPage? goodsTabPage;
        private static TabPage? ordersTabPage;
        Log rch = new Log();

        public Form3(Log log)
        {
            rch = log;
            InitializeComponent();
            ApplyStyles();
        }

        private void InitializeComponent()
        {
            this.mainTabControl = new TabControl();
            clientsTabPage = new TabPage();
            goodsTabPage = new TabPage();
            ordersTabPage = new TabPage();

            this.mainTabControl.Dock = DockStyle.Fill;
            this.mainTabControl.Controls.Add(clientsTabPage);
            this.mainTabControl.Controls.Add(goodsTabPage);
            this.mainTabControl.Controls.Add(ordersTabPage);
            this.mainTabControl.Location = new Point(0, 0);
            this.mainTabControl.Size = new Size(1000, 700);

            // Устанавливаем фиксированные названия вкладок
            clientsTabPage.Text = MetaInformation.tables[0];
            clientsTabPage.Padding = new Padding(10);
            clientsTabPage.BackColor = Styles.LightColor;

            goodsTabPage.Text = MetaInformation.tables[1];
            goodsTabPage.Padding = new Padding(10);
            goodsTabPage.BackColor = Styles.LightColor;

            ordersTabPage.Text = MetaInformation.tables[3];
            ordersTabPage.Padding = new Padding(10);
            ordersTabPage.BackColor = Styles.LightColor;

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
                    // Используем стандартный режим отрисовки вместо OwnerDraw
                    Styles.ApplyTabControlStyle(this.mainTabControl);

                    this.mainTabControl.ItemSize = new Size(120, 35);
                    this.mainTabControl.SizeMode = TabSizeMode.Fixed;

                    // Убедимся, что используется стандартная отрисовка
                    this.mainTabControl.DrawMode = TabDrawMode.Normal;

                    foreach (TabPage tabPage in this.mainTabControl.TabPages)
                    {
                        tabPage.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                        // Явно устанавливаем цвет текста для вкладок
                        tabPage.ForeColor = Styles.DarkColor;
                    }

                    // Устанавливаем цвет текста для активной вкладки
                    this.mainTabControl.SelectedTab?.SetForeColor(Styles.DarkColor);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка применения стилей: {ex.Message}");
                rch.LogError($"Ошибка применения стилей: {ex.Message}");
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
                LoadOrdersTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rch.LogError($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void LoadClientsTab()
        {
            if (clientsTabPage == null) return;

            try
            {
                Clients clients = new Clients(rch);
                clients.Dock = DockStyle.Fill;
                clientsTabPage.Controls.Clear();
                clientsTabPage.Controls.Add(clients);

                if (AppSettings.IsConnectionStringSet)
                {
                    clients.Show(AppSettings.SqlConnection);
                }
                else
                {
                    AddInfoLabelToTab(clientsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                    rch.LogInfo("Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                }
            }
            catch (Exception ex)
            {
                AddErrorLabelToTab(clientsTabPage, $"Ошибка загрузки клиентов: {ex.Message}");
                rch.LogError($"Ошибка загрузки клиентов: {ex.Message}");
            }
        }

        private void LoadGoodsTab()
        {
            if (goodsTabPage == null) return;

            try
            {
                GoodsUC goods = new GoodsUC(rch);
                goods.Dock = DockStyle.Fill;
                goodsTabPage.Controls.Clear();
                goodsTabPage.Controls.Add(goods);

                if (AppSettings.IsConnectionStringSet)
                {
                    goods.Show(AppSettings.SqlConnection);
                }
                else
                {
                    AddInfoLabelToTab(goodsTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                    rch.LogInfo("Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                }
            }
            catch (Exception ex)
            {
                AddErrorLabelToTab(goodsTabPage, $"Ошибка загрузки товаров: {ex.Message}");
                rch.LogError($"Ошибка загрузки товаров: {ex.Message}");
            }
        }

        private void LoadOrdersTab()
        {
            if (ordersTabPage == null) return;

            try
            {
                SellsUC sells = new SellsUC(rch);
                sells.Dock = DockStyle.Fill;
                ordersTabPage.Controls.Clear();
                ordersTabPage.Controls.Add(sells);

                if (AppSettings.IsConnectionStringSet)
                {
                    sells.LoadData(AppSettings.SqlConnection);
                }
                else
                {
                    AddInfoLabelToTab(ordersTabPage, "Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                    rch.LogInfo("Подключение к базе данных не установлено.\nПожалуйста, сначала подключитесь к базе данных.");
                }
            }
            catch (Exception ex)
            {
                AddErrorLabelToTab(ordersTabPage, $"Ошибка загрузки заказов: {ex.Message}");
                rch.LogError($"Ошибка загрузки заказов: {ex.Message}");
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
        public static void RefreshData()
        {
            clientsTabPage.Text = MetaInformation.tables[0];
            goodsTabPage.Text = MetaInformation.tables[1];
            ordersTabPage.Text = MetaInformation.tables[3];
        }
    }
    public static class TabPageExtensions
    {
        public static void SetForeColor(this TabPage tabPage, Color color)
        {
            tabPage.ForeColor = color;
        }
    }
}