<%@ Page Title="Chi tiết đơn hàng" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="OrderDetailView.aspx.cs" Inherits="FastFood.Pages.OrderDetailView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Chi tiết đơn hàng</h3>

    <asp:Label ID="lblOrderID" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
    <br />

    <asp:GridView ID="GridViewOrderDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Tên món" />
            <asp:BoundField DataField="Quantity" HeaderText="Số lượng" />
            <asp:BoundField DataField="UnitPrice" HeaderText="Đơn giá" DataFormatString="{0:N0} ₫" />
            <asp:BoundField DataField="TotalPrice" HeaderText="Thành tiền" DataFormatString="{0:N0} ₫" />
        </Columns>
    </asp:GridView>

</asp:Content>
