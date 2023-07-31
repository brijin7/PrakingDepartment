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

public partial class Master_ShiftMaster : System.Web.UI.Page
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
            txtSfStarttime.Text = DateTime.Now.ToString("HH\\:mm");
            txtSfEndtime.Text = DateTime.Now.ToString("HH\\:mm");
            txtBrStarttime.Text = DateTime.Now.ToString("HH\\:mm");
            txtBrEndtime.Text = DateTime.Now.ToString("HH\\:mm");
            BindBranchName();
            BindShiftName();
            BindShiftMaster();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion
    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion
    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertShift();
        }
        else
        {
            UpdateShift();
        }
    }
    #endregion
    #region Cancel Click Fucntion
    public void Cancel()
    {
        spAddorEdit.InnerText = "";
        divGv.Visible = true;
        divFomr.Visible = false;
        btnSubmitText();
        ddlShiftName.ClearSelection();
        ddlShiftName.SelectedValue = "0";
        txtSfStarttime.Text = DateTime.Now.ToString("HH\\:mm");
        txtSfEndtime.Text = DateTime.Now.ToString("HH\\:mm");
        txtBrStarttime.Text = DateTime.Now.ToString("HH\\:mm");
        txtBrEndtime.Text = DateTime.Now.ToString("HH\\:mm");
        txtGracePeriod.Text = string.Empty;
    }
    #endregion
    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divFomr.Visible = true;
    }
    #endregion
    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        ADD();
    }
    #endregion
    #region Bind Branch Name
    public void BindBranchName()
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
                          + "branchMaster?activeStatus=A&branchId=";
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
                            ddwnBranchName.DataSource = dt;
                            ddwnBranchName.DataValueField = "branchId";
                            ddwnBranchName.DataTextField = "branchName";
                            ddwnBranchName.DataBind();
                            if (dt.Rows.Count == 1)
                            {
                                ddwnBranchName.SelectedValue = dt.Rows[0]["branchId"].ToString();
                                ddwnBranchName.Enabled = false;
                            }
                        }
                        else
                        {
                            ddwnBranchName.DataBind();
                        }
                        ddwnBranchName.Items.Insert(0, new ListItem("Select", "0"));
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
    #region Bind Shift Name
    public void BindShiftName()
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
                      + "configMaster?configTypename=shiftName&activestatus=A";

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
                            ddlShiftName.DataSource = dt;
                            ddlShiftName.DataValueField = "configId";
                            ddlShiftName.DataTextField = "configName";
                            ddlShiftName.DataBind();
                        }
                        else
                        {
                            ddlShiftName.DataSource = null;
                            ddlShiftName.DataBind();
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlShiftName.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Bind Shift Master
    public void BindShiftMaster()
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
                        + "shiftMaster?branchId=";
                sUrl += Session["branchId"].ToString().Trim();

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
                            gvShiftmaster.DataSource = dt;
                            gvShiftmaster.DataBind();
                        }
                        else
                        {
                            spAddorEdit.InnerText = "Add ";
                            ADD();
                            gvShiftmaster.DataSource = null;
                            gvShiftmaster.DataBind();
                        }
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        ADD();
                       // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region Insert Function
    public void InsertShift()
    {
        try
        {
            DateTime ShiftstartTime = DateTime.Parse(txtSfStarttime.Text);
            DateTime ShiftendTime = DateTime.Parse(txtSfEndtime.Text);
            DateTime BrkstartTime = DateTime.Parse(txtBrStarttime.Text);
            DateTime BrkendTime = DateTime.Parse(txtBrEndtime.Text);

            if (BrkstartTime < ShiftstartTime || BrkstartTime > ShiftendTime)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break Start Time should be between Shift Start & End Time');", true);
                return;
            }
            if (BrkendTime < ShiftstartTime || BrkendTime > ShiftendTime)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Shift Start & End Time');", true);
                return;
            }
            if (BrkendTime < BrkstartTime)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Break Start Time & ShiftEnd Time');", true);
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new ShiftMasterClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = ddwnBranchName.SelectedValue,
                    shiftName = ddlShiftName.SelectedValue,
                    startTime = txtSfStarttime.Text,
                    endTime = txtSfEndtime.Text,
                    breakStartTime = txtBrStarttime.Text,
                    breakEndTime = txtBrEndtime.Text,
                    gracePeriod = txtGracePeriod.Text,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("shiftMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindShiftMaster();
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
    #region Update Function
    public void UpdateShift()
    {
        try
        {
            DateTime ShiftstartTime = DateTime.Parse(txtSfStarttime.Text);
            DateTime ShiftendTime = DateTime.Parse(txtSfEndtime.Text);
            DateTime BrkstartTime = DateTime.Parse(txtBrStarttime.Text);
            DateTime BrkendTime = DateTime.Parse(txtBrEndtime.Text);

            if (BrkstartTime < ShiftstartTime || BrkstartTime > ShiftendTime)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break Start Time should be between Shift Start & End Time');", true);
                return;
            }
            if (BrkendTime < ShiftstartTime || BrkendTime > ShiftendTime)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Shift Start & End Time');", true);
                return;
            }
            if (BrkendTime < BrkstartTime)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Break Start Time & ShiftEnd Time');", true);
                return;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new ShiftMasterClass()
                {
                    branchId = ddwnBranchName.SelectedValue.Trim(),
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    shiftName = ddlShiftName.SelectedValue,
                    startTime = txtSfStarttime.Text,
                    endTime = txtSfEndtime.Text,
                    breakStartTime = txtBrStarttime.Text,
                    breakEndTime = txtBrEndtime.Text,
                    gracePeriod = txtGracePeriod.Text,
                    activeStatus = "A",
                    shiftId = hfshiftId.Value,
                    updatedBy = Session["UserId"].ToString().Trim()

                };
                HttpResponseMessage response = client.PutAsJsonAsync("shiftMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmitText();
                        BindShiftMaster();
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
    #region Update Text
    public void btnUpdateText()
    {
        btnSubmit.Text = "Update";
    }
    #endregion
    #region Submit Text
    public void btnSubmitText()
    {
        btnSubmit.Text = "Submit";
    }
    #endregion
    #region Shift  Master Class
    public class ShiftMasterClass
    {
        public String parkingOwnerId { get; set; }
        public String activeStatus { get; set; }
        public String branchId { get; set; }
        public String shiftName { get; set; }
        public String shiftId { get; set; }
        public String startTime { get; set; }
        public String endTime { get; set; }
        public String breakStartTime { get; set; }
        public String breakEndTime { get; set; }
        public String gracePeriod { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }

    }
    #endregion
    #region Edit Click
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            spAddorEdit.InnerText = "Edit ";

            btnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvshiftName = (Label)gvrow.FindControl("lblgvshiftName");
            Label lblShiftId = (Label)gvrow.FindControl("lblgvshiftId");
            Label lblgvbranchId = (Label)gvrow.FindControl("lblgvbranchId");
            Label lblgvparkingOwnerId = (Label)gvrow.FindControl("lblgvparkingOwnerId");
            Label lblgvstartTime = (Label)gvrow.FindControl("lblgvstartTime");
            Label lblgvendTime = (Label)gvrow.FindControl("lblgvendTime");
            Label lblgvbreakStartTime = (Label)gvrow.FindControl("lblgvbreakStartTime");
            Label lblgvbreakEndTime = (Label)gvrow.FindControl("lblgvbreakEndTime");
            Label lblgvgracePeriod = (Label)gvrow.FindControl("lblgvgracePeriod");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvShiftmaster = gvShiftmaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.ToString() == "Active" ? "A" : "D";
            ViewState["shiftId"] = sgvShiftmaster.ToString().Trim();
            ddwnBranchName.SelectedValue = lblgvbranchId.Text;
            BindShiftName();
            ddlShiftName.SelectedValue = lblgvshiftName.Text;
            txtSfStarttime.Text = lblgvstartTime.Text;
            txtSfEndtime.Text = lblgvendTime.Text;
            txtBrStarttime.Text = lblgvbreakStartTime.Text;
            txtBrEndtime.Text = lblgvbreakEndTime.Text;
            txtGracePeriod.Text = lblgvgracePeriod.Text;
            hfshiftId.Value = lblShiftId.Text;
            ADD();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
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
                string sShiftId = gvShiftmaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "shiftMaster?shiftId=" + sShiftId
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindShiftMaster();
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

    #region break time validation
    protected void txtBrStarttime_TextChanged(object sender, EventArgs e)
    {
        DateTime ShiftstartTime = DateTime.Parse(txtSfStarttime.Text);
        DateTime ShiftendTime = DateTime.Parse(txtSfEndtime.Text);
        DateTime BrkstartTime = DateTime.Parse(txtBrStarttime.Text);
        DateTime BrkendTime = DateTime.Parse(txtBrEndtime.Text);

        if (BrkstartTime < ShiftstartTime || BrkstartTime > ShiftendTime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break Start Time should be between Shift Start & End Time');", true);
            return;
        }
        if (BrkendTime < ShiftstartTime || BrkendTime > ShiftendTime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Shift Start & End Time');", true);
            return;
        }
        if (BrkendTime < BrkstartTime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Break Start Time & ShiftEnd Time');", true);
            return;
        }

    }

    protected void txtBrEndtime_TextChanged(object sender, EventArgs e)
    {
        DateTime ShiftstartTime = DateTime.Parse(txtSfStarttime.Text);
        DateTime ShiftendTime = DateTime.Parse(txtSfEndtime.Text);
        DateTime BrkstartTime = DateTime.Parse(txtBrStarttime.Text);
        DateTime BrkendTime = DateTime.Parse(txtBrEndtime.Text);

        if (BrkstartTime < ShiftstartTime || BrkstartTime > ShiftendTime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break Start Time should be between Shift Start & End Time');", true);
            return;
        }
        if (BrkendTime < ShiftstartTime || BrkendTime > ShiftendTime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Shift Start & End Time');", true);
            return;
        }
        if (BrkendTime < BrkstartTime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Break End Time should be between Break Start Time & ShiftEnd Time');", true);
            return;
        }

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
                        var Option = lst1.Where(x => x.optionName == "shiftMaster" && x.MenuOptionAccessActiveStatus == "A")
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
                                btnAdd.Visible = true;
                            }
                            else
                            {
                                btnAdd.Visible = false;
                            }
                            if (View[0] == "True")
                            {
                                divGridView.Visible = true;
                                if (Edit[0] == "True")
                                {
                                    gvShiftmaster.Columns[9].Visible = true;
                                }
                                else
                                {
                                    gvShiftmaster.Columns[9].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvShiftmaster.Columns[10].Visible = true;
                                }
                                else
                                {
                                    gvShiftmaster.Columns[10].Visible = false;
                                }

                            }
                            else
                            {
                                divGridView.Visible = false;
                            }

                        }
                        else
                        {
                            divFomr.Visible = false;
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