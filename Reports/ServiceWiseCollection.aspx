<%@ Page Title="Service Wise Report" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="ServiceWiseCollection.aspx.cs" Inherits="Reports_ServiceWiseCollection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        table.gvv th {
            text-align: left;
            background: linear-gradient(to bottom, #2196f3 0%, #3280c0 100%);
            color: #fff;
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
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Reports"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Service Wise Report"></asp:Label>
            </div>
        </div>

        <div id="divForm" runat="server" visible="true">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Service Wise <span class="Card-title-second">Report </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="From Date"></asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtFromDate" runat="server" AutoComplete="off" onkeypress="return isNumber(event);"
                        MaxLength="5" CssClass="form-control fromDate"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ServiceWiseReport"
                        ControlToValidate="txtFromDate" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter From Date">
                    </asp:RequiredFieldValidator>
                </div>
           <%--     <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="To Date"></asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtTodate" runat="server" AutoComplete="off" onkeypress="return isNumber(event);"
                        MaxLength="5" CssClass="form-control toDate" OnTextChanged="txtTodate_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        ValidationGroup="ServiceWiseReport" ControlToValidate="txtTodate" runat="server"
                        CssClass="rfvClr" ErrorMessage="Enter To Date">
                    </asp:RequiredFieldValidator>
                </div>--%>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="Payment Type"></asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="ServiceWiseReport" InitialValue="Select"
                        ControlToValidate="ddlPaymentType" runat="server" CssClass="rfvClr" ErrorMessage="Select Payment Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="Services"></asp:Label><span class="spanStar">*</span>

                    <asp:DropDownList ID="ddlServices" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlServices_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="DW">Department Web</asp:ListItem>
                        <asp:ListItem Value="PW">Public Web</asp:ListItem>
                        <asp:ListItem Value="DA">Department Android</asp:ListItem>
                        <asp:ListItem Value="PA">Public Android</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="ServiceWiseReport" InitialValue="Select"
                        ControlToValidate="ddlServices" runat="server" CssClass="rfvClr" ErrorMessage="Select Shift Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">

                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="User Name"></asp:Label><span class="spanStar">*</span>

                    <asp:DropDownList ID="ddlUserName" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdUserName" ValidationGroup="ServiceWiseReport"
                        InitialValue="Select" ControlToValidate="ddlUserName" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select User Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="Vehicle Type"></asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlVehicleType_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdBlockName" ValidationGroup="ServiceWiseReport" InitialValue="Select"
                        ControlToValidate="ddlVehicleType" runat="server" CssClass="rfvClr" ErrorMessage="Select Vehicle Type">
                    </asp:RequiredFieldValidator>
                </div>

            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnDetailedPrint" runat="server" Text="Detailed Report" OnClick="btnDetailedPrint_Click" ValidationGroup="ServiceWiseReport" CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                </div>
            </div>
        </div>
        <div id="divGv" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Collection</h4>
                </div>
            </div>

            <div id="divGridView" runat="server" class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvServiceWiseReport"
                        runat="server"
                        AllowPaging="True"
                        CssClass="gvv"
                        BorderStyle="None"
                        PageSize="25000"
                        AutoGenerateColumns="false" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Particular" Visible="true">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvParticular"
                                        runat="server"
                                        Text='<%#Bind("Particular") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Count" Visible="true">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvCount"
                                        runat="server"
                                        Text='<%#Bind("Count") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvTotalAmount"
                                        runat="server"
                                        Text='<%#Eval("TotalAmount","{0:n}" )%>' Visible="true"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>

                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                    </asp:GridView>
                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField ID="hfshiftId" runat="server" />
</asp:Content>

