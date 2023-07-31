using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Booking_AddOnServiceBooking : System.Web.UI.Page
{
    #region Page Loadveh
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
            ViewState["BlockCount"] = "0";
            ViewState["FloorCount"] = "0";
            BindBlock();
            GetPaymentType();
            divSummary.Visible = true;

            if (ViewState["BlockCount"].ToString() == "0" || ViewState["FloorCount"].ToString() == "0")
            {
                divBlockFloor.Visible = true;
            }
            else
            {
                divBlockFloor.Visible = false;
            }
            if (string.IsNullOrEmpty(Session["MobNo"] as string))
            {
                ChkMobileNo.Checked = true;
            }
            else
            {
                ChkMobileNo.Checked = false;
            }
            if (ChkMobileNo.Checked == true)
            {
                Session["MobNo"] = null;
                divMobileNo.Visible = true;
                RfvMobNo.Enabled = true;
            }
            else
            {
                Session["MobNo"] = 1;
                btnBook.Enabled = true;
                divMobileNo.Visible = false;
                RfvMobNo.Enabled = false;
                divOtpDetails.Visible = false;
            }
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


        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }

        if (Session["UserRole"].ToString() == "SA")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Booking Not Allowed For Sadmin');", true);
            return;
        }

    }
    #endregion

    #region Init 
    #region Get Payment Type
    public void GetPaymentType()
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
                          + "configMaster?activestatus=A&configTypename=Payment";
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
                            ddlPaymentType.DataSource = dt;
                            ddlPaymentType.DataValueField = "configId";
                            ddlPaymentType.DataTextField = "configName";
                            ddlPaymentType.DataBind();
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("payLater"));
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Pay At CheckOut"));
                        }
                        else
                        {
                            ddlPaymentType.DataBind();
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlPaymentType.ClearSelection();
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
    #region Bind Block
    public void BindBlock()
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
                                //BindFloorName();
                                ViewState["BlockCount"] = "1";

                            }
                            else
                            {
                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                                //BindFloorName();
                            }
                            BindFloorName();
                        }
                        else
                        {
                            ddlblock.DataBind();
                        }
                        ddlblock.Items.Insert(0, new ListItem("Select", "0"));
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
        BindFloorName();
    }
    #endregion
    #region  Bind Floor
    public void BindFloorName()
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
                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                                ViewState["FloorCount"] = "1";
                            }
                            else
                            {
                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                            }
                            BindVehicle();

                        }
                        else
                        {
                            ddlfloor.Items.Clear();
                        }
                        ddlfloor.Items.Insert(0, new ListItem("Select", "0"));
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
        Clear();
        divForm.Visible = true;
        BindVehicle();
    }
    #endregion
    #region Bind Vehicle
    public void BindVehicle()
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
                      + "floorVehicleMaster?activeStatus=A&floorId=";
                sUrl += ddlfloor.SelectedValue;

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
                            gvVehicleType.DataSource = dt;
                            gvVehicleType.DataBind();
                            ViewState["data"] = dt;
                            ViewState["AccessoriesTotalAmount"] = "0";
                            ViewState["AccessoriesAmount"] = "0";
                            ViewState["AccessoriesTax"] = "0";
                            //LinkButton divvehicle = (LinkButton)gvVehicleType.Items[0].FindControl("divvehicle");
                            //divvehicle.Style.Add("color", "#fff");
                            HtmlControl divrower = gvVehicleType.Items[0].FindControl("divvehicles") as HtmlControl;
                            divrower.Attributes["class"] = "carimage2";
                            HtmlControl divcarname = gvVehicleType.Items[0].FindControl("divcarname") as HtmlControl;
                            divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";
                            lblVehicleTypeId.Text = dt.Rows[0]["vehicleType"].ToString();
                            ViewState["vehicleType"] = lblVehicleTypeId.Text;
                            ViewState["VehicleTypeName"] = dt.Rows[0]["vehicleName"].ToString();
                            DivSummaryFull.Visible = true;
                            divSummary.Visible = true;
                            divForm.Visible = true;
                            Formbackground.Style.Add("background-image", "../images/Backgroundcar.png");
                            Formbackground.Style.Add("background-repeat", "no-repeat");
                            Formbackground.Style.Add("opacity", "0.2");
                            Formbackground.Style.Add("height", "300px");
                            Formbackground.Style.Add("background-position", "center");
                            Formbackground.Style.Add("background-size", "40%");
                            Formbackground.Style.Add("margin-top", "-5rem");
                            BindAccessories();
                            TotalAmountCal();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }

                    }
                    else
                    {
                        gvVehicleType.DataBind();
                        divForm.Visible = false;
                        DivSummaryFull.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region Vehicle Type Click
    protected void lblVehicleTypes_Click(object sender, EventArgs e)
    {
        
        ViewState["Row"] = null;
        ViewState["CartRow"] = null;
        ViewState["AccessoriesTotalAmount"] = "0";
        ViewState["AccessoriesAmount"] = "0";
        ViewState["AccessoriesTax"] = "0";
        dtlExtraFeeSummary.DataSource = null;
        dtlExtraFeeSummary.DataBind();
        extrafeeandfeatursContainer.Visible = false;
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;
            Label lblvehicleConfigId = (Label)gvrow.FindControl("lblvehicleConfigId");
            Label lblvehicleName = (Label)gvrow.FindControl("lblvehicleName");
            ViewState["VehicleTypeName"] = lblvehicleName.Text;
            ViewState["vehicleType"] = lblvehicleConfigId.Text;
            lblVehicleTypeId.Text = lblvehicleConfigId.Text;
            Formbackground.Visible = true;
            if (lblvehicleName.Text == "Car")
            {
                Formbackground.Style.Add("background-image", "../images/Backgroundcar.png");
            }
            else if (lblvehicleName.Text == "Bike")
            {
                Formbackground.Style.Add("background-image", "../images/Backgroundbike.png");
            }
            Formbackground.Style.Add("background-repeat", "no-repeat");
            Formbackground.Style.Add("opacity", "0.2");
            Formbackground.Style.Add("height", "300px");
            Formbackground.Style.Add("background-position", "center");
            Formbackground.Style.Add("background-size", "40%");
            Formbackground.Style.Add("margin-top", "-5rem");
            divSummary.Visible = true;
            if (ChkMobileNo.Checked == true)
            {
                btnBook.Enabled = false;
            }
            else
            {
                btnBook.Enabled = true;
            }
            BindAccessories();
            TotalAmountCal();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }


    }
    #endregion
    #region Get PIN
    public void GetPIN()
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
                        + "generatePin";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        hfPinNo.Value = ResponseMsg;
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
    #region VehicleType Color Change
    protected void gvVehicleType_ItemCommand(object source, DataListCommandEventArgs e)
    {
        for (int j = 0; j < gvVehicleType.Items.Count; j++)
        {
            int i = e.Item.ItemIndex;
            if (i == j)
            {
                Label divvehicle = (Label)gvVehicleType.Items[j].FindControl("lblvehicleName");
                //divvehicle.Style.Add("color", "#fff");
                divvehicle.Style.Add("color", "black");
                HtmlControl divrower = gvVehicleType.Items[j].FindControl("divvehicles") as HtmlControl;
                divrower.Attributes["class"] = "carimage2";
                HtmlControl divcarname = gvVehicleType.Items[j].FindControl("divcarname") as HtmlControl;
                divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";
            }
            else
            {
                Label divvehicle = (Label)gvVehicleType.Items[j].FindControl("lblvehicleName");
                divvehicle.Style.Add("color", "#000");
                HtmlControl divrower = gvVehicleType.Items[j].FindControl("divvehicles") as HtmlControl;
                divrower.Attributes["class"] = "carimage";
                HtmlControl divcarname = gvVehicleType.Items[j].FindControl("divcarname") as HtmlControl;
                divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname";
            }
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
    #endregion

    #region Booking
    #region Booking Click
    protected void btnBook_Click(object sender, EventArgs e)
    {
        if (Session["UserRole"].ToString() == "SA")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Booking Not Allowed For Sadmin');", true);
            return;
        }
        DivSummaryFull.Visible = true;
        divSummary.Visible = true;
        if (ChkMobileNo.Checked == true)
        {
            if (txtmobileNo.Text != "")
            {
                btnBook.Enabled = true;
            }
        }
        if (dtlExtraFeeSummary.Rows.Count > 0)
        {
            GetPIN();
            TotalAmountCal();
            BookingFunction();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('Select Any One Add-On Service');", true);
        }

    }
    #endregion

    #region Total Amount Cal    
    public void TotalAmountCal()
    {
        decimal AccTotal = 0;
        decimal finaltax = Convert.ToDecimal(ViewState["AccessoriesTax"].ToString());
        lblGSTAmount.Text = finaltax.ToString("0.00");
        lblTotalAmount.Text = Convert.ToDecimal(ViewState["AccessoriesTotalAmount"].ToString()).ToString("0.00");

        if (dtlExtraFeeSummary.Rows.Count > 0)
        {
            AccTotal = Convert.ToDecimal(ViewState["AccessoriesTotalAmount"]);
        }
        decimal TotalAmount = AccTotal;
        ViewState["TotalAmount"] = TotalAmount;
        lblTotalAmount.Text = Convert.ToDecimal(ViewState["TotalAmount"].ToString()).ToString("0.00");
        if (lblTotalAmount.Text == "0.00")
        {
            divPaymentTypeBook.Visible = false;
        }
        else
        {
            divPaymentTypeBook.Visible = true;
        }
        btnBook.Text = lblTotalAmount.Text;
        if (Convert.ToDecimal(lblGSTAmount.Text) == 0)
        {
            divTax.Visible = false;
        }
        else
        {
            divTax.Visible = true;
        }
    }
    #endregion

    #region Booking Function
    public void BookingFunction()
    {
        try
        {
            string MobNo = txtmobileNo.Text;
            string accessories = string.Empty;
            DateTime utcTime = DateTime.Now;
            string times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            string[] time = times.Split('T');

            if (dtlExtraFeeSummary.Rows.Count > 0)
            {
                accessories = "Y";
            }
            else
            {
                accessories = "N";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new Booking()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    blockId = ddlblock.SelectedValue,
                    floorId = ddlfloor.SelectedValue,
                    userId = Session["UserId"].ToString(),
                    phoneNumber = txtmobileNo.Text,
                    booking = "DW",
                    loginType = Session["UserRole"].ToString(),
                    fromTime = time[1],
                    fromDate = time[0],
                    accessories = accessories,
                    bookingType = "P",
                    paymentStatus = ddlPaymentType.SelectedItem.Text == "Pay At CheckOut" ? "N" : "P",
                    paymentType = ddlPaymentType.SelectedValue,
                    totalAmount = ViewState["TotalAmount"].ToString(),
                    pinNo = hfPinNo.Value,
                    createdBy = Session["UserId"].ToString(),
                };

                if (dtlExtraFeeSummary.Rows.Count > 0)
                {
                    string feePriceids = string.Empty;
                    string feetureCount = string.Empty;
                    string feeName = string.Empty;
                    string feeAmount = string.Empty;
                    foreach (GridViewRow item in dtlExtraFeeSummary.Rows)
                    {
                        Label lblgvfeetimeslabId = item.FindControl("lblgvfeetimeslabId") as Label;
                        Label lblfeeCount = item.FindControl("lblfeeCount") as Label;
                        Label lblfeevehicleAccessoriesName = item.FindControl("lblfeevehicleAccessoriesName") as Label;
                        Label lblgvfeeTotalAmount = item.FindControl("lblgvfeeTotalAmount") as Label;

                        feePriceids += lblgvfeetimeslabId.Text + ',';
                        feetureCount += lblfeeCount.Text + ',';
                        feeAmount += lblgvfeeTotalAmount.Text + ',';
                        feeName += lblfeevehicleAccessoriesName.Text + ',';
                    }
                    Insert.extraFees = GetExtraFeesDetails(feetureCount.ToString().TrimEnd(','),
                        feePriceids.ToString().TrimEnd(','), feeName.ToString().TrimEnd(','),
                        feeAmount.ToString().TrimEnd(','));
                }
                if (Insert.paymentStatus == "P")
                {
                    Insert.paidAmount = Insert.totalAmount;
                }
                HttpResponseMessage response = client.PostAsJsonAsync("bookingMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        string ResponseMsg1 = JObject.Parse(SmartParkingList)["bookingId"].ToString();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        if (MobNo != "")
                        {
                            Session["MobNo"] = null;
                        }
                        else
                        {
                            Session["MobNo"] = "1";
                        }
                        Clear();
                        Response.Redirect("Print.aspx?rt=O&bi=" + ResponseMsg1.ToString() + "");
                        //GetBookingDetails(ResponseMsg1);
                        divSummary.Visible = true;
                    }
                    else
                    {
                        btnBook.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    btnBook.Enabled = true;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + response.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Extra Fees Details
    public static List<extraFees> GetExtraFeesDetails(string count, string timeslabId, string extraFeesDetails, string extraFee)
    {
        string[] counts;
        string[] timeslabIds;
        string[] extraFees;
        string[] extraFeesDetailss;
        counts = count.Split(',');
        extraFees = extraFee.Split(',');
        extraFeesDetailss = extraFeesDetails.Split(',');
        timeslabIds = timeslabId.Split(',');
        List<extraFees> lst = new List<extraFees>();
        for (int i = 0; i < counts.Count(); i++)
        {
            lst.AddRange(new List<extraFees>
            {
                new extraFees { count=counts[i] ,extraFee=extraFees[i],timeslabId= timeslabIds[i],extraFeesDetails = extraFeesDetailss[i] }

            });
        }
        return lst;

    }
    #endregion

    #endregion

    #region Accessories
    #region Bind Accessories
    public void BindAccessories()
    {
        extraFee__container.Visible = true;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim() + "priceMaster?activeStatus=A";
                sUrl += "&floorId=" + ddlfloor.SelectedValue + "&idType=A"
                 + "&vehicleConfigId=" + ViewState["vehicleType"].ToString() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        Formbackground.Visible = true;
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            extraFee__container.Visible = true;
                            DivSummaryFull.Visible = true;
                            divSummary.Visible = true;
                            lblGvAccessoriesNo.Visible = false;
                            dtlextraFee.Visible = true;
                            dtlextraFee.DataSource = dt;
                            dtlextraFee.DataBind();
                        }
                        else
                        {
                            extraFee__container.Visible = false;
                            DivSummaryFull.Visible = true;
                            lblGvAccessoriesNo.Text = ResponseMsg.Trim();
                            lblGvAccessoriesNo.Visible = true;
                            dtlextraFee.Visible = false;
                            dtlextraFee.DataSource = null;
                            dtlextraFee.DataBind();
                            divSummary.Visible = true;
                        }
                    }
                    else
                    {
                        Formbackground.Visible = false;
                        extraFee__container.Visible = false;
                        DivSummaryFull.Visible = false;
                        divSummary.Visible = true;
                        lblGvAccessoriesNo.Text = ResponseMsg.Trim();
                        lblGvAccessoriesNo.Visible = true;
                        dtlextraFee.Visible = false;
                        dtlextraFee.DataSource = null;
                        dtlextraFee.DataBind();
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.Trim() + "');", true);
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion   

    #region Accessories Datalist Plus Click    
    protected void dtlextraFee_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            LinkButton lblgvFeeName = (LinkButton)e.Item.FindControl("lblgvFeeName");
            Label lblgvFeetax = (Label)e.Item.FindControl("lblgvFeetax");
            Label lblgvFeeAmount = (Label)e.Item.FindControl("lblgvFeeAmount");
            LinkButton lblgvFeeTotalAmount = (LinkButton)e.Item.FindControl("lblgvFeeTotalAmount");
            Label lblGvtimeslabId = (Label)e.Item.FindControl("lblGvtimeslabId");
            int Count = 1;
            DataTable dt = new DataTable();
            int Uniqueid = 1;
            if (ViewState["Row"] != null)
            {
                dt = (DataTable)ViewState["Row"];
                DataRow dr = null;

                string Values = lblGvtimeslabId.Text;

                DataRow[] fndUniqueId = dt.Select("timeslabId = '" + Values.Trim() + "'");

                if (dt.Rows.Count > 0)
                {
                    Uniqueid = dt.Rows.Count + 1;
                    dr = dt.NewRow();
                    dr["UniqueID"] = Uniqueid;
                    dr["timeslabId"] = lblGvtimeslabId.Text;
                    dr["vehicleAccessoriesName"] = lblgvFeeName.Text;
                    dr["amount"] = lblgvFeeAmount.Text;
                    dr["tax"] = lblgvFeetax.Text;
                    dr["totalAmount"] = lblgvFeeTotalAmount.Text;
                    dr["Count"] = Count;
                    dt.Rows.Add(dr);
                    ViewState["Row"] = dt;
                }
            }
            else
            {
                dt.Columns.Add(new DataColumn("UniqueID", typeof(string)));
                dt.Columns.Add(new DataColumn("timeslabId", typeof(string)));
                dt.Columns.Add(new DataColumn("vehicleAccessoriesName", typeof(string)));
                dt.Columns.Add(new DataColumn("amount", typeof(decimal)));
                dt.Columns.Add(new DataColumn("tax", typeof(decimal)));
                dt.Columns.Add(new DataColumn("totalAmount", typeof(decimal)));
                dt.Columns.Add(new DataColumn("Count", typeof(Int32)));

                DataRow dr1 = dt.NewRow();
                dr1 = dt.NewRow();
                dr1["UniqueID"] = Uniqueid;
                dr1["timeslabId"] = lblGvtimeslabId.Text;
                dr1["vehicleAccessoriesName"] = lblgvFeeName.Text;
                dr1["amount"] = lblgvFeeAmount.Text;
                dr1["tax"] = lblgvFeetax.Text;
                dr1["totalAmount"] = lblgvFeeTotalAmount.Text;
                dr1["Count"] = Count;

                dt.Rows.Add(dr1);

                ViewState["Row"] = dt;
            }

            DataTable dts = dt.Clone();

            var CartTable = (from row in dt.AsEnumerable()
                             group row by new
                             {
                                 timeslabId = row.Field<string>("timeslabId"),
                                 vehicleAccessoriesName = row.Field<string>("vehicleAccessoriesName"),
                             } into t1
                             select new
                             {
                                 timeslabId = t1.Key.timeslabId,
                                 vehicleAccessoriesName = t1.Key.vehicleAccessoriesName,
                                 amount = t1.Sum(a => a.Field<decimal>("amount")),
                                 tax = t1.Sum(a => a.Field<decimal>("tax")),
                                 totalAmount = t1.Sum(a => a.Field<decimal>("totalAmount")),
                                 Count = t1.Sum(a => a.Field<Int32>("Count")),

                             })
                 .Select(g =>
                 {
                     var h = dts.NewRow();
                     h["timeslabId"] = g.timeslabId;
                     h["vehicleAccessoriesName"] = g.vehicleAccessoriesName;
                     h["amount"] = g.amount;
                     h["tax"] = g.tax;
                     h["totalAmount"] = g.totalAmount;
                     h["Count"] = g.Count;

                     return h;
                 }).CopyToDataTable();

            if (CartTable.Rows.Count > 0)
            {
                ViewState["CartRow"] = CartTable;
                DataTable dtss = (DataTable)ViewState["CartRow"];

                if (dtss.Rows.Count > 0)
                {
                    dtlExtraFeeSummary.DataSource = dtss;
                    dtlExtraFeeSummary.DataBind();
                    divExtrafeeSummary1.Visible = true;
                    divExtraFeeAmount.Visible = true;
                    extrafeeandfeatursContainer.Visible = true;
                    decimal dServiceTotalAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("totalAmount")));
                    decimal dServiceAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("amount")));
                    decimal dTax = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("tax")));
                    ViewState["AccessoriesTotalAmount"] = Convert.ToDecimal(dServiceTotalAmount).ToString("0.00");
                    ViewState["AccessoriesAmount"] = Convert.ToDecimal(dServiceAmount).ToString("0.00");
                    ViewState["AccessoriesTax"] = Convert.ToDecimal(dTax).ToString("0.00");
                    TotalAmountCal();
                    ViewState["TotalTaxFee"] = (Convert.ToDecimal(dTax)).ToString();
                    lblAccessoriesTotalAmounts.Text = ViewState["AccessoriesAmount"].ToString();
                }
            }
            else
            {
                TotalAmountCal();
                ViewState["CartRow"] = null;
                ViewState["Row"] = null;
                ViewState["AccessoriesTotalAmount"] = "0";
                ViewState["AccessoriesAmount"] = "0";
                ViewState["AccessoriesTax"] = "0";
                lblAccessoriesTotalAmounts.Text = "0.00";
                dtlExtraFeeSummary.DataSource = null;
                dtlExtraFeeSummary.DataBind();
                divExtrafeeSummary1.Visible = false;
                divExtraFeeAmount.Visible = false;
                extrafeeandfeatursContainer.Visible = false;
            }
            header.Visible = false;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblfeevehicleAccessoriesName = (Label)gvrow.FindControl("lblfeevehicleAccessoriesName");
            Label lblgvfeeTotalAmount = (Label)gvrow.FindControl("lblgvfeeTotalAmount");
            Label lblfeeCount = (Label)gvrow.FindControl("lblfeeCount");
            Label lblgvfeetimeslabId = (Label)gvrow.FindControl("lblgvfeetimeslabId");
            DataTable dt = (DataTable)ViewState["Row"];
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dt.Rows[i];
                if (drO["timeslabId"].ToString().Trim() == lblgvfeetimeslabId.Text)
                {
                    drO.Delete();
                }
            }
            dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                DataTable dts = dt.Clone();
                var CartTable = (from row in dt.AsEnumerable()
                                 group row by new
                                 {
                                     timeslabId = row.Field<string>("timeslabId"),
                                     vehicleAccessoriesName = row.Field<string>("vehicleAccessoriesName"),

                                 } into t1
                                 select new
                                 {
                                     timeslabId = t1.Key.timeslabId,
                                     vehicleAccessoriesName = t1.Key.vehicleAccessoriesName,
                                     amount = t1.Sum(a => a.Field<decimal>("amount")),
                                     tax = t1.Sum(a => a.Field<decimal>("tax")),
                                     totalAmount = t1.Sum(a => a.Field<decimal>("totalAmount")),
                                     Count = t1.Sum(a => a.Field<Int32>("Count")),
                                 })
                     .Select(g =>
                     {
                         var h = dts.NewRow();
                         h["timeslabId"] = g.timeslabId;
                         h["vehicleAccessoriesName"] = g.vehicleAccessoriesName;
                         h["amount"] = g.amount;
                         h["tax"] = g.tax;
                         h["totalAmount"] = g.totalAmount;
                         h["Count"] = g.Count;
                         return h;
                     }).CopyToDataTable();

                ViewState["CartRow"] = CartTable;
                DataTable dtss = (DataTable)ViewState["CartRow"];

                if (dtss.Rows.Count > 0)
                {
                    dtlExtraFeeSummary.DataSource = dtss;
                    dtlExtraFeeSummary.DataBind();
                    divExtrafeeSummary1.Visible = true;
                    divExtraFeeAmount.Visible = true;
                    extrafeeandfeatursContainer.Visible = true;
                    decimal dServiceTotalAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("totalAmount")));
                    decimal dServiceAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("amount")));
                    decimal dTax = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("tax")));
                    ViewState["AccessoriesTotalAmount"] = Convert.ToDecimal(dServiceTotalAmount).ToString("0.00");
                    ViewState["AccessoriesAmount"] = Convert.ToDecimal(dServiceAmount).ToString("0.00");
                    ViewState["AccessoriesTax"] = Convert.ToDecimal(dTax).ToString("0.00");
                    TotalAmountCal();
                    ViewState["TotalTaxFee"] = (Convert.ToDecimal(dTax)).ToString();
                    lblAccessoriesTotalAmounts.Text = ViewState["AccessoriesAmount"].ToString();
                }
            }
            else
            {
                lblAccessoriesTotalAmounts.Text = "0.00";
                dtlExtraFeeSummary.DataSource = null;
                dtlExtraFeeSummary.DataBind();
                divExtrafeeSummary1.Visible = false;
                divExtraFeeAmount.Visible = false;
                extrafeeandfeatursContainer.Visible = false;
                ViewState["AccessoriesTotalAmount"] = "0";
                ViewState["AccessoriesAmount"] = "0";
                ViewState["AccessoriesTax"] = "0";
                TotalAmountCal();
                ViewState["CartRow"] = null;
                ViewState["Row"] = null;
            }


            header.Visible = false;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #endregion

    #region OTP Details
    protected void btnCfmOtp_Click(object sender, EventArgs e)
    {

        try
        {
            if (txtOTP.Text == ViewState["OTP"].ToString().Trim())
            {
                txtOTP.Text = string.Empty;
                btnBook.Enabled = true;
                divOtpDetails.Visible = false;
                divResend.Visible = false;
                txtmobileNo.Enabled = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "successalert('OTP Verified Successfully');", true);
            }
            else
            {
                txtOTP.Text = string.Empty;
                btnResend.Text = "Resend";
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "infoalert('Please Check the OTP sent / Time Out / Click Resend');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    protected void btnResend_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["OTP"] = "";
            hfOtp.Value = "";
            btnResend.Text = "00:30";
            btnCfmOtp.Visible = true;
            SendOTP();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        try
        {
            SendOTP();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void SendOTP()
    {
        try
        {
            if (txtmobileNo.Text.Length == 10)
            {
                var reg = new Regex("[0-9]{10}");
                if (!reg.IsMatch(txtmobileNo.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "infoalert('Please Enter Valid 10 digit Mobile No.');", true);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var Insert = new LoginClass()
                        {
                            username = txtmobileNo.Text.Trim()
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("verifyOTP", Insert).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                            string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                            if (StatusCode == 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                string Otp = JObject.Parse(SmartParkingList)["OTP"].ToString();
                                hfOtp.Value = Otp;
                                ViewState["OTP"] = Otp.Trim();
                                divEnterOtp.Visible = true;
                                divResend.Visible = true;
                                divSendOtp.Visible = false;
                                ScriptManager.RegisterStartupScript(this, GetType(), "SendOtp", "SendOtp();", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "successalert('" + Otp + "');", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            }
                        }
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "infoalert('Please Enter Valid 10 digit Mobile No.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "erroralert('" + ex + "');", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.Message.ToString().Trim() + "');", true);
        }



    }
    public class LoginClass
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    #endregion

    #region MobileNo TextChanged
    protected void txtmobileNo_TextChanged(object sender, EventArgs e)
    {
        if (ChkMobileNo.Checked == true)
        {
            divMobileNo.Visible = true;
            divOtpDetails.Visible = true;
            divSendOtp.Visible = true;
            RfvMobNo.Enabled = true;
        }
        else
        {
            divMobileNo.Visible = false;
            RfvMobNo.Enabled = false;
            divOtpDetails.Visible = false;
        }
    }
    #endregion

    #region ChkMobileNo CheckedChanged
    protected void ChkMobileNo_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkMobileNo.Checked == true)
        {
            Session["MobNo"] = null;
            divMobileNo.Visible = true;
            RfvMobNo.Enabled = true;
            btnBook.Enabled = false;
        }
        else
        {
            Session["MobNo"] = 1;
            divMobileNo.Visible = false;
            RfvMobNo.Enabled = false;
            divOtpDetails.Visible = false;
            btnBook.Enabled = true;
        }
        //ChkMobileNo.Enabled = false;
    }
    #endregion

    #region Close & Clear Function
    public void Clear()
    {
        divOtpDetails.Visible = false;
        divEnterOtp.Visible = false;
        divResend.Visible = false;
        lblGvAccessoriesNo.Visible = false;
        ViewState["AccessoriesTotalAmount"] = "0";
        ViewState["AccessoriesAmount"] = "0";
        ViewState["AccessoriesTax"] = "0";
        lblAccessoriesTotalAmounts.Text = "0.00";
        dtlExtraFeeSummary.DataSource = null;
        dtlExtraFeeSummary.DataBind();
        extrafeeandfeatursContainer.Visible = false;
        divExtrafeeSummary1.Visible = false;
        ddlPaymentType.ClearSelection();
        txtmobileNo.Text = "";
        txtmobileNo.Text = "";
        lblTotalAmount.Text = "";
        if (ChkMobileNo.Checked == true)
        {
            btnBook.Enabled = false;
        }
        else
        {
            btnBook.Enabled = true;
        }
        txtmobileNo.Enabled = true;
        ViewState["ExtraRowCartRow"] = null;
        ViewState["ExtraRow"] = null;
        ViewState["Row"] = null;
        ViewState["CartRow"] = null;
        extraFee__container.Visible = true;
        BindAccessories();
        ViewState["paylaterOption"] = "0";
        ChkMobileNo.Enabled = true;
        TotalAmountCal();
    }

    #region cancel click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    #endregion

    #endregion

    #region classes

    #region Booking Class
    public class Booking
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userId { get; set; }
        public string phoneNumber { get; set; }
        public string booking { get; set; }
        public string bookingDurationType { get; set; }
        public string accessories { get; set; }
        public string Dates { get; set; }
        public string bookingType { get; set; }
        public string subscriptionId { get; set; }
        public string taxId { get; set; }
        public string offerId { get; set; }
        public string paidAmount { get; set; }
        public string transactionId { get; set; }
        public string bankName { get; set; }
        public string bankReferenceNumber { get; set; }
        public string pinNo { get; set; }
        public string createdBy { get; set; }
        public string loginType { get; set; }
        public string bookingId { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string taxAmount { get; set; }
        public string totalAmount { get; set; }
        public string paymentStatus { get; set; }
        public string paymentType { get; set; }
        public List<extraFees> extraFees { get; set; }
    }
    public class extraFees
    {
        public string timeslabId { get; set; }
        public string extraFeesDetails { get; set; }
        public string priceId { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }
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
                        var Option = lst1.Where(x => x.optionName == "addOnService" && x.MenuOptionAccessActiveStatus == "A")
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
                            form1.Visible = true;
                            var Add = Option.Select(y => y.AddRights).ToList();
                            var Edit = Option.Select(y => y.EditRights).ToList();
                            var Delete = Option.Select(y => y.DeleteRights).ToList();
                            var View = Option.Select(y => y.ViewRights).ToList();
                        }
                        else
                        {
                            form1.Visible = true;
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
    #endregion     
}