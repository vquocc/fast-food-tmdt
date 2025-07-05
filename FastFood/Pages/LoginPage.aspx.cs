using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FastFood.Pages
{
    public partial class LoginPage : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txt_UserName.Text;
            string password = txt_Password.Text;
            string hashPassWord = HashPassword(password);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, RoleID, Fullname FROM Users WHERE UserName = @UserName AND PassWordHash = @PassWordHash";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@PassWordHash", hashPassWord);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Session["UserID"] = reader["UserID"].ToString();
                        Session["UserName"] = reader["UserName"].ToString();
                        Session["RoleID"] = reader["RoleID"].ToString();
                        Session["FullName"] = reader["FullName"].ToString();
                        string roleID = reader["RoleID"].ToString();

                        if (roleID == "1") 
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                "Swal.fire('Thành công', 'Đăng nhập thành công!', 'success').then(() => { window.location='AdminDashboard.aspx'; });", true);
                        }
                        else 
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                "Swal.fire('Thành công', 'Đăng nhập thành công!', 'success').then(() => { window.location='HomePage.aspx'; });", true);
                        }
                        
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire('Lỗi', 'Tài khoản hoặc mật khẩu không chính xác!', 'error');", true);
                    }
                }
                catch (Exception err)
                {

                    string errorMsg = HttpUtility.JavaScriptStringEncode(err.Message);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire('Lỗi', '{errorMsg}', 'error');", true);

                }

            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txt_RegFullName.Text.Trim();
            string userName = txt_RegUserName.Text.Trim();
            string password = txt_RegPassword.Text;
            string confirmPassword = txt_ConfirmPassword.Text;
            if (password != confirmPassword)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire('Lỗi', 'Mật khẩu xác nhận không khớp!', 'error');", true);
                return;
            }

            string hashedPassword = HashPassword(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users(Fullname, UserName, PasswordHash, RoleID) VALUES(@FullName, @UserName, @PasswordHash,  @RoleID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                cmd.Parameters.AddWithValue("@RoleID", 2);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "Swal.fire('Thành công', 'Đăng ký thành công!', 'success').then(() => { showLogin(); });", true);

                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    string errorMsg = HttpUtility.JavaScriptStringEncode(ex.Message);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Swal.fire('Lỗi', '{errorMsg}', 'error');", true);
                }
            }

        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }
    }
}