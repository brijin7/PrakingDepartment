<%@ Page Title="" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="RFID_Registeration.aspx.cs" Inherits="Master_RFID_Registeration_RFID_Registeration" EnableEventValidation="false" %>

<asp:Content ID="frmRFIDregisteration" ContentPlaceHolderID="MasterPage" runat="Server">
    <div class="container-fluid containerBg">
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="RFID Registration"></asp:Label>
            </div>
        </div>

        <div id="divForm" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>RFID <span class="Card-title-second">Registration </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblTagId"
                        Text="User TagId"
                        CssClass="form-check-label"
                        runat="server">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtUserTagId"
                        runat="server" MaxLength="50"
                        CssClass="form-control"
                        AutoComplete="off"
                        TabIndex="2">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvtxtUserTagId"
                        ControlToValidate="txtUserTagId"
                        runat="server"
                        ValidationGroup="RFIDRegisteration"
                        ErrorMessage="Enter a vehicle number."
                        CssClass="rfvClr">
                    </asp:RequiredFieldValidator>
                </div>

                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblVehicleNumber"
                        Text="Vehicle Number"
                        CssClass="form-check-label"
                        runat="server">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtVehicleNumber"
                        runat="server" MaxLength="50"
                        CssClass="form-control"
                        AutoComplete="off"
                        TabIndex="1">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvVehicleNumbe"
                        ControlToValidate="txtVehicleNumber"
                        runat="server"
                        ValidationGroup="RFIDRegisteration"
                        ErrorMessage="Enter a vehicle number."
                        CssClass="rfvClr">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end btnSubmit-container">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        OnClick="btnSubmit_Click"
                        ValidationGroup="RFIDRegisteration"
                        class="pure-material-button-contained btnBgColorAdd"
                        Text="Submit" />
                </div>
                <div>
                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        OnClick="btnCancel_Click"
                        Text="Cancel"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>

        <div id="divGridView" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">RFID <span class="Card-title-second">Registration </span></h4>
                </div>
                <div>
                    <asp:LinkButton
                        ID="LnkAddNew"
                        runat="server"
                        OnClick="LnkAddNew_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd"
                        Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                </div>
            </div>

            <div runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvRFIDRegisterartion"
                        runat="server"
                        AutoGenerateColumns="false"
                        DataKeyNames="RegistrtionId,vehicleNumber,UserId,Activestatus"
                        CssClass="gvv display">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="User TagId">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvUserId"
                                        runat="server"
                                        Text='<%#Bind("UserId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Vehicle Number">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvVehicleNumber"
                                        runat="server"
                                        Text='<%#Bind("vehicleNumber") %>'></asp:Label>
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
    </div>
</asp:Content>

