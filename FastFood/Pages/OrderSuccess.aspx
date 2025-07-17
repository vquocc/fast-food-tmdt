<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="OrderSuccess.aspx.cs" Inherits="FastFood.Pages.OrderSuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container text-center mt-5">
        <h2 class="text-success mb-3">🎉 Đặt hàng thành công!</h2>
        <p class="fs-5">Cảm ơn bạn đã đặt hàng tại FastFood.</p>
        <p class="fs-5">
            Mã đơn hàng của bạn là: 
            <span class="fw-bold text-primary">
                <asp:Label ID="lblOrderId" runat="server" />
            </span>
        </p>

        <asp:LinkButton ID="lnkContinueShopping" runat="server" CssClass="btn btn-outline-primary mt-3" PostBackUrl="~/Pages/HomePage.aspx">
            Tiếp tục mua sắm
        </asp:LinkButton>
        <asp:LinkButton ID="lnkViewOrders" runat="server" CssClass="btn btn-link mt-3" PostBackUrl="~/Pages/UserOrders.aspx">
            Xem đơn hàng của bạn
        </asp:LinkButton>
    </div>
</asp:Content>
