using System;
using System.Collections;
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
    public partial class WebForm2 : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Load_Categories();
            }
            ;
        }

        protected void Load_Categories()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string queryCategories = "SELECT * FROM Categories";
                SqlDataAdapter da = new SqlDataAdapter(queryCategories, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptCategories.DataSource = dt;
                rptCategories.DataBind();

            }
            

        }

        protected void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string categoryId = e.CommandArgument.ToString();
            Context.Items["CategoryID"] = categoryId;
            Server.Transfer("CategoryProduct.aspx");
        }
    }
}