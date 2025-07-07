using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FastFood.Pages.Components;

namespace FastFood.Pages
{
    public partial class CategoryProduct : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.Items["CategoryID"] != null)
                {
                    string categoryID = Context.Items["CategoryID"].ToString();
                    LoadProductCategory(categoryID);
                }
                else
                {
                    Response.Write("<script>alert('Không tìm thấy danh mục phù hợp!'); window.location='MenuPage.aspx';</script>");
                }
            }
        }

        protected void LoadProductCategory(string categoryId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Products WHERE IsActive = 1 and CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                bool hasProduct = false;
                while (reader.Read())
                {
                    hasProduct = true;

                    var productControl = (ProductItem)LoadControl("~/Pages/Components/ProductItem.ascx");

                    productControl.ProductName = reader["ProductName"].ToString();
                    productControl.Price = string.Format("{0:#,##0}", reader["Price"]);
                    productControl.ImageUrl = reader["Product_Image"].ToString();
                    productControl.AverageRating = reader["AverageRating"].ToString();
                    productControl.ReviewCount = reader["ReviewCount"].ToString();

                    phProducts.Controls.Add(productControl);
                }

                reader.Close();
                emptyMessage.Visible = !hasProduct;
            }
        }
    }
}