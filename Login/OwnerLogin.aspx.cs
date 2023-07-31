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

public partial class Login_OwnerLogin : System.Web.UI.Page
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
            if (Session["UserRole"].ToString() == "SA")
            {
                Session["ShowPage"] = "0";
                Session["parkingOwnerId"] = "0";
                ViewState["parkingOwnerId"] = "0";
                Session["branchId"] = "";
                Session["parkingName"] = "";
                Session["branchName"] = "";
                Session["slotExist"] = "";
                Session["multiBook"] = "";
                //Booking
                Session["BKvehicleType"] = "";
                Session["BKvehicleTypeName"] = "";
                Session["BKvehiclePlaceHolderImageUrl"] = "";
                Session["BKisvehicleNumberRequired"] = "";
                Session["BKitemIndex"] = "";
                //Booking
                BindParkingOwner();
                if (ddlParkingname.SelectedValue == "0")
                {
                    ViewState["parkingOwnerId"] = "0";
                }
                else
                {
                    ViewState["parkingOwnerId"] = ddlParkingname.SelectedValue;
                }
                BindBranch();
            }
            else
            {
                Session.Clear();
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
            }
        }
    }
    #endregion

    #region Bind Parking
    public void BindParkingOwner()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "parkingOwnerMaster";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<OwnerMaster> OwnerMstr = JsonConvert.DeserializeObject<List<OwnerMaster>>(ResponseMsg);
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("addressDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        dt.Columns.Add(new DataColumn("parking", System.Type.GetType("System.String"), "shortName + ' ~ ' + parkingName"));
                        if (dt.Rows.Count > 0)
                        {
                            gvOwnerList.DataSource = dt;
                            gvOwnerList.DataBind();

                            ddlParkingname.DataSource = dt;
                            ddlParkingname.DataValueField = "parkingOwnerId";
                            ddlParkingname.DataTextField = "parking";
                            ddlParkingname.DataBind();
                        }
                        else
                        {
                            gvOwnerList.DataBind();
                            ddlParkingname.DataBind();
                        }
                    }
                    ddlParkingname.Items.Insert(0, new ListItem("All", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #endregion       

    #region Parking click
    protected void lnkBtnParking_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;

        Label lblOwnerId = (Label)gvrow.FindControl("lblOwnerId");

        ViewState["parkingOwnerId"] = lblOwnerId.Text;
        Session["ShowPage"] = "1";
        BindBranch();
        //Response.Redirect("~/Login/BranchLogin.aspx", false);
    }

    protected void ddlParkingname_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["parkingOwnerId"] = ddlParkingname.SelectedValue;
        BindBranch();
        ScriptManager.RegisterStartupScript(this, GetType(), "Calltogglecheck", "togglecheck();", true);
    }
    #endregion


    #region Bind Branch Admin
    public void BindBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (ViewState["parkingOwnerId"].ToString() == "0")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                       + "branchMaster?activeStatus=A&approvalStatus=A";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                             + "branchMaster?activeStatus=A&approvalStatus=A&parkingOwnerId=";
                    sUrl += ViewState["parkingOwnerId"].ToString().Trim();
                }
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        List<BranchMasterClass> Branch = JsonConvert.DeserializeObject<List<BranchMasterClass>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Branch);

                        //DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        dt.Columns.Add(new DataColumn("branch", System.Type.GetType("System.String"), "shortName + ' ~ ' + branchName"));
                        if (dt.Rows.Count > 0)
                        {
                            gvBranchList.DataSource = dt;
                            gvBranchList.DataBind();
                            ddlBranch.DataSource = dt;
                            ddlBranch.DataValueField = "branchId";
                            ddlBranch.DataTextField = "branch";
                            ddlBranch.DataBind();

                            Session["parkingName"] = dt.Rows[0]["parkingName"].ToString();
                            Session["OneBranch"] = "false";
                            if (dt.Rows.Count == 1)
                            {
                                Session["branchId"] = dt.Rows[0]["branchId"].ToString();
                                Session["slotExist"] = dt.Rows[0]["slotExist"].ToString();
                                Session["multiBook"] = dt.Rows[0]["multiBook"].ToString();
                                Session["OneBranch"] = "true";
                                //GetOwnerId(Session["branchId"].ToString());
                            }
                        }
                        else
                        {
                            gvBranchList.DataBind();
                            ddlBranch.DataBind();
                        }
                    }
                    else
                    {
                        gvBranchList.DataBind();
                        ddlBranch.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlBranch.Items.Insert(0, new ListItem("Select", "0"));
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
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
    #region Branch click
    protected void lnkBtnBranch_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;

        Label lblbranchId = (Label)gvrow.FindControl("lblbranchId");
        Label lblbranchName = (Label)gvrow.FindControl("lblbranchName");
        Label lblbranchOptions = (Label)gvrow.FindControl("lblbranchOptions");
        Session["branchName"] = lblbranchName.Text;
        GetOwnerId(lblbranchId.Text);

    }
    #endregion
    #region Get Owner Id
    public void GetOwnerId(string branchId)
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
                sUrl += branchId;
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        List<BranchMasterClass> Branch = JsonConvert.DeserializeObject<List<BranchMasterClass>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Branch);

                        Session["parkingName"] = dt.Rows[0]["parkingName"].ToString();
                        Session["parkingOwnerId"] = dt.Rows[0]["parkingOwnerId"].ToString();
                        Session["branchName"] = dt.Rows[0]["branchName"].ToString();
                        Session["branchId"] = dt.Rows[0]["branchId"].ToString();
                        Session["slotExist"] = dt.Rows[0]["slotExist"].ToString();
                        Session["multiBook"] = dt.Rows[0]["multiBook"].ToString();
                        Session["branchOptions"] = dt.Rows[0]["branchOptions"].ToString();
                        Session["ShowPage"] = "1";
                        if (Session["branchOptions"].ToString() == "Y")
                        {

                            var firstItem = Branch.ElementAt(0);
                            var lst1 = firstItem.branchOptionDetails.ToList();
                            DataTable branchOptionDetails = ConvertToDataTable(lst1);
                            Session["blockOption"] = branchOptionDetails.Rows[0]["blockOption"].ToString();
                            Session["floorOption"] = branchOptionDetails.Rows[0]["floorOption"].ToString();
                            Session["slotsOption"] = branchOptionDetails.Rows[0]["slotsOption"].ToString();
                            Response.Redirect("~/DashBoard.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("~/PO_Preference.aspx", false);

                        }
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



    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {       
        GetOwnerId(ddlBranch.SelectedValue);
        Session["Branch"] = "1";
    }
    #region Owner Master Class
    public class OwnerMaster
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string parkingName { get; set; }
        public string shortName { get; set; }
        public string founderName { get; set; }
        public string logoUrl { get; set; }
        public string websiteUrl { get; set; }
        public string gstNumber { get; set; }
        public string placeType { get; set; }
    }
    #endregion
    #region Branch  Master Class 
    public class BranchMasterClass
    {
        public String branchId { get; set; }
        public string branchName { get; set; }
        public string shortName { get; set; }
        public String parkingOwnerId { get; set; }
        public string parkingName { get; set; }
        public string blockOption { get; set; }
        public string slotExist { get; set; }
        public string multiBook { get; set; }
        public string slotsOption { get; set; }
        public string branchOptions { get; set; }
        public string createdDate { get; set; }
        public List<branchOptionDetails> branchOptionDetails { get; set; }

    }
    public class branchOptionDetails
    {
        public string parkingOwnerConfigId { get; set; }
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockOption { get; set; }
        public string employeeOption { get; set; }
        public string floorOption { get; set; }
        public string slotsOption { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
    }

    #endregion

}