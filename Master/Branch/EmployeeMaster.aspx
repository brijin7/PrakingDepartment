<%@ Page Title="Assign Employee" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="EmployeeMaster.aspx.cs"
    Inherits="Master_EmployeeMaster" EnableEventValidation="false" %>

<asp:Content ID="FrmEmployeeMaster" ContentPlaceHolderID="MasterPage" runat="Server">

    <style>
        .tab {
            display: none;
        }

        button:hover {
            opacity: 0.8;
        }

        .prevBtn {
            background-color: #8a9499;
            width: 108px;
        }

        .nextBtn {
            background-color: #1c75d8;
            width: 108px;
        }

        .Upload {
            background-color: #04AA6D;
        }

        /* Make circles that indicate the steps of the form: */
        .step {
            height: 15px;
            width: 15px;
            margin: 0 2px;
            background-color: #bbbbbb;
            border: none;
            border-radius: 50%;
            display: inline-block;
            opacity: 0.5;
        }

            .step.active {
                opacity: 1;
            }

            /* Mark the steps that are finished and valid: */
            .step.finish {
                background-color: #2296f3;
            }

        .WrapDiv {
            display: block;
            position: relative;
            font-size: 1.5rem;
            border: 2px #6c757d solid;
            text-align: center;
            padding: 3px;
            color: #6c757d;
            font-family: inherit;
            font-weight: 600;
            border-radius: 5px;
            width: 150px;
        }

        input[type=file] {
            width: 100%;
            z-index: 1;
            position: absolute;
            left: 0;
            opacity: 0;
        }

        .imageOverlay {
            background: rgba(245, 245, 245, 0.4);
            height: 20px;
            position: absolute;
            bottom: 0px;
            width: 100%;
            text-align: center;
            color: black;
            font-size: 12px;
            font-weight: 700;
        }

        .divImg {
            /*width: 150px;*/
            position: relative;
        }

            .divImg:hover {
                box-shadow: 0 0 20px #dddddd;
                cursor: pointer;
            }

        .rbl input[type="radio"] {
            margin-left: 10px;
            margin-right: 6px;
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Assign Employee"></asp:Label>
            </div>


        </div>

        <div id="divForm" runat="server" visible="false">

            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Assign <span class="Card-title-second">Employee </span></h4>
                </div>
            </div>


            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblUser" runat="server" Text="User"
                        CssClass="form-check-label"></asp:Label>
                    <span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlUser" runat="server" TabIndex="1"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="ddlUser" CssClass="rfvClr"
                        ErrorMessage="Select User" InitialValue="0" ValidationGroup="EmployeeMaster">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblBlock" runat="server" Text="Block"
                        CssClass="form-check-label"></asp:Label>
                    <span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlBlock" runat="server" TabIndex="2"
                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBlock_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="ddlBlock" CssClass="rfvClr"
                        ErrorMessage="Select Block" InitialValue="0" ValidationGroup="EmployeeMaster">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblFloor" runat="server" Text="Floor"
                        CssClass="form-check-label">  </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlFloor" runat="server" TabIndex="3"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                        ControlToValidate="ddlFloor" CssClass="rfvClr"
                        ErrorMessage="Select Floor" InitialValue="0" ValidationGroup="EmployeeMaster">
                    </asp:RequiredFieldValidator>
                </div>

            </div>

            <div class="row ">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblShift" runat="server" Text="Shift"
                        CssClass="form-check-label">  </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlShift" runat="server" TabIndex="4"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                        ControlToValidate="ddlShift" ValidationGroup="EmployeeMaster" CssClass="rfvClr"
                        ErrorMessage="Select Shift" InitialValue="0">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblDesignation" runat="server" Text="Designation"
                        CssClass="form-check-label">  </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlDesignation" runat="server" TabIndex="5"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                        ControlToValidate="ddlDesignation" ValidationGroup="EmployeeMaster" CssClass="rfvClr"
                        ErrorMessage="Select Designation" InitialValue="0">
                    </asp:RequiredFieldValidator>
                </div>

                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblEmpType" runat="server" Text="Employee Type"
                        CssClass="form-check-label"> </asp:Label><span class="spanStar">*</span>
                    <asp:DropDownList ID="ddlEmpType" runat="server" TabIndex="6"
                        CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                        ControlToValidate="ddlEmpType" ValidationGroup="EmployeeMaster" CssClass="rfvClr"
                        ErrorMessage="Select Employee Type" InitialValue="0">
                    </asp:RequiredFieldValidator>
                </div>

            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblDOJ" runat="server" Text="DOJ" CssClass="lblContent_Common myCal">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtDOJ" runat="server" CssClass="form-control fromDate" AutoComplete="off"
                        TabIndex="7"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="EmployeeMaster"
                        ControlToValidate="txtDOJ" CssClass="rfvClr" ErrorMessage="Enter DOJ"> </asp:RequiredFieldValidator>
                </div>

            </div>


            <div class="row p-4 justify-content-end">
                <asp:Button runat="server" CssClass="pure-material-button-contained nextBtn mr-2" ValidationGroup="EmployeeMaster" Text="Submit" ID="btnSubmit"
                    TabIndex="8" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" CssClass="pure-material-button-contained btnBgColorCancel" Text="Cancel"
                    TabIndex="9" CausesValidation="false" runat="server" Style="width: 108px;" OnClick="btnReset_Click" />
            </div>


        </div>

        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Assign <span class="Card-title-second">Employee </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%-- <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="add +" CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="ddwnoverflow">
                    <div class="table-responsive section">
                        <asp:GridView ID="gvEmpmaster" runat="server" AllowPaging="True"
                            DataKeyNames="userId" CssClass="gvv display" AutoGenerateColumns="false"
                            BorderStyle="None" PageSize="100">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UserName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvuserName" runat="server" Text='<%#Bind("userName") %>'></asp:Label>
                                        <asp:Label ID="lblgvuserId" runat="server" Text='<%#Bind("userId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvpassword" runat="server" Text='<%#Bind("password") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvemailId" runat="server" Text='<%#Bind("emailId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvphoneNumber" runat="server" Text='<%#Bind("phoneNumber") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvDOJ" runat="server" Text='<%#Bind("DOJ") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvempType" runat="server" Text='<%#Bind("empType") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvempDesignation" runat="server" Text='<%#Bind("empDesignation") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvuserRole" runat="server" Text='<%#Bind("userRole") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvempTypeName" runat="server" Text='<%#Bind("empTypeName") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvempDesignationName" runat="server" Text='<%#Bind("empDesignationName") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvemployeeId" runat="server" Text='<%#Bind("employeeId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvaddressId" runat="server" Text='<%#Bind("addressId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvaddress" runat="server" Text='<%#Bind("address") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvalternatePhoneNumber" runat="server" Text='<%#Bind("alternatePhoneNumber") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvdistrict" runat="server" Text='<%#Bind("district") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvstate" runat="server" Text='<%#Bind("state") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvcity" runat="server" Text='<%#Bind("city") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvparkingOwnerId" runat="server" Text='<%#Bind("parkingOwnerId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvpincode" runat="server" Text='<%#Bind("pincode") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvimageUrl" runat="server" Text='<%#Bind("imageUrl") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Name" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvbranchId" runat="server" Text='<%#Bind("branchId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvbranchName" runat="server" Text='<%#Bind("branchName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Block Name" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvblockId" runat="server" Text='<%#Bind("blockId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvblockName" runat="server" Text='<%#Bind("blockName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Floor Name" HeaderStyle-CssClass="gvHeader" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvfloorName" runat="server" Text='<%#Bind("floorName") %>'></asp:Label>
                                        <asp:Label ID="lblgvfloorId" runat="server" Text='<%#Bind("floorId") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shift" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvshiftId" runat="server" Text='<%#Bind("shiftId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvshiftName" runat="server" Text='<%#Bind("shiftName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Emp. Type" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvempTypeNameemp" runat="server" Text='<%#Bind("empTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvempDesignationNameemp" runat="server" Text='<%#Bind("empDesignationName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="LnkEdit"
                                            runat="server"
                                            Text="Edit"
                                            src="../../images/edit-icon.png" alt="image"
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

    </div>


    <input type="hidden" runat="server" id="hflatitude" />
    <input type="hidden" runat="server" id="hflongitude" />
    <asp:HiddenField runat="server" ID="hfImageCheckValue" Value="0" />

    <asp:HiddenField ID="hfImageUrl" runat="server" EnableViewState="true" />

    <asp:HiddenField ID="hfState" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfDistrict" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfCity" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfPassword" runat="server" />
    <asp:HiddenField ID="hfPrevImageLink" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfDOJ" runat="server" />


</asp:Content>

