using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FastFood.Pages.Components;

namespace FastFood.Pages
{
    public partial class MenuPage : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadProductsFromDB();

            UpdateCartCount();
        }


        private void LoadProductsFromDB()
        {

            string query = "SELECT ProductID, ProductName, Price, Product_Image, AverageRating, ReviewCount FROM Products WHERE IsActive = 1";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var productControl = (ProductItem)LoadControl("~/Pages/Components/ProductItem.ascx");

                    productControl.ProductID = Convert.ToInt32(reader["ProductID"]);
                    productControl.ProductName = reader["ProductName"].ToString();
                    productControl.Price = string.Format("{0:#,##0}", reader["Price"]);
                    productControl.ImageUrl = reader["Product_Image"].ToString();
                    productControl.AverageRating = reader["AverageRating"].ToString();
                    productControl.ReviewCount = reader["ReviewCount"].ToString();
                    productControl.AddToCartClicked += ProductControl_AddToCartClicked;

                    phProducts.Controls.Add(productControl);
                }

                reader.Close();
            }
        }

        private void ProductControl_AddToCartClicked(object sender, AddToCartEventArgs e)

        {
            Debug.WriteLine(e.ProductID);
            AddProductToCart(e.ProductID);
            UpdateCartCount();
        }


        protected void AddProductToCart(int productId)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("./LoginPage.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            string queryCheck = "SELECT Quantity FROM Cart WHERE UserID = @UserID AND ProductID = @ProductID";
            string queryInsert = "INSERT INTO Cart (UserID, ProductID, Quantity) VALUES (@UserID, @ProductID, 1)";
            string queryUpdate = "UPDATE Cart SET Quantity = Quantity + 1 WHERE UserID = @UserID AND ProductID = @ProductID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmdCheck = new SqlCommand(queryCheck, conn);
                cmdCheck.Parameters.AddWithValue("@UserID", userId);
                cmdCheck.Parameters.AddWithValue("@ProductID", productId);

                object result = cmdCheck.ExecuteScalar();
                if (result != null)
                {
                    SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                    cmdUpdate.Parameters.AddWithValue("@UserID", userId);
                    cmdUpdate.Parameters.AddWithValue("@ProductID", productId);
                    cmdUpdate.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmdInsert = new SqlCommand(queryInsert, conn);
                    cmdInsert.Parameters.AddWithValue("@UserID", userId);
                    cmdInsert.Parameters.AddWithValue("@ProductID", productId);
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        protected void UpdateCartCount()
        {
            if (Session["UserID"] == null)
            {
                HtmlGenericControl lblCartCount = (HtmlGenericControl)Master.FindControl("cartCount");
                if (lblCartCount != null)
                    lblCartCount.InnerText = "0";
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            string query = "SELECT SUM(Quantity) FROM Cart WHERE UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                int count = (result != DBNull.Value && result != null) ? Convert.ToInt32(result) : 0;

                HtmlGenericControl lblCartCount = (HtmlGenericControl)Master.FindControl("cartCount");
                if (lblCartCount != null)
                    lblCartCount.InnerText = count.ToString();
            }
        }



    }

}