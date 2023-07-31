<%@ Page Title="FAQ" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FAQ.aspx.cs" Inherits="Master_FAQ" %>

<asp:Content ID="FrmFAQ" ContentPlaceHolderID="MasterPage" runat="Server">
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="FAQ Master"></asp:Label>
            </div>
        </div>

        <div id="divForm" runat="server" visible="false">

            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>FAQ <span class="Card-title-second">Master </span></h4>
                </div>
            </div>

            <div class="row">

                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                    <asp:Label
                        ID="lblQuestion"
                        runat="server"
                        Text="Question"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtque" runat="server" TabIndex="1" TextMode="MultiLine" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="BlockMaster" ControlToValidate="txtque"
                        runat="server" CssClass="rfvClr" ErrorMessage="Enter Question">
                    </asp:RequiredFieldValidator>
                </div>

                <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                    <asp:Label
                        ID="lblAnswer"
                        runat="server"
                        Text="Answer"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:TextBox ID="txtans" runat="server" TabIndex="2" AutoComplete="off" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="BlockMaster" ControlToValidate="txtans"
                        runat="server" CssClass="rfvClr" ErrorMessage="Enter Answer">
                    </asp:RequiredFieldValidator>
                </div>

            </div>

            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="BlockMaster" TabIndex="3"
                        CssClass="pure-material-button-contained btnBgColorAdd" OnClick="btnSubmit_Click" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" TabIndex="4"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>

        </div>

        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">FAQ <span class="Card-title-second">Master </span></h4>
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
                        ID="gvFAQDetails"
                        runat="server"
                        Visible="true"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        DataKeyNames="faqid"
                        CssClass="gvv display">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="FAQ Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvFAQId"
                                        runat="server"
                                        Text='<%#Bind("faqId") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Question">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvQuestion"
                                        runat="server"
                                        Text='<%#Bind("question") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Answer">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvAnswer"
                                        runat="server"
                                        Text='<%#Bind("answer") %>'>
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

