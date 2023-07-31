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

public partial class Master_ParkingPassConfiguration : System.Web.UI.Page
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
            BindTaxName();
            BindVehicle();
            BindParkingPass();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion
    #region Bind Tax 
    public void BindTaxName()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "taxMaster?activeStatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        dt.Columns.Add(new DataColumn("tax", System.Type.GetType("System.String"), "taxName + '~' + taxPercentage"));
                        if (dt.Rows.Count > 0)
                        {
                            ddltax.DataSource = dt;
                            ddltax.DataValueField = "taxId";
                            ddltax.DataTextField = "tax";
                            ddltax.DataBind();
                        }
                        else
                        {
                            ddltax.DataBind();
                        }
                        ddltax.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
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
                          + "vehicleConfigMaster?activeStatus=A";
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
                            ddlvehicleType.DataSource = dt;
                            ddlvehicleType.DataValueField = "vehicleConfigId";
                            ddlvehicleType.DataTextField = "vehicleName";
                            ddlvehicleType.DataBind();
                        }
                        else
                        {
                            ddlvehicleType.DataBind();
                        }
                        ddlvehicleType.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    var Errorresponse = response.Content.ReadAsStringAsync().Result;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + Errorresponse.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Bind Parking Pass config details
    public void BindParkingPass()
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
                        + "parkingPassConfig?branchId=";
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
                            gvparkingpass.DataSource = dt;
                            gvparkingpass.DataBind();
                        }
                        else
                        {
                            gvparkingpass.DataSource = null;
                            gvparkingpass.DataBind();
                        }
                    }
                    else
                    {
                        Cancel();
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
        Cancel();
        ADD();
    }
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
        spAddorEdit.InnerText = "Add ";
    }
    #endregion
    #region Submit Text
    public void btnSubmitText()
    {
        btnSubmit.Text = "Submit";
    }
    #endregion      
    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertParkingPass();
        }
        else
        {
            UpdateParkingPass();
        }
    }

    #endregion
    #region Insert Function
    public void InsertParkingPass()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new ParkingPassClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    passCategory = ddlpassCategory.SelectedValue,
                    totalAmount = txtTotalAmount.Text,
                    taxId = ddltax.SelectedValue,
                    vehicleType = ddlvehicleType.SelectedValue,
                    remarks = txtremarks.Text,
                    passType = ddlpasstype.SelectedValue,
                    noOfDays = txtnoofdays.Text,
                    parkingLimit = txtparkingLimit.Text,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("parkingPassConfig", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindParkingPass();
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
    #region Update Function
    public void UpdateParkingPass()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new ParkingPassClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    passCategory = ddlpassCategory.SelectedValue,
                    parkingPassConfigId = hfparkingPassConfigId.Value,
                    passType = ddlpasstype.SelectedValue,
                    noOfDays = txtnoofdays.Text,
                    parkingLimit = txtparkingLimit.Text,
                    totalAmount = txtTotalAmount.Text,
                    taxId = ddltax.SelectedValue,
                    vehicleType = ddlvehicleType.SelectedValue,
                    remarks = txtremarks.Text,
                    activeStatus = "A",
                    updatedBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("parkingPassConfig", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmitText();
                        BindParkingPass();
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

    #region Edit Click 
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            btnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvparkingPassConfigId = (Label)gvrow.FindControl("lblgvparkingPassConfigId");
            Label lblgvpassType = (Label)gvrow.FindControl("lblgvpassType");
            Label lblgvpassCategory = (Label)gvrow.FindControl("lblgvpassCategory");
            Label lblgvnoOfDays = (Label)gvrow.FindControl("lblgvnoOfDays");
            Label lblgvparkingLimit = (Label)gvrow.FindControl("lblgvparkingLimit");
            Label lblgvvehicleType = (Label)gvrow.FindControl("lblgvvehicleType");
            Label lblgvremarks = (Label)gvrow.FindControl("lblgvremarks");
            Label lblgvtaxId = (Label)gvrow.FindControl("lblgvtaxId");
            Label lblgvtax = (Label)gvrow.FindControl("lblgvtax");
            Label lblgvamount = (Label)gvrow.FindControl("lblgvamount");
            Label lblgvTotalamount = (Label)gvrow.FindControl("lblgvtotalAmount");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvParkingPass = gvparkingpass.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.ToString() == "Active" ? "A" : "D";
            ViewState["parkingPassConfigId"] = sgvParkingPass.ToString().Trim();

            ddlpassCategory.SelectedValue = lblgvpassCategory.Text == "VIP" ? "V" : "N";
            ddlpassCategory.Enabled = false;

            ddlpasstype.SelectedValue = lblgvpassType.Text;
            ddlpasstype.Enabled = false;

            txtnoofdays.Text = lblgvnoOfDays.Text;
            txtparkingLimit.Text = lblgvparkingLimit.Text;

            ddlvehicleType.SelectedValue = lblgvvehicleType.Text;
            ddlvehicleType.Enabled = false;

            ddltax.SelectedValue = lblgvtaxId.Text;
            txtTaxAmount.Text = lblgvtax.Text;
            txtAmount.Text = lblgvamount.Text;
            txtTotalAmount.Text = lblgvTotalamount.Text;
            txtremarks.Text = lblgvremarks.Text;
            hfparkingPassConfigId.Value = lblgvparkingPassConfigId.Text;
            ADD();
            spAddorEdit.InnerText = "Edit ";
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excp = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excp + "');", true);
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
                string sPassConfigId = gvparkingpass.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "parkingPassConfig?parkingPassConfigId=" + sPassConfigId
                            + "&activestatus="
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
                        BindParkingPass();
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

    #region Amount Cal  
    protected void txtTotalAmount_TextChanged(object sender, EventArgs e)
    {
        AmountCalc();
    }
    protected void ddltax_SelectedIndexChanged(object sender, EventArgs e)
    {
        AmountCalc();
    }
    public void AmountCalc()
    {
        try
        {
            if (txtTotalAmount.Text != null && txtTotalAmount.Text != "" && Convert.ToInt32(ddltax.SelectedValue) != 0)
            {
                string[] tax = ddltax.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                int totalAmount = Convert.ToInt32(txtTotalAmount.Text);
                double TaxPercent = Convert.ToDouble(taxPt.Trim());

                string Tax = (Math.Round(totalAmount * TaxPercent / 100)).ToString();
                string Amount = (Math.Round(totalAmount - Convert.ToDecimal(Tax))).ToString();
                txtAmount.Text = Amount;
                txtTaxAmount.Text = Tax;
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        btnSubmitText();
        ddlpassCategory.ClearSelection();
        ddlpasstype.ClearSelection();
        ddltax.ClearSelection();
        ddlvehicleType.ClearSelection();
        txtTotalAmount.Text = "";
        txtnoofdays.Text = "";
        txtparkingLimit.Text = "";
        txtremarks.Text = "";
        txtAmount.Text = "";
        txtTaxAmount.Text = "";
        ddlpassCategory.Enabled = true;
        ddlpasstype.Enabled = true;
        ddlvehicleType.Enabled = true;
    }
    #endregion

    #region Parking Pass  Class
    public class ParkingPassClass
    {
        public string parkingOwnerId { get; set; }
        public string activeStatus { get; set; }
        public string branchId { get; set; }
        public string parkingPassConfigId { get; set; }
        public string passCategory { get; set; }
        public string passType { get; set; }
        public string noOfDays { get; set; }
        public string parkingLimit { get; set; }
        public string totalAmount { get; set; }
        public string tax { get; set; }
        public string taxId { get; set; }
        public string vehicleType { get; set; }
        public string remarks { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
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
                        var Option = lst1.Where(x => x.optionName == "passConfig" && x.MenuOptionAccessActiveStatus == "A")
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
                                    gvparkingpass.Columns[16].Visible = true;
                                }
                                else
                                {
                                    gvparkingpass.Columns[16].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvparkingpass.Columns[17].Visible = true;
                                }
                                else
                                {
                                    gvparkingpass.Columns[17].Visible = false;
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