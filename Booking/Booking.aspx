<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Booking.aspx.cs" Inherits="Booking_Booking" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html style="overflow-x: hidden; overflow-y: hidden;">
<%--class="section"--%>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Booking</title>
    <link href="../fav.ico" rel="shortcut icon" type="image/x-icon" />
    <!-- jQuery CDN - Slim version (=without AJAX) -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <!-- Popper.JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js" integrity="sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ" crossorigin="anonymous"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <!-- Bootstrap JS -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js" integrity="sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm" crossorigin="anonymous"></script>

    <!-- Bootstrap CSS CDN -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" integrity="sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4" crossorigin="anonymous">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"
        integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">
        function showLoader() {
            document.getElementById("loader").style.display = 'block';
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
    <link href="../Style/Booking.css" rel="stylesheet" />
    <link href="../Style/BookingSlots.css" rel="stylesheet" />
    <link href="../Style/SlotSummary.css" rel="stylesheet" />
    <link href="../Style/Navigation.css" rel="stylesheet" />
    <link href="../Style/Nav.css" rel="stylesheet" />
    <link href="../Style/ContentPage.css" rel="stylesheet" />
    <link href="../Style/Table.css" rel="stylesheet" />
    <link href="../Style/ParkingPass.css" rel="stylesheet" />

    <style>
        /*1366*586  -- sridar bro system
1540*495   --- Mam
1536*450   ----mam------
358
1280*309--Demo screen
1237*272--Demo screen------*/

        @media only screen and (min-width: 1200px) and (max-height:800px) {
            .table-responsive {
                overflow-y: auto;
                max-height: 475px !important;
                margin-bottom: 0 !important;
            }
        }

        @media only screen and (min-width: 1024px) and (max-height:600px) {
            .table-responsive {
                overflow-y: auto;
                max-height: 435px !important;
                margin-bottom: 0 !important;
            }
        }

        .DotText {
            color: #000000;
            font-size: 14px;
            font-weight: 500;
            margin: 0;
        }

        .tooltiptext1 {
            visibility: visible;
            width: 90px !important;
            color: #dc3545;
            text-align: center;
            padding: 4px 4px;
            position: absolute;
            bottom: 58%;
            margin-left: -45px;
        }

        .gvHead {
            background-color: #b4cfe5;
        }

        .SelectedItem {
            background-color: #0e892a;
        }

        .BtnOtp {
            border-radius: 10px;
            border: 1px solid #24b53c;
            background-color: #24b53c;
            color: #FFFFFF;
            font-size: 1.40rem !important;
            font-weight: bold;
            padding: 5px 5px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            width: auto;
            cursor: pointer;
        }

        .btnResnd {
            border-radius: 10px;
            border: 1px solid #007bff;
            background-color: #007bff;
            color: #FFFFFF;
            font-size: 1.40rem !important;
            font-weight: bold;
            padding: 5px 5px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            width: auto;
            cursor: pointer;
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
            overflow: auto;
        }

        .extraFeatures {
            max-height: 25rem;
            height: 8rem;
            box-shadow: #00000059 0px 4px 16px !important;
            border-radius: 1rem;
            overflow: hidden;
            width: auto;
            cursor: pointer;
            background: #ffffff;
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
            background: #ffffff;
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
            border-radius: 12px;
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
            border-radius: 10px;
            text-align: center;
            color: #900a86ab;
        }

        .checkout {
            background-color: #f687668a;
            border-radius: 10px;
            text-align: center;
            color: #c54919;
        }

        .checkin {
            background-color: #5c40f436;
            border-radius: 10px;
            text-align: center;
            color: #750c77;
        }

        .carimage {
            border-radius: 25px;
            padding-top: 6px;
            padding-left: 20px;
            /*padding-right: 20px !important;*/
            height: 48px;
            color: black;
            opacity: 0.35;
        }

        .carimage2 {
            border-radius: 25px;
            padding-top: 6px;
            padding-left: 20px;
            /*padding-right: 20px !important;*/
            height: 48px;
            color: black;
        }

        .divcarname {
            margin-right: -32px;
            margin-left: -10px;
            background: white;
            border-radius: 30px;
            height: 35px;
            width: 18rem;
            color: black;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .divcarname2 {
            margin-right: -32px;
            margin-left: -10px;
            background: white;
            border-radius: 30px;
            height: 35px;
            width: 18rem;
            color: black;
            border: solid #0075ff 2px;
            display: flex;
            justify-content: center;
            align-items: center;
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
            width: 110%;
            height: 35px !important;
            border-color: skyblue;
            border-radius: 30px;
            font-size: 14px !important;
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
            width: auto;
            height: auto;
            box-shadow: rgb(0 0 0 / 56%) 0px 10px 70px 4px;
            background-color: #ffffff;
            top: 25%;
            border-bottom-left-radius: 25px;
            border-top-left-radius: 25px;
            right: 25%;
            bottom: 25%;
        }

        .divIcon {
            text-align: center;
            padding: 1rem;
            margin: 1rem;
            color: white;
            transition: all 0.3s;
        }

        legend {
            display: block;
            width: inherit;
            max-width: 100%;
            /* padding: 0px 0px; */
            font-size: 13px;
            font-weight: bold;
            line-height: inherit;
            color: inherit;
            white-space: normal;
            margin-left: -65px;
            /* margin-bottom: -6px; */
            margin-top: 2px !important;
            position: absolute;
        }

        .legBorder {
            border-width: 1px;
            border-color: #2196f373;
            border-radius: 6px;
            border-style: dashed;
            padding: 0px 6px;
            /* margin-left: 5px; */
            height: 28px;
        }

        .LaneNo {
            position: sticky;
            left: -15px;
            z-index: 1;
            box-shadow: #00000059 0px 4px 8px !important;
            background-color: #fff !important;
        }

        .LaneTop {
            position: sticky;
            top: 0;
            z-index: 1;
            box-shadow: #00000059 0px 4px 8px !important;
            background-color: #fff !important;
        }

        .CustomGrid {
            margin-bottom: 1px;
        }

        .form-control {
            border: 1px solid #e5e5e6;
        }




        .newpass {
            position: absolute;
            top: 70%;
            rotate: 270deg;
            padding: 0.5rem;
            color: black;
            font-size: 1.6rem;
            width: 10rem;
            font-weight: 900;
            z-index: 1;
            text-align: center;
            margin-left: -2.6rem;
            border-radius: 2rem;
            background-color: #c6ebff;
        }

        .vehiclename {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .lnkAddVehicle {
            position: absolute;
            margin-top: -0.4rem;
            margin-left: 0.4rem;
            color: black;
            font-size: 2rem;
            border-radius: 0.6rem;
        }

        .PrintChk {
            display: flex;
            position: absolute;
            margin-left: -1.5rem;
            margin-top: 0.5rem;
        }

        .lblPrint {
            font-size: 1.5rem;
            font-weight: 900;
            padding-left: 0.3rem;
        }

        .lnkRePrint {
            margin-right: 2rem;
            font-size: 3rem;
        }

        #imgBtnCamera {
            width: 3.2rem;
        }

        .divOverlayMannualBooking {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: #0000009c;
            z-index: 9998;
            cursor: pointer;
        }

        .divMannualBookingPopup {
            position: absolute;
            top: 50%;
            left: 50%;
            width: 75%;
            height: 90%;
            background-color: #fff;
            z-index: 9999;
            transform: translate(-50%,-50%);
            border-radius: 0.4rem;
            overflow: hidden;
        }

        .closeImgBtnMannualPopup {
            height: 3.2rem;
            width: 3.2rem;
        }

        .closeImgBtnMannualPopup {
            height: 3.2rem;
            width: 3.2rem;
            position: absolute;
            right: 2%;
            top: 2%;
        }
    </style>
    <%--Date Picker--%>
    <link href="../Style/DatePicker.css" rel="stylesheet" />
</head>
<body>
    <form id="form" runat="server">
        <div id="divOverlayMannualBooking" class="divOverlayMannualBooking" runat="server" visible="false"></div>
        <div id="divMannualBookingPopup" class="divMannualBookingPopup p-5" runat="server" visible="false">

            <asp:ImageButton
                ID="imgMannualPopupClose"
                runat="server"
                ImageUrl="~/SVG/close-svgrepo-com.svg"
                OnClick="imgMannualPopupClose_Click"
                CssClass="closeImgBtnMannualPopup" />
            <div class="table-responsive section p-5 mt-5">

                <asp:GridView
                    ID="gvMannualBooking"
                    runat="server"
                    AllowPaging="True"
                    CssClass="gvv display"
                    DataKeyNames="UniqueId,vehicleNumber,ActiveStatus"
                    AutoGenerateColumns="false"
                    PageSize="25000"
                    BorderStyle="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Vehicle Number">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvManualVehicleNumber"
                                    runat="server"
                                    Text=' <%#Bind("VehicleNumber") %>'></asp:Label>
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Vehicle Type">
                            <ItemTemplate>
                                <asp:DropDownList
                                    ID="ddlgvManualVehicleType"
                                    CssClass="form-control"
                                    runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlgvManualVehicleType"
                                    runat="server"
                                    ControlToValidate="ddlgvManualVehicleType"
                                    Display="Dynamic"
                                    CssClass="rfvClr"
                                    ErrorMessage="Please select an option"
                                    InitialValue="0"
                                    ValidationGroup='<%# Eval("UniqueId") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Check In" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:ImageButton ID="gvManualbtnCheckIn"
                                    runat="server"
                                    ValidationGroup='<%# Eval("UniqueId") %>'
                                    OnClick="gvManualbtnCheckIn_Click"
                                    src="../images/check-in.png" alt="image" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
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
                        <div class="col-sm-7 col-xs-12" style="margin-left: -3rem; display: flex; align-items: center;">
                            <asp:LinkButton ID="ImageHome" runat="server" PostBackUrl="~/DashBoard.aspx" Style="cursor: pointer;"> 
                             <span><i class="fa-solid fa-house" style="font-size: 22px; color:black;cursor:pointer;"></i></span>
                            Home</asp:LinkButton>
                            &nbsp; | &nbsp;
                             <span class="text-black fw-bold">
                                 <asp:Label ID="lblUserName" runat="server" /></span>
                            &nbsp; | &nbsp;
                             <span class="text-black fw-bold">
                                 <asp:Label ID="lblUserRole" runat="server" /></span>
                            &nbsp; | &nbsp;
                                <asp:ImageButton
                                    ID="imgBtnCamera"
                                    runat="server"
                                    src="../SVG/camera-svgrepo-com.svg"
                                    OnClick="imgBtnCamera_Click"
                                    alt="Camera" />
                        </div>


                    </div>
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager"
            runat="server" />

        <asp:UpdatePanel ID="UpdatePanel1"
            UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <fieldset>
                    <div style="display: flex; margin-bottom: 1rem;">
                        <div id="divBlockFloor" runat="server" visible="false" class="row col-3 col-sm-3 col-md-3 col-lg-3 col-xl-3" style="margin-top: 5px; margin-left: 0px;">
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
                        <div id="divCount" runat="server" visible="false" style="display: flex; flex-wrap: wrap;"
                            class="divcountbg col-9 col-sm-9 col-md-9 col-lg-9 col-xl-9">
                            <div class="col-4 col-sm-4 col-md-4 col-lg-4 col-xl-4" style="margin-right: 2rem !important;">
                                <fieldset class="legBorder" style="margin-left: 4.2rem !important;">
                                    <legend>Available</legend>
                                    <div>
                                        <span id="spanA" runat="server" class="dot" style="background: #40F44336"></span>&nbsp;
                                    <span class="DotText">Normal (<asp:Label
                                        ID="lblNormalCount" runat="server" class="pghr"
                                        Style="font-size: 14px; font-weight: bold; display: inline;"></asp:Label>)
                                    </span>&nbsp;|&nbsp;
                                    <span id="spanV" runat="server" class="dot" style="background: #ffee2162;"></span>&nbsp;
                                    <span class="DotText">VIP (<asp:Label
                                        ID="lblVIPCount" runat="server" class="pghr"
                                        Style="font-size: 14px; font-weight: bold; display: inline;"></asp:Label>)
                                    </span>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-8 col-sm-8 col-md-8 col-lg-8 col-xl-8" style="margin-right: -2rem !important;">
                                <fieldset class="legBorder" style="margin-left: 1.5rem !important;">
                                    <legend>Bookings</legend>
                                    <div>
                                        <span class="DotText">Booked (<asp:Label
                                            ID="lblBookedCount" runat="server" class="pghr"
                                            Style="font-size: 14px; font-weight: bold; display: inline;"></asp:Label>)</span>&nbsp;|
                                    <span class="DotText">Reserved (<asp:Label
                                        ID="lblReservedCount" runat="server" class="pghr"
                                        Style="font-size: 14px; font-weight: bold; display: inline;"></asp:Label>)</span>&nbsp;|
                                    <span class="DotText">Pass (<asp:Label ID="lblpass" runat="server" class="pghr"
                                        Style="font-size: 14px; font-weight: bold; display: inline;"></asp:Label>)</span>&nbsp;|
                                    <span class="DotText">Check In (<asp:Label ID="lblcheckin" runat="server" class="pghr"
                                        Style="font-size: 14px; font-weight: bold; display: inline;"></asp:Label>)</span>&nbsp;|
                                    <span class="DotText">Check Out (<asp:Label ID="lblcheckout" runat="server" class="pghr"
                                        Style="font-size: 14px; font-weight: bold; display: inline;"></asp:Label>)</span>

                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <div class="modal-wrapper model" id="modalSearch" runat="server" visible="false">

                        <div id="divmodal" runat="server" class="modal-body cardModalSearch table-responsive section">
                            <div id="divModalColor" runat="server">
                                <div style="display: flex; flex-wrap: wrap;">
                                    <div class="row col-5 col-sm-5 col-md-5 col-lg-5 col-xs-5" id="divNormalHeader" runat="server" style="padding: 1px; margin-left: 14px; font-size: 1.8rem;">
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
                                    <div class="row col-5 col-sm-5 col-md-5 col-lg-5 col-xs-5" id="divPassHeader" runat="server"
                                        style="padding: 1px; margin-left: 14px; font-size: 1.8rem;" visible="false">
                                        <label for="lblmaxcharge" class="labels" style="font-size: 1.8rem;">
                                            Booking Id 
                                        </label>
                                        &nbsp;
                                <asp:Label ID="Label7" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"> :</asp:Label>
                                        &nbsp;
                                     <asp:Label ID="lblBookingPassId" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"></asp:Label>

                                    </div>
                                    <div id="divInTime" runat="server" visible="false" class="row col-7 col-sm-7 col-md-7 col-lg-7 col-xs-7">
                                        <div class="row col-7 col-sm-7 col-md-7 col-lg-7 col-xs-7">
                                            <label for="lblmaxcharge" class="labels" style="font-size: 1.8rem;">
                                                In Time 
                                            </label>
                                            &nbsp;
                                <asp:Label ID="Label6" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"> :</asp:Label>
                                            &nbsp;
                                        <asp:Label ID="lblInTime" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"></asp:Label>
                                        </div>
                                        <div class="row col-5 col-sm-5 col-md-5 col-lg-5 col-xs-5">
                                            <label for="lblmaxcharge" class="labels" style="font-size: 1.8rem;">
                                                Parking Hrs
                                            </label>
                                            &nbsp;
                                <asp:Label ID="Label17" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"> :</asp:Label>
                                            &nbsp;
                                        <asp:Label ID="lblParkedHrs" runat="server" Font-Bold="true" Style="margin-top: -1px; font-size: 1.8rem;"></asp:Label>
                                        </div>
                                    </div>
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
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divExtendVehicleNo" runat="server">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <asp:Image ID="ImgExtnVhcl" runat="server" Visible="false" Style="height: 3rem; margin-top: -5px;" />
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
                                                        Remaining Amount <%--<span id="Span2" runat="server"
                                                            style="font-size: 10px; color: black; padding-left: 18px">(incl. of all taxes)</span>--%>
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
                                                        ₹&nbsp;       Extended Tax Amount
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
                                                        Extended Amount <%--<span id="Span1" runat="server"
                                                            style="font-size: 10px; color: black; padding-left: 16px">(incl. of all taxes)</span>--%>
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

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divRemainingVehicleNo" runat="server">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <asp:Image ID="ImgRemVhcl" runat="server" Visible="false" Style="height: 3rem; margin-top: -5px;" />
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
                                                        Remaining Amount <%--<span id="Span3" runat="server"
                                                            style="font-size: 10px; color: black; padding-left: 18px">(incl. of all taxes)</span>--%>
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
                                                        Block & Floor
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblBlockIn" runat="server" Font-Bold="true"></asp:Label>
                                                    &
                                                <asp:Label ID="lblFloorIn" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divPassBookingVehicleNo" runat="server">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <asp:Image ID="ImgPasVhcl" runat="server" Visible="false" Style="height: 3rem; margin-top: -5px;" />
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
                                    <div class="row">
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                    <label for="lblmaxcharge" class="labels">
                                                        Lane - Slot No
                                                   
                                                    </label>
                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                    :
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <asp:Label ID="lblLaneslotsNo" runat="server" Font-Bold="true"></asp:Label>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divBookingAmountPAss" runat="server">
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

                                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                                            <div class="row" style="padding: 3px; margin-left: 10px;" id="divpassstatus" runat="server" visible="true">
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
                                            <div id="divPayment" runat="server">
                                                <div class="row" style="padding: 3px; margin-left: 10px;">
                                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                        <label for="lblmaxcharge" class="labels">
                                                            To Pay 
                                                        </label>
                                                    </div>
                                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                                        :
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right" style="color: green; font-size: 25px;">
                                                        ₹&nbsp;
                                    <asp:Label ID="lblTopayAmt" runat="server" Font-Bold="true"></asp:Label>
                                                    </div>
                                                </div>

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
                                                        <asp:DropDownList ID="ddlPassPaymentMode" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator
                                                            ID="RequiredFieldValidator2"
                                                            runat="server"
                                                            ControlToValidate="ddlPassPaymentMode"
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

                            <div class="row justify-content-end" style="margin-top: 18px">
                                <div id="divRePrint" runat="server">
                                    <asp:LinkButton ID="lnkRePrint" OnClick="lnkRePrint_Click" runat="server"><i  class="fa-solid fa-print lnkRePrint"></i></asp:LinkButton>
                                </div>
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
                        <div class="modal-body cardModalPass section">
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
                                                            <asp:Image ID="lblVehicleTypes" Style="height: 40px !important; width: 40px;"
                                                                runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                                            <asp:Label align="left" ID="lblvehicleNamePass" runat="server"
                                                                Text='<%# Bind("vehicleName") %>' Font-Bold="true" Font-Size="14px" Visible="true">
                                                            </asp:Label>
                                                        </div>
                                                        <%-- <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" id="DivVehileTypes" runat="server" style="margin-top: -14px;">
                                                            <asp:Image ID="lblVehicleTypes" Style="height: 60px !important; width: 60px;"
                                                                runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                                        </div>--%>
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
                            <div id="Passbackground" runat="server"></div>
                        </div>

                    </div>
                    <div id="Formbackground" runat="server" class="Formbackground">
                        <div id="divForm" runat="server" class="divformbg" visible="true" style="margin-top: -40px;">
                            <div class="row">

                                <div class="col-12 col-xs-8 col-sm-8 col-md-8 col-lg-8 col-xl-8 divVehiclebg table-responsive section" style="overflow-y: hidden; top: 5px;">
                                    <asp:DataList ID="gvVehicleType" runat="server" Visible="true" RepeatDirection="Horizontal"
                                        CssClass="mb-1" OnItemCommand="gvVehicleType_ItemCommand">
                                        <ItemTemplate>

                                            <div id="divvehicles" runat="server" class="carimage" style="display: inline-flex;">

                                                <asp:LinkButton ID="divvehicle" runat="server" OnClick="lblVehicleTypes_Click">
                                                    <div class="row" style="width: max-content;">
                                                        <div id="divcarname" runat="server" class="col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname">
                                                            <asp:Image ID="image" Style="height: 35px !important; width: 35px;"
                                                                runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                                            <asp:Label ID="lblvehicleName" runat="server" CssClass="vehiclename"
                                                                Text='<%# Bind("vehicleName") %>' Font-Bold="true" Font-Size="14px" Visible="true">
                                                            </asp:Label>
                                                        </div>

                                                        <%--<div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" id="DivVehileTypes" runat="server" style="margin-top: -14px;">
                                                            <asp:Image ID="image" Style="height: 60px !important; width: 60px;"
                                                                runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                                        </div>--%>
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
                                <div class="col-12 col-xs-4 col-sm-4 col-md-4 col-lg-4 col-xl-4 divVehicleNobg d-flex" style="right: 4rem;">
                                    <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control BarCodeTextStart"
                                        AutoPostBack="true" OnTextChanged="txtBookingId_TextChanged" Style="text-transform: uppercase;"
                                        placeholder="Scan QR Code / Enter Vehicle No./ Mobile No./ Pin No."
                                        AutoComplete="Off"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="divtab" runat="server" id="DivSlotOtherStab">
                                <div class="row" style="margin-left: 11px !important; margin-bottom: -11px; margin-top: 5px">
                                    <div>
                                        <asp:Button
                                            ID="btnslottab"
                                            runat="server"
                                            Text="Slots"
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
                            <div style="padding-left: 55px; padding-top: 4px;">

                                <asp:CheckBox ID="ChkMobileNo" runat="server" Style="zoom: 1.5;" Width="17px" Height="21px"
                                    OnCheckedChanged="ChkMobileNo_CheckedChanged" Checked="true"
                                    AutoPostBack="true" /><b style="font-size: 19px; color: black;">Mobile No</b>

                            </div>
                        </div>

                        <div style="margin-top: 1px; overflow-x: hidden;" class="slot__container divslotbg table-responsive section">
                            <div class="row">
                                <div id="Divslotrow" runat="server" style="margin-bottom: 3rem!important"
                                    class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 table-responsive section">
                                    <div class="Bookingslot__container">
                                        <div class="row">
                                            <div style="display: flex;">
                                                <div class="Bookingslot__container" runat="server" id="divslot">
                                                    <asp:DataList ID="maindata" runat="server" OnItemDataBound="maindata_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:DataList ID="SubData" runat="server" RepeatDirection="Horizontal" OnItemCommand="SubData_ItemCommand">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lblSlotNumberdtl" runat="server" Text='<%#Bind("slotNumber") %>'
                                                                        CssClass="tdsOthers" OnClick="lblSlotNumberdtl_Click">
                                                                    </asp:LinkButton>

                                                                    <asp:Label ID="lbldtlparkingSlotId" runat="server" Text='<%#Bind("parkingSlotId") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlparkingLotLineId" runat="server" Text='<%#Bind("parkingLotLineId") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtllaneNumber" runat="server" Text='<%#Bind("laneNumber") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlrowId" runat="server" Text='<%#Bind("rowId") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlcolumnId" runat="server" Text='<%#Bind("columnId") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlslotType" runat="server" Text='<%#Bind("slotType") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlslotState" runat="server" Text='<%#Bind("slotState") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlisChargeUnitAvailable" runat="server" Text='<%#Bind("isChargeUnitAvailable") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlchargePinType" runat="server" Text='<%#Bind("chargePinType") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlactiveStatus" runat="server" Text='<%#Bind("activeStatus") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lbldtlvehicleNumber" runat="server" Text='<%#Bind("vehicleNumber") %>' Visible="false"></asp:Label>

                                                                </ItemTemplate>
                                                                <ItemStyle />
                                                            </asp:DataList>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div id="extraFee__container" runat="server"
                                    class="col-8 col-8 col-sm-8 col-md-8 col-lg-8 col-xl-8 extracontainer2 table-responsive section" style="overflow-x: hidden">

                                    <asp:Label ID="lblGvAccessoriesNo" runat="server"
                                        Visible="false" Style="text-align: center !important; color: red"></asp:Label>
                                    <asp:DataList ID="dtlextraFee" RepeatColumns="3" Style="width: auto;"
                                        RepeatDirection="Horizontal" runat="server" OnItemCommand="dtlextraFee_ItemCommand">

                                        <ItemTemplate>
                                            <div class="col-12">

                                                <div class="extraFeatures mt-4 mb-4">
                                                    <div id="DivSelected" runat="server" class="feature__item">
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

                                <div class="col-4 col-4 col-sm-4 col-md-4 col-lg-4 col-xl-4 table-responsive section">
                                    <div id="divSummary" class="Slot__Summary pr-4 mt-1" runat="server" visible="false">
                                        <div id="divSummaryColor" runat="server" style="background-color: #c1c0c36b; border-radius: 10px; padding: 10px">
                                            <div class="divslotno" id="divlaneSlotNo" runat="server" visible="false">
                                                <span class="slotlaneno">Lane-Slot
                                    <asp:Label ID="lblLaneSlot" runat="server"></asp:Label></span>
                                            </div>
                                            <div class="Slot__Header-container Slot__Header" style="margin-bottom: 0rem">
                                                <div class="row" style="margin-bottom: 0.2rem; margin-left: 0rem;">
                                                    <h3 class="summary__header" style="color: #000000; font-weight: bolder;">summary</h3>
                                                </div>
                                            </div>
                                            <div class="DateAndFeatures__Container" style="margin-bottom: 0rem">
                                                <div id="divTimeType" runat="server" class="Date__Container" style="margin-bottom: 0rem">
                                                    <div class="Date From__Date">
                                                        <asp:Label
                                                            ID="lbltimetype"
                                                            runat="server"
                                                            Text="Duration" CssClass="lblSummary lblFromDate lblDate">
                                                        </asp:Label>
                                                        <asp:RadioButtonList
                                                            ID="rbtnTimeType" AutoPostBack="true"
                                                            runat="server" OnSelectedIndexChanged="rbtnTimeType_SelectedIndexChanged"
                                                            TabIndex="11" RepeatDirection="Horizontal" Style="height: 2.8rem; font-size: 1.6rem;"
                                                            CssClass="txtSummary txtDate txtFromDate rbtnList">
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
                                                    <div class="Date To__Date">
                                                        <div id="divfromdate" runat="server" visible="false">
                                                            <asp:Label
                                                                ID="lblfromDate"
                                                                runat="server"
                                                                Text="From Date" CssClass="lblSummary lblFromDate lblDate">
                                                            </asp:Label>
                                                            <asp:TextBox
                                                                ID="txtFromDate"
                                                                runat="server"
                                                                CssClass="txtSummary form-control datePicker">
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
                                                                Text="To Date" CssClass="lblSummary lblFromDate lblDate">
                                                            </asp:Label>
                                                            <asp:TextBox
                                                                ID="txtTodate"
                                                                runat="server"
                                                                TabIndex="12"
                                                                CssClass="txtSummary form-control datePicker classTargetDate"
                                                                OnTextChanged="txtTodate_TextChanged" AutoPostBack="true">
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
                                                                Text="From Time" CssClass="lblSummary">
                                                            </asp:Label>
                                                            <asp:TextBox
                                                                ID="txtfromtime"
                                                                runat="server"
                                                                AutoComplete="off"
                                                                CssClass="txtSummary form-control timePicker"
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
                                                                Text="To Time" CssClass="lblSummary">
                                                            </asp:Label>

                                                            <asp:TextBox
                                                                ID="txtTotime"
                                                                runat="server"
                                                                TabIndex="13"
                                                                onkeypress="return isNumber(event);"
                                                                MaxLength="5"
                                                                OnTextChanged="txtTotime_TextChanged"
                                                                AutoPostBack="true"
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
                                                <div class="Date__Container" style="margin-bottom: 0rem">

                                                    <div class="Date From__Date" id="divMobileNo" runat="server">
                                                        <asp:Label
                                                            ID="lblMobNo"
                                                            runat="server"
                                                            CssClass="lblSummary"
                                                            Text="Mobile No. / Pass Id">
                                                        </asp:Label><span class="spanStar">*</span>
                                                        <%--   onkeypress="return isNumber(event);" MaxLength="10" MinLength="10"--%>
                                                        <asp:TextBox
                                                            ID="txtmobileNo"
                                                            runat="server"
                                                            TabIndex="8" AutoComplete="off"
                                                            CssClass="txtSummary txtDate txtFromDate form-control"
                                                            OnTextChanged="txtmobileNo_TextChanged" AutoPostBack="true">
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator
                                                            ID="RfvMobNo"
                                                            ValidationGroup="BookingSlot"
                                                            ControlToValidate="txtmobileNo"
                                                            runat="server"
                                                            CssClass="rfvClr"
                                                            ErrorMessage="Enter Mobile No./ Pass Id">
                                                        </asp:RequiredFieldValidator>
                                                        <%--       <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                        ControlToValidate="txtmobileNo" ErrorMessage="Invalid Mobile No."
                                                        ValidationExpression="[0-9]{10}" CssClass="rfvClr" Style="float: left; margin-top: -19px"
                                                        ValidationGroup="BookingSlot"></asp:RegularExpressionValidator>--%>
                                                    </div>
                                                    <div class="Date To__Date" id="divVehicleNo" visible="false" runat="server">
                                                        <asp:Image ID="ImgSummary" runat="server" Visible="false" Style="height: 3rem; margin-top: -5px;" />
                                                        <asp:Label
                                                            ID="lblVehicleNo"
                                                            runat="server"
                                                            CssClass="lblSummary"
                                                            Text="Vehicle No.">
                                                        </asp:Label><span class="spanStar">*</span>
                                                        <asp:LinkButton ID="lnkAddBack" Visible="false" OnClick="lnkAddBack_Click"
                                                            CssClass="lnkAddVehicle" runat="server">
                                                                <i class="fa-solid fa-circle-minus"></i></asp:LinkButton>
                                                        <asp:TextBox
                                                            ID="txtVehicleNo"
                                                            runat="server" placeholder="XX00XX0000" AutoComplete="off"
                                                            TabIndex="9" MaxLength="10" Style="text-transform: uppercase; width: 80%;"
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
                                                                ID="revVehicleNo"
                                                                ValidationGroup="BookingSlot"
                                                                ControlToValidate="txtVehicleNo"
                                                                ValidationExpression="^[A-Z|a-z]{2}[0-9]{2}[A-Z|a-z]{1,2}[0-9]{4}$"
                                                                runat="server" Style="margin-left: -5rem"
                                                                CssClass="rfvClr"
                                                                ErrorMessage="Invalid Vehicle No.">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>


                                                    <div class="Date__Container" style="margin-bottom: 0rem">

                                                        <div class="Date From__Date" id="divVehicleNoDropdown" runat="server" visible="false">
                                                            <asp:Image ID="ImgSummary1" runat="server" Visible="false" Style="height: 3rem; margin-top: -5px;" />
                                                            <asp:Label
                                                                ID="lblVehicleDropDown"
                                                                runat="server"
                                                                CssClass="lblSummary"
                                                                Text="Vehicle No.">
                                                            </asp:Label><span class="spanStar">*</span>
                                                            <asp:LinkButton ID="lnkAddVehicle" OnClick="lnkAddVehicle_Click"
                                                                CssClass="lnkAddVehicle" runat="server">
                                                               <i class="fa-solid fa-circle-plus"></i> </asp:LinkButton>
                                                            <asp:DropDownList
                                                                ID="ddlVehicle" AutoPostBack="true"
                                                                runat="server" OnSelectedIndexChanged="ddlVehicle_SelectedIndexChanged"
                                                                TabIndex="10"
                                                                CssClass="form-control">
                                                            </asp:DropDownList>

                                                            <asp:RequiredFieldValidator
                                                                ID="rfvddlVehicleNo"
                                                                ValidationGroup="BookingSlot"
                                                                ControlToValidate="ddlVehicle" InitialValue="0"
                                                                runat="server"
                                                                CssClass="rfvClr"
                                                                ErrorMessage="Select Vehicle No.">
                                                            </asp:RequiredFieldValidator>
                                                        </div>

                                                    </div>

                                                </div>


                                                <%--  OTP Send Details--%>
                                                <div id="divOtpDetails" class="Date__Container" runat="server" visible="false">

                                                    <div class="Date From__Date">
                                                        <div id="divSendOtp" runat="server" visible="false">
                                                            <asp:Button ID="btnSend" runat="server" CssClass="BtnOtp mb-2" ValidationGroup="BookingSlot"
                                                                Text="Send OTP" OnClick="btnSend_Click" />
                                                        </div>
                                                        <div id="divEnterOtp" runat="server" visible="false">
                                                            <asp:Label
                                                                ID="lblOTP"
                                                                runat="server"
                                                                CssClass="lblSummary"
                                                                Text="Enter OTP">
                                                            </asp:Label><span class="spanStar">*</span>
                                                            <asp:TextBox
                                                                ID="txtOTP" onkeypress="return isNumber(event);"
                                                                runat="server" MaxLength="6"
                                                                TabIndex="8"
                                                                CssClass="txtSummary txtDate txtFromDate form-control">
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
                                                    <div class="Date To__Date">
                                                        <div id="divResend" runat="server" visible="false" style="margin-top: 2rem; justify-content: space-between">
                                                            <asp:Button ID="btnCfmOtp" runat="server" CssClass="BtnOtp" ValidationGroup="OTP"
                                                                Text="Submit" OnClick="btnCfmOtp_Click" />
                                                            <asp:Button ID="btnResend" runat="server" Text="00:30" CssClass="btnResnd"
                                                                OnClick="btnResend_Click" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <%--   Passdetails--%>
                                                <div id="divPassdetails" runat="server" visible="false" style="margin-bottom: 0.5rem">
                                                    <div class="Date__Container">
                                                        <div class="Date From__Date">
                                                            <asp:Label ID="Label9" runat="server" Text="Pass Id :" CssClass="lblPass"></asp:Label>
                                                            <asp:Label ID="lblpassId" runat="server" CssClass="lblPassSummary"></asp:Label>

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
                                                        <%--  <div class="Date From__Date">
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

                                                </div>


                                            </div>
                                            <div class="Feature__container">
                                                <div class="Feature__summary-container">
                                                    <div id="extrafeeandfeatursContainer" runat="server" class="extrasummary2 mb-3" visible="false">

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
                                                            <%--                                                        <asp:DataList ID="dtlExtraFeeSummary" runat="server" Width="100%">
                                                            <HeaderTemplate>
                                                                <div class="row">
                                                                    <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                                                                        <div class="row">
                                                                            <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                                                                <span class="main__head Extra__text" style="color: #000000; font-weight: bolder; font-size: 1.35rem;">Add-On Service</span>
                                                                            </div>
                                                                            <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                                                                                <span class="main__head ExtraFeatures__text" style="color: #000000; font-weight: bolder; font-size: 1.35rem;">Count</span>
                                                                            </div>
                                                                            <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12  text-right">
                                                                                <span class="main__value" style="font-weight: bolder; color: black; font-size: 1.35rem;"
                                                                                    runat="server">Amount </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div class="row">
                                                                    <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12" style="height: 20px; font-size: 13px;">
                                                                        <div class="row">
                                                                            <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12" style="margin-top: -23px;">
                                                                                <span class="main__head ExtraFeatures__text">
                                                                                    <asp:Label ID="lblfeevehicleAccessoriesName"
                                                                                        runat="server"
                                                                                        Text=' <%# Eval("vehicleAccessoriesName") %>' Style="text-transform: capitalize;"> </asp:Label>
                                                                                    <asp:Label ID="lblgvfeePriceid"
                                                                                        runat="server"
                                                                                        Text=' <%# Eval("priceID") %>' Visible="false"> </asp:Label>
                                                                                </span>
                                                                            </div>
                                                                            <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12" style="margin-top: -23px;">
                                                                                <span class="main__head ExtraFeatures__text">
                                                                                    <asp:Label ID="lblfeeCount"
                                                                                        runat="server"
                                                                                        Text='<%# Eval("Count") %>'> </asp:Label>
                                                                                </span>
                                                                            </div>
                                                                            <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-right" style="margin-top: -2px;">
                                                                                <span class="main__head ExtraFeatures__text" runat="server">₹&nbsp;<asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                                                    Text='<%# Convert.ToDecimal(Eval("totalAmount")).ToString("0.00") %>'
                                                                                    CssClass="lblSummary"> </asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>

                                                        </asp:DataList>--%>
                                                        </div>
                                                    </div>
                                                    <div id="divParkingAmount" runat="server">
                                                        <div class="Feature__summary Feature__Parking">
                                                            <span id="Parking__amount" class="main__head parking__text" style="color: #000000; font-weight: 700;">parking amount</span>
                                                            <span class="main__value parking__value" runat="server">&#8377;&nbsp;
                                         <asp:Label ID="lblparkingAmount" runat="server" Text="0.00" Style="color: #000000; font-weight: 700;" CssClass="lblSummary"> </asp:Label>
                                                            </span>
                                                        </div>
                                                        <%--<hr style="margin-top: 0rem; margin-bottom: 0rem" />--%>
                                                    </div>
                                                    <div id="divExtraFeeAmount" runat="server" visible="true">
                                                        <div class="Feature__summary Feature__Extra">
                                                            <span class="main__head Extra__text" style="color: #000000; font-weight: 700;">Add-On Service</span>
                                                            <span class="main__value Extra__value" runat="server">&#8377;&nbsp;
                                          <asp:Label ID="lblAccessoriesTotalAmount" Style="color: #000000; font-weight: 700;" runat="server" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                            </span>

                                                        </div>
                                                        <%-- <hr style="margin-top: 0rem; margin-bottom: 0rem" />--%>
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

                                                    <div class="Feature__summary Feature__Total">
                                                        <span class="main__head Total__text">total</span>
                                                        <span class="main__value Total__value" style="color: green;">&#8377;&nbsp;
                                         <asp:Label ID="lblTotalAmount" runat="server" Style="font-size: 22px; color: green;" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                        </span>
                                                    </div>
                                                </div>

                                                <%-- <div class="Date__Container" style="display: flex; justify-content: space-between; position: relative;" runat="server" id="divPaymentTypeBook">
                                                <div class="Date From__Date">
                                                    <asp:Label
                                                        ID="lblPaymentType"
                                                        runat="server"
                                                        Text="Payment Type" CssClass=" lblFeaturesDate lblDate" Style="color: #000000; font-weight: 700;">
                                                    </asp:Label>
                                                </div>
                                                <div class="Date To_Date">
                                                    <asp:DropDownList
                                                        ID="ddlPaymentType"
                                                        runat="server"
                                                        TabIndex="14"
                                                        CssClass="txtSummary txtFeature form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            
                                            <div class="book1 mt-3" style="justify-content: space-between;">
                                                <asp:Button ID="btnBook" OnClick="btnBook_Click" TabIndex="15"
                                                    CssClass="bookbtn" Width="35%" Enabled="false"
                                                    Text="Book" ValidationGroup="BookingSlot" runat="server" />
                                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" TabIndex="16"
                                                    CssClass="Cancelbtn" Width="35%"
                                                    Text="Cancel" CausesValidation="false" runat="server" />

                                            </div>--%>
                                                <div id="divPrintCheck" runat="server" style="margin-left: 2rem;">
                                                    <asp:CheckBox CssClass="PrintChk" ID="ChkPrint" runat="server" /><a class="lblPrint">Print</a>
                                                </div>
                                                <div class="book1 mt-3 col-sm-12 col-xs-12" style="display: flex; justify-content: space-between; padding: 0px;">
                                                    <div class="col-sm-4 col-xs-6 p-0 text-center">

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
            <Triggers>
                <asp:PostBackTrigger ControlID="btnBook" />
            </Triggers>
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
        <div id="div1" runat="server" visible="true">
            <div class="row">
                <asp:LinkButton ID="btnNewPass" runat="server" OnClick="btnNewPass_Click"
                    CssClass="btnBgColorAdd newpass">
                      New Pass</asp:LinkButton>

            </div>
        </div>

        <asp:HiddenField ID="hfOtp" runat="server" />
        <!-- jQuery CDN - Slim version (=without AJAX) -->
        <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
        <!-- Popper.JS -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js" integrity="sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ" crossorigin="anonymous"></script>


        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>



        <asp:HiddenField ID="HiddenField1" EnableViewState="true" runat="server" />
        <asp:HiddenField ID="hfPrevImageLink" EnableViewState="true" runat="server" />
        <asp:HiddenField ID="hfImageUrl" runat="server" />


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
    <!-- Grid ViewSearch -->
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>

    <%-- <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>--%>
    <script src="../js/flatpickr.js"></script>
    <script>
        const datepicker = document.getElementsByClassName('datePicker');
        const fromDate = document.getElementsByClassName('fromDate');
        const toDate = document.getElementsByClassName('toDate');
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

    <script>
        function show() {
            document.getElementById('<%=modalpasssub.ClientID %>').style.cssText = "#model";
        }
        function hide() {
            document.getElementById('<%=modalpasssub.ClientID %>').style.display = "none";

        }
    </script>
    <script type="text/javascript">
        function PlusOnclick() {
            document.getElementById('<%=txtBookingId.ClientID %>').value = "";
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
    <!-- Grid ViewSearch -->
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $(".gvv").prepend($("<thead></thead>").append($(".display").find("tr:first"))).dataTable({
                "lengthMenu": [[5, 10, 15, 20, 25, -1], [5, 10, 15, 20, 25, "All"]] //value:item pair
            });
        });
    </script>
</body>
</html>

