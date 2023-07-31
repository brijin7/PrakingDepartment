<%@ Page Title="General Print Instructions" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
     CodeFile="PrintingInsSA.aspx.cs" Inherits="Master_PrintingInsSA" %>

<asp:Content ID="frmPrintingInstructions" ContentPlaceHolderID="MasterPage" runat="Server">
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
                    Text="General Print  Instructions"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>General Print  <span class="Card-title-second">Instructions </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblinstructionType"
                        runat="server"
                        CssClass="form-check-label"
                        Text="Instruction Type">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:RadioButtonList
                        ID="rbtnlinstructionType"
                        runat="server"
                        CssClass="inline-rb"
                        TabIndex="1"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="Receipt" Selected="True" Value="R"></asp:ListItem>
                        <asp:ListItem Text="Pass" Value="P"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator
                        ID="rfdinstructionType"
                        ValidationGroup="PrintMaster"
                        ControlToValidate="rbtnlinstructionType"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Select Instruction Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-8">
                    <asp:Label
                        ID="lblinstructions"
                        runat="server"
                        CssClass="form-check-label"
                        Text="Instructions">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtinstructions"
                        runat="server"
                        TabIndex="2" TextMode="MultiLine" AutoComplete="off"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdinstructions"
                        ValidationGroup="PrintMaster"
                        ControlToValidate="txtinstructions"
                        runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Instructions">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit"
                        OnClick="btnSubmit_Click"
                        ValidationGroup="PrintMaster"
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
                    <h4 class="card-title">General Print  <span class="Card-title-second">Instructions</span></h4>
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
                        ID="gvPrintingInstructions"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="uniqueId"
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
                            <asp:TemplateField HeaderText="Instruction Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvinstructionType"
                                        runat="server"
                                        Text='<%#Eval("instructionType").ToString() =="P"?"Pass":"Receipt"%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Instructions">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvinstructions"
                                        runat="server"
                                        Text='<%#Bind("instructions") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="uniqueId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvuniqueId"
                                        runat="server"
                                        Text='<%#Bind("uniqueId") %>'></asp:Label>
                                </ItemTemplate>
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
                                        runat="server"
                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>' OnClick="lnkActiveOrInactive_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfuniqueId" runat="server" />
</asp:Content>

