<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Shared/Site.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="FastFood.Pages.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .slider img {
            width: 100%;
            height: 500px;
            object-fit: cover;
            border-radius: 10px;
        }

        .carousel-item img {
            height: 500px;
            object-fit: cover;
        }

        .info-widgets {
            display: flex;
            justify-content: space-around;
        }

        #menu-item {
            box-sizing: border-box;
            border-radius: 15px;
            width: 400px;
            height: 200px;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            gap: 20px;
        }

            #menu-item:hover {
                background-color: white;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
            }

        .menu-image {
            transition: transform 0.3s ease;
        }

            .menu-image:hover {
                cursor: pointer;
                transform: scale(1.1);
            }
    </style>
    <div id="carouselExampleIndicators" class="carousel slide mb-5" data-bs-ride="carousel" data-bs-interval="3000">
        <div class="carousel-indicators">
            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0"
                class="active" aria-current="true" aria-label="Slide 1">
            </button>
            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1"
                aria-label="Slide 2">
            </button>
            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2"
                aria-label="Slide 3">
            </button>
        </div>

        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="/Content/images/slide1.jpg" class="d-block w-100 rounded" alt="Slide 1">
            </div>
            <div class="carousel-item">
                <img src="/Content/images/slide2.jpg" class="d-block w-100 rounded" alt="Slide 2">
            </div>
            <div class="carousel-item">
                <img src="/Content/images/slide3.jpg" class="d-block w-100 rounded" alt="Slide 3">
            </div>
        </div>

        <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators"
            data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators"
            data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
        </button>
    </div>

    <%--trang trí--%>
    <div class="d-flex align-items-center my-5">
        <div class="flex-grow-1 border-top border-2 border-secondary"></div>

        <div class="d-flex mx-3 gap-3">
            <div class="rounded-pill bg-danger" style="width: 19px; height: 50px;"></div>
            <div class="rounded-pill bg-danger" style="width: 19px; height: 50px;"></div>
            <div class="rounded-pill bg-danger" style="width: 19px; height: 50px;"></div>
        </div>

        <div class="flex-grow-1 border-top border-2 border-secondary"></div>
    </div>

    <div class="d-flex mb-5 align-items-center flex-column p-2 rounded-2" style="background-color: #fafafa; height: 300px">
        <div>
            <h3>MENU</h3>
        </div>
        <div class="d-flex " style="width: 100%;">
            <asp:Repeater ID="rptCategories" runat="server" OnItemCommand="rptCategories_ItemCommand">
                <ItemTemplate>
                    <div id="menu-item">
                        <div class="menu-image">
                            <img style="width: 150px; height: 150px" src='<%# Eval("Img_url") %>' />
                        </div>
                        <h6>
                            <asp:LinkButton ID="lnkCategory" runat="server" CommandName="ViewProducts" CommandArgument='<%# Eval("CategoryID") %>' CssClass="text-decoration-none text-dark">
                        <%# Eval("CategoryName") %>
                            </asp:LinkButton>
                        </h6>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

    </div>
    <div class="info-widgets">
        <div class="d-flex flex-column align-items-center" style="width: 150px;">
            <div>
                <img src="https://kfcku.com/assets/theme/img/icon-deals.svg" />
            </div>
            <div class="text-uppercase fw-bold text-center">EVEryday saver deals</div>
            <div class="text-center">Find interesting promos and vouchers only at KFC.</div>
        </div>
        <div class="d-flex flex-column align-items-center" style="width: 150px;">
            <div>
                <img src="https://kfcku.com/assets/theme/img/icon-menu.svg" />
            </div>
            <div class="text-uppercase fw-bold text-center">SEE our variety of menu</div>
            <div class="text-center">See our variety of menu. And order to enjoy it.</div>
        </div>
        <div class="d-flex flex-column align-items-center" style="width: 150px;">
            <div>
                <img src="https://kfcku.com/assets/theme/img/icon-location.svg" />
            </div>
            <div class="text-uppercase fw-bold text-center">FIND OUR Outlet Near you</div>
            <div class="text-center">Find our nearest shop and outlet in your location.</div>
        </div>
        <div class="d-flex flex-column align-items-center" style="width: 150px;">
            <div>
                <img src="https://kfcku.com/assets/theme/img/icon-envelop.svg" />
            </div>
            <div class="text-uppercase fw-bold text-center">LET’S CONNECT WITH US</div>
            <div class="text-center">Feedbacks or contact us for further discussion.</div>
        </div>
    </div>

    <%--Intagram--%>
    <div class="d-flex justify-content-around align-items-center mt-5 rounded-2" style="background-color: #333333; height: 100px">
        <div style="color: white; font-size: 34px;">Instagram FastFood</div>
        <div>
            <div style="background-color: #dc3545; padding: 10px; border-radius: 10px">
                <a href="#" style="color: white; font-size: 14px; text-decoration: none;">Click vào đi!!</a>
            </div>
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
