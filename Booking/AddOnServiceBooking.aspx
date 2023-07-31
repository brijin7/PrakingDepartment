<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddOnServiceBooking.aspx.cs" Inherits="Booking_AddOnServiceBooking" %>

<!DOCTYPE html>

<html style="overflow-x: hidden;" class="section">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Add-On Service Booking</title>
    <link href="../fav.ico" rel="shortcut icon" type="image/x-icon" />
    <link rel="stylesheet" href="./style.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css" integrity="sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4" crossorigin="anonymous">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"
        integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />
    <%-- Style and Script for the Time Picker--%>

    <!-- Material design Cdn -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/MaterialDesign-Webfont/6.7.96/css/materialdesignicons.min.css" integrity="sha512-q0UoFEi8iIvFQbO/RkDgp3TtGAu2pqYHezvn92tjOq09vvPOxgw4GHN3aomT9RtNZeOuZHIoSPK9I9bEXT3FYA==" crossorigin="anonymous" referrerpolicy="no-referrer" />

    <!-- Material design Icon -->
    <script src="https://code.iconify.design/2/2.2.1/iconify.min.js"></script>

    <!-- Summary -->
    <%--Date Picker--%>
    <link href="../Style/DatePicker.css" rel="stylesheet" />

    <!-- Owl  Carousel stylesheet -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/assets/owl.carousel.min.css" integrity="sha512-tS3S5qG0BlhnQROyJXvNjeEM4UpMXHrQfTGmbQ1gKmelCxlSEBUaxhRBj/EFTzpbP4RVSrpEikbmdJobCvhE3g==" crossorigin="anonymous" referrerpolicy="no-referrer" />

    <!-- Owl Carousel Theme -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/assets/owl.theme.default.min.css" integrity="sha512-sMXtMNL1zRzolHYKEujM2AqCLUR9F2C4/05cdbxjjLSRvMQIciEPCQZo++nk7go3BtSuK9kfa/s+a4f4i5pLkw==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link href="../Style/Booking.css" rel="stylesheet" />
    <link href="../Style/BookingSlots.css" rel="stylesheet" />
    <link href="../Style/SlotSummary.css" rel="stylesheet" />
    <link href="../Style/Navigation.css" rel="stylesheet" />
    <link href="../Style/Nav.css" rel="stylesheet" />
    <link href="../Style/ContentPage.css" rel="stylesheet" />
    <link href="../Style/Table.css" rel="stylesheet" />
    <link href="../Style/ParkingPass.css" rel="stylesheet" />

    <style>
        .divIcon {
            text-align: center;
            padding: 1rem;
            margin: 1rem;
            color: white;
            transition: all 0.3s;
        }

        .divIcon1 {
            text-align: center;
            padding: 1rem;
            margin: 1rem;
            color: #1ca4ff;
            transition: all 0.3s;
        }

        .extraFeatures {
            max-height: 25rem;
            height: 8rem;
            box-shadow: #00000059 0px 4px 16px !important;
            border-radius: 1rem;
            overflow: hidden;
            width: auto;
            cursor: pointer;
        }

        .gvHead {
            background-color: #b4cfe5;
        }

        /*.parkinglogoimg {
            margin-top: -20px;
            width: 100px;
            height: 100px;
        }*/
        .parkinglogoimg {
            margin-top: -42px;
            margin-bottom: -40px;
            width: 52px;
            height: 52px;
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
            margin: 5px;
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
            padding: 8px 8px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #1b211a;
            outline: none;
            margin: 5px;
            background: #283138;
            color: #fff;
            margin-left: -2px;
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
            border-radius: 12px;
            border-style: dashed;
            padding: 5px;
        }

        .carimage {
            border-radius: 25px;
            padding-top: 6px;
            padding-left: 20px;
            /*padding-right: 20px !important;*/
            height: 48px;
            color: black;
            opacity:0.35;
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

        .modal-body {
            opacity: 1 !important;
            transform: translateY(1);
        }

        .model {
            opacity: 1;
            visibility: visible;
            z-index: 1;
        }


        .modal-body {
            max-width: 500px;
            opacity: 0;
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
            height: 69%;
            max-height: 500px;
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
            width: 100%;
            height: 40px;
            border-color: skyblue;
            border-radius: 30px;
            font-size: 18px !important;
            text-align: center;
            border: none;
            margin: 8px;
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

        .form-control {
            border: 1px solid #e5e5e6;
        }

        .vehiclename {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .backgroundimage {
            background-image: url(../images/Backgroundcar.png);
            background-repeat: no-repeat;
            background-size: cover;
            height: 1000px;
        }
    </style>
    <!--jquery-3.3.1-->
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
            let buttonConfrm = document.getElementById('<%=btnCfmOtp.ClientID %>');
            let divEnterOtp = document.getElementById('<%=divEnterOtp.ClientID %>');
            let txtmobileNo = document.querySelector('#<%=txtmobileNo.ClientID%>');

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

</head>
<body>
    <form id="form1" runat="server">

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
                            <h5 class="FormName mt-1">Add-On Service Booking</h5>

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
        <div class="row mt-3" id="divFullPage" runat="server" style="width: 100%;">
            <div class="col-md-6 col-sm-6 col-lg-6 col-xl-6">
                <div id="divBlockFloor" runat="server" visible="false" class="row" style="margin-bottom: 5px; margin-left: 0px;">
                    <div class="col-6 col-sm-4 col-md-4 col-lg-4 col-xl-4" style="display: inline-flex; height: 30px;">
                        <%--  <asp:Label
                            ID="lblblock"
                            runat="server"
                            Text="Block" CssClass="mt-2 mr-1">
                        </asp:Label>--%>
                        <asp:DropDownList
                            ID="ddlblock"
                            runat="server" AutoPostBack="true"
                            TabIndex="1" OnSelectedIndexChanged="ddlblock_SelectedIndexChanged"
                            CssClass="form-control ">
                        </asp:DropDownList>
                    </div>
                    <div class="col-6 col-sm-4 col-md-4 col-lg-4 col-xl-4" style="display: inline-flex; height: 30px;">
                        <%--   <asp:Label
                            ID="lblfloor"
                            runat="server"
                            Text="Floor" CssClass="mt-2 mr-1">
                        </asp:Label>--%>
                        <asp:DropDownList
                            ID="ddlfloor" AutoPostBack="true"
                            runat="server" OnSelectedIndexChanged="ddlfloor_SelectedIndexChanged"
                            TabIndex="2"
                            CssClass="form-control ">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager"
            runat="server" EnablePartialRendering="true" />
        <asp:UpdatePanel ID="UpdatePanel1"
            UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <fieldset>

                    <div class="Formbackground">
                        <div id="divForm" runat="server" class="divformbg" visible="true" style="margin-top: -30px;">
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12  col-xl-12  divVehiclebgtable-responsive section" style="overflow-y: hidden">
                                    <asp:DataList ID="gvVehicleType" runat="server" Visible="true"
                                        RepeatDirection="Horizontal" OnItemCommand="gvVehicleType_ItemCommand">
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

                                                        <%-- <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3" id="DivVehileTypes" runat="server" style="margin-top: -14px;">
                                                            <asp:Image ID="image" Style="height: 60px !important; width: 60px;"
                                                                runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                                        </div>--%>

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


                            </div>
                        </div>
                        <div style="padding-left: 55px; padding-top: 4px; margin-bottom: -14px;">
                            <asp:CheckBox ID="ChkMobileNo" runat="server" Style="zoom: 1.5;" Width="17px"
                                OnCheckedChanged="ChkMobileNo_CheckedChanged" Checked="true"
                                AutoPostBack="true" /><b style="font-size: 20px; color: black;">Mobile No</b>
                        </div>
                        <div class="slot__container divslotbg mt-4" runat="server" id="DivSummaryFull">
                            <div class="row">
                                <div class="col-7 col-sm-7 col-md-7 col-lg-7 col-xl-7 mt-3 table-responsive section">
                                    <div id="divSummary" class="Slot__Summary mb-2"
                                        runat="server" visible="false">
                                        <div class="DateAndFeatures__Container" style="margin-bottom: 0rem">
                                            <div class="Date__Container" style="margin-bottom: 0rem;">
                                                <div class="Date From__Date" style="width: 200px; margin-left: 16px;" id="divMobileNo" runat="server">
                                                    <asp:Label
                                                        ID="lblMobNo"
                                                        runat="server"
                                                        Text="Mobile No.">
                                                    </asp:Label><span class="spanStar">*</span>
                                                    <asp:TextBox
                                                        ID="txtmobileNo" onkeypress="return isNumber(event);"
                                                        MaxLength="10" MinLength="10" AutoPostBack="true"
                                                        runat="server"
                                                        TabIndex="7" OnTextChanged="txtmobileNo_TextChanged"
                                                        CssClass="txtSummary txtDate txtFromDate form-control">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator
                                                        ID="RfvMobNo"
                                                        ValidationGroup="BookingSlot"
                                                        ControlToValidate="txtmobileNo"
                                                        runat="server"
                                                        CssClass="rfvClr"
                                                        ErrorMessage="Enter Mobile No.">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                        ControlToValidate="txtmobileNo" ErrorMessage="Invalid Mobile No."
                                                        ValidationExpression="[0-9]{10}" CssClass="rfvClr" ValidationGroup="BookingSlot"></asp:RegularExpressionValidator>
                                                </div>

                                            </div>

                                            <div id="divOtpDetails" class="Date__Container" runat="server" style="margin-left: 16px;" visible="false">

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
                                                            TabIndex="8" Style="width: 200px;"
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
                                                <div class="Date To__Date" style="margin-left: -168px;">
                                                    <div id="divResend" runat="server" visible="false" style="margin-top: 2rem; justify-content: space-between">
                                                        <asp:Button ID="btnCfmOtp" runat="server" CssClass="BtnOtp" ValidationGroup="OTP"
                                                            Text="Submit" OnClick="btnCfmOtp_Click" />
                                                        <asp:Button ID="btnResend" runat="server" Text="00:30" CssClass="btnResnd"
                                                            OnClick="btnResend_Click" />
                                                    </div>
                                                </div>
                                            </div>
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
                                                                    <asp:Label ID="lblGvtimeslabId" runat="server"
                                                                        CssClass="feature__name" Visible="false" Text='<%# Eval("timeslabId") %>'></asp:Label>
                                                                </div>
                                                                <div class="feature__Amount">
                                                                    <span>₹</span><asp:LinkButton ID="lblgvFeeTotalAmount" runat="server" Text='<%# Eval("totalAmount") %>'></asp:LinkButton>
                                                                </div>

                                                                </span>
                                                           <%--<div class="addFeature__item">
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
                                <div class="col-5 col-5 col-sm-5 col-md-5 col-lg-5 col-xl-5 mt-3 table-responsive section">

                                    <div id="divslotSummary" class="Slot__Summary mb-3"
                                        runat="server">
                                        <div id="divSummaryColor" runat="server" style="background-color: #c1c0c36b; border-radius: 10px; padding: 10px">
                                            <div class="col-6">
                                                <h3 class="summary__header" style="margin-left: -15px; font-weight: 700;">Summary</h3>
                                            </div>
                                            <div class="Feature__container">
                                                <div class="Feature__summary-container">

                                                    <div id="extrafeeandfeatursContainer" runat="server" class="extrasummary2 mb-3" visible="false">
                                                        <div class="Feature__summary Feature__Extra" id="header" runat="server" visible="true">
                                                            <span class="main__head Extra__text" style="color: #000000; font-weight: bolder;">Add-On service</span>
                                                            <span class="main__head ExtraFeatures__text" style="color: #000000; margin-left: -46px; font-weight: bolder;">Count</span>
                                                            <span class="main__value Extra__value" style="color: #000000; font-weight: bolder;" runat="server">
                                                                <asp:Label ID="lblAccessoriesTotalAmount" runat="server" Text="Amount" CssClass="lblSummary"> </asp:Label>
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
                                                                            <asp:Label ID="lblgvfeetimeslabId"
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
                                                    <div class="Feature__summary Feature__Extra" id="divExtraFeeAmount" runat="server" visible="true" style="margin-top: 5px">
                                                        <span class="main__head Extra__text" style="color: #000000; font-weight: 700;">Add-On Service</span>
                                                        <span class="main__value Extra__value" runat="server">&#8377;&nbsp;
                                          <asp:Label ID="lblAccessoriesTotalAmounts" Style="color: #000000; font-weight: 700;" runat="server" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                        </span>

                                                    </div>
                                                    <%--  <hr style="margin-top: 0rem; margin-bottom: 0rem" />--%>
                                                    <div class="Feature__summary Feature__ExtraFeatures" id="divTax" runat="server" visible="true">
                                                        <span class="main__head ExtraFeatures__text" style="color: #000000; font-weight: 700;">GST</span>
                                                        <span class="main__value ExtraFeatures__value" runat="server">&#8377;&nbsp;
                                          <asp:Label ID="lblGSTAmount" runat="server" Style="color: #000000; font-weight: 700;" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                        </span>
                                                    </div>
                                                    <hr style="margin-top: 0rem; margin-bottom: 0rem" />
                                                    <div class="Feature__summary Feature__Total" style="margin-top: 5px;">
                                                        <span class="main__head Total__text" style="font-size: 22px;">total</span>
                                                        <span class="main__value Total__value" style="color: green;">&#8377;&nbsp;
                                         <asp:Label ID="lblTotalAmount" runat="server" Style="font-size: 22px; color: green;" Text="0.00" CssClass="lblSummary"> </asp:Label>
                                                        </span>
                                                    </div>
                                                </div>

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
                    <div id="Formbackground" runat="server"></div>
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

        <asp:HiddenField runat="server" ID="hfPinNo" />
        <asp:HiddenField ID="hfOtp" runat="server" />

    </form>
    <script src="./script.js"></script>

    <!-- Bootstrap JS -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js" integrity="sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm" crossorigin="anonymous"></script>

    <!-- Grid ViewSearch -->
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
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
                dateFormat: "d-m-Y",
                time_24hr: false,
                minDate: "today",
                defaultDate: "today",
                onOpen: function () {
                    const numInput = document.querySelectorAll('.numInput');
                    numInput.forEach((input) => input.type = '');
                }

            });

        fp = flatpickr(fromDate,
            {
                enableTime: false,
                dateFormat: "d-m-Y",
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
                            dateFormat: "d-m-Y",
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
                        dateFormat: "d-m-Y",
                        time_24hr: false,
                        minDate: new Date().fp_incr(1),
                        onOpen: function () {
                            const numInput = document.querySelectorAll('.numInput');
                            numInput.forEach((input) => input.type = '');
                        }
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
                        minTime: "today",

                    });
            }

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
