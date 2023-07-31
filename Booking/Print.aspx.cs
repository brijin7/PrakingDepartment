using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.IO;
using QRCoder;


public partial class Booking_Print : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["rt"] = Request.QueryString["rt"].ToString();
            string bi = Request.QueryString["bi"].ToString();

            divparkingCharge.Visible = true;
            DivPaymentDtls.Visible = true;
            divPrintInstructions.Visible = true;
            DivChkOutAmt.Visible = false;

            if (ViewState["rt"].ToString() == "bW")
            {
                GetBookingDetailsWitSlot(bi);
            }
            if (ViewState["rt"].ToString() == "b")
            {
                GetBookingDetailsWitOtSlot(bi);
            }

            if (ViewState["rt"].ToString() == "pW")
            {
                GetPassBookingDetailsWitSlot(bi);
            }

            if (ViewState["rt"].ToString() == "p")
            {
                GetPassBookingDetailsWitOtSlot(bi);
            }
            if (ViewState["rt"].ToString() == "O" || ViewState["rt"].ToString() == "ObW" || ViewState["rt"].ToString() == "Ob")
            {
                GetAddonServiceTicket(bi);
                divparkingCharge.Visible = false;
            }
            if (ViewState["rt"].ToString() == "CO" || ViewState["rt"].ToString() == "CiCo")
            {
                CheckOutTicket(bi);
            }

            GetPrintingInstruction();

        }
    }

    #region Booking Ticket  
    public void GetBookingDetailsWitSlot(string bookingid)
    {
        try
        {
            Divaccessories.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?bookingId=" + bookingid + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        //List<Booking> BookingDetails = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                        //DataTable dt = ConvertToDataTable(BookingDetails);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            divparkingCharge.Visible = true;
                            passhr.Visible = true;
                            List<GetBooking> lst = JsonConvert.DeserializeObject<List<GetBooking>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            var lst3 = firstItem.extraFeesDetails.ToList();
                            var lst4 = firstItem.userSlotDetails.ToList();
                            DataTable vehicleDetails = ConvertToDataTable(lst1);
                            DataTable GetextraFeesDetails = ConvertToDataTable(lst3);
                            DataTable GetuserSlotDetails = ConvertToDataTable(lst4);
                            PassBookingId.Visible = false;
                            divQr.Visible = true;
                            divVhBookingDetails.Visible = true;
                            lblParkinName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblBookingAmount.Text = "₹" + dt.Rows[0]["bookingAmount"].ToString();
                            lblAddonAmount.Text = "₹" + dt.Rows[0]["extraFeesAmount"].ToString();

                            lblBookingTaxAmount.Text = "₹" + dt.Rows[0]["taxAmount"].ToString();
                            lblTicketPaymentType.Text = dt.Rows[0]["paymentTypeName"].ToString();
                            if (lblTicketPaymentType.Text == "Pay At CheckOut")
                            {
                                lblTicketPaymentType.Text = "UnPaid";
                                DivPaymentDtls.Visible = false;
                            }
                            lblPaidAmount.Text = "₹" + dt.Rows[0]["paidAmount"].ToString();
                            lblBranch.Text = "Br-" + dt.Rows[0]["branchName"].ToString();
                            lblAddress.Text = dt.Rows[0]["address1"].ToString() + " " + dt.Rows[0]["address2"].ToString() + " " + dt.Rows[0]["city"].ToString()
                           + " " + dt.Rows[0]["district"].ToString() + " " + dt.Rows[0]["pincode"].ToString() + ", Mob- " + dt.Rows[0]["branchPhoneNumber"].ToString();
                            lblBookingDate.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy");
                            laneSlotNo.Visible = true;
                            lblLaneNoSlot.Text = GetuserSlotDetails.Rows[0]["laneNumber"].ToString() + " - " + GetuserSlotDetails.Rows[0]["slotNumber"].ToString();
                            lblslotactiveStatusName.Text = GetuserSlotDetails.Rows[0]["slotactiveStatusName"].ToString();

                            //lblIn.Text = dt.Rows[0]["fromDate"].ToString() + " " + dt.Rows[0]["fromTime"].ToString();

                            if (vehicleDetails.Rows[0]["inTime"].ToString() != "")
                            {
                                divInTime.Visible = true;
                                DateTime lblInTime = Convert.ToDateTime(vehicleDetails.Rows[0]["inTime"].ToString());
                                lblIn.Text = lblInTime.ToString("dd-MM-yyyy HH:mm");
                            }
                            else
                            {
                                divInTime.Visible = false;
                            }

                            //lblTaxAmount.Text = "₹" + dt.Rows[0]["taxAmount"].ToString();
                            lblAmount.Text = "₹" + dt.Rows[0]["totalAmount"].ToString();
                            lblBookingId.Text = dt.Rows[0]["bookingId"].ToString();
                            lblBookingPin.Text = "PIN : " + dt.Rows[0]["pinNo"].ToString();
                            lblTicketVehicleNo.Text = vehicleDetails.Rows[0]["vehicleNumber"].ToString();
                            lblVehicleType.Text = vehicleDetails.Rows[0]["vehicleTypeName"].ToString();
                            if (lblTicketVehicleNo.Text.Trim() == "")
                            {
                                TicketVehicleNo.Visible = false;
                            }
                            else
                            {
                                TicketVehicleNo.Visible = true;
                            }
                            lblPrintBy.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/") + "," + Session["UserName"].ToString();

                            string PinNo = dt.Rows[0]["pinNo"].ToString();

                            GetQRcode(bookingid, PinNo);
                            if (Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                divtax.Visible = true;
                            }
                            else
                            {
                                divtax.Visible = false;
                            }
                            if (Convert.ToString(dt.Rows[0]["gstNumber"]) != "" && Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                lblGstNo.Text = "GST No. - " + dt.Rows[0]["gstNumber"].ToString();
                                divGstNo.Visible = true;
                            }
                            else
                            {
                                divGstNo.Visible = false;
                            }

                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divAddOn.Visible = true;
                                Divaccessories.Visible = true;
                                dtlaccessories.DataSource = GetextraFeesDetails;
                                dtlaccessories.DataBind();

                            }
                            else
                            {
                                divAddOn.Visible = false;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {

        }
    }
    public void GetBookingDetailsWitOtSlot(string bookingid)
    {
        try
        {
            Divaccessories.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?bookingId=" + bookingid + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            List<GetBooking> lst = JsonConvert.DeserializeObject<List<GetBooking>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            var lst3 = firstItem.extraFeesDetails.ToList();

                            DataTable vehicleDetails = ConvertToDataTable(lst1);
                            DataTable GetextraFeesDetails = ConvertToDataTable(lst3);
                            PassBookingId.Visible = false;
                            divparkingCharge.Visible = true;
                            passhr.Visible = true;
                            divQr.Visible = true;
                            divVhBookingDetails.Visible = true;
                            lblParkinName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblBookingAmount.Text = "₹" + dt.Rows[0]["bookingAmount"].ToString();
                            lblAddonAmount.Text = "₹" + dt.Rows[0]["extraFeesAmount"].ToString();
                            lblBookingTaxAmount.Text = "₹" + dt.Rows[0]["taxAmount"].ToString();
                            lblTicketPaymentType.Text = dt.Rows[0]["paymentTypeName"].ToString();
                            if (lblTicketPaymentType.Text == "Pay At CheckOut")
                            {
                                lblTicketPaymentType.Text = "UnPaid";
                                DivPaymentDtls.Visible = false;
                            }
                            lblPaidAmount.Text = "₹" + dt.Rows[0]["paidAmount"].ToString();
                            lblBranch.Text = "Br-" + dt.Rows[0]["branchName"].ToString();
                            lblAddress.Text = dt.Rows[0]["address1"].ToString() + " " + dt.Rows[0]["address2"].ToString() + " " + dt.Rows[0]["city"].ToString()
                               + " " + dt.Rows[0]["district"].ToString() + " " + dt.Rows[0]["pincode"].ToString() + ", Mob- " + dt.Rows[0]["branchPhoneNumber"].ToString();
                            lblBookingDate.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy");
                            laneSlotNo.Visible = false;

                            //lblIn.Text = dt.Rows[0]["fromDate"].ToString() + " " + dt.Rows[0]["fromTime"].ToString();

                            if (vehicleDetails.Rows[0]["inTime"].ToString() != "")
                            {
                                divInTime.Visible = true;
                                DateTime lblInTime = Convert.ToDateTime(vehicleDetails.Rows[0]["inTime"].ToString());
                                lblIn.Text = lblInTime.ToString("dd-MM-yyyy HH:mm");
                            }
                            else
                            {
                                divInTime.Visible = false;
                            }

                            // lblTaxAmount.Text = "₹" + dt.Rows[0]["taxAmount"].ToString();
                            lblAmount.Text = "₹" + dt.Rows[0]["totalAmount"].ToString();
                            lblBookingId.Text = dt.Rows[0]["bookingId"].ToString();
                            lblBookingPin.Text = "PIN : " + dt.Rows[0]["pinNo"].ToString();
                            lblTicketVehicleNo.Text = vehicleDetails.Rows[0]["vehicleNumber"].ToString();
                            lblVehicleType.Text = vehicleDetails.Rows[0]["vehicleTypeName"].ToString();
                            if (lblTicketVehicleNo.Text.Trim() == "")
                            {
                                TicketVehicleNo.Visible = false;
                            }
                            else
                            {
                                TicketVehicleNo.Visible = true;
                            }
                            lblPrintBy.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/") + "," + Session["UserName"].ToString();

                            string PinNo = dt.Rows[0]["pinNo"].ToString();

                            GetQRcode(bookingid, PinNo);

                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divAddOn.Visible = true;
                                Divaccessories.Visible = true;
                                dtlaccessories.DataSource = GetextraFeesDetails;
                                dtlaccessories.DataBind();

                            }
                            else
                            {
                                divAddOn.Visible = false;
                            }
                            if (Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                divtax.Visible = true;
                            }
                            else
                            {
                                divtax.Visible = false;
                            }
                            if (Convert.ToString(dt.Rows[0]["gstNumber"]) != "" && Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                lblGstNo.Text = "GST No. - " + dt.Rows[0]["gstNumber"].ToString();
                                divGstNo.Visible = true;
                            }
                            else
                            {
                                divGstNo.Visible = false;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {

        }
    }

    #endregion

    #region QR Code Generate 
    public void GetQRcode(string BookingId, string PinNo)
    {
        string sAssetCode = BookingId + ";" + PinNo;
        string code = sAssetCode.ToString();
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                imgEmpPhotoPrev.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                string result = Convert.ToBase64String(byteImage, 0, byteImage.Length);

            }
        }
    }

    #endregion

    #region Pass Ticket   
    public void GetPassBookingDetailsWitSlot(string PasstransactionId)
    {
        try
        {
            Divaccessories.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "passBookingTransaction?passBookingTransactionId=" + PasstransactionId + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<GetBooking> lst = JsonConvert.DeserializeObject<List<GetBooking>>(ResponseMsg);
                        var firstItem = lst.ElementAt(0);
                        var lst3 = firstItem.extraFeesDetails.ToList();
                        DataTable GetextraFeesDetails = ConvertToDataTable(lst3);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);


                        if (dt.Rows.Count > 0)
                        {
                            divparkingCharge.Visible = false;
                            passhr.Visible = false;
                            lblParkinName.Text = dt.Rows[0]["parkingName"].ToString();
                            PassBookingId.Visible = true;
                            lblPassBookingId.Text = dt.Rows[0]["passBookingTransactionId"].ToString();
                            lblBranch.Text = "Br-" + dt.Rows[0]["branchName"].ToString();
                            lblBookingDate.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy");
                            lblAddress.Text = dt.Rows[0]["address1"].ToString() + " " + dt.Rows[0]["address2"].ToString() + " " + dt.Rows[0]["city"].ToString()
                                        + " " + dt.Rows[0]["district"].ToString() + " " + dt.Rows[0]["pincode"].ToString() + ", Mob- " + dt.Rows[0]["phoneNumber"].ToString();
                            divQr.Visible = false;

                            divVhBookingDetails.Visible = false;
                            lblTicketPaymentType.Text = dt.Rows[0]["paymentTypeName"].ToString();
                            if (lblTicketPaymentType.Text == "Pay At CheckOut")
                            {
                                lblTicketPaymentType.Text = "UnPaid";
                                DivPaymentDtls.Visible = false;
                            }
                            lblAmount.Text = dt.Rows[0]["totalAmount"].ToString();
                            lblBookingId.Text = dt.Rows[0]["passBookingTransactionId"].ToString();
                            lblPrintBy.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/") + "," + Session["UserName"].ToString();
                            lblAddonAmount.Text = "₹" + dt.Rows[0]["extraFeesAmount"].ToString();
                            lblBookingTaxAmount.Text = "₹" + dt.Rows[0]["extraFeesTaxAmount"].ToString();
                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divAddOn.Visible = true;
                                Divaccessories.Visible = true;
                                dtlaccessories.DataSource = GetextraFeesDetails;
                                dtlaccessories.DataBind();

                            }
                            else
                            {
                                divAddOn.Visible = false;
                            }
                            if (Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                divtax.Visible = true;
                            }
                            else
                            {
                                divtax.Visible = false;
                            }
                            if (Convert.ToString(dt.Rows[0]["gstNumber"]) != "" && Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                lblGstNo.Text = "GST No. - " + dt.Rows[0]["gstNumber"].ToString();
                                divGstNo.Visible = true;
                            }
                            else
                            {
                                divGstNo.Visible = false;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {

        }
    }
    public void GetPassBookingDetailsWitOtSlot(string PasstransactionId)
    {
        try
        {
            Divaccessories.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "passBookingTransaction?passBookingTransactionId=" + PasstransactionId + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<GetBooking> lst = JsonConvert.DeserializeObject<List<GetBooking>>(ResponseMsg);
                        var firstItem = lst.ElementAt(0);
                        var lst3 = firstItem.extraFeesDetails.ToList();
                        DataTable GetextraFeesDetails = ConvertToDataTable(lst3);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            divparkingCharge.Visible = false;
                            passhr.Visible = false;
                            lblParkinName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblBranch.Text = "Br-" + dt.Rows[0]["branchName"].ToString();
                            lblBookingDate.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy");
                            lblAddress.Text = dt.Rows[0]["address1"].ToString() + " " + dt.Rows[0]["address2"].ToString() + " " + dt.Rows[0]["city"].ToString()
                                         + " " + dt.Rows[0]["district"].ToString() + " " + dt.Rows[0]["pincode"].ToString() + ", Mob- " + dt.Rows[0]["phoneNumber"].ToString();
                            divQr.Visible = false;
                            divVhBookingDetails.Visible = false;
                            PassBookingId.Visible = true;
                            lblPassBookingId.Text = dt.Rows[0]["passBookingTransactionId"].ToString();
                            lblTicketPaymentType.Text = dt.Rows[0]["paymentTypeName"].ToString();
                            if (lblTicketPaymentType.Text == "Pay At CheckOut")
                            {
                                lblTicketPaymentType.Text = "UnPaid";
                                DivPaymentDtls.Visible = false;
                            }
                            lblAddonAmount.Text = "₹" + dt.Rows[0]["extraFeesAmount"].ToString();
                            lblBookingTaxAmount.Text = "₹" + dt.Rows[0]["extraFeesTaxAmount"].ToString();
                            lblAmount.Text = "₹" + dt.Rows[0]["totalAmount"].ToString();
                            lblBookingId.Text = dt.Rows[0]["passBookingTransactionId"].ToString();
                            lblPrintBy.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/") + "," + Session["UserName"].ToString();
                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divAddOn.Visible = true;
                                Divaccessories.Visible = true;
                                dtlaccessories.DataSource = GetextraFeesDetails;
                                dtlaccessories.DataBind();
                            }
                            else
                            {
                                divAddOn.Visible = false;
                            }

                            if (Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                divtax.Visible = true;
                            }
                            else
                            {
                                divtax.Visible = false;
                            }
                            if (Convert.ToString(dt.Rows[0]["gstNumber"]) != "" && Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                lblGstNo.Text = "GST No. - " + dt.Rows[0]["gstNumber"].ToString();
                                divGstNo.Visible = true;
                            }
                            else
                            {
                                divGstNo.Visible = false;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {

        }
    }
    #endregion

    #region Add On Service Ticket  
    public void GetAddonServiceTicket(string PasstransactionId)
    {
        try
        {
            Divaccessories.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?bookingId=" + PasstransactionId + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<GetBooking> lst = JsonConvert.DeserializeObject<List<GetBooking>>(ResponseMsg);
                        var firstItem = lst.ElementAt(0);
                        var lst3 = firstItem.extraFeesDetails.ToList();
                        DataTable GetextraFeesDetails = ConvertToDataTable(lst3);

                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            lblParkinName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblBranch.Text = "Br-" + dt.Rows[0]["branchName"].ToString();
                            lblBookingDate.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy");
                            lblAddress.Text = dt.Rows[0]["address1"].ToString() + " " + dt.Rows[0]["address2"].ToString() + " " + dt.Rows[0]["city"].ToString()
                                                         + " " + dt.Rows[0]["district"].ToString() + " " + dt.Rows[0]["pincode"].ToString() + ", Mob- " + dt.Rows[0]["branchPhoneNumber"].ToString();
                            divQr.Visible = false;
                            divVhBookingDetails.Visible = false;
                            lblTicketPaymentType.Text = dt.Rows[0]["paymentTypeName"].ToString();
                            if (lblTicketPaymentType.Text == "Pay At CheckOut")
                            {
                                lblTicketPaymentType.Text = "UnPaid";
                                DivPaymentDtls.Visible = false;
                            }
                            PassBookingId.Visible = true;
                            lblPassBookingId.Text = dt.Rows[0]["bookingId"].ToString();
                            lblAddonAmount.Text = "₹" + dt.Rows[0]["extraFeesAmount"].ToString();
                            lblBookingTaxAmount.Text = "₹" + dt.Rows[0]["extraFeesTax"].ToString();
                            lblAmount.Text = "₹" + dt.Rows[0]["totalAmount"].ToString();
                            lblBookingId.Text = dt.Rows[0]["bookingId"].ToString();
                            lblBookingPin.Text = "PIN : " + dt.Rows[0]["pinNo"].ToString();
                            lblPrintBy.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/") + "," + Session["UserName"].ToString();
                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divAddOn.Visible = true;
                                Divaccessories.Visible = true;
                                dtlaccessories.DataSource = GetextraFeesDetails;
                                dtlaccessories.DataBind();

                            }
                            else
                            {
                                divAddOn.Visible = false;
                            }
                            if (Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                divtax.Visible = true;
                            }
                            else
                            {
                                divtax.Visible = false;
                            }
                            if (Convert.ToString(dt.Rows[0]["gstNumber"]) != "" && Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                lblGstNo.Text = "GST No. - " + dt.Rows[0]["gstNumber"].ToString();
                                divGstNo.Visible = true;
                            }
                            else
                            {
                                divGstNo.Visible = false;
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {

        }
    }
    #endregion

    #region Check Out Ticket
    public void CheckOutTicket(string BookingId)
    {
        try
        {
            Divaccessories.Visible = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?bookingId=" + BookingId + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            divparkingCharge.Visible = true;
                            passhr.Visible = true;
                            List<GetBooking> lst = JsonConvert.DeserializeObject<List<GetBooking>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            var lst3 = firstItem.extraFeesDetails.ToList();
                            var lst4 = firstItem.userSlotDetails.ToList();
                            DataTable vehicleDetails = ConvertToDataTable(lst1);
                            DataTable GetextraFeesDetails = ConvertToDataTable(lst3);
                            DataTable GetuserSlotDetails = ConvertToDataTable(lst4);
                            divQr.Visible = false;
                            PassBookingId.Visible = false;

                            divVhBookingDetails.Visible = true;

                            lblParkinName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblBranch.Text = "Br-" + dt.Rows[0]["branchName"].ToString();
                            lblAddress.Text = dt.Rows[0]["address1"].ToString() + " " + dt.Rows[0]["address2"].ToString() + " " + dt.Rows[0]["city"].ToString()
                           + " " + dt.Rows[0]["district"].ToString() + " " + dt.Rows[0]["pincode"].ToString() + ", Mob- " + dt.Rows[0]["branchPhoneNumber"].ToString();

                            if(GetuserSlotDetails.Rows.Count>0)
                            {
                                laneSlotNo.Visible = true;
                                lblLaneNoSlot.Text = GetuserSlotDetails.Rows[0]["laneNumber"].ToString() + " - " + GetuserSlotDetails.Rows[0]["slotNumber"].ToString();
                                lblslotactiveStatusName.Text = GetuserSlotDetails.Rows[0]["slotactiveStatusName"].ToString();
                            }
                            else
                            {
                                laneSlotNo.Visible = false;
                            }
                          

                            DateTime lblInTime, lblOutTime;

                            if (vehicleDetails.Rows[0]["inTime"].ToString() != "")
                            {
                                divInTime.Visible = true;
                                lblInTime = Convert.ToDateTime(vehicleDetails.Rows[0]["inTime"].ToString());
                                lblIn.Text = lblInTime.ToString("dd-MM-yyyy HH:mm");
                            }
                            else
                            {
                                divInTime.Visible = false;
                                lblInTime = DateTime.Now;
                            }

                            if (vehicleDetails.Rows[0]["outTime"].ToString() != "")
                            {
                                divOutTime.Visible = true;
                                lblOutTime = Convert.ToDateTime(vehicleDetails.Rows[0]["outTime"].ToString());
                                lblOut.Text = lblOutTime.ToString("dd-MM-yyyy HH:mm");
                            }
                            else
                            {
                                divOutTime.Visible = false;
                                lblOutTime = DateTime.Now;
                            }

                            if (vehicleDetails.Rows[0]["inTime"].ToString() != "" && vehicleDetails.Rows[0]["outTime"].ToString() != "")
                            {
                                divParkedHrs.Visible = true;

                                string[] Hours = Convert.ToString(lblOutTime.Subtract(lblInTime).TotalHours).Split('.');
                                string Mins = Convert.ToString(lblOutTime.Subtract(lblInTime).Minutes);
                                lblParkedHrs.Text = Hours[0].ToString() + ":" + Mins.ToString();
                            }
                            else
                            {
                                divParkedHrs.Visible = false;
                            }

                            DivPaymentDtls.Visible = false;
                            divPrintInstructions.Visible = false;
                            DivChkOutAmt.Visible = true;

                            lblChkOutAmount.Text = "₹" + dt.Rows[0]["totalAmount"].ToString();
                            lblBookingId.Text = dt.Rows[0]["bookingId"].ToString();
                            lblBookingPin.Text = "PIN : " + dt.Rows[0]["pinNo"].ToString();
                            lblTicketVehicleNo.Text = vehicleDetails.Rows[0]["vehicleNumber"].ToString();
                            lblVehicleType.Text = vehicleDetails.Rows[0]["vehicleTypeName"].ToString();
                            if (lblTicketVehicleNo.Text.Trim() == "")
                            {
                                TicketVehicleNo.Visible = false;
                            }
                            else
                            {
                                TicketVehicleNo.Visible = true;
                            }
                            lblPrintBy.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt").Replace("-", "/") + "," + Session["UserName"].ToString();

                            if (Convert.ToString(dt.Rows[0]["gstNumber"]) != "" && Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString()) != 0)
                            {
                                lblGstNo.Text = "GST No. - " + dt.Rows[0]["gstNumber"].ToString();
                                divGstNo.Visible = true;
                            }
                            else
                            {
                                divGstNo.Visible = false;
                            }

                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {

        }
    }
    #endregion

    #region Get Printing Instruction
    public void GetPrintingInstruction()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "printingInstructionsConfig?branchId= " + Session["branchId"].ToString() + "&instructionType=R&activeStatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            dlPrintingInstructions.DataSource = dt;
                            dlPrintingInstructions.DataBind();
                        }
                        else
                        {
                            dlPrintingInstructions.DataBind();
                            divPrintInstructions.Visible = false;
                        }
                    }
                    else
                    {
                        dlPrintingInstructions.DataBind();
                        divPrintInstructions.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Print Back
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (ViewState["rt"].ToString() == "bW" || ViewState["rt"].ToString() == "pW" || ViewState["rt"].ToString() == "Ob" || ViewState["rt"].ToString() == "CO")
        {
            Response.Redirect("Booking.aspx");
        }
        if (ViewState["rt"].ToString() == "O")
        {
            Response.Redirect("AddOnServiceBooking.aspx");
        }
        if (ViewState["rt"].ToString() == "ObW" || ViewState["rt"].ToString() == "b" || ViewState["rt"].ToString() == "p")
        {
            Response.Redirect("BookingWithoutSlot.aspx");
        }
        if (ViewState["rt"].ToString() == "CiCo")
        {
            Response.Redirect("CheckInCheckOut.aspx");
        }

    }
    #endregion

    #region Ticket Booking Class  
    public class GetBooking
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userId { get; set; }
        public string phoneNumber { get; set; }
        public string booking { get; set; }
        public string bookingId { get; set; }
        public string bookingDurationType { get; set; }
        public string branchPhoneNumber { get; set; }
        public string accessories { get; set; }
        public string Dates { get; set; }
        public string bookingType { get; set; }
        public string subscriptionId { get; set; }
        public string taxId { get; set; }
        public string offerId { get; set; }
        public string paidAmount { get; set; }
        public string transactionId { get; set; }
        public string bankName { get; set; }
        public string vehicleTypeName { get; set; }
        public string pinNo { get; set; }
        public string createdBy { get; set; }
        public string loginType { get; set; }
        public string parkingName { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string branchName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string taxAmount { get; set; }
        public string totalAmount { get; set; }
        public string paymentTypeName { get; set; }
        public string paymentStatus { get; set; }
        public string bookingAmount { get; set; }
        public string bookingTax { get; set; }
        public string paymentType { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string pincode { get; set; }
        public string extraFeesAmount { get; set; }
        public string extraFeesTaxAmount { get; set; }
        public string extraFeesTax { get; set; }

        public List<vehicleDetails> vehicleDetails { get; set; }
        public List<GetuserSlotDetails> userSlotDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetail { get; set; }
        public List<GetextraFees> extraFeesDetails { get; set; }

    }
    public class GetBookings
    {
        public string passBookingTransactionId { get; set; }
        public string vehicleHeaderId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string slotId { get; set; }
        public string vehicleTypeName { get; set; }
        public string vehicleImageUrl { get; set; }
        public string extendAmount { get; set; }
        public string extendTax { get; set; }
        public string extendDayHour { get; set; }
        public string remainingAmount { get; set; }
        public string boookingAmount { get; set; }
        public string initialAmount { get; set; }
        public string pinNo { get; set; }
        public string vehicleName { get; set; }
        public string vehicleParkedTime { get; set; }
        public string bookingDurationType { get; set; }
        public List<vehicleDetails> vehicleDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<GetextraFees> extraFeesDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetail { get; set; }
        public List<GetextraFees> extraFeesDetail { get; set; }

    }
    public class vehicleDetails
    {
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string vehicleTypeName { get; set; }
    }
    public class GetuserSlotDetails
    {
        public string slotactiveStatusName { get; set; }
        public string slotId { get; set; }
        public string slotNumber { get; set; }
        public string vehicleType { get; set; }
        public string laneNumber { get; set; }
    }
    public class GetextraFeaturesDetails
    {
        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraDetail { get; set; }
        public string featureName { get; set; }
        public string tax { get; set; }
        public string totalAmount { get; set; }
    }
    public class GetextraFees
    {
        public string extraFeesDetails { get; set; }
        public string priceId { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }
    }

    DataTable ConvertToDataTable<TSource>(IEnumerable<TSource> source)
    {
        var props = typeof(TSource).GetProperties();

        var dt = new DataTable();
        dt.Columns.AddRange(
          props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray()
        );

        source.ToList().ForEach(
          i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
        );

        return dt;
    }

    #endregion
}