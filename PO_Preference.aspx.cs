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

public partial class PO_Preference : System.Web.UI.Page
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
            BindFloorType();
            GetParkingOwnerConfig();
            if (Session["UserRole"].ToString() == "SA")
            {
                btnEdit.Visible = true;
                DiVContactSAdmin.Visible = false;
            }
            else
            {
                btnEdit.Visible = false;
                DiVContactSAdmin.Visible = true;
            }
        }
    }
    #endregion
    #region Insert Function
    public void InsertPO()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new POClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    blockOption = rbtnblockOption.SelectedValue,
                    floorOption = rbtnfloorOption.SelectedValue,
                    squareFeet = "0",
                    floorType = "0",
                    employeeOption = "N",
                    slotsOption = rbtnslotsOption.SelectedValue,
                    createdBy = Session["UserId"].ToString().Trim()
                };
                if (rbtnfloorOption.SelectedValue == "N")
                {
                    Insert.floorType = ddllFloorType.SelectedValue;
                    Insert.squareFeet = txtsqft.Text;
                }
                HttpResponseMessage response = client.PostAsJsonAsync("parkingOwnerConfig", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        GetParkingOwnerConfig();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        if (ResponseMsg.ToString() == "Data Already Exists")
                        {
                            GetParkingOwnerConfig();
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
    #region FloorType
    public void BindFloorType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddllFloorType.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
              + "configMaster?activestatus=A&configTypename=floorType";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtFloorType = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddllFloorType.DataSource = dtFloorType;
                        ddllFloorType.DataValueField = "configId";
                        ddllFloorType.DataTextField = "configName";
                        ddllFloorType.DataBind();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    //ddllFloorType.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertPO();
        }
        else
        {
            UpdatePO();
        }

    }
    #endregion
    #region PO  Class
    public class POClass
    {
        public String parkingOwnerId { get; set; }
        public String branchId { get; set; }
        public String blockOption { get; set; }
        public String floorOption { get; set; }
        public String squareFeet { get; set; }
        public String floorType { get; set; }
        public String employeeOption { get; set; }
        public String slotsOption { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }
        public String parkingOwnerConfigId { get; set; }


    }
    #endregion
    #region Get Parking Owner Config
    public void GetParkingOwnerConfig()
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
                          + "parkingOwnerConfig?branchId=";
                sUrl += Session["branchId"].ToString();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        ViewState["Flag"] = "1";
                        DivFirst.Visible = false;
                        DivFinal.Visible = true;
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ViewState["blockOption"] = dt.Rows[0]["blockOption"].ToString();
                        ViewState["floorOption"] = dt.Rows[0]["floorOption"].ToString();
                        ViewState["employeeOption"] = dt.Rows[0]["employeeOption"].ToString();
                        ViewState["slotsOption"] = dt.Rows[0]["slotsOption"].ToString();
                        ConditionCheck();
                    }
                    else
                    {
                        ViewState["Flag"] = "0";
                        DivFirst.Visible = true;
                        DivFinal.Visible = false;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    #endregion
    #region Condition Check
    public void ConditionCheck()
    {
        DivFinal.Visible = true;
        DivFirst.Visible = false;
        if (ViewState["blockOption"].ToString() == "Y")
        {
            lblBlock.Text = "Yes";
        }
        else
        {
            lblBlock.Text = "No";
        }
        if (ViewState["floorOption"].ToString() == "Y")
        {
            lblFloor.Text = "Yes";
        }
        else
        {
            lblFloor.Text = "No";
        }
        //if (ViewState["employeeOption"].ToString() == "Y") 
        //{
        //    lblEmp.Text = "Yes";
        //}
        //else
        //{
        //    lblEmp.Text = "No";
        //}
        if (ViewState["slotsOption"].ToString() == "Y")
        {
            lblslot.Text = "Yes";
        }
        else
        {
            lblslot.Text = "No";
        }


    }

    #endregion
    #region Back Click
    protected void btnback_Click(object sender, EventArgs e)
    {
        if (ViewState["Flag"].ToString() == "0")
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
        else
        {
            Response.Redirect("~/DashBoard.aspx", false);
        }

    }
    #endregion
    #region Edit Click
    protected void btnEdit_Click(object sender, EventArgs e)
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
                          + "parkingOwnerConfig?branchId=";
                sUrl += Session["branchId"].ToString();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {

                        ViewState["Flag"] = "1";
                        lnkBtnBack.Visible = true;
                        DivFirst.Visible = true;
                        DivFinal.Visible = false;
                        divfloor.Style.Add("display", "block");
                        divOtpDetails.Visible = true;
                        divSendOtp.Visible = true;
                        divEnterOtp.Visible = false;
                        divResend.Visible = false;
                        divSlot.Style.Add("display", "block");
                        Divbtn.Style.Add("display", "none");
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ViewState["blockOption"] = dt.Rows[0]["blockOption"].ToString();
                        ViewState["floorOption"] = dt.Rows[0]["floorOption"].ToString();
                        ViewState["employeeOption"] = dt.Rows[0]["employeeOption"].ToString();
                        ViewState["slotsOption"] = dt.Rows[0]["slotsOption"].ToString();

                        rbtnblockOption.SelectedValue = dt.Rows[0]["blockOption"].ToString();
                        rbtnfloorOption.SelectedValue = dt.Rows[0]["floorOption"].ToString();
                        if (rbtnfloorOption.SelectedValue == "N")
                        {
                            DivfloorDetails.Style.Add("display", "block");
                            ddllFloorType.SelectedValue = dt.Rows[0]["floorType"].ToString();
                            txtsqft.Text = dt.Rows[0]["squareFeet"].ToString();
                        }

                        rbtnslotsOption.SelectedValue = dt.Rows[0]["slotsOption"].ToString();
                        ViewState["parkingOwnerConfigId"] = dt.Rows[0]["parkingOwnerConfigId"].ToString();
                        btnSubmit.Text = "Update";
                        GetBranchMobileNo();


                    }
                    else
                    {
                        ViewState["Flag"] = "0";
                        lnkBtnBack.Visible = false;
                        DivFirst.Visible = true;
                        DivFinal.Visible = false;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    #endregion
    #region Update Function
    public void UpdatePO()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new POClass()
                {
                    parkingOwnerConfigId = ViewState["parkingOwnerConfigId"].ToString(),
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    blockOption = rbtnblockOption.SelectedValue,
                    floorOption = rbtnfloorOption.SelectedValue,
                    squareFeet = "0",
                    floorType = "0",
                    employeeOption = "N",
                    slotsOption = rbtnslotsOption.SelectedValue,
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                if (rbtnfloorOption.SelectedValue == "N")
                {
                    Update.floorType = ddllFloorType.SelectedValue;
                    Update.squareFeet = txtsqft.Text;
                }
                HttpResponseMessage response = client.PutAsJsonAsync("parkingOwnerConfig", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        GetParkingOwnerConfig();
                        btnSubmit.Text = "Submit";
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        if (ResponseMsg.ToString() == "Data Already Exists")
                        {
                            GetParkingOwnerConfig();
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
    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lnkBtnBack.Visible = false;
        if (ViewState["Flag"].ToString() == "0")
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
        else
        {
            Response.Redirect("~/DashBoard.aspx", false);
        }
    }
    #endregion

    #region OTP Details
    protected void btnCfmOtp_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtOTP.Text == ViewState["OTP"].ToString().Trim())
            {
                Divbtn.Style.Add("display", "block");
                txtOTP.Text = string.Empty;
                divOtpDetails.Visible = false;
                divResend.Visible = false;
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
            btnResend.Text = "00:29";
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new LoginClass()
                {
                    username = ViewState["phoneNumber"].ToString().Trim()
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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "erroralert('" + ex + "');", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.Message.ToString().Trim() + "');", true);
        }

    }
    public void GetBranchMobileNo()
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
                          + "branchMaster?activeStatus=A&approvalStatus=A&branchId=";
                sUrl += Session["branchId"].ToString();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ViewState["phoneNumber"] = dt.Rows[0]["phoneNumber"].ToString();
                        txtMobileNo.Text = ViewState["phoneNumber"].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    public class LoginClass
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    #endregion

}