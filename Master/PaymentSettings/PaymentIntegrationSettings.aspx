<%@ Page Title="Payment Integration Settings" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="PaymentIntegrationSettings.aspx.cs" Inherits="Master_PaymentSettings_PaymentIntegrationSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterPage" runat="Server">
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Payment Integration"></asp:Label>
            </div>

        </div>

        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Payment Integration <span class="Card-title-second">Master </span></h4>
                </div>
            </div>
            <div class="row">

                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Name</asp:Label>
                    <asp:TextBox ID="txttName" TabIndex="1" AutoComplete="off" runat="server" TextMode="MultiLine" CssClass="form-control" MaxLength="100"></asp:TextBox>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Phone No.</asp:Label>
                    <asp:TextBox ID="txtphoneNumber" runat="server" AutoComplete="off"
                        onkeypress="return isNumber(event);" MaxLength="10" CssClass="form-control " TabIndex="2"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txtphoneNumber" ErrorMessage="Invalid Phone No."
                        ValidationExpression="[0-9]{10}" CssClass="rfvClr" ValidationGroup="PaymentMaster"></asp:RegularExpressionValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        UPI Id</asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtUPIId" runat="server" AutoComplete="off"
                        MaxLength="30" CssClass="form-control " TabIndex="3"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="PaymentMaster"
                        ControlToValidate="txtUPIId" runat="server"
                        CssClass="rfvClr" ErrorMessage="Enter UPI Id">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Merchant Id</asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtmerchantId" runat="server" AutoComplete="off"
                        CssClass="form-control " MaxLength="50" TabIndex="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="PaymentMaster"
                        ControlToValidate="txtmerchantId" runat="server"
                        CssClass="rfvClr" ErrorMessage="Enter Merchant Id">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Merchant Code</asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtmerchantCode" runat="server" AutoComplete="off"
                        CssClass="form-control " MaxLength="50" TabIndex="5"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="PaymentMaster"
                        ControlToValidate="txtmerchantCode"
                        runat="server" CssClass="rfvClr" ErrorMessage="Enter Merchant Code"> </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Mode</asp:Label>
                    <asp:TextBox ID="txtMode" runat="server" AutoComplete="off"
                        CssClass="form-control " MaxLength="50" TabIndex="6"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Org Id</asp:Label>
                    <asp:TextBox ID="txtOrgid" runat="server" AutoComplete="off"
                        CssClass="form-control " MaxLength="50" TabIndex="7"></asp:TextBox>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Sign</asp:Label>
                    <asp:TextBox ID="txtSign" runat="server" AutoComplete="off"
                        CssClass="form-control " MaxLength="50" TabIndex="8"></asp:TextBox>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Url</asp:Label>
                    <asp:TextBox ID="txtUrl" runat="server" AutoComplete="off"
                        CssClass="form-control " MaxLength="50" TabIndex="9"></asp:TextBox>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" TabIndex="10" Text="Submit" OnClick="btnSubmit_Click"
                        ValidationGroup="PaymentMaster"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" TabIndex="11"
                        Text="Cancel" OnClick="btnCancel_Click" CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Payment Integration <span class="Card-title-second">Master </span></h4>
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
                        ID="gvPaymentDetails"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="paymentUPIDetailsId"
                        CssClass="gvv display"
                        BorderStyle="None"
                        PageSize="25000"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbranchId"
                                        runat="server"
                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvName"
                                        runat="server"
                                        Text='<%#Bind("name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone No.">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvphoneNumber"
                                        runat="server"
                                        Text='<%#Bind("phoneNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UPI Id">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvUPIId"
                                        runat="server"
                                        Text='<%#Bind("UPIId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Merchant Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvmerchantId"
                                        runat="server"
                                        Text='<%#Bind("merchantId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Merchant Code" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvmerchantCode"
                                        runat="server"
                                        Text='<%#Bind("merchantCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mode" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvmode"
                                        runat="server"
                                        Text='<%#Bind("mode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="orgId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvorgId"
                                        runat="server"
                                        Text='<%#Bind("orgId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="sign" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvsign"
                                        runat="server"
                                        Text='<%#Bind("sign") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="url" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvurl"
                                        runat="server"
                                        Text='<%#Bind("url") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="LnkEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="LnkEdit_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="lnkActiveOrInactive"
                                        runat="server"
                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'
                                        OnClick="lnkActiveOrInactive_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfpaymentUPIDetailsId" runat="server" />
</asp:Content>

