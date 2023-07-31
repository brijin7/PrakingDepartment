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
using System.Text;

public partial class Booking_ParkingPassTransaction : System.Web.UI.Page
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
            BindVehicle();
            BindPaymentType();

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
                            gvVehicleType.DataSource = dt;
                            gvVehicleType.DataBind();
                            ViewState["data"] = dt;
                            Label lblvehicleConfigId = (Label)gvVehicleType.Items[0].FindControl("lblvehicleConfigId");
                            ViewState["vehicleType"] = lblvehicleConfigId.Text;
                            lblVehicleTypeId.Text = lblvehicleConfigId.Text;
                            Label divvehicle = gvVehicleType.Items[0].FindControl("lblvehicleName") as Label;
                            //divvehicle.Style.Add("color", "#fff");
                            divvehicle.Style.Add("color", "black");
                            HtmlControl divrower = gvVehicleType.Items[0].FindControl("divvehicles") as HtmlControl;
                            HtmlControl divcarname = gvVehicleType.Items[0].FindControl("divcarname") as HtmlControl;
                            divrower.Attributes["class"] = "carimage2";
                            divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";
                            divPassType.Visible = true;
                            divoffer.Visible = true;
                            lnkRemove.Visible = false;
                            Formbackground.Style.Add("background-image", "../images/Backgroundcar.png");
                            Formbackground.Style.Add("background-repeat", "no-repeat");
                            Formbackground.Style.Add("opacity", "0.2");
                            Formbackground.Style.Add("height", "300px");
                            Formbackground.Style.Add("background-position", "center");
                            Formbackground.Style.Add("background-size", "50%");
                            Formbackground.Style.Add("margin-top", "0rem");
                        }
                        else
                        {

                        }
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
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Clear Function
    public void Clear()
    {
        lblVehicleType.Text = "";
        lblvehicleamt.Text = "";
        lbltaxamt.Text = "";
        lbltotalamt.Text = "";
        lbltotalamt.Style.Add("text-decoration", "none");
        lblOffertotal.Text = "";
        ddlPassType.ClearSelection();
        txtMobileNo.Text = "";
        divpasssummary.Visible = false;
        ViewState["OfferId"] = "";
        ViewState["OfferType"] = "";
        ViewState["OfferValue"] = "";

    }
    #endregion
    #region Bind Pass Type
    public void BindPassType()
    {
        ViewState["Flag"] = "0";
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
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
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
    public void BindPaymentType()
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
                            ddlPaymentType.SelectedValue = dt.Rows[0]["configId"].ToString();
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("payLater"));
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Pass"));
                            ddlPaymentType.Items.Remove(ddlPaymentType.Items.FindByText("Pay At CheckOut"));
                        }
                        else
                        {
                            ddlPaymentType.DataBind();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlPaymentType.Items.Clear();
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
    protected void lblVehicleTypes_Click(object sender, EventArgs e)
    {
        Clear();
        DivPassTicket.Visible = false;
        divDownload.Visible = false;
        ddlPassCategory.ClearSelection();
        LinkButton lnkbtn = sender as LinkButton;
        DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;
        Label lblvehicleConfigId = (Label)gvrow.FindControl("lblvehicleConfigId");
        ViewState["vehicleType"] = lblvehicleConfigId.Text;
        lblVehicleTypeId.Text = lblvehicleConfigId.Text;
        DataTable dt = (DataTable)ViewState["data"];
        gvVehicleType.DataSource = dt;
        gvVehicleType.DataBind();
        foreach (DataListItem item in gvVehicleType.Items)
        {
            Label lblvehicleConfigIds = item.FindControl("lblvehicleConfigId") as Label;
            if (lblvehicleConfigId.Text == lblvehicleConfigIds.Text)
            {
                Label divvehicle = item.FindControl("lblvehicleName") as Label;
                //divvehicle.Style.Add("color", "#fff");
                divvehicle.Style.Add("color", "black");
                HtmlControl divrower = item.FindControl("divvehicles") as HtmlControl;
                divrower.Attributes["class"] = "carimage2";
                HtmlControl divcarname = item.FindControl("divcarname") as HtmlControl;
                divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname2";
            }
            else
            {
                Label divvehicle = item.FindControl("lblvehicleName") as Label;
                divvehicle.Style.Add("color", "#000");
                HtmlControl divrower = item.FindControl("divvehicles") as HtmlControl;
                divrower.Attributes["class"] = "carimage";
                HtmlControl divcarname = item.FindControl("divcarname") as HtmlControl;
                divcarname.Attributes["class"] = "col-xs-9 col-sm-9 col-md-9 col-lg-9 divcarname";
            }
            Label lblvehicleName = (Label)gvrow.FindControl("lblvehicleName");
            if (lblvehicleName.Text == "Car")
            {
                Formbackground.Style.Add("background-image", "../images/Backgroundcar.png");
                Formbackground.Style.Add("background-size", "50%");
            }
            else if (lblvehicleName.Text == "Bike")
            {
                Formbackground.Style.Add("background-image", "../images/Backgroundbike.png");
                Formbackground.Style.Add("background-size", "40%");

            }
            Formbackground.Style.Add("background-repeat", "no-repeat");
            Formbackground.Style.Add("opacity", "0.2");
            Formbackground.Style.Add("height", "300px");
            Formbackground.Style.Add("background-position", "center");
            Formbackground.Style.Add("margin-top", "0rem");
        }
        divPassType.Visible = true;
        divoffer.Visible = true;
        lnkRemove.Visible = false;
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
                      + "parkingPassConfig?branchId=" + Session["branchId"].ToString() + "&vehicleType=" + ViewState["vehicleType"] +
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
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                            Clear();
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        Clear();
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
        ddlPassType.ClearSelection();
        DivPassTicket.Visible = false;
        divDownload.Visible = false;
        divpasssummary.Visible = false;
        BindPassType();
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
                    Session["branchId"].ToString(), ViewState["PassId"].ToString(), txtMobileNo.Text, ViewState["vehicleType"].ToString(),
                    Convert.ToDecimal(ViewState["TotalAmount"].ToString()).ToString(),
                     ViewState["TaxId"].ToString(), "P", ddlPaymentType.SelectedValue, "A",
                     Session["UserId"].ToString().Trim(), null, null)
                };
                if (ViewState["Flag"].ToString() == "1")
                {
                    if (ViewState["OfferId"].ToString() != "0" && ViewState["OfferId"].ToString() != "")
                    {
                        Insert.passTransactionDetails = GetpassTransactionDetails(Session["parkingOwnerId"].ToString(),
                        Session["branchId"].ToString(), ViewState["PassId"].ToString(), txtMobileNo.Text, ViewState["vehicleType"].ToString(),
                         ViewState["OfferAmount"].ToString(), ViewState["TaxId"].ToString(), "P", ddlPaymentType.SelectedValue, "A",
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


                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        Clear();
                        ddlPassCategory.ClearSelection();
                        ddlPassType.Items.Clear();
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
        InsertParkingPass();
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
    #region Offer Click

    protected void lblofferHeading_Click(object sender, EventArgs e)
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
                      + "offerMaster?activeStatus=A&branchId=" + Session["branchId"].ToString() + "&Amount=" + ViewState["TotalAmount"] + "";

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
                            ViewState["Flag"] = "1";
                        }
                        else
                        {


                        }

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "hide()", true);
                        modal.Visible = false;
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
    #region Remove Offer Click
    protected void lnkRemove_Click(object sender, EventArgs e)
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
    #endregion
    #region Offer Click Apply
    protected void Lnkoffer_Click(object sender, EventArgs e)
    {
        modal.Visible = true;
        divoffer.Visible = false;
        lnkRemove.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "show()", true);
        GetOfferDetails();
    }
    #endregion
    #region Offer Close Click
    protected void linkoffclose_Click(object sender, EventArgs e)
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
    #endregion
    #region Pass Details
    protected void ddlPassType_SelectedIndexChanged(object sender, EventArgs e)
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
                        var Option = lst1.Where(x => x.optionName == "passTransaction" && x.MenuOptionAccessActiveStatus == "A")
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
                            divpasssummary.Visible = false;
                            divPassType.Visible = false;
                            gvVehicleType.Visible = false;

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

    //Pass Ticket
    #region QR Code Generate 
    public void GetQRcode(string parkingPassTransId)
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
                imgEmpPhotoPrev.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                string result = Convert.ToBase64String(byteImage, 0, byteImage.Length);

            }
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
                            List<GetPassTicket> lst = JsonConvert.DeserializeObject<List<GetPassTicket>>(ResponseMsg);
                            var firstItem = lst.ElementAt(0);
                            var lst1 = firstItem.passType.ToList();
                            DataTable passtype = ConvertToDataTable(lst1);

                            GetQRcode(parkingPassTransId);
                            lblPassParkinngName.Text = dt.Rows[0]["parkingName"].ToString();
                            lblPassBranchName.Text = dt.Rows[0]["branchName"].ToString();
                            lblTicketPassType.Text = passtype.Rows[0]["passType"].ToString();
                            lblPassMobileNo.Text = dt.Rows[0]["phoneNumber"].ToString();
                            lblPassStartDate.Text = dt.Rows[0]["validStartDate"].ToString();
                            lblPassEndDate.Text = dt.Rows[0]["validEndDate"].ToString();
                            lblPassMode.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblUserPassType.Text = passtype.Rows[0]["passCategory"].ToString() == "V" ? "VIP" : "Normal";
                            lblPassVehicleType.Text = dt.Rows[0]["vehicleName"].ToString();
                            lblPassId.Text = dt.Rows[0]["parkingPassTransId"].ToString();
                            if (lblPassMode.Text == "VIP")
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
        finally
        {

        }
    }

    #endregion
    #region Pass Ticket Class  
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
                var Insert = new sendNotification()
                {
                    type = "P",
                    mobileNo = lblPassMobileNo.Text,
                    link = hfImageUrl.Value
                };
                string sUrl = Session["BaseUrl"].ToString().Trim()
                + "sendNotification?type=P&mobileNo=" + lblPassMobileNo.Text + "&link=" + link.Trim() + "";
                //+lblPassMobileNo.Text +
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
    public class sendNotification
    {
        public string type { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }
        public string link { get; set; }
    }

    #endregion



}