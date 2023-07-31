<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingSummary.aspx.cs" Inherits="BookingSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Pre-Parking</title>
    <link href="../fav.ico" rel="shortcut icon" type="image/x-icon" />
    <!-- Bootstrap CSS CDN -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" integrity="sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4" crossorigin="anonymous">
    <!-- Our Custom CSS -->
    <%--<link rel="stylesheet" href="Style/Gridview.css">--%>
    <link rel="stylesheet" href="Style/ContentPage.css">
    <link rel="stylesheet" href="Style/Navigation.css">
    <link rel="stylesheet" href="Style/Table.css">
    <link rel="stylesheet" href="Style/Nav.css">

    <!-- Font Awesome JS -->


    <!-- Material design Cdn -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/MaterialDesign-Webfont/6.7.96/css/materialdesignicons.min.css" integrity="sha512-q0UoFEi8iIvFQbO/RkDgp3TtGAu2pqYHezvn92tjOq09vvPOxgw4GHN3aomT9RtNZeOuZHIoSPK9I9bEXT3FYA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

    <!-- Material design Icon -->
    <script src="https://code.iconify.design/2/2.2.1/iconify.min.js"></script>

    <!-- jQuery CDN - Slim version (=without AJAX) -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <!-- Popper.JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js" integrity="sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ" crossorigin="anonymous"></script>

    <%--Popup Related--%>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>


    <%--Popup Related--%>
    <style>
        .divHome {
            background-image: url(../images/HomeBg4.jpg);
            background-size: cover;
            background-position: top;
            height: 88vh;
            position: relative;
            text-align: center;
            width: 99%;
            margin-left: 6px;
        }

        label {
            font-weight: 900;
            color: #2196f3;
            font-size: 1.9rem;
        }

        .Label1 {
            font-size: 1.9rem;
        }

        .card {
            box-shadow: 0 6px 8px 0 rgba(0, 0, 0, 0.38);
            transition: 0.3s;
            width: 258px;
            height: 132px;
            background-color: var(--white);
            padding-left: 16px;
        }

            .card:hover {
                box-shadow: 10px 8px 16px 0 rgba(0,0,0,0.2);
            }

        .cardIn {
            box-shadow: 0 6px 8px 0 rgba(0, 0, 0, 0.38);
            transition: 0.3s;
            width: 258px;
            height: 112px;
            background-color: var(--white);
            padding-left: 16px;
        }

        .container {
            padding: 2px 16px;
        }

        .gradient4 {
            /*background: rgba(102, 122, 102, 0.6);*/
            background-color: white;
            align-content: center;
        }

        .wrap {
            white-space: nowrap;
        }

        .content-wrapper {
            font-size: 1.1em;
            margin-bottom: 44px;
        }

            .content-wrapper:last-child {
                margin-bottom: 0;
            }

        .link {
            color: #121943;
        }

            .link:focus {
                box-shadow: 0px 0px 0px 2px #121943;
            }

        .input-wrapper {
            display: flex;
            flex-direction: column;
        }

            .input-wrapper .label {
                align-items: baseline;
                display: flex;
                font-weight: 700;
                justify-content: space-between;
                margin-bottom: 8px;
            }

            .input-wrapper .optional {
                color: #5a72b5;
                font-size: 0.9em;
            }

            .input-wrapper .input {
                border: 1px solid #5a72b5;
                border-radius: 4px;
                height: 40px;
                padding: 8px;
            }

        code {
            background: #e5efe9;
            border: 1px solid #5a72b5;
            border-radius: 4px;
            padding: 2px 4px;
        }

        .modal-header {
            align-items: baseline;
            display: flex;
            justify-content: space-between;
        }

        .close {
            background: none;
            border: none;
            cursor: pointer;
            display: flex;
            height: 16px;
            text-decoration: none;
            width: 16px;
            box-sizing: unset;
        }

            .close svg {
                width: 16px;
                box-sizing: unset;
            }

        .modal-wrapper {
            align-items: center;
            background: rgba(0, 0, 0, 0.7);
            bottom: 0;
            display: flex;
            justify-content: center;
            left: 0;
            position: fixed;
            right: 0;
            top: 0;
        }

        #modal {
            transition: opacity 0.25s ease-in-out;
        }

            #modal:target {
                opacity: 1;
                visibility: visible;
            }

                #modal:target .modal-body {
                    opacity: 1;
                    transform: translateY(1);
                }

            #modal .modal-body {
                max-width: 700px;
                opacity: 1;
                overflow-y: scroll;
                max-height: 920px;
                height: 800px;
                transform: translateY(-100px);
                transition: opacity 0.25s ease-in-out;
                width: 100%;
                z-index: -1;
            }

        .cardmodal {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 32px;
            margin-left: 21px;
            margin-top: 200px;
            max-width: 700px;
            width: 100%;
            background: #ffffff;
        }

        .w3-border {
            border: 1px solid #ccc !important;
            padding: 0.01em 16px;
            border-radius: 16px;
            box-sizing: inherit;
            text-align: center;
        }

        .w3-borders {
            border: 1px solid #cccccc8a !important;
            padding: 0.01em 16px;
            border-radius: 6px;
            box-sizing: inherit;
        }

        hr {
            border-top: 3px dotted #887b7b4f;
            color: #fff;
            background-color: #fff;
            width: 92%;
        }
    </style>

</head>
<body class="section">
    <form id="form" runat="server">

        <div class="wrapper">
            <div class="main">
                <a id="btn-navig" class="btn-navig" href="#">
                    <i class="fas fa-bars open icon"></i>
                </a>

                <asp:Image ID="img" runat="server" CssClass="ImgStyle" ImageUrl="~/images/parking.svg" />
            </div>
        </div>

        <br />
        <div class="divHome row justify-content-center">
        </div>
        <div class="modal-wrapper" id="modal" runat="server">
            <div class="modal-body cardmodal section">
                <div id="divextend" runat="server" class="w3-border">
                    <div style="text-align: center">
                        <asp:Image ID="Image1" runat="server" Style="width: 196px; margin-top: 10px;"
                            ImageUrl="~/images/parkinglogo.png" />
                    </div>
                    <asp:Label ID="lblConfirm" runat="server" Text="Your booking is confirmed!"
                        Style="color: #339933; font-size: 24px; font-weight: 800;"></asp:Label>
                    <div style="text-align: center; margin-top: 10px;">
                        <asp:Label ID="lblBookingId" runat="server" Text="Booking Id"
                            Style="font-size: 20px; color: darkgrey"></asp:Label>
                        - 
                        <asp:Label ID="lblBookingText" runat="server"
                            Style="color: black; font-size: 21px; font-weight: 800;"></asp:Label>
                        <div style="text-align: center; margin-top: 10px;">
                            <asp:Label ID="Label13" runat="server" Text="Pin No."
                                Style="font-size: 20px; color: darkgrey"></asp:Label>
                            - 
                        <asp:Label ID="lblPinNo" runat="server"
                            Style="color: black; font-size: 21px; font-weight: 800;"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div style="text-align: left; margin-left: 22px; margin-right: 35px;">
                        <div class="w3-borders" style="background-color: #a9a9a926">
                            <div class=" row col-xs-12">
                                <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12" style="padding-top: 10px">
                                    <asp:Image ID="imgEmpPhotoPrev" runat="server" 
                                        Width="100%" />
                                </div>
                                <div class="col-sm-8 col-md-8 col-lg-8 col-xs-12">
                                    <div class="row" style="padding: 3px;">
                                        <div class="col-sm-8 col-md-8 col-lg-8 col-xs-12">
                                            <asp:Label ID="lblBranchName" runat="server" CssClass="card-title" Style="font-size: 2.1rem;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px;">
                                        <div class="col-sm-8 col-md-8 col-lg-8 col-xs-12">
                                            <%-- <asp:Label ID="lblTime" runat="server"
                                                Style="font-size: 15px; color: #948a8a"></asp:Label>
                                            <span style="font-size: 20px; color: #948a8a">|</span>--%>
                                            <asp:Label ID="lblDate" runat="server"
                                                Style="font-size: 15px; color: #948a8a"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label1" runat="server" Text="Block Name"
                                                Style="font-size: 15px; color: darkgrey"></asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                            :
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            <asp:Label ID="lblBlockName" runat="server"
                                                Style="color: black; font-size: 16px; font-weight: 800;"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label2" runat="server" Text="Floor Name"
                                                Style="font-size: 15px; color: darkgrey"></asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                            :
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            <asp:Label ID="lblFloorName" runat="server"
                                                Style="color: black; font-size: 16px; font-weight: 800;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label3" runat="server" Text="User Name"
                                                Style="font-size: 15px; color: darkgrey"></asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                            :
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            <asp:Label ID="lblUserName" runat="server"
                                                Style="color: black; font-size: 16px; font-weight: 800;"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label4" runat="server" Text="Vehicle No."
                                                Style="font-size: 15px; color: darkgrey"></asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                            :
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            <asp:Label ID="lblVehicleNo" runat="server"
                                                Style="color: black; font-size: 16px; font-weight: 800;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label5" runat="server" Text="Vehicle Type"
                                                Style="font-size: 15px; color: darkgrey"></asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                            :
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            <asp:Label ID="lblVehicleType" runat="server"
                                                Style="color: black; font-size: 16px; font-weight: 800;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="lblSlot" runat="server" Text="Slot"
                                                Style="color: black; font-size: 17px; font-weight: 800;"></asp:Label>
                                        </div>
                                        <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                            :
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            <asp:Label ID="txtSlot" runat="server"
                                                Style="color: black; font-size: 17px; font-weight: 800;"></asp:Label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<br />--%>
                    <%--<div style="text-align: left; margin-left: 22px; margin-right: 35px;">
                        <span style="font-size: 20px; color: #948a8a;">Instructions</span>
                        <div class="w3-borders">

                            <asp:DataList runat="server" ID="dtlInstructions" RepeatDirection="Vertical" CssClass="col-xs-12">
                                <ItemTemplate>
                                    <span style="font-size: 15px; color: green">*</span>
                                    <asp:Label ID="lblInstructions" runat="server" Text='<%# Bind("instructions") %>' Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </div>--%>

                    <%--             <div style="text-align: left; margin-left: 22px; margin-right: 35px;">
                        <span style="font-size: 20px; color: #948a8a;">Other Items</span>
                        <div class="w3-borders">
                            <asp:DataList runat="server" ID="dtlOtherItems" RepeatDirection="Vertical"
                                CellSpacing="50">
                                <HeaderTemplate>
                                    <asp:Label ID="Label5" runat="server" Text="Item"
                                        Style="font-size: 15px; color: darkgrey"></asp:Label>
                                    <asp:Label ID="Label6" runat="server" Text="Qty"
                                        Style="padding-left: 263px; font-size: 15px; color: darkgrey"></asp:Label>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("featureName") %>'
                                        Style="font-size: 15px; color: black"></asp:Label>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("count") %>'
                                        Font-Bold="true" Style="padding-left: 263px; font-size: 15px; color: black"></asp:Label>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </div>--%>
                    <br />
                    <%--   <div style="text-align: left; margin-left: 22px; margin-right: 35px;">
                        <span style="font-size: 20px; color: #948a8a;">Booking Summary</span>
                        <div class="w3-borders">
                            <div class=" row col-xs-12">
                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label10" runat="server" Text="Total Amount"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            ₹ 
                                        <asp:Label ID="lblTotalAmount" runat="server"
                                            Style="color: black; font-size: 16px;"></asp:Label>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label7" runat="server" Text="Parking Amount"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            ₹ 
                                        <asp:Label ID="lblParkingAmount" runat="server"
                                            Style="color: black; font-size: 16px;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px;">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label12" runat="server" Text="Parking Slot"
                                                Style="font-size: 13px; color: #716a6a"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            
                                        ₹ 
                                        <asp:Label ID="lblBookingSlotAmount" runat="server"
                                            Style="color: #716a6a; font-size: 13px;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label14" runat="server" Text="Parking Slot Tax"
                                                Style="font-size: 13px; color: #716a6a"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            
                                        ₹ 
                                        <asp:Label ID="lblBookingTaxAmount" runat="server"
                                            Style="color: #716a6a; font-size: 13px;"></asp:Label>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label16" runat="server" Text="Extra Features"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            ₹ 
                                        <asp:Label ID="lblOtherAmount" runat="server"
                                            Style="color: black; font-size: 16px;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label18" runat="server" Text="Extra Features"
                                                Style="font-size: 13px; color: #716a6a"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            
                                        ₹ 
                                        <asp:Label ID="lblOtherBookedAmount" runat="server"
                                            Style="color: #716a6a; font-size: 13px;"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label20" runat="server" Text="Extra Features Tax"
                                                Style="font-size: 13px; color: #716a6a"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            
                                        ₹ 
                                        <asp:Label ID="lblOtherTaxAmount" runat="server"
                                            Style="color: #716a6a; font-size: 13px;"></asp:Label>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label8" runat="server" Text="Extra Fee"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            ₹ 
                                        <asp:Label ID="lblExtraFee" runat="server"
                                            Style="color: black; font-size: 17px;"></asp:Label>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row" style="padding: 3px; background-color: #a9a9a926;">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label9" runat="server" Text="Amount Paid"
                                                Style="font-size: 21px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                                            ₹ 
                                        <asp:Label ID="lblPaidAmount" runat="server"
                                            Style="color: black; font-size: 21px; font-weight: bold"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>--%>

                    <div style="text-align: left; margin-left: 22px; margin-right: 35px;">
                        <span style="font-size: 20px; color: #948a8a;">Booking Summary</span>
                        <div class="w3-borders">
                            <div class=" row col-xs-12">
                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label10" runat="server" Text="Total Amount"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                            ₹ 
                                        <asp:Label ID="lblTotalAmount" runat="server"
                                            Style="color: black; font-size: 16px;"></asp:Label>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label7" runat="server" Text="Parking Amount"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                            ₹ 
                                        <asp:Label ID="lblParkingAmount" runat="server"
                                            Style="color: black; font-size: 16px;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px;">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label12" runat="server" Text="Parking Slot"
                                                Style="font-size: 13px; color: #716a6a"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                           
                                        ₹ 
                                        <asp:Label ID="lblBookingSlotAmount" runat="server"
                                            Style="color: #716a6a; font-size: 13px;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label14" runat="server" Text="Parking Slot Tax"
                                                Style="font-size: 13px; color: #716a6a"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                           
                                       ₹ 
                                          
                                        <asp:Label ID="lblBookingTaxAmount" runat="server"
                                            Style="color: #716a6a; font-size: 13px;text-align:right"></asp:Label>
                                                
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row" style="padding: 3px" runat="server" id="extrafeatures">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label16" runat="server" Text="Extra Features"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                         ₹ 
                                        <asp:Label ID="lblOtherAmount" runat="server"
                                            Style="color: black; font-size: 16px;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:DataList runat="server" ID="dtlextraFeatures" RepeatDirection="Vertical"
                                                CellSpacing="50">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("featureName") %>'
                                                        Style="font-size: 13px; color: #716a6a"></asp:Label>(Qty :
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("count") %>'
                                                        Font-Bold="true" Style="font-size: 13px; color: #716a6a"></asp:Label>
                                                                    )
                                                    <br />
                                                    <asp:Label ID="lblextaxra" runat="server" Text="tax"
                                                        Style="font-size: 13px; color: #716a6a"></asp:Label>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                            <asp:DataList runat="server" ID="dtlextraAmount" RepeatDirection="Vertical"
                                                width="100%">
                                                <ItemTemplate>
                                                   ₹ 
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Convert.ToDecimal(Eval("totalAmount")).ToString("0.00") %>'
                                                        Style="font-size: 13px; color: #716a6a"></asp:Label>
                                                    <br />
                                                ₹ 
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("tax") %>'
                                                        Style="font-size: 13px; color: #716a6a"></asp:Label>

                                                </ItemTemplate>
                                           
                                            </asp:DataList>
                                        </div>
                                    </div>
                                    <hr id="hrextra" runat="server" />
                                    <div class="row" style="padding: 3px" id="divextrafee" runat="server" >
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label8" runat="server" Text="Extra Fee"
                                                Style="font-size: 15px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                            ₹ 
                                        <asp:Label ID="lblExtraFee" runat="server"
                                            Style="color: black; font-size: 17px;"></asp:Label>
                                        </div>
                                    </div>
                                      <div class="row" style="padding: 3px">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:DataList runat="server" ID="dtlextrafee" RepeatDirection="Vertical"
                                                CellSpacing="50">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("extraFeesDetails") %>'
                                                        Style="font-size: 13px; color: #716a6a"></asp:Label>(Qty :
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("count") %>'
                                                        Font-Bold="true" Style="font-size: 13px; color: #716a6a"></asp:Label>
                                                                    )
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                            <asp:DataList runat="server" ID="dtlextrafeeAmount" RepeatDirection="Vertical"
                                                width="100%">
                                                <ItemTemplate>
                                                   ₹ 
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Convert.ToDecimal(Eval("extraFee")).ToString("0.00") %>'
                                                        Style="font-size: 13px; color: #716a6a"></asp:Label>
                                                   
                                                </ItemTemplate>
                                           
                                            </asp:DataList>
                                        </div>
                                    </div>
                                    <hr id="hrextrafee" runat="server" />
                                    <div class="row" style="padding: 3px; background-color: #a9a9a926;" runat="server" id="divAmountpaid">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label9" runat="server" Text="Amount Paid"
                                                Style="font-size: 21px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                            ₹ 
                                        <asp:Label ID="lblPaidAmount" runat="server"
                                            Style="color: black; font-size: 21px; font-weight: bold"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 3px; background-color: #a9a9a926;" runat="server" id="divTopay">
                                        <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                            <asp:Label ID="Label17" runat="server" Text="To Pay"
                                                Style="font-size: 21px; color: black" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                        </div>
                                        <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12 text-right">
                                            ₹ 
                                        <asp:Label ID="lblTopay" runat="server"
                                            Style="color: black; font-size: 21px; font-weight: bold"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <br />
                    <div style="text-align: left; margin-left: 22px; margin-right: 35px;">
                        <div class=" row col-xs-12">
                            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                <asp:Label ID="BookedDatetime" runat="server" Text="Booking Date & Time"
                                    Style="font-size: 15px; color: darkgrey" Font-Bold="true">
                                </asp:Label>
                                <br />
                                <asp:Label ID="lblBookedDateTime" runat="server" Text="Booking Date & Time"
                                    Style="font-size: 13px; color: black; font-weight: bold">
                                </asp:Label>
                            </div>
                            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                <asp:Label ID="Label11" runat="server" Text="Payment Type"
                                    Style="font-size: 15px; color: darkgrey" Font-Bold="true">
                                </asp:Label>
                                <br />
                                <asp:Label ID="lblPaymentType" runat="server"
                                    Style="font-size: 13px; color: black; font-weight: bold">
                                </asp:Label>
                            </div>

                            <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                <asp:Label ID="Label15" runat="server" Text="Payment Status"
                                    Style="font-size: 15px; color: darkgrey" Font-Bold="true">
                                </asp:Label>
                                <br />
                                <asp:Label ID="lblPaymentStatus" runat="server"
                                    Style="font-size: 13px; color: black; font-weight: bold">
                                </asp:Label>
                            </div>
                        </div>
                    </div>
                    <hr />
                </div>
            </div>
        </div>



        <asp:HiddenField ID="hfHasClassActive" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfConfigChk" runat="server"></asp:HiddenField>
    </form>
    <!-- Bootstrap JS -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js" integrity="sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm" crossorigin="anonymous"></script>

    <!-- Grid ViewSearch -->
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>

    <script>
        function show() {
            ;
            document.getElementById('<%=modal.ClientID %>').style.display = "inline";

        }
        function hide() {
            document.getElementById('<%=modal.ClientID %>').style.display = "none";

        }
    </script>

</body>
</html>
