<%@ Page Title="Cancellation Rules" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="CancellationRules.aspx.cs" Inherits="Master_CancellationRules" EnableEventValidation="false" %>

<asp:Content ID="FrmCancellation" ContentPlaceHolderID="MasterPage" runat="Server">
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Cancellation Rules"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Cancellation <span class="Card-title-second">Rules</span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblCancellationType" runat="server" Text="Cancellation Type"
                        CssClass="form-check-label"></asp:Label>
                    <span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddltype"
                        runat="server"
                        CssClass="form-control"
                        TabIndex="1"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Day" Value="D"></asp:ListItem>
                        <asp:ListItem Text="Minutes" Value="M"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvType" runat="server" CssClass="rfvClr"
                        ControlToValidate="ddltype" InitialValue="0"
                        ValidationGroup="vgCancellationRules" ErrorMessage="Select Cancellation Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblDuration" runat="server" Text="Duration"
                        CssClass="form-check-label"></asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" AutoComplete="off"
                        onkeypress="return isNumber(event);"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                        ValidationGroup="vgCancellationRules"
                        ControlToValidate="txtDuration" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Duration">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblNoOfTimes" runat="server" Text="No. Of Times Per User"
                        CssClass="form-check-label"></asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtperuser" runat="server" CssClass="form-control" MaxLength="2" AutoComplete="off"
                        onkeypress="return isNumber(event);"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        ValidationGroup="vgCancellationRules"
                        ControlToValidate="txtperuser" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter No. Of Times">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblCancellationCharges" runat="server"
                        Text="Cancellation Charges"
                        CssClass="form-check-label"></asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtcancellationCharges" runat="server" AutoComplete="off"
                        CssClass="form-control" onkeypress="return AllowOnlyAmountAndDot(this.id);" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                        ValidationGroup="vgCancellationRules"
                        ControlToValidate="txtcancellationCharges" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Cancellation Charges">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit"
                        ValidationGroup="vgCancellationRules"
                        OnClick="btnSubmit_Click" CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>

        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Cancellation  <span class="Card-title-second">Rules </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%-- <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="add +"
                        CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvCancellationRules"
                        runat="server"
                        Visible="true"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        DataKeyNames="uniqueId"
                        CssClass="gvv display">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Unique Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvUniqueId"
                                        runat="server"
                                        Text='<%#Bind("uniqueId") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvType"
                                        runat="server"
                                        Text='<%#Eval("type").ToString() =="D" ? "Day" :"Minutes" %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Duration">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvDuration"
                                        runat="server"
                                        Text='<%#Bind("duration") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    No. Of Times
                                     <span style="font-size: 13px;">Per User </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvNoOfTimes"
                                        runat="server"
                                        Text='<%#Bind("noOfTimesPerUser") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Charges">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvCancellationCharges"
                                        runat="server"
                                        Text='<%#Bind("cancellationCharges") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="imgBtnEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="imgBtnEdit_Click" />
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


</asp:Content>

