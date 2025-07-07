using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages.Shared
{
    public partial class SiteAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentUrl = Request.Url.AbsolutePath.ToLower();
            Debug.WriteLine(currentUrl);
            if (currentUrl.EndsWith("addashboard"))
                adminDashboard.Attributes["class"] = "active";
            else if (currentUrl.EndsWith("adproducts"))
                adminProducts.Attributes["class"] = "active";
        }
    }
}