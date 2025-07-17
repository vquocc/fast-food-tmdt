<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductItem.ascx.cs" Inherits="FastFood.Pages.Components.ProductItem" %>

<style>
    .product-card {
        width: 280px;
        background-color: white;
        border-radius: 12px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        overflow: hidden;
        transition: transform 0.3s ease;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }

        .product-card:hover {
            transform: translateY(-6px);
            box-shadow: 0 8px 20px rgba(0,0,0,0.2);
        }

        .product-card img {
            width: 100%;
            height: 180px;
            object-fit: cover;
            border-bottom: 1px solid #eee;
        }

    .product-info {
        padding: 16px;
    }

        .product-info h5 {
            font-size: 18px;
            margin-bottom: 8px;
            color: #333;
            font-weight: 600;
        }

        .product-info p {
            font-size: 16px;
            color: #E21836;
            font-weight: bold;
            margin-bottom: 12px;
        }

    .btn-add {
        background-color: #E21836;
        color: white;
        border: none;
        width: 100%;
        padding: 10px;
        border-radius: 8px;
        font-weight: 600;
        cursor: pointer;
        transition: background-color 0.3s ease;
    }

        .btn-add:hover {
            background-color: #c5112c;
        }

    .reComment {
        display: flex;
        justify-content: space-between;
    }
</style>

<div class="product-card product-item">
    <asp:Image ID="Image1" runat="server" />
    <div class="product-info">
        <h5>
            <span class="product-name">
                <asp:Literal ID="litProductName" runat="server" />
            </span>
        </h5>

        <p>
            <asp:Literal ID="litPrice" runat="server" />
            đ
        </p>
        <div class="reComment">
            <p>
                <asp:Literal ID="litAverageRating" runat="server" />
                <i class="bi bi-star-fill" style="color: yellow"></i>
            </p>
            <p>
                <asp:Literal ID="litReviewCount" runat="server" />
                <i class="bi bi-person-check-fill" style="color: black"></i>
            </p>
        </div>
        <asp:Button ID="AddToCart" runat="server" Text="Thêm vào giỏ hàng" CssClass="btn-add" OnClick="AddToCart_Click" />

    </div>
</div>
