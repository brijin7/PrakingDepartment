<%@ Page Title="Printing Instructions" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="PrintingInstructions.aspx.cs" Inherits="Master_PrintingInstructions" %>

<asp:Content ID="frmPrintingInstructions" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
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

        .holepagenames {
            padding: 8px;
            border-radius: 2px;
            cursor: pointer;
            transition: 0.2s all linear;
            border: 1px solid #fff;
            overflow: scroll;
            max-height: 475px;
        }

        .holepagenamesorg {
            background: #ffffff;
            color: #000000;
        }

        table.gvv th {
            text-align: left;
            background: linear-gradient(to bottom, #2196f3 0%, #3280c0 100%);
            color: #fff;
        }

        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 25px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 18px;
                width: 18px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .tooltiptext {
            visibility: hidden;
            width: 160px;
            background-color: #1ca4ff;
            color: #fff;
            text-align: center;
            border-radius: 16px;
            padding: 5px 5px;
            /* Position the tooltip */
            position: absolute;
            z-index: 1;
            bottom: 100%;
            left: 50%;
            margin-left: -80px;
        }

            .tooltiptext::after {
                content: "";
                position: absolute;
                top: 100%;
                left: 50%;
                margin-left: -5px;
                border-width: 5px;
                border-style: solid;
                border-color: #1ca4ff transparent transparent transparent;
            }

        .switch:hover .tooltiptext {
            visibility: visible;
        }
    </style>
    <script>
        function togglecheck() {            
            var Checked = document.getElementById("<%= togglecheck.ClientID %>").checked;
            var divGnrlIns = document.getElementById("divGnrlIns");
            var InsTxtBox = document.getElementById("InsTxtBox");
            if (Checked == false) {
                divGnrlIns.style.display = "none";
                InsTxtBox.style.display = "block";
                document.getElementById("<%= rfdinstructions.ClientID %>").enabled = true;
            } else {
                divGnrlIns.style.display = "block";
                InsTxtBox.style.display = "none";
                document.getElementById("<%= rfdinstructions.ClientID %>").enabled = false;
            }
        }

        function SingleCheckBox(val) {
            var gvcheck = document.getElementById('<%=gvGeneralsIns.ClientID %>');
            console.log($('input:checkbox:checked').length);
            console.log($('input:checkbox').length);
            var gvcheck = document.getElementById('<%=gvGeneralsIns.ClientID %>');
            var inputs = gvcheck.rows[0].getElementsByTagName('input');
            console.log(inputs[0].checked);
            if (($('input:checkbox:checked').length == $('input:checkbox').length ||
                $('input:checkbox:checked').length == $('input:checkbox').length - 1)
                && inputs[0].checked == false) {
                inputs[0].checked = true;
            }
            else {
                inputs[0].checked = false;
            }
        }
        function SelectAllCheckboxes(chk, selector) {
            $('#<%=gvGeneralsIns.ClientID%>').find(selector + " input:checkbox").each(function () {
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol"
                    Text="Printing Instructions"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Printing <span class="Card-title-second">Instructions </span></h4>
                </div>
                <div class="justify-content-end" id="divToggle" runat="server">
                    <%--  General Instructions--%>
                    <label class="switch">
                        <asp:CheckBox runat="server" Checked="false" ID="togglecheck" onclick="togglecheck()" />
                        <span class="slider round"></span>
                        <span class="tooltiptext mb-2">Click for General Instructions</span>
                    </label>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label
                        ID="lblinstructionType"
                        runat="server"
                        CssClass="form-check-label"
                        Text="Instruction Type">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:RadioButtonList
                        ID="rbtnlinstructionType"
                        runat="server"
                        CssClass="inline-rb"
                        TabIndex="1"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="rbtnlinstructionType_SelectedIndexChanged"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Text="Receipt" Selected="True" Value="R"></asp:ListItem>
                        <asp:ListItem Text="Pass" Value="P"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator
                        ID="rfdinstructionType"
                        ValidationGroup="PrintMaster"
                        ControlToValidate="rbtnlinstructionType"
                        runat="server"
                        CssClass="rfvClr"
                        ErrorMessage="Select Instruction Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8" id="InsTxtBox">
                    <asp:Label
                        ID="lblinstructions"
                        runat="server"
                        CssClass="form-check-label"
                        Text="Instructions">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtinstructions"
                        runat="server"
                        TabIndex="2" TextMode="MultiLine"
                        CssClass="form-control ">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfdinstructions"
                        ValidationGroup="PrintMaster"
                        ControlToValidate="txtinstructions"
                        runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter Instructions">
                    </asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row" id="divGnrlIns" style="display: none;">
                <div class="col-sm-12 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading panelheadchk">
                            General Printing Instructions
                        </div>
                        <div class="panel-body">
                            <div class="row p-0 m-0">
                                <div class="holepagenames holepagenamesorg col-sm-12 col-xs-12 section" id="divBooking" runat="server">
                                    <asp:GridView ID="gvGeneralsIns" runat="server"
                                        Width="100%" CssClass="gvv" AutoGenerateColumns="false"
                                        BorderStyle="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader"  Visible="false">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="General Instructions" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblinstructions" runat="server" Text='<%# Bind("instructions") %>' Font-Bold="true" ></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-CssClass="grdHead" Visible="true">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblFormRights" runat="server" Text="Select All"></asp:Label>                                         
                                                    <asp:CheckBox ID="CheckBox2" runat="server" onclick="javascript:SelectAllCheckboxes(this,'.FormRights');" />
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="center" Width="10px"/>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox1" runat="server" CssClass="FormRights" onclick="javascript:SingleCheckBox(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" Width="10px" />
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

            <div class="row p-4 justify-content-end">
                <div class="mr-3">
                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit"
                        OnClick="btnSubmit_Click"
                        ValidationGroup="PrintMaster"
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
                    <h4 class="card-title">Printing <span class="Card-title-second">Instructions</span></h4>
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
                        ID="gvPrintingInstructions"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="uniqueId"
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
                            <asp:TemplateField HeaderText="Branch Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvbranchName"
                                        runat="server"
                                        Text='<%#Bind("branchName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Instruction Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvinstructionType"
                                        runat="server"
                                        Text='<%#Eval("instructionType").ToString() =="P"?"Pass":"Receipt"%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Instructions">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvinstructions"
                                        runat="server"
                                        Text='<%#Bind("instructions") %>'></asp:Label>
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
                                        runat="server"
                                        src="../../images/edit-icon.png" alt="image"
                                        Text="Edit" OnClick="LnkEdit_Click"
                                        Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' />
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
    <asp:HiddenField ID="hfuniqueId" runat="server" />
</asp:Content>

