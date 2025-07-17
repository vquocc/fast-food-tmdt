<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="FastFood.Pages.OrderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .status-buttons {
            margin-bottom: 20px;
        }

            .status-buttons .btn {
                margin-right: 10px;
            }

        .order-card {
            background-color: #f9f9f9;
            border-left: 5px solid #0d6efd;
            padding: 15px;
            margin-bottom: 15px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgb(0 0 0 / 0.1);
            transition: box-shadow 0.3s ease;
        }

            .order-card:hover {
                box-shadow: 0 4px 16px rgb(0 0 0 / 0.2);
            }

            .order-card p {
                margin: 0;
                font-size: 14px;
                color: #333;
            }

        .datalist-label {
            font-weight: 600;
            color: #555;
        }

        .datalist-value {
            margin-left: 6px;
            color: #222;
            font-weight: 500;
        }

        .datalist-actions {
            margin-top: 10px;
        }

            .datalist-actions .btn-linkbutton {
                margin-right: 12px;
                color: #0d6efd;
                cursor: pointer;
                text-decoration: underline;
                font-weight: 600;
            }

                .datalist-actions .btn-linkbutton:hover {
                    color: #084298;
                    text-decoration: none;
                }
    </style>

    <div class="container mt-4">
        <h3 class="mb-4 text-danger">Danh sách đơn hàng</h3>

        <div class="status-buttons">
            <asp:Button ID="btnPending" runat="server" Text="Chờ xác nhận" CssClass="btn btn-outline-secondary" OnClick="btnPending_Click" />
            <asp:Button ID="btnConfirmed" runat="server" Text="Đã xác nhận" CssClass="btn btn-outline-primary" OnClick="btnConfirmed_Click" />
            <asp:Button ID="btnCompleted" runat="server" Text="Hoàn tất" CssClass="btn btn-outline-success" OnClick="btnCompleted_Click" />
        </div>
        <asp:DataList ID="DataList1" runat="server" OnSelectedIndexChanged="DataList1_SelectedIndexChanged">
            <ItemTemplate>
                <div class="order-card">
                    <p>
                        <span class="datalist-label">Mã đơn:</span>
                        <asp:Label ID="Label3" runat="server" CssClass="datalist-value" Text='<%# Eval("OrderID") %>'></asp:Label>
                    </p>
                    <p>
                        <span class="datalist-label">Địa chỉ:</span>
                        <asp:Label ID="Label4" runat="server" CssClass="datalist-value" Text='<%# Eval("ShippingAddress") %>'></asp:Label>
                    </p>
                    <p>
                        <span class="datalist-label">Số Điện Thoại:</span>
                        <asp:Label ID="Label5" runat="server" CssClass="datalist-value" Text='<%# Eval("Phone") %>'></asp:Label>
                    </p>
                    <p>
                        <span class="datalist-label">Ngày đặt:</span>
                        <asp:Label ID="Label6" runat="server" CssClass="datalist-value" Text='<%# Eval("CreatedAt", "{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                    </p>
                    <p>
                        <span class="datalist-label">Tổng tiền:</span>
                        <asp:Label ID="Label7" runat="server" CssClass="datalist-value" Text='<%# Eval("TotalAmount", "{0:N0} ₫") %>'></asp:Label>
                    </p>

                    <p>
                        Trạng thái:
                        <asp:Label ID="Label8" runat="server" Text='<%# GetStatusText(Eval("OrderStatus").ToString()) %>'></asp:Label>
                    </p>

                    <div class="datalist-actions">
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="ViewDetails" CommandArgument='<%# Eval("OrderID") %>' OnCommand="LinkButton_Command" CssClass="btn-linkbutton">Xem chi tiết</asp:LinkButton>
                    </div>
                </div>
            </ItemTemplate>

        </asp:DataList>

    </div>

</asp:Content>
