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
using System.Text;

public partial class BookingSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["BaseUrl"] = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].Trim();
        if (!IsPostBack)
        {
            string bookingid = Request.QueryString["ReferenceNo"].ToString();
            string id = StringEncryption.Decrypt(bookingid.Trim());
            GetPassDetails(id);
        }
    }
    #region QR Code Generate 
    public void GetQRcode(string parkingPassTransId)
    {
        string sAssetCode = "P" + parkingPassTransId;
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
    #region Get Pass Details
    public void GetPassDetails(string parkingPassTransId)
    {
        try
        {
            DivPassTicket.Visible = true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "passTransaction?ParkingPassTransactionId=" + parkingPassTransId + "";
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
                            List<GetPassTicket> lst = JsonConvert.DeserializeObject<List<GetPassTicket>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.passType.ToList();
                            DataTable passtype = ConvertToDataTable(lst1);

                            GetQRcode(parkingPassTransId);
                            lblPassParkinngName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblPassBranchName.Text = dt.Rows[0]["branchName"].ToString();
                            lblTicketPassType.Text = passtype.Rows[0]["passType"].ToString();
                            lblPassMobileNo.Text = dt.Rows[0]["phoneNumber"].ToString();
                            lblPassStartDate.Text = dt.Rows[0]["validStartDate"].ToString();
                            lblPassEndDate.Text = dt.Rows[0]["validEndDate"].ToString();
                            lblPassMode.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblUserPassType.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblPassVehicleType.Text = dt.Rows[0]["vehicleName"].ToString();
                            lblPassId.Text = dt.Rows[0]["parkingPassTransId"].ToString();
                            if (lblPassMode.Text == "VIP")
                            {
                                DivPassimg.Attributes.Add("class", "passVIP");
                            }
                            else
                            {
                                DivPassimg.Attributes.Add("class", "passNormal");
                            }
                            Session["Pass"] = StringEncryption.Encrypt(parkingPassTransId);

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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "click", "TicketClick(event);", true);
        }
    }
    #endregion
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
    #region Pass Ticket Class  
    public class GetPassTicket
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userId { get; set; }
        public string phoneNumber { get; set; }
        public string parkingPassTransId { get; set; }
        public string validStartDate { get; set; }
        public string validEndDate { get; set; }
        public string validStartTime { get; set; }
        public string validEndTime { get; set; }
        public string amount { get; set; }
        public string tax { get; set; }
        public string totalAmount { get; set; }
        public string taxId { get; set; }
        public string vehicleName { get; set; }
        public string parkingName { get; set; }
        public string branchName { get; set; }
        public List<GetpassType> passType { get; set; }

    }
    public class GetpassType
    {
        public string passType { get; set; }
        public string passCategory { get; set; }

    }

    #endregion

}