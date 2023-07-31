<%@ Page Title="DashBoard" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<asp:Content ID="frmDashBoard" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .DisplyCard {
            position: fixed;
            text-align: center;
            height: 100%;
            width: 100%;
            top: 0;
            right: 0;
            left: 0;
            z-index: 9999999;
            background-color: #585858ad;
            opacity: 1.9;
        }

        .DisplyCardPostion {
            position: fixed;
            width: 50%;
            height: 50%;
            box-shadow: rgb(0 0 0 / 56%) 0px 10px 70px 4px;
            background-color: #ffffff;
            left: 30%;
            top: 20%;
            border-radius: 25px;
        }

        .close {
            float: right;
            font-size: 20px;
            color: red;
            position: sticky;
            opacity: 1;
        }

        .card-title {
            font-size: 24px;
            color: #ffffff;
            font-weight: 700;
            margin-bottom: 0px;
            padding: 8px;
        }

        .card-title1 {
            font-size: 24px;
            color: #1ca4ff;
            font-weight: 700;
            margin-bottom: 0px;
            padding: 8px;
        }

        .Card-title-second {
            color: #050505;
            font-weight: 300;
        }
    </style>

    <link type="text/css" href="Dashboard/assets/css/master.css" rel="stylesheet">
    <div class="container-fluid containerBg">
        <div id="divForm" runat="server">
            <div class="divTitle row pt-4 pl-2 mt-4 justify-content-between" style="margin-left: -26px;">
                <div>
                    <h4 class="card-title1"><span id="spAddorEdit" runat="server"></span>Dashboard <span class="Card-title-second">Overview </span></h4>
                </div>
            </div>
            <div class="row" style="margin-top: -25px;">
                <div id="divddlBranch" runat="server" class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3" visible="false">
                    <asp:Label ID="lblBranches" runat="server" Text="Branch List"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="vdgDashBoard"
                        ControlToValidate="txtfrmdate" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Branch List">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblFromDate"
                        runat="server"
                        Text="From Date" CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtfrmdate" runat="server" TabIndex="1"
                        CssClass="form-control fromDate labels"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="vdgDashBoard"
                        ControlToValidate="txtfrmdate" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select From Date">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblTodate"
                        runat="server"
                        Text="To Date" CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txttodate" runat="server" TabIndex="2"
                        CssClass="form-control toDate labels"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="vdgDashBoard"
                        ControlToValidate="txttodate" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select To Date">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 align-self-center">
                    <asp:Button ID="btnSubmit" CssClass="pure-material-button-contained btnBgColorAdd mr-2" OnClick="btnSubmit_Click"
                        Text="Submit" ValidationGroup="vdgDashBoard" runat="server" />
                </div>
            </div>
            <div class="row" style="margin-top: -15px;">
                <div class="col-sm-6 col-md-6 col-lg-3 mt-3">
                    <div class="card">
                        <div class="cardcontent">
                            <div class="row">
                                <div class="col-sm-4" style="height: 80px; margin-top: -20px; background-image: url(Dashboard/assets/Images/icons100.png); background-size: cover;">
                                    <div class="icon-big text-center">
                                        <%-- <i class="teal fas fa-shopping-cart"></i>--%>
                                        <%--    <img src="Dashboard/assets/Images/icons100.png" />--%>
                                    </div>
                                </div>
                                <div class="col-sm-8">
                                    <div class="detail">
                                        <p class="detail-subtitle">Available</p>
                                        <asp:LinkButton ID="lblAvailable" runat="server" OnClick="lblAvailable_Click" class="linknumber"></asp:LinkButton>
                                        /
                                        <asp:Label ID="lblTotalSlot" runat="server" class="number"></asp:Label>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-3 mt-3">
                    <div class="card">
                        <div class="cardcontent">
                            <div class="row">
                                <div class="col-sm-4" style="height: 58px; background-image: url(Dashboard/assets/Images/checkin100.png); background-size: cover;">
                                    <div class="icon-big text-center">
                                        <%-- <i class="olive fas fa-money-bill-alt"></i>--%>
                                    </div>
                                </div>
                                <div class="col-sm-8">
                                    <div class="detail">
                                        <p class="detail-subtitle">Check In</p>
                                        <asp:LinkButton ID="lblCheckIn" runat="server" OnClick="lblCheckIn_Click" class="linknumber"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-3 mt-3">
                    <div class="card">
                        <div class="cardcontent">
                            <div class="row">
                                <div class="col-sm-4" style="height: 58px; background-image: url(Dashboard/assets/Images/checkin100.png); background-size: cover; transform: rotate(180deg);">
                                    <div class="icon-big text-center">
                                        <%--  <i class="violet fas fa-eye"></i>--%>
                                    </div>
                                </div>
                                <div class="col-sm-8">
                                    <div class="detail">
                                        <p class="detail-subtitle">Check Out</p>
                                        <asp:LinkButton ID="lblCheckOut" runat="server" class="linknumber" OnClick="lblCheckOut_Click"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-3 mt-3">
                    <div class="card">
                        <div class="cardcontent">
                            <div class="row">
                                <div class="col-sm-4" style="background-image: url(Dashboard/assets/Images/booked75.png); background-size: cover;">
                                    <div class="icon-big text-center">
                                        <%--<i class="orange fas fa-envelope"></i>--%>
                                    </div>
                                </div>
                                <div class="col-sm-8">
                                    <div class="detail">
                                        <p class="detail-subtitle">Booked</p>
                                        <asp:LinkButton ID="lblBooked" runat="server" class="linknumber" OnClick="lblBooked_Click"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>



            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-6" style="display: none">
                            <div class="card">
                                <div class="cardcontent">
                                    <div class="head">
                                        <h5 class="mb-0">Parking Overview</h5>
                                        <p class="text-muted">Current Year Parking visitor data</p>
                                    </div>
                                    <div class="canvas-wrapper">
                                        <canvas class="chart" id="trafficflow"></canvas>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="card">
                                <div class="cardcontent">
                                    <div class="head">
                                        <p class="text-muted">Income Overview</p>
                                    </div>
                                    <div class="canvas-wrapper">
                                        <canvas class="chart" id="Income"></canvas>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfTotalAmount" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfPaidAmount" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfRemainingAmount" runat="server" ClientIDMode="Static" />

    <script src="Dashboard/assets/vendor/chartsjs/Chart.min.js" type="text/javascript"></script>
    <script src="Dashboard/assets/js/dashboard-charts.js" type="text/javascript"></script>
    <style>
        hr {
            margin-bottom: 2rem !important;
        }
    </style>


    <div id="modal" class="DisplyCard" runat="server" style="display: none">
        <div class="DisplyCardPostion table-responsive section">


            <div class="divTitle row mt-4 pt-4 pl-4 justify-content-between" style="background: #1ca4ff; position: sticky; left: -15px; top: 15px;">
                <div>
                    <h4 class="card-title"><span id="Span1" runat="server"></span>
                        <asp:Label ID="lblGvCount" runat="server"></asp:Label>
                        <span class="Card-title-second">List </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="linkclose" CssClass="close" runat="server" OnClick="linkclose_Click">
                <i class="fa-solid fa-xmark"></i></asp:LinkButton>
                </div>
            </div>
            <div id="divBookedGridView" runat="server" visible="false">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvBooked"
                        runat="server"
                        AllowPaging="True"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbookingId"
                                        runat="server"
                                        Text='<%#Bind("bookingId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Slot Id" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvslotId"
                                        runat="server"
                                        Text='<%#Bind("slotId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleTypeName"
                                        runat="server"
                                        Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtotalAmount"
                                        runat="server"
                                        Text='<%#Bind("totalAmount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="divCheckOutGridView" runat="server" visible="false">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvCheckOut"
                        runat="server"
                        AllowPaging="True"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbookingId"
                                        runat="server"
                                        Text='<%#Bind("bookingId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Number" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleNumber"
                                        runat="server"
                                        Text='<%#Bind("vehicleNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleTypeName"
                                        runat="server"
                                        Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="divCheckInGridView" runat="server" visible="false">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvCheckIn"
                        runat="server"
                        AllowPaging="True"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Booking Id" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbookingId"
                                        runat="server"
                                        Text='<%#Bind("bookingId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Number" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleNumber"
                                        runat="server"
                                        Text='<%#Bind("vehicleNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleTypeName"
                                        runat="server"
                                        Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="divAvailableGridView" runat="server" visible="false">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvAvailableList"
                        runat="server"
                        AllowPaging="True"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VIP" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbookingId"
                                        runat="server"
                                        Text='<%#Bind("VIP") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleNumber"
                                        runat="server"
                                        Text='<%#Bind("Normal") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <script>
        function show() {
            document.getElementById('<%=modal.ClientID %>').style.cssText = "#model";

        }
        function hide() {
            document.getElementById('<%=modal.ClientID %>').style.display = "none";

        }
    </script>

</asp:Content>


