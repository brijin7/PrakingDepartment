<%@ Page Title="Menu Access Rights" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="MenuAccessRights.aspx.cs"
    Inherits="Master_MenuAccessRights" EnableEventValidation="false" %>

<asp:Content ID="FrmEmployeeMaster" ContentPlaceHolderID="MasterPage" runat="Server">

    <script type="text/javascript">

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
    <style>
        label {
            font-weight: 500;
            color: black !important;
            font-size: 1.3rem;
        }

        .panel-group .panel + .panel {
            margin-top: 5px;
        }

        .panel-group .panel {
            margin-bottom: 0;
            border-radius: 4px;
        }

        .panel-success {
            border: 1px;
            border-color: #d6e9c6;
        }

        .panel {
            margin-bottom: 5px;
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px;
            -webkit-box-shadow: 0 1px 1px rgba(0,0,0,.05);
            box-shadow: 0 1px 1px rgba(0,0,0,.05);
        }

        .panel-success > .panel-heading {
            color: #1ca4ff;
            background-color: #d8e9f0;
            border-color: #c6cee9;
            font-size: 18px;
            font-weight: 800;
            text-align: left;
        }


        .panel-group .panel-heading {
            border-bottom: 0;
        }

        .panel-heading {
            padding: 5px 5px;
            border-bottom: 1px solid transparent;
            border-top-left-radius: 3px;
            border-top-right-radius: 3px;
        }

        .panel-body {
            padding: 0px;
            border: 1px solid #c6e1e9;
        }

        .lblChrg {
            padding: 10px 10px;
        }

        .chkChoi {
            color: white !important;
        }

        .chkChoice input {
            margin-right: 5px;
        }

        .holepagenames {
            padding: 8px;
            border-radius: 2px;
            cursor: pointer;
            transition: 0.2s all linear;
            border: 1px solid #fff;
            overflow: scroll;
            max-height:475px;
                 
        }

        .holepagenamesdef {
            background: #daeefd;
            color: #000000;
        }

        .holepagenamesorg {
            background: #ffffff;
            color: #000000;            
        }

        .holepagenamesdarkpurp {
            background: #d5d5fd;
            color: #000000;
        }

        .holepagenamespurp {
            background: #f1dfff;
            color: #000000;
        }

        .holepagenamesgre {
            background: #f5ffdf;
            color: #000000;
        }

        .dividerprpage {
            border-bottom: 1px dashed #000;
            width: 15px;
            /* height: 5px; */
            position: absolute;
            margin-top: -30px;
            margin-left: -15px;
        }

            .dividerprpage:last-child::after {
                background: #eee;
                content: '';
                position: absolute;
                width: 2px;
                height: 20px;
                margin-top: 2px;
                margin-left: -2px;
            }

        table.gvv th {
            text-align: left;
            background: linear-gradient(to bottom, #2196f3 0%, #3280c0 100%);
            color: #fff;
        }

        .highlight {
            width: 100%;
            background-color: #eaba93;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function SingleCheckBox(val, i) {
            ;
            var gvcheck = document.getElementById('<%=gvOption.ClientID %>');
            var totalRowCount = $("[id*=gvOption] tr").length;
            var rowCount = $("[id*=gvOption] td").closest("tr").length;
            console.log($('input:checkbox:checked').length);
            console.log($('input:checkbox').length);
            var gvcheck = document.getElementById('<%=gvOption.ClientID %>');
            var inputs = gvcheck.rows[0].getElementsByTagName('input');
            console.log(inputs[i].checked);
          
            if (i == 1) {
                if ($("table[id*=gvOption] input[type=checkbox][id*=AddCheckBox1]:checked").length != rowCount) {
                    inputs[i].checked = false;

                }
                else {
                    inputs[i].checked = true;
                }

            }
            if (i == 2) {
                if ($("table[id*=gvOption] input[type=checkbox][id*=EditCheckBox1]:checked").length != rowCount) {
                    inputs[i].checked = false;
                }
                else {
                    inputs[i].checked = true;
                }

            }
            if (i == 3) {
                if ($("table[id*=gvOption] input[type=checkbox][id*=ViewCheckBox1]:checked").length != rowCount) {
                    inputs[i].checked = false;
                }
                else {
                    inputs[i].checked = true;
                }

            }
            if (i == 4) {
                if ($("table[id*=gvOption] input[type=checkbox][id*=DeleteCheckBox1]:checked").length != rowCount) {
                    inputs[i].checked = false;
                }
                else {
                    inputs[i].checked = true;
                }

            }
            if (i == 0) {
                var $id = $(val).closest("tr");
                var id = $id.index();
                var inputss = gvcheck.rows[id].getElementsByTagName('input');
                var uncheckheader = gvcheck.rows[0].getElementsByTagName('input');
                if (inputss[0].checked == true) {
                    inputss[1].checked = true;
                    inputss[1].disabled = false;
                    inputss[2].checked = true;
                    inputss[2].disabled = false;
                    inputss[3].checked = true;
                    inputss[3].disabled = false;
                    inputss[4].checked = true;
                    inputss[4].disabled = false;
                    if ($("table[id*=gvOption] input[type=checkbox][id*=AddCheckBox1]:checked").length != rowCount) {
                        uncheckheader[0].checked = false;

                    }
                    else {
                        uncheckheader[0].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=AddCheckBox1]:checked").length != rowCount) {
                        uncheckheader[1].checked = false;

                    }
                    else {
                        uncheckheader[1].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=EditCheckBox1]:checked").length != rowCount) {
                        uncheckheader[2].checked = false;
                    }
                    else {
                        uncheckheader[2].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=ViewCheckBox1]:checked").length != rowCount) {
                        uncheckheader[3].checked = false;
                    }
                    else {
                        uncheckheader[3].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=DeleteCheckBox1]:checked").length != rowCount) {
                        uncheckheader[4].checked = false;
                    }
                    else {
                        uncheckheader[4].checked = true;
                    }

                }
                else {
                    inputss[1].checked = false;
                    inputss[2].checked = false;
                    inputss[3].checked = false;
                    inputss[4].checked = false;
                    inputss[1].disabled = true;
                    inputss[2].disabled = true;
                    inputss[3].disabled = true;
                    inputss[4].disabled = true;
                    if ($("table[id*=gvOption] input[type=checkbox][id*=AddCheckBox1]:checked").length != rowCount) {
                        uncheckheader[0].checked = false;

                    }
                    else {
                        uncheckheader[0].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=AddCheckBox1]:checked").length != rowCount) {
                        uncheckheader[1].checked = false;

                    }
                    else {
                        uncheckheader[1].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=EditCheckBox1]:checked").length != rowCount) {
                        uncheckheader[2].checked = false;
                    }
                    else {
                        uncheckheader[2].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=ViewCheckBox1]:checked").length != rowCount) {
                        uncheckheader[3].checked = false;
                    }
                    else {
                        uncheckheader[3].checked = true;
                    }
                    if ($("table[id*=gvOption] input[type=checkbox][id*=DeleteCheckBox1]:checked").length != rowCount) {
                        uncheckheader[4].checked = false;
                    }
                    else {
                        uncheckheader[4].checked = true;
                    }
                   
                }
            }


        }

        function SelectAllCheckboxes(chk, selector) {
            $('#<%=gvOption.ClientID%>').find(selector + " input:checkbox").each(function () {
                ;
                $(this).prop("checked", $(chk).prop("checked"));
              
            });
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Menu Access Rights"></asp:Label>
            </div>


        </div>

        <div id="divForm" runat="server" visible="false">

            <div class="divTitle row pt-2 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Menu Access <span class="Card-title-second">Rights </span></h4>
                </div>
            </div>

            <div id="divEntry" runat="server" style="margin-top: -14px;">
                <div class="row" runat="server">
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblUser" runat="server" Text="User Name"
                            CssClass="form-check-label"></asp:Label>
                        <span class="spanStar">*</span>
                        <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control" TabIndex="1" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlUserName"
                            ValidationGroup="Employee" CssClass="rfvClr" SetFocusOnError="True" InitialValue="0">Select User Name</asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 p-3 mt-3 justify-content-end">
                        <asp:Button runat="server" CssClass="pure-material-button-contained btnBgColorAdd" Text="Submit" ID="btnSubmit"
                            TabIndex="2" OnClick="btnSubmit_Click" ValidationGroup="Employee" Style="margin-right: 5px;" />
                        <asp:Button ID="btnReset" CssClass="pure-material-button-contained btnBgColorCancel" Text="Cancel"
                            TabIndex="3" CausesValidation="false" runat="server" OnClick="btnReset_Click" />
                    </div>

                </div>

                <div class="row" runat="server" id="divPages">

                    <div class="col-sm-12 col-xs-12">
                        <div class="panel panel-success">
                            <div class="panel-heading panelheadchk">
                                Menu Rights  <span class="spanStar">*</span>
                            </div>
                            <div class="panel-body">
                                <div class="row p-0 m-0">
                                    <div class="holepagenames holepagenamesorg col-sm-12 col-xs-12 section" id="divBooking" runat="server">
                                        <asp:GridView ID="gvOption" runat="server"
                                            Width="100%" CssClass="gvv" AutoGenerateColumns="false"
                                            BorderStyle="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Option Name" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbloptionId" runat="server" Text='<%# Bind("optionId") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                        <asp:Label ID="lbloptionName" runat="server" Text='<%# Bind("optionName") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-CssClass="grdHead" Visible="true">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblFormRights" runat="server" Text="Form"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="CheckBox2" runat="server" onclick="javascript:SelectAllCheckboxes(this,'.FormRights');" Style="display:none" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" CssClass="FormRights" onclick="javascript:SingleCheckBox(this,0);"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="grdHead" Visible="true">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblAddrights" runat="server" Text="Add"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="AddCheckBox2" runat="server" onclick="javascript:SelectAllCheckboxes(this,'.Addrights');"  Style="display:none" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="AddCheckBox1" runat="server" CssClass="Addrights" onclick="javascript:SingleCheckBox(this,1);" Enabled="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="grdHead" Visible="true">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblEditrights" runat="server" Text="Edit"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="EditCheckBox2" runat="server" onclick="javascript:SelectAllCheckboxes(this,'.Editrights');"   Style="display:none"/>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="EditCheckBox1" runat="server" CssClass="Editrights" onclick="javascript:SingleCheckBox(this,2);" Enabled="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="grdHead" Visible="true">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblViewrights" runat="server" Text="View"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="ViewCheckBox2" runat="server" onclick="javascript:SelectAllCheckboxes(this,'.Viewrights');"  Style="display:none" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ViewCheckBox1" runat="server" CssClass="Viewrights" onclick="javascript:SingleCheckBox(this,3);" Enabled="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="grdHead" Visible="true">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDeleterights" runat="server" Text="Delete"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="DeleteCheckBox2" runat="server" onclick="javascript:SelectAllCheckboxes(this,'.Deleterights');" Style="display:none"  />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="DeleteCheckBox1" runat="server" CssClass="Deleterights" onclick="javascript:SingleCheckBox(this,4);" Enabled="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                            </Columns>
                                            <AlternatingRowStyle CssClass="alt" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </div>




        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Menu Access <span class="Card-title-second">Rights </span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>

                </div>
            </div>
          
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="ddwnoverflow">
                    <div class="table-responsive section">
                        <asp:GridView ID="gvMenuAccess" runat="server" AllowPaging="True"
                            CssClass="gvv display" AutoGenerateColumns="false"
                            BorderStyle="None" PageSize="100" OnRowDataBound="gvMenuAccess_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvuserId" runat="server" Text='<%#Bind("userId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgvuserName" runat="server" Text='<%#Bind("userName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User Role" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblgvEmphoneNumber"
                                            runat="server"
                                            Text='<%#Eval("userRole").ToString().Trim() == "E" ? "Employee":"Employee"  %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OptionDetails" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:DataList runat="server" ID="DataList1" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloptionId" runat="server" Text='<%# Bind("optionId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                <asp:Label ID="lbloptionName" runat="server" Text='<%# Bind("optionName") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblmenuOptionActiveStatus" runat="server" Text='<%# Bind("menuOptionActiveStatus") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblMenuOptionAccessId" runat="server" Text='<%# Bind("MenuOptionAccessId") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblMenuOptionAccessActiveStatus" runat="server" Text='<%# Bind("MenuOptionAccessActiveStatus") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopAddRights" runat="server" Text='<%# Bind("AddRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopEditRights" runat="server" Text='<%# Bind("EditRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopViewRights" runat="server" Text='<%# Bind("ViewRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopDeleteRights" runat="server" Text='<%# Bind("DeleteRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>

                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="LnkEdit"
                                            runat="server" OnClick="LnkEdit_Click"
                                            src="../images/edit-icon.png" alt="image" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True"
                            CssClass="gvv display" AutoGenerateColumns="false"
                            BorderStyle="None" PageSize="100" OnRowDataBound="GridView1_RowDataBound" Visible="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgv1userId" runat="server" Text='<%#Bind("userId") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblgv1userName" runat="server" Text='<%#Bind("userName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User Role" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblgvEmphoneNumber1"
                                            runat="server"
                                            Text='<%#Eval("userRole") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="OptionDetails" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:DataList runat="server" ID="DataList12" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloptionId1" runat="server" Text='<%# Bind("optionId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                <asp:Label ID="lbloptionName1" runat="server" Text='<%# Bind("optionName") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblmenuOptionActiveStatus1" runat="server" Text='<%# Bind("menuOptionActiveStatus") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblMenuOptionAccessId1" runat="server" Text='<%# Bind("MenuOptionAccessId") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblMenuOptionAccessActiveStatus1" runat="server" Text='<%# Bind("MenuOptionAccessActiveStatus") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopAddRights1" runat="server" Text='<%# Bind("AddRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopEditRights1" runat="server" Text='<%# Bind("EditRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopViewRights1" runat="server" Text='<%# Bind("ViewRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblopDeleteRights1" runat="server" Text='<%# Bind("DeleteRights") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>

                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

    </div>


</asp:Content>

