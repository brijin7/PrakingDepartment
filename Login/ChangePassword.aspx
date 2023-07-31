<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="Login_ChangePassword" %>

<asp:Content ID="CntChangePassword" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .nextBtn {
            background-color: #1c75d8;
            width: 108px;
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
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Change Password"></asp:Label>
            </div>

        </div>
        <div id="divForm" runat="server">
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblOldPassword" runat="server" Text="Old Password"
                        CssClass="lblContent_Common">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form-control" AutoComplete="off"
                        TabIndex="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server"
                        ControlToValidate="txtOldPassword" ValidationGroup="Password" CssClass="rfvClr"
                        ErrorMessage="Enter Old Password">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblNewPassword" runat="server" Text="New Password"
                        CssClass="lblContent_Common">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" AutoComplete="off"
                        TabIndex="2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="Rfv2" runat="server"
                        ControlToValidate="txtNewPassword" ValidationGroup="Password" CssClass="rfvClr"
                        ErrorMessage="Enter New Password">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row p-4 justify-content-end">
                <asp:Button runat="server" CssClass="pure-material-button-contained nextBtn mr-2" OnClick="btnSubmit_Click" ValidationGroup="Password"
                    Text="Update" ID="btnSubmit" TabIndex="3" />
                <asp:Button ID="btnReset" OnClick="btnReset_Click" CssClass="pure-material-button-contained btnBgColorCancel" Text="Cancel"
                    TabIndex="4" CausesValidation="false" runat="server" Style="width: 108px;" />
            </div>
        </div>
    </div>
</asp:Content>

