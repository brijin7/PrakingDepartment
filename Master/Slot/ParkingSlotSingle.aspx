<%@ Page Title="Single Slot Master" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="ParkingSlotSingle.aspx.cs" Inherits="Master_ParkingSlot" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterPage" runat="Server">
    <style>
        .table-responsive {
            overflow-y: auto;
            max-height: 525px;
            margin-bottom: 0;
        }

        .tdsactive {
            height: 40px !important;
            width: 35px !important;
            padding: 10px 3px 10px 3px;
            margin: 3px 5px 3px 5px;
            border-radius: 10%;
            display: inline-grid;
            font-size: 12px !important;
            font-weight: bold;
            border: none;
            /* transform: scale(1.05) translate(5px) rotate(90deg);*/
        }

            .tdsactive:hover {
                transform: scale(1.05);
            }

        .imgpassage {
            /*  height: 180px;
            width: 180px;*/
            height: -webkit-fill-available;
            width: -webkit-fill-available;
        }
        /*New*/
        .card {
            background: #fff;
            background-image: linear-gradient(48deg, #fff 0%, #e5efe9 100%);
            border-top-right-radius: 16px;
            border-bottom-left-radius: 16px;
            box-shadow: -20px 20px 35px 1px rgba(10, 49, 86, 0.18);
            display: flex;
            flex-direction: column;
            padding: 25px;
            max-width: 50%;
            width: 100%;
        }

        .content-wrapper {
            font-size: 1.1em;
            margin-bottom: 44px;
        }

            .content-wrapper:last-child {
                margin-bottom: 0;
            }

        .link {
            color: #121943;
        }

            .link:focus {
                box-shadow: 0px 0px 0px 2px #121943;
            }

        .input-wrapper {
            display: flex;
            flex-direction: column;
        }

            .input-wrapper .label {
                align-items: baseline;
                display: flex;
                font-weight: 700;
                justify-content: space-between;
                margin-bottom: 8px;
            }

            .input-wrapper .optional {
                color: #5a72b5;
                font-size: 0.9em;
            }

            .input-wrapper .input {
                border: 1px solid #5a72b5;
                border-radius: 4px;
                height: 40px;
                padding: 8px;
            }

        code {
            background: #e5efe9;
            border: 1px solid #5a72b5;
            border-radius: 4px;
            padding: 2px 4px;
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

        #modal {
            opacity: 0;
            transition: opacity 0.25s ease-in-out;
            visibility: hidden;
        }

            #modal:target {
                opacity: 1;
                visibility: visible;
            }

                #modal:target .modal-body {
                    opacity: 1;
                    transform: translateY(1);
                }

            #modal .modal-body {
                max-width: 500px;
                opacity: 0;
                transform: translateY(-100px);
                transition: opacity 0.25s ease-in-out;
                width: 100%;
                z-index: 1;
            }

        .outside-trigger {
            bottom: 0;
            cursor: default;
            left: 0;
            position: fixed;
            right: 0;
            top: 0;
        }

        .button__link {
            text-decoration: none;
        }


        /*ChargePin*/
        .chargepinimage {
            Height: 60px !important;
            Width: auto;
        }


        .pincard {
            box-shadow: 0 6px 8px 0 rgb(0 0 0 / 38%);
            transition: 0.3s;
            width: 140px;
            height: 100%;
            background-color: var(--white);
            padding-left: 25px;
            padding-top: 10px;
            border-radius: 1.25rem;
        }

            .pincard:hover {
                box-shadow: 10px 8px 16px 0 rgba(0,0,0,0.2);
            }

        .pincardIn {
            box-shadow: 0 6px 8px 0 rgba(0, 0, 0, 0.38);
            transition: 0.3s;
            width: 258px;
            height: 112px;
            background-color: var(--white);
            padding-left: 16px;
        }

        .dot {
            height: 15px;
            width: 15px;
            border-radius: 35%;
            display: inline-block;
        }

        .DotText {
            color: #008eef;
            font-size: 13px;
            margin: 5px 5px 5px 5px;
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
                <asp:Label ID="lblNavSecond" runat="server" CssClass="pageRoutecol"
                    Text="Single Slot Master"></asp:Label>
            </div>
        </div>

        <div id="divslotback" class="divTitle row pt-4 pl-4 justify-content-between" runat="server" visible="true">
            <div>
                <h4 class="card-title">Edit Single <span class="Card-title-second">Slot Master</span></h4>
            </div>
            <div>
                <asp:Button ID="btnslotback" runat="server" OnClick="btnslotback_Click" Text="Back"
                    CssClass="pure-material-button-contained btnBgColorCancel" />
            </div>
        </div>
        <div id="divSlotlistdetails" runat="server" visible="true" style="display: inline-flex;">
            <h4 class="Card-title-secondslot ">Block  : <span class="card-title-slotdetails">
                <asp:Label ID="lblBlockName" runat="server"></asp:Label>
            </span></h4>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <h4 class="Card-title-secondslot ">Floor  : <span class="card-title-slotdetails">
                <asp:Label ID="lblFloorName" runat="server"></asp:Label></span> </h4>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <h4 class="Card-title-secondslot">Vehicle : <span class="card-title-slotdetails">
                <asp:Label ID="lblvehicleName" runat="server"></asp:Label></span>  </h4>
        </div>
        <div id="divslotlisttitle" runat="server" visible="false" class="justify-content-center" align="center">
            <h4 class="card-title">Slot  <span class="Card-title-second">List </span></h4>
            <hr />
        </div>
        <div class="row p-2 justify-content-center">
            <div>
                <span id="spanA" runat="server" class="dot" style="background: #40F44336"></span>
                <span class="DotText">Normal </span>
                <span id="spanV" runat="server" class="dot" style="background: #ffee2162"></span>
                <span class="DotText">VIP </span>
                <span id="spanD" runat="server" class="dot" style="background: red"></span>
                <span class="DotText">DeActive </span>
                <span id="spanP" runat="server" class="dot" style="background: #747474"></span>
                <span class="DotText">Path,PathV</span>
                <span id="spanR" runat="server" class="dot" style="background: #DFF6FF"></span>
                <span class="DotText">Reserved,VIP Reserved</span>
                <span id="span1" runat="server" class="dot" style="background: #cebebe"></span>
                <span class="DotText">Way </span>
                <span id="span2" runat="server" class="dot" style="background: #00d4ff"></span>
                <span class="DotText">In </span>
                <span id="span3" runat="server" class="dot" style="background: #1ba2bd"></span>
                <span class="DotText">Out </span>
                <span id="span4" runat="server" class="dot" style="background: #24222238"></span>
                <span class="DotText">Lane No,Lane Top</span>
            </div>
        </div>
        <div class=" mb-5" id="divslot" runat="server" visible="true">
            <div class="row">
                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 align-self-center justify-content-right">
                    <asp:Image ID="imgpassageleft" Visible="false" CssClass="imgpassage"
                        align="right" runat="server"
                        ImageUrl="~/images/parkinslotleft.png" />
                </div>
                <div id="dydiv" class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 align-self-center table-responsive section"
                    runat="server" align="center">
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    <asp:Button ID="btnLoadButton" runat="server" Text="Button" Visible="false" OnClick="btnLoadButton_Click"></asp:Button>
                    <asp:Label ID="lbl" runat="server" ForeColor="Red" Font-Size="Larger"></asp:Label>

                </div>

                <div class="col-12 col-sm-3 col-md-3 col-lg-3 col-xl-3 align-self-center justify-content-left">
                    <asp:Image ID="imgpassageright" Visible="false" CssClass="imgpassage"
                        align="left" runat="server"
                        ImageUrl="~/images/parkingslotright.png" />
                </div>
            </div>


        </div>
        <!-- Slot Model -->
        <div class="modal-wrapper" id="modal" runat="server" style="display: none">
            <div class="modal-body card">
                <div class="modal-header">
                    <h4 class="card-title">Slot <span class="Card-title-second">Update </span></h4>
                    <a role="button" class="close" aria-label="close this modal" onclick="hide()">
                        <svg viewBox="0 0 24 24">
                            <path d="M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 
                                3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z" />
                        </svg>
                    </a>
                </div>
                <div class="row" style="padding: 3px">
                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12">
                        <asp:Label ID="lblslotnumbe" runat="server" Text="Slot No."
                            Style="color: blue; font-size: 17px; font-weight: 800;"></asp:Label>
                        <span class="spanStar">*</span>
                    </div>
                    <div class="col-sm-1 col-md-1 col-lg-1 col-xs-12">
                        :
                    </div>
                    <div class="col-sm-5 col-md-5 col-lg-5 col-xs-12 text-left">
                        <asp:TextBox ID="lblslotnumber" runat="server"
                            Style="color: black; font-size: 17px; font-weight: 800;"></asp:TextBox>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1"
                            ValidationGroup="SlotUpdate"
                            ControlToValidate="lblslotnumber"
                            runat="server"
                            CssClass="rfvClr"
                            ErrorMessage="Enter Solt Number">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label
                            ID="lblChargeType"
                            runat="server"
                            Text="Charge Pin Type"
                            CssClass="form-check-label">
                        </asp:Label>
                        <span class="spanStar">*</span>
                        <asp:RadioButtonList
                            ID="rbtnChargeType"
                            runat="server"
                            CssClass="inline-rb"
                            TabIndex="1"
                            AutoPostBack="true" OnSelectedIndexChanged="rbtnChargeType_SelectedIndexChanged"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Selected="true" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator
                            ID="rfvchargetype"
                            ValidationGroup="SlotUpdate"
                            ControlToValidate="rbtnChargeType"
                            runat="server"
                            CssClass="rfvClr"
                            ErrorMessage="Select Charge Pin Type">
                        </asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="row mt-2">
                    <div id="CharType" runat="server" visible="false" class="col-12 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                        <asp:Label
                            ID="lblCharType"
                            runat="server"
                            Text="Charge Type"
                            CssClass="form-check-label">
                        </asp:Label>
                        <asp:TextBox
                            ID="txtCharType" Visible="false"
                            TabIndex="3"
                            runat="server" TextMode="Number"
                            CssClass="form-control ">
                        </asp:TextBox>
                        <span class="spanStar">*</span>
                        <div class="wrap mt-3">
                            <asp:DataList ID="gvChargePinType" runat="server" RepeatColumns="4"
                                Visible="true" OnItemCommand="gvChargePinType_ItemCommand">
                                <ItemTemplate>
                                    <div id="divpin" runat="server" class="pincard">
                                        <div class="row">
                                            <asp:ImageButton ID="lnkBtnChargeType" runat="server"
                                                CssClass="chargepinimage" AlternateText="Charge Pin"
                                                ImageUrl='<%# Bind("chargePinImageUrl") %>' OnClick="lnkBtnChargeType_Click"></asp:ImageButton>
                                            <asp:Label align="left" ID="lblChargePinName" runat="server"
                                                Text='<%# Bind("chargePinConfig") %>' Font-Bold="true"></asp:Label>
                                            <asp:Label align="left" ID="lblChargePinId" runat="server"
                                                Text='<%# Bind("chargePinId") %>' Visible="false" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>
                                    <br />
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <table width="5" style="height: 5px">
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </SeparatorTemplate>
                            </asp:DataList>
                        </div>
                    </div>
                </div>
                <div class="row p-4 justify-content-center">
                    <div class="mr-3">
                        <asp:Button ID="btnUpdateSlot" OnClick="btnUpdateSlot_Click"
                            CssClass="pure-material-button-contained btnBgColorAdd"
                            Text="Update" ValidationGroup="SlotUpdate" runat="server" />
                    </div>
                    <div>
                        <%-- <asp:Button ID="btnCanclerule" CssClass="pure-material-button-contained btnBgColorCancel"
                            Text="Clear" CausesValidation="false" OnClick="btnCanclerule_Click"
                            runat="server" />--%>
                    </div>
                </div>

            </div>
        </div>
        <asp:HiddenField ID="hfPin" runat="server" />

    </div>
    <asp:HiddenField ID="hfslotcheck" runat="server" Value="0" />
    <asp:HiddenField ID="txtcolumn" runat="server" />
    <asp:HiddenField ID="txtrow" runat="server" />
    <script>
        function show() {

            document.getElementById('<%=modal.ClientID %>').style.cssText = "#model";

        }
        function hide() {
            document.getElementById('<%=modal.ClientID %>').style.display = "none";
            <%--document.getElementById('<%=txtCharType.ClientID %>').value = "";--%>
        }

        <%--function changeColor(elemet, e) {
            e.preventDefault();
            console.log(elemet.querySelectorAll('span')[0].textContent+' '+elemet.querySelectorAll('span')[1].textContent);
            elemet.style.backgroundColor = "red";
            document.getElementById('<%=hfslotcheck.ClientID%>').value = elemet.querySelectorAll('span')[1].textContent;
        }--%>
    </script>
    <%--<style>
        .hidden {
            display: none;
        }
    </style>--%>
</asp:Content>

