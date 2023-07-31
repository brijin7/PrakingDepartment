<%@ Page Title="Pass Transaction" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ParkingPassTransaction.aspx.cs" Inherits="Booking_ParkingPassTransaction" %>

<asp:Content ID="frmParkingPassTransaction" ContentPlaceHolderID="MasterPage" runat="Server">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"
        integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />
    <link href="../Style/Booking.css" rel="stylesheet" />
    <link href="../Style/ParkingPass.css" rel="stylesheet" />
    <style>
        .carimage {
            border-radius: 25px;
            padding-top: 6px;
            padding-left: 20px;
            padding-right: 0px !important;
            height: 48px;
            color: black;
            opacity:0.35;
        }

        .carimage2 {
            border-radius: 25px;
            padding-top: 6px;
            padding-left: 20px;
            padding-right: 0px !important;
            height: 48px;
            color: black;
        }

        .divcarname {
            margin-right: -32px;
            margin-left: 0px;
            background: white;
            border-radius: 30px;
            height: 35px;
            width: 18rem;
            color: black;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .divcarname2 {
            margin-right: -32px;
            margin-left: 0px;
            background: white;
            border-radius: 30px;
            height: 35px;
            width: 18rem;
            color: black;
            border: solid #0075ff 2px;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .divcarname1 {
            margin-right: -40px;
            margin-left: -10px;
            background: #2196f3;
            border-radius: 30px;
            height: 30px;
            color: white;
        }

        .vehiclename {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .Headervehicle {
            background-color: #c6ebff;
            border-radius: 10px;
            padding: 0px 15px;
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Pass Transaction"></asp:Label>
            </div>

        </div>
        <div class="divTitle row pt-4 pl-4 mt-4 justify-content-between">
            <h4 class="card-title"><span id="spAdd" runat="server"></span>Pass <span class="Card-title-second">Transaction</span></h4>
        </div>
        <div class="row mb-3" style="margin-top: -20px;">
            <div class="col-12 col-sm-4 col-md-4 col-lg-12 table-responsive section" style="overflow-y: hidden;">
                <asp:DataList ID="gvVehicleType" runat="server" RepeatColumns="10" CssClass="Headervehicle"
                    Visible="true">
                    <ItemTemplate>
                        <div id="divvehicles" runat="server" class="carimage" style="display: inline-flex;">

                            <asp:LinkButton ID="divvehicle" runat="server" OnClick="lblVehicleTypes_Click">
                                <div class="row" style="width: max-content;">
                                    <div id="divcarname" runat="server" class="col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname">
                                        <asp:Image ID="image" Style="height: 35px !important; width: 35px;"
                                            runat="server" ImageUrl='<%# Bind("vehicleImageUrl") %>' />
                                        <asp:Label ID="lblvehicleName" runat="server" CssClass="vehiclename"
                                            Text='<%# Bind("vehicleName") %>' Font-Bold="true" Font-Size="14px" Visible="true">
                                        </asp:Label>
                                    </div>
                                    <asp:Label align="left" ID="lblvehicleConfigId" runat="server"
                                        Text='<%# Bind("vehicleConfigId") %>' Visible="false" Font-Bold="true"></asp:Label>
                                </div>
                            </asp:LinkButton>

                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <table style="height: 5px; width: 5px; padding-left: 10px; padding-bottom: 40px;">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </SeparatorTemplate>
                </asp:DataList>
                <asp:Label ID="lblVehicleTypeId" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div id="divPassType" runat="server" class="row" visible="false">

            <div runat="server" class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                <asp:Label
                    ID="lblMobileNo"
                    runat="server"
                    Text="Mobile No.">
                </asp:Label><span class="spanStar">*</span>
                <asp:TextBox
                    ID="txtMobileNo"
                    runat="server" onkeypress="return isNumber(event);" MaxLength="10" MinLength="10"
                    TabIndex="3"
                    CssClass="form-control ">
                </asp:TextBox>
                <asp:RequiredFieldValidator
                    ID="rfvMobileNo"
                    ValidationGroup="ParkingPass"
                    ControlToValidate="txtMobileNo"
                    runat="server"
                    CssClass="rfvClr"
                    ErrorMessage="Enter Mobile No.">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                    ControlToValidate="txtMobileNo" ErrorMessage="Invalid Mobile No."
                    ValidationExpression="[0-9]{10}" CssClass="rfvClr" ValidationGroup="ParkingPass"></asp:RegularExpressionValidator>
            </div>

            <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                <asp:Label
                    ID="lblPassCategory"
                    runat="server"
                    Text="Pass Category">
                </asp:Label><span class="spanStar">*</span>
                <asp:DropDownList
                    ID="ddlPassCategory"
                    runat="server" AutoPostBack="true"
                    TabIndex="1" OnSelectedIndexChanged="ddlPassCategory_SelectedIndexChanged"
                    CssClass="form-control">
                    <asp:ListItem Value="0">Select</asp:ListItem>
                    <asp:ListItem Value="N">Normal</asp:ListItem>
                    <asp:ListItem Value="V">VIP</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator
                    ID="RfvPassCategory"
                    ValidationGroup="ParkingPass"
                    ControlToValidate="ddlPassCategory"
                    runat="server"
                    CssClass="rfvClr"
                    InitialValue="0"
                    ErrorMessage="Select Pass Category">
                </asp:RequiredFieldValidator>
            </div>

            <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                <asp:Label
                    ID="lblPassType"
                    runat="server"
                    Text="Pass Type">
                </asp:Label><span class="spanStar">*</span>
                <asp:DropDownList
                    ID="ddlPassType" OnSelectedIndexChanged="ddlPassType_SelectedIndexChanged"
                    runat="server" AutoPostBack="true"
                    TabIndex="2"
                    CssClass="form-control ">
                </asp:DropDownList>
                <asp:RequiredFieldValidator
                    ID="rfvPasstype"
                    ValidationGroup="ParkingPass"
                    ControlToValidate="ddlPassType"
                    runat="server"
                    CssClass="rfvClr"
                    InitialValue="0"
                    ErrorMessage="Select Pass Type">
                </asp:RequiredFieldValidator>
            </div>

            <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-3">
                <asp:Label
                    ID="lblPaymentType"
                    runat="server"
                    Text="Payment Type">
                </asp:Label><span class="spanStar">*</span>
                <asp:DropDownList
                    ID="ddlPaymentType"
                    runat="server"
                    TabIndex="4"
                    CssClass="form-control ">
                </asp:DropDownList>
                <asp:RequiredFieldValidator
                    ID="RfvPaymentType"
                    ValidationGroup="ParkingPass"
                    ControlToValidate="ddlPaymentType"
                    runat="server"
                    CssClass="rfvClr"
                    InitialValue="0"
                    ErrorMessage="Select Payment Type">
                </asp:RequiredFieldValidator>
            </div>
        </div>
        <div id="divpasssummary" runat="server" visible="false" class="cardpass  mt-5 mb-5 row">
            <div id="divSummaryImg" runat="server">
            </div>
            <div class="cardpass__text col-12 col-sm-4 col-md-4 col-lg-4 col-xl-12">
                <h1>Parking  Pass Summary</h1>
                <div class="container__text__timing_time row">
                    <div class="container__text__timing col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <h3>
                            <asp:Label ID="lblVehicleType" runat="server"></asp:Label>
                            <span style="font-size: 12px">Parking Charge </span></h3>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <p>
                            <asp:Label ID="lblvehicleamt" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="container__text__timing_time row">
                    <div class="container__text__timing col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <h3>
                            <asp:Label ID="lblTax" runat="server" Text="Tax"></asp:Label></h3>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <p>
                            <asp:Label ID="lbltaxamt" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="container__text__timing_time row">
                    <div class="container__text__timing col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <h3>
                            <asp:Label ID="lblTotal" runat="server" Text="Total Amount"></asp:Label></h3>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-6">
                        <p>
                            <asp:Label ID="lbltotalamt" runat="server"></asp:Label>
                            <asp:Label ID="lblOffertotal" runat="server"></asp:Label>
                            <asp:LinkButton ID="lnkRemove" CssClass="lnkremove"
                                OnClick="lnkRemove_Click" Visible="false"
                                runat="server">Remove <i class="fa-solid fa-delete-left"></i></asp:LinkButton>
                        </p>
                    </div>
                </div>
                <div id="divoffer" runat="server" class="container__text__timing_time row">
                    <div class="container__text__timing col-12 col-sm-6 col-md-6 col-lg-6 col-xl-12">
                        <asp:LinkButton ID="Lnkoffer" CssClass="flag-discount" Text="Click To Apply Offer %" OnClick="Lnkoffer_Click" runat="server"></asp:LinkButton>
                    </div>
                </div>
                <div class="justify-content-end">
                    <asp:Button ID="btnSubmit" runat="server" class="btn" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="ParkingPass"></asp:Button>
                </div>
            </div>
        </div>
    </div>

    <div id="modal" class="DisplyCard" runat="server" style="display: none">
        <div class="DisplyCardPostion table-responsive section">
            <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 mt-3" style="text-align-last: center;">
                <asp:Label
                    ID="lblOfflist"
                    runat="server"
                    Text="List Of Offers" Style="color: #1cb7fd; font-size: 24px; font-weight: bold;">                      
                </asp:Label>
                <asp:LinkButton ID="linkoffclose" CssClass="offerclose" runat="server" OnClick="linkoffclose_Click">
                <i class="fa-solid fa-xmark"></i></asp:LinkButton>
                <hr style="margin-top: 1rem !important; margin-bottom: 1rem !important; border: 0 !important; border-top: 1px solid rgb(28 183 253 / 27%) !important; width: 350px !important; margin-inline: auto !important;" />
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="wrap">
                    <asp:DataList ID="dlOfferDetails" runat="server" RepeatColumns="4" RepeatDirection="Vertical"
                        Visible="true">
                        <ItemTemplate>
                            <div id="divpin" runat="server">
                                <div class="cardoffer">
                                    <div class="row">
                                        <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                            <asp:LinkButton align="left" ID="lblofferHeading" CssClass="lbloff"
                                                OnClick="lblofferHeading_Click" runat="server"
                                                Text='<%# Bind("offerHeading")  %>'></asp:LinkButton>
                                            <br />
                                            <asp:Label ID="Label1" runat="server" CssClass="lbloffsub" Text='<%# Eval("OfferType").ToString() == "F" ? "₹":"" %>'></asp:Label>
                                            <asp:Label align="left" ID="lblOfferValue" runat="server"
                                                Text='<%# Bind("OfferValue") %>' CssClass="lbloffsub">                                   
                                            </asp:Label><asp:Label ID="lblRs" runat="server" CssClass="lbloffsub" Text='<%# Eval("OfferType").ToString() == "P" ? "%":"" %>'></asp:Label>
                                            <asp:Label align="left" ID="lblOfferType" runat="server"
                                                Text='<%# Bind("OfferType") %>' Font-Bold="true" Visible="false"></asp:Label>
                                            <asp:Label align="left" ID="lblOfferId" runat="server"
                                                Text='<%# Bind("OfferId") %>' Font-Bold="true" Visible="false"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <table style="height: 4px; width: 25px;">
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </SeparatorTemplate>
                    </asp:DataList>
                </div>
            </div>

        </div>
    </div>



    <div id="divPassTicket">
        <div id="DivPassTicket" runat="server" visible="false">
            <div class="pass-container">
                <div id="DivPassimg" class="passVIP u-clearfix" runat="server">
                    <div class="text-center">
                        <h2 class="pass-title">
                            <asp:Label ID="lblPassParkinngName" runat="server"></asp:Label></h2>
                    </div>
                    <div class="text-center">
                        <h2 class="pass-title-sub">
                            <asp:Label ID="lblPassBranchName" runat="server"></asp:Label></h2>
                    </div>
                    <div class="pass-body">
                        <span class="pass-user subtle">
                            <asp:Label ID="lblPassUserName" runat="server"></asp:Label></span>
                        <div class="row">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="Label1" runat="server" Text="Pass Id :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassId" runat="server"></asp:Label></span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblTicketPass" runat="server" Text="Pass Type :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblTicketPassType" runat="server"></asp:Label></span>
                            </div>
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassMobile" runat="server" Text="Mobile No :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassMobileNo" runat="server"></asp:Label></span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassFrmDate" runat="server" Text="Start Date :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassStartDate" runat="server"></asp:Label></span>
                            </div>
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassToDate" runat="server" Text="End Date :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassEndDate" runat="server"></asp:Label></span>
                            </div>
                        </div>
                        <div class="row ">
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassMod" runat="server" Text="Pass Category :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassMode" runat="server"></asp:Label></span>
                            </div>
                            <div class="col-6">
                                <span class="pass-description subtle">
                                    <asp:Label ID="lblPassVehicle" runat="server" Text="Vehicle Type :"></asp:Label></span>
                                <span class="pass-description-sub subtle">
                                    <asp:Label ID="lblPassVehicleType" runat="server"></asp:Label></span>
                            </div>
                        </div>
                        <div class="pass-read">
                            <i class="fa-solid fa-crown"></i>
                            <asp:Label ID="lblUserPassType" runat="server"></asp:Label>
                            Pass
                        </div>
                    </div>
                    <div>
                        <asp:Image ID="imgEmpPhotoPrev" runat="server" CssClass="pass-media" alt="Image" class="QR" />
                    </div>
                </div>
                <div class="pass-shadow"></div>
            </div>

        </div>
    </div>
    <div id="divDownload" runat="server" visible="false">
        <%--   <button id="btnExportPdf" class="pure-material-button-contained btnBgColorAdd">Send SMS</button>--%>
        <%-- <asp:Button
            ID="btn1"
            runat="server"
            Text="Send SMS"
            TabIndex="7" 
            OnClick="btn1_Click" 
            CssClass="pure-material-button-contained btnBgColorAdd" />--%>
        <asp:Button
            ID="btnExportPdf"
            runat="server"
            Text="Send SMS"
            TabIndex="7"
            OnClick="btnExportPdf_Click"
            CssClass="pure-material-button-contained btnBgColorAdd" />

    </div>
    <div id="Formbackground" runat="server"></div>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <script src="../js/jspdf.debug.js"></script>
    <script src="../js/html2canvas.min.js"></script>
    <script src="../js/html2pdf.min.js"></script>

    <%--    <script>

        //const options = {
        //    margin: 0.5,
        //    filename: 'ParkingPass.pdf',

        //    image: {
        //        type: 'jpeg',
        //        quality: 500
        //    },
        //    html2canvas: {
        //        scale: 7
        //    },
        //    jsPDF: {
        //        unit: 'in',
        //        format: 'letter',
        //        orientation: 'portrait'
        //    }
        //}


        // const blob = new Blob([value], { type: 'application/pdf'});

        $('#btnExportPdf').click(function (e) {

            e.preventDefault();

            let element = document.getElementById('divPassTicket');
            const options = {
                margin: 0.5,
                filename: 'ParkingPass.pdf',

                image: {
                    type: 'jpeg',
                    quality: 500
                },
                html2canvas: {
                    scale: 7
                },
                jsPDF: {
                    unit: 'in',
                    format: 'letter',
                    orientation: 'portrait'
                }
            }

            // html2pdf().from(element).set(options).save();
<%--            var filename = options.filename
            html2pdf()
                .from(element)
                .set(options)
                .toPdf()
                .outputPdf('datauristring')
                .then(function (pdfBase64) {
                    const file = new File(
                        [pdfBase64],
                        filename,
                        { type: 'application/pdf' }
                    );

                    const formData = new FormData();
                    formData.append("image", file);

                    alert(formData);
                    $.ajax({
                        url: '<%= Session["ImageUrl"].ToString() %>',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: formData,
                    success: function (result) {
                        alert(result.response);
                        $('#<%=hfImageUrl.ClientID%>').val(result.response);
                        $('#<%=btn1.ClientID%>').click();
                    },
                    error: function (err) {
                        $('#<%=btn1.ClientID%>').click();
                        alert(err.statusText);
                    }
                });
                });

           // html2pdf().from(element).set(options).save();

        });--%>


    <%--            html2pdf().from(element).set(options).save()
       .outputPdf('datauristring').then(function (pdfAsString) {
           var file = new File([pdfAsString], 'ParkingPass.pdf', {
               type: 'application/pdf',
           });

           var fileData = new FormData();
           fileData.append("image", file);
           //alert(fileData);
           $.ajax({
               url: '<%= Session["ImageUrl"].ToString() %>',
               type: "POST",
               data: fileData,
               contentType: false, // Not to set any content header  
               processData: false, // Not to process data                 
               success: function (result) {
                   //alert(result.response);
                   $('#<%=hfImageUrl.ClientID%>').val(result.response);
                   $('#<%=btn1.ClientID%>').click();
               },
               error: function (err) {
                   $('#<%=btn1.ClientID%>').click();
                   //alert(err.statusText);
               }
           });

       });
        });



    </script>--%>

    <script>
        function show() {

            document.getElementById('<%=modal.ClientID %>').style.cssText = "#model";

        }
        function hide() {
            document.getElementById('<%=modal.ClientID %>').style.display = "none";

        }
    </script>
    <asp:HiddenField ID="HiddenField1" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfPrevImageLink" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfImageUrl" runat="server" />
</asp:Content>

