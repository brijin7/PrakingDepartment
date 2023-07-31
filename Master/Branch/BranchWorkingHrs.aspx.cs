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

public partial class Master_Branch_BranchWorkingHrs : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
              Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }


        if (!IsPostBack)
        {

            BindBranchWorkingHrs();
            rbtnisHoliday.SelectedValue = "false";
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion
    #region Cancel Click Fucntion
    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        btnSubmitText();
        rbtnisHoliday.ClearSelection();
        txtfrmtime.Text = "";
        txttotime.Text = "";
        spAddorEdit.InnerText = "";
        lblWorkingDaysValue.Text = "";
        btnMon.Style.Clear();
        btnTue.Style.Clear();
        btnWed.Style.Clear();
        btnThu.Style.Clear();
        btnFri.Style.Clear();
        btnSat.Style.Clear();
        btnSun.Style.Clear();
    }
    #endregion
    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
    }
    #endregion
    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        ADD();
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
            InsertBranchWorkingHrs();
        }
        else
        {
            UpdateBranchWorkingHrs();
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
    #region Bind Branch Working Hours
    public void BindBranchWorkingHrs()
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
                        + "branchWorkingHrs?branchId=";
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
                            gvbranchworkinghrs.DataSource = dt;
                            gvbranchworkinghrs.DataBind();
                        }
                        else
                        { 
                            spAddorEdit.InnerText = "Add ";
                            ADD();
                            gvbranchworkinghrs.DataSource = null;
                            gvbranchworkinghrs.DataBind();
                        }
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        ADD();
                      //  ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region Working Days Click
    protected void btnSun_Click(object sender, EventArgs e)
    {
        btnSun.Style.Add("background-color", "green"); 
        lblWorkingDaysValue.Text = "Sunday";
        DivHrDetails.Visible = true;
        btnMon.Style.Clear();
        btnTue.Style.Clear();
        btnWed.Style.Clear();
        btnThu.Style.Clear();
        btnFri.Style.Clear();
        btnSat.Style.Clear();
    }
    protected void btnMon_Click(object sender, EventArgs e)
    {
        lblWorkingDaysValue.Text = "Monday";
        DivHrDetails.Visible = true;
        btnMon.Style.Add("background-color", "green");
        btnTue.Style.Clear();
        btnWed.Style.Clear();
        btnThu.Style.Clear();
        btnFri.Style.Clear();
        btnSat.Style.Clear();
        btnSun.Style.Clear();
    }

    protected void btnTue_Click(object sender, EventArgs e)
    {
        lblWorkingDaysValue.Text = "Tuesday";
        DivHrDetails.Visible = true;
        btnTue.Style.Add("background-color", "green");
        btnMon.Style.Clear();
        btnWed.Style.Clear();
        btnThu.Style.Clear();
        btnFri.Style.Clear();
        btnSat.Style.Clear();
        btnSun.Style.Clear();
    }

    protected void btnWed_Click(object sender, EventArgs e)
    {
        lblWorkingDaysValue.Text = "Wednesday";
        DivHrDetails.Visible = true;
        btnWed.Style.Add("background-color", "green");
        btnMon.Style.Clear();
        btnTue.Style.Clear();
        btnThu.Style.Clear();
        btnFri.Style.Clear();
        btnSat.Style.Clear();
        btnSun.Style.Clear();
    }

    protected void btnThu_Click(object sender, EventArgs e)
    {
        lblWorkingDaysValue.Text = "Thursday";
        DivHrDetails.Visible = true;
        btnThu.Style.Add("background-color", "green");
        btnMon.Style.Clear();
        btnTue.Style.Clear();
        btnWed.Style.Clear();
        btnFri.Style.Clear();
        btnSat.Style.Clear();
        btnSun.Style.Clear();
    }

    protected void btnFri_Click(object sender, EventArgs e)
    {
        lblWorkingDaysValue.Text = "Friday";
        DivHrDetails.Visible = true;
        btnFri.Style.Add("background-color", "green");
        btnMon.Style.Clear();
        btnTue.Style.Clear();
        btnWed.Style.Clear();
        btnThu.Style.Clear();
        btnSat.Style.Clear();
        btnSun.Style.Clear();
    }

    protected void btnSat_Click(object sender, EventArgs e)
    {
        lblWorkingDaysValue.Text = "Saturday";
        DivHrDetails.Visible = true;
        btnSat.Style.Add("background-color", "green");
        btnTue.Style.Clear();
        btnWed.Style.Clear();
        btnThu.Style.Clear();
        btnFri.Style.Clear();
        btnSun.Style.Clear();
    }

    #endregion
    #region Insert Function
    public void InsertBranchWorkingHrs()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new BranchWorkingHrsClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    workingDay = lblWorkingDaysValue.Text,
                    isHoliday = rbtnisHoliday.SelectedValue,
                    fromTime = txtfrmtime.Text,
                    toTime = txttotime.Text,
                    createdBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("branchWorkingHrs", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindBranchWorkingHrs();
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
    public void UpdateBranchWorkingHrs()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new BranchWorkingHrsClass()
                {
                    workingDay = lblWorkingDaysValue.Text,
                    isHoliday = rbtnisHoliday.SelectedValue,
                    fromTime = txtfrmtime.Text,
                    toTime = txttotime.Text,
                    uniqueId = hfuniqueId.Value,
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    updatedBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("branchWorkingHrs", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmitText();
                        BindBranchWorkingHrs();
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
    #region Branch Working Hours  Class
    public class BranchWorkingHrsClass
    {
        public string parkingOwnerId { get; set; }
        public string uniqueId { get; set; }
        public string branchId { get; set; }
        public string isHoliday { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string workingDay { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }

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
            Label lblgvworkingDay = (Label)gvrow.FindControl("lblgvworkingDay");
            Label lblgvfromTime = (Label)gvrow.FindControl("lblgvfromTime");
            Label lblgvtoTime = (Label)gvrow.FindControl("lblgvtoTime");
            Label lblgvisHoliday = (Label)gvrow.FindControl("lblgvisHoliday");
            Label lblgvuniqueId = (Label)gvrow.FindControl("lblgvuniqueId");
            string sgvPrintingInstructions = gvbranchworkinghrs.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["uniqueId"] = sgvPrintingInstructions.ToString().Trim();
            lblWorkingDaysValue.Text = lblgvworkingDay.Text;
            txtfrmtime.Text = lblgvfromTime.Text;
            txttotime.Text = lblgvtoTime.Text;
            if (lblgvisHoliday.Text == "True")
            {
                rbtnisHoliday.SelectedValue = "true";
            }
            else
            {
                rbtnisHoliday.SelectedValue = "false";
            }
            switch (lblWorkingDaysValue.Text)
            {
                case "Monday":
                    DivHrDetails.Visible = true;
                    btnMon.Style.Add("background-color", "green");
                    btnTue.Style.Clear();
                    btnWed.Style.Clear();
                    btnThu.Style.Clear();
                    btnFri.Style.Clear();
                    btnSat.Style.Clear();
                    btnSun.Style.Clear();
                    break;
                case "Tuesday":

                    DivHrDetails.Visible = true;
                    btnTue.Style.Add("background-color", "green");
                    btnMon.Style.Clear();
                    btnWed.Style.Clear();
                    btnThu.Style.Clear();
                    btnFri.Style.Clear();
                    btnSat.Style.Clear();
                    btnSun.Style.Clear();
                    break;
                case "Wednesday":
                    DivHrDetails.Visible = true;
                    btnWed.Style.Add("background-color", "green");
                    btnMon.Style.Clear();
                    btnTue.Style.Clear();
                    btnThu.Style.Clear();
                    btnFri.Style.Clear();
                    btnSat.Style.Clear();
                    btnSun.Style.Clear();

                    break;
                case "Thursday":
                    DivHrDetails.Visible = true;
                    btnThu.Style.Add("background-color", "green");
                    btnMon.Style.Clear();
                    btnTue.Style.Clear();
                    btnWed.Style.Clear();
                    btnFri.Style.Clear();
                    btnSat.Style.Clear();
                    btnSun.Style.Clear();
                    break;
                case "Friday":
                    DivHrDetails.Visible = true;
                    btnFri.Style.Add("background-color", "green");
                    btnMon.Style.Clear();
                    btnTue.Style.Clear();
                    btnWed.Style.Clear();
                    btnThu.Style.Clear();
                    btnSat.Style.Clear();
                    btnSun.Style.Clear();
                    break;
                case "Saturday":
                    DivHrDetails.Visible = true;
                    btnSat.Style.Add("background-color", "green");
                    btnTue.Style.Clear();
                    btnWed.Style.Clear();
                    btnThu.Style.Clear();
                    btnFri.Style.Clear();
                    btnSun.Style.Clear();
                    break;
                default:
                    btnSun.Style.Add("background-color", "green");
                    DivHrDetails.Visible = true;
                    btnMon.Style.Clear();
                    btnTue.Style.Clear();
                    btnWed.Style.Clear();
                    btnThu.Style.Clear();
                    btnFri.Style.Clear();
                    btnSat.Style.Clear();
                    break;
            }

            hfuniqueId.Value = lblgvuniqueId.Text;
            ADD();

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
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
                        var Option = lst1.Where(x => x.optionName == "workingHours" && x.MenuOptionAccessActiveStatus == "A")
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
                                    gvbranchworkinghrs.Columns[8].Visible = true;
                                }
                                else
                                {
                                    gvbranchworkinghrs.Columns[8].Visible = false;
                                }

                            }
                            else
                            {
                                divGridView.Visible = false;
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