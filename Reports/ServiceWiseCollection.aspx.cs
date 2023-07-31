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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Reports_ServiceWiseCollection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx", true);
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
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //txtTodate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            BindUser();
            GetPaymentType();
            BindVehicle();
            BindGvCollection();
        }
    }
    #region User
    public void BindUser()
    {
        try
        {
            ddlUserName.Items.Clear();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                        + "userMaster?userRole=E&activeStatus=A&branchId=" + Session["branchId"].ToString().Trim() + "&type=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<menuOptionAccess> dt = JsonConvert.DeserializeObject<List<menuOptionAccess>>(ResponseMsg);
                        ddlUserName.DataSource = dt;
                        ddlUserName.DataValueField = "userId";
                        ddlUserName.DataTextField = "userName";
                        ddlUserName.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlUserName.Items.Insert(0, new ListItem("All", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Get Payment Type
    public void GetPaymentType()
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
                          + "configMaster?activestatus=A&configTypename=Payment";
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
                            ddlPaymentType.DataSource = dt;
                            ddlPaymentType.DataValueField = "configId";
                            ddlPaymentType.DataTextField = "configName";
                            ddlPaymentType.DataBind();

                        }
                        else
                        {
                            ddlPaymentType.DataBind();
                        }
                        ddlPaymentType.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlPaymentType.ClearSelection();
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
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Bind Vehicle
    public void BindVehicle()
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
                      + "vehicleConfigMaster?branchId=" + Session["branchId"].ToString() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
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
                            ddlVehicleType.DataSource = dt;
                            ddlVehicleType.DataValueField = "vehicleConfigId";
                            ddlVehicleType.DataTextField = "vehicleName";
                            ddlVehicleType.DataBind();

                        }
                        else
                        {

                        }

                        ddlVehicleType.Items.Insert(0, new ListItem("All", "0"));
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        return;
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
    #region Bind Collection
    public void BindGvCollection()
    {
        DateTime Fromdate = Convert.ToDateTime(txtFromDate.Text);
        //DateTime Todate = Convert.ToDateTime(txtTodate.Text);
        string booking = string.Empty;
        try
        {
            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
                sUrl = Session["BaseUrl"].ToString().Trim()
                    + "ServiceReports?fromDate=" + Fromdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm") + "&toDate=" + Fromdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm") + "&Type=RE&branchId=" + Session["branchId"].ToString() + "";

                sUrl += "&paymentType=" + ddlPaymentType.SelectedValue + "&userId=" + ddlUserName.SelectedValue + "&category=" + ddlVehicleType.SelectedValue + "";
                if (ddlServices.SelectedValue != "0")
                {
                    sUrl += "&booking=" + ddlServices.SelectedValue + "";

                }
                else
                {
                    sUrl += "&booking="+ booking.Trim() + "";

                }
                
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        List<servicewisereports> dt = JsonConvert.DeserializeObject<List<servicewisereports>>(ResponseMsg);
                        if (dt.Count > 0)
                        {
                            gvServiceWiseReport.Visible = true;
                            divGv.Visible = true;
                            decimal TotalPrice = dt.AsEnumerable().Sum(
                               row => decimal.Parse(row.TotalAmount.ToString()));

                            int TotalCount = dt.AsEnumerable().Count();
                            DataTable Collection = new DataTable();
                            Collection.Columns.Add("Particular", typeof(String));
                            Collection.Columns.Add("Count", typeof(Int64));
                            Collection.Columns.Add("TotalAmount", typeof(Double));
                            Collection.Rows.Add("Parking Revenue", TotalCount, TotalPrice);
                            gvServiceWiseReport.DataSource = Collection;
                            gvServiceWiseReport.DataBind();
                            int TotalCounts = Collection.AsEnumerable().Sum(row => Convert.ToInt32(row.Field<Int64>("Count")));
                            decimal TotalAmount = Collection.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<Double>("TotalAmount")));

                            gvServiceWiseReport.FooterRow.Cells[1].Text = "Total";
                            gvServiceWiseReport.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            gvServiceWiseReport.FooterRow.Cells[2].Text = TotalCount.ToString();
                            gvServiceWiseReport.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                            gvServiceWiseReport.FooterRow.Cells[3].Text = TotalAmount.ToString("N2");
                            gvServiceWiseReport.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;

                        }
                        else
                        {
                            divGv.Visible = false;
                            gvServiceWiseReport.DataSource = null;
                            gvServiceWiseReport.DataBind();
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                        }
                    }
                    else
                    {
                        divGv.Visible = false;
                        gvServiceWiseReport.DataSource = null;
                        gvServiceWiseReport.DataBind();
                        gvServiceWiseReport.Visible = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    divGv.Visible = false;
                    gvServiceWiseReport.DataSource = null;
                    gvServiceWiseReport.DataBind();
                    gvServiceWiseReport.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }

    }

    public class gvCollection
    {
        public string Particular { get; set; }
        public string Count { get; set; }
        public string TotalAmount { get; set; }

    }

    public class servicewisereports
    {
        public string bookingPassId { get; set; }
        public string bookingType { get; set; }
        public string booking { get; set; }
        public string paymentType { get; set; }

        public string userId { get; set; }
        public string category { get; set; }
        public string paymentTypeName { get; set; }
        public string userName { get; set; }
        public string categoryName { get; set; }
        public string TotalAmount { get; set; }
        public string Amount { get; set; }
        public string taxAmount { get; set; }
        public string Date { get; set; }

    }

    #endregion
    #region Detailed Report Click
    protected void btnDetailedPrint_Click(object sender, EventArgs e)
    {
        string booking = string.Empty;

        ReportDocument objReportDocument = new ReportDocument();
        ExportOptions objReportExport = new ExportOptions();
        DiskFileDestinationOptions objReportDiskOption = new DiskFileDestinationOptions();

        string sFilePath = string.Empty;
        sFilePath = "Report" + Session.SessionID.ToString() + ".pdf";
        string rFileName = string.Empty;

        DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
       // DateTime ToDate = Convert.ToDateTime(txtTodate.Text);

        try
        {
            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                sUrl = Session["BaseUrl"].ToString().Trim()
                     + "ServiceReports?fromDate=" + FromDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm") + "&toDate=" + FromDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm") + "&Type=RE&branchId=" + Session["branchId"].ToString() + "";

                sUrl += "&paymentType=" + ddlPaymentType.SelectedValue + "&userId=" + ddlUserName.SelectedValue + "&category=" + ddlVehicleType.SelectedValue + "";
                if (ddlServices.SelectedValue != "0")
                {
                    sUrl += "&booking=" + ddlServices.SelectedValue + "";
                }
                else
                {
                    sUrl += "&booking=" + booking.Trim() + "";
                }

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
                            
                            objReportDocument.Load(Server.MapPath("DetailedReport.rpt"));
                            objReportDocument.SetDataSource(dt);
                            objReportDocument.SetParameterValue(0, "Branch - " + Session["branchName"].ToString().Trim());
                            //objReportDocument.SetParameterValue(1, ddlServices.SelectedItem.Text + " Service - " + ddlUserName.SelectedItem.Text.Trim() + " Sales Entries details Between " + FromDate.ToString("dd-MM-yyyy").Trim() + " And " + ToDate.ToString("dd-MM-yyyy").Trim());
                            objReportDocument.SetParameterValue(1, ddlServices.SelectedItem.Text + " Service - " + ddlUserName.SelectedItem.Text.Trim() + " Sales Entries On " + FromDate.ToString("dd-MM-yyyy").Trim());
                            objReportDiskOption.DiskFileName = Server.MapPath(sFilePath);
                            objReportExport = objReportDocument.ExportOptions;
                            objReportExport.ExportDestinationOptions = objReportDiskOption;
                            objReportExport.ExportDestinationType = ExportDestinationType.DiskFile;
                            objReportExport.ExportFormatType = ExportFormatType.PortableDocFormat;
                            objReportDocument.Export();
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(Server.MapPath(sFilePath));
                            Response.Flush();
                            Response.Close();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "';", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + ResponseMsg.ToString() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('" + response.ReasonPhrase.ToString() + "');", true);
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "") + "');", true);
            return;
        }
        finally
        {
            if (System.IO.File.Exists(Server.MapPath(sFilePath)) == true)
            {
                System.IO.File.Delete(Server.MapPath(sFilePath));
            }

            objReportDocument.Dispose();

            objReportDiskOption = null;
            objReportDocument = null;
            objReportExport = null;
            GC.Collect();
        }


    }
    #endregion
    #region Selected Index Changed Services,Paymenttype,Username,VehicleType
    protected void ddlServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGvCollection();
    }

    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGvCollection();

    }

    protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGvCollection();
    }

    protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGvCollection();

    }

    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        BindGvCollection();
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
                        var Option = lst1.Where(x => x.optionName == "servicebaseReport" && x.MenuOptionAccessActiveStatus == "A")
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

}