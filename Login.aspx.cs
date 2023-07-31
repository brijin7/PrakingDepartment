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

public partial class Login : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["BaseUrl"] = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].Trim();
            Session["ImageUrl"] = System.Configuration.ConfigurationManager.AppSettings["ImageUrl"].Trim();
            Session["SmsUrl"] = System.Configuration.ConfigurationManager.AppSettings["SmsUrl"].Trim();

            txtFgUserName.Text = string.Empty;
            txtOTP.Text = string.Empty;
            txtOtpUsername.Text = string.Empty;
            txtLgnPassword.Text = string.Empty;
            txtLgnUserName.Text = string.Empty;
        }
    }
    #endregion

    #region Login Click
    protected void btnLogin_Click(object sender, EventArgs e)
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
                          + "login?user=" + txtLgnUserName.Text + "&password=" + txtLgnPassword.Text + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        Session["UserId"] = dt.Rows[0]["userId"].ToString();
                        Session["UserRole"] = dt.Rows[0]["userRole"].ToString();
                        Session["UserName"] = dt.Rows[0]["userName"].ToString();
                        Session["Branch"] = "0";
                        Session["ShowPage"] = "0";
                        Session["Bookingnav"] = "";
                       
                        if (Session["UserRole"].ToString().Trim() == "SA")
                        {
                            Session["parkingOwnerId"] = "0";
                            Response.Redirect("~/DashBoard.aspx", false);
                        }
                        else if (Session["UserRole"].ToString().Trim() == "A")
                        {
                            GetOwnerID();
                            Response.Redirect("~/Login/BranchLogin.aspx",false);
                        }
                        else if (Session["UserRole"].ToString().Trim() == "E")
                        {
                            Session["ShowPage"] = "1";
                            Session["branchId"] = dt.Rows[0]["branchId"].ToString();
                            Response.Redirect("~/DashBoard.aspx", false);
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

    #region Get ParkingOwnerID
    public void GetOwnerID()
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
                          + "parkingOwnerMaster?activeStatus=A&userId=";
                sUrl += Session["UserId"].ToString();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        Session["parkingName"] = dt.Rows[0]["parkingName"].ToString();
                        Session["parkingOwnerId"] = dt.Rows[0]["parkingOwnerId"].ToString();
                    }
                    else
                    {

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

    #region Forgot Pwd Click
    protected void lnkbtnFgPwd_Click(object sender, EventArgs e)
    {
        divLogin.Visible = false;
        divFgPwd.Visible = true;
    }
    #endregion

    #region Back to login Click
    protected void lnkbtnBackToLogin_Click(object sender, EventArgs e)
    {
        divLogin.Visible = true;
        divFgPwd.Visible = false;
        divCfmOtp.Visible = false;
    }
    #endregion

    #region Send / Re Send OTP
    protected void btnSend_Click(object sender, EventArgs e)
    {
        sendOtP(txtFgUserName.Text);
    }
    protected void btnResend_Click(object sender, EventArgs e)
    {
        btnCfmOtp.Visible = true;
        sendOtP(txtOtpUsername.Text);
    }
    public void sendOtP(string Username)
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
                    username = Username.Trim()
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
                        ViewState["OTPUserName"] = Username.Trim();
                        txtFgUserName.Text = string.Empty;
                        txtOtpUsername.Text = ViewState["OTPUserName"].ToString();
                        divCfmOtp.Visible = true;
                        divFgPwd.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "SendOtp", "SendOtp();", true);
                        btnResend.Visible = true;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    #endregion

    #region Confirm OTP
    protected void btnCfmOtp_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtOTP.Text == hfOtp.Value)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var update = new LoginClass()
                    {
                        username = txtOtpUsername.Text,
                        password = txtNewPassword.Text
                    };
                    HttpResponseMessage response = client.PutAsJsonAsync("forgotpassword", update).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                        int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                        string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                        if (StatusCode == 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            divCfmOtp.Visible = false;
                            divFgPwd.Visible = false;
                            divLogin.Visible = true;
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            hfOtp.Value = string.Empty;
                            ViewState["OTPUserName"] = string.Empty;
                            txtFgUserName.Text = string.Empty;
                            txtOtpUsername.Text = string.Empty;
                            txtOTP.Text = string.Empty;
                            divCfmOtp.Visible = false;
                            divFgPwd.Visible = true;
                        }
                    }
                }
            }
            else
            {
                txtOTP.Text = string.Empty;
                txtNewPassword.Text = string.Empty;
                btnCfmOtp.Visible = false;
                btnResend.Text = "Resend";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Check the OTP sent / Time Out / Click Resend');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    #endregion

    #region Login Class
    public class LoginClass
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    #endregion
}
