using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages
{
    public partial class OrderDetail : System.Web.UI.Page
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
                LoadOrders("Pending"); 
            }
        }

        public string GetStatusText(string statusCode)
        {
            switch (statusCode)
            {
                case "Pending":
                    return "Chờ xác nhận";
                case "Confirmed":
                    return "Đã xác nhận";
                case "Completed":
                    return "Hoàn tất";
                case "Cancelled":
                    return "Đã huỷ"; 
                default:
                    return statusCode; 
            }
        }


        protected void LoadOrders(string status)
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            string query = "SELECT OrderID, ShippingAddress, Phone, CreatedAt, TotalAmount , OrderStatus FROM Orders  WHERE UserID = @UserID AND OrderStatus = @Status ORDER BY CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Status", status);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataList1.DataSource = dt;
                    DataList1.DataBind();
                }
            }
        }

        protected void btnPending_Click(object sender, EventArgs e)
        {
            LoadOrders("Pending");
        }

        protected void btnConfirmed_Click(object sender, EventArgs e)
        {
            LoadOrders("Confirmed");
        }

        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            LoadOrders("Completed");
        }

        protected void DataList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void LinkButton_Command(object sender, CommandEventArgs e)
        {
            string orderID = e.CommandArgument.ToString();
            Response.Redirect("OrderDetailView.aspx?orderId=" + orderID);
        }
    }
}