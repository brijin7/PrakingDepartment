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

public partial class Login_ChangePassword : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }

    }
    #endregion
    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new OldPassword()
                {

                    userId = Session["UserId"].ToString() == "" ? null : Session["UserId"].ToString(),
                    Oldpassword = txtOldPassword.Text.Trim(),
                    Newpassword = txtNewPassword.Text.Trim()

                };
                HttpResponseMessage response = client.PostAsJsonAsync("Changepassword", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        clear();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        txtOldPassword.Text = "";
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
    #region  Old Password Class
    public class OldPassword
    {
        public string userId { get; set; }
        public string Oldpassword { get; set; }
        public string Newpassword { get; set; }

    }

    #endregion
    #region Cancel Click
    protected void btnReset_Click(object sender, EventArgs e)
    {

        clear();
    }
    public void clear()
    {
        txtNewPassword.Text = "";
        txtOldPassword.Text = "";
    }
    #endregion
}