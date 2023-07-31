<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pass.aspx.cs" Inherits="BookingSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Pre-Parking</title>
    <!-- Bootstrap CSS CDN -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" integrity="sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4" crossorigin="anonymous">
    <!-- Our Custom CSS -->
    <%--<link rel="stylesheet" href="Style/Gridview.css">--%>
    <link rel="stylesheet" href="Style/ContentPage.css">
    <link rel="stylesheet" href="Style/Navigation.css">
    <link rel="stylesheet" href="Style/Table.css">
    <link rel="stylesheet" href="Style/Nav.css">
        <link href="../Style/Booking.css" rel="stylesheet" />
    <link href="../Style/ParkingPass.css" rel="stylesheet" />
   
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
          
            <div id="divPassTicket">
        <div id="DivPassTicket" runat="server" visible="true">
            <div class="pass-container">
                <div id="DivPassimg" class="passVIP u-clearfix" runat="server">
                    <div class="text-center">
                        <h2 class="pass-title">
                            <asp:Label ID="lblPassParkinngName" runat="server"></asp:Label></h2>
                    </div>
                    <div class="text-center">
                        <h2 class="pass-title-sub">
                            <asp:Label ID="lblPassBranchName" runat="server"></asp:Label></h2>
                    </div>
                    <div class="pass-body">
                        <span class="pass-user subtle">
                            <asp:Label ID="lblPassUserName" runat="server"></asp:Label></span>
                        <div class="row">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="Label1" runat="server" Text="Pass Id :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassId" runat="server"></asp:Label></span>
                            </div>
                            </div>
                          <div class="row">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblTicketPass" runat="server" Text="Pass Type :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblTicketPassType" runat="server"></asp:Label></span>
                            </div>
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassMobile" runat="server" Text="Mobile No :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassMobileNo" runat="server"></asp:Label></span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassFrmDate" runat="server" Text="Start Date :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassStartDate" runat="server"></asp:Label></span>
                            </div>
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassToDate" runat="server" Text="End Date :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassEndDate" runat="server"></asp:Label></span>
                            </div>
                        </div>
                        <div class="row ">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassMod" runat="server" Text="Pass Category :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassMode" runat="server"></asp:Label></span>
                            </div>
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassVehicle" runat="server" Text="Vehicle Type :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassVehicleType" runat="server"></asp:Label></span>
                            </div>
                        </div>
                        <div class="pass-read">
                            <i class="fa-solid fa-crown"></i>
                            <asp:Label ID="lblUserPassType" runat="server"></asp:Label>
                            Pass
                        </div>
                    </div>
                    <div>
                        <asp:Image ID="imgEmpPhotoPrev" runat="server" CssClass="pass-media" alt="Image" class="QR" />
                    </div>
                </div>
                <div class="pass-shadow"></div>
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

   
</body>
</html>
