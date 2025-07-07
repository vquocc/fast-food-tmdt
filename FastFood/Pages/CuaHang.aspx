<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="CuaHang.aspx.cs" Inherits="FastFood.Pages.CuaHang" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .store-image {
        width: 500px;
        height: 600px; 
        display: block;
        margin: 0 auto;
        border-radius: 15px;
    }
</style>
    <asp:Image ID="Image1" CssClass="store-image" runat="server" ImageUrl="./img/cuahang.jpg" />
</asp:Content>
