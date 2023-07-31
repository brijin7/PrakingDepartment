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
public partial class Master_Login_BranchLogin : System.Web.UI.Page
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
            Session["ShowPage"] = "0";
            Session["branchId"] = "";
            Session["branchName"] = "";
            Session["slotExist"] = "";
            Session["multiBook"] = "";
            Session["isBookCheckInAvailable"] = "";
            //Booking
            Session["BKvehicleType"] = "";
            Session["BKvehicleTypeName"] = "";
            Session["BKvehiclePlaceHolderImageUrl"] = "";
            Session["BKisvehicleNumberRequired"] = "";
            Session["BKitemIndex"] = "";
            //Booking
            BindBranchName();
        }
    }
    #endregion  

    #region Bind Branch
    public void BindBranchName()
    {
        if (Session["UserRole"].ToString() == "SA")
        {
            BindBranchSA();
        }
        else
        {
            BindBranchAdmin();
        }
    }
    #endregion

    #region Bind Branch SA
    public void BindBranchSA()
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
                          + "branchMaster?activeStatus=A&approvalStatus=A";

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
                        if (dt.Rows.Count > 0)
                        {
                            gvBranchList.DataSource = dt;
                            //ddlbranch.DataValueField = "branchId";
                            //ddlbranch.DataTextField = "branchName";
                            gvBranchList.DataBind();

                        }
                        else
                        {
                            gvBranchList.DataBind();
                            //ddlbranch.DataBind();
                        }
                        //ddlbranch.Items.Insert(0, new ListItem("Select", "0"));
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

    #region Bind Branch Admin
    public void BindBranchAdmin()
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
                          + "branchMaster?activeStatus=A&approvalStatus=A&parkingOwnerId=";
                sUrl += Session["parkingOwnerId"].ToString().Trim();
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

                        if (dt.Rows.Count > 0)
                        {
                            gvBranchList.DataSource = dt;
                            gvBranchList.DataBind();
                            Session["parkingName"] = dt.Rows[0]["parkingName"].ToString();
                            Session["OneBranch"] = "false";
                            if (dt.Rows.Count == 1)
                            {
                                Session["branchId"] = dt.Rows[0]["branchId"].ToString();
                                Session["slotExist"] = dt.Rows[0]["slotExist"].ToString();
                                Session["multiBook"] = dt.Rows[0]["multiBook"].ToString();
                                Session["isBookCheckInAvailable"] = dt.Rows[0]["isBookCheckInAvailable"].ToString();
                                Session["OneBranch"] = "true";
                                GetOwnerId(Session["branchId"].ToString());
                            }


                        }
                        else
                        {
                            gvBranchList.DataBind();
                        }
                    }
                    else
                    {
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
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        Session["parkingName"] = dt.Rows[0]["parkingName"].ToString();
                        Session["parkingOwnerId"] = dt.Rows[0]["parkingOwnerId"].ToString();
                        Session["branchName"] = dt.Rows[0]["branchName"].ToString();
                        Session["branchId"] = dt.Rows[0]["branchId"].ToString();
                        Session["slotExist"] = dt.Rows[0]["slotExist"].ToString();
                        Session["multiBook"] = dt.Rows[0]["multiBook"].ToString();
                        Session["isBookCheckInAvailable"] = dt.Rows[0]["isBookCheckInAvailable"].ToString();
                        
                        Session["branchOptions"] = dt.Rows[0]["branchOptions"].ToString();
                        Session["ShowPage"] = "1";
                        if (Session["branchOptions"].ToString() == "Y")
                        {
                            List<BranchMasterClass> Branch = JsonConvert.DeserializeObject<List<BranchMasterClass>>(ResponseMsg);
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
        public string isBookCheckInAvailable { get; set; }
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