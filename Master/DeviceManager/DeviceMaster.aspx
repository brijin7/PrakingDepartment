<%@ Page Title="" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="DeviceMaster.aspx.cs" Inherits="Master_DeviceManager_DeviceMaster" %>

<asp:Content ID="CtnDeviceMaster" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .btnSubmit-container {
            margin-top: 6.4rem;
            text-align: right;
        }
    </style>
    <div class="row PageRoutePos">
        <div>
            <asp:Label ID="lblMainPage" CssClass="pageRoutecol" runat="server" Text="Home"></asp:Label>
        </div>
        <div>
            <span class="iconify" data-icon="mdi:slash-forward"></span>
        </div>
        <div>
            <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Master Setup"></asp:Label>
        </div>
        <div>
            <span class="iconify" data-icon="mdi:slash-forward"></span>
        </div>
        <div>
            <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Device Master"></asp:Label>
        </div>
        <%--        <div>
            <span class="iconify" data-icon="mdi:slash-forward"></span>
        </div>
        <div>
            <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="RFID  Registeration"></asp:Label>
        </div>--%>
    </div>
    <div id="divForm" runat="server">
        <div class="divTitle row pt-4 pl-4 justify-content-between">
            <div>
                <h4 class="card-title">Device  <span class="Card-title-second">Manager </span></h4>
            </div>
        </div>

        <div class="row pt-4 pl-4">
            <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                <asp:Label
                    ID="lblParking"
                    runat="server"
                    Text="Parking"
                    CssClass="form-check-label">
                </asp:Label>
                <span class="spanStar">*</span>
                <asp:DropDownList
                    ID="ddlParking"
                    runat="server"
                    CssClass="form-control"
                    AutoComplete="off"
                    OnSelectedIndexChanged="DdlParking_SelectedIndexChanged"
                    AutoPostBack="true"
                    TabIndex="1">
                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator
                    ID="rfdDdlParking"
                    ValidationGroup="DeviceManager"
                    ControlToValidate="ddlParking"
                    runat="server" CssClass="rfvClr"
                    InitialValue="0"
                    ErrorMessage="Select a parking">
                </asp:RequiredFieldValidator>
            </div>
            <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                <asp:Label
                    ID="lblBranch"
                    runat="server"
                    Text="Branch"
                    CssClass="form-check-label">
                </asp:Label>
                <span class="spanStar">*</span>
                <asp:DropDownList
                    ID="ddlBranch"
                    runat="server"
                    CssClass="form-control"
                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                    AutoPostBack="true"
                    AutoComplete="off"
                    TabIndex="2">
                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator
                    ID="rfvDdlBranch"
                    ValidationGroup="DeviceManager"
                    ControlToValidate="ddlBranch"
                    runat="server"
                    CssClass="rfvClr"
                    InitialValue="0"
                    ErrorMessage="Select a branch">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="FormGrid">
            <div class="col-12">
                <asp:GridView
                    ID="gvAddIPAndMAC"
                    runat="server"
                    AllowPaging="false"
                    CssClass="gvv display"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="IP Address">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvIPAddress"
                                    runat="server"
                                    Text='<%#Bind("Ipaddress") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="MAC Address">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvMACAddress"
                                    runat="server"
                                    Text='<%#Bind("Macaddress") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Device Name" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="gvTxtDeviceDescription"
                                    CssClass="form-control"
                                    Text='<%#Bind("DeviceType") %>'
                                    runat="server">
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Machine Id" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="gvTxtMachineId"
                                    CssClass="form-control"
                                    Text='<%#Bind("MachineId") %>'
                                    runat="server">
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="In Or Out" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:DropDownList
                                    ID="gvDdlInOut"
                                    CssClass="form-control"
                                    runat="server">
                                    <asp:ListItem Value="In" Text="In"></asp:ListItem>
                                    <asp:ListItem Value="Out" Text="Out"></asp:ListItem>
                                    <asp:ListItem Value="Both" Text="Both"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="gvBtnCheckBox"
                                    runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="row p-4 justify-content-end btnSubmit-container">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        ValidationGroup="DeviceManager"
                        OnClick="btnSubmit_Click"
                        class="pure-material-button-contained btnBgColorAdd"
                        Text="Submit" />
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
    </div>

    <div id="divGridView" runat="server">
        <div class="divTitle row pt-4 pl-4 justify-content-between">
            <div>
                <h4 class="card-title">Device <span class="Card-title-second">Manager </span></h4>
            </div>
            <div>
                <asp:LinkButton ID="LnkAddNew" runat="server" OnClick="LnkAddNew_Click"
                    CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
            </div>
        </div>

        <div runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
            <div class="table-responsive section">
                <asp:GridView
                    ID="gvShowIPAndMAC"
                    runat="server"
                    AutoGenerateColumns="false"
                    DataKeyNames="UniqueId,ActiveStatus,OwnerId,BranchId,MachineId"
                    CssClass="gvv display">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch Name">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvbranchName"
                                    runat="server"
                                    Text='<%#Bind("branchName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Parking Owner Name">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvIParkingOwnerName"
                                    runat="server"
                                    Text='<%#Bind("ParkingOwnerName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IP Address">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvIPAddress"
                                    runat="server"
                                    Text='<%#Bind("Ipaddress") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MAC Address">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvMACAddress"
                                    runat="server"
                                    Text='<%#Bind("Macaddress") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Machine Id" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvMachineId"
                                    runat="server"
                                    Text='<%#Bind("MachineId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Device Type">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvDeviceType"
                                    runat="server"
                                    Text='<%#Bind("DeviceType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Device Direction">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvDeviceDirection"
                                    runat="server"
                                    Text='<%#Bind("DeviceDirection") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:ImageButton
                                    ID="LnkEdit"
                                    runat="server"
                                    src="../../images/edit-icon.png" alt="image"
                                    Text="Edit"
                                    OnClick="LnkEdit_Click"
                                    Visible='<%#Eval("ActiveStatus").ToString() =="A"?true:false%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Or Inactive" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkActiveOrInactive"
                                    runat="server"
                                    OnClick="lnkActiveOrInactive_Click"
                                    CssClass='<%#Eval("ActiveStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                    Text='<%#Eval("ActiveStatus").ToString() =="A"?"Active":"Inactive"%>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

