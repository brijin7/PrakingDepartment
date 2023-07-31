<%@ Page Title="Check In & Check Out" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="CheckInCheckOut.aspx.cs" Inherits="CheckInandOut_CheckInCheckOut" EnableEventValidation="false" %>

<asp:Content ID="cnCheckInCheckOut" ContentPlaceHolderID="MasterPage" runat="Server">
    <link href="../Style/SlotSummary.css" rel="stylesheet" />
    <style>
        .form-control {
            border: 1px solid #e5e5e6;
        }

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

        .btnCheckIn {
            border: 1px solid #2eab4a;
            background-color: white;
            color: #2eab4a;
            border-radius: 13px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
        }

        .btnCheckout {
            border: 1px solid var(--danger);
            background-color: white;
            color: var(--danger);
            border-radius: 13px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            margin-left: 5px;
        }

            .btnCheckout:hover, .btnCheckout:active {
                color: white;
                background-color: rgb( 33, 150, 243);
                border: 1px solid var(--blue);
            }

        .btnCheckIn:hover, .btnCheckIn:active {
            color: white;
            background-color: rgb( 33, 150, 243);
            border: 1px solid var(--blue);
        }

        label {
            font-weight: 900;
            color: black;
            font-size: 1.8rem;
        }

        labels {
            font-weight: 900;
            color: #2196f3;
            font-size: 1.5rem;
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
            opacity: 0;
            transition: opacity 0.25s ease-in-out;
            visibility: hidden;
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
                max-width: 500px;
                opacity: 0;
                transform: translateY(-100px);
                transition: opacity 0.25s ease-in-out;
                width: 100%;
                z-index: 1;
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
            margin-top: -217px;
            max-width: 918px;
            width: 100%;
        }

        .BarCodeTextStart {
            width: 200%;
            height: 45.5px !important;
            border-color: skyblue;
            FONT-WEIGHT: 200;
            font-size: 16.5px !important;
            font-weight: bold;
            margin-top: 10px;
            text-transform: uppercase;
        }

        .extrasummary2 {
            border-width: 2px;
            border-color: #2196f373;
            border-radius: 13px;
            border-style: dashed;
            padding: 5px;
        }
    </style>

    <div class="container-fluid containerBg" id="divScreen" runat="server">
        <div class="row PageRoutePos">
            <div>
                <asp:Label ID="lblMainPage" CssClass="pageRoutecol" runat="server" Text="Home"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Check In & Check Out"></asp:Label>
            </div>

        </div>
        <div id="divForm" runat="server" visible="true">
            <div class="row" style="margin-top: -6px;">
                <div class="row col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8" id="divBlockFloor" runat="server">
                    <div class="col-12 col-sm-6 col-md-6 col-lg-5 col-xl-5" style="display: inline-flex;">
                        <asp:Label
                            ID="lblblock"
                            runat="server"
                            CssClass="form-check-label"
                            Font-Bold="true">Block
                        </asp:Label><span class="spanStar">*</span> &nbsp;&nbsp;
                        <asp:DropDownList
                            ID="ddlblock"
                            runat="server" AutoPostBack="true" style="height:30px !important"
                            TabIndex="1" OnSelectedIndexChanged="ddlblock_SelectedIndexChanged"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-sm-6 col-md-6 col-lg-5 col-xl-5" style="display: inline-flex;">
                        <asp:Label
                            ID="lblfloor"
                            runat="server"
                            CssClass="form-check-label"
                            Text="Floor" Font-Bold="true">
                        </asp:Label><span class="spanStar">*</span> &nbsp;&nbsp;
                        <asp:DropDownList
                            ID="ddlfloor" AutoPostBack="true" style="height:30px !important"
                            runat="server" OnSelectedIndexChanged="ddlfloor_SelectedIndexChanged"
                            TabIndex="2"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4 pb-4" style="color: white">
                    <asp:RadioButtonList
                        ID="rbtnTypeFP"
                        runat="server"
                        CssClass="inline-rb"
                        TabIndex="1"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="rbtnTypeFP_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="Booking" Value="B" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Pass" Value="P"></asp:ListItem>
                    </asp:RadioButtonList>

                </div>
            </div>
            <div class="divTitle row pt-4 pl-4 justify-content-between" id="divBTitle" runat="server" visible="false">
                <div>
                    <h4 class="card-title" runat="server" id="Header"><span class="Card-title-second" runat="server" id="Headerspan"></span></h4>
                </div>
            </div>

            <div class="row" id="divBtncheckinandout" runat="server" visible="false">

                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 justify-content-start form-inline">
                    <span
                        data-toggle="tooltip"
                        data-placement="bottom"
                        data-original-title="Check In">
                        <asp:LinkButton class="mdi mdi-car-arrow-left mdi-36px btnCheckIn"
                            Style="width: 60px; outline: none;"
                            ID="btnCheckIn" runat="server" TabIndex="5"
                            OnClick="btnCheckIn_Click" OnClientClick="showLoader();" ValidationGroup="CheckINCheckOut" /></span>
                    <span
                        data-toggle="tooltip"
                        data-placement="bottom" data-size=""
                        data-original-title="Check Out">
                        <asp:LinkButton class="mdi mdi-car-arrow-right mdi-36px btnCheckout"
                            Style="width: 60px; outline: none;" ID="btnCheckOut" runat="server" TabIndex="5"
                            OnClick="btnCheckOut_Click" OnClientClick="showLoader();" ValidationGroup="CheckINCheckOut" /></span>

                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4" style="margin-top: -5px;">
                    <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control BarCodeTextStart" AutoPostBack="true"
                        placeholder=" Scan QR Code/ Enter Vehicle No./ Mobile No./ Pin No."
                        OnTextChanged="txtBookingId_TextChanged" AutoComplete="Off"></asp:TextBox>
                </div>
            </div>


        </div>
        <br />
        <div id="divCheckIn" runat="server" visible="false">
            <br />
            <div style="margin-left: auto; margin-right: auto; text-align: center; margin-top: -35px; margin-bottom: 11px;">
                <asp:Label ID="lblGridIn" runat="server" Style="color: #106d25"
                    CssClass="blink" Font-Bold="true" Font-Size="X-Large" Visible="false"></asp:Label>
                <asp:Label ID="lblAlreadyIn" runat="server" ForeColor="Blue"
                    CssClass="blink" Font-Bold="true" Font-Size="X-Large" Visible="false"></asp:Label>
                <br />
                <asp:Label ID="lblgridmsgIn" runat="server" ForeColor="Red" Visible="false"></asp:Label>

            </div>

            <div class="table-responsive section">

                <asp:GridView
                    ID="gvCheckIn" runat="server" AllowPaging="True" DataKeyNames="bookingPassId"
                    CssClass="gvv display" AutoGenerateColumns="false" BorderStyle="None"
                    PageSize="25000" OnRowDataBound="gvCheckIn_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booking Id">
                            <ItemTemplate>
                                <asp:Label ID="lblgvINbookingPassId" runat="server" Text='<%# Eval("bookingPassId") %>'></asp:Label>
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
                                    src="../images/check-in.png" alt="image"
                                    OnClick="gvbtnCheckIn_Click" OnClientClick="showLoader();" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView
                    ID="gvCheckInPass" runat="server" AllowPaging="True" DataKeyNames="bookingPassId"
                    CssClass="gvv display" AutoGenerateColumns="false" BorderStyle="None"
                    PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booking Id">
                            <ItemTemplate>
                                <asp:Label ID="lblgvINbookingPassId" runat="server" Text='<%# Eval("bookingPassId") %>'></asp:Label>
                                <asp:Label
                                    ID="lblbookingPassIdIn"
                                    runat="server"
                                    Text='<%#Bind("bookingPassId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleHeaderId" runat="server" Text='<%#Bind("vehicleHeaderId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleType" runat="server" Text='<%#Bind("vehicleType") %> ' Visible="false"></asp:Label>

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
                        <asp:TemplateField HeaderText="Check In" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:ImageButton ID="gvbtnCheckInPass"
                                    runat="server"
                                    Text="Edit"
                                    src="../images/check-in.png" alt="image"
                                    OnClick="gvbtnCheckInPass_Click" OnClientClick="showLoader();" />
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
                <br />
                <asp:Label ID="lblgridmsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>


            <div class="table-responsive section">

                <asp:GridView
                    ID="gvCheckOut" runat="server" AllowPaging="True" DataKeyNames="bookingPassId"
                    CssClass="gvv display" AutoGenerateColumns="false" BorderStyle="None"
                    PageSize="25000" OnRowDataBound="gvCheckOut_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booking Id">
                            <ItemTemplate>
                                <asp:Label ID="lblgvbookingPassId" runat="server" Text='<%# Eval("bookingPassId") %>'></asp:Label>
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
                                    src="../images/Check-out.png" alt="image"
                                    OnClick="gvbtnCheckout_Click" OnClientClick="showLoader();" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:GridView
                    ID="gvCheckOutPass" runat="server" AllowPaging="True" DataKeyNames="bookingPassId"
                    CssClass="gvv display" AutoGenerateColumns="false" BorderStyle="None"
                    PageSize="25000">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Booking Id">
                            <ItemTemplate>
                                <asp:Label ID="lblgvbookingPassId" runat="server" Text='<%# Eval("bookingPassId") %>'></asp:Label>
                                <asp:Label ID="lblbookingPassId" runat="server" Text='<%#Bind("bookingPassId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleHeaderId" runat="server" Text='<%#Bind("vehicleHeaderId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblinTime" runat="server" Text='<%#Bind("inTime") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleStatus" runat="server" Text='<%#Bind("vehicleStatus") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblvehicleType" runat="server" Text='<%#Bind("vehicleType") %> ' Visible="false"></asp:Label>
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

                        <asp:TemplateField HeaderText="Check Out" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:ImageButton ID="gvbtnCheckoutPass"
                                    runat="server"
                                    Text="Edit"
                                    src="../images/Check-out.png" alt="image"
                                    OnClick="gvbtnCheckoutPass_Click" OnClientClick="showLoader();" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>

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
                                    <label for="lblmaxcharge" class="labels">
                                        Block & Floor Name
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

                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" class="labels">
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
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divExtendVehicleNo" runat="server">
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
                                    <asp:Label ID="lblVehicleNo" runat="server" Font-Bold="true"></asp:Label>
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
                                    <asp:Label ID="lblPaymentType" runat="server" Font-Bold="true"></asp:Label>

                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" class="labels">
                                        Remaining  Amount <%--<span id="Span2" runat="server"
                                            style="font-size: 11px; color: black; padding-left: 18px">(incl of all taxes)</span>--%>
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
                                    <asp:Label ID="lbltaxAmount" runat="server" Font-Bold="true"></asp:Label>
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
                                    <asp:Label ID="lblTotalAmount" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" class="labels">
                                        Extended   Amount <%-- <span id="Span1" runat="server"
                                            style="font-size: 11px; color: black; padding-left: 16px">(incl of all taxes)</span>--%>
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
                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right" style="font-size: 25px">
                                    ₹&nbsp; 
                                    <asp:Label ID="lblTopayAmount" runat="server" Font-Bold="true" Font-Size="25px"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6 ">
                            <div class="extrasummary2" id="divExtraFeeFeatures" runat="server" visible="false">
                                <div class="row" style="margin-right: 5px; margin-left: 5px; padding: 2px;">

                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12" style="padding-left: 0px;">
                                        <asp:Label ID="label4" runat="server"
                                            Style="color: #000000; font-weight: bolder;">Add On Services</asp:Label>
                                    </div>
                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12" style="margin-left: -16px;">
                                        <asp:Label ID="label5" runat="server" Style="color: #000000; font-weight: bolder;">
                                            Count
                                        </asp:Label>

                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                        <asp:Label ID="Label6" runat="server" Text="Amount" Style="color: #000000; margin-right: -29px; font-weight: bolder;"> </asp:Label>
                                    </div>


                                    <asp:DataList ID="Extrafee" runat="server" Width="100%">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">

                                                    <asp:Label ID="lblfeevehicleAccessoriesName"
                                                        runat="server"
                                                        Text=' <%# Eval("extraFeesDetails") %>'> </asp:Label>

                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">

                                                    <asp:Label ID="lblfeeCount"
                                                        runat="server"
                                                        Text='<%# Eval("Count") %>'> </asp:Label>

                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <span runat="server">₹&nbsp;<asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                        Text='<%# Convert.ToDecimal(Eval("extraFee")).ToString("0.00") %>'> </asp:Label>
                                                    </span>
                                                </div>
                                            </div>


                                        </ItemTemplate>

                                    </asp:DataList>
                                </div>
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
                <div id="divRemaining" runat="server" visible="false">
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" class="labels">
                                        Block & Floor Name
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

                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" class="labels">
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
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6" id="divRemainingVehicleNo" runat="server">

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
                                        Remaining  Amount <%--<span id="Span3" runat="server"
                                            style="font-size: 11px; color: black; padding-left: 18px">(incl of all taxes)</span>--%>
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
                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right" style="font-size: 25px">
                                    ₹&nbsp;  
                                    <asp:Label ID="lblReToPay" runat="server" Font-Bold="true" Font-Size="25px"></asp:Label>
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6 ">
                            <div class="extrasummary2" id="divReminingExtra" runat="server" visible="false">
                                <div class="row" style="margin-right: 5px; margin-left: 5px; padding: 2px;">

                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12" style="padding-left: 0px;">
                                        <asp:Label ID="label3" runat="server"
                                            Style="color: #000000; font-weight: bolder;">Add On Services</asp:Label>
                                    </div>
                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12" style="margin-left: -16px;">
                                        <asp:Label ID="label7" runat="server" Style="color: #000000; font-weight: bolder;">
                                            Count
                                        </asp:Label>

                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                        <asp:Label ID="Label8" runat="server" Text="Amount" Style="color: #000000; margin-right: -29px; font-weight: bolder;"> </asp:Label>
                                    </div>

                                    <asp:DataList ID="dtlExtrafee" runat="server" Width="100%">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">

                                                    <asp:Label ID="lblfeevehicleAccessoriesName"
                                                        runat="server"
                                                        Text=' <%# Eval("extraFeesDetails") %>'> </asp:Label>

                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">

                                                    <asp:Label ID="lblfeeCount"
                                                        runat="server"
                                                        Text='<%# Eval("Count") %>'> </asp:Label>

                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <span runat="server">₹&nbsp;<asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                        Text='<%# Convert.ToDecimal(Eval("extraFee")).ToString("0.00") %>'> </asp:Label>
                                                    </span>
                                                </div>
                                            </div>


                                        </ItemTemplate>

                                    </asp:DataList>
                                </div>
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
                <div id="divPassDetails" runat="server" visible="false">
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6">
                            <div class="row" style="padding: 3px; margin-left: 10px;">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                                    <label for="lblmaxcharge" class="labels">
                                        Block & Floor Name
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

                    <hr style="margin-bottom: 1rem;" />
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
                                    <asp:Label ID="lblPaymentTypePass" runat="server" Font-Bold="true"></asp:Label>

                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-6 extrasummary2">
                            <div id="divExtraFeatursSummary1" runat="server" visible="true">
                                <div class="row" style="margin-right: 5px; margin-left: 5px; padding: 2px;">

                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12" style="padding-left: 0px;">
                                        <asp:Label ID="label1" runat="server"
                                            Style="color: #000000; font-weight: bolder;">Add On Services</asp:Label>
                                    </div>
                                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12" style="margin-left: -16px;">
                                        <asp:Label ID="label12" runat="server" Style="color: #000000; font-weight: bolder;">
                                            Count
                                        </asp:Label>

                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                        <asp:Label ID="Label2" runat="server" Text="Amount" Style="color: #000000; margin-right: -29px; font-weight: bolder;"> </asp:Label>
                                    </div>

                                    <asp:DataList ID="dtlExtraFeeSummary" runat="server" Width="100%">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">

                                                    <asp:Label ID="lblfeevehicleAccessoriesName"
                                                        runat="server"
                                                        Text=' <%# Eval("extraFeesDetails") %>'> </asp:Label>

                                                </div>
                                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">

                                                    <asp:Label ID="lblfeeCount"
                                                        runat="server"
                                                        Text='<%# Eval("Count") %>'> </asp:Label>

                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
                                                    <span runat="server">₹&nbsp;<asp:Label ID="lblgvfeeTotalAmount" runat="server"
                                                        Text='<%# Convert.ToDecimal(Eval("extraFee")).ToString("0.00") %>'> </asp:Label>
                                                    </span>
                                                </div>
                                            </div>


                                        </ItemTemplate>

                                    </asp:DataList>
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
                                    <label for="lblmaxcharge" class="labels">
                                        To Pay 
                                    </label>
                                </div>
                                <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                                    :
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12 text-right">
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
                                        ControlToValidate="ddlPayment"
                                        CssClass="rfvClr"
                                        ErrorMessage="Select Payment Mode" ValidationGroup="Check" InitialValue="0">
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
    </div>
    <script>
        function show() {
            document.getElementById('<%=modal.ClientID %>').style.display = "inline";

        }
        function hide() {
            document.getElementById('<%=modal.ClientID %>').style.display = "none";

        }
        window.onload = function () {
            document.getElementById('<%=txtBookingId.ClientID %>').focus();
        };
    </script>
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
    </script>
    <script type="text/javascript">
        function HideLabelout() {
            var seconds = 5;

            setTimeout(function () {
                document.getElementById("<%=lblout.ClientID %>").style.display = "none";
                document.getElementById("<%=lblout.ClientID %>").innerText = "";
                var BookingId = document.getElementById('<%=txtBookingId.ClientID %>');
                BookingId.focus();
            }, seconds * 2000);
        };
    </script>


    <script type="text/javascript">
        function AlreadyLabelout() {
            var seconds = 5;

            setTimeout(function () {

                document.getElementById("<%=lblAlreadyOut.ClientID %>").style.display = "none";
                document.getElementById("<%=lblAlreadyOut.ClientID %>").innerText = "";
                var BookingId = document.getElementById('<%=txtBookingId.ClientID %>');
                BookingId.focus();
            }, seconds * 2000);
        };
    </script>
    <script type="text/javascript">
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
    <script type="text/javascript">

        window.onload = StartBox();
        function StartBox() {
            var txtStart = document.getElementById('<%=txtBookingId.ClientID %>');
            txtStart.focus();
        }
    </script>


</asp:Content>

