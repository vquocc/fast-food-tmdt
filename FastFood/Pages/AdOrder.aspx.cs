using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace FastFood.Pages
{
    public partial class AdOrder : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("HomePage.aspx");
            }
            loadOrder();
            loadProductDdlproduct();
            loadUserDdl();
            
        }

        private void loadOrder()
        {
            string query = "SELECT OrderID, UserID, OrderStatus, TotalAmount, QRCode, CreatedAt, UpdatedAt FROM Orders ORDER BY CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }
        protected string GetStatusCssClass(string status)
        {
            switch (status)
            {
                case "Pending": return "btn btn-warning status-btn";
                case "Confirmed": return "btn btn-primary status-btn";
                case "Completed": return "btn btn-success status-btn";
                default: return "btn btn-secondary status-btn";
            }
        }
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ChangeStatus")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);

                string currentStatus = GetOrderStatus(orderId);

                if (currentStatus == "Completed")
                    return;
                string newStatus = GetNextStatus(currentStatus);

                UpdateOrderStatus(orderId, newStatus);

                loadOrder();
            }
        }

        private string GetOrderStatus(int orderId)
        {
            string status = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT OrderStatus FROM Orders WHERE OrderID = @OrderID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    conn.Open();
                    status = cmd.ExecuteScalar()?.ToString();
                }
            }
            return status;
        }

        private string GetNextStatus(string current)
        {
            switch (current)
            {
                case "Pending": return "Confirmed";
                case "Confirmed": return "Completed";
                default: return current;
            }
        }

        private void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Orders SET OrderStatus = @Status, UpdatedAt = GETDATE() WHERE OrderID = @OrderID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            string[] productIds = hfSelectedProductIDs.Value.Split(',');
            string[] quantities = hfSelectedQuantities.Value.Split(',');

            string userId = ddlUser.SelectedValue;
            string status = rblStatus.SelectedValue;
            decimal totalAmount = decimal.Parse(txtTotalAmount.Text);
            string qrCode = txtQRCode.Text;

            if (productIds.Length != quantities.Length)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Insert into Orders
                    string insertOrderSql = @"
                INSERT INTO Orders (UserID, OrderStatus, TotalAmount, QRCode, CreatedAt)
                OUTPUT INSERTED.OrderID
                VALUES (@UserID, @Status, @TotalAmount, @QRCode, GETDATE())";

                    SqlCommand cmdOrder = new SqlCommand(insertOrderSql, conn, transaction);
                    cmdOrder.Parameters.AddWithValue("@UserID", userId);
                    cmdOrder.Parameters.AddWithValue("@Status", status);
                    cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmdOrder.Parameters.AddWithValue("@QRCode", qrCode ?? (object)DBNull.Value);

                    int orderId = (int)cmdOrder.ExecuteScalar(); // lấy OrderID mới

                    // 2. Với mỗi sản phẩm được chọn:
                    for (int i = 0; i < productIds.Length; i++)
                    {
                        int productId = int.Parse(productIds[i]);
                        int quantity = int.Parse(quantities[i]);

                        // Lấy thông tin sản phẩm và category
                        string productSql = @"
                    SELECT p.ProductName, p.Price AS UnitPrice, c.CategoryID, c.CategoryName
                    FROM Products p
                    JOIN Categories c ON p.CategoryID = c.CategoryID
                    WHERE p.ProductID = @ProductID";

                        SqlCommand cmdProduct = new SqlCommand(productSql, conn, transaction);
                        cmdProduct.Parameters.AddWithValue("@ProductID", productId);

                        using (SqlDataReader reader = cmdProduct.ExecuteReader())
                        {
                            if (!reader.Read()) continue;

                            string productName = reader["ProductName"].ToString();
                            decimal unitPrice = (decimal)reader["UnitPrice"];
                            int categoryId = (int)reader["CategoryID"];
                            string categoryName = reader["CategoryName"].ToString();

                            reader.Close();

                            // 3. Insert into OrderProductSnapshots
                            string insertSnapshotSql = @"
                        INSERT INTO OrderProductSnapshots (OrderID, ProductID, ProductName, UnitPrice, CategoryID, CategoryName)
                        OUTPUT INSERTED.SnapshotID
                        VALUES (@OrderID, @ProductID, @ProductName, @UnitPrice, @CategoryID, @CategoryName)";

                            SqlCommand cmdSnapshot = new SqlCommand(insertSnapshotSql, conn, transaction);
                            cmdSnapshot.Parameters.AddWithValue("@OrderID", orderId);
                            cmdSnapshot.Parameters.AddWithValue("@ProductID", productId);
                            cmdSnapshot.Parameters.AddWithValue("@ProductName", productName);
                            cmdSnapshot.Parameters.AddWithValue("@UnitPrice", unitPrice);
                            cmdSnapshot.Parameters.AddWithValue("@CategoryID", categoryId);
                            cmdSnapshot.Parameters.AddWithValue("@CategoryName", categoryName);

                            int snapshotId = (int)cmdSnapshot.ExecuteScalar();

                            // 4. Insert into OrderDetails
                            string insertDetailSql = @"
                        INSERT INTO OrderDetails (OrderID, SnapshotID, Quantity, Subtotal)
                        VALUES (@OrderID, @SnapshotID, @Quantity, @Subtotal)";

                            SqlCommand cmdDetail = new SqlCommand(insertDetailSql, conn, transaction);
                            cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
                            cmdDetail.Parameters.AddWithValue("@SnapshotID", snapshotId);
                            cmdDetail.Parameters.AddWithValue("@Quantity", quantity);
                            cmdDetail.Parameters.AddWithValue("@Subtotal", unitPrice * quantity);

                            cmdDetail.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    // Có thể reset UI, thông báo thành công, reload lại đơn hàng...
                    Response.Write("<script>alert('Tạo đơn hàng thành công!');");
                    Response.Redirect("AdOrder.aspx", false); 
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    // Log lỗi nếu cần
                    Response.Write("<script>alert('Có lỗi xảy ra: " + ex.Message + "');</script>");
                }
            }

        }

       

        private string GetProductNameById(int productId)
        {

            string productName = string.Empty;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT ProductName FROM Products WHERE ProductID = @ProductID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    productName = result.ToString();
                }
            }
            return productName;
        }

        private void loadProductDdlproduct()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string qr = "select * from Products";
                using (SqlCommand cmd = new SqlCommand(qr, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        ddlProductPopup.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            ListItem item = new ListItem
                            {
                                Text = row["ProductName"].ToString(),
                                Value = row["ProductID"].ToString()
                            };
                            item.Attributes["data-price"] = row["Price"].ToString(); // Gắn giá trị `Price` vào attribute
                            ddlProductPopup.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void loadUserDdl()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string qr = "select * from Users where RoleID = 2";
                using (SqlCommand cmd = new SqlCommand(qr, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        this.ddlUser.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            ListItem item = new ListItem
                            {
                                Text = row["Username"].ToString(),
                                Value = row["UserID"].ToString()
                            };
                            this.ddlUser.Items.Add(item);
                        }
                    }
                }
            }
        }

    }
}