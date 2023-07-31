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

public partial class DashBoardNew : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserId"] == null && Session["UserRole"] == null)
            {
                Session.Clear();
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
            }
            if (Session["Type"].ToString().Trim() == "CU")
            {
                lblGvCount.Text = "Booked";
                divBookedGridView.Visible = true;
                if (Session["Branch"].ToString() == "0")
                {
                    if (Session["UserRole"].ToString() == "SA")
                    {
                        BindBookedCountSA();
                    }
                    else
                    {
                        BindBookedCount();
                    }
                }
                else
                {
                    BindBookedCount();
                }
                // lblBrName.Text = Session["SAbranchName"].ToString();
            }
            if (Session["Type"].ToString().Trim() == "CO")
            {
                lblGvCount.Text = "Check Out";
                divCheckOutGridView.Visible = true;

                if (Session["Branch"].ToString() == "0")
                {
                    if (Session["UserRole"].ToString() == "SA")
                    {
                        BindCheckOutCountSA();
                    }
                    else
                    {
                        BindCheckOutCount();
                    }
                }
                else
                {
                    BindCheckOutCount();
                }
                // lblBrName.Text = Session["SAbranchName"].ToString();
            }
            if (Session["Type"].ToString().Trim() == "CI")
            {
                lblGvCount.Text = "Check In";
                divCheckInGridView.Visible = true;

                if (Session["Branch"].ToString() == "0")
                {
                    if (Session["UserRole"].ToString() == "SA")
                    {
                        BindCheckInCountSA();
                    }
                    else
                    {
                        BindCheckInCount();
                    }
                }
                else
                {
                    BindCheckInCount();
                }
                // lblBrName.Text = Session["SAbranchName"].ToString();
            }
            if (Session["Type"].ToString().Trim() == "A")
            {
                lblGvCount.Text = "Available";
                divAvailableGridView.Visible = true;

                if (Session["Branch"].ToString() == "0")
                {
                    if (Session["UserRole"].ToString() == "SA")
                    {
                        BindAvailbleCountSA();
                    }
                    else
                    {
                        BindAvailbleCount();
                    }
                }
                else
                {
                    BindAvailbleCount();
                }
                // lblBrName.Text = Session["SAbranchName"].ToString();
            }

        }
    }
    #endregion

    #region Bind Booked  Count 
    #region Booked Count Click
    protected void lblBooked_Click(object sender, EventArgs e)
    {
        lblGvCount.Text = "Booked";
        divBookedGridView.Visible = true;

        if (Session["UserRole"].ToString() == "SA")
        {
            BindBookedCountSA();
        }
        else
        {
            BindBookedCount();
        }

    }
    #endregion
    #region Bind Booked  Count 
    public void BindBookedCount()
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
                   + "bookingMaster?branchId=" + Session["branchId"].ToString() + "&type=CU&fromDate=" + Session["FromDate"].ToString() + "" +
                   "&toDate=" + Session["Todate"].ToString() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable BookingDetails = new DataTable();
                        List<Booking> lst = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                        BookingDetails.Columns.Add("bookingId");
                        BookingDetails.Columns.Add("totalAmount");
                        BookingDetails.Columns.Add("slotId");
                        BookingDetails.Columns.Add("vehicleTypeName");
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var firstItem = lst.ElementAt(i);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            BookingDetails.NewRow();
                            BookingDetails.Rows.Add(lst[i].bookingId, lst[i].totalAmount, lst1[0].slotId, lst1[0].vehicleTypeName);
                        }
                        if (BookingDetails.Rows.Count > 0)
                        {
                            gvBooked.DataSource = BookingDetails;
                            gvBooked.DataBind();
                        }
                        else
                        {
                            gvBooked.DataSource = null;
                            gvBooked.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);

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
    #region Bind Booked  Count  SA
    public void BindBookedCountSA()
    {
        try
        {
            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["ddlBranch"].ToString() == "0")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                     + "bookingMaster?type=CU&fromDate=" + Session["FromDate"].ToString() + "" +
                     "&toDate=" + Session["Todate"].ToString() + "";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                     + "bookingMaster?branchId=" + Session["ddlBranch"].ToString() + "&type=CU&fromDate=" + Session["FromDate"].ToString() + "" +
                     "&toDate=" + Session["Todate"].ToString() + "";
                }

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable BookingDetails = new DataTable();
                        List<Booking> lst = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                        BookingDetails.Columns.Add("bookingId");
                        BookingDetails.Columns.Add("totalAmount");
                        BookingDetails.Columns.Add("slotId");
                        BookingDetails.Columns.Add("vehicleTypeName");
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var firstItem = lst.ElementAt(i);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            BookingDetails.NewRow();
                            BookingDetails.Rows.Add(lst[i].bookingId, lst[i].totalAmount, lst1[0].slotId, lst1[0].vehicleTypeName);
                        }
                        if (BookingDetails.Rows.Count > 0)
                        {
                            gvBooked.DataSource = BookingDetails;
                            gvBooked.DataBind();
                        }
                        else
                        {
                            gvBooked.DataSource = null;
                            gvBooked.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
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
    #endregion
    #region Check Out Count
    #region Check Out Count Click
    protected void lblCheckOut_Click(object sender, EventArgs e)
    {
        lblGvCount.Text = "Check Out";
        divCheckOutGridView.Visible = true;

        if (Session["UserRole"].ToString() == "SA")
        {
            BindCheckOutCountSA();
        }
        else
        {
            BindCheckOutCount();
        }

    }
    #endregion
    #region Bind CheckOut  Count 
    public void BindCheckOutCount()
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
                    + "bookingMaster?branchId=" + Session["branchId"].ToString().Trim() + "&type=CO&fromDate=" + Session["FromDate"].ToString() + "" +
                    "&toDate=" + Session["Todate"].ToString() + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable BookingDetails = new DataTable();
                        List<Booking> lst = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                        BookingDetails.Columns.Add("bookingId");
                        BookingDetails.Columns.Add("totalAmount");
                        BookingDetails.Columns.Add("vehicleNumber");
                        BookingDetails.Columns.Add("vehicleTypeName");
                        BookingDetails.Columns.Add("outTime");
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var firstItem = lst.ElementAt(i);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            BookingDetails.NewRow();
                            BookingDetails.Rows.Add(lst[i].bookingId, lst[i].totalAmount, lst1[0].vehicleNumber, lst1[0].vehicleTypeName, lst1[0].outTime);
                        }
                        if (BookingDetails.Rows.Count > 0)
                        {
                            gvCheckOut.DataSource = BookingDetails;
                            gvCheckOut.DataBind();
                        }
                        else
                        {
                            gvCheckOut.DataSource = null;
                            gvCheckOut.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
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
    #region Bind CheckOut  Count  SA
    public void BindCheckOutCountSA()
    {
        try
        {
            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["ddlBranch"].ToString() == "0")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                    + "bookingMaster?type=CO&fromDate=" + Session["FromDate"].ToString() + "" +
                    "&toDate=" + Session["Todate"].ToString() + "";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?branchId=" + Session["ddlBranch"].ToString() + "&type=CO&fromDate=" + Session["FromDate"].ToString() + "" +
                      "&toDate=" + Session["Todate"].ToString() + "";
                }
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable BookingDetails = new DataTable();
                        List<Booking> lst = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                        BookingDetails.Columns.Add("bookingId");
                        BookingDetails.Columns.Add("totalAmount");
                        BookingDetails.Columns.Add("vehicleNumber");
                        BookingDetails.Columns.Add("vehicleTypeName");
                        BookingDetails.Columns.Add("outTime");
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var firstItem = lst.ElementAt(i);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            BookingDetails.NewRow();
                            BookingDetails.Rows.Add(lst[i].bookingId, lst[i].totalAmount, lst1[0].vehicleNumber, lst1[0].vehicleTypeName, lst1[0].outTime);
                        }
                        if (BookingDetails.Rows.Count > 0)
                        {
                            gvCheckOut.DataSource = BookingDetails;
                            gvCheckOut.DataBind();
                        }
                        else
                        {
                            gvCheckOut.DataSource = null;
                            gvCheckOut.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
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

    #endregion
    #region Check In Count

    #region Check In Count Click
    protected void lblCheckIn_Click(object sender, EventArgs e)
    {
        lblGvCount.Text = "Check In";
        divCheckInGridView.Visible = true;

        if (Session["UserRole"].ToString() == "SA")
        {
            BindCheckInCountSA();
        }
        else
        {
            BindCheckInCount();
        }


    }
    #endregion
    #region Bind CheckIn Count 
    public void BindCheckInCount()
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
                   + "bookingMaster?branchId=" + Session["branchId"].ToString().Trim() + "&type=CI&fromDate=" + Session["FromDate"].ToString() + "" +
                   "&toDate=" + Session["ToDate"].ToString() + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable BookingDetails = new DataTable();
                        List<Booking> lst = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                        BookingDetails.Columns.Add("bookingId");
                        BookingDetails.Columns.Add("totalAmount");
                        BookingDetails.Columns.Add("vehicleNumber");
                        BookingDetails.Columns.Add("vehicleTypeName");
                        BookingDetails.Columns.Add("inTime");
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var firstItem = lst.ElementAt(i);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            BookingDetails.NewRow();
                            BookingDetails.Rows.Add(lst[i].bookingId, lst[i].totalAmount, lst1[0].vehicleNumber, lst1[0].vehicleTypeName, lst1[0].inTime);
                        }
                        if (BookingDetails.Rows.Count > 0)
                        {
                            gvCheckIn.DataSource = BookingDetails;
                            gvCheckIn.DataBind();
                        }
                        else
                        {
                            gvCheckIn.DataSource = null;
                            gvCheckIn.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
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
    #region Bind CheckIn Count SA
    public void BindCheckInCountSA()
    {
        try
        {

            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["ddlBranch"].ToString() == "0")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                    + "bookingMaster?type=CI&fromDate=" + Session["FromDate"].ToString() + "" +
                    "&toDate=" + Session["ToDate"].ToString() + "";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?branchId=" + Session["ddlBranch"].ToString() + "&type=CI&fromDate=" + Session["FromDate"].ToString() + "" +
                      "&toDate=" + Session["ToDate"].ToString() + "";
                }

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {

                        DataTable BookingDetails = new DataTable();
                        List<Booking> lst = JsonConvert.DeserializeObject<List<Booking>>(ResponseMsg);
                        BookingDetails.Columns.Add("bookingId");
                        BookingDetails.Columns.Add("totalAmount");
                        BookingDetails.Columns.Add("vehicleNumber");
                        BookingDetails.Columns.Add("vehicleTypeName");
                        BookingDetails.Columns.Add("inTime");
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var firstItem = lst.ElementAt(i);
                            var lst1 = firstItem.vehicleDetails.ToList();
                            BookingDetails.NewRow();
                            BookingDetails.Rows.Add(lst[i].bookingId, lst[i].totalAmount, lst1[0].vehicleNumber, lst1[0].vehicleTypeName, lst1[0].inTime);
                        }
                        if (BookingDetails.Rows.Count > 0)
                        {
                            gvCheckIn.DataSource = BookingDetails;
                            gvCheckIn.DataBind();
                        }
                        else
                        {
                            gvCheckIn.DataSource = null;
                            gvCheckIn.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
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

    #endregion
    #region Available Count

    #region Available Count Click
    protected void lblAvailable_Click(object sender, EventArgs e)
    {
        lblGvCount.Text = "Check In";
        divCheckInGridView.Visible = true;

        if (Session["UserRole"].ToString() == "SA")
        {
            BindAvailbleCount();
        }
        else
        {
            BindAvailbleCountSA();
        }
    }
    #endregion
    #region Bind Available Count  
    public void BindAvailbleCount()
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
                       + "parkingSlot?branchId=" + Session["branchId"].ToString().Trim() + "&type=A";
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
                            gvAvailableList.DataSource = dt;
                            gvAvailableList.DataBind();
                        }
                        else
                        {
                            gvCheckIn.DataSource = null;
                            gvCheckIn.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
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
    #region Bind Available Count SA
    public void BindAvailbleCountSA()
    {
        try
        {
            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["ddlBranch"].ToString() == "0")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                    + "parkingSlot?type=A";
                    //Response.Redirect("DashBoardDetails.aspx?qUrl=" + sUrl.Trim() + "&qType= CI");
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                      + "parkingSlot?branchId=" + Session["ddlBranch"].ToString() + "&type=A";
                    //Response.Redirect("DashBoardDetails.aspx?qUrl=" + sUrl.Trim() + "&qType= CI");
                }

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
                            gvAvailableList.DataSource = dt;
                            gvAvailableList.DataBind();
                        }
                        else
                        {
                            gvCheckIn.DataSource = null;
                            gvCheckIn.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
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

    #endregion
    #region  Close Click
    protected void linkclose_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
        divBookedGridView.Visible = false;
        divCheckInGridView.Visible = false;
        divCheckOutGridView.Visible = false;
    }
    #endregion
    #region Booking Class
    public class Booking
    {
        public string bookingId { get; set; }
        public string totalAmount { get; set; }
        public List<vehicleDetails> vehicleDetails { get; set; }

    }
    public class vehicleDetails
    {
        public string vehicleTypeName { get; set; }
        public string slotId { get; set; }
        public string vehicleNumber { get; set; }
        public string outTime { get; set; }
        public string inTime { get; set; }

    }
    #endregion
}