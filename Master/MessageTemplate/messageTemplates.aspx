<%@ Page Title="Message Templates" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="messageTemplates.aspx.cs" Inherits="Master_messageTemplates" %>

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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol"
                    Text="Message Templates"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Message <span class="Card-title-second">Templates </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lbltemplateType"
                        runat="server"
                        Text="Template Type">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:RadioButtonList
                        ID="rbtnltemplateType"
                        runat="server"
                        CssClass="inline-rb" AutoPostBack="true"
                        TabIndex="1" OnSelectedIndexChanged="rbtnltemplateType_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="Mail" Value="M"></asp:ListItem>
                        <asp:ListItem Text="SMS" Value="S"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator
                        ID="rfdtemplateType"
                        ValidationGroup="MailMaster"
                        ControlToValidate="rbtnltemplateType"
                        runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Template Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div id="divpeid" runat="server" class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblpeid"
                        runat="server"
                        Text="Pe Id">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtpeid"
                        runat="server" AutoComplete="off"
                        TabIndex="2"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdpeid"
                        ValidationGroup="MailMaster"
                        ControlToValidate="txtpeid"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Pe Id">
                    </asp:RequiredFieldValidator>
                </div>
                <div id="divtpid" class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4" runat="server">
                    <asp:Label
                        ID="lblttpd"
                        runat="server"
                        Text="Tp Id">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txttpid"
                        runat="server"
                        TabIndex="3" AutoComplete="off"
                        MaxLength="30"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdtpid"
                        ValidationGroup="MailMaster"
                        ControlToValidate="txttpid"
                        runat="server"
                        CssClass="rfvClr"
                        Maxlength="30"
                        ErrorMessage="Enter Tp Id">
                    </asp:RequiredFieldValidator>
                </div>

            </div>

            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblheader"
                        runat="server"
                        Text="Header">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtHeader"
                        runat="server"
                        TabIndex="4" MaxLength="50"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdinstructionType"
                        ValidationGroup="MailMaster"
                        ControlToValidate="txtHeader"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Header">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8">
                    <asp:Label
                        ID="lblsubject"
                        runat="server"
                        Text="Subject">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtsubject" AutoComplete="off"
                        runat="server"
                        TabIndex="5" MaxLength="150" TextMode="MultiLine"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdsubject"
                        ValidationGroup="MailMaster"
                        ControlToValidate="txtsubject"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Subject">
                    </asp:RequiredFieldValidator>
                </div>

            </div>

            <div class="row">

                <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <asp:Label
                        ID="lblmessageBody"
                        runat="server"
                        Text="Body">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtmessageBody"
                        runat="server"
                        TabIndex="6" TextMode="MultiLine" AutoComplete="off"
                        CssClass="form-control">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdmessageBody"
                        ValidationGroup="MailMaster"
                        ControlToValidate="txtmessageBody"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Body">
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
                        ValidationGroup="MailMaster"
                        TabIndex="7"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="Cancel"
                        TabIndex="8"
                        CausesValidation="false"
                        OnClick="btnCancel_Click"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>

        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Message <span class="Card-title-second">Templates </span></h4>
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
                        ID="gvMsgTemplateDetails"
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

                            <asp:TemplateField HeaderText="Template Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvTemplateType"
                                        runat="server"
                                        Text='<%#Eval("templateType").ToString()=="M" ? "Mail" : "SMS" %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Header">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvMessageHeader"
                                        runat="server"
                                        Text='<%#Bind("messageHeader") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Subject" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvSubject"
                                        runat="server"
                                        Text='<%#Bind("subject") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Body" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvMessageBody"
                                        runat="server"
                                        Text='<%#Bind("messageBody") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pe id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvPeid"
                                        runat="server"
                                        Text='<%#Bind("peid") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Tp id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvTpid"
                                        runat="server"
                                        Text='<%#Bind("tpid") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="imgBtnEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="Edit"
                                        Text="Edit"
                                        OnClick="imgBtnEdit_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="imgBtnDelete"
                                        runat="server"
                                        src="../../images/delete-icon.png"
                                        alt="Delete"
                                        Text="Delete"
                                        OnClick="imgBtnDelete_Click" />
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

