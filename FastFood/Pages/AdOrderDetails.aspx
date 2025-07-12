<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdOrderDetails.aspx.cs" Inherits="FastFood.Pages.AdOrderDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
    <style>
        .product-row {
            display: flex;
            gap: 15px;
            padding: 10px;
            border-bottom: 1px solid #ccc;
            align-items: center;
        }
        .product-row img {
            width: 60px;
            height: 60px;
            object-fit: cover;
            border-radius: 5px;
        }
    </style>
</head>
<body>
     <form id="form1" runat="server">
        <h3>Chi tiết đơn hàng</h3>
        <asp:Repeater ID="rptOrderDetails" runat="server">
            <ItemTemplate>
                <div class="product-row">
                    <img src='<%# Eval("Product_Image") %>' alt="Ảnh sản phẩm" />
                    <div>
                        <strong><%# Eval("ProductName") %></strong><br />
                        Số lượng: <%# Eval("Quantity") %><br />
                        Đơn giá: <%# Eval("UnitPrice", "{0:N0}") %>₫<br />
                        Thành tiền: <%# Eval("Subtotal", "{0:N0}") %>₫
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
