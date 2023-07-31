<%@ Page Title="Branch Working Hours" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="BranchWorkingHrs.aspx.cs" Inherits="Master_Branch_BranchWorkingHrs" %>

<asp:Content ID="FrmBranchWorkingHrs" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .Daybtn {
            background-color: #97b5c9;
            border-radius: 2px;
            color: black;
            cursor: pointer;
            margin: 3px;
            display: inline-block;
            font-family: CerebriSans-Regular,-apple-system,system-ui,Roboto,sans-serif;
            padding: 7px 10px;
            text-align: center;
            text-decoration: none;
            transition: all 250ms;
            border: 0;
            -webkit-user-select: none;
            touch-action: manipulation;
        }

            .Daybtn:hover {
                transform: scale(1.05);
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
                <asp:Label ID="lblnavsecond" runat="server" CssClass="pageRoutecol" Text="Branch"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavthird" runat="server" CssClass="pageRoutecol"
                    Text="Branch Working Hours"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Branch Working <span class="Card-title-second">Hours </span></h4>
                </div>
            </div>
            <div class="row">
                <div>
                    <div>
                        <asp:Label
                            ID="lblWorkingDays"
                            runat="server"
                            Text="Working Days">
                        </asp:Label><span class="spanStar">*</span>
                    </div>
                    <asp:Label runat="server" ID="lblWorkingDaysValue" Visible="false"></asp:Label>
                    <asp:Button ID="btnSun" runat="server" Text="Sunday" CssClass="Daybtn" OnClick="btnSun_Click"></asp:Button>
                    <asp:Button ID="btnMon" runat="server" Text="Monday" CssClass="Daybtn" OnClick="btnMon_Click"></asp:Button>
                    <asp:Button ID="btnTue" runat="server" Text="Tuesday" CssClass="Daybtn" OnClick="btnTue_Click"></asp:Button>
                    <asp:Button ID="btnWed" runat="server" Text="Wednesday" CssClass="Daybtn" OnClick="btnWed_Click"></asp:Button>
                    <asp:Button ID="btnThu" runat="server" Text="Thursday" CssClass="Daybtn" OnClick="btnThu_Click"></asp:Button>
                    <asp:Button ID="btnFri" runat="server" Text="Friday" CssClass="Daybtn" OnClick="btnFri_Click"></asp:Button>
                    <asp:Button ID="btnSat" runat="server" Text="Saturday" CssClass="Daybtn" OnClick="btnSat_Click"></asp:Button>
                </div>
            </div>
            <div id="DivHrDetails" runat="server" visible="false">
                <div class="row">
                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 mt-4">
                        <asp:Label
                            ID="lblisHoliday"
                            runat="server"
                            Text="Is Holiday">
                        </asp:Label><span class="spanStar">*</span>
                        <asp:RadioButtonList
                            ID="rbtnisHoliday"
                            runat="server"
                            CssClass="inline-rb"
                            TabIndex="2"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Selected="true" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            ID="rfvisHoliday"
                            ValidationGroup="WorkingHrs"
                            ControlToValidate="rbtnisHoliday"
                            runat="server" CssClass="rfvClr"
                            ErrorMessage="Select Any One">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                        <asp:Label
                            ID="lblfrmtime"
                            runat="server"
                            Text="From Time">
                        </asp:Label><span class="spanStar">*</span>
                        <asp:TextBox
                            ID="txtfrmtime"
                            runat="server"
                            TabIndex="3"
                            onkeypress="return isNumber(event);"
                            MaxLength="5"
                            CssClass="form-control timePicker">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator
                            ID="rfvfrmtime"
                            ValidationGroup="WorkingHrs"
                            ControlToValidate="txtfrmtime"
                            runat="server" CssClass="rfvClr"
                            ErrorMessage="Select From Time">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                        <asp:Label
                            ID="lbltotime"
                            runat="server"
                            Text="To Time">
                        </asp:Label><span class="spanStar">*</span>
                        <asp:TextBox
                            ID="txttotime"
                            runat="server"
                            TabIndex="4"
                            onkeypress="return isNumber(event);"
                            MaxLength="5"
                            CssClass="form-control timePicker">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator
                            ID="rfvtotime"
                            ValidationGroup="WorkingHrs"
                            ControlToValidate="txttotime"
                            runat="server" CssClass="rfvClr"
                            ErrorMessage="Select To Time">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>



            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit"
                        OnClick="btnSubmit_Click"
                        ValidationGroup="WorkingHrs"
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
                    <h4 class="card-title">Branch Working  <span class="Card-title-second">Hours </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%--  <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="add +"
                        CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvbranchworkinghrs"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="branchId"
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
                            <asp:TemplateField HeaderText="Parking Owner" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvparkingOwnerId"
                                        runat="server"
                                        Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbranchId"
                                        runat="server"
                                        Text='<%#Bind("branchId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Working Day">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvworkingDay"
                                        runat="server"
                                        Text='<%#Bind("workingDay") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From Time">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvfromTime"
                                        runat="server"
                                        Text='<%#Bind("fromTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Time">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtoTime"
                                        runat="server"
                                        Text='<%#Bind("toTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Is Holiday">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvisHoliday"
                                        runat="server"
                                        Text='<%#Bind("isHoliday") %>'></asp:Label>
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
                                        runat="server" OnClick="LnkEdit_Click"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit" />
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

