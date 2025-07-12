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
            <div class="col-lg-12" style="display:flex;justify-content:center">
                <div class="card shadow-sm" style="width:700px">
                    <div class="card-body">
                        <h5 class="card-title">Đơn hàng theo trạng thái</h5>
                        <canvas id="orderStatusChart" height="150"></canvas>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-12 mt-4">
            <div class="card shadow-sm">
                <div class="card-body">
                    <h5 class="card-title">Doanh thu theo thời gian</h5>
                    <canvas id="revenueChart" height="200"></canvas>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var revenueData = <%= RevenueDataJson %>;
        window.addEventListener('DOMContentLoaded', function () {
            const ctx = document.getElementById('revenueChart').getContext('2d');

            const labels = revenueData.map(r => r.date);
            const values = revenueData.map(r => r.revenue);

            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Doanh thu (VNĐ)',
                        data: values,
                        borderColor: '#28a745',
                        backgroundColor: 'rgba(40, 167, 69, 0.1)',
                        tension: 0.2,
                        fill: true,
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: true,
                            position: 'top',
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    return context.dataset.label + ': ' + context.formattedValue.replace(/\B(?=(\d{3})+(?!\d))/g, ',') + '₫';
                                }
                            }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                callback: function (value) {
                                    return value.toLocaleString() + '₫';
                                }
                            }
                        }
                    }
                }
            });
        });
    </script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</asp:Content>
