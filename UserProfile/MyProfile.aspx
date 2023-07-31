<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="MyProfile.aspx.cs"
    Inherits="Master_MyProfile" EnableEventValidation="false" %>

<asp:Content ID="FrmMyProfile" ContentPlaceHolderID="MasterPage" runat="Server">

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

        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 25px;
            margin-left: 11rem;
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
            width: 140px;
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
            margin-left: -70px;
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
            var divImgCapture = document.getElementById("divImgCapture");
            if (Checked == false) {
                divImgCapture.style.display = "none";
            } else {
                divImgCapture.style.display = "block";
            }
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
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="My Profile"></asp:Label>
            </div>

        </div>

        <div id="divForm" runat="server">

            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Basic <span class="Card-title-second">Info</span></h4>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8 ">
                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblEmployeename" runat="server" Text="User Name"
                                CssClass="lblContent_Common">
                            </asp:Label><span class="spanStar">*</span>
                            <asp:TextBox ID="txtEmployeename" runat="server" CssClass="form-control" AutoComplete="off"
                                TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblPassword" runat="server" Text="Password"
                                CssClass="lblContent_Common"> </asp:Label>
                            <span class="spanStar">*</span>
                            <div class="input-group-append">
                                <asp:TextBox ID="txtPassword" runat="server"
                                    TabIndex="2" CssClass="form-control passBox1" TextMode="Password">  </asp:TextBox>

                                <div class="input-group-prepend">
                                    <div class="input-group-text p-0">
                                        <button id="show_password" class="btn btn-light pt-1" type="button" style="height: 28px; width: 40px;">
                                            <span class="fa fa-eye icons p-0"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="EmployeeMaster1"
                                ControlToValidate="txtPassword" CssClass="rfvClr" ErrorMessage=" Enter Password"> </asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblPhoneNo" runat="server" Text="Phone No."
                                CssClass="lblContent_Common">
                            </asp:Label><span class="spanStar">*</span>
                            <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control " AutoComplete="off"
                                TabIndex="3" onkeypress="return isNumber(event);" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                ControlToValidate="txtPhoneNo" ValidationGroup="EmployeeMaster1" CssClass="rfvClr"
                                ErrorMessage="Enter Phone No.">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ControlToValidate="txtPhoneNo" ErrorMessage="Invalid Phone No."
                                ValidationExpression="[0-9]{10}" CssClass="rfvClr" ValidationGroup="EmployeeMaster1"></asp:RegularExpressionValidator>

                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblEmail" runat="server" Text="Email"
                                CssClass="form-check-label">
                            </asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server"
                                TabIndex="4" CssClass="form-control" AutoComplete="off">  </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail"
                                SetFocusOnError="True" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                Display="Dynamic" CssClass="rfvClr" ErrorMessage="Invalid email address" ValidationGroup="EmployeeMaster1" />

                        </div>
                    </div>
                    <div class="row" runat="server" id="divUserDetails" title="Address Info">
                        <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                            <asp:Label ID="lblAddress" runat="server" Text="Address Line 1"
                                CssClass="lblContent_Common">
                            </asp:Label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control " AutoComplete="off"
                                TabIndex="5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="txtAddress" CssClass="rfvClr" ErrorMessage="Enter Address" ValidationGroup="EmployeeeMaster">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblPincode" runat="server" Text="Pincode" CssClass="lblContent_Common"></asp:Label>
                            <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control " AutoComplete="off" MaxLength="6"
                                TabIndex="6" onkeypress="return isNumber(event);" onchange="myFunction()"></asp:TextBox>

                        </div>

                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblCity" runat="server" Text="City" CssClass="form-check-label" ForeColor="#1ca4ff"></asp:Label>
                            :
                    <asp:Label ID="txtCity" runat="server" Style="font-weight: bold;" CssClass="form-check-label"></asp:Label>
                            <br />
                            <asp:Label ID="lblDistrict" runat="server" Text="District" CssClass="form-check-label" ForeColor="#1ca4ff"></asp:Label>
                            :
                    <asp:Label ID="txtDistrict" runat="server" Style="font-weight: bold;" CssClass="form-check-label"></asp:Label>
                            <br />
                            <asp:Label ID="lblState" runat="server" Text="State" CssClass="form-check-label" ForeColor="#1ca4ff"></asp:Label>
                            :
                    <asp:Label ID="txtState" runat="server" Style="font-weight: bold;" CssClass="form-check-label"></asp:Label>
                        </div>


                    </div>

                </div>


                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div class="row panel panel-success">
                        <div class="panel-heading">
                            User Image  
                        </div>
                        <label id="Toggle" class="switch" style="text-align-last: end;">
                            <asp:CheckBox runat="server" Checked="false" ID="togglecheck" onclick="togglecheck()" />
                            <span class="slider round"></span>
                            <span class="tooltiptext mb-2">Click for Webcam</span>
                        </label>
                        <div class="panel-body">
                            <asp:FileUpload ID="fupEmpLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgEmpPhotoPrev" runat="server" alt="User Image"
                                    ImageUrl="~/images/EmptyImage.png" Width="100%" />
                                <div id="Image1" runat="server" width="100%"></div>
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                            <div id="divImgCapture" style="display: none;">
                                <div class="row flex mt-4">
                                    <div id="webcamNew" runat="server"></div>
                                </div>
                                <div class="row mt-3">
                                    <input type="button" class="pure-material-button-contained text-black" value="Capture"
                                        onclick="take_snapshot()">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div class="row p-4 justify-content-end">
                <asp:Button runat="server" CssClass="pure-material-button-contained nextBtn mr-2" ValidationGroup="EmployeeMaster1"
                    Text="Update" ID="btnSubmit" TabIndex="7" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" CssClass="pure-material-button-contained btnBgColorCancel" Text="Cancel"
                    TabIndex="8" CausesValidation="false" runat="server" Style="width: 108px;" OnClick="btnReset_Click" />
            </div>
        </div>

    </div>

    <asp:HiddenField runat="server" ID="hfImageCheckValue" Value="0" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script type="text/javascript">
        function ShowImagePreview(input) {
            var fup = document.getElementById("<%=fupEmpLink.ClientID %>");
            var fileName = fup.value;
            var maxfilesize = 1024 * 1024;
            filesize = input.files[0].size;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" || ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG") {
                if (filesize <= maxfilesize) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#<%=imgEmpPhotoPrev.ClientID%>').prop('src', e.target.result);
                            $('#<%=hfImageCheckValue.ClientID%>').val("1");
                        };
                        reader.readAsDataURL(input.files[0]);
                        var formData = new FormData()
                        formData.append("image", $('input[type=file]')[0].files[0])

                        $.ajax({
                            url: '<%= Session["ImageUrl"].ToString() %>',
                            type: 'POST',
                            data: formData,
                            contentType: false,
                            processData: false,
                            success: function (result) {
                                $('#<%=hfImageUrl.ClientID%>').val(result.response);
                            },

                        });
                    }
                }
                else {
                    swal("Please, Upload image file less than or equal to 1 MB !!!");
                    fup.focus();
                    return false;
                }
            }
            else {
                swal("Please, Upload Gif, Jpg, Jpeg or Bmp Images only !!!");
                fup.focus();
                return false;
            }
        }

        $(function () {
            var fileupload = $('#<%=fupEmpLink.ClientID%>');
                var image = $('#divImgPreview');
                image.click(function () {
                    fileupload.click();
                });
            });
    </script>
    <!-- Webcam.min.js -->
    <script type="text/javascript" src="../js/webcam.min.js"></script>
    <script>
        $(function () {

            let toggle = document.getElementById("Toggle")

            if (toggle == null)
                return;

            Webcam.set({
                height: 150,
                width: 280,
                image_format: 'jpeg',
                jpeg_quality: 100
            });
            navigator.getUserMedia({
                video: true
            }, () => {
                Webcam.attach(<%=webcamNew.ClientID%>);
                toggle.style.display = "block";
            }, () => {
                toggle.style.display = "none";
            });
        });

        function take_snapshot() {
            Webcam.snap(function (data_uri) {
                document.getElementById('divImgPreview').innerHTML =
                    '<img src="' + data_uri + '"/>';
                $('#<%=hfImageUrl.ClientID%>').val(data_uri);
                    $.ajax({
                        url: '<%= Session["ImageUrl"].ToString() %>',
                        type: 'POST',
                        data: "{'" + $('#<%=hfImageUrl.ClientID%>').val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        processData: false,
                        success: function (data) {
                            $('#<%=hfImageUrl.ClientID%>').val(data.response);
                        },
                    });
                });


        }
    </script>

    <script>
        /******************************ZipCode*******************************/

        //document.getElementById("txtPincode").addEventListener("change", myFunction);
        function myFunction() {
            var NewArea = $('[id*=txtPincode]').val();
            event.preventDefault();

            fetch("https://api.postalpincode.in/pincode/" + $('[id*=txtPincode]').val())
                .then(response => response.json())
                .then(
                    function (data) {
                        if (data[0].Status == 'Success') {
                            document.getElementById('<%=txtCity.ClientID%>').textContent = data[0].PostOffice[0].Block;
                       document.getElementById('<%=txtDistrict.ClientID%>').textContent = data[0].PostOffice[0].District;
                       document.getElementById('<%=txtState.ClientID%>').textContent = data[0].PostOffice[0].State;
                       $('#<%=hfCity.ClientID%>').val(data[0].PostOffice[0].Block);
                       $('#<%=hfDistrict.ClientID%>').val(data[0].PostOffice[0].District);
                       $('#<%=hfState.ClientID%>').val(data[0].PostOffice[0].State);
                       document.getElementById('<%=txtCity.ClientID%>').style.color = 'black';
                   }
                   else {
                       if (data[0].PostOffice == null) {
                           document.getElementById('<%=txtCity.ClientID%>').textContent = 'Invalid Pincode';
                           document.getElementById('<%=txtDistrict.ClientID%>').textContent = '';
                           document.getElementById('<%=txtState.ClientID%>').textContent = '';
                           document.getElementById('<%=txtCity.ClientID%>').style.color = 'red';
                        }
                    }
                }
            )
                .catch()
        }


    </script>

    <asp:HiddenField ID="hfImageUrl" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfState" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfDistrict" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfCity" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfPassword" runat="server" />
    <asp:HiddenField ID="hfPrevImageLink" runat="server" EnableViewState="true" />

    <script type="text/javascript">
        $(document).ready(function () {
            let showPass = document.querySelector("#show_password");

            if (showPass == null)
                return;

            showPass.addEventListener("click", function () {
                let passBox = document.querySelector(".passBox1");
                if (passBox.type === "password") {
                    passBox.type = "text";
                    $('.icons').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
                else {
                    passBox.type = "password";
                    $('.icons').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                }
            })
        });
    </script>

</asp:Content>

