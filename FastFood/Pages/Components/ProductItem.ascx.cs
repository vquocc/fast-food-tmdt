using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages.Components
{
    public partial class ProductItem : UserControl
    {
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string AverageRating { get; set; }
        public string ReviewCount { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Image1.ImageUrl = ImageUrl;
                litProductName.Text = ProductName;
                litPrice.Text = Price;
                litAverageRating.Text = AverageRating;
                litReviewCount.Text = ReviewCount;
            }
        }

    }
}
