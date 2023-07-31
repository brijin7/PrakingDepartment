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


public partial class Master_TaxMaster : System.Web.UI.Page
{
    IFormatProvider objEnglishDate = new System.Globalization.CultureInfo("en-GB", true);

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        txttodate.Attributes.Add("Readonly", "Readonly");
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (!IsPostBack)
        {
            BindServicemName();
            BindTaxMaster();
        }
    }
    #endregion

    #region Bind Service Name
    public void BindServicemName()
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
                          + "configMaster?activestatus=A&configTypename=tax";
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
                            ddlservicename.DataSource = dt;
                            ddlservicename.DataValueField = "configId";
                            ddlservicename.DataTextField = "configName";
                            ddlservicename.DataBind();
                        }
                        else
                        {
                            ddlservicename.DataBind();
                        }
                        ddlservicename.Items.Insert(0, new ListItem("Select", "0"));
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

    #region Insert Function
    public void InsertTaxMaster()
    {
        try
        {
            string frmDate = string.Empty;
            DateTime Date = Convert.ToDateTime(txtfrmDate.Text);
            frmDate = Date.ToString("yyyy-MM-dd");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new Taxmasterclass()
                {
                    serviceName = ddlservicename.SelectedValue.Trim(),
                    taxName = txttax.Text,
                    taxDescription = txtTaxDescription.Text,
                    taxPercentage = txtPercentage.Text,
                    effectiveFrom = frmDate.Trim(),
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("taxMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindTaxMaster();
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
    public void UpdateTaxMaster()
    {
        try
        {
            string frmDate = string.Empty;
            DateTime Date = Convert.ToDateTime(txtfrmDate.Text);
            frmDate = Date.ToString("yyyy-MM-dd");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new Taxmasterclass()
                {
                    serviceName = ddlservicename.SelectedValue.Trim(),
                    taxId = hftaxid.Value,
                    taxName = txttax.Text,
                    taxDescription = txtTaxDescription.Text,
                    taxPercentage = txtPercentage.Text,
                    effectiveFrom = frmDate.Trim(),
                    activeStatus = "A",
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("taxMaster", Update).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmit.Text = "Submit";
                        BindTaxMaster();
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

    #region Tax  Master Class
    public class Taxmasterclass
    {
        public string taxId { get; set; }
        public string serviceName { get; set; }
        public string taxName { get; set; }
        public string taxDescription { get; set; }
        public string taxPercentage { get; set; }
        public string activeStatus { get; set; }
        public string effectiveFrom { get; set; }
        public string effectiveTill { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }

    }
    #endregion

    #region Bind Tax  Master 
    public void BindTaxMaster()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("taxMaster").Result;
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
                            gvTaxmaster.DataSource = dt;
                            gvTaxmaster.DataBind();
                        }
                        else
                        {
                            gvTaxmaster.DataSource = null;
                            gvTaxmaster.DataBind();
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

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertTaxMaster();
        }
        else
        {
            UpdateTaxMaster();
        }
    }
    #endregion

    #region Update Text
    public void btnUpdateText()
    {
        btnSubmit.Text = "Update";
    }
    #endregion      

    #region Edit Click
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string frmDate = string.Empty;
            spAddorEdit.InnerText = "Edit ";
            btnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblserviceName = (Label)gvrow.FindControl("lblgvserviceNameId");
            Label lbltaxId = (Label)gvrow.FindControl("lblgvtaxId");
            Label lbltaxName = (Label)gvrow.FindControl("lblgvtaxName");
            Label lbltaxDescription = (Label)gvrow.FindControl("lblgvtaxDescription");
            Label lbltaxPercentage = (Label)gvrow.FindControl("lblgvtaxPercentage");
            Label lbleffectiveFrom = (Label)gvrow.FindControl("lblgveffectiveFrom");
            Label lbleffectiveTill = (Label)gvrow.FindControl("lblgveffectiveTill");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvTaxmaster = gvTaxmaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text.ToString() == "Active" ? "A" : "D";
            ViewState["taxId"] = sgvTaxmaster.ToString().Trim();
            ddlservicename.SelectedValue = lblserviceName.Text;
            ddlservicename.Enabled = false;
            hftaxid.Value = lbltaxId.Text;
            txttax.Text = lbltaxName.Text;
            txtTaxDescription.Text = lbltaxDescription.Text;
            txtPercentage.Text = lbltaxPercentage.Text;
            DateTime fromDate = Convert.ToDateTime(lbleffectiveFrom.Text);
            txtfrmDate.Text = fromDate.ToString("yyyy-MM-dd");
            txttodate.Text = lbleffectiveTill.Text;
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
                string sTaxId = gvTaxmaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "taxMaster?taxId=" + sTaxId
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
                        BindTaxMaster();
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

    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        ADD();
    }
    #endregion

    #region Cancel Click Fucntion
    public void Cancel()
    {
        spAddorEdit.InnerText = "";
        divGv.Visible = true;
        divForm.Visible = false;
        btnSubmit.Text = "Submit";
        ddlservicename.ClearSelection();
        txtfrmDate.Text = "";
        txtPercentage.Text = "";
        txttax.Text = "";
        txtTaxDescription.Text = "";
        txttodate.Text = "";
        ddlservicename.Enabled = true;
    }
    #endregion

    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
    }
    #endregion
}