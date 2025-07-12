using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace FastFood.Pages
{
    public partial class AdProducts : System.Web.UI.Page
    {

        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
                LoadCategories(ddlAddCategory);
                //for(int i = 15; i <= 20;i++)
                //{
                //    DeleteOrder(i);
                //}
              
            }
        }
 
        private void LoadCategories(DropDownList ddl)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM Categories";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddl.DataSource = reader;
                ddl.DataValueField = "CategoryID";
                ddl.DataTextField = "CategoryName";
                ddl.DataBind();
            }

            ddl.Items.Insert(0, new ListItem("-- Chọn danh mục --", ""));
        }

        protected void btnSaveNewProduct_Click(object sender, EventArgs e)
        {
            string name = txtAddName.Text.Trim();
            string desc = txtAddDescription.Text.Trim();
            string priceText = txtAddPrice.Text.Trim();
            int categoryId;
            bool isActive = chkAddIsActive.Checked;

            string error = "";

            if (string.IsNullOrEmpty(name))
                error += "Tên sản phẩm không được để trống.<br/>";
            if (string.IsNullOrEmpty(desc))
                error += "Mô tả sản phẩm không được để trống.<br/>";
            if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
                error += "Giá phải là một số lớn hơn 0.<br/>";
            if (!int.TryParse(ddlAddCategory.SelectedValue, out categoryId))
                error += "Danh mục không hợp lệ.<br/>";
            if (!fuAddImage.HasFile)
                error += "Bạn cần chọn file ảnh sản phẩm.<br/>";
            else
            {
                string ext = System.IO.Path.GetExtension(fuAddImage.FileName).ToLower();
                string[] allowed = { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowed.Contains(ext))
                    error += "Chỉ cho phép file ảnh định dạng jpg, jpeg, png, gif.<br/>";
            }



            if (!string.IsNullOrEmpty(error))
            {
                lblAddProductError.Text = $"<div class='alert alert-danger'>{error}</div>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowAddModal", @"
            var modal = new bootstrap.Modal(document.getElementById('addProductModal'));
            modal.show();", true);
                return;
            }

            string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fuAddImage.FileName);
            string savePath = Server.MapPath("~/Uploads/") + fileName;
            fuAddImage.SaveAs(savePath);

            string imageUrl = "/Uploads/" + fileName;


            string query = @"
        INSERT INTO Products (ProductName, Description, Price, Product_Image, CategoryID, IsActive)
        VALUES (@Name, @Description, @Price, @Image, @CategoryID, @IsActive)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", desc);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Image", imageUrl);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                cmd.Parameters.AddWithValue("@IsActive", isActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtAddName.Text = txtAddDescription.Text = txtAddPrice.Text = "";
            chkAddIsActive.Checked = true;
            lblAddProductError.Text = "";
           
            string script = @"
                  var modalEl = document.getElementById('addProductModal');
                  var modal = bootstrap.Modal.getInstance(modalEl);
                  modal.hide();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "AddSuccess", script, true);
            LoadProducts();
        }



        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        p.ProductID,
                        p.ProductName,
                        p.Description,
                        p.Price,
                        p.Product_Image,
                        p.AverageRating,
                        p.ReviewCount,
                        p.IsActive,
                        p.CreatedAt,
                        c.CategoryName
                    FROM Products p
                    JOIN Categories c ON p.CategoryID = c.CategoryID
                    WHERE p.IsActive = 1
                    ORDER BY p.CreatedAt DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditProduct")
            {
                LoadProductForEdit(productId);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", @"
                   var myModal = new bootstrap.Modal(document.getElementById('editProductModal'));
                   myModal.show();", true);

            }
            else if (e.CommandName == "DeleteProduct")
            {
                DeleteProduct(productId);
                LoadProducts();
            }
        }
        private void LoadProductForEdit(int productId)
        {
            LoadCategories(ddlEditCategory);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Products WHERE ProductID = @ProductID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    hfEditProductID.Value = reader["ProductID"].ToString();
                    txtEditName.Text = reader["ProductName"].ToString();
                    txtEditDescription.Text = reader["Description"].ToString();
                    txtEditPrice.Text = reader["Price"].ToString();
                    hfOldImage.Value = reader["Product_Image"].ToString();
                    chkEditIsActive.Checked = (bool)reader["IsActive"];
                    ddlEditCategory.SelectedValue = reader["CategoryID"].ToString();
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string name = txtEditName.Text.Trim();
            string desc = txtEditDescription.Text.Trim();
            string priceText = txtEditPrice.Text.Trim();
            bool isActive = chkEditIsActive.Checked;
            int productId = int.Parse(hfEditProductID.Value);
            int categoryId;
            string oldImage = hfOldImage.Value;

            string error = "";
            if (string.IsNullOrEmpty(name)) error += "Tên sản phẩm không được để trống.<br/>";
            if (string.IsNullOrEmpty(desc)) error += "Mô tả không được để trống.<br/>";
            if (!decimal.TryParse(priceText, out decimal price) || price <= 0) error += "Giá không hợp lệ.<br/>";
            if (!int.TryParse(ddlEditCategory.SelectedValue, out categoryId)) error += "Danh mục không hợp lệ.<br/>";

            string newImageUrl = oldImage;

            if (fuEditImage.HasFile)
            {
                string ext = Path.GetExtension(fuEditImage.FileName).ToLower();
                string[] allowed = { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowed.Contains(ext))
                {
                    error += "Chỉ cho phép ảnh định dạng jpg, jpeg, png, gif.<br/>";
                }
                else
                {
                    string fileName = Guid.NewGuid() + ext;
                    string path = Server.MapPath("~/Uploads/") + fileName;
                    fuEditImage.SaveAs(path);
                    newImageUrl = "/Uploads/" + fileName;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                lblEditError.Text = $"<div class='alert alert-danger'>{error}</div>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowEditModal", @"
            var modal = new bootstrap.Modal(document.getElementById('editProductModal'));
            modal.show();", true);
                return;
            }

            // Update DB
            string query = @"
        UPDATE Products SET
            ProductName = @Name,
            Description = @Description,
            Price = @Price,
            Product_Image = @Image,
            CategoryID = @CategoryID,
            IsActive = @IsActive
        WHERE ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", desc);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Image", newImageUrl);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                cmd.Parameters.AddWithValue("@IsActive", isActive);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadProducts();

            lblEditError.Text = "";
            string script = @"
             var modal = bootstrap.Modal.getInstance(document.getElementById('editProductModal'));
             modal.hide();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", script, true);
        }


        private void DeleteProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Products SET IsActive = 0 WHERE ProductID = @ProductID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void DeleteOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Xóa OrderDetails
                    string deleteDetailsSql = @"
                DELETE FROM OrderDetails 
                WHERE OrderID = @OrderID";

                    SqlCommand cmdDetails = new SqlCommand(deleteDetailsSql, conn, transaction);
                    cmdDetails.Parameters.AddWithValue("@OrderID", orderId);
                    cmdDetails.ExecuteNonQuery();

                    // 2. Xóa OrderProductSnapshots
                    string deleteSnapshotsSql = @"
                DELETE FROM OrderProductSnapshots 
                WHERE OrderID = @OrderID";

                    SqlCommand cmdSnapshots = new SqlCommand(deleteSnapshotsSql, conn, transaction);
                    cmdSnapshots.Parameters.AddWithValue("@OrderID", orderId);
                    cmdSnapshots.ExecuteNonQuery();

                    // 3. Xóa Orders
                    string deleteOrderSql = @"
                DELETE FROM Orders 
                WHERE OrderID = @OrderID";

                    SqlCommand cmdOrder = new SqlCommand(deleteOrderSql, conn, transaction);
                    cmdOrder.Parameters.AddWithValue("@OrderID", orderId);
                    cmdOrder.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Lỗi khi xóa đơn hàng: " + ex.Message);
                }
            }
        }

    }
}