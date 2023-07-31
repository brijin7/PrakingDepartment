<%@ Page Title="Floor Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
     CodeFile="FloorMaster.aspx.cs" Inherits="Master_Floor_FloorMaster" EnableEventValidation="false" %>

<asp:Content ID="FrmFloorMaster" ContentPlaceHolderID="MasterPage" runat="Server">
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Floor"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Floor Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Floor <span class="Card-title-second">Master</span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblBlock"
                        runat="server"
                        Text="Block"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlBlock"
                        runat="server"
                        TabIndex="1"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfvBlock"
                        runat="server"
                        ControlToValidate="ddlBlock"
                        InitialValue="0"
                        CssClass="rfvClr"
                        ErrorMessage="Select Block">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblFloorName"
                        runat="server"
                        Text="Floor Name"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlFloorName"
                        runat="server"
                        TabIndex="2"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfvFloorName"
                        runat="server"
                        ControlToValidate="ddlFloorName"
                        InitialValue="0"
                        CssClass="rfvClr"
                        ErrorMessage="Select Floor Name">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblFloorType"
                        runat="server"
                        Text="Floor Type"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList
                        ID="ddlFloorType"
                        runat="server"
                        TabIndex="3"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="rfvFloorType"
                        runat="server"
                        ControlToValidate="ddlFloorType"
                        InitialValue="0"
                        CssClass="rfvClr"
                        ErrorMessage="Select Floor Type">
                    </asp:RequiredFieldValidator>
                </div>

            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblSquarefeet"
                        runat="server"
                        Text="Square Feet"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtSquarefeet"
                        runat="server"
                        TabIndex="4"
                        CssClass="form-control"
                        AutoComplete="Off"
                        onkeypress="return isNumber(event);"
                        MaxLength="12">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvSquareFeet"
                        runat="server"
                        ControlToValidate="txtSquarefeet"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Square Feet">
                    </asp:RequiredFieldValidator>
                </div>

            </div>

            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit"
                        TabIndex="5"
                        OnClick="btnSubmit_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="cancel"
                        TabIndex="6"
                        OnClick="btnCancel_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
            <div class="row p-4 justify-content-md-start" id="divAddMasters" runat="server" visible="false">
                <div class="mr-3">
                    <asp:Button
                        ID="btnFloorFeatures"
                        runat="server"
                        Text="Floor Features"
                        CausesValidation="false"
                        TabIndex="5"
                        OnClick="btnFloorFeatures_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div class="mr-3">
                    <asp:Button
                        ID="btnFloorvehicleMaster"
                        runat="server"
                        Text="Floor Vehicle"
                        TabIndex="6"
                        OnClick="btnFloorvehicleMaster_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
                <div>
                    <asp:Button
                        ID="btnPriceMaster"
                        runat="server"
                        Text="Price"
                        TabIndex="6"
                        OnClick="btnPriceMaster_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                </div>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Floor <span class="Card-title-second">Master</span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%--    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                         Text="add +" CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="table-responsive section">

                    <asp:GridView
                        ID="gvFloorDetails"
                        runat="server"
                        DataKeyNames="floorId"
                        AutoGenerateColumns="false"
                        CssClass="gvv display" BorderStyle="None">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="10px">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Block">
                                <ItemTemplate>
                                    <asp:Label ID="lblGvBlockName" runat="server" Text='<%#Bind("blockName") %>'>
                                    </asp:Label>
                                    <asp:Label ID="lblblockId" runat="server" Visible="false" Text='<%#Bind("blockId") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Floor Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblfloorNameId" runat="server" Text='<%#Bind("floorNameId") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblfloorName" runat="server" Text='<%#Bind("floorName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Floor Type">
                                <ItemTemplate>
                                    <asp:Label ID="lblfloorType" runat="server" Text='<%#Bind("floorType") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblfloorTypeName" runat="server" Text='<%#Bind("floorTypeName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sq. ft" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblsquareFeet"
                                        runat="server"
                                        Text='<%#Bind("squareFeet") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="LnkEdit"
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="LnkEdit_Click" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="45px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="lnkActiveOrInactive"
                                        runat="server"
                                        CssClass='<%#Eval("activeStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                        Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'
                                        OnClick="lnkActiveOrInactive_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="55px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

