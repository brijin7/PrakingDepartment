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

public partial class Master_SubscriptionMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTaxName();
            BindSubscriptionMaster();
            txtTodate.Attributes.Add("readonly", "readonly");
        }
    }
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
                            ddltaxFP.DataSource = dt;
                            ddltaxFP.DataValueField = "taxId";
                            ddltaxFP.DataTextField = "tax";
                            ddltaxFP.DataBind();
                        }
                        else
                        {
                            ddltaxFP.DataBind();
                        }
                        ddltaxFP.Items.Insert(0, new ListItem("Select", "0"));
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
    #region Bind Subscription Details
    public void BindSubscriptionMaster()
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
                        + "subscriptionMaster";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Columns.Contains("parkingLimit"))
                        {

                        }
                        else
                        {
                            dt.Columns.Add("parkingLimit");
                        }
                        if (dt.Rows.Count > 0)
                        {
                            gvSubscriptionMaster.DataSource = dt;
                            gvSubscriptionMaster.DataBind();
                        }
                        else
                        {
                            gvSubscriptionMaster.DataSource = null;
                            gvSubscriptionMaster.DataBind();
                        }
                    }
                    else
                    {
                        divFormFP.Visible = true;
                        divGvFP.Visible = false;
                        spAddorEditFP.InnerText = "Add ";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region btnAdd
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        divFormFP.Visible = true;
        divGvFP.Visible = false;
        Clear();
        spAddorEditFP.InnerText = "Add ";
    }
    #endregion
    #region btnSubmit
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            Insert();
        }
        else
        {
            Update();
        }

    }
    #endregion

    #region Insert Function
    public void Insert()
    {
        string OfferValue = string.Empty;
        if (rdoofftype.SelectedValue == "P")
        {
            OfferValue = txtoffvalueper.Text;

        }
        else
        {
            OfferValue = txtOffervalueFix.Text;
        }
        if (OfferValue.Trim() == "0")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Enter Offer Value');", true);
            return;
        }
        string FromDate = string.Empty;
        string ToDate = string.Empty;

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new Subscription()
                {
                    subscriptionName = txtSubScrName.Text,
                    validity = txtValidity.Text,
                    offerType = rdoofftype.SelectedValue.Trim(),
                    offerValue = OfferValue.TrimStart('0'),
                    totalAmount = txtamountFP.Text,
                    taxId = ddltaxFP.SelectedValue.Trim(),
                    rules = txtRules.Text,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString()
                };
                string times;
                string[] date2;
                DateTime utcTime = DateTime.Now;
                times = utcTime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff'Z'");
                date2 = times.Split(' ');


                if (txtFromDate.Text != "")
                {
                    DateTime FrmDate = Convert.ToDateTime(txtFromDate.Text);
                    FromDate = FrmDate.ToString("yyyy-MM-dd");
                    Insert.validityFrom = FromDate.Trim() + "T" + date2[1];
                }
                if (txtTodate.Text != "")
                {
                    DateTime TDate = Convert.ToDateTime(txtTodate.Text);
                    ToDate = TDate.ToString("yyyy-MM-dd");
                    Insert.validityTo = ToDate.Trim() + "T" + date2[1];
                }
                if (txtParkingLimit.Text != "")
                {
                    Insert.parkingLimit = txtParkingLimit.Text;
                }
                HttpResponseMessage response = client.PostAsJsonAsync("subscriptionMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindSubscriptionMaster();
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
    #endregion
    #region Update Function
    public void Update()
    {
        string OfferValue = string.Empty;
        if (rdoofftype.SelectedValue == "P")
        {
            OfferValue = txtoffvalueper.Text;
        }
        else
        {
            OfferValue = txtOffervalueFix.Text;
        }
        string FromDate = string.Empty;
        string ToDate = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new Subscription()
                {
                    subscriptionName = txtSubScrName.Text,
                    validity = txtValidity.Text,
                    offerType = rdoofftype.SelectedValue.Trim(),
                    offerValue = OfferValue.TrimStart('0'),
                    rules = txtRules.Text,
                    totalAmount = txtamountFP.Text,
                    activeStatus = ViewState["ActiveStatus"].ToString(),
                    updatedBy = Session["UserId"].ToString(),
                    subscriptionId = ViewState["SubScriptionId"].ToString(),
                    taxId = ddltaxFP.SelectedValue.Trim()
                };
                string times;
                string[] date2;
                DateTime utcTime = DateTime.Now;
                times = utcTime.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff'Z'");
                date2 = times.Split(' ');
                if (txtFromDate.Text != "")
                {
                    DateTime FrmDate = Convert.ToDateTime(txtFromDate.Text);
                    FromDate = FrmDate.ToString("yyyy-MM-dd");
                    Update.validityFrom = FromDate.Trim() + "T" + date2[1];
                }
                if (txtTodate.Text != "")
                {
                    DateTime TDate = Convert.ToDateTime(txtTodate.Text);
                    ToDate = TDate.ToString("yyyy-MM-dd");
                    Update.validityTo = ToDate.Trim() + "T" + date2[1];
                }
                if (txtParkingLimit.Text != "")
                {
                    Update.parkingLimit = txtParkingLimit.Text;
                }
                HttpResponseMessage response = client.PutAsJsonAsync("subscriptionMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindSubscriptionMaster();
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
    #endregion
    #region Edit Click
    protected void LnkEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvsubscriptionName = (Label)gvrow.FindControl("lblgvsubscriptionName");
            Label lblgvofferType = (Label)gvrow.FindControl("lblgvofferType");
            Label lblgvamountFP = (Label)gvrow.FindControl("lblgvamountFP");
            Label lblgvvalidity = (Label)gvrow.FindControl("lblgvvalidity");
            Label lblgvofferValue = (Label)gvrow.FindControl("lblgvofferValue");
            Label lblgvrules = (Label)gvrow.FindControl("lblgvrules");
            Label lblgvtax = (Label)gvrow.FindControl("lblgvtax");
            Label lblgvvalidityFrom = (Label)gvrow.FindControl("lblgvvalidityFrom");
            Label lblgvvalidityTo = (Label)gvrow.FindControl("lblgvvalidityTo");
            Label lblgvtaxName = (Label)gvrow.FindControl("lblgvtaxName");
            Label lblgvtaxFP = (Label)gvrow.FindControl("lblgvtaxFP");
            Label lblgvtotalAmountFP = (Label)gvrow.FindControl("lblgvtotalAmountFP");
            Label lblgvparkingLimit = (Label)gvrow.FindControl("lblgvparkingLimit");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string SubScriptionId = gvSubscriptionMaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text.Trim().ToString() == "Active" ? "A" : "D";
            ViewState["SubScriptionId"] = SubScriptionId.ToString().Trim();
            txtSubScrName.Text = lblgvsubscriptionName.Text;
            txtSubScrName.Enabled = false;
            rdoofftype.SelectedValue = lblgvofferType.Text == "Percentage" ? "P" : "F";
            if (rdoofftype.SelectedValue == "P")
            {
                txtoffvalueper.Text = lblgvofferValue.Text;
                divPercentage.Visible = true;
                divFixed.Visible = false;
                txtOffervalueFix.Text = "";
            }
            else
            {
                txtOffervalueFix.Text = lblgvofferValue.Text;
                txtoffvalueper.Text = "";
                divPercentage.Visible = false;
                divFixed.Visible = true;
            }
            txtValidity.Text = lblgvvalidity.Text;
            if (lblgvvalidityFrom.Text != "")
            {
                DateTime Fromdate = Convert.ToDateTime(lblgvvalidityFrom.Text);
                txtFromDate.Text = Fromdate.ToString("yyyy-MM-dd");
            }
            if (lblgvvalidityTo.Text != "")
            {
                DateTime Todate = Convert.ToDateTime(lblgvvalidityTo.Text);
                txtTodate.Text = Todate.ToString("yyyy-MM-dd");
            }
            txtamountFP.Text = lblgvtotalAmountFP.Text;
            BindTaxName();
            ddltaxFP.SelectedValue = lblgvtaxFP.Text;
            txtRules.Text = lblgvrules.Text;
            txtParkingLimit.Text = lblgvparkingLimit.Text;
            divFormFP.Visible = true;
            divGvFP.Visible = false;
            btnSubmit.Text = "Update";
            spAddorEditFP.InnerText = "Edit ";
        }
        catch (Exception ex)
        {
            string[] excp = ex.Message.Replace("'", "").Split('.');
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + excp[0].Trim() + "');", true);
        }
    }
    #endregion
    #region Active Inactive Click
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string SubScriptionId = gvSubscriptionMaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "subscriptionMaster?subscriptionId=" + SubScriptionId
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
                        BindSubscriptionMaster();
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
    #region btnCancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    #endregion
    #region Clear()
    public void Clear()
    {
        txtSubScrName.Enabled = true;
        txtSubScrName.Text = "";
        txtValidity.Text = "";
        rdoofftype.ClearSelection();
        txtOffervalueFix.Text = "";
        txtoffvalueper.Text = "";
        txtParkingLimit.Text = "";
        txtamountFP.Text = "";
        txtFromDate.Text = "";
        txtTodate.Text = "";
        btnSubmit.Text = "Submit";
        txtRules.Text = "";
        ddltaxFP.ClearSelection();
        divFormFP.Visible = false;
        divGvFP.Visible = true;
        spAddorEditFP.InnerText = " ";
    }
    #endregion
    #region Offertype Selected Index Changed
    protected void rdoofftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoofftype.SelectedValue == "P")
        {
            txtoffvalueper.Text = "";
            txtOffervalueFix.Text = "";
            divPercentage.Visible = true;
            divFixed.Visible = false;
        }
        else
        {
            txtoffvalueper.Text = "";
            txtOffervalueFix.Text = "";
            divPercentage.Visible = false;
            divFixed.Visible = true;

        }
    }
    #endregion
    #region Subscription  Master Class
    public class Subscription
    {
        public string subscriptionId { get; set; }
        public string parkingLimit { get; set; }
        public string subscriptionName { get; set; }
        public string validity { get; set; }
        public string offerType { get; set; }
        public string offerValue { get; set; }
        public string rules { get; set; }
        public string taxId { get; set; }
        public string amount { get; set; }
        public string tax { get; set; }
        public string totalAmount { get; set; }
        public string validityFrom { get; set; }
        public string validityTo { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public string taxName { get; set; }

    }
    #endregion
}