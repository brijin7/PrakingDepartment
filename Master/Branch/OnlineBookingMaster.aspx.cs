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
using System.Globalization;

public partial class Master_OnlineBookingSetup : System.Web.UI.Page
{
    IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserId"] == null && Session["UserRole"] == null)
            {
                Session.Clear();
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
            }

            if (!IsPostBack)
            {
                if (Session["UserRole"].ToString().Trim() == "SA")
                {
                    gvOBMaster.Columns[12].Visible = false;//VIEW
                    gvOBMaster.Columns[13].Visible = true;//Edit                   
                }
                else
                {
                    gvOBMaster.Columns[12].Visible = true;//VIEW
                    gvOBMaster.Columns[13].Visible = false;
                }
                BindOwner();
                BindBranch();
                txtminDay.Text = "0";
                txtMaxhour.Text = "0";
                txtMaxDay.Text = "0";
                txtminHour.Text = "0";
                txtNoofDorH.Text = "0";
                txtAdBCharge.Text = "0";
            }
            if (Session["UserRole"].ToString() == "E")
            {
                BindMenuAccess();
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);

        }

    }

    #region Bind Dropdown Owner,Branch
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
    public void BindBranch()
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
                //if (Session["parkingOwnerId"].ToString() == "0")
                //{
                //    sUrl = Session["BaseUrl"].ToString().Trim()
                //       + "branchMaster?activeStatus=A&approvalStatus=A";
                //}
                //else
                //{
                //    sUrl = Session["BaseUrl"].ToString().Trim()
                //             + "branchMaster?activeStatus=A&approvalStatus=A&parkingOwnerId=";
                //    sUrl += Session["parkingOwnerId"].ToString().Trim();
                //}
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
                         + "branchMaster?activeStatus=A";
                }
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        List<OnlineBookingSetup> Branch = JsonConvert.DeserializeObject<List<OnlineBookingSetup>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Branch);

                        //dt.Columns.Add(new DataColumn("branch", System.Type.GetType("System.String"), "shortName + ' ~ ' + branchName"));
                        if (dt.Rows.Count > 0)
                        {
                            gvOBMaster.DataSource = dt;
                            gvOBMaster.DataBind();

                            ddlBranch.DataSource = dt;
                            ddlBranch.DataValueField = "branchId";
                            ddlBranch.DataTextField = "branchName";
                            ddlBranch.DataBind();
                        }
                        else
                        {

                        }
                        ddlBranch.Items.Insert(0, new ListItem("Select", "0"));

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
    }
    #endregion  
    #region Bind GV Online Booking Master 
    public void BindOBMaster()
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
                         + "branchMaster?activeStatus=A";
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
                        List<OnlineBookingSetup> OB = JsonConvert.DeserializeObject<List<OnlineBookingSetup>>(ResponseMsg);

                        DataTable dt = ConvertToDataTable(OB);
                        if (dt.Rows.Count > 0)
                        {
                            gvOBMaster.DataSource = OB;
                            gvOBMaster.DataBind();
                        }
                        else
                        {
                            spAddorEdit.InnerText = "Add ";
                            ADD();
                            gvOBMaster.DataSource = null;
                            gvOBMaster.DataBind();
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
    #endregion

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        ADD();
    }

    #endregion

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Update")
        {
            UpdateEmployee();
        }
    }
    #endregion

    #region Update Function
    public void UpdateEmployee()
    {
        string MinHour = string.Empty;
        string MaxHour = string.Empty;
        string MaxDay = string.Empty;
        string MinDay = string.Empty;
        string DayorMin = string.Empty;
        string ABcharge = string.Empty;
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

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                OnlineBookingSetup Insert = new OnlineBookingSetup()
                {
                    branchId = ddlBranch.SelectedValue.Trim(),
                    advanceBookingHourOrDayType = ddlHorDType.SelectedValue,
                    advanceBookingHourOrDay = DayorMin,
                    advanceBookingCharges = ABcharge,
                    minHour = MinHour,
                    maxHour = MaxHour,
                    minDay = MinDay,
                    maxDay = MaxDay,
                    updatedBy = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString()
                };

                HttpResponseMessage response = client.PutAsJsonAsync("hourDayUpdateBranchMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindBranch();
                        Cancel();
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

    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
    }
    #endregion


    protected void LnkEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            spAddorEdit.InnerText = "Edit ";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblbranchId = (Label)gvrow.FindControl("lblbranchId");
            Label lblparkingOwnerId = (Label)gvrow.FindControl("lblparkingOwnerId");
            ddlownername.SelectedValue = lblparkingOwnerId.Text;
            ddlBranch.SelectedValue = lblbranchId.Text;
            Label lblbranchName = (Label)gvrow.FindControl("lblbranchName");
            Label lblshortName = (Label)gvrow.FindControl("lblshortName");

            Label lblmultiBook = (Label)gvrow.FindControl("lblmultiBook");
            Label lblonlineBookingAvailability = (Label)gvrow.FindControl("lblonlineBookingAvailability");

            Label lbladvanceBookingHourOrDayType = (Label)gvrow.FindControl("lbladvanceBookingHourOrDayType");
            Label lbladvanceBookingHourOrDay = (Label)gvrow.FindControl("lbladvanceBookingHourOrDay");
            Label lbladvanceBookingCharges = (Label)gvrow.FindControl("lbladvanceBookingCharges");
            Label lblminHour = (Label)gvrow.FindControl("lblminHour");
            Label lblmaxHour = (Label)gvrow.FindControl("lblmaxHour");
            Label lblminDay = (Label)gvrow.FindControl("lblminDay");
            Label lblmaxDay = (Label)gvrow.FindControl("lblmaxDay");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");

            txtNoofDorH.Text = lbladvanceBookingHourOrDay.Text;
            txtAdBCharge.Text = lbladvanceBookingCharges.Text;
            txtminHour.Text = lblminHour.Text;
            txtMaxhour.Text = lblmaxHour.Text;
            txtminDay.Text = lblminDay.Text;
            txtMaxDay.Text = lblmaxDay.Text;

            if (lbladvanceBookingHourOrDayType.Text != "0")
            {
                ddlHorDType.SelectedValue = lbladvanceBookingHourOrDayType.Text == "Days" ? "D" : "H";

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

            string sgvBranchmaster = gvOBMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["branchId"] = sgvBranchmaster.ToString().Trim();

            if (lblmultiBook.Text == "Y")
            {
                dvMultiDays.Visible = true;
            }
            else
            {
                dvMultiDays.Visible = false;
            }
            if (lblonlineBookingAvailability.Text == "Y")
            {
                dvOnlineBooking.Visible = true;
            }
            else
            {
                dvOnlineBooking.Visible = false;
            }

            ADD();
            ddlownername.Enabled = false;
            ddlBranch.Enabled = false;
            btnSubmit.Visible = true;
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }

    }


    protected void lnkView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ddlHorDType.Enabled = false;
            txtNoofDorH.Enabled = false;
            txtAdBCharge.Enabled = false;
            txtminHour.Enabled = false;
            txtMaxhour.Enabled = false;
            txtminDay.Enabled = false;
            txtMaxDay.Enabled = false;
            spAddorEdit.InnerText = "View ";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblbranchId = (Label)gvrow.FindControl("lblbranchId");
            Label lblparkingOwnerId = (Label)gvrow.FindControl("lblparkingOwnerId");
            ddlownername.SelectedValue = lblparkingOwnerId.Text;
            ddlBranch.SelectedValue = lblbranchId.Text;
            Label lblbranchName = (Label)gvrow.FindControl("lblbranchName");
            Label lblshortName = (Label)gvrow.FindControl("lblshortName");

            Label lblmultiBook = (Label)gvrow.FindControl("lblmultiBook");
            Label lblonlineBookingAvailability = (Label)gvrow.FindControl("lblonlineBookingAvailability");

            Label lbladvanceBookingHourOrDayType = (Label)gvrow.FindControl("lbladvanceBookingHourOrDayType");
            Label lbladvanceBookingHourOrDay = (Label)gvrow.FindControl("lbladvanceBookingHourOrDay");
            Label lbladvanceBookingCharges = (Label)gvrow.FindControl("lbladvanceBookingCharges");
            Label lblminHour = (Label)gvrow.FindControl("lblminHour");
            Label lblmaxHour = (Label)gvrow.FindControl("lblmaxHour");
            Label lblminDay = (Label)gvrow.FindControl("lblminDay");
            Label lblmaxDay = (Label)gvrow.FindControl("lblmaxDay");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");

            txtNoofDorH.Text = lbladvanceBookingHourOrDay.Text;
            txtAdBCharge.Text = lbladvanceBookingCharges.Text;
            txtminHour.Text = lblminHour.Text;
            txtMaxhour.Text = lblmaxHour.Text;
            txtminDay.Text = lblminDay.Text;
            txtMaxDay.Text = lblmaxDay.Text;

            if (lbladvanceBookingHourOrDayType.Text != "0")
            {
                ddlHorDType.SelectedValue = lbladvanceBookingHourOrDayType.Text == "Days" ? "D" : "H";

                if (ddlHorDType.SelectedValue == "D")
                {
                    lblDorHNo.Text = "Advance Booking Days";
                }
                else
                {
                    lblDorHNo.Text = "Advance Booking Hours";
                }
            }

            string sgvBranchmaster = gvOBMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["branchId"] = sgvBranchmaster.ToString().Trim();

            if (lblmultiBook.Text == "Y")
            {
                dvMultiDays.Visible = true;
            }
            else
            {
                dvMultiDays.Visible = false;
            }
            if (lblonlineBookingAvailability.Text == "Y")
            {
                dvOnlineBooking.Visible = true;
            }
            else
            {
                dvOnlineBooking.Visible = false;
            }

            ADD();
            ddlownername.Enabled = false;
            ddlBranch.Enabled = false;
            btnSubmit.Visible = false;
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }

    }

    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion

    #region Cancel Fucntion
    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        ddlownername.ClearSelection();
        ddlBranch.ClearSelection();
        ddlHorDType.ClearSelection();
        txtminDay.Text = "0";
        txtMaxhour.Text = "0";
        txtMaxDay.Text = "0";
        txtminHour.Text = "0";
        txtNoofDorH.Text = "0";
        txtAdBCharge.Text = "0";
        btnSubmit.Text = "Update";
        spAddorEdit.InnerText = "";
        btnSubmit.Visible = true;
    }
    #endregion

    #region OnlineBookingSetup Class
    public class OnlineBookingSetup
    {
        public String parkingOwnerId { get; set; }
        public String branchId { get; set; }
        public String parkingName { get; set; }
        public String branchName { get; set; }
        public String multiBook { get; set; }
        public String onlineBookingAvailability { get; set; }
        public String advanceBookingHourOrDayType { get; set; }
        public String advanceBookingHourOrDay { get; set; }
        public String advanceBookingCharges { get; set; }
        public String minHour { get; set; }
        public String maxHour { get; set; }
        public String minDay { get; set; }
        public String maxDay { get; set; }
        public String activeStatus { get; set; }
        public String updatedBy { get; set; }
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
                            var Edit = Option.Select(y => y.EditRights).ToList();
                            var Delete = Option.Select(y => y.DeleteRights).ToList();
                            var View = Option.Select(y => y.ViewRights).ToList();
                            if (Add[0] == "True")
                            {
                                //gvOBMaster.Columns[13].Visible = true;//Edit       
                                gvOBMaster.Columns[12].Visible = true;//VIEW                              
                            }
                            else
                            {
                                //gvOBMaster.Columns[13].Visible = false;//Edit
                                gvOBMaster.Columns[12].Visible = false;//VIEW                               
                            }
                        }
                        else
                        {
                            divForm.Visible = false;
                            divGv.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('You donot have Access right to this Form');", true);
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