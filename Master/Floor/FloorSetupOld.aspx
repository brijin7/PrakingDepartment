<%@ Page Title="Floor Setup" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="FloorSetupOld.aspx.cs" Inherits="Master_Floor_FloorSetup" %>

<asp:Content ID="FrmFloorSetup" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .btnfloorVehicle {
            border-color: #ccffe7;
            border-width: 3px;
            border-style: groove;
            /*box-shadow: rgba(17, 17, 26, 0.1) 0px 4px 16px, rgba(17, 17, 26, 0.1) 0px 8px 24px, rgba(17, 17, 26, 0.1) 0px 16px 56px;*/
        }

        .btnfloorFeature {
            border-color: #d6e5b3;
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

        .DivBox {
            border-color: #0ab9ff;
            border-width: 1px;
            border-style: solid;
            padding: 10px;
            border-radius: 25px;
        }

        .DivVipBox {
            border-color: #fb8a00;
            border-width: 1px;
            border-style: solid;
            padding: 10px;
            border-radius: 25px;
        }

        .chkVIP {
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
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Floor Setup"></asp:Label>
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
                        <div style="display: none">
                            <asp:Button
                                ID="btnFloorFeatures"
                                runat="server"
                                Text="Features"
                                CausesValidation="false"
                                TabIndex="5"
                                OnClick="btnFloorFeatures_Click"
                                CssClass="pure-material-button-contained text-black  btnfloorFeature" />
                        </div>
                    </div>
                </div>
                <div id="divFloorSetup" runat="server" visible="false" style="border-radius: 5px;"
                    class="p-4 col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">

                    <div id="divFloorVehicle" runat="server" visible="false">
                        <div id="divFormFV" runat="server" visible="false">

                            <div class="divTitle row pt-4 pl-4 justify-content-between">
                                <div>
                                    <h4 class="card-title"><span id="spAddorEditFV" runat="server"></span>Floor Vehicle <span class="Card-title-second">Master </span></h4>
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
                                        runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtCapacityFV" runat="server" CssClass="form-control" onkeypress="return isNumber(event);" MaxLength="10"
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
                                    <asp:TextBox ID="txtRulesFV" runat="server" CssClass="form-control section"
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
                                    <h4 class="card-title">Floor Vehicle <span class="Card-title-second">Master </span></h4>
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
                                            <asp:TemplateField HeaderText="Floor Vehicle" Visible="false">
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
                                        <asp:ListItem Text="Accessories" Value="A"></asp:ListItem>
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
                                        <asp:TextBox ID="txtGraceTime" runat="server" CssClass="form-control" MaxLength="12"
                                            onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="4"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="row col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" id="divddlaccessoriesType" runat="server" visible="false">
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label
                                            ID="lblaccessoriestype"
                                            runat="server"
                                            Text="Accessories Type"
                                            CssClass="lblHead">
                                        </asp:Label>
                                        <span class="spanStar">*</span>
                                        <asp:DropDownList ID="ddlAccessoriesType" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAccessoriesType_SelectedIndexChanged" CssClass="form-control"
                                            runat="server" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ValidationGroup="PriceMaster"
                                            InitialValue="0" ControlToValidate="ddlAccessoriesType" runat="server" CssClass="rfvClr"
                                            ErrorMessage=" Select Accessories Type">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label
                                            ID="lblAccessoriesFP"
                                            runat="server"
                                            Text="Accessories"
                                            CssClass="lblHead">
                                        </asp:Label>
                                        <span class="spanStar">*</span>
                                        <asp:DropDownList ID="ddlaccessoriesFP" CssClass="form-control"
                                            runat="server" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvaccessories" ValidationGroup="PriceMaster"
                                            InitialValue="0" ControlToValidate="ddlaccessoriesFP" runat="server" CssClass="rfvClr"
                                            ErrorMessage=" Select Accessories">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-top: -22px">
                                <div class="row col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8 ml-1 mb-2">
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
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
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="margin-top: 25px" id="divtimetype" runat="server">
                                        <%--  <asp:CheckBox ID="chkVIP" Checked="true" AutoPostBack="true" Text="VIP" CssClass="chkVIP"
                                            OnCheckedChanged="chkVIP_CheckedChanged" runat="server" /> --%>
                                        <%--  General Instructions--%>
                                        <div class="row">
                                            <asp:Label ID="lblVIP" runat="server" CssClass="chkVIP">VIP
                                            <label class="switch">
                                                <asp:CheckBox ID="chkVIP" Checked="true" AutoPostBack="true" OnCheckedChanged="chkVIP_CheckedChanged" runat="server" />
                                                <span class="slider round"></span>
                                            </label>
                                            </asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4 justify-content-end mt-4">
                                    <div class="mr-3">
                                        <asp:Button ID="btnSubmitFP" runat="server" Text="Submit" OnClick="btnSubmitFP_Click"
                                            TabIndex="8" ValidationGroup="PriceMaster"
                                            CssClass="pure-material-button-contained btnBgColorAdd" />
                                    </div>
                                    <div class="mr-2">
                                        <asp:Button ID="btnCancelFP" runat="server" Text="Cancel" OnClick="btnCancelFP_Click"
                                            TabIndex="9" CssClass="pure-material-button-contained btnBgColorCancel" />
                                    </div>
                                </div>
                            </div>

                            <div id="DivFPVNDetails" class="row" runat="server" visible="false">
                                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <div runat="server" id="DivNormal" class="DivBox">
                                        <div class="mb-3 text-center">
                                            <asp:Label CssClass="lblHead" runat="server">Normal</asp:Label>
                                        </div>
                                        <hr style="margin-bottom: 2px !important; margin-top: -9px; border-top: 1px solid rgb(60 209 209) !important;" />
                                        <div class="mb-2">
                                            <asp:Label ID="lblNHourly" CssClass="lblHeadSubHead" runat="server">Hourly</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lbltotalamountFP"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>
                                                <asp:TextBox ID="txtNHtotalamountFP"
                                                    OnTextChanged="txtNHtotalamountFP_TextChanged"
                                                    runat="server" CssClass="form-control" MaxLength="12" AutoPostBack="true"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvamount" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtNHtotalamountFP" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblAmountFP"
                                                    runat="server"
                                                    CssClass="lblHeadSub" ForeColor="#1ca4ff">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtNHAmountFP"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblTaxFp"
                                                    runat="server"
                                                    CssClass="lblHeadSub" ForeColor="#1ca4ff">Tax Amount
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtNHTaxFP"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                            </div>
                                        </div>
                                        <div style="margin-top: -10px;">
                                            <asp:Label ID="lblNEHourly" CssClass="lblHeadSubHead" runat="server">Extension Hourly</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblEHtotalAmt"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>

                                                <asp:TextBox ID="txtEHTotalAmount"
                                                    runat="server" CssClass="form-control"
                                                    MaxLength="12" AutoPostBack="true" OnTextChanged="txtEHTotalAmount_TextChanged"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"
                                                    TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtEHTotalAmount" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNEHAmount"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtNEHamount"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblNEHtaxamt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtNEHtaxamt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>
                                            </div>
                                        </div>
                                        <hr style="margin-bottom: 0px !important; margin-top: -1px; border-top: 1px solid rgb(60 209 209) !important;" />
                                        <div>
                                            <asp:Label ID="lblNDaily" CssClass="lblHeadSubHead" runat="server">Daily</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNDTotal"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>
                                                <asp:TextBox ID="txtNDtotal"
                                                    OnTextChanged="txtNHtotalamountFP_TextChanged"
                                                    runat="server" CssClass="form-control" MaxLength="12" AutoPostBack="true"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtNDtotal" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNDAmt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtNDAmt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblNDTax"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtNDTax"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                            </div>
                                        </div>
                                        <div style="margin-top: -10px;">
                                            <asp:Label ID="lblNEDaily" CssClass="lblHeadSubHead" runat="server">Extension Daily</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNEDTotal"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>

                                                <asp:TextBox ID="txtNEDTotal"
                                                    runat="server" CssClass="form-control"
                                                    MaxLength="12" AutoPostBack="true" OnTextChanged="txtEHTotalAmount_TextChanged"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"
                                                    TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtNEDTotal" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblNEDAmt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtNEDAmt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblNEDTax"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtNEDTax"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <div runat="server" id="DivVIP" class="DivVipBox">
                                        <div class="mb-3 text-center">
                                            <asp:Label CssClass="lblHead" runat="server">VIP</asp:Label>
                                        </div>
                                        <hr style="margin-bottom: 2px !important; margin-top: -8px; border-top: 1px solid rgb(60 209 209) !important;" />
                                        <div class="mb-2">
                                            <asp:Label ID="lblVHourly" CssClass="lblHeadSubHead" runat="server">Hourly</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVHtotalAmt"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>

                                                <asp:TextBox ID="txtVHtotalAmt"
                                                    runat="server" CssClass="form-control" AutoPostBack="true"
                                                    MaxLength="12" OnTextChanged="txtVHtotalAmt_TextChanged"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtVHtotalAmt" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVHAmt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtVHAmt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>
                                                <br />
                                                <asp:Label
                                                    ID="lblVHTaxamt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtVHTaxamt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>
                                            </div>
                                        </div>
                                        <div style="margin-top: -10px;">
                                            <asp:Label ID="lblVEHourly" CssClass="lblHeadSubHead" runat="server">Extension Hourly</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVEHtotalamt"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>

                                                <asp:TextBox ID="txtVEHtotalamt"
                                                    runat="server" AutoPostBack="true"
                                                    CssClass="form-control"
                                                    MaxLength="12" OnTextChanged="txtVEHtotalamt_TextChanged"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtVEHtotalamt" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVEHAmt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtVEHAmt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>
                                                <br />
                                                <asp:Label
                                                    ID="lblVEHtaxAmt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtVEHtaxAmt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>
                                            </div>
                                        </div>
                                        <hr style="margin-bottom: 0px !important; margin-top: -1px; border-top: 1px solid rgb(60 209 209) !important;" />
                                        <div>
                                            <asp:Label ID="lblVDaily" CssClass="lblHeadSubHead" runat="server">Daily</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVDTotal"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>
                                                <asp:TextBox ID="txtVDTotal"
                                                    OnTextChanged="txtVHtotalAmt_TextChanged"
                                                    runat="server" CssClass="form-control" MaxLength="12" AutoPostBack="true"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtVDTotal" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVDAmt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtVDAmt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblVDTax"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtVDTax"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                            </div>
                                        </div>
                                        <div style="margin-top: -10px;">
                                            <asp:Label ID="lblVEDaily" CssClass="lblHeadSubHead" runat="server">Extension Daily</asp:Label>
                                        </div>
                                        <div class="row">
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVEDTotal"
                                                    runat="server"
                                                    CssClass="lblHeadSub">Total 
                                                </asp:Label>
                                                <span class="spanStar">*</span>

                                                <asp:TextBox ID="txtVEDTotal"
                                                    runat="server" CssClass="form-control"
                                                    MaxLength="12" AutoPostBack="true" OnTextChanged="txtVEHtotalamt_TextChanged"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"
                                                    TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtVEDTotal" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                                <asp:Label
                                                    ID="lblVEDAmt"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Amount <span style="font-size:9px;"> (Excl.Tax)</span>
                                                </asp:Label>
                                                :

                                                <asp:Label ID="txtVEDAmt"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>

                                                <br />
                                                <asp:Label
                                                    ID="lblVEDTax"
                                                    runat="server" ForeColor="#1ca4ff"
                                                    CssClass="lblHeadSub">Tax Amount
                                                </asp:Label>
                                                :
                                                <asp:Label ID="txtVEDTax"
                                                    runat="server" Enabled="false" CssClass="lblHeadSub" MaxLength="12"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="DivFPVNAccDetails" class="row text-center" runat="server" visible="false">
                                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <div runat="server" id="DivNormalAcc" class="DivBox">
                                        <div class="mb-3 text-center">
                                            <asp:Label CssClass="lblHead" runat="server">Accessories </asp:Label>
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
                                                    AutoPostBack="true"
                                                    onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="3"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="PriceMaster"
                                                    ControlToValidate="txtNAcctotalamt" runat="server" CssClass="rfvClr"
                                                    ErrorMessage="Enter Total Amount">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="row">
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
                                        </div>
                                    </div>
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
                                        <asp:ListItem Value="A">Accessories</asp:ListItem>
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

                                            <asp:TemplateField HeaderText="Price Details " HeaderStyle-CssClass="gvHeader" Visible="true">
                                                <ItemTemplate>
                                                    <asp:DataList runat="server" ID="dlVPriceDetails" RepeatDirection="Horizontal">
                                                       
                                                        <ItemTemplate>
                                                            <asp:Label
                                                                ID="lblUserType"
                                                                runat="server" Font-Size="13px"
                                                                Text='<%#Eval("userMode").ToString() =="N"?"Normal": "VIP" %>'></asp:Label>

                                                            <asp:Label
                                                                ID="lblTimeType"
                                                                runat="server" Font-Size="13px"
                                                                Text='<%#Eval("timeType").ToString() == "D "?"Dly": Eval("timeType").ToString() == "ED"?"Ext.Dly":
                                                                   Eval("timeType").ToString() == "H "?"Hrly": "Ext.Hrly" %>'></asp:Label>
                                                            <asp:Label
                                                                ID="lblgvuserModeFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("userMode") %>'></asp:Label>
                                                            <asp:Label
                                                                ID="lblgvtimeTypeFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("timeType") %>'></asp:Label>
                                                            <asp:Label
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
                                                                Text='<%#Bind("amount") %>'></asp:Label>

                                                            <asp:Label
                                                                ID="lblgvactiveStatusFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("activeStatus") %>'></asp:Label>
                                                            <asp:Label
                                                                ID="lblgvpriceIdFPV"
                                                                runat="server" Visible="false"
                                                                Text='<%#Bind("priceId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle BorderColor="#87cefd" BorderStyle="Solid" BorderWidth="1px"/>
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
                                            <asp:TemplateField HeaderText="User Type" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblgvusermodeFP"
                                                        runat="server"
                                                        Text='<%#Eval("userMode").ToString() =="V"?"VIP":"Normal"%>'></asp:Label>
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

                    <div id="divFloorFeatures" runat="server" visible="false">
                        <div id="divFormFF" runat="server" visible="false">

                            <div class="divTitle row pt-4 pl-4 justify-content-between">
                                <div>
                                    <h4 class="card-title"><span id="spAddorEditFF" runat="server"></span>Floor Features <span class="Card-title-second">Master </span></h4>
                                </div>
                            </div>

                            <div class="row">

                                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                                    <asp:Label ID="lblFeatureNameFF" runat="server" Text="Feature Name" CssClass="form-check-label"></asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:TextBox ID="txtFeaturenameFF" runat="server" CssClass="form-control" TabIndex="3" MaxLength="150"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfdBlockName" ValidationGroup="vdgFloorFeatures" ControlToValidate="txtFeaturenameFF"
                                        runat="server" CssClass="rfvClr" ErrorMessage="Enter Feature Name">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8">
                                    <asp:Label ID="lblDescriptionFF" runat="server" Text="Description" CssClass="form-check-label"></asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:TextBox ID="txtdescriptionFF" runat="server" TextMode="MultiLine" CssClass="form-control" MaxLength="250"
                                        TabIndex="4"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescription" ValidationGroup="vdgFloorFeatures"
                                        ControlToValidate="txtdescriptionFF" runat="server" CssClass="rfvClr" ErrorMessage="Enter Description">
                                    </asp:RequiredFieldValidator>
                                </div>

                            </div>

                            <div class="row">

                                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                    <asp:Label ID="lblAmountFF" runat="server" Text="Total Amount" CssClass="form-check-label"></asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:TextBox ID="txtamountFF" runat="server" CssClass="form-control" TabIndex="5"
                                        MaxLength="10" onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="vdgFloorFeatures"
                                        ControlToValidate="txtamountFF" runat="server" CssClass="rfvClr" ErrorMessage="Enter Total Amount">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                    <asp:Label ID="lblTaxFF" runat="server" Text="Tax" CssClass="form-check-label"></asp:Label>
                                    <span class="spanStar">*</span>
                                    <asp:DropDownList ID="ddltaxFF" CssClass="form-control" TabIndex="6" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddltaxFF_SelectedIndexChanged" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="vdgFloorFeatures" InitialValue="0"
                                        ControlToValidate="ddltaxFF" runat="server" CssClass="rfvClr" ErrorMessage="Select Tax">
                                    </asp:RequiredFieldValidator>
                                </div>

                                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                    <asp:Label
                                        ID="lblTAmountFF"
                                        runat="server"
                                        CssClass="form-check-label">Amount <span style="font-size:13px;"> (Excl.Tax)</span>
                                    </asp:Label>

                                    <asp:TextBox ID="txtTamountFF"
                                        runat="server" Enabled="false" CssClass="form-control" MaxLength="12"
                                        onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:TextBox>
                                </div>

                                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                                    <asp:Label
                                        ID="lblTaxamountFF"
                                        runat="server"
                                        CssClass="form-check-label">Tax Amount 
                                    </asp:Label>

                                    <asp:TextBox ID="txttaxamountFF"
                                        runat="server" Enabled="false" CssClass="form-control" MaxLength="12"
                                        onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row p-4 justify-content-end">
                                <div class="mr-3">
                                    <asp:Button ID="btnSubmitFF" runat="server" Text="Submit" ValidationGroup="vdgFloorFeatures" TabIndex="8"
                                        CssClass="pure-material-button-contained btnBgColorAdd" OnClick="btnSubmitFF_Click" />
                                </div>
                                <div>
                                    <asp:Button ID="btnCancelFF" runat="server" Text="Cancel" OnClick="btnCancelFF_Click" TabIndex="9"
                                        CssClass="pure-material-button-contained btnBgColorCancel" />
                                </div>
                            </div>

                        </div>

                        <div id="divGvFF" runat="server">

                            <div class="divTitle row pt-4 pl-4 justify-content-between">
                                <div>
                                    <h4 class="card-title">Floor Features <span class="Card-title-second">Master </span></h4>
                                </div>
                                <div class="row mr-0 mt-2 justify-content-end">
                                    <asp:LinkButton ID="btnAddFF" runat="server" OnClick="btnAddFF_Click"
                                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                                             <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                                </div>
                            </div>

                            <div id="divGridViewFF" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                <div class="table-responsive section">
                                    <asp:GridView
                                        ID="gvFloorFeatures"
                                        runat="server"
                                        Visible="true"
                                        AutoGenerateColumns="false"
                                        BorderStyle="None"
                                        DataKeyNames="featuresId"
                                        CssClass="gvv display">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Block Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvBlockIdFF"
                                                        runat="server"
                                                        Text='<%#Bind("blockId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Block" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvBlockNameFF"
                                                        runat="server"
                                                        Text='<%#Bind("blockName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Floor Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvFloorIdFF"
                                                        runat="server"
                                                        Text='<%#Bind("floorId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Floor" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvFloorNameFF"
                                                        runat="server"
                                                        Text='<%#Bind("floorName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="features Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvFeaturesIdFF"
                                                        runat="server"
                                                        Text='<%#Bind("featuresId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Feature Name">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvFeatureNameFF"
                                                        runat="server"
                                                        Text='<%#Bind("featureName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Description" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvDescriptionFF"
                                                        runat="server"
                                                        Text='<%#Bind("description") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Amount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvAmountFF"
                                                        runat="server"
                                                        Text='<%#Bind("amount") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tax Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvTaxIdFF"
                                                        runat="server"
                                                        Text='<%#Bind("taxId") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tax Amount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvTaxAmountFF"
                                                        runat="server"
                                                        Text='<%#Bind("tax") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tax Percentage" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvTaxPercentageFF"
                                                        runat="server"
                                                        Text='<%#Bind("taxPercentage") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <span>Total Amount</span><%--<br />--%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label
                                                        ID="lblGvTotalAmountFF"
                                                        runat="server"
                                                        Text='<%#Bind("totalAmount") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:ImageButton
                                                        ID="imgBtnEditFF"
                                                        runat="server"
                                                        ImageUrl="~/images/edit-icon.png"
                                                        ToolTip="Edit"
                                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="imgBtnEditFF_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                                <ItemTemplate>
                                                    <asp:LinkButton
                                                        ID="lnkActiveOrInactiveFF"
                                                        runat="server"
                                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'
                                                        OnClick="lnkActiveOrInactiveFF_Click"></asp:LinkButton>
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

