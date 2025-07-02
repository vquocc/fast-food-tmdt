<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="FastFood.Pages.LoginPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .login-container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 70vh;
            width: 100%;
            box-sizing: border-box;
        }

        .login-left {
            width: 500px;
            display: flex;
            justify-content: center;
            align-items: center;
            opacity: 0;
            transform: translateX(-100px);
            animation: slideInLeft 0.7s ease forwards;
        }

        .login-image {
            width: 100%;
            height: 70vh;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
        }

        .login-right {
            width: 330px;
            display: flex;
            flex-direction: column;
            justify-content: center;
            height: 70vh;
            background-color: #fff;
            padding: 40px 30px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            border-radius: 10px;
            opacity: 0;
            transform: translateX(100px);
            animation: slideInRight 0.7s ease forwards;
        }

        .login-title {
            text-align: center;
            font-size: 24px;
            margin-bottom: 30px;
            color: #333;
            font-weight: bold;
        }

        .login-field {
            margin-bottom: 20px;
        }

        .login-label {
            display: block;
            margin-bottom: 8px;
            color: #555;
            font-size: 14px;
        }

        .login-input {
            width: 100%;
            padding: 10px 15px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            transition: all 0.3s ease;
            box-sizing: border-box;
        }

            .login-input:focus {
                border-color: #007bff;
                outline: none;
                box-shadow: 0 0 5px rgba(0, 123, 255, 0.3);
            }

        .login-button {
            width: 100%;
            padding: 12px 15px;
            background-color: #dc3545;
            color: #fff;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

            .login-button:hover {
                background-color: #0056b3;
            }

        @keyframes slideInLeft {
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }

        @keyframes slideInRight {
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }
    </style>
    <div class="login-container">
        <div class="login-left">
            <asp:Image ID="Image1" CssClass="login-image" runat="server" ImageUrl="./img/login.jpg" />
        </div>

        <!-- Phần Đăng nhập -->
        <div class="login-right" id="loginForm">
            <h4 class="login-title">ĐĂNG NHẬP</h4>

            <div class="login-field">
                <asp:Label ID="Label1" CssClass="login-label" runat="server" Text="Tên đăng nhập"></asp:Label>
                <asp:TextBox ID="txt_UserName" CssClass="login-input" runat="server" Placeholder="Tên đăng nhập..." />
            </div>

            <div class="login-field">
                <asp:Label ID="Label2" CssClass="login-label" runat="server" Text="Mật khẩu"></asp:Label>
                <asp:TextBox ID="txt_Password" CssClass="login-input" runat="server" TextMode="Password" Placeholder="Mật khẩu" />
            </div>

            <asp:Button ID="btnLogin" CssClass="login-button" runat="server" Text="Đăng nhập" OnClick="btnLogin_Click" />

            <p style="text-align: center; margin-top: 15px;">
                Chưa có tài khoản? 
            <a href="javascript:void(0);" onclick="showRegister()">Đăng ký</a>
            </p>
        </div>

        <!-- Phần Đăng ký -->
        <div class="login-right" id="registerForm" style="display: none;">
            <h4 class="login-title">ĐĂNG KÝ</h4>
            <div class="login-field">
                <asp:Label ID="Label6" CssClass="login-label" runat="server" Text="Tên người dùng"></asp:Label>
                <asp:TextBox ID="txt_RegFullName" CssClass="login-input" runat="server" Placeholder="Họ và tên quý khách..." />
            </div>
            <div class="login-field">
                <asp:Label ID="Label3" CssClass="login-label" runat="server" Text="Tên đăng nhập"></asp:Label>
                <asp:TextBox ID="txt_RegUserName" CssClass="login-input" runat="server" Placeholder="Tên đăng nhập..." />
            </div>

            <div class="login-field">
                <asp:Label ID="Label4" CssClass="login-label" runat="server" Text="Mật khẩu"></asp:Label>
                <asp:TextBox ID="txt_RegPassword" CssClass="login-input" runat="server" TextMode="Password" Placeholder="Mật khẩu" />
            </div>

            <div class="login-field">
                <asp:Label ID="Label5" CssClass="login-label" runat="server" Text="Xác nhận mật khẩu"></asp:Label>
                <asp:TextBox ID="txt_ConfirmPassword" CssClass="login-input" runat="server" TextMode="Password" Placeholder="Xác nhận mật khẩu" />
            </div>

            <asp:Button ID="btnRegister" CssClass="login-button" runat="server" Text="Đăng ký" OnClick="btnRegister_Click" />

            <p style="text-align: center; margin-top: 15px;">
                Đã có tài khoản? 
            <a href="javascript:void(0);" onclick="showLogin()">Đăng nhập</a>
            </p>
        </div>
    </div>
    <script>
        function showRegister() {
            document.getElementById("loginForm").style.display = "none";
            document.getElementById("registerForm").style.display = "flex";
        }

        function showLogin() {
            document.getElementById("registerForm").style.display = "none";
            document.getElementById("loginForm").style.display = "flex";
        }
    </script>


</asp:Content>
