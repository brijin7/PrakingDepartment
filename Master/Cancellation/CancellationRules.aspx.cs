using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_CancellationRules : System.Web.UI.Page
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
            BindGvCancellationRules();
        }
    }
    #endregion

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        divGv.Visible = false;
        divForm.Visible = true;
        ddltype.Enabled = true;
    }

    #endregion

    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    public void Cancel()
    {
        spAddorEdit.InnerText = "";
        divGv.Visible = true;
        divForm.Visible = false;
        ddltype.ClearSelection();
        txtDuration.Text = string.Empty;
        txtperuser.Text = string.Empty;
        txtcancellationCharges.Text = string.Empty;
        btnSubmit.Text = "Submit";
    }
    #endregion

    #region Bind GridView
    public void BindGvCancellationRules()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "cancellationRules";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        gvCancellationRules.DataSource = dt;
                        gvCancellationRules.DataBind();
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        divGv.Visible = false;
                        divForm.Visible = true;
                        ddltype.Enabled = true;
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

    #region Submit / Insert 
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertCancellationRules();
        }
        else
        {
            UpdateCancellationRules();
        }
    }
    public void InsertCancellationRules()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new CancellationRules()
                {
                    type = ddltype.SelectedValue,
                    duration = txtDuration.Text.Trim(),
                    noOfTimesPerUser = txtperuser.Text.Trim(),
                    cancellationCharges = txtcancellationCharges.Text.Trim(),
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("cancellationRules", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindGvCancellationRules();
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

    #region Edit & Update
    protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            spAddorEdit.InnerText = "Edit ";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblGvUniqueId = (Label)gvrow.FindControl("lblGvUniqueId");
            Label lblGvType = (Label)gvrow.FindControl("lblGvType");
            Label lblGvDuration = (Label)gvrow.FindControl("lblGvDuration");
            Label lblGvNoOfTimes = (Label)gvrow.FindControl("lblGvNoOfTimes");
            Label lblGvCancellationCharges = (Label)gvrow.FindControl("lblGvCancellationCharges");

            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvCancellationRules = gvCancellationRules.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.ToString() == "Active" ? "A" : "D";
            ViewState["uniqueId"] = sgvCancellationRules.ToString().Trim();
            ddltype.SelectedValue = lblGvType.Text == "Day" ? "D" : "M";
            txtDuration.Text = lblGvDuration.Text;
            txtperuser.Text = lblGvNoOfTimes.Text;
            txtcancellationCharges.Text = lblGvCancellationCharges.Text;

            divGv.Visible = false;
            divForm.Visible = true;
            btnSubmit.Text = "Update";
            ddltype.Enabled = false;
           
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void UpdateCancellationRules()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new CancellationRules()
                {
                    uniqueId = ViewState["uniqueId"].ToString(),
                    type = ddltype.SelectedValue,
                    duration = txtDuration.Text.Trim(),
                    noOfTimesPerUser = txtperuser.Text.Trim(),
                    cancellationCharges = txtcancellationCharges.Text.Trim(),
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("cancellationRules", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmit.Text = "Submit";
                        BindGvCancellationRules();
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

    #region Delete
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sUniqueId = gvCancellationRules.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "cancellationRules?uniqueId=" + sUniqueId
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
                        BindGvCancellationRules();
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

    public class CancellationRules
    {
        public string uniqueId { get; set; }
        public string type { get; set; }
        public string duration { get; set; }
        public string noOfTimesPerUser { get; set; }
        public string cancellationCharges { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
    }
}