<%@ Page Title="DashBoard" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="DashBoardDetails.aspx.cs" Inherits="DashBoardNew" %>

<asp:Content ID="FrmDashBoard" ContentPlaceHolderID="MasterPage" runat="Server">
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
            color: #1ca4ff;
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
    <div class="container-fluid containerBg">

        <div class="row" runat="server" visible="false">
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
                <asp:Button ID="btnSubmit" CssClass="pure-material-button-contained btnBgColorAdd mr-2"
                    Text="Submit" ValidationGroup="vdgDashBoard" runat="server" />
            </div>
        </div>

        <div class="divTitle row mt-4 pt-4 pl-4 justify-content-between" style="position: sticky; left: -15px; top: 15px;">
            <div>
                <h4 class="card-title"><span id="Span1" runat="server">
                    <asp:Label ID="lblBrName" runat="server"></asp:Label></span>
                    <asp:Label ID="lblGvCount" runat="server"></asp:Label>
                    <span class="Card-title-second">List </span></h4>
            </div>
            <div>
                <asp:LinkButton ID="linkclose" CssClass="close" Visible="false" runat="server" OnClick="linkclose_Click">
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
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Slot Id" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvslotId"
                                    runat="server"
                                    Text='<%#Bind("slotId") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Type">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvvehicleTypeName"
                                    runat="server"
                                    Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvtotalAmount"
                                    runat="server"
                                    Text='<%#Bind("totalAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
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
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle No." HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvvehicleNumber"
                                    runat="server"
                                    Text='<%#Bind("vehicleNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Type">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvvehicleTypeName"
                                    runat="server"
                                    Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Out Date & Time">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvtoDate"
                                    runat="server"
                                    Text='<%#Bind("outTime") %>'></asp:Label>
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
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Number" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvvehicleNumber"
                                    runat="server"
                                    Text='<%#Bind("vehicleNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Type">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvvehicleTypeName"
                                    runat="server"
                                    Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="In Date & Time">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvtoDate"
                                    runat="server"
                                    Text='<%#Bind("inTime") %>'></asp:Label>
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
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Normal" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvvehicleNumber"
                                    runat="server"
                                    Text='<%#Bind("Normal") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </div>


</asp:Content>

