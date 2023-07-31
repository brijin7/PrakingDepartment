<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:300,400&display=swap" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css" integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@600;700&display=swap" rel="stylesheet" />

    <link rel="stylesheet" href="fonts/icomoon/style.css" />
    <link rel="stylesheet" href="css/owl.carousel.min.css" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <!-- Style -->
    <link href="Style/login.css" rel="stylesheet" />
    <title>Login </title>
     <link href="fav.ico" rel="shortcut icon" type="image/x-icon" />
    <style>
        .rfvClr {
            color: red;
            font-size: 0.7rem;
        }

        hr {
            margin-top: 2rem;
            margin-bottom: 2rem;
            border: 0;
            border-top: 1px solid rgb(28 183 253 / 68%);
            width: 325px !important;
            margin-inline: auto;
        }

        .btnResnd {
            width: auto;
            font-size: 12px;
            font-weight: bold;
            padding: 12px 12px;
            letter-spacing: 1px;
            text-transform: uppercase;
            transition: transform 80ms ease-in;
            border-radius: 10px;
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <script lang="javascript" type="text/javascript">

        function successalert(sMsg) {
            swal({
                title: 'PayPre Parking',
                text: sMsg,
                icon: "success"
            });
        }
        function infoalert(sMsg) {
            swal({
                title: 'PayPre Parking',
                text: sMsg,
                icon: "info"
            });
        }
        function erroralert(sMsg) {
            swal({
                title: 'PayPre Parking',
                text: sMsg,
                icon: "error"
            });
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        // Disable F5 Key

        $(document).ready(function () {
            function disableF5(e) {
                if (e.keyCode == 116 || e.keyCode == 17) e.preventDefault();
            };
            $(document).on("keydown", disableF5);
        });

        // Back Button Disable

        $(document).ready(function () {
            history.pushState(null, null, location.href);
            window.onpopstate = function () {
                history.go(1);
            };
        });

        function SendOtp() {
            let seconds = 31;
            let button = document.querySelector('#<%=btnResend.ClientID%>');
            let buttonConfrm = document.getElementById('<%=btnCfmOtp.ClientID %>');

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
                    seconds = 31;
                    buttonConfrm.style.display = 'none';
                    document.getElementById('<%=hfOtp.ClientID %>').value = "";
                    button.value = "ReSend OTP";
                    clearInterval(cancel);
                    button.disabled = false;
                }
            }
            var cancel = setInterval(incrementSeconds, 1000);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <div class="row">
            <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <img class="img-style" src="images/loginbg.png" />
                <div class="row">
                    <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="container" id="divLogin" runat="server">
                            <div class="form-container sign-in-container">
                                <div class="row mt-5">
                                    <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                        <h1 class="card-title">Sign In </h1>
                                        <span>or use your account</span>
                                    </div>
                                    <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 social-container" style="display:none">
                                        <a href="#" class="social"><i class="fab fa-facebook-f"></i>&nbsp  </a>
                                        <a href="#" class="social"><i class="fab fa-google-plus-g"></i>&nbsp  </a>
                                    </div>
                                </div>

                                <div class="row inp-style mt-5">
                                    <div class="col-12">
                                        <label for="inp" class="inp">
                                            <asp:TextBox ID="txtLgnUserName" runat="server" placeholder="&nbsp;"></asp:TextBox>
                                            <span class="label">User Name *</span>
                                            <span class="focus-bg"></span>
                                            <asp:RequiredFieldValidator ID="rfvuserid" runat="server" Style="color: red; font-size: 0.8rem;"
                                                ValidationGroup="Login" ControlToValidate="txtLgnUserName" ErrorMessage="Enter User Name">
                                            </asp:RequiredFieldValidator>
                                        </label>

                                    </div>
                                    <div class="col-12">
                                        <label for="inp" class="inp">
                                            <asp:TextBox ID="txtLgnPassword" runat="server" placeholder="&nbsp;" TextMode="Password"></asp:TextBox>
                                            <span class="label">Password *</span>
                                            <span class="focus-bg"></span>
                                            <asp:RequiredFieldValidator ID="rfvpassword" runat="server" Style="color: red; font-size: 0.8rem;"
                                                ValidationGroup="Login" ControlToValidate="txtLgnPassword" ErrorMessage="Enter Password">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="justify-content-center mb-3">
                                        <asp:Button ID="btnLogin" CssClass="BtnLogin" runat="server" ValidationGroup="Login"
                                            Text="Login" OnClick="btnLogin_Click" />
                                    </div>
                                    <div class="mb-5">
                                        <asp:LinkButton ID="lnkbtnFgPwd" runat="server"
                                            Text="Forgot password?" OnClick="lnkbtnFgPwd_Click" />
                                        <%-- <a href="#">Forgot password?</a>--%>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="container" id="divFgPwd" runat="server" visible="false">
                            <div class="form-container sign-in-container">
                                <div class="row mt-5">
                                    <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                        <h1 class="card-title">Forgot Password? </h1>
                                       
                                        
                                        <span style=" margin: 6px 16px 0px 16px; display: inline-flex;">
                                            To reset your password, Enter email address used for PayPre-Parking
                                        </span>
<%--                                        <span style=" margin: 6px 16px 0px 16px; display: inline-flex;">Enter the email address you use for Pre-Parking and a OTP will be sent to you,
                                         to reset your password. </span>--%>
                                    </div>
                                </div>

                                <div class="row inp-style">
                                    <div class="col-12">
                                        <label for="inp" class="inp">
                                            <asp:TextBox ID="txtFgUserName" runat="server" placeholder="&nbsp;"></asp:TextBox>
                                            <span class="label">Email Id / Phone No. *</span>
                                            <span class="focus-bg"></span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="FgPwd"
                                                ControlToValidate="txtFgUserName" runat="server" CssClass="rfvClr"
                                                ErrorMessage="Enter Email Id / Phone No.">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="justify-content-center mb-3">
                                        <asp:Button ID="btnSend" runat="server" CssClass="BtnOtp" ValidationGroup="FgPwd"
                                            Text="Send OTP" OnClick="btnSend_Click" />
                                    </div>
                                    <br />
                                    <hr />
                                    <div class="mb-5">
                                        <asp:LinkButton ID="lnkbtnBackToLogin" runat="server" CssClass="BtnLogin"
                                            Text="Return To Login" OnClick="lnkbtnBackToLogin_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="container" id="divCfmOtp" runat="server" visible="false">
                            <div class="form-container sign-in-container">
                                <div class="row mt-5">
                                    <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                        <h1 class="card-title">Confirm OTP </h1>
                                    </div>
                                </div>

                                <div class="row inp-style">
                                    <div class="col-12">
                                        <label for="inp" class="inp">
                                            <asp:TextBox ID="txtOtpUsername" runat="server" placeholder="&nbsp;" Enabled="false"></asp:TextBox>
                                            <span class="label">User Name</span>
                                            <span class="focus-bg"></span>
                                        </label>
                                    </div>
                                    <div class="col-12">
                                        <label for="inp" class="inp">
                                            <asp:TextBox ID="txtNewPassword" runat="server" placeholder="&nbsp;"></asp:TextBox>
                                            <span class="label">New Password *</span>
                                            <span class="focus-bg"></span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="OTP"
                                                ControlToValidate="txtNewPassword"
                                                runat="server" CssClass="rfvClr"
                                                ErrorMessage="Enter New Password">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </div>
                                    <div class="col-12">
                                        <label for="inp" class="inp">
                                            <asp:TextBox ID="txtOTP" runat="server" placeholder="&nbsp;"></asp:TextBox>
                                            <span class="label">Enter OTP *</span>
                                            <span class="focus-bg"></span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="OTP"
                                                ControlToValidate="txtOTP"
                                                runat="server" CssClass="rfvClr"
                                                ErrorMessage="Enter OTP sent to you">
                                            </asp:RequiredFieldValidator>
                                        </label>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="justify-content-center mb-3">
                                        <asp:Button ID="btnCfmOtp" runat="server" CssClass="BtnOtp" ValidationGroup="OTP"
                                            Text="Submit" OnClick="btnCfmOtp_Click" />
                                        <asp:Button ID="btnResend" runat="server" class="btn btn-primary btnResnd" Visible="false"
                                            OnClick="btnResend_Click" />
                                    </div>
                                    <br />
                                    <hr />
                                    <div class="mb-5">
                                        <asp:LinkButton ID="lnkbtnBck2login" runat="server" CssClass="BtnLogin"
                                            Text="Return To Login" OnClick="lnkbtnBackToLogin_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfOtp" runat="server" />
    </form>

</body>
</html>
