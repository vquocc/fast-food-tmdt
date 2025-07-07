using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FastFood.Pages.Components;
using System.Configuration;

namespace FastFood.Pages
{
    public partial class MenuPage : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProductsFromDB();
            }
        }

        private void LoadProductsFromDB()
        {
            
            string query = "SELECT ProductName, Price, Product_Image, AverageRating, ReviewCount FROM Products WHERE IsActive = 1";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var productControl = (ProductItem)LoadControl("~/Pages/Components/ProductItem.ascx");

                    productControl.ProductName = reader["ProductName"].ToString();
                    productControl.Price = string.Format("{0:#,##0}", reader["Price"]);
                    productControl.ImageUrl = reader["Product_Image"].ToString();
                    productControl.AverageRating = reader["AverageRating"].ToString();
                    productControl.ReviewCount = reader["ReviewCount"].ToString();

                    phProducts.Controls.Add(productControl); 
                }

                reader.Close();
            }
        }
    }

}