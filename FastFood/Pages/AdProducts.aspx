<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdProducts.aspx.cs" Inherits="FastFood.Pages.AdProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceholder" runat="server">
    <h3 class="mb-4">Danh sách sản phẩm</h3>
    <asp:Button ID="btnShowAddModal" runat="server" CssClass="btn btn-success mb-3" Text="➕ Thêm sản phẩm" OnClientClick="showAddModal(); return false;" />
    <asp:GridView ID="gvProducts" runat="server" CssClass="table table-bordered table-striped table-hover"
        AutoGenerateColumns="False" DataKeyNames="ProductID" EmptyDataText="Không có sản phẩm nào." GridLines="None" OnRowCommand="gvProducts_RowCommand">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Tên sản phẩm" />
            <asp:BoundField DataField="CategoryName" HeaderText="Danh mục" />
            <asp:BoundField DataField="Description" HeaderText="Mô tả" />
            <asp:BoundField DataField="Price" HeaderText="Giá (VNĐ)" DataFormatString="{0:N0}" />
            <asp:TemplateField HeaderText="Hình ảnh">
                <ItemTemplate>
                    <img src='<%# Eval("Product_Image") %>' alt="Image" style="height: 50px;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="AverageRating" HeaderText="Đánh giá" />
            <asp:BoundField DataField="ReviewCount" HeaderText="Lượt đánh giá" />
            <asp:TemplateField HeaderText="Trạng thái">
                <ItemTemplate>
                    <%# (bool)Eval("IsActive") ? "Hoạt động" : "Ẩn" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreatedAt" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:TemplateField HeaderText="Thao tác">
                <ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-warning me-2"
                        CommandName="EditProduct" CommandArgument='<%# Eval("ProductID") %>'>
            📝 Sửa
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger"
                        CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>'
                        OnClientClick="return confirm('Bạn có chắc chắn muốn xoá sản phẩm này?');">
            🗑️ Xoá
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <div class="modal fade" id="editProductModal" tabindex="-1" aria-labelledby="editProductLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-warning text-white">
                    <h5 class="modal-title" id="editProductLabel">📝 Sửa sản phẩm</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Đóng"></button>
                </div>
                <asp:Label ID="lblEditError" runat="server" EnableViewState="false" />
                <div class="modal-body">
                    <asp:HiddenField ID="hfEditProductID" runat="server" />
                    <div class="mb-3">
                        <label>Tên sản phẩm</label>
                        <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>Mô tả</label>
                        <asp:TextBox ID="txtEditDescription" runat="server" CssClass="form-control" TextMode="MultiLine" />
                    </div>
                    <div class="mb-3">
                        <label>Giá</label>
                        <asp:TextBox ID="txtEditPrice" runat="server" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="mb-3">
                        <label>Ảnh</label>
                        <asp:FileUpload ID="fuEditImage" runat="server" CssClass="form-control" />
                        <asp:HiddenField ID="hfOldImage" runat="server" />
                        <small class="form-text text-muted">Để trống nếu không muốn thay đổi ảnh.</small>
                    </div>
                     <div class="mb-3">
                        <label>Danh mục</label>
                        <asp:DropDownList ID="ddlEditCategory" runat="server" CssClass="form-select" />
                    </div>
                    <div class="form-check mb-3">
                        <asp:CheckBox ID="chkEditIsActive" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label">Hoạt động</label>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSaveChanges" runat="server" CssClass="btn btn-success" Text="💾 Lưu thay đổi" OnClick="btnSaveChanges_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="addProductModal" tabindex="-1" aria-labelledby="addProductLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header bg-success text-white">
                    <h5 class="modal-title" id="addProductLabel">➕ Thêm sản phẩm mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Đóng"></button>
                </div>
                <asp:Label ID="lblAddProductError" runat="server" EnableViewState="false" />
                <div class="modal-body">
                    <div class="mb-3">
                        <label>Tên sản phẩm</label>
                        <asp:TextBox ID="txtAddName" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>Mô tả</label>
                        <asp:TextBox ID="txtAddDescription" runat="server" CssClass="form-control" TextMode="MultiLine" />
                    </div>
                    <div class="mb-3">
                        <label>Giá</label>
                        <asp:TextBox ID="txtAddPrice" runat="server" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="mb-3">
                        <label>Ảnh sản phẩm</label>
                        <asp:FileUpload ID="fuAddImage" runat="server" CssClass="form-control" />
                    </div>
                    <div class="mb-3">
                        <label>Danh mục</label>
                        <asp:DropDownList ID="ddlAddCategory" runat="server" CssClass="form-select" />
                    </div>
                    <div class="form-check mb-3">
                        <asp:CheckBox ID="chkAddIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                        <label class="form-check-label">Hoạt động</label>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSaveNewProduct" runat="server" CssClass="btn btn-primary" Text="💾 Lưu" OnClick="btnSaveNewProduct_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                </div>
            </div>
        </div>
    </div>
    <script>
        function showAddModal() {
            var myModal = new bootstrap.Modal(document.getElementById('addProductModal'));
            myModal.show();
        }
    </script>
</asp:Content>
