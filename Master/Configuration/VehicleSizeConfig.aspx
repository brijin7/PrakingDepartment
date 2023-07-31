<%@ Page Title="Vehicle Size Master" Language="C#" MasterPageFile="~/PreParking.master"
    AutoEventWireup="true" CodeFile="VehicleSizeConfig.aspx.cs"
    Inherits="Master_Configuration_VehicleSizeConfig" EnableEventValidation="false" %>

<asp:Content ID="frmVehicleConfigMaster" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .imageOverlay {
            background: rgba(245, 245, 245, 0.4);
            height: 20px;
            position: absolute;
            bottom: 0px;
            width: 45%;
            text-align: center;
            color: black;
            font-size: 10px;
            font-weight: 500;
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Configuration"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Vehicle Size Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Vehicle Size <span class="Card-title-second">Master</span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblvehicletype"
                        runat="server"
                        Text="Vehicle Type"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlvehicletype" CssClass="form-control"
                        runat="server" TabIndex="1">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvtype" ValidationGroup="VehicleSizeMaster"
                        InitialValue="0" ControlToValidate="ddlvehicletype" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Vehicle Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblModelName" runat="server" Text="Model Name"
                        CssClass="form-check-label">
                    </asp:Label>
                    <%--  <span
                        data-toggle="tooltip"
                        data-placement="right"
                        data-original-title="i.e. Swift, Innova, Omni etc.,"
                        class="mdi mdi-information-outline tooltipIcon"></span>--%><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtModelName"
                        runat="server"
                        TabIndex="2" MaxLength="150"
                        CssClass="form-control" AutoComplete="Off">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="VehicleSizeMaster"
                        ControlToValidate="txtModelName" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Model Name">
                    </asp:RequiredFieldValidator>
                </div>

            </div>
            <div class="row">

                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblLengthFV"
                        runat="server"
                        CssClass="form-check-label">Vehicle Length <span style="font-size:13px;"> (in mtrs.)</span>
                    </asp:Label>
                  
                    <asp:TextBox ID="txtLength" AutoComplete="off" onkeypress="return isNumber(event);" runat="server" CssClass="form-control" MaxLength="10"
                        TabIndex="3"></asp:TextBox>
                    <%-- <asp:RequiredFieldValidator ID="rfvLength" ControlToValidate="txtLength" ValidationGroup="VehicleSizeMaster"
                        runat="server" CssClass="rfvClr" ErrorMessage="Enter Length">
                    </asp:RequiredFieldValidator>--%>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblHeightFV"
                        runat="server"
                        CssClass="form-check-label">Vehicle Height <span style="font-size:13px;"> (in mtrs.)</span>
                    </asp:Label>
              
                    <asp:TextBox ID="txtHeight"
                        runat="server" AutoComplete="off"
                        CssClass="form-control" MaxLength="10" onkeypress="return isNumber(event);"
                        TabIndex="4"></asp:TextBox>
                    <%--   <asp:RequiredFieldValidator ID="rfvHeight"
                        ControlToValidate="txtHeight" runat="server" CssClass="rfvClr" ValidationGroup="VehicleSizeMaster"
                        ErrorMessage="Enter Height">
                    </asp:RequiredFieldValidator>--%>
                </div>
            </div>

            <div class="row p-4 justify-content-end">
                <div class="mr-3">

                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit"
                        ValidationGroup="VehicleSizeMaster"
                        TabIndex="5" OnClick="btnSubmit_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" />

                </div>
                <div>
                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="cancel"
                        TabIndex="6"
                        OnClick="btnCancel_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Vehicle Size <span class="Card-title-second">Master</span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>

                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12" visible="false">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvVehicleSizeConfig"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="vehicleSizeConfigId"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Size Config Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblvehicleSizeConfigId"
                                        runat="server"
                                        Text='<%#Bind("vehicleSizeConfigId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblvehicleName"
                                        runat="server"
                                        Text='<%#Bind("vehicleConfigIdName") %>'></asp:Label>

                                    <asp:Label
                                        ID="lblvehicleConfigId"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("vehicleConfigId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblmodelName"
                                        runat="server"
                                        Text='<%#Bind("modelName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Length">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblvehiclelength"
                                        runat="server"
                                        Text='<%#Bind("length") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Height">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblvehicleheight"
                                        runat="server"
                                        Text='<%#Bind("height") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="LnkEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="LnkEdit_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="45px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="lnkActiveOrInactive"
                                        runat="server"
                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'
                                        OnClick="lnkActiveOrInactive_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="55px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

