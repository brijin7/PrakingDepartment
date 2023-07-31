using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using QRCoder;


public partial class BookingSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string bookingid = Request.QueryString["bookingId"].ToString();
            FilterList(bookingid);
            GetQRcode(bookingid);
           // BindPrintingInstructions();
        }
    }
     public void GetQRcode(string filepath)
    {
        string sAssetCode = filepath;
        string code = sAssetCode.ToString();
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        using (Bitmap bitMap = qrCode.GetGraphic(25))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                imgEmpPhotoPrev.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                string result = Convert.ToBase64String(byteImage, 0, byteImage.Length);
                //string ImgSrc = StoreImage(result.ToString(), code);


            }
        }
    }
    public void FilterList(string Ids)
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
                            + "bookingMaster?inOutDetails=" + Ids.ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtBlock = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dtBlock.Rows.Count > 0)
                        {
                            List<Booking> lst = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            var lst2 = firstItem.extraFeaturesDetails.ToList();
                            var lst3 = firstItem.extraFeesDetails.ToList();
                            var lst4 = firstItem.userSlotDetails.ToList();
                            DataTable vehicleDetails = ConvertToDataTable(lst1);
                            DataTable extraFeaturesDetails = ConvertToDataTable(lst2);
                            DataTable extraFeesDetails = ConvertToDataTable(lst3);
                            DataTable userSlotDetails = ConvertToDataTable(lst4);
                            decimal total = Convert.ToDecimal(dtBlock.Rows[0]["totalAmount"].ToString().Trim());
                            decimal toTax = Convert.ToDecimal(dtBlock.Rows[0]["taxAmount"].ToString().Trim());
                            decimal exTax;
                            decimal extratot;
                            if (extraFeaturesDetails.Rows.Count > 0)
                            {
                                 extratot = Convert.ToDecimal(extraFeaturesDetails.Rows[0]["totalAmount"].ToString().Trim());
                                 exTax = Convert.ToDecimal(extraFeaturesDetails.Rows[0]["tax"].ToString().Trim());
                            }
                            else
                            {
                                extratot = 0;
                                exTax = 0;
                            }
                            decimal extrafee;
                            if (extraFeesDetails.Rows.Count >0)
                            {
                                 extrafee = Convert.ToDecimal(extraFeesDetails.Rows[0]["extraFee"].ToString().Trim());
                            }
                            else
                            {
                                 extrafee = 0;

                            }
                            
                            decimal Bookingtot = total - toTax - extratot - extrafee;
                            decimal BookingTax = toTax - exTax;
                            decimal bookingTotal = Bookingtot + BookingTax;
                            decimal ExtraTotal = exTax + extratot;
                            lblBookingText.Text = dtBlock.Rows[0]["bookingId"].ToString().Trim();
                            lblPinNo.Text = dtBlock.Rows[0]["pinNo"].ToString().Trim();
                            lblBlockName.Text = dtBlock.Rows[0]["blockName"].ToString().Trim();
                            lblFloorName.Text = dtBlock.Rows[0]["floorName"].ToString().Trim();
                            lblTotalAmount.Text = total.ToString("0.00").Trim();
                            lblVehicleNo.Text = vehicleDetails.Rows[0]["vehicleNumber"].ToString().Trim();
                            lblVehicleType.Text = vehicleDetails.Rows[0]["vehicleTypeName"].ToString().Trim();
                            lblUserName.Text = dtBlock.Rows[0]["userName"].ToString().Trim();
                            lblBranchName.Text = dtBlock.Rows[0]["branchName"].ToString().Trim();
                            
                            lblParkingAmount.Text = bookingTotal.ToString("0.00");
                            lblBookingSlotAmount.Text = Bookingtot.ToString("0.00");
                            lblBookingTaxAmount.Text = BookingTax.ToString("0.00");
                            lblOtherAmount.Text = ExtraTotal.ToString("0.00");
                            lblExtraFee.Text = extrafee.ToString("0.00");
                            decimal paidAmount = Convert.ToDecimal(dtBlock.Rows[0]["paidAmount"].ToString().Trim());
                            lblPaidAmount.Text = paidAmount.ToString("0.00");
                            DateTime Date = Convert.ToDateTime(dtBlock.Rows[0]["fromDate"].ToString().Trim());
                            string dates = Date.ToString("dd MMM yyyy");
                            string Dateweekk = Date.ToString("ddd");
                            string time = dtBlock.Rows[0]["fromTime"].ToString().Trim();
                            lblBookedDateTime.Text = Dateweekk + ',' + dates;
                            lblDate.Text = Dateweekk + ',' + dates;
                            DateTime date = Convert.ToDateTime(time);
                            string s = date.ToString("HH:mm ");
                            //lblTime.Text = s;
                            if(total != paidAmount)
                            {
                                if (paidAmount == 0)
                                {
                                    divAmountpaid.Visible = false;
                                }
                                decimal topay = total - paidAmount;
                                lblTopay.Text = topay.ToString("0.00");
                                divTopay.Visible = true;

                            }
                            else
                            {
                                decimal topay = total - paidAmount;
                                lblTopay.Text = topay.ToString("0.00");
                                divTopay.Visible = false;

                            }
                          
                            lblPaymentType.Text = dtBlock.Rows[0]["paymentTypeName"].ToString().Trim();
                           
                            lblPaymentStatus.Text = dtBlock.Rows[0]["paymentStatus"].ToString().Trim();
                            if (lblPaymentStatus.Text == "P")
                            {
                                lblPaymentStatus.Text = "Paid";
                                lblPaymentStatus.Style.Add("color", "green");
                            }
                            else
                            {
                                lblPaymentStatus.Text = "UnPaid";
                                lblPaymentStatus.Style.Add("color", "red");

                            }
                            txtSlot.Text = userSlotDetails.Rows[0]["slotId"].ToString().Trim();
                            
                            if (extraFeaturesDetails.Rows.Count == 0)
                            {
                                extrafeatures.Visible = false;
                                hrextra.Visible = false;
                            }
                            else
                            {
                                extrafeatures.Visible = true;
                                hrextra.Visible = true;
                                dtlextraFeatures.DataSource = extraFeaturesDetails;
                                dtlextraFeatures.DataBind();
                                dtlextraAmount.DataSource = extraFeaturesDetails;
                                dtlextraAmount.DataBind();
                            }

                            if (extraFeesDetails.Rows.Count == 0)
                            {
                                divextrafee.Visible = false;
                                hrextrafee.Visible = false;
                            }
                            else
                            {
                                divextrafee.Visible = true;
                                hrextrafee.Visible = true;
                                dtlextrafee.DataSource = extraFeesDetails;
                                dtlextrafee.DataBind();
                                dtlextrafeeAmount.DataSource = extraFeesDetails;
                                dtlextrafeeAmount.DataBind();
                            }



                        }
                        else
                        {
                            modal.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        modal.Visible = false;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    //public void BindPrintingInstructions()
    //{
    //    try
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
    //            client.DefaultRequestHeaders.Clear();
    //            client.DefaultRequestHeaders.Accept.Clear();
    //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //            string sUrl = Session["BaseUrl"].ToString().Trim()
    //                    + "printingInstructionsConfig?branchId=45";
    //            //sUrl += Session["branchId"].ToString().Trim();

    //            HttpResponseMessage response = client.GetAsync(sUrl).Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var SmartParkingList = response.Content.ReadAsStringAsync().Result;
    //                int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
    //                string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

    //                if (StatusCode == 1)
    //                {
    //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        dtlInstructions.DataSource = dt;
    //                        dtlInstructions.DataBind();
    //                    }
    //                    else
    //                    {
    //                        dtlInstructions.DataSource = null;
    //                        dtlInstructions.DataBind();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
    //    }
    //}

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
    public class Booking
    {
        public string bookingId { get; set; }
        public string pinNo { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string taxAmount { get; set; }
        public string totalAmount { get; set; }
        public string paidAmount { get; set; }
        public string paymentStatus { get; set; }
        public string paymentType { get; set; }
        public string blockName { get; set; }
        public string floorName { get; set; }
        public string userName { get; set; }
        public string phoneNumber { get; set; }
        public string parkingName { get; set; }
        public string branchName { get; set; }
        public string branchPhoneNumber { get; set; }
        public List<vehicleDetails> vehicleDetails { get; set; }
        public List<userSlotDetails> userSlotDetails { get; set; }
        public List<extraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<extraFeesDetailss> extraFeesDetails { get; set; }

    }
    public class vehicleDetails
    {
        public string vehicleHeaderId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string vehicleTypeName { get; set; }
    }

    public class userSlotDetails
    {
        public string userSlotId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleTypeName { get; set; }
        public string vehicleImageUrl { get; set; }

    }
    public class extraFeaturesDetails
    {
        public string extraFeatureId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraDetail { get; set; }
        public string featureName { get; set; }
        public string tax { get; set; }
        public string totalAmount { get; set; }

    }

    public class extraFeesDetailss
    {
        public string extraFeesDetails { get; set; }
        public string extraFeesId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }

    }



}