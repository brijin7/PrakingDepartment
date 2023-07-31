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

public partial class DashBoard : System.Web.UI.Page
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
            if (Session["UserRole"].ToString() == "A")
            {
                Response.Redirect("~/Login/BranchLogin.aspx");
            }
            else if (Session["UserRole"].ToString() == "E")
            {
                Response.Redirect("~/DashBoard.aspx", false);
            }
        }
        if (txtfrmdate.Text == "" & txttodate.Text == "")
        {
            txtfrmdate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            txttodate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
        }

        if (!IsPostBack)
        {
            if (Session["Branch"].ToString() == "0")
            {

                if (Session["UserRole"].ToString() == "SA")
                {
                    BindBranch();
                    BindCountSA();
                    divddlBranch.Visible = true;
                }
                else
                {
                    BindCount();
                    divddlBranch.Visible = false;
                }
            }
            else
            {
                BindCount();
                divddlBranch.Visible = false;
            }
            BindCountEnable();
        }
    }
    #endregion
    #region Count Enable 
    public void BindCountEnable()
    {
        if (lblAvailable.Text == "0")
        {
            lblAvailable.Enabled = false;
        }
        else
        {
            lblAvailable.Enabled = true;
        }
        if (lblBooked.Text == "0")
        {
            lblBooked.Enabled = false;
        }
        else
        {
            lblBooked.Enabled = true;
        }
        if (lblCheckOut.Text == "0")
        {
            lblCheckOut.Enabled = false;
        }
        else
        {
            lblCheckOut.Enabled = true;
        }
        if (lblCheckIn.Text == "0")
        {
            lblCheckIn.Enabled = false;
        }
        else
        {
            lblCheckIn.Enabled = true;
        }

    }
    #endregion
    #region Bind Count 
    public void BindCount()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?branchId=" + Session["branchId"].ToString().Trim() + "&type=C&fromDate=" + Fromdate + "" +
                      "&toDate=" + Todate + "";

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
                            lblAvailable.Text = dt.Rows[0]["available"].ToString();
                            lblTotalSlot.Text = dt.Rows[0]["totalCount"].ToString();
                            lblCheckIn.Text = dt.Rows[0]["checkedInCount"].ToString();
                            lblCheckOut.Text = dt.Rows[0]["checkedOutCount"].ToString();
                            lblBooked.Text = dt.Rows[0]["bookedCount"].ToString();
                            BindAmount();
                        }
                        else
                        {

                        }
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
    #region Bind Amount 
    public void BindAmount()
    {
        try

        {
            DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
            string Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            DateTime todate = Convert.ToDateTime(txttodate.Text);
            string Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?branchId=" + Session["branchId"].ToString().Trim() + "&type=DG&fromDate=" + Fromdate + "" +
                      "&toDate=" + Todate + "";

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
                            //hfTotalAmount.Value = dt.Rows[0]["TotalAmount"].ToString();
                            hfPaidAmount.Value = dt.Rows[0]["bookingAmount"].ToString();
                            hfRemainingAmount.Value = dt.Rows[0]["extrafeesAmount"].ToString();

                        }
                        else
                        {

                        }
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
    #region Bind Count  SA
    public void BindCountSA()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }

            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (ddlBranch.SelectedValue == "0" || ddlBranch.SelectedValue == "")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                    + "bookingMaster?type=C&fromDate=" + Fromdate + "" +
                    "&toDate=" + Todate + "";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?branchId=" + ddlBranch.SelectedValue + "&type=C&fromDate=" + Fromdate + "" +
                      "&toDate=" + Todate + "";
                }
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
                            lblAvailable.Text = dt.Rows[0]["available"].ToString();
                            lblTotalSlot.Text = dt.Rows[0]["totalCount"].ToString();
                            lblCheckIn.Text = dt.Rows[0]["checkedInCount"].ToString();
                            lblCheckOut.Text = dt.Rows[0]["checkedOutCount"].ToString();
                            lblBooked.Text = dt.Rows[0]["bookedCount"].ToString();
                            BindAmountSA();
                        }
                        else
                        {

                        }
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
    #region Bind Amount SA
    public void BindAmountSA()
    {
        try
        {
            DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
            string Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            DateTime todate = Convert.ToDateTime(txttodate.Text);
            string Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (ddlBranch.SelectedValue == "0" || ddlBranch.SelectedValue == "")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                     + "bookingMaster?type=DG&fromDate=" + Fromdate + "" +
                     "&toDate=" + Todate + "";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                     + "bookingMaster?branchId=" + ddlBranch.SelectedValue + "&type=DG&fromDate=" + Fromdate + "" +
                     "&toDate=" + Todate + "";
                }


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
                            //hfTotalAmount.Value = dt.Rows[0]["TotalAmount"].ToString();
                            hfPaidAmount.Value = dt.Rows[0]["bookingAmount"].ToString();
                            hfRemainingAmount.Value = dt.Rows[0]["extrafeesAmount"].ToString();

                        }
                        else
                        {

                        }
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
        if (Session["UserRole"].ToString() == "SA")
        {
            BindCountSA();
        }
        else
        {
            BindCount();
        }
        BindCountEnable();
    }
    #endregion
    #region Bind Branch Admin
    public void BindBranch()
    {
        try
        {
            using (var client = new HttpClient())
            {
                string sUrl = string.Empty;
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (Session["parkingOwnerId"].ToString() == "0")
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                       + "branchMaster?activeStatus=A&approvalStatus=A";
                }
                else
                {
                    sUrl = Session["BaseUrl"].ToString().Trim()
                             + "branchMaster?activeStatus=A&approvalStatus=A&parkingOwnerId=";
                    sUrl += Session["parkingOwnerId"].ToString().Trim();
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

                        dt.Columns.Add(new DataColumn("branch", System.Type.GetType("System.String"), "shortName + ' ~ ' + branchName"));
                        if (dt.Rows.Count > 0)
                        {
                            ddlBranch.DataSource = dt;
                            ddlBranch.DataValueField = "branchId";
                            ddlBranch.DataTextField = "branch";
                            ddlBranch.DataBind();
                        }
                        else
                        {

                        }
                        ddlBranch.Items.Insert(0, new ListItem("All", "0"));

                    }
                    else
                    {
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

    #region Bind Booked  Count 
    #region Booked Count Click
    protected void lblBooked_Click(object sender, EventArgs e)
    {
        lblGvCount.Text = "Booked";
        divBookedGridView.Visible = true;

        if (Session["Branch"].ToString() == "0")
        {
            if (Session["UserRole"].ToString() == "SA")
            {
                BindBookedCountSA();
            }
            else
            {
                BindBookedCount();
            }
        }
        else
        {
            BindBookedCount();
        }

    }
    #endregion
    #region Bind Booked  Count 
    public void BindBookedCount()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }

            Session["FromDate"] = Fromdate;
            Session["Todate"] = Todate;
            Session["Type"] = "CU";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick",
            "<script>window.open('DashBoardDetails.aspx');</script>", false);

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Bind Booked  Count  SA
    public void BindBookedCountSA()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }

            Session["FromDate"] = Fromdate;
            Session["Todate"] = Todate;
            Session["ddlBranch"] = ddlBranch.SelectedValue;
            Session["Type"] = "CU";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick",
            "<script>window.open('DashBoardDetails.aspx');</script>", false);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #endregion
    #region Check Out Count
    #region Check Out Count Click
    protected void lblCheckOut_Click(object sender, EventArgs e)
    {
        lblGvCount.Text = "Check Out";
        divCheckOutGridView.Visible = true;

        if (Session["Branch"].ToString() == "0")
        {
            if (Session["UserRole"].ToString() == "SA")
            {
                BindCheckOutCountSA();
            }
            else
            {
                BindCheckOutCount();
            }
        }
        else
        {
            BindCheckOutCount();
        }

    }
    #endregion
    #region Bind CheckOut  Count 
    public void BindCheckOutCount()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }

            Session["FromDate"] = Fromdate;
            Session["Todate"] = Todate;
            Session["Type"] = "CO";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick",
            "<script>window.open('DashBoardDetails.aspx');</script>", false);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Bind CheckOut  Count  SA
    public void BindCheckOutCountSA()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }


            Session["FromDate"] = Fromdate;
            Session["Todate"] = Todate;
            Session["ddlBranch"] = ddlBranch.SelectedValue;
            Session["Type"] = "CO";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick",
               "<script>window.open('DashBoardDetails.aspx');</script>", false);

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #endregion
    #region Check In Count

    #region Check In Count Click
    protected void lblCheckIn_Click(object sender, EventArgs e)
    {
        lblGvCount.Text = "Check In";
        divCheckInGridView.Visible = true;

        if (Session["Branch"].ToString() == "0")
        {
            if (Session["UserRole"].ToString() == "SA")
            {
                BindCheckInCountSA();
            }
            else
            {
                BindCheckInCount();
            }
        }
        else
        {
            BindCheckInCount();
        }

    }
    #endregion
    #region Bind CheckIn Count 
    public void BindCheckInCount()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            Session["FromDate"] = Fromdate;
            Session["Todate"] = Todate;
            Session["Type"] = "CI";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick",
            "<script>window.open('DashBoardDetails.aspx');</script>", false);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Bind CheckIn Count SA
    public void BindCheckInCountSA()
    {
        try
        {
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            if (txtfrmdate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm") &
                txttodate.Text == DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm"))
            {
                Fromdate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                Todate = DateTime.Now.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }
            else
            {
                DateTime frmdate = Convert.ToDateTime(txtfrmdate.Text);
                Fromdate = frmdate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
                DateTime todate = Convert.ToDateTime(txttodate.Text);
                Todate = todate.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            }

            Session["FromDate"] = Fromdate;
            Session["Todate"] = Todate;
            Session["ddlBranch"] = ddlBranch.SelectedValue;
            Session["Type"] = "CI";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick",
            "<script>window.open('DashBoardDetails.aspx');</script>", false);


        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #endregion
    #region Available Count Click
    protected void lblAvailable_Click(object sender, EventArgs e)
    {
        try
        {
            lblGvCount.Text = "Available list";
            divAvailableGridView.Visible = true;
            if (Session["Branch"].ToString() == "0")
            {
                if (Session["UserRole"].ToString() == "SA")
                {
                    Session["Type"] = "A";
                    Session["ddlBranch"] = ddlBranch.SelectedValue;
                }
                else
                {
                    Session["Type"] = "A";
                }
            }
            else
            {
                Session["Type"] = "A";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick",
            "<script>window.open('DashBoardDetails.aspx');</script>", false);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region  Close Click
    protected void linkclose_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
        divBookedGridView.Visible = false;
        divCheckInGridView.Visible = false;
        divCheckOutGridView.Visible = false;
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