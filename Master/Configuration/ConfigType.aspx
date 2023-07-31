<%@ Page Title="Configuration Type" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ConfigType.aspx.cs" Inherits="Master_ConfigType" %>

<asp:Content ID="frmConfigType" ContentPlaceHolderID="MasterPage" runat="Server">
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
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Configuration Type"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Configuration <span class="Card-title-second">Type </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblConfigType"
                        Text="Type Name"
                        CssClass="form-check-label"
                        runat="server">
                    </asp:Label>
                    <span
                        data-toggle="tooltip"
                        data-placement="right"
                        data-original-title="i.e. Payment, Floor Type etc.,"
                        class="mdi mdi-information-outline tooltipIcon"></span><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtTypeName"
                        runat="server" MaxLength="50"
                        CssClass="form-control"
                        AutoComplete="off"
                        TabIndex="1">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvTypeName"
                        ControlToValidate="txtTypeName"
                        runat="server"
                        ValidationGroup="configtype"
                        ErrorMessage="Enter Type Name"
                        CssClass="rfvClr">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="configtype" CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Configuration <span class="Card-title-second">Type </span></h4>
                </div>
                <div>
                    <%--<asp:Button ID="lnkAddNew" runat="server" OnClick="lnkAddNew_Click" Text="add +" 
                        CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                    <asp:LinkButton ID="lnkAddNew" runat="server" OnClick="lnkAddNew_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvConfigType"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="configTypeId"
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
                            <asp:TemplateField HeaderText="Configuration Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvConfigType"
                                        runat="server"
                                        Text='<%#Bind("typeName") %>'></asp:Label>
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
</asp:Content>

