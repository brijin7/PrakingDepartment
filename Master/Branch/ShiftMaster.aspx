<%@ Page Title="Shift Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ShiftMaster.aspx.cs" Inherits="Master_ShiftMaster" %>

<asp:Content ID="frmShiftMaster" ContentPlaceHolderID="MasterPage" runat="Server">

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
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Shift Master"></asp:Label>
            </div>
        </div>

        <div id="divFomr" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Shift <span class="Card-title-second">Master </span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Branch Name</asp:Label><span class="spanStar">*</span>

                    <asp:DropDownList ID="ddwnBranchName" CssClass="form-control" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdBranchName" ValidationGroup="ShiftMaster" InitialValue="0" ControlToValidate="ddwnBranchName" runat="server" CssClass="rfvClr"
                        ErrorMessage="Select Branch Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Shift Name</asp:Label><span class="spanStar">*</span>

                    <asp:DropDownList ID="ddlShiftName" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfdBlockName" ValidationGroup="ShiftMaster" InitialValue="0"
                        ControlToValidate="ddlShiftName" runat="server" CssClass="rfvClr" ErrorMessage="Select Shift Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Shift Start Time</asp:Label><span class="spanStar">*</span>

                    <asp:TextBox ID="txtSfStarttime" runat="server" AutoComplete="off" onkeypress="return isNumber(event);" MaxLength="5" CssClass="form-control timePicker"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="ShiftMaster" ControlToValidate="txtSfStarttime" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Shift Start Time">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Shift End Time</asp:Label><span class="spanStar">*</span>

                    <asp:TextBox ID="txtSfEndtime" runat="server" AutoComplete="off" onkeypress="return isNumber(event);" MaxLength="5" CssClass="form-control timePicker"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="ShiftMaster" ControlToValidate="txtSfEndtime" runat="server"
                        CssClass="rfvClr" ErrorMessage="Enter Shift End Time">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Break Start Time</asp:Label><span class="spanStar">*</span>

                    <asp:TextBox ID="txtBrStarttime" runat="server" AutoComplete="off" CssClass="form-control timePicker" MaxLength="5"
                        AutoPostBack="false" onkeypress="return isNumber(event);" OnTextChanged="txtBrStarttime_TextChanged"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="ShiftMaster" ControlToValidate="txtBrStarttime" runat="server"
                        CssClass="rfvClr" ErrorMessage="Enter Break Start Time">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Break End Time</asp:Label><span class="spanStar">*</span>

                    <asp:TextBox ID="txtBrEndtime" runat="server" AutoComplete="off" CssClass="form-control timePicker" MaxLength="5"
                        AutoPostBack="false" onkeypress="return isNumber(event);" OnTextChanged="txtBrEndtime_TextChanged"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="ShiftMaster" ControlToValidate="txtBrEndtime"
                        runat="server" CssClass="rfvClr" ErrorMessage="Enter Break End Time"> </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label runat="server" CssClass="form-check-label">
                        Grace Time </asp:Label><span class="spanStar">*</span>  <span style="font-size: 11px; font-weight: bold">(In Min.)</span>

                    <asp:TextBox ID="txtGracePeriod" runat="server" onkeypress="return isNumber(event);" MaxLength="5" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="ShiftMaster" ControlToValidate="txtGracePeriod" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Grace Time">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="ShiftMaster" CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Shift <span class="Card-title-second">Master </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%--<asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="add +" 
                        CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvShiftmaster"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="shiftId"
                        CssClass="gvv display"
                        BorderStyle="None"
                        PageSize="25000"
                        AutoGenerateColumns="false">
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
                            <asp:TemplateField HeaderText="Shift Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvshiftId"
                                        runat="server" Visible="false"
                                        Text='<%#Bind("shiftId") %>'></asp:Label>
                                    <asp:Label
                                        ID="lblgvshiftName"
                                        runat="server"
                                        Text='<%#Bind("shiftName") %>' Visible="false"></asp:Label>
                                    <asp:Label
                                        ID="lblGvshiftConfigName"
                                        runat="server"
                                        Text='<%#Bind("shiftConfigName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shift Start Time">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvstartTime"
                                        runat="server"
                                        Text='<%#Bind("startTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shift End Time">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvendTime"
                                        runat="server"
                                        Text='<%#Bind("endTime") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Break Start Time" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbreakStartTime"
                                        runat="server"
                                        Text='<%#Bind("breakStartTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Break End Time" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbreakEndTime"
                                        runat="server"
                                        Text='<%#Bind("breakEndTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Grace Period" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvgracePeriod"
                                        runat="server"
                                        Text='<%#Bind("gracePeriod") %>'></asp:Label>
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
    <asp:HiddenField ID="hfshiftId" runat="server" />
</asp:Content>
