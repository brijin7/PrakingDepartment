<%@ Page Title="Vehicle Setup" Language="C#" MasterPageFile="~/PreParking.master"
    AutoEventWireup="true" CodeFile="FloorSetup.aspx.cs" Inherits="Master_Floor_FloorSetup" %>

<asp:Content ID="FrmFloorSetup" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .btnfloorVehicle {
            border-color: #ccffe7;
            border-width: 3px;
            border-style: groove;
            /*box-shadow: rgba(17, 17, 26, 0.1) 0px 4px 16px, rgba(17, 17, 26, 0.1) 0px 8px 24px, rgba(17, 17, 26, 0.1) 0px 16px 56px;*/
        }

        .btnfloorPrice {
            border-color: #85ffff;
            border-width: 3px;
            border-style: groove;
            /*box-shadow: rgba(17, 17, 26, 0.1) 0px 4px 16px, rgba(17, 17, 26, 0.1) 0px 8px 24px, rgba(17, 17, 26, 0.1) 0px 16px 56px;*/
        }

        .lblHead {
            font-size: 1.4rem;
            font-weight: 900;
        }

        .lblHeadSub {
            font-size: 1.2rem;
            font-weight: 700;
        }

        .lblHeadSubHead {
            font-size: 1.4rem;
            font-weight: 900;
        }

        .DivdashedBox {
            border-color: #0ab9ff;
            border-width: 1px;
            border-style: dashed;
            padding: 5px;
            border-radius: 5px;
        }

        .DivBox {
            border-color: #0ab9ff;
            border-width: 1px;
            border-style: solid;
            padding: 5px;
            border-radius: 5px;
        }
        .DivNormalBox {
            border-color: #00d304d1;
            border-width: 1px;
            border-style: solid;
            padding: 5px;
            border-radius: 5px;
        }

        .DivVipBox {
            border-color: #e7d822;
            border-width: 1px;
            border-style: solid;
            padding: 5px;
            border-radius: 5px;
        }

        .chkUpdtPrice {
            color: #ff5e00;
            font-weight: 900;
            padding: 2px;
            margin-left: 16px;
        }

        label {
            display: inline-block;
            margin-bottom: 0.5rem;
            margin-left: 5px;
        }

        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 25px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 18px;
                width: 18px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .CustomGrid {
            margin-bottom: 1px;
        }

        .gvHead {
            background-color: #b4cfe5;
            font-size: 1.5rem;
            text-align: center;
        }
    </style>
    <div>
        <div class="row PageRoutePos" style="margin-left: 3%;">
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Floor"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Vehicle Setup"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" style="margin-top: -10px;">
            <div class="mb-2">
                <div class="row col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <div class="row col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7 ml-0 mb-2 mr-1" id="divBlockFloor" runat="server">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="display: inline-flex; height: fit-content;">
                            <asp:Label
                                ID="lblBlock"
                                runat="server"
                                Text="Block"
                                CssClass="form-check-label mt-2 mr-1">
                            </asp:Label>
                            <asp:DropDownList
                                ID="ddlBlock" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="ddlBlock_SelectedIndexChanged"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="display: inline-flex; height: fit-content;">
                            <asp:Label
                                ID="lblFloorName"
                                runat="server"
                                Text="Floor "
                                CssClass="form-check-label mt-2 mr-1">
                            </asp:Label>
                            <asp:DropDownList
                                ID="ddlFloorName" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="ddlFloorName_SelectedIndexChanged"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5" id="divAddMasters" runat="server" visible="false" style="margin-left: 10px !important;">
                        <div>
                            <asp:Button
                                ID="btnFloorvehicleMaster"
                                runat="server"
                                Text="Vehicle"
                                TabIndex="6"
                                OnClick="btnFloorvehicleMaster_Click"
                                CausesValidation="false"
                                CssClass="pure-material-button-contained text-black btnfloorVehicle" />
                        </div>
                        <div>
                            <asp:Button
                                ID="btnPriceMaster"
                                runat="server"
                                Text="Price"
                                TabIndex="7"
                                OnClick="btnPriceMaster_Click"
                                CausesValidation="false"
                                CssClass="pure-material-button-contained text-black btnfloorPrice" />
                        </div>

                    </div>
                </div>
                <div id="divFloorSetup" runat="server" visible="false" style="border-radius: 5px;"
                    class="p-4 col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">

                    <div id="divFloorVehicle" runat="server" visible="false">
                        <div id="divFormFV" runat="server" visible="false">

                            <div class="divTitle row pt-4 pl-4 justify-content-between">
                                <div>
                                    <h4 class="card-title"><span id="spAddorEditFV" runat="server"></span>Vehicle <span class="Card-title-second">Master </span></h4>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                    <asp:Label
                                        ID="lblvehicletypeFV"
                                        runat="server"
                                        Text="Vehicle Type"
                                        CssClass="form-check-label">
                                    </asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:DropDownList ID="ddlvehicletypeFV" CssClass="form-control"
                                        runat="server" TabIndex="1" OnSelectedIndexChanged="ddlvehicletypeFV_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvtype" ValidationGroup="FloorVehicleMaster"
                                        InitialValue="0" ControlToValidate="ddlvehicletypeFV" runat="server" CssClass="rfvClr"
                                        ErrorMessage="Select Vehicle Type">
                                    </asp:RequiredFieldValidator>
                                </div>
                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                    <asp:Label
                                        ID="lblCapacityFV"
                                        runat="server"
                                        Text=" Capacity"
                                        CssClass="form-check-label">
                                    </asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:TextBox ID="txtCapacityFV" AutoComplete="off" runat="server" CssClass="form-control" onkeypress="return isNumber(event);" MaxLength="10"
                                        TabIndex="2"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCapacity" ValidationGroup="FloorVehicleMaster"
                                        ControlToValidate="txtCapacityFV" runat="server" CssClass="rfvClr"
                                        ErrorMessage="Enter Capacity">
                                    </asp:RequiredFieldValidator>
                                </div>
                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                    <asp:Label
                                        ID="lblvehicleSize"
                                        runat="server"
                                        Text="Vehicle Size"
                                        CssClass="form-check-label">
                                    </asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:DropDownList ID="ddlvehicleSizeFV" CssClass="form-control"
                                        runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ValidationGroup="FloorVehicleMaster"
                                        InitialValue="0" ControlToValidate="ddlvehicleSizeFV" runat="server" CssClass="rfvClr"
                                        ErrorMessage="Select Vehicle Size">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-8">
                                    <asp:Label
                                        ID="lblRulesFV"
                                        runat="server"
                                        Text=" Rules / Instructions"
                                        CssClass="form-check-label">
                                    </asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:TextBox ID="txtRulesFV" runat="server" CssClass="form-control section" AutoComplete="off"
                                        TabIndex="5" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRules" ControlToValidate="txtRulesFV" ValidationGroup="FloorVehicleMaster"
                                        runat="server" CssClass="rfvClr" ErrorMessage="Enter Rules">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row p-4 justify-content-end">
                                <div class="mr-3">
                                    <asp:Button ID="btnSubmitFV" runat="server" Text="Submit" OnClick="btnSubmitFV_Click"
                                        ValidationGroup="FloorVehicleMaster"
                                        TabIndex="6" CssClass="pure-material-button-contained btnBgColorAdd" />
                                </div>
                                <div>
                                    <asp:Button ID="btnCancelFV" runat="server" Text="Cancel" OnClick="btnCancelFV_Click"
                                        TabIndex="7" CausesValidation="false" CssClass="pure-material-button-contained btnBgColorCancel" />
                                </div>
                            </div>

                        </div>

                        <div id="divGvFV" runat="server">
                            <div class="divTitle row pt-4 pl-4 justify-content-between">
                                <div>
                                    <h4 class="card-title">Vehicle <span class="Card-title-second">Master </span></h4>
                                </div>
                                <div class="row mr-0 mt-2 justify-content-end">
                                    <asp:LinkButton ID="btnAddFV" runat="server" OnClick="btnAddFV_Click"
                                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                                             <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                                </div>
                            </div>
                            <div id="divGridViewFV" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                <div class="table-responsive section">
                                    <asp:GridView
                                        ID="gvfloorvehiclemaster"
                                        runat="server"
                                        DataKeyNames="floorVehicleId"
                                        CssClass="gvv display"
                                        BorderStyle="None"
                                        AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvfloorVehicleIdFV"
                                                        runat="server"
                                                        Text='<%#Bind("floorVehicleId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Branch Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvbranchNameFV"
                                                        runat="server"
                                                        Text='<%#Bind("branchName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Floor ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvfloorIdFV"
                                                        runat="server"
                                                        Text='<%#Bind("floorId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Floor" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvfloorNameFV"
                                                        runat="server"
                                                        Text='<%#Bind("floorName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="VehicleType Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvvehicleTypeFV"
                                                        runat="server"
                                                        Text='<%#Bind("vehicleType") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle Type">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvvehicleNameFV"
                                                        runat="server"
                                                        Text='<%#Bind("vehicleName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Capacity">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvcapacityFV"
                                                        runat="server"
                                                        Text='<%#Bind("capacity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Length">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvvehicleSize"
                                                        runat="server" Visible="false"
                                                        Text='<%#Bind("vehicleSizeConfigId") %>'></asp:Label>
                                                    <asp:Label
                                                        ID="lblgvvehiclelength"
                                                        runat="server"
                                                        Text='<%#Bind("lengthValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Height">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvvehicleheight"
                                                        runat="server"
                                                        Text='<%#Bind("heightValue") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Rules" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvrulesFV"
                                                        runat="server"
                                                        Text='<%#Bind("rules") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:ImageButton
                                                        ID="LnkEditFV"
                                                        runat="server"
                                                        ImageUrl="~/images/edit-icon.png"
                                                        ToolTip="Edit"
                                                        OnClick="LnkEditFV_Click"
                                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:LinkButton
                                                        ID="lnkActiveOrInactiveFV"
                                                        runat="server" OnClick="lnkActiveOrInactiveFV_Click"
                                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="divFloorPrice" runat="server">
                        <div id="divFormFP" runat="server" visible="false">
                            <div class="divTitle row pt-3 pl-4 mb-3 justify-content-between">
                                <div>
                                    <h4 class="card-title"><span id="spAddorEditFP"
                                        runat="server"></span>Price <span
                                            class="Card-title-second">Master </span></h4>
                                </div>
                            </div>

                            <div class="row" style="margin-top: -25px;">
                                <asp:Label ID="lblvcvalueFP" runat="server" Visible="false"></asp:Label>
                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                    <asp:Label
                                        ID="lblvcFP"
                                        runat="server"
                                        Text="Type"
                                        CssClass="lblHead">
                                    </asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:RadioButtonList
                                        ID="rbtnTypeFP"
                                        runat="server"
                                        CssClass="inline-rb"
                                        TabIndex="1"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="rbtnTypeFP_SelectedIndexChanged"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Vehicle" Value="V"></asp:ListItem>
                                        <asp:ListItem Text="Add-On Service" Value="A"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="rfvOfferTypePeriod" ValidationGroup="PriceMaster"
                                        ControlToValidate="rbtnTypeFP" runat="server" CssClass="rfvClr"
                                        ErrorMessage="Select Type">
                                    </asp:RequiredFieldValidator>
                                </div>
                                <div class="row col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" id="divddlVehicle" runat="server" visible="false">
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label
                                            ID="lblVehicleFP"
                                            runat="server"
                                            Text="Vehicle"
                                            CssClass="lblHead">
                                        </asp:Label>
                                        <span class="spanStar">*</span>
                                        <asp:DropDownList ID="ddlVehicleFP" CssClass="form-control"
                                            runat="server"
                                            TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvVehicle" ValidationGroup="PriceMaster"
                                            InitialValue="0" ControlToValidate="ddlVehicleFP" runat="server" CssClass="rfvClr"
                                            ErrorMessage="Select Vehicle">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label ID="lblGraceTime" runat="server" CssClass="lblHead">Grace Time <span style="font-size:9px;"> (In Min.)</span> </asp:Label>
                                        <asp:TextBox ID="txtGraceTime" runat="server" CssClass="form-control" MaxLength="12" AutoComplete="off"
                                            onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="4"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="row col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" id="divddlaccessoriesType" runat="server" visible="false">
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label
                                            ID="lblaccessoriestype"
                                            runat="server"
                                            Text="Service Category"
                                            CssClass="lblHead">
                                        </asp:Label>
                                        <span class="spanStar">*</span>
                                        <asp:DropDownList ID="ddlAccessoriesType" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAccessoriesType_SelectedIndexChanged" CssClass="form-control"
                                            runat="server" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ValidationGroup="PriceMaster"
                                            InitialValue="0" ControlToValidate="ddlAccessoriesType" runat="server" CssClass="rfvClr"
                                            ErrorMessage=" Select Service Category">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label
                                            ID="lblAccessoriesFP"
                                            runat="server"
                                            Text="Add-On Service"
                                            CssClass="lblHead">
                                        </asp:Label>
                                        <span class="spanStar">*</span>
                                        <asp:DropDownList ID="ddlaccessoriesFP" CssClass="form-control"
                                            runat="server" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvaccessories" ValidationGroup="PriceMaster"
                                            InitialValue="0" ControlToValidate="ddlaccessoriesFP" runat="server" CssClass="rfvClr"
                                            ErrorMessage=" Select Add-On Service" Style="float: left;">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-top: -22px">
                                <div class="row col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8 ml-1 mb-2">
                                    <div class="col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5">
                                        <asp:Label
                                            ID="lbltaxnameFP"
                                            runat="server"
                                            Text="Tax"
                                            CssClass="lblHead">
                                        </asp:Label>
                                        <span class="spanStar">*</span>
                                        <asp:DropDownList ID="ddltaxFP" CssClass="form-control" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddltaxFP_SelectedIndexChanged" TabIndex="4">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvtaxname" ValidationGroup="PriceMaster"
                                            InitialValue="0" ControlToValidate="ddltaxFP" runat="server" CssClass="rfvClr"
                                            ErrorMessage="Select Tax">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="margin-top: 25px"
                                        id="divtimetype" runat="server" visible="false">
                                        <div class="row">
                                            <asp:Label ID="lblVIP" runat="server" CssClass="chkUpdtPrice">Update Price
                                            <label class="switch">
                                                <asp:CheckBox ID="ChkUpdtPrice" Checked="false" AutoPostBack="true"
                                                    OnCheckedChanged="ChkUpdtPrice_CheckedChanged" runat="server" />
                                                <span class="slider round"></span>
                                            </label>
                                            </asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="DivFPVNDetails" class="row" runat="server" visible="false">
                                <div class="col-12 col-xs-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                    <div runat="server" id="DivPriceDetails" class="DivdashedBox">
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label
                                                    ID="lbluserMode"
                                                    runat="server"
                                                    CssClass="lblHeadSub"
                                                    Text="User Type">
                                                </asp:Label><span class="spanStar">*</span>
                                                <asp:DropDownList
                                                    ID="ddluserMode"
                                                    runat="server"
                                                    TabIndex="10"
                                                    CssClass="form-control ">
                                                    <asp:ListItem Value="N">Normal</asp:ListItem>
                                                    <asp:ListItem Value="V">VIP</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label
                                                    ID="lblFromFP"
                                                    runat="server"
                                                    CssClass="lblHeadSub"
                                                    Text="From Hour">
                                                </asp:Label><span class="spanStar">*</span>
                                                <asp:TextBox
                                                    ID="txtFromFP"
                                                    runat="server" MaxLength="2" AutoComplete="off"
                                                    TabIndex="11" onkeypress="return isNumber(event);"
                                                    CssClass="form-control ">                                                
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator
                                                    ID="rfvddlFromFP"
                                                    ValidationGroup="PriceMasterAdd"
                                                    ControlToValidate="txtFromFP"
                                                    runat="server"
                                                    CssClass="rfvClr"
                                                    ErrorMessage="Enter From Hour">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label
                                                    ID="lblToFP"
                                                    runat="server"
                                                    CssClass="lblHeadSub"
                                                    Text="To Hour">
                                                </asp:Label><span class="spanStar">*</span>
                                                <asp:TextBox
                                                    ID="txtToFP" MaxLength="2" AutoComplete="off"
                                                    runat="server" onkeypress="return  isNumber(event);"
                                                    TabIndex="12"
                                                    CssClass="form-control ">                                                
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator
                                                    ID="rfvtxtToFP"
                                                    ValidationGroup="PriceMasterAdd"
                                                    ControlToValidate="txtToFP"
                                                    runat="server"
                                                    CssClass="rfvClr"
                                                    ErrorMessage="Enter To Hour">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label
                                                    ID="lbltotalamountFP"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total Amount
                                                </asp:Label>
                                                <span class="spanStar">*</span>
                                                <asp:TextBox ID="txttotalamountFP" AutoComplete="off"
                                                    OnTextChanged="txttotalamountFP_TextChanged"
                                                    runat="server" CssClass="form-control" MaxLength="12" AutoPostBack="true"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="13"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvamount" ValidationGroup="PriceMasterAdd"
                                                    ControlToValidate="txttotalamountFP" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 mt-3">
                                                <asp:Label
                                                    ID="lblAmountFP"
                                                    runat="server"
                                                    CssClass="lblHeadSub" ForeColor="#1ca4ff">Amount <span style="font-size:8px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtAmountFP" runat="server" CssClass="lblHeadSub"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblTaxFp"
                                                    runat="server"
                                                    CssClass="lblHeadSub" ForeColor="#1ca4ff">Tax Amount
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtTaxFP" runat="server" CssClass="lblHeadSub"></asp:Label>

                                            </div>
                                            <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 mt-4" style="text-align: center;">
                                                <asp:Button ID="BtnAddPrice" runat="server" Text="Add Price" OnClick="BtnAddPrice_Click"
                                                    TabIndex="14" ValidationGroup="PriceMasterAdd"
                                                    CssClass="pure-material-button-contained btnBgColorAdd" />
                                            </div>
                                        </div>
                                        <div style="display: flex;">
                                            <div id="divNormalFP" runat="server" class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 DivNormalBox mr-1" visible="false">
                                                <div class="text-center">
                                                    <asp:Label CssClass="lblHead" runat="server">Normal</asp:Label>
                                                </div>
                                                <hr style="margin-bottom: 2px !important; margin-right: 2px; margin-top: 0px; border-top: 1px solid #2dda30 !important;" />
                                                <div class="section" style="max-height: 185px !important; overflow-y: auto;">
                                                    <asp:GridView ID="NrmlPriceDetails" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                                                        AutoGenerateColumns="False" DataKeyNames="slabId">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sno" Visible="false">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                    <asp:Label ID="lblgvslabId" runat="server" Visible="false"
                                                                        Text=' <%# Eval("slabId") %>'></asp:Label>
                                                                    <asp:Label ID="lblgvpriceId" runat="server" Visible="false"
                                                                        Text=' <%# Eval("priceId") %>'></asp:Label>
                                                                    <asp:Label ID="lblgvAmount" runat="server" Visible="false"
                                                                        Text=' <%# Eval("amount") %>'></asp:Label>
                                                                    <asp:Label ID="lblgvtax" runat="server" Visible="false"
                                                                        Text=' <%# Eval("tax") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="User Mode" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUserMode"
                                                                        runat="server"
                                                                        Text=' <%# Eval("userMode") %>'> </asp:Label>
                                                                    <%--  <asp:Label ID="lblgvUserModetxt"
                                                                    runat="server"
                                                                    Text=' <%# Eval("userModetxt") %>'> </asp:Label>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="From">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvFrom"
                                                                        runat="server"
                                                                        Text='<%# Eval("From") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="To">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTo"
                                                                        runat="server"
                                                                        Text='<%# Eval("To") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Total Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTotalAmount" runat="server"
                                                                        Text='<%# Convert.ToDecimal(Eval("totalAmount")).ToString("0.0") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="NrmlImgBtnDelete" runat="server" ImageUrl="~/images/Close.svg" Width="20px" ToolTip="Delete"
                                                                        OnClick="NrmlImgBtnDelete_Click" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grdHead" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton
                                                                        ID="NrmllnkEditFP"
                                                                        runat="server"
                                                                        ImageUrl="~/images/edit-icon.png"
                                                                        ToolTip="Edit"
                                                                        OnClick="NrmllnkEditFP_Click"
                                                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton
                                                                        ID="NrmllnkActiveOrInactiveFP"
                                                                        runat="server" OnClick="NrmllnkActiveOrInactiveFP_Click"
                                                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="gvHead" />
                                                        <AlternatingRowStyle CssClass="gvRow" />
                                                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div id="divVipFP" runat="server" class="col-12 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 DivVipBox" visible="false">
                                                <div class="text-center">
                                                    <asp:Label CssClass="lblHead" runat="server">VIP</asp:Label>
                                                </div>
                                                <hr style="margin-bottom: 2px !important; margin-right: 2px; margin-top: 0px; border-top: 1px solid #e7d822 !important;" />
                                                <div class="section" style="max-height: 185px !important; overflow-y: auto;">
                                                    <asp:GridView ID="VipPriceDetails" runat="server" CssClass="CustomGrid table table-bordered table-condenced"
                                                        AutoGenerateColumns="False" DataKeyNames="slabId">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sno" Visible="false">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                    <asp:Label ID="lblgvslabId" runat="server" Visible="false"
                                                                        Text=' <%# Eval("slabId") %>'></asp:Label>
                                                                    <asp:Label ID="lblgvpriceId" runat="server" Visible="false"
                                                                        Text=' <%# Eval("priceId") %>'></asp:Label>
                                                                    <asp:Label ID="lblgvAmount" runat="server" Visible="false"
                                                                        Text=' <%# Eval("amount") %>'></asp:Label>
                                                                    <asp:Label ID="lblgvtax" runat="server" Visible="false"
                                                                        Text=' <%# Eval("tax") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="User Mode" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvUserMode" Visible="false"
                                                                        runat="server"
                                                                        Text=' <%# Eval("userMode") %>'> </asp:Label>
                                                                    <%-- <asp:Label ID="lblgvUserModetxt"
                                                                    runat="server"
                                                                    Text=' <%# Eval("userModetxt") %>'> </asp:Label>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="From">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvFrom"
                                                                        runat="server"
                                                                        Text='<%# Eval("From") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="To">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTo"
                                                                        runat="server"
                                                                        Text='<%# Eval("To") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Total Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvTotalAmount" runat="server"
                                                                        Text='<%# Convert.ToDecimal(Eval("totalAmount")).ToString("0.0") %>'> </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="VipImgBtnDelete" runat="server" ImageUrl="~/images/Close.svg" Width="20px" ToolTip="Delete"
                                                                        OnClick="VipImgBtnDelete_Click" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grdHead" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Edit" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton
                                                                        ID="ViplnkEditFP"
                                                                        runat="server"
                                                                        ImageUrl="~/images/edit-icon.png"
                                                                        ToolTip="Edit"
                                                                        OnClick="ViplnkEditFP_Click"
                                                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Delete" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton
                                                                        ID="ViplnkActiveOrInactiveFP"
                                                                        runat="server" OnClick="ViplnkActiveOrInactiveFP_Click"
                                                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="gvHead" />
                                                        <AlternatingRowStyle CssClass="gvRow" />
                                                        <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="DivFPVNAccDetails" class="row text-center" runat="server" visible="false">
                                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <div runat="server" id="DivNormalAcc" class="DivBox">
                                        <div class="mb-3 text-center">
                                            <asp:Label CssClass="lblHead" runat="server">Add-On Service </asp:Label>
                                        </div>
                                        <hr style="margin-bottom: 2px !important; border-top: 1px solid rgb(60 209 209) !important;" />
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNAcctotalamt"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>
                                                <asp:TextBox ID="txtNAcctotalamt"
                                                    runat="server" OnTextChanged="txtNAcctotalamt_TextChanged"
                                                    CssClass="form-control"
                                                    MaxLength="12"
                                                    AutoPostBack="true" AutoComplete="off"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtNAcctotalamt" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 mt-3">
                                                <asp:Label
                                                    ID="lblNAccAmt"
                                                    runat="server"
                                                    CssClass="lblHeadSub" ForeColor="#1ca4ff">Amount <span style="font-size:8px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtNAccAmt" runat="server" CssClass="lblHeadSub"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblNAcctaxamt"
                                                    runat="server"
                                                    CssClass="lblHeadSub" ForeColor="#1ca4ff" Style="margin-right: 16px;">Tax Amount
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtNAcctaxamt" runat="server" CssClass="lblHeadSub"></asp:Label>

                                            </div>
                                        </div>
                                        <%--  <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNAccAmt"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>

                                                <asp:TextBox ID="txtNAccAmt"
                                                    runat="server" Enabled="false" CssClass="form-control" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:TextBox>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNAcctaxamt"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                <asp:TextBox ID="txtNAcctaxamt"
                                                    runat="server" Enabled="false" CssClass="form-control" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:TextBox>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>

                            <div class="row p-4 justify-content-end" style="padding-bottom: 0px !important">
                                <div class="mr-3">
                                    <asp:Button ID="btnSubmitFP" runat="server" Text="Submit" OnClick="btnSubmitFP_Click"
                                        TabIndex="8" ValidationGroup="PriceMaster"
                                        CssClass="pure-material-button-contained btnBgColorAdd" />
                                </div>
                                <div>
                                    <asp:Button ID="btnCancelFP" runat="server" Text="Cancel" OnClick="btnCancelFP_Click"
                                        TabIndex="9" CssClass="pure-material-button-contained btnBgColorCancel" />
                                </div>
                            </div>
                        </div>

                        <div id="divGvFP" runat="server">
                            <div class="divTitle row pt-4 pl-4 justify-content-between">
                                <div>
                                    <h4 class="card-title">Price <span class="Card-title-second">Master </span></h4>
                                </div>
                                <div class="row mr-0 mt-2 justify-content-end">
                                    <asp:LinkButton ID="btnAddFP" runat="server" OnClick="btnAddFP_Click"
                                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                                </div>
                            </div>
                            <div class="row mb-5" style="margin-top: -20px;">
                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                    <asp:RadioButtonList ID="rbtnGVTypeFP" CssClass="rbtnList" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rbtnGVTypeFP_SelectedIndexChanged"
                                        runat="server" AutoPostBack="true">
                                        <asp:ListItem Value="V">Vehicle</asp:ListItem>
                                        <asp:ListItem Value="A">Add-On Service</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>

                            </div>

                            <div id="divVehicleGVFP" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 ">
                                <div class="table-responsive section">
                                    <asp:GridView
                                        ID="gvPricemaster"
                                        runat="server"
                                        AllowPaging="True"
                                        DataKeyNames="floorId"
                                        CssClass="gvv display"
                                        BorderStyle="None"
                                        AutoGenerateColumns="false" OnRowDataBound="gvPricemaster_RowDataBound"
                                        PageSize="100">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id Type" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvidTypeFPV"
                                                        runat="server"
                                                        Text='<%#Bind("idType") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Parking Owner" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvparkingOwnerIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvbranchIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Floor ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvfloorIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("floorId") %>'></asp:Label>
                                                    <asp:Label
                                                        ID="lblgvfloorNameFP"
                                                        runat="server"
                                                        Text='<%#Bind("floorName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="tax ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvtaxIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("taxId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvvehicleAccessoriesFP"
                                                        runat="server" Visible="false"
                                                        Text='<%#Bind("vehicleaccessories") %>'></asp:Label>
                                                    <%--vehicleaccessoriesID--%>
                                                    <asp:Label
                                                        ID="lblgvvehicleAccessoriesNameFP"
                                                        runat="server"
                                                        Text='<%#Bind("vehicleAccessoriesName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Grace Time" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvgraceTimeFPV"
                                                        runat="server"
                                                        Text='<%#Bind("graceTime") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Price Details " HeaderStyle-CssClass="gvHeader" Visible="false">
                                                <ItemTemplate>
                                                    <asp:DataList runat="server" ID="dlVPriceDetails"
                                                        RepeatDirection="Horizontal" OnItemDataBound="dlVPriceDetails_ItemDataBound">

                                                        <ItemTemplate>


                                                            <asp:Label
                                                                ID="lblgvtimeTypeFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("timeType") %>'></asp:Label>
                                                            <%--  <asp:Label
                                                                ID="lblgvtotalAmountFPV"
                                                                runat="server" Font-Size="14px"
                                                                Text='<%#Bind("totalAmount") %>'></asp:Label>
                                                            <asp:Label
                                                                ID="lblgvtaxFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("tax") %>'></asp:Label>
                                                            <asp:Label
                                                                ID="lblgvamountFPV"
                                                                runat="server" Visible="false" Font-Bold="true"
                                                                Text='<%#Bind("amount") %>'></asp:Label>--%>

                                                            <asp:Label
                                                                ID="lblgvactiveStatusFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("activeStatus") %>'></asp:Label>
                                                            <asp:Label
                                                                ID="lblgvpriceIdFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("priceId") %>'></asp:Label>
                                                            <asp:DataList runat="server" ID="dtlNrmlVipSlabDetails"
                                                                RepeatDirection="Horizontal">
                                                                <ItemTemplate>
                                                                    <asp:Label
                                                                        ID="lblgvuserModeFPV"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("userMode") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNslabId"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("slabId") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNpriceId"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("priceId") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNtotalAmount"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("totalAmount") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNamount"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("amount") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNtax"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("tax") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNfromDate"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("from") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNtoDate"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("to") %>'></asp:Label>
                                                                    <asp:Label
                                                                        ID="gVNactiveStatus"
                                                                        runat="server" Visible="false"
                                                                        Text='<%#Bind("activeStatus") %>'></asp:Label>
                                                                </ItemTemplate>

                                                            </asp:DataList>
                                                        </ItemTemplate>
                                                        <ItemStyle BorderColor="#87cefd" BorderStyle="Solid" BorderWidth="1px" />
                                                    </asp:DataList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:ImageButton
                                                        ID="LnkEditFP"
                                                        runat="server"
                                                        ImageUrl="~/images/edit-icon.png"
                                                        ToolTip="Edit"
                                                        OnClick="LnkEditFP_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--                  <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:LinkButton
                                                        ID="lnkActiveOrInactiveFP"
                                                        runat="server" OnClick="lnkActiveOrInactiveFP_Click"
                                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div id="divAccessoriesGVFP" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12" visible="false">
                                <div class="table-responsive section">
                                    <asp:GridView
                                        ID="gvAccessoriesFP"
                                        runat="server"
                                        AllowPaging="True"
                                        DataKeyNames="priceId"
                                        CssClass="gvv display"
                                        BorderStyle="None"
                                        AutoGenerateColumns="false"
                                        PageSize="100">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Parking Owner" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvparkingOwnerIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvbranchIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Floor ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvfloorIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("floorId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Accessiores Type">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvconfigTypeNameFP"
                                                        runat="server"
                                                        Text='<%#Bind("configTypeName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="configTypeId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvconfigTypeId"
                                                        runat="server"
                                                        Text='<%#Bind("configTypeId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Floor" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvfloorNameFP"
                                                        runat="server"
                                                        Text='<%#Bind("floorName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvamountFP"
                                                        runat="server"
                                                        Text='<%#Bind("amount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Amount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvtaxFP"
                                                        runat="server"
                                                        Text='<%#Bind("tax") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle Accessories Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvvehicleAccessoriesFP"
                                                        runat="server"
                                                        Text='<%#Bind("vehicleAccessories") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvvehicleAccessoriesNameFP"
                                                        runat="server"
                                                        Text='<%#Bind("vehicleAccessoriesName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Id Type" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvidTypeFP"
                                                        runat="server"
                                                        Text='<%#Bind("idType") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvtaxIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("taxId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvtaxNameFP"
                                                        runat="server"
                                                        Text='<%#Bind("taxName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Total Amount</span>
                                                    <%--<br /><span style="font-size: 12px">(incl. tax) </span>--%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvtotalAmountFP"
                                                        runat="server"
                                                        Text='<%#Bind("totalAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Price ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvpriceIdFP"
                                                        runat="server"
                                                        Text='<%#Bind("priceId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:ImageButton
                                                        ID="LnkEditFPA"
                                                        runat="server"
                                                        ImageUrl="~/images/edit-icon.png"
                                                        ToolTip="Edit"
                                                        OnClick="LnkEditFPA_Click"
                                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:LinkButton
                                                        ID="lnkActiveOrInactiveFP"
                                                        runat="server" OnClick="AcclnkActiveOrInactiveFP_Click"
                                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hffloorVehicleIdFV" runat="server" />
    <asp:HiddenField ID="hfflooridFV" runat="server" />
    <asp:HiddenField ID="HfpriceId" runat="server" />
</asp:Content>

