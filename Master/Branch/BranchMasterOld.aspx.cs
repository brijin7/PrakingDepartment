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
using System.IO;
using System.Net;
using System.Text;

public partial class Master_BranchMaster : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        txtLicensePeriodTo.Attributes.Add("ReadOnly", "ReadOnly");
        txtLatitude.Attributes.Add("ReadOnly", "ReadOnly");
        txtLongitude.Attributes.Add("ReadOnly", "ReadOnly");
        spAddorEdit.InnerText = "Add ";
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (!IsPostBack)
        {
            txtminDay.Text = "0";
            txtMaxhour.Text = "0";
            txtMaxDay.Text = "0";
            txtminHour.Text = "0";
            txtNoofDorH.Text = "0";
            txtAdBCharge.Text = "0";

            if (Session["UserRole"].ToString().Trim() == "SA")
            {
                gvBranchmaster.Columns[31].Visible = false;//VIEW
                gvBranchmaster.Columns[30].Visible = true;//Approval
                gvBranchmaster.Columns[29].Visible = true;//Delete
                gvBranchmaster.Columns[28].Visible = true;//Edit              
                gvBranchmaster.Columns[27].Visible = false;//phoneNumber
            }
            else
            {
                gvBranchmaster.Columns[31].Visible = true;//VIEW
                gvBranchmaster.Columns[30].Visible = false;
                gvBranchmaster.Columns[29].Visible = false;
                gvBranchmaster.Columns[28].Visible = false;
                gvBranchmaster.Columns[27].Visible = true;
            }
            //else if (Session["UserRole"].ToString().Trim() == "A")
            BindOwner();
            BindBranchMaster();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion
    #region Bind Dropdown Ownername
    public void BindOwner()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "parkingOwnerMaster?activeStatus=A";

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
                            item.Property("addressDetails").Remove();
                            item.Property("branchDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {

                            ddlownername.DataSource = dt;
                            ddlownername.DataValueField = "parkingOwnerId";
                            ddlownername.DataTextField = "parkingName";
                            ddlownername.DataBind();
                            ddlownername.Items.Insert(0, new ListItem("Select", "0"));
                            if (Session["parkingOwnerId"].ToString() != "0")
                            {
                                var firstitem = ddlownername.Items.FindByValue(Session["parkingOwnerId"].ToString());
                                ddlownername.Items.Clear();
                                ddlownername.Items.Add(firstitem);
                                ddlownername.SelectedValue = firstitem.Value;
                                ddlownername.Enabled = false;
                            }

                        }
                        else
                        {
                            ddlownername.DataBind();
                            ddlownername.Items.Insert(0, new ListItem("Select", "0"));
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region Bind Branch  Master 
    public void BindBranchMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = string.Empty;
                if (Session["parkingOwnerId"].ToString().Trim() != "0")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                         + "branchMaster?parkingOwnerId=";
                    sUrl += Session["parkingOwnerId"].ToString().Trim()
                         + "&activeStatus=A";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                         + "branchMaster";
                }

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    var Response = JObject.Parse(SmartParkingList)["response"].ToArray();
                    if (StatusCode == 1)
                    {
                        List<BranchMasterClass> Branch = JsonConvert.DeserializeObject<List<BranchMasterClass>>(ResponseMsg);

                        DataTable dt = ConvertToDataTable(Branch);
                        if (dt.Rows.Count > 0)
                        {
                            gvBranchmaster.DataSource = Branch;
                            gvBranchmaster.DataBind();
                        }
                        else
                        {
                            spAddorEdit.InnerText = "Add ";
                            ADD();
                            gvBranchmaster.DataSource = null;
                            gvBranchmaster.DataBind();
                        }
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        ADD();
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        ADD();
    }
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
    }
    #endregion
    #region Working Days Click
    public void ChkBoxSelect()
    {
        DivHrDetails.Visible = true;
        StringBuilder sb = new StringBuilder();
        string bindwd = string.Empty;
        string IsHoliday = string.Empty;
        for (int i = 0; i < chkWorkingDays.Items.Count; i++)
        {
            string str = chkWorkingDays.Items[i].ToString();

            if (str == "Sunday")
            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    bindwd += "Sunday" + ",";
                    chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
                    IsHoliday += "false" + ",";
                }
                else
                {
                    bindwd += "Sunday" + ",";
                    IsHoliday += "true" + ",";
                }
            }
            else if (str == "Monday")
            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    bindwd += "Monday" + ",";
                    chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
                    IsHoliday += "false" + ",";

                }
                else
                {
                    bindwd += "Monday" + ",";
                    IsHoliday += "true" + ",";
                }
            }
            else if (str == "Tuesday")
            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    bindwd += "Tuesday" + ",";
                    chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
                    IsHoliday += "false" + ",";
                }
                else
                {
                    bindwd += "Tuesday" + ",";
                    IsHoliday += "true" + ",";
                }
            }
            else if (str == "Wednesday")
            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    bindwd += "Wednesday" + ",";
                    chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
                    IsHoliday += "false" + ",";

                }
                else
                {
                    bindwd += "Wednesday" + ",";
                    IsHoliday += "true" + ",";
                }
            }

            else if (str == "Thursday")

            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    bindwd += "Thursday" + ",";
                    chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
                    IsHoliday += "false" + ",";

                }
                else
                {
                    bindwd += "Thursday" + ",";
                    IsHoliday += "true" + ",";
                }
            }
            else if (str == "Friday")

            {
                if (chkWorkingDays.Items[i].Selected)
                {

                    bindwd += "Friday" + ",";
                    chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
                    IsHoliday += "false" + ",";
                }
                else
                {
                    bindwd += "Friday" + ",";
                    IsHoliday += "true" + ",";
                }
            }
            else if (str == "Saturday")

            {
                if (chkWorkingDays.Items[i].Selected)
                {
                    bindwd += "Saturday" + ",";
                    chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
                    IsHoliday += "false" + ",";
                }
                else
                {
                    bindwd += "Saturday" + ",";
                    IsHoliday += "true" + ",";
                }
            }

        }
        Session["Workingdays"] = bindwd;
        ViewState["IsHoliday"] = IsHoliday;
    }
    protected void chkWorkingDays_SelectedIndexChanged(object sender, EventArgs e)
    {
        DivHrDetails.Visible = true;
        for (int i = 0; i < chkWorkingDays.Items.Count; i++)
        {
            if (chkWorkingDays.Items[i].Selected)
            {
                chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
            }
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "nextPrev(0,event);", true);
    }
    protected void LnkSameTime_Click(object sender, EventArgs e)
    {
        DivHrDetails.Visible = true;
        chkWorkingDays.Items[1].Selected = true;
        chkWorkingDays.Items[2].Selected = true;
        chkWorkingDays.Items[3].Selected = true;
        chkWorkingDays.Items[4].Selected = true;
        chkWorkingDays.Items[5].Selected = true;
        chkWorkingDays.Items[6].Selected = true;
        chkWorkingDays.Items[0].Selected = true;
        for (int i = 0; i < chkWorkingDays.Items.Count; i++)
        {
            if (chkWorkingDays.Items[i].Selected)
            {
                chkWorkingDays.Items[i].Attributes.Add("Class", "DaybtnClick");
            }
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "nextPrev(0,event);", true);

    }
    #endregion

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "showLoader()", true);
        if (btnSubmit.Text == "Submit")
        {
            InsertBranch();
        }
        else
        {
            UpdateBranchMaster();
        }
    }
    #endregion
    #region Insert Function
    public void InsertBranch()
    {
        try
        {
            ChkBoxSelect();
            string multiBook = string.Empty;
            string onlineBooking = string.Empty;
            string PayBook = string.Empty;
            string PayLater = string.Empty;
            string CheckIN = string.Empty;
            string CheckOut = string.Empty;
            string MinHour = string.Empty;
            string MaxHour = string.Empty;
            string MaxDay = string.Empty;
            string MinDay = string.Empty;
            string DayorMin = string.Empty;
            string ABcharge = string.Empty;

            if (chkMultiBook.Checked)
            {
                multiBook = "Y";
            }
            else
            {
                multiBook = "N";
            }
            if (chkOnlineB.Checked)
            {
                onlineBooking = "Y";
            }
            else
            {
                onlineBooking = "N";
            }
            if (chkPayB.Checked)
            {
                PayBook = "Y";
            }
            else
            {
                PayBook = "N";
            }
            if (chkPaylater.Checked)
            {
                PayLater = "Y";
            }
            else
            {
                PayLater = "N";
            }
            if (chkChechIn.Checked)
            {
                CheckIN = "Y";
            }
            else
            {
                CheckIN = "N";
            }
            if (chkBCheckout.Checked)
            {
                CheckOut = "Y";
            }
            else
            {
                CheckOut = "N";
            }

            if (txtminHour.Text == "")
            {
                MinHour = "0";
            }
            else
            {
                MinHour = txtminHour.Text;

            }
            if (txtMaxhour.Text == "")
            {
                MaxHour = "0";
            }
            else
            {
                MaxHour = txtMaxhour.Text;

            }
            if (txtminDay.Text == "")
            {
                MinDay = "0";
            }
            else
            {
                MinDay = txtminDay.Text;

            }
            if (txtMaxDay.Text == "")
            {
                MaxDay = "0";
            }
            else
            {
                MaxDay = txtMaxDay.Text;

            }
            if (txtNoofDorH.Text == "")
            {
                DayorMin = "0";
            }
            else
            {
                DayorMin = txtNoofDorH.Text;

            }
            if (txtAdBCharge.Text == "")
            {
                ABcharge = "0";
            }
            else
            {
                ABcharge = txtAdBCharge.Text;

            }

            DateTime FromDate = Convert.ToDateTime(txtLicensePeriodFrom.Text);
            DateTime Todate = Convert.ToDateTime(txtLicensePeriodTo.Text);
            string LicensefromDate = FromDate.ToString("yyyy-MM-dd");
            string LicenseToDate = Todate.ToString("yyyy-MM-dd");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new BranchMasterClass()
                {
                    parkingOwnerId = ddlownername.SelectedValue,
                    branchName = txtBranchName.Text,
                    shortName = txtBranchShortName.Text,
                    latitude = txtLatitude.Text,
                    longitude = txtLongitude.Text,
                    address1 = txtAddress1.Text,
                    address2 = txtAddress2.Text,
                    district = hfDistrict.Value.Trim(),
                    state = hfState.Value.Trim(),
                    city = hfCity.Value.Trim(),
                    pincode = txtPincode.Text,
                    phoneNumber = txtPhoneNo.Text,
                    alternatePhoneNumber = txtAlterPhoneNo.Text,
                    emailId = txtEmailId.Text,
                    licenseNo = txtLicenseNo.Text,
                    licensePeriodFrom = LicensefromDate.Trim(),
                    licensePeriodTo = LicenseToDate.Trim(),
                    license = hfLicence.Value,
                    document1 = hfdocument1.Value,
                    document2 = hfdocument2.Value,
                    multiBook = multiBook,
                    isPayBookAvailable = PayBook,
                    isPayLaterAvaialble = PayLater,
                    isBookCheckInAvailable = CheckIN,
                    isPayAtCheckoutAvailable = CheckOut,
                    advanceBookingHourOrDayType = ddlHorDType.SelectedValue,
                    advanceBookingHourOrDay = DayorMin,
                    advanceBookingCharges = ABcharge,
                    minHour = MinHour,
                    maxHour = MaxHour,
                    maxDay = MinDay,
                    minDay = MaxDay,
                    activeStatus = "A",
                    approvalStatus = Session["approvalStatus"].ToString().Trim(),
                    onlineBookingAvailability = onlineBooking,
                    createdBy = Session["UserId"].ToString(),
                    branchImageMasterDetails = GetImageUrl(hfImageUrl.Value),
                    branchWorkingHrsDetails = BranchWorkingHrsDetails(ViewState["IsHoliday"].ToString().TrimEnd(','),
                    txtfrmtime.Text, txttotime.Text, Session["Workingdays"].ToString().TrimEnd(','))
                };

                HttpResponseMessage response = client.PostAsJsonAsync("branchMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        Cancel();
                        BindBranchMaster();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + response.ReasonPhrase.Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    public List<branchWorkingHrsDetails> BranchWorkingHrsDetails(string isHoliday, string fromTime,
       string toTime, string workingDay)
    {

        string[] workingDays = workingDay.Split(',');
        string[] isHolidays = isHoliday.Split(',');

        List<branchWorkingHrsDetails> lst = new List<branchWorkingHrsDetails>();
        for (int i = 0; i < workingDays.Count(); i++)
        {
            if (workingDays[i] != "")
            {
                lst.AddRange(new List<branchWorkingHrsDetails>
            {

            new branchWorkingHrsDetails { isHoliday=isHolidays[i] ,fromTime=fromTime, toTime=toTime,workingDay=workingDays[i]

            }
            });
            }

        }


        return lst;

    }

    #endregion
    #region Update Function
    public void UpdateBranchMaster()
    {
        try
        {
            ChkBoxSelect();
            string multiBook = string.Empty;
            string onlineBooking = string.Empty;
            string PayBook = string.Empty;
            string PayLater = string.Empty;
            string CheckIN = string.Empty;
            string CheckOut = string.Empty;
            string MinHour = string.Empty;
            string MaxHour = string.Empty;
            string MaxDay = string.Empty;
            string MinDay = string.Empty;
            string DayorMin = string.Empty;
            string ABcharge = string.Empty;
            if (chkMultiBook.Checked)
            {
                multiBook = "Y";
            }
            else
            {
                multiBook = "N";
            }
            if (chkOnlineB.Checked)
            {
                onlineBooking = "Y";
            }
            else
            {
                onlineBooking = "N";
            }
            if (chkPayB.Checked)
            {
                PayBook = "Y";
            }
            else
            {
                PayBook = "N";
            }
            if (chkPaylater.Checked)
            {
                PayLater = "Y";
            }
            else
            {
                PayLater = "N";
            }
            if (chkChechIn.Checked)
            {
                CheckIN = "Y";
            }
            else
            {
                CheckIN = "N";
            }
            if (chkBCheckout.Checked)
            {
                CheckOut = "Y";
            }
            else
            {
                CheckOut = "N";
            }
            if (txtminHour.Text == "")
            {
                MinHour = "0";
            }
            else
            {
                MinHour = txtminHour.Text;

            }
            if (txtMaxhour.Text == "")
            {
                MaxHour = "0";
            }
            else
            {
                MaxHour = txtMaxhour.Text;

            }
            if (txtminDay.Text == "")
            {
                MinDay = "0";
            }
            else
            {
                MinDay = txtminDay.Text;

            }
            if (txtMaxDay.Text == "")
            {
                MaxDay = "0";
            }
            else
            {
                MaxDay = txtMaxDay.Text;

            }
            if (txtNoofDorH.Text == "")
            {
                DayorMin = "0";
            }
            else
            {
                DayorMin = txtNoofDorH.Text;

            }
            if (txtAdBCharge.Text == "")
            {
                ABcharge = "0";
            }
            else
            {
                ABcharge = txtAdBCharge.Text;

            }
            DateTime FromDate = Convert.ToDateTime(txtLicensePeriodFrom.Text);
            DateTime Todate = Convert.ToDateTime(txtLicensePeriodTo.Text);
            string LicensefromDate = FromDate.ToString("yyyy-MM-dd");
            string LicenseToDate = Todate.ToString("yyyy-MM-dd");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new BranchMasterUpdateClass()
                {
                    branchId = ViewState["branchId"].ToString(),
                    parkingOwnerId = ddlownername.SelectedValue,
                    shortName = txtBranchShortName.Text,
                    branchName = txtBranchName.Text,
                    latitude = txtLatitude.Text,
                    longitude = txtLongitude.Text,
                    address1 = txtAddress1.Text,
                    address2 = txtAddress2.Text,
                    district = hfDistrict.Value.Trim(),
                    state = hfState.Value.Trim(),
                    city = hfCity.Value.Trim(),
                    pincode = txtPincode.Text,
                    phoneNumber = txtPhoneNo.Text,
                    alternatePhoneNumber = txtAlterPhoneNo.Text,
                    licenseNo = txtLicenseNo.Text,
                    emailId = txtEmailId.Text,
                    licensePeriodFrom = LicensefromDate.Trim(),
                    licensePeriodTo = LicenseToDate.Trim(),
                    license = hfLicence.Value,
                    document1 = hfdocument1.Value,
                    document2 = hfdocument2.Value,
                    multiBook = multiBook,
                    isPayBookAvailable = PayBook,
                    isPayLaterAvaialble = PayLater,
                    isBookCheckInAvailable = CheckIN,
                    isPayAtCheckoutAvailable = CheckOut,
                    advanceBookingHourOrDayType = ddlHorDType.SelectedValue,
                    advanceBookingHourOrDay = DayorMin,
                    advanceBookingCharges = ABcharge,
                    minHour = MinHour,
                    maxHour = MaxHour,
                    maxDay = MaxDay,
                    minDay = MinDay,
                    approvalStatus = ViewState["approvalStatus"].ToString(),
                    onlineBookingAvailability = onlineBooking,
                    activeStatus = "A",
                    branchImageMasterDetails = UpdateGetImageUrl(hfImageUrl.Value, ViewState["ImageId"].ToString().Trim()),
                    updatedBy = Session["UserId"].ToString(),
                    branchWorkingHrs = UpdateBranchWorkingHrsDetails(ViewState["IsHoliday"].ToString().TrimEnd(','), txtfrmtime.Text, txttotime.Text,
                    Session["Workingdays"].ToString(), Session["hfWorkingHrsuniqueId"].ToString())
                };
                HttpResponseMessage response = client.PutAsJsonAsync("branchMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Replace("'", "").Trim() + "');", true);
                        Cancel();
                        BindBranchMaster();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Replace("'", "").Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    public List<UpdatebranchWorkingHrsDetails> UpdateBranchWorkingHrsDetails(string isHoliday, string fromTime,
     string toTime, string workingDay, string uniqueId)
    {

        string[] workingDays;
        string[] isHolidays;
        string[] uniqueIds;

        workingDays = workingDay.TrimEnd().Split(',');
        uniqueIds = uniqueId.TrimEnd().Split(',');
        isHolidays = isHoliday.TrimEnd().Split(',');
        if (workingDays.Count() != uniqueIds.Count())
        {

        }


        List<UpdatebranchWorkingHrsDetails> lst = new List<UpdatebranchWorkingHrsDetails>();

        for (int i = 0; i < workingDays.Count(); i++)
        {
            if (workingDays[i] != "" & uniqueIds[i] != "")
            {
                lst.AddRange(new List<UpdatebranchWorkingHrsDetails>
            {

            new UpdatebranchWorkingHrsDetails { isHoliday=isHolidays[i] ,fromTime=fromTime, toTime=toTime,workingDay=workingDays[i],uniqueId=uniqueIds[i]

            }

            });
            }

        }
        return lst;

    }

    #endregion
    #region Arrays for Insert and Update
    public static List<branchImageMasterDetails> GetImageUrl(string ImageUrl)
    {
        string[] ImageUrls;


        ImageUrls = ImageUrl.Split(',');
        List<branchImageMasterDetails> lst = new List<branchImageMasterDetails>();
        for (int i = 0; i < ImageUrls.Count(); i++)
        {
            lst.AddRange(new List<branchImageMasterDetails>
            {
                new branchImageMasterDetails { imageUrl=ImageUrls[i]}

            });
        }

        return lst;

    }
    public static List<UpdatebranchImageMasterDetails> UpdateGetImageUrl(string ImageUrl, string ImageId)
    {
        string[] ImageUrls;
        string[] ImageIds;

        ImageUrls = ImageUrl.Split(',');
        ImageIds = ImageId.Split(',');
        List<UpdatebranchImageMasterDetails> lst = new List<UpdatebranchImageMasterDetails>();
        for (int i = 0; i < ImageUrls.Count(); i++)
        {
            lst.AddRange(new List<UpdatebranchImageMasterDetails>
            {
                new UpdatebranchImageMasterDetails { imageUrl=ImageUrls[i],imageId=ImageIds[i]}

            });
        }

        return lst;

    }
    #endregion

    #region Grid View RowDataBound 
    protected void gvBranchmaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowIndex != -1)
        {
            var Branch = e.Row.DataItem as BranchMasterClass;
            var dataList = e.Row.FindControl("DataList1") as DataList;
            dataList.DataSource = Branch.branchImageMasterDetails;
            dataList.DataBind();
            var dataList1 = e.Row.FindControl("dlWorkingHrs") as DataList;
            dataList1.DataSource = Branch.branchWorkingHrsDetails;
            dataList1.DataBind();

        }

    }
    #endregion
    #region Update Approval Status
    protected void ImgClosed_Click(object sender, ImageClickEventArgs e)
    {
        modalCard.Visible = false;
        txtReason.Text = "";
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
    }

    protected void btnSubmitPopup_Click(object sender, EventArgs e)
    {
        ChangeApprovalStatus(ViewState["ApproveBranchId"].ToString(), "N");
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        modalCard.Visible = false;
        txtReason.Text = "";
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
        ChangeApprovalStatus(ViewState["ApproveBranchId"].ToString(), "A");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divCancellationReason.Visible = true;
        divsubmit.Visible = true;
    }
    protected void lblGvapprovalStatus_Click(object sender, EventArgs e)
    {
        modalCard.Visible = true;
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
        LinkButton lnkbtn = sender as LinkButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        LinkButton lblGvapprovalStatus = (LinkButton)gvrow.FindControl("lblGvapprovalStatus");
        Label lblbranchId = (Label)gvrow.FindControl("lblbranchId");
        string sActiveStatus = lblGvapprovalStatus.Text.Trim() == "Waiting List" ? "W" : "A";
        ViewState["ApproveBranchId"] = lblbranchId.Text;
        ViewState["ApproveStatus"] = sActiveStatus.Trim();

    }

    public void ChangeApprovalStatus(string BranchId, string ApprovalStatus)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "approvalStatus?branchId=" + BranchId + "&approvalStatus=" + ApprovalStatus + "";
                sUrl += " cancellationReason = " + txtReason.Text == "" ? null : txtReason.Text + "";
                var Insert = new Approval()
                {
                    branchId = BranchId,
                    approvalStatus = ApprovalStatus,
                    cancellationReason = txtReason.Text == "" ? null : txtReason.Text
                };

                HttpResponseMessage response = client.PutAsJsonAsync(sUrl, Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        Cancel();
                        BindBranchMaster();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    public class Approval
    {
        public string userId { get; set; }
        public string branchId { get; set; }
        public string approvalStatus { get; set; }
        public string cancellationReason { get; set; }

    }


    #endregion

    #region Edit Click
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            hfImageUrl.Value = "../../images/EmptyImageNew.png";
            imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
            btnSubmit.Text = "Update";
            spAddorEdit.InnerText = "Edit ";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblbranchId = (Label)gvrow.FindControl("lblbranchId");
            Label lblparkingOwnerId = (Label)gvrow.FindControl("lblparkingOwnerId");
            hfParkingOwnerId.Value = lblparkingOwnerId.Text;
            ddlownername.SelectedValue = lblparkingOwnerId.Text;
            Label lblbranchName = (Label)gvrow.FindControl("lblbranchName");
            Label lblshortName = (Label)gvrow.FindControl("lblshortName");
            Label lbllatitude = (Label)gvrow.FindControl("lbllatitude");
            Label lblurl = (Label)gvrow.FindControl("lblurl");
            Label lblAddress1 = (Label)gvrow.FindControl("lblAddress1");
            Label lblphoneNumber = (Label)gvrow.FindControl("lblphoneNumber");
            Label lbllicenseNo = (Label)gvrow.FindControl("lbllicenseNo");
            Label lblmultiBook = (Label)gvrow.FindControl("lblmultiBook");
            Label lblapprovalStatus = (Label)gvrow.FindControl("lblapprovalStatus");
            Label lblonlineBookingAvailability = (Label)gvrow.FindControl("lblonlineBookingAvailability");
            Label lblisPayBookAvailable = (Label)gvrow.FindControl("lblisPayBookAvailable");
            Label lblisBookCheckInAvailable = (Label)gvrow.FindControl("lblisBookCheckInAvailable");
            Label lblisPayAtCheckoutAvailable = (Label)gvrow.FindControl("lblisPayAtCheckoutAvailable");
            Label lblisPayLaterAvaialble = (Label)gvrow.FindControl("lblisPayLaterAvaialble");
            Label lbladvanceBookingHourOrDayType = (Label)gvrow.FindControl("lbladvanceBookingHourOrDayType");
            Label lbladvanceBookingHourOrDay = (Label)gvrow.FindControl("lbladvanceBookingHourOrDay");
            Label lbladvanceBookingCharges = (Label)gvrow.FindControl("lbladvanceBookingCharges");
            Label lblminHour = (Label)gvrow.FindControl("lblminHour");
            Label lblmaxHour = (Label)gvrow.FindControl("lblmaxHour");
            Label lblminDay = (Label)gvrow.FindControl("lblminDay");
            Label lblmaxDay = (Label)gvrow.FindControl("lblmaxDay");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvBranchmaster = gvBranchmaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["approvalStatus"] = lblapprovalStatus.Text;
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text == "Active" ? "A" : "D";
            ViewState["branchId"] = sgvBranchmaster.ToString().Trim();
            txtBranchName.Text = lblbranchName.Text;
            txtNoofDorH.Text = lbladvanceBookingHourOrDay.Text;
            txtAdBCharge.Text = lbladvanceBookingCharges.Text;
            txtminHour.Text = lblminHour.Text;
            txtMaxhour.Text = lblmaxHour.Text;
            txtminDay.Text = lblminDay.Text;
            txtMaxDay.Text = lblmaxDay.Text;
            string Latitude = lbllatitude.Text;
            string[] sLatitude = Latitude.Split(':');

            txtLatitude.Text = sLatitude[0].ToString().Trim();
            hflatitude.Value = sLatitude[0].ToString().Trim();
            txtLongitude.Text = sLatitude[1].ToString().Trim();
            hflongitude.Value = sLatitude[1].ToString().Trim();

            string Address = lblAddress1.Text;
            string[] sAddress = Address.Split(':');
            txtAddress1.Text = sAddress[0].ToString().Trim();
            txtBranchShortName.Text = lblshortName.Text;
            txtAddress2.Text = sAddress[1].ToString().Trim();
            txtDistrict.Text = sAddress[2].ToString().Trim();
            txtState.Text = sAddress[3].ToString().Trim();
            txtCity.Text = sAddress[4].ToString().Trim();
            hfDistrict.Value = txtDistrict.Text.Trim();
            hfState.Value = txtState.Text.Trim();
            hfCity.Value = txtCity.Text.Trim();
            txtPincode.Text = sAddress[5].ToString().Trim();

            string PhoneNo = lblphoneNumber.Text;
            string[] sPhoneNo = PhoneNo.Split(':');
            txtPhoneNo.Text = sPhoneNo[0].ToString().Trim();
            txtAlterPhoneNo.Text = sPhoneNo[1].ToString().Trim();
            txtEmailId.Text = sPhoneNo[2].ToString().Trim();
            string licenseNo = lbllicenseNo.Text;
            string[] slicenseNo = licenseNo.Split(':');
            txtLicenseNo.Text = slicenseNo[0].ToString().Trim();
            txtLicensePeriodFrom.Text = slicenseNo[1].ToString().Trim();
            txtLicensePeriodTo.Text = slicenseNo[2].ToString().Trim();
            if (lbladvanceBookingHourOrDayType.Text != "0")
            {
                ddlHorDType.SelectedValue = lbladvanceBookingHourOrDayType.Text;

                if (ddlHorDType.SelectedValue == "D")
                {
                    lblDorHNo.Text = "Advance Booking Days";
                }
                else
                {
                    lblDorHNo.Text = "Advance Booking Hours";
                }
                ClientScript.RegisterStartupScript(this.GetType(), "callfunction", "lblDaysorMin();", true);
            }
            string url = lblurl.Text;
            string[] slblurl = url.Split('~');
            hfLicence.Value = slblurl[0].ToString().Trim();

            if (slblurl[1] != "")
            {
                hfdocument1.Value = slblurl[1].ToString().Trim();
                string[] Doc1 = hfdocument1.Value.Split('=');
                lblDoc1View.Text = Doc1[1].ToString().Trim();
            }
            if (slblurl[2] != "")
            {
                hfdocument2.Value = slblurl[2].ToString().Trim();
                string[] Doc2 = hfdocument2.Value.Split('=');
                lblDoc2View.Text = Doc2[1].ToString().Trim();
            }
            if (hfLicence.Value != "")
            {
                string[] licurl = hfLicence.Value.Split('=');
                lbllicenseview.Text = licurl[1].ToString().Trim();
            }

            var dataList = gvrow.FindControl("DataList1") as DataList;
            Label lblimageUrl = dataList.Items[0].FindControl("lblimageUrl") as Label;
            Label lblimageId = dataList.Items[0].FindControl("lblimageId") as Label;
            ViewState["ImageId"] = lblimageId.Text;
            hfImageUrl.Value = lblimageUrl.Text;
            imgEmpPhotoPrev.ImageUrl = lblimageUrl.Text;
            if (lblimageUrl.Text == "")
            {
                hfImageUrl.Value = "../../images/EmptyImageNew.png";
                imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
            }
            if (slblurl[0].ToString().Trim() != "")
            {
                LicenceImage.Visible = true;
                WrapDiv1.Attributes.Add("class", "DisplayHide");
            }
            if (slblurl[1].ToString().Trim() != "")
            {
                Document1Image.Visible = true;
                WrapDiv2.Attributes.Add("class", "DisplayHide");
            }
            if (slblurl[2].ToString().Trim() != "")
            {
                Document2Image.Visible = true;
                WrapDiv3.Attributes.Add("class", "DisplayHide");
            }

            if (lblmultiBook.Text == "Y")
            {
                chkMultiBook.Checked = true;
            }
            else
            {
                chkMultiBook.Checked = false;
            }
            if (lblonlineBookingAvailability.Text == "Y")
            {
                chkOnlineB.Checked = true;
            }
            else
            {
                chkOnlineB.Checked = false;
            }
            if (lblisPayBookAvailable.Text == "Y")
            {
                chkPayB.Checked = true;
            }
            else
            {
                chkPayB.Checked = false;
            }
            if (lblisBookCheckInAvailable.Text == "Y")
            {
                chkChechIn.Checked = true;
            }
            else
            {
                chkChechIn.Checked = false;
            }
            if (lblisPayAtCheckoutAvailable.Text == "Y")
            {
                chkBCheckout.Checked = true;
            }
            else
            {
                chkBCheckout.Checked = false;
            }
            if (lblisPayLaterAvaialble.Text == "Y")
            {
                chkPaylater.Checked = true;
            }
            else
            {
                chkPaylater.Checked = false;
            }
            var dataList1 = gvrow.FindControl("dlWorkingHrs") as DataList;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataList1.Items.Count; i++)
            {
                string hfWorkingHrsuniqueId = string.Empty;
                string hfid = string.Empty;
                Label lblgvworkingDay = dataList1.Items[i].FindControl("lblgvworkingDay") as Label;
                Label lblgvfromTime = dataList1.Items[i].FindControl("lblgvfromTime") as Label;
                Label lblgvtoTime = dataList1.Items[i].FindControl("lblgvtoTime") as Label;
                Label lblgvisHoliday = dataList1.Items[i].FindControl("lblgvisHoliday") as Label;
                Label lblgvuniqueId = dataList1.Items[i].FindControl("lblgvuniqueId") as Label;
                lblWorkingDaysValue.Text = lblgvworkingDay.Text;
                txtfrmtime.Text = lblgvfromTime.Text;
                txttotime.Text = lblgvtoTime.Text;
                hfWorkingHrsuniqueId = lblgvuniqueId.Text;
                if (hfWorkingHrsuniqueId != "0")
                {
                    hfid = hfWorkingHrsuniqueId + ",";

                }

                switch (lblWorkingDaysValue.Text)
                {
                    case "Monday":
                        DivHrDetails.Visible = true;
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[1].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[1].Selected = true;
                            chkWorkingDays.Items[1].Attributes.Add("Class", "DaybtnClick");
                        }

                        break;
                    case "Tuesday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[2].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[2].Selected = true;
                            chkWorkingDays.Items[2].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;

                    case "Wednesday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[3].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[3].Selected = true;
                            chkWorkingDays.Items[3].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    case "Thursday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[4].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[4].Selected = true;
                            chkWorkingDays.Items[4].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    case "Friday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[5].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[5].Selected = true;
                            chkWorkingDays.Items[5].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    case "Saturday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[6].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[6].Selected = true;
                            chkWorkingDays.Items[6].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    default:
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[0].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[0].Selected = true;
                            chkWorkingDays.Items[0].Attributes.Add("Class", "DaybtnClick");
                        }
                        break;
                }
                Session["hfWorkingHrsuniqueId"] = sb.Append(hfid);
            }
            ADD();
            ddlownername.Enabled = false;

        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }
    }
    #endregion
    #region Delete Click
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sBranchId = gvBranchmaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "branchMaster?branchId=" + sBranchId
                            + "&activeStatus="
                            + sActiveStatus;

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        BindBranchMaster();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region View Click
    protected void lnkView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            hfView.Value = "1";
            hfImageUrl.Value = "../../images/EmptyImageNew.png";
            imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
            spAddorEdit.InnerText = "View ";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblbranchId = (Label)gvrow.FindControl("lblbranchId");
            Label lblparkingOwnerId = (Label)gvrow.FindControl("lblparkingOwnerId");
            hfParkingOwnerId.Value = lblparkingOwnerId.Text;
            ddlownername.SelectedValue = lblparkingOwnerId.Text;
            Label lblbranchName = (Label)gvrow.FindControl("lblbranchName");
            Label lblshortName = (Label)gvrow.FindControl("lblshortName");
            Label lbllatitude = (Label)gvrow.FindControl("lbllatitude");
            Label lblurl = (Label)gvrow.FindControl("lblurl");
            Label lblAddress1 = (Label)gvrow.FindControl("lblAddress1");
            Label lblphoneNumber = (Label)gvrow.FindControl("lblphoneNumber");
            Label lbllicenseNo = (Label)gvrow.FindControl("lbllicenseNo");
            Label lblmultiBook = (Label)gvrow.FindControl("lblmultiBook");
            Label lblapprovalStatus = (Label)gvrow.FindControl("lblapprovalStatus");
            Label lblonlineBookingAvailability = (Label)gvrow.FindControl("lblonlineBookingAvailability");
            Label lblisPayBookAvailable = (Label)gvrow.FindControl("lblisPayBookAvailable");
            Label lblisBookCheckInAvailable = (Label)gvrow.FindControl("lblisBookCheckInAvailable");
            Label lblisPayAtCheckoutAvailable = (Label)gvrow.FindControl("lblisPayAtCheckoutAvailable");
            Label lblisPayLaterAvaialble = (Label)gvrow.FindControl("lblisPayLaterAvaialble");
            Label lbladvanceBookingHourOrDayType = (Label)gvrow.FindControl("lbladvanceBookingHourOrDayType");
            Label lbladvanceBookingHourOrDay = (Label)gvrow.FindControl("lbladvanceBookingHourOrDay");
            Label lbladvanceBookingCharges = (Label)gvrow.FindControl("lbladvanceBookingCharges");
            Label lblminHour = (Label)gvrow.FindControl("lblminHour");
            Label lblmaxHour = (Label)gvrow.FindControl("lblmaxHour");
            Label lblminDay = (Label)gvrow.FindControl("lblminDay");
            Label lblmaxDay = (Label)gvrow.FindControl("lblmaxDay");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvBranchmaster = gvBranchmaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["approvalStatus"] = lblapprovalStatus.Text;
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text == "Active" ? "A" : "D";
            ViewState["branchId"] = sgvBranchmaster.ToString().Trim();
            txtBranchName.Text = lblbranchName.Text;
            txtNoofDorH.Text = lbladvanceBookingHourOrDay.Text;
            txtAdBCharge.Text = lbladvanceBookingCharges.Text;
            txtminHour.Text = lblminHour.Text;
            txtMaxhour.Text = lblmaxHour.Text;
            txtminDay.Text = lblminDay.Text;
            txtMaxDay.Text = lblmaxDay.Text;
            string Latitude = lbllatitude.Text;
            string[] sLatitude = Latitude.Split(':');

            txtLatitude.Text = sLatitude[0].ToString().Trim();
            hflatitude.Value = sLatitude[0].ToString().Trim();
            txtLongitude.Text = sLatitude[1].ToString().Trim();
            hflongitude.Value = sLatitude[1].ToString().Trim();

            string Address = lblAddress1.Text;
            string[] sAddress = Address.Split(':');
            txtAddress1.Text = sAddress[0].ToString().Trim();
            txtBranchShortName.Text = lblshortName.Text;
            txtAddress2.Text = sAddress[1].ToString().Trim();
            txtDistrict.Text = sAddress[2].ToString().Trim();
            txtState.Text = sAddress[3].ToString().Trim();
            txtCity.Text = sAddress[4].ToString().Trim();
            hfDistrict.Value = txtDistrict.Text.Trim();
            hfState.Value = txtState.Text.Trim();
            hfCity.Value = txtCity.Text.Trim();
            txtPincode.Text = sAddress[5].ToString().Trim();

            string PhoneNo = lblphoneNumber.Text;
            string[] sPhoneNo = PhoneNo.Split(':');
            txtPhoneNo.Text = sPhoneNo[0].ToString().Trim();
            txtAlterPhoneNo.Text = sPhoneNo[1].ToString().Trim();
            txtEmailId.Text = sPhoneNo[2].ToString().Trim();
            string licenseNo = lbllicenseNo.Text;
            string[] slicenseNo = licenseNo.Split(':');
            txtLicenseNo.Text = slicenseNo[0].ToString().Trim();
            txtLicensePeriodFrom.Text = slicenseNo[1].ToString().Trim();
            txtLicensePeriodTo.Text = slicenseNo[2].ToString().Trim();
            if (lbladvanceBookingHourOrDayType.Text != "0")
            {
                ddlHorDType.SelectedValue = lbladvanceBookingHourOrDayType.Text;

                if (ddlHorDType.SelectedValue == "D")
                {
                    lblDorHNo.Text = "Advance Booking Days";
                }
                else
                {
                    lblDorHNo.Text = "Advance Booking Hours";
                }
                ClientScript.RegisterStartupScript(this.GetType(), "callfunction", "lblDaysorMin();", true);
            }
            string url = lblurl.Text;
            string[] slblurl = url.Split('~');
            hfLicence.Value = slblurl[0].ToString().Trim();

            if (slblurl[1] != "")
            {
                hfdocument1.Value = slblurl[1].ToString().Trim();
                string[] Doc1 = hfdocument1.Value.Split('=');
                lblDoc1View.Text = Doc1[1].ToString().Trim();
            }
            if (slblurl[2] != "")
            {
                hfdocument2.Value = slblurl[2].ToString().Trim();
                string[] Doc2 = hfdocument2.Value.Split('=');
                lblDoc2View.Text = Doc2[1].ToString().Trim();
            }
            if (hfLicence.Value != "")
            {
                string[] licurl = hfLicence.Value.Split('=');
                lbllicenseview.Text = licurl[1].ToString().Trim();
            }

            var dataList = gvrow.FindControl("DataList1") as DataList;
            Label lblimageUrl = dataList.Items[0].FindControl("lblimageUrl") as Label;
            Label lblimageId = dataList.Items[0].FindControl("lblimageId") as Label;
            ViewState["ImageId"] = lblimageId.Text;
            hfImageUrl.Value = lblimageUrl.Text;
            imgEmpPhotoPrev.ImageUrl = lblimageUrl.Text;
            if (lblimageUrl.Text == "")
            {
                hfImageUrl.Value = "../../images/EmptyImageNew.png";
                imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
            }
            if (slblurl[0].ToString().Trim() != "")
            {
                LicenceImage.Visible = true;
                WrapDiv1.Attributes.Add("class", "DisplayHide");
            }
            if (slblurl[1].ToString().Trim() != "")
            {
                Document1Image.Visible = true;
                WrapDiv2.Attributes.Add("class", "DisplayHide");
            }
            if (slblurl[2].ToString().Trim() != "")
            {
                Document2Image.Visible = true;
                WrapDiv3.Attributes.Add("class", "DisplayHide");
            }

            if (lblmultiBook.Text == "Y")
            {
                chkMultiBook.Checked = true;
            }
            else
            {
                chkMultiBook.Checked = false;
            }
            if (lblonlineBookingAvailability.Text == "Y")
            {
                chkOnlineB.Checked = true;
            }
            else
            {
                chkOnlineB.Checked = false;
            }
            if (lblisPayBookAvailable.Text == "Y")
            {
                chkPayB.Checked = true;
            }
            else
            {
                chkPayB.Checked = false;
            }
            if (lblisBookCheckInAvailable.Text == "Y")
            {
                chkChechIn.Checked = true;
            }
            else
            {
                chkChechIn.Checked = false;
            }
            if (lblisPayAtCheckoutAvailable.Text == "Y")
            {
                chkBCheckout.Checked = true;
            }
            else
            {
                chkBCheckout.Checked = false;
            }
            if (lblisPayLaterAvaialble.Text == "Y")
            {
                chkPaylater.Checked = true;
            }
            else
            {
                chkPaylater.Checked = false;
            }
            var dataList1 = gvrow.FindControl("dlWorkingHrs") as DataList;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataList1.Items.Count; i++)
            {
                string hfWorkingHrsuniqueId = string.Empty;
                string hfid = string.Empty;
                Label lblgvworkingDay = dataList1.Items[i].FindControl("lblgvworkingDay") as Label;
                Label lblgvfromTime = dataList1.Items[i].FindControl("lblgvfromTime") as Label;
                Label lblgvtoTime = dataList1.Items[i].FindControl("lblgvtoTime") as Label;
                Label lblgvisHoliday = dataList1.Items[i].FindControl("lblgvisHoliday") as Label;
                Label lblgvuniqueId = dataList1.Items[i].FindControl("lblgvuniqueId") as Label;
                lblWorkingDaysValue.Text = lblgvworkingDay.Text;
                txtfrmtime.Text = lblgvfromTime.Text;
                txttotime.Text = lblgvtoTime.Text;
                hfWorkingHrsuniqueId = lblgvuniqueId.Text;
                if (hfWorkingHrsuniqueId != "0")
                {
                    hfid = hfWorkingHrsuniqueId + ",";

                }

                switch (lblWorkingDaysValue.Text)
                {
                    case "Monday":
                        DivHrDetails.Visible = true;
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[1].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[1].Selected = true;
                            chkWorkingDays.Items[1].Attributes.Add("Class", "DaybtnClick");
                        }

                        break;
                    case "Tuesday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[2].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[2].Selected = true;
                            chkWorkingDays.Items[2].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;

                    case "Wednesday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[3].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[3].Selected = true;
                            chkWorkingDays.Items[3].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    case "Thursday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[4].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[4].Selected = true;
                            chkWorkingDays.Items[4].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    case "Friday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[5].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[5].Selected = true;
                            chkWorkingDays.Items[5].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    case "Saturday":
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[6].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[6].Selected = true;
                            chkWorkingDays.Items[6].Attributes.Add("Class", "DaybtnClick");
                        }
                        DivHrDetails.Visible = true;
                        break;
                    default:
                        if (lblgvisHoliday.Text == "true")
                        {
                            chkWorkingDays.Items[0].Selected = false;
                        }
                        else
                        {
                            chkWorkingDays.Items[0].Selected = true;
                            chkWorkingDays.Items[0].Attributes.Add("Class", "DaybtnClick");
                        }
                        break;
                }
                Session["hfWorkingHrsuniqueId"] = sb.Append(hfid);
            }
            ADD();
            ddlownername.Enabled = false;
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }

    }
    #endregion
    #region Reset Click
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion
    #region Cancel 
    public void Cancel()
    {
        hfView.Value = "0";
        modalCard.Visible = false;
        txtReason.Text = "";
        divCancellationReason.Visible = false;
        divsubmit.Visible = false;
        divGv.Visible = true;
        divForm.Visible = false;
        DivHrDetails.Visible = false;
        txtBranchName.Text = string.Empty;
        txtLatitude.Text = string.Empty;
        txtLongitude.Text = string.Empty;
        txtAddress1.Text = string.Empty;
        txtAddress2.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        txtState.Text = string.Empty;
        txtCity.Text = string.Empty;
        hfDistrict.Value = string.Empty;
        hfState.Value = string.Empty;
        hfCity.Value = string.Empty;
        txtPincode.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtAlterPhoneNo.Text = string.Empty;
        txtEmailId.Text = string.Empty;
        txtLicenseNo.Text = string.Empty;
        txtLicensePeriodFrom.Text = string.Empty;
        txtLicensePeriodTo.Text = string.Empty;
        hfdocument1.Value = "";
        hfdocument2.Value = "";
        hfImageUrl.Value = "";
        txttotime.Text = "";
        txtfrmtime.Text = "";
        hfLicence.Value = "";
        hflatitude.Value = "";
        hflongitude.Value = "";
        chkBCheckout.Checked = false;
        chkChechIn.Checked = false;
        chkPayB.Checked = false;
        chkPaylater.Checked = false;
        ddlHorDType.ClearSelection();
        txtminDay.Text = "0";
        txtMaxhour.Text = "0";
        txtMaxDay.Text = "0";
        txtminHour.Text = "0";
        txtNoofDorH.Text = "0";
        txtAdBCharge.Text = "0";
        txtBranchShortName.Text = "";
        chkMultiBook.Checked = false;
        chkOnlineB.Checked = false;
        imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
        btnSubmit.Text = "Submit";
        LicenceImage.Visible = false;
        Document1Image.Visible = false;
        Document2Image.Visible = false;
        WrapDiv1.Attributes.Add("class", "WrapDiv");
        WrapDiv2.Attributes.Add("class", "WrapDiv");
        WrapDiv3.Attributes.Add("class", "WrapDiv");
        ddlownername.ClearSelection();
        ddlownername.Enabled = true;
        lbllicenseview.Text = "";
        lblDoc1View.Text = "";
        lblDoc2View.Text = "";
        chkWorkingDays.ClearSelection();
        if (Session["parkingOwnerId"].ToString() != "0")
        {
            ddlownername.Enabled = false;
        }
    }
    #endregion 

    #region Branch  Master Class For Insert
    public class BranchMasterClass
    {

        public String parkingOwnerId { get; set; }
        public String branchId { get; set; }
        public String parkingName { get; set; }
        public String branchName { get; set; }
        public String shortName { get; set; }
        public String latitude { get; set; }
        public String longitude { get; set; }
        public String address1 { get; set; }
        public String address2 { get; set; }
        public String district { get; set; }
        public String state { get; set; }
        public String city { get; set; }
        public String pincode { get; set; }
        public String phoneNumber { get; set; }
        public String alternatePhoneNumber { get; set; }
        public String emailId { get; set; }
        public String licenseNo { get; set; }
        public String licensePeriodFrom { get; set; }
        public String licensePeriodTo { get; set; }
        public String license { get; set; }
        public String document1 { get; set; }
        public String document2 { get; set; }
        public String multiBook { get; set; }
        public String isPayBookAvailable { get; set; }
        public String isPayLaterAvaialble { get; set; }
        public String isBookCheckInAvailable { get; set; }
        public String isPayAtCheckoutAvailable { get; set; }
        public String advanceBookingHourOrDayType { get; set; }
        public String advanceBookingHourOrDay { get; set; }
        public String advanceBookingCharges { get; set; }
        public String minHour { get; set; }
        public String maxHour { get; set; }
        public String minDay { get; set; }
        public String maxDay { get; set; }
        public String activeStatus { get; set; }
        public String approvalStatus { get; set; }
        public String onlineBookingAvailability { get; set; }
        public String createdBy { get; set; }
        public List<branchImageMasterDetails> branchImageMasterDetails { get; set; }
        public List<branchWorkingHrsDetails> branchWorkingHrsDetails { get; set; }

    }
    public class branchWorkingHrsDetails
    {
        public string isHoliday { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string workingDay { get; set; }
        public string uniqueId { get; set; }
    }
    public class branchImageMasterDetails
    {
        public string imageUrl { get; set; }
        public string imageId { get; set; }
    }
    #endregion
    #region Branch  Master Class For Update
    public class BranchMasterUpdateClass
    {
        public String branchId { get; set; }
        public String parkingOwnerId { get; set; }
        public String branchName { get; set; }
        public String shortName { get; set; }
        public String latitude { get; set; }
        public String longitude { get; set; }
        public String address1 { get; set; }
        public String address2 { get; set; }
        public String district { get; set; }
        public String state { get; set; }
        public String city { get; set; }
        public String pincode { get; set; }
        public String phoneNumber { get; set; }
        public String alternatePhoneNumber { get; set; }
        public String emailId { get; set; }
        public String licenseNo { get; set; }
        public String licensePeriodFrom { get; set; }
        public String licensePeriodTo { get; set; }
        public String license { get; set; }
        public String document1 { get; set; }
        public String document2 { get; set; }
        public String multiBook { get; set; }
        public String isPayBookAvailable { get; set; }
        public String isPayLaterAvaialble { get; set; }
        public String isBookCheckInAvailable { get; set; }
        public String isPayAtCheckoutAvailable { get; set; }
        public String advanceBookingHourOrDayType { get; set; }
        public String advanceBookingHourOrDay { get; set; }
        public String advanceBookingCharges { get; set; }
        public String minHour { get; set; }
        public String maxHour { get; set; }
        public String minDay { get; set; }
        public String maxDay { get; set; }
        public String approvalStatus { get; set; }
        public String onlineBookingAvailability { get; set; }
        public String updatedBy { get; set; }
        public String activeStatus { get; set; }
        public List<UpdatebranchWorkingHrsDetails> branchWorkingHrs { get; set; }
        public List<UpdatebranchImageMasterDetails> branchImageMasterDetails { get; set; }
    }
    public class UpdatebranchWorkingHrsDetails
    {
        public string isHoliday { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string workingDay { get; set; }
        public string uniqueId { get; set; }


    }
    public class UpdatebranchImageMasterDetails
    {
        public string imageUrl { get; set; }
        public string imageId { get; set; }
    }
    #endregion

    /// <summary>
    /// menu Access Rights 
    /// Created By Abhinaya K
    /// Created Date 02-AUG-2022
    /// </summary>
    #region menu Option Access
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
                        var Option = lst1.Where(x => x.optionName == "branchMaster" && x.MenuOptionAccessActiveStatus == "A")
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
                            var Add = Option.Select(y => y.AddRights).ToList();

                            if (Add[0] == "True")
                            {
                                btnAdd.Visible = true;
                            }
                            else
                            {
                                btnAdd.Visible = false;
                            }

                        }
                        else
                        {
                            divForm.Visible = false;
                            divGv.Visible = false;
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