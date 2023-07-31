<%@ Page Title="Tax Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="TaxMaster.aspx.cs" Inherits="Master_TaxMaster" %>

<asp:Content ID="FrmTaxmaster" ContentPlaceHolderID="MasterPage" runat="Server">
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
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Configuration"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Tax Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
           <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Tax <span class="Card-title-second">Master </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div>
                        <asp:Label ID="lblservicename" runat="server"
                            CssClass="form-check-label" Text="Service Name"></asp:Label><span class="spanStar">*</span>
                    </div>
                    <asp:DropDownList ID="ddlservicename" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdservicename" ValidationGroup="TaxMaster" InitialValue="0"
                        ControlToValidate="ddlservicename" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Service Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div>
                        <asp:Label ID="lbltax" runat="server" CssClass="form-check-label" Text="Tax Name"></asp:Label>
                        <span class="spanStar">*</span>
                    </div>
                    <asp:TextBox ID="txttax" runat="server" CssClass="form-control" MaxLength="50" AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfdtax" ValidationGroup="TaxMaster" ControlToValidate="txttax" runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Tax Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div>
                        <asp:Label ID="lblTaxDescription" runat="server"
                            CssClass="form-check-label" Text="Tax Description"></asp:Label>
                        <span class="spanStar">*</span>
                    </div>
                    <asp:TextBox ID="txtTaxDescription" runat="server" CssClass="form-control" MaxLength="50" AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfdtaxdes" ValidationGroup="TaxMaster"
                        ControlToValidate="txtTaxDescription" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Tax Description">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div>
                        <asp:Label ID="lblPercentage" runat="server" CssClass="form-check-label"
                            Text="Tax Percentage (%)"></asp:Label>
                        <span class="spanStar">*</span>
                    </div>
                    <asp:TextBox ID="txtPercentage" runat="server" CssClass="form-control" onkeypress = "return AllowOnlyAmountAndDot(this.id);" MaxLength="5"
                         AutoComplete="Off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfdpercentage" ValidationGroup="TaxMaster"
                        ControlToValidate="txtPercentage" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Tax Percentage (%)">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div>
                        <asp:Label ID="lblfrmdate" runat="server" CssClass="form-check-label" Text="Effective From"></asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtfrmDate" runat="server" CssClass="form-control fromDate" AutoComplete="Off" >
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfdfrmdate" ValidationGroup="TaxMaster"
                            ControlToValidate="txtfrmDate" runat="server" CssClass="rfvClr"
                            ErrorMessage="Select Effective From Date">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4" runat="server" Visible="false">
                    <div>
                        <asp:Label ID="lbltodate" runat="server" CssClass="form-check-label" Text="Effective Till" ></asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control toDate" 
                            AutoComplete="Off" >
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfdtxttodate" ValidationGroup="TaxMaster"
                            ControlToValidate="txttodate" runat="server" CssClass="rfvClr"
                            ErrorMessage="Select Effective Till Date">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                        ValidationGroup="TaxMaster" CssClass="pure-material-button-contained btnBgColorAdd" />
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
                    <h4 class="card-title">Tax <span class="Card-title-second">Master </span></h4>
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
                        ID="gvTaxmaster"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="taxId"
                        CssClass="gvv display"
                        BorderStyle="None"
                        AutoGenerateColumns="False"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvserviceName"
                                        runat="server"
                                        Text='<%#Bind("serviceName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Name ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvserviceNameId"
                                        runat="server"
                                        Text='<%#Bind("serviceNameId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtaxName"
                                        runat="server"
                                        Text='<%#Bind("taxName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtaxId"
                                        runat="server"
                                        Text='<%#Bind("taxId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax Description" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtaxDescription"
                                        runat="server"
                                        Text='<%#Bind("taxDescription") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tax (%)">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtaxPercentage"
                                        runat="server"
                                        Text='<%#Bind("taxPercentage") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective From" >
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgveffectiveFrom"
                                        runat="server"
                                        Text='<%#Convert.ToDateTime(Eval("effectiveFrom")).ToString("dd-MM-yyyy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effect Till" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgveffectiveTill"
                                        runat="server"
                                        Text='<%#Bind("effectiveTill") %>'></asp:Label>
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
    <asp:HiddenField ID="hftaxid" runat="server" />
</asp:Content>

