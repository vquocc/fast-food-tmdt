<%@ Register TagPrefix="uc" TagName="ProductItem" Src="~/Pages/Components/ProductItem.ascx" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="MenuPage.aspx.cs" Inherits="FastFood.Pages.MenuPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .search-wrapper {
            width: 100%;
            display: flex;
            justify-content: center;
            padding: 20px;
        }

        .inputSearch {
            width: 100%;
            padding: 10px;
            font-size: 16px;
            border-radius: 8px;
            border: 1px solid #ccc;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            max-width: 800px;   
        }
    </style>
    <div class="d-flex flex-column align-items-center">
        <div class="search-wrapper">
            <input type="text" id="searchBox" placeholder="Tìm món..." class="inputSearch" />
        </div>
        <div class="product-list d-flex flex-wrap gap-4 justify-content-center mt-3">
            <asp:PlaceHolder ID="phProducts" runat="server" />
        </div>

    </div>
    <script>
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
