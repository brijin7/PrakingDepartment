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

public partial class CheckInandOut_CheckInCheckOut : System.Web.UI.Page
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
            ViewState["BlockCount"] = "0";
            ViewState["FloorCount"] = "0";
            BindBlock();
            Header.InnerText = "Check In";
            Headerspan.InnerText = "Details";
            txtBookingId.Focus();
            if (ViewState["BlockCount"].ToString() == "0" || ViewState["FloorCount"].ToString() == "0")
            {
                divBlockFloor.Visible = true;
            }
            else
            {
                divBlockFloor.Visible = false;
            }
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }

    }
    #endregion
    #region Bind Block
    public void BindBlock()
    {
        ViewState["floorId"] = "";
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "blockMaster?approvalStatus=A&activeStatus=A&branchId=";
                sUrl += Session["branchId"].ToString().Trim();
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
                            ddlblock.DataSource = dt;
                            ddlblock.DataValueField = "blockId";
                            ddlblock.DataTextField = "blockName";
                            ddlblock.DataBind();
                            if (dt.Rows.Count == 1)
                            {
                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                                BindFloorName();
                                ViewState["BlockCount"] = "1";
                            }
                            else
                            {

                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                                BindFloorName();
                            }
                        }
                        else
                        {
                            ddlblock.DataBind();
                        }
                        //ddlblock.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlblock.ClearSelection();
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
    #region Block SelectedIndexChanged
    protected void ddlblock_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["floorId"] = "";
        ddlfloor.Items.Clear();
        BindFloorName();

    }
    #endregion
    #region  Bind Floor
    public void BindFloorName()
    {
        ViewState["floorId"] = "";
        ddlfloor.Items.Clear();
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "floorMaster?activeStatus=A&blockId=";
                sUrl += ddlblock.SelectedValue;
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
                            item.Property("floorVehicleDetails").Remove();
                            item.Property("floorFeaturesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {
                            ddlfloor.DataSource = dt;
                            ddlfloor.DataValueField = "floorId";
                            ddlfloor.DataTextField = "floorName";
                            ddlfloor.DataBind();
                            if (dt.Rows.Count == 1)
                            {
                                ViewState["FloorCount"] = "1";
                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                                ViewState["floorId"] = ddlfloor.SelectedValue;
                                divForm.Visible = true;
                                divBtncheckinandout.Visible = true;
                                divBTitle.Visible = true;
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }
                            else
                            {

                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                                ViewState["floorId"] = ddlfloor.SelectedValue;
                                divForm.Visible = true;
                                divBtncheckinandout.Visible = true;
                                divBTitle.Visible = true;
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }
                        }
                        else
                        {
                            ddlfloor.Items.Clear();
                        }
                        ddlfloor.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        gvCheckIn.Visible = false;
                        gvCheckOut.Visible = false;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlfloor.Items.Clear();
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
    #region Floor SelectedIndexChanged
    protected void ddlfloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlfloor.SelectedIndex == 0)
        {
            divBtncheckinandout.Visible = false;
            divBTitle.Visible = false;
            divCheckIn.Visible = false;
            divGvCheckOut.Visible = false;
        }
        else
        {
            divForm.Visible = true;
            divBtncheckinandout.Visible = true;
            divBTitle.Visible = true;
            ViewState["floorId"] = ddlfloor.SelectedValue;
            if (rbtnTypeFP.SelectedValue == "P")
            {
                BindCheckInListPass();
            }
            else
            {
                BindCheckInList();
            }
        }

    }
    #endregion
    #region  Bind Floor ID
    public void BindFloorId()
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
                            + "floorMaster?branchId=" + Session["branchId"].ToString();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("floorVehicleDetails").Remove();
                            item.Property("floorFeaturesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["floorId"] = dt.Rows[0]["floorId"].ToString();
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
    #region Booking Or Pass Radio Button Selected Index Changed
    protected void rbtnTypeFP_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBookingId.Text = "";
        lblGridIn.Visible = false;
        lblGridIn.Text = "";
        lblout.Visible = false;
        lblout.Text = "";
        lblAlreadyOut.Visible = false;
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Visible = false;
        Header.InnerText = "Check In";
        Headerspan.InnerText = "Details";
        lblAlreadyIn.Text = "";
        if (rbtnTypeFP.SelectedValue == "P")
        {
            BindCheckInListPass();
        }
        else
        {
            BindCheckInList();
        }
        btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
        btnCheckIn.Style.Add("color", "white");
        btnCheckIn.Style.Add("border", "1px solid var(--blue)");
        btnCheckOut.Style.Add("background-color", "white");
        btnCheckOut.Style.Add("color", "var(--danger)");
        btnCheckOut.Style.Add("border", " 1px solid var(--danger)");

    }
    #endregion
    #region Btn Bind Check In and GridView For Check In List
    #region  Bind Check In
    public void BindCheckInList()
    {
        try
        {
            ViewState["BindCheckInList"] = "CheckIn";
            ViewState["BindCheckOutList"] = "";
            divScreen.Style.Add("background-color", "#6ce488a8");
            Header.InnerText = "Check In";
            Headerspan.InnerText = "Details";

            Header.Style.Add("color", "#106d25;");
            divCheckIn.Visible = true;
            divGvCheckOut.Visible = false;
            gvCheckIn.Visible = true;
            gvCheckInPass.Visible = false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "bookingMaster?&type=I&floorId=" + ViewState["floorId"].ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<OutDetails> Inout = JsonConvert.DeserializeObject<List<OutDetails>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Inout);

                        if (dt.Rows.Count > 0)
                        {

                            lblgridmsgIn.Visible = false;
                            lblgridmsgIn.Text = "";
                            gvCheckIn.DataSource = Inout;
                            gvCheckIn.DataBind();
                            txtBookingId.Focus();
                            btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckIn.Style.Add("color", "white");
                            btnCheckIn.Style.Add("border", "1px solid var(--blue)");
                            btnCheckOut.Style.Add("background-color", "white");
                            btnCheckOut.Style.Add("color", "var(--danger)");
                            btnCheckOut.Style.Add("border", " 1px solid var(--danger)");
                        }
                        else
                        {
                            gvCheckIn.DataSource = null;
                            gvCheckIn.DataBind();
                            btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckIn.Style.Add("color", "white");
                            btnCheckIn.Style.Add("border", "1px solid var(--blue)");
                            btnCheckOut.Style.Add("background-color", "white");
                            btnCheckOut.Style.Add("color", "var(--danger)");
                            btnCheckOut.Style.Add("border", " 1px solid var(--danger)");
                            lblgridmsg.Visible = true;
                            lblgridmsgIn.Text = ResponseMsg.ToString().Trim();
                            txtBookingId.Focus();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        }
                    }
                    else
                    {
                        gvCheckIn.DataSource = null;
                        gvCheckIn.DataBind();
                        btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
                        btnCheckIn.Style.Add("color", "white");
                        btnCheckIn.Style.Add("border", "1px solid var(--blue)");
                        btnCheckOut.Style.Add("background-color", "white");
                        btnCheckOut.Style.Add("color", "var(--danger)");
                        btnCheckOut.Style.Add("border", " 1px solid var(--danger)");
                        lblgridmsgIn.Visible = true;
                        lblgridmsgIn.Text = ResponseMsg.ToString().Trim();
                        txtBookingId.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    public void BindCheckInListPass()
     {
        try
        {
            ViewState["BindCheckInList"] = "CheckIn";
            ViewState["BindCheckOutList"] = "";
            divScreen.Style.Add("background-color", "#6ce488a8");
            Header.InnerText = "Check In";
            Headerspan.InnerText = "Details";

            Header.Style.Add("color", "#106d25;");
            divCheckIn.Visible = true;
            divGvCheckOut.Visible = false;
            gvCheckIn.Visible = false;
            gvCheckInPass.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "passBooking?&type=I&floorId=" + ViewState["floorId"].ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<OutDetails> Inout = JsonConvert.DeserializeObject<List<OutDetails>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Inout);

                        if (dt.Rows.Count > 0)
                        {

                            lblgridmsgIn.Visible = false;
                            lblgridmsgIn.Text = "";
                            gvCheckInPass.DataSource = Inout;
                            gvCheckInPass.DataBind();
                            txtBookingId.Focus();
                            btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckIn.Style.Add("color", "white");
                            btnCheckIn.Style.Add("border", "1px solid var(--blue)");
                            btnCheckOut.Style.Add("background-color", "white");
                            btnCheckOut.Style.Add("color", "var(--danger)");
                            btnCheckOut.Style.Add("border", " 1px solid var(--danger)");
                        }
                        else
                        {
                            gvCheckInPass.DataSource = null;
                            gvCheckInPass.DataBind();
                            btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckIn.Style.Add("color", "white");
                            btnCheckIn.Style.Add("border", "1px solid var(--blue)");
                            btnCheckOut.Style.Add("background-color", "white");
                            btnCheckOut.Style.Add("color", "var(--danger)");
                            btnCheckOut.Style.Add("border", " 1px solid var(--danger)");
                            lblgridmsg.Visible = true;
                            lblgridmsgIn.Text = ResponseMsg.ToString().Trim();
                            txtBookingId.Focus();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        }
                    }
                    else
                    {
                        gvCheckInPass.DataSource = null;
                        gvCheckInPass.DataBind();
                        btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
                        btnCheckIn.Style.Add("color", "white");
                        btnCheckIn.Style.Add("border", "1px solid var(--blue)");
                        btnCheckOut.Style.Add("background-color", "white");
                        btnCheckOut.Style.Add("color", "var(--danger)");
                        btnCheckOut.Style.Add("border", " 1px solid var(--danger)");
                        lblgridmsgIn.Visible = true;
                        lblgridmsgIn.Text = ResponseMsg.ToString().Trim();
                        txtBookingId.Focus();
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
    protected void gvCheckIn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var Options = e.Row.DataItem as OutDetails;
            var dataList = e.Row.FindControl("DataList1") as DataList;
            dataList.DataSource = Options.slotDetails;
            dataList.DataBind();
        }
    }
    #endregion
    #region btnCheckIN
    protected void btnCheckIn_Click(object sender, EventArgs e)
    {
        txtBookingId.Text = "";
        lblGridIn.Visible = false;
        lblGridIn.Text = "";
        lblout.Visible = false;
        lblout.Text = "";
        lblAlreadyOut.Visible = false;
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Visible = false;
        Header.InnerText = "Check In";
        Headerspan.InnerText = "Details";
        lblAlreadyIn.Text = "";
        if(rbtnTypeFP.SelectedValue == "P")
        {
            BindCheckInListPass();
        }
        else
        {
            BindCheckInList();
        }
        btnCheckIn.Style.Add("background-color", "rgb( 33, 150, 243)");
        btnCheckIn.Style.Add("color", "white");
        btnCheckIn.Style.Add("border", "1px solid var(--blue)");
        btnCheckOut.Style.Add("background-color", "white");
        btnCheckOut.Style.Add("color", "var(--danger)");
        btnCheckOut.Style.Add("border", " 1px solid var(--danger)");

    }
    #endregion
    #endregion
    #region Btn Bind Check Out and GridView For Check Out List
    #region Bind Check Out
    public void BindCheckOutList()
    {
        ViewState["BindCheckOutList"] = "CheckOut";
        ViewState["BindCheckInList"] = "";
        divScreen.Style.Add("background-color", "#f58a8a9e");
        Header.InnerText = "Check Out";
        Headerspan.InnerText = "Details";
        Header.Style.Add("color", "#c3263b");
        gvCheckOut.Visible = true;
        gvCheckOutPass.Visible = false;
        try
        {
            divCheckIn.Visible = false;
            divGvCheckOut.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "bookingMaster?type=O&floorId=" + ViewState["floorId"].ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<OutDetails> Inout = JsonConvert.DeserializeObject<List<OutDetails>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Inout);
                        if (dt.Rows.Count > 0)
                        {
                            lblgridmsg.Visible = false;
                            lblgridmsg.Text = "";
                            gvCheckOut.DataSource = Inout;
                            gvCheckOut.DataBind();
                            txtBookingId.Focus();
                            btnCheckIn.Style.Add("background-color", "white");
                            btnCheckIn.Style.Add("color", "#2eab4a");
                            btnCheckIn.Style.Add("border", "1px solid #2eab4a");
                            btnCheckOut.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckOut.Style.Add("color", "white");
                            btnCheckOut.Style.Add("border", " 1px solid var(--blue)");
                        }
                        else
                        {
                            btnCheckIn.Style.Add("background-color", "white");
                            btnCheckIn.Style.Add("color", "#2eab4a");
                            btnCheckIn.Style.Add("border", "1px solid #2eab4a");
                            btnCheckOut.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckOut.Style.Add("color", "white");
                            btnCheckOut.Style.Add("border", " 1px solid var(--blue)");
                            lblgridmsg.Visible = true;
                            lblgridmsg.Text = ResponseMsg.ToString().Trim();
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        gvCheckOut.DataSource = null;
                        gvCheckOut.DataBind();
                        btnCheckIn.Style.Add("background-color", "white");
                        btnCheckIn.Style.Add("color", "#2eab4a");
                        btnCheckIn.Style.Add("border", "1px solid #2eab4a");
                        btnCheckOut.Style.Add("background-color", "rgb( 33, 150, 243)");
                        btnCheckOut.Style.Add("color", "white");
                        btnCheckOut.Style.Add("border", " 1px solid var(--blue)");
                        lblgridmsg.Visible = true;
                        lblgridmsg.Text = ResponseMsg.ToString().Trim();
                        txtBookingId.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    public void BindCheckOutListPass()
    {
        ViewState["BindCheckOutList"] = "CheckOut";
        ViewState["BindCheckInList"] = "";
        divScreen.Style.Add("background-color", "#f58a8a9e");
        Header.InnerText = "Check Out";
        Headerspan.InnerText = "Details";
        Header.Style.Add("color", "#c3263b");
        gvCheckOut.Visible = false;
        gvCheckOutPass.Visible = true;
        try
        {
            divCheckIn.Visible = false;
            divGvCheckOut.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "passBooking?type=O&floorId=" + ViewState["floorId"].ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<OutDetails> Inout = JsonConvert.DeserializeObject<List<OutDetails>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(Inout);
                        if (dt.Rows.Count > 0)
                        {
                            lblgridmsg.Visible = false;
                            lblgridmsg.Text = "";
                            gvCheckOutPass.DataSource = Inout;
                            gvCheckOutPass.DataBind();
                            txtBookingId.Focus();
                            btnCheckIn.Style.Add("background-color", "white");
                            btnCheckIn.Style.Add("color", "#2eab4a");
                            btnCheckIn.Style.Add("border", "1px solid #2eab4a");
                            btnCheckOut.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckOut.Style.Add("color", "white");
                            btnCheckOut.Style.Add("border", " 1px solid var(--blue)");
                        }
                        else
                        {
                            btnCheckIn.Style.Add("background-color", "white");
                            btnCheckIn.Style.Add("color", "#2eab4a");
                            btnCheckIn.Style.Add("border", "1px solid #2eab4a");
                            btnCheckOut.Style.Add("background-color", "rgb( 33, 150, 243)");
                            btnCheckOut.Style.Add("color", "white");
                            btnCheckOut.Style.Add("border", " 1px solid var(--blue)");
                            lblgridmsg.Visible = true;
                            lblgridmsg.Text = ResponseMsg.ToString().Trim();
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        gvCheckOutPass.DataSource = null;
                        gvCheckOutPass.DataBind();
                        btnCheckIn.Style.Add("background-color", "white");
                        btnCheckIn.Style.Add("color", "#2eab4a");
                        btnCheckIn.Style.Add("border", "1px solid #2eab4a");
                        btnCheckOut.Style.Add("background-color", "rgb( 33, 150, 243)");
                        btnCheckOut.Style.Add("color", "white");
                        btnCheckOut.Style.Add("border", " 1px solid var(--blue)");
                        lblgridmsg.Visible = true;
                        lblgridmsg.Text = ResponseMsg.ToString().Trim();
                        txtBookingId.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    protected void gvCheckOut_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var Options = e.Row.DataItem as OutDetails;
            var dataList = e.Row.FindControl("DataList2") as DataList;
            dataList.DataSource = Options.slotDetails;
            dataList.DataBind();
        }
    }
    #endregion
    #region btnCheckOut
    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        txtBookingId.Text = "";
        lblAlreadyOut.Visible = false;
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Visible = false;
        lblAlreadyIn.Text = "";
        lblGridIn.Visible = false;
        lblGridIn.Text = "";
        lblout.Visible = false;
        lblout.Text = "";
        Header.InnerText = "Check Out";
        Headerspan.InnerText = "Details";
        if (rbtnTypeFP.SelectedValue == "P")
        {
            BindCheckOutListPass();
        }
        else
        {
            BindCheckOutList();
        }
        btnCheckIn.Style.Add("background-color", "white");
        btnCheckIn.Style.Add("color", "#2eab4a");
        btnCheckIn.Style.Add("border", "1px solid #2eab4a");
        btnCheckOut.Style.Add("background-color", "rgb( 33, 150, 243)");
        btnCheckOut.Style.Add("color", "white");
        btnCheckOut.Style.Add("border", " 1px solid var(--blue)");
    }
    #endregion
    #endregion
    #region Btn Click Gridview CheckIn and Check Out Details
    #region GridBtnCheck In
    protected void gvbtnCheckIn_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string times;
            string[] date2;
            string slotd = string.Empty;
            DateTime utcTime = DateTime.Now;
            times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            date2 = times.Split(' ');
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblbookingPassIdIn = (Label)gvrow.FindControl("lblbookingPassIdIn");
            Label lblvehicleNumberIn = (Label)gvrow.FindControl("lblvehicleNumberIn");
            Label lblvehicleHeaderId = (Label)gvrow.FindControl("lblvehicleHeaderId");
            var dataList = gvrow.FindControl("DataList1") as DataList;
            if (dataList.Items.Count > 0)
            {
                Label lbluserslotIdIn = dataList.Items[0].FindControl("lbluserslotIdIn") as Label;
                Label lblSlotIdIn = dataList.Items[0].FindControl("lblSlotIdIn") as Label;
                slotd = lblSlotIdIn.Text;
            }

            UpdateCheckInOutDetails(lblvehicleNumberIn.Text, lblvehicleHeaderId.Text, times, null, "I", slotd.Trim(),"","");
            BindCheckInList();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region GridBtnCheck Out
    protected void gvbtnCheckout_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblout.Visible = false;
            string times;
            string[] date2;
            string slotid = string.Empty;
            DateTime utcTime = DateTime.Now;
            times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            date2 = times.Split(' ');
            string[] dayhourlbl;
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            var dataList = gvrow.FindControl("DataList2") as DataList;
            if (dataList.Items.Count > 0)
            {
                Label lbluserslotIdOut = dataList.Items[0].FindControl("lbluserslotIdOut") as Label;
                Label lblSlotIdOut = dataList.Items[0].FindControl("lblSlotIdOut") as Label;
                slotid = lblSlotIdOut.Text;
            }
            ViewState["SlotId"] = slotid.Trim();
            Label lblbookingPassId = (Label)gvrow.FindControl("lblbookingPassId");
            ViewState["BookingId"] = lblbookingPassId.Text;
            Label lblvehicleNumber = (Label)gvrow.FindControl("lblvehicleNumber");
            Label lblvehicleHeaderId = (Label)gvrow.FindControl("lblvehicleHeaderId");
            Label lblinTime = (Label)gvrow.FindControl("lblinTime");
            Label lblvehicleStatus = (Label)gvrow.FindControl("lblvehicleStatus");
            Label lblextendDayHour = (Label)gvrow.FindControl("lblextendDayHour");
            dayhourlbl = lblextendDayHour.Text.Split('-');
            Label lblextendAmount = (Label)gvrow.FindControl("lblextendAmount");
            Label lblextendtax = (Label)gvrow.FindControl("lblextendtax");
            Label lblremainingAmount = (Label)gvrow.FindControl("lblremainingAmount");
            Label lbltotalAmount = (Label)gvrow.FindControl("lblinitialAmount");
            if (lblvehicleNumber.Text.Trim() == "")
            {
                divPassBookingVehicleNo.Visible = false;
                divRemainingVehicleNo.Visible = false;
                divExtendVehicleNo.Visible = false;
            }
            else
            {
                divPassBookingVehicleNo.Visible = true;
                divRemainingVehicleNo.Visible = true;
                divExtendVehicleNo.Visible = true;

            }
            if (lbltotalAmount.Text == "")
            {
                lbltotalAmount.Text = "0";
            }
            if (lblremainingAmount.Text == "")
            {
                lblremainingAmount.Text = "0";
            }
            if (lblextendAmount.Text == "")
            {
                lblextendAmount.Text = "0";
            }
            if (lblextendtax.Text == "")
            {
                lblextendtax.Text = "0";
            }
            decimal extendAmount = Convert.ToDecimal(lblextendAmount.Text.Trim());
            decimal TotalInitial = Convert.ToDecimal(lbltotalAmount.Text.Trim());
            decimal remaining = Convert.ToDecimal(lblremainingAmount.Text.Trim());
            lblRemAmount.Text = remaining.ToString("0.00");
            DateTime date = Convert.ToDateTime(lblinTime.Text);
            string intimes = date.ToString("yyyy'-'MM'-'dd'T'hh':'mm':'ss'.'fff'Z'");
            //Direct Check Out
            if (extendAmount == 0 && remaining == 0)
            {
                UpdateCheckInOutDetails(lblvehicleNumber.Text, lblvehicleHeaderId.Text, intimes, times, "O", slotid.Trim(),"","");
            }
            // Extented Amount
            else if (extendAmount != 0)
            {
                ViewState["Flag"] = "0";
                BindDdlPayment();
                FilterList(lblbookingPassId.Text.Trim());
                ViewState["vehicleHeaderId"] = lblvehicleHeaderId.Text.Trim();
                string extendDayHours = lblextendDayHour.Text.Trim();
                if (extendDayHours.EndsWith("r"))
                {
                    dayorHour.InnerText = "(in hours)";
                }
                else
                {
                    dayorHour.InnerText = "(in days)";
                }
                lbltaxAmount.Text = lblextendtax.Text.Trim();
                lblTimeExtended.Text = dayhourlbl[0].Trim();
                lblExtendedAmount.Text = extendAmount.ToString("0.00").Trim();
                lblVehicleNo.Text = lblvehicleNumber.Text.Trim();
                lblInitialAmount.Text = TotalInitial.ToString("0.00");
                decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(lblRemAmount.Text.Trim());
                decimal TotAmount = Convert.ToDecimal(TotalInitial) + Convert.ToDecimal(extendAmount);
                lblTopayAmount.Text = Amount.ToString("0.00");
                lblTotalAmount.Text = TotAmount.ToString("0.00");
                modal.Visible = true;
                divRemaining.Visible = false;
                divextend.Visible = true;
                remainingre.Visible = false;
                extended.Visible = true;
            }
            else if (remaining != 0)
            {
                remainingre.Visible = true;
                extended.Visible = false;
                ViewState["Flag"] = "1";
                BindReDdlPayment();
                FilterList(lblbookingPassId.Text.Trim());
                ViewState["vehicleHeaderId"] = lblvehicleHeaderId.Text.Trim();
                lblReRemainingAmount.Text = remaining.ToString("0.00");
                lblReVehicleNo.Text = lblvehicleNumber.Text.Trim();
                lblReInitialAmount.Text = TotalInitial.ToString("0.00");
                decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(lblRemAmount.Text.Trim());
                decimal TotAmount = Convert.ToDecimal(TotalInitial) + Convert.ToDecimal(extendAmount);
                lblReToPay.Text = Amount.ToString("0.00");
                lblReTotalAmount.Text = TotAmount.ToString("0.00");
                modal.Visible = true;
                divRemaining.Visible = true;
                divextend.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion

    #region GridBtnCheckIN Pass
    protected void gvbtnCheckInPass_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        Label lblbookingPassIdIn = (Label)gvrow.FindControl("lblgvINbookingPassId");
        ScanBookingPassId(lblbookingPassIdIn.Text.Trim());
    }

    #endregion
    #region GridBtnCheck Out Pass
    protected void gvbtnCheckoutPass_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton lnkbtn = sender as ImageButton;
        GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
        Label lblbookingPassIdIn = (Label)gvrow.FindControl("lblgvbookingPassId");
        ScanBookingPassId(lblbookingPassIdIn.Text.Trim());
    }
    #endregion
    #endregion
    #region  Extended and Remaining Amount Popup btn
    protected void btnCheckInPopup_Click(object sender, EventArgs e)
    {
        try
        {
            string times;
            string[] date2;
            DateTime utcTime = DateTime.Now;
            times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            date2 = times.Split('T');
            string date = utcTime.ToString("yyyy-MM-dd");
            if (ViewState["Flag"].ToString() == "0")
            {
                UpdateExtendedTimeAmount(lblVehicleNo.Text, ViewState["bookingDurationType"].ToString(), date2[1], date, lblTopayAmount.Text,
                lblTopayAmount.Text,
                lblBookingId.Text, ViewState["vehicleHeaderId"].ToString(), lbltaxAmount.Text, ViewState["SlotId"].ToString(), ddlPayment.SelectedValue);
            }
            else if(ViewState["Flag"].ToString() == "P")
            {
                UpdateCheckInOutDetails(ViewState["vehicleNumber"].ToString(), ViewState["vehicleHeaderId"].ToString(), null, times, "O", ViewState["SlotId"].ToString(), lblTopayAmt.Text,ddlPassPaymentMode.SelectedValue);
            }
            else
            {
                UpdateCheckInOutDetailsrem(lblReVehicleNo.Text, ViewState["vehicleHeaderId"].ToString(), null, times,
                  "O", ViewState["SlotId"].ToString(), lblReToPay.Text, ddlRepayment.SelectedValue);
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        clear();
    }
    #endregion
    #region Update In time and Out Time Details
    #region UpdateCheckInOutDetails
    public void UpdateCheckInOutDetails(string BookingID, string svehicleHeaderId, string Intime,
        string OutTime, string vehicleStatus, string slotId,string PaidAmount,string PaymentType)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                VehicleHeader Insert = new VehicleHeader()
                {
                    vehicleHeaderId = svehicleHeaderId,//Value From Login
                    updatedBy = Session["UserId"].ToString(),
                    vehicleStatus = vehicleStatus,
                };
                if (vehicleStatus == "I")
                {
                    Insert.inTime = Intime;
                }
                if (slotId.Trim() != "")
                {
                    Insert.slotId = slotId.Trim();
                }
                if (vehicleStatus.Trim() == "O")
                {
                    Insert.outTime = OutTime;
                }
                if (PaidAmount.Trim() != "")
                {
                    Insert.paidAmount = PaidAmount.Trim();

                }
                if (PaymentType.Trim() != "")
                {
                    Insert.paymentType = PaymentType.Trim();
                }
                HttpResponseMessage response = client.PutAsJsonAsync("vehicleHeader", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        if (vehicleStatus == "O")
                        {

                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";

                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblout.Text = BookingID.Trim() + " Checked Out Successfully";
                                lblout.Visible = true;
                                lblout.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckOutListPass();
                                }
                                else
                                {
                                    Response.Redirect("Print.aspx?rt=CiCo&bi=" + ViewState["BookingId"].ToString() + "", false);
                                    BindCheckOutList();
                                }
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblGridIn.Visible = true;
                                lblGridIn.Text = BookingID.Trim() + " Checked Out Successfully";
                                lblGridIn.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }


                        }
                        else
                        {
                            lblout.Visible = false;
                            lblout.Text = "";
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblout.Visible = true;
                                lblout.Text = BookingID.Trim() + " Checked In Successfully";
                                lblout.Style.Add("color", "#106d25");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckOutListPass();
                                }
                                else
                                {
                                    BindCheckOutList();
                                }
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblGridIn.Visible = true;
                                lblGridIn.Text = BookingID.Trim() + " Checked In Successfully";
                                lblGridIn.Style.Add("color", "#106d25");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }
                        }

                        ResetInput();
                        clear();
                    }
                    else
                    {
                        ResetInput();
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckOutListPass();
                            }
                            else
                            {
                                BindCheckOutList();
                            }
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {

                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckInListPass();
                            }
                            else
                            {
                                BindCheckInList();
                            }
                        }
                        txtBookingId.Focus();
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

    public void UpdateCheckInOutDetailsrem(string BookingID, string svehicleHeaderId, string Intime,
        string OutTime, string vehicleStatus, string slotId, string paidAmount, string Paymenttype)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                VehicleHeader Insert = new VehicleHeader()
                {
                    vehicleHeaderId = svehicleHeaderId,//Value From Login
                    updatedBy = Session["UserId"].ToString(),
                    vehicleStatus = vehicleStatus,
                    paidAmount = paidAmount.Trim(),
                    paymentType = Paymenttype.Trim()

                };
                if (vehicleStatus == "O")
                {
                    Insert.outTime = OutTime;
                }
                if (slotId != "")
                {
                    Insert.slotId = slotId.Trim();
                }
                HttpResponseMessage response = client.PutAsJsonAsync("vehicleHeader", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        if (StatusCode == 1)
                        {                           
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblout.Text = BookingID.Trim() + " Checked Out Successfully";
                                lblout.Visible = true;
                                lblout.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckOutListPass();
                                }
                                else
                                {
                                    Response.Redirect("Print.aspx?rt=CiCo&bi=" + lblBookingId.Text + "", false);
                                    BindCheckOutList();
                                }
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblGridIn.Visible = true;
                                lblGridIn.Text = BookingID.Trim() + " Checked Out Successfully";
                                lblGridIn.Style.Add("color", "#c3263b");
                                ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }
                            clear();
                        }
                        else
                        {
                            txtBookingId.Focus();
                            clear();
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckOutListPass();
                                }
                                else
                                {
                                    BindCheckOutList();
                                }
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {

                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }

                        ResetInput();
                    }
                    else
                    {
                        txtBookingId.Focus();
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
    #region UpdateExtendedTimeAmount
    public void UpdateExtendedTimeAmount(string VehicleNo, string bookingDurationType, string totime, string todate, string paidamount, string totalAmount, string BookingId,
        string vehicleHeaderId, string taxAmount, string slotId, string paymentType)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                bookingMasterDateTimeExtend Insert = new bookingMasterDateTimeExtend()
                {
                    bookingDurationType = bookingDurationType,
                    toTime = totime,
                    toDate = todate,
                    paidAmount = paidamount,
                    totalAmount = totalAmount,
                    taxAmount = taxAmount,
                    bookingId = BookingId,
                    vehicleHeaderId = vehicleHeaderId,
                    updatedBy = Session["UserId"].ToString().Trim(),
                    vehicleStatus = "O",
                    paymentType = paymentType.Trim()

                };
                if (slotId != "")
                {
                    Insert.slotId = slotId.Trim();
                }
                HttpResponseMessage response = client.PutAsJsonAsync("bookingMasterDateTimeExtend", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        clear();
                        lblGridIn.Visible = false;
                        lblAlreadyOut.Visible = false;
                        lblAlreadyIn.Visible = false;
                        lblGridIn.Text = "";
                        lblAlreadyOut.Text = "";
                        lblAlreadyIn.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblout.Text = VehicleNo.Trim() + " Checked Out Successfully";
                            lblout.Visible = true;
                            lblout.Style.Add("color", "#c3263b");
                            ClientScript.RegisterStartupScript(Page.GetType(), "HideLabelout", "javascript:HideLabelout();", true);
                            
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckOutListPass();
                            }
                            else
                            {
                                Response.Redirect("Print.aspx?rt=CiCo&bi=" + BookingId.ToString() + "", false);
                                BindCheckOutList();
                            }
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblGridIn.Visible = true;
                            lblGridIn.Text = VehicleNo.Trim() + " Checked Out Successfully";
                            lblGridIn.Style.Add("color", "#c3263b");
                            ClientScript.RegisterStartupScript(Page.GetType(), "HideLabel", "javascript:HideLabel();", true);
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckInListPass();
                            }
                            else
                            {
                                BindCheckInList();
                            }
                        }
                    }
                    else
                    {

                        txtBookingId.Focus();
                        clear();
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckOutListPass();
                            }
                            else
                            {
                                BindCheckOutList();
                            }
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {

                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckInListPass();
                            }
                            else
                            {
                                BindCheckInList();
                            }
                        }
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
    #endregion
    #region Payment Mode Bind and Selected Index Changed
    #region Payment Mode
    public void BindDdlPayment()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlPayment.Items.Clear();
                ddlPassPaymentMode.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "configMaster?configTypename=Payment&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtDEmployeeType = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        ddlPayment.DataSource = dtDEmployeeType;
                        ddlPayment.DataValueField = "configId";
                        ddlPayment.DataTextField = "configName";
                        ddlPayment.DataBind();
                        ddlPayment.Items.Remove(ddlPayment.Items.FindByText("payLater"));
                        ddlPayment.Items.Remove(ddlPayment.Items.FindByText("Pass"));
                        ddlPayment.Items.Remove(ddlPayment.Items.FindByText("Pay At CheckOut"));

                        ddlPassPaymentMode.DataSource = dtDEmployeeType;
                        ddlPassPaymentMode.DataValueField = "configId";
                        ddlPassPaymentMode.DataTextField = "configName";
                        ddlPassPaymentMode.DataBind();
                        ddlPassPaymentMode.Items.Remove(ddlPayment.Items.FindByText("payLater"));
                        ddlPassPaymentMode.Items.Remove(ddlPayment.Items.FindByText("Pass"));
                        ddlPassPaymentMode.Items.Remove(ddlPassPaymentMode.Items.FindByText("Pay At CheckOut"));

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
    public void BindReDdlPayment()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlRepayment.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "configMaster?configTypename=Payment&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtDEmployeeType = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlRepayment.DataSource = dtDEmployeeType;
                        ddlRepayment.DataValueField = "configId";
                        ddlRepayment.DataTextField = "configName";
                        ddlRepayment.DataBind();
                        ddlRepayment.Items.Remove(ddlRepayment.Items.FindByText("payLater"));
                        ddlRepayment.Items.Remove(ddlRepayment.Items.FindByText("Pass"));
                        ddlRepayment.Items.Remove(ddlRepayment.Items.FindByText("Pay At CheckOut"));
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

    #endregion
    #region Scan Text Change Event and Get MEthod
    public void FilterList(string Ids)
    {
        try
        {
            divReminingExtra.Visible = false;
            divExtraFeeFeatures.Visible = false;
            divCheckIn.Visible = true;
            divGvCheckOut.Visible = false;
            gvCheckIn.Visible = true;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "bookingMaster?inOutDetails=" + Ids.ToString().Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        List<GetBookings> extra = JsonConvert.DeserializeObject<List<GetBookings>>(ResponseMsg);
                        DataTable dtBlock = ConvertToDataTable(extra);
                        if (dtBlock.Rows.Count > 0)
                        {
                            var firstItemextra = extra.ElementAt(0);
                            var lst3 = firstItemextra.extraFeesDetails.ToList();
                            DataTable GetextraFeesDetails = ConvertToDataTable(lst3);

                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divReminingExtra.Visible = true;
                                divExtraFeeFeatures.Visible = true;
                                Extrafee.DataSource = GetextraFeesDetails;
                                Extrafee.DataBind();
                                dtlExtrafee.DataSource = GetextraFeesDetails;
                                dtlExtrafee.DataBind();
                            }

                            lblBookingId.Text = dtBlock.Rows[0]["bookingId"].ToString().Trim();
                            lblPinNo.Text = dtBlock.Rows[0]["pinNo"].ToString().Trim();
                            lblBlockName.Text = dtBlock.Rows[0]["blockName"].ToString().Trim();
                            lblFloorName.Text = dtBlock.Rows[0]["floorName"].ToString().Trim();
                            lblReBookingId.Text = dtBlock.Rows[0]["bookingId"].ToString().Trim();
                            lblRePinNo.Text = dtBlock.Rows[0]["pinNo"].ToString().Trim();
                            lblReBlockName.Text = dtBlock.Rows[0]["blockName"].ToString().Trim();
                            lblReFloorName.Text = dtBlock.Rows[0]["floorName"].ToString().Trim();
                            lblPaymentType.Text = dtBlock.Rows[0]["paymentStatus"].ToString().Trim()  == "P" ? "Paid" : "Unpaid";
                            lblPaymentStatusRe.Text = dtBlock.Rows[0]["paymentStatus"].ToString().Trim() == "P" ? "Paid" : "Unpaid";
                            ViewState["bookingDurationType"] = dtBlock.Rows[0]["bookingDurationType"].ToString().Trim();
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region BookingID TextChange
    protected void txtBookingId_TextChanged(object sender, EventArgs e)
    {
        if (txtBookingId.Text != "")
        {

            string BookingPassId = string.Empty;
            string BorP = string.Empty;
            BookingPassId = txtBookingId.Text.ToUpper();
            string[] BookingPassIds = BookingPassId.Split(';');
            BorP = BookingPassIds[0].Substring(0, 1);
            if (BookingPassIds.Length > 0)
            {
                ClearLabel();
                if (BorP.Trim() == "B")
                {
                    ScanBookingId(BookingPassIds[0].Trim());
                }
                else if (BorP.Trim() == "P")
                {
                    ClearLabel();
                    ScanBookingPassId(BookingPassIds[0].Trim());
                }
                else
                {
                    GetDataBasedOnVehicleNumberPhoneDetails(BookingPassIds[0].Trim());
                    //txtBookingId.Focus();
                    //ResetInput();
                    //lblAlreadyOut.Visible = true;
                    //lblAlreadyOut.Text = "Invalid QRcode";
                    //lblAlreadyIn.Visible = true;
                    //lblAlreadyIn.Text = "Invalid QRcode";
                    //lblout.Visible = false;
                    //lblGridIn.Visible = false;
                    //lblGridIn.Text = "";
                    //lblout.Text = "";
                    //ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                    //BindCheckInList();
                    //txtBookingId.Focus();
                }
            }
            else
            {
                txtBookingId.Focus();
                ResetInput();
                lblAlreadyOut.Visible = true;
                lblAlreadyOut.Text = "Invalid QRcode";
                lblAlreadyIn.Visible = true;
                lblAlreadyIn.Text = "Invalid QRcode";
                lblout.Visible = false;
                lblGridIn.Visible = false;
                lblGridIn.Text = "";
                lblout.Text = "";
                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                if (rbtnTypeFP.SelectedValue == "P")
                {
                    BindCheckInListPass();
                }
                else
                {
                    BindCheckInList();
                }
                txtBookingId.Focus();
            }

        }
    }
    public void GetDataBasedOnVehicleNumberPhoneDetails(string BookingIds)
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
                      + "getDataBasedOnVehicleNumberPhone?inOutDetails=" + BookingIds.Trim() + "&floorId=" + ddlfloor.SelectedValue + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        var other = JsonConvert.DeserializeObject<dynamic>(ResponseMsg);
                        foreach (var item in other)
                        {
                            item.Property("extraFeesDetails").Remove();
                        }
                        var others = JsonConvert.SerializeObject(other);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(others);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Columns.Contains("passBookingTransactionId"))
                            {
                                ScanBookingPassId(dt.Rows[0]["passBookingTransactionId"].ToString());
                            }
                            else
                            {
                                ScanBookingId(dt.Rows[0]["bookingId"].ToString());
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            ResetInput();
                            clear();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtBookingId.Text = string.Empty;
                        ResetInput();
                        clear();
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
    #region ScanBookingId And Pass Id get Method

    public void ScanBookingId(string BookingIds)
    {
        divReminingExtra.Visible = false;
        divExtraFeeFeatures.Visible = false;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?inOutDetails=" + BookingIds.Trim() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<GetBookings> extra = JsonConvert.DeserializeObject<List<GetBookings>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(extra);
                        if (dt.Rows.Count > 0)
                        {
                            var firstItemextra = extra.ElementAt(0);
                            var lst1 = firstItemextra.vehicleDetails.ToList();
                            var lst3 = firstItemextra.extraFeesDetails.ToList();
                            DataTable GetvehicleDetails = ConvertToDataTable(lst1);
                            DataTable GetextraFeesDetails = ConvertToDataTable(lst3);
                            var userSlot = firstItemextra.userSlotDetails.ToList();
                            DataTable userSlotTable = ConvertToDataTable(userSlot);
                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divReminingExtra.Visible = true;
                                divExtraFeeFeatures.Visible = true;
                                Extrafee.DataSource = GetextraFeesDetails;
                                Extrafee.DataBind();
                                dtlExtrafee.DataSource = GetextraFeesDetails;
                                dtlExtrafee.DataBind();
                            }
                            string slotd = string.Empty;
                            if (userSlotTable.Rows.Count > 0)
                            {
                                 slotd = userSlotTable.Rows[0]["slotId"].ToString();

                            }
                            else
                            {
                                slotd = GetvehicleDetails.Rows[0]["slotId"].ToString();
                            }

                            string times;
                            string[] date2;
                            DateTime utcTime = DateTime.Now;
                            times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                            date2 = times.Split(' ');
                            string[] dayhourlbl;
                            string vehicleStatus = GetvehicleDetails.Rows[0]["vehicleStatus"].ToString();
                            string vehicleNumber = GetvehicleDetails.Rows[0]["vehicleNumber"].ToString();
                            string vehicleHeaderId = GetvehicleDetails.Rows[0]["vehicleHeaderId"].ToString();
                            string BookingId = dt.Rows[0]["bookingId"].ToString();
                            string pinNo = dt.Rows[0]["pinNo"].ToString();
                            decimal initialAmount = Convert.ToDecimal(dt.Rows[0]["bookingAmount"].ToString());
                            decimal remainingAmount = Convert.ToDecimal(dt.Rows[0]["remainingAmount"].ToString());
                            decimal extendAmount = Convert.ToDecimal(dt.Rows[0]["extendAmount"].ToString());
                            decimal extendTax = Convert.ToDecimal(dt.Rows[0]["extendTax"].ToString());
                            decimal topayAmount = remainingAmount + extendAmount;
                           
                            string extendDayHour = dt.Rows[0]["extendDayHour"].ToString();
                            string bookingDurationType = dt.Rows[0]["bookingDurationType"].ToString();
                            string paymentStatus = dt.Rows[0]["paymentStatus"].ToString();
                            dayhourlbl = extendDayHour.Trim().Split('-');
                            string extendDayHours = extendDayHour.Trim();
                            if (extendDayHours.EndsWith("r"))
                            {
                                dayorHour.InnerText = "(in hours)";
                            }
                            else
                            {
                                dayorHour.InnerText = "(in days)";
                            }
                            if (vehicleNumber.Trim() == "")
                            {
                                divPassBookingVehicleNo.Visible = false;
                                divRemainingVehicleNo.Visible = false;
                                divExtendVehicleNo.Visible = false;
                            }
                            else
                            {
                                divPassBookingVehicleNo.Visible = true;
                                divRemainingVehicleNo.Visible = true;
                                divExtendVehicleNo.Visible = true;

                            }
                            ViewState["vehicleStatus"] = vehicleStatus.Trim();
                            ViewState["BookingId"] = BookingId.Trim();
                            ViewState["vehicleNumber"] = vehicleNumber.Trim();
                            ViewState["vehicleHeaderId"] = vehicleHeaderId.Trim();
                            ViewState["slotd"] = slotd.Trim();
                            ViewState["remainingAmount"] = remainingAmount.ToString().Trim();
                            ViewState["extendAmount"] = extendAmount.ToString().Trim();
                            ViewState["extendTax"] = extendTax.ToString().Trim();
                            ViewState["topayAmount"] = topayAmount.ToString().Trim();
                            ViewState["bookingDurationType"] = bookingDurationType.Trim();
                            lblBookingId.Text = dt.Rows[0]["bookingId"].ToString().Trim();
                            lblPinNo.Text = dt.Rows[0]["pinNo"].ToString().Trim();
                            lblBlockName.Text = dt.Rows[0]["blockName"].ToString().Trim();
                            lblFloorName.Text = dt.Rows[0]["floorName"].ToString().Trim();
                            lblReBookingId.Text = dt.Rows[0]["bookingId"].ToString().Trim();
                            lblRePinNo.Text = dt.Rows[0]["pinNo"].ToString().Trim();
                            lblReBlockName.Text = dt.Rows[0]["blockName"].ToString().Trim();
                            lblReFloorName.Text = dt.Rows[0]["floorName"].ToString().Trim();
                            lblPaymentType.Text = paymentStatus == "P" ? "Paid" : "Unpaid";
                            lblPaymentStatusRe.Text = paymentStatus == "P" ? "Paid" : "Unpaid";
                            if (vehicleStatus.Trim() == "")
                            {
                                if (Session["UserRole"].ToString() == "E")
                                {
                                    if (ViewState["CheckIn"].ToString() != "O")
                                    {
                                        string msg = "User Does not have Rights to Check In";
                                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + msg.ToString().Trim() + "');", true);
                                        clear();
                                        return;
                                    }
                                }

                                UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), times, null, "I", slotd.Trim(), "", "");
                                txtBookingId.Focus();
                            }
                            else if (vehicleStatus.Trim() == "I")
                            {
                                if (Session["UserRole"].ToString() == "E")
                                {
                                    if (ViewState["CheckOut"].ToString() != "O")
                                    {
                                        string msg = "User Doesnot have Rights to Check Out";
                                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + msg.ToString().Trim() + "');", true);
                                        clear();
                                        return;
                                    }
                                }
                                if (extendAmount != 0)
                                {

                                    ViewState["Flag"] = "0";
                                    BindDdlPayment();
                                    lbltaxAmount.Text = extendTax.ToString().Trim();
                                    dayhourlbl = extendDayHour.Split('-');
                                    lblTimeExtended.Text = dayhourlbl[0].Trim();
                                    lblExtendedAmount.Text = extendAmount.ToString("0.00").Trim();
                                    lblVehicleNo.Text = vehicleNumber.Trim();
                                    lblInitialAmount.Text = topayAmount.ToString("0.00");
                                    lblRemAmount.Text = remainingAmount.ToString();
                                    decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(remainingAmount);
                                    decimal TotAmount = Convert.ToDecimal(topayAmount) + Convert.ToDecimal(extendAmount);
                                    lblTopayAmount.Text = Amount.ToString("0.00");
                                    //lblTotalAmount.Text = TotAmount.ToString("0.00");
                                    modal.Visible = true;
                                    divRemaining.Visible = false;
                                    divextend.Visible = true;
                                    remainingre.Visible = false;
                                    extended.Visible = true;

                                }
                                else if (remainingAmount != 0)
                                {
                                    remainingre.Visible = true;
                                    ViewState["Flag"] = "1";
                                    BindReDdlPayment();
                                    lblReRemainingAmount.Text = remainingAmount.ToString("0.00");
                                    lblReVehicleNo.Text = vehicleNumber.Trim();
                                    lblReInitialAmount.Text = topayAmount.ToString("0.00");
                                    decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(remainingAmount);
                                    decimal TotAmount = Convert.ToDecimal(topayAmount) + Convert.ToDecimal(extendAmount);
                                    lblReToPay.Text = Amount.ToString("0.00");
                                    lblReTotalAmount.Text = TotAmount.ToString("0.00");
                                    modal.Visible = true;
                                    divRemaining.Visible = true;
                                    divextend.Visible = false;
                                    extended.Visible = false;

                                }
                                else if (remainingAmount == 0 && extendAmount == 0)
                                {
                                    UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), "", times, "O", slotd.Trim(), "", "");
                                    txtBookingId.Focus();

                                }

                            }
                            else
                            {
                                txtBookingId.Focus();
                                ResetInput();
                                lblout.Visible = false;
                                lblGridIn.Visible = false;
                                lblGridIn.Text = "";
                                lblout.Text = "";
                                if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                                {
                                    lblAlreadyOut.Visible = true;
                                    lblAlreadyOut.Text = "Already Checked Out";
                                    lblAlreadyIn.Visible = false;
                                    lblAlreadyIn.Text = "";
                                    ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                                    if (rbtnTypeFP.SelectedValue == "P")
                                    {
                                        BindCheckOutListPass();
                                    }
                                    else
                                    {
                                        BindCheckOutList();
                                    }
                                }
                                else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                                {
                                    lblAlreadyIn.Visible = true;
                                    lblAlreadyIn.Text = "Already Checked Out";
                                    lblAlreadyOut.Visible = false;
                                    lblAlreadyOut.Text = "";
                                    ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                                    if (rbtnTypeFP.SelectedValue == "P")
                                    {
                                        BindCheckInListPass();
                                    }
                                    else
                                    {
                                        BindCheckInList();
                                    }
                                }
                                txtBookingId.Focus();
                            }
                        }
                        else
                        {
                            txtBookingId.Focus();
                            ResetInput();
                            lblout.Visible = false;
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            lblout.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblAlreadyOut.Visible = true;
                                lblAlreadyOut.Text = ResponseMsg;
                                lblAlreadyIn.Visible = false;
                                lblAlreadyIn.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckOutListPass();
                                }
                                else
                                {
                                    BindCheckOutList();
                                }
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblAlreadyIn.Visible = true;
                                lblAlreadyIn.Text = ResponseMsg;
                                lblAlreadyOut.Visible = false;
                                lblAlreadyOut.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        txtBookingId.Focus();
                        ResetInput();
                        lblout.Visible = false;
                        lblGridIn.Visible = false;
                        lblGridIn.Text = "";
                        lblout.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblAlreadyOut.Visible = true;
                            lblAlreadyOut.Text = ResponseMsg;
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckOutListPass();
                            }
                            else
                            {
                                BindCheckOutList();
                            }
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblAlreadyIn.Visible = true;
                            lblAlreadyIn.Text = ResponseMsg;
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckInListPass();
                            }
                            else
                            {
                                BindCheckInList();
                            }
                        }
                        txtBookingId.Focus();
                    }
                    }
                    else
                    {

                        txtBookingId.Focus();
                        ResetInput();
                        lblout.Visible = false;
                        lblGridIn.Visible = false;
                        lblGridIn.Text = "";
                        lblout.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblAlreadyOut.Visible = true;
                            lblAlreadyOut.Text = "Invalid QRcode";
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                        if (rbtnTypeFP.SelectedValue == "P")
                        {
                            BindCheckOutListPass();
                        }
                        else
                        {
                            BindCheckOutList();
                        }
                    }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblAlreadyIn.Visible = true;
                            lblAlreadyIn.Text = "Invalid QRcode";
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                        if (rbtnTypeFP.SelectedValue == "P")
                        {
                            BindCheckInListPass();
                        }
                        else
                        {
                            BindCheckInList();
                        }
                    }
                        txtBookingId.Focus();
                    }
                
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
   
    public void ScanBookingPassId(string Ids)
    {
        try
        {
            divCheckIn.Visible = true;
            divGvCheckOut.Visible = false;
            gvCheckIn.Visible = true;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                             + "passBooking?inOutDetails=" + Ids + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        
                        List<PassBooking> extra = JsonConvert.DeserializeObject<List<PassBooking>>(ResponseMsg);
                        var firstItemextra = extra.ElementAt(0);
                        var lst1 = firstItemextra.vehicleDetails.ToList();
                        var lst3 = firstItemextra.extraFeesDetails.ToList();
                        DataTable GetvehicleDetails = ConvertToDataTable(lst1);
                        DataTable GetextraFeesDetailsPass = ConvertToDataTable(lst3);
                        DataTable dtPassIN =ConvertToDataTable(extra);
                        var userSlot = firstItemextra.userSlotDetails.ToList();
                        DataTable userSlotTable = ConvertToDataTable(userSlot);
                        if (GetextraFeesDetailsPass.Rows.Count > 0)
                        {
                            dtlExtraFeeSummary.DataSource = GetextraFeesDetailsPass;
                            dtlExtraFeeSummary.DataBind();
                        }
                        string SlotId = string.Empty;
                        if (userSlotTable.Rows.Count > 0)
                        {
                            SlotId = userSlotTable.Rows[0]["slotId"].ToString();

                        }
                        else
                        {
                            SlotId = GetvehicleDetails.Rows[0]["slotId"].ToString();
                        }
                        string paymentStatus = dtPassIN.Rows[0]["paymentStatus"].ToString();
                        string vehicleStatus = GetvehicleDetails.Rows[0]["vehicleStatus"].ToString();
                        string vehicleNumber = GetvehicleDetails.Rows[0]["vehicleNumber"].ToString();
                        string vehicleHeaderId = GetvehicleDetails.Rows[0]["vehicleHeaderId"].ToString();
                        string BookingId = GetvehicleDetails.Rows[0]["bookingPassId"].ToString();
                        string passTransactionId = dtPassIN.Rows[0]["passTransactionId"].ToString();
                     
                       // decimal Extrafeatures = Convert.ToDecimal(dtPassIN.Rows[0]["extraFeaturesAmount"].ToString().Trim());
                        decimal Extrafee = Convert.ToDecimal(dtPassIN.Rows[0]["extraFeesAmount"].ToString().Trim());
                        decimal Total= Extrafee;
                        string times;
                        lblBlockIn.Text = ddlblock.SelectedItem.Text;
                        lblFloorIn.Text = ddlfloor.SelectedItem.Text;
                        lblVehicleIn.Text = vehicleNumber.Trim();
                      
                        lblTopayAmt.Text = Total.ToString("0.00");
                        lblPaymentTypePass.Text = paymentStatus == "P" ? "Paid":"Unpaid";
                        string[] date2;
                        DateTime utcTime = DateTime.Now;
                        times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                        date2 = times.Split(' ');
                        ViewState["vehicleNumber"] = vehicleNumber.Trim();
                        ViewState["vehicleHeaderId"] = vehicleHeaderId.Trim();
                        ViewState["SlotId"] = SlotId.Trim();
                        if (vehicleNumber.Trim() == "")
                        {
                            divPassBookingVehicleNo.Visible = false;
                            divRemainingVehicleNo.Visible = false;
                            divExtendVehicleNo.Visible = false;
                        }
                        else
                        {
                            divPassBookingVehicleNo.Visible = true;
                            divRemainingVehicleNo.Visible = true;
                            divExtendVehicleNo.Visible = true;

                        }
                        if (vehicleStatus == " ")
                        {
                            if (Session["UserRole"].ToString() == "E")
                            {
                                if (ViewState["CheckIn"].ToString() != "O")
                                {
                                    string msg = "User Doesnot have Rights to Check In";
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + msg.ToString().Trim() + "');", true);
                                    clear();
                                    return;
                                }
                            }

                        
                            UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), times, null, "I", SlotId.Trim(),"","");
                            txtBookingId.Focus();

                        }
                        else if(vehicleStatus == "I")
                        {
                            if (Session["UserRole"].ToString() == "E")
                            {
                                if (ViewState["CheckOut"].ToString() != "O")
                                {
                                    string msg = "User Doesnot have Rights to Check Out";
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + msg.ToString().Trim() + "');", true);
                                    clear();
                                    return;
                                }
                            }
                            if(paymentStatus.Trim() == "P" )
                            {
                                UpdateCheckInOutDetails(vehicleNumber, vehicleHeaderId.Trim(), null, times, "O", SlotId.Trim(), "", "");
                            }
                            else
                            {
                                ViewState["Flag"] = "P";
                                BindDdlPayment();
                                GetPassDetailsInoutDetails(passTransactionId);
                                modal.Visible = true;
                                divRemaining.Visible = false;
                                divextend.Visible = false;
                                remainingre.Visible = true;
                                extended.Visible = false;
                                divPassDetails.Visible = true;                              
                            }
                        }
                        else
                        {
                            txtBookingId.Focus();
                            ResetInput();
                            lblout.Visible = false;
                            lblGridIn.Visible = false;
                            lblGridIn.Text = "";
                            lblout.Text = "";
                            if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                            {
                                lblAlreadyOut.Visible = true;
                                lblAlreadyOut.Text = "Already Checked Out";
                                lblAlreadyIn.Visible = false;
                                lblAlreadyIn.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckOutListPass();
                                }
                                else
                                {
                                    BindCheckOutList();
                                }
                            }
                            else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                            {
                                lblAlreadyIn.Visible = true;
                                lblAlreadyIn.Text = "Already Checked Out";
                                lblAlreadyOut.Visible = false;
                                lblAlreadyOut.Text = "";
                                ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                                if (rbtnTypeFP.SelectedValue == "P")
                                {
                                    BindCheckInListPass();
                                }
                                else
                                {
                                    BindCheckInList();
                                }
                            }
                            txtBookingId.Focus();
                        }
                    }
                    else
                    {
                        txtBookingId.Focus();
                        ResetInput();
                        lblout.Visible = false;
                        lblGridIn.Visible = false;
                        lblGridIn.Text = "";
                        lblout.Text = "";
                        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                        {
                            lblAlreadyOut.Visible = true;
                            lblAlreadyOut.Text = ResponseMsg;
                            lblAlreadyIn.Visible = false;
                            lblAlreadyIn.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckOutListPass();
                            }
                            else
                            {
                                BindCheckOutList();
                            }
                        }
                        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                        {
                            lblAlreadyIn.Visible = true;
                            lblAlreadyIn.Text = ResponseMsg;
                            lblAlreadyOut.Visible = false;
                            lblAlreadyOut.Text = "";
                            ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckInListPass();
                            }
                            else
                            {
                                BindCheckInList();
                            }
                        }
                        txtBookingId.Focus();
                    }
                }
                else
                {
                    txtBookingId.Focus();
                    ResetInput();
                    lblout.Visible = false;
                    lblGridIn.Visible = false;
                    lblGridIn.Text = "";
                    lblout.Text = "";
                    if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
                    {
                        lblAlreadyOut.Visible = true;
                        lblAlreadyOut.Text = "Invalid QRcode";
                        lblAlreadyIn.Visible = false;
                        lblAlreadyIn.Text = "";
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelout", "javascript:AlreadyLabelout();", true);
                        if (rbtnTypeFP.SelectedValue == "P")
                        {
                            BindCheckOutListPass();
                        }
                        else
                        {
                            BindCheckOutList();
                        }
                    }
                    else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
                    {
                        lblAlreadyIn.Visible = true;
                        lblAlreadyIn.Text = "Invalid QRcode";
                        lblAlreadyOut.Visible = false;
                        lblAlreadyOut.Text = "";
                        ClientScript.RegisterStartupScript(Page.GetType(), "AlreadyLabelIn", "javascript:AlreadyLabelIn();", true);
                        if (rbtnTypeFP.SelectedValue == "P")
                        {
                            BindCheckInListPass();
                        }
                        else
                        {
                            BindCheckInList();
                        }
                    }
                    txtBookingId.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void GetPassDetailsInoutDetails(string PassId)
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
                      + "passTransaction?ParkingPassTransactionId=" + PassId.Trim() + "";
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
                            List<GetPassTicket> lst = JsonConvert.DeserializeObject<List<GetPassTicket>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.passType.ToList();
                            DataTable passtype = ConvertToDataTable(lst1);
                            lblPassTransactionPassId.Text = dt.Rows[0]["parkingPassTransId"].ToString();
                            lblPassTypeINout.Text = passtype.Rows[0]["passType"].ToString();
                            lblMobileNoPass.Text = dt.Rows[0]["phoneNumber"].ToString();
                            lblStartPass.Text = dt.Rows[0]["validStartDate"].ToString();
                            lblEndPass.Text = dt.Rows[0]["validEndDate"].ToString();
                            lblCategoryPAss.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblVehicleTypeNamePass.Text = dt.Rows[0]["vehicleName"].ToString();
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
    #endregion
    #region Header and Inout Classess
    #region VehicleHeaderClass
    public class VehicleHeader
    {
        public string vehicleHeaderId { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string updatedBy { get; set; }
        public string vehicleStatus { get; set; }
        public string paidAmount { get; set; }
        public string slotId { get; set; }
        public string paymentType { get; set; }

    }

    #endregion
    #region bookingMasterDateTimeExtend
    public class bookingMasterDateTimeExtend
    {
        public string bookingDurationType { get; set; }
        public string toTime { get; set; }
        public string toDate { get; set; }
        public string paidAmount { get; set; }
        public string vehicleStatus { get; set; }
        public string totalAmount { get; set; }
        public string bookingId { get; set; }
        public string vehicleHeaderId { get; set; }
        public string updatedBy { get; set; }
        public string outTime { get; set; }
        public string taxAmount { get; set; }
        public string slotId { get; set; }
        public string paymentType { get; set; }

    }

    #endregion
    #region  OutDetails
    public class OutDetails
    {
        public string vehicleHeaderId { get; set; }
        public string bookingDurationType { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string vehicleTypeName { get; set; }
        public string vehicleImageUrl { get; set; }
        public string extendAmount { get; set; }
        public string extendTax { get; set; }
        public string extendDayHour { get; set; }
        public string remainingAmount { get; set; }
        public string boookingAmount { get; set; }
        public string initialAmount { get; set; }
        public string pinNo { get; set; }
        public List<slotDetails> slotDetails { get; set; }

    }

    public class slotDetails
    {
        public string userSlotId { get; set; }
        public string slotId { get; set; }
    }

    #endregion
    #region PassBooking
    public class PassBooking
    {

        public string taxAmount { get; set; }
        public string amount { get; set; }
        public string totalAmount { get; set; }
        public string paymentStatus { get; set; }
        public string paymentType { get; set; }
        public string passTransactionId { get; set; }
        public string branchName { get; set; }
        public string extraFeesAmount { get; set; }
        public string extraFeaturesAmount { get; set; }
        public string bookingAmount { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string createdBy { get; set; }
        public List<PassvehicleHeaderDetails> vehicleDetails { get; set; }
        public List<PassuserSlotDetails> userSlotDetails { get; set; }
        public List<PassextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<PassextraFeesDetails> extraFeesDetails { get; set; }

    }
    public class PassvehicleHeaderDetails
    {
        public string vehicleHeaderId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
    }
    public class PassuserSlotDetails
    {
        public string slotId { get; set; }
        public string vehicleType { get; set; }
    }
    public class PassextraFeaturesDetails
    {
        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraDetail { get; set; }
        public string totalAmount { get; set; }
    }
    public class PassextraFeesDetails
    {
        public string extraFeesDetails { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }
    }
    #endregion
    #region Pass  Class  
    public class GetPassTicket
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userId { get; set; }
        public string phoneNumber { get; set; }
        public string parkingPassTransId { get; set; }
        public string validStartDate { get; set; }
        public string validEndDate { get; set; }
        public string validStartTime { get; set; }
        public string validEndTime { get; set; }
        public string amount { get; set; }
        public string tax { get; set; }
        public string totalAmount { get; set; }
        public string taxId { get; set; }
        public string vehicleName { get; set; }
        public string parkingName { get; set; }
        public string branchName { get; set; }
        public List<GetpassType> passType { get; set; }

    }
    public class GetpassType
    {
        public string passType { get; set; }
        public string passCategory { get; set; }

    }

   
    #endregion
    #endregion
    #region Clear
    public void ResetInput()
    {
        txtBookingId.Text = "";
        txtBookingId.Focus();
        lblgridmsg.Visible = false;
        lblgridmsg.Text = "";
        lblgridmsgIn.Visible = false;
        lblgridmsgIn.Text = "";
    }
    public void ClearLabel()
    {
        lblgridmsg.Visible = false;
        lblgridmsg.Text = "";
        lblgridmsgIn.Visible = false;
        lblgridmsgIn.Text = "";
        lblAlreadyOut.Visible = false;
        lblAlreadyIn.Visible = false;
        lblout.Visible = false;
        lblGridIn.Visible = false;
        lblGridIn.Text = "";
        lblout.Text = "";
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Text = "";
    }
    public void clear()
    {
    
        modal.Visible = false;
        txtBookingId.Text = "";
        lblBookingId.Text = "";
        lblPinNo.Text = "";
        lblRemAmount.Text = "";
        lblBlockName.Text = "";
        lblFloorName.Text = "";
        lblTimeExtended.Text = "";
        lblExtendedAmount.Text = "";
        lblVehicleNo.Text = "";
        lblReBookingId.Text = "";
        lblReFloorName.Text = "";
        lblReInitialAmount.Text = "";
        lblReRemainingAmount.Text = "";
        lblRePinNo.Text = "";
        lblReToPay.Text = "";
        lblReVehicleNo.Text = "";
        lblReTotalAmount.Text = "";
        lblReBlockName.Text = "";
        txtBookingId.Text = "";
        txtBookingId.Focus();
        lblgridmsg.Visible = false;
        lblgridmsg.Text = "";
        lblgridmsgIn.Visible = false;
        lblgridmsgIn.Text = "";
        lblAlreadyOut.Visible = false;
        lblAlreadyIn.Visible = false;
        lblAlreadyOut.Text = "";
        lblAlreadyIn.Text = "";
        ViewState["bookingDurationType"] = "";
        ViewState["vehicleHeaderId"] = "";
        ViewState["SlotId"] = "";
        ViewState["vehicleNumber"] = "";
        lblBlockIn.Text = "";
        lblFloorIn.Text = "";
        lblVehicleIn.Text = "";
        lblPaymentStatusRe.Text = "";
        lblPaymentType.Text = "";
        lblTopayAmt.Text = "";
       
        lblPassTransactionPassId.Text = "";
        lblPassTypeINout.Text = "";
        lblMobileNoPass.Text = "";
        lblStartPass.Text = "";
        lblEndPass.Text = "";
        lblCategoryPAss.Text = "";
        lblVehicleTypeNamePass.Text = "";
        if (ViewState["BindCheckOutList"].ToString().Trim() == "CheckOut")
        {
            if (rbtnTypeFP.SelectedValue == "P")
            {
                BindCheckOutListPass();
            }
            else
            {
                BindCheckOutList();
            }
        }
        else if (ViewState["BindCheckInList"].ToString().Trim() == "CheckIn")
        {

            if (rbtnTypeFP.SelectedValue == "P")
            {
                BindCheckInListPass();
            }
            else
            {
                BindCheckInList();
            }
        }
    }
    #endregion
    #region Booking Class
    public class GetBookings
    {
        public string passBookingTransactionId { get; set; }
        public string vehicleHeaderId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string slotId { get; set; }
        public string paidAmount { get; set; }
        public string paymentStatus { get; set; }
        public string extendAmount { get; set; }
        public string extendTax { get; set; }
        public string extendDayHour { get; set; }
        public string remainingAmount { get; set; }
        public string bookingAmount { get; set; }
        public string initialAmount { get; set; }
        public string pinNo { get; set; }
        public string vehicleName { get; set; }
        public string vehicleParkedTime { get; set; }
        public string bookingDurationType { get; set; }
        public string blockName { get; set; }
        public string floorName { get; set; }
        public List<GetuserSlotDetails> userSlotDetails { get; set; }
        public List<vehicleDetails> vehicleDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<GetextraFees> extraFeesDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetail { get; set; }
        public List<GetextraFees> extraFeesDetail { get; set; }

    }
    public class vehicleDetails
    {
        public string vehicleHeaderId { get; set; }
        public string bookingPassId { get; set; }
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
    }
    public class GetuserSlotDetails
    {
        public string slotId { get; set; }
        public string slotNumber { get; set; }
        public string vehicleType { get; set; }
        public string laneNumber { get; set; }
    }
    public class GetextraFeaturesDetails
    {

        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraDetail { get; set; }
        public string featureName { get; set; }
        public string tax { get; set; }
        public string totalAmount { get; set; }

    }
    public class GetextraFees
    {

        public string extraFeesDetails { get; set; }
        public string priceId { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }

    }
    #endregion
    #region Menu Access rights
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
        ViewState["CheckIn"] = "I";
        ViewState["CheckOut"] = "I";
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
                        int count = 0;
                        List<menuOptionAccess> lst = JsonConvert.DeserializeObject<List<menuOptionAccess>>(ResponseMsg);
                        var firstItem = lst.ElementAt(0);
                        var lst1 = firstItem.optionDetails.ToList();
                        DataTable optionDetails = ConvertToDataTable(lst1);
                        var Option = lst1.Where(x => x.optionName == "checkIn" && x.MenuOptionAccessActiveStatus == "A")
                            .Select(x => new
                            {
                                optionName = x.optionName,
                                AddRights = x.AddRights,
                                EditRights = x.EditRights,
                                ViewRights = x.ViewRights,
                                DeleteRights = x.DeleteRights
                            }).ToList();
                        var Option1 = lst1.Where(x => x.optionName == "checkOut" && x.MenuOptionAccessActiveStatus == "A")
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
                            ViewState["CheckIn"] = "O";
                            count = 1;
                            divForm.Visible = true;
                            divCheckIn.Visible = true;
                            divGvCheckOut.Visible = false;
                            btnCheckOut.Visible = false;
                            btnCheckIn.Visible = true;
                            if (rbtnTypeFP.SelectedValue == "P")
                            {
                                BindCheckInListPass();
                            }
                            else
                            {
                                BindCheckInList();
                            }

                        }
                        else
                        {
                            btnCheckIn.Visible = false;
                            btnCheckOut.Visible = false;
                            divForm.Visible = false;
                            divCheckIn.Visible = false;
                            divGvCheckOut.Visible = false;

                        }
                        if (Option1.Count > 0)
                        {
                            ViewState["CheckOut"] = "O";
                            if (count == 1)
                            {
                                btnCheckIn.Visible = true;
                                btnCheckOut.Visible = true;
                                divForm.Visible = true;
                                divCheckIn.Visible = true;
                                divGvCheckOut.Visible = true;
                               
                            }
                            else
                            {
                                BindCheckOutList();
                                btnCheckIn.Visible = false;
                                btnCheckOut.Visible = true;
                                divForm.Visible = true;
                                divCheckIn.Visible = false;
                                divGvCheckOut.Visible = true;
                            }


                        }
                        else
                        {
                            if (count == 1)
                            {
                                btnCheckIn.Visible = true;
                                btnCheckOut.Visible = false;
                                divForm.Visible = true;
                                divCheckIn.Visible = true;
                                divGvCheckOut.Visible = false;
                            }
                            else
                            {
                                btnCheckIn.Visible = false;
                                btnCheckOut.Visible = false;
                                divForm.Visible = false;
                                divCheckIn.Visible = false;
                                divGvCheckOut.Visible = false;
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
    #endregion
    
}