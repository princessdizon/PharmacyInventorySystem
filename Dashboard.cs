using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;

namespace PharmacyInventorySystem
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private async void Dashboard_Load(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // ================= PRODUCTS COUNT =================
                    string inventoryJson =
                        await client.GetStringAsync("http://localhost:3000/api/inventory");

                    JArray inventory = JArray.Parse(inventoryJson);

                    lblTotalProducts.Text = inventory.Count.ToString();

                    // ================= SOLD COUNT =================
                    string soldJson =
                        await client.GetStringAsync("http://localhost:3000/api/sold");

                    JArray solds = JArray.Parse(soldJson);

                    lblTotalPurchase.Text = solds.Count.ToString();

                    // ================= TOTAL INCOME =================
                    decimal totalIncome = 0;

                    foreach (var item in solds)
                    {
                        totalIncome += Convert.ToDecimal(item["amount"]);
                    }

                    lblTotalIncome.Text = "₱ " + totalIncome.ToString("N2");

                    // ================= CATEGORY COUNT =================

                    string categoryJson =
                        await client.GetStringAsync(
                            "http://localhost:3000/api/categories"
                        );

                    JArray categories =
                        JArray.Parse(categoryJson);

                    lblTotalCategory.Text =
                        categories.Count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "API Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();

            // Hide Login Form
            this.Hide();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            AddItem at = new AddItem();
            at.Show();

            // Hide Login Form
            this.Hide();
        }

        private void btnProductList_Click(object sender, EventArgs e)
        {
            ProductList at = new ProductList();
            at.Show();

            // Hide Login Form
            this.Hide();
        }
    }
}