<%@ Page Title="Login" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FastFood.Pages.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Đăng nhập</h2>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:Panel ID="pnlLogin" runat="server">
        <asp:Label ID="lblUsername" runat="server" Text="Tên đăng nhập:" AssociatedControlID="txtUsername" />
        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
        <br />
        <asp:Label ID="lblPassword" runat="server" Text="Mật khẩu:" AssociatedControlID="txtPassword" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
        <br />
        <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
    </asp:Panel>
</asp:Content>
