<%@ Page Title="Slot Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ParkingSlotWeb.aspx.cs" Inherits="Master_ParkingSlotWeb" %>

<asp:Content ID="frmParkingSlot" ContentPlaceHolderID="MasterPage" runat="Server">

    <style>
        .table-responsive {
            overflow-y: auto;
            max-height: 505px;
            margin-bottom: 1em;
        }

        /*.tdsactive {
            height: 56px;
            width: 36px !important;
            padding: 10px 3px 10px 3px;
            margin: 3px 5px 3px 5px;
            border-radius: 10%;
            display: inline-grid;
            font-size: 12px;
            font-weight: bold;
        }*/
        .tdsactive {
            height: 39px;
            width: 35px !important;
            padding: 5px 3px 5px 3px;
            margin: 0px 0px 3px 4px;
            border-radius: 10%;
            display: inline-grid;
            font-size: 10px;
            font-weight: bold;
        }

            .tdsactive:hover {
                transform: scale(1.05);
            }

        .dot {
            height: 15px;
            width: 15px;
            border-radius: 35%;
            display: inline-block;
        }

        .DotText {
            color: #008eef;
            font-size: 13px;
            margin: 5px 5px 5px 5px;
        }

        .chargepinimage {
            Height: 60px !important;
            Width: auto;
        }

        .pincard {
            box-shadow: 0 6px 8px 0 rgb(0 0 0 / 38%);
            transition: 0.3s;
            width: 130px;
            height: 100px;
            background-color: var(--white);
            padding-left: 50px;
            padding-top: 10px;
            border-radius: 1.25rem;
        }

            .pincard:hover {
                box-shadow: 10px 8px 16px 0 rgba(0,0,0,0.2);
            }

        .pincardIn {
            box-shadow: 0 6px 8px 0 rgba(0, 0, 0, 0.38);
            transition: 0.3s;
            width: 258px;
            height: 112px;
            background-color: var(--white);
            padding-left: 16px;
        }
    </style>

    <div class="container-fluid containerBg">
        <div class="row PageRoutePos">
            <div>
                <asp:Label ID="lblMainPage" CssClass="pageRoutecol" runat="server" Text="Home"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Master"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol"
                    Text="Slot Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAdd" runat="server"></span>Slot <span class="Card-title-second">Master</span></h4>
                </div>

            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblblock"
                        runat="server"
                        Text="Block">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlblock"
                        runat="server" AutoPostBack="true"
                        TabIndex="1" OnSelectedIndexChanged="ddlblock_SelectedIndexChanged"
                        CssClass="form-control  ">
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
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblfloor"
                        runat="server"
                        Text="Floor">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlfloor"
                        runat="server"
                        TabIndex="2"
                        CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlfloor_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfvfloor"
                        ValidationGroup="ParkingSlot"
                        ControlToValidate="ddlfloor"
                        runat="server"
                        CssClass="rfvClr"
                        InitialValue="0"
                        ErrorMessage="Select Floor">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblvehicletype"
                        runat="server"
                        Text="Vehicle Type">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlvehicletype"
                        runat="server"
                        TabIndex="3"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfvvehicletype"
                        ValidationGroup="ParkingSlot"
                        ControlToValidate="ddlvehicletype"
                        runat="server"
                        CssClass="rfvClr"
                        InitialValue="0"
                        ErrorMessage="Select Vehicle Type">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblparkingtype"
                        runat="server"
                        Text="Slot Inclination Type">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlparkingtype"
                        runat="server"
                        TabIndex="4"
                        CssClass="form-control 
                        ">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfvparkingtype"
                        ValidationGroup="ParkingSlot"
                        ControlToValidate="ddlparkingtype"
                        runat="server"
                        CssClass="rfvClr"
                        InitialValue="0"
                        ErrorMessage="Select Slot Inclination Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblrow"
                        runat="server"
                        Text="No. Of Rows">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtrow"
                        runat="server" AutoComplete="off"
                        TabIndex="8"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvrow"
                        ValidationGroup="ParkingSlot"
                        ControlToValidate="txtrow"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter No. Of Rows">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblcolumn"
                        runat="server"
                        Text="No. Of Columns">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtcolumn"
                        runat="server" AutoComplete="off"
                        TabIndex="9"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvcolumn"
                        ValidationGroup="ParkingSlot"
                        ControlToValidate="txtcolumn"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter No. Of Columns">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit" OnClick="btnSubmit_Click"
                        ValidationGroup="ParkingSlot"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="Cancel"
                        OnClick="btnCancel_Click"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>
        <div id="divSlotUpdate" runat="server" visible="false">
            <div id="divslotback" class="row justify-content-between" runat="server" style="transform: translateY(-40%);">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Slot <span class="Card-title-second">Master </span></h4>
                </div>
                <div>
                    <div id="divSlotlistdetails" runat="server" visible="true" style="display: inline-flex;">
                        <span style="color: #1ca4ff; font-weight: bold;">
                            <asp:Label ID="lblBlockName" runat="server"></asp:Label>
                        </span>
                        <span style="color: #050505; font-weight: bold;">&nbsp; - &nbsp;</span>
                        <span style="color: #1ca4ff; font-weight: bold;">
                            <asp:Label ID="lblFloorName" runat="server"></asp:Label></span>
                        <span style="color: #050505; font-weight: bold;">&nbsp; - &nbsp;</span>
                        <span style="color: #1ca4ff; font-weight: bold;">
                            <asp:Label ID="lblvehicleName" runat="server"></asp:Label></span>
                    </div>
                </div>
                <div>
                    <asp:Button
                        ID="btnSingleUpdateSlot"
                        runat="server"
                        Text="Change Slot No."
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorAdd" OnClick="btnSingleUpdateSlot_Click" />
                    <asp:Button ID="btnslotback" runat="server" OnClick="btnslotback_Click" Text="Back"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
            <div class="row" style="margin-top: -16px;">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblSlotStatus"
                        runat="server"
                        Text="Slot Type" CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlSlotStatus"
                        runat="server"
                        TabIndex="2"
                        CssClass="form-control ">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="Rfvslotstatus"
                        ValidationGroup="SlotUpdate"
                        ControlToValidate="ddlSlotStatus"
                        runat="server"
                        CssClass="rfvClr"
                        InitialValue="0"
                        ErrorMessage="Select Slot Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lbllaneno"
                        runat="server"
                        Text="Lane No.">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtlaneno"
                        TabIndex="5"
                        runat="server"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdamount"
                        ValidationGroup="SlotUpdate"
                        ControlToValidate="txtlaneno"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Lane No.">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="mt-4 col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Button
                        ID="btnSlotUpdate"
                        runat="server"
                        Text="Update"
                        ValidationGroup="SlotUpdate"
                        CssClass="pure-material-button-contained btnBgColorAdd" OnClick="btnSlotUpdate_Click" />
                </div>
            </div>
            <div class="row">

                <span class="spanStar ml-3" style="font-size: 13px;">**Note: Slot Type for 1st Row should be "Lane Top" & for 1st Column should be "Lane No"</span>
            </div>
            <div class="row p-2 justify-content-center">
                <div>
                    <span id="spanA" runat="server" class="dot" style="background: #40F44336"></span>
                    <span class="DotText">Normal </span>
                    <span id="spanV" runat="server" class="dot" style="background: #ffee2162"></span>
                    <span class="DotText">VIP </span>
                    <span id="spanD" runat="server" class="dot" style="background: red"></span>
                    <span class="DotText">DeActive </span>
                    <span id="spanP" runat="server" class="dot" style="background: #747474"></span>
                    <span class="DotText">Path,PathV</span>
                    <span id="spanR" runat="server" class="dot" style="background: #DFF6FF"></span>
                    <span class="DotText">Reserved,VIP Reserved</span>
                    <span id="span1" runat="server" class="dot" style="background: #cebebe"></span>
                    <span class="DotText">Way </span>
                    <span id="span2" runat="server" class="dot" style="background: #00d4ff"></span>
                    <span class="DotText">In </span>
                    <span id="span3" runat="server" class="dot" style="background: #1ba2bd"></span>
                    <span class="DotText">Out </span>
                    <span id="span4" runat="server" class="dot" style="background: #24222238"></span>
                    <span class="DotText">Lane No,Lane Top</span>
                </div>
            </div>
        </div>
        <div id="divslot" runat="server" visible="false">
            <div class="row">
                <div id="dydiv" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 align-self-center table-responsive  section"
                    runat="server" align="center">
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    <asp:Button ID="btnLoadButton" runat="server" Text="Button" Visible="false"></asp:Button>
                    <asp:Label ID="lbl" runat="server" ForeColor="Red" Font-Size="Larger"></asp:Label>

                </div>
            </div>

        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Slot <span class="Card-title-second">Master </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvparkingslot"
                        runat="server"
                        AllowPaging="false"
                        DataKeyNames="parkingLotLineId"
                        CssClass="gvv display"
                        BorderStyle="None"
                        AutoGenerateColumns="false"
                        PageSize="2500">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parking LotLineId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvparkingLotLineId"
                                        runat="server"
                                        Text='<%#Bind("parkingLotLineId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbranchId"
                                        runat="server"
                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblgvblockId"
                                        runat="server"
                                        Text='<%#Bind("blockId") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblgvfloorId"
                                        runat="server"
                                        Text='<%#Bind("floorId") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblgvparkingOwnerId"
                                        runat="server"
                                        Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblgvnoOfRows"
                                        runat="server"
                                        Text='<%#Bind("noOfRows") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblgvnoOfColumns"
                                        runat="server"
                                        Text='<%#Bind("noOfColumns") %>'></asp:Label>
                                    <%--  <asp:Label
                                        ID="lblpassageLeftAvailable"
                                        runat="server"
                                        Text='<%#Bind("passageLeftAvailable") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblpassageRightAvailable"
                                        runat="server"
                                        Text='<%#Bind("passageRightAvailable") %>'></asp:Label>--%>
                                    <asp:Label
                                        ID="lbltypeOfVehicle"
                                        runat="server"
                                        Text='<%#Bind("typeOfVehicle") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Block Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvblockName"
                                        runat="server"
                                        Text='<%#Bind("blockName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Floor Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvNameofFloor"
                                        runat="server"
                                        Text='<%#Bind("NameofFloor") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parking Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvparkingName"
                                        runat="server"
                                        Text='<%#Bind("parkingName") %>'></asp:Label>
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

                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="LnkSlotEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png"
                                        OnClick="LnkSlotEdit_Click" OnClientClick="showLoader();" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="lnkActiveOrInactive"
                                        runat="server"
                                        src="../../images/delete-icon.png" alt="image"
                                        OnClick="lnkActiveOrInactive_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfslotcheck" runat="server" Value="0" />

</asp:Content>

