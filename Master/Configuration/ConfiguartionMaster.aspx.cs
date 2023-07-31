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


public partial class Master_ConfiguartionMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
              Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (!IsPostBack)
        {
            GetConfigurationType();
            GetConfigurationMaster();
        }
    }

    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Bind Configuration Type in Drop Down
    /// </summary>
    public void GetConfigurationType()
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
                           + "configType?activestatus=A";

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

                            ddlConfigType.DataSource = dt;
                            ddlConfigType.DataValueField = "configTypeId";
                            ddlConfigType.DataTextField = "typeName";
                            ddlConfigType.DataBind();

                        }
                        else
                        {

                            ddlConfigType.DataBind();
                        }
                        ddlConfigType.Items.Insert(0, new ListItem("Select", "0"));
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
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Bind Configuration Type in Drop Down
    /// </summary>
    public void GetConfigurationMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("configMaster").Result;
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
                            gvConfigMaster.DataSource = dt;
                            gvConfigMaster.DataBind();
                        }
                        else
                        {
                            gvConfigMaster.DataSource = null;
                            gvConfigMaster.DataBind();
                        }
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        ADD();
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Submit click event insert process
    /// </summary>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertConfigMaster();
        }
        else
        {
            UpdateConfigMaster();
        }

    }
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for :  Crud operation for Configuration Master
    /// </summary>
    public void InsertConfigMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new ConfigMasterclass()
                {
                    configTypeId = ddlConfigType.SelectedValue.Trim(),
                    configName = txtConfigName.Text,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("configMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        GetConfigurationMaster();
                        Clear();
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
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Class for ConfigMaster
    /// </summary>
    public class ConfigMasterclass
    {
        public String configTypeId { get; set; }
        public String configName { get; set; }
        public String configId { get; set; }
        public String activeStatus { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }

    }
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Clear Inputs
    /// </summary>
    public void Clear()
    {
        spAddorEdit.InnerText = "";
        divGv.Visible = true;
        divFomr.Visible = false;
        ddlConfigType.SelectedIndex = 0;
        txtConfigName.Text = string.Empty;
        btnSubmit.Text = "Submit";
        ddlConfigType.Enabled = true;
    }
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Edit that particular Row
    /// </summary>
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            spAddorEdit.InnerText = "Edit ";
            btnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblconfigName = (Label)gvrow.FindControl("lblconfigName");
            Label lblconfigId = (Label)gvrow.FindControl("lblconfigId");
            Label lblconfigTypeName = (Label)gvrow.FindControl("lblconfigTypeName");
            Label lblconfigTypeId = (Label)gvrow.FindControl("lblconfigTypeId");
            LinkButton lblActiveStatus = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sConfigTypeId = gvConfigMaster.DataKeys[gvrow.RowIndex].Value.ToString();

            ViewState["configId"] = sConfigTypeId.ToString().Trim();
            ViewState["ActiveStatus"] = lblActiveStatus.Text.ToString() == "Active" ? "A" : "D";
            ddlConfigType.SelectedValue = lblconfigTypeId.Text;
            ddlConfigType.Enabled = false;
            txtConfigName.Text = lblconfigName.Text;
            ADD();
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }
    }
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Change the Text to Update
    /// </summary>
    public void btnUpdateText()
    {
        btnSubmit.Text = "Update";
    }
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for :  Crud operation of Update for Configuration Master
    /// </summary>
    public void UpdateConfigMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new ConfigMasterclass()
                {

                    configTypeId = ddlConfigType.SelectedValue.Trim(),
                    configId = ViewState["configId"].ToString().Trim(),
                    configName = txtConfigName.Text,
                    activeStatus = ViewState["ActiveStatus"].ToString().Trim(),
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("configMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        GetConfigurationMaster();
                        Clear();
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
    /// <summary>
    /// Created By : Imran 
    /// Created Date : 2022-06-01
    /// Created for : Active or Inactive that Particular Row Data
    /// </summary>
    protected void lnkActiveorInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sConfigId = gvConfigMaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "configMaster?configId=" + sConfigId
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
                        GetConfigurationMaster();
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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Clear();
    }

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
}
