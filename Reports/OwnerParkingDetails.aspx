<%@ Page Title="Owner Parking Details" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="OwnerParkingDetails.aspx.cs" Inherits="Reports_OwnerParkingDetails" %>

<asp:Content ID="FrmOwnerMaster" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
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
            font-weight: 500;
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
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Reports"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Owner Parking Details"></asp:Label>
            </div>

        </div>

        <div id="divForm" runat="server">

            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Owner <span class="Card-title-second">Parking Details</span></h4>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label
                                ID="lblParkingName"
                                runat="server"
                                Text="Owner / Parking Name"
                                CssClass="form-check-label">
                            </asp:Label>
                            <span class="spanStar">*</span>
                            <asp:DropDownList ID="ddlParkingName" runat="server" TabIndex="1" CssClass="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator
                                ID="rfvParkingName"
                                runat="server"                               
                                InitialValue="0"
                                ControlToValidate="ddlParkingName"
                                ValidationGroup="OwnerMaster"
                                CssClass="rfvClr"
                                ErrorMessage="Select Owner / Parking Name">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6 justify-content-end " style="margin-top: 2rem;">
                            <asp:Button
                                ID="btnSubmit"
                                runat="server"
                                Text="Submit"
                                TabIndex="2"
                                OnClick="btnSubmit_Click"
                                CausesValidation="true"
                                ValidationGroup="OwnerMaster"
                                CssClass="pure-material-button-contained btnBgColorAdd mr-3" />
                            <asp:Button
                                ID="btnCancel"
                                runat="server"
                                Text="Cancel"
                                TabIndex="3"
                                OnClick="btnCancel_Click"
                                CausesValidation="false"
                                CssClass="pure-material-button-contained btnBgColorCancel" />
                        </div>
                    </div>
                </div>

            </div>


        </div>
        <div id="divGv" runat="server">
            <div class="table-responsive section">
                <asp:GridView
                    ID="gvBranchList"
                    runat="server"
                    Visible="true"
                    AutoGenerateColumns="false"
                    DataKeyNames="parkingOwnerId"
                    BorderStyle="None"
                    CssClass="gvv display">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Parking Owner Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvParkingOwnerId"
                                    runat="server"
                                    Text='<%#Bind("parkingOwnerId") %>'>
                                </asp:Label>
                                <asp:Label
                                    ID="lblGvParkingName"
                                    runat="server"
                                    Text='<%#Bind("parkingName") %>'></asp:Label>
                                <asp:Label
                                    ID="lblGvBranchId"
                                    runat="server"
                                    Text='<%#Bind("branchId") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch Name">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvbranchName"
                                    runat="server"
                                    Text='<%#Bind("branchName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="City">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvCity"
                                    runat="server"
                                    Text='<%#Bind("city") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Phone No.">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvphoneNumber"
                                    runat="server"
                                    Text='<%#Bind("phoneNumber") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Approval Status">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvUserName"
                                    runat="server"
                                    Text='<%#Eval("approvalStatus").ToString() == "A" ? "Approved" : "Waiting List" %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
