<%@ Page Title="Vehicle Master" Language="C#" MasterPageFile="~/PreParking.master"
    AutoEventWireup="true" CodeFile="VehicleConfigMaster.aspx.cs"
    Inherits="Master_Configuration_VehicleConfigMaster" EnableEventValidation="false" %>

<asp:Content ID="frmVehicleConfigMaster" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .imageOverlay {
            background: rgba(245, 245, 245, 0.4);
            height: 20px;
            position: absolute;
            bottom: 0px;
            width: 45%;
            text-align: center;
            color: black;
            font-size: 10px;
            font-weight: 500;
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Configuration"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Vehicle Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Vehicle <span class="Card-title-second">Master</span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblVehicleName" runat="server" Text="Vehicle Type"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span
                        data-toggle="tooltip"
                        data-placement="right"
                        data-original-title="i.e. Car, Bike etc.,"
                        class="mdi mdi-information-outline tooltipIcon"></span><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtVehicleName"
                        runat="server"
                        TabIndex="1" MaxLength="50"
                        CssClass="form-control" AutoComplete="Off">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvVehicleName"
                        runat="server"
                        ControlToValidate="txtVehicleName"
                        CssClass="rfvClr"
                        ValidationGroup="VehConfig"
                        ErrorMessage="Enter Vehicle Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblVehicleKeyName" runat="server" Text="Vehicle Key Name"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span
                        data-toggle="tooltip"
                        data-placement="right"
                        data-original-title="i.e. 2WN, LMV, MPV etc.,"
                        class="mdi mdi-information-outline tooltipIcon"></span><%--<span class="spanStar">*</span>--%>
                    <asp:TextBox
                        ID="txtVehicleKeyName"
                        runat="server"
                        TabIndex="2" MaxLength="150"
                        CssClass="form-control" AutoComplete="Off">
                    </asp:TextBox>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div class="panel panel-success">
                        <div class="panel-heading form-check-label">Vehicle Place Holder Image <span class="spanStar">*</span> </div>
                        <div class="panel-body" style="text-align: center;">
                            <asp:FileUpload
                                ID="fupVehiclePHImage"
                                runat="server"
                                Style="display: none"
                                onchange="ShowImagePHPreview(this);" />
                            <div class="divImg" id="divPHImgPreview">
                                <asp:Image
                                    ID="imgVehPHPhotoPrev"
                                    runat="server"
                                    alt="Vehicle Place Holder Image"
                                    ImageUrl="~/images/vehicle-icon.png"
                                    Width="23%" />
                                <div class="imageOverlay" style="margin-left: 22%; height: 16px;">Click To Upload</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="Label1" runat="server" Text="Is Vehicle No. Required"
                        CssClass="form-check-label">
                    </asp:Label>
                    <span class="spanStar">*</span>
                    <asp:RadioButtonList
                        ID="rbtnVehicleNo"
                        runat="server"
                        TabIndex="4" RepeatDirection="Horizontal"
                        CssClass="inline-rb" AutoComplete="Off">
                        <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator
                        ID="rfvVehicleNo"
                        runat="server"
                        ControlToValidate="rbtnVehicleNo"
                        CssClass="rfvClr"
                        ValidationGroup="VehConfig"
                        ErrorMessage="Select Vehicle No. Required">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div class="panel panel-success">
                        <div class="panel-heading form-check-label">Vehicle Image <span class="spanStar">*</span> </div>
                        <div class="panel-body">
                            <asp:FileUpload
                                ID="fupVehicleImage"
                                runat="server"
                                ClientIDMode="Static"
                                Style="display: none"
                                onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image
                                    ID="imgVehPhotoPrev"
                                    runat="server"
                                    alt="Vehicle Image"
                                    ImageUrl="~/images/vehicle-icon.png"
                                    Width="50%" />
                                <div class="imageOverlay">Click To Upload</div>
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
                        ValidationGroup="VehConfig"
                        TabIndex="7" OnClick="btnSubmit_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" />

                </div>
                <div>
                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="cancel"
                        TabIndex="8"
                        OnClick="btnCancel_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorCancel" />
                </div>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Vehicle <span class="Card-title-second">Master</span></h4>
                </div>
                <div>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>

                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12" visible="false">
                <div class="table-responsive section">
                    <asp:GridView
                        ID="gvVehicleConfigType"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="vehicleConfigId"
                        CssClass="gvv display"
                        AutoGenerateColumns="false"
                        BorderStyle="None"
                        PageSize="100">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Config Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblvehicleConfigId"
                                        runat="server"
                                        Text='<%#Bind("vehicleConfigId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblvehicleName"
                                        runat="server"
                                        Text='<%#Bind("vehicleName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle Key Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblvehicleKeyName"
                                        runat="server"
                                        Text='<%#Bind("vehicleKeyName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vehicle No. Required">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblVehicleNo"
                                        runat="server"
                                        Text='<%#Eval("isvehicleNumberRequired").ToString() == "Y" ? "Yes" : "No" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="Vehicle Image">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="lblVehicleImage"
                                        runat="server" ImageUrl='<%#Bind("vehicleImageUrl") %>' />
                                </ItemTemplate>

                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Vehicle Image" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhotoLink" runat="server" Text='<%#Bind("vehicleImageUrl") %>'></asp:Label>
                                    <asp:Label ID="lblVehPHUrl" runat="server" Text='<%#Bind("vehiclePlaceHolderImageUrl") %>'></asp:Label>
                                </ItemTemplate>

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
    <script type="text/javascript">
        function ShowImagePreview(input) {
            var fup = document.getElementById("<%=fupVehicleImage.ClientID %>");
            var fileName = fup.value;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" ||
                ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG" || ext == "webp" || ext == "WEBP" ||
                ext == "jfif" || ext == "JFIF") {

                if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    //Read the contents of Image File.
                    reader.readAsDataURL(input.files[0]);

                    reader.onload = function (e) {
                        var image = new Image();
                        image.src = e.target.result;
                        image.onload = function () {
                            if ('512x512' == this.width + 'x' + this.height) {
                                $('#<%=imgVehPhotoPrev.ClientID%>').prop('src', e.target.result);
                                var formData = new FormData()
                                formData.append("image", $('input[type=file]')[1].files[0])
                                $.ajax({
                                    url: '<%= Session["ImageUrl"].ToString() %>',
                                    type: 'POST',
                                    data: formData,
                                    contentType: false,
                                    processData: false,
                                    success: function (data) {
                                        $('#<%=hfImageUrl.ClientID%>').val(data.response);
                                    },

                                });
                            }
                            else {
                                swal("Please, Upload image file equal to 512 * 512 !!! " + "Your Image Dimension is" + this.width + 'x' + this.height);
                                fup.focus();
                                return false;
                            }
                        }
                    }
                }

            }
            else {
                swal("Please, Upload Gif, Jpg, Jpeg or Bmp Images only !!!");
                fup.focus();
                return false;
            }
        }

        $(function () {
            var fileupload = $('#<%=fupVehicleImage.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });
        function ShowImagePHPreview(input) {
            var fup = document.getElementById("<%=fupVehiclePHImage.ClientID %>");
            var fileName = fup.value;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" ||
                ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG" || ext == "webp" || ext == "WEBP" ||
                ext == "jfif" || ext == "JFIF") {

                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    //Read the contents of Image File.
                    reader.readAsDataURL(input.files[0]);

                    reader.onload = function (e) {
                        var image = new Image();
                        image.src = e.target.result;
                        image.onload = function () {
                            if ('512x512' == this.width + 'x' + this.height) {
                                $('#<%=imgVehPHPhotoPrev.ClientID%>').prop('src', e.target.result);
                                var formData = new FormData()
                                formData.append("image", $('input[type=file]')[0].files[0])
                                $.ajax({
                                    url: '<%= Session["ImageUrl"].ToString() %>',
                                    type: 'POST',
                                    data: formData,
                                    contentType: false,
                                    processData: false,
                                    success: function (data) {
                                        $('#<%=hfPHImageUrl.ClientID%>').val(data.response);
                                    },

                                });
                            }
                            else {
                                swal("Please, Upload image file equal to 512 * 512 !!! " + "Your Image Dimension is" + this.width + 'x' + this.height);
                                fup.focus();
                                return false;
                            }
                        }
                    }
                }

            }
            else {
                swal("Please, Upload Gif, Jpg, Jpeg or Bmp Images only !!!");
                fup.focus();
                return false;
            }
        }

        $(function () {
            var fileupload = $('#<%=fupVehiclePHImage.ClientID%>');
            var image = $('#divPHImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });
    </script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <asp:HiddenField ID="hfImageUrl" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfPHImageUrl" EnableViewState="true" runat="server" />


</asp:Content>

