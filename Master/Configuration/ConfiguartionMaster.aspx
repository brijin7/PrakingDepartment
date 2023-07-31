<%@ Page Title="Configuration Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ConfiguartionMaster.aspx.cs" Inherits="Master_ConfiguartionMaster" %>

<asp:Content ID="frmConfigMaster" ContentPlaceHolderID="MasterPage" runat="server">
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
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Configuration Master"></asp:Label>
            </div>
        </div>
        <div id="divFomr" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Configuration <span class="Card-title-second">Master </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="ConfigType"
                        runat="server"
                        Text="Configuration Type"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span
                        data-toggle="tooltip"
                        data-placement="right"
                        data-original-title="i.e. Payment,  Floor Type etc.,"
                        class="mdi mdi-information-outline tooltipIcon"></span><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlConfigType"
                        runat="server"
                        CssClass="form-control"
                        AutoComplete="off"
                        TabIndex="1">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfdConfigType"
                        ValidationGroup="ConfigMaster"
                        ControlToValidate="ddlConfigType"
                        runat="server" CssClass="rfvClr"
                        InitialValue="0"
                        ErrorMessage="Select Configuration Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="ConfigName"
                        runat="server"
                        Text="Configuration Name"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span
                        data-toggle="tooltip"
                        data-placement="right"
                        data-original-title="i.e. Cash, Card, UPI etc.,"
                        class="mdi mdi-information-outline tooltipIcon"></span><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtConfigName"
                        runat="server" MaxLength="50"
                        CssClass="form-control"
                        AutoComplete="off"
                        TabIndex="2">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdconfigName"
                        ValidationGroup="ConfigMaster"
                        ControlToValidate="txtConfigName"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Configuration Name"> 
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        CssClass="pure-material-button-contained btnBgColorAdd"
                        Text="Submit"
                        ValidationGroup="ConfigMaster"
                        OnClick="btnSubmit_Click"                                         
                        runat="server" />
                </div>
                <div>
                    <asp:Button
                        ID="btnReset"
                        CssClass="pure-material-button-contained btnBgColorCancel"
                        OnClick="btnReset_Click"
                        Text="Cancel"                      
                        CausesValidation="false"
                        runat="server" />
                </div>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Configuration <span class="Card-title-second">Master </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%--    <asp:Button
                        ID="btnAdd"
                        runat="server"
                        OnClick="btnAdd_Click"
                        Text="add +"
                        CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvConfigMaster" runat="server"
                        AllowPaging="True"
                        DataKeyNames="configId"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None" PageSize="25500">
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
                                        ID="lblconfigTypeName"
                                        runat="server"
                                        Text='<%#Bind("configTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="configTypeId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblconfigTypeId"
                                        runat="server"
                                        Text='<%#Bind("configTypeId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Configuration Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblconfigName"
                                        runat="server"
                                        Text='<%#Bind("configName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="configId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblconfigId"
                                        runat="server"
                                        Text='<%#Bind("configId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="lnkEdit"
                                        runat="server"
                                        Text="Edit"
                                        src="../../images/edit-icon.png"
                                        alt="image"
                                        Visible='<%#Eval("activeStatus").ToString()=="A"? true:false %>'
                                        OnClick="lnkEdit_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="lnkActiveorInactive"
                                        runat="server"
                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'
                                        OnClick="lnkActiveorInactive_Click"></asp:LinkButton>
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

