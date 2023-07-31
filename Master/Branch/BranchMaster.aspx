<%@ Page Title="Branch Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true"
    CodeFile="BranchMaster.aspx.cs" Inherits="Master_BranchMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterPage" runat="Server">

    <style>
        .tab {
            display: none;
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
                background-color: #2296f3;
            }

        .WrapDiv {
            display: block;
            position: relative;
            font-size: 1.3rem;
            border: 2px #6c757d solid;
            text-align: center;
            padding: 3px;
            color: #6c757d;
            font-family: inherit;
            font-weight: 600;
            border-radius: 5px;
            width: 100px;
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
            width: 75%;
            text-align: center;
            color: black;
            font-size: 12px;
            font-weight: 500;
        }

        .divImg {
            /*width: 150px;*/
            position: relative;
            border-radius: 40px;
        }


            .divImg:hover {
                box-shadow: 0 0 10px #7a7a7ac4;
                cursor: pointer;
            }

        .rbl input[type="radio"] {
            margin-left: 10px;
            margin-right: 6px;
        }

        .lblView {
            font-size: 10px;
            font-weight: 900;
        }

        .lblSameTime {
            font-size: 13px;
            font-weight: 900;
        }

            .lblSameTime:hover {
                transform: scale(1.05);
            }

        .lblSameTimeClick {
            font-size: 13px;
            font-weight: 900;
            color: darkred;
            text-decoration: underline;
        }

        label {
            display: inline-block;
            margin-bottom: 0.5rem;
            padding-left: 4px;
        }
        /*
        tbody tr td span {
            max-width: 150px !important;
             width: 96% !important; 
            white-space: pre-line;
        }*/

        .Daybtn {
            font-size: 15px;
            background-color: #e9ecef;
            border-radius: 25px;
            color: black;
            cursor: pointer;
            margin: 5px;
            box-shadow: rgba(17, 17, 26, 0.1) 0px 4px 16px, rgba(17, 17, 26, 0.05) 0px 8px 32px;
            display: inline;
            padding: 6px 8px;
            text-align: center;
            text-decoration: none;
            transition: all 250ms;
            border: 0;
            -webkit-user-select: none;
            touch-action: manipulation;
        }

            .Daybtn:hover {
                transform: scale(1.05);
            }

        /*.DaybtnClick {
            background-color: #1ca4ff;
            border-radius: 25px;
            color: white;
            cursor: pointer;
            margin: 7px;
            box-shadow: rgba(17, 17, 26, 0.1) 0px 4px 16px, rgba(17, 17, 26, 0.05) 0px 8px 32px;
            display: inline;
            font-family: CerebriSans-Regular,-apple-system,system-ui,Roboto,sans-serif;
            padding: 7px 10px;
            text-align: center;
            text-decoration: none;
            transition: all 250ms;
            border: 0;
            -webkit-user-select: none;
            touch-action: manipulation;
        }*/

        .DaybtnClick {
            font-size: 15px;
            background-color: #1ca4ff;
            border-radius: 25px;
            color: white;
            cursor: pointer;
            margin: 5px;
            box-shadow: rgb(17 17 26 / 10%) 0px 4px 16px, rgb(17 17 26 / 5%) 0px 8px 32px;
            display: inline;
            padding: 6px 8px;
            text-align: center;
            text-decoration: none;
            transition: all 250ms;
            border: 0;
            -webkit-user-select: none;
            touch-action: manipulation;
        }

            .DaybtnClick:hover {
                transform: scale(1.05);
            }

        legend {
            display: block;
            width: inherit;
            max-width: 100%;
            padding: 0px 5px;
            font-size: inherit;
            font-weight: bold;
            line-height: inherit;
            color: inherit;
            white-space: normal;
        }

        .legBorder {
            border-width: 1px;
            border-color: #2196f373;
            border-radius: 6px;
            border-style: dashed;
            padding: 6px;
        }

        .modal-header {
            align-items: baseline;
            display: flex;
            justify-content: space-between;
        }

        .close {
            background: none;
            border: none;
            cursor: pointer;
            display: flex;
            height: 16px;
            text-decoration: none;
            width: 16px;
            box-sizing: unset;
        }

            .close svg {
                width: 16px;
                box-sizing: unset;
            }

        .modal-wrapper {
            align-items: center;
            background: rgba(0, 0, 0, 0.7);
            bottom: 0;
            display: flex;
            justify-content: center;
            left: 0;
            position: fixed;
            right: 0;
            top: 0;
        }

        .modal-body {
            opacity: 1 !important;
            transform: translateY(1);
        }

        .model {
            opacity: 1;
            visibility: visible;
            z-index: 5;
        }


        .modal-body {
            max-width: 500px;
            opacity: 0;
            transform: translateY(-100px);
            transition: opacity 0.25s ease-in-out;
            width: 100%;
            z-index: 5;
        }

        .cardModal {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 32px;
            margin: 234px;
            max-width: 616px;
            width: 100%;
            height: auto;
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol" Text="Branch"></asp:Label>
            </div>
            <div>
                <span class="iconify" data-icon="mdi:slash-forward"></span>
            </div>
            <div>
                <asp:Label ID="lblNavThird" runat="server" CssClass="pageRoutecol" Text="Branch Master"></asp:Label>
            </div>

        </div>
        <div id="divForm" runat="server" visible="false">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title"><span id="spAddorEdit" runat="server"></span>Branch <span class="Card-title-second">Master </span></h4>
                </div>
            </div>
            <div class="tab">

                <div class="row">
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblOwner" runat="server" Text="Parking Owner Name"
                            CssClass="form-check-label">
                        </asp:Label><span class="spanStar">*</span>
                        <asp:DropDownList ID="ddlownername" runat="server"
                            CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                            ValidationGroup="BranchMaster"
                            ControlToValidate="ddlownername"
                            runat="server" CssClass="rfvClr" InitialValue="0"
                            ErrorMessage="Select Parking Owner Name"> </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblBranchName" runat="server" Text="Parking Branch Name"
                            CssClass="form-check-label">
                        </asp:Label><span class="spanStar">*</span>
                        <asp:TextBox ID="txtBranchName" runat="server" CssClass="form-control"
                            AutoComplete="off" MaxLength="50" TabIndex="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBranchName" ValidationGroup="BranchMaster" ControlToValidate="txtBranchName" runat="server" CssClass="rfvClr" class="txt"
                            ErrorMessage="Enter Parking Branch Name"> </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblBranchShortName" runat="server" Text="Short Name" CssClass="form-check-label">
                        </asp:Label><span class="spanStar">*</span>
                        <asp:TextBox ID="txtBranchShortName" runat="server" CssClass="form-control" MaxLength="50" TabIndex="3"
                            AutoComplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv1" ValidationGroup="BranchMaster" ControlToValidate="txtBranchShortName"
                            runat="server" CssClass="rfvClr" ErrorMessage="Enter Short Name"> </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8">
                        <fieldset class="legBorder mb-3">
                            <legend>Address</legend>
                            <div>
                                <div class="row">
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label ID="lblAddress1" runat="server" Text="Address Line 1"
                                            CssClass="form-check-label">
                                        </asp:Label>
                                        <asp:TextBox ID="txtAddress1" runat="server" TextMode="MultiLine" CssClass="form-control" AutoComplete="off" MaxLength="50"
                                            TabIndex="4"></asp:TextBox>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label ID="lblAddress2" runat="server" Text="Address Line 2" CssClass="form-check-label"></asp:Label>
                                        <asp:TextBox ID="txtAddress2" runat="server" TextMode="MultiLine" CssClass="form-control" AutoComplete="off" MaxLength="50"
                                            TabIndex="5"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                        <asp:Label ID="lblPincode" runat="server" Text="Pincode" CssClass="form-check-label"></asp:Label><span class="spanStar">*</span>
                                        <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" MaxLength="6" onkeypress="return isNumber(event);"
                                            onchange="myFunction()" AutoComplete="off" ValidationGroup="BranchMaster" TabIndex="6"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="BranchMaster" ControlToValidate="txtPincode" runat="server" CssClass="rfvClr"
                                            ErrorMessage="Enter Pincode"> </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" id="CityDetails" runat="server">
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
                        </fieldset>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <div class="panel panel-success">
                            <div class="panel-heading">Branch Image</div>
                            <div class="panel-body">
                                <asp:FileUpload ID="fupEmpLink" runat="server" Style="display: none" TabIndex="7" onchange="ShowImagePreview(this);" />
                                <div id="divImgPreview" style="position: relative;">
                                    <asp:Image class="divImg" ID="imgEmpPhotoPrev" runat="server" TabIndex="7" alt="Branch Image"
                                        ImageUrl="../../images/EmptyImageNew.png" Width="75%" />
                                    <div class="imageOverlay">Click To Upload</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblPhoneNo" runat="server" Text="Phone No."
                            CssClass="form-check-label">
                        </asp:Label><span class="spanStar">*</span>
                        <asp:TextBox ID="txtPhoneNo" runat="server" onkeypress="return isNumber(event);" CssClass="form-control" AutoComplete="off"
                            MaxLength="10" TabIndex="8"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv10" ValidationGroup="BranchMaster" ControlToValidate="txtPhoneNo"
                            runat="server" CssClass="rfvClr" ErrorMessage="Enter Phone No."> </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                            ControlToValidate="txtPhoneNo" ErrorMessage="Invalid Phone No."
                            ValidationExpression="[0-9]{10}" CssClass="rfvClr" ValidationGroup="BranchMaster"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblalterPhone" runat="server" Text="Alternate Phone No." CssClass="form-check-label"></asp:Label>
                        <asp:TextBox ID="txtAlterPhoneNo" runat="server" CssClass="form-control" TabIndex="9"
                            onkeypress="return isNumber(event);" AutoComplete="off" MaxLength="10"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                            ControlToValidate="txtAlterPhoneNo" ErrorMessage="Invalid Phone No."
                            ValidationExpression="[0-9]{10}" CssClass="rfvClr" ValidationGroup="BranchMaster"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblEmailId" runat="server" Text="Email Id" CssClass="form-check-label"></asp:Label>
                        <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" AutoComplete="off"
                            TabIndex="10"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RefExpVal1" runat="server" ControlToValidate="txtEmailId"
                            ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            CssClass="rfvClr" ErrorMessage="Enter Valid EmailId">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <div id="chkTimslab">
                            <asp:CheckBox ID="chkMultiBook" runat="server" AutoComplete="off" TabIndex="11" />
                            <asp:Label ID="lblMultiBook" runat="server" Text="Multiple Days Booking" CssClass="form-check-label">
                            </asp:Label>
                        </div>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <div id="chkOnline" style="margin-left: 14px">
                            <asp:CheckBox ID="chkOnlineB" runat="server" AutoComplete="off" TabIndex="12" />
                            <asp:Label ID="lblOnline" runat="server" Text="Online Booking" CssClass="form-check-label">
                            </asp:Label>
                        </div>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <div id="chkpayBook" style="margin-left: 14px">
                            <asp:CheckBox ID="chkPayB" runat="server" AutoComplete="off" TabIndex="13" />
                            <asp:Label ID="lblPayB" runat="server" Text="Pay & Book" CssClass="form-check-label">
                            </asp:Label>
                        </div>
                    </div>
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <div id="chkBpaylater" style="margin-left: 14px">
                            <asp:CheckBox ID="chkPaylater" runat="server" AutoComplete="off" TabIndex="14" />
                            <asp:Label ID="lblPaylater" runat="server" Text="Reservation" CssClass="form-check-label">
                            </asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row mt-4">
                    <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <div id="chkBcheckIn">
                            <asp:CheckBox ID="chkChechIn" runat="server" AutoComplete="off" TabIndex="15" />
                            <asp:Label ID="lblBchkIn" runat="server" Text="Book & Check In" CssClass="form-check-label">
                            </asp:Label>
                        </div>
                    </div>
                    <div class="col-12 col-sm-9 col-md-9 col-lg-9 col-xl-9">
                        <div id="chkcheckout" style="margin-left: 6px">
                            <%--<asp:CheckBox ID="chkBCheckout" runat="server" AutoComplete="off" TabIndex="16" />
                            <asp:Label ID="Label3" runat="server" Text="Pay At Check Out" CssClass="form-check-label">
                            </asp:Label>
                        </div>
                        <div>--%>
                            <asp:RadioButtonList ID="rbtnPayType" runat="server" TabIndex="16"
                                RepeatDirection="Horizontal" Style="height: 2.8rem;"
                                CssClass="form-check-label rbtnList">
                                <asp:ListItem Value="PCI">Pay At Check In</asp:ListItem>
                                <asp:ListItem style="margin-left: 7rem!important;" Value="PCO">Pay At Check Out</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="row mt-2">
                    <span style="color: red; font-size: 13px; font-weight: bold;">** Note : User can select only either "Pay At Check In" (or) "Pay At Check Out"</span>
                    <span style="color: red; font-size: 13px; font-weight: bold; margin-left: 5.1rem;">"Pay At Check In" refers to collection of Amount only at "Check In"</span>
                    <span style="color: red; font-size: 13px; font-weight: bold; margin-left: 5.1rem;">"Pay At Check Out" refers to collection of Amount only at "Check Out"</span>
                </div>
            </div>

            <div class="tab">
                <fieldset class="legBorder mb-3">
                    <legend>Documents</legend>
                    <div class="row">
                        <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                            <asp:Label ID="lblLicenseNo" runat="server" Text="License No."
                                CssClass="form-check-label">
                            </asp:Label><span class="spanStar">*</span>
                            <asp:TextBox ID="txtLicenseNo" runat="server" CssClass="form-control" AutoComplete="off" MaxLength="20"
                                TabIndex="24"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13"
                                ValidationGroup="BranchMasterNxt"
                                ControlToValidate="txtLicenseNo"
                                runat="server" CssClass="rfvClr"
                                ErrorMessage="Enter License No."> </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                            <asp:Label ID="lblLicensePeriodFrom" runat="server" Text="License Period From"
                                CssClass="form-check-label"></asp:Label><span class="spanStar">*</span>
                            <asp:TextBox ID="txtLicensePeriodFrom" runat="server" CssClass="form-control fromDate " AutoComplete="off"
                                TabIndex="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ValidationGroup="BranchMasterNxt" ControlToValidate="txtLicensePeriodFrom" runat="server" CssClass="rfvClr"
                                ErrorMessage="Enter Period From"> </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                            <asp:Label ID="lblLicensePeriodTo" runat="server" Text="License Period To"
                                CssClass="form-check-label"></asp:Label><span class="spanStar">*</span>
                            <asp:TextBox ID="txtLicensePeriodTo" runat="server" CssClass="form-control toDate" AutoComplete="off" onchange="comparedate()"
                                TabIndex="26"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ValidationGroup="BranchMasterNxt" ControlToValidate="txtLicensePeriodTo" runat="server" CssClass="rfvClr"
                                ErrorMessage="Enter Period To"> </asp:RequiredFieldValidator>
                        </div>

                    </div>
                    <div class="row col-12 col-xs-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <div class="row">
                                <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <asp:Label ID="lblLicense" runat="server" Text="License Document" CssClass="form-check-label"> </asp:Label><span class="spanStar">*</span>
                                    <span class="WrapDiv" id="WrapDiv1" runat="server">Choose File
                        <asp:FileUpload ID="txtLicense" runat="server" onchange="ShowLicence(this);" />
                                    </span>
                                    <asp:ImageButton ID="LicenceImage" runat="server" Visible="false" ImageUrl="~/images/Document.png" Width="14%" OnClientClick="LicenceImage(event)" />
                                    <asp:Label ID="lbllicenseview" CssClass="lblView" runat="server"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="BranchMasterNxt" ControlToValidate="txtLicense" runat="server" CssClass="rfvClr"
                                        ErrorMessage="Upload License"> </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.doc|.docx|.pdf|.png|.jpg)$"
                                        ControlToValidate="txtLicense" runat="server" CssClass="rfvClr" ErrorMessage="select a valid Image, Word or PDF File."
                                        Display="Dynamic" />
                                </div>
                                <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6" runat="server" visible="false">
                                    <asp:Label ID="lbllicenseviewText" runat="server" Text="License Doc. FileName"></asp:Label>
                                </div>

                            </div>

                        </div>
                        <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                            <div class="row">
                                <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <asp:Label ID="lblOthrDocFile" runat="server" Text="Other Document Files" Font-Bold="true" CssClass="form-check-label"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <asp:Label ID="lblDocument1" runat="server" Text="Document File 1" CssClass="form-check-label"></asp:Label>
                                    <span class="WrapDiv" id="WrapDiv2" runat="server">Choose File
                        <asp:FileUpload ID="txtDocument1" runat="server" onchange="Showdocument1(this);" />
                                    </span>
                                    <asp:ImageButton ID="Document1Image" runat="server" Visible="false" ImageUrl="~/images/Document.png" Width="10%" OnClientClick="Document1Image(event)" />
                                    <asp:Label ID="lblDoc1View" CssClass="lblView" runat="server"></asp:Label>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.doc|.docx|.pdf|.png|.jpg)$"
                                        ControlToValidate="txtDocument1" runat="server" CssClass="rfvClr" ErrorMessage="select a valid Image, Word or PDF File."
                                        Display="Dynamic" />
                                </div>
                                <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6" runat="server" visible="false">
                                    <asp:Label ID="lblDoc1" runat="server" Text="Doc.1 Name"></asp:Label>

                                </div>
                                <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                                    <asp:Label ID="lblDocument2" runat="server" Text="Document File 2" CssClass="form-check-label"></asp:Label>
                                    <span class="WrapDiv" id="WrapDiv3" runat="server">Choose File
                        <asp:FileUpload ID="txtDocument2" runat="server" onchange="Showdocument2(this);" />
                                    </span>
                                    <asp:ImageButton ID="Document2Image" runat="server" Visible="false" ImageUrl="~/images/Document.png" Width="10%" OnClientClick="Document2Image(event)" />
                                    <asp:Label ID="lblDoc2View" CssClass="lblView" runat="server"></asp:Label>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.doc|.docx|.pdf|.png|.jpg)$"
                                        ControlToValidate="txtDocument2" runat="server" CssClass="rfvClr" ErrorMessage="select a valid Image, Word or PDF File."
                                        Display="Dynamic" />
                                </div>
                                <div class="col-6 col-xs-6 col-sm-6 col-md-6 col-lg-6 col-xl-6" runat="server" visible="false">
                                    <asp:Label ID="lblDoc2" runat="server" Text="Doc.2 Name"></asp:Label>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                            <asp:Label
                                ID="lblGSTNumber"
                                runat="server"
                                Text="GST Number"
                                CssClass="form-check-label">
                            </asp:Label>
                            <asp:TextBox
                                ID="txtGSTNumber"
                                runat="server"
                                TabIndex="17"
                                MaxLength="15"
                                CssClass="form-control">
                            </asp:TextBox>
                        </div>
                    </div>
                </fieldset>
            </div>
            <asp:ScriptManager ID="ScriptManager" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1"
                UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <fieldset>

                        <div class="tab">
                            <div class="row">
                                <div class="ml-4">
                                    <div>
                                        <asp:Label
                                            ID="lblWorkingDays"
                                            runat="server"
                                            Text="Working Days">
                                        </asp:Label><span class="spanStar">*</span>
                                    </div>
                                    <asp:Label runat="server" ID="lblWorkingDaysValue" Visible="false"></asp:Label>
                                    <div class="mt-3 mb-4 ml-5">
                                        <asp:Label ID="Label1" CssClass="lblSameTime" runat="server"> 
                                      *  Does All days are Working days ?</asp:Label>
                                        <asp:LinkButton ID="LnkSameTime" CssClass="lblSameTimeClick" runat="server"
                                            OnClick="LnkSameTime_Click">Click Here</asp:LinkButton>
                                    </div>
                                    <asp:CheckBoxList ID="chkWorkingDays" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="chkWorkingDays_SelectedIndexChanged">
                                        <asp:ListItem class="Daybtn" Value="1">Sunday</asp:ListItem>
                                        <asp:ListItem class="Daybtn" Value="2">Monday</asp:ListItem>
                                        <asp:ListItem class="Daybtn" Value="3">Tuesday</asp:ListItem>
                                        <asp:ListItem class="Daybtn" Value="4">Wednesday</asp:ListItem>
                                        <asp:ListItem class="Daybtn" Value="5">Thursday</asp:ListItem>
                                        <asp:ListItem class="Daybtn" Value="6">Friday</asp:ListItem>
                                        <asp:ListItem class="Daybtn" Value="7">Saturday</asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>

                            </div>
                            <div id="DivHrDetails" runat="server" visible="false">
                                <div class="row mt-5">

                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4">
                                        <asp:Label
                                            ID="lblfrmtime"
                                            runat="server"
                                            Text="From Time">
                                        </asp:Label><span class="spanStar">*</span>
                                        <asp:TextBox
                                            ID="txtfrmtime"
                                            runat="server"
                                            TabIndex="3"
                                            onkeypress="return isNumber(event);"
                                            MaxLength="5"
                                            CssClass="form-control timePicker classTargetTime">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvfrmtime"
                                            ValidationGroup="BranchMasterNxt"
                                            ControlToValidate="txtfrmtime"
                                            runat="server" CssClass="rfvClr"
                                            ErrorMessage="Select From Time">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4">
                                        <asp:Label
                                            ID="lbltotime"
                                            runat="server"
                                            Text="To Time">
                                        </asp:Label><span class="spanStar">*</span>
                                        <asp:TextBox
                                            ID="txttotime"
                                            runat="server"
                                            TabIndex="4"
                                            onkeypress="return isNumber(event);"
                                            MaxLength="5"
                                            CssClass="form-control timePicker classTargetTime">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtotime"
                                            ValidationGroup="BranchMasterNxt"
                                            ControlToValidate="txttotime"
                                            runat="server" CssClass="rfvClr"
                                            ErrorMessage="Select To Time">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="tab">
                <div class="row">
                </div>
                <div class="row">
                    <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblLatitude" runat="server" Text="Latitude" CssClass="form-check-label"></asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                            TabIndex="27"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="BranchMasterSub" ControlToValidate="txtLatitude" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter Latitude"> </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lblLongitude" runat="server" Text="Longitude" CssClass="form-check-label"></asp:Label>
                        <span class="spanStar">*</span>
                        <asp:TextBox ID="txtLongitude" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                            TabIndex="28"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="BranchMasterSub" ControlToValidate="txtLongitude" runat="server" CssClass="rfvClr"
                            ErrorMessage="Enter Longitude"> </asp:RequiredFieldValidator>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-12" id="dvMapget" style="height: 450px;">
                </div>
            </div>

            <div class="row justify-content-end justify-content-sm-end justify-content-md-end justify-content-lg-end justify-content-xl-end  mt-4">
                <div>
                    <asp:Button runat="server" CssClass="pure-material-button-contained prevBtn mr-2" TabIndex="29" ID="prevBtn" Text="Previous" OnClientClick="nextPrev(-1,event)" />
                </div>
                <div>
                    <asp:Button runat="server" CssClass="pure-material-button-contained nextBtn mr-2" TabIndex="30" Text="Next" ID="nextBtn" OnClientClick="nextPrev(1,event)" ValidationGroup="BranchMaster" />
                </div>
                <div>
                    <asp:Button runat="server" CssClass="pure-material-button-contained nextBtn mr-2" TabIndex="31" Text="Next" ID="btnnxt" OnClientClick="nextPrev(1,event)" ValidationGroup="BranchMasterNxt" />
                </div>
                <div>
                    <asp:Button ID="btnSubmit" CssClass="pure-material-button-contained btnBgColorAdd mr-2" TabIndex="32" Text="Submit"
                        ValidationGroup="BranchMasterSub" runat="server" OnClick="btnSubmit_Click" />
                </div>
                <div>
                    <asp:Button ID="btnReset" runat="server" CssClass="pure-material-button-contained btnBgColorCancel mr-2" TabIndex="33"
                        Text="Cancel" CausesValidation="false" OnClick="btnReset_Click" />
                </div>
            </div>
            <div style="text-align: center; margin-top: 40px;">
                <span class="step"></span>
                <span class="step"></span>
                <span class="step"></span>
                <span class="step"></span>

            </div>
        </div>
        <div id="divGv" runat="server">
            <div class="divTitle row pt-4 pl-4 justify-content-between">
                <div>
                    <h4 class="card-title">Branch <span class="Card-title-second">Master </span></h4>
                </div>
                <div>
                    <%--onclientclick="return Map()"--%>
                    <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="font-size: 16px">
                      <i class="mdi mdi-plus" style="font-size:16px"></i> Add</asp:LinkButton>
                    <%-- <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="add +" CssClass="pure-material-button-contained btnBgColorAdd" />--%>
                </div>
            </div>
            <div id="divGridView" runat="server" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="ddwnoverflow">
                    <div class="table-responsive section">
                        <asp:GridView
                            ID="gvBranchmaster"
                            runat="server"
                            AllowPaging="True"
                            DataKeyNames="branchId"
                            CssClass="gvv display"
                            AutoGenerateColumns="false"
                            BorderStyle="None"
                            PageSize="1000" OnRowDataBound="gvBranchmaster_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblbranchId"
                                            runat="server"
                                            Text='<%#Bind("branchId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ParkingOwner Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblparkingOwnerId"
                                            runat="server"
                                            Text='<%#Bind("parkingOwnerId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parking Name">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblownerName"
                                            runat="server"
                                            Text='<%#Bind("parkingName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Name">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblbranchName"
                                            runat="server"
                                            Text='<%#Bind("branchName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Short Name" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblshortName"
                                            runat="server"
                                            Text='<%#Bind("shortName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Latitude & Longitude" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lbllatitude"
                                            runat="server"
                                            Text='<%#Eval("latitude").ToString() + ":" + Eval("longitude").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address & Location" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblAddress1"
                                            runat="server"
                                            Text='<%#Eval("address1").ToString() + ":" + Eval("address2").ToString() + ":" + Eval("district").ToString() + ":" + Eval("state").ToString() + ":" + Eval("city").ToString() + ":" + Eval("pincode").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="150px"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PhoneNumber & EmailId" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblphoneNumber"
                                            runat="server"
                                            Text='<%#Eval("phoneNumber").ToString() + ":" + Eval("alternatePhoneNumber").ToString() + ":" + Eval("emailId")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="License Details" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblGvGSTNumber"
                                            runat="server"
                                            Text='<%#Bind("gstNumber") %>'>
                                        </asp:Label>
                                        <asp:Label
                                            ID="lbllicenseNo"
                                            runat="server"
                                            Text='<%#Eval("licenseNo").ToString() + ":" + Convert.ToDateTime(Eval("licensePeriodFrom")).ToString("yyyy-MM-dd") + ":"+ Convert.ToDateTime(Eval("licensePeriodTo")).ToString("yyyy-MM-dd") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Multiple Days Booking" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblmultiBook"
                                            runat="server"
                                            Text='<%#Bind("multiBook") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="online Booking" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblonlineBookingAvailability"
                                            runat="server"
                                            Text='<%#Bind("onlineBookingAvailability") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pay Book Available" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblisPayBookAvailable"
                                            runat="server"
                                            Text='<%#Bind("isPayBookAvailable") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="isBookCheckInAvailable" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblisBookCheckInAvailable"
                                            runat="server"
                                            Text='<%#Bind("isBookCheckInAvailable") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="isPayAtCheckoutAvailable" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblisPayAtCheckoutAvailable"
                                            runat="server"
                                            Text='<%#Bind("isPayAtCheckoutAvailable") %>'></asp:Label>
                                        <asp:Label
                                            ID="lblisPayAtCheckInAvailable"
                                            runat="server"
                                            Text='<%#Bind("isPayAtCheckInAvailable") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="isPayLaterAvaialble" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblisPayLaterAvaialble"
                                            runat="server"
                                            Text='<%#Bind("isPayLaterAvaialble") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Approval Status" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblapprovalStatus"
                                            runat="server"
                                            Text='<%#Bind("approvalStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Multiple Days Booking" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblurl"
                                            runat="server"
                                            Text='<%#Eval("license") + "~" + Eval("document1") + "~"+ Eval("document2") %>'>

                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ImageDetails" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:DataList runat="server" ID="DataList1" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:Label ID="lblimageId" runat="server" Text='<%# Bind("imageId") %>' Font-Bold="true" Width="100px"></asp:Label>
                                                <asp:Label ID="lblimageUrl" runat="server" Text='<%# Bind("imageUrl") %>' Font-Bold="true" Width="100px" Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Working Hrs" Visible="false" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:DataList runat="server" ID="dlWorkingHrs" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:Label
                                                    ID="lblgvworkingDay"
                                                    runat="server"
                                                    Text='<%#Bind("workingDay") %>'></asp:Label>
                                                <asp:Label
                                                    ID="lblgvfromTime"
                                                    runat="server"
                                                    Text='<%#Bind("fromTime") %>'></asp:Label>
                                                <asp:Label
                                                    ID="lblgvtoTime"
                                                    runat="server"
                                                    Text='<%#Bind("toTime") %>'></asp:Label>
                                                <asp:Label
                                                    ID="lblgvisHoliday"
                                                    runat="server"
                                                    Text='<%#Bind("isHoliday") %>'></asp:Label>
                                                <asp:Label
                                                    ID="lblgvuniqueId"
                                                    runat="server"
                                                    Text='<%#Bind("uniqueId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Phone No." HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:Label
                                            ID="lblphoneNo"
                                            runat="server"
                                            Text='<%#Bind("phoneNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="gvHeader">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="LnkEdit"
                                            runat="server"
                                            Text="Edit"
                                            src="../../images/edit-icon.png" alt="image" OnClientClick="showLoader();"
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
                                            Text='<%#Eval("activeStatus").ToString() =="A"?"Active":"Inactive"%>'
                                            OnClick="lnkActiveOrInactive_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approval Status" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="lblGvapprovalStatus"
                                            runat="server"
                                            CssClass='<%#Eval("approvalStatus").ToString() =="A"?"btnActive":"btnInActive"%>'
                                            Text='<%#Eval("approvalStatus").ToString() == "A" ? "Approved" : "Waiting List" %>'
                                            Enabled='<%#Eval("approvalStatus").ToString() == "A" ? false : true %>'
                                            OnClick="lblGvapprovalStatus_Click">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View" HeaderStyle-CssClass="gvHeader" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton
                                            ID="lnkView"
                                            runat="server"
                                            Width="45px"
                                            src="../../images/View.png"
                                            alt="View"
                                            Text="Edit"
                                            Visible='<%#Eval("activeStatus").ToString() =="A"?true:false%>' OnClick="lnkView_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="modal-wrapper model" id="modalCard" runat="server" visible="false">
        <div class="modal-body cardModal">
            <div class="modal-header">
                <h4 class="card-title" style="font-weight: bold">Approval Status</h4>
                <asp:ImageButton ID="ImgClosed" runat="server"
                    OnClick="ImgClosed_Click" ImageUrl="~/images/Close.svg" Width="25px"></asp:ImageButton>
            </div>

            <div class="row" style="margin-left: -1px !important; margin-top: 10px;">
                <div>
                    <asp:Button
                        ID="btnApprove"
                        runat="server"
                        Text="Approve"
                        OnClick="btnApprove_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorAdd" />
                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="Cancel"
                        OnClick="btnCancel_Click"
                        CausesValidation="false"
                        CssClass="pure-material-button-contained btnBgColorAdd" Style="background-color: red" />
                </div>


            </div>
            <div class="row" style="margin-left: -1px !important; margin-top: 10px;">
                <div id="divCancellationReason" runat="server" class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6" style="margin-top: 20px;">
                    <asp:Label
                        ID="lblcancel"
                        runat="server"
                        Text="Cancellation Reason" CssClass="form-check-label">
                    </asp:Label>

                    <asp:TextBox ID="txtReason" TextMode="MultiLine"
                        Height="50px" runat="server" CssClass="form-control section labels"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Visible="false" ControlToValidate="txttc" runat="server" CssClass="rfvClr"
                        ErrorMessage="Enter About Offer">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row justify-content-end" id="divsubmit" runat="server" visible="false">
                <div class="mr-3">
                    <asp:Button ID="btnSubmitPopup" CssClass="pure-material-button-contained btnBgColorAdd"
                        Text="Submit" ValidationGroup="Check" runat="server" OnClick="btnSubmitPopup_Click" />
                </div>
                <div>
                    <asp:Button ID="btnCancelPopup" CssClass="pure-material-button-contained btnBgColorCancel"
                        Text="Cancel" CausesValidation="false" OnClick="btnCancelPopup_Click"
                        runat="server" />
                </div>
            </div>


        </div>
    </div>
    <%--Hidden Field Values--%>
    <asp:HiddenField ID="hflongitude" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hflatitude" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfWorkingHrsuniqueId" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfImageUrl" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfLicence" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfdocument1" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfdocument2" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hfView" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfParkingOwnerId" runat="server" />
    <asp:HiddenField ID="hfState" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfDistrict" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hfCity" runat="server" EnableViewState="true" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <%-- Upload for Branch Image--%>
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
                            $('#<%=fupEmpLink.ClientID%>').val("1");
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
                                $('#<%=hfImageUrl.ClientID%>').val(data.response);
                                console.log(data.response, 'BranchImage');
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
    <%-- Upload for Licence--%>
    <script type="text/javascript">
        function ShowLicence(input) {

            this.formData1 = new FormData();
            formData1.append("image", $('input[type=file]')[1].files[0])
            //var file = files[0]
            //alert(file)
            $.ajax({
                url: '<%= Session["ImageUrl"].ToString() %>',
                type: 'POST',
                data: formData1,
                contentType: false,
                processData: false,
                success: function (data) {
                    $('#<%=hfLicence.ClientID%>').val(data.response);
                    var fieldid = [];
                    fieldid = data.response.split('=');
                    $('#<%=lbllicenseview.ClientID%>').text(fieldid[1]);
                },
            });
        }
    </script>
    <%-- Upload for Document1--%>
    <script type="text/javascript">
        function Showdocument1(input) {
            var formData2 = new FormData()
            formData2.append("image", $('input[type=file]')[2].files[0])
            $.ajax({
                url: '<%= Session["ImageUrl"].ToString() %>',
                type: 'POST',
                data: formData2,
                contentType: false,
                processData: false,
                success: function (data) {
                    $('#<%=hfdocument1.ClientID%>').val(data.response);
                    var fieldid = [];
                    fieldid = data.response.split('=');
                    $('#<%=lblDoc1View.ClientID%>').text(fieldid[1]);
                },
            });
        }
    </script>
    <%-- Upload for Document2--%>
    <script type="text/javascript">
        function Showdocument2(input) {
            var formData3 = new FormData()
            formData3.append("image", $('input[type=file]')[3].files[0])
            $.ajax({
                url: '<%= Session["ImageUrl"].ToString() %>',
                type: 'POST',
                data: formData3,
                contentType: false,
                processData: false,
                success: function (data) {
                    $('#<%=hfdocument2.ClientID%>').val(data.response);
                    var fieldid = [];
                    fieldid = data.response.split('=');
                    $('#<%=lblDoc2View.ClientID%>').text(fieldid[1]);
                },
            });
        }
    </script>
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/4.5.1/1/MicrosoftAjax.js" type="text/javascript"></script>

    <%--Script for the Next Tab--%>
    <script>
        var currentTab = 0; // Current tab is set to be the first tab (0)
        showTab(currentTab); // Display the current tab

        function showTab(n) {
            // This function will display the specified tab of the form...
            var x = document.getElementsByClassName('tab');
            if (x.length != 0) {
                x[n].style.display = 'Block';

                // x[n].style.display = "block";

                //... and fix the Previous/Next buttons:
                if (n == 0) {
                    //alert("1")
                    document.getElementById('<%=prevBtn.ClientID %>').style.display = "none";
                    document.getElementById('<%=btnSubmit.ClientID %>').style.display = "none";
                    document.getElementById('<%=btnnxt.ClientID %>').style.display = "none";
                    document.getElementById('<%=nextBtn.ClientID %>').style.display = "inline";
                } else {
                    //alert("2")
                    document.getElementById('<%=prevBtn.ClientID %>').style.display = "inline";
                    document.getElementById('<%=nextBtn.ClientID %>').style.display = "none";
                    document.getElementById('<%=btnnxt.ClientID %>').style.display = "inline";
                    document.getElementById('<%=btnSubmit.ClientID %>').style.display = "none";
                }
                if (n == (x.length - 1)) {
                    //alert("3")
                    document.getElementById('<%=nextBtn.ClientID %>').style.display = "none";
                document.getElementById('<%=btnnxt.ClientID %>').style.display = "none";
                document.getElementById('<%=btnSubmit.ClientID %>').style.display = "inline";
                var View = document.getElementById('<%=hfView.ClientID %>').value;
                if (View == "1") {
                    document.getElementById('<%=btnSubmit.ClientID %>').style.display = "none";
                    }
                } <%--else {
                alert("4")
                //document.getElementById('<%=nextBtn.ClientID %>').value = "Next";
                document.getElementById('<%=nextBtn.ClientID %>').style.display = "inline";
                document.getElementById('<%=btnSubmit.ClientID %>').style.display = "none";
                document.getElementById('<%=btnnxt.ClientID %>').style.display = "none"
            }--%>

                //... and run a function that will display the correct step indicator:
                fixStepIndicator(n)
            }

        }

        function nextPrev(n, e) {
            /* */
            e.preventDefault();
            // This function will figure out which tab to display
            var x = document.getElementsByClassName("tab");
            // Exit the function if any field in the current tab is invalid:
            if (n == 1 && !validateForm()) return false;
            // Hide the current tab:
            x[currentTab].style.display = "none";
            // Increase or decrease the current tab by 1:
            currentTab = currentTab + n;
            // if you have reached the end of the form...
            if (currentTab >= x.length) {
                // ... the form gets submitted:
                document.getElementById("regForm").submit();
                return false;
            }
            // Otherwise, display the correct tab:
            showTab(currentTab);
            const datepicker = document.getElementsByClassName('datePicker');
            const fromDate = document.getElementsByClassName('fromDate');
            const toDate = document.getElementsByClassName('toDate');
            const timePicker = document.getElementsByClassName('timePicker');
            const daterangepicker = document.getElementsByClassName('daterangepicker');

            let date = new Date();
            var Todate;
            let fp = flatpickr(daterangepicker,
                {
                    mode: "range",
                    minDate: "today",
                    dateFormat: "Y-m-d",
                    altFormat: "d-m-Y",
                    altInput: true,

                });

            fp = flatpickr(datepicker,
                {
                    enableTime: false,
                    dateFormat: "Y-m-d",
                    altFormat: "d-m-Y",
                    altInput: true,
                    time_24hr: false,
                    minDate: "today",

                    onOpen: function () {
                        const numInput = document.querySelectorAll('.numInput');
                        numInput.forEach((input) => input.type = '');
                    }

                });

            fp = flatpickr(toDate,
                {
                    enableTime: false,
                    //dateFormat: "d-m-Y",
                    dateFormat: "Y-m-d",
                    altFormat: "d-m-Y",
                    altInput: true,
                    time_24hr: false,

                });

            fp = flatpickr(fromDate,
                {
                    enableTime: false,
                    dateFormat: "Y-m-d",
                    altFormat: "d-m-Y",
                    altInput: true,
                    time_24hr: false,

                    onOpen: function () {
                        const numInput = document.querySelectorAll('.numInput');
                        numInput.forEach((input) => input.type = '');
                    },
                    onChange: function (selectedDates, dateStr, instance) {
                        toDate[0].value = '';
                        flatpickr(toDate,
                            {
                                enableTime: false,
                                //dateFormat: "d-m-Y",
                                dateFormat: "Y-m-d",
                                altFormat: "d-m-Y",
                                altInput: true,
                                minDate: selectedDates,

                                time_24hr: false,
                                minDate: dateStr
                            });
                    },
                });

            fp = flatpickr(timePicker,
                {
                    enableTime: true,
                    noCalendar: true,
                    time_24hr: true,
                    dateFormat: "H:i",
                    minTime: "today",

                });


        }

        function validateForm() {
            // This function deals with validation of the form fields
            var x, y, z, i, j, m, valid = true;

            x = document.getElementsByClassName("tab");
            y = x[currentTab].getElementsByTagName("input");
            z = x[currentTab].getElementsByTagName("select");
            f = x[currentTab].getElementsByTagName("file");
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
                        else if (i == 2) {
                            valid = false;
                            break;
                        }
                        else if (i == 4) {

                            valid = false;
                            break;
                        }
                        else {
                            valid = true;

                        }

                        //alert(valid + ', ' + i);

                    }


                }
                for (j = 0; j <= z.length - 1; j++) {
                    // If a field is empty...
                    if (z[j].value == "0") {
                        // add an "invalid" class to the field:
                        z[j].className += " invalid";
                        // and set the current valid status to false
                        if (j == 0) {
                            valid = false;
                        }
                    }
                }
                if (y[4].value.length < 10) {
                    infoalert('Enter 10 Digit Phone No.');
                    valid = false;
                }

            }

            // A loop that checks every input field in the current tab:          
            if (m == 1) {

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

                        } else if (i == 5) {
                            if (document.getElementById('<%=hfLicence.ClientID %>').value == "") {
                                valid = false;
                                break;
                            }

                        }

                        else {
                            valid = true;
                        }
                    }
                    else {
                        valid = true;
                    }
                    //alert(valid + ', ' + i);

                }
                for (j = 0; j <= f.length - 1; j++) {
                    // If a field is empty...
                    if (f[j].value == "0") {
                        // add an "invalid" class to the field:
                        f[j].className += " invalid";
                        // and set the current valid status to false
                        if (j == 0) {
                            valid = false;
                        }
                    }
                }

            }
            if (m == 2) {

                var selectedvalue = $('#<%= chkWorkingDays.ClientID %> input:checked').val();

                if (selectedvalue == undefined) {
                    infoalert('Select Working Days');

                }
                for (i = 0; i <= y.length - 1; i++) {
                    // If a field is empty...
                    valid = true;


                    if (selectedvalue == undefined || y[i].value == "") {
                        // add an "invalid" class to the field:
                        y[i].className += " invalid";
                        // and set the current valid status to false
                        if (i == 0) {
                            valid = false;
                            break;
                        }

                    }
                    if (y[i].value == "") {
                        if (i == 7) {
                            valid = false;
                            break;
                        }

                        else if (i == 8) {
                            valid = false;
                            break;
                        }
                    }

                    else {
                        valid = true;
                    }


                }
            }
            if (m == 3) {
                for (i = 0; i <= y.length - 1; i++) {
                    // If a field is empty...
                    valid = true;

                    if (y[i].value == "") {
                        // add an "invalid" class to the field:
                        y[i].className += " invalid";
                        // and set the current valid status to false
                        if (i == 1) {
                            valid = false;
                            break;
                        } if (i == 0) {
                            valid = false;
                            break;
                        }

                    }
                    else {
                        valid = true;
                    }


                }
            }

            // If the valid status is true, mark the step as finished and valid:
            if (valid) {
                document.getElementsByClassName("step")[currentTab].className += " finish";
            }
            return valid; // return the valid status
        }

        function fixStepIndicator(n) {

            // This function removes the "active" class of all steps...
            var i, x = document.getElementsByClassName("step");
            for (i = 0; i < x.length; i++) {
                x[i].className = x[i].className.replace(" active", "");
            }
            //... and adds the "active" class on the current step:
            x[n].className += " active";
        }
    </script>

    <%--Script for the Document Preview--%>
    <script>
        function LicenceImage(e) {
            e.preventDefault();
            //alert(document.getElementById('<%=hfLicence.ClientID %>').value);
            window.open(document.getElementById('<%=hfLicence.ClientID %>').value, '', 'width=400, height=400')
        }
        function Document1Image(e) {
            e.preventDefault();
            window.open(document.getElementById('<%=hfdocument1.ClientID %>').value, '', 'width=400, height=400')
        }
        function Document2Image(e) {
            e.preventDefault();
            window.open(document.getElementById('<%=hfdocument2.ClientID %>').value, '', 'width=400, height=400')
        }
    </script>
    <%--ZipCode--%>
    <script>

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
                                $('#<%=hfDistrict.ClientID%>').val('');
                                $('#<%=hfState.ClientID%>').val('');
                                $('#<%=hfCity.ClientID%>').val('Invalid Pincode');
                            }
                        }
                    }
                )
                .catch()
        }


    </script>
    <%--Compare License date--%>
    <script>
        function comparedate() {
            var fromdate = document.getElementById('<%=txtLicensePeriodFrom.ClientID %>').value;
            var todate = document.getElementById('<%=txtLicensePeriodTo.ClientID %>').value;
            if (todate < fromdate) {
                alert("License PeriodTo should be greater than License PeriodFrom");
                document.getElementById('<%=txtLicensePeriodTo.ClientID %>').value = "";
            }
        }
    </script>

    <%--    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k"></script>--%>

    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k" type="text/javascript"></script>
    <%-- Script for the Get the Lat and Log--%>
    <script type="text/javascript">
        window.onload = function () {
            const options = {
                enableHighAccuracy: true,
                maximumAge: 30000
            };

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(success, fail, options);
            }
            else {
                alert("Sorry, your browser does not support geolocation services.");
            }
            var latt, longg, map;

            function success(position) {
                document.getElementById('<%=hflongitude.ClientID %>').value = position.coords.longitude;
                document.getElementById('<%=hflatitude.ClientID %>').value = position.coords.latitude;
               
                var myEle = document.getElementById('<%=spAddorEdit.ClientID%>')
                if (myEle) {
                    var txt = document.getElementById("<%=spAddorEdit.ClientID %>").innerHTML;
                    if (txt == 'Add ') {
                        latt = document.getElementById('<%=hflatitude.ClientID %>').value;
                        longg = document.getElementById('<%=hflongitude.ClientID %>').value;
                        latt = position.coords.latitude;
                        longg = position.coords.longitude;
                    }
                    else {
                        latt = document.getElementById('<%=txtLatitude.ClientID %>').value;
                        longg = document.getElementById('<%=txtLongitude.ClientID %>').value;
                    }
                    setlatlong(latt, longg);
                }

            }

            function setlatlong(latt, longg) {
                document.getElementById('<%=txtLatitude.ClientID %>').value = latt;
                document.getElementById('<%=txtLongitude.ClientID %>').value = longg;

                var mapOptions = {
                    center: new google.maps.LatLng(latt, longg),
                    zoom: 10,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                map = new google.maps.Map(document.getElementById("dvMapget"), mapOptions);

                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(latt, longg),
                });
                marker.setMap(map);

                google.maps.event.addListener(map, "click", function (e) {
                    var latLng = e.latLng;
                    var lat = e.latLng.lat();
                    var long = e.latLng.lng();
                    document.getElementById('<%=hflatitude.ClientID %>').value = lat;
                    document.getElementById('<%=hflongitude.ClientID %>').value = long;

                    document.getElementById('<%=txtLatitude.ClientID %>').value = lat;
                    document.getElementById('<%=txtLongitude.ClientID %>').value = long
                    marker.setMap(null);//remove other markers

                    marker = new google.maps.Marker({
                        position: new google.maps.LatLng(lat, long)
                    });

                    marker.setMap(map);//Set new marker

                    google.maps.event.addDomListener(window, 'load', initialize);
                });

            }

            function fail(error) {
                if (alert(error.message) == "") {
                    alert("Sorry, Failed to get Your Location.");
                }
                else {
                    alert(error.message);
                }
            }
        }

    </script>


</asp:Content>

