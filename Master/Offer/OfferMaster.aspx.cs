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

public partial class Master_Offer_OfferMaster : System.Web.UI.Page
{
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
            BindOfferMaster();
            BindDdlOfferRules();
            //rbtntypeperiod.SelectedValue = "B";
            rdoofftype.SelectedValue = "P";
            txtfrmtime.Text = "0:00";
            txttotime.Text = "23:59";
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
        if(IsPostBack)
        {
            if (hfofferimage.Value == "")
            {
                imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
            }
            else
            {
                imgEmpPhotoPrev.ImageUrl = hfofferimage.Value;
            }
        }
    }
    #endregion

    #region Bind Offer  Master 
    public void BindOfferMaster()
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
                        + "offerMaster";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    var Response = JObject.Parse(SmartParkingList)["response"].ToArray();

                    if (StatusCode == 1)
                    {
                        List<OfferMasterClass> Offer = JsonConvert.DeserializeObject<List<OfferMasterClass>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Offer);

                        if (dt.Rows.Count > 0)
                        {
                            gvoffermaster.DataSource = Offer;
                            gvoffermaster.DataBind();
                        }
                        else
                        {
                            gvoffermaster.DataSource = null;
                            gvoffermaster.DataBind();
                        }
                    }
                    else
                    {
                        ADD();
                        // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    #region rbtn Off Period type  SelectedIndexChanged
    protected void rbtntypeperiod_SelectedIndexChanged(object sender, EventArgs e)
    {
        divAmount.Visible = true;
        divDate.Visible = true;
        divValue.Visible = true;
              
        if (rbtntypeperiod.SelectedValue == "F")
        {
            txtmaxamount.Text = "0";
            txtminamount.Text = "0";
            txtminamount.Enabled = false;
            txtmaxamount.Enabled = false;
        }
        else
        {
            txtmaxamount.Text = "";
            txtminamount.Text = "";
            txtminamount.Enabled = true;
            txtmaxamount.Enabled = true;
        }
    }
    #endregion

    #region rbtn off Value type SelectedIndexChanged
    protected void rdoofftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        imgEmpPhotoPrev.ImageUrl = hfofferimage.Value;
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

    #region offerVal/Min/Max Amount TextChange
    protected void txtOffervalueFix_TextChanged(object sender, EventArgs e)
    {
        if (rdoofftype.SelectedValue == "F")
        {
            if (txtminamount.Text != "")
            {
                if (Convert.ToDecimal(txtminamount.Text) < Convert.ToDecimal(txtOffervalueFix.Text))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Offer Value must be Lesser than Offer Value Min. Amount');", true);
                    txtminamount.Text = "";
                }
            }           
        }

    }

    protected void txtminamount_TextChanged(object sender, EventArgs e)
    {      
        if (rdoofftype.SelectedValue == "F")
        {
            if (txtminamount.Text != "" & txtOffervalueFix.Text !="")
            {
                if (Convert.ToDecimal(txtminamount.Text) < Convert.ToDecimal(txtOffervalueFix.Text))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Min. Amount must be Greater than Offer Value');", true);
                    txtminamount.Text = "";
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Enter Minimum Amount');", true);
            }
        }
        if (txtminamount.Text != "" && txtmaxamount.Text != "")
        {
            if (Convert.ToDecimal(txtminamount.Text) > Convert.ToDecimal(txtmaxamount.Text))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Min. Amount must be Lesser than Max. Amount');", true);
                txtminamount.Text = "";
            }
        }

    }
  
    protected void txtmaxamount_TextChanged(object sender, EventArgs e)
    {           
        if(txtminamount.Text != "" && txtmaxamount.Text !="")
        {
            if (Convert.ToDecimal(txtminamount.Text) > Convert.ToDecimal(txtmaxamount.Text))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Max. Amount must be Greater than Min. Amount');", true);
                txtmaxamount.Text = "";
            }
        }
    }
    #endregion

    #region OfferRulesId 
    public void BindDdlOfferRules()
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim() + "configMaster?configTypename=ruleType&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtfloorRules = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        for (int i = 0; i < dtfloorRules.Rows.Count; i++)
                        {
                            if (dtfloorRules.Rows[i]["configName"].ToString() == "Terms and Condition")
                            {
                                ViewState["TermsandCondition"] = dtfloorRules.Rows[i]["configId"].ToString();

                            }
                            if (dtfloorRules.Rows[i]["configName"].ToString() == "Offer Availability")
                            {
                                ViewState["OfferAvailability"] = dtfloorRules.Rows[i]["configId"].ToString();

                            }
                            if (dtfloorRules.Rows[i]["configName"].ToString() == "Rules")
                            {
                                ViewState["Rules"] = dtfloorRules.Rows[i]["configId"].ToString();

                            }
                            if (dtfloorRules.Rows[i]["configName"].ToString() == "About Offer")
                            {
                                ViewState["AboutOffer"] = dtfloorRules.Rows[i]["configId"].ToString();

                            }

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
    #endregion

    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
      
        if (btnSubmit.Text == "Submit")
        {
            InsertOffer();
        }
        else
        {
            UpdateOffer();
        }
    }
    #endregion

    #region Cancel Click Fucntion
    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        rbtntypeperiod.SelectedIndex = -1;
        rdoofftype.SelectedValue = "P";
        txtoffhead.Text = string.Empty;
        txtoffdescription.Text = string.Empty;
        txtoffcode.Text = string.Empty;
        txtfrmdate.Text = string.Empty;
        txttodate.Text = string.Empty;
        txtfrmtime.Text = "0:00";
        txttotime.Text = "23:59";
        txtminamount.Text = string.Empty;
        txtmaxamount.Text = string.Empty;
        txtperuser.Text = string.Empty;
        txttc.Text = string.Empty;
        txtOffervalueFix.Text = "";
        txtoffvalueper.Text = "";
        hfofferimage.Value = "";
        imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
        btnSubmit.Text = "Submit";
        txttc.Text = string.Empty;
        ViewState["TermsRuleId"] = "";
        txtOfferAvailabilty.Text = "";
        ViewState["OfferRuleId"] = "";
        txtRules.Text = "";
        ViewState["RulesRuleId"] = "";
        txtAboutOffer.Text = "";
        ViewState["AboutRuleId"] = "";
        txtminamount.Enabled = true;
        txtmaxamount.Enabled = true;
        divAmount.Visible = false;
        divDate.Visible = false;
        divValue.Visible = false;

    }
    #endregion

    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
        spAddorEdit.InnerText = "Add ";
    }
    #endregion

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ADD();
    }
    #endregion

    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion 

    #region Insert Offer Function
    public void InsertOffer()
    {
        if (rbtntypeperiod.SelectedValue == "")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Select Offer Period Type');", true);
            return;
        }
        if (hfofferimage.Value == "")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Upload Offer Image');", true);
            return;
        }
        string offerRule = string.Empty;
        string ruleType = string.Empty;
        string activeStatus = string.Empty;
        if (txtoffvalueper.Text == "" && txtOffervalueFix.Text=="")
        {

            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Enter Offer Value');", true);
            return;
        }

        if (txttc.Text != "")
        {
            offerRule += txttc.Text + ',';
            ruleType += ViewState["TermsandCondition"].ToString().Trim() + ',';
            activeStatus += "A" + ',';
        }
        if (txtOfferAvailabilty.Text != "")
        {
            offerRule += txtOfferAvailabilty.Text + ',';
            ruleType += ViewState["OfferAvailability"].ToString().Trim() + ',';
            activeStatus += "A" + ',';
        }
        if (txtAboutOffer.Text != "")
        {
            offerRule += txtAboutOffer.Text + ',';
            ruleType += ViewState["AboutOffer"].ToString().Trim() + ',';
            activeStatus += "A" + ',';

        }
        if (txtRules.Text != "")
        {
            offerRule += txtRules.Text + ',';
            ruleType += ViewState["Rules"].ToString().Trim() + ',';
            activeStatus += "A" + ',';
        }
        string OfferValue = string.Empty;
        if (rdoofftype.SelectedValue == "P")
        {
            OfferValue = txtoffvalueper.Text;

        }
        else
        {
            OfferValue = txtOffervalueFix.Text;
        }
        DateTime frmrtime = DateTime.Parse(txtfrmtime.Text);
        DateTime totime = DateTime.Parse(txttotime.Text);

        if (frmrtime > totime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('To Time Should be Greater Than From Time');", true);
            return;
        }

        try
        {
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            DateTime FrmDate = Convert.ToDateTime(txtfrmdate.Text);
            DateTime TDate = Convert.ToDateTime(txttodate.Text);
            FromDate = FrmDate.ToString("yyyy-MM-dd");
            ToDate = TDate.ToString("yyyy-MM-dd");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new OfferMasterClass()
                {
                    offerTypePeriod = rbtntypeperiod.SelectedValue,
                    offerHeading = txtoffhead.Text,
                    offerDescription = txtoffdescription.Text,
                    offerCode = txtoffcode.Text,
                    offerImageUrl = hfofferimage.Value,
                    fromDate = FromDate.Trim(),
                    toDate = ToDate.Trim(),
                    fromTime = txtfrmtime.Text,
                    toTime = txttotime.Text,
                    offerType = rdoofftype.SelectedValue,
                    offerValue = OfferValue,
                    minAmt = txtminamount.Text,
                    maxAmt = txtmaxamount.Text,
                    noOfTimesPerUser = txtperuser.Text,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString(),
                };
                if (txttc.Text != "" || txtOfferAvailabilty.Text != "" || txtAboutOffer.Text != "" || txtRules.Text != "")
                {
                    Insert.offerRulesDetails = GetofferRulesDetails(offerRule.ToString().TrimEnd(','),
                 ruleType.ToString().TrimEnd(','), activeStatus.ToString().TrimEnd(','));
                }

                HttpResponseMessage response = client.PostAsJsonAsync("offerMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        Cancel();
                        BindOfferMaster();
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
    public static List<offerRulesDetails> GetofferRulesDetails(string offerRule, string ruleType, string activeStatus)
    {
        string[] offerRules;
        string[] ruleTypes;
        string[] activeStatuss;
        offerRules = offerRule.Split(',');
        ruleTypes = ruleType.Split(',');
        activeStatuss = activeStatus.Split(',');
        List<offerRulesDetails> lst = new List<offerRulesDetails>();
        for (int i = 0; i < activeStatuss.Count(); i++)
        {
            lst.AddRange(new List<offerRulesDetails>
            {
                new offerRulesDetails { offerRule=offerRules[i] ,ruleType=ruleTypes[i],activeStatus=activeStatuss[i]}

            });
        }

        return lst;

    }
    #endregion

    #region Update Offer Function
    public void UpdateOffer()
    {
        if (hfofferimage.Value == "")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Please Upload Offer Image');", true);
            return;
        }
        DateTime frmrtime = DateTime.Parse(txtfrmtime.Text);
        DateTime totime = DateTime.Parse(txttotime.Text);

        if (frmrtime > totime)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('To Time Should be Greater Than From Time');", true);
            return;
        }
        try
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
            string offerRule = string.Empty;
            string ruleType = string.Empty;
            string activeStatus = string.Empty;
            string ruleId = string.Empty;
            string offerId = string.Empty;
            if (txttc.Text != "")
            {
                offerRule += txttc.Text + ',';
                ruleType += ViewState["TermsandCondition"].ToString().Trim() + ',';
                activeStatus += "A" + ',';
                offerId += ViewState["OfferId"].ToString().Trim() + ',';
                if (ViewState["TermsRuleId"].ToString().Trim() == "")
                {
                    ruleId += "0" + ',';
                }
                else
                {
                    ruleId += ViewState["TermsRuleId"].ToString().Trim() + ',';
                }

            }
            if (txtOfferAvailabilty.Text != "")
            {
                offerRule += txtOfferAvailabilty.Text + ',';
                ruleType += ViewState["OfferAvailability"].ToString().Trim() + ',';
                activeStatus += "A" + ',';
                offerId += ViewState["OfferId"].ToString().Trim() + ',';
                if (ViewState["OfferRuleId"].ToString().Trim() == "")
                {
                    ruleId += "0" + ',';
                }
                else
                {
                    ruleId += ViewState["OfferRuleId"].ToString().Trim() + ',';
                }
            }
            if (txtAboutOffer.Text != "")
            {
                offerRule += txtAboutOffer.Text + ',';
                ruleType += ViewState["AboutOffer"].ToString().Trim() + ',';
                activeStatus += "A" + ',';
                offerId += ViewState["OfferId"].ToString().Trim() + ',';
                if (ViewState["AboutRuleId"].ToString().Trim() == "")
                {
                    ruleId += "0" + ',';
                }
                else
                {
                    ruleId += ViewState["AboutRuleId"].ToString().Trim() + ',';
                }

            }
            if (txtRules.Text != "")
            {
                offerRule += txtRules.Text + ',';
                ruleType += ViewState["Rules"].ToString().Trim() + ',';
                activeStatus += "A" + ',';
                offerId += ViewState["OfferId"].ToString().Trim() + ',';
                if (ViewState["RulesRuleId"].ToString().Trim() == "")
                {
                    ruleId += "0" + ',';
                }
                else
                {
                    ruleId += ViewState["RulesRuleId"].ToString().Trim() + ',';
                }
            }

            string FromDate = string.Empty;
            string ToDate = string.Empty;
            DateTime FrmDate = Convert.ToDateTime(txtfrmdate.Text);
            DateTime TDate = Convert.ToDateTime(txttodate.Text);
            FromDate = FrmDate.ToString("yyyy-MM-dd");
            ToDate = TDate.ToString("yyyy-MM-dd");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new OfferMasterClass()
                {
                    offerId = ViewState["OfferId"].ToString().Trim(),
                    offerTypePeriod = rbtntypeperiod.SelectedValue,
                    offerHeading = txtoffhead.Text,
                    offerDescription = txtoffdescription.Text,
                    offerCode = txtoffcode.Text,
                    offerImageUrl = hfofferimage.Value,
                    fromDate = FromDate.Trim(),
                    toDate = ToDate.Trim(),
                    fromTime = txtfrmtime.Text,
                    toTime = txttotime.Text,
                    offerType = rdoofftype.SelectedValue,
                    offerValue = OfferValue,
                    minAmt = txtminamount.Text,
                    maxAmt = txtmaxamount.Text,
                    noOfTimesPerUser = txtperuser.Text,
                    activeStatus = ViewState["ActiveStatus"].ToString().Trim(),
                    updatedBy = Session["UserId"].ToString().Trim(),
                };
                if (txttc.Text != "" || txtOfferAvailabilty.Text != "" || txtAboutOffer.Text != "" || txtRules.Text != "")
                {
                    Update.offerRulesDetails = UpdateList(ruleId.ToString().TrimEnd(','),
                    offerId.ToString().TrimEnd(','), offerRule.ToString().TrimEnd(','),
                    ruleType.ToString().TrimEnd(','), activeStatus.ToString().TrimEnd(','));
                }
                HttpResponseMessage response = client.PutAsJsonAsync("offerMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        Cancel();
                        BindOfferMaster();
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
    public static List<offerRulesDetails> UpdateList(string offerRuleId, string offerId, string offerRule, string ruleType,
     string activeStatus)
    {
        string[] offerRuleIds;
        string[] offerIds;
        string[] offerRules;
        string[] ruleTypes;
        string[] activeStatuss;
        offerRuleIds = offerRuleId.TrimEnd().Split(',');
        offerIds = offerId.TrimEnd().Split(',');
        offerRules = offerRule.TrimEnd().Split(',');
        ruleTypes = ruleType.TrimEnd().Split(',');
        activeStatuss = activeStatus.TrimEnd().Split(',');
        List<offerRulesDetails> lst = new List<offerRulesDetails>();

        for (int i = 0; i < activeStatuss.Count(); i++)
        {
            lst.AddRange(new List<offerRulesDetails>
            {
                new offerRulesDetails { offerRuleId=offerRuleIds[i],offerId=offerIds[i],
                    offerRule =offerRules[i] ,ruleType=ruleTypes[i],activeStatus=activeStatuss[i]}

            });
        }
        return lst;
    }

    #endregion

    #region Offer  Master Class
    public class OfferMasterClass
    {
        public String offerId { get; set; }
        public String parkingOwnerId { get; set; }
        public String offerTypePeriod { get; set; }
        public String offerHeading { get; set; }
        public String offerDescription { get; set; }
        public String offerCode { get; set; }
        public String offerImageUrl { get; set; }
        public String fromDate { get; set; }
        public String toDate { get; set; }
        public String fromTime { get; set; }
        public String toTime { get; set; }
        public String offerType { get; set; }
        public String offerValue { get; set; }
        public String minAmt { get; set; }
        public String maxAmt { get; set; }
        public String noOfTimesPerUser { get; set; }
        public String termsAndConditions { get; set; }
        public String offerRule { get; set; }
        public String ruleType { get; set; }
        public String createdBy { get; set; }
        public String activeStatus { get; set; }
        public String ExpireStatus { get; set; }
        public String updatedBy { get; set; }
        public List<offerRulesDetails> offerRulesDetails { get; set; }

    }
    public class offerRulesDetails
    {
        public String offerRuleId { get; set; }
        public String offerId { get; set; }
        public String offerRule { get; set; }
        public String ruleType { get; set; }
        public String activeStatus { get; set; }
    }

    #endregion

    #region Edit Click
    protected void LnkEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvofferId = (Label)gvrow.FindControl("lblgvofferId");
            Label lblgvofferTypePeriod = (Label)gvrow.FindControl("lblgvofferTypePeriod");
            Label lblgvofferHeading = (Label)gvrow.FindControl("lblgvofferHeading");
            Label lblgvofferDescription = (Label)gvrow.FindControl("lblgvofferDescription");
            Label lblgvofferImageUrl = (Label)gvrow.FindControl("lblgvofferImageUrl");
            Label lblgvofferCode = (Label)gvrow.FindControl("lblgvofferCode");
            Label lblgvfromDate = (Label)gvrow.FindControl("lblgvfromDate");
            Label lblgvtoDate = (Label)gvrow.FindControl("lblgvtoDate");
            Label lblgvfromTime = (Label)gvrow.FindControl("lblgvfromTime");
            Label lblgvtoTime = (Label)gvrow.FindControl("lblgvtoTime");
            Label lblgvofferType = (Label)gvrow.FindControl("lblgvofferType");
            Label lblgvofferValue = (Label)gvrow.FindControl("lblgvofferValue");
            Label lblgvminAmt = (Label)gvrow.FindControl("lblgvminAmt");
            Label lblgvnoOfTimesPerUser = (Label)gvrow.FindControl("lblgvnoOfTimesPerUser");

            Label lblExpireStatus = (Label)gvrow.FindControl("lblExpireStatus");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sgvOffermaster = gvoffermaster.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.Text == "Active" ? "A" : "D";
            ViewState["OfferId"] = sgvOffermaster.ToString().Trim();
            txtoffhead.Text = lblgvofferHeading.Text;
            txtoffdescription.Text = lblgvofferDescription.Text;
            hfofferimage.Value = lblgvofferImageUrl.Text;
            imgEmpPhotoPrev.ImageUrl = lblgvofferImageUrl.Text.Trim();
            if (hfofferimage.Value == "")
            {
                imgEmpPhotoPrev.ImageUrl = "../../images/EmptyImageNew.png";
            }
            else
            {
                imgEmpPhotoPrev.ImageUrl = hfofferimage.Value;
            }
            txtoffcode.Text = lblgvofferCode.Text;

            DateTime Fromdate = Convert.ToDateTime(lblgvfromDate.Text);
            txtfrmdate.Text = Fromdate.ToString("yyyy-MM-dd");
            DateTime Todate = Convert.ToDateTime(lblgvtoDate.Text);
            txttodate.Text = Todate.ToString("yyyy-MM-dd");

            //txtfrmdate.Text = lblgvfromDate.Text;
            //txttodate.Text = lblgvtoDate.Text;
            txtfrmtime.Text = lblgvfromTime.Text;
            txttotime.Text = lblgvtoTime.Text;

            string Amt = lblgvminAmt.Text;
            string[] sAmt = Amt.Split('/');
            txtminamount.Text = sAmt[0].ToString().Trim();
            txtmaxamount.Text = sAmt[1].ToString().Trim();
            txtperuser.Text = lblgvnoOfTimesPerUser.Text;
            if (lblgvofferTypePeriod.Text == "B")
            {
                rbtntypeperiod.SelectedValue = "B";
            }
            else
            {
                rbtntypeperiod.SelectedValue = "A";
            }
            if (lblgvofferType.Text == "P")
            {
                rdoofftype.SelectedValue = "P";
            }
            else
            {
                rdoofftype.SelectedValue = "F";
            }

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
            var dlOfferrules = gvrow.FindControl("dlOfferrules") as DataList;
            ViewState["TermsRuleId"] = "";
            ViewState["OfferRuleId"] = "";
            ViewState["RulesRuleId"] = "";
            ViewState["AboutRuleId"] = "";
            string offerruleid = string.Empty;
            for (int i = 0; i < dlOfferrules.Items.Count; i++)
            {
                Label lblgvofferRuleId = dlOfferrules.Items[i].FindControl("lblgvofferRuleId") as Label;
                Label lblgvofferRule = dlOfferrules.Items[i].FindControl("lblgvofferRule") as Label;
                Label lblgvruleType = dlOfferrules.Items[i].FindControl("lblgvruleType") as Label;
                Label lblgvofferactiveStatus = dlOfferrules.Items[i].FindControl("lblgvofferactiveStatus")
               as Label;


                if (lblgvruleType.Text == ViewState["TermsandCondition"].ToString().Trim())
                {
                    txttc.Text = lblgvofferRule.Text;
                    ViewState["TermsRuleId"] = lblgvofferRuleId.Text;

                }
                if (lblgvruleType.Text == ViewState["OfferAvailability"].ToString().Trim())
                {
                    txtOfferAvailabilty.Text = lblgvofferRule.Text;
                    ViewState["OfferRuleId"] = lblgvofferRuleId.Text;

                }
                if (lblgvruleType.Text == ViewState["Rules"].ToString().Trim())
                {
                    txtRules.Text = lblgvofferRule.Text;
                    ViewState["RulesRuleId"] = lblgvofferRuleId.Text;

                }
                if (lblgvruleType.Text == ViewState["AboutOffer"].ToString().Trim())
                {
                    txtAboutOffer.Text = lblgvofferRule.Text;
                    ViewState["AboutRuleId"] = lblgvofferRuleId.Text;

                }
                offerruleid += lblgvofferRuleId.Text;
            }
            ADD();
            spAddorEdit.InnerText = "Edit ";
            divAmount.Visible = true;
            divDate.Visible = true;
            divValue.Visible = true;
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

    #region Delete Click
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sOfferMasterId = gvoffermaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "offerMaster?offerId=" + sOfferMasterId
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
                        BindOfferMaster();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
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

    #region gvoffermaster RowDataBound
    protected void gvoffermaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var offer = e.Row.DataItem as OfferMasterClass;
            var dlOfferrules = e.Row.FindControl("dlOfferrules") as DataList;
            dlOfferrules.DataSource = offer.offerRulesDetails;
            dlOfferrules.DataBind();
        }

    }
    #endregion


   
}