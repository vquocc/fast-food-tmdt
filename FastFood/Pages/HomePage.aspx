<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="FastFood.Pages.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .slider img {
            width: 100%;
            height: 500px;
            object-fit: cover;
            border-radius: 10px;
        }
    </style>
    <div class="slider mb-5">
        <div>
            <img src="/Content/images/slide1.jpg" class="img-fluid rounded" alt="Slide 1" />
        </div>
        <div>
            <img src="/Content/images/slide2.jpg" class="img-fluid rounded" alt="Slide 2" />
        </div>
        <div>
            <img src="/Content/images/slide3.jpg" class="img-fluid rounded" alt="Slide 3" />
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.slider').slick({
                autoplay: true,
                autoplaySpeed: 3000,
                arrows: true,
                dots: true,
                infinite: true,
                speed: 500,
                fade: false,
                cssEase: 'linear'
            });
        });
    </script>
</asp:Content>
