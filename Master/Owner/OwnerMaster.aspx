<%@ Page Title="Owner Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="OwnerMaster.aspx.cs" Inherits="Master_OwnerMaster" %>

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
                <asp:Label ID="lblNavFirst" runat="server" CssClass="pageRoutecol" Text="Master"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Owner Master"></asp:Label>
            </div>

        </div>

        <div id="divForm" runat="server" visible="false">

            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Owner <span class="Card-title-second">Master</span></h4>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8">
                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label
                                ID="lblParkingName"
                                runat="server"
                                Text="Parking Name"
                                CssClass="form-check-label">
                            </asp:Label>
                            <span class="spanStar">*</span>
                            <asp:TextBox
                                ID="txtParkingName"
                                runat="server"
                                TabIndex="1" AutoComplete="off"
                                TextMode="MultiLine"
                                MaxLength="50"
                                CssClass="form-control">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator
                                ID="rfvParkingName"
                                runat="server"
                                ControlToValidate="txtParkingName"
                                ValidationGroup="OwnerMaster"
                                CssClass="rfvClr"
                                ErrorMessage="Enter Parking Name">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label
                                ID="lblShortName"
                                runat="server"
                                Text="Short Name"
                                CssClass="form-check-label">
                            </asp:Label>
                            <span class="spanStar">*</span>
                            <asp:TextBox
                                ID="txtShortName"
                                runat="server"
                                MaxLength="50" AutoComplete="off"
                                TabIndex="2"
                                CssClass="form-control">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator
                                ID="rfvShortName"
                                runat="server"
                                ControlToValidate="txtShortName"
                                ValidationGroup="OwnerMaster"
                                CssClass="rfvClr"
                                ErrorMessage="Enter Short Name">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblUserName" runat="server" Text="User Name" CssClass="form-check-label"> </asp:Label>
                            <span class="spanStar">*</span>
                            <asp:TextBox ID="txtUserName" AutoComplete="off" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" CssClass="rfvClr" ValidationGroup="OwnerMaster"
                                ControlToValidate="txtUserName" ErrorMessage=" Enter User Name"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="Label1" runat="server" Text="Password"
                                CssClass="lblContent_Common"> </asp:Label>
                            <span class="spanStar">*</span>
                            <div class="input-group-append">
                                <asp:TextBox ID="txtPassword" runat="server" AutoComplete="off"
                                    TabIndex="4" CssClass="form-control passBox1" TextMode="Password" MaxLength="50">  </asp:TextBox>

                                <div class="input-group-prepend">
                                    <div class="input-group-text p-0">
                                        <button id="show_password" class="btn btn-light pt-1" type="button" style="height: 28px; width: 40px;">
                                            <span class="fa fa-eye icons p-0"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="OwnerMaster"
                                ControlToValidate="txtPassword" CssClass="rfvClr" ErrorMessage="Enter Password">
                            </asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblPhone" runat="server" Text="Phone No." CssClass="form-check-label"> </asp:Label>
                            <span class="spanStar">*</span>
                            <asp:TextBox ID="txtPhoneNumber" runat="server" AutoComplete="off" CssClass="form-control" MaxLength="10" TabIndex="5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="rfvClr" ValidationGroup="OwnerMaster"
                                ControlToValidate="txtPhoneNumber" ErrorMessage=" Enter Phone No."></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <asp:Label ID="lblEmail" runat="server" Text="Email" CssClass="form-check-label"> </asp:Label>
                            <asp:TextBox
                                ID="txtEmailId"
                                runat="server" AutoComplete="off"
                                MaxLength="50"
                                TabIndex="6"
                                CssClass="form-control">
                            </asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmailId"
                                SetFocusOnError="True" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                Display="Dynamic" CssClass="rfvClr" ErrorMessage="Invalid email address" ValidationGroup="OwnerMaster" />

                        </div>
                    </div>
                </div>

                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">

                    <div class="panel panel-success">
                        <div class="panel-heading">Logo </div>
                        <div class="panel-body">
                            <asp:FileUpload ID="fupEmpLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image ID="imgEmpPhotoPrev" runat="server" alt="Logo Image" TabIndex="8"
                                    ImageUrl="~/images/emptylogo.png" Width="100%" />
                                <div class="imageOverlay">Click To Upload</div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="row justify-content-end p-3">
                <asp:Button
                    ID="btnSubmit"
                    runat="server"
                    Text="Submit"
                    TabIndex="9"
                    OnClick="btnSubmit_Click"
                    CausesValidation="true"
                    ValidationGroup="OwnerMaster"
                    CssClass="pure-material-button-contained btnBgColorAdd mr-3" />
                <asp:Button
                    ID="btnCancel"
                    runat="server"
                    Text="Cancel"
                    TabIndex="10"
                    OnClick="btnCancel_Click"
                    CausesValidation="false"
                    CssClass="pure-material-button-contained btnBgColorCancel" />
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Owner <span class="Card-title-second">Master</span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                </div>
            </div>
            <div class="table-responsive section">
                <asp:GridView
                    ID="gvOwnerDetails"
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
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvUserId"
                                    runat="server"
                                    Text='<%#Bind("userId") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Parking Name">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvParkingName"
                                    runat="server"
                                    Text='<%#Bind("parkingName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Short Name" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvShortName"
                                    runat="server"
                                    Text='<%#Bind("shortName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Name">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvUserName"
                                    runat="server"
                                    Text='<%#Bind("userName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Password" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvPassword"
                                    runat="server"
                                    Text='<%#Bind("password") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Phone No.">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvPhoneNo"
                                    runat="server"
                                    Text='<%#Bind("phoneNumber") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvEmailId"
                                    runat="server"
                                    Text='<%#Bind("emailId") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="logo" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblgvlogoUrl"
                                    runat="server"
                                    Text='<%#Bind("logoUrl") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--                        <asp:TemplateField HeaderText="GST Number" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvGSTNumber"
                                    runat="server"
                                    Text='<%#Bind("gstNumber") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Founder Name" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvFounderName"
                                    runat="server"
                                    Text='<%#Bind("founderName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Place Type" Visible="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblGvPlaceType"
                                    runat="server"
                                    Text='<%#Bind("placeType") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
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
                                    Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>' OnClick="lnkActiveOrInactive_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

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
                            success: function (data) {
                                $('#<%=hfImageUrl.ClientID%>').val(data.response);
                            },
                            error: function (data) {
                                alert('Error');
                            }
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

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <asp:HiddenField ID="hfImageCheckValue" Value="0" runat="server" />
    <asp:HiddenField ID="hfImageUrl" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfPassword" runat="server" />

    <script type="text/javascript">
        $(document).ready(function () {
            let showPass = document.querySelector("#show_password");
            let passBox = document.querySelector(".passBox1");

            if (showPass == null)
                return;

            showPass.addEventListener("click", function () {


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


        //function ClearPassword() {
        //    let txtPassword = document.querySelector(".passBox1");
        //    if (txtPassword == null)
        //        return;

        //    document.querySelector(".passBox1").value = null;
        //}
    </script>
</asp:Content>
