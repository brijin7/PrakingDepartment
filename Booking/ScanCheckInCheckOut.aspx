<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScanCheckInCheckOut.aspx.cs"
    Inherits="ScanCheckInCheckOut" EnableEventValidation="false" %>

<!DOCTYPE html>
<html style="overflow-x: hidden;">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0,shrink-to-fit=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Smart Check In & Check Out</title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.0/jquery.min.js"></script>
    <!-- jQuery CDN - Slim version (=without AJAX) -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <!-- Popper.JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js" integrity="sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ" crossorigin="anonymous"></script>
    <%--Popup Related--%>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <!-- Bootstrap CSS CDN -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" integrity="sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4" crossorigin="anonymous">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"
        integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <%-- Style and Script for the Time Picker--%>
    <!-- Bootstrap JS -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js" integrity="sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm" crossorigin="anonymous"></script>

    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblGridIn.ClientID %>").style.display = "none";
                document.getElementById("<%=lblGridIn.ClientID %>").innerText = "";
                var BookingId = document.getElementById('<%=txtBookingId.ClientID %>');
                BookingId.focus();
            }, seconds * 2000);
        };

        function HideLabelout() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblout.ClientID %>").style.display = "none";
                document.getElementById("<%=lblout.ClientID %>").innerText = "";
                var BookingId = document.getElementById('<%=txtBookingId.ClientID %>');
                BookingId.focus();
            }, seconds * 2000);
        };

        function AlreadyLabelout() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblAlreadyOut.ClientID %>").style.display = "none";
                document.getElementById("<%=lblAlreadyOut.ClientID %>").innerText = "";
                var BookingId = document.getElementById('<%=txtBookingId.ClientID %>');
                BookingId.focus();
            }, seconds * 2000);
        };

        function AlreadyLabelIn() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblAlreadyIn.ClientID %>").style.display = "none";
                document.getElementById("<%=lblAlreadyIn.ClientID %>").innerText = "";
                var BookingId = document.getElementById('<%=txtBookingId.ClientID %>');
                BookingId.focus();
            }, seconds * 2000);
        };
    </script>
    <%-- <script type="text/javascript">
        var myVar = setInterval(StartBox, 0);
        function StartBox() {
            var txtStart = document.getElementById('<%=txtBookingId.ClientID %>');
            txtStart.focus();
        }
    </script>--%>
    <link href="../Style/Booking.css" rel="stylesheet" />
    <style>
        .blink {
            animation: blink-animation 1s linear infinite;
            -webkit-animation: blink-animation 1s linear infinite;
        }

        @keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }

        @-webkit-keyframes blink-animation {
            50% {
                opacity: 0;
            }
        }
    </style>
    <style>
        .BtnOtp {
            border-radius: 20px;
            border: 1px solid #0cc800;
            background-color: #24b53c;
            color: #FFFFFF;
            font-size: 12px;
            font-weight: bold;
            padding: 8px 8px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            width: auto;
            cursor: pointer;
        }

        .btnResnd {
            width: auto;
            font-size: 12px;
            font-weight: bold;
            padding: 8px 8px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            cursor: pointer;
            border-radius: 25px;
        }

        .HeaderColor {
            padding: 10px;
            font-size: 1.8rem !important;
            font-family: 'Arial Rounded MT';
            height: 2em;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #3278be;
        }

        .sp3 {
            font-size: 3.0rem !important;
            font-weight: 600;
            display: flex;
            align-items: center;
            justify-content: center;
            -webkit-background-clip: text;
            color: #4f5a60;
            font-family: 'Roboto', sans-serif;
        }

        .sp4 {
            padding: -12px 52px 21px 2px;
            font-size: 2.0rem !important;
            font-weight: 400;
            display: flex;
            align-items: center;
            justify-content: center;
            -webkit-background-clip: text;
            font-family: 'Roboto', sans-serif;
        }

        .normal {
            background-color: #40F44336;
            border-radius: 10px;
            text-align: center;
            color: #0c770f;
        }

        .vip {
            background-color: #ffee2162;
            border-radius: 10px;
            text-align: center;
            color: #fb5100;
        }

        .reserved {
            background-color: #DFF6FF;
            border-radius: 10px;
            text-align: center;
            color: #3d84f9;
        }

        .booked {
            background-color: #90bdf4;
            border: #90bdf4 solid 2px;
            border-radius: 10px;
            text-align: center;
            color: #696a6a;
        }


        .modal-body {
            opacity: 1 !important;
            transform: translateY(1);
        }

        .model {
            opacity: 1;
            visibility: visible;
            z-index: 5;
        }

        .modal-body {
            max-width: 500px;
            opacity: 0;
            transform: translateY(-100px);
            transition: opacity 0.25s ease-in-out;
            width: 100%;
            z-index: 5;
        }

        .cardModal {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 32px;
            margin: 234px;
            max-width: 800px;
            width: 100%;
            height: 69%;
            max-height: 500px;
        }

        .cardModalSearch {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 21px;
            margin: 192px;
            max-width: 999px;
            width: 100%;
            height: fit-content;
            /*max-height: 500px;*/
        }

        .cardModalPass {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 32px;
            margin: 192px;
            max-width: 999px;
            width: 100%;
            height: 72%;
            max-height: 700px;
        }

        .feature__item {
            text-align: center;
        }


        .addFeature__item .btn__AddandSub {
            width: 3.5rem;
            height: 3.5rem;
            border-radius: 25%;
            border: 2px solid #2196f3;
            background-color: #2196f3;
            color: #fff;
            box-shadow: 0 2px 5px rgba(0,0,0,0.4);
            font-size: 2rem;
        }

        .addFeature__item .SlotAddandSub__text {
            width: 80%;
            height: 100%;
            border-radius: 0.5rem;
            font-size: 1.6rem;
            border: 2px solid #2196f3;
            margin: auto;
        }

        .feature__item {
            font-size: 1.6rem;
            text-transform: capitalize;
            font-weight: 500;
            margin: 0;
            background: #2196f3;
            padding: 0.4rem;
            color: #fff;
        }

        .feature__Amount {
            font-size: 2.4rem;
            text-transform: capitalize;
            font-weight: 500;
            color: #757575;
            margin: 0;
            text-align: center;
        }

        .btnfloorVehicle {
            border-radius: 8px 8px 0px 0px;
            border: 1px solid #262a2f;
            display: inline-block;
            cursor: pointer;
            font-size: -1px;
            padding: 4px 4px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #1b211a;
            outline: none;
            margin: 5px;
            /* margin-left: 5px; */
            background: #283138;
            color: #fff;
            font-weight: 700;
        }

        .btnfloorFeature {
            font-weight: 700;
            border-radius: 8px 8px 0px 0px;
            border: 1px solid #262a2f;
            display: inline-block;
            cursor: pointer;
            font-size: 28px;
            padding: 4px 4px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #1b211a;
            outline: none;
            margin-top: 5px;
            background: #283138;
            color: #fff;
            margin-left: -4px;
        }

        .extrasummary2 {
            border-width: 2px;
            border-color: #2196f373;
            border-radius: 13px;
            border-style: dashed;
            padding: 5px;
        }

        .parkinglogoimg {
            margin-top: -47px;
            margin-bottom: -40px;
            width: 52px;
            height: 48px;
        }

        .extracontainer2 {
            border-style: solid;
            border-width: 3px;
            border-color: #c8cacc73;
            /*margin-left: 6px;*/
            margin-top: -5px;
            border-radius: 13px;
            box-shadow: 0 7px 29px 0 rgb(100 100 111 / 20%);
        }

        .Pass {
            background-color: #fa4dd54a;
            border: #fa4dd54a solid 2px;
            border-radius: 10px;
            text-align: center;
            color: #900a86ab;
        }

        .checkout {
            background-color: #f687668a;
            border: #f687668a solid 2px;
            border-radius: 10px;
            text-align: center;
            color: #c54919;
        }

        .checkin {
            background-color: #5c40f436;
            border: #5c40f436 solid 2px;
            border-radius: 10px;
            text-align: center;
            color: #750c77;
        }

        .carimage {
            border-radius: 25px;
            padding-top: 6px;
            padding-left: 20px;
            padding-right: 20px !important;
            height: 48px;
            color: black;
        }

        .carimage2 {
            border-radius: 25px;
            padding-top: 6px;
            padding-left: 20px;
            padding-right: 20px !important;
            height: 48px;
            color: black;
        }

        .divcarname {
            margin-right: -40px;
            margin-left: -10px;
            background: white;
            border-radius: 30px;
            height: 30px;
            color: black;
        }

        .divcarname2 {
            margin-right: -40px;
            margin-left: -10px;
            background: #2196f3;
            border-radius: 30px;
            height: 30px;
            color: white;
        }

        .divcarname1 {
            margin-right: -40px;
            margin-left: -10px;
            background: #2196f3;
            border-radius: 30px;
            height: 30px;
            color: white;
        }

        .BarCodeTextStart {
            width: 100%;
            height: 40px;
            border-color: skyblue;
            /*border-radius: 30px;*/
            font-size: x-large !important;
            text-align: center;
            border: none;
            margin: 8px;
        }

        .divIcon {
            text-align: center;
            padding: 1rem;
            margin: 1rem;
            color: white;
        }

        .labels {
            font-weight: 900;
            color: #2196f3;
            font-size: 1.5rem;
        }

        .divIcon1 {
            text-align: center;
            padding: 1rem;
            margin: 1rem;
            color: #1ca4ff;
            transition: all 0.3s;
        }

        .DisplyCardPostion1 {
            border-width: 0px;
            position: fixed;
            width: 50%;
            height: 40%;
            box-shadow: rgba(0, 0, 0, 0.56) 0px 10px 70px 4px;
            background-color: #ffffff;
            left: 30%;
            top: 40%;
            border-bottom-left-radius: 25px;
            border-top-left-radius: 25px;
        }

        .cardoffer {
            box-shadow: rgba(17, 17, 26, 0.1) 0px 8px 24px, rgba(17, 17, 26, 0.1) 0px 16px 56px, rgba(17, 17, 26, 0.1) 0px 24px 80px;
            transition: 0.3s;
            width: 100%;
            height: auto;
            border-radius: 1.25rem;
            background: #fff;
            display: flex;
            flex-direction: column;
            padding: 50px;
            margin: 10px;
        }

        .divIcon {
            text-align: center;
            padding: 1rem;
            margin: 1rem;
            color: white;
            transition: all 0.3s;
        }

        .form-control {
            border: 1px solid #e5e5e6;
        }
    </style>
    <link href="../Style/BookingSlots.css" rel="stylesheet" />
    <link href="../Style/SlotSummary.css" rel="stylesheet" />
    <link href="../Style/Navigation.css" rel="stylesheet" />
    <link href="../Style/Nav.css" rel="stylesheet" />
    <link href="../Style/ContentPage.css" rel="stylesheet" />
    <link href="../Style/Table.css" rel="stylesheet" />
    <%--Date Picker--%>
    <link href="../Style/DatePicker.css" rel="stylesheet" />
    <!-- Material design Cdn -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/MaterialDesign-Webfont/6.7.96/css/materialdesignicons.min.css" integrity="sha512-q0UoFEi8iIvFQbO/RkDgp3TtGAu2pqYHezvn92tjOq09vvPOxgw4GHN3aomT9RtNZeOuZHIoSPK9I9bEXT3FYA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <!-- Material design Icon -->
    <script src="https://code.iconify.design/2/2.2.1/iconify.min.js"></script>
</head>
<body>
    <form id="form" runat="server">

        <div class="row mb-2" style="margin-top: 10px;">
            <div class="col-sm-7 col-xs-12">
                <%--  <asp:Image ID="img" runat="server" CssClass="ImgStyle" ImageUrl="~/images/parkllogo.png" Style="margin-left: 1rem;" />--%>
                <div style="display: flex;">
                    <div class="col-sm-2 col-xs-12" style="text-align: center;">
                        <asp:Image runat="server" ImageUrl="~/images/parking.svg" CssClass="parkinglogoimg" />
                    </div>
                    <div class="col-sm-10 col-xs-12" style="display: flex">
                        <h1 class="ParkingName">
                            <asp:Label ID="lblParkingName" runat="server" />
                        </h1>
                        &nbsp; / &nbsp;
                            <h3 class="BranchName">
                                <asp:Label ID="lblBranchName" runat="server" /></h3>
                        &nbsp; - &nbsp; 
                            <h5 class="FormName mt-1">Check In / Check Out</h5>

                    </div>
                </div>
            </div>
            <div class="col-sm-5 col-xs-12">
                <div class="row">
                    <div class="col-sm-5 col-xs-12">
                        <div class="DateTime">
                            <h3 id="Date"></h3>
                            &nbsp; &nbsp;
                                <h3 id="Time"></h3>
                        </div>
                    </div>
                    <div class="col-sm-7 col-xs-12" style="margin-left: -3rem;">
                        <asp:LinkButton ID="ImageHome" runat="server" PostBackUrl="~/DashBoard.aspx" Style="cursor: pointer;"> 
                             <span><i class="fa-solid fa-house" style="font-size: 22px; color:black;cursor:pointer;"></i></span>
                            Home</asp:LinkButton>
                        &nbsp; | &nbsp;
                             <span class="text-black fw-bold">
                                 <asp:Label ID="lblUserName" runat="server" /></span>
                        &nbsp; | &nbsp;
                             <span class="text-black fw-bold">
                                 <asp:Label ID="lblUserRole" runat="server" /></span>
                    </div>
                </div>
            </div>
        </div>

        <div style="display: flex;">
            <div id="divBlockFloor" runat="server" visible="false" class="row col-4 col-sm-4 col-md-4 col-lg-4 col-xl-4" style="margin-top: 5px; margin-left: 0px;">
                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="display: inline-flex; height: 30px;">
                    <asp:DropDownList
                        ID="ddlblock"
                        runat="server" AutoPostBack="true"
                        TabIndex="1" OnSelectedIndexChanged="ddlblock_SelectedIndexChanged"
                        CssClass="form-control ">
                    </asp:DropDownList>
                </div>
                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="display: inline-flex; height: 30px;">
                    <asp:DropDownList
                        ID="ddlfloor" AutoPostBack="true"
                        runat="server" OnSelectedIndexChanged="ddlfloor_SelectedIndexChanged"
                        TabIndex="2"
                        CssClass="form-control ">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="divCount" runat="server" class="divcountbg row col-8 col-sm-8 col-md-8 col-lg-8 col-xl-8 justify-content-center">

                <span class="DotText">Booked (<asp:Label
                    ID="lblBooked" runat="server" class="pghr"
                    Style="font-size: 16px; font-weight: bold; display: inline;"></asp:Label>)</span>&nbsp;&nbsp;|&nbsp;&nbsp;
                
                 <span class="DotText">Pass (<asp:Label ID="lblpass" runat="server" class="pghr"
                     Style="font-size: 16px; font-weight: bold; display: inline;"></asp:Label>)</span>&nbsp;&nbsp;|&nbsp;&nbsp;
                
                <span class="DotText">Check In (<asp:Label
                    ID="lblcheckin" runat="server" class="pghr"
                    Style="font-size: 16px; font-weight: bold; display: inline;"></asp:Label>)</span>&nbsp;&nbsp;|&nbsp;&nbsp;
                
                <span class="DotText">Check Out (<asp:Label
                    ID="lblcheckout" runat="server" class="pghr"
                    Style="font-size: 16px; font-weight: bold; display: inline;"></asp:Label>)</span>

            </div>
        </div>
        <div id="divForm" runat="server" visible="true">

            <div class="row justify-content-center" id="divBtncheckinandout" runat="server" visible="false">

                <div class="mt-2 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control BarCodeTextStart" AutoPostBack="true"
                        placeholder="Scan QR Code"
                        OnTextChanged="txtBookingId_TextChanged" AutoComplete="Off"></asp:TextBox>
                </div>
            </div>
        </div>
        <div id="divCheckIn" runat="server" visible="false">
            <br />
            <div style="margin-left: auto; margin-right: auto; text-align: center; margin-top: -15px; margin-bottom: 11px;">
                <asp:Label ID="lblGridIn" runat="server" Style="color: #106d25"
                    CssClass="blink" Font-Bold="true" Font-Size="X-Large" Visible="false"></asp:Label>
                <asp:Label ID="lblAlreadyIn" runat="server" ForeColor="Blue"
                    CssClass="blink" Font-Bold="true" Font-Size="X-Large" Visible="false"></asp:Label>
            </div>

            <div class="table-responsive section">

                <asp:GridView
                    ID="gvCheckIn" runat="server" AllowPaging="True" DataKeyNames="bookingPassId"
                    CssClass="gvv display" AutoGenerateColumns="false" BorderStyle="None"
                    PageSize="5" OnRowDataBound="gvCheckIn_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booking Id">
                            <ItemTemplate>
                                <asp:Label ID="lblgvINbookingPassId" runat="server" Text='<%# "B"+Eval("bookingPassId") %>'></asp:Label>
                                <asp:Label
                                    ID="lblbookingPassIdIn"
                                    runat="server"
                                    Text='<%#Bind("bookingPassId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleHeaderId" runat="server" Text='<%#Bind("vehicleHeaderId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleType" runat="server" Text='<%#Bind("vehicleType") %> ' Visible="false"></asp:Label>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblpinNoIn"
                                    runat="server"
                                    Text='<%#Eval("pinNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle No.">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblvehicleNumberIn"
                                    runat="server"
                                    Text='<%#Bind("vehicleNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Type">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblvehicleTypeNameIn"
                                    runat="server"
                                    Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SlotDetails" Visible="false" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:DataList runat="server" ID="DataList1" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluserslotIdIn" runat="server" Text='<%# Bind("userSlotId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                        <asp:Label ID="lblSlotIdIn" runat="server" Text='<%# Bind("slotId") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>

                                    </ItemTemplate>
                                </asp:DataList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Check In" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:ImageButton ID="gvbtnCheckIn"
                                    runat="server"
                                    Text="Edit"
                                    src="../../images/check-in.png" alt="image"
                                    OnClick="gvbtnCheckIn_Click" OnClientClick="showLoader();" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div id="divGvCheckOut" runat="server" visible="false">
            <br />
            <div style="margin-left: auto; margin-right: auto; text-align: center; margin-top: -35px; margin-bottom: 11px;">
                <asp:Label ID="lblout" runat="server" Style="color: #c3263b"
                    CssClass="blink" Font-Bold="true" Font-Size="X-Large" Visible="false"></asp:Label>
                <asp:Label ID="lblAlreadyOut" runat="server" ForeColor="Blue"
                    CssClass="blink" Font-Bold="true" Font-Size="X-Large" Visible="false"></asp:Label>
            </div>
            <div class="table-responsive section">

                <asp:GridView
                    ID="gvCheckOut" runat="server" AllowPaging="True" DataKeyNames="bookingPassId"
                    CssClass="gvv display" AutoGenerateColumns="false" BorderStyle="None"
                    PageSize="5" OnRowDataBound="gvCheckOut_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booking Id">
                            <ItemTemplate>
                                <asp:Label ID="lblgvbookingPassId" runat="server" Text='<%# "B"+Eval("bookingPassId") %>'></asp:Label>
                                <asp:Label ID="lblbookingPassId" runat="server" Text='<%#Bind("bookingPassId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleHeaderId" runat="server" Text='<%#Bind("vehicleHeaderId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblinTime" runat="server" Text='<%#Bind("inTime") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleStatus" runat="server" Text='<%#Bind("vehicleStatus") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleType" runat="server" Text='<%#Bind("vehicleType") %> ' Visible="false"></asp:Label>
                                <asp:Label ID="lblextendDayHour" runat="server" Text='<%#Bind("extendDayHour") %> ' Visible="false"></asp:Label>
                                <asp:Label ID="lblextendAmount" runat="server" Text='<%#Bind("extendAmount") %> ' Visible="false"></asp:Label>
                                <asp:Label ID="lblextendtax" runat="server" Text='<%#Bind("extendtax") %> ' Visible="false"></asp:Label>
                                <asp:Label ID="lblremainingAmount" runat="server" Text='<%#Bind("remainingAmount") %> ' Visible="false"></asp:Label>
                                <asp:Label ID="lblinitialAmount" runat="server" Text='<%#Bind("initialAmount") %> ' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pin No." HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblpinNoOut"
                                    runat="server"
                                    Text='<%#Eval("pinNo")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle No.">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblvehicleNumber"
                                    runat="server"
                                    Text='<%#Bind("vehicleNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Type">
                            <ItemTemplate>
                                <asp:Label ID="lblvehicleTypeName" runat="server" Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OptionDetails" Visible="false" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:DataList runat="server" ID="DataList2" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluserslotIdOut" runat="server" Text='<%# Bind("userSlotId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                        <asp:Label ID="lblSlotIdOut" runat="server" Text='<%# Bind("slotId") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>

                                    </ItemTemplate>
                                </asp:DataList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check Out" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:ImageButton ID="gvbtnCheckout"
                                    runat="server"
                                    Text="Edit"
                                    src="../../images/Check-out.png" alt="image"
                                    OnClick="gvbtnCheckout_Click" OnClientClick="showLoader();" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <!-- Rules Model -->
            <asp:ScriptManager ID="ScriptManager"
                runat="server" />

            <asp:UpdatePanel ID="UpdatePanel1"
                UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <fieldset>

                        <div class="modal-wrapper" id="modal" runat="server" visible="false">
                            <div class="modal-body cardmodal">
                                <div class="modal-header">
                                    <div id="extended" runat="server">
                                        <h4 class="card-title">Extended <span class="Card-title-second" runat="server">Summary</span></h4>
                                    </div>
                                    <div id="remainingre" runat="server">
                                        <h4 class="card-title">To Pay <span class="Card-title-second" runat="server">Summary</span></h4>
                                    </div>
                                </div>

                                <div id="divextend" runat="server">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Block Name
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblBlockName" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Initial Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblInitialAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Floor Name
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblFloorName" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Remaining  Amount <span id="Span2" runat="server"
                                                            style="font-size: 11px; color: black; padding-left: 18px">(incl of all taxes)</span>
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblRemAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Booking Id
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblBookingId" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Extended   Amount <span id="Span1" runat="server"
                                                            style="font-size: 11px; color: black; padding-left: 16px">(incl of all taxes)</span>
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblExtendedAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Pin No.
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblPinNo" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" runat="server" visible="false">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Extended  Tax  Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lbltaxAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Total Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblTotalAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Vehicle No.
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblVehicleNo" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        To Pay
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblTopayAmount" runat="server" Font-Bold="true" Font-Size="25px"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Time Extended <span id="dayorHour" runat="server"
                                                            style="font-size: 11px; color: black; padding-left: 24px"></span>
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblTimeExtended" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Payment Mode 
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:DropDownList ID="ddlPayment" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator
                                                        ID="rfvVehicleName"
                                                        runat="server"
                                                        ControlToValidate="ddlPayment"
                                                        CssClass="rfvClr"
                                                        ErrorMessage="Select Payment Mode" ValidationGroup="Check" InitialValue="0">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div id="divRemaining" runat="server" visible="false">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Block Name
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReBlockName" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Initial Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReInitialAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Floor Name
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReFloorName" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Remaining  Amount <span id="Span3" runat="server"
                                                            style="font-size: 11px; color: black; padding-left: 18px">(incl of all taxes)</span>
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReRemainingAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Booking Id
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReBookingId" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Total Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReTotalAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Pin No.
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblRePinNo" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        To Pay
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReToPay" runat="server" Font-Bold="true" Font-Size="25px"></asp:Label>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Vehicle No.
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReVehicleNo" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge">
                                                        Payment Mode 
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:DropDownList ID="ddlRepayment" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator1"
                                                        runat="server"
                                                        ControlToValidate="ddlRepayment"
                                                        CssClass="rfvClr"
                                                        ErrorMessage="Select Payment Mode"
                                                        ValidationGroup="Check" InitialValue="0">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>


                                <div class="row p-4 justify-content-end">
                                    <div class="mr-3">
                                        <asp:Button ID="btnCheckInPopup" CssClass="pure-material-button-contained btnBgColorAdd"
                                            Text="Pay and Check Out" ValidationGroup="Check" runat="server" OnClick="btnCheckInPopup_Click" OnClientClick="showLoader();" />
                                    </div>
                                    <div>
                                        <asp:Button ID="btnCancelPopup" CssClass="pure-material-button-contained btnBgColorCancel"
                                            Text="Cancel" CausesValidation="false" OnClick="btnCancelPopup_Click" OnClientClick="showLoader();"
                                            runat="server" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </fieldset>
                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
        <div>
        </div>

    </form>
    <!-- Bootstrap JS -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js" integrity="sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm" crossorigin="anonymous"></script>
    <!-- Grid ViewSearch -->
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script>
        function show() {
            document.getElementById('<%=modal.ClientID %>').style.display = "inline";
        }
        function hide() {
            document.getElementById('<%=modal.ClientID %>').style.display = "none";
        }
    </script>
    <script lang="javascript" type="text/javascript">
        var myVar = setInterval(myTimer, 0);
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        const DayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        function myTimer() {
            const d = new Date();
            const Time = d.toLocaleTimeString('en-US', { hour12: false });
            document.getElementById("Date").innerText = DayNames[d.getDay()] + ", " + d.getDate() + "-" + monthNames[d.getMonth()];
            document.getElementById("Time").innerText = Time;
        }
    </script>
    <script type="text/javascript">
        function successalert(sMsg) {
            swal({
                title: 'PayPre Parking',
                text: sMsg,
                icon: "success",
                timer: 3000
            });
        }

        function infoalert(sMsg) {
            swal({
                title: 'PayPre Parking',
                text: sMsg,
                icon: "info",
                timer: 3000
            });
        }
        function erroralert(sMsg) {
            swal({
                title: 'PayPre Parking',
                text: sMsg,
                icon: "error",
                timer: 3000
            });
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function AllowOnlyAmountAndDot(txt) {
            if (event.keyCode > 47 && event.keyCode < 58 || event.keyCode == 46) {
                var txtbx = document.getElementById(txt);
                var amount = document.getElementById(txt).value;
                var present = 0;
                var count = 0;

                if (amount.indexOf(".", present) || amount.indexOf(".", present + 1));
                {
                    // alert('0');
                }


                do {
                    present = amount.indexOf(".", present);
                    if (present != -1) {
                        count++;
                        present++;
                    }
                }
                while (present != -1);
                if (present == -1 && amount.length == 0 && event.keyCode == 46) {
                    event.keyCode = 0;
                    //alert("Wrong position of decimal point not  allowed !!");
                    return false;
                }

                if (count >= 1 && event.keyCode == 46) {

                    event.keyCode = 0;
                    //alert("Only one decimal point is allowed !!");
                    return false;
                }
                if (count == 1) {
                    var lastdigits = amount.substring(amount.indexOf(".") + 1, amount.length);
                    if (lastdigits.length >= 2) {
                        //alert("Two decimal places only allowed");
                        event.keyCode = 0;
                        return false;
                    }
                }
                return true;
            }
            else {
                event.keyCode = 0;
                //alert("Only Numbers with dot allowed !!");
                return false;
            }

        }
        function minmax(value, min, max) {
            if (parseInt(value) < min || isNaN(parseInt(value)))
                return min;
            else if (parseInt(value) > max)
                return max;
            else return value;

        }
        //function preventBack() { window.history.forward(1); }
        //setTimeout("preventBack()", 0);
        //window.onunload = function () { null };
    </script>
</body>
</html>
