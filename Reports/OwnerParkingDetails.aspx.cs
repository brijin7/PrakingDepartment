using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_OwnerParkingDetails : System.Web.UI.Page
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
            BindOwner();
        }
    }

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
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            ddlParkingName.DataSource = dt;
                            ddlParkingName.DataValueField = "parkingOwnerId";
                            ddlParkingName.DataTextField = "parkingName";
                            ddlParkingName.DataBind();
                          
                        }
                        else
                        {
                            ddlParkingName.DataBind();
                        }
                        ddlParkingName.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                       
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

    #region Bind Branch Admin
    public void BindBranchDetails()
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
                          + "branchMaster?activeStatus=A&parkingOwnerId=";
                sUrl += ddlParkingName.SelectedValue;
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
                            gvBranchList.DataSource = dt;
                            gvBranchList.DataBind();
                            divGv.Visible = true;                      
                        }
                        else
                        {
                            gvBranchList.DataBind();
                        }

                    }
                    else
                    {
                        gvBranchList.DataBind();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    #endregion
    #region Submit_Click/Insert
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BindBranchDetails();
    }
  
    #endregion
       

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlParkingName.ClearSelection();
        divGv.Visible = false;
    }
   
    #endregion
       
    #region Owner Master Class
    public class OwnerMaster
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string parkingOwnerId { get; set; }
        public string parkingName { get; set; }
        public string shortName { get; set; }
        public string founderName { get; set; }
        public string logoUrl { get; set; }
        public string websiteUrl { get; set; }
        public string gstNumber { get; set; }
        public string placeType { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
    }
    #endregion
}
