using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FastFood.Pages.Components;

namespace FastFood.Pages
{
    public partial class CheckOut : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            gvCart.RowCommand += gvCart_RowCommand;
            LoadMoreProducts();
            if (!IsPostBack)
            {
                LoadCartItem();
                UpdateCartCount();
            }
        }


        protected void LoadCartItem()
        {
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT c.ProductId, p.ProductName, p.Product_Image, p.Price, c.Quantity, (p.Price * c.Quantity) AS tPrice
                                 FROM Cart c 
                                 JOIN Products p ON c.ProductId = p.ProductId 
                                 WHERE c.UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCart.DataSource = dt;
                gvCart.DataBind();

                decimal grandTotal = 0;
                foreach (DataRow row in dt.Rows)
                {
                    grandTotal += Convert.ToDecimal(row["tPrice"]);
                }

                lblGrandTotal.Text = grandTotal.ToString("N0") + "₫";
                btnThanhToan.Enabled = dt.Rows.Count > 0;
            }
        }

        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Session["UserId"] == null) return;

            int userId = Convert.ToInt32(Session["UserId"]);
            int productId = Convert.ToInt32(e.CommandArgument);

            string query = "";
            if (e.CommandName == "Increase")
            {
                query = "UPDATE Cart SET Quantity = Quantity + 1 WHERE UserID = @UserId AND ProductId = @ProductId";
            }
            else if (e.CommandName == "Decrease")
            {
                query = "UPDATE Cart SET Quantity = CASE WHEN Quantity > 1 THEN Quantity - 1 ELSE 1 END WHERE UserID = @UserId AND ProductId = @ProductId";
            }
            else if (e.CommandName == "Remove")
            {
                query = "DELETE FROM Cart WHERE UserID = @UserId AND ProductId = @ProductId";
            }

            if (!string.IsNullOrEmpty(query))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadCartItem();
                UpdateCartCount();
            }
        }


        private void LoadMoreProducts()
        {
            string query = "SELECT ProductID, ProductName, Price, Product_Image, AverageRating, ReviewCount FROM Products WHERE IsActive = 1 ORDER BY ProductID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var productControl = (ProductItem)LoadControl("~/Pages/Components/ProductItem.ascx");
                    productControl.ID = "ProductItem_" + reader["ProductID"];

                    productControl.ProductID = Convert.ToInt32(reader["ProductID"]);
                    productControl.ProductName = reader["ProductName"].ToString();
                    productControl.Price = string.Format("{0:#,##0}", reader["Price"]);
                    productControl.ImageUrl = reader["Product_Image"].ToString();
                    productControl.AverageRating = reader["AverageRating"].ToString();
                    productControl.ReviewCount = reader["ReviewCount"].ToString();

                    productControl.AddToCartClicked += ProductControl_AddToCartClicked;

                    phMoreProducts.Controls.Add(productControl);
                }

                reader.Close();
            }
        }


        private void ProductControl_AddToCartClicked(object sender, AddToCartEventArgs e)
        {
            AddProductToCart(e.ProductID);
            LoadCartItem();
            UpdateCartCount();
        }

        protected void AddProductToCart(int productId)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("./LoginPage.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            string queryCheck = "SELECT Quantity FROM Cart WHERE UserID = @UserId AND ProductID = @ProductId";
            string queryInsert = "INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@UserID, @ProductID, 1)";
            string queryUpdate = "UPDATE Cart SET Quantity = Quantity + 1 WHERE UserID = @UserId AND ProductID = @ProductId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmdCheck = new SqlCommand(queryCheck, conn);
                cmdCheck.Parameters.AddWithValue("@UserId", userId);
                cmdCheck.Parameters.AddWithValue("@ProductId", productId);

                object result = cmdCheck.ExecuteScalar();
                if (result != null)
                {
                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                    cmdUpdate.Parameters.AddWithValue("@UserId", userId);
                    cmdUpdate.Parameters.AddWithValue("@ProductId", productId);
                    cmdUpdate.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmdInsert = new SqlCommand(queryInsert, conn);
                    cmdInsert.Parameters.AddWithValue("@UserId", userId);
                    cmdInsert.Parameters.AddWithValue("@ProductId", productId);
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        protected void UpdateCartCount()
        {
            Literal cartCount = (Literal)Master.FindControl("cartCount");

            if (Session["UserId"] == null)
            {
                if (cartCount != null)
                    cartCount.Text = "0";
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            string query = "SELECT SUM(Quantity) FROM Cart WHERE UserID = @UserId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                int count = (result != DBNull.Value && result != null) ? Convert.ToInt32(result) : 0;

                if (cartCount != null)
                    cartCount.Text = count.ToString();
            }
        }

        protected void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Pages/LoginPage.aspx");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullAddress.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "swal", "Swal.fire('Thiếu thông tin', 'Vui lòng nhập địa chỉ giao hàng!', 'warning');", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "swal", "Swal.fire('Thiếu thông tin', 'Vui lòng nhập số điện thoại!', 'warning');", true);
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            int newOrderId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    string insertOrderQuery = "INSERT INTO Orders (UserID, OrderStatus, TotalAmount, ShippingAddress, Phone) OUTPUT INSERTED.OrderID VALUES (@UserID, @Status, @Total, @ShippingAddress, @Phone)";
                    SqlCommand cmdOrder = new SqlCommand(insertOrderQuery, conn, tran);
                    cmdOrder.Parameters.AddWithValue("@UserID", userId);
                    cmdOrder.Parameters.AddWithValue("@Status", "Pending");

                    decimal totalAmount = 0;
                    cmdOrder.Parameters.AddWithValue("@Total", totalAmount);
                    cmdOrder.Parameters.AddWithValue("@ShippingAddress", txtFullAddress.Text.Trim());
                    cmdOrder.Parameters.AddWithValue("@Phone", txtPhoneNumber.Text.Trim());

                    newOrderId = (int)cmdOrder.ExecuteScalar();

                    string selectCartQuery = @"SELECT c.ProductId, p.ProductName, p.Product_Image, p.Price, c.Quantity, cat.CategoryID, cat.CategoryName
                                       FROM Cart c
                                       JOIN Products p ON c.ProductId = p.ProductId
                                       JOIN Categories cat ON p.CategoryID = cat.CategoryID
                                       WHERE c.UserID = @UserId";
                    SqlCommand cmdCart = new SqlCommand(selectCartQuery, conn, tran);
                    cmdCart.Parameters.AddWithValue("@UserId", userId);
                    SqlDataAdapter da = new SqlDataAdapter(cmdCart);
                    DataTable dtCart = new DataTable();
                    da.Fill(dtCart);

                    foreach (DataRow row in dtCart.Rows)
                    {
                        int productId = Convert.ToInt32(row["ProductId"]);
                        string productName = row["ProductName"].ToString();
                        int categoryId = Convert.ToInt32(row["CategoryID"]);
                        string categoryName = row["CategoryName"].ToString();
                        decimal unitPrice = Convert.ToDecimal(row["Price"]);
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        decimal subtotal = unitPrice * quantity;
                        totalAmount += subtotal;

                        string insertSnap = @"INSERT INTO OrderProductSnapshots (OrderID, ProductID, ProductName, UnitPrice, CategoryID, CategoryName) 
                                      OUTPUT INSERTED.SnapshotID
                                      VALUES (@OrderID, @ProductID, @ProductName, @UnitPrice, @CategoryID, @CategoryName)";
                        SqlCommand cmdSnap = new SqlCommand(insertSnap, conn, tran);
                        cmdSnap.Parameters.AddWithValue("@OrderID", newOrderId);
                        cmdSnap.Parameters.AddWithValue("@ProductID", productId);
                        cmdSnap.Parameters.AddWithValue("@ProductName", productName);
                        cmdSnap.Parameters.AddWithValue("@UnitPrice", unitPrice);
                        cmdSnap.Parameters.AddWithValue("@CategoryID", categoryId);
                        cmdSnap.Parameters.AddWithValue("@CategoryName", categoryName);

                        int snapshotId = (int)cmdSnap.ExecuteScalar();

                        string insertDetail = @"INSERT INTO OrderDetails (OrderID, SnapshotID, Quantity, Subtotal)
                                        VALUES (@OrderID, @SnapshotID, @Quantity, @Subtotal)";
                        SqlCommand cmdDetail = new SqlCommand(insertDetail, conn, tran);
                        cmdDetail.Parameters.AddWithValue("@OrderID", newOrderId);
                        cmdDetail.Parameters.AddWithValue("@SnapshotID", snapshotId);
                        cmdDetail.Parameters.AddWithValue("@Quantity", quantity);
                        cmdDetail.Parameters.AddWithValue("@Subtotal", subtotal);
                        cmdDetail.ExecuteNonQuery();
                    }

                    string updateTotal = "UPDATE Orders SET TotalAmount = @Total WHERE OrderID = @OrderID";
                    SqlCommand cmdUpdateTotal = new SqlCommand(updateTotal, conn, tran);
                    cmdUpdateTotal.Parameters.AddWithValue("@Total", totalAmount);
                    cmdUpdateTotal.Parameters.AddWithValue("@OrderID", newOrderId);
                    cmdUpdateTotal.ExecuteNonQuery();

                    SqlCommand cmdDelete = new SqlCommand("DELETE FROM Cart WHERE UserId = @UserId", conn, tran);
                    cmdDelete.Parameters.AddWithValue("@UserId", userId);
                    cmdDelete.ExecuteNonQuery();

                    tran.Commit();
                    LoadCartItem();
                    UpdateCartCount();
                    Response.Redirect("~/Pages/OrderSuccess.aspx?orderId=" + newOrderId, false);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Debug.WriteLine("Error placing order: " + ex.Message);
                }
            }
        }



    }
}
