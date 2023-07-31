<%@ Page Title="Online Booking Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="OnlineBookingMaster.aspx.cs"
    Inherits="Master_OnlineBookingSetup" EnableEventValidation="false" %>

<asp:Content ID="FrmOnlineBookingSetup" ContentPlaceHolderID="MasterPage" runat="Server">

    <style>
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

    <%--Days Or Min Dropdown--%>
    <script>

        function lblDaysorMin() {
            var value = document.getElementById("<%=ddlHorDType.ClientID%>");
            var getvalue = value.options[value.selectedIndex].value;
            if (getvalue == 'D') {
                document.getElementById("lblDorHNo").innerText = 'Advance Booking Days';
            }
            else {
                document.getElementById("lblDorHNo").innerText = 'Advance Booking Hours';
            }

        }
    </script>

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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Branch"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>

            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Online Booking Master"></asp:Label>
            </div>
        </div>

        <div id="divForm" runat="server" visible="false">

            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Online <span class="Card-title-second">Booking Master</span></h4>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblOwner" runat="server" Text="Parking Owner Name"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlownername" runat="server"
                        CssClass="form-control" TabIndex="1">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                        ValidationGroup="BranchMaster"
                        ControlToValidate="ddlownername"
                        runat="server" CssClass="rfvClr" InitialValue="0"
                        ErrorMessage="Select Parking Owner Name"> </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblBranchName" runat="server" Text="Branch Name"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlBranch" runat="server"
                        CssClass="form-control" TabIndex="2">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvBranchName" runat="server" ValidationGroup="BranchMaster"
                        ControlToValidate="ddlBranch" InitialValue="0"
                        CssClass="rfvClr" ErrorMessage="Select Branch Name"> </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <div id="dvOnlineBooking" runat="server">
                        <fieldset class="legBorder mb-3">
                            <legend>Online Advance Booking</legend>
                            <div>
                                <div class="row">
                                    <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                        <asp:Label ID="lblDorH" runat="server" Text="Advance Booking Type"
                                            CssClass="form-check-label">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlHorDType" runat="server"
                                            CssClass="form-control" AutoComplete="off" OnChange="lblDaysorMin()"
                                            TabIndex="17">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="D">Days</asp:ListItem>
                                            <asp:ListItem Value="H">Hours</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                        <asp:Label ID="lblDorHNo" runat="server" Text="Days / Min." ClientIDMode="Static"
                                            CssClass="form-check-label">
                                        </asp:Label>
                                        <asp:TextBox ID="txtNoofDorH" runat="server"
                                            CssClass="form-control" AutoComplete="off"
                                            MaxLength="20" onkeypress="return isNumber(event);"
                                            TabIndex="18"></asp:TextBox>
                                    </div>
                                    <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                        <asp:Label ID="lblAdvanceBooking" runat="server" Text="Advance Booking Charges (₹)"
                                            CssClass="form-check-label">
                                        </asp:Label>
                                        <asp:TextBox ID="txtAdBCharge" runat="server"
                                            CssClass="form-control" AutoComplete="off"
                                            MaxLength="6" onkeypress="return AllowOnlyAmountAndDot(this.id);"
                                            TabIndex="19"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <br />
                    <div id="dvMultiDays" runat="server">
                        <fieldset class="legBorder mb-3">
                            <legend>Multiple Days Booking</legend>
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                    <div>
                                        <div class="row">
                                            <div id="divMinHr" class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label ID="lblMinHour" runat="server" Text="Min. Hour"
                                                    CssClass="form-check-label">
                                                </asp:Label>
                                                <asp:TextBox ID="txtminHour" runat="server"
                                                    CssClass="form-control" AutoComplete="off"
                                                    MaxLength="6" onkeypress="return AllowOnlyAmountAndDot(this.id);"
                                                    TabIndex="20"></asp:TextBox>
                                            </div>
                                            <div id="divMaxHr" class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label ID="lblMaxHour" runat="server" Text="Max. Hour"
                                                    CssClass="form-check-label">
                                                </asp:Label>
                                                <asp:TextBox ID="txtMaxhour" runat="server"
                                                    CssClass="form-control" AutoComplete="off"
                                                    MaxLength="6" onkeypress="return AllowOnlyAmountAndDot(this.id);"
                                                    TabIndex="21"></asp:TextBox>
                                            </div>
                                            <div id="divMinD" class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label ID="lblMinday" runat="server" Text="Min. Day"
                                                    CssClass="form-check-label">
                                                </asp:Label>
                                                <asp:TextBox ID="txtminDay" runat="server"
                                                    CssClass="form-control" AutoComplete="off"
                                                    MaxLength="6" onkeypress="return isNumber(event);"
                                                    TabIndex="22"></asp:TextBox>
                                            </div>
                                            <div id="divMaxD" class="col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">
                                                <asp:Label ID="lblMaxDay" runat="server" Text="Max. Day"
                                                    CssClass="form-check-label">
                                                </asp:Label>
                                                <asp:TextBox ID="txtMaxDay" runat="server"
                                                    CssClass="form-control" AutoComplete="off"
                                                    MaxLength="6" onkeypress="return isNumber(event);"
                                                    TabIndex="23"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>

            <div class="row p-3 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="pure-material-button-contained btnBgColorAdd" Text="Update"
                        TabIndex="8" ValidationGroup="EmployeeMaster" OnClick="btnSubmit_Click" />
                </div>
                <asp:Button ID="btnCancel" runat="server" CssClass="pure-material-button-contained btnBgColorCancel" Text="Cancel"
                    TabIndex="9" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>

        </div>

        <div id="divGv" runat="server" visible="true">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Online <span class="Card-title-second">Booking Master </span></h4>
                </div>
                <%-- <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                </div>--%>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView ID="gvOBMaster" runat="server" AllowPaging="True"
                        DataKeyNames="branchId" CssClass="gvv display" AutoGenerateColumns="false"
                        BorderStyle="None" PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Parking Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblparkingOwnerId"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblownerName"
                                        runat="server"
                                        Text='<%#Bind("parkingName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Branch Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblbranchId"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblbranchName"
                                        runat="server"
                                        Text='<%#Bind("branchName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Online Booking" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblonlineBookingAvailability"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("onlineBookingAvailability") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblonlineBookingAvailabilityStat"
                                        runat="server"
                                        Text='<%#Eval("onlineBookingAvailability").ToString()=="Y"? "Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="MultiDays" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblmultiBook"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("multiBook") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblmultiBookStat"
                                        runat="server"
                                        Text='<%#Eval("multiBook").ToString()=="Y"? "Yes":"No" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Booking Type" HeaderStyle-CssClass="gvHeader" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lbladvanceBookingHourOrDayType"
                                        runat="server"
                                        Text='<%#Eval("advanceBookingHourOrDayType").ToString()=="D"? "Days":"Hours" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hour/Day" HeaderStyle-CssClass="gvHeader" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lbladvanceBookingHourOrDay"
                                        runat="server"
                                        Text='<%#Bind("advanceBookingHourOrDay") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Charges" HeaderStyle-CssClass="gvHeader" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lbladvanceBookingCharges"
                                        runat="server"
                                        Text='<%#Bind("advanceBookingCharges") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="minHour" HeaderStyle-CssClass="gvHeader" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblminHour"
                                        runat="server"
                                        Text='<%#Bind("minHour") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="maxHour" HeaderStyle-CssClass="gvHeader" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblmaxHour"
                                        runat="server"
                                        Text='<%#Bind("maxHour") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="minDay" HeaderStyle-CssClass="gvHeader" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblminDay"
                                        runat="server"
                                        Text='<%#Bind("minDay") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="maxDay" HeaderStyle-CssClass="gvHeader" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblmaxDay"
                                        runat="server"
                                        Text='<%#Bind("maxDay") %>'></asp:Label>
                                </ItemTemplate>
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
                                        OnClick="lnkView_Click"
                                        Visible='<%#Eval("multiBook").ToString() =="N"? Eval("onlineBookingAvailability").ToString() =="N" ?false:true:true%>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%-- Visible='<%#Eval("multiBook").ToString() =="N"? Eval("onlineBookingAvailability").ToString() =="N" ? false:true:true%>'/>--%>

                            <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:ImageButton ID="LnkEdit"
                                        runat="server"
                                        Text="Edit"
                                        src="../../images/edit-icon.png" alt="image" OnClientClick="showLoader();"
                                        OnClick="LnkEdit_Click"
                                        Visible='<%#Eval("multiBook").ToString() =="N"? Eval("onlineBookingAvailability").ToString() =="N" ? false:true:true%>' />
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

