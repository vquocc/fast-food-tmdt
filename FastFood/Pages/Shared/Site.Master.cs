using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood
{
    public partial class SiteMaster : MasterPage
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    lnkLogin.Visible = false;
                    btnLogout.Visible = true;
                }

            }
            LoadCart();
        }

        protected void LoadCart()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    cartCount.Text = "0";
                    return;
                }

                int userId = Convert.ToInt32(Session["UserId"]);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT c.ProductId, p.ProductName, p.Product_Image, c.Quantity, p.Price FROM Cart c JOIN Products p ON c.ProductId = p.ProductId WHERE c.UserID = @UserId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataList1.DataSource = dt;
                    DataList1.DataBind();

                    cartCount.Text = dt.Rows.Count.ToString();
                }
            }
            catch
            {
                cartCount.Text = "0"; 
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Pages/HomePage.aspx");
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                Response.Redirect("~/Pages/CheckOut.aspx");
            }
            else
            {
                Response.Redirect("~/Pages/LoginPage.aspx");
            }
        }
    }
}