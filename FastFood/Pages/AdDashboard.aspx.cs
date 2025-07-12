using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages
{
    public partial class AdDashboard : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
                LoadOrderStatusChartData();
                LoadRevenueData();
            }
        }
        protected string RevenueDataJson { get; set; }

        private void LoadRevenueData()
        {

            var revenueData = new List<object>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                CONVERT(date, CreatedAt) AS OrderDate,
                SUM(TotalAmount) AS Revenue
            FROM Orders
            WHERE OrderStatus = 'Completed'
            GROUP BY CONVERT(date, CreatedAt)
            ORDER BY OrderDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    revenueData.Add(new
                    {
                        date = Convert.ToDateTime(reader["OrderDate"]).ToString("yyyy-MM-dd"),
                        revenue = Convert.ToDecimal(reader["Revenue"])
                    });
                }
            }

            // Chuyển thành JSON để JS dùng
            RevenueDataJson = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(revenueData);
        }
        private void LoadStatistics()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT
          (SELECT COUNT(*) FROM Products WHERE IsActive=1) AS TotalProducts,
          (SELECT COUNT(*) FROM Orders) AS TotalOrders,
          (SELECT COUNT(*) FROM Users) AS TotalUsers,
          (SELECT COUNT(*) FROM Categories) AS TotalCategories;
    ", conn))
            {
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    lblTotalProducts.Text = r["TotalProducts"].ToString();
                    lblTotalOrders.Text = r["TotalOrders"].ToString();
                    lblTotalUsers.Text = r["TotalUsers"].ToString();
                    lblTotalCategories.Text = r["TotalCategories"].ToString();
                }
            }
        }

        private void LoadOrderStatusChartData()
        {
            var statuses = new Dictionary<string, int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT OrderStatus, COUNT(*) AS Cnt
        FROM Orders
        GROUP BY OrderStatus;
    ", conn))
            {
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                    statuses[r["OrderStatus"].ToString()] = Convert.ToInt32(r["Cnt"]);
            }

            var labels = statuses.Keys.Select(s => $"'{s}'").ToArray();
            var data = statuses.Values.Select(v => v.ToString()).ToArray();

            string script = $@"
        const ctx = document.getElementById('orderStatusChart').getContext('2d');
        new Chart(ctx, {{
          type: 'doughnut',
          data: {{
            labels: [{string.Join(",", labels)}],
            datasets: [{{
              label: 'Đơn hàng',
              data: [{string.Join(",", data)}],
              backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', '#f6c23e']
            }}]
          }}
        }});
    ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoadChart", script, true);
        }
    }
}