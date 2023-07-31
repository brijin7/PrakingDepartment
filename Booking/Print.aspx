<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print.aspx.cs" Inherits="Booking_Print" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/Booking.css" rel="stylesheet" />
    <link href="../Style/BookingSlots.css" rel="stylesheet" />
    <link href="../Style/SlotSummary.css" rel="stylesheet" />
    <link href="../Style/Ticket.css" rel="stylesheet" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.7.1.min.js"></script>
    <style>
        .row {
            display: flex;
            flex-wrap: wrap;
            padding-left: 2px;
        }
    </style>
</head>
<body style="width: 250px">
    <form id="form1" runat="server">
        <div class="f14CNR">

            <input id="btnPrint" type="button" value="Print" onclick="javascript: printDiv('div2Print')" style="display: none" />
            <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-primary btnFinal" OnClick="btnBack_Click" Style="display: none" />
        </div>
        <div id="div2Print" runat="server">
            <div id="Booking__Ticket__Container">
                <div class="Booking__ticket">
                    <div class="Booking__heading">
                        <div class="parkingName__container">
                            <asp:Label ID="lblParkinName" CssClass="lblBooking__ParkingName" runat="server"></asp:Label>

                        </div>
                        <div class="parkingName__container">
                            <asp:Label ID="lblBranch" CssClass="lblBranchNameAndPhNo" runat="server"></asp:Label>
                        </div>
                        <div style="font-weight: 300; font-size: 0.7rem">
                            <asp:Label ID="lblAddress" CssClass="lblBranchNameAndPhNo" runat="server"></asp:Label>
                        </div>
                        <div class="BranchNameAndPhNo__container" style="margin-top: 3px;">
                            <asp:Label ID="lblBookingPin" CssClass="lblBranchNameAndPhNo" runat="server"></asp:Label>

                            <asp:Label ID="lblBookingDate" CssClass="lblBranchNameAndPhNo lblBooking__BranchPhNo" runat="server"></asp:Label>
                        </div>

                    </div>
                    <div id="divQr" runat="server">
                        <div class="Booking__qr">
                            <asp:Label ID="lblslotactiveStatusName" runat="server" CssClass="imageQr" Style="font-weight: bold"></asp:Label>
                            <asp:Image ID="imgEmpPhotoPrev" runat="server" CssClass="imageQr" alt="Image" class="QR" />
                        </div>

                    </div>
                    <div class="Booking__details">
                        <div class="Booking__details__Sub" id="PassBookingId" runat="server" visible="false" style="margin-top: 10px; margin-bottom: 10px;">
                            <span class="Booking__details__Sub__heading">Booking Id</span>
                            <asp:Label ID="lblPassBookingId" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>

                        </div>

                        <div id="divVhBookingDetails" runat="server">
                            <div class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">Booking Id</span>
                                <asp:Label ID="lblBookingId" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div class="Booking__details__Sub" id="TicketVehicleNo" runat="server">
                                <span class="Booking__details__Sub__heading">Vehicle No.</span>
                                <asp:Label ID="lblTicketVehicleNo" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">Vehicle Type</span>
                                <asp:Label ID="lblVehicleType" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>

                            <div id="laneSlotNo" runat="server" class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">Lane - Slot</span>
                                <asp:Label ID="lblLaneNoSlot" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                                <%--                                   <asp:Label ID="lblLaneNo" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>--%>
                            </div>

                            <div id="divInTime" runat="server" class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">In</span>
                                <asp:Label ID="lblIn" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div id="divOutTime" runat="server" class="Booking__details__Sub" visible="false">
                                <span class="Booking__details__Sub__heading">Out</span>
                                <asp:Label ID="lblOut" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div id="divParkedHrs" runat="server" class="Booking__details__Sub" visible="false">
                                <span class="Booking__details__Sub__heading">Parked Hrs</span>
                                <asp:Label ID="lblParkedHrs" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                        </div>

                        <div id="Divaccessories" runat="server" visible="false">
                            <hr id="passhr" runat="server" style="border-style: solid; margin-top: 2px; margin-bottom: 2px" />
                            <div class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">Add-On Service</span>
                            </div>
                            <asp:DataList ID="dtlaccessories" runat="server" Style="inline-size: -webkit-fill-available;">

                                <ItemTemplate>

                                    <div class="Booking__details__Sub">
                                        <span class="Booking__details__Sub__sub">
                                            <asp:Label ID="lblTicktetAccessories" runat="server"
                                                Text='<%# Eval("extraFeesDetails").ToString() + " (Qty:" +Eval("count").ToString() + ")" %>'></asp:Label>
                                        </span>
                                        <span class="Booking__details__Sub__sub">
                                            <asp:Label ID="lblTicktetAccessoriesAmount" runat="server"
                                                Text='<%# "₹"+Eval("extraFee").ToString() %>'> </asp:Label>
                                        </span>

                                    </div>
                                </ItemTemplate>

                            </asp:DataList>

                        </div>
                        <div id="DivPaymentDtls" runat="server">
                            <hr style="border-style: solid; margin-top: 2px; margin-bottom: 2px" />
                            <div class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">Payment Type </span>
                                <asp:Label ID="lblTicketPaymentType" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div class="Booking__details__Sub" id="divparkingCharge" runat="server">
                                <span class="Booking__details__Sub__heading">Parking Charge </span>
                                <asp:Label ID="lblBookingAmount" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div class="Booking__details__Sub" id="divAddOn" runat="server">
                                <span class="Booking__details__Sub__heading">Add-On Service </span>
                                <asp:Label ID="lblAddonAmount" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div class="Booking__details__Sub" id="divtax" runat="server">
                                <span class="Booking__details__Sub__heading">Tax Amount </span>
                                <asp:Label ID="lblBookingTaxAmount" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <hr style="border-style: solid; margin-top: 1px; margin-bottom: 0px" />
                            <div class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">Net </span>
                                <asp:Label ID="lblAmount" CssClass="Booking__details__Sub__sub" runat="server"></asp:Label>
                            </div>
                            <hr style="border-style: solid; margin-top: 1px; margin-bottom: 0px" />
                            <div class="Booking__details__Sub" runat="server" visible="false">
                                <span class="Booking__details__Sub__heading">Paid Amount </span>
                                <asp:Label ID="lblPaidAmount" runat="server" CssClass="Booking__details__Sub__sub"></asp:Label>
                            </div>
                            <div id="divGstNo" runat="server">
                                <div class="Booking__details__Sub" runat="server">
                                    <span class="Booking__details__Sub__heading">Prices are Inclusive of GST</span>
                                </div>
                                <div style="text-align: center;" runat="server" visible="true">

                                    <asp:Label ID="lblGstNo" runat="server" Style="font-weight: 600; font-size: 13px;"></asp:Label>
                                </div>
                            </div>

                        </div>

                        <div id="DivChkOutAmt" runat="server" visible="false">
                            <hr style="border-style: solid; margin-top: 1px; margin-bottom: 0px" />
                            <div class="Booking__details__Sub">
                                <span class="Booking__details__Sub__heading">Amount </span>
                                <asp:Label ID="lblChkOutAmount" CssClass="Booking__details__Sub__sub" runat="server"></asp:Label>
                            </div>
                            <hr style="border-style: solid; margin-top: 1px; margin-bottom: 0px" />

                        </div>
                    </div>
                    <div id="divPrintInstructions" class="Booking__details" runat="server">
                        <div class="Booking__details__Sub">
                            <span class="Booking__details__Sub__heading">Instructions :</span>
                        </div>
                        <asp:DataList ID="dlPrintingInstructions" runat="server" Style="text-align: left; line-height: 10px;">
                            <ItemTemplate>
                                <li>
                                    <asp:Label ID="lblInstructionDtl" runat="server" Style="font-size: 12px; font-family: Roboto-Regular, sans-serif; font-weight: normal"
                                        Text='<%# Bind("instructions") %>'></asp:Label>
                                </li>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>

                    <div style="font-weight: 300; font-size: 0.6rem; text-align: center; white-space: nowrap; margin-top: 10px; margin-bottom: 10px;">
                        <asp:Label ID="lblPaypre" Text="***** www.paypre.in *****" runat="server"></asp:Label>
                    </div>
                    <div style="font-weight: 300; font-size: 0.6rem; text-align: right; white-space: nowrap;">
                        <asp:Label ID="lblPrintBy" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            function wait(ms) {
                var start = new Date().getTime();
                var end = start;
                while (end < start + ms) {
                    end = new Date().getTime();
                }
            }
            function printDiv(divId) {
                var divelements = document.getElementById(divId).innerHTML;
                var oldPage = document.body.innerHTML;
                document.body.innerHTML = "<html><head><title></title></head><body>" +
                    divelements + "</body>";
                window.print();
                //setTimeout(function ()
                //{
                //    //window.close();
                //    $("#btnBack").trigger("click");
                //}, 200);
                document.body.innerHTML = oldPage;
                return false;
            }
            (function () {
                var beforePrint = function () {
                    console.log('Functionality to run before printing !');
                };

                var afterPrint = function () {
                    console.log('Functionality to run after printing !');
                    $("#btnBack").trigger("click");
                };

                if (window.matchMedia) {
                    var mediaQueryList = window.matchMedia('print');
                    mediaQueryList.addListener(function (mql) {
                        if (mql.matches) {
                            beforePrint();
                        } else {
                            afterPrint();
                        }
                    });
                }
                window.onbeforeprint = beforePrint;
                window.onafterprint = afterPrint;

            }());

            $(document).ready(function () {
                $("#btnPrint").trigger("click");
                //wait(7000);
                //$("#btnBack").trigger("click");
            });
        </script>
    </form>
</body>
</html>
