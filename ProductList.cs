using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyInventorySystem
{
    public partial class ProductList : Form
    {
        public ProductList()
        {
            InitializeComponent();

            dgvList.ColumnCount = 5;

            dgvList.Columns[0].Name = "ID";
            dgvList.Columns[1].Name = "Product Name";
            dgvList.Columns[2].Name = "Category";
            dgvList.Columns[3].Name = "Quantity";
            dgvList.Columns[4].Name = "Price";

            dgvList.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvList.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvList.AllowUserToAddRows = false;

            dgvList.ReadOnly = true;

            dgvList.RowTemplate.Height = 35;
        }

        // ================= FORM LOAD =================

        private async void ProductList_Load_1(
            object sender,
            EventArgs e
        )
        {
            await LoadProducts();
        }

        // ================= LOAD BUTTON =================

        private async void btnLoad_Click(
            object sender,
            EventArgs e
        )
        {
            await LoadProducts();
        }

        // ================= FETCH PRODUCTS =================

        private async Task LoadProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    dgvList.Rows.Clear();

                    string json =
                        await client.GetStringAsync(
                            "http://localhost:3000/api/inventory"
                        );

                    JArray products = JArray.Parse(json);

                    foreach (var item in products)
                    {
                        dgvList.Rows.Add(
                            item["id"]?.ToString() ?? "",
                            item["name"]?.ToString() ?? "",
                            item["category"]?.ToString() ?? "",
                            item["quantity"]?.ToString() ?? "",
                            "₱ " + (
                                item["price"]?.ToString() ?? "0"
                            )
                        );
                    }
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

        private void dgvList_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {

        }

        private void btnDashboard_Click(
            object sender,
            EventArgs e
        )
        {
            Dashboard db = new Dashboard();
            db.Show();

            this.Hide();
        }

        private void btnAddItem_Click(
            object sender,
            EventArgs e
        )
        {
            AddItem at = new AddItem();
            at.Show();

            this.Hide();
        }

        private void btnLogout_Click(
            object sender,
            EventArgs e
        )
        {
            this.Hide();
        }
    }
}