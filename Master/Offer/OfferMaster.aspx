<%@ Page Title="Offer Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="OfferMaster.aspx.cs" Inherits="Master_Offer_OfferMaster" %>

<asp:Content ID="frmOffermaster" ContentPlaceHolderID="MasterPage" runat="Server">
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css"
        integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g=="
        crossorigin="anonymous"
        referrerpolicy="no-referrer" />
    <style>
        .tab {
            display: none;
        }

        .labels {
            margin-top: 3px;
        }

        button:hover {
            opacity: 0.8;
        }

        .prevBtn {
            background-color: #8d8d8d;
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
                visibility: visible;
            }

            /* Mark the steps that are finished and valid: */
            .step.finish {
                background-color: #0c6cb9;
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

        .inline-rb label {
            display: inline;
            margin-right: 10px;
            font-size: 13px;
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
                box-shadow: 0 0 10px #7a7a7ac4;
                cursor: pointer;
                border-radius: 50px;
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Offers"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Offer Master"></asp:Label>
            </div>
        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Offer <span class="Card-title-second" runat="server" id="formhdr">Master </span></h4>
                </div>
            </div>
            <div class="tab">
                <div class="row">
                    <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8">
                        <div class="row">
                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                <asp:Label
                                    ID="lblOfferHeading"
                                    runat="server"
                                    Text="Offer Heading"
                                    CssClass="form-check-label">
                                </asp:Label>
                                <span class="spanStar">*</span>
                                <asp:TextBox ID="txtoffhead" runat="server" AutoComplete="off" CssClass="form-control labels" MaxLength="50" TabIndex="1">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="vdgOfferMasterN"
                                    ControlToValidate="txtoffhead"
                                    runat="server" CssClass="rfvClr" ErrorMessage="Enter Offer Heading">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                <asp:Label
                                    ID="lblOfferCode"
                                    runat="server"
                                    Text="Offer Code" CssClass="form-check-label">
                                </asp:Label>
                                <span class="spanStar">*</span>
                                <asp:TextBox ID="txtoffcode" AutoComplete="off" runat="server" CssClass="form-control labels" MaxLength="10" TabIndex="2">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="vdgOfferMasterN"
                                    ControlToValidate="txtoffcode"
                                    runat="server" CssClass="rfvClr" ErrorMessage="Enter Offer Code">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                <asp:Label
                                    ID="lblOfferDescription"
                                    runat="server"
                                    Text="Offer Description" CssClass="form-check-label">
                                </asp:Label>
                                <span class="spanStar">*</span>
                                <asp:TextBox ID="txtoffdescription" runat="server" AutoComplete="off"
                                    TextMode="MultiLine" TabIndex="3"
                                    CssClass="form-control section labels" Height="50px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="vdgOfferMasterN"
                                    ControlToValidate="txtoffdescription" runat="server" CssClass="rfvClr"
                                    ErrorMessage="Enter Offer Description">
                                </asp:RequiredFieldValidator>
                            </div>


                        </div>
                        <div class="row">
                            <div class="col-12 col-sm-7 col-md-7 col-lg-7 col-xl-7">
                                <asp:Label
                                    ID="lblOfferTypePeriod"
                                    runat="server"
                                    Text="Offer Period Type"
                                    CssClass="form-check-label">
                                </asp:Label>
                                <span class="spanStar">*</span>
                                <asp:RadioButtonList
                                    ID="rbtntypeperiod"
                                    runat="server"
                                    CssClass="inline-rb labels"
                                    TabIndex="5"
                                    RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtntypeperiod_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Before Trans" Value="B"></asp:ListItem>
                                    <asp:ListItem Text="After Trans" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Flat Offer" Value="F"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="rfvOfferTypePeriod" ValidationGroup="vdgOfferMasterN"
                                    ControlToValidate="rbtntypeperiod" runat="server" CssClass="rfvClr"
                                    ErrorMessage="Select Period Type">
                                </asp:RequiredFieldValidator>

                            </div>
                            <div class="col-12 col-sm-5 col-md-5 col-lg-5 col-xl-5">
                                <asp:Label
                                    ID="lblperuser"
                                    runat="server"
                                    Text="No. Of Times Appl. Per User" CssClass="form-check-label">
                                </asp:Label>
                                <span class="spanStar">*</span>
                                <asp:TextBox ID="txtperuser" runat="server" AutoComplete="off"
                                    onkeypress="return isNumber(event);" MaxLength="3" CssClass="form-control labels" TabIndex="7"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ValidationGroup="vdgOfferMasterN" ControlToValidate="txtperuser" runat="server" CssClass="rfvClr"
                                    ErrorMessage="Enter No. Of Times Appl./ User">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row" id="divValue" runat="server" visible="false">
                            <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                <asp:Label
                                    ID="lblofftype"
                                    runat="server"
                                    Text="Offer Value Type" CssClass="form-check-label">
                                </asp:Label>
                                <span class="spanStar">*</span>
                                <asp:RadioButtonList
                                    ID="rdoofftype"
                                    runat="server"
                                    CssClass="inline-rb labels"
                                    TabIndex="6"
                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoofftype_SelectedIndexChanged">
                                    <asp:ListItem Text="Percentage (%)" Value="P"></asp:ListItem>
                                    <asp:ListItem Text="Fixed" Value="F"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ValidationGroup="vdgOfferMasterN"
                                    ControlToValidate="rdoofftype" runat="server" CssClass="rfvClr"
                                    ErrorMessage="Select Offer Type">
                                </asp:RequiredFieldValidator>
                            </div>

                        </div>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <div class="panel panel-success">
                            <asp:Label ID="lblupimgurl" runat="server" Text="Offer Image"
                                CssClass="panel-heading"></asp:Label>
                            <span class="spanStar">*</span>
                            <div class="panel-body">
                                <asp:FileUpload ID="upimgurl" runat="server" Style="display: none" TabIndex="8"
                                    onchange="ShowImagePreview(this);" />
                                <div class="divImg" id="divImgPreview">
                                    <asp:Image ID="imgEmpPhotoPrev" runat="server" alt="Offer Image" TabIndex="8"
                                        ImageUrl="../../images/EmptyImageNew.png" Width="100%" />
                                    <div class="imageOverlay">Click To Upload</div>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="vdgOfferMasterN"
                                    ControlToValidate="upimgurl" runat="server" CssClass="rfvClr"
                                    ErrorMessage="Upload Offer Image">
                                </asp:RequiredFieldValidator>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row" id="divAmount" runat="server" visible="false">
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3" id="divPercentage" runat="server" visible="false">
                        <asp:Label
                            ID="Label1"
                            runat="server"
                            CssClass="form-check-label">Offer Value(<i class="fas fa-percentage" style="font-size: 12px;"></i>)
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtoffvalueper" runat="server" 
                            CssClass="form-control labels" onkeyup="this.value = minmax(this.value, 0, 100);"
                            onkeypress=" return isNumber(event);"
                            MaxLength="12" TabIndex="9" AutoComplete="Off">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOfferValueper" ValidationGroup="vdgOfferMasterN"
                            ControlToValidate="txtoffvalueper" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter Offer Value">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3" id="divFixed" runat="server">
                        <asp:Label
                            ID="lbloffervaluefix"
                            runat="server"
                            CssClass="form-check-label">Offer Value(<i class="fas fa-rupee-sign" style="font-size: 12px;"></i>)
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtOffervalueFix" runat="server" OnTextChanged="txtOffervalueFix_TextChanged" AutoPostBack="true"  
                            CssClass="form-control labels" onkeypress="return AllowOnlyAmountAndDot(this.id);"
                            MaxLength="12" TabIndex="9" AutoComplete="Off">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvoffervalueFix" ValidationGroup="vdgOfferMasterN"
                            ControlToValidate="txtOffervalueFix" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter Offer Value">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <asp:Label
                            ID="lblminamt"
                            runat="server"
                            CssClass="form-check-label" Style="font-size: 15px;">Min. Amount(<i class="fas fa-rupee-sign" style="font-size: 12px;"></i>)
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtminamount" runat="server"
                            onkeypress="return AllowOnlyAmountAndDot(this.id);" OnTextChanged="txtminamount_TextChanged" AutoPostBack="true"
                            MaxLength="12" CssClass="form-control labels" TabIndex="10" AutoComplete="Off">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ValidationGroup="vdgOfferMasterN"
                            ControlToValidate="txtminamount" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter Min. Amount">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <asp:Label
                            ID="lblmaxamt"
                            runat="server"
                            CssClass="form-check-label" Style="font-size: 15px;">Max. Amount(<i class="fas fa-rupee-sign" style="font-size: 12px;"></i>)
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtmaxamount" runat="server" OnTextChanged="txtmaxamount_TextChanged" AutoPostBack="true"
                            CssClass="form-control labels" onkeypress="return AllowOnlyAmountAndDot(this.id);"
                            MaxLength="12" TabIndex="11" AutoComplete="Off">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ValidationGroup="vdgOfferMasterN" ControlToValidate="txtmaxamount" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter Max. Amount">
                        </asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="row" id="divDate" runat="server" visible="false">
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <asp:Label
                            ID="lblFromDate"
                            runat="server"
                            Text="From Date" CssClass="form-check-label">
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtfrmdate" runat="server" TabIndex="12"
                            CssClass="form-control fromDate labels"
                            onchange="change();"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="vdgOfferMasterN"
                            ControlToValidate="txtfrmdate" runat="server" CssClass="rfvClr"
                            ErrorMessage="Select From Date">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <asp:Label
                            ID="lblTodate"
                            runat="server"
                            Text="To Date" CssClass="form-check-label">
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txttodate" runat="server" TabIndex="13" AutoComplete="off"
                            CssClass="form-control toDate labels"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6"
                            ControlToValidate="txttodate" runat="server" CssClass="rfvClr"  ValidationGroup="vdgOfferMasterN"
                            ErrorMessage="Select To Date">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <asp:Label
                            ID="lblFromTime"
                            runat="server"
                            Text="From Time" CssClass="form-check-label">
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtfrmtime" runat="server" TabIndex="14" AutoComplete="off"
                            CssClass="form-control timePicker labels"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7"
                            ValidationGroup="vdgOfferMasterN" ControlToValidate="txtfrmtime"
                            runat="server" CssClass="rfvClr"
                            ErrorMessage="Select From Time">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <asp:Label
                            ID="lblTotime"
                            runat="server"
                            Text="To Time" CssClass="form-check-label toTime">
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txttotime" runat="server" TabIndex="15" AutoComplete="off"
                            CssClass="form-control timePicker labels"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="vdgOfferMasterN"
                            ControlToValidate="txttotime" runat="server" CssClass="rfvClr"
                            ErrorMessage="Select To Time">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                </div>

            </div>
            <div class="tab">
                <div class="row">
                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                        <asp:Label
                            ID="lbltc"
                            runat="server"
                            Text="Terms and Conditions" CssClass="form-check-label">
                        </asp:Label>

                        <asp:TextBox ID="txttc" TextMode="MultiLine"
                            runat="server" CssClass="form-control section labels" Height="50px" TabIndex="16"></asp:TextBox>

                    </div>

                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                        <asp:Label
                            ID="lblOfferAvaila"
                            runat="server"
                            Text="Offer Availablity" CssClass="form-check-label">
                        </asp:Label>

                        <asp:TextBox ID="txtOfferAvailabilty" TextMode="MultiLine" Height="50px" runat="server" AutoComplete="off"
                            CssClass="form-control section labels" TabIndex="17"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Visible="false" ControlToValidate="txttc" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter Offer Availablity">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="margin-top: 20px;">
                        <asp:Label
                            ID="lblAboutOffer"
                            runat="server"
                            Text="About Offer" CssClass="form-check-label">
                        </asp:Label>

                        <asp:TextBox ID="txtAboutOffer" TextMode="MultiLine" AutoComplete="off"
                            Height="50px" runat="server" CssClass="form-control section labels" TabIndex="18"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" Visible="false" ControlToValidate="txttc" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter About Offer">
                        </asp:RequiredFieldValidator>
                    </div>

                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="margin-top: 20px;">
                        <asp:Label
                            ID="lblRules"
                            runat="server"
                            Text="Rules" CssClass="form-check-label">
                        </asp:Label>
                        <asp:TextBox ID="txtRules" TextMode="MultiLine" Height="50px" runat="server" AutoComplete="off" 
                            CssClass="form-control section labels" TabIndex="19"></asp:TextBox>

                    </div>
                </div>
            </div>
            <div class="row justify-content-end justify-content-sm-end
                        justify-content-md-end justify-content-lg-end justify-content-xl-end  mt-4"
                style="margin-top: -40px;">
                <div>
                    <asp:Button runat="server" CssClass="pure-material-button-contained prevBtn mr-2"
                        ID="prevBtn" Text="Previous" OnClientClick="nextPrev(-1,event)" />
                </div>
                <div>
                    <asp:Button runat="server" CssClass="pure-material-button-contained nextBtn mr-2"
                        Text="Next" ID="nextBtn" ValidationGroup="vdgOfferMasterN" OnClientClick="nextPrev(1,event)" />
                </div>
                <div>
                    <asp:Button ID="btnSubmit" CssClass="pure-material-button-contained btnBgColorAdd mr-2"
                        Text="Submit" ValidationGroup="vdgOfferMaster" runat="server" OnClick="btnSubmit_Click" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" CssClass="pure-material-button-contained btnBgColorCancel mr-2"
                        Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false"
                        runat="server" />
                </div>
            </div>
            <div style="text-align: center; margin-top: 40px;">
                <span class="step"></span>
                <span class="step"></span>
            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Offer <span class="Card-title-second">Master </span></h4>
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
                        ID="gvoffermaster"
                        runat="server"
                        AllowPaging="True"
                        DataKeyNames="offerId"
                        CssClass="gvv display"
                        BorderStyle="None"
                        AutoGenerateColumns="false"
                        PageSize="100" OnRowDataBound="gvoffermaster_RowDataBound">
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
                                        ID="lblgvofferId"
                                        runat="server"
                                        Text='<%#Bind("offerId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Type Period" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferTypePeriod"
                                        runat="server"
                                        Text='<%#Bind("offerTypePeriod") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Name">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferHeading"
                                        runat="server"
                                        Text='<%#Bind("offerHeading") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Description" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferDescription"
                                        runat="server"
                                        Text='<%#Bind("offerDescription") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Image" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferImageUrl"
                                        runat="server"
                                        Text='<%#Bind("offerImageUrl") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Code">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferCode"
                                        runat="server"
                                        Text='<%#Bind("offerCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvfromDate"
                                        runat="server"
                                        Text='<%#Eval("fromDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="To Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtoDate"
                                        runat="server"
                                        Text='<%#Eval("toDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="From Time" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvfromTime"
                                        runat="server"
                                        Text='<%#Bind("fromTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="To Time" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvtoTime"
                                        runat="server"
                                        Text='<%#Bind("toTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Type" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferType"
                                        runat="server"
                                        Text='<%#Bind("offerType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Offer Value" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvofferValue"
                                        runat="server"
                                        Text='<%#Bind("offerValue") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Min/Max Amount" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvminAmt"
                                        runat="server"
                                        Text='<%#Eval("minAmt").ToString() + "/" + Eval("maxAmt").ToString() %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="No. Of Times Per User" Visible="false">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblgvnoOfTimesPerUser"
                                        runat="server"
                                        Text='<%#Bind("noOfTimesPerUser") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>



                            <asp:TemplateField HeaderText="OfferRules" Visible="false" HeaderStyle-CssClass="gvHeader">
                                <ItemTemplate>
                                    <asp:DataList runat="server" ID="dlOfferrules" RepeatDirection="Horizontal">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvofferRuleId" runat="server" Text='<%# Bind("offerRuleId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                            <asp:Label ID="lblgvofferRule" runat="server" Text='<%# Bind("offerRule") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                            <asp:Label ID="lblgvruleType" runat="server" Text='<%# Bind("ruleType") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                            <asp:Label ID="lblgvofferactiveStatus" runat="server" Text='<%# Bind("activeStatus") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label
                                        ID="lblExpireStatus"
                                        runat="server"
                                        CssClass='<%#Eval("ExpireStatus").ToString() =="Not Expired"?"btnActive":"btnInActive"%>'
                                        Text='<%#Eval("ExpireStatus").ToString() =="Not Expired"?"Not Expired":"Expired"%>'></asp:Label>
                                </ItemTemplate>

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

    </div>
    <%--Script for Stepper--%>
    <script type="text/javascript">
        var currentTab = 0;
        showTab(currentTab);

        function showTab(n) {
            var x = document.getElementsByClassName("tab");
            x[n].style.display = "block";

            if (n == 0) {
                // alert("1")
                document.getElementById('<%=prevBtn.ClientID %>').style.display = "none";
                document.getElementById('<%=btnSubmit.ClientID %>').style.display = "none";
                document.getElementById('<%=nextBtn.ClientID %>').style.display = "inline";
                document.getElementById('<%=formhdr.ClientID %>').textContent = "Master";
            } else {
                // alert("2")
                document.getElementById('<%=prevBtn.ClientID %>').style.display = "inline";
                document.getElementById('<%=nextBtn.ClientID %>').style.display = "none";
                document.getElementById('<%=formhdr.ClientID %>').textContent = "Master";
            }
            if (n == (x.length - 1)) {
                // alert("3")
                document.getElementById('<%=nextBtn.ClientID %>').style.display = "none";
                document.getElementById('<%=btnSubmit.ClientID %>').style.display = "inline";
                document.getElementById('<%=formhdr.ClientID %>').textContent = "Rules";

            } <%--else {
                document.getElementById('<%=nextBtn.ClientID %>').value = "Next";
                document.getElementById('<%=nextBtn.ClientID %>').style.display = "inline";
                document.getElementById('<%=btnSubmit.ClientID %>').style.display = "none";
            }--%>
            fixStepIndicator(n)
        }

        function nextPrev(n, e) {

            e.preventDefault();
            var x = document.getElementsByClassName("tab");
            if (n == 1 && !validateForm()) return false;
            x[currentTab].style.display = "none";
            currentTab = currentTab + n;
            if (currentTab >= x.length) {

                document.getElementById("regForm").submit();
                return false;
            }
            showTab(currentTab);
        }
        function validateForm() {

            var x, y, z, i, valid = true;
            x = document.getElementsByClassName("tab");
            y = x[currentTab].getElementsByTagName("input");
            z = x[currentTab].getElementsByTagName("textarea");
            m = currentTab;
            if (m == 0) {


                for (i = 0; i <= y.length - 1; i++) {
                    // If a field is empty...
                    valid = true;
                    if (y[i].value == "") {
                        // add an "invalid" class to the field:
                        y[i].className += " invalid";
                        // and set the current valid status to false
                        if (i == 0) {
                            valid = false;
                            break;
                        }
                        else if (i == 1) {
                            valid = false;
                            break;
                        }
                        else if (i == 3) {
                            valid = false;
                            break;
                        }
                        else if (i == 5) {
                            valid = false;
                            break;
                        }
                        else if (i == 6) {
                            valid = false;
                            break;
                        }

                        else if (i == 7) {
                            valid = false;
                            break;
                        }
                        else if (i == 8) {

                            if ($('#<%= hfofferimage.ClientID %>').val() != "") {
                                    valid = true;
                                }
                                else {
                                    valid = false;

                                }
                                break;


                            }
                            else if (i == 9) {
                                valid = false;
                                break;
                            }
                            else if (i == 10) {
                                valid = false;
                                break;
                            }
                            else if (i == 11) {
                                valid = false;
                                break;
                            }
                            else if (i == 12) {
                                valid = false;
                                break;
                            }
                            else if (i == 14) {
                                valid = false;
                                break;
                            }
                            else if (i == 13) {
                                valid = false;
                                break;
                            }
                            else if (i == 15) {
                                valid = false;
                                break;
                            }
                            else {
                                valid = true;

                            }


            }

        }
                for (j = 0; j <= z.length - 1; j++) {
            // If a field is empty...
            if (z[j].value == "") {
                // add an "invalid" class to the field:
                z[j].className += " invalid";
                // and set the current valid status to false
                if (j == 0) {
                    valid = false;
                }
            }
        }

    }
    var selectedvalue = $('#<%= rbtntypeperiod.ClientID %> input:checked').val();
            if (selectedvalue == undefined) {               infoalert('Select Off Period Type');               valid = false;
            }

            if (valid) {
                document.getElementsByClassName("step")[currentTab].className += " finish";
            }
            return valid;
        }

        function fixStepIndicator(n) {
            var i, x = document.getElementsByClassName("step");
            for (i = 0; i < x.length; i++) {
                x[i].className = x[i].className.replace(" active", "");
            }
            x[n].className += " active";
        }
    </script>
    <%-- Upload for Offer Image--%>
    <script type="text/javascript">
        function ShowImagePreview(input) {
            //alert(document.getElementById("hfImageUrl").value);

            var fup = document.getElementById("<%=upimgurl.ClientID %>");
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
                            $('#<%=upimgurl.ClientID%>').val("1");
                        };
                        reader.readAsDataURL(input.files[0]);
                        this.formData = new FormData();
                        formData.append("image", $('input[type=file]')[0].files[0])
                        $.ajax({
                            url: '<%= Session["ImageUrl"].ToString() %>',
                            type: 'POST',
                            data: formData,
                            contentType: false,
                            processData: false,
                            success: function (data) {
                                $('#<%=hfofferimage.ClientID%>').val(data.response);
                                console.log(data.response, 'BranchImage');
                            },
                        });
                        }
                    }
                    else {
                        swal("Please Upload image file less than or equal to 1 MB !!!");
                        fup.focus();
                        return false;
                    }
                }
                else {
                    swal("Please Upload jpg, jpeg, png, gif or bmp Images files only !!!");
                    fup.focus();
                    return false;
                }

            }

            $(function () {
                var fileupload = $('#<%=upimgurl.ClientID%>');
                var image = $('#divImgPreview');
                image.click(function () {
                    fileupload.click();
                });
            });
    </script>
    <script>
        function change() {
            var date = $('#<%=txtfrmdate.ClientID%>').val();
            var findme = " to ";
            if (date.indexOf(findme) > -1) {
                let txtfrmdate = $('#<%=txtfrmdate.ClientID%>').val();
                const myArray = txtfrmdate.split(" to ");
                $('#<%=txtfrmdate.ClientID%>').val(myArray[0]);
                $('#<%=txttodate.ClientID%>').val(myArray[1]);
            }
        }

    </script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <asp:HiddenField ID="hfofferimage" runat="server" />
</asp:Content>

