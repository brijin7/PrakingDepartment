<%@ Page Title="Pass Transaction Details" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="PassTransList.aspx.cs" Inherits="Reports_PassTransList" %>

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
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Reports"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Pass Transaction Report"></asp:Label>
            </div>
        </div>

        <div id="divForm" runat="server" visible="true">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Pass Transaction <span class="Card-title-second">Report </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="From Date"></asp:Label><span class="spanStar">*</span>     
                    <asp:TextBox ID="txtFromDate" runat="server" AutoComplete="off"
                        MaxLength="0" CssClass="form-control fromDate"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="PassTransList"
                        ControlToValidate="txtFromDate" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select From Date">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <asp:Label runat="server" CssClass="form-check-label"
                        Text="To Date"></asp:Label><span class="spanStar">*</span>    
                    <asp:TextBox ID="txtTodate" runat="server" AutoComplete="off"
                        MaxLength="0" CssClass="form-control toDate"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                        ValidationGroup="PassTransList" ControlToValidate="txtTodate" runat="server"
                        CssClass="rfvClr" ErrorMessage="Select To Date">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                    <div class="justify-content-end" style="padding-top: 2.5rem;">

                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            ValidationGroup="PassTransList" CssClass="pure-material-button-contained btnBgColorAdd" />

                    </div>
                </div>
            </div>

        </div>


        <div id="divGv" runat="server" class="col-12 col-xs-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
            <div class="table-responsive section">
                <asp:GridView
                    ID="gvPassTransList"
                    runat="server"
                    AllowPaging="True"
                    CssClass="gvv display"
                    BorderStyle="None"
                    PageSize="25000"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vehicle Type">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvvehicleName"
                                    runat="server"
                                    Text='<%#Bind("vehicleName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pass Id">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvPassId"
                                    runat="server"
                                    Text='<%#Bind("parkingPassTransId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone No." HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvPhoneNumber"
                                    runat="server"
                                    Text='<%#Bind("phoneNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Date" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvStartDate"
                                    runat="server"
                                    Text='<%#Eval("validStartDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Date" HeaderStyle-CssClass="gvHeader">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvEndDate"
                                    runat="server"
                                    Text='<%#Eval("validEndDate") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>


    </div>

</asp:Content>

