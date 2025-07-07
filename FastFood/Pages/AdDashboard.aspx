<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdDashboard.aspx.cs" Inherits="FastFood.Pages.AdDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceholder" runat="server">
    <div class="container py-4">
        <div class="row g-4">
  
            <div class="col-sm-6 col-lg-3">
                <div class="card text-center shadow-sm">
                    <div class="card-body">
                        <h6 class="text-uppercase">Tổng sản phẩm</h6>
                        <h2>
                            <asp:Label ID="lblTotalProducts" runat="server" Text="0"></asp:Label></h2>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-3">
                <div class="card text-center shadow-sm">
                    <div class="card-body">
                        <h6 class="text-uppercase">Đơn hàng</h6>
                        <h2>
                            <asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label></h2>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-3">
                <div class="card text-center shadow-sm">
                    <div class="card-body">
                        <h6 class="text-uppercase">Người dùng</h6>
                        <h2>
                            <asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label></h2>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-3">
                <div class="card text-center shadow-sm">
                    <div class="card-body">
                        <h6 class="text-uppercase">Danh mục</h6>
                        <h2>
                            <asp:Label ID="lblTotalCategories" runat="server" Text="0"></asp:Label></h2>
                    </div>
                </div>
            </div>
        </div>

        <div class="row my-4">
            <div class="col-lg-8">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <h5 class="card-title">Đơn hàng theo trạng thái</h5>
                        <canvas id="orderStatusChart" height="150"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
