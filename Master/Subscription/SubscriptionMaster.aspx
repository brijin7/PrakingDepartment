<%@ Page Title="Subscription" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="SubscriptionMaster.aspx.cs" Inherits="Master_SubscriptionMaster" %>

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
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Subscription Master"></asp:Label>
            </div>
        </div>
        <div id="divFormFP" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEditFP" runat="server"></span>Subscription <span class="Card-title-second">Master </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblSubscName"
                        runat="server"
                        Text="Subscription Name"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtSubScrName" runat="server" CssClass="form-control section"
                        TabIndex="1" AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" ValidationGroup="SubscriptionMaster"
                        ControlToValidate="txtSubScrName" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Subscription Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblofftype"
                        runat="server"
                        Text="Offer Type" CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:RadioButtonList
                        ID="rdoofftype"
                        runat="server"
                        CssClass="inline-rb"
                        TabIndex="2"
                        RepeatDirection="Horizontal" AutoPostBack="true"
                        OnSelectedIndexChanged="rdoofftype_SelectedIndexChanged">
                        <asp:ListItem Text="Percentage (%)" Value="P"></asp:ListItem>
                        <asp:ListItem Text="Fixed" Value="F"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvOfferType" ValidationGroup="SubscriptionMaster"
                        ControlToValidate="rdoofftype" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Offer Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4" id="divPercentage" runat="server" visible="false">
                    <asp:Label
                        ID="lbloffvalue"
                        runat="server"
                        Text="Offer Value" CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtoffvalueper" runat="server"
                        CssClass="form-control" onkeyup="this.value = minmax(this.value, 0, 100);"
                        onkeypress=" return isNumber(event);"
                        MaxLength="12" TabIndex="3" AutoComplete="Off">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOfferValueper" ValidationGroup="SubscriptionMaster"
                        ControlToValidate="txtoffvalueper" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Offer Value">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4" id="divFixed" runat="server">
                    <asp:Label
                        ID="lbloffervaluefix"
                        runat="server"
                        Text="Offer Value" CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtOffervalueFix" runat="server"
                        CssClass="form-control" onkeypress="return AllowOnlyAmountAndDot(this.id);"
                        MaxLength="12" TabIndex="4" AutoComplete="Off">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvoffervalueFix" ValidationGroup="SubscriptionMaster"
                        ControlToValidate="txtOffervalueFix" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Offer Value">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblValidity"
                        runat="server"
                        Text="Validity"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtValidity" runat="server" CssClass="form-control section"
                        TabIndex="5" onkeypress=" return isNumber(event);" AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvValidity" ValidationGroup="SubscriptionMaster"
                        ControlToValidate="txtValidity" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Validity">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblfromDate"
                        runat="server"
                        Text="Validity From" CssClass="form-check-label">
                    </asp:Label>
                    <asp:TextBox
                        ID="txtFromDate"
                        runat="server"
                        CssClass="form-control fromDate" TabIndex="6" AutoComplete="Off">
                    </asp:TextBox>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblTodate"
                        runat="server"
                        Text="Validity To" CssClass="form-check-label">
                    </asp:Label>
                    <asp:TextBox
                        ID="txtTodate"
                        runat="server"
                        TabIndex="7"
                        CssClass="form-control toDate" AutoComplete="Off">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblParkingLimit"
                        runat="server"
                        Text="Parking Limit"
                        CssClass="form-check-label">
                    </asp:Label>
                    <asp:TextBox ID="txtParkingLimit" runat="server" CssClass="form-control section"
                        TabIndex="8" onkeypress="return isNumber(event);" AutoComplete="Off"></asp:TextBox>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblamountFP"
                        runat="server"
                        Text="Amount"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span style="font-size: 12px">(incl. tax) </span>
                    <span class="spanStar">*</span>
                    <br>
                    <asp:TextBox ID="txtamountFP" runat="server" CssClass="form-control" MaxLength="12"
                        onkeypress="return AllowOnlyAmountAndDot(this.id);" TabIndex="9"
                        AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvamount" ValidationGroup="SubscriptionMaster"
                        ControlToValidate="txtamountFP" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Amount">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lbltaxnameFP"
                        runat="server"
                        Text="Tax"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <br>
                    <asp:DropDownList ID="ddltaxFP" CssClass="form-control"
                        runat="server" TabIndex="10">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvtaxname" ValidationGroup="SubscriptionMaster"
                        InitialValue="0" ControlToValidate="ddltaxFP" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Tax">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div id="divRules" runat="server" class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8">
                    <asp:Label
                        ID="lblrules"
                        runat="server"
                        Text="Rules"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtRules" runat="server" CssClass="form-control section"
                        TabIndex="11" TextMode="MultiLine" AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRules" ValidationGroup="SubscriptionMaster"
                        ControlToValidate="txtRules" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Rules">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                        TabIndex="12" ValidationGroup="SubscriptionMaster"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                        TabIndex="13" CausesValidation="false" CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>
        <div id="divGvFP" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Subscription <span class="Card-title-second">Master </span></h4>
                </div>
                <div class="row mr-0 justify-content-end">
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView ID="gvSubscriptionMaster" runat="server"
                        AllowPaging="True" DataKeyNames="subscriptionId" CssClass="gvv display"
                        BorderStyle="None" AutoGenerateColumns="false" PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subscription Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvsubscriptionName"
                                        runat="server"
                                        Text='<%#Bind("subscriptionName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Offer Value">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferType"
                                        runat="server" Visible="false"
                                        Text='<%#Eval("offerType").ToString() =="P"?"Percentage":"Fixed"%>'></asp:Label>
                                    <asp:Label
                                        ID="lblGvofferValueType"
                                        runat="server"
                                        Text='<%#Eval("offerValue").ToString() + ""+ (Eval("offerType").ToString() == "P" ? "(%)":"(₹)") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvamountFP" runat="server" Text='<%#Bind("amount") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvvalidity" runat="server" Text='<%#Bind("validity") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvofferValue" runat="server" Text='<%#Bind("offerValue") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvrules" runat="server" Text='<%#Bind("rules") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvtax" runat="server" Text='<%#Bind("tax") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvvalidityFrom" runat="server" Text='<%#Bind("validityFrom") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvvalidityTo" runat="server" Text='<%#Bind("validityTo") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvtaxName" runat="server" Text='<%#Bind("taxName") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblgvparkingLimit" runat="server" Text='<%#Bind("parkingLimit") %>' Visible="false"></asp:Label>
                                    <asp:Label
                                        ID="lblgvtaxFP"
                                        runat="server"
                                        Text='<%#Bind("taxId") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span>Total Amount</span><br />
                                    <span style="font-size: 12px">(incl. tax) </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtotalAmountFP"
                                        runat="server"
                                        Text='<%#Bind("totalAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="LnkEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit" OnClick="LnkEdit_Click"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="lnkActiveOrInactive"
                                        runat="server" OnClick="lnkActiveOrInactive_Click"
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
</asp:Content>

