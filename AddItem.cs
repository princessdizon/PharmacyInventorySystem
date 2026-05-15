using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyInventorySystem
{
    public partial class AddItem : Form
    {
        public AddItem()
        {
            InitializeComponent();
        }

        // ================= LOGOUT =================

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        // ================= OPEN ADD ITEM =================

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            AddItem at = new AddItem();
            at.Show();

            this.Hide();
        }

        // ================= OPEN DASHBOARD =================

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();

            this.Hide();
        }

        // ================= FORM LOAD =================

        private async void AddItem_Load(object sender, EventArgs e)
        {
            await LoadCategories();
        }

        // ================= LOAD CATEGORIES =================

        private async Task LoadCategories()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json =
                        await client.GetStringAsync(
                            "http://localhost:3000/api/categories"
                        );

                    JArray categories =
                        JArray.Parse(json);

                    cbxCategory.Items.Clear();

                    foreach (var item in categories)
                    {
                        cbxCategory.Items.Add(
                            new ComboBoxItem
                            {
                                Text = item["name"].ToString(),
                                Value = item["id"].ToString()
                            }
                        );
                    }

                    cbxCategory.DisplayMember = "Text";
                    cbxCategory.ValueMember = "Value";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Category Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ================= SAVE ITEM =================

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // ================= VALIDATION =================

                if (txtProductName.Text.Trim() == "" ||
                    txtPrice.Text.Trim() == "" ||
                    cbxCategory.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Please fill all required fields!",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                // ================= GET VALUES =================

                string productName =
                    txtProductName.Text.Trim();

                string unit =
                    txtUnit.Text.Trim();

                string price =
                    txtPrice.Text.Trim();

                string supplier =
                    txtSupplier.Text.Trim();

                string company =
                    txtCompany.Text.Trim();

                string description =
                    txtDescription.Text.Trim();

                int quantity =
                    Convert.ToInt32(nudQuantity.Value);

                // ================= CATEGORY ID =================

                ComboBoxItem selectedCategory =
                    (ComboBoxItem)cbxCategory.SelectedItem;

                int categoryId =
                    Convert.ToInt32(selectedCategory.Value);

                // ================= API URL =================

                string url =
                    "http://localhost:3000/api/inventory";

                // ================= JSON OBJECT =================
                // MUST MATCH MYSQL COLUMNS

                var itemData = new
                {
                    name = productName,
                    unit = unit,
                    price = price,
                    quantity = quantity,
                    supplier = supplier,
                    company = company,
                    catId = categoryId,
                    description = description
                };

                // ================= CONVERT TO JSON =================

                string json =
                    JsonConvert.SerializeObject(itemData);

                using (HttpClient client = new HttpClient())
                {
                    var content =
                        new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json"
                        );

                    // ================= SEND TO API =================

                    HttpResponseMessage response =
                        await client.PostAsync(
                            url,
                            content
                        );

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(
                            "Item Saved Successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        // RESET FORM
                        ClearFields();
                    }
                    else
                    {
                        string error =
                            await response.Content.ReadAsStringAsync();

                        MessageBox.Show(
                            error,
                            "Failed To Save Item",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ================= RESET FIELDS =================

        private void ClearFields()
        {
            txtProductName.Clear();
            txtUnit.Clear();
            txtPrice.Clear();
            txtSupplier.Clear();
            txtCompany.Clear();
            txtDescription.Clear();

            nudQuantity.Value = 0;

            cbxCategory.SelectedIndex = -1;
        }

        private void btnProductList_Click(object sender, EventArgs e)
        {
            ProductList at = new ProductList();
            at.Show();

            // Hide Login Form
            this.Hide();
        }
    }

    // ================= COMBOBOX CLASS =================

    public class ComboBoxItem
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}