<%@ Page Title="Offer Mapping" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="OfferMapping.aspx.cs" Inherits="Master_OfferMapping" %>

<asp:Content ID="FrmOfferMapping" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .labels {
            font-weight: 900;
            color: #2196f3;
            font-size: 1.5rem;
            margin-left: 10px;
        }

        legend {
            display: block;
            width: inherit;
            max-width: 100%;
            padding: 0px 5px;
            font-size: inherit;
            font-weight: bold;
            line-height: inherit;
            color: inherit;
            white-space: normal;
        }

        .legBorder {
            border-width: 1px;
            border-color: #2196f373;
            border-radius: 6px;
            border-style: dashed;
            padding: 6px;
        }
    </style>
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
                    Text="Offers"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol"
                    Text="Offer Mapping"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Offer <span class="Card-title-second">Mapping </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblbranchname"
                        runat="server"
                        Text="Branch Name">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlbranchname"
                        runat="server"
                        TabIndex="2"
                        Enabled="false"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfdbranchname"
                        ValidationGroup="OfferMaster"
                        ControlToValidate="ddlbranchname"
                        InitialValue="0"
                        runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Branch Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lbloffname"
                        runat="server"
                        Text="Offer Name">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddloffname"
                        runat="server"
                        TabIndex="1"
                        CssClass="form-control" OnSelectedIndexChanged="ddloffname_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfdoffname"
                        ValidationGroup="OfferMaster"
                        ControlToValidate="ddloffname"
                        runat="server"
                        InitialValue="0"
                        CssClass="rfvClr"
                        ErrorMessage="Select Offer Name">
                    </asp:RequiredFieldValidator>
                </div>

            </div>

            <div id="DivCheckIn" runat="server" visible="false">
                <fieldset class="legBorder mb-3">
                    <legend>Offer Details</legend>
                    <asp:Label ID="lblOfferNameheading" runat="server" Font-Bold="true" CssClass="labels" Visible="false"
                        Style="font-size: 25px"></asp:Label>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12">
                            <div class="row">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-5">
                                    <label for="lblmaxcharge" class="labels">
                                        Description :
                                    </label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-lg-7 col-xs-7 ">
                                    <asp:Label ID="lblOfferDes" runat="server" Font-Bold="true"></asp:Label>

                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12">
                            <div class="row">
                                <div class="col-sm-5 col-md-5 col-lg-5 col-xs-5">
                                    <label for="lblmaxcharge" class="labels" style="margin-left: 0px;">
                                        Offer Value<span id="PerorFix" runat="server"
                                            style="font-size: 10px; color: black; padding-left: 6px"></span> :
                                    </label>
                                </div>
                                <div class="col-sm-7 col-md-7 col-lg-7 col-xs-7">
                                    <asp:Label ID="lblAmount" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>

                    </div>

                    <fieldset class="legBorder m-3" style="border-style: dotted;">
                        <legend>Validity</legend>
                        <div class="row">
                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-5">
                                        <label for="lblmaxcharge" class="labels">
                                            From :
                                        </label>
                                    </div>

                                    <div class="col-sm-7 col-md-7 col-lg-7 col-xs-7 ">
                                        <asp:Label ID="lblfromdate" runat="server" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-6 col-xs-12">
                                <div class="row">
                                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-5">
                                        <label for="lblmaxcharge" class="labels">
                                            To :
                                        </label>
                                    </div>
                                    <div class="col-sm-7 col-md-7 col-lg-7 col-xs-7 ">
                                        <asp:Label ID="lblTodate" runat="server" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>

                    <div class="row" id="Rules" runat="server">
                        <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                    <label for="lblmaxcharge" class="labels">
                                        Rules :
                                    </label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12 col-xs-12 mb-2">
                                    <asp:DataList ID="dtlRules" RepeatColumns="1"
                                        RepeatDirection="Vertical" runat="server" Width="100%">

                                        <ItemTemplate>
                                            <div class="col-12" style="height: 22px">
                                                <span style="font-size: 16px; color: #2196f3;" class="mdi mdi-star"></span>
                                                <asp:Label ID="lblRulestext" runat="server"
                                                    Visible="true" Text='<%# Eval("offerRule") %>' Font-Bold="true"></asp:Label>
                                            </div>
                                        </ItemTemplate>

                                    </asp:DataList>
                                </div>
                            </div>
                        </div>

                    </div>
                </fieldset>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit"
                        ValidationGroup="OfferMaster"
                        OnClick="btnSubmit_Click"
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
                    <h4 class="card-title">Offer <span class="Card-title-second">Mapping </span></h4>
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
                        ID="gvOfferMapping"
                        runat="server"
                        Visible="true"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        DataKeyNames="offerMappingId"
                        CssClass="gvv display">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="OfferMapping Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvOfferMappingId"
                                        runat="server"
                                        Text='<%#Bind("offerMappingId") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Branch" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvBranchId"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("branchId") %>'>
                                    </asp:Label>
                                    <asp:Label
                                        ID="lblGvBranchName"
                                        runat="server"
                                        Text='<%#Bind("branchName") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvOfferId"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("offerId") %>'>
                                    </asp:Label>
                                    <asp:Label
                                        ID="lblGvOfferHeading"
                                        runat="server"
                                        Text='<%#Bind("offerHeading") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Value" Visible="true">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvofferType"
                                        runat="server" Visible="false"
                                        Text='<%#Eval("offerType").ToString() == "P" ? "Percentage":"Fixed" %>'>
                                    </asp:Label>
                                    <asp:Label
                                        ID="lblGvofferValue"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("offerValue")%>'>
                                    </asp:Label>
                                    <asp:Label
                                        ID="lblGvofferValueType"
                                        runat="server"
                                        Text='<%#Eval("offerValue").ToString() + ""+ (Eval("offerType").ToString() == "P" ? "(%)":"(₹)") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Min. Amt" Visible="true" HeaderStyle-Width="30">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvminAmt"
                                        runat="server"
                                        Text='<%#Bind("minAmt") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Max. Amt" Visible="true" HeaderStyle-Width="30">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblGvmaxAmt"
                                        runat="server"
                                        Text='<%#Bind("maxAmt") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="View" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="lnkView"
                                        runat="server"                                       
                                        Width="45px"
                                        src="../../images/View.png"
                                        alt="View"
                                        Text="Edit"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="lnkView_Click" />
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

