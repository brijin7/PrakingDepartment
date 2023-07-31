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


public partial class ScanCheckInCheckOut : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (string.IsNullOrEmpty(Session["BranchId"] as string))
        {
            if (Session["UserRole"].ToString() == "SA")
            {
                Response.Redirect("~/Login/OwnerLogin.aspx");
            }
            else
            {
                Response.Redirect("~/Login/BranchLogin.aspx");
            }
        }
        if (!IsPostBack)
        {
            BindCount();
            ViewState["BlockCount"] = "0";
            ViewState["FloorCount"] = "0";
            BindBlock();

            txtBookingId.Focus();
            if (ViewState["BlockCount"].ToString() == "0" || ViewState["FloorCount"].ToString() == "0")
            {
                divBlockFloor.Visible = true;
            }
            else
            {
                divBlockFloor.Visible = false;
            }
        }

        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
        ViewState["BindCheckInList"] = "CheckIn";
        ViewState["BindCheckOutList"] = "";
        lblBranchName.Text = Session["branchName"].ToString();
        lblUserName.Text = Session["UserName"].ToString().Trim();
        lblParkingName.Text = Session["parkingName"].ToString().Trim();
        if (Session["UserRole"].ToString() == "SA")
        {
            lblUserRole.Text = "Sadmin";
        }
        else if (Session["UserRole"].ToString() == "A")
        {
            lblUserRole.Text = "Admin";
        }
        else
        {
            lblUserRole.Text = "User";

        }
    }
    #endregion

    #region Bind Block
    public void BindBlock()
    {
        ViewState["floorId"] = "";
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "blockMaster?approvalStatus=A&activeStatus=A&branchId=";
                sUrl += Session["branchId"].ToString().Trim();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlblock.DataSource = dt;
                            ddlblock.DataValueField = "blockId";
                            ddlblock.DataTextField = "blockName";
                            ddlblock.DataBind();
                            if (dt.Rows.Count == 1)
                            {
                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                                BindFloorName();
                                ViewState["BlockCount"] = "1";
                            }
                            else
                            {
                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                                BindFloorName();
                            }
                        }
                        else
                        {
                            ddlblock.DataBind();
                        }
                        //ddlblock.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlblock.ClearSelection();
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Block SelectedIndexChanged
    protected void ddlblock_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["floorId"] = "";
        ddlfloor.ClearSelection();

        if (ddlblock.SelectedIndex == 0)
        {
            divBtncheckinandout.Visible = false;

            divCheckIn.Visible = false;
            divGvCheckOut.Visible = false;
        }
        else
        {
            BindFloorName();
        }
        if (ddlfloor.SelectedIndex == 0)
        {
            divBtncheckinandout.Visible = false;

            divCheckIn.Visible = false;
            divGvCheckOut.Visible = false;
        }
    }
    #endregion
    #region  Bind Floor
    public void BindFloorName()
    {
        ViewState["floorId"] = "";
        ddlfloor.Items.Clear();
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "floorMaster?activeStatus=A&blockId=";
                sUrl += ddlblock.SelectedValue;
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("floorVehicleDetails").Remove();
                            item.Property("floorFeaturesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {
                            ddlfloor.DataSource = dt;
                            ddlfloor.DataValueField = "floorId";
                            ddlfloor.DataTextField = "floorName";
                            ddlfloor.DataBind();
                            if (dt.Rows.Count == 1)
                            {
                                ViewState["FloorCount"] = "1";
                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                                ViewState["floorId"] = ddlfloor.SelectedValue;
                                divForm.Visible = true;
                                divBtncheckinandout.Visible = true;

                            }
                            else
                            {
                                ViewState["floorId"] = ddlfloor.SelectedValue;
                                divForm.Visible = true;
                                divBtncheckinandout.Visible = true;
                            }

                        }
                        else
                        {
                            ddlfloor.Items.Clear();
                        }
                        // ddlfloor.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlfloor.Items.Clear();
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Floor SelectedIndexChanged
    protected void ddlfloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlfloor.SelectedIndex == 0)
        {
            divBtncheckinandout.Visible = false;

            divCheckIn.Visible = false;
            divGvCheckOut.Visible = false;
        }
        else
        {
            divForm.Visible = true;
            divBtncheckinandout.Visible = true;

            ViewState["floorId"] = ddlfloor.SelectedValue;
            BindCheckInList();
        }

    }
    #endregion
    #region  Bind Floor ID
    public void BindFloorId()
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
                            + "floorMaster?branchId=" + Session["branchId"].ToString();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("floorVehicleDetails").Remove();
                            item.Property("floorFeaturesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["floorId"] = dt.Rows[0]["floorId"].ToString();
                        }
                    }
                    else
                    {
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
    #endregion
    #region Bind Count
    public void BindCount()
    {
        DateTime utcTime = DateTime.Now;
        string date = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
        string[] time = date.Split('T');
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?branchId=" + Session["branchId"].ToString().Trim() + "&type=C"
                      + "&fromDate=" + date.ToString() + "&toDate=" + date.ToString() + "";

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
                            lblpass.Text = dt.Rows[0]["passCheckedIn"].ToString();
                            lblcheckin.Text = dt.Rows[0]["checkedInCount"].ToString();
                            lblcheckout.Text = dt.Rows[0]["checkedOutCount"].ToString();
                            lblBooked.Text = dt.Rows[0]["bookedCount"].ToString();

                            //"passCheckedIn": 0,
                            //"bookedCount": 0,
                            //"checkedInCount": 0,
                            //"checkedOutCount": 0,
                            //"available": 3775,
                            //"filled": 3970,   
                            //"reserved": 0
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
    }
    #endregion

    #region Payment Mode Bind and Selected Index Changed
    #region Payment Mode
    public void BindDdlPayment()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlPayment.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "configMaster?configTypename=Payment&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtDEmployeeType = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        ddlPayment.DataSource = dtDEmployeeType;
                        ddlPayment.DataValueField = "configId";
                        ddlPayment.DataTextField = "configName";
                        ddlPayment.DataBind();
                        ddlPayment.Items.Remove(ddlPayment.Items.FindByText("payLater"));
                        ddlPayment.Items.Remove(ddlPayment.Items.FindByText("Pass"));

                    }
                    else
                    {
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
    public void BindReDdlPayment()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlRepayment.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "configMaster?configTypename=Payment&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtDEmployeeType = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlRepayment.DataSource = dtDEmployeeType;
                        ddlRepayment.DataValueField = "configId";
                        ddlRepayment.DataTextField = "configName";
                        ddlRepayment.DataBind();
                        ddlRepayment.Items.Remove(ddlPayment.Items.FindByText("payLater"));
                        ddlRepayment.Items.Remove(ddlPayment.Items.FindByText("Pass"));
                    }
                    else
                    {
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
    #endregion
    //#region Payment Mode selected
    //protected void ddlPayment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlPayment.SelectedItem.Text == "Card" || ddlPayment.SelectedItem.Text == "Online")
    //    {
    //        btnCheckInPopup.Text = "Pay and CheckOut";
    //    }
    //    else
    //    {
    //        btnCheckInPopup.Text = "Pay and Check Out";

    //    }
    //}

    //protected void ddlRepayment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlRepayment.SelectedItem.Text == "Card" || ddlRepayment.SelectedItem.Text == "Online")
    //    {
    //        btnCheckInPopup.Text = "Pay and CheckOut";
    //    }
    //    else
    //    {
    //        btnCheckInPopup.Text = "Pay and Check Out";

    //    }

    //}
    //#endregion
    #endregion

    #region Scan Text Change Event and Get MEthod
    #region BookingID TextChange
    protected void txtBookingId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtBookingId.Text != "")
            {
                string BookingPassId = string.Empty;
                string BorP = string.Empty;

                BookingPassId = txtBookingId.Text.ToUpper();
                string[] BookingPassIds = BookingPassId.Split(';');
                BorP = BookingPassIds[0].Substring(0, 1);
                if (BookingPassIds.Length > 0)
                {
                    ClearLabel();
                    if (BorP.Trim() == "B")
                    {
                        ScanBookingId(BookingPassIds[0].Trim());
                    }
                    else if (BorP.Trim() == "P")
                    {
                        ClearLabel();
                        ScanBookingPassId(BookingPassIds[0].Trim());
                    }
                    else
                    {
                        GetDataBasedOnVehicleNumberPhoneDetails(BookingPassIds[0].Trim());
                    }
                }
                else
                {
                    txtBookingId.Focus();
                    ResetInput();
                    lblAlreadyOut.Visible = true;
                    lblAlreadyOut.Text = "Invalid QRcode";
                    lblAlreadyIn.Visible = true;
                    lblAlreadyIn.Text = "Invalid QRcode";
                    lblout.Visible = false;
                    lblGridIn.Visible = false;
                    lblGridIn.Text = "";
                    lblout.Text = "";
                    ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                    BindCheckInList();
                    txtBookingId.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }


    }
    public void GetDataBasedOnVehicleNumberPhoneDetails(string BookingIds)
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
                      + "getDataBasedOnVehicleNumberPhone?inOutDetails=" + BookingIds.Trim() + "&floorId=" + ddlfloor.SelectedValue + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("extraFeesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Columns.Contains("passBookingTransactionId"))
                            {
                                ScanBookingPassId(dt.Rows[0]["passBookingTransactionId"].ToString());
                            }
                            else
                            {
                                ScanBookingId(dt.Rows[0]["bookingId"].ToString());
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            ResetInput();
                            clear();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtBookingId.Text = string.Empty;
                        ResetInput();
                        clear();
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
    #region ScanBookingId And Pass Id get Method
    public void FilterList(string Ids)
    {
        try
        {
            divCheckIn.Visible = false;
            divGvCheckOut.Visible = false;
            gvCheckIn.Visible = false;
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
                            lblBookingId.Text = dtBlock.Rows[0]["bookingId"].ToString().Trim();
                            lblPinNo.Text = dtBlock.Rows[0]["pinNo"].ToString().Trim();
                            lblBlockName.Text = dtBlock.Rows[0]["blockName"].ToString().Trim();
                            lblFloorName.Text = dtBlock.Rows[0]["floorName"].ToString().Trim();
                            lblReBookingId.Text = dtBlock.Rows[0]["bookingId"].ToString().Trim();
                            lblRePinNo.Text = dtBlock.Rows[0]["pinNo"].ToString().Trim();
                            lblReBlockName.Text = dtBlock.Rows[0]["blockName"].ToString().Trim();
                            lblReFloorName.Text = dtBlock.Rows[0]["floorName"].ToString().Trim();

                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
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
    public void ScanBookingId(string Ids)
    {
        try
        {
            divCheckIn.Visible = true;
            divGvCheckOut.Visible = true;
            gvCheckIn.Visible = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?inOutDetails=" + Ids.Trim() + "&type=P";
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
                            List<OutDetails> lst = JsonConvert.DeserializeObject<List<OutDetails>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);

                            string[] dayhourlbl;
                            var lst1 = firstItem.slotDetails.ToList();
                            DataTable userSlotTable = ConvertToDataTable(lst1);
                            string vehicleStatus = dt.Rows[0]["vehicleStatus"].ToString();
                            string slotd = string.Empty;
                            if (userSlotTable.Rows.Count > 0)
                            {
                                slotd = userSlotTable.Rows[0]["slotId"].ToString();

                            }
                            else
                            {
                                slotd = dt.Rows[0]["slotId"].ToString();
                            }
                        
                            
                            string vehicleNumber = dt.Rows[0]["vehicleNumber"].ToString();
                            string vehicleHeaderId = dt.Rows[0]["vehicleHeaderId"].ToString();
                            string BookingId = dt.Rows[0]["bookingPassId"].ToString();
                            string pinNo = dt.Rows[0]["pinNo"].ToString();
                            decimal initialAmount = Convert.ToDecimal(dt.Rows[0]["initialAmount"].ToString());
                            decimal remainingAmount = Convert.ToDecimal(dt.Rows[0]["remainingAmount"].ToString());
                            decimal extendAmount = Convert.ToDecimal(dt.Rows[0]["extendAmount"].ToString());
                            decimal extendTax = Convert.ToDecimal(dt.Rows[0]["extendTax"].ToString());
                            decimal topayAmount = remainingAmount + extendAmount;

                            string extendDayHour = dt.Rows[0]["extendDayHour"].ToString();
                            string bookingDurationType = "D"; //dt.Rows[0]["bookingDurationType"].ToString();
                            dayhourlbl = extendDayHour.Trim().Split('-');
                            string extendDayHours = extendDayHour.Trim();
                            if (extendDayHours.EndsWith("r"))
                            {
                                dayorHour.InnerText = "(in hours)";
                            }
                            else
                            {
                                dayorHour.InnerText = "(in days)";
                            }

                            ViewState["vehicleStatus"] = vehicleStatus.Trim();
                            ViewState["BookingId"] = BookingId.Trim();
                            ViewState["vehicleNumber"] = vehicleNumber.Trim();
                            ViewState["vehicleHeaderId"] = vehicleHeaderId.Trim();
                            ViewState["slotd"] = slotd.Trim();
                            ViewState["remainingAmount"] = remainingAmount.ToString().Trim();
                            ViewState["extendAmount"] = extendAmount.ToString().Trim();
                            ViewState["extendTax"] = extendTax.ToString().Trim();
                            ViewState["topayAmount"] = topayAmount.ToString().Trim();
                            ViewState["bookingDurationType"] = bookingDurationType.Trim();
                            if (vehicleStatus.Trim() == "")
                            {
                                string times;
                                string[] date2;
                                DateTime utcTime = DateTime.Now;
                                times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                date2 = times.Split(' ');
                                UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), times, null, "I", slotd.Trim());
                                txtBookingId.Focus();

                            }
                            else if (vehicleStatus.Trim() == "I")
                            {
                                lblRemAmount.Text = remainingAmount.ToString("0.00");
                                if (extendAmount != 0)
                                {
                                    if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                                    {
                                        lblAlreadyOut.Visible = true;
                                        lblAlreadyOut.Text = "Please Contact Booking Counter Can not Check Out Here";
                                        lblAlreadyIn.Visible = false;
                                        lblAlreadyIn.Text = "";
                                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                                        BindCheckOutList();
                                    }
                                    else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                                    {
                                        lblAlreadyIn.Visible = true;
                                        lblAlreadyIn.Text = "Please Contact Booking Counter Can not Check Out Here";
                                        lblAlreadyOut.Visible = false;
                                        lblAlreadyOut.Text = "";
                                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                                        BindCheckInList();
                                    }
                                    txtBookingId.Text = "";
                                   
                                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Please Contact Booking Counter Can not Check Out Here');", true);
                                    //ViewState["Flag"] = "0";
                                    //BindDdlPayment();
                                    //FilterList(BookingId.Trim());
                                    //lbltaxAmount.Text = extendTax.ToString("0.00").Trim();
                                    //dayhourlbl = extendDayHour.Split('-');
                                    //lblTimeExtended.Text = dayhourlbl[0].Trim();
                                    //lblExtendedAmount.Text = extendAmount.ToString("0.00").Trim();
                                    //lblVehicleNo.Text = vehicleNumber.Trim();
                                    //lblInitialAmount.Text = initialAmount.ToString("0.00");
                                    //decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(lblRemAmount.Text.Trim());
                                    //decimal TotAmount = Convert.ToDecimal(initialAmount) + Convert.ToDecimal(extendAmount);
                                    //lblTopayAmount.Text = Amount.ToString("0.00");
                                    //lblTotalAmount.Text = TotAmount.ToString("0.00");
                                    //modal.Visible = true;
                                    //divRemaining.Visible = false;
                                    //divextend.Visible = true;
                                    //remainingre.Visible = false;
                                    //extended.Visible = true;

                                }
                                else if (remainingAmount != 0)
                                {                                   
                                    txtBookingId.Focus();
                                    txtBookingId.Text = "";
                                    lblAlreadyOut.Visible = true;
                                    lblAlreadyOut.Text = "Please Contact Booking Counter Can not Check Out Here";
                                    lblAlreadyIn.Visible = false;
                                    lblAlreadyIn.Text = "";
                                    ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);

                                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Please Contact Booking Counter Can not Check Out Here');", true);
                                    //remainingre.Visible = true;
                                    //ViewState["Flag"] = "1";
                                    //BindReDdlPayment();
                                    //FilterList(BookingId.Trim());
                                    //lblReRemainingAmount.Text = remainingAmount.ToString("0.00");
                                    //lblReVehicleNo.Text = vehicleNumber.Trim();
                                    //lblReInitialAmount.Text = initialAmount.ToString("0.00");
                                    //decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(lblRemAmount.Text.Trim());
                                    //decimal TotAmount = Convert.ToDecimal(initialAmount) + Convert.ToDecimal(extendAmount);
                                    //lblReToPay.Text = Amount.ToString("0.00");
                                    //lblReTotalAmount.Text = TotAmount.ToString("0.00");
                                    //modal.Visible = true;
                                    //divRemaining.Visible = true;
                                    //divextend.Visible = false;
                                    //extended.Visible = false;

                                }
                                else if (remainingAmount == 0 && extendAmount == 0)
                                {

                                    string times;
                                    string[] date2;
                                    DateTime utcTime = DateTime.Now;
                                    times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                    date2 = times.Split(' ');
                                    UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), null, times, "O", slotd.Trim());
                                    txtBookingId.Focus();

                                }

                            }

                        }
                        else
                        {
                            txtBookingId.Focus();
                            ResetInput();
                            lblout.Visible = false;
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            lblout.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblAlreadyOut.Visible = true;
                                lblAlreadyOut.Text = "Already Checked Out";
                                lblAlreadyIn.Visible = false;
                                lblAlreadyIn.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                                BindCheckOutList();
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblAlreadyIn.Visible = true;
                                lblAlreadyIn.Text = "Already Checked Out";
                                lblAlreadyOut.Visible = false;
                                lblAlreadyOut.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                                BindCheckInList();
                            }
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        txtBookingId.Focus();
                        ResetInput();
                        lblout.Visible = false;
                        lblGridIn.Visible = false;
                        lblGridIn.Text = "";
                        lblout.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblAlreadyOut.Visible = true;
                            lblAlreadyOut.Text = ResponseMsg;
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                            BindCheckOutList();
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblAlreadyIn.Visible = true;
                            lblAlreadyIn.Text = ResponseMsg;
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                            BindCheckInList();
                        }
                        txtBookingId.Focus();

                    }
                }
                else
                {
                    txtBookingId.Focus();
                    ResetInput();
                    lblout.Visible = false;
                    lblGridIn.Visible = false;
                    lblGridIn.Text = "";
                    lblout.Text = "";
                    if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                    {
                        lblAlreadyOut.Visible = true;
                        lblAlreadyOut.Text = "Invalid QRcode";
                        lblAlreadyIn.Visible = false;
                        lblAlreadyIn.Text = "";
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                        BindCheckOutList();
                    }
                    else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                    {
                        lblAlreadyIn.Visible = true;
                        lblAlreadyIn.Text = "Invalid QRcode";
                        lblAlreadyOut.Visible = false;
                        lblAlreadyOut.Text = "";
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                        BindCheckInList();
                    }
                    txtBookingId.Focus();
                }

            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }



    public void ScanBookingPassId(string Ids)
    {
        try
        {
            divCheckIn.Visible = true;
            divGvCheckOut.Visible = true;
            gvCheckIn.Visible = true;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                             + "passBooking?inOutDetails=" + Ids + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {

                        List<PassBooking> extra = JsonConvert.DeserializeObject<List<PassBooking>>(ResponseMsg);
                        var firstItemextra = extra.ElementAt(0);
                        var lst1 = firstItemextra.vehicleDetails.ToList();
                        var lst3 = firstItemextra.extraFeesDetails.ToList();
                        DataTable GetvehicleDetails = ConvertToDataTable(lst1);
                        DataTable GetextraFeesDetailsPass = ConvertToDataTable(lst3);
                        DataTable dtPassIN = ConvertToDataTable(extra);
                        var userSlot = firstItemextra.userSlotDetails.ToList();
                        DataTable userSlotTable = ConvertToDataTable(userSlot);
                        string SlotId = string.Empty;
                        if (userSlotTable.Rows.Count > 0)
                        {
                            SlotId = userSlotTable.Rows[0]["slotId"].ToString();

                        }
                        else
                        {
                            SlotId = GetvehicleDetails.Rows[0]["slotId"].ToString();
                        }
                        string paymentStatus = dtPassIN.Rows[0]["paymentStatus"].ToString();
                        string vehicleStatus = GetvehicleDetails.Rows[0]["vehicleStatus"].ToString();
                        string vehicleNumber = GetvehicleDetails.Rows[0]["vehicleNumber"].ToString();
                        string vehicleHeaderId = GetvehicleDetails.Rows[0]["vehicleHeaderId"].ToString();
                        string BookingId = GetvehicleDetails.Rows[0]["bookingPassId"].ToString();
                        string passTransactionId = dtPassIN.Rows[0]["passTransactionId"].ToString();
                      
                        decimal Extrafee = Convert.ToDecimal(dtPassIN.Rows[0]["extraFeesAmount"].ToString().Trim());
                        decimal Total = Extrafee;
                        string times;

                        string[] date2;
                        DateTime utcTime = DateTime.Now;
                        times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                        date2 = times.Split(' ');
                        ViewState["vehicleNumber"] = vehicleNumber.Trim();
                        ViewState["vehicleHeaderId"] = vehicleHeaderId.Trim();
                        ViewState["SlotId"] = SlotId.Trim();
                        if (vehicleStatus == " ")
                        {

                            UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), times, null, "I", SlotId.Trim());
                            txtBookingId.Focus();

                        }
                        else if (vehicleStatus == "I")
                        {
                            if (paymentStatus.Trim() == "P")
                            {
                                UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), null, times, "O", SlotId.Trim());
                            }
                            else if (paymentStatus.Trim() == "N")
                            {
                                txtBookingId.Focus();
                                txtBookingId.Text = "";
                                lblAlreadyOut.Visible = true;
                                lblAlreadyOut.Text = "Please Contact Booking Counter Can not Check Out Here";
                                lblAlreadyIn.Visible = false;
                                lblAlreadyIn.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);


                            }


                        }
                        else
                        {
                            txtBookingId.Focus();
                            ResetInput();
                            lblout.Visible = false;
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            lblout.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblAlreadyOut.Visible = true;
                                lblAlreadyOut.Text = "Already Checked Out";
                                lblAlreadyIn.Visible = false;
                                lblAlreadyIn.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                                BindCheckOutList();
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblAlreadyIn.Visible = true;
                                lblAlreadyIn.Text = "Already Checked Out";
                                lblAlreadyOut.Visible = false;
                                lblAlreadyOut.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                                BindCheckInList();
                            }
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        txtBookingId.Focus();
                        ResetInput();
                        lblout.Visible = false;
                        lblGridIn.Visible = false;
                        lblGridIn.Text = "";
                        lblout.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblAlreadyOut.Visible = true;
                            lblAlreadyOut.Text = ResponseMsg;
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                            BindCheckOutList();
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblAlreadyIn.Visible = true;
                            lblAlreadyIn.Text = ResponseMsg;
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                            BindCheckInList();
                        }
                        txtBookingId.Focus();
                    }

                }
                else
                {
                    txtBookingId.Focus();
                    ResetInput();
                    lblout.Visible = false;
                    lblGridIn.Visible = false;
                    lblGridIn.Text = "";
                    lblout.Text = "";
                    if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                    {
                        lblAlreadyOut.Visible = true;
                        lblAlreadyOut.Text = "Invalid QRcode";
                        lblAlreadyIn.Visible = false;
                        lblAlreadyIn.Text = "";
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                        //BindCheckOutList();
                    }
                    else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                    {
                        lblAlreadyIn.Visible = true;
                        lblAlreadyIn.Text = "Invalid QRcode";
                        lblAlreadyOut.Visible = false;
                        lblAlreadyOut.Text = "";
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                        //BindCheckInList();
                    }
                    txtBookingId.Focus();
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

    #region Btn Check In and GridView For Check In List
    #region  Bind Check In
    public void BindCheckInList()
    {
        try
        {
            ViewState["BindCheckInList"] = "CheckIn";
            ViewState["BindCheckOutList"] = "";

            divCheckIn.Visible = true;
            divGvCheckOut.Visible = false;
            gvCheckIn.Visible = false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "bookingMaster?&type=I&floorId=" + ViewState["floorId"].ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<OutDetails> Inout = JsonConvert.DeserializeObject<List<OutDetails>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Inout);

                        if (dt.Rows.Count > 0)
                        {
                            gvCheckIn.DataSource = Inout;
                            gvCheckIn.DataBind();
                            txtBookingId.Focus();

                        }
                        else
                        {
                            txtBookingId.Focus();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        }
                    }
                    else
                    {
                        gvCheckIn.DataSource = null;
                        gvCheckIn.DataBind();
                        txtBookingId.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    protected void gvCheckIn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var Options = e.Row.DataItem as OutDetails;
            var dataList = e.Row.FindControl("DataList1") as DataList;
            dataList.DataSource = Options.slotDetails;
            dataList.DataBind();
        }
    }
    #endregion
    #region btnCheckIN
    protected void btnCheckIn_Click(object sender, EventArgs e)
    {
        txtBookingId.Text = "";
        lblGridIn.Visible = false;
        lblGridIn.Text = "";
        lblout.Visible = false;
        lblout.Text = "";
        lblAlreadyOut.Visible = false;
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Visible = false;


        lblAlreadyIn.Text = "";
        BindCheckInList();

    }
    #endregion
    #endregion

    #region Btn Check Out and GridView For Check Out List
    #region Bind Check Out
    public void BindCheckOutList()
    {
        ViewState["BindCheckOutList"] = "CheckOut";
        ViewState["BindCheckInList"] = "";

        try
        {
            divCheckIn.Visible = false;
            divGvCheckOut.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "bookingMaster?type=O&floorId=" + ViewState["floorId"].ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<OutDetails> Inout = JsonConvert.DeserializeObject<List<OutDetails>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Inout);
                        if (dt.Rows.Count > 0)
                        {
                            gvCheckOut.DataSource = Inout;
                            gvCheckOut.DataBind();
                            txtBookingId.Focus();

                        }
                        else
                        {
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        gvCheckOut.DataSource = null;
                        gvCheckOut.DataBind();
                        txtBookingId.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }

    protected void gvCheckOut_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var Options = e.Row.DataItem as OutDetails;
            var dataList = e.Row.FindControl("DataList2") as DataList;
            dataList.DataSource = Options.slotDetails;
            dataList.DataBind();
        }
    }
    #endregion
    #region btnCheckOut
    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        txtBookingId.Text = "";
        lblAlreadyOut.Visible = false;
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Visible = false;
        lblAlreadyIn.Text = "";
        lblGridIn.Visible = false;
        lblGridIn.Text = "";
        lblout.Visible = false;
        lblout.Text = "";
        BindCheckOutList();

    }
    #endregion
    #endregion

    #region Btn Gridview CheckIn and Check Out Details
    #region GridBtnCheck In
    protected void gvbtnCheckIn_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string times;
            string[] date2;
            string slotd = string.Empty;
            DateTime utcTime = DateTime.Now;
            times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            date2 = times.Split(' ');
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblbookingPassIdIn = (Label)gvrow.FindControl("lblbookingPassIdIn");
            Label lblvehicleNumberIn = (Label)gvrow.FindControl("lblvehicleNumberIn");
            Label lblvehicleHeaderId = (Label)gvrow.FindControl("lblvehicleHeaderId");
            var dataList = gvrow.FindControl("DataList1") as DataList;
            if (dataList.Items.Count > 0)
            {
                Label lbluserslotIdIn = dataList.Items[0].FindControl("lbluserslotIdIn") as Label;
                Label lblSlotIdIn = dataList.Items[0].FindControl("lblSlotIdIn") as Label;
                slotd = lblSlotIdIn.Text;
            }

            UpdateCheckInOutDetails(lblvehicleNumberIn.Text, lblvehicleHeaderId.Text, times, null, "I", slotd.Trim());
            BindCheckInList();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region GridBtnCheck Out
    protected void gvbtnCheckout_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblout.Visible = false;
            string times;
            string[] date2;
            string slotid = string.Empty;
            DateTime utcTime = DateTime.Now;
            times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            date2 = times.Split(' ');
            string[] dayhourlbl;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            var dataList = gvrow.FindControl("DataList2") as DataList;
            if (dataList.Items.Count > 0)
            {
                Label lbluserslotIdOut = dataList.Items[0].FindControl("lbluserslotIdOut") as Label;
                Label lblSlotIdOut = dataList.Items[0].FindControl("lblSlotIdOut") as Label;
                slotid = lblSlotIdOut.Text;
            }
            ViewState["SlotId"] = slotid.Trim();
            Label lblbookingPassId = (Label)gvrow.FindControl("lblbookingPassId");
            Label lblvehicleNumber = (Label)gvrow.FindControl("lblvehicleNumber");
            Label lblvehicleHeaderId = (Label)gvrow.FindControl("lblvehicleHeaderId");
            Label lblinTime = (Label)gvrow.FindControl("lblinTime");
            Label lblvehicleStatus = (Label)gvrow.FindControl("lblvehicleStatus");
            Label lblextendDayHour = (Label)gvrow.FindControl("lblextendDayHour");
            dayhourlbl = lblextendDayHour.Text.Split('-');
            Label lblextendAmount = (Label)gvrow.FindControl("lblextendAmount");
            Label lblextendtax = (Label)gvrow.FindControl("lblextendtax");
            Label lblremainingAmount = (Label)gvrow.FindControl("lblremainingAmount");
            Label lbltotalAmount = (Label)gvrow.FindControl("lblinitialAmount");

            if (lbltotalAmount.Text == "")
            {
                lbltotalAmount.Text = "0";
            }
            if (lblremainingAmount.Text == "")
            {
                lblremainingAmount.Text = "0";
            }
            if (lblextendAmount.Text == "")
            {
                lblextendAmount.Text = "0";
            }
            if (lblextendtax.Text == "")
            {
                lblextendtax.Text = "0";
            }
            decimal extendAmount = Convert.ToDecimal(lblextendAmount.Text.Trim());
            decimal TotalInitial = Convert.ToDecimal(lbltotalAmount.Text.Trim());
            decimal remaining = Convert.ToDecimal(lblremainingAmount.Text.Trim());
            lblRemAmount.Text = remaining.ToString("0.00");
            DateTime date = Convert.ToDateTime(lblinTime.Text);
            string intimes = date.ToString("yyyy'-'MM'-'dd'T'hh':'mm':'ss'.'fff'Z'");
            //Direct Check Out
            if (extendAmount == 0 && remaining == 0)
            {
                UpdateCheckInOutDetails(lblvehicleNumber.Text, lblvehicleHeaderId.Text, intimes, times, "O", slotid.Trim());
            }
            // Extented Amount
            else if (extendAmount != 0)
            {
                ViewState["Flag"] = "0";
                BindDdlPayment();
                FilterList(lblbookingPassId.Text.Trim());
                ViewState["vehicleHeaderId"] = lblvehicleHeaderId.Text.Trim();
                string extendDayHours = lblextendDayHour.Text.Trim();
                if (extendDayHours.EndsWith("r"))
                {
                    dayorHour.InnerText = "(in hours)";
                }
                else
                {
                    dayorHour.InnerText = "(in days)";
                }
                lbltaxAmount.Text = lblextendtax.Text.Trim();
                lblTimeExtended.Text = dayhourlbl[0].Trim();
                lblExtendedAmount.Text = extendAmount.ToString("0.00").Trim();
                lblVehicleNo.Text = lblvehicleNumber.Text.Trim();
                lblInitialAmount.Text = TotalInitial.ToString("0.00");
                decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(lblRemAmount.Text.Trim());
                decimal TotAmount = Convert.ToDecimal(TotalInitial) + Convert.ToDecimal(extendAmount);
                lblTopayAmount.Text = Amount.ToString("0.00");
                lblTotalAmount.Text = TotAmount.ToString("0.00");
                modal.Visible = true;
                divRemaining.Visible = false;
                divextend.Visible = true;
                remainingre.Visible = false;
                extended.Visible = true;
            }
            else if (remaining != 0)
            {
                remainingre.Visible = true;
                extended.Visible = false;
                ViewState["Flag"] = "1";
                BindReDdlPayment();
                FilterList(lblbookingPassId.Text.Trim());
                ViewState["vehicleHeaderId"] = lblvehicleHeaderId.Text.Trim();
                lblReRemainingAmount.Text = remaining.ToString("0.00");
                lblReVehicleNo.Text = lblvehicleNumber.Text.Trim();
                lblReInitialAmount.Text = TotalInitial.ToString("0.00");
                decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(lblRemAmount.Text.Trim());
                decimal TotAmount = Convert.ToDecimal(TotalInitial) + Convert.ToDecimal(extendAmount);
                lblReToPay.Text = Amount.ToString("0.00");
                lblReTotalAmount.Text = TotAmount.ToString("0.00");
                modal.Visible = true;
                divRemaining.Visible = true;
                divextend.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion
    #endregion

    #region  Extended and Remaining Amount Popup btn
    protected void btnCheckInPopup_Click(object sender, EventArgs e)
    {
        try
        {
            string times;
            string[] date2;
            DateTime utcTime = DateTime.Now;
            times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            date2 = times.Split('T');
            string date = utcTime.ToString("yyyy-MM-dd");
            if (ViewState["Flag"].ToString() == "0")
            {
                UpdateExtendedTimeAmount(lblVehicleNo.Text, ViewState["bookingDurationType"].ToString(), date2[1], date, lblTopayAmount.Text,
                lblTopayAmount.Text,
                lblBookingId.Text, ViewState["vehicleHeaderId"].ToString(), lbltaxAmount.Text, ViewState["slotId"].ToString(), ddlPayment.SelectedValue);
            }
            else
            {
                UpdateCheckInOutDetailsrem(lblReVehicleNo.Text, ViewState["vehicleHeaderId"].ToString(), null, times,
                    "O", ViewState["slotId"].ToString(), lblReToPay.Text, ddlRepayment.SelectedValue);
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        clear();
    }
    #endregion

    #region Update In time and Out Time Details
    #region UpdateCheckInOutDetails
    public void UpdateCheckInOutDetails(string BookingID, string svehicleHeaderId,
        string Intime, string OutTime, string vehicleStatus, string slotId)
    {
        try
        {
            string SlotIds = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                VehicleHeader Insert = new VehicleHeader()
                {
                    vehicleHeaderId = svehicleHeaderId,//Value From Login
                    updatedBy = Session["UserId"].ToString(),
                    vehicleStatus = vehicleStatus,


                };
                SlotIds = slotId.Trim();
                if (vehicleStatus.Trim() == "I")
                {
                    Insert.inTime = Intime;
                }
                if (SlotIds.Trim() == "0" )
                {
                    slotId ="";
                }
                if (slotId.Trim() != "")
                {
                    Insert.slotId = slotId.Trim();
                }
                if (vehicleStatus.Trim() == "O")
                {
                    Insert.outTime = OutTime;
                }
                HttpResponseMessage response = client.PutAsJsonAsync("vehicleHeader", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        if (vehicleStatus == "O")
                        {

                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";

                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblout.Text = "Vehicle No. : " + BookingID.Trim() + " Checked Out Successfully";
                                lblout.Visible = true;
                                lblout.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                                BindCheckOutList();
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblGridIn.Visible = true;
                                lblGridIn.Text = "Vehicle No. : " + BookingID.Trim() + " Checked Out Successfully";
                                lblGridIn.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                BindCheckInList();
                            }


                        }
                        else
                        {
                            lblout.Visible = false;
                            lblout.Text = "";
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblout.Visible = true;
                                lblout.Text = "Vehicle No. : " + BookingID.Trim() + " Checked In Successfully";
                                lblout.Style.Add("color", "#106d25");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                                BindCheckOutList();
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblGridIn.Visible = true;
                                lblGridIn.Text = "Vehicle No. : " + BookingID.Trim() + " Checked In Successfully";
                                lblGridIn.Style.Add("color", "#106d25");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                BindCheckInList();
                            }
                        }

                        ResetInput();
                    }
                    else
                    {
                        ResetInput();
                        lblout.Visible = false;
                        lblGridIn.Visible = false;
                        lblGridIn.Text = "";
                        lblout.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblAlreadyOut.Visible = true;
                            lblAlreadyOut.Text = ResponseMsg;
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                            BindCheckOutList();
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblAlreadyIn.Visible = true;
                            lblAlreadyIn.Text = ResponseMsg;
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                            BindCheckInList();
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    public void UpdateCheckInOutDetailsrem(string BookingID, string svehicleHeaderId, string Intime,
        string OutTime, string vehicleStatus, string slotId, string paidAmount, string Paymenttype)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                VehicleHeader Insert = new VehicleHeader()
                {
                    vehicleHeaderId = svehicleHeaderId,//Value From Login
                    updatedBy = Session["UserId"].ToString(),
                    vehicleStatus = vehicleStatus,
                    paidAmount = paidAmount.Trim(),
                    paymentType = Paymenttype.Trim()

                };
                if (vehicleStatus == "O")
                {
                    Insert.outTime = OutTime;
                }
                if (slotId != "")
                {
                    Insert.slotId = slotId.Trim();
                }
                HttpResponseMessage response = client.PutAsJsonAsync("vehicleHeader", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        if (StatusCode == 1)
                        {
                            clear();
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblout.Text = "Vehicle No. : " + BookingID.Trim() + " Checked Out Successfully";
                                lblout.Visible = true;
                                lblout.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                                BindCheckOutList();
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblGridIn.Visible = true;
                                lblGridIn.Text = "Vehicle No. : " + BookingID.Trim() + " Checked Out Successfully";
                                lblGridIn.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                BindCheckInList();
                            }
                        }
                        else
                        {
                            txtBookingId.Focus();
                            clear();
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                BindCheckOutList();
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {

                                BindCheckInList();
                            }
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }

                        ResetInput();
                    }
                    else
                    {
                        txtBookingId.Focus();
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
    #endregion
    #region UpdateExtendedTimeAmount
    public void UpdateExtendedTimeAmount(string VehicleNo, string bookingDurationType, string totime, string todate, string paidamount, string totalAmount, string BookingId,
        string vehicleHeaderId, string taxAmount, string slotId, string paymentType)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                bookingMasterDateTimeExtend Insert = new bookingMasterDateTimeExtend()
                {
                    bookingDurationType = bookingDurationType,
                    toTime = totime,
                    toDate = todate,
                    paidAmount = paidamount,
                    totalAmount = totalAmount,
                    taxAmount = taxAmount,
                    bookingId = BookingId,
                    vehicleHeaderId = vehicleHeaderId,
                    updatedBy = Session["UserId"].ToString().Trim(),
                    vehicleStatus = "O",
                    paymentType = paymentType.Trim()

                };
                if (slotId != "")
                {
                    Insert.slotId = slotId.Trim();
                }
                HttpResponseMessage response = client.PutAsJsonAsync("bookingMasterDateTimeExtend", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        clear();
                        lblGridIn.Visible = false;
                        lblAlreadyOut.Visible = false;
                        lblAlreadyIn.Visible = false;
                        lblGridIn.Text = "";
                        lblAlreadyOut.Text = "";
                        lblAlreadyIn.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblout.Text = "Vehicle No. : " + VehicleNo.Trim() + " Checked Out Successfully";
                            lblout.Visible = true;
                            lblout.Style.Add("color", "#c3263b");
                            ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                            BindCheckOutList();
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblGridIn.Visible = true;
                            lblGridIn.Text = "Vehicle No. : " + VehicleNo.Trim() + " Checked Out Successfully";
                            lblGridIn.Style.Add("color", "#c3263b");
                            ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                            BindCheckInList();
                        }
                    }
                    else
                    {

                        txtBookingId.Focus();
                        clear();
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            BindCheckOutList();
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {

                            BindCheckInList();
                        }
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
    #endregion

    #endregion

    #region Clear
    public void ResetInput()
    {
        txtBookingId.Text = "";
        txtBookingId.Focus();
    }
    public void ClearLabel()
    {
        lblAlreadyOut.Visible = false;
        lblAlreadyIn.Visible = false;
        lblout.Visible = false;
        lblGridIn.Visible = false;
        lblGridIn.Text = "";
        lblout.Text = "";
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Text = "";
    }
    public void clear()
    {
        modal.Visible = false;
        txtBookingId.Text = "";
        lblBookingId.Text = "";
        lblPinNo.Text = "";
        lblRemAmount.Text = "";
        lblBlockName.Text = "";
        lblFloorName.Text = "";
        lblTimeExtended.Text = "";
        lblExtendedAmount.Text = "";
        lblVehicleNo.Text = "";
        lblReBookingId.Text = "";
        lblReFloorName.Text = "";
        lblReInitialAmount.Text = "";
        lblReRemainingAmount.Text = "";
        lblRePinNo.Text = "";
        lblReToPay.Text = "";
        lblReVehicleNo.Text = "";
        lblReTotalAmount.Text = "";
        lblReBlockName.Text = "";
        txtBookingId.Text = "";
        txtBookingId.Focus();

        lblAlreadyOut.Visible = false;
        lblAlreadyIn.Visible = false;
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Text = "";
        ViewState["bookingDurationType"] = "";
        ViewState["vehicleHeaderId"] = "";
        ViewState["SlotId"] = "";
        ViewState["vehicleHeaderId"] = "";

    }
    #endregion
    #region PassBooking
    public class PassBooking
    {

        public string taxAmount { get; set; }
        public string amount { get; set; }
        public string totalAmount { get; set; }
        public string paymentStatus { get; set; }
        public string paymentType { get; set; }
        public string passTransactionId { get; set; }
        public string branchName { get; set; }
        public string extraFeesAmount { get; set; }
        public string extraFeaturesAmount { get; set; }
        public string bookingAmount { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string createdBy { get; set; }
        public List<PassvehicleHeaderDetails> vehicleDetails { get; set; }
        public List<PassuserSlotDetails> userSlotDetails { get; set; }
        public List<PassextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<PassextraFeesDetails> extraFeesDetails { get; set; }

    }
    public class PassvehicleHeaderDetails
    {
        public string vehicleHeaderId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
    }
    public class PassuserSlotDetails
    {
        public string slotId { get; set; }
        public string vehicleType { get; set; }
    }
    public class PassextraFeaturesDetails
    {
        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraDetail { get; set; }
        public string totalAmount { get; set; }
    }
    public class PassextraFeesDetails
    {
        public string extraFeesDetails { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }
    }
    #endregion

    #region Header and Inout Classess
    #region VehicleHeaderClass
    public class VehicleHeader
    {
        public string vehicleHeaderId { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string updatedBy { get; set; }
        public string vehicleStatus { get; set; }
        public string paidAmount { get; set; }
        public string slotId { get; set; }
        public string paymentType { get; set; }

    }

    #endregion
    #region bookingMasterDateTimeExtend
    public class bookingMasterDateTimeExtend
    {
        public string bookingDurationType { get; set; }
        public string toTime { get; set; }
        public string toDate { get; set; }
        public string paidAmount { get; set; }
        public string vehicleStatus { get; set; }
        public string totalAmount { get; set; }
        public string bookingId { get; set; }
        public string vehicleHeaderId { get; set; }
        public string updatedBy { get; set; }
        public string outTime { get; set; }
        public string taxAmount { get; set; }
        public string slotId { get; set; }
        public string paymentType { get; set; }

    }

    #endregion
    #region  OutDetails
    public class OutDetails
    {
        public string vehicleHeaderId { get; set; }
        public string bookingDurationType { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string vehicleTypeName { get; set; }
        public string vehicleImageUrl { get; set; }
        public string extendAmount { get; set; }
        public string extendTax { get; set; }
        public string extendDayHour { get; set; }
        public string remainingAmount { get; set; }
        public string boookingAmount { get; set; }
        public string initialAmount { get; set; }
        public string pinNo { get; set; }
        public List<slotDetails> slotDetails { get; set; }

    }

    public class slotDetails
    {
        public string userSlotId { get; set; }
        public string slotId { get; set; }
    }

    #endregion

    #endregion

    /// <summary>
    /// menu Access Rights 
    /// Created By Abhinaya K
    /// Created Date 02-AUG-2022
    /// </summary>
    #region menu Option Access Class
    public class menuOptionAccess
    {
        public String parkingOwnerId { get; set; }
        public String userId { get; set; }
        public String userName { get; set; }
        public String userRole { get; set; }
        public String moduleId { get; set; }
        public List<optionDetails> optionDetails { get; set; }
        public String createdBy { get; set; }
    }
    public class optionDetails
    {
        public string optionId { get; set; }
        public string optionName { get; set; }
        public string menuOptionActiveStatus { get; set; }
        public string MenuOptionAccessId { get; set; }
        public string MenuOptionAccessActiveStatus { get; set; }
        public string activeStatus { get; set; }
        public string AddRights { get; set; }
        public string EditRights { get; set; }
        public string ViewRights { get; set; }
        public string DeleteRights { get; set; }
    }

    #endregion
    #region Get Method
    public void BindMenuAccess()
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
                       + "menuOptionAccess?userId=" + Session["UserId"].ToString() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<menuOptionAccess> lst = JsonConvert.DeserializeObject<List<menuOptionAccess>>(ResponseMsg);
                        var firstItem = lst.ElementAt(0);
                        var lst1 = firstItem.optionDetails.ToList();
                        DataTable optionDetails = ConvertToDataTable(lst1);
                        var Option = lst1.Where(x => x.optionName == "smartCheckInCheckOut" && x.MenuOptionAccessActiveStatus == "A")
                            .Select(x => new
                            {
                                optionName = x.optionName,
                                AddRights = x.AddRights,
                                EditRights = x.EditRights,
                                ViewRights = x.ViewRights,
                                DeleteRights = x.DeleteRights
                            }).ToList();
                        if (Option.Count > 0)
                        {
                            form.Visible = true;
                            var Add = Option.Select(y => y.AddRights).ToList();
                            var Edit = Option.Select(y => y.EditRights).ToList();
                            var Delete = Option.Select(y => y.DeleteRights).ToList();
                            var View = Option.Select(y => y.ViewRights).ToList();
                        }
                        else
                        {
                            form.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
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