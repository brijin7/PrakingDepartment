<%@ Page Title="Charge Pin Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ChargePinConfig.aspx.cs" Inherits="Master_Configuration_ChargePinConfig" %>

<asp:Content ID="frmChargePinConfig" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .imageOverlay {
            background: rgba(245, 245, 245, 0.4);
            height: 20px;
            position: absolute;
            bottom: 0px;
            width: 66%;
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
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Charge Pin Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Charge Pin <span class="Card-title-second">Master</span></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <asp:Label ID="lblVehicleName" runat="server" Text="Pin Type"
                        CssClass="form-check-label">
                    </asp:Label><span class="spanStar">*</span>
                    <asp:TextBox
                        ID="txtChargePinType"
                        runat="server"
                        TabIndex="1" MaxLength="15"
                        CssClass="form-control" AutoComplete="Off">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="rfvVehicleName"
                        runat="server"
                        ControlToValidate="txtChargePinType"
                        CssClass="rfvClr"
                        ErrorMessage="Enter Pin Type">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                    <div class="panel panel-success">
                        <div class="panel-heading">Pin Type Image <span class="spanStar">*</span> </div>
                        <div class="panel-body">
                            <asp:FileUpload
                                ID="fupVehicleImage"
                                runat="server"
                                Style="display: none"
                                onchange="ShowImagePreview(this);" />
                            <div class="divImg" id="divImgPreview">
                                <asp:Image
                                    ID="imgEmpPhotoPrev"
                                    runat="server"
                                    alt="Pin Type Image"
                                   
                                    ImageUrl="~/images/ChargePin2.jpg"
                                    Width="75%" />
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
                    <h4 class="card-title">Charge Pin <span class="Card-title-second">Master</span></h4>
                </div>
                <div>
                    <%--<asp:Button
                        ID="btnAdd"
                        runat="server"
                        OnClick="btnAdd_Click"
                        Text="add +"
                        CssClass="pure-material-button-contained btnBgColorAdd" />--%>
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
                        DataKeyNames="chargePinId"
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
                            <asp:TemplateField HeaderText="Charge Pin Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblchargePinId"
                                        runat="server"
                                        Text='<%#Bind("chargePinId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pin Type">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblchargePinConfig"
                                        runat="server"
                                        Text='<%#Bind("chargePinConfig") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="Vehicle Image">
                                <ItemTemplate>
                                    <asp:ImageButton
                                        ID="lblVechicleImage"
                                        runat="server" ImageUrl='<%#Bind("vehicleImageUrl") %>' />
                                </ItemTemplate>

                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Charge Pin Image" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhotoLink" runat="server" Text='<%#Bind("chargePinImageUrl") %>'></asp:Label>
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
            var maxfilesize = 1024 * 1024;
            filesize = input.files[0].size;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" || ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG") {
                if (filesize <= maxfilesize) {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#<%=imgEmpPhotoPrev.ClientID%>').prop('src', e.target.result);
                            $('#<%=fupVehicleImage.ClientID%>').val("1");
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
                var fileupload = $('#<%=fupVehicleImage.ClientID%>');
                var image = $('#divImgPreview');
                image.click(function () {
                    fileupload.click();
                });
            });
    </script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <asp:HiddenField ID="hfImageUrl" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfPrevImageLink" EnableViewState="true" runat="server" />
</asp:Content>

