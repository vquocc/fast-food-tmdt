using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    lnkLogin.Visible = false;
                    btnLogout.Visible = true;
                }
                else
                {
                    lnkLogin.Visible = true;
                    btnLogout.Visible = false;
                }
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Pages/HomePage.aspx");
        }
    }
}