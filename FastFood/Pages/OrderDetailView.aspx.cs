using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace FastFood.Pages
{
    public partial class OrderDetailView : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Pages/LoginPage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string orderId = Request.QueryString["orderId"];
                if (!string.IsNullOrEmpty(orderId))
                {
                    LoadOrderDetails(orderId);
                }
                else
                {
                    Response.Write("Order ID không hợp lệ.");
                }
            }
        }

        private void LoadOrderDetails(string orderId)
        {
            lblOrderID.Text = $"Chi tiết đơn hàng #{orderId}";

            string query = "SELECT ops.ProductName, od.Quantity, ops.UnitPrice,  (od.Quantity * ops.UnitPrice) AS TotalPrice FROM OrderDetails od JOIN OrderProductSnapshots ops ON od.SnapshotID = ops.SnapshotID WHERE od.OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                GridViewOrderDetails.DataSource = dt;
                GridViewOrderDetails.DataBind();
            }
        }
    }
}
