using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // TODO: Thay đoạn này bằng kiểm tra username/password thật từ database hoặc dịch vụ
            if (username == "admin" && password == "123456")
            {
                // Đăng nhập thành công
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Đăng nhập thành công!";
                // Ví dụ chuyển hướng đến trang chính:
                Response.Redirect("~/Pages/Home.aspx");
            }
            else
            {
                // Đăng nhập thất bại
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
            }
        }
    }
}