<%@ Page Title="" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="PO_Preference.aspx.cs" Inherits="PO_Preference" %>

<asp:Content ID="frmPO_Preference" ContentPlaceHolderID="MasterPage" runat="Server">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"
        integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />
    <style>
        .DisplyCard {
            position: fixed;
            text-align: center;
            height: 100%;
            width: 100%;
            top: 0;
            right: 0;
            left: 0;
            z-index: 9999999;
            background-color: #fffdfb;
            opacity: 0.8;
        }

        .DisplyCardPostion {
            border-width: 0px;
            position: fixed;
            width: 50%;
            height: auto;
            padding: 0px 50px 0px 50px;
            box-shadow: rgba(0, 0, 0, 0.56) 0px 10px 70px 4px;
            background-color: #ffffff;
            font-size: 40px;
            left: 35%;
            top: 11%;
            border-radius: 25px;
        }

        .EditDisplyCardPostion {
            border-width: 0px;
            position: fixed;
            width: 50%;
            height: auto;
            padding: 0px 50px 0px 50px;
            box-shadow: rgba(0, 0, 0, 0.56) 0px 10px 70px 4px;
            background-color: #ffffff;
            font-size: 40px;
            left: 35%;
            top: 5%;
            border-radius: 25px;
        }

        .Displaycard-title {
            font-size: 1.7rem;
            color: #1ca4ff;
            font-weight: 700;
            margin-bottom: 20px;
        }

        .Displaycard-sub {
            font-size: 1.5rem;
            font-weight: 600;
            background-color: #cde0f3;
            border-radius: 15px;
        }

        .Displaycard-subFloor {
            font-size: 1.2rem;
            font-weight: 900;
        }

        .Displaycard-Link {
            font-size: 1.5rem;
            font-weight: 700;
            color: #1ca4ff;
            margin-left: 0.3rem;
            padding: 1.5rem;
            margin: 1.9rem;
            transition: all 0.3s;
        }

        .Displaycard-LinkActive {
            font-size: 1.2rem;
            text-decoration: underline;
            background: #1ca4ff;
            color: #fff;
            border-radius: 20px;
            box-shadow: 0px 2px 4px -1px rgb(0 0 0 / 20%), 0px 4px 5px 0px rgb(0 0 0 / 14%), 0px 1px 10px 0px rgb(0 0 0 / 12%);
            font-weight: 700;
            padding: 0.3rem;
            margin: 0.2rem;
            transition: all 0.3s;
        }

        .DisplaycardImg {
            width: auto;
            height: 200px;
        }

        p {
            margin: revert !important;
        }

        .btnBookColor {
            background-color: rgb( 33, 150, 243);
        }

        .Icon {
            color: #e52a00;
        }

        .IconBack {
            color: white;
        }

        .fa-Size {
            font-size: 3rem;
        }

        .txtbox {
            width: 100px;
            border-style: solid;
            border-radius: 5px;
            border-color: #48b5fe;
        }

        .BtnOtp {
            border-radius: 10px;
            border: 1px solid #24b53c;
            background-color: #24b53c;
            color: #FFFFFF;
            font-size: 1.40rem !important;
            font-weight: bold;
            padding: 5px 5px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            width: auto;
            cursor: pointer;
        }

        .btnResnd {
            border-radius: 10px;
            border: 1px solid #007bff;
            background-color: #007bff;
            color: #FFFFFF;
            font-size: 1.40rem !important;
            font-weight: bold;
            padding: 5px 5px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            width: auto;
            cursor: pointer;
        }

        .lblSummary {
            width: 100%;
            font-size: 1.2rem;
            margin-bottom: 0;
            text-transform: capitalize;
            font-weight: 600;
        }

        .spanStar {
            font-weight: bold;
            color: red;
            margin-left: 1px;
            font-size: 15px;
        }

        .txtOtp {
            margin-left: 16px;
            margin-top: -2px;
            width: 146px;
            height: 28px !important;
        }
    </style>

    <div id="DivFirst" class="DisplyCard" runat="server" visible="true">
        <div class="EditDisplyCardPostion">
            <div class="row pt-4">
                <asp:LinkButton ID="lnkBtnBack" runat="server" Visible="false"
                    CssClass="pure-material-button-contained btn-dark" OnClick="btnback_Click">
                      <i class="fa-solid fa-chevron-left IconBack" ></i> Back</asp:LinkButton>
            </div>
            <div class="text-center" style="margin-top: -2.5rem;">
                <img class="DisplaycardImg" src="images/PO.png" />
            </div>
            <div class="text-center">
                <h4 class="Displaycard-title">Kindly spare some time to setup the application based on your preference
                </h4>
            </div>

            <div class="row text-center Displaycard-sub">
                <div class="col-12">
                    <p class="Displaycard-sub">1.Do you have more than one Block?</p>
                </div>
            </div>
            <div align="center">
                <asp:RadioButtonList ID="rbtnblockOption" CssClass="Displaycard-Link inline-rb" runat="server" onchange="ShowFloor()"
                    RepeatDirection="Horizontal">
                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                    <asp:ListItem Value="N">No</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div id="divfloor" runat="server" style="display: none">
                <div class="row text-center Displaycard-sub">
                    <div class="col-12">
                        <p class="Displaycard-sub">2.Are there multiple Floors in the Block?</p>
                    </div>
                </div>
                <div align="center">
                    <asp:RadioButtonList ID="rbtnfloorOption" OnChange="ShowFD()"
                        CssClass="Displaycard-Link inline-rb" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div id="DivfloorDetails" runat="server" style="margin-top: -15px; display: none">
                    <p class="Displaycard-subFloor">
                        We have &nbsp;&nbsp;
                        <asp:DropDownList ID="ddllFloorType"
                            CssClass="txtbox" runat="server" RepeatDirection="Horizontal">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        floor with &nbsp;&nbsp;
                        <asp:TextBox ID="txtsqft" runat="server" CssClass="txtbox" />&nbsp;&nbsp;
                        Sq.ft                       
                    </p>
                </div>
            </div>
            <%--                <div class="row text-center Displaycard-sub">
                    <div class="col-12">
                        <p class="Displaycard-sub">
                            3.Managing yourself or Hired
                            Employee ?
                        </p>
                    </div>
                </div>
                <div align="center">
                    <asp:RadioButtonList ID="rbtnemployeeOption" CssClass="Displaycard-Link inline-rb" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>--%>
            <div id="divSlot" runat="server" style="display: none">
                <div class="row text-center Displaycard-sub">
                    <div class="col-12">
                        <p>3.Do you allow the users to park vehicles  in the allotted place?</p>
                    </div>
                </div>
                <div align="center">
                    <asp:RadioButtonList ID="rbtnslotsOption" OnChange="Showbtn()"
                        CssClass="Displaycard-Link inline-rb" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div id="divOtpDetails" runat="server" visible="false">
                <div class="row ml-5 mb-4">
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 mt-2" style="display: flex;">
                        <asp:Label
                            ID="Label1"
                            runat="server"
                            CssClass="form-check-label"
                            Text="Mobile No."></asp:Label>

                        <asp:TextBox
                            ID="txtMobileNo" onkeypress="return isNumber(event);"
                            runat="server" TabIndex="8" Enabled="false"
                            CssClass="form-control txtOtp">
                        </asp:TextBox>
                    </div>
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6 col-xl-6 mb-2" id="divSendOtp" runat="server" visible="false"
                        style="display: inline-flex;">
                        <asp:Button ID="btnSend" runat="server" CssClass="BtnOtp" ValidationGroup="BookingSlot"
                            Text="Send OTP" OnClick="btnSend_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6 col-sm-6 col-md-6 col-lg-6 col-xl-6"
                        id="divEnterOtp" runat="server" visible="false">
                        <div style="display: inline-flex; margin-left: 4rem;">
                            <asp:Label
                                ID="lblOTP"
                                runat="server"
                                CssClass="form-check-label"
                                Text="Enter OTP">
                            </asp:Label><span class="spanStar">*</span>
                            <asp:TextBox
                                ID="txtOTP" onkeypress="return isNumber(event);"
                                runat="server" MaxLength="6"
                                TabIndex="8"
                                CssClass="form-control txtOtp">
                            </asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator3"
                            ValidationGroup="OTP"
                            ControlToValidate="txtOTP"
                            runat="server"
                            CssClass="rfvClr"
                            ErrorMessage="Enter OTP">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="col-6 col-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                        <div id="divResend" runat="server" visible="false" style="justify-content: space-between">
                            <asp:Button ID="btnCfmOtp" runat="server" CssClass="BtnOtp" ValidationGroup="OTP"
                                Text="Submit" OnClick="btnCfmOtp_Click" />
                            <asp:Button ID="btnResend" runat="server" Text="00:29" CssClass="btnResnd"
                                OnClick="btnResend_Click" />
                        </div>
                    </div>
                </div>

            </div>

            <div id="Divbtn" runat="server" class="row justify-content-center mt-4 pb-3" style="display: none;">
                <asp:Button ID="btnSubmit"
                    CssClass="pure-material-button-contained btnBookColor"
                    OnClick="btnSubmit_Click"
                    Text="Submit" runat="server" />
                <asp:Button ID="btnCancel"
                    CssClass="pure-material-button-contained btnBgColorCancel"
                    OnClick="btnCancel_Click"
                    Text="Cancel" runat="server" />
            </div>

        </div>
    </div>
    <div id="DivFinal" class="DisplyCard" runat="server" visible="false">
        <div class="DisplyCardPostion">
            <div class="row pt-4 pb-4">
                <asp:LinkButton ID="btnback" runat="server"
                    CssClass="pure-material-button-contained btn-dark" OnClick="btnback_Click">
                      <i class="fa-solid fa-chevron-left IconBack" ></i> Back</asp:LinkButton>
                <div class="col text-center">
                    <h4 class="card-title">Your Preference</h4>
                </div>
                <asp:LinkButton ID="btnEdit" runat="server" Visible="false"
                    CssClass="pure-material-button-contained btn-dark" OnClick="btnEdit_Click">
                      <i class="fa-solid fa-pen-to-square IconBack" ></i> Edit</asp:LinkButton>
            </div>

            <%--                <div class="row text-center Displaycard-sub">
                    <div class="col-12">
                        <p class="Displaycard-sub">1.Do you have more than one Branch?</p>
                    </div>
                </div>
                <div align="center">
                    <asp:Label CssClass="Displaycard-Link" runat="server" ID="lblBranch"></asp:Label>
                </div>--%>

            <div class="row text-center Displaycard-sub">
                <div class="col-12">
                    <p class="Displaycard-sub">1.Do you have more than one Block?</p>
                </div>
            </div>
            <div>
                <asp:Label CssClass="Displaycard-Link" runat="server" ID="lblBlock"></asp:Label>
            </div>
            <div class="row text-center Displaycard-sub">
                <div class="col-12">
                    <p class="Displaycard-sub">2.Are there multiple Floors in the Block?</p>
                </div>
            </div>
            <div>
                <asp:Label CssClass="Displaycard-Link" runat="server" ID="lblFloor"></asp:Label>
            </div>
            <%--                <div class="row text-center Displaycard-sub">
                    <div class="col-12">
                        <p class="Displaycard-sub">
                            4.Managing yourself or Hired
                            Employee ?
                        </p>
                    </div>
                </div>
                <div align="center">
                    <asp:Label CssClass="Displaycard-Link" runat="server" ID="lblEmp"></asp:Label>
                </div>--%>
            <div class="row text-center Displaycard-sub">
                <div class="col-12">
                    <p>3.Do you allow the users to park vehicles  in the allotted place?</p>
                </div>
            </div>
            <div align="center">
                <asp:Label CssClass="Displaycard-Link" runat="server" ID="lblslot"></asp:Label>
            </div>
            <div class="pb-4" id="DiVContactSAdmin" runat="server" visible="false">
                <div class="row text-center Displaycard-sub ">
                    <div class="col-12 ">
                        <p><i class="fa-solid fa-circle-exclamation Icon mr-2"></i><a>For Changes Contact Super Admin</a></p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function ShowFloor() {
            $('#<%= divfloor.ClientID %>').css("display", "block");
            $('#<%= divfloor.ClientID %>').style.transition = 'all 2s';
        }
        function ShowFD() {
            var selectedvalue = $('#<%= rbtnfloorOption.ClientID %> input:checked').val();

            $('#<%= divSlot.ClientID %>').css("display", "block");
            if (selectedvalue == 'N') {
                $('#<%= DivfloorDetails.ClientID %>').css("display", "block");

            }
            else {
                $('#<%= DivfloorDetails.ClientID %>').css("display", "none");
            }

        }

        function ShowSlot() {
            $('#<%= divSlot.ClientID %>').css("display", "block");
            $('#<%= divSlot.ClientID %>').style.transition = 'all 2s';
        }
        function Showbtn() {
            $('#<%= Divbtn.ClientID %>').css("display", "block");
            $('#<%= Divbtn.ClientID %>').style.transition = 'all 2s';
        }
    </script>
    <script type="text/javascript">
        function SendOtp() {
            let seconds = 29;
            let button = document.querySelector('#<%=btnResend.ClientID%>');
            let buttonConfrm = document.getElementById('<%=btnCfmOtp.ClientID %>');
            let divEnterOtp = document.getElementById('<%=divEnterOtp.ClientID %>');

            function incrementSeconds() {

                seconds = seconds - 1;
                if (seconds < 10) {
                    button.value = '00:0' + seconds;
                    button.disabled = true;
                }
                else {
                    button.value = '00:' + seconds;
                    button.disabled = true;
                }
                if (seconds == 0) {
                    seconds = 29;
                    button.value = "ReSend";
                    clearInterval(cancel);
                    button.disabled = false;
                }
            }
            var cancel = setInterval(incrementSeconds, 1000);
        }

    </script>
</asp:Content>

