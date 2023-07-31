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

public partial class Master_OfferMapping : System.Web.UI.Page
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
            BindGvOfferMapping();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion

    #region Bind ddl Branch 
    public void BindBranch()
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
                          + "branchMaster?branchId=";
                sUrl += Session["branchId"].ToString().Trim() + "&activeStatus=A";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("branchWorkingHrsDetails").Remove();
                            item.Property("branchImageMasterDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {
                            ddlbranchname.DataSource = dt;
                            ddlbranchname.DataValueField = "branchId";
                            ddlbranchname.DataTextField = "branchName";
                            ddlbranchname.DataBind();
                            ddlbranchname.SelectedValue = Session["branchId"].ToString();
                        }
                        else
                        {
                            ddlbranchname.DataBind();
                        }
                        ddlbranchname.Items.Insert(0, new ListItem("Select", "0"));
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

    #region Bind ddl Offer 
    public void BindOffer()
    {
        DivCheckIn.Visible = false;
        try
        {
            using (var client = new HttpClient())
            {
                DateTime NowDate = DateTime.Now;
                string Date = NowDate.ToString("yyyy'-'MM'-'dd");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "offerMaster?branchId=" + Session["branchId"].ToString() + "&date=" + Date + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("offerRulesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {
                            ddloffname.DataSource = dt;
                            ddloffname.DataValueField = "offerId";
                            ddloffname.DataTextField = "offerHeading";
                            ddloffname.DataBind();
                        }
                        else
                        {
                            ddloffname.DataBind();
                        }
                        ddloffname.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divForm.Visible = false;
                        divGv.Visible = true;
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

    #region Bind GridView
    public void BindGvOfferMapping()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "offerMapping?branchId=";
                sUrl += Session["branchId"].ToString();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<OfferMapping> OwnerMstr = JsonConvert.DeserializeObject<List<OfferMapping>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(OwnerMstr);
                        if (dt.Rows.Count > 0)
                        {
                            gvOfferMapping.DataSource = dt;
                            gvOfferMapping.DataBind();
                        }
                        else
                        {
                            divGv.Visible = false;
                            divForm.Visible = true;
                            BindOffer();
                            BindBranch();
                            spAddorEdit.InnerText = "Add ";
                            gvOfferMapping.DataBind();
                        }
                    }
                    else
                    {
                        divGv.Visible = false;
                        divForm.Visible = true;
                        BindOffer();
                        BindBranch();
                        spAddorEdit.InnerText = "Add ";
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

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Cancel();
        divGv.Visible = false;
        divForm.Visible = true;
        BindOffer();
        BindBranch();
        spAddorEdit.InnerText = "Add ";
    }
    #endregion



    #region Submit / Insert 
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertOfferMapping();
        }
    }
    public void InsertOfferMapping()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new OfferMapping()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString().Trim(),
                    branchId = ddlbranchname.SelectedValue.Trim(),
                    offerId = ddloffname.SelectedValue.Trim(),
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("offerMapping", Insert).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindGvOfferMapping();
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
                string sofferMappingId = gvOfferMapping.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "offerMapping?offerMappingId=" + sofferMappingId
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
                        BindGvOfferMapping();
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

    #region OfferMapping Details Show When Add
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
                        + "offerMaster?offerId=" + ddloffname.SelectedValue + "";
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

                        DivCheckIn.Visible = true;
                        var OfferItems = Offer.ElementAt(0);

                        string Offertype = OfferItems.offerType.ToString();
                        if (Offertype == "P")
                        {
                            PerorFix.InnerText = "(in %)";
                        }
                        else
                        {
                            PerorFix.InnerText = "(in ₹)";
                        }
                        lblAmount.Text = OfferItems.offerValue.ToString();
                        lblOfferDes.Text = OfferItems.offerDescription.ToString();
                        lblOfferNameheading.Text = OfferItems.offerHeading.ToString();
                        DateTime fromDate = Convert.ToDateTime(OfferItems.fromDate.ToString());
                        string dates = fromDate.ToString("dd MMM yyyy");
                        string fromTime = OfferItems.fromTime.ToString();
                        DateTime date = Convert.ToDateTime(fromTime);
                        string fromTimes = date.ToString("HH:mm ");
                        DateTime toDate = Convert.ToDateTime(OfferItems.toDate.ToString());
                        string datess = toDate.ToString("dd MMM yyyy");
                        string toTime = OfferItems.toTime.ToString();
                        DateTime datetoTime = Convert.ToDateTime(toTime);
                        string datetoTimes = datetoTime.ToString("HH:mm ");
                        lblfromdate.Text = dates + " - " + fromTimes;
                        lblTodate.Text = datess + " - " + datetoTimes;
                        if (ResponseMsg.Contains("offerRulesDetails"))
                        {
                            if (OfferItems.offerRulesDetails == null)
                            {
                                dtlRules.DataSource = null;
                                dtlRules.DataBind();
                                Rules.Visible = false;
                            }
                            else
                            {
                                Rules.Visible = true;
                                var OfferRules = OfferItems.offerRulesDetails.ToList();
                                DataTable OfferRulesType = ConvertToDataTable(OfferRules);
                                dtlRules.DataSource = OfferRulesType;
                                dtlRules.DataBind();
                            }

                        }
                        else
                        {
                            dtlRules.DataBind();
                            Rules.Visible = false;
                        }



                    }
                    else
                    {
                        dtlRules.DataSource = null;
                        dtlRules.DataBind();
                        Rules.Visible = false;
                        DivCheckIn.Visible = false;
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
    protected void ddloffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindOfferMaster();
    }
    protected void lnkView_Click(object sender, EventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        Label lblGvBranchId = (Label)gvrow.FindControl("lblGvBranchId");
        Label lblGvBranchName = (Label)gvrow.FindControl("lblGvBranchName");
        Label lblGvOfferId = (Label)gvrow.FindControl("lblGvOfferId");
        Label lblGvOfferHeading = (Label)gvrow.FindControl("lblGvOfferHeading");

        ddlbranchname.Items.Insert(0, new ListItem(lblGvBranchName.Text, lblGvBranchId.Text));
        ddlbranchname.SelectedValue = lblGvBranchId.Text;

        ddloffname.Items.Insert(0, new ListItem(lblGvOfferHeading.Text, lblGvOfferId.Text));
        ddloffname.SelectedValue = lblGvOfferId.Text;
        ddloffname.Enabled = false;
        divGv.Visible = false;
        divForm.Visible = true;
        btnSubmit.Visible = false;
        spAddorEdit.InnerText = "View ";
        BindOfferMaster();
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
        ddloffname.Items.Clear();
        ddlbranchname.Items.Clear();
        btnSubmit.Visible = true;
        ddloffname.Enabled = true;
    }
    #endregion

    #region Offer Class
    public class OfferMasterClass
    {
        public String offerId { get; set; }
        public String offerHeading { get; set; }
        public String offerDescription { get; set; }
        public String fromDate { get; set; }
        public String toDate { get; set; }
        public String fromTime { get; set; }
        public String toTime { get; set; }
        public String offerType { get; set; }
        public String offerValue { get; set; }
        public String minAmt { get; set; }
        public String maxAmt { get; set; }
        public String noOfTimesPerUser { get; set; }
        public List<offerRulesDetails> offerRulesDetails { get; set; }

    }
    public class OfferMapping
    {
        public string uniqueId { get; set; }
        public string offerHeading { get; set; }
        public string offerMappingId { get; set; }
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string branchName { get; set; }
        public string offerId { get; set; }
        public string offerType { get; set; }
        public string offerValue { get; set; }
        public string minAmt { get; set; }
        public string maxAmt { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }

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
                        var Option = lst1.Where(x => x.optionName == "offerMapping" && x.MenuOptionAccessActiveStatus == "A")
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
                                if (Delete[0] == "True")
                                {
                                    gvOfferMapping.Columns[6].Visible = true;
                                }
                                else
                                {
                                    gvOfferMapping.Columns[6].Visible = false;
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
    #endregion

}