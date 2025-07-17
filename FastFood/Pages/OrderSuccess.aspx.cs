using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages
{
    public partial class OrderSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["orderId"] != null)
            {
                lblOrderId.Text = Request.QueryString["orderId"];
            }
            else
            {
                lblOrderId.Text = "Không xác định";
            }
        }

        protected void lnkContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/HomePage.aspx");
        }

    }
}