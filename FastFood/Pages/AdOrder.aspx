<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdOrder.aspx.cs" Inherits="FastFood.Pages.AdOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headPlaceholder" runat="server">
    <style>
        .orders-grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 14px;
        }

            .orders-grid th {
                background-color: #007bff;
                color: white;
                padding: 10px;
                text-align: left;
            }

            .orders-grid td {
                padding: 10px;
                border-bottom: 1px solid #dee2e6;
            }

            .orders-grid tr:nth-child(even) {
                background-color: #f8f9fa;
            }

            .orders-grid tr:hover {
                background-color: #e2e6ea;
            }

            .orders-grid td:nth-child(5) {
                word-break: break-word;
                max-width: 200px;
            }


        .status-btn {
            transition: all 0.3s ease-in-out;
            padding: 5px 12px;
            font-weight: bold;
            border: none;
            border-radius: 4px;
        }

            .status-btn:hover {
                opacity: 0.85;
                transform: scale(1.05);
                cursor: pointer;
            }

        .btn-warning {
            background-color: #ffc107;
            color: #212529;
        }

        .btn-primary {
            background-color: #007bff;
            color: white;
        }

        .btn-success {
            background-color: #28a745;
            color: white;
        }

        .btn-secondary {
            background-color: #6c757d;
            color: white;
        }

        .modal-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.5);
            z-index: 1000;
        }

        .modal-box {
            background: white;
            width: 450px;
            margin: 80px auto;
            padding: 20px;
            border-radius: 8px;
            position: relative;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-control {
            width: 100%;
            padding: 6px 10px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .form-check input[type=radio] {
            margin-right: 6px;
        }

        .popup-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.4);
            z-index: 1100;
        }

        .popup-box {
            background: white;
            width: 350px;
            padding: 20px;
            margin: 150px auto;
            border-radius: 6px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.3);
        }

        .popup-buttons {
            margin-top: 15px;
            text-align: right;
        }

            .popup-buttons button {
                margin-left: 8px;
            }

        .user-select-group {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .add-customer-btn {
            background: none;
            border: none;
            cursor: pointer;
            padding: 6px;
            font-size: 18px;
            color: #007bff;
            transition: transform 0.2s;
        }

            .add-customer-btn:hover {
                transform: scale(1.2);
                color: #0056b3;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceholder" runat="server">
    <asp:Button ID="btnOpenModal" runat="server" Text="Tạo Đơn Hàng Mới" CssClass="btn btn-success" OnClientClick="showOrderModal(); return false;" />
    <br />
    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="orders-grid" OnRowCommand="gvOrders_RowCommand">
        <Columns>
            <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
            <asp:BoundField DataField="UserID" HeaderText="User ID" />

            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:N0}" />
            <asp:TemplateField HeaderText="Order Status">
                <ItemTemplate>
                    <asp:LinkButton ID="btnStatus"
                        runat="server"
                        Text='<%# Eval("OrderStatus") %>'
                        CommandName="ChangeStatus"
                        CommandArgument='<%# Eval("OrderID") %>'
                        CssClass='<%# GetStatusCssClass(Eval("OrderStatus").ToString()) %>'
                        Enabled='<%# Eval("OrderStatus").ToString() != "Completed" %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="QRCode" HeaderText="QR Code" />
            <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="UpdatedAt" HeaderText="Updated At" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:TemplateField HeaderText="Chi tiết">
                <ItemTemplate>
                    <button type="button" class="btn btn-info btn-sm" onclick="showOrderDetails(<%# Eval("OrderID") %>)">
                        Xem
                    </button>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    <!-- Modal tạo đơn hàng -->
    <div id="orderModal" class="modal-overlay" style="display: none;">
        <div class="modal-box">
            <h4>Tạo Đơn Hàng Mới</h4>
            <asp:Panel ID="pnlCreateOrder" runat="server">

                <div class="form-group">
                    <label for="ddlUser">Người Dùng:</label>
                    <div class="user-select-group">
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control" />
                        <button type="button" class="add-customer-btn" title="Thêm khách hàng" onclick="onAddCustomerClick()">
                            👤➕
                        </button>
                    </div>
                </div>

                <div class="form-group">
                    <label>Sản Phẩm:</label>
                    <div id="productListContainer" runat="server">
                        <!-- Các sản phẩm được chọn sẽ hiển thị ở đây -->

                    </div>
                    <button type="button" class="btn btn-secondary" onclick="showAddProduct()">+ Thêm sản phẩm</button>
                </div>

                <!-- Popup nhỏ để chọn product và số lượng -->
                <div id="addProductPopup" class="popup-overlay" style="display: none;">
                    <div class="popup-box">
                        <h5>Chọn Sản Phẩm</h5>
                        <asp:DropDownList ID="ddlProductPopup" runat="server" CssClass="form-control" />
                        <label for="txtQuantity">Số lượng:</label>
                        <input type="number" id="txtQuantity" class="form-control" min="1" value="1" />
                        <div class="popup-buttons">
                            <button type="button" class="btn btn-primary" onclick="addProductToList()">OK</button>
                            <button type="button" class="btn btn-secondary" onclick="hideAddProduct()">Cancel</button>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <label>Trạng Thái:</label><br />
                    <asp:RadioButtonList ID="rblStatus" runat="server" CssClass="form-check">
                        <asp:ListItem Text="Pending" Value="Pending" Selected="True" />
                        <asp:ListItem Text="Confirmed" Value="Confirmed" />
                        <asp:ListItem Text="Completed" Value="Completed" />
                    </asp:RadioButtonList>
                </div>

                <div class="form-group">
                    <label for="txtTotalAmount">Tổng Tiền:</label>
                    <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" TextMode="Number" />
                </div>

                <div class="form-group">
                    <label for="txtQRCode">QRCode:</label>
                    <asp:TextBox ID="txtQRCode" runat="server" CssClass="form-control" />
                </div>
                <asp:HiddenField ID="hfSelectedProductIDs" runat="server" />
                <asp:HiddenField ID="hfSelectedQuantities" runat="server" />

                <%--  OnClick="btnCreateOrder_Click"--%>
                <asp:Button ID="btnCreateOrder" runat="server" Text="Tạo Đơn Hàng" CssClass="btn btn-primary" OnClick="btnCreateOrder_Click" />
                <button type="button" onclick="hideOrderModal()" class="btn btn-secondary">Hủy</button>
                <div class="form-group">
                    <strong>Tổng giá sản phẩm:</strong> <span id="totalPrice">0₫</span>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div id="detailsModal" class="modal-overlay" style="display: none;">
        <div class="modal-box" style="width: 600px;">
            <h4>Chi tiết đơn hàng</h4>
            <iframe id="detailsIframe" style="width: 100%; height: 400px; border: none;"></iframe>
            <button onclick="closeDetailsModal()" class="btn btn-secondary" style="margin-top: 10px;">Đóng</button>
        </div>
    </div>


    <script>
        function showOrderModal() {
            document.getElementById('orderModal').style.display = 'block';
        }

        function hideOrderModal() {
            document.getElementById('orderModal').style.display = 'none';
        }

        function showAddProduct() {
            document.getElementById('addProductPopup').style.display = 'block';
        }

        function hideAddProduct() {
            document.getElementById('addProductPopup').style.display = 'none';
            document.getElementById('txtQuantity').value = 1;
        }

        function addProductToList() {
            var ddl = document.getElementById('<%= ddlProductPopup.ClientID %>');
            var selectedValue = ddl.value;
            var selectedText = ddl.options[ddl.selectedIndex].text;
            var selectedPrice = parseFloat(ddl.options[ddl.selectedIndex].getAttribute("data-price")) || 0;
            var quantity = parseInt(document.getElementById('txtQuantity').value);

            if (!selectedValue) {
                alert('Vui lòng chọn sản phẩm.');
                return;
            }
            if (!quantity || quantity < 1) {
                alert('Số lượng phải lớn hơn 0.');
                return;
            }

            var container = document.getElementById('<%= productListContainer.ClientID %>');
            if (!container) {
                alert('Không tìm thấy container chứa danh sách sản phẩm!');
                return;
            }

            var existingInputQty = container.querySelector('input[name="SelectedQuantities"][data-productid="' + selectedValue + '"]');
            if (existingInputQty) {
                var oldQty = parseInt(existingInputQty.value);
                existingInputQty.value = oldQty + quantity;
                var parent = existingInputQty.closest('.selected-product-item');
                parent.querySelector('.qty-display').innerText = oldQty + quantity;
                updateTotalPrice();
                hideAddProduct();
                return;
            }

            var div = document.createElement('div');
            div.className = 'selected-product-item';
            div.style.marginBottom = '8px';

            div.innerHTML = `
        <span>${selectedText} - Số lượng: <span class="qty-display">${quantity}</span> - Giá: <span class="price-display">${selectedPrice.toLocaleString()}₫</span></span>
        <button type="button" onclick="removeProduct(this)" class="btn btn-sm btn-danger" style="margin-left:10px;">Xóa</button>
        <input type="hidden" name="SelectedProductIDs" value="${selectedValue}" />
        <input type="hidden" name="SelectedQuantities" data-productid="${selectedValue}" value="${quantity}" />
        <input type="hidden" class="price-hidden" value="${selectedPrice}" />
    `;

            container.appendChild(div);

            updateHiddenFields();
            updateTotalPrice();
            hideAddProduct();

        }

        function removeProduct(btn) {
            btn.parentElement.remove();
            updateHiddenFields();
            updateTotalPrice();
        }
        function updateTotalPrice() {
            var container = document.getElementById('<%= productListContainer.ClientID %>');
            if (!container) return;

            var total = 0;

            // Lấy tất cả các input hidden có class price-hidden trong container
            var priceInputs = container.querySelectorAll('input.price-hidden');

            priceInputs.forEach(function (priceInput) {
                var price = parseFloat(priceInput.value);
                var productId = priceInput.previousElementSibling ? priceInput.previousElementSibling.getAttribute('data-productid') : null;

                var qtyInput = container.querySelector('input[name="SelectedQuantities"][data-productid="' + productId + '"]');

                var qty = qtyInput ? parseInt(qtyInput.value) : 0;

                if (!isNaN(price) && !isNaN(qty)) {
                    total += price * qty;
                }
            });

            document.getElementById('totalPrice').innerText = total.toLocaleString() + '₫';

            var aspTextbox = document.getElementById('<%= txtTotalAmount.ClientID %>');
            if (aspTextbox) aspTextbox.value = total;
        }
        function onAddCustomerClick() {
            alert("Chức năng thêm khách hàng chưa được triển khai.");
        }

        function updateHiddenFields() {
            var productIds = [];
            var quantities = [];

            var items = document.querySelectorAll('.selected-product-item');
            items.forEach(function (item) {
                var id = item.querySelector('input[name="SelectedProductIDs"]').value;
                var qty = item.querySelector('input[name="SelectedQuantities"]').value;

                productIds.push(id);
                quantities.push(qty);
            });

            document.getElementById('<%= hfSelectedProductIDs.ClientID %>').value = productIds.join(',');
            document.getElementById('<%= hfSelectedQuantities.ClientID %>').value = quantities.join(',');
        }

        function showOrderDetails(orderId) {
            var modal = document.getElementById('detailsModal');
            var iframe = document.getElementById('detailsIframe');
            iframe.src = 'AdOrderDetails.aspx?id=' + orderId;
            modal.style.display = 'block';
        }

        function closeDetailsModal() {
            document.getElementById('detailsModal').style.display = 'none';
        }
    </script>
</asp:Content>
