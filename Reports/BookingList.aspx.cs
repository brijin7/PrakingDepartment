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

public partial class Reports_BookingList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtTodate.Text = DateTime.Now.ToString("yyyy-MM-dd");          
        }
    }

    #region Bind Collection
    public void BindGvCollection()
    {
        DateTime Fromdate = Convert.ToDateTime(txtFromDate.Text);
        DateTime Todate = Convert.ToDateTime(txtTodate.Text);
       
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
                      + "bookingMaster?fromDate=" + Fromdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm") + "&toDate=" + Todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm") + "&type=RE&branchId=" + Session["branchId"].ToString() + "";
              
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
                            divGv.Visible = true;
                            decimal TotalPrice = dt.AsEnumerable().Sum(
                               row => decimal.Parse(row["TotalAmount"].ToString()));
                          
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('"+ResponseMsg.ToString()+"');", true);
                        }
                    }
                    else
                    {
                        divGv.Visible = false;
                        gvServiceWiseReport.DataSource = null;
                        gvServiceWiseReport.DataBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "infoalert('"+ResponseMsg.ToString()+"');", true);
                    }
                }
                else
                {
                    divGv.Visible = false;
                    gvServiceWiseReport.DataSource = null;
                    gvServiceWiseReport.DataBind();
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

    #endregion

    #region Selected Index Changed   
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