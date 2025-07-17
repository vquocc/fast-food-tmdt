<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="CheckOut.aspx.cs" Inherits="FastFood.Pages.CheckOut" %>
<%@ Register TagPrefix="uc" TagName="ProductItem" Src="~/Pages/Components/ProductItem.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .checkout-columns {
            display: flex;
            gap: 24px;
            align-items: start;
            flex-wrap: wrap;
        }

        .cart-column {
            flex: 1;
            min-width: 300px;
            max-width: 700px;
        }

        .more-products-column {
            flex: 1;
            min-width: 200px;
            max-width: 600px;
            max-height: 550px;
            overflow-y: auto;
            padding-right: 10px;
            border-left: 1px solid #eee;
            margin-top: 30px;
        }

        .inputSearch {
            width: 100%;
            padding: 10px;
            font-size: 16px;
            border-radius: 8px;
            border: 1px solid #ccc;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            max-width: 100%;
        }

        .more-products-column::-webkit-scrollbar {
            width: 6px;
            display: none;
        }

        .more-products-column::-webkit-scrollbar-thumb {
            background-color: #ccc;
            border-radius: 4px;
        }

        .table-wrapper {
            max-height: 400px;
            overflow-y: auto;
            -ms-overflow-style: none;
            scrollbar-width: none;
        }

        .table-wrapper::-webkit-scrollbar {
            width: 0;
            background: transparent;
        }
    </style>

    <div class="container mt-4">
        <h2 class="text-danger mb-4">Giỏ Hàng Của Bạn</h2>

        <div class="checkout-columns">
            <div class="cart-column">

                <div class="mb-4">
                    <div class="d-flex flex-wrap gap-3">
                        <div class="flex-fill">
                            <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtFullAddress" Text="Địa chỉ giao hàng:" CssClass="form-label" />
                            <asp:TextBox ID="txtFullAddress" runat="server" CssClass="form-control" Placeholder="Nhập địa chỉ đầy đủ..." />
                        </div>
                        <div style="width: 300px;">
                            <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhoneNumber" Text="Số điện thoại:" CssClass="form-label" />
                            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" Placeholder="Nhập số điện thoại liên hệ..." />
                        </div>
                    </div>
                </div>

                <div class="table-wrapper">
                    <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                        DataKeyNames="ProductId" ShowHeaderWhenEmpty="True" EmptyDataText="Giỏ hàng trống.">
                        <Columns>
                            <asp:TemplateField HeaderText="Ảnh">
                                <ItemTemplate>
                                    <img src='<%# Eval("Product_Image") %>' alt="Sản phẩm" style="width: 80px; border-radius: 8px;" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ProductName" HeaderText="Tên Món" />
                            <asp:BoundField DataField="Price" HeaderText="Đơn Giá" DataFormatString="{0:N0}₫" />

                            <asp:TemplateField HeaderText="Số lượng">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDecrease" runat="server" CommandName="Decrease" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn btn-sm btn-outline-secondary">-</asp:LinkButton>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="mx-2" />
                                    <asp:LinkButton ID="btnIncrease" runat="server" CommandName="Increase" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn btn-sm btn-outline-secondary">+</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Thành Tiền">
                                <ItemTemplate>
                                    <%# (Convert.ToInt32(Eval("Quantity")) * Convert.ToDecimal(Eval("tPrice"))).ToString("N0") + "₫" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Xoá">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnRemove" runat="server"
                                        CommandName="Remove"
                                        CommandArgument='<%# Eval("ProductId") %>'
                                        CssClass="btn btn-sm btn-danger">Xoá
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="text-end mt-3">
                    <h4>Tổng cộng:
                        <asp:Label ID="lblGrandTotal" runat="server" CssClass="text-danger fw-bold">0₫</asp:Label>
                    </h4>
                    <asp:Button ID="btnThanhToan" runat="server" Text="Tiến hành đặt hàng" CssClass="btn btn-success fw-bold px-4" Enabled="false" OnClick="btnThanhToan_Click" />
                </div>
            </div>

            <div class="more-products-column">
                <div class="search-wrapper mb-3">
                    <input type="text" id="searchBox" placeholder="Tìm món..." class="inputSearch" />
                </div>
                <div class="d-flex flex-wrap gap-2 justify-content-start">
                    <asp:PlaceHolder ID="phMoreProducts" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <script>
        const searchBox = document.getElementById("searchBox");
        searchBox.addEventListener("keyup", function () {
            const keyword = this.value.toLowerCase();
            const items = document.querySelectorAll(".product-item");
            items.forEach(item => {
                const name = item.querySelector(".product-name")?.innerText.toLowerCase() || "";
                item.style.display = name.includes(keyword) ? "block" : "none";
            });
        });
    </script>

</asp:Content>
