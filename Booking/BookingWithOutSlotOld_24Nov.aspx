﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingWithOutSlotOld_24Nov.aspx.cs" Inherits="Booking_BookingWithOutSlot" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html style="overflow-x: hidden;" class="section">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Booking</title>
    <link href="fav.ico" rel="shortcut icon" type="image/x-icon" />
    <!-- jQuery CDN - Slim version (=without AJAX) -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <!-- Popper.JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js" integrity="sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ" crossorigin="anonymous"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <link rel="stylesheet" href="./style.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" integrity="sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4" crossorigin="anonymous">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"
        integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />
    <!-- Summary -->
    <%--Date Picker--%>
    <link href="../Style/DatePicker.css" rel="stylesheet" />
    <link href="../Style/Booking.css" rel="stylesheet" />
    <link href="../Style/BookingSlots.css" rel="stylesheet" />
    <link href="../Style/SlotSummary.css" rel="stylesheet" />
    <link href="../Style/Navigation.css" rel="stylesheet" />
    <link href="../Style/Nav.css" rel="stylesheet" />
    <link href="../Style/ContentPage.css" rel="stylesheet" />
    <link href="../Style/Table.css" rel="stylesheet" />
    <link href="../Style/ParkingPass.css" rel="stylesheet" />
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
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

        function SendOtp() {
            let seconds = 30;
            let button = document.querySelector('#<%=btnResend.ClientID%>');
            let txtmobileNo = document.querySelector('#<%=txtmobileNo.ClientID%>');
            let buttonConfrm = document.getElementById('<%=btnCfmOtp.ClientID %>');
            let divEnterOtp = document.getElementById('<%=divEnterOtp.ClientID %>');

            function incrementSeconds() {

                seconds = seconds - 1;
                if (seconds < 10) {
                    button.value = '00:0' + seconds;
                    button.disabled = true;
                    txtmobileNo.disabled = true;
                }
                else {
                    button.value = '00:' + seconds;
                    button.disabled = true;
                    txtmobileNo.disabled = true;
                }
                if (seconds == 0) {
                    seconds = 30;
                    button.value = "ReSend";
                    clearInterval(cancel);
                    button.disabled = false;
                    txtmobileNo.disabled = false;
                }
            }
            var cancel = setInterval(incrementSeconds, 1000);
        }
    </script>
    <style>
        .gvHead {
            background-color: #b4cfe5;
        }

        .BtnOtp {
            border-radius: 10px;
            border: 1px solid #0cc800;
            background-color: #24b53c;
            color: #FFFFFF;
            font-size: 12px;
            font-weight: bold;
            padding: 6px 6px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            cursor: pointer;
            width: auto;
        }

        .btnResnd {
            width: auto;
            font-size: 12px;
            font-weight: bold;
            padding: 6px 6px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            border-radius: 10px;
            cursor: pointer;
        }

        .extraFeatures {
            max-height: 25rem;
            height: 8rem;
            box-shadow: #00000059 0px 4px 16px !important;
            border-radius: 1rem;
            overflow: hidden;
            width: auto;
        }

        /*.parkinglogoimg {
            margin-top: -20px;
            width: 100px;
            height: 100px;
        }*/
        .parkinglogoimg {
            margin-top: -47px;
            margin-bottom: -40px;
            width: 52px;
            height: 48px;
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
            width: 100%;
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
            background: #ffffff;
            margin: 1.5rem;
            color: black;
            text-align: center;
        }

        .feature__Amount {
            font-size: 2rem;
            text-transform: capitalize;
            font-weight: 500;
            color: #757575;
            margin: -1rem 0rem 0rem 0rem;
            text-align: center;
        }

        .btnfloorVehicle {
            border-radius: 8px 8px 0px 0px;
            border: 1px solid #262a2f;
            display: inline-block;
            cursor: pointer;
            font-size: 28px;
            padding: 8px 8px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #1b211a;
            outline: none;
            margin: 3px;
            background: #283138;
            color: #fff;
            font-weight: 700;
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
            background-color: #4dfa6254;
            margin-left: 19px;
            border-radius: 10px;
            text-align: center;
        }

        .checkout {
            background-color: #f687668a;
            margin-left: 5px;
            border-radius: 10px;
            text-align: center;
        }

        .reserved {
            background-color: #4db7fa4a;
            margin-left: 5px;
            border-radius: 10px;
            text-align: center;
        }

        .Pass {
            background-color: #fa4dd54a;
            margin-left: 5px;
            border-radius: 10px;
            text-align: center;
        }

        .Booked {
            background-color: #9093944a;
            margin-left: 5px;
            border-radius: 10px;
            text-align: center;
        }

        .extrasummary2 {
            border-width: 2px;
            border-color: #2196f373;
            margin-left: 0px;
            margin-top: -5px;
            border-radius: 13px;
            border-style: dashed;
            padding: 6px;
        }

        .extrasummary1 {
            border-width: 2px;
            border-color: #2196f373;
            margin-top: -5px;
            border-radius: 13px;
            border-style: dashed;
            padding: 6px;
        }

        .extracontainer2 {
            border-style: solid;
            border-width: 3px;
            border-color: #c8cacc73;
            margin-left: -5px;
            margin-top: -5px;
            border-radius: 13px;
            box-shadow: 0 7px 29px 0 rgb(100 100 111 / 20%);
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
            background: #0c770f;
            border-radius: 30px;
            height: 30px;
            color: white;
        }

        .model {
            opacity: 1;
            visibility: visible;
            z-index: 1;
        }

        .modal-body {
            max-width: 500px;
            opacity: 1;
            transform: translateY(-100px);
            transition: opacity 0.25s ease-in-out;
            width: 100%;
            z-index: 1;
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
            padding: 32px;
            margin: 192px;
            max-width: 999px;
            width: 100%;
            height: fit-content;
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

        .labels {
            font-weight: 900;
            color: #2196f3;
            font-size: 1.5rem;
        }

        .BarCodeTextStart {
            width: 110%;
            height: 35px !important;
            border-color: skyblue;
            border-radius: 30px;
            font-size: 14px !important;
            text-align: center;
            border: none;
            margin: 8px;
        }


        .DisplyCardPostion1 {
            border-width: 0px;
            position: fixed;
            width: auto;
            height: auto;
            box-shadow: rgb(0 0 0 / 56%) 0px 10px 70px 4px;
            background-color: #ffffff;
            /*left: 25%;*/
            top: 25%;
            border-bottom-left-radius: 25px;
            border-top-left-radius: 25px;
            right: 25%;
            bottom: 25%;
        }

        /*.cardoffer {
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
        }*/

        .CustomGrid {
            margin-bottom: 1px;
        }

        .form-control {
            border: 1px solid #e5e5e6;
        }

        .Grpbooking {
            margin-top: -5px;
            border-radius: 10px;
            padding: 6px;
            /*box-shadow: #00000059 0px 2px 8px !important;*/
            border: ridge #70cdff38 2px;
        }
    </style>

</head>
<body>
    <form id="form" runat="server">

        <div style="top: 0; z-index: 99999;">
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
                            <h5 class="FormName mt-1">Booking</h5>

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
        </div>
        <%--   <div class="row" id="divFullPage" runat="server" style="width: 100%;">
            <div class="col-md-6 col-sm-6 col-lg-6 col-xl-6">
                <div id="divBlockFloor" runat="server" visible="false" class="row" style="margin-bottom: 5px; margin-left: 40px;">
                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="display: inline-flex; height: fit-content;">
                        <asp:Label
                            ID="lblblock"
                            runat="server"
                            Text="Block" CssClass="mt-2 mr-1">
                        </asp:Label>
                        <asp:DropDownList
                            ID="ddlblock"
                            runat="server" AutoPostBack="true"
                            TabIndex="1" OnSelectedIndexChanged="ddlblock_SelectedIndexChanged"
                            CssClass="form-control ">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rfdblock"
                            ValidationGroup="ParkingSlot"
                            ControlToValidate="ddlblock"
                            runat="server"
                            CssClass="rfvClr"
                            InitialValue="0"
                            ErrorMessage="Select Block">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="display: inline-flex; height: fit-content;">
                        <asp:Label
                            ID="lblfloor"
                            runat="server"
                            Text="Floor" CssClass="mt-2 mr-1">
                        </asp:Label>
                        <asp:DropDownList
                            ID="ddlfloor" AutoPostBack="true"
                            runat="server" OnSelectedIndexChanged="ddlfloor_SelectedIndexChanged"
                            TabIndex="2"
                            CssClass="form-control ">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rfvfloor"
                            ValidationGroup="ParkingSlot"
                            ControlToValidate="ddlfloor"
                            runat="server"
                            CssClass="rfvClr"
                            InitialValue="1"
                            ErrorMessage="Select Floor">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>--%>
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
                
                <span class="DotText">Reserved (<asp:Label
                    ID="lblReserved" runat="server" class="pghr"
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

        <asp:ScriptManager ID="ScriptManager" EnablePartialRendering="true"
            runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1"
            UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <fieldset>
                    <div class="modal-wrapper model" id="modalSearch" runat="server" visible="false">
                        <div id="divmodal" runat="server" class="modal-body cardModalSearch table-responsive section">
                            <div id="divModalColor" runat="server">
                                <div class="row" id="divNormalHeader" runat="server" style="padding: 1px; margin-left: 14px; font-size: 1.8rem;">
                                    <label for="lblmaxcharge" class="labels" style="font-size: 1.8rem;">
                                        Booking Id & Pin
                                    </label>
                                    &nbsp;
                                <asp:Label ID="Label15" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"> :</asp:Label>
                                    &nbsp;
                                     <asp:Label ID="lblBookingIn" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"></asp:Label>
                                    &nbsp;
                                     & &nbsp;
                                     <asp:Label ID="lblPinIn" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"></asp:Label>

                                </div>
                                <div class="row" id="divPassHeader" runat="server"
                                    style="padding: 1px; margin-left: 14px; font-size: 1.8rem;" visible="false">
                                    <label for="lblmaxcharge" class="labels" style="font-size: 1.8rem;">
                                        Booking Id 
                                    </label>
                                    &nbsp;
                                <asp:Label ID="Label7" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"> :</asp:Label>
                                    &nbsp;
                                     <asp:Label ID="lblBookingPassId" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"></asp:Label>

                                </div>
                                <div class="modal-header">
                                    <h4 class="card-title" id="CheckInorCheckout" runat="server" style="font-weight: bold"></span></h4>
                                    <asp:ImageButton ID="ImageCloseCheckInCheckOut" runat="server"
                                        OnClick="ImageCloseCheckInCheckOut_Click" OnClientClick="PlusOnclick();" ImageUrl="~/images/Close.svg" Width="25px"></asp:ImageButton>
                                </div>
                                <div id="divextend" runat="server" style="margin-right: 1rem">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Block & Floor
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblBlockName" runat="server" Font-Bold="true"></asp:Label>
                                                    &
                                     <asp:Label ID="lblFloorName" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Vehicle No.
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblvehicles" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <hr style="margin-bottom: 1rem;" />
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Payment Status
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblPaymentTypes" runat="server" Font-Bold="true"></asp:Label>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Remaining  Amount <span id="Span2" runat="server"
                                                            style="font-size: 11px; color: black; padding-left: 18px">(incl of all taxes)</span>
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    ₹&nbsp;<asp:Label ID="lblRemAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" runat="server" visible="false">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        ₹&nbsp;       Extended  Tax  Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="Label14" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>



                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
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

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" runat="server" visible="false">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Total Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    ₹&nbsp;   
                                    <asp:Label ID="Label16" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Extended   Amount <span id="Span1" runat="server"
                                                            style="font-size: 11px; color: black; padding-left: 16px">(incl of all taxes)</span>
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    ₹&nbsp;    
                                    <asp:Label ID="lblExtendedAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Booking Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    ₹&nbsp; 
                                    <asp:Label ID="lblInitialAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        To Pay
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right" style="font-size: 25px; color: green;">
                                                    ₹&nbsp; 
                                    <asp:Label ID="lblTopayAmount" runat="server" Font-Bold="true" Font-Size="25px"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6 ">
                                            <div id="divExtraFeeFeatures" runat="server" visible="false" style="padding-left: 27px; padding-bottom: 10px;">

                                                <asp:GridView ID="Extrafee" runat="server"
                                                    CssClass="CustomGrid table table-bordered table-condenced"
                                                    AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Add-On Service">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfeevehicleAccessoriesName"
                                                                    runat="server"
                                                                    Text=' <%# Eval("extraFeesDetails") %>'> </asp:Label>

                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" BackColor="White" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Count">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfeeCount"
                                                                    runat="server"
                                                                    Text='<%# Eval("Count") %>'> </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" BackColor="White" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <span runat="server">₹&nbsp;<asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                                    Text='<%# Convert.ToDecimal(Eval("extraFee")).ToString("0.00") %>'> </asp:Label>
                                                                </span>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" BackColor="White" />
                                                        </asp:TemplateField>


                                                    </Columns>

                                                    <HeaderStyle CssClass="gvHead" />
                                                    <AlternatingRowStyle CssClass="gvRow" />
                                                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                                </asp:GridView>

                                            </div>

                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
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
                                <div id="divRemaining" runat="server" visible="false" style="margin-right: 1rem">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Block & Floor
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblReBlockName" runat="server" Font-Bold="true"></asp:Label>
                                                    &
                                       <asp:Label ID="lblReFloorName" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
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

                                    </div>

                                    <hr style="margin-bottom: 1rem;" />
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Payment Status
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblPaymentStatusRe" runat="server" Font-Bold="true"></asp:Label>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Remaining  Amount <span id="Span3" runat="server"
                                                            style="font-size: 11px; color: black; padding-left: 18px">(incl of all taxes)</span>
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    ₹&nbsp;    
                                    <asp:Label ID="lblReRemainingAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" runat="server" visible="false">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Total Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    ₹&nbsp;
                                    <asp:Label ID="lblReTotalAmount" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Booking Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    ₹&nbsp; 
                                    <asp:Label ID="lblReInitialAmount" runat="server" Font-Bold="true"></asp:Label>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        To Pay
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right" style="font-size: 25px; color: green;">
                                                    ₹&nbsp;  
                                    <asp:Label ID="lblReToPay" runat="server" Font-Bold="true" Font-Size="25px"></asp:Label>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6 ">
                                            <div id="divReminingExtra" runat="server" visible="false" style="padding-left: 27px; padding-bottom: 10px;">
                                                <asp:GridView ID="dtlextrafeess" runat="server"
                                                    CssClass="CustomGrid table table-bordered table-condenced"
                                                    AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Add-On Service">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfeevehicleAccessoriesName"
                                                                    runat="server"
                                                                    Text=' <%# Eval("extraFeesDetails") %>'> </asp:Label>

                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" BackColor="White" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Count">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfeeCount"
                                                                    runat="server"
                                                                    Text='<%# Eval("Count") %>'> </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" BackColor="White" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <span runat="server">₹&nbsp;<asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                                    Text='<%# Convert.ToDecimal(Eval("extraFee")).ToString("0.00") %>'> </asp:Label>
                                                                </span>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" BackColor="White" />
                                                        </asp:TemplateField>


                                                    </Columns>

                                                    <HeaderStyle CssClass="gvHead" />
                                                    <AlternatingRowStyle CssClass="gvRow" />
                                                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                                </asp:GridView>
                                            </div>

                                        </div>


                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        >
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
                                <div id="divpass" runat="server" visible="false" style="margin-right: 1rem">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Block 
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblBlockIn" runat="server" Font-Bold="true"></asp:Label>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Vehicle No.
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblVehicleIn" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row" id="divBookingAmountParkedMins" runat="server" visible="true">

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Floor
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblFloorIn" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Booking Amount
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>

                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblBookingAmountPass" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <hr style="margin-bottom: 1rem;" />
                                    <div id="divpassDetailss" runat="server">
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            Pass Id
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>

                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                        <asp:Label ID="lblPassTransactionPassId" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            Mobile No
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>

                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                        <asp:Label ID="lblMobileNoPass" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            Pass Type
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>

                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                        <asp:Label ID="lblPassTypeINout" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            Category 
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>

                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                        <asp:Label ID="lblCategoryPAss" runat="server" Font-Bold="true"></asp:Label>
                                                        /
                                                  <asp:Label ID="lblVehicleTypeNamePass" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            Start Date
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>

                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                        <asp:Label ID="lblStartPass" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            End Date
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>

                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                        <asp:Label ID="lblEndPass" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <hr style="margin-bottom: 1rem;" />
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divReminingExtras" runat="server" visible="true" style="padding-left: 27px; padding-bottom: 10px;">
                                            <asp:GridView ID="dtlExtrafees" runat="server"
                                                CssClass="CustomGrid table table-bordered table-condenced"
                                                AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Add-On Service">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfeevehicleAccessoriesName"
                                                                runat="server"
                                                                Text=' <%# Eval("extraFeesDetails") %>'> </asp:Label>

                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Count">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfeeCount"
                                                                runat="server"
                                                                Text='<%# Eval("Count") %>'> </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <span runat="server">₹&nbsp;<asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                                Text='<%# Convert.ToDecimal(Eval("extraFee")).ToString("0.00") %>'> </asp:Label>
                                                            </span>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>


                                                </Columns>

                                                <HeaderStyle CssClass="gvHead" />
                                                <AlternatingRowStyle CssClass="gvRow" />
                                                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                            </asp:GridView>
                                        </div>

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divpassstatus" runat="server" visible="true">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Payment Status
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblPaymentTypePass" runat="server" Font-Bold="true"></asp:Label>

                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div id="divPayment" runat="server">
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            To Pay 
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right" style="color: green;">
                                                        ₹&nbsp;
                                    <asp:Label ID="lblTopayAmt" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
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
                                                        <asp:DropDownList ID="ddlPassPaymentMode" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator2"
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

                                </div>
                            </div>
                            <div class="row justify-content-end" style="margin-top: 3px">
                                <div class="mr-3">
                                    <asp:Button ID="btnCheckInPopup" CssClass="pure-material-button-contained btnBgColorAdd"
                                        Text="Pay and Check Out" ValidationGroup="Check" runat="server" OnClick="btnCheckInPopup_Click" />
                                </div>
                                <div>
                                    <asp:Button ID="btnCancelPopup" CssClass="pure-material-button-contained btnBgColorCancel"
                                        Text="Cancel" CausesValidation="false" OnClick="btnCancelPopup_Click" OnClientClick="PlusOnclick();"
                                        runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-wrapper model" id="modalPass" runat="server" visible="false">
                        <div class="modal-body cardModalPass">
                            <div class="modal-header">
                                <h4 class="card-title" id="H1" runat="server" style="font-weight: bold">Pass <span class="Card-title-second">Transaction</span></h4>
                                <asp:ImageButton ID="ImageClosePass" runat="server"
                                    OnClick="ImageClosePass_Click" ImageUrl="~/images/Close.svg" Width="25px"></asp:ImageButton>
                            </div>
                            <div class="row">
                                <div class="col-12 col-sm-4 col-md-4 col-lg-12 table-responsive section" style="overflow-y: hidden; background-color: #c6ebff; border-radius: 10px;">
                                    <asp:DataList ID="gvVehicleTypePass" runat="server" RepeatColumns="10"
                                        Visible="true" OnItemCommand="gvVehicleTypePass_ItemCommand">
                                        <ItemTemplate>
                                            <div id="divvehicless" runat="server" class="carimage" style="display: inline-flex; margin: 5px;">

                                                <asp:LinkButton ID="DivVehile" runat="server" OnClick="lblVehicleTypesPass_Click">
                                                    <div class="row" style="width: max-content;">
                                                        <div id="divcarnames" runat="server" class="col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname" style="margin-right: -40px;">
                                                            <asp:Label align="left" ID="lblvehicleNamePass" runat="server"
                                                                Text='<%# Bind("vehicleName") %>' Font-Bold="true" Font-Size="14px" Visible="true">
                                                            </asp:Label>
                                                        </div>
                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" id="DivVehileTypes" runat="server" style="margin-top: -14px;">
                                                            <asp:Image ID="lblVehicleTypes" Style="height: 60px !important; width: 60px;"
                                                                runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                                        </div>
                                                        <asp:Label align="left" ID="lblvehicleConfigIdPass" runat="server"
                                                            Text='<%# Bind("vehicleConfigId") %>' Visible="false" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </asp:LinkButton>

                                            </div>
                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                            <table style="height: 5px; width: 5px;">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </SeparatorTemplate>
                                    </asp:DataList>
                                    <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <div id="divPassType" runat="server" class="row" visible="false">
                                <div runat="server" class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                                    <asp:Label
                                        ID="lblMobileNo"
                                        runat="server"
                                        Text="Mobile No.">
                                    </asp:Label><span class="spanStar">*</span>
                                    <asp:TextBox
                                        ID="txtMobileNoPass"
                                        runat="server" onkeypress="return isNumber(event);" MaxLength="10" MinLength="10"
                                        TabIndex="3"
                                        CssClass="form-control ">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvMobileNo"
                                        ValidationGroup="ParkingPass"
                                        ControlToValidate="txtMobileNoPass"
                                        runat="server"
                                        CssClass="rfvClr"
                                        ErrorMessage="Enter Mobile No.">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revMobileNo" runat="server"
                                        ControlToValidate="txtMobileNoPass" ErrorMessage="Invalid Mobile No."
                                        ValidationExpression="[0-9]{10}" CssClass="rfvClr" ValidationGroup="ParkingPass"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                                    <asp:Label
                                        ID="lblPassCategory"
                                        runat="server"
                                        Text="Pass Category">
                                    </asp:Label><span class="spanStar">*</span>
                                    <asp:DropDownList
                                        ID="ddlPassCategory"
                                        runat="server" AutoPostBack="true"
                                        TabIndex="1" OnSelectedIndexChanged="ddlPassCategory_SelectedIndexChanged"
                                        CssClass="form-control">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="N">Normal</asp:ListItem>
                                        <asp:ListItem Value="V">VIP</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator
                                        ID="RfvPassCategory"
                                        ValidationGroup="ParkingPass"
                                        ControlToValidate="ddlPassCategory"
                                        runat="server"
                                        CssClass="rfvClr"
                                        InitialValue="0"
                                        ErrorMessage="Select Pass Category">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                                    <asp:Label
                                        ID="lblPassType"
                                        runat="server"
                                        Text="Pass Type">
                                    </asp:Label><span class="spanStar">*</span>
                                    <asp:DropDownList
                                        ID="ddlPassType" OnSelectedIndexChanged="ddlPassType_SelectedIndexChanged"
                                        runat="server" AutoPostBack="true"
                                        TabIndex="2"
                                        CssClass="form-control ">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator
                                        ID="rfvPasstype"
                                        ValidationGroup="ParkingPass"
                                        ControlToValidate="ddlPassType"
                                        runat="server"
                                        CssClass="rfvClr"
                                        InitialValue="0"
                                        ErrorMessage="Select Pass Type">
                                    </asp:RequiredFieldValidator>
                                </div>


                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                                    <asp:Label
                                        ID="Label4"
                                        runat="server"
                                        Text="Payment Type">
                                    </asp:Label><span class="spanStar">*</span>
                                    <asp:DropDownList
                                        ID="ddlpaymenttypepass"
                                        runat="server"
                                        TabIndex="4"
                                        CssClass="form-control ">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator
                                        ID="RfvPaymentType"
                                        ValidationGroup="ParkingPass"
                                        ControlToValidate="ddlpaymenttypepass"
                                        runat="server"
                                        CssClass="rfvClr"
                                        InitialValue="0"
                                        ErrorMessage="Select Payment Type">
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>
                            <div id="divpasssummary" runat="server" visible="false" class="cardpass  mt-5 mb-5 row">
                                <div id="divSummaryImg" runat="server">
                                </div>
                                <div class="cardpass__text col-12 col-sm-4 col-md-4 col-lg-4 col-xl-12">
                                    <h1>Parking  Pass Summary</h1>
                                    <div class="container__text__timing_time row">
                                        <div class="container__text__timing col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                            <h3>
                                                <asp:Label ID="lblVehicleType" runat="server"></asp:Label>
                                                <span style="font-size: 12px">Parking Charge </span></h3>
                                        </div>
                                        <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                            <p>
                                                <asp:Label ID="lblvehicleamt" runat="server"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="container__text__timing_time row">
                                        <div class="container__text__timing col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                            <h3>
                                                <asp:Label ID="lblTax" runat="server" Text="Tax"></asp:Label></h3>
                                        </div>
                                        <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                            <p>
                                                <asp:Label ID="lbltaxamt" runat="server"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="container__text__timing_time row">
                                        <div class="container__text__timing col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                            <h3>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total Amount"></asp:Label></h3>
                                        </div>
                                        <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                            <p>
                                                <asp:Label ID="lbltotalamt" runat="server"></asp:Label>
                                                <asp:Label ID="lblOffertotal" runat="server"></asp:Label>
                                                <asp:LinkButton ID="lnkRemove" CssClass="lnkremove"
                                                    OnClick="lnkRemove_Click" Visible="false"
                                                    runat="server">Remove <i class="fa-solid fa-delete-left"></i></asp:LinkButton>
                                            </p>
                                        </div>
                                    </div>
                                    <div id="divoffer" runat="server" class="container__text__timing_time row">
                                        <div class="container__text__timing col-12 col-sm-6 col-md-6 col-lg-6 col-xl-12">
                                            <asp:LinkButton ID="Lnkoffer" CssClass="flag-discount" Text="Click To Apply Offer %" OnClick="Lnkoffer_Click" runat="server"></asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="justify-content-end">
                                        <asp:Button ID="btnSubmit" runat="server" class="btn" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="ParkingPass"></asp:Button>
                                    </div>
                                </div>
                            </div>
                            <div id="modalpasssub" class="DisplyCard" runat="server" style="display: none">
                                <div class="DisplyCardPostion1 table-responsive section">
                                    <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 mt-3" style="text-align-last: center;">
                                        <asp:Label
                                            ID="lblOfflist"
                                            runat="server"
                                            Text="List Of Offers" Style="color: #1cb7fd; font-size: 24px; font-weight: bold;">                      
                                        </asp:Label>
                                        <asp:LinkButton ID="linkoffclose" CssClass="offerclose" runat="server" OnClick="linkoffclose_Click">
                <i class="fa-solid fa-xmark"></i></asp:LinkButton>
                                        <hr style="margin-top: 1rem !important; margin-bottom: 1rem !important; border: 0 !important; border-top: 1px solid rgb(28 183 253 / 27%) !important; width: 350px !important; margin-inline: auto !important;" />
                                    </div>
                                    <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                        <div class="wrap">
                                            <asp:DataList ID="dlOfferDetails" runat="server" RepeatColumns="3" RepeatDirection="Vertical"
                                                Visible="true">
                                                <ItemTemplate>
                                                    <div id="divpin" runat="server">
                                                        <div class="cardoffer">
                                                            <div class="row">
                                                                <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                                                    <asp:LinkButton align="left" ID="lblofferHeading" CssClass="lbloff"
                                                                        OnClick="lblofferHeading_Click" runat="server"
                                                                        Text='<%# Bind("offerHeading")  %>'></asp:LinkButton>
                                                                    <br />
                                                                    <asp:Label ID="Label1" runat="server" CssClass="lbloffsub" Text='<%# Eval("OfferType").ToString() == "F" ? "₹":"" %>'></asp:Label>
                                                                    <asp:Label align="left" ID="lblOfferValue" runat="server"
                                                                        Text='<%# Bind("OfferValue") %>' CssClass="lbloffsub">                                   
                                                                    </asp:Label><asp:Label ID="lblRs" runat="server" CssClass="lbloffsub" Text='<%# Eval("OfferType").ToString() == "P" ? "%":"" %>'></asp:Label>
                                                                    <asp:Label align="left" ID="lblOfferType" runat="server"
                                                                        Text='<%# Bind("OfferType") %>' Font-Bold="true" Visible="false"></asp:Label>
                                                                    <asp:Label align="left" ID="lblOfferId" runat="server"
                                                                        Text='<%# Bind("OfferId") %>' Font-Bold="true" Visible="false"></asp:Label>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <SeparatorTemplate>
                                                    <table style="height: 4px; width: 25px;">
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </SeparatorTemplate>
                                            </asp:DataList>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divPassTicket">
                                <div id="DivPassTicket" runat="server" visible="false">
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
                                                            <asp:Label ID="Label2" runat="server" Text="Pass Id :"></asp:Label></span>
                                                        <span class="pass-description-sub subtle">
                                                            <asp:Label ID="lblPAssIdPAss" runat="server"></asp:Label></span>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-6">
                                                        <span class="pass-description subtle">
                                                            <asp:Label ID="Label5" runat="server" Text="Pass Type :"></asp:Label></span>
                                                        <span class="pass-description-sub subtle">
                                                            <asp:Label ID="lblTicketPassTypePass" runat="server"></asp:Label></span>
                                                    </div>
                                                    <div class="col-6">
                                                        <span class="pass-description subtle">
                                                            <asp:Label ID="lblPassMobile" runat="server" Text="Mobile No :"></asp:Label></span>
                                                        <span class="pass-description-sub subtle">
                                                            <asp:Label ID="lblPassMobileNoPass" runat="server"></asp:Label></span>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-6">
                                                        <span class="pass-description subtle">
                                                            <asp:Label ID="Label8" runat="server" Text="Start Date :"></asp:Label></span>
                                                        <span class="pass-description-sub subtle">
                                                            <asp:Label ID="lblPassStartDatePass" runat="server"></asp:Label></span>
                                                    </div>
                                                    <div class="col-6">
                                                        <span class="pass-description subtle">
                                                            <asp:Label ID="Label10" runat="server" Text="End Date :"></asp:Label></span>
                                                        <span class="pass-description-sub subtle">
                                                            <asp:Label ID="lblPassEndDatePass" runat="server"></asp:Label></span>
                                                    </div>
                                                </div>
                                                <div class="row ">
                                                    <div class="col-6">
                                                        <span class="pass-description subtle">
                                                            <asp:Label ID="Label12" runat="server" Text="Pass Category :"></asp:Label></span>
                                                        <span class="pass-description-sub subtle">
                                                            <asp:Label ID="lblPassModePass" runat="server"></asp:Label></span>
                                                    </div>
                                                    <div class="col-6">
                                                        <span class="pass-description subtle">
                                                            <asp:Label ID="lblPassVehicle" runat="server" Text="Vehicle Type :"></asp:Label></span>
                                                        <span class="pass-description-sub subtle">
                                                            <asp:Label ID="lblPassVehicleTypePass" runat="server"></asp:Label></span>
                                                    </div>
                                                </div>
                                                <div class="pass-read">
                                                    <i class="fa-solid fa-crown"></i>
                                                    <asp:Label ID="lblUserPassType" runat="server"></asp:Label>
                                                    Pass
                                                </div>
                                            </div>
                                            <div>
                                                <asp:Image ID="imgEmpPhotoPrevPass" runat="server" CssClass="pass-media" alt="Image" class="QR" />
                                            </div>
                                        </div>
                                        <div class="pass-shadow"></div>
                                    </div>

                                </div>
                            </div>

                            <div id="divDownload" runat="server" visible="false">
                                <%--  <asp:Button id="btnExportPdf" runat="server" CssClass="pure-material-button-contained btnBgColorAdd"
                                     Text="Send SMS" OnClick="btnExportPdf_Click" />--%>
                                <%--<asp:Button
                                    ID="btn1"
                                    runat="server"
                                    Text="Send SMS"
                                    TabIndex="7"
                                    OnClick="btn1_Click"
                                    CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                                <asp:Button
                                    ID="btnExportPdf"
                                    runat="server"
                                    Text="Send SMS"
                                    TabIndex="7"
                                    OnClick="btnExportPdf_Click"
                                    CssClass="pure-material-button-contained btnBgColorAdd" />

                            </div>

                        </div>
                    </div>
                    <div id="Formbackground" runat="server" class="Formbackground">
                        <div id="divForm" runat="server" class="divformbg" visible="true" style="margin-top: -30px;">
                            <div class="row">

                                <div class="col-12 col-xs-8 col-sm-8 col-md-8 col-lg-8 col-xl-8 divVehiclebg table-responsive section" style="overflow-y: hidden; top: 5px;">
                                    <asp:DataList ID="gvVehicleType" runat="server" Visible="true" RepeatDirection="Horizontal"
                                        CssClass="mb-1" OnItemCommand="gvVehicleType_ItemCommand">
                                        <ItemTemplate>

                                            <div id="divvehicles" runat="server" class="carimage" style="display: inline-flex;">

                                                <asp:LinkButton ID="divvehicle" runat="server" OnClick="lblVehicleTypes_Click">
                                                    <div class="row" style="width: max-content;">
                                                        <div id="divcarname" runat="server" class="col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname">
                                                            <asp:Label ID="lblvehicleName" runat="server"
                                                                Text='<%# Bind("vehicleName") %>' Font-Bold="true" Font-Size="14px" Visible="true">
                                                            </asp:Label>
                                                        </div>

                                                        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" id="DivVehileTypes" runat="server" style="margin-top: -14px;">
                                                            <asp:Image ID="image" Style="height: 60px !important; width: 60px;"
                                                                runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                                        </div>
                                                        <asp:Label align="left" ID="lblisvehicleNumberRequired" runat="server"
                                                            Text='<%# Bind("isvehicleNumberRequired") %>' Visible="false" Font-Bold="true"></asp:Label>
                                                        <asp:Label align="left" ID="lblvehicleConfigId" runat="server"
                                                            Text='<%# Bind("vehicleType") %>' Visible="false" Font-Bold="true"></asp:Label>
                                                        <asp:Label align="left" ID="lblvehicleImageUrl" runat="server"
                                                            Text='<%# Bind("vehiclePlaceHolderImageUrl") %>' Visible="false" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </asp:LinkButton>

                                            </div>

                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                            <table style="height: 5px; margin-left: 12px;">
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </SeparatorTemplate>
                                    </asp:DataList>
                                    <asp:Label ID="lblVehicleTypeId" runat="server" Visible="false"></asp:Label>
                                </div>
                                <div class="col-12 col-xs-4 col-sm-4 col-md-4 col-lg-4 col-xl-4 divVehicleNobg" style="right: 4rem;">
                                    <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control BarCodeTextStart"
                                        AutoPostBack="true" OnTextChanged="txtBookingId_TextChanged" Style="text-transform: uppercase;"
                                        placeholder="Scan QR Code / Enter Vehicle No./ Mobile No./ Pin No."
                                        AutoComplete="Off"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="divtab" runat="server" id="DivSlotOtherStab">
                                <div class="row" style="margin-left: 11px !important; margin-bottom: -5px; margin-top: 5px">
                                    <div>
                                        <asp:Button
                                            ID="btnslottab"
                                            runat="server"
                                            Text="Booking"
                                            TabIndex="6"
                                            OnClick="btnslottab_Click"
                                            CausesValidation="false"
                                            CssClass="btnss" />
                                    </div>
                                    <div>
                                        <asp:Button
                                            ID="btnAddonservices"
                                            runat="server"
                                            Text="Add-On Service"
                                            TabIndex="6"
                                            OnClick="btnAddonservices_Click"
                                            CausesValidation="false"
                                            CssClass="btnss" />
                                    </div>


                                </div>
                            </div>

                            <div style="padding-left: 55px;">
                                <asp:CheckBox ID="ChkAddonService" runat="server" Style="zoom: 1.5;" Width="17px" Height="21px"
                                    OnCheckedChanged="ChkAddonService_CheckedChanged"
                                    AutoPostBack="true" /><b style="font-size: 19px; color: black;">Only Add-On Service</b>
                                <asp:CheckBox ID="ChkMobileNo" runat="server" Style="zoom: 1.5; margin-left: 55px" Width="17px" Height="21px"
                                    OnCheckedChanged="ChkMobileNo_CheckedChanged"
                                    AutoPostBack="true" /><b style="font-size: 19px; color: black;">Mobile No</b>
                            </div>
                        </div>

                        <div class="slot__container col-12 divslotbg " runat="server" id="DivSummaryFull" style="margin-top: 6px;">
                            <div class="row">
                                <div class="col-8 col-xs-8 col-sm-8 col-md-8 col-lg-8 col-xl-8 mt-3 table-responsive section">
                                    <div id="divSummary" class="Slot__Summary p-4 mb-2"
                                        runat="server" visible="false">

                                        <div class="DateAndFeatures__Container" style="margin-bottom: 0rem">
                                            <div class="Date__Container" style="margin-bottom: 1rem" id="divMain" runat="server">
                                                <div class="Date From__Date" style="width: 198px;" id="divBookingType" runat="server">
                                                    <asp:Label
                                                        ID="lblBookingType"
                                                        runat="server"
                                                        Text="Booking Type">
                                                    </asp:Label><span class="spanStar">*</span>
                                                    <asp:RadioButtonList
                                                        ID="ddlBookingType" Style="font-weight: bold;"
                                                        runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                                        TabIndex="4" OnSelectedIndexChanged="ddlBookingType_SelectedIndexChanged" CssClass="inline-rb">
                                                        <asp:ListItem Value="0" Selected="True">Normal</asp:ListItem>
                                                        <asp:ListItem Value="2">VIP</asp:ListItem>
                                                        <asp:ListItem Value="1">Pass</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator
                                                        ID="RfvBookingType"
                                                        ValidationGroup="BookingSlot"
                                                        ControlToValidate="ddlBookingType"
                                                        runat="server"
                                                        CssClass="rfvClr"
                                                        ErrorMessage="Select Booking Type">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="Date To__Date Grpbooking" style="width: 530px;">
                                                    <div id="divTimeType" runat="server" style="display: flex; margin-bottom: 0rem;">
                                                        <div class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                            <asp:Label
                                                                ID="lbltimetype"
                                                                runat="server"
                                                                Text="Duration">
                                                            </asp:Label>
                                                            <asp:RadioButtonList
                                                                ID="rbtnTimeType" AutoPostBack="true" Style="font-weight: bold;"
                                                                runat="server" OnSelectedIndexChanged="rbtnTimeType_SelectedIndexChanged"
                                                                TabIndex="8" RepeatDirection="Horizontal" CssClass="inline-rb">
                                                                <asp:ListItem Value="D">Daily</asp:ListItem>
                                                                <asp:ListItem Value="H">Hourly</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:RequiredFieldValidator
                                                                ID="RfvrTimeType"
                                                                ValidationGroup="BookingSlot"
                                                                ControlToValidate="rbtnTimeType"
                                                                runat="server"
                                                                CssClass="rfvClr"
                                                                ErrorMessage="Select Time Duration">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                            <div id="divfromdate" runat="server" visible="false">
                                                                <asp:Label
                                                                    ID="lblfromDate"
                                                                    runat="server"
                                                                    Text="From Date">
                                                                </asp:Label>
                                                                <asp:TextBox
                                                                    ID="txtFromDate"
                                                                    runat="server"
                                                                    CssClass=" form-control datePicker">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RfvFromDate"
                                                                    ValidationGroup="BookingSlot"
                                                                    ControlToValidate="txtFromDate"
                                                                    runat="server"
                                                                    CssClass="rfvClr"
                                                                    ErrorMessage="Select From Date">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                            <div id="divTodate" runat="server" visible="false">
                                                                <asp:Label
                                                                    ID="lblTodate"
                                                                    runat="server"
                                                                    Text="To Date">
                                                                </asp:Label>
                                                                <asp:TextBox
                                                                    ID="txtTodate"
                                                                    runat="server" Style="font-weight: bold;"
                                                                    TabIndex="9" OnTextChanged="txtTodate_TextChanged" AutoPostBack="true"
                                                                    CssClass="form-control datePicker classTargetDate">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="rfvTodate"
                                                                    ValidationGroup="BookingSlot"
                                                                    ControlToValidate="txtTodate"
                                                                    runat="server"
                                                                    CssClass="rfvClr"
                                                                    ErrorMessage="Enter To Date">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                            <div id="divFromTime" runat="server" visible="false">
                                                                <asp:Label
                                                                    ID="lblFromTime"
                                                                    runat="server"
                                                                    Text="From Time">
                                                                </asp:Label>
                                                                <asp:TextBox
                                                                    ID="txtfromtime"
                                                                    runat="server"
                                                                    AutoComplete="off"
                                                                    CssClass=" form-control timePicker"
                                                                    MaxLength="5"
                                                                    AutoPostBack="false"
                                                                    onkeypress="return isNumber(event);"></asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RfvFromTime"
                                                                    ValidationGroup="BookingSlot"
                                                                    ControlToValidate="txtfromtime"
                                                                    runat="server"
                                                                    CssClass="rfvClr"
                                                                    ErrorMessage="Select From Time">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                            <div id="divTotime" runat="server" visible="false">
                                                                <asp:Label
                                                                    ID="lblTotime"
                                                                    runat="server"
                                                                    Text="To Time" CssClass="lblFromDate lblDate ">
                                                                </asp:Label>

                                                                <asp:TextBox
                                                                    ID="txtTotime"
                                                                    runat="server"
                                                                    TabIndex="10" Style="font-weight: bold;"
                                                                    onkeypress="return isNumber(event);"
                                                                    MaxLength="5" AutoPostBack="true"
                                                                    OnTextChanged="txtTotime_TextChanged"
                                                                    CssClass="txtSummary form-control timePicker classTargetTime">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="Rfvtotime"
                                                                    ValidationGroup="BookingSlot"
                                                                    ControlToValidate="txtTotime"
                                                                    runat="server"
                                                                    CssClass="rfvClr"
                                                                    ErrorMessage="Select To Time">
                                                                </asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div style="display: flex; margin-bottom: 0rem;">
                                                        <div class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6" id="divMobileNo" runat="server">
                                                            <asp:Label
                                                                ID="lblMobNo"
                                                                runat="server"
                                                                Text="Mobile No. / Pass Id">
                                                            </asp:Label><span class="spanStar">*</span>
                                                            <asp:TextBox
                                                                ID="txtmobileNo"
                                                                runat="server"
                                                                TabIndex="7" Style="font-weight: bold;"
                                                                CssClass="txtSummary txtDate txtFromDate form-control"
                                                                OnTextChanged="txtmobileNo_TextChanged" AutoPostBack="true">
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator
                                                                ID="RfvMobNo"
                                                                ValidationGroup="BookingSlot"
                                                                ControlToValidate="txtmobileNo"
                                                                runat="server"
                                                                CssClass="rfvClr"
                                                                ErrorMessage="Enter Mobile No. / Pass Id">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6" id="divVehicleNo" runat="server">
                                                            <asp:Image ID="ImgSummary" runat="server" Style="height: 3.5rem; margin-top: -14px;" />
                                                            <asp:Label
                                                                ID="lblVehicleNo"
                                                                runat="server"
                                                                Text="Vehicle No.">
                                                            </asp:Label><span class="spanStar">*</span>
                                                            <asp:TextBox
                                                                ID="txtVehicleNo"
                                                                runat="server" placeholder="XX00XX0000" AutoComplete="On"
                                                                TabIndex="6" MaxLength="10" Style="text-transform: uppercase; font-weight: bold;"
                                                                CssClass="txtSummary txtDate txtFromDate form-control ">
                                                            </asp:TextBox>
                                                            <div style="display: -webkit-box;">
                                                                <asp:RequiredFieldValidator
                                                                    ID="RfvVehicleNo"
                                                                    ValidationGroup="BookingSlot"
                                                                    ControlToValidate="txtVehicleNo"
                                                                    runat="server"
                                                                    CssClass="rfvClr"
                                                                    ErrorMessage="Enter Vehicle No.">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator
                                                                    ID="RegularExpressionValidator3"
                                                                    ValidationGroup="BookingSlot"
                                                                    ControlToValidate="txtVehicleNo"
                                                                    ValidationExpression="^[A-Z|a-z]{2}[0-9]{2}[A-Z|a-z]{1,2}[0-9]{4}$"
                                                                    runat="server" Style="margin-left: -2rem"
                                                                    CssClass="rfvClr"
                                                                    ErrorMessage="Invalid Vehicle No.">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div style="display: flex; margin-bottom: 0rem">
                                                        <div runat="server" id="divPassId" visible="false">
                                                            <asp:Label
                                                                ID="lblPassId"
                                                                runat="server"
                                                                Text="Pass Id">
                                                            </asp:Label><span class="spanStar">*</span>
                                                            <asp:TextBox
                                                                ID="txtPassId" Style="font-weight: bold;"
                                                                runat="server" AutoPostBack="true"
                                                                TabIndex="5" OnTextChanged="txtPassId_TextChanged"
                                                                CssClass="form-control" Width="190px">
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator
                                                                ID="rfv"
                                                                ValidationGroup="BookingSlot"
                                                                ControlToValidate="txtPassId"
                                                                runat="server"
                                                                CssClass="rfvClr"
                                                                ErrorMessage="Enter Pass Id">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <%--  OTP Send Details--%>
                                                        <div class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 pb-2" id="divOtpDetails" runat="server" visible="false">
                                                            <div id="divSendOtp" runat="server" visible="false">
                                                                <asp:Button ID="btnSend" runat="server" CssClass="BtnOtp mt-4" ValidationGroup="BookingSlot"
                                                                    Text="Send OTP" OnClick="btnSend_Click" />
                                                            </div>
                                                            <div id="divEnterOtp" runat="server" visible="false">
                                                                <asp:Label
                                                                    ID="lblOTP"
                                                                    runat="server"
                                                                    Text="Enter OTP">
                                                                </asp:Label><span class="spanStar">*</span>
                                                                <asp:TextBox
                                                                    ID="txtOTP" onkeypress="return isNumber(event);"
                                                                    runat="server" MaxLength="6"
                                                                    TabIndex="8"
                                                                    CssClass="txtSummary txtDate txtFromDate form-control"
                                                                    Style="font-weight: bold;">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator3"
                                                                    ValidationGroup="OTP"
                                                                    ControlToValidate="txtOTP"
                                                                    runat="server"
                                                                    CssClass="rfvClr"
                                                                    ErrorMessage="Enter OTP">
                                                                </asp:RequiredFieldValidator>

                                                            </div>
                                                        </div>
                                                        <div class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 mt-4" id="divResend" runat="server" visible="false" style="margin-left: -1rem;">
                                                            <div style="display: flex; margin-top: 3px;">
                                                                <asp:Button ID="btnCfmOtp" runat="server" CssClass="BtnOtp ml-3" ValidationGroup="OTP"
                                                                    Text="Submit" OnClick="btnCfmOtp_Click" />
                                                                <asp:Button ID="btnResend" runat="server" Text="00:30" CssClass="btn-primary btnResnd ml-3"
                                                                    OnClick="btnResend_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <asp:HiddenField ID="hfOtp" runat="server" />



                                            <%--                                            <div class="row" id="divbtnextrafee" runat="server" style="margin-left: -1px !important;">
                                                <div>
                                                    <asp:Button
                                                        ID="btnextraFee"
                                                        runat="server"
                                                        Text="Add-On Service"
                                                        TabIndex="6"
                                                        OnClick="btnextraFee_Click"
                                                        CausesValidation="false"
                                                        CssClass="btnfloorVehicle" />
                                                </div>
                                            </div>--%>

                                            <div id="extraFee__container" runat="server"
                                                class="table-responsive section" style="overflow-x: hidden">
                                                <asp:Label ID="lblGvAccessoriesNo" runat="server"
                                                    Visible="false" Style="text-align: center !important; color: red"></asp:Label>
                                                <asp:DataList ID="dtlextraFee" RepeatColumns="3"
                                                    RepeatDirection="Horizontal" runat="server" Style="width: auto"
                                                    OnItemCommand="dtlextraFee_ItemCommand">

                                                    <ItemTemplate>
                                                        <div class="col-12">

                                                            <div class="extraFeatures mt-4 mb-4">
                                                                <div class="feature__item">
                                                                    <asp:LinkButton ID="lblgvFeeName" runat="server" CssClass="feature__name"
                                                                        Text='<%# Eval("vehicleAccessoriesName") %>'></asp:LinkButton>
                                                                    <asp:Label ID="lblgvFeetax" runat="server"
                                                                        CssClass="feature__name" Visible="false" Text='<%# Eval("tax") %>'></asp:Label>
                                                                    <asp:Label ID="lblgvFeeamount" runat="server"
                                                                        CssClass="feature__name" Visible="false" Text='<%# Eval("amount") %>'></asp:Label>
                                                                    <asp:Label ID="lblGvTimeSlabId" runat="server"
                                                                        CssClass="feature__name" Visible="false" Text='<%# Eval("timeslabId") %>'></asp:Label>
                                                                </div>
                                                                <div class="feature__Amount">
                                                                    <span>₹</span><asp:LinkButton ID="lblgvFeeTotalAmount" runat="server" Text='<%# Eval("totalAmount") %>'></asp:LinkButton>
                                                                </div>

                                                                </span>
                                                          <%-- <div class="addFeature__item">
                                                               <div class="row">
                                                                   <div class="col-4 text-right p-0">
                                                                       <asp:Button ID="Sub" runat="server" Text="-"
                                                                           CssClass="btn__AddandSub btn__sub btn__SubExtraFeatures"
                                                                           OnClick="Sub_Click" />
                                                                   </div>
                                                                   <div class="col-4">
                                                                       <asp:Button class="SlotAddandSub__text txtExtraFeatures form-control" runat="server"
                                                                           readonly="readonly" ID="valueCount" Text="0" />
                                                                   </div>
                                                                   <div class="col-4 text-left p-0">
                                                                       <asp:Button ID="plus" runat="server"
                                                                           CssClass="btn__AddandSub btn__Add btn__AddExtraFeatures"
                                                                           OnClick="plus_Click" Text="+" />
                                                                   </div>
                                                               </div>
                                                           </div>--%>
                                                            </div>

                                                        </div>
                                                    </ItemTemplate>
                                                    <SeparatorTemplate>
                                                        <table style="height: 4px; width: -20px;">
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </SeparatorTemplate>
                                                </asp:DataList>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-4 col-xs-4 col-sm-4 col-md-4 col-lg-4 col-xl-4 mt-3 table-responsive section">
                                    <div id="divslotSummary" class="Slot__Summary mb-3"
                                        runat="server">
                                        <div id="divSummaryColor" runat="server" style="background-color: #c1c0c36b; border-radius: 10px; padding: 10px">
                                            <div class="col-6">
                                                <h3 class="summary__header mb-2" style="margin-left: -15px; font-weight: 700; font-size: 2rem;">Summary</h3>
                                            </div>
                                            <%--   Passdetails--%>
                                            <div class="DateAndFeatures__Container slot__container" id="divPassdetails" runat="server" visible="false" style="margin-bottom: 0.5rem;">
                                                <div class="Date__Container">
                                                    <div class="Date From__Date">
                                                        <asp:Label ID="lblTicketPassId" runat="server" Text="Pass Id :" CssClass="lblPass"></asp:Label>
                                                        <asp:Label ID="lblTicketPassIdNo" runat="server" CssClass="lblPassSummary"></asp:Label>
                                                    </div>
                                                    <div class="Date To__Date">
                                                        <asp:Label ID="Label11" runat="server" Text="Vehicle Type :" CssClass="lblPass"></asp:Label>
                                                        <asp:Label ID="lblPassVehicleType" runat="server" CssClass="lblPassSummary"></asp:Label>

                                                    </div>
                                                </div>
                                                <div class="Date__Container">
                                                    <div class="Date From__Date">
                                                        <asp:Label ID="lblTicketPass" runat="server" Text="Pass Type :" CssClass="lblPass"></asp:Label>
                                                        <asp:Label ID="lblTicketPassType" runat="server" CssClass="lblPassSummary"></asp:Label>
                                                        <asp:Label ID="lblPassMobileNo" runat="server" Visible="false" CssClass="lblPassSummary"></asp:Label>
                                                    </div>
                                                    <div class="Date To__Date">
                                                        <asp:Label ID="lblPassMod" runat="server" Text="Category :" CssClass="lblPass"></asp:Label>
                                                        <asp:Label ID="lblPassMode" runat="server" CssClass="lblPassSummary"></asp:Label>

                                                    </div>
                                                </div>
                                                <div class="Date__Container">
                                                    <%--<div class="Date From__Date">
                                                        <asp:Label ID="lblPassFrmDate" runat="server" Text="Start :" CssClass="lblPass"></asp:Label>
                                                        <asp:Label ID="lblPassStartDate" runat="server" CssClass="lblPassSummary"></asp:Label>
                                                    </div>
                                                    <div class="Date To__Date">
                                                        <asp:Label ID="lblPassToDate" runat="server" Text="End :" CssClass="lblPass"></asp:Label>
                                                        <asp:Label ID="lblPassEndDate" runat="server" CssClass="lblPassSummary"></asp:Label>
                                                    </div>--%>
                                                    <div class="Date From__Date">
                                                        <asp:Label ID="lblPassFrmDate" runat="server" Text="Validity :" CssClass="lblPass"></asp:Label>
                                                        <asp:Label ID="lblPassStartDate" runat="server" CssClass="lblPassSummary"></asp:Label>
                                                        &nbsp;-&nbsp; 
                                                              <asp:Label ID="lblPassEndDate" runat="server" CssClass="lblPassSummary"></asp:Label>
                                                    </div>
                                                </div>
                                                <hr />
                                            </div>
                                            <div class="Feature__container">
                                                <div class="Feature__summary-container">

                                                    <div id="extrafeeandfeatursContainer" runat="server" class="extrasummary2 mt-2 mb-2" visible="false">
                                                        <div class="Feature__summary Feature__Extra" id="header" runat="server" visible="false">
                                                            <span class="main__head Extra__text" style="color: #000000; font-weight: bolder;">Add-On Service</span>
                                                            <span class="main__head ExtraFeatures__text" style="color: #000000; margin-left: -46px; font-weight: bolder;">Count</span>
                                                            <span class="main__value Extra__value" style="color: #000000; font-weight: bolder;" runat="server">
                                                                <asp:Label ID="lblAccessoriesTotalAmounts" runat="server" Text="Amount" CssClass="lblSummary"> </asp:Label>
                                                            </span>
                                                        </div>
                                                        <div id="divExtrafeeSummary1" runat="server" visible="false">

                                                            <asp:GridView ID="dtlExtraFeeSummary" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                                                                AutoGenerateColumns="False">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Add-On Service">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblfeevehicleAccessoriesName"
                                                                                runat="server"
                                                                                Text=' <%# Eval("vehicleAccessoriesName") %>' Style="text-transform: capitalize;"> </asp:Label>
                                                                            <asp:Label ID="lblgvfeeTimeSlabId"
                                                                                runat="server"
                                                                                Text=' <%# Eval("timeslabId") %>' Visible="false"> </asp:Label>

                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" BackColor="White" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Count">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblfeeCount"
                                                                                runat="server"
                                                                                Text='<%# Eval("Count") %>'> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" BackColor="White" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                                                Text='<%# Convert.ToDecimal(Eval("totalAmount")).ToString("0.00") %>'
                                                                                CssClass="lblSummary"> </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" BackColor="White" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="grdHead">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/Close.svg" Width="20px" ToolTip="Delete"
                                                                                OnClick="ImgBtnDelete_Click" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="grdHead" />
                                                                        <ItemStyle HorizontalAlign="Center" BackColor="White" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="gvHead" />
                                                                <AlternatingRowStyle CssClass="gvRow" />
                                                                <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                                            </asp:GridView>

                                                        </div>
                                                    </div>
                                                    <div id="divParkingAmount" runat="server">
                                                        <div class="Feature__summary Feature__Parking mt-2">
                                                            <span id="Parking__amount" class="main__head parking__text" style="color: #000000; font-weight: 700;">parking amount</span>
                                                            <span class="main__value parking__value" runat="server">&#8377;&nbsp;
                                         <asp:Label ID="lblparkingAmount" runat="server" Style="color: #000000; font-weight: 700;" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                            </span>
                                                        </div>
                                                        <%--   <hr style="margin-top: 0rem; margin-bottom: 0rem" />--%>
                                                    </div>
                                                    <div id="divExtraFeeAmount" runat="server" visible="true">
                                                        <div class="Feature__summary Feature__Extra">
                                                            <span class="main__head Extra__text" style="color: #000000; font-weight: 700;">Add-On Service</span>
                                                            <span class="main__value Extra__value" runat="server">&#8377;&nbsp;
                                          <asp:Label ID="lblAccessoriesTotalAmount" Style="color: #000000; font-weight: 700;" runat="server" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                            </span>
                                                        </div>
                                                        <%--   <hr style="margin-top: 0rem; margin-bottom: 0rem" />--%>
                                                    </div>
                                                    <div id="divTax" runat="server" visible="true">
                                                        <div class="Feature__summary Feature__ExtraFeatures">
                                                            <span class="main__head ExtraFeatures__text" style="color: #000000; font-weight: 700;">GST</span>
                                                            <span class="main__value ExtraFeatures__value" runat="server">&#8377;&nbsp;
                                          <asp:Label ID="lblGSTAmount" runat="server" Style="color: #000000; font-weight: 700;" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                            </span>
                                                        </div>
                                                        <hr style="margin-top: 0rem; margin-bottom: 0rem" />
                                                    </div>
                                                    <div class="Feature__summary Feature__Total" style="margin-top: 5px;">
                                                        <span class="main__head Total__text" style="font-size: 22px;">total</span>
                                                        <span class="main__value Total__value" style="color: green;">&#8377;&nbsp;
                                         <asp:Label ID="lblTotalAmount" runat="server" Style="font-size: 22px; color: green;" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                        </span>
                                                    </div>
                                                </div>

                                                <%--  <div class="Date__Container mb-3" runat="server" id="divPaymentTypeBook" style="position: relative; display: flex; justify-content: space-between;">
                                                    <div class="Date From__Date">
                                                        <asp:Label
                                                            ID="lblPaymentType"
                                                            runat="server"
                                                            Text="Payment Type" CssClass=" lblFeaturesDate lblDate" Style="color: #000000; font-weight: 700;">
                                                        </asp:Label>
                                                    </div>
                                                    <div class="Date To__Date">
                                                        <asp:DropDownList
                                                            ID="ddlPaymentType"
                                                            runat="server"
                                                            TabIndex="13"
                                                            CssClass="txtSummary txtFeature form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>--%>


                                                <%--<div class="book1" style="justify-content: space-between;">
                                                    <asp:Button ID="btnBook" OnClick="btnBook_Click" TabIndex="14"
                                                        CssClass="bookbtn" Width="35%" Enabled="false"
                                                        Text="Book" ValidationGroup="BookingSlot" runat="server" />

                                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" TabIndex="15"
                                                        CssClass="Cancelbtn" Width="35%"
                                                        Text="Cancel" CausesValidation="false" runat="server" />

                                                </div>--%>
                                                <div class="book1 mt-3 col-sm-12 col-xs-12" style="display: flex; justify-content: space-between; padding: 0px;">
                                                    <div class="col-sm-4 col-xs-12 p-0 text-center">
                                                        <div runat="server" id="divPaymentTypeBook">
                                                            <asp:DropDownList
                                                                ID="ddlPaymentType"
                                                                runat="server"
                                                                TabIndex="14"
                                                                CssClass="txtSummary txtFeature form-control"
                                                                Style="border-radius: 2.25rem; font-size: 2rem !important; position: sticky;">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4 col-xs-12 p-0 text-center">
                                                        <asp:Button ID="btnBook" OnClick="btnBook_Click" TabIndex="15"
                                                            CssClass="bookbtn" Enabled="false"
                                                            Text="Book" ValidationGroup="BookingSlot" runat="server" />
                                                    </div>
                                                    <div class="col-sm-4 col-xs-12 p-0 text-center">
                                                        <asp:Button ID="btnCancel" OnClick="btnCancel_Click" TabIndex="16"
                                                            CssClass="Cancelbtn"
                                                            Text="Cancel" CausesValidation="false" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000a3; opacity: 0.8;">
                    <span style="border-width: 0px; position: fixed; padding: 20px; background-color: #ffffff; font-size: 25px; left: 40%; top: 40%; border-radius: 50px;">
                        <img class="rotate" src="../images/Loader/carloader.png" />
                    </span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>


        <div id="div1" runat="server" visible="true" style="position: absolute; top: 45%; left: 0;">
            <div class="row">

                <asp:LinkButton ID="btnNewPass" runat="server" OnClick="btnNewPass_Click"
                    CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 19px; border-radius: 6px; height: 30px; width: 137px; text-align: center; padding-top: -8px; transform: rotate(270deg); margin-top: 238px; margin-left: -49px; background: #c6ebff; color: black; font-weight: 900; z-index: 1;">
                      New Pass</asp:LinkButton>

            </div>
        </div>
        <!-- jQuery CDN - Slim version (=without AJAX) -->
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
        <!-- Popper.JS -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js" integrity="sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ" crossorigin="anonymous"></script>


        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

        <script src="../js/jspdf.debug.js"></script>
        <script src="../js/html2canvas.min.js"></script>
        <script src="../js/html2pdf.min.js"></script>


        <%--        <script>
            //$('#btnExportPdf').click(function (e) {
            function SendPdf() {
             
                let element = document.getElementById('divPassTicket');
                const options = {
                    margin: 0.5,
                    filename: 'ParkingPass.pdf',

                    image: {
                        type: 'jpeg',
                        quality: 500
                    },
                    html2canvas: {
                        scale: 7
                    },
                    jsPDF: {
                        unit: 'in',
                        format: 'letter',
                        orientation: 'portrait'
                    }
                }

                html2pdf().from(element).set(options).save()
           .outputPdf('datauristring').then(function (pdfAsString) {
               var file = new File([pdfAsString], 'ParkingPass.pdf', {
                   type: 'application/pdf',
               });
               //var newBlob = new Blob([pdfAsString], { type: "application/pdf" })
               var fileData = new FormData();
               fileData.append("image", file);
               //alert(fileData);

               $.ajax({
                   url: '<%= Session["ImageUrl"].ToString() %>',
                   type: "POST",
                   data: fileData,
                   contentType: false, // Not to set any content header  
                   processData: false, // Not to process data                 
                   success: function (result) {
                       // alert(result.response);
                       $('#<%=hfImageUrl.ClientID%>').val(result.response);
                       $('#<%=btn1.ClientID%>').click();
                   },
                   error: function (err) {
                       $('#<%=btn1.ClientID%>').click();
                       // alert(err.statusText);
                   }
               });

           });
                //});
           }
        </script>--%>
        <asp:HiddenField ID="HiddenField1" EnableViewState="true" runat="server" />
        <asp:HiddenField ID="hfPrevImageLink" EnableViewState="true" runat="server" />
        <asp:HiddenField ID="hfImageUrl" runat="server" />


        <%--....Ticket END....--%>
        <asp:Button ID="cancel" runat="server" OnClick="Close_Click" Style="display: none;" />
        <asp:HiddenField runat="server" ID="hfPinNo" />
        <asp:TextBox
            ID="txtrow"
            runat="server"
            TabIndex="15" Visible="false"
            CssClass="form-control ">
        </asp:TextBox>
        <asp:TextBox
            ID="txtcolumn"
            runat="server" Visible="false"
            TabIndex="16"
            CssClass="form-control ">
        </asp:TextBox>
        <asp:HiddenField ID="hfslotcheck" runat="server" Value="0" />
        <asp:HiddenField ID="hftodate" runat="server" />
    </form>

    <script type="text/javascript">
        function PlusOnclick() {
            document.getElementById('<%=txtBookingId.ClientID %>').value = "";
        }

    </script>

    <script src="./script.js"></script>

    <!-- Bootstrap JS -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js" integrity="sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm" crossorigin="anonymous"></script>

    <!-- Grid ViewSearch -->
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>

    <%-- <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>--%>
    <script src="../js/flatpickr.js"></script>
    <script>
        const datepicker = document.getElementsByClassName('datePicker');
        const fromDate = document.getElementsByClassName('fromDate');
        const toDate = document.getElementsByClassName('toDate');
        const dateTimepicker = document.getElementsByClassName('dateTimepicker');
        const timePicker = document.getElementsByClassName('timePicker');
        let date = new Date();
        var Todate;

        let fp = flatpickr(datepicker,
            {
                enableTime: false,
                dateFormat: "Y-m-d",
                altFormat: "d-m-Y",
                altInput: true,
                time_24hr: false,
                minDate: "today",
                onOpen: function () {
                    const numInput = document.querySelectorAll('.numInput');
                    numInput.forEach((input) => input.type = '');
                }

            });

        fp = flatpickr(fromDate,
            {
                enableTime: false,
                dateFormat: "Y-m-d",
                altFormat: "d-m-Y",
                altInput: true,
                time_24hr: false,
                onOpen: function () {
                    const numInput = document.querySelectorAll('.numInput');
                    numInput.forEach((input) => input.type = '');
                },
                onChange: function (selectedDates, dateStr, instance) {
                    toDate[0].value = '';
                    flatpickr(toDate,
                        {
                            enableTime: false,
                            dateFormat: "Y-m-d",
                            altFormat: "d-m-Y",
                            altInput: true,
                            time_24hr: false,
                            minDate: dateStr
                        });
                },
            });
        fp = flatpickr(timePicker,
            {
                enableTime: true,
                noCalendar: true,
                time_24hr: true,
                dateFormat: "H:i",
                minTime: "today",

            });

        fp = flatpickr(dateTimepicker,
            {
                enableTime: true,
                dateFormat: "d/m/Y h:i:K",
                time_24hr: false,
                minDate: "today",
                onOpen: function () {
                    const numInput = document.querySelectorAll('.numInput');
                    numInput.forEach((input) => input.type = '');
                },
                maxDate: new Date().fp_incr(1) // 14 days from now
            });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerDate);
            const datepicker = document.getElementsByClassName('classTargetDate');
            function EndRequestHandlerDate(sender, args) {
                flatpickr(datepicker,
                    {
                        enableTime: false,
                        dateFormat: "Y-m-d",
                        altFormat: "d-m-Y",
                        altInput: true,
                        time_24hr: false,
                        minDate: new Date().fp_incr(1),
                        onOpen: function () {
                            const numInput = document.querySelectorAll('.numInput');
                            numInput.forEach((input) => input.type = '');
                        },

                    });
            }

        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerTime);
            const timePicker = document.getElementsByClassName('classTargetTime');
            function EndRequestHandlerTime(sender, args) {
                flatpickr(timePicker,
                    {
                        enableTime: true,
                        noCalendar: true,
                        time_24hr: true,
                        dateFormat: "H:i",
                        minTime: Time,
                        defaultHour: Time,
                        hourIncrement: 1,
                        minuteIncrement: 60
                    });

            }
            flatpickr.minute.style.display = "none";

        });

    </script>

    <script type="text/javascript">
        function showHourGlass() {
            document.getElementById("HourGlass").style.display = 'block';
        }

    </script>
    <script lang="javascript" type="text/javascript">
        var myVar = setInterval(myTimer, 0);
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        const DayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sats"];
        function myTimer() {
            const d = new Date();
            let date = DayNames[d.getDay()] + ", " + d.getDate() + "-" + monthNames[d.getMonth()];
            const Time = d.toLocaleTimeString('en-US', { hour12: false });
            document.getElementById("Date").innerText = date;
            document.getElementById("Time").innerText = Time;
        }
    </script>

    <script>
        function enableCheckBox() {
            //document.getElementById('<%=ChkMobileNo.ClientID %>').parentElement.disabled = false;
            document.getElementById('ChkMobileNo').disabled = false;
        }
        function disableCheckBox() {
            //document.getElementById('ChkMobileNo').parentElement.disabled = true;
            document.getElementById('ChkMobileNo').disabled = true;
        }
        function show() {
            document.getElementById('<%=modalpasssub.ClientID %>').style.cssText = "#model";
        }
        function hide() {
            document.getElementById('<%=modalpasssub.ClientID %>').style.display = "none";
        }
    </script>
    <style>
        .rotate {
            animation: rotation 3s infinite linear;
        }

        @keyframes rotation {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(359deg);
            }
        }
    </style>
</body>

</html>

