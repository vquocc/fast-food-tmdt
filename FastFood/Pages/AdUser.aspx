<%@ Page Title="Quản lý người dùng" Language="C#" MasterPageFile="~/Pages/Shared/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdUser.aspx.cs" Inherits="FastFood.Pages.AdUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headPlaceholder" runat="server">
    <style>
        .form-group {
            margin-bottom: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceholder" runat="server">

    <h2>Quản lý người dùng</h2>
    <hr />
    <!-- Danh sách người dùng -->
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
         CssClass="table table-bordered">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Username" HeaderText="Tên đăng nhập" />
            <asp:BoundField DataField="FullName" HeaderText="Họ tên" />
            <asp:BoundField DataField="RoleName" HeaderText="Vai trò" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
        </Columns>
    </asp:GridView>

</asp:Content>
