using System;
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
        public int ProductID { get; set; }

        public event EventHandler<AddToCartEventArgs> AddToCartClicked;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Image1.ImageUrl = ImageUrl;
                litProductName.Text = ProductName;
                litPrice.Text = Price;
                litAverageRating.Text = AverageRating;
                litReviewCount.Text = ReviewCount;
                AddToCart.CommandArgument = ProductID.ToString();
                AddToCart.CommandName = "AddToCart";
            }
        }

        protected void AddToCart_Click(object sender, EventArgs e)
        {
            if (AddToCartClicked != null)
            {
                AddToCartClicked(this, new AddToCartEventArgs(ProductID));
            }
        }
    }

    public class AddToCartEventArgs : EventArgs
    {
        public int ProductID { get; private set; }

        public AddToCartEventArgs(int productId)
        {
            ProductID = productId;
        }
    }
}
