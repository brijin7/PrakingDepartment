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
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.IO;
using QRCoder;
using System.Text.RegularExpressions;
using System.Globalization;
using CrystalDecisions.Web;
using System.Runtime.Remoting.Contexts;
using System.Configuration;
using System.Xml;

public partial class Booking_Booking : System.Web.UI.Page
{
    readonly Helper helper = new Helper();
    readonly string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
    readonly string manualCheckInGridUrl;
    readonly string manualVehicleTypeDdlUrl;
    readonly string manualCheckingPostUrl;
    public Booking_Booking()
    {
        manualCheckInGridUrl = baseUrl + "DeviceMacDeatails?branchId=";//grid
        manualVehicleTypeDdlUrl = baseUrl + "vehicleConfigMaster?activeStatus=A";//ddl
        manualCheckingPostUrl = baseUrl + "DeviceMacDeatails1";//check in
    }
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
            DateTime date = DateTime.Now.AddDays(1);
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtTodate.Text = date.ToString("yyyy-MM-dd");
            ViewState["dynamictable"] = false;
            ViewState["BlockCount"] = "0";
            ViewState["FloorCount"] = "0";
            ViewState["BookingType"] = "N";
            BindBlock();
            BindCount();
            ViewState["paylaterOption"] = "0";
            GetPaymentType();
            divSummary.Visible = false;

            if (ViewState["BlockCount"].ToString() == "0" || ViewState["FloorCount"].ToString() == "0")
            {
                divBlockFloor.Visible = true;
            }
            else
            {
                divBlockFloor.Visible = false;
            }
            if (string.IsNullOrEmpty(Session["MobNo"] as string))
            {
                ChkMobileNo.Checked = true;
            }
            else
            {
                ChkMobileNo.Checked = false;
            }
            if (ChkMobileNo.Checked == true)
            {
                Session["MobNo"] = null;
                divMobileNo.Visible = true;
                RfvMobNo.Enabled = true;
                btnBook.Enabled = false;
            }
            else
            {
                Session["MobNo"] = "1";
                divMobileNo.Visible = false;
                RfvMobNo.Enabled = false;
                divOtpDetails.Visible = false;
                btnBook.Enabled = true;
            }
            lblBranchName.Text = Session["branchName"].ToString();
            lblUserName.Text = Session["UserName"].ToString().Trim();
            lblParkingName.Text = Session["parkingName"].ToString().Trim();
            if (Session["UserRole"].ToString() == "SA")
            {
                lblUserRole.Text = "Sadmin";
            }
            else if (Session["UserRole"].ToString() == "A")
            {
                lblUserRole.Text = "Admin";
            }
            else
            {
                lblUserRole.Text = "User";

            }
        }

        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }

    #endregion

    #region Init 
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
                            if (Session["multiBook"].ToString() == "Y")
                            {
                                ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Pay At CheckOut"));
                            }
                            else if (ViewState["paylaterOption"].ToString() == "1")
                            {
                                ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Pay At CheckOut"));
                            }
                        }
                        else
                        {
                            ddlPaymentType.DataBind();
                        }
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
    #region Bind Block
    public void BindBlock()
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
                                ddlfloor.Enabled = false;
                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                                ViewState["BlockCount"] = "1";
                            }
                            else
                            {
                                ddlfloor.Enabled = true;
                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                            }
                            BindFloorName();
                        }
                        else
                        {
                            ddlblock.DataBind();
                        }
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
        try
        {
            DivSlotOtherStab.Visible = false;
            divslot.Visible = false;
            divCount.Visible = false;
            divSummary.Visible = false;
            BindFloorName();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion
    #region  Bind Floor
    public void BindFloorName()
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
                                ddlfloor.Enabled = false;
                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                                ViewState["FloorCount"] = "1";
                            }
                            else
                            {
                                ddlfloor.Enabled = true;
                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                            }
                            BindVehicle();
                        }
                        else
                        {
                            ddlfloor.Items.Clear();
                        }
                    }
                    else
                    {
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
        try
        {
            Clear();
            divslot.Visible = false;
            divCount.Visible = false;
            divSummary.Visible = false;
            BindVehicle();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
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
                      + "floorVehicleMaster?activeStatus=A&floorId=";
                sUrl += ddlfloor.SelectedValue;

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
                            gvVehicleType.DataSource = dt;
                            gvVehicleType.DataBind();

                            ViewState["data"] = dt;
                            ViewState["AccessoriesTotalAmount"] = "0";
                            ViewState["AccessoriesAmount"] = "0";
                            ViewState["AccessoriesTax"] = "0";
                            int i;
                            if (string.IsNullOrEmpty(Session["BKitemIndex"] as string))
                            {
                                i = 0;
                            }
                            else
                            {
                                i = Convert.ToInt32(Session["BKitemIndex"].ToString());
                            }

                            HtmlContainerControl divVehicles = gvVehicleType.Items[i].FindControl("divvehicles") as HtmlContainerControl;
                            divVehicles.Attributes["class"] = "carimage2";
                            HtmlContainerControl divcarname = gvVehicleType.Items[i].FindControl("divcarname") as HtmlContainerControl;
                            divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";

                            if (string.IsNullOrEmpty(Session["BKvehicleType"] as string))
                            {
                                ViewState["vehicleType"] = dt.Rows[0]["vehicleType"].ToString();
                                ViewState["VehicleTypeName"] = dt.Rows[0]["vehicleName"].ToString();
                                ViewState["vehiclePlaceHolderImageUrl"] = dt.Rows[0]["vehiclePlaceHolderImageUrl"].ToString();
                                ViewState["lblisvehicleNumberRequired"] = dt.Rows[0]["isvehicleNumberRequired"].ToString();
                            }
                            else
                            {
                                ViewState["vehicleType"] = Session["BKvehicleType"];
                                ViewState["VehicleTypeName"] = Session["BKvehicleTypeName"];
                                ViewState["vehiclePlaceHolderImageUrl"] = Session["BKvehiclePlaceHolderImageUrl"];
                                ViewState["lblisvehicleNumberRequired"] = Session["BKisvehicleNumberRequired"];
                            }

                            CheckSlotExists();
                            ImgSummary.ImageUrl = ViewState["vehiclePlaceHolderImageUrl"].ToString().Trim();
                            ImgSummary1.ImageUrl = ViewState["vehiclePlaceHolderImageUrl"].ToString().Trim();
                            lblVehicleTypeId.Text = ViewState["vehicleType"].ToString();

                            divForm.Visible = true;
                            divslot.Visible = true;
                            divCount.Visible = true;
                            divSummary.Visible = false;
                            if (ChkMobileNo.Checked == true)
                            {
                                divMobileNo.Visible = true;
                                RfvMobNo.Enabled = true;
                                btnBook.Enabled = false;
                            }
                            else
                            {
                                divMobileNo.Visible = false;
                                RfvMobNo.Enabled = false;
                                divOtpDetails.Visible = false;
                                btnBook.Enabled = true;
                            }
                            if (ViewState["lblisvehicleNumberRequired"].ToString().Trim() == "N")
                            {
                                divVehicleNo.Visible = false;
                            }
                            else
                            {
                                divVehicleNo.Visible = true;
                            }
                            if (ViewState["BookingType"].ToString().Trim() != "P")
                            {
                                divParkingAmount.Visible = true;
                            }

                            Passbackground.Style.Add("background-image", "../images/Backgroundcar.png");
                            Passbackground.Style.Add("background-repeat", "no-repeat");
                            Passbackground.Style.Add("opacity", "0.2");
                            Passbackground.Style.Add("height", "500px");
                            Passbackground.Style.Add("background-position", "center");
                            Passbackground.Style.Add("background-size", "50%");
                            Passbackground.Style.Add("margin-top", "0rem");

                            BindParkingSlot();
                            BindAccessories();
                            extraFee__container.Visible = false;
                            Divslotrow.Visible = true;
                        }
                        else
                        {
                            gvVehicleType.DataBind();
                            divForm.Visible = false;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        divForm.Visible = false;
                        gvVehicleType.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    #region Vehicle Type Click
    protected void lblVehicleTypes_Click(object sender, EventArgs e)
    {
        try
        {
            ChkMobileNo.Enabled = true;
            if (ChkMobileNo.Checked == true)
            {
                divMobileNo.Visible = true;
                RfvMobNo.Enabled = true;
                btnBook.Enabled = false;
            }
            else
            {
                divMobileNo.Visible = false;
                RfvMobNo.Enabled = false;
                divOtpDetails.Visible = false;
                btnBook.Enabled = true;
            }
            ViewState["ExtraRowCartRow"] = null;
            ViewState["ExtraRow"] = null;
            ViewState["Row"] = null;
            ViewState["CartRow"] = null;
            ViewState["AccessoriesTotalAmount"] = "0";
            ViewState["AccessoriesAmount"] = "0";
            ViewState["AccessoriesTax"] = "0";
            lblAccessoriesTotalAmount.Text = "0.00";
            dtlExtraFeeSummary.DataSource = null;
            dtlExtraFeeSummary.DataBind();
            extrafeeandfeatursContainer.Visible = false;
            TotalAmountCal();
            Divslotrow.Attributes.Add("class", "col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 table-responsive section");

            LinkButton lnkbtn = sender as LinkButton;
            DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;
            Label lblvehicleConfigId = (Label)gvrow.FindControl("lblvehicleConfigId");
            Label lblvehicleName = (Label)gvrow.FindControl("lblvehicleName");
            Label lblvehicleImageUrl = (Label)gvrow.FindControl("lblvehicleImageUrl");
            Label lblisvehicleNumberRequired = (Label)gvrow.FindControl("lblisvehicleNumberRequired");

            ViewState["vehicleType"] = lblvehicleConfigId.Text;
            ViewState["VehicleTypeName"] = lblvehicleName.Text;
            ViewState["vehiclePlaceHolderImageUrl"] = lblvehicleImageUrl.Text;
            ViewState["lblisvehicleNumberRequired"] = lblisvehicleNumberRequired.Text.Trim();


            if (ViewState["lblisvehicleNumberRequired"].ToString().Trim() == "N")
            {
                divVehicleNo.Visible = false;
            }
            else
            {
                divVehicleNo.Visible = true;
            }

            ImgSummary.ImageUrl = ViewState["vehiclePlaceHolderImageUrl"].ToString().Trim();
            ImgSummary1.ImageUrl = ViewState["vehiclePlaceHolderImageUrl"].ToString().Trim();
            lblVehicleTypeId.Text = ViewState["vehicleType"].ToString();
            extraFee__container.Visible = false;
            Divslotrow.Visible = true;
            divslot.Visible = true;
            divCount.Visible = true;
            divSummary.Visible = false;
            if (ViewState["BookingType"].ToString().Trim() != "Pass")
            {
                divParkingAmount.Visible = true;
            }
            BindParkingSlot();
            BindAccessories();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region VehicleType Color Change
    protected void gvVehicleType_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            for (int j = 0; j < gvVehicleType.Items.Count; j++)
            {
                int i = e.Item.ItemIndex;
                Session["BKitemIndex"] = Convert.ToString(e.Item.ItemIndex);
                if (i == j)
                {
                    Label divvehicle = (Label)gvVehicleType.Items[j].FindControl("lblvehicleName");
                    //divvehicle.Style.Add("color", "#fff");
                    divvehicle.Style.Add("color", "black");
                    HtmlControl divrower = gvVehicleType.Items[j].FindControl("divvehicles") as HtmlControl;
                    divrower.Attributes["class"] = "carimage2";
                    HtmlControl divcarname = gvVehicleType.Items[j].FindControl("divcarname") as HtmlControl;
                    divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";
                }
                else
                {
                    Label divvehicle = (Label)gvVehicleType.Items[j].FindControl("lblvehicleName");
                    divvehicle.Style.Add("color", "#000");
                    HtmlControl divrower = gvVehicleType.Items[j].FindControl("divvehicles") as HtmlControl;
                    divrower.Attributes["class"] = "carimage";
                    HtmlControl divcarname = gvVehicleType.Items[j].FindControl("divcarname") as HtmlControl;
                    divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname";
                }
            }
            CheckSlotExists();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region CheckSlotExists
    public void CheckSlotExists()
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
                      + "floorMaster?floorId=";
                sUrl += ddlfloor.SelectedValue + "&type=U&vehicleType=";
                sUrl += ViewState["vehicleType"];
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

                            Session["BKvehicleType"] = ViewState["vehicleType"];
                            Session["BKvehicleTypeName"] = ViewState["VehicleTypeName"].ToString();
                            Session["BKvehiclePlaceHolderImageUrl"] = ViewState["vehiclePlaceHolderImageUrl"].ToString();
                            Session["BKisvehicleNumberRequired"] = ViewState["lblisvehicleNumberRequired"];

                            if (dt.Rows[0]["slotExits"].ToString() == "N")
                            {
                                Response.Redirect("BookingWithOutSlot.aspx", false);
                            }
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

    #region Bind Parking Slot
    public void BindParkingSlot()
    {
        divslot.Visible = true;
        lblNormalCount.Text = "0";
        lblVIPCount.Text = "0";
        lblReservedCount.Text = "0";
        lblBookedCount.Text = "0";
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                        + "parkingSlot?activeStatus=A&floorId=";
                sUrl += ddlfloor.SelectedValue
                     + "&typeOfVehicle=";
                sUrl += lblVehicleTypeId.Text;
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
                            List<ParkingSlotClass> lst = JsonConvert.DeserializeObject<List<ParkingSlotClass>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.ParkingSlotDetails.ToList();
                            DataTable ParkingSlotDetails = ConvertToDataTable(lst1);
                            ViewState["ParkingSlotDetails"] = ParkingSlotDetails;
                            txtcolumn.Text = dt.Rows[0]["noOfColumns"].ToString();
                            txtrow.Text = dt.Rows[0]["noOfRows"].ToString();
                            ViewState["noOfColumns"] = dt.Rows[0]["noOfColumns"].ToString();
                            ViewState["noOfRows"] = dt.Rows[0]["noOfRows"].ToString();
                            ViewState["typeOfParking"] = dt.Rows[0]["typeOfParking"].ToString();
                            hfslotcheck.Value = "Y";
                            maindata.DataSource = lst;
                            maindata.DataBind();
                            DivSlotOtherStab.Visible = true;
                            btnAddonservices.Style.Remove("border-top");
                            btnAddonservices.Style.Remove("border-right");
                            btnAddonservices.Style.Remove("border-left");
                            btnAddonservices.Style.Add("border-bottom", "2px solid #c3c4c6");
                            btnslottab.Style.Add("border-top-left-radius", "6px");
                            btnslottab.Style.Add("border-top-right-radius", "6px");
                            btnslottab.Style.Add("border-top", "2px solid #a2a4a796");
                            btnslottab.Style.Add("border-right", "2px solid #a2a4a796");
                            btnslottab.Style.Add("border-left", "2px solid #a2a4a796");
                            btnslottab.Style.Remove("border-bottom");

                            if (dt.Rows[0]["vehicleTypeName"].ToString() == "Car")
                            {
                                divslot.Style.Add("background-image", "../images/Backgroundcar.png");
                                divslot.Style.Add("background-size", "90%");
                            }
                            else if (dt.Rows[0]["vehicleTypeName"].ToString() == "Bike")
                            {
                                divslot.Style.Add("background-image", "../images/Backgroundbike.png");
                                divslot.Style.Add("background-size", "70%");

                            }
                            divslot.Style.Add("background-repeat", "no-repeat");
                            divslot.Style.Add("opacity", "0.75");
                            divslot.Style.Add("background-position", "center");
                            Pathdesigns();
                        }
                        else
                        {
                            // Response.Redirect("BookingWithOutSlot.aspx?vehicletype=" + lblVehicleTypeId.Text + "");
                        }
                    }
                    else
                    {

                        divslot.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void maindata_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemIndex != -1)
            {
                var slots = e.Item.DataItem as ParkingSlotClass;
                var SubData = e.Item.FindControl("SubData") as DataList;
                SubData.DataSource = slots.ParkingSlotDetails;
                SubData.DataBind();
                SubData.RepeatColumns = Convert.ToInt16(txtcolumn.Text);


            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);

        }
    }
    protected void SubData_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int z = e.Item.ItemIndex;
        if (e.Item.ItemIndex != -1)
        {
            for (int j = 0; j < maindata.Items.Count; j++)
            {
                var SubData = maindata.Items[j].FindControl("SubData") as DataList;

                for (int i = 0; i < SubData.Items.Count; i++)
                {
                    LinkButton txtBox = (LinkButton)SubData.Items[i].FindControl("lblSlotNumberdtl");
                    Label slotState = (Label)SubData.Items[i].FindControl("lbldtlslotState");
                    if (z == i)
                    {
                        if ((slotState.Text.Trim() == "B") || (slotState.Text.Trim() == "R"))
                        {

                        }
                        else
                        {
                            if (Session["UserRole"].ToString() != "SA")
                            {
                                txtBox.CssClass = "slotClick";
                            }
                        }

                    }
                    else
                    {
                        txtBox.CssClass = "tdsOthers";
                    }
                }
            }
        }
    }
    public void Pathdesigns()
    {
        try
        {
            int lblNormal = 0, lblVip = 0, lblReserved = 0, lblBooked = 0;
            lblNormalCount.Text = "0";
            lblVIPCount.Text = "0";
            lblReservedCount.Text = "0";
            lblBookedCount.Text = "0";
            string VehicleNo = string.Empty;
            string vehicleNoLast = string.Empty;
            for (int j = 0; j < maindata.Items.Count; j++)
            {
                var SubData = maindata.Items[j].FindControl("SubData") as DataList;

                for (int i = 0; i < SubData.Items.Count; i++)
                {
                    DataListItem itemsss = SubData.Items[i];
                    LinkButton txtBox = SubData.Items[i].FindControl("lblSlotNumberdtl") as LinkButton;
                    Label slotState = SubData.Items[i].FindControl("lbldtlslotState") as Label;
                    Label slot = SubData.Items[i].FindControl("lbldtlslotType") as Label;
                    Label vehicleNumber = SubData.Items[i].FindControl("lbldtlvehicleNumber") as Label;
                    if (vehicleNumber.Text.Trim() != "")
                    {
                        VehicleNo = vehicleNumber.Text.Trim();
                        vehicleNoLast = VehicleNo.Substring(VehicleNo.Length - 4);
                    }
                    else
                    {
                        VehicleNo = vehicleNumber.Text.Trim();
                        vehicleNoLast = "";
                    }


                    txtBox.Controls.Clear();
                    switch (slot.Text.ToString())
                    {
                        case "DeActive":
                            txtBox.Enabled = false;
                            txtBox.Style.Add("background", "white");
                            txtBox.Attributes.Add("class", "tdsOthers");
                            break;

                        case "Path":
                            txtBox.Enabled = false;
                            txtBox.Controls.Add(new Image { ImageUrl = "../images/Path.jpg", CssClass = "tdsInOut" });
                            txtBox.Visible = true;
                            txtBox.Attributes.Add("class", "tdsOthers");
                            //txtBox.CssClass = "tdsOthersNew";
                            break;

                        case "PathV":
                            txtBox.Enabled = false;
                            txtBox.Controls.Add(new Image { ImageUrl = "../images/Path.jpg", CssClass = "tdspath" });
                            txtBox.Visible = true;
                            txtBox.Attributes.Add("class", "tdsOthers");
                            //  txtBox.CssClass = "tdsOthersNew";
                            break;

                        case "Way":
                            txtBox.Enabled = false;
                            txtBox.Style.Add("background", "#24222238");
                            txtBox.Visible = true;
                            txtBox.Attributes.Add("class", "tdsOthers");
                            break;

                        case "In":
                            txtBox.Enabled = false;
                            txtBox.Controls.Add(new Image { ImageUrl = "../images/VehicleIcon/entry.svg", CssClass = "tdsInOut" });
                            txtBox.Visible = true;
                            txtBox.Attributes.Add("class", "tdsOthers");
                            break;

                        case "Out":
                            txtBox.Enabled = false;
                            txtBox.Controls.Add(new Image { ImageUrl = "../images/VehicleIcon/exit.svg", CssClass = "tdsInOut" });
                            txtBox.Visible = true;
                            txtBox.Attributes.Add("class", "tdsOthers");
                            break;

                        case "Reserved":
                        case "VIP Reserved":
                            if (slotState.Text == "B" || slotState.Text == "R")
                            {
                                lblBooked = lblBooked + 1;
                                txtBox.Enabled = true;
                                txtBox.Controls.Add(new Image { ImageUrl = ViewState["vehiclePlaceHolderImageUrl"].ToString(), CssClass = "tdsInOut" });
                                if (vehicleNumber.Text.Trim() != "")
                                {
                                    txtBox.Controls.Add(new Label { Text = vehicleNoLast.Trim(), CssClass = "tooltiptext1" });
                                    txtBox.Controls.Add(new Label { Text = vehicleNumber.Text, CssClass = "tooltiptext" });
                                }
                                txtBox.Attributes.Add("class", "tdsOthers");
                            }
                            else
                            {
                                txtBox.Enabled = true;
                                txtBox.Style.Add("color", "#000");
                                txtBox.Style.Add("background", "#DFF6FF");
                                lblReserved = lblReserved + 1;
                            }
                            break;

                        case "VIP":
                            if (slotState.Text == "B" || slotState.Text == "R")
                            {
                                lblBooked = lblBooked + 1;
                                txtBox.Enabled = true;
                                txtBox.Controls.Add(new Image { ImageUrl = ViewState["vehiclePlaceHolderImageUrl"].ToString(), CssClass = "tdsInOut" });
                                if (vehicleNumber.Text.Trim() != "")
                                {
                                    txtBox.Controls.Add(new Label { Text = vehicleNoLast.Trim(), CssClass = "tooltiptext1" });
                                    txtBox.Controls.Add(new Label { Text = vehicleNumber.Text, CssClass = "tooltiptext" });
                                }

                                txtBox.Attributes.Add("class", "tdsOthers");
                            }

                            else
                            {
                                txtBox.Style.Add("color", "#000");
                                txtBox.Style.Add("background", "#ffee2162");

                                lblVip = lblVip + 1;
                            }
                            break;


                        case "Lane Top":
                            if (txtBox.Text == "WP")
                            {
                                txtBox.Enabled = false;
                                txtBox.Visible = true;
                                txtBox.Attributes.Add("class", "tdsOthers");
                                txtBox.Style.Add("color", "white");
                            }
                            else
                            {
                                txtBox.Enabled = false;
                                txtBox.Style.Add("box-shadow", "#00000059 0px 4px 8px");
                                txtBox.Visible = true;
                                txtBox.Attributes.Add("class", "tdsOthers");
                            }

                            itemsss.CssClass = "LaneTop";
                            break;
                        case "Lane No":
                            if (txtBox.Text == "WP")
                            {
                                txtBox.Enabled = false;
                                txtBox.Visible = true;
                                txtBox.Attributes.Add("class", "tdsOthers");
                                txtBox.Style.Add("color", "white");
                                txtBox.Style.Add("box-shadow", "none !important");

                            }
                            else
                            {
                                txtBox.Enabled = false;
                                //txtBox.Style.Add("background", "#24222238");
                                txtBox.Attributes.Add("class", "tdsOthers");
                                txtBox.Style.Add("box-shadow", "#00000059 0px 4px 8px");
                                txtBox.Visible = true;


                            }
                            itemsss.CssClass = "LaneNo";
                            break;
                        default:
                            if (slotState.Text == "B" || slotState.Text == "R")
                            {
                                lblBooked = lblBooked + 1;
                                txtBox.Enabled = true;
                                txtBox.Controls.Add(new Image { ImageUrl = ViewState["vehiclePlaceHolderImageUrl"].ToString(), CssClass = "tdsInOut" });
                                if (vehicleNumber.Text.Trim() != "")
                                {
                                    txtBox.Controls.Add(new Label { Text = vehicleNoLast.Trim(), CssClass = "tooltiptext1" });
                                    txtBox.Controls.Add(new Label { Text = vehicleNumber.Text, CssClass = "tooltiptext" });
                                }

                                txtBox.Attributes.Add("class", "tdsOthers");
                            }
                            else
                            {
                                txtBox.Enabled = true;
                                txtBox.Style.Add("background", " #40F44336");
                                txtBox.Style.Add("color", "#000");
                                lblNormal = lblNormal + 1;
                            }
                            break;
                    }

                }
            }
            lblNormalCount.Text = Convert.ToString(lblNormal);
            lblVIPCount.Text = Convert.ToString(lblVip);
            //lblReservedCount.Text = Convert.ToString(lblReserved);
            //lblBookedCount.Text = Convert.ToString(lblBooked);

            divCount.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void lblSlotNumberdtl_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
            if (ChkMobileNo.Checked == true)
            {
                divPrintCheck.Visible = true;
            }
            else
            {
                divPrintCheck.Visible = false;
            }

            ChkMobileNo.Enabled = false;
            //btnBook.Enabled = true;
            divSummary.Visible = true;
            divlaneSlotNo.Visible = true;
            ImgSummary.Visible = true;
            ImgSummary1.Visible = true;
            Divslotrow.Attributes.Add("class", "col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8 table-responsive section");
            LinkButton lnkbtn = sender as LinkButton;
            DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;
            LinkButton txtBox = (LinkButton)gvrow.FindControl("lblSlotNumberdtl");
            Label slotState = (Label)gvrow.FindControl("lbldtlslotState");
            Label slot = (Label)gvrow.FindControl("lbldtlslotType");
            Label vehicleNumber = (Label)gvrow.FindControl("lbldtlvehicleNumber");
            Label lbldtlparkingSlotId = (Label)gvrow.FindControl("lbldtlparkingSlotId");
            Label lbldtlparkingLotLineId = (Label)gvrow.FindControl("lbldtlparkingLotLineId");
            Label lbldtllaneNumber = (Label)gvrow.FindControl("lbldtllaneNumber");
            Label lbldtlrowId = (Label)gvrow.FindControl("lbldtlrowId");
            Label lbldtlcolumnId = (Label)gvrow.FindControl("lbldtlcolumnId");
            Label lbldtlisChargeUnitAvailable = (Label)gvrow.FindControl("lbldtlisChargeUnitAvailable");
            Label lbldtlchargePinType = (Label)gvrow.FindControl("lbldtlchargePinType");
            Label lbldtlactiveStatus = (Label)gvrow.FindControl("lbldtlactiveStatus");

            if (ViewState["lblisvehicleNumberRequired"].ToString().Trim() == "N")
            {
                divVehicleNo.Visible = false;
            }
            else
            {
                divVehicleNo.Visible = true;
            }
            if (Session["UserRole"].ToString() != "SA")
            {
                rbtnTimeType.SelectedValue = "D";
                divTodate.Visible = true;
                ViewState["parkingSlotId"] = lbldtlparkingSlotId.Text;
                ViewState["parkingLotLineId"] = lbldtlparkingLotLineId.Text;
                ViewState["laneNumber"] = lbldtllaneNumber.Text;
                ViewState["SlotNumber"] = txtBox.Text;
                ViewState["rowId"] = lbldtlrowId.Text;
                ViewState["columnId"] = lbldtlcolumnId.Text;
                ViewState["isChargeUnitAvailable"] = lbldtlisChargeUnitAvailable.Text;
                ViewState["chargePinType"] = lbldtlchargePinType.Text;
                ViewState["activeStatus"] = lbldtlactiveStatus.Text;
                ViewState["slotType"] = slot.Text;
                lblLaneSlot.Text = ViewState["laneNumber"].ToString() + "-" + ViewState["SlotNumber"].ToString();
                lblLaneslotsNo.Text = ViewState["laneNumber"].ToString() + "-" + ViewState["SlotNumber"].ToString();

                if (slotState.Text.Trim() == "B" || slotState.Text.Trim() == "R")
                {
                    divSummary.Visible = false;
                    BindDdlPayment();
                    BindSlotClickDetails(ViewState["parkingSlotId"].ToString().Trim());
                }
                else
                {
                    txtTodate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    txtTotime.Text = DateTime.Now.AddHours(1).ToString("HH:MM");
                    rbtnTimeType.SelectedValue = "D";
                    ViewState["BookingType"] = "N";
                    divParkingAmount.Visible = true;
                    divTimeType.Visible = true;
                    divPassdetails.Visible = false;
                    divOtpDetails.Visible = false;
                    GetSlotAmount();
                    //TotalAmountCal();
                }

            }
            else
            {
                divSummary.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Booking Not Allowed For Sadmin');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    DataTable ConvertToDataTable<TSource>(IEnumerable<TSource> source)
    {
        var dt = new DataTable();
        try
        {
            var props = typeof(TSource).GetProperties();

            dt.Columns.AddRange(
              props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray()
            );

            source.ToList().ForEach(
              i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
            );
            return dt;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
            return null;
        }

    }
    #endregion

    #region Get PIN
    public void GetPIN()
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
                        + "generatePin";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        hfPinNo.Value = ResponseMsg;
                        ViewState["PinNo"] = ResponseMsg;
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

    #region TimeType/Amount
    #region TimeType SelectedIndexChanged 
    protected void rbtnTimeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Pathdesigns();
            if (rbtnTimeType.SelectedValue == "D")
            {
                divfromdate.Visible = false;
                divTodate.Visible = true;
                divFromTime.Visible = false;
                divTotime.Visible = false;
                txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTodate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                divfromdate.Visible = false;
                divTodate.Visible = false;
                divFromTime.Visible = false;
                divTotime.Visible = true;
                txtfromtime.Text = DateTime.Now.ToString("HH:mm");
                txtTotime.Text = DateTime.Now.AddHours(1).ToString("HH:mm");
            }
            GetSlotAmount();
            //TotalAmountCal();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Totime TextChanged
    protected void txtTotime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Pathdesigns();
            GetSlotAmount();
            //TotalAmountCal();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion
    #region Get Slot Amount 
    public void GetSlotAmount()
    {
        try
        {
            string Duration = string.Empty;
            if (rbtnTimeType.SelectedValue == "D")
            {
                string Fromdate = string.Empty;
                string Todate = string.Empty;

                int NoofDays = 0;

                Fromdate = DateTime.Now.ToString("yyyy-MM-dd");
                Todate = txtTodate.Text;
                DateTime todaytime = DateTime.Now;
                string today = todaytime.ToString("yyyy-MM-dd"); ;
                if (Convert.ToDateTime(txtTodate.Text) <= Convert.ToDateTime(today))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('To Date Should Greater Than Present Date');", true);
                    txtTodate.Focus();
                    txtTodate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    return;
                }
                if (Todate == "")
                {
                    Todate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
                if (Fromdate == Todate)
                {
                    NoofDays = 1;
                }
                else
                {
                    DateTime Date1 = Convert.ToDateTime(Fromdate);
                    DateTime Date2 = Convert.ToDateTime(Todate);
                    NoofDays = (Date2 - Date1).Days;
                }
                int DaysToHour = NoofDays * 24;
                Duration = DaysToHour.ToString();

            }
            else
            {
                string FromTime = string.Empty;
                string ToTime = string.Empty;
                FromTime = DateTime.Now.ToString("HH:mm");
                ToTime = txtTotime.Text;
                DateTime todaytime = DateTime.Now;
                string today = todaytime.ToString("HH:mm");
                if (Convert.ToDateTime(txtTotime.Text) <= Convert.ToDateTime(today))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('To Time Should Greater Than Present Time');", true);
                    txtTotime.Focus();
                    txtTotime.Text = DateTime.Now.AddHours(1).ToString("HH:mm");
                    return;
                }
                int NoofHours = 1;

                if (FromTime == ToTime)
                {
                    NoofHours = 1;
                }
                else
                {
                    DateTime Date1 = Convert.ToDateTime(FromTime);
                    DateTime Date2 = Convert.ToDateTime(ToTime).AddMinutes(1);
                    NoofHours = (Date2 - Date1).Hours;

                }
                Duration = NoofHours.ToString();
            }
            string usermode = string.Empty;
            if (ViewState["slotType"].ToString() == "VIP")
            {
                usermode = "V";
            }
            else
            {
                usermode = "N";
            }
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "priceMaster?activeStatus=A&timeType=TS"
                          + "&floorId=" + ddlfloor.SelectedValue + "&userMode=" + usermode + "&idType=V&type=C&"
                          + "toDuration=" + Duration + "&vehicleAccessories="
                          + ViewState["vehicleType"].ToString();

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
                            ViewState["TaxId"] = dt.Rows[0]["taxId"].ToString();
                            Decimal SlotTotalAmount = Convert.ToDecimal(dt.Rows[0]["totalAmount"].ToString());
                            Decimal SlotAmount = Convert.ToDecimal(dt.Rows[0]["amount"].ToString());
                            Decimal SlotTax = Convert.ToDecimal(dt.Rows[0]["taxAmount"].ToString());
                            ViewState["SlotFinalAmount"] = SlotAmount;
                            ViewState["SlotFinalTax"] = SlotTax;
                            ViewState["SlotFinalTotalAmount"] = SlotTotalAmount;
                        }
                        else
                        {
                            ViewState["SlotFinalAmount"] = "0.00";
                            ViewState["SlotFinalTax"] = "0.00";
                            ViewState["SlotFinalTotalAmount"] = "0.00";
                            btnBook.Enabled = false;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Add Price Details');", true);
                        }
                    }
                    else
                    {
                        ViewState["SlotFinalAmount"] = "0.00";
                        ViewState["SlotFinalTax"] = "0.00";
                        ViewState["SlotFinalTotalAmount"] = "0.00";
                        btnBook.Enabled = false;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Add Price Details');", true);
                    }
                    TotalAmountCal();
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
    #endregion

    #region  Check Paylater Option and Binding Pass Dropdown Based On Mobile No
    #region Todate Multi Book Validate
    protected void txtTodate_TextChanged(object sender, EventArgs e)
    {
        Pathdesigns();
        DateTime date = DateTime.Now.AddDays(1);
        DateTime todate = Convert.ToDateTime(txtTodate.Text);

        if (todate > date)
        {
            if (Session["multiBook"].ToString() == "N")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Booking Allowed For Only One Day');", true);
                txtTodate.Text = date.ToString("yyyy-MM-dd");
            }
            else
            {
                txtTodate.Text = todate.ToString("yyyy-MM-dd");

            }

        }
        else
        {
            txtTodate.Text = todate.ToString("yyyy-MM-dd");
        }

        hftodate.Value = todate.ToString();
        ScriptManager.RegisterStartupScript(this, GetType(), "dates", "dates()", true);
        GetSlotAmount();
        //TotalAmountCal();

    }
    #endregion
    #region MobileNo Changed Event
    protected void txtmobileNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (lblLaneSlot.Text != "")
            {
                ViewState["paylaterOption"] = "0";
                divSummaryColor.Style.Add("background-color", "#c1c0c36b;");
                Pathdesigns();
                BindAccessories();

                rbtnTimeType.SelectedValue = "D";
                divTodate.Visible = true;
                divTotime.Visible = false;
                ViewState["BookingType"] = "N";
                divParkingAmount.Visible = true;
                divTimeType.Visible = true;
                divPassdetails.Visible = false;
                ViewState["ExtraRowCartRow"] = null;
                ViewState["ExtraRow"] = null;
                ViewState["Row"] = null;
                ViewState["CartRow"] = null;
                ViewState["AccessoriesTotalAmount"] = "0";
                ViewState["AccessoriesAmount"] = "0";
                ViewState["AccessoriesTax"] = "0";
                lblAccessoriesTotalAmount.Text = "0.00";
                divExtraFeeAmount.Visible = false;
                dtlExtraFeeSummary.DataSource = null;
                dtlExtraFeeSummary.DataBind();
                extrafeeandfeatursContainer.Visible = false;
                GetSlotAmount();
                //TotalAmountCal();
                if (txtmobileNo.Text.Length < 10)
                {
                    GetPassDetailsPass(txtmobileNo.Text);
                }
                else
                {
                    BindPassIds();
                }
                BindVehicleNo();
                Bindpaylateroption();
                GetPaymentType();
                txtVehicleNo.Focus();
                if (ViewState["lblisvehicleNumberRequired"].ToString().Trim() == "N")
                {
                    divVehicleNo.Visible = false;
                }
                //else
                //{
                //    divVehicleNo.Visible = true;
                //}
            }
            if (ChkMobileNo.Checked == true)
            {
                divOtpDetails.Visible = true;
                divSendOtp.Visible = true;
                btnBook.Enabled = false;
            }
            else
            {
                divOtpDetails.Visible = false;
                divSendOtp.Visible = false;
                btnBook.Enabled = true;
            }
            divEnterOtp.Visible = false;
            divResend.Visible = false;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion
    #region Pay later option  
    public void Bindpaylateroption()
    {
        DateTime utcTime = DateTime.Now;
        string date = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
        string[] time = date.Split('T');
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?phoneNumber=" + txtmobileNo.Text + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        List<GetBooking> lst = JsonConvert.DeserializeObject<List<GetBooking>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(lst);
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.Sort = "bookingId DESC";
                            dt = dv.ToTable();
                            DateTime dates = DateTime.Now;
                            string DurationType = dt.Rows[0]["bookingDurationType"].ToString();
                            DateTime fromDate = Convert.ToDateTime(dt.Rows[0]["fromDate"].ToString());
                            DateTime toDate = Convert.ToDateTime(dt.Rows[0]["toDate"].ToString());
                            DateTime fromTime = Convert.ToDateTime(dt.Rows[0]["fromTime"].ToString());
                            DateTime toTime = Convert.ToDateTime(dt.Rows[0]["toTime"].ToString());
                            string Todaydate = dates.ToString("dd/MM/yyyy");
                            string fromDates = fromDate.ToString("dd/MM/yyyy");
                            string toDates = toDate.ToString("dd/MM/yyyy");
                            string Todaytime = dates.ToString("HH:SS");
                            string fromTimes = fromTime.ToString("dd/MM/yyyy");
                            string toTimes = toTime.ToString("dd/MM/yyyy");
                            if (DurationType == "D")
                            {
                                if (Todaydate == fromDates || Todaydate == toDates)
                                {
                                    ViewState["paylaterOption"] = "1";
                                }
                                else
                                {
                                    ViewState["paylaterOption"] = "0";
                                }

                            }
                            else
                            {
                                if (Todaytime == fromTimes || Todaytime == toTimes)
                                {
                                    ViewState["paylaterOption"] = "1";
                                }
                                else
                                {
                                    ViewState["paylaterOption"] = "0";
                                }
                            }

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
    #region Bind Pass Id Drop down
    public void BindPassIds()
    {

        try
        {
            string usermode = string.Empty;
            if (ViewState["slotType"].ToString() == "VIP")
            {
                usermode = "V";
            }
            else
            {
                usermode = "N";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "passTransaction?phoneNumber=" + txtmobileNo.Text + "&vehicleType=" + ViewState["vehicleType"].ToString() + ""
                      + "&userMode=" + usermode.Trim() + ""
                      + "&branchId=" + Session["branchId"].ToString().Trim() + "";
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
                            GetPassDetailsPass(dt.Rows[0]["parkingPassTransId"].ToString());
                            divTax.Visible = false;
                        }
                        else
                        {
                            ViewState["BookingType"] = "N";
                            divParkingAmount.Visible = true;
                            divTax.Visible = true;
                            divTimeType.Visible = true;
                            divPassdetails.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        ViewState["BookingType"] = "N";
                        divParkingAmount.Visible = true;
                        divTax.Visible = true;
                        divTimeType.Visible = true;
                        divPassdetails.Visible = false;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ViewState["BookingType"] = "N";
                    divParkingAmount.Visible = true;
                    divTax.Visible = true;
                    divTimeType.Visible = true;
                    divPassdetails.Visible = false;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }
                TotalAmountCal();
                if (ViewState["BookingType"].ToString() == "P")
                {
                    divTax.Visible = false;
                    lblGSTAmount.Text = "0.00";
                }
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Bind Vehicle No based on  MobileNo   
    public void BindVehicleNo()
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
                          + "vehicleMaster?phoneNumber=" + txtmobileNo.Text + "&vehicleType=" + ViewState["vehicleType"].ToString() + "";
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
                            divVehicleNoDropdown.Visible = true;
                            ddlVehicle.DataSource = dt;
                            ddlVehicle.DataValueField = "vehicleNumber";
                            ddlVehicle.DataTextField = "vehicleNumber";
                            ddlVehicle.DataBind();
                            txtVehicleNo.Enabled = false;
                            divVehicleNo.Visible = false;
                            txtVehicleNo.Text = dt.Rows[0]["vehicleNumber"].ToString();
                        }
                        else
                        {
                            divVehicleNoDropdown.Visible = false;
                            divVehicleNo.Visible = true;
                            ddlVehicle.DataBind();
                        }
                        //ddlVehicle.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {

                        divVehicleNoDropdown.Visible = false;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlVehicle.ClearSelection();
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

    protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtVehicleNo.Text = ddlVehicle.SelectedItem.Text;
        Pathdesigns();
    }


    protected void lnkAddVehicle_Click(object sender, EventArgs e)
    {
        divVehicleNoDropdown.Visible = false;
        txtVehicleNo.Enabled = true;
        divVehicleNo.Visible = true;
        txtVehicleNo.Text = "";
        lnkAddBack.Visible = true;
        txtVehicleNo.Attributes.Add("placeholder", "XX00XX0000");
        Pathdesigns();
    }

    protected void lnkAddBack_Click(object sender, EventArgs e)
    {
        divVehicleNoDropdown.Visible = true;
        txtVehicleNo.Enabled = false;
        divVehicleNo.Visible = false;
        txtVehicleNo.Text = "";
        lnkAddBack.Visible = false;
        txtVehicleNo.Attributes.Add("placeholder", "XX00XX0000");
        Pathdesigns();
    }
    #endregion

    #endregion

    #region Booking
    #region Booking  Click 
    protected void btnBook_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblLaneSlot.Text == "")
            {
                RfvVehicleNo.Enabled = false;
                revVehicleNo.Enabled = false;
            }
            else
            {
                RfvVehicleNo.Enabled = true;
                revVehicleNo.Enabled = true;
                if (ViewState["lblisvehicleNumberRequired"].ToString().Trim() == "Y")
                {
                    if (txtVehicleNo.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Enter Vehicle No.');", true);
                        return;
                    }

                }

            }
            if (ChkMobileNo.Checked == true)
            {
                if (txtmobileNo.Text.Length < 10)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Enter 10 Digit Mobile No.');", true);
                    return;
                }
            }


            if (rbtnTimeType.SelectedValue == "H")
            {
                DateTime todaytime = DateTime.Now;
                string today = todaytime.ToString("HH:mm");
                if (Convert.ToDateTime(txtTotime.Text) <= Convert.ToDateTime(today))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('To Time Should be Greater Than Present Time');", true);
                    txtTotime.Focus();
                    txtTotime.Text = DateTime.Now.AddHours(1).ToString("HH:mm");
                    return;
                }
            }
            if (rbtnTimeType.SelectedValue == "D")
            {
                DateTime todaytime = DateTime.Now;
                string today = todaytime.ToString("yyyy-MM-dd"); ;
                if (Convert.ToDateTime(txtTodate.Text) <= Convert.ToDateTime(today))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('To Date Should be Greater Than Present Date');", true);
                    txtTodate.Focus();
                    txtTodate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    return;
                }
            }
            btnBook.Enabled = false;
            GetPIN();
            if (ViewState["BookingType"].ToString().Trim() == "N")
            {
                // TotalAmountCal();
                BookingFunction();
            }
            else
            {
                PassBookingFunction();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Total Amount Cal    
    public void TotalAmountCal()
    {
        try
        {
            decimal SlotAmount = 0, AccTotal = 0, finaltax = 0;
            if (ViewState["BookingType"].ToString() == "P")
            {
                ViewState["SlotFinalTax"] = "0.00";
            }
            if (lblLaneSlot.Text != "")
            {
                divParkingAmount.Visible = true;
                finaltax = Convert.ToDecimal(ViewState["AccessoriesTax"].ToString()) + Convert.ToDecimal(ViewState["SlotFinalTax"].ToString());
            }
            else
            {
                divParkingAmount.Visible = false;
                finaltax = Convert.ToDecimal(ViewState["AccessoriesTax"].ToString());
            }


            lblGSTAmount.Text = finaltax.ToString("0.00");
            if (lblLaneSlot.Text != "")
            {
                lblparkingAmount.Text = Convert.ToDecimal(ViewState["SlotFinalAmount"].ToString()).ToString("0.00");
                lblTotalAmount.Text = Convert.ToDecimal(ViewState["SlotFinalTotalAmount"].ToString()).ToString("0.00");
                if (ViewState["BookingType"].ToString().Trim() != "P")
                {
                    SlotAmount = Convert.ToDecimal(ViewState["SlotFinalTotalAmount"]);
                    divTimeType.Visible = true;
                }
                else
                {
                    divParkingAmount.Visible = false;
                    ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Pay At CheckOut"));
                }
            }
            if (dtlExtraFeeSummary.Rows.Count > 0)
            {
                divTax.Visible = true;
                AccTotal = Convert.ToDecimal(ViewState["AccessoriesTotalAmount"]);
            }

            decimal TotalAmount = SlotAmount + AccTotal;
            ViewState["TotalAmount"] = TotalAmount;
            lblTotalAmount.Text = Convert.ToDecimal(ViewState["TotalAmount"].ToString()).ToString("0.00");
            if (lblTotalAmount.Text == "0.00")
            {
                divParkingAmount.Visible = false;
                divTax.Visible = false;
                divPaymentTypeBook.Visible = false;
            }
            else
            {
                divTax.Visible = true;
                divPaymentTypeBook.Visible = true;
            }
            btnBook.Text = lblTotalAmount.Text;
            if (Convert.ToDecimal(lblGSTAmount.Text) == 0)
            {
                divTax.Visible = false;
            }
            else
            {
                divTax.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Booking Function
    public void BookingFunction()
    {
        try
        {
            txtVehicleNo.Text = txtVehicleNo.Text.ToUpper();
            string MobNo = txtmobileNo.Text;
            string accessories = string.Empty;
            string VehicleCheckInStatus = string.Empty;
            string VehicleCheckIntime = string.Empty;
            string BfromDate = string.Empty;
            string BToDate = string.Empty;
            DateTime utcTime = DateTime.Now;
            string times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            string[] time = times.Split('T');
            if (rbtnTimeType.SelectedValue == "H")
            {
                DateTime toTime = Convert.ToDateTime(txtTotime.Text);
                string Tfromtime = toTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'0''");
                string[] ToDates = Tfromtime.Split('T');
            }
            //else
            //{

            DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime Todate = Convert.ToDateTime(txtTodate.Text);
            BfromDate = FromDate.ToString("yyyy-MM-dd");
            BToDate = Todate.ToString("yyyy-MM-dd");
            //}
            if (dtlExtraFeeSummary.Rows.Count > 0)
            {
                accessories = "Y";
            }
            else
            {
                accessories = "N";
            }
            if (Session["isBookCheckInAvailable"].ToString().Trim() == "Y")
            {
                VehicleCheckIntime = times;
                VehicleCheckInStatus = "I";
            }
            else
            {
                VehicleCheckIntime = "";
                VehicleCheckInStatus = "";
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new Booking();
                if (lblLaneSlot.Text == "")
                {
                    Insert = new Booking()
                    {
                        parkingOwnerId = Session["parkingOwnerId"].ToString(),
                        branchId = Session["branchId"].ToString(),
                        blockId = ddlblock.SelectedValue,
                        floorId = ddlfloor.SelectedValue,
                        userId = Session["UserId"].ToString(),
                        phoneNumber = txtmobileNo.Text,
                        booking = "DW",
                        loginType = Session["UserRole"].ToString(),
                        fromTime = time[1],
                        fromDate = time[0],
                        accessories = accessories,
                        bookingType = "P",
                        paymentStatus = ddlPaymentType.SelectedItem.Text == "Pay At CheckOut" ? "N" : "P",
                        paymentType = ddlPaymentType.SelectedValue,
                        totalAmount = ViewState["TotalAmount"].ToString(),
                        pinNo = ViewState["PinNo"].ToString(),
                        createdBy = Session["UserId"].ToString(),
                    };


                    if (dtlExtraFeeSummary.Rows.Count > 0)
                    {
                        string feePriceids = string.Empty;
                        string feetureCount = string.Empty;
                        string feeName = string.Empty;
                        string feeAmount = string.Empty;
                        foreach (GridViewRow item in dtlExtraFeeSummary.Rows)
                        {

                            Label lblgvfeeTimeSlabId = item.FindControl("lblgvfeeTimeSlabId") as Label;
                            Label lblfeeCount = item.FindControl("lblfeeCount") as Label;
                            Label lblfeevehicleAccessoriesName = item.FindControl("lblfeevehicleAccessoriesName") as Label;
                            Label lblgvfeeTotalAmount = item.FindControl("lblgvfeeTotalAmount") as Label;

                            feePriceids += lblgvfeeTimeSlabId.Text + ',';
                            feetureCount += lblfeeCount.Text + ',';
                            feeAmount += lblgvfeeTotalAmount.Text + ',';
                            feeName += lblfeevehicleAccessoriesName.Text + ',';
                        }
                        Insert.extraFees = GetExtraFeesDetails(feetureCount.ToString().TrimEnd(','),
                            feePriceids.ToString().TrimEnd(','), feeName.ToString().TrimEnd(','),
                            feeAmount.ToString().TrimEnd(','));
                    }
                    if (Insert.paymentStatus == "P")
                    {
                        Insert.paidAmount = Insert.totalAmount;

                    }
                }
                else
                {
                    Insert = new Booking()
                    {
                        parkingOwnerId = Session["parkingOwnerId"].ToString(),
                        branchId = Session["branchId"].ToString(),
                        blockId = ddlblock.SelectedValue,
                        floorId = ddlfloor.SelectedValue,
                        userId = Session["UserId"].ToString(),
                        phoneNumber = txtmobileNo.Text,
                        booking = "DW",
                        loginType = Session["UserRole"].ToString(),
                        bookingDurationType = rbtnTimeType.SelectedValue,
                        fromTime = txtfromtime.Text == "" ? time[1] : txtfromtime.Text,
                        toTime = txtTotime.Text == "" ? time[1] : txtTotime.Text,
                        fromDate = BfromDate.Trim() == "" ? time[0] : BfromDate.Trim(),
                        toDate = BToDate.Trim() == "" ? time[0] : BToDate.Trim(),
                        accessories = accessories,
                        bookingType = "P",
                        taxId = ViewState["TaxId"].ToString(),
                        paymentStatus = ddlPaymentType.SelectedItem.Text == "Pay At CheckOut" ? "N" : "P",
                        paymentType = ddlPaymentType.SelectedValue,
                        totalAmount = ViewState["TotalAmount"].ToString(),
                        pinNo = ViewState["PinNo"].ToString(),
                        createdBy = Session["UserId"].ToString(),
                        userSlotDetails = GetUserSlotDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString()),
                    };
                    //if (rbtnTimeType.SelectedValue == "H")
                    //{
                    //    Insert.fromTime = txtfromtime.Text == "" ? time[1] : txtfromtime.Text;
                    //    Insert.toTime = txtTotime.Text == "" ? time[1] : txtTotime.Text;
                    //}
                    //else
                    //{
                    //    Insert.fromDate = BfromDate.Trim() == "" ? time[0] : BfromDate.Trim();
                    //    Insert.toDate = BToDate.Trim() == "" ? time[0] : BToDate.Trim();
                    //}
                    if (Session["isBookCheckInAvailable"].ToString().Trim() == "Y")
                    {
                        if (txtVehicleNo.Text == "")
                        {
                            Insert.vehicleHeaderDetails = GetvehicleHeaderDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString(), VehicleCheckIntime, VehicleCheckInStatus);
                        }
                        else
                        {
                            Insert.vehicleHeaderDetails = GetvehicleHeaderDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString(), txtVehicleNo.Text, VehicleCheckIntime, VehicleCheckInStatus);
                        }

                    }
                    else
                    {
                        if (txtVehicleNo.Text == "")
                        {
                            Insert.vehicleHeaderDetails = GetvehicleHeaderDetails(ViewState["vehicleType"].ToString());

                        }
                        else
                        {
                            Insert.vehicleHeaderDetails = GetvehicleHeaderDetails(ViewState["vehicleType"].ToString(), txtVehicleNo.Text);
                        }


                    }
                    if (dtlExtraFeeSummary.Rows.Count > 0)
                    {
                        string feePriceids = string.Empty;
                        string feetureCount = string.Empty;
                        string feeName = string.Empty;
                        string feeAmount = string.Empty;
                        foreach (GridViewRow item in dtlExtraFeeSummary.Rows)
                        {
                            Label lblgvfeeTimeSlabId = item.FindControl("lblgvfeeTimeSlabId") as Label;
                            Label lblfeeCount = item.FindControl("lblfeeCount") as Label;
                            Label lblfeevehicleAccessoriesName = item.FindControl("lblfeevehicleAccessoriesName") as Label;
                            Label lblgvfeeTotalAmount = item.FindControl("lblgvfeeTotalAmount") as Label;

                            feePriceids += lblgvfeeTimeSlabId.Text + ',';
                            feetureCount += lblfeeCount.Text + ',';
                            feeAmount += lblgvfeeTotalAmount.Text + ',';
                            feeName += lblfeevehicleAccessoriesName.Text + ',';
                        }
                        Insert.extraFees = GetExtraFeesDetails(feetureCount.ToString().TrimEnd(','),
                            feePriceids.ToString().TrimEnd(','), feeName.ToString().TrimEnd(','),
                            feeAmount.ToString().TrimEnd(','));
                    }
                    if (Insert.paymentStatus == "P")
                    {
                        Insert.paidAmount = Insert.totalAmount;

                    }
                    else
                    {
                        Insert.paidAmount = "0";
                    }
                }

                HttpResponseMessage response = client.PostAsJsonAsync("bookingMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        string ResponseMsg1 = JObject.Parse(SmartParkingList)["bookingId"].ToString();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        if (ddlPaymentType.SelectedItem.Text == "Online" && txtmobileNo.Text != "")
                        {
                            SendPaymentLink(txtmobileNo.Text, ResponseMsg1);
                        }
                        else
                        {
                            if (ChkMobileNo.Checked == true)
                            {
                                if (ChkPrint.Checked == true)
                                {

                                    if (lblLaneSlot.Text == "")
                                    {
                                        Response.Redirect("Print.aspx?rt=Ob&bi=" + ResponseMsg1.ToString() + "", false);
                                    }
                                    else
                                    {
                                        Response.Redirect("Print.aspx?rt=bW&bi=" + ResponseMsg1.ToString() + "", false);
                                    }
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                }
                            }
                            else
                            {
                                if (lblLaneSlot.Text == "")
                                {
                                    Response.Redirect("Print.aspx?rt=Ob&bi=" + ResponseMsg1.ToString() + "", false);
                                }
                                else
                                {
                                    Response.Redirect("Print.aspx?rt=bW&bi=" + ResponseMsg1.ToString() + "", false);
                                }
                            }



                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                }

                if (MobNo != "")
                {
                    Session["MobNo"] = null;
                }
                else
                {
                    Session["MobNo"] = "1";
                }
                Clear();
                //GetvehicleHeaderDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString(), txtVehicleNo.Text, times, "I").Clear();
                //GetUserSlotDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString()).Clear();
                BindParkingSlot();
                divSummary.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Vehicle Header Details
    public static List<vehicleHeaderDetails> GetvehicleHeaderDetails(string slotId, string vehicleType, string vehicleNumber,
        string inTime, string vehicleStatus)
    {

        string[] slotIds;
        string[] vehicleTypes;
        string[] vehicleNumbers;
        string[] inTimes;
        string[] vehicleStatuss;
        slotIds = slotId.Split(',');
        vehicleTypes = vehicleType.Split(',');
        vehicleNumbers = vehicleNumber.Split(',');
        inTimes = inTime.Split(',');
        vehicleStatuss = vehicleStatus.Split(',');
        List<vehicleHeaderDetails> lst = new List<vehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<vehicleHeaderDetails>
            {
                new vehicleHeaderDetails {slotId=slotIds[i], vehicleType=vehicleTypes[i],vehicleNumber=vehicleNumbers[i],inTime=inTimes[i]
                ,vehicleStatus = vehicleStatuss[i] }

            });
        }

        return lst;

    }

    public static List<vehicleHeaderDetails> GetvehicleHeaderDetails(string slotId, string vehicleType,
       string inTime, string vehicleStatus)
    {

        string[] slotIds;
        string[] vehicleTypes;

        string[] inTimes;
        string[] vehicleStatuss;
        slotIds = slotId.Split(',');
        vehicleTypes = vehicleType.Split(',');

        inTimes = inTime.Split(',');
        vehicleStatuss = vehicleStatus.Split(',');
        List<vehicleHeaderDetails> lst = new List<vehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<vehicleHeaderDetails>
            {
                new vehicleHeaderDetails {slotId=slotIds[i], vehicleType=vehicleTypes[i],inTime=inTimes[i]
                ,vehicleStatus = vehicleStatuss[i] }

            });
        }

        return lst;

    }
    public static List<vehicleHeaderDetails> GetvehicleHeaderDetails(string vehicleType, string vehicleNumber)
    {
        string[] vehicleTypes;
        string[] vehicleNumbers;

        vehicleTypes = vehicleType.Split(',');
        vehicleNumbers = vehicleNumber.Split(',');

        List<vehicleHeaderDetails> lst = new List<vehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<vehicleHeaderDetails>
            {
                new vehicleHeaderDetails { vehicleType=vehicleTypes[i],vehicleNumber=vehicleNumbers[i] }

            });
        }

        return lst;

    }

    public static List<vehicleHeaderDetails> GetvehicleHeaderDetails(string vehicleType)
    {
        string[] vehicleTypes;
        vehicleTypes = vehicleType.Split(',');
        List<vehicleHeaderDetails> lst = new List<vehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<vehicleHeaderDetails>
            {
                new vehicleHeaderDetails { vehicleType=vehicleTypes[i]}

            });
        }

        return lst;

    }
    #endregion
    #region Extra Features Details
    public static List<extraFeaturesDetails> GetExtraFeaturesDetails(string floorFeaturesId, string count, string extraDetail)
    {
        string[] floorFeaturesIds;
        string[] counts;
        string[] extraDetails;
        floorFeaturesIds = floorFeaturesId.Split(',');
        counts = count.Split(',');
        extraDetails = extraDetail.Split(',');
        List<extraFeaturesDetails> lst = new List<extraFeaturesDetails>();
        for (int i = 0; i < floorFeaturesIds.Count(); i++)
        {
            lst.AddRange(new List<extraFeaturesDetails>
            {
                new extraFeaturesDetails { floorFeaturesId=floorFeaturesIds[i] ,count=counts[i] , extraDetail = extraDetails[i]
                }

            });
        }
        return lst;

    }
    #endregion
    #region Extra Fees Details
    public static List<extraFees> GetExtraFeesDetails(string count, string timeslabId, string extraFeesDetails, string extraFee)
    {
        string[] counts;
        string[] timeSlabIds;
        string[] extraFees;
        string[] extraFeesDetailss;
        counts = count.Split(',');
        extraFees = extraFee.Split(',');
        extraFeesDetailss = extraFeesDetails.Split(',');
        timeSlabIds = timeslabId.Split(',');
        List<extraFees> lst = new List<extraFees>();
        for (int i = 0; i < counts.Count(); i++)
        {
            lst.AddRange(new List<extraFees>
            {
                new extraFees { count=counts[i] ,extraFee=extraFees[i],timeslabId= timeSlabIds[i],extraFeesDetails = extraFeesDetailss[i] }

            });
        }
        return lst;

    }
    #endregion
    #region User Slot Details
    public static List<userSlotDetails> GetUserSlotDetails(string slotId, string vehicleType)
    {
        string[] slotIds;
        string[] vehicleTypes;
        slotIds = slotId.Split(',');
        vehicleTypes = vehicleType.Split(',');
        List<userSlotDetails> lst = new List<userSlotDetails>();
        for (int i = 0; i < slotIds.Count(); i++)
        {
            lst.AddRange(new List<userSlotDetails>
            {
                new userSlotDetails { slotId=slotIds[i] ,vehicleType=vehicleTypes[i]}

            });
        }
        return lst;

    }
    #endregion
    #endregion

    #region Pass Booking

    #region Get Pass Details  

    public void GetPassDetailsPass(string PassID)
    {
        try
        {
            string usermode = string.Empty;
            if (ViewState["slotType"].ToString() == "VIP")
            {
                usermode = "V";
            }
            else
            {
                usermode = "N";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "passTransaction?ParkingPassTransactionId=" + PassID.Trim() + ""
                      + "&vehicleType=" + ViewState["vehicleType"].ToString() + "&userMode=" + usermode.Trim() + ""
                      + "&branchId=" + Session["branchId"].ToString().Trim() + "";
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
                            divPassdetails.Visible = true;
                            ViewState["BookingType"] = "P";
                            List<GetPassTicket> lst = JsonConvert.DeserializeObject<List<GetPassTicket>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.passType.ToList();
                            DataTable passtype = ConvertToDataTable(lst1);
                            lblpassId.Text = dt.Rows[0]["parkingPassTransId"].ToString();
                            lblTicketPassType.Text = passtype.Rows[0]["passType"].ToString();
                            lblPassMobileNo.Text = dt.Rows[0]["phoneNumber"].ToString();
                            txtmobileNo.Text = dt.Rows[0]["phoneNumber"].ToString();
                            DateTime StartDate = Convert.ToDateTime(dt.Rows[0]["validStartDate"].ToString());
                            // lblPassStartDate.Text = StartDate.ToString("yyyy-MM-dd");
                            lblPassStartDate.Text = StartDate.ToString("dd-MM-yyyy");
                            DateTime EndDate = Convert.ToDateTime(dt.Rows[0]["validEndDate"].ToString());
                            // lblPassEndDate.Text = EndDate.ToString("yyyy-MM-dd");
                            lblPassEndDate.Text = EndDate.ToString("dd-MM-yyyy");
                            lblPassMode.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblPassVehicleType.Text = dt.Rows[0]["vehicleName"].ToString();
                            ViewState["passCategory"] = passtype.Rows[0]["passCategory"].ToString();
                            divPrintCheck.Visible = false;
                            if (dt.Rows[0]["expiry"].ToString() == "Y")
                            {
                                ViewState["BookingType"] = "N";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Pass has been Expired');", true);
                                Clear();
                            }
                            else
                            {
                                divSummaryColor.Style.Add(" background-color", "#eaaef16b");
                                ViewState["BookingType"] = "P";
                                ViewState["passCategory"] = "N";
                                divTimeType.Visible = false;
                                divParkingAmount.Visible = false;
                                divTax.Visible = false;
                                divOtpDetails.Visible = true;
                                divSendOtp.Visible = true;
                                btnBook.Enabled = false;
                            }
                        }
                        else
                        {
                            ViewState["BookingType"] = "N";
                            divTimeType.Visible = true;
                            divParkingAmount.Visible = true;
                            divTax.Visible = true;
                            divPassdetails.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                    }
                    else
                    {
                        if (txtmobileNo.Text.Contains("P"))
                        {
                            txtmobileNo.Text = string.Empty;
                        }
                        ViewState["BookingType"] = "N";
                        divParkingAmount.Visible = true;
                        divTax.Visible = true;
                        divTimeType.Visible = true;
                        divPassdetails.Visible = false;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    TotalAmountCal();
                    if (ViewState["BookingType"].ToString() == "P")
                    {
                        divTax.Visible = false;
                        lblGSTAmount.Text = "0.00";
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
    #region Pass Vehicle Header Details
    public static List<PassvehicleHeaderDetails> GetPassvehicleHeaderDetails(string slotId, string vehicleType, string vehicleNumber,
        string inTime, string vehicleStatus)
    {

        string[] slotIds;
        string[] vehicleTypes;
        string[] vehicleNumbers;
        string[] inTimes;
        string[] vehicleStatuss;
        slotIds = slotId.Split(',');
        vehicleTypes = vehicleType.Split(',');
        vehicleNumbers = vehicleNumber.Split(',');
        inTimes = inTime.Split(',');
        vehicleStatuss = vehicleStatus.Split(',');
        List<PassvehicleHeaderDetails> lst = new List<PassvehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<PassvehicleHeaderDetails>
            {
                new PassvehicleHeaderDetails {slotId=slotIds[i], vehicleType=vehicleTypes[i],vehicleNumber=vehicleNumbers[i],inTime=inTimes[i]
                ,vehicleStatus = vehicleStatuss[i] }

            });
        }

        return lst;

    }
    public static List<PassvehicleHeaderDetails> GetPassvehicleHeaderDetails(string slotId, string vehicleType,
      string inTime, string vehicleStatus)
    {

        string[] slotIds;
        string[] vehicleTypes;
        string[] vehicleNumbers;
        string[] inTimes;
        string[] vehicleStatuss;
        slotIds = slotId.Split(',');
        vehicleTypes = vehicleType.Split(',');

        inTimes = inTime.Split(',');
        vehicleStatuss = vehicleStatus.Split(',');
        List<PassvehicleHeaderDetails> lst = new List<PassvehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<PassvehicleHeaderDetails>
            {
                new PassvehicleHeaderDetails {slotId=slotIds[i], vehicleType=vehicleTypes[i],inTime=inTimes[i]
                ,vehicleStatus = vehicleStatuss[i] }

            });
        }

        return lst;

    }
    public static List<PassvehicleHeaderDetails> GetPassvehicleHeaderDetails(string vehicleType, string vehicleNumber)
    {
        string[] vehicleTypes;
        string[] vehicleNumbers;

        vehicleTypes = vehicleType.Split(',');
        vehicleNumbers = vehicleNumber.Split(',');

        List<PassvehicleHeaderDetails> lst = new List<PassvehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<PassvehicleHeaderDetails>
            {
                new PassvehicleHeaderDetails { vehicleType=vehicleTypes[i],vehicleNumber=vehicleNumbers[i] }

            });
        }

        return lst;

    }
    public static List<PassvehicleHeaderDetails> GetPassvehicleHeaderDetails(string vehicleType)
    {
        string[] vehicleTypes;
        vehicleTypes = vehicleType.Split(',');

        List<PassvehicleHeaderDetails> lst = new List<PassvehicleHeaderDetails>();
        for (int i = 0; i < vehicleTypes.Count(); i++)
        {
            lst.AddRange(new List<PassvehicleHeaderDetails>
            {
                new PassvehicleHeaderDetails { vehicleType=vehicleTypes[i] }

            });
        }

        return lst;

    }
    #endregion
    #region Pass Extra Features Details
    public static List<PassextraFeaturesDetails> GetPassExtraFeaturesDetails(string floorFeaturesId, string count, string extraDetail)
    {
        string[] floorFeaturesIds;
        string[] counts;
        string[] extraDetails;
        floorFeaturesIds = floorFeaturesId.Split(',');
        counts = count.Split(',');
        extraDetails = extraDetail.Split(',');
        List<PassextraFeaturesDetails> lst = new List<PassextraFeaturesDetails>();
        for (int i = 0; i < floorFeaturesIds.Count(); i++)
        {
            lst.AddRange(new List<PassextraFeaturesDetails>
            {
                new PassextraFeaturesDetails { floorFeaturesId=floorFeaturesIds[i] ,count=counts[i] , extraDetail = extraDetails[i]
                }

            });
        }
        return lst;

    }
    #endregion
    #region Pass Extra Fees Details
    public static List<PassextraFeesDetails> GetPassExtraFeesDetails(string count, string timeslabId, string extraFeesDetails, string extraFee)
    {
        string[] counts;
        string[] timeSlabIds;
        string[] extraFees;
        string[] extraFeesDetailss;
        counts = count.Split(',');
        extraFees = extraFee.Split(',');
        extraFeesDetailss = extraFeesDetails.Split(',');
        timeSlabIds = timeslabId.Split(',');
        List<PassextraFeesDetails> lst = new List<PassextraFeesDetails>();
        for (int i = 0; i < counts.Count(); i++)
        {
            lst.AddRange(new List<PassextraFeesDetails>
            {
                new PassextraFeesDetails { count=counts[i] ,extraFee=extraFees[i],timeslabId= timeSlabIds[i],extraFeesDetails = extraFeesDetailss[i] }

            });
        }
        return lst;

    }
    #endregion
    #region Pass User Slot Details
    public static List<PassuserSlotDetails> GetPassUserSlotDetails(string slotId, string vehicleType)
    {
        string[] slotIds;
        string[] vehicleTypes;
        slotIds = slotId.Split(',');
        vehicleTypes = vehicleType.Split(',');
        List<PassuserSlotDetails> lst = new List<PassuserSlotDetails>();
        for (int i = 0; i < slotIds.Count(); i++)
        {
            lst.AddRange(new List<PassuserSlotDetails>
            {
                new PassuserSlotDetails { slotId=slotIds[i] ,vehicleType=vehicleTypes[i]}

            });
        }
        return lst;

    }
    #endregion
    #region Pass Booking
    public void PassBookingFunction()
    {
        try
        {
            string VehicleCheckInStatus = string.Empty;
            string VehicleCheckIntime = string.Empty;
            txtVehicleNo.Text = txtVehicleNo.Text.ToUpper();
            DateTime utcTime = DateTime.Now;
            string times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            string[] time = times.Split('T');
            if (Session["isBookCheckInAvailable"].ToString().Trim() == "Y")
            {
                VehicleCheckIntime = times;
                VehicleCheckInStatus = "I";
            }
            else
            {
                VehicleCheckIntime = "";
                VehicleCheckInStatus = "";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new PassBooking()
                {
                    passTransactionId = lblpassId.Text,
                    blockId = ddlblock.SelectedValue,
                    floorId = ddlfloor.SelectedValue,
                    createdBy = Session["UserId"].ToString(),
                    userSlotDetails = GetPassUserSlotDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString()),

                };
                if (Session["isBookCheckInAvailable"].ToString().Trim() == "Y")
                {
                    if (txtVehicleNo.Text == "")
                    {
                        Insert.vehicleHeaderDetails = GetPassvehicleHeaderDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString(), VehicleCheckIntime, VehicleCheckInStatus);
                    }
                    else
                    {
                        Insert.vehicleHeaderDetails = GetPassvehicleHeaderDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString(), txtVehicleNo.Text, VehicleCheckIntime, VehicleCheckInStatus);
                    }

                }
                else
                {
                    if (txtVehicleNo.Text == "")
                    {
                        Insert.vehicleHeaderDetails = GetPassvehicleHeaderDetails(ViewState["vehicleType"].ToString());

                    }
                    else
                    {
                        Insert.vehicleHeaderDetails = GetPassvehicleHeaderDetails(ViewState["vehicleType"].ToString(), txtVehicleNo.Text);
                    }


                }
                if (lblTotalAmount.Text != "0.00")
                {
                    Insert.paymentStatus = ddlPaymentType.SelectedItem.Text == "Pay At CheckOut" ? "N" : "P";
                    Insert.paymentType = ddlPaymentType.SelectedValue;
                }

                if (lblTotalAmount.Text == "0.00")
                {
                    Insert.paymentStatus = "P";
                    Insert.totalAmount = "0";
                }
                if (dtlExtraFeeSummary.Rows.Count > 0)
                {
                    string feePriceids = string.Empty;
                    string feetureCount = string.Empty;
                    string feeName = string.Empty;
                    string feeAmount = string.Empty;
                    foreach (GridViewRow item in dtlExtraFeeSummary.Rows)
                    {

                        Label lblgvfeeTimeSlabId = item.FindControl("lblgvfeeTimeSlabId") as Label;
                        Label lblfeeCount = item.FindControl("lblfeeCount") as Label;
                        Label lblfeevehicleAccessoriesName = item.FindControl("lblfeevehicleAccessoriesName") as Label;
                        Label lblgvfeeTotalAmount = item.FindControl("lblgvfeeTotalAmount") as Label;

                        feePriceids += lblgvfeeTimeSlabId.Text + ',';
                        feetureCount += lblfeeCount.Text + ',';
                        feeAmount += lblgvfeeTotalAmount.Text + ',';
                        feeName += lblfeevehicleAccessoriesName.Text + ',';
                    }
                    Insert.extraFeesDetails = GetPassExtraFeesDetails(feetureCount.ToString().TrimEnd(','),
                        feePriceids.ToString().TrimEnd(','), feeName.ToString().TrimEnd(','),
                        feeAmount.ToString().TrimEnd(','));
                    Insert.totalAmount = ViewState["AccessoriesTotalAmount"].ToString();

                }

                HttpResponseMessage response = client.PostAsJsonAsync("passBooking", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        string ResponseMsg1 = JObject.Parse(SmartParkingList)["passBookingTransactionId"].ToString();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        // GetPassBookingDetails(ResponseMsg1);
                        if (ddlPaymentType.SelectedItem.Text == "Online" && txtmobileNo.Text != "")
                        {
                            SendPaymentLink(txtmobileNo.Text, ResponseMsg1);
                        }
                        else
                        {
                            if (dtlExtraFeeSummary.Rows.Count > 0)
                            {
                                Response.Redirect("Print.aspx?rt=pW&bi=" + ResponseMsg1.ToString() + "", false);
                            }
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + response.ToString().Trim() + "');", true);
                }
                ChkMobileNo.Enabled = true;
                Clear();
                GetvehicleHeaderDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString(), txtVehicleNo.Text, times, "I").Clear();
                GetUserSlotDetails(ViewState["parkingSlotId"].ToString(), ViewState["vehicleType"].ToString()).Clear();
                BindParkingSlot();
                Divslotrow.Attributes.Add("class", "col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 table-responsive section");
                divSummary.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion   
    #endregion

    #region Extra Features /Accessories 
    #region Bind Accessories
    public void BindAccessories()
    {
        try
        {
            string usermode = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim() + "priceMaster?activeStatus=A";
                sUrl += "&floorId=" + ddlfloor.SelectedValue + "&idType=A"
                 + "&vehicleConfigId=" + ViewState["vehicleType"].ToString() + "";
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
                            lblGvAccessoriesNo.Visible = false;
                            dtlextraFee.Visible = true;
                            dtlextraFee.DataSource = dt;
                            dtlextraFee.DataBind();
                        }
                        else
                        {
                            lblGvAccessoriesNo.Text = ResponseMsg.Trim();
                            lblGvAccessoriesNo.Visible = true;
                            dtlextraFee.Visible = false;
                            dtlextraFee.DataSource = null;
                            dtlextraFee.DataBind();
                        }
                        DivSlotOtherStab.Visible = true;
                    }
                    else
                    {
                        DivSlotOtherStab.Visible = false;
                        lblGvAccessoriesNo.Text = ResponseMsg.Trim();
                        lblGvAccessoriesNo.Visible = false;
                        dtlextraFee.Visible = false;
                        dtlextraFee.DataSource = null;
                        dtlextraFee.DataBind();
                        // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.Trim() + "');", true);
                    }

                }
                else
                {
                    DivSlotOtherStab.Visible = false;
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

    protected void dtlextraFee_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (lblLaneSlot.Text == "")
            {
                RfvVehicleNo.Enabled = false;
                revVehicleNo.Enabled = false;
                divSummary.Visible = true;
                divTimeType.Visible = false;
                // txtmobileNo.AutoPostBack = false;
                btnBook.Enabled = true;
            }
            else
            {
                //txtmobileNo.AutoPostBack = true;

            }
            LinkButton lblgvFeeName = (LinkButton)e.Item.FindControl("lblgvFeeName");
            Label lblgvFeetax = (Label)e.Item.FindControl("lblgvFeetax");
            Label lblgvFeeAmount = (Label)e.Item.FindControl("lblgvFeeAmount");
            LinkButton lblgvFeeTotalAmount = (LinkButton)e.Item.FindControl("lblgvFeeTotalAmount");
            Label lblGvTimeSlabId = (Label)e.Item.FindControl("lblGvTimeSlabId");
            int Count = 1;
            DataTable dt = new DataTable();
            int Uniqueid = 1;
            if (ViewState["Row"] != null)
            {
                dt = (DataTable)ViewState["Row"];
                DataRow dr = null;

                string Values = lblGvTimeSlabId.Text;

                DataRow[] fndUniqueId = dt.Select("timeslabId = '" + Values.Trim() + "'");

                if (dt.Rows.Count > 0)
                {
                    Uniqueid = dt.Rows.Count + 1;
                    dr = dt.NewRow();
                    dr["UniqueID"] = Uniqueid;
                    dr["timeslabId"] = lblGvTimeSlabId.Text;
                    dr["vehicleAccessoriesName"] = lblgvFeeName.Text;
                    dr["amount"] = lblgvFeeAmount.Text;
                    dr["tax"] = lblgvFeetax.Text;
                    dr["totalAmount"] = lblgvFeeTotalAmount.Text;
                    dr["Count"] = Count;
                    dt.Rows.Add(dr);
                    ViewState["Row"] = dt;
                }
            }
            else
            {
                dt.Columns.Add(new DataColumn("UniqueID", typeof(string)));
                dt.Columns.Add(new DataColumn("timeslabId", typeof(string)));
                dt.Columns.Add(new DataColumn("vehicleAccessoriesName", typeof(string)));
                dt.Columns.Add(new DataColumn("amount", typeof(decimal)));
                dt.Columns.Add(new DataColumn("tax", typeof(decimal)));
                dt.Columns.Add(new DataColumn("totalAmount", typeof(decimal)));
                dt.Columns.Add(new DataColumn("Count", typeof(Int32)));

                DataRow dr1 = dt.NewRow();
                dr1 = dt.NewRow();
                dr1["UniqueID"] = Uniqueid;
                dr1["timeslabId"] = lblGvTimeSlabId.Text;
                dr1["vehicleAccessoriesName"] = lblgvFeeName.Text;
                dr1["amount"] = lblgvFeeAmount.Text;
                dr1["tax"] = lblgvFeetax.Text;
                dr1["totalAmount"] = lblgvFeeTotalAmount.Text;
                dr1["Count"] = Count;

                dt.Rows.Add(dr1);

                ViewState["Row"] = dt;
            }


            DataTable dts = dt.Clone();

            var CartTable = (from row in dt.AsEnumerable()
                             group row by new
                             {
                                 timeslabId = row.Field<string>("timeslabId"),
                                 vehicleAccessoriesName = row.Field<string>("vehicleAccessoriesName"),
                             } into t1
                             select new
                             {
                                 timeslabId = t1.Key.timeslabId,
                                 vehicleAccessoriesName = t1.Key.vehicleAccessoriesName,
                                 amount = t1.Sum(a => a.Field<decimal>("amount")),
                                 tax = t1.Sum(a => a.Field<decimal>("tax")),
                                 totalAmount = t1.Sum(a => a.Field<decimal>("totalAmount")),
                                 Count = t1.Sum(a => a.Field<Int32>("Count")),

                             })
                 .Select(g =>
                 {
                     var h = dts.NewRow();
                     h["timeslabId"] = g.timeslabId;
                     h["vehicleAccessoriesName"] = g.vehicleAccessoriesName;
                     h["amount"] = g.amount;
                     h["tax"] = g.tax;
                     h["totalAmount"] = g.totalAmount;
                     h["Count"] = g.Count;


                     return h;
                 }).CopyToDataTable();


            if (CartTable.Rows.Count > 0)
            {
                ViewState["CartRow"] = CartTable;
                DataTable dtss = (DataTable)ViewState["CartRow"];

                if (dtss.Rows.Count > 0)
                {
                    dtlExtraFeeSummary.DataSource = dtss;
                    dtlExtraFeeSummary.DataBind();
                    divExtrafeeSummary1.Visible = true;
                    divExtraFeeAmount.Visible = true;
                    extrafeeandfeatursContainer.Visible = true;
                    decimal dServiceTotalAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("totalAmount")));
                    decimal dServiceAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("amount")));
                    decimal dTax = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("tax")));
                    ViewState["AccessoriesTotalAmount"] = Convert.ToDecimal(dServiceTotalAmount).ToString("0.00");
                    ViewState["AccessoriesAmount"] = Convert.ToDecimal(dServiceAmount).ToString("0.00");
                    ViewState["AccessoriesTax"] = Convert.ToDecimal(dTax).ToString("0.00");
                    TotalAmountCal();
                    ViewState["TotalTaxFee"] = (Convert.ToDecimal(dTax)).ToString();
                    lblAccessoriesTotalAmount.Text = ViewState["AccessoriesAmount"].ToString();
                }
                else
                {
                    dtlExtraFeeSummary.DataSource = dtss;
                    dtlExtraFeeSummary.DataBind();
                }
                //for (int i = 0; i< dtss.Rows.Count;i++)
                //{

                //    HtmlControl divcarname = dtlextraFee.Items[i].FindControl("DivSelected") as HtmlControl;
                //    divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";
                //}

            }
            else
            {
                if (lblLaneSlot.Text == "")
                {
                    Clear();
                }
                TotalAmountCal();
                ViewState["CartRow"] = null;
                ViewState["Row"] = null;
                ViewState["AccessoriesTotalAmount"] = "0";
                ViewState["AccessoriesAmount"] = "0";
                ViewState["AccessoriesTax"] = "0";
                lblAccessoriesTotalAmount.Text = "0.00";
                dtlExtraFeeSummary.DataSource = null;
                dtlExtraFeeSummary.DataBind();
                divExtrafeeSummary1.Visible = false;
                divExtraFeeAmount.Visible = false;
                extrafeeandfeatursContainer.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }

    protected void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblfeevehicleAccessoriesName = (Label)gvrow.FindControl("lblfeevehicleAccessoriesName");
            Label lblgvfeeTotalAmount = (Label)gvrow.FindControl("lblgvfeeTotalAmount");
            Label lblfeeCount = (Label)gvrow.FindControl("lblfeeCount");
            Label lblgvfeeTimeSlabId = (Label)gvrow.FindControl("lblgvfeeTimeSlabId");
            DataTable dt = (DataTable)ViewState["Row"];
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dt.Rows[i];
                if (drO["timeslabId"].ToString().Trim() == lblgvfeeTimeSlabId.Text)
                {
                    drO.Delete();

                }
            }
            dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {


                DataTable dts = dt.Clone();

                var CartTable = (from row in dt.AsEnumerable()
                                 group row by new
                                 {
                                     timeslabId = row.Field<string>("timeslabId"),
                                     vehicleAccessoriesName = row.Field<string>("vehicleAccessoriesName"),

                                 } into t1
                                 select new
                                 {
                                     timeslabId = t1.Key.timeslabId,
                                     vehicleAccessoriesName = t1.Key.vehicleAccessoriesName,
                                     amount = t1.Sum(a => a.Field<decimal>("amount")),
                                     tax = t1.Sum(a => a.Field<decimal>("tax")),
                                     totalAmount = t1.Sum(a => a.Field<decimal>("totalAmount")),
                                     Count = t1.Sum(a => a.Field<Int32>("Count")),

                                 })
                     .Select(g =>
                     {
                         var h = dts.NewRow();
                         h["timeslabId"] = g.timeslabId;
                         h["vehicleAccessoriesName"] = g.vehicleAccessoriesName;
                         h["amount"] = g.amount;
                         h["tax"] = g.tax;
                         h["totalAmount"] = g.totalAmount;
                         h["Count"] = g.Count;


                         return h;
                     }).CopyToDataTable();

                ViewState["CartRow"] = CartTable;
                DataTable dtss = (DataTable)ViewState["CartRow"];

                if (dtss.Rows.Count > 0)
                {
                    dtlExtraFeeSummary.DataSource = dtss;
                    dtlExtraFeeSummary.DataBind();
                    divExtrafeeSummary1.Visible = true;
                    divExtraFeeAmount.Visible = true;
                    extrafeeandfeatursContainer.Visible = true;
                    decimal dServiceTotalAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("totalAmount")));
                    decimal dServiceAmount = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("amount")));
                    decimal dTax = dtss.AsEnumerable().Sum(row => Convert.ToDecimal(row.Field<decimal>("tax")));
                    ViewState["AccessoriesTotalAmount"] = Convert.ToDecimal(dServiceTotalAmount).ToString("0.00");
                    ViewState["AccessoriesAmount"] = Convert.ToDecimal(dServiceAmount).ToString("0.00");
                    ViewState["AccessoriesTax"] = Convert.ToDecimal(dTax).ToString("0.00");

                    TotalAmountCal();
                    ViewState["TotalTaxFee"] = (Convert.ToDecimal(dTax)).ToString();
                    lblAccessoriesTotalAmount.Text = ViewState["AccessoriesAmount"].ToString();

                }
            }
            else
            {
                if (lblLaneSlot.Text == "")
                {
                    Clear();
                }
                lblAccessoriesTotalAmount.Text = "0.00";
                dtlExtraFeeSummary.DataSource = null;
                dtlExtraFeeSummary.DataBind();
                divExtrafeeSummary1.Visible = false;
                divExtraFeeAmount.Visible = false;
                extrafeeandfeatursContainer.Visible = false;
                ViewState["AccessoriesTotalAmount"] = "0";
                ViewState["AccessoriesAmount"] = "0";
                ViewState["AccessoriesTax"] = "0";
                TotalAmountCal();

                ViewState["CartRow"] = null;
                ViewState["Row"] = null;

            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {
            Pathdesigns();
        }
    }
    #endregion

    #region Slot Click CheckIn Check out
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
                            divRePrint.Visible = false;
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
    public void BindSlotClickDetails(string Id)
    {
        lblBookingAmountPass.Text = string.Empty;
        divNormalHeader.Visible = false;
        divPassHeader.Visible = true;
        divReminingExtras.Visible = false;
        divRePrint.Visible = true;
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "getDataBasedOnSlotId?slotId=" + Id.Trim() + "";
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
                            if (dt.Columns.Contains("passTransactionId"))
                            {
                                List<PassInoutdetails> PassInOut = JsonConvert.DeserializeObject<List<PassInoutdetails>>(ResponseMsg);
                                var PassInoutItems = PassInOut.ElementAt(0);
                                var PassTable = PassInoutItems.vehicleDetails.ToList();
                                var userSlot = PassInoutItems.userSlotDetails.ToList();
                                DataTable userSlotTable = ConvertToDataTable(userSlot);
                                DataTable PassInoutTable = ConvertToDataTable(PassTable);

                                string vehicleStatus = PassInoutTable.Rows[0]["vehicleStatus"].ToString();
                                string vehicleNumber = PassInoutTable.Rows[0]["vehicleNumber"].ToString();
                                string vehicleHeaderId = PassInoutTable.Rows[0]["vehicleHeaderId"].ToString();
                                string BookingId = PassInoutTable.Rows[0]["bookingPassId"].ToString();
                                string slotd = userSlotTable.Rows[0]["slotId"].ToString();
                                string passTransactionId = dt.Rows[0]["passTransactionId"].ToString();
                                //string inTime = PassInoutTable.Rows[0]["inTime"].ToString();
                                string paymentStatus = dt.Rows[0]["paymentStatus"].ToString().Trim();
                                decimal totalAmount = Convert.ToDecimal(dt.Rows[0]["totalAmount"].ToString().Trim());
                                lblLaneslotsNo.Text = userSlotTable.Rows[0]["laneNumber"].ToString() + "-" + userSlotTable.Rows[0]["slotNumber"].ToString();
                                if (totalAmount == 0)
                                {
                                    divBookingAmountPAss.Visible = false;
                                }
                                else
                                {
                                    divBookingAmountPAss.Visible = true;
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
                                lblBookingAmountPass.Text = totalAmount.ToString("0.00");
                                // decimal Extrafeatures = Convert.ToDecimal(dt.Rows[0]["extraFeaturesAmount"].ToString().Trim());
                                decimal Extrafees = Convert.ToDecimal(dt.Rows[0]["extraFeesAmount"].ToString().Trim());
                                decimal Total = Extrafees;
                                var lst3 = PassInoutItems.extraFeesDetails.ToList();

                                DataTable GetextraFeesDetailsPass = ConvertToDataTable(lst3);
                                if (GetextraFeesDetailsPass.Rows.Count > 0)
                                {
                                    divReminingExtras.Visible = true;
                                    dtlExtrafees.DataSource = GetextraFeesDetailsPass;
                                    dtlExtrafees.DataBind();
                                }
                                else
                                {
                                    dtlExtrafees.DataSource = GetextraFeesDetailsPass;
                                    dtlExtrafees.DataBind();
                                }

                                divNormalHeader.Visible = false;
                                divPassHeader.Visible = true;
                                GetPassDetailsInoutDetails(passTransactionId);
                                lblTopayAmt.Text = Total.ToString("0.00");
                                lblBlockIn.Text = ddlblock.SelectedItem.Text;
                                lblFloorIn.Text = ddlfloor.SelectedItem.Text;
                                lblBookingPassId.Text = BookingId.Trim();
                                lblVehicleIn.Text = vehicleNumber.Trim();
                                if (PassInoutTable.Rows[0]["vehiclePlaceHolderImageUrl"].ToString().Trim() != "")
                                {
                                    ImgPasVhcl.Visible = true;
                                    ImgPasVhcl.ImageUrl = PassInoutTable.Rows[0]["vehiclePlaceHolderImageUrl"].ToString().Trim();
                                }

                                lblPaymentTypePass.Text = dt.Rows[0]["paymentStatus"].ToString().Trim() == "P" ? "Paid" : "Unpaid";
                                GetPassDetailsInoutDetails(passTransactionId);
                                ViewState["vehicleStatus"] = vehicleStatus.Trim();
                                ViewState["BookingId"] = BookingId.Trim();
                                ViewState["vehicleNumber"] = vehicleNumber.Trim();
                                ViewState["vehicleHeaderId"] = vehicleHeaderId.Trim();
                                ViewState["slotd"] = slotd.Trim();
                                ViewState["paidAmount"] = Total.ToString().Trim();

                                if (vehicleStatus.Trim() == "")
                                {
                                    CheckInorCheckout.InnerText = "Check In";
                                    modalSearch.Visible = true;
                                    divpass.Visible = true;
                                    divextend.Visible = false;
                                    divRemaining.Visible = false;
                                    divpassDetailss.Visible = true;
                                    divPayment.Visible = false;
                                    btnCheckInPopup.Text = "Check In";
                                    ViewState["Flag"] = "0";
                                    divModalColor.Attributes.Add("style", "background:#bcffce; ");
                                    divModalColor.Style.Add("border-radius", "10px");
                                }
                                else if (vehicleStatus.Trim() == "I")
                                {
                                    if (GetextraFeesDetailsPass.Rows.Count > 0)
                                    {
                                        if (paymentStatus.Trim() == "P")
                                        {
                                            divpassstatus.Visible = true;
                                            CheckInorCheckout.InnerText = "Check Out";
                                            modalSearch.Visible = true;
                                            divpass.Visible = true;
                                            divextend.Visible = false;
                                            divRemaining.Visible = false;
                                            divPayment.Visible = false;
                                            divpassDetailss.Visible = true;
                                            btnCheckInPopup.Text = "Check Out";
                                            ViewState["Flag"] = "1";
                                        }
                                        else
                                        {
                                            divpassstatus.Visible = true;
                                            CheckInorCheckout.InnerText = "Check Out";
                                            modalSearch.Visible = true;
                                            divpass.Visible = true;
                                            divextend.Visible = false;
                                            divRemaining.Visible = false;
                                            divpass.Visible = true;
                                            divPayment.Visible = true;
                                            divpassDetailss.Visible = true;
                                            btnCheckInPopup.Text = "Pay And CheckOut";
                                            ViewState["Flag"] = "4";
                                        }
                                    }
                                    else
                                    {
                                        divpassstatus.Visible = false;
                                        CheckInorCheckout.InnerText = "Check Out";
                                        modalSearch.Visible = true;
                                        divpass.Visible = true;
                                        divextend.Visible = false;
                                        divRemaining.Visible = false;
                                        divPayment.Visible = false;
                                        divpassDetailss.Visible = true;
                                        btnCheckInPopup.Text = "Check Out";
                                        ViewState["Flag"] = "1";
                                    }
                                    divModalColor.Attributes.Add("style", "background:#ffbcbc; ");
                                    divModalColor.Style.Add("border-radius", "10px");

                                }
                                else
                                {
                                    txtBookingId.Text = string.Empty;
                                    Clear();
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Already Checked Out');", true);
                                }
                            }
                            else
                            {
                                BindBookingIdDetails(dt.Rows[0]["bookingId"].ToString());
                                BindDdlPayment();
                            }

                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtBookingId.Text = string.Empty;
                        Clear();
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                    txtBookingId.Text = string.Empty;
                    Clear();
                }


            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #region  PassInoutClass
    public class PassInoutdetails
    {
        public string extraFeesAmount { get; set; }
        public string extraFeaturesAmount { get; set; }
        public string passTransactionId { get; set; }
        public string extendAmount { get; set; }
        public string extendTax { get; set; }
        public string extendDayHour { get; set; }
        public string taxAmount { get; set; }
        public string amount { get; set; }
        public string totalAmount { get; set; }
        public List<GetuserSlotDetails> userSlotDetails { get; set; }
        public List<vehicleDetailsPass> vehicleDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<GetextraFees> extraFeesDetails { get; set; }

    }
    public class vehicleDetailsPass
    {
        public string vehicleHeaderId { get; set; }
        public string bookingPassId { get; set; }
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string slotNumber { get; set; }
        public string vehiclePlaceHolderImageUrl { get; set; }
    }
    #endregion
    #endregion

    #region RePrint Click
    protected void lnkRePrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("Print.aspx?rt=CO&bi=" + ViewState["BookingId"].ToString() + "", false);
    }
    #endregion

    #region Scan BookngId,vehicleNo,MobileNo 
    protected void txtBookingId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtBookingId.Text != "")
            {
                string BookingPassId = string.Empty;
                string BorP = string.Empty;
                string[] BookingPassIds;

                txtBookingId.Text = txtBookingId.Text.ToUpper();
                BookingPassId = txtBookingId.Text;
                BookingPassIds = BookingPassId.Split(';');
                BorP = BookingPassIds[0].Substring(0, 1);
                if (BookingPassIds.Length > 0)
                {
                    if (BorP.Trim() == "B")
                    {
                        BindBookingIdDetails(BookingPassIds[0].Trim());
                    }
                    else if (BorP.Trim() == "P")
                    {
                        ScanBookingPassId(BookingPassIds[0].Trim());
                    }
                    else
                    {
                        GetDataBasedOnVehicleNumberPhoneDetails(BookingPassIds[0].Trim());
                    }
                    BindDdlPayment();
                }
                else
                {
                    txtBookingId.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    public void GetDataBasedOnVehicleNumberPhoneDetails(string BookingIds)
    {
        lblBookingAmountPass.Text = string.Empty;
        divNormalHeader.Visible = false;
        divPassHeader.Visible = true;
        divReminingExtras.Visible = false;
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
                                List<PassInoutdetails> PassInOut = JsonConvert.DeserializeObject<List<PassInoutdetails>>(ResponseMsg);
                                var PassInoutItems = PassInOut.ElementAt(0);
                                var PassTable = PassInoutItems.vehicleDetails.ToList();
                                var userSlot = PassInoutItems.userSlotDetails.ToList();
                                DataTable userSlotTable = ConvertToDataTable(userSlot);
                                DataTable PassInoutTable = ConvertToDataTable(PassTable);
                                string vehicleStatus = PassInoutTable.Rows[0]["vehicleStatus"].ToString();
                                string vehicleNumber = PassInoutTable.Rows[0]["vehicleNumber"].ToString();
                                string vehicleHeaderId = PassInoutTable.Rows[0]["vehicleHeaderId"].ToString();
                                string BookingId = PassInoutTable.Rows[0]["bookingPassId"].ToString();
                                string slotd = PassInoutTable.Rows[0]["slotId"].ToString();
                                string passTransactionId = dt.Rows[0]["passTransactionId"].ToString();
                                string paymentStatus = dt.Rows[0]["paymentStatus"].ToString().Trim();
                                lblLaneslotsNo.Text = userSlotTable.Rows[0]["laneNumber"].ToString() + "-" + userSlotTable.Rows[0]["slotNumber"].ToString();
                                decimal totalAmount = Convert.ToDecimal(dt.Rows[0]["totalAmount"].ToString().Trim());
                                lblBookingAmountPass.Text = totalAmount.ToString("0.00");
                                if (totalAmount == 0)
                                {
                                    divBookingAmountPAss.Visible = false;
                                }
                                else
                                {
                                    divBookingAmountPAss.Visible = true;
                                }
                                if (vehicleNumber.Trim() == "")
                                {
                                    divPassBookingVehicleNo.Visible = false;
                                    divRemainingVehicleNo.Visible = false;
                                    divExtendVehicleNo.Visible = false;
                                    ImgPasVhcl.Visible = false;
                                }
                                else
                                {
                                    divPassBookingVehicleNo.Visible = true;
                                    divRemainingVehicleNo.Visible = true;
                                    divExtendVehicleNo.Visible = true;
                                    ImgPasVhcl.Visible = true;

                                }
                                // decimal Extrafeatures = Convert.ToDecimal(dt.Rows[0]["extraFeaturesAmount"].ToString().Trim());
                                decimal Extrafees = Convert.ToDecimal(dt.Rows[0]["extraFeesAmount"].ToString().Trim());
                                decimal Total = Extrafees;
                                var lst3 = PassInoutItems.extraFeesDetails.ToList();

                                DataTable GetextraFeesDetailsPass = ConvertToDataTable(lst3);
                                if (GetextraFeesDetailsPass.Rows.Count > 0)
                                {
                                    divReminingExtras.Visible = true;
                                    dtlExtrafees.DataSource = GetextraFeesDetailsPass;
                                    dtlExtrafees.DataBind();
                                }
                                else
                                {
                                    divReminingExtras.Visible = false;
                                    dtlExtrafees.DataSource = GetextraFeesDetailsPass;
                                    dtlExtrafees.DataBind();

                                }
                                divNormalHeader.Visible = false;
                                divPassHeader.Visible = true;
                                GetPassDetailsInoutDetails(passTransactionId);
                                lblTopayAmt.Text = Total.ToString("0.00");
                                lblBlockIn.Text = ddlblock.SelectedItem.Text;
                                lblFloorIn.Text = ddlfloor.SelectedItem.Text;
                                lblBookingPassId.Text = BookingId.Trim();
                                lblVehicleIn.Text = vehicleNumber.Trim();
                                ImgPasVhcl.ImageUrl = PassInoutTable.Rows[0]["vehiclePlaceHolderImageUrl"].ToString().Trim();
                                lblPaymentTypePass.Text = dt.Rows[0]["paymentStatus"].ToString().Trim() == "P" ? "Paid" : "Unpaid";
                                GetPassDetailsInoutDetails(passTransactionId);
                                ViewState["vehicleStatus"] = vehicleStatus.Trim();
                                ViewState["BookingId"] = BookingId.Trim();
                                ViewState["vehicleNumber"] = vehicleNumber.Trim();
                                ViewState["vehicleHeaderId"] = vehicleHeaderId.Trim();
                                ViewState["slotd"] = slotd.Trim();
                                ViewState["paidAmount"] = Total.ToString().Trim();
                                if (vehicleStatus.Trim() == "")
                                {
                                    CheckInorCheckout.InnerText = "Check In";
                                    modalSearch.Visible = true;
                                    divpass.Visible = true;
                                    divextend.Visible = false;
                                    divRemaining.Visible = false;
                                    divpassDetailss.Visible = true;
                                    divPayment.Visible = false;
                                    btnCheckInPopup.Text = "Check In";
                                    ViewState["Flag"] = "0";
                                    divModalColor.Attributes.Add("style", "background:#bcffce; ");
                                    divModalColor.Style.Add("border-radius", "10px");
                                }
                                else if (vehicleStatus.Trim() == "I")
                                {
                                    if (GetextraFeesDetailsPass.Rows.Count > 0)
                                    {
                                        if (paymentStatus.Trim() == "P")
                                        {
                                            divpassstatus.Visible = true;
                                            CheckInorCheckout.InnerText = "Check Out";
                                            modalSearch.Visible = true;
                                            divpass.Visible = true;
                                            divextend.Visible = false;
                                            divRemaining.Visible = false;
                                            divPayment.Visible = false;
                                            divpassDetailss.Visible = true;
                                            btnCheckInPopup.Text = "Check Out";
                                            ViewState["Flag"] = "1";
                                        }
                                        else
                                        {
                                            divpassstatus.Visible = true;
                                            CheckInorCheckout.InnerText = "Check Out";
                                            modalSearch.Visible = true;
                                            divpass.Visible = true;
                                            divextend.Visible = false;
                                            divRemaining.Visible = false;
                                            divpass.Visible = true;
                                            divPayment.Visible = true;
                                            divpassDetailss.Visible = true;
                                            btnCheckInPopup.Text = "Pay And CheckOut";
                                            ViewState["Flag"] = "4";
                                        }
                                    }
                                    else
                                    {
                                        divpassstatus.Visible = false;
                                        CheckInorCheckout.InnerText = "Check Out";
                                        modalSearch.Visible = true;
                                        divpass.Visible = true;
                                        divextend.Visible = false;
                                        divRemaining.Visible = false;
                                        divPayment.Visible = false;
                                        divpassDetailss.Visible = true;
                                        btnCheckInPopup.Text = "Check Out";
                                        ViewState["Flag"] = "1";
                                    }
                                    divModalColor.Attributes.Add("style", "background:#ffbcbc; ");
                                    divModalColor.Style.Add("border-radius", "10px");
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Already Checked Out');", true);
                                    txtBookingId.Text = string.Empty;
                                    Clear();
                                }
                            }
                            else
                            {
                                BindBookingIdDetails(dt.Rows[0]["bookingId"].ToString());
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            Clear();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtBookingId.Text = string.Empty;
                        Clear();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {
            Pathdesigns();
        }
    }
    public void ScanBookingPassId(string Ids)
    {
        try
        {
            divNormalHeader.Visible = false;
            divPassHeader.Visible = true;
            divReminingExtras.Visible = false;
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
                        DataTable dtPassIN = ConvertToDataTable(extra);
                        var userSlot = firstItemextra.userSlotDetails.ToList();
                        DataTable userSlotTable = ConvertToDataTable(userSlot);

                        if (GetextraFeesDetailsPass.Rows.Count > 0)
                        {
                            divReminingExtras.Visible = true;
                            dtlExtrafees.DataSource = GetextraFeesDetailsPass;
                            dtlExtrafees.DataBind();
                        }
                        else
                        {
                            divReminingExtras.Visible = false;
                            dtlExtrafees.DataSource = GetextraFeesDetailsPass;
                            dtlExtrafees.DataBind();
                        }
                        string paymentStatus = dtPassIN.Rows[0]["paymentStatus"].ToString();
                        string vehicleStatus = GetvehicleDetails.Rows[0]["vehicleStatus"].ToString();
                        string vehicleNumber = GetvehicleDetails.Rows[0]["vehicleNumber"].ToString();
                        string vehicleHeaderId = GetvehicleDetails.Rows[0]["vehicleHeaderId"].ToString();
                        string BookingId = GetvehicleDetails.Rows[0]["bookingPassId"].ToString();
                        string passTransactionId = dtPassIN.Rows[0]["passTransactionId"].ToString();
                        string SlotId = userSlotTable.Rows[0]["slotId"].ToString().Trim();
                        lblLaneslotsNo.Text = userSlotTable.Rows[0]["laneNumber"].ToString() + "-" + userSlotTable.Rows[0]["slotNumber"].ToString();
                        // decimal Extrafeatures = Convert.ToDecimal(dtPassIN.Rows[0]["extraFeaturesAmount"].ToString().Trim());
                        decimal totalAmount = Convert.ToDecimal(dtPassIN.Rows[0]["totalAmount"].ToString().Trim());
                        lblBookingAmountPass.Text = totalAmount.ToString("0.00");
                        if (totalAmount == 0)
                        {
                            divBookingAmountPAss.Visible = false;
                        }
                        else
                        {
                            divBookingAmountPAss.Visible = true;
                        }
                        if (vehicleNumber.Trim() == "")
                        {
                            divPassBookingVehicleNo.Visible = false;
                            divRemainingVehicleNo.Visible = false;
                            divExtendVehicleNo.Visible = false;
                            ImgPasVhcl.Visible = false;
                        }
                        else
                        {
                            divPassBookingVehicleNo.Visible = true;
                            divRemainingVehicleNo.Visible = true;
                            divExtendVehicleNo.Visible = true;
                            ImgPasVhcl.Visible = true;
                        }
                        decimal Extrafee = Convert.ToDecimal(dtPassIN.Rows[0]["extraFeesAmount"].ToString().Trim());
                        decimal Total = Extrafee;
                        string times;
                        if (passTransactionId.Trim() != "")
                        {
                            lblBookingPassId.Text = BookingId.Trim();
                        }
                        else
                        {
                            lblBookingPassId.Text = BookingId.Trim();
                        }

                        lblBlockIn.Text = ddlblock.SelectedItem.Text;
                        lblFloorIn.Text = ddlfloor.SelectedItem.Text;
                        lblVehicleIn.Text = vehicleNumber.Trim();
                        ImgPasVhcl.ImageUrl = GetvehicleDetails.Rows[0]["vehiclePlaceHolderImageUrl"].ToString().Trim();
                        GetPassDetailsInoutDetails(passTransactionId);
                        lblTopayAmt.Text = Total.ToString("0.00");
                        lblPaymentTypePass.Text = paymentStatus == "P" ? "Paid" : "Unpaid";
                        string[] date2;
                        DateTime utcTime = DateTime.Now;
                        times = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                        date2 = times.Split(' ');
                        ViewState["vehicleStatus"] = vehicleStatus.Trim();
                        ViewState["BookingId"] = BookingId.Trim();
                        ViewState["vehicleNumber"] = vehicleNumber.Trim();
                        ViewState["vehicleHeaderId"] = vehicleHeaderId.Trim();
                        ViewState["slotd"] = SlotId.Trim();
                        ViewState["paidAmount"] = Total.ToString().Trim();
                        if (vehicleStatus.Trim() == "")
                        {
                            CheckInorCheckout.InnerText = "Check In";
                            modalSearch.Visible = true;
                            divpass.Visible = true;
                            divextend.Visible = false;
                            divRemaining.Visible = false;
                            divpassDetailss.Visible = true;
                            divPayment.Visible = false;
                            btnCheckInPopup.Text = "Check In";
                            ViewState["Flag"] = "0";
                            divModalColor.Attributes.Add("style", "background:#bcffce;");
                            divModalColor.Style.Add("border-radius", "10px");

                        }
                        else if (vehicleStatus.Trim() == "I")
                        {
                            if (GetextraFeesDetailsPass.Rows.Count > 0)
                            {
                                if (paymentStatus.Trim() == "P")
                                {
                                    divpassstatus.Visible = true;
                                    //CheckInorCheckout.InnerText = "Check Out";
                                    modalSearch.Visible = true;
                                    divpass.Visible = true;
                                    divextend.Visible = false;
                                    divRemaining.Visible = false;
                                    divPayment.Visible = false;
                                    divpassDetailss.Visible = true;
                                    btnCheckInPopup.Text = "Check Out";
                                    ViewState["Flag"] = "1";
                                }
                                else
                                {
                                    divpassstatus.Visible = true;
                                    //CheckInorCheckout.InnerText = "Check Out";
                                    modalSearch.Visible = true;
                                    divpass.Visible = true;
                                    divextend.Visible = false;
                                    divRemaining.Visible = false;
                                    divpass.Visible = true;
                                    divPayment.Visible = true;
                                    divpassDetailss.Visible = true;
                                    btnCheckInPopup.Text = "Pay And CheckOut";
                                    ViewState["Flag"] = "4";
                                }
                            }
                            else
                            {
                                divpassstatus.Visible = false;
                                //CheckInorCheckout.InnerText = "Check Out";
                                modalSearch.Visible = true;
                                divpass.Visible = true;
                                divextend.Visible = false;
                                divRemaining.Visible = false;
                                divPayment.Visible = false;
                                divpassDetailss.Visible = true;
                                btnCheckInPopup.Text = "Check Out";
                                ViewState["Flag"] = "1";
                            }
                            CheckInorCheckout.InnerText = "Check Out";
                            divModalColor.Attributes.Add("style", "background:#ffbcbc;");
                            divModalColor.Style.Add("border-radius", "10px");

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Already Checked Out');", true);
                            txtBookingId.Text = string.Empty;
                            Clear();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtBookingId.Text = string.Empty;
                        Clear();
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                    txtBookingId.Text = string.Empty;
                    Clear();

                }
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {
            Pathdesigns();
        }
    }
    public void BindBookingIdDetails(string BookingIds)
    {
        divReminingExtras.Visible = false;
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
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            List<GetBookings> extra = JsonConvert.DeserializeObject<List<GetBookings>>(ResponseMsg);
                            var firstItemextra = extra.ElementAt(0);
                            var lst1 = firstItemextra.vehicleDetails.ToList();
                            var lst3 = firstItemextra.extraFeesDetails.ToList();
                            var userSlot = firstItemextra.userSlotDetails.ToList();
                            DataTable userSlotTable = ConvertToDataTable(userSlot);

                            DataTable GetextraFeesDetails = ConvertToDataTable(lst3);
                            if (GetextraFeesDetails.Rows.Count > 0)
                            {
                                divReminingExtras.Visible = true;
                                divExtraFeeFeatures.Visible = true;
                                Extrafee.DataSource = GetextraFeesDetails;
                                Extrafee.DataBind();
                                dtlextrafeess.DataSource = GetextraFeesDetails;
                                dtlextrafeess.DataBind();
                                divReminingExtras.Visible = true;
                                dtlExtrafees.DataSource = GetextraFeesDetails;
                                dtlExtrafees.DataBind();
                            }
                            else
                            {
                                divReminingExtras.Visible = false;
                                divExtraFeeFeatures.Visible = false;
                                Extrafee.DataSource = GetextraFeesDetails;
                                Extrafee.DataBind();
                                dtlextrafeess.DataSource = GetextraFeesDetails;
                                dtlextrafeess.DataBind();
                                divReminingExtras.Visible = false;
                                dtlExtrafees.DataSource = GetextraFeesDetails;
                                dtlExtrafees.DataBind();
                            }

                            DataTable GetvehicleDetails = ConvertToDataTable(lst1);
                            List<VehicleInoutClass> lst = JsonConvert.DeserializeObject<List<VehicleInoutClass>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);

                            string[] dayhourlbl;

                            string vehicleStatus = GetvehicleDetails.Rows[0]["vehicleStatus"].ToString();
                            string vehicleNumber = GetvehicleDetails.Rows[0]["vehicleNumber"].ToString();
                            string vehicleHeaderId = GetvehicleDetails.Rows[0]["vehicleHeaderId"].ToString();

                            if (GetvehicleDetails.Rows[0]["inTime"].ToString() != "")
                            {
                                divInTime.Visible = true;
                                DateTime CurTime = DateTime.Now;
                                DateTime InTime = Convert.ToDateTime(GetvehicleDetails.Rows[0]["inTime"].ToString());
                                lblInTime.Text = InTime.ToString("dd-MM-yyyy HH:mm");
                                //lblParkedHrs.Text = Convert.ToString(CurTime.Subtract(InTime).Hours) + " : " + Convert.ToString(CurTime.Subtract(InTime).Minutes);

                                string[] Hours = Convert.ToString(CurTime.Subtract(InTime).TotalHours).Split('.');
                                string Mins = Convert.ToString(CurTime.Subtract(InTime).Minutes);
                                lblParkedHrs.Text = Hours[0].ToString() + ":" + Mins.ToString();
                            }

                            //TimeSpan Duration = CurTime - InTime;
                            //string[] durationhrs = Convert.ToString(Duration.ToString()).Split('.');

                            lblLaneslotsNo.Text = userSlotTable.Rows[0]["laneNumber"].ToString() + "-" + userSlotTable.Rows[0]["slotNumber"].ToString();
                            string BookingId = dt.Rows[0]["bookingId"].ToString();
                            string pinNo = dt.Rows[0]["pinNo"].ToString();
                            decimal initialAmount = Convert.ToDecimal(dt.Rows[0]["totalAmount"].ToString());
                            decimal remainingAmount = Convert.ToDecimal(dt.Rows[0]["remainingAmount"].ToString());
                            decimal extendAmount = Convert.ToDecimal(dt.Rows[0]["extendAmount"].ToString());
                            decimal extendTax = Convert.ToDecimal(dt.Rows[0]["extendTax"].ToString());
                            decimal topayAmount = remainingAmount + extendAmount;
                            string slotd = userSlotTable.Rows[0]["slotId"].ToString();

                            string extendDayHour = dt.Rows[0]["extendDayHour"].ToString();
                            string bookingDurationType = dt.Rows[0]["bookingDurationType"].ToString();
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
                                ImgExtnVhcl.Visible = false;
                                ImgRemVhcl.Visible = false;
                                ImgPasVhcl.Visible = false;
                            }
                            else
                            {
                                divPassBookingVehicleNo.Visible = true;
                                divRemainingVehicleNo.Visible = true;
                                divExtendVehicleNo.Visible = true;
                                ImgExtnVhcl.Visible = true;
                                ImgRemVhcl.Visible = true;
                                ImgPasVhcl.Visible = true;
                            }
                            lblReVehicleNo.Text = vehicleNumber.Trim();
                            lblvehicles.Text = vehicleNumber.Trim();
                            lblVehicleIn.Text = vehicleNumber.Trim();

                            ImgExtnVhcl.ImageUrl = GetvehicleDetails.Rows[0]["vehiclePlaceHolderImageUrl"].ToString().Trim();
                            ImgRemVhcl.ImageUrl = GetvehicleDetails.Rows[0]["vehiclePlaceHolderImageUrl"].ToString().Trim();
                            ImgPasVhcl.ImageUrl = GetvehicleDetails.Rows[0]["vehiclePlaceHolderImageUrl"].ToString().Trim();

                            lblBlockName.Text = dt.Rows[0]["blockName"].ToString().Trim();
                            lblFloorName.Text = dt.Rows[0]["floorName"].ToString().Trim();
                            //lblReBookingId.Text = dt.Rows[0]["bookingId"].ToString().Trim();
                            //lblRePinNo.Text = dt.Rows[0]["pinNo"].ToString().Trim();
                            lblReBlockName.Text = dt.Rows[0]["blockName"].ToString().Trim();
                            lblReFloorName.Text = dt.Rows[0]["floorName"].ToString().Trim();
                            lblPaymentTypes.Text = dt.Rows[0]["paymentStatus"].ToString().Trim() == "P" ? "Paid" : "Unpaid";
                            lblPaymentStatusRe.Text = dt.Rows[0]["paymentStatus"].ToString().Trim() == "P" ? "Paid" : "Unpaid";
                            lblBlockIn.Text = ddlblock.SelectedItem.Text;
                            lblFloorIn.Text = ddlfloor.SelectedItem.Text;
                            lblBookingIn.Text = BookingId.Trim();
                            lblPinIn.Text = pinNo.Trim();

                            lblInitialAmount.Text = initialAmount.ToString("0.00").Trim();
                            lblTimeExtended.Text = dayhourlbl[0].Trim();
                            lblExtendedAmount.Text = extendAmount.ToString("0.00").Trim();
                            lblRemAmount.Text = remainingAmount.ToString("0.00").Trim();
                            lblTopayAmount.Text = topayAmount.ToString("0.00").Trim();
                            lblPaymentTypePass.Text = dt.Rows[0]["paymentStatus"].ToString().Trim() == "P" ? "Paid" : "Unpaid";
                            //lblParkedHours.Text = dt.Rows[0]["vehicleParkedTime"].ToString();
                            lblBookingAmountPass.Text = initialAmount.ToString("0.00").Trim();
                            if (initialAmount == 0)
                            {
                                divBookingAmountPAss.Visible = false;
                            }
                            else
                            {
                                divBookingAmountPAss.Visible = true;
                            }
                            lblReRemainingAmount.Text = remainingAmount.ToString("0.00");
                            // lblReVehicleNo.Text = vehicleNumber.Trim();
                            lblReInitialAmount.Text = topayAmount.ToString("0.00");
                            decimal Amount = Convert.ToDecimal(extendAmount) + Convert.ToDecimal(lblRemAmount.Text.Trim());
                            decimal TotAmount = Convert.ToDecimal(topayAmount) + Convert.ToDecimal(extendAmount);
                            lblReToPay.Text = Amount.ToString("0.00");
                            lblReTotalAmount.Text = TotAmount.ToString("0.00");
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
                            divNormalHeader.Visible = true;
                            divPassHeader.Visible = false;
                            if (vehicleStatus.Trim() == "")
                            {
                                CheckInorCheckout.InnerText = "Check In";
                                modalSearch.Visible = true;
                                divpass.Visible = true;
                                divextend.Visible = false;
                                divRemaining.Visible = false;
                                divpassDetailss.Visible = false;
                                divPayment.Visible = false;
                                btnCheckInPopup.Text = "Check In";
                                ViewState["Flag"] = "0";
                                divModalColor.Attributes.Add("style", "background:#bcffce;");
                                divModalColor.Style.Add("border-radius", "10px");
                            }
                            else if (vehicleStatus.Trim() == "I")
                            {
                                if (extendAmount != 0)
                                {
                                    divExtraFeeFeatures.Visible = true;
                                    modalSearch.Visible = true;
                                    divextend.Visible = true;
                                    //CheckInorCheckout.InnerText = "Check Out";
                                    divRemaining.Visible = false;
                                    divpass.Visible = false;
                                    btnCheckInPopup.Text = "Pay And CheckOut";
                                    ViewState["Flag"] = "2";

                                }
                                else if (remainingAmount != 0)
                                {
                                    divReminingExtra.Visible = true;
                                    modalSearch.Visible = true;
                                    divRemaining.Visible = true;
                                    divpass.Visible = false;
                                    //CheckInorCheckout.InnerText = "Check Out";
                                    divextend.Visible = false;
                                    btnCheckInPopup.Text = "Pay And CheckOut";
                                    ViewState["Flag"] = "3";
                                    divRemaining.Visible = true;
                                }
                                else if (remainingAmount == 0 && extendAmount == 0)
                                {
                                    //CheckInorCheckout.InnerText = "Check Out";
                                    modalSearch.Visible = true;
                                    divpass.Visible = true;
                                    divextend.Visible = false;
                                    divRemaining.Visible = false;
                                    divpassDetailss.Visible = false;
                                    divpassstatus.Visible = true;
                                    divPayment.Visible = false;
                                    btnCheckInPopup.Text = "Check Out";
                                    ViewState["Flag"] = "1";
                                }
                                CheckInorCheckout.InnerText = "Check Out";
                                divModalColor.Attributes.Add("style", "background:#ffbcbc; ");
                                divModalColor.Style.Add("border-radius", "10px");
                            }
                            else
                            {
                                txtBookingId.Text = string.Empty;
                                Clear();
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Already Checked Out');", true);
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            txtBookingId.Text = string.Empty;
                            Clear();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        txtBookingId.Text = string.Empty;
                        Clear();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + response.ReasonPhrase.ToString().Trim() + "');", true);
                    txtBookingId.Text = string.Empty;
                    Clear();
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
        finally
        {
            Pathdesigns();
        }
    }
    public void BindDdlPayment()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlPayment.Items.Clear();
                ddlPassPaymentMode.Items.Clear();
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

                        ddlPayment.DataSource = dtDEmployeeType;
                        ddlPayment.DataValueField = "configId";
                        ddlPayment.DataTextField = "configName";
                        ddlPayment.DataBind();

                        ddlPayment.Items.Remove(ddlPayment.Items.FindByText("Pass"));
                        ddlPayment.Items.Remove(ddlPayment.Items.FindByText("Pay At CheckOut"));
                        ddlPassPaymentMode.DataSource = dtDEmployeeType;
                        ddlPassPaymentMode.DataValueField = "configId";
                        ddlPassPaymentMode.DataTextField = "configName";
                        ddlPassPaymentMode.DataBind();

                        ddlPassPaymentMode.Items.Remove(ddlPassPaymentMode.Items.FindByText("Pass"));
                        ddlPassPaymentMode.Items.Remove(ddlPassPaymentMode.Items.FindByText("Pay At CheckOut"));

                        ddlRepayment.DataSource = dtDEmployeeType;
                        ddlRepayment.DataValueField = "configId";
                        ddlRepayment.DataTextField = "configName";
                        ddlRepayment.DataBind();

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
    protected void ImageCloseCheckInCheckOut_Click(object sender, ImageClickEventArgs e)
    {
        modalSearch.Visible = false;
        Clear();
    }
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
                UpdateCheckInOutDetails(ViewState["vehicleNumber"].ToString().Trim(),
                   ViewState["vehicleHeaderId"].ToString().Trim(), times, null, "I", ViewState["slotd"].ToString().Trim(), "", "");
                //BindParkingSlot();
            }
            else if (ViewState["Flag"].ToString() == "1")
            {
                UpdateCheckInOutDetails(ViewState["vehicleNumber"].ToString().Trim(),
                   ViewState["vehicleHeaderId"].ToString().Trim(),
                    null, times, "O", ViewState["slotd"].ToString().Trim(), "", "");
                //BindParkingSlot();
            }
            else if (ViewState["Flag"].ToString() == "4")
            {
                UpdateCheckInOutDetails(ViewState["vehicleNumber"].ToString().Trim(),
                   ViewState["vehicleHeaderId"].ToString().Trim(),
                    null, times, "O", ViewState["slotd"].ToString().Trim(), ViewState["paidAmount"].ToString().Trim(), ddlPassPaymentMode.SelectedValue);
                // BindParkingSlot();
            }
            else if (ViewState["Flag"].ToString() == "2")
            {
                UpdateExtendedTimeAmount(ViewState["vehicleNumber"].ToString(), ViewState["bookingDurationType"].ToString(),
                    date2[1], date, ViewState["topayAmount"].ToString(),
                     ViewState["topayAmount"].ToString(),
                      ViewState["BookingId"].ToString(), ViewState["vehicleHeaderId"].ToString(),
                      ViewState["extendTax"].ToString(), ViewState["slotd"].ToString(), ddlPayment.SelectedValue);
                //BindParkingSlot();
            }
            else if (ViewState["Flag"].ToString() == "3")
            {
                UpdateCheckInOutDetailsrem(ViewState["vehicleNumber"].ToString().Trim(),
                    ViewState["vehicleHeaderId"].ToString(), null, times,
                           "O", ViewState["slotd"].ToString(), ViewState["topayAmount"].ToString(), ddlRepayment.SelectedValue);
                //BindParkingSlot();
            }
            BindParkingSlot();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {
        Clear();
    }
    public void Clearmodal()
    {
        modalSearch.Visible = false;

        lblPinIn.Text = string.Empty;
        lblBlockIn.Text = string.Empty;
        lblPaymentStatusRe.Text = string.Empty;
        lblFloorIn.Text = string.Empty;
        lblVehicleIn.Text = string.Empty;
        txtBookingId.Text = "";
        lblBlockName.Text = "";
        lblFloorName.Text = "";
        lblTimeExtended.Text = "";
        lblExtendedAmount.Text = "";
        lblvehicles.Text = "";
        lblRemAmount.Text = "";
        lblReFloorName.Text = "";
        lblReInitialAmount.Text = "";
        lblReRemainingAmount.Text = "";
        lblReToPay.Text = "";
        lblReVehicleNo.Text = "";
        lblReTotalAmount.Text = "";
        lblReBlockName.Text = "";
        divExtraFeeFeatures.Visible = false;
        lblPassTransactionPassId.Text = "";
        lblPassTypeINout.Text = "";
        lblMobileNoPass.Text = "";
        lblStartPass.Text = "";
        lblEndPass.Text = "";
        lblCategoryPAss.Text = "";
        lblVehicleTypeNamePass.Text = "";
        ViewState["vehicleStatus"] = "";
        ViewState["vehicleNumber"] = "";
        ViewState["vehicleHeaderId"] = "";
        ViewState["slotd"] = "";
        ViewState["remainingAmount"] = "";
        ViewState["extendAmount"] = "";
        ViewState["extendTax"] = "";
        ViewState["topayAmount"] = "";
        ViewState["BookingId"] = "";
        ViewState["bookingDurationType"] = "";
        txtBookingId.Focus();
        ImgExtnVhcl.ImageUrl = "";
        ImgRemVhcl.ImageUrl = "";
        ImgPasVhcl.ImageUrl = "";
    }

    #region UpdateCheckInOutDetails
    public void UpdateCheckInOutDetails(string BookingID, string svehicleHeaderId, string Intime,
       string OutTime, string vehicleStatus, string slotId, string PaidAmount, string PaymentType)
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
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        string BorP = ViewState["BookingId"].ToString().Substring(0, 1);
                        if (ddlPaymentType.SelectedItem.Text == "Online" && txtmobileNo.Text != "")
                        {
                            SendPaymentLink(txtmobileNo.Text, ViewState["BookingId"].ToString());
                        }
                        else
                        {
                            if (BorP.Trim() == "B" && vehicleStatus.Trim() == "O")
                            {
                                Response.Redirect("Print.aspx?rt=CO&bi=" + ViewState["BookingId"].ToString() + "", false);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    txtBookingId.Text = string.Empty;
                    Clear();
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
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            string BorP = ViewState["BookingId"].ToString().Substring(0, 1);
                            if (ddlPaymentType.SelectedItem.Text == "Online" && txtmobileNo.Text != "")
                            {
                                SendPaymentLink(txtmobileNo.Text, ViewState["BookingId"].ToString());
                            }
                            else
                            {
                                if (BorP.Trim() == "B")
                                {
                                    Response.Redirect("Print.aspx?rt=CO&bi=" + ViewState["BookingId"].ToString() + "", false);
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        }
                        txtBookingId.Text = string.Empty;
                        Clear();

                    }
                    else
                    {
                        txtBookingId.Focus();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        //Response.Redirect("Print.aspx?rt=CO&bi=" + BookingId.ToString() + "", false);
                        string BorP = BookingId.ToString().Substring(0, 1);
                        if (ddlPaymentType.SelectedItem.Text == "Online" && txtmobileNo.Text != "")
                        {
                            SendPaymentLink(txtmobileNo.Text, BookingId);
                        }
                        else
                        {
                            if (BorP.Trim() == "B")
                            {
                                Response.Redirect("Print.aspx?rt=CO&bi=" + BookingId.ToString() + "", false);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    txtBookingId.Text = string.Empty;
                    Clear();
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

    #region Pass Transaction
    #region Remove Offer Click
    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        try
        {
            divoffer.Visible = true;
            lbltotalamt.Style.Add("text-decoration", "none");
            lblOffertotal.Text = "";
            lnkRemove.Visible = false;
            if (ddlPassCategory.SelectedValue == "V")
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/prime-pass.png" });
            }
            else
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/normie-pass.png" });

            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region NewPass Click
    protected void btnNewPass_Click(object sender, EventArgs e)
    {
        Pathdesigns();
        modalPass.Visible = true;
        BindPassTransVehicles();
        BindPassTransPaymentType();

    }
    #endregion
    #region ImageClosePass Click
    protected void ImageClosePass_Click(object sender, ImageClickEventArgs e)
    {
        Pathdesigns();
        modalPass.Visible = false;
    }
    #endregion
    #region Bind Vehicles
    public void BindPassTransVehicles()
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
                      + "branchMaster?branchId=" + Session["branchId"].ToString() + "&type=V";
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
                            ClearPass();
                            DivPassTicket.Visible = false;
                            divDownload.Visible = false;
                            ddlPassCategory.ClearSelection();
                            ddlPassType.Items.Clear();
                            gvVehicleTypePass.DataSource = dt;
                            gvVehicleTypePass.DataBind();
                            ViewState["data1"] = dt;

                            Label divvehicle = (Label)gvVehicleTypePass.Items[0].FindControl("lblvehicleNamePass");
                            //divvehicle.Style.Add("color", "#fff");
                            divvehicle.Style.Add("color", "black");
                            HtmlControl divrower = gvVehicleTypePass.Items[0].FindControl("divvehicless") as HtmlControl;
                            divrower.Attributes["class"] = "carimage2";
                            HtmlControl divcarname = gvVehicleTypePass.Items[0].FindControl("divcarnames") as HtmlControl;
                            divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";
                            Label lblvehicleConfigId = (Label)gvVehicleTypePass.Items[0].FindControl("lblvehicleConfigIdPass");
                            ViewState["vehicleTypes"] = lblvehicleConfigId.Text;
                            lblVehicleTypeId.Text = lblvehicleConfigId.Text;
                            divPassType.Visible = true;
                            divoffer.Visible = true;
                            lnkRemove.Visible = false;
                        }
                        else
                        {
                            ddlPassCategory.ClearSelection();
                            ddlPassType.Items.Clear();
                            gvVehicleTypePass.DataSource = dt;
                            gvVehicleTypePass.DataBind();
                            ViewState["data1"] = dt;
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
    #region Clear Function
    public void ClearPass()
    {
        lblVehicleType.Text = "";
        lblvehicleamt.Text = "";
        lbltaxamt.Text = "";
        lbltotalamt.Text = "";
        lbltotalamt.Style.Add("text-decoration", "none");
        lblOffertotal.Text = "";
        ddlPassType.ClearSelection();
        ddlPaymentType.ClearSelection();
        txtMobileNoPass.Text = "";
        divpasssummary.Visible = false;
        ViewState["OfferId"] = "";
        ViewState["OfferType"] = "";
        ViewState["OfferValue"] = "";

    }
    #endregion
    #region Bind Pass Type
    public void BindPassType()
    {
        ViewState["Flags"] = "0";
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                     + "parkingPassConfig?branchId=" + Session["branchId"].ToString() + "&vehicleType=" + ViewState["vehicleType"] +
                     "&passCategory=" + ddlPassCategory.SelectedValue + "&activeStatus=A";

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
                            ddlPassType.DataSource = dt;
                            ddlPassType.DataValueField = "parkingPassConfigId";
                            ddlPassType.DataTextField = "passType";
                            ddlPassType.DataBind();
                        }
                        else
                        {
                            ddlPassType.DataBind();
                        }
                        ddlPassType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlPassType.Items.Clear();
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
    #region Bind Payment Type
    public void BindPassTransPaymentType()
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
                            ddlpaymenttypepass.DataSource = dt;
                            ddlpaymenttypepass.DataValueField = "configId";
                            ddlpaymenttypepass.DataTextField = "configName";
                            ddlpaymenttypepass.DataBind();
                            ddlpaymenttypepass.SelectedValue = dt.Rows[0]["configId"].ToString();

                            ddlpaymenttypepass.Items.Remove(ddlpaymenttypepass.Items.FindByText("Pass"));
                            ddlpaymenttypepass.Items.Remove(ddlpaymenttypepass.Items.FindByText("Pay At CheckOut"));
                        }
                        else
                        {
                            ddlpaymenttypepass.DataBind();
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlpaymenttypepass.Items.Clear();
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
    #region Vehicle Type Click
    protected void lblVehicleTypesPass_Click(object sender, EventArgs e)
    {
        try
        {
            ClearPass();
            DivPassTicket.Visible = false;
            divDownload.Visible = false;
            ddlPassCategory.ClearSelection();
            LinkButton lnkbtn = sender as LinkButton;
            DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;
            Label lblvehicleConfigId = (Label)gvrow.FindControl("lblvehicleConfigIdPass");
            ViewState["vehicleTypes"] = lblvehicleConfigId.Text;
            lblVehicleTypeId.Text = lblvehicleConfigId.Text;
            divPassType.Visible = true;
            divoffer.Visible = true;
            lnkRemove.Visible = false;
            Label lblvehicleName = (Label)gvrow.FindControl("lblvehicleNamePass");
            if (lblvehicleName.Text == "Car")
            {
                Passbackground.Style.Add("background-image", "../images/Backgroundcar.png");
                Passbackground.Style.Add("background-size", "50%");
            }
            else if (lblvehicleName.Text == "Bike")
            {
                Passbackground.Style.Add("background-image", "../images/Backgroundbike.png");
                Passbackground.Style.Add("background-size", "40%");

            }
            Passbackground.Style.Add("background-repeat", "no-repeat");
            Passbackground.Style.Add("opacity", "0.2");
            Passbackground.Style.Add("height", "500px");
            Passbackground.Style.Add("background-position", "center");
            Passbackground.Style.Add("margin-top", "0rem");
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion
    #region VehicleType Color Change
    protected void gvVehicleTypePass_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            for (int j = 0; j < gvVehicleTypePass.Items.Count; j++)
            {
                int i = e.Item.ItemIndex;
                if (i == j)
                {
                    Label divvehicle = (Label)gvVehicleTypePass.Items[j].FindControl("lblvehicleNamePass");
                    //divvehicle.Style.Add("color", "#fff");
                    divvehicle.Style.Add("color", "black");
                    HtmlControl divrower = gvVehicleTypePass.Items[j].FindControl("divvehicless") as HtmlControl;
                    divrower.Attributes["class"] = "carimage2";
                    HtmlControl divcarname = gvVehicleTypePass.Items[j].FindControl("divcarnames") as HtmlControl;
                    divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";

                }
                else
                {
                    Label divvehicle = (Label)gvVehicleTypePass.Items[j].FindControl("lblvehicleNamePass");
                    divvehicle.Style.Add("color", "black");
                    HtmlControl divrower = gvVehicleTypePass.Items[j].FindControl("divvehicless") as HtmlControl;
                    divrower.Attributes["class"] = "carimage";
                    HtmlControl divcarname = gvVehicleTypePass.Items[j].FindControl("divcarnames") as HtmlControl;
                    divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname";
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Get Pass Amount Function
    public void GetPassAmount()
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
                      + "parkingPassConfig?branchId=" + Session["branchId"].ToString() + "&vehicleType=" + ViewState["vehicleTypes"] +
                      "&passType=" + ddlPassType.SelectedItem + "&passCategory="
                      + ddlPassCategory.SelectedValue + "&activeStatus=A";
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
                            ViewState["PassId"] = dt.Rows[0]["parkingPassConfigId"].ToString();
                            ViewState["VehicleAmount"] = dt.Rows[0]["amount"].ToString();
                            ViewState["TaxAmount"] = dt.Rows[0]["tax"].ToString();
                            ViewState["TotalAmount"] = dt.Rows[0]["totalAmount"].ToString();
                            ViewState["TaxId"] = dt.Rows[0]["taxId"].ToString();
                            ViewState["VehicleName"] = dt.Rows[0]["vehicleTypeName"].ToString();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            ClearPass();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ClearPass();
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
    #region Get Pass Amount
    protected void ddlPassCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlPassType.ClearSelection();
            DivPassTicket.Visible = false;
            divDownload.Visible = false;
            divpasssummary.Visible = false;
            BindPassType();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #endregion
    #region Insert Function
    public void InsertParkingPass()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Insert = new PassTransaction()
                {
                    passTransactionDetails = GetpassTransactionDetails(Session["parkingOwnerId"].ToString(),
                    Session["branchId"].ToString(), ViewState["PassId"].ToString(), txtMobileNoPass.Text, ViewState["vehicleType"].ToString(),
                    Convert.ToDecimal(ViewState["TotalAmount"].ToString()).ToString(),
                     ViewState["TaxId"].ToString(), "P", ddlpaymenttypepass.SelectedValue, "A",
                     Session["UserId"].ToString().Trim(), null, null)
                };
                if (ViewState["Flags"].ToString() == "1")
                {
                    if (ViewState["OfferId"].ToString() != "0" && ViewState["OfferId"].ToString() != "")
                    {
                        Insert.passTransactionDetails = GetpassTransactionDetails(Session["parkingOwnerId"].ToString(),
                        Session["branchId"].ToString(), ViewState["PassId"].ToString(), txtMobileNoPass.Text, ViewState["vehicleType"].ToString(),
                         ViewState["OfferAmount"].ToString(), ViewState["TaxId"].ToString(), "P", ddlpaymenttypepass.SelectedValue, "A",
                          Session["UserId"].ToString().Trim(),
                          ViewState["OfferId"].ToString(), ViewState["OfferValue"].ToString());

                    }
                }

                HttpResponseMessage response = client.PostAsJsonAsync("passTransaction", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        string parkingPassTransId = JObject.Parse(SmartParkingList)["parkingPassTransId"].ToString();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(parkingPassTransId);
                        if (dt.Rows.Count > 0)
                        {
                            string passTransactionId = dt.Rows[0]["passTransactionId"].ToString();
                            GetQRcode(passTransactionId);
                            GetPassDetails(passTransactionId);
                            divoffer.Visible = true;
                            lnkRemove.Visible = false;
                        }

                        ClearPass();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlPassCategory.ClearSelection();
                        ddlPassType.Items.Clear();
                    }
                    else
                    {
                        ClearPass();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlPassCategory.ClearSelection();
                        ddlPassType.Items.Clear();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public static List<passTransactionDetails> GetpassTransactionDetails(string parkingOwnerId, string branchId,
       string passId, string phoneNumber, string vehicleType, string totalAmount, string taxId, string paymentStatus, string paymentType
        , string activeStatus, string createdBy, string offerId, string offerAmount)
    {
        List<passTransactionDetails> lst = new List<passTransactionDetails>();

        lst.AddRange(new List<passTransactionDetails>
            {
                new passTransactionDetails { parkingOwnerId=parkingOwnerId ,branchId=branchId, passId=passId,phoneNumber=phoneNumber
                ,vehicleType=vehicleType,totalAmount=totalAmount,taxId=taxId,paymentStatus=paymentStatus , paymentType =paymentType
                ,activeStatus=activeStatus ,createdBy=createdBy,offerId=offerId,offerAmount=offerAmount}

            });


        return lst;

    }


    #endregion
    #region Submit Click 
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMobileNoPass.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Enter Mobile No.');", true);
                return;
            }

            if (txtMobileNoPass.Text.Length < 10)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('Enter 10 Digit Mobile No.');", true);
                return;
            }

            InsertParkingPass();
            if (ddlPassCategory.SelectedValue == "V")
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/prime-pass.png" });
            }
            else
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/normie-pass.png" });
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Parking  Pass Class 
    public class PassTransaction
    {
        public List<passTransactionDetails> passTransactionDetails { get; set; }
    }
    public class passTransactionDetails
    {
        public String passId { get; set; }
        public String parkingOwnerId { get; set; }
        public String activeStatus { get; set; }
        public String branchId { get; set; }
        public String phoneNumber { get; set; }
        public String userId { get; set; }
        public String vehicleType { get; set; }
        public String paymentType { get; set; }
        public String totalAmount { get; set; }
        public String tax { get; set; }
        public String taxId { get; set; }
        public String paymentStatus { get; set; }
        public String offerId { get; set; }
        public String offerAmount { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }
    }

    public class offer
    {
        public String offerId { get; set; }
        public String offerValue { get; set; }
        public String offerType { get; set; }
        public String offerHeading { get; set; }
    }

    public class parkingPassTransId
    {
        public List<GetparkingPassTransId> GetparkingPassTransId { get; set; }
    }
    public class GetparkingPassTransId
    {
        public String parkingPassTransId { get; set; }
    }
    #endregion

    #region Get Offer Details 
    public void GetOfferDetails()
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
                      + "offerMaster?branchId=" + Session["branchId"].ToString() + "&Amount=" + ViewState["TotalAmount"] + "&activeStatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {

                        List<offer> offer = JsonConvert.DeserializeObject<List<offer>>(ResponseMsg);
                        DataTable dt = ConvertToDataTable(offer);

                        if (dt.Rows.Count > 0)
                        {
                            dlOfferDetails.DataSource = dt;
                            dlOfferDetails.DataBind();
                            ViewState["Flags"] = "1";

                        }
                        else
                        {
                            dlOfferDetails.DataSource = dt;
                            dlOfferDetails.DataBind();
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        modalpasssub.Visible = false;
                        divoffer.Visible = true;
                        lnkRemove.Visible = false;
                        if (ddlPassCategory.SelectedValue == "V")
                        {
                            divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/prime-pass.png" });
                        }
                        else
                        {
                            divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/normie-pass.png" });

                        }
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
    #region Offer Click
    protected void lblofferHeading_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;

            Label lblOfferId = (Label)gvrow.FindControl("lblOfferId");
            Label lblOfferType = (Label)gvrow.FindControl("lblOfferType");
            Label lblOfferValue = (Label)gvrow.FindControl("lblOfferValue");

            ViewState["OfferId"] = lblOfferId.Text;
            ViewState["OfferType"] = lblOfferType.Text;
            ViewState["OfferValue"] = lblOfferValue.Text;
            if (ViewState["OfferType"].ToString() == "P")
            {
                decimal Amount = Convert.ToDecimal(ViewState["TotalAmount"].ToString());
                string per = ViewState["OfferValue"].ToString().Replace(".0", string.Empty);
                int Percent = Convert.ToInt32(per);
                decimal totalAmount = Convert.ToDecimal((Math.Round(Amount - (Amount * Percent) / 100)).ToString());
                ViewState["OfferAmount"] = totalAmount;

            }
            else
            {
                decimal Amount = Convert.ToDecimal(ViewState["TotalAmount"].ToString());
                string per = ViewState["OfferValue"].ToString().Replace(".0", string.Empty);
                int Offer = Convert.ToInt32(per);
                decimal totalAmount = Convert.ToDecimal(Amount - Offer);
                ViewState["OfferAmount"] = totalAmount;
            }
            lbltotalamt.Style.Add("text-decoration", "line-through");
            lblOffertotal.Text = "₹ " + ViewState["OfferAmount"].ToString();
            divpasssummary.Visible = true;
            if (ddlPassCategory.SelectedValue == "V")
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/prime-pass.png" });
            }
            else
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/normie-pass.png" });

            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Offer Click Apply
    protected void Lnkoffer_Click(object sender, EventArgs e)
    {
        try
        {
            modalpasssub.Visible = true;
            divoffer.Visible = false;
            lnkRemove.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "show()", true);
            GetOfferDetails();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Offer Close Click
    protected void linkoffclose_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
            divoffer.Visible = true;
            lnkRemove.Visible = false;
            if (ddlPassCategory.SelectedValue == "V")
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/prime-pass.png" });
            }
            else
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/normie-pass.png" });

            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Pass Details
    protected void ddlPassType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divpasssummary.Visible = true;
            divoffer.Visible = true;
            lnkRemove.Visible = false;
            lbltotalamt.Style.Add("text-decoration", "none");
            lblOffertotal.Text = "";
            if (ddlPassCategory.SelectedValue == "V")
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/prime-pass.png" });
            }
            else
            {
                divSummaryImg.Controls.Add(new Image { ImageUrl = "~/images/normie-pass.png" });

            }
            GetPassAmount();
            lblVehicleType.Text = ViewState["VehicleName"].ToString();
            lblvehicleamt.Text = "₹ " + ViewState["VehicleAmount"].ToString();
            lbltaxamt.Text = "₹ " + ViewState["TaxAmount"].ToString();
            lbltotalamt.Text = "₹ " + ViewState["TotalAmount"].ToString();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    //Pass Ticket
    #region QR Code Generate 
    public void GetQRcode(string parkingPassTransId)
    {
        try
        {
            string sAssetCode = parkingPassTransId;
            string code = sAssetCode.ToString();
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgEmpPhotoPrevPass.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    string result = Convert.ToBase64String(byteImage, 0, byteImage.Length);

                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #endregion
    #region Get Pass Details  
    public void GetPassDetails(string parkingPassTransId)
    {
        try
        {
            DivPassTicket.Visible = true;
            divDownload.Visible = true;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "passTransaction?ParkingPassTransactionId=" + parkingPassTransId + "";
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
                            List<GetPassTicketPass> lst = JsonConvert.DeserializeObject<List<GetPassTicketPass>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.passType.ToList();
                            DataTable passtype = ConvertToDataTable(lst1);

                            GetQRcode(parkingPassTransId);
                            lblPassParkinngName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblPassBranchName.Text = dt.Rows[0]["branchName"].ToString();
                            lblTicketPassTypePass.Text = passtype.Rows[0]["passType"].ToString();
                            lblPassMobileNoPass.Text = dt.Rows[0]["phoneNumber"].ToString();
                            lblPassStartDatePass.Text = dt.Rows[0]["validStartDate"].ToString();
                            lblPassEndDatePass.Text = dt.Rows[0]["validEndDate"].ToString();
                            lblPassModePass.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblUserPassType.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblPassVehicleTypePass.Text = dt.Rows[0]["vehicleName"].ToString();
                            lblPAssIdPAss.Text = dt.Rows[0]["parkingPassTransId"].ToString();
                            if (lblPassModePass.Text == "VIP")
                            {
                                DivPassimg.Attributes.Add("class", "passVIP");
                            }
                            else
                            {
                                DivPassimg.Attributes.Add("class", "passNormal");
                            }
                            Session["Pass"] = StringEncryption.Encrypt(parkingPassTransId.Trim());
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
    #region Pass Ticket Class  
    public class GetPassTicketPass
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
        public List<GetpassTypePass> passType { get; set; }

    }
    public class GetpassTypePass
    {
        public string passType { get; set; }
        public string passCategory { get; set; }

    }

    #endregion
    #region SendNotification 
    public class SendNotification
    {
        public string type { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }
        public string link { get; set; }

    }


    #endregion
    #region Send SMS
    protected void btnExportPdf_Click(object sender, EventArgs e)
    {

        string image = hfImageUrl.Value;
        string link = System.Configuration.ConfigurationManager.AppSettings["PassUrl"].Trim()
        + "Pass.aspx?ReferenceNo=" + Session["Pass"].ToString() + "";
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new SendNotification()
                {
                    type = "P",
                    mobileNo = lblPassMobileNo.Text,
                    link = hfImageUrl.Value
                };

                HttpResponseMessage response = client.PostAsJsonAsync("sendNotification", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DivPassTicket.Visible = false;
                        divDownload.Visible = false;
                        lblPassParkinngName.Text = "";
                        lblPassBranchName.Text = "";
                        lblTicketPassType.Text = "";
                        lblPassMobileNo.Text = "";
                        lblPassStartDate.Text = "";
                        lblPassEndDate.Text = "";
                        lblPassMode.Text = "";
                        lblUserPassType.Text = "";
                        lblPassVehicleType.Text = "";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #endregion
    //protected void btnExportPdf_Click(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "SendPdf", "SendPdf();", true);
    //}
    #endregion

    #region Bind Count
    public void BindCount()
    {
        DateTime utcTime = DateTime.Now;
        string date = utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
        string[] time = date.Split('T');
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                      + "bookingMaster?branchId=" + Session["branchId"].ToString().Trim() + "&type=C"
                      + "&fromDate=" + date.ToString() + "&toDate=" + date.ToString() + "";

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
                            lblpass.Text = dt.Rows[0]["passCheckedIn"].ToString();
                            lblcheckin.Text = dt.Rows[0]["checkedInCount"].ToString();
                            lblcheckout.Text = dt.Rows[0]["checkedOutCount"].ToString();
                            lblBookedCount.Text = dt.Rows[0]["bookedCount"].ToString();
                            lblReservedCount.Text = dt.Rows[0]["reserved"].ToString();
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

    #region OTP Details
    protected void btnCfmOtp_Click(object sender, EventArgs e)
    {
        Pathdesigns();
        try
        {
            if (txtOTP.Text == ViewState["OTP"].ToString().Trim())
            {
                txtOTP.Text = string.Empty;
                btnBook.Enabled = true;
                divOtpDetails.Visible = false;
                divResend.Visible = false;
                txtmobileNo.Enabled = false;
                txtVehicleNo.Enabled = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "successalert('OTP Verified Successfully');", true);
            }
            else
            {
                txtOTP.Text = string.Empty;
                txtmobileNo.Enabled = true;
                txtVehicleNo.Enabled = true;
                btnResend.Text = "Resend";
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "infoalert('Please Check the OTP sent / Time Out / Click Resend');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }

    protected void btnResend_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["OTP"] = "";
            btnResend.Text = "00:30";
            btnCfmOtp.Visible = true;
            SendOTP();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        try
        {
            SendOTP();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void SendOTP()
    {
        try
        {
            if (txtmobileNo.Text.Length == 10)
            {
                var reg = new Regex("[0-9]{10}");
                if (!reg.IsMatch(txtmobileNo.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "infoalert('Please Enter Valid 10 digit Mobile No.');", true);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var Insert = new LoginClass()
                        {
                            username = txtmobileNo.Text.Trim()
                        };
                        HttpResponseMessage response = client.PostAsJsonAsync("verifyOTP", Insert).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                            int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                            string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                            if (StatusCode == 1)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                                string Otp = JObject.Parse(SmartParkingList)["OTP"].ToString();
                                ViewState["OTP"] = Otp.Trim();
                                divEnterOtp.Visible = true;
                                divResend.Visible = true;
                                divSendOtp.Visible = false;
                                ScriptManager.RegisterStartupScript(this, GetType(), "SendOtp", "SendOtp();", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "successalert('" + Otp + "');", true);
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            }
                        }
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "infoalert('Please Enter Valid 10 digit Mobile No.');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "erroralert('" + ex + "');", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.Message.ToString().Trim() + "');", true);
        }
        finally
        {
            Pathdesigns();
        }
    }

    public class LoginClass
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    #endregion

    #region Slot and Add on service tab
    protected void btnslottab_Click(object sender, EventArgs e)
    {
        try
        {
            Pathdesigns();
            Divslotrow.Attributes.Add("class", "col-12 col-sm-8 col-md-8 col-lg-8 col-xl-8 table-responsive section");
            btnAddonservices.Style.Remove("border-top");
            btnAddonservices.Style.Remove("border-right");
            btnAddonservices.Style.Remove("border-left");
            btnAddonservices.Style.Add("border-bottom", "2px solid #c3c4c6");
            btnslottab.Style.Remove("border-bottom");
            btnslottab.Style.Add("border-top-left-radius", "6px");
            btnslottab.Style.Add("border-top-right-radius", "6px");
            btnslottab.Style.Add("border-top", "2px solid #a2a4a796");
            btnslottab.Style.Add("border-right", "2px solid #a2a4a796");
            btnslottab.Style.Add("border-left", "2px solid #a2a4a796");
            //btnslottab.Style.Add("background-color", "#0c770f");
            //btnslottab.Style.Add("border", "1px solid #2eab4a");
            //btnAddonservices.Style.Add("background-color", "#283138");
            //btnAddonservices.Style.Add("border", "1px solid #262a2f");
            extraFee__container.Visible = false;
            Divslotrow.Visible = true;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void btnAddonservices_Click(object sender, EventArgs e)
    {
        ChkMobileNo.Enabled = false;
        try
        {
            if (ViewState["BookingType"].ToString() == "P")
            {
                divTimeType.Visible = false;
            }
            if (lblLaneSlot.Text == "")
            {
                divVehicleNo.Visible = false;
            }
            else
            {
                divVehicleNo.Visible = false;
                divVehicleNoDropdown.Visible = true;

            }
            divTimeType.Visible = true;
            btnslottab.Style.Remove("border-top");
            btnslottab.Style.Remove("border-right");
            btnslottab.Style.Remove("border-left");
            btnslottab.Style.Add("border-bottom", "2px solid #c3c4c6");
            btnAddonservices.Style.Remove("border-bottom");
            btnAddonservices.Style.Add("border-top-left-radius", "6px");
            btnAddonservices.Style.Add("border-top-right-radius", "6px");
            btnAddonservices.Style.Add("border-top", "2px solid #a2a4a796");
            btnAddonservices.Style.Add("border-right", "2px solid #a2a4a796");
            btnAddonservices.Style.Add("border-left", "2px solid #a2a4a796");

            //btnslottab.Style.Add("background-color", "#283138");
            //btnslottab.Style.Add("border", "1px solid #262a2f");
            //btnAddonservices.Style.Add("background-color", "#0c770f");
            //btnAddonservices.Style.Add("border", "1px solid #2eab4a");
            extraFee__container.Visible = true;
            Divslotrow.Visible = false;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Mobile No check changed
    protected void ChkMobileNo_CheckedChanged(object sender, EventArgs e)
    {
        Pathdesigns();
        if (ChkMobileNo.Checked == true)
        {
            Session["MobNo"] = null;
            divMobileNo.Visible = true;
            RfvMobNo.Enabled = true;
            btnBook.Enabled = false;
        }
        else
        {
            Session["MobNo"] = "1";
            divMobileNo.Visible = false;
            RfvMobNo.Enabled = false;
            divOtpDetails.Visible = false;
            btnBook.Enabled = true;
        }
        ChkMobileNo.Enabled = false;
    }
    #endregion

    #region Cancel click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Divslotrow.Attributes.Add("class", "col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12 table-responsive section");
        ChkMobileNo.Enabled = true;
        BindParkingSlot();
        Clear();
    }
    #endregion

    #region Close & Clear Function
    #region Modal Close Click
    protected void Close_Click(object sender, EventArgs e)
    {
        Clear();
    }
    #endregion
    public void Clear()
    {
        divSummaryColor.Style.Add("background-color", "#c1c0c36b;");
        Clearmodal();
        btnAddonservices.Style.Remove("border-top");
        btnAddonservices.Style.Remove("border-right");
        btnAddonservices.Style.Remove("border-left");
        btnAddonservices.Style.Add("border-bottom", "2px solid #c3c4c6");
        btnslottab.Style.Add("border-top-left-radius", "6px");
        btnslottab.Style.Add("border-top-right-radius", "6px");
        btnslottab.Style.Add("border-top", "2px solid #a2a4a796");
        btnslottab.Style.Add("border-right", "2px solid #a2a4a796");
        btnslottab.Style.Add("border-left", "2px solid #a2a4a796");
        btnslottab.Style.Remove("border-bottom");

        extraFee__container.Visible = false;
        Divslotrow.Visible = true;
        dtlExtraFeeSummary.DataSource = null;
        dtlExtraFeeSummary.DataBind();
        extrafeeandfeatursContainer.Visible = false;
        divExtraFeeAmount.Visible = false;
        divExtrafeeSummary1.Visible = false;

        divfromdate.Visible = false;
        divTodate.Visible = false;
        divFromTime.Visible = false;
        divTotime.Visible = false;
        divPassdetails.Visible = false;
        divVehicleNoDropdown.Visible = false;
        divMobileNo.Visible = true;
        divParkingAmount.Visible = true;
        divTax.Visible = true;
        divSummary.Visible = false;

        divlaneSlotNo.Visible = false;
        ImgSummary.Visible = false;
        ImgSummary1.Visible = false;


        ddlPaymentType.ClearSelection();
        rbtnTimeType.ClearSelection();

        txtmobileNo.Enabled = true;
        txtVehicleNo.Enabled = true;

        lblLaneSlot.Text = "";
        txtmobileNo.Text = "";
        txtVehicleNo.Text = "";
        lblTicketPassType.Text = "";
        lblPassMobileNo.Text = "";
        lblPassStartDate.Text = "";
        lblPassEndDate.Text = "";
        lblPassMode.Text = "";
        lblPassVehicleType.Text = "";

        DateTime date = DateTime.Now.AddDays(1);
        txtTodate.Text = date.ToString("yyyy-MM-dd");
        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        ViewState["PinNo"] = "";
        ViewState["AccessoriesTotalAmount"] = "0";
        ViewState["AccessoriesAmount"] = "0";
        ViewState["AccessoriesTax"] = "0";
        ViewState["paylaterOption"] = "0";
        ViewState["ExtraRowCartRow"] = null;
        ViewState["ExtraRow"] = null;
        ViewState["Row"] = null;
        ViewState["CartRow"] = null;

        if (ChkMobileNo.Checked == true)
        {
            divMobileNo.Visible = true;
            RfvMobNo.Enabled = true;
            btnBook.Enabled = false;
        }
        else
        {
            divMobileNo.Visible = false;
            RfvMobNo.Enabled = false;
            divOtpDetails.Visible = false;
            btnBook.Enabled = true;
        }
        DivSlotOtherStab.Visible = false;
        BindAccessories();
        divResend.Visible = false;
        BindCount();
        Pathdesigns();
    }
    #endregion

    #region Bind SendPaymentLink
    public void SendPaymentLink(string MobNo, string BookingId)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Insert = new Notification()
                {
                    type = "U",
                    mobileNo = MobNo,
                    link = Session["SmsUrl"].ToString() + "upi.aspx?BookingId=" + BookingId
                };

                HttpResponseMessage response = client.PostAsJsonAsync("sendNotification", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "erroralert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public class Notification
    {
        public string type { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }
        public string link { get; set; }
    }
    #endregion
    #region Classes
    #region Parking Slot Class
    public class ParkingSlotClass
    {
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string parkingOwnerId { get; set; }
        public string typeOfVehicle { get; set; }
        public string noOfRows { get; set; }
        public string noOfColumns { get; set; }
        public string passageLeftAvailable { get; set; }
        public string passageRightAvailable { get; set; }
        public string typeOfParking { get; set; }
        public string laneNumber { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public List<ParkingSlotDetails> ParkingSlotDetails { get; set; }

    }
    public class ParkingSlotDetails
    {
        public string parkingSlotId { get; set; }
        public string parkingLotLineId { get; set; }
        public string laneNumber { get; set; }
        public string slotNumber { get; set; }
        public string rowId { get; set; }
        public string columnId { get; set; }
        public string slotType { get; set; }
        public string slotState { get; set; }
        public string isChargeUnitAvailable { get; set; }
        public string chargePinType { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string vehicleNumber { get; set; }
    }
    #endregion
    #region Booking Class
    public class Booking
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userId { get; set; }
        public string phoneNumber { get; set; }
        public string booking { get; set; }
        public string bookingDurationType { get; set; }
        public string accessories { get; set; }
        public string Dates { get; set; }
        public string bookingType { get; set; }
        public string subscriptionId { get; set; }
        public string taxId { get; set; }
        public string offerId { get; set; }
        public string paidAmount { get; set; }
        public string transactionId { get; set; }
        public string bankName { get; set; }
        public string bankReferenceNumber { get; set; }
        public string pinNo { get; set; }
        public string createdBy { get; set; }
        public string loginType { get; set; }
        public string bookingId { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string taxAmount { get; set; }
        public string totalAmount { get; set; }
        public string paymentStatus { get; set; }
        public string paymentType { get; set; }

        public List<vehicleHeaderDetails> vehicleHeaderDetails { get; set; }
        public List<userSlotDetails> userSlotDetails { get; set; }
        public List<extraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<extraFees> extraFees { get; set; }

    }
    public class vehicleHeaderDetails
    {
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
    }
    public class userSlotDetails
    {
        public string slotId { get; set; }
        public string vehicleType { get; set; }
    }
    public class extraFeaturesDetails
    {
        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraDetail { get; set; }
    }
    public class extraFees
    {
        public string timeslabId { get; set; }
        public string extraFeesDetails { get; set; }
        public string priceId { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }
    }
    #endregion
    #region Search bar  Class
    public class VehicleInoutClass
    {
        public string vehicleHeaderId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string slotId { get; set; }
        public string vehicleTypeName { get; set; }
        public string vehicleImageUrl { get; set; }
        public string extendAmount { get; set; }
        public string extendTax { get; set; }
        public string extendDayHour { get; set; }
        public string remainingAmount { get; set; }
        public string boookingAmount { get; set; }
        public string initialAmount { get; set; }
        public string pinNo { get; set; }
        public string vehicleName { get; set; }
        public List<slotDetails> slotDetails { get; set; }

    }
    public class slotDetails
    {
        public string userSlotId { get; set; }
        public string bookingIdType { get; set; }
        public string bookingPassId { get; set; }
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string slotNumber { get; set; }

    }
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

    #endregion
    #region Ticket Booking Class  
    public class GetBooking
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public string floorId { get; set; }
        public string userId { get; set; }
        public string phoneNumber { get; set; }
        public string booking { get; set; }
        public string bookingId { get; set; }
        public string bookingDurationType { get; set; }
        public string branchPhoneNumber { get; set; }
        public string accessories { get; set; }
        public string Dates { get; set; }
        public string bookingType { get; set; }
        public string subscriptionId { get; set; }
        public string taxId { get; set; }
        public string offerId { get; set; }
        public string paidAmount { get; set; }
        public string transactionId { get; set; }
        public string bankName { get; set; }
        public string bankReferenceNumber { get; set; }
        public string pinNo { get; set; }
        public string createdBy { get; set; }
        public string loginType { get; set; }
        public string parkingName { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public string branchName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string taxAmount { get; set; }
        public string totalAmount { get; set; }
        public string paymentTypeName { get; set; }
        public string paymentStatus { get; set; }
        public string bookingAmount { get; set; }
        public string bookingTax { get; set; }
        public string paymentType { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string pincode { get; set; }

        public List<vehicleDetails> vehicleDetails { get; set; }
        public List<GetuserSlotDetails> userSlotDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<GetextraFeaturesDetails> extraFeaturesDetail { get; set; }
        public List<GetextraFees> extraFeesDetails { get; set; }

    }
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
        public string slotNumber { get; set; }
        public string laneNumber { get; set; }
        public string vehiclePlaceHolderImageUrl { get; set; }

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
        public List<PassvehicleHeaderDetailss> vehicleDetails { get; set; }
        public List<PassvehicleHeaderDetails> vehicleHeaderDetails { get; set; }
        public List<PassuserSlotDetails> userSlotDetails { get; set; }
        public List<PassextraFeaturesDetails> extraFeaturesDetails { get; set; }
        public List<PassextraFeesDetails> extraFeesDetails { get; set; }

    }
    public class PassvehicleHeaderDetails
    {
        public string slotId { get; set; }
        public string vehicleType { get; set; }
        public string vehicleNumber { get; set; }
        public string inTime { get; set; }
        public string outTime { get; set; }
        public string vehicleStatus { get; set; }
        public string slotNumber { get; set; }
        public string laneNumber { get; set; }
    }
    public class PassuserSlotDetails
    {
        public string slotId { get; set; }
        public string slotNumber { get; set; }
        public string vehicleType { get; set; }
        public string laneNumber { get; set; }
    }
    public class PassextraFeaturesDetails
    {
        public string totalAmount { get; set; }
        public string floorFeaturesId { get; set; }
        public string count { get; set; }
        public string extraDetail { get; set; }
    }
    public class PassextraFeesDetails
    {

        public string timeslabId { get; set; }
        public string extraFeesDetails { get; set; }
        public string priceId { get; set; }
        public string count { get; set; }
        public string extraFee { get; set; }
    }
    public class PassvehicleHeaderDetailss
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
        public string slotNumber { get; set; }
        public string laneNumber { get; set; }
    }
    #endregion
    #endregion

    /// <summary>
    /// menu Access Rights 
    /// Created By Abhinaya K
    /// Created Date 02-AUG-2022
    /// </summary>
    #region menu Option Access Class
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
                        var Option = lst1.Where(x => x.optionName == "booking" && x.MenuOptionAccessActiveStatus == "A")
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
                            form.Visible = false;
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
    #endregion

    /// <summary>
    /// manual checkIn
    /// Created By Jayasuriya A
    /// Created Date April-2023
    /// </summary>
    #region manual checkIn
    protected void imgMannualPopupClose_Click(object sender, ImageClickEventArgs e)
    {
        divOverlayMannualBooking.Visible = false;
        divMannualBookingPopup.Visible = false;
    }

    protected void imgBtnCamera_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            BindMannualCheckInGrid();
        }
        catch (Exception Ex)
        {
            ShowErrorPopup(Ex);
        }
    }

    private void BindMannualCheckInGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            int statusCode;
            string response;
            string requestUrl = manualCheckInGridUrl + Session["branchId"].ToString();
            helper.APIGet<DataTable>(requestUrl, out dt, out statusCode, out response);

            if (statusCode == 1)
            {
                divOverlayMannualBooking.Visible = true;
                divMannualBookingPopup.Visible = true;

                gvMannualBooking.DataSource = dt;
                gvMannualBooking.DataBind();

                BindDropdownList();
            }
            else
            {
                divOverlayMannualBooking.Visible = false;
                divMannualBookingPopup.Visible = false;
                ShowInfoPopup(response);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void gvManualbtnCheckIn_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton imgBtn = sender as ImageButton;
            GridViewRow gvRow = imgBtn.NamingContainer as GridViewRow;
            string uniqueId = gvMannualBooking.DataKeys[gvRow.RowIndex]["UniqueId"].ToString();
            string vehicleNumber = gvMannualBooking.DataKeys[gvRow.RowIndex]["vehicleNumber"].ToString();

            DropDownList ddlVehicleType = gvRow.FindControl("ddlgvManualVehicleType") as DropDownList;

            ParkingRecord data = new ParkingRecord
            {
                parkingOwnerId =Convert.ToInt16(Session["parkingOwnerId"].ToString()),
                branchId = Convert.ToInt16(Session["branchId"].ToString()),
                vehicleNumber = vehicleNumber,
                vehicleType = Convert.ToInt16(ddlVehicleType.SelectedValue.Trim()),
                createdBy = Convert.ToInt16(Session["UserId"].ToString()),
                uniqueId = Convert.ToInt16(uniqueId)
            };

            var datass = JsonConvert.SerializeObject(data);
            int statusCode;
            string response;
            string bookingId;
            helper.APIpost<ParkingRecord>(manualCheckingPostUrl, data, out statusCode, out response,out bookingId);

            if (statusCode == 1)
            {
                ShowSuccessPopup(response);
                BindMannualCheckInGrid();
                Response.Redirect("Print.aspx?rt=Ob&bi=" + bookingId.ToString() + "", false);
            }
            else
            {
                ShowInfoPopup(response);
            }
        }
        catch (Exception ex)
        {
            ShowErrorPopup(ex);
        }
    }

    private void BindDropdownList()
    {
        try
        {
            DataTable dt = new DataTable();
            int statusCode;
            string response;

            helper.APIGet<DataTable>(manualVehicleTypeDdlUrl, out dt, out statusCode, out response);

            foreach (GridViewRow gvr in gvMannualBooking.Rows)
            {
                DropDownList ddlVehicleType = gvr.FindControl("ddlgvManualVehicleType") as DropDownList;
                ddlVehicleType.Items.Clear();
                ddlVehicleType.DataSource = dt;
                ddlVehicleType.DataValueField = "vehicleConfigId";
                ddlVehicleType.DataTextField = "vehicleName";
                ddlVehicleType.DataBind();
                ddlVehicleType.Items.Insert(0, new ListItem() { Text = "Select", Value = "0" });
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public class ParkingRecord
    {
        public int parkingOwnerId { get; set; }
        public int branchId { get; set; }
        public string vehicleNumber { get; set; }
        public int vehicleType { get; set; }
        public int createdBy { get; set; }
        public int uniqueId { get; set; }
    }

    #region Alerts
    public void ShowSuccessPopup(string Message)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert(`" + Message.Trim() + "`);", true);
    }
    public void ShowInfoPopup(string Message)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert(`" + Message.Trim() + "`);", true);
    }
    public void ShowErrorPopup(Exception Ex)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert(`" + Ex.Message.Trim() + "`);", true);
    }
    #endregion
    #endregion
}