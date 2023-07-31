<%@ Page Title="Pass Configuration" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ParkingPassConfiguration.aspx.cs" Inherits="Master_ParkingPassConfiguration" %>

<asp:Content ID="frmParkingPass" ContentPlaceHolderID="MasterPage" runat="Server">
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
                    Text="Pass Configuration"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Pass <span class="Card-title-second">Configuration </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblpassCategory"
                        runat="server"
                        Text="Pass Category">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlpassCategory"
                        runat="server"
                        TabIndex="1"
                        CssClass="form-control ">                    
                        <asp:ListItem Value="N">Normal</asp:ListItem>
                        <asp:ListItem Value="V">VIP</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="RfvpassCategory"
                        ValidationGroup="ParkingPassMaster"
                        ControlToValidate="ddlpassCategory"
                        runat="server"
                        CssClass="rfvClr"
                        InitialValue="0"
                        ErrorMessage="Select Pass Category">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblpasstype"
                        runat="server"
                        Text="Pass Type">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlpasstype"
                        runat="server"
                        TabIndex="2"
                        CssClass="form-control">
                        <asp:ListItem Value="0">Select</asp:ListItem>
                        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                        <asp:ListItem Value="Daily">Daily</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfdpasstype"
                        ValidationGroup="ParkingPassMaster"
                        ControlToValidate="ddlpasstype"
                        runat="server"
                        CssClass="rfvClr"
                        InitialValue="0"
                        ErrorMessage="Select Pass Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblvehicleType"
                        runat="server"
                        Text="Vehicle Type">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlvehicleType"
                        runat="server"
                        TabIndex="5"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfdvehicleType"
                        ValidationGroup="ParkingPassMaster"
                        InitialValue="0"
                        ControlToValidate="ddlvehicleType"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Select Vehicle Type">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblnoofdays"
                        runat="server"
                        Text="No. of Days">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtnoofdays"
                        runat="server"
                        TabIndex="3" MaxLength="5" AutoComplete="off"
                        onkeypress="return isNumber(event);"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdnoofdays"
                        ValidationGroup="ParkingPassMaster"
                        ControlToValidate="txtnoofdays"
                        runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter No. Of Days">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblparkingLimit"
                        runat="server"
                        Text="Parking Limit">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtparkingLimit"
                        runat="server" AutoComplete="off"
                        TabIndex="4" onkeypress="return isNumber(event);" MaxLength="6"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdparkinglimit"
                        ValidationGroup="ParkingPassMaster"
                        ControlToValidate="txtparkingLimit"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Parking Limit">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblTotalAmount"
                        runat="server"
                        Text="Total Amount">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtTotalAmount" AutoComplete="off"
                        TabIndex="6" MaxLength="12"
                        runat="server" onkeypress="return AllowOnlyAmountAndDot(this.id);"
                        AutoPostBack="true" OnTextChanged="txtTotalAmount_TextChanged"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdamount"
                        ValidationGroup="ParkingPassMaster"
                        ControlToValidate="txtTotalAmount"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Total Amount">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lbltax"
                        runat="server"
                        Text="Tax">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddltax"
                        runat="server"
                        TabIndex="7"
                        CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddltax_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfdtax"
                        ValidationGroup="ParkingPassMaster"
                        InitialValue="0"
                        ControlToValidate="ddltax"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Select Tax">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblAmount"
                        runat="server"
                        CssClass="form-check-label">Amount <span style="font-size:13px;"> (Excl.Tax)</span>
                    </asp:Label>
                    <asp:TextBox ID="txtAmount" AutoComplete="off"
                        runat="server" Enabled="false" CssClass="form-control" MaxLength="12"
                        onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:TextBox>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label
                        ID="lblTaxamountFF"
                        runat="server"
                        CssClass="form-check-label">Tax Amount 
                    </asp:Label>
                    <asp:TextBox ID="txtTaxAmount" AutoComplete="off"
                        runat="server" Enabled="false" CssClass="form-control" MaxLength="12"
                        onkeypress="return AllowOnlyAmountAndDot(this.id);"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-9 col-md-9 col-lg-9 col-xl-9">
                    <asp:Label
                        ID="lblremarks"
                        runat="server"
                        Text="Remarks">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtremarks"
                        runat="server"
                        TabIndex="8" AutoComplete="off"
                        TextMode="MultiLine"
                        CssClass="form-control ">
                    </asp:TextBox>
                     <asp:RequiredFieldValidator
                        ID="rfvRemarks"
                        ValidationGroup="ParkingPassMaster"
                        ControlToValidate="txtremarks"
                        runat="server"
                        CssClass="rfvClr"                      
                        ErrorMessage="Enter Remarks">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit" OnClick="btnSubmit_Click"
                        ValidationGroup="ParkingPassMaster"
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
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Pass <span class="Card-title-second">Configuration </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-xs-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvparkingpass"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="parkingPassConfigId"
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
                                        ID="lblgvparkingOwnerId"
                                        runat="server"
                                        Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbranchId"
                                        runat="server"
                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvpassCategory"
                                        runat="server"
                                        Text='<%#Eval("passCategory").ToString() =="V"?"VIP":"Normal"%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pass">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvpassType"
                                        runat="server"
                                        Text='<%#Bind("passType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleTypeName"
                                        runat="server"
                                        Text='<%#Bind("vehicleTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. Of Days" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvnoOfDays"
                                        runat="server"
                                        Text='<%#Bind("noOfDays") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parking Limit" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvparkingLimit"
                                        runat="server"
                                        Text='<%#Bind("parkingLimit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvamount"
                                        runat="server"
                                        Text='<%#Bind("amount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Amount" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtax"
                                        runat="server"
                                        Text='<%#Bind("tax") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="tax Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtaxId"
                                        runat="server"
                                        Text='<%#Bind("taxId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Total Amount<br />
                                    <span style="font-size: 12px;">(incl. tax) </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtotalAmount"
                                        runat="server"
                                        Text='<%#Bind("totalAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="vehicleType" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvvehicleType"
                                        runat="server"
                                        Text='<%#Bind("vehicleType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtaxName"
                                        runat="server"
                                        Text='<%#Bind("taxName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvremarks"
                                        runat="server"
                                        Text='<%#Bind("remarks") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="parkingPassConfigId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvparkingPassConfigId"
                                        runat="server"
                                        Text='<%#Bind("parkingPassConfigId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="LnkEdit"
                                        runat="server"
                                        ImageUrl="~/images/edit-icon.png"
                                        ToolTip="Edit"
                                        OnClick="LnkEdit_Click"
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
    <asp:HiddenField ID="hfparkingPassConfigId" runat="server" />
</asp:Content>

