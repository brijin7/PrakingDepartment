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
using System.Net;
using System.Text;
using System.Reflection;

public partial class Master_Floor_FloorSetup : System.Web.UI.Page
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
            ViewState["BlockCount"] = "0";
            ViewState["FloorCount"] = "0";
            BindDdlBlock();
            if (ViewState["BlockCount"].ToString() == "1" && ViewState["FloorCount"].ToString() == "1")
            {
                divBlockFloor.Visible = false;
            }
            else
            {
                divBlockFloor.Visible = true;
            }
        }

        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }

    }
    #endregion
    #region Block
    public void BindDdlBlock()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlBlock.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "blockMaster?branchId=" + Session["branchId"].ToString()
                            + "&activeStatus=A&approvalStatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtBlock = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlBlock.DataSource = dtBlock;
                        ddlBlock.DataValueField = "blockId";
                        ddlBlock.DataTextField = "blockName";
                        ddlBlock.DataBind();
                        ddlBlock.SelectedValue = dtBlock.Rows[0]["blockId"].ToString();
                        if (dtBlock.Rows.Count == 1)
                        {
                            ViewState["BlockCount"] = "1";
                        }
                        BindDdlFloorName();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlBlock.Items.Insert(0, new ListItem("Select Block", "0"));
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
    protected void ddlBlock_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdlFloorName();
    }
    #endregion
    #region Floor
    public void BindDdlFloorName()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlFloorName.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                         + "floorMaster?activeStatus=A&blockId=";
                sUrl += ddlBlock.SelectedValue;
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
                            ddlFloorName.DataSource = dt;
                            ddlFloorName.DataValueField = "floorId";
                            ddlFloorName.DataTextField = "floorName";
                            ddlFloorName.DataBind();
                            ddlFloorName.SelectedValue = dt.Rows[0]["floorId"].ToString();
                            if (dt.Rows.Count == 1)
                            {
                                ViewState["FloorCount"] = "1";
                            }

                            ViewState["floorId"] = ddlFloorName.SelectedValue;
                            FloorVehicle();
                        }
                    }
                    else
                    {
                        divFloorSetup.Visible = false;
                        divAddMasters.Visible = false;
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlFloorName.Items.Insert(0, new ListItem("Select Floor", "0"));
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
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["floorId"] = ddlFloorName.SelectedValue;
        FloorVehicle();
    }

    #endregion

    #region btnFloorvehicleMaster   
    protected void btnFloorvehicleMaster_Click(object sender, EventArgs e)
    {
        FloorVehicle();
    }
    public void FloorVehicle()
    {
        divAddMasters.Visible = true;
        divGvFV.Visible = true;
        divFormFV.Visible = false;
        FVBindVehicle();
        //FVBindVehicleSize();
        BindFloorVehicle();
        divFloorSetup.Visible = true;
        divFloorVehicle.Visible = true;
        divFloorPrice.Visible = false;
        divFloorSetup.Style.Add("border-style", "groove");
        divFloorSetup.Style.Add("border-width", "1px");
        divFloorSetup.Style.Add("border-color", "#ccffe7");
        divFloorSetup.Style.Add("box-shadow", "rgba(17, 17, 26, 0.1) " +
           "0px 4px 16px, rgba(17, 17, 26, 0.1) " +
           "0px 8px 24px, rgba(17, 17, 26, 0.1) " +
           "0px 16px 56px");
    }
    #endregion

    #region btnPriceMaster
    protected void btnPriceMaster_Click(object sender, EventArgs e)
    {
        divGvFP.Visible = true;
        divFormFP.Visible = false;

        FPBindVehicle();
        BindAccessoriesType();
        BindTaxName();

        divFloorSetup.Visible = true;
        divFloorVehicle.Visible = false;
        divFloorPrice.Visible = true;

        divFloorSetup.Style.Add("border-style", "groove");
        divFloorSetup.Style.Add("border-width", "1px");
        divFloorSetup.Style.Add("border-color", "#85ffff");
        divFloorSetup.Style.Add("box-shadow", "rgba(17, 17, 26, 0.1) " +
           "0px 4px 16px, rgba(17, 17, 26, 0.1) " +
           "0px 8px 24px, rgba(17, 17, 26, 0.1) " +
           "0px 16px 56px");

        rbtnGVTypeFP.SelectedValue = "V";
        divVehicleGVFP.Visible = true;
        divAccessoriesGVFP.Visible = false;
        BindVehiclePriceMaster();
    }
    #endregion

    #region FV

    #region Add Click    
    protected void btnAddFV_Click(object sender, EventArgs e)
    {
        FVADD();
    }
    public void FVADD()
    {

        divGvFV.Visible = false;
        divFormFV.Visible = true;
        spAddorEditFV.InnerText = "Add ";
        ddlvehicletypeFV.Enabled = true;
    }
    #endregion

    #region Cancel Click
    protected void btnCancelFV_Click(object sender, EventArgs e)
    {
        FVCancel();
    }
    public void FVCancel()
    {
        divGvFV.Visible = true;
        divFormFV.Visible = false;
        ddlvehicletypeFV.ClearSelection();
        txtCapacityFV.Text = "";
        ddlvehicleSizeFV.ClearSelection();
        txtRulesFV.Text = "";
        FVbtnSubmitText();

    }
    #endregion

    #region Bind Vehicle /FVBindVehicleSize
    public void FVBindVehicle()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "vehicleConfigMaster?activeStatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {

                            ddlvehicletypeFV.DataSource = dt;
                            ddlvehicletypeFV.DataValueField = "vehicleConfigId";
                            ddlvehicletypeFV.DataTextField = "vehicleName";
                            ddlvehicletypeFV.DataBind();

                        }
                        else
                        {

                            ddlvehicletypeFV.DataBind();
                        }
                        ddlvehicletypeFV.Items.Insert(0, new ListItem("Select", "0"));
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
    protected void ddlvehicletypeFV_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlvehicletypeFV.SelectedIndex !=0 )
        {
            FVBindVehicleSize();
        }
        
    }
    public void FVBindVehicleSize()
    {
        try
        {
            ddlvehicleSizeFV.Items.Clear();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "vehicleSizeConfigMaster?activestatus=A&vehicleConfigId="+ddlvehicletypeFV.SelectedValue+"";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        dt.Columns.Add(new DataColumn("model", System.Type.GetType("System.String"), "modelName + '~' + length + '*' + height"));
                        if (dt.Rows.Count > 0)
                        {

                            ddlvehicleSizeFV.DataSource = dt;
                            ddlvehicleSizeFV.DataValueField = "vehicleSizeConfigId";
                            ddlvehicleSizeFV.DataTextField = "model";
                            ddlvehicleSizeFV.DataBind();

                        }
                        else
                        {

                            ddlvehicleSizeFV.DataBind();
                        }
                        ddlvehicleSizeFV.Items.Insert(0, new ListItem("Select~len*ht ", "0"));
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

    #region Insert Function
    public void InsertFloorVehicle()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));
                var Insert = new FloorVehicleClass()
                {
                    floorId = ViewState["floorId"].ToString().Trim(),
                    vehicleType = ddlvehicletypeFV.SelectedValue,
                    capacity = txtCapacityFV.Text,
                    vehicleSizeConfigId = ddlvehicleSizeFV.SelectedValue,
                    rules = txtRulesFV.Text,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PostAsJsonAsync
                    ("floorVehicleMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindFloorVehicle();
                        FVCancel();
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

    #region Update Function
    public void UpdateFloorVehicle()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));
                var Update = new FloorVehicleClass()
                {
                    floorVehicleId = hffloorVehicleIdFV.Value,
                    floorId = hfflooridFV.Value,
                    vehicleType = ddlvehicletypeFV.SelectedValue,
                    capacity = txtCapacityFV.Text,
                    vehicleSizeConfigId = ddlvehicleSizeFV.SelectedValue,
                    rules = txtRulesFV.Text,
                    updatedBy = Session["UserId"].ToString()

                };
                HttpResponseMessage response = client.PutAsJsonAsync
                    ("floorVehicleMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        FVbtnSubmitText();
                        BindFloorVehicle();
                        FVCancel();
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
    protected void btnSubmitFV_Click(object sender, EventArgs e)
    {
        if (btnSubmitFV.Text == "Submit")
        {
            InsertFloorVehicle();
        }
        else
        {
            UpdateFloorVehicle();

        }
    }
    #endregion

    #region Update Text
    public void FVbtnUpdateText()
    {
        btnSubmitFV.Text = "Update";
    }
    #endregion

    #region Submit Text
    public void FVbtnSubmitText()
    {
        btnSubmitFV.Text = "Submit";
    }
    #endregion   

    #region Bind Floor Vehicle Master
    public void BindFloorVehicle()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                        + "floorVehicleMaster?floorId=";
                sUrl += ViewState["floorId"].ToString().Trim();
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            gvfloorvehiclemaster.DataSource = dt;
                            gvfloorvehiclemaster.DataBind();
                            divGvFV.Visible = true;
                        }
                        else
                        {
                            gvfloorvehiclemaster.DataSource = null;
                            gvfloorvehiclemaster.DataBind();

                        }
                    }
                    else
                    {
                        gvfloorvehiclemaster.DataSource = null;
                        gvfloorvehiclemaster.DataBind();
                        FVADD();
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
    #endregion

    #region Edit Click 
    protected void LnkEditFV_Click(object sender, EventArgs e)
    {
        try
        {
            FVbtnUpdateText();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvfloorVehicleId = (Label)gvrow.FindControl("lblgvfloorVehicleIdFV");
            Label lblgvvehicleType = (Label)gvrow.FindControl("lblgvvehicleTypeFV");
            Label lblgvcapacity = (Label)gvrow.FindControl("lblgvcapacityFV");
            Label lblgvvehicleSize = (Label)gvrow.FindControl("lblgvvehicleSize");

            Label lblgvrules = (Label)gvrow.FindControl("lblgvrulesFV");
            Label lblgvfloorId = (Label)gvrow.FindControl("lblgvfloorIdFV");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactiveFV");
            string sgvfloorvehiclemaster = gvfloorvehiclemaster.DataKeys
                [gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.ToString() == "Active" ? "A" : "D";
            ddlvehicletypeFV.SelectedValue = lblgvvehicleType.Text;
            txtCapacityFV.Text = lblgvcapacity.Text;
            if (ddlvehicletypeFV.SelectedIndex != 0)
            {
                FVBindVehicleSize();
            }
            ddlvehicleSizeFV.SelectedValue = lblgvvehicleSize.Text;
            txtRulesFV.Text = lblgvrules.Text;
            hffloorVehicleIdFV.Value = lblgvfloorVehicleId.Text;
            hfflooridFV.Value = lblgvfloorId.Text;
            FVADD();
            ddlvehicletypeFV.Enabled = false;
            spAddorEditFV.InnerText = "Edit ";
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }

    }
    #endregion

    #region Delete Click
    protected void lnkActiveOrInactiveFV_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sFloorVehicleId = gvfloorvehiclemaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactiveFV");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "floorVehicleMaster?floorVehicleId=" + sFloorVehicleId
                            + "&activeStatus="
                            + sActiveStatus;

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindFloorVehicle();
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

    #region Floor Vehicle Class
    public class FloorVehicleClass
    {
        public String parkingOwnerId { get; set; }
        public String activeStatus { get; set; }
        public String floorId { get; set; }
        public String floorVehicleId { get; set; }
        public String vehicleType { get; set; }
        public String capacity { get; set; }
        public String vehicleSizeConfigId { get; set; }
        public String rules { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }

    }
    #endregion


    #endregion

    #region FP   
    #region ADD Click
    protected void btnAddFP_Click(object sender, EventArgs e)
    {
        FPADDMethod();
    }
    public void FPADDMethod()
    {
        ViewState["SlabFlag"] = "Add";
        BtnAddPrice.Enabled = true;
        FPCancel();
        FPADD();
        NrmlPriceDetails.DataSource = null;
        NrmlPriceDetails.DataBind();
        divNormalFP.Visible = false;
        divVipFP.Visible = false;
        VipPriceDetails.DataSource = null;
        VipPriceDetails.DataBind();
        if (rbtnGVTypeFP.SelectedItem.Text == "Vehicle")
        {
            divddlVehicle.Visible = true;
            ////divddlaccessories.Visible = false;
            divddlaccessoriesType.Visible = false;
            //DivFPVNDetails.Visible = true;
            DivFPVNAccDetails.Visible = false;
        }
        else
        {
            ChkUpdtPrice.Visible = false;
            //divddlaccessories.Visible = true;
            divddlaccessoriesType.Visible = true;
            DivFPVNDetails.Visible = false;
            DivFPVNAccDetails.Visible = true;
            divtimetype.Visible = false;
            divddlVehicle.Visible = false;
        }
    }
    public void FPADD()
    {

        divGvFP.Visible = false;
        divFormFP.Visible = true;
        //divddlaccessories.Visible = false;
        divddlaccessoriesType.Visible = false;
        divddlVehicle.Visible = false;
        spAddorEditFP.InnerText = "Add ";
        VehicleAccessories();

        if (rbtnGVTypeFP.SelectedItem.Text == "Vehicle")
        {
            rbtnTypeFP.SelectedValue = "V";
            divddlVehicle.Visible = true;
            //divddlaccessories.Visible = false;
            divddlaccessoriesType.Visible = false;
            if (ViewState["SlabFlag"].ToString() == "Add")
            {
                divtimetype.Visible = false;
                ChkUpdtPrice.Visible = false;
            }
            else
            {
                divtimetype.Visible = true;
                ChkUpdtPrice.Visible = true;
            }

            //DivFPVNDetails.Visible = true;
            DivFPVNAccDetails.Visible = false;
            NrmlPriceDetails.Columns[5].Visible = true;
            NrmlPriceDetails.Columns[6].Visible = false;
            NrmlPriceDetails.Columns[7].Visible = false;
            VipPriceDetails.Columns[5].Visible = true;
            VipPriceDetails.Columns[6].Visible = false;
            VipPriceDetails.Columns[7].Visible = false;

        }
        else
        {
            btnSubmitFP.Enabled = true;
            rbtnTypeFP.SelectedValue = "A";
            ChkUpdtPrice.Visible = false;
            //divddlaccessories.Visible = true;
            divddlaccessoriesType.Visible = true;
            DivFPVNDetails.Visible = false;
            DivFPVNAccDetails.Visible = true;
            divtimetype.Visible = false;
            divddlVehicle.Visible = false;
        }
        BtnAddPrice.Enabled = true;
        ddluserMode.Enabled = true;
        txtFromFP.Enabled = true;
        txtToFP.Enabled = true;
        txttotalamountFP.Enabled = true;
        txtAmountFP.Enabled = true;
        txtTaxFP.Enabled = true;
    }
    #endregion   
    #region Bind ddl Vehicle
    public void FPBindVehicle()
    {
        try
        {
            ddlVehicleFP.Items.Clear();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                          + "floorVehicleMaster?floorId=" + ViewState["floorId"].ToString() + "&activeStatus=A";
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
                            ddlVehicleFP.DataSource = dt;
                            ddlVehicleFP.DataValueField = "vehicleType";
                            ddlVehicleFP.DataTextField = "vehicleName";
                            ddlVehicleFP.DataBind();
                        }
                        else
                        {
                            ddlVehicleFP.DataBind();
                        }

                    }
                    else
                    {
                        ddlVehicleFP.DataBind();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlVehicleFP.Items.Insert(0, new ListItem("Select", "0"));
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
    #region Bind Accessories Type
    public void BindAccessoriesType()
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
                          + "configType?type=R";
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
                            ddlAccessoriesType.DataSource = dt;
                            ddlAccessoriesType.DataValueField = "configTypeId";
                            ddlAccessoriesType.DataTextField = "typeName";
                            ddlAccessoriesType.DataBind();
                        }
                        else
                        {
                            ddlAccessoriesType.DataBind();
                        }
                        ddlAccessoriesType.Items.Insert(0, new ListItem("Select", "0"));
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
    #region AccessoriesType Change 
    protected void ddlAccessoriesType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAccessories();
    }
    #endregion
    #region Bind Accessories Master
    public void BindAccessories()
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
                          + "configMaster?activestatus=A&configTypeId= " + ddlAccessoriesType.SelectedValue + "";
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
                            ddlaccessoriesFP.DataSource = dt;
                            ddlaccessoriesFP.DataValueField = "configId";
                            ddlaccessoriesFP.DataTextField = "configName";
                            ddlaccessoriesFP.DataBind();
                        }
                        else
                        {
                            ddlaccessoriesFP.DataBind();
                        }
                        ddlaccessoriesFP.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlaccessoriesFP.Items.Clear();
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
    #region Bind Tax 
    public void BindTaxName()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "taxMaster?activeStatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        dt.Columns.Add(new DataColumn("tax", System.Type.GetType("System.String"), "taxName + '~' + taxPercentage"));
                        if (dt.Rows.Count > 0)
                        {
                            ddltaxFP.DataSource = dt;
                            ddltaxFP.DataValueField = "taxId";
                            ddltaxFP.DataTextField = "tax";
                            ddltaxFP.DataBind();
                        }
                        else
                        {
                            ddltaxFP.DataBind();
                        }
                        ddltaxFP.Items.Insert(0, new ListItem("Select", "0"));
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
    #region Submit Click
    protected void btnSubmitFP_Click(object sender, EventArgs e)
    {
        lblvcvalueFP.Text = "";
        if (Convert.ToInt32(ddlVehicleFP.SelectedValue) > 0)
        {
            lblvcvalueFP.Text = ddlVehicleFP.SelectedValue.Trim();
        }
        else if (Convert.ToInt32(ddlaccessoriesFP.SelectedValue) > 0)
        {
            lblvcvalueFP.Text = ddlaccessoriesFP.SelectedValue.Trim();
        }
        if (rbtnTypeFP.SelectedValue == "V")
        {
            if (NrmlPriceDetails.Rows.Count == 0 && VipPriceDetails.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "infoalert('Add Slab Details');", true);
                return;
            }
        }
        if (btnSubmitFP.Text == "Submit")
        {
            InsertPrice();
        }
        else
        {
            if (rbtnTypeFP.SelectedValue == "V")
            {
                UpdatePrice();
            }
            else
            {
                UpdateAcceessories();
            }
        }
    }
    #endregion

    #region Insert 
    public void InsertPrice()
    {
        try
        {
            string timeType = string.Empty;
            string TotalAmount = string.Empty;
            string UserMode = string.Empty;
            if (rbtnTypeFP.SelectedValue == "V")
            {
                if (NrmlPriceDetails.Rows.Count > 0 && VipPriceDetails.Rows.Count > 0)
                {
                    UserMode = "N,V";
                }
                else
                {
                    UserMode = ddluserMode.SelectedValue;

                }
                timeType = "TS";
            }
            else
            {
                UserMode = "";
                timeType = "";
                TotalAmount = txtNAcctotalamt.Text;
                txtGraceTime.Text = "0";
            }


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new PriceMasterClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    floorId = ViewState["floorId"].ToString(),
                    idType = rbtnTypeFP.SelectedValue,
                    vehicle_accessories = lblvcvalueFP.Text,
                    graceTime = txtGraceTime.Text == "" ? "0" : txtGraceTime.Text,
                    createdBy = Session["UserId"].ToString(),
                    priceDetails = GetpriceDetails(timeType, ddltaxFP.SelectedValue, UserMode.Trim(), "A")

                };

                HttpResponseMessage response = client.PostAsJsonAsync("priceMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindVehiclePriceMaster();
                        BindAccessoriesPriceMaster();
                        FPCancel();
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

    public List<priceDetails> GetpriceDetails(string timeType, string taxId, string userMode, string activeStatus)
    {
        string[] userModes;
        string[] taxIds;
        string totalAmount = string.Empty;
        string from = string.Empty;
        string to = string.Empty;
        userModes = userMode.Split(',');
        taxIds = taxId.Split(',');
        List<timeSlabDetails> timeslab = new List<timeSlabDetails>();
        List<timeSlabDetails> Viptimeslab = new List<timeSlabDetails>();
        List<priceDetails> lst = new List<priceDetails>();
        if (rbtnTypeFP.SelectedValue == "V")
        {
            if (userModes.Contains("N"))
            {
                DataTable dt = (DataTable)ViewState["NrmlRow"];
                timeslab = (from DataRow dr in dt.Rows
                            select new timeSlabDetails()
                            {
                                userMode = dr["userMode"].ToString(),
                                totalAmount = dr["totalAmount"].ToString(),
                                toDate = dr["To"].ToString(),
                                fromDate = dr["From"].ToString(),
                                activeStatus = "A"
                            }).ToList();
            }
            if (userModes.Contains("V"))
            {
                DataTable dtVip = (DataTable)ViewState["VipRow"];
                Viptimeslab = (from DataRow dr in dtVip.Rows
                               select new timeSlabDetails()
                               {
                                   userMode = dr["userMode"].ToString(),
                                   totalAmount = dr["totalAmount"].ToString(),
                                   toDate = dr["To"].ToString(),
                                   fromDate = dr["From"].ToString(),
                                   activeStatus = "A"
                               }).ToList();
            }
            List<timeSlabDetails> times = new List<timeSlabDetails>();
            times = timeslab.Union(Viptimeslab).ToList();


            for (int i = 0; i < taxIds.Count(); i++)
            {
                lst.AddRange(new List<priceDetails>
            {
                new priceDetails { timeType=timeType, taxId=taxId,
                    activeStatus =activeStatus, timeSlabDetails=times}
            });

            }

        }
        else
        {

            for (int i = 0; i < taxIds.Count(); i++)
            {
                lst.AddRange(new List<priceDetails>
            {
                new priceDetails { timeType=timeType, taxId=taxId,
                    activeStatus =activeStatus,
                    timeSlabDetails =GettimeSlabDetails(txtNAcctotalamt.Text,"0","1","A")}
            });

            }
        }
        return lst;

    }

    public List<priceDetails> GetpriceDetails(string timeType, string taxId, string PriceId, string userMode, string activeStatus)
    {
        string[] userModes;
        string[] taxIds;
        string totalAmount = string.Empty;
        string from = string.Empty;
        string to = string.Empty;
        userModes = userMode.Split(',');
        taxIds = taxId.Split(',');
        //List<timeSlabDetails> timeslab = new List<timeSlabDetails>();
        //List<timeSlabDetails> Viptimeslab = new List<timeSlabDetails>();
        //if (userModes.Contains("N"))
        //{
        //    DataTable dt = (DataTable)ViewState["NrmlRow"];
        //    timeslab = (from DataRow dr in dt.Rows
        //                select new timeSlabDetails()
        //                {
        //                    userMode = dr["userMode"].ToString(),
        //                    totalAmount = dr["totalAmount"].ToString(),
        //                    toDate = dr["To"].ToString(),
        //                    fromDate = dr["From"].ToString(),
        //                    activeStatus = "A"
        //                }).ToList();
        //}
        //if (userModes.Contains("V"))
        //{
        //    DataTable dtVip = (DataTable)ViewState["VipRow"];
        //    Viptimeslab = (from DataRow dr in dtVip.Rows
        //                   select new timeSlabDetails()
        //                   {
        //                       userMode = dr["userMode"].ToString(),
        //                       totalAmount = dr["totalAmount"].ToString(),
        //                       toDate = dr["To"].ToString(),
        //                       fromDate = dr["From"].ToString(),
        //                       activeStatus = "A"
        //                   }).ToList();
        //}
        //List<timeSlabDetails> times = new List<timeSlabDetails>();
        //times = timeslab.Union(Viptimeslab).ToList();

        List<priceDetails> lst = new List<priceDetails>();
        for (int i = 0; i < taxIds.Count(); i++)
        {
            lst.AddRange(new List<priceDetails>
            {
                new priceDetails { timeType=timeType, taxId=taxId,priceId=PriceId,
                    activeStatus =activeStatus}
            });

        }


        return lst;

    }
    public static List<timeSlabDetails> GettimeSlabDetails(string totalAmount, string from, string to, string activeStatus)
    {
        string[] totalAmounts;
        string[] fromT;
        string[] toT;
        totalAmounts = totalAmount.Split(',');
        fromT = from.Split(',');
        toT = to.Split(',');
        List<timeSlabDetails> lst = new List<timeSlabDetails>();
        for (int i = 0; i < totalAmounts.Count(); i++)
        {
            lst.AddRange(new List<timeSlabDetails>
            {
                new timeSlabDetails { totalAmount=totalAmounts[i] , fromDate=fromT[i] ,toDate=toT[i] ,activeStatus=activeStatus}

            });
        }

        return lst;

    }


    #endregion

    #region Update 
    public void UpdatePrice()
    {
        try
        {
            string UserMode = string.Empty;
            if (NrmlPriceDetails.Rows.Count > 0 && VipPriceDetails.Rows.Count > 0)
            {
                UserMode = "N,V";
            }
            else
            {
                UserMode = ddluserMode.SelectedValue;
            }

            string timeType = "TS";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new PriceMasterClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    floorId = ViewState["floorId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    idType = rbtnTypeFP.SelectedValue,
                    vehicle_accessories = lblvcvalueFP.Text,
                    graceTime = txtGraceTime.Text == "" ? "0" : txtGraceTime.Text,
                    updatedBy = Session["UserId"].ToString(),
                    priceDetails = GetpriceDetails(timeType, ddltaxFP.SelectedValue, ViewState["priceId"].ToString(),
                    UserMode.Trim(), ViewState["gvactiveStatusFPV"].ToString())
                };

                HttpResponseMessage response = client.PutAsJsonAsync("priceMaster", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        FPbtnSubmitText();
                        BindVehiclePriceMaster();
                        BindAccessoriesPriceMaster();
                        FPCancel();

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

    public void UpdateAcceessories()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new PriceMasterClass()
                {
                    totalAmount = txtNAcctotalamt.Text,
                    taxId = ddltaxFP.SelectedValue,
                    priceId = HfpriceId.Value,
                    updatedBy = Session["UserId"].ToString(),

                };

                HttpResponseMessage response = client.PutAsJsonAsync("priceMasterAcc", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        FPbtnSubmitText();
                        BindVehiclePriceMaster();
                        BindAccessoriesPriceMaster();
                        FPCancel();
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

    #region Update Text
    public void FPbtnUpdateText()
    {
        btnSubmitFP.Text = "Update";
    }
    #endregion

    #region Submit Text
    public void FPbtnSubmitText()
    {
        btnSubmitFP.Text = "Submit";
    }
    #endregion

    #region  Amount Cal
    protected void ddltaxFP_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnTypeFP.SelectedValue == "V")
        {
            DivFPVNDetails.Visible = true;
            if (btnSubmitFP.Text == "Update")
            {
                divtimetype.Visible = false;
            }

        }
        AmountCalcFP();
        NormalAccAmountCalcFP();
    }

    protected void txttotalamountFP_TextChanged(object sender, EventArgs e)
    {
        AmountCalcFP();
    }

    public void AmountCalcFP()
    {
        try
        {
            if (txttotalamountFP.Text != null && txttotalamountFP.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txttotalamountFP.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtAmountFP.Text = totalAmount;
                txtTaxFP.Text = Tax.ToString("0.0");
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void txtNAcctotalamt_TextChanged(object sender, EventArgs e)
    {
        NormalAccAmountCalcFP();
    }
    public void NormalAccAmountCalcFP()
    {
        try
        {
            if (txtNAcctotalamt.Text != null && txtNAcctotalamt.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtNAcctotalamt.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtNAccAmt.Text = totalAmount;
                txtNAcctaxamt.Text = Tax.ToString("0.0");
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion

    #region Cancel Click
    protected void btnCancelFP_Click(object sender, EventArgs e)
    {
        FPCancel();
        BindVehiclePriceMaster();
    }
    public void FPCancel()
    {
        ddluserMode.Enabled = true;
        ChkUpdtPrice.Checked = false;
        divGvFP.Visible = true;
        divFormFP.Visible = false;
        FPbtnSubmitText();
        EnabledTrueFields();
        ddlaccessoriesFP.ClearSelection();
        ddlVehicleFP.ClearSelection();
        ddltaxFP.ClearSelection();
        rbtnTypeFP.SelectedValue = "V";
        //divddlaccessories.Visible = false;
        divddlaccessoriesType.Visible = false;
        divddlVehicle.Visible = false;
        ddlAccessoriesType.ClearSelection();
        rbtnTypeFP.Enabled = true;
        ddlaccessoriesFP.Enabled = true;
        ddlAccessoriesType.Enabled = true;

        ddlVehicleFP.Enabled = true;
        if (rbtnGVTypeFP.SelectedValue == "A")
        {
            rbtnGVTypeFP.SelectedValue = "A";
        }
        else
        {
            rbtnGVTypeFP.SelectedValue = "V";
        }
        txttotalamountFP.Text = "";
        txtTaxFP.Text = "";
        txtAmountFP.Text = "";
        txtGraceTime.Text = "";
        txtNAcctotalamt.Text = "";
        txtNAcctaxamt.Text = "";
        txtNAccAmt.Text = "";
        DivFPVNAccDetails.Visible = false;
        DivFPVNDetails.Visible = false;
        if (ChkUpdtPrice.Checked == false)
        {
            txtGraceTime.Enabled = true;
            ddltaxFP.Enabled = true;
            btnSubmitFP.Enabled = true;
            BtnAddPrice.Enabled = false;
            ddluserMode.Enabled = false;
            txtFromFP.Enabled = false;
            txtToFP.Enabled = false;
            txttotalamountFP.Enabled = false;
            txtAmountFP.Enabled = false;
            txtTaxFP.Enabled = false;
            ddltaxFP.Enabled = true;
        }
        else
        {
            txtGraceTime.Enabled = false;
            ddltaxFP.Enabled = false;
            btnSubmitFP.Enabled = false;
            BtnAddPrice.Enabled = true;
            ddluserMode.Enabled = true;
            txtFromFP.Enabled = true;
            txtToFP.Enabled = true;
            txttotalamountFP.Enabled = true;
            txtAmountFP.Enabled = true;
            txtTaxFP.Enabled = true;
        }
        BtnAddPrice.Text = "Add Price";
        ddluserMode.SelectedValue = "N";
        txtFromFP.Text = string.Empty;
        txtToFP.Text = string.Empty;
        txttotalamountFP.Text = string.Empty;
        txtAmountFP.Text = string.Empty;
        txtTaxFP.Text = string.Empty;
        ViewState["NrmlRow"] = null;
        ViewState["VipRow"] = null;
    }
    #endregion

    #region Check Update Price Enable
    protected void ChkUpdtPrice_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkUpdtPrice.Checked == false)
        {
            txtGraceTime.Enabled = true;
            ddltaxFP.Enabled = true;
            btnSubmitFP.Enabled = true;
            BtnAddPrice.Enabled = false;
            ddluserMode.Enabled = false;
            txtFromFP.Enabled = false;
            txtToFP.Enabled = false;
            txttotalamountFP.Enabled = false;
            txtAmountFP.Enabled = false;
            txtTaxFP.Enabled = false;
        }
        else
        {
            txtGraceTime.Enabled = false;
            ddltaxFP.Enabled = false;
            btnSubmitFP.Enabled = false;
            BtnAddPrice.Enabled = true;
            ddluserMode.Enabled = true;
            txtFromFP.Enabled = true;
            txtToFP.Enabled = true;
            txttotalamountFP.Enabled = true;
            txtAmountFP.Enabled = true;
            txtTaxFP.Enabled = true;
        }
        BtnAddPrice.Text = "Add Price";
        ddluserMode.SelectedValue = "N";
        txtFromFP.Text = string.Empty;
        txtToFP.Text = string.Empty;
        txttotalamountFP.Text = string.Empty;
        txtAmountFP.Text = string.Empty;
        txtTaxFP.Text = string.Empty;
    }
    #endregion

    #region rbtnTypeFP SelectedIndexChanged
    protected void rbtnTypeFP_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddltaxFP.ClearSelection();
        VehicleAccessories();
    }

    public void VehicleAccessories()
    {
        if (rbtnTypeFP.SelectedValue == "V")
        {
            if (ViewState["SlabFlag"].ToString() == "Add")
            {
                divtimetype.Visible = false;
                ChkUpdtPrice.Visible = false;
            }
            else
            {
                divtimetype.Visible = true;
                ChkUpdtPrice.Visible = true;
            }
            divddlVehicle.Visible = true;
            //divddlaccessories.Visible = false;
            divddlaccessoriesType.Visible = false;
            DivFPVNAccDetails.Visible = false;
        }
        else
        {
            btnSubmitFP.Enabled = true;
            ChkUpdtPrice.Visible = false;
            //divddlaccessories.Visible = true;
            divddlaccessoriesType.Visible = true;
            DivFPVNDetails.Visible = false;
            DivFPVNAccDetails.Visible = true;
            divtimetype.Visible = false;
            divddlVehicle.Visible = false;
        }
    }
    #endregion

    #region Vehicle Grid Edit Click 
    protected void LnkEditFP_Click(object sender, EventArgs e)
    {

        try
        {

            SlabDetailsClear();
            ViewState["SlabFlag"] = "Edit";
            FPbtnUpdateText();
            FPBindVehicle();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvfloorId = (Label)gvrow.FindControl("lblgvfloorIdFP");
            Label lblgvfloorName = (Label)gvrow.FindControl("lblgvfloorNameFP");
            Label lblgvidType = (Label)gvrow.FindControl("lblgvidTypeFPV");
            Label lblgvvehicleAccessories = (Label)gvrow.FindControl("lblgvvehicleAccessoriesFP");
            rbtnTypeFP.Enabled = false;
            Label lblgvvehicleAccessoriesName = (Label)gvrow.FindControl("lblgvvehicleAccessoriesNameFP");
            Label lblgvgraceTimeFPV = (Label)gvrow.FindControl("lblgvgraceTimeFPV");
            Label lblgvtimeType = (Label)gvrow.FindControl("lblgvidTypeFPV");
            Label lblgvtaxId = (Label)gvrow.FindControl("lblgvtaxIdFP");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactiveFP");
            string sgvPricemaster = gvPricemaster.DataKeys[gvrow.RowIndex].Value.ToString();
            //ViewState["ActiveStatus"] = lnkActiveOrInactive.ToString() == "Active" ? "A" : "D";
            //txtamountFP.Text = lblgvTotalamount.Text;
            //txttotalAmountFP.Text = lblgvamount.Text;
            rbtnTypeFP.SelectedValue = lblgvidType.Text;
            ddltaxFP.SelectedValue = lblgvtaxId.Text;
            txtGraceTime.Text = lblgvgraceTimeFPV.Text;
            //txtTaxFP.Text = lblgvtax.Text;
            //HfpriceId.Value = lblgvpriceId.Text;
            EnabledFalseFields();
            FPADD();
            spAddorEditFP.InnerText = "Edit ";
            if (lblgvidType.Text == "V")
            {
                divddlVehicle.Visible = true;
                ddlVehicleFP.SelectedValue = lblgvvehicleAccessories.Text;
                divtimetype.Visible = true;
                NrmlPriceDetails.Columns[5].Visible = false;
                NrmlPriceDetails.Columns[6].Visible = true;
                NrmlPriceDetails.Columns[7].Visible = true;
                VipPriceDetails.Columns[5].Visible = false;
                VipPriceDetails.Columns[6].Visible = true;
                VipPriceDetails.Columns[7].Visible = true;
            }
            else
            {
                btnSubmitFP.Enabled = true;
                //divddlaccessories.Visible = true;
                divddlaccessoriesType.Visible = true;

                ddlaccessoriesFP.SelectedValue = lblgvvehicleAccessories.Text;
            }

            var dataList = gvrow.FindControl("dlVPriceDetails") as DataList;
            StringBuilder sb = new StringBuilder();
            DataTable dtNrml = new DataTable();
            DataTable dtVip = new DataTable();

            dtNrml.Columns.Add("slabId");
            dtNrml.Columns.Add("priceId");
            dtNrml.Columns.Add("userMode");
            dtNrml.Columns.Add("totalAmount");
            dtNrml.Columns.Add("amount");
            dtNrml.Columns.Add("tax");
            dtNrml.Columns.Add("From");
            dtNrml.Columns.Add("To");
            dtNrml.Columns.Add("activeStatus");


            dtVip.Columns.Add("slabId");
            dtVip.Columns.Add("priceId");
            dtVip.Columns.Add("userMode");
            dtVip.Columns.Add("totalAmount");
            dtVip.Columns.Add("amount");
            dtVip.Columns.Add("tax");
            dtVip.Columns.Add("From");
            dtVip.Columns.Add("To");
            dtVip.Columns.Add("activeStatus");
            for (int i = 0; i < dataList.Items.Count; i++)
            {
                string hfid = string.Empty;
                Label lblgvtotalAmountFPV = dataList.Items[i].FindControl("lblgvtotalAmountFPV") as Label;
                Label lblgvtaxFPV = dataList.Items[i].FindControl("lblgvtaxFPV") as Label;
                Label lblgvtimeTypeFPV = dataList.Items[i].FindControl("lblgvtimeTypeFPV") as Label;
                Label lblgvamountFPV = dataList.Items[i].FindControl("lblgvamountFPV") as Label;
                Label lblgvactiveStatusFPV = dataList.Items[i].FindControl("lblgvactiveStatusFPV") as Label;
                Label lblgvpriceIdFPV = dataList.Items[i].FindControl("lblgvpriceIdFPV") as Label;

                DataList dtlNrmlVipSlabDetails = dataList.Items[i].FindControl("dtlNrmlVipSlabDetails") as DataList;


                ViewState["priceId"] = lblgvpriceIdFPV.Text;
                ViewState["gvactiveStatusFPV"] = lblgvactiveStatusFPV.Text;
                for (int j = 0; j < dtlNrmlVipSlabDetails.Items.Count; j++)
                {
                    Label gVNslabId = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNslabId") as Label;
                    Label gVNpriceId = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNpriceId") as Label;
                    Label gVNtotalAmount = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNtotalAmount") as Label;
                    Label gVNamount = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNamount") as Label;
                    Label gVNtax = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNtax") as Label;
                    Label gVNfromDate = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNfromDate") as Label;
                    Label gVNtoDate = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNtoDate") as Label;
                    Label gVNactiveStatus = dtlNrmlVipSlabDetails.Items[j].FindControl("gVNactiveStatus") as Label;
                    Label lblgvuserModeFPV = dtlNrmlVipSlabDetails.Items[j].FindControl("lblgvuserModeFPV") as Label;
                    if (lblgvuserModeFPV.Text == "N")
                    {
                        DataRow dr1 = dtNrml.NewRow();
                        dtNrml.Rows.Add(gVNslabId.Text, gVNpriceId.Text, lblgvuserModeFPV.Text, gVNtotalAmount.Text, gVNamount.Text,
                            gVNtax.Text, gVNfromDate.Text, gVNtoDate.Text, gVNactiveStatus.Text);
                    }
                    else if (lblgvuserModeFPV.Text == "V")
                    {
                        DataRow dr1 = dtVip.NewRow();
                        dtVip.Rows.Add(gVNslabId.Text, gVNpriceId.Text, lblgvuserModeFPV.Text, gVNtotalAmount.Text, gVNamount.Text,
                            gVNtax.Text, gVNfromDate.Text, gVNtoDate.Text, gVNactiveStatus.Text);
                    }

                }

            }



            if (dtNrml.Rows.Count > 0)
            {
                ViewState["NrmlRow"] = dtNrml;
                NrmlPriceDetails.DataSource = dtNrml;
                NrmlPriceDetails.DataBind();
                divNormalFP.Visible = true;
            }
            if (dtVip.Rows.Count > 0)
            {
                VipPriceDetails.DataSource = dtVip;
                VipPriceDetails.DataBind();
                ViewState["VipRow"] = dtVip;
                divVipFP.Visible = true;
            }
            if (dtVip.Rows.Count == 0)
            {
                ViewState["VipRow"] = null;
                VipPriceDetails.DataSource = null;
                VipPriceDetails.DataBind();

            }
            if (dtNrml.Rows.Count == 0)
            {
                ViewState["NrmlRow"] = null;
                NrmlPriceDetails.DataSource = null;
                NrmlPriceDetails.DataBind();

            }
            if (ChkUpdtPrice.Checked == false)
            {
                txtGraceTime.Enabled = true;
                ddltaxFP.Enabled = true;
                btnSubmitFP.Enabled = true;
                BtnAddPrice.Enabled = false;
                ddluserMode.Enabled = false;
                txtFromFP.Enabled = false;
                txtToFP.Enabled = false;
                txttotalamountFP.Enabled = false;
                txtAmountFP.Enabled = false;
                txtTaxFP.Enabled = false;
            }
            else
            {
                txtGraceTime.Enabled = false;
                ddltaxFP.Enabled = false;
                btnSubmitFP.Enabled = false;
                BtnAddPrice.Enabled = true;
                ddluserMode.Enabled = true;
                txtFromFP.Enabled = true;
                txtToFP.Enabled = true;
                txttotalamountFP.Enabled = true;
                txtAmountFP.Enabled = true;
                txtTaxFP.Enabled = true;
            }
            DivFPVNDetails.Visible = true;
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }

    }
    #endregion

    #region Accessories Grid Edit Click
    protected void LnkEditFPA_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            FPbtnUpdateText();
            BindAccessoriesType();
            FPBindVehicle();
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvpriceId = (Label)gvrow.FindControl("lblgvpriceIdFP");
            Label lblgvfloorId = (Label)gvrow.FindControl("lblgvfloorIdFP");
            Label lblgvfloorName = (Label)gvrow.FindControl("lblgvfloorNameFP");
            Label lblgvTotalamount = (Label)gvrow.FindControl("lblgvtotalAmountFP");
            Label lblgvamount = (Label)gvrow.FindControl("lblgvamountFP");
            Label lblgvtax = (Label)gvrow.FindControl("lblgvtaxFP");
            Label lblgvidType = (Label)gvrow.FindControl("lblgvidTypeFP");
            Label lblgvconfigTypeId = (Label)gvrow.FindControl("lblgvconfigTypeId");
            Label lblgvvehicleAccessories = (Label)gvrow.FindControl("lblgvvehicleAccessoriesFP");
            rbtnTypeFP.Enabled = false;
            Label lblgvvehicleAccessoriesName = (Label)gvrow.FindControl("lblgvvehicleAccessoriesNameFP");
            Label lblgvtimeType = (Label)gvrow.FindControl("lblgvidTypeFPV");
            Label lblgvtaxId = (Label)gvrow.FindControl("lblgvtaxIdFP");
            Label lblgvtaxName = (Label)gvrow.FindControl("lblgvtaxNameFP");
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactiveFP");
            string sgvPricemaster = gvAccessoriesFP.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.ToString() == "Active" ? "A" : "D";
            txtNAcctotalamt.Text = lblgvTotalamount.Text;
            txtNAccAmt.Text = lblgvamount.Text;
            rbtnTypeFP.SelectedValue = lblgvidType.Text;
            ddltaxFP.SelectedValue = lblgvtaxId.Text;
            txtNAcctaxamt.Text = lblgvtax.Text;
            HfpriceId.Value = lblgvpriceId.Text;
            EnabledFalseFields();
            spAddorEditFP.InnerText = "Edit ";
            FPADD();
            if (lblgvidType.Text == "V")
            {
                divddlVehicle.Visible = true;
                ddlVehicleFP.SelectedValue = lblgvvehicleAccessories.Text;
                rbtnTypeFP.SelectedValue = "V";
            }
            else
            {
                //divddlaccessories.Visible = true;
                ddlAccessoriesType.SelectedValue = lblgvconfigTypeId.Text;
                BindAccessories();
                ddlaccessoriesFP.SelectedValue = lblgvvehicleAccessories.Text;
                ChkUpdtPrice.Visible = false;
                divddlaccessoriesType.Visible = true;
                DivFPVNDetails.Visible = false;
                DivFPVNAccDetails.Visible = true;
                divtimetype.Visible = false;
                divddlVehicle.Visible = false;
                rbtnTypeFP.SelectedValue = "A";
            }

        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }
    }
    #endregion

    #region Update Enabled False Fields
    public void EnabledFalseFields()
    {
        divtimetype.Visible = false;
        ddlVehicleFP.Enabled = false;
        ddlaccessoriesFP.Enabled = false;
        ddlAccessoriesType.Enabled = false;
    }
    #endregion

    #region Update Enabled True Fields
    public void EnabledTrueFields()
    {

        divtimetype.Visible = true;
        divddlVehicle.Visible = true;
        //divddlaccessories.Visible = true;
        divddlaccessoriesType.Visible = true;

    }
    #endregion

    #region Vehicle Delete Click
    protected void lnkActiveOrInactiveFP_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sPriceId = gvPricemaster.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactiveFP");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "priceMaster?priceId=" + sPriceId
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
                        BindVehiclePriceMaster();
                        BindAccessoriesPriceMaster();
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

    #region Accessories Delete Click
    protected void AcclnkActiveOrInactiveFP_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sPriceId = gvAccessoriesFP.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactiveFP");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "priceMaster?priceId=" + sPriceId
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
                        BindVehiclePriceMaster();
                        BindAccessoriesPriceMaster();
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

    #region Grid Bind   
    #region Grid rbtnGVTypeFP SelectedIndexChanged
    protected void rbtnGVTypeFP_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnGVTypeFP.SelectedItem.Text == "Vehicle")
        {
            BindVehiclePriceMaster();
            divVehicleGVFP.Visible = true;
            divAccessoriesGVFP.Visible = false;

        }
        else
        {
            BindAccessoriesPriceMaster();
            divVehicleGVFP.Visible = false;
            divAccessoriesGVFP.Visible = true;

        }

    }
    #endregion

    #region Bind Vehicle Grid
    public void BindVehiclePriceMaster()
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
                        + "priceMaster?idType=V&type=G&floorId=";
                sUrl += ViewState["floorId"].ToString().Trim();

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    var Response = JObject.Parse(SmartParkingList)["response"].ToArray();
                    if (StatusCode == 1)
                    {
                        List<GetPriceMasterClass> Price = JsonConvert.DeserializeObject<List<GetPriceMasterClass>>(ResponseMsg);

                        DataTable dt = ConvertToDataTable(Price);
                        if (dt.Rows.Count > 0)
                        {
                            gvPricemaster.DataSource = Price;
                            gvPricemaster.DataBind();
                        }
                        else
                        {
                            gvPricemaster.DataSource = null;
                            gvPricemaster.DataBind();

                        }
                    }
                    else
                    {
                        gvPricemaster.DataBind();
                        FPADDMethod();
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
    #endregion

    #region Bind Accessories Grid
    public void BindAccessoriesPriceMaster()
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
                        + "priceMaster?idType=A&floorId=";
                sUrl += ViewState["floorId"].ToString().Trim();

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
                            gvAccessoriesFP.DataSource = dt;
                            gvAccessoriesFP.DataBind();
                            divGvFP.Visible = true;
                        }
                        else
                        {
                            gvAccessoriesFP.DataSource = null;
                            gvAccessoriesFP.DataBind();
                        }
                    }
                    else
                    {
                        FPADDMethod();
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
    #endregion
    #endregion

    #region  GV Vehicle RowDateBound 
    protected void gvPricemaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var PriceH = e.Row.DataItem as GetPriceMasterClass;
            var dataList = e.Row.FindControl("dlVPriceDetails") as DataList;
            dataList.DataSource = PriceH.priceDetails;
            dataList.DataBind();

        }
    }
    protected void dlVPriceDetails_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemIndex != -1)
        {
            var PriceH = e.Item.DataItem as GetpriceDetailss;
            var dataList = e.Item.FindControl("dtlNrmlVipSlabDetails") as DataList;
            dataList.DataSource = PriceH.timeSlabDetails;
            dataList.DataBind();

        }
    }
    #endregion

    #region Bind Slab Details
    public void BindSlabDetails()
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
                        + "timeSlabRules?priceId=" + ViewState["priceId"].ToString() + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    var Response = JObject.Parse(SmartParkingList)["response"].ToArray();
                    if (StatusCode == 1)
                    {
                        List<GettimeSlabDetailss> Price = JsonConvert.DeserializeObject<List<GettimeSlabDetailss>>(ResponseMsg);
                        List<GettimeSlabDetailss> NrmlPrice = new List<GettimeSlabDetailss>();
                        List<GettimeSlabDetailss> VipPrice = new List<GettimeSlabDetailss>();
                        NrmlPrice = Price.Where(x => x.userMode == "N").Select(x => x).ToList();
                        VipPrice = Price.Where(x => x.userMode == "V").Select(x => x).ToList();
                        DataTable Nrmldt = ConvertToDataTable(NrmlPrice);
                        DataTable Vipdt = ConvertToDataTable(VipPrice);
                        if (Nrmldt.Rows.Count > 0)
                        {
                            divNormalFP.Visible = true;
                            ViewState["NrmlRow"] = Nrmldt;
                            NrmlPriceDetails.DataSource = Nrmldt;
                            NrmlPriceDetails.DataBind();
                        }
                        if (Vipdt.Rows.Count > 0)
                        {
                            divVipFP.Visible = true;
                            ViewState["VipRow"] = Vipdt;
                            VipPriceDetails.DataSource = Vipdt;
                            VipPriceDetails.DataBind();
                        }
                        if (Vipdt.Rows.Count == 0)
                        {
                            ViewState["VipRow"] = null;
                            VipPriceDetails.DataSource = null;
                            VipPriceDetails.DataBind();

                        }
                        if (Nrmldt.Rows.Count == 0)
                        {
                            ViewState["NrmlRow"] = null;
                            NrmlPriceDetails.DataSource = null;
                            NrmlPriceDetails.DataBind();

                        }
                    }
                    else
                    {
                        NrmlPriceDetails.DataSource = null;
                        NrmlPriceDetails.DataBind();
                        VipPriceDetails.DataSource = null;
                        VipPriceDetails.DataBind();
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
    #region Btn Add Price
    protected void BtnAddPrice_Click(object sender, EventArgs e)
    {
        try
        {
            ddlVehicleFP.Enabled = false;
            if (BtnAddPrice.Text == "Add Price")
            {
                if (ViewState["SlabFlag"].ToString() == "Add")
                {
                    SlabDetails();
                }
                else
                {
                    AddslabDetails(ViewState["priceId"].ToString());
                }
            }
            else
            {
                UpdateslabDetails(ViewState["priceIds"].ToString(), ViewState["slabId"].ToString(),
                   ViewState["activeStatusSlab"].ToString());
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    #endregion
    #region Post Time Slab details
    public void SlabDetails()
    {
        try
        {
            string priceId = string.Empty;
            if (ddluserMode.SelectedValue == "N")
            {
                priceId = "0";
                divNormalFP.Visible = true;
                DataTable dt = new DataTable();
                int Uniqueid = 1;
                if (ViewState["NrmlRow"] != null)
                {
                    dt = (DataTable)ViewState["NrmlRow"];
                    DataRow dr = null;

                    if (dt.Rows.Count > 0)
                    {
                        List<timeSlabDetailss> TimeSlab = new List<timeSlabDetailss>();
                        TimeSlab = ConvertDataTable<timeSlabDetailss>(dt);

                        var ToHour = TimeSlab.OrderByDescending(x => x.slabId)
                          .Select(x => x.To).Take(1).ToList();
                        string Hour = ToHour[0];
                        if ((Convert.ToInt16(Hour) <= Convert.ToInt16(txtFromFP.Text)) &&
                            (Convert.ToInt16(txtFromFP.Text) < Convert.ToInt16(txtToFP.Text)) &&
                            (Convert.ToInt16(txtFromFP.Text) < 25) && (Convert.ToInt16(txtToFP.Text) < 25))
                        {
                            Uniqueid = dt.Rows.Count + 1;
                            dr = dt.NewRow();
                            dr["slabId"] = Uniqueid;
                            dr["priceId"] = priceId;
                            dr["userMode"] = ddluserMode.SelectedValue;
                            // dr["userModetxt"] = ddluserMode.SelectedItem.Text;
                            dr["From"] = txtFromFP.Text;
                            dr["To"] = txtToFP.Text;
                            dr["amount"] = txtAmountFP.Text;
                            dr["tax"] = txtTaxFP.Text;
                            dr["totalAmount"] = txttotalamountFP.Text;
                            dr["activeStatus"] = "A";
                            dt.Rows.Add(dr);
                            ViewState["NrmlRow"] = dt;
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('From Hour should be greater or equal to previous To Hour');", true);
                            return;
                        }
                    }
                }
                else
                {
                    if ((Convert.ToInt16(txtFromFP.Text) < Convert.ToInt16(txtToFP.Text)) &&
                       (Convert.ToInt16(txtFromFP.Text) < 25) && (Convert.ToInt16(txtToFP.Text) < 25))
                    {
                        dt.Columns.Add(new DataColumn("slabId", typeof(string)));
                        dt.Columns.Add(new DataColumn("priceId", typeof(string)));
                        dt.Columns.Add(new DataColumn("userMode", typeof(string)));
                        // dt.Columns.Add(new DataColumn("userModetxt", typeof(string)));
                        dt.Columns.Add(new DataColumn("From", typeof(string)));
                        dt.Columns.Add(new DataColumn("To", typeof(string)));
                        dt.Columns.Add(new DataColumn("amount", typeof(decimal)));
                        dt.Columns.Add(new DataColumn("tax", typeof(decimal)));
                        dt.Columns.Add(new DataColumn("totalAmount", typeof(decimal)));
                        dt.Columns.Add(new DataColumn("activeStatus", typeof(string)));

                        DataRow dr1 = dt.NewRow();
                        dr1 = dt.NewRow();
                        dr1["slabId"] = Uniqueid;
                        dr1["priceId"] = priceId;
                        dr1["userMode"] = ddluserMode.SelectedValue;
                        //  dr1["userModetxt"] = ddluserMode.SelectedItem.Text;
                        dr1["From"] = txtFromFP.Text;
                        dr1["To"] = txtToFP.Text;
                        dr1["amount"] = txtAmountFP.Text;
                        dr1["tax"] = txtTaxFP.Text;
                        dr1["totalAmount"] = txttotalamountFP.Text;
                        dr1["activeStatus"] = "A";
                        dt.Rows.Add(dr1);
                        ViewState["NrmlRow"] = dt;
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('To Hour Should be Greater Than From Hour');", true);
                        return;
                    }
                }


                if (dt.Rows.Count > 0)
                {
                    NrmlPriceDetails.DataSource = dt;
                    NrmlPriceDetails.DataBind();
                    txtFromFP.Text = string.Empty;
                    txtToFP.Text = string.Empty;
                    txttotalamountFP.Text = string.Empty;
                    txtAmountFP.Text = string.Empty;
                    txtTaxFP.Text = string.Empty;
                }
                else
                {
                    ViewState["NrmlRow"] = null;
                    divNormalFP.Visible = false;
                    NrmlPriceDetails.DataSource = null;
                    NrmlPriceDetails.DataBind();
                }
            }
            else
            {
                divVipFP.Visible = true;
                DataTable dt = new DataTable();
                int Uniqueid = 1;
                if (ViewState["VipRow"] != null)
                {
                    dt = (DataTable)ViewState["VipRow"];
                    DataRow dr = null;
                    if (dt.Rows.Count > 0)
                    {
                        List<timeSlabDetailss> TimeSlab = new List<timeSlabDetailss>();
                        TimeSlab = ConvertDataTable<timeSlabDetailss>(dt);

                        var ToHour = TimeSlab.OrderByDescending(x => x.slabId)
                          .Select(x => x.To).Take(1).ToList();
                        string Hour = ToHour[0];
                        if ((Convert.ToInt16(Hour) <= Convert.ToInt16(txtFromFP.Text)) &&
                            (Convert.ToInt16(txtFromFP.Text) < Convert.ToInt16(txtToFP.Text)) &&
                            (Convert.ToInt16(txtFromFP.Text) < 25) && (Convert.ToInt16(txtToFP.Text) < 25))
                        {
                            Uniqueid = dt.Rows.Count + 1;
                            dr = dt.NewRow();
                            dr["slabId"] = Uniqueid;
                            dr["priceId"] = priceId;
                            dr["userMode"] = ddluserMode.SelectedValue;
                            dr["From"] = txtFromFP.Text;
                            dr["To"] = txtToFP.Text;
                            dr["amount"] = txtAmountFP.Text;
                            dr["tax"] = txtTaxFP.Text;
                            dr["totalAmount"] = txttotalamountFP.Text;
                            dr["activeStatus"] = "A";
                            dt.Rows.Add(dr);
                            ViewState["VipRow"] = dt;
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('From Hour should be greater or equal to previous To Hour');", true);
                            return;
                        }
                    }

                }
                else
                {
                    if ((Convert.ToInt16(txtFromFP.Text) < Convert.ToInt16(txtToFP.Text)) &&
                     (Convert.ToInt16(txtFromFP.Text) < 25) && (Convert.ToInt16(txtToFP.Text) < 25))
                    {
                        dt.Columns.Add(new DataColumn("slabId", typeof(string)));
                        dt.Columns.Add(new DataColumn("priceId", typeof(string)));
                        dt.Columns.Add(new DataColumn("userMode", typeof(string)));
                        // dt.Columns.Add(new DataColumn("userModetxt", typeof(string)));
                        dt.Columns.Add(new DataColumn("From", typeof(string)));
                        dt.Columns.Add(new DataColumn("To", typeof(string)));
                        dt.Columns.Add(new DataColumn("amount", typeof(decimal)));
                        dt.Columns.Add(new DataColumn("tax", typeof(decimal)));
                        dt.Columns.Add(new DataColumn("totalAmount", typeof(decimal)));
                        dt.Columns.Add(new DataColumn("activeStatus", typeof(string)));

                        DataRow dr1 = dt.NewRow();
                        dr1 = dt.NewRow();
                        dr1["slabId"] = Uniqueid;
                        dr1["priceId"] = priceId;
                        dr1["userMode"] = ddluserMode.SelectedValue;
                        //  dr1["userModetxt"] = ddluserMode.SelectedItem.Text;
                        dr1["From"] = txtFromFP.Text;
                        dr1["To"] = txtToFP.Text;
                        dr1["amount"] = txtAmountFP.Text;
                        dr1["tax"] = txtTaxFP.Text;
                        dr1["totalAmount"] = txttotalamountFP.Text;
                        dr1["activeStatus"] = "A";
                        dt.Rows.Add(dr1);
                        ViewState["VipRow"] = dt;
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('To Hour Should be Greater Than From Hour');", true);
                        return;
                    }

                }

                if (dt.Rows.Count > 0)
                {

                    VipPriceDetails.DataSource = dt;
                    VipPriceDetails.DataBind();
                    txtFromFP.Text = string.Empty;
                    txtToFP.Text = string.Empty;
                    txttotalamountFP.Text = string.Empty;
                    txtAmountFP.Text = string.Empty;
                    txtTaxFP.Text = string.Empty;
                }
                else
                {
                    ViewState["VipRow"] = null;
                    divVipFP.Visible = false;
                    VipPriceDetails.DataSource = null;
                    VipPriceDetails.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }

    }
    public class timeSlabDetailss
    {
        public String userMode { get; set; }
        public String slabId { get; set; }
        public String taxId { get; set; }
        public String priceId { get; set; }
        public decimal totalAmount { get; set; }
        public decimal amount { get; set; }
        public decimal tax { get; set; }
        public String From { get; set; }
        public String To { get; set; }
        public String activeStatus { get; set; }


    }
    private static List<T> ConvertDataTable<T>(DataTable dt)
    {
        List<T> data = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            T item = GetItem<T>(row);
            data.Add(item);
        }
        return data;
    }
    private static T GetItem<T>(DataRow dr)
    {
        Type temp = typeof(T);
        T obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName)
                    pro.SetValue(obj, dr[column.ColumnName], null);
                else
                    continue;
            }
        }
        return obj;
    }
    public void AddslabDetails(string Priceid)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new GettimeSlabDetailss()
                {
                    priceId = Priceid,
                    fromDate = txtFromFP.Text,
                    toDate = txtToFP.Text,
                    totalAmount = txttotalamountFP.Text,
                    activeStatus = "A",
                    userMode = ddluserMode.SelectedValue,
                    createdBy = Session["UserId"].ToString(),

                };

                HttpResponseMessage response = client.PostAsJsonAsync("timeSlabRules", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BtnAddPrice.Text = "Add Price";
                        ddluserMode.Enabled = true;
                        txtFromFP.Text = string.Empty;
                        txtToFP.Text = string.Empty;
                        txttotalamountFP.Text = string.Empty;
                        txtAmountFP.Text = string.Empty;
                        txtTaxFP.Text = string.Empty;
                        BindSlabDetails();
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
    #region Put Time Slabdetails
    public void UpdateslabDetails(string Priceid, string SlabId, string ActiveStatus)
    {
        try
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new GettimeSlabDetailss()
                {
                    priceId = Priceid,
                    slabId = SlabId,
                    taxId = ddltaxFP.SelectedValue,
                    fromDate = txtFromFP.Text,
                    toDate = txtToFP.Text,
                    totalAmount = txttotalamountFP.Text,
                    activeStatus = ActiveStatus,
                    userMode = ddluserMode.SelectedValue,
                    updatedBy = Session["UserId"].ToString(),

                };

                HttpResponseMessage response = client.PutAsJsonAsync("timeSlabRules", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BtnAddPrice.Text = "Add Price";
                        ddluserMode.Enabled = true;
                        txtFromFP.Text = string.Empty;
                        txtToFP.Text = string.Empty;
                        txttotalamountFP.Text = string.Empty;
                        txtAmountFP.Text = string.Empty;
                        txtTaxFP.Text = string.Empty;
                        BindSlabDetails();
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
    #region Normal and Vip Grid View Delete Button
    protected void NrmlImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvslabId = (Label)gvrow.FindControl("lblgvslabId");

            DataTable dt = (DataTable)ViewState["NrmlRow"];
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dt.Rows[i];
                if (drO["slabId"].ToString().Trim() == lblgvslabId.Text)
                {
                    drO.Delete();
                }
            }
            dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                NrmlPriceDetails.DataSource = dt;
                NrmlPriceDetails.DataBind();
                divNormalFP.Visible = true;
                ViewState["NrmlRow"] = dt;
            }
            else
            {
                NrmlPriceDetails.DataSource = null;
                NrmlPriceDetails.DataBind();
                divNormalFP.Visible = false;
                ViewState["NrmlRow"] = null;
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    protected void VipImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvslabId = (Label)gvrow.FindControl("lblgvslabId");

            DataTable dt = (DataTable)ViewState["VipRow"];
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow drO = dt.Rows[i];
                if (drO["slabId"].ToString().Trim() == lblgvslabId.Text)
                {
                    drO.Delete();
                }
            }
            dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                VipPriceDetails.DataSource = dt;
                VipPriceDetails.DataBind();
                divVipFP.Visible = true;
                ViewState["VipRow"] = dt;
            }
            else
            {
                VipPriceDetails.DataSource = null;
                VipPriceDetails.DataBind();
                divVipFP.Visible = false;
                ViewState["VipRow"] = null;
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Normal  Grid View edit  Button
    protected void NrmllnkEditFP_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvslabId = (Label)gvrow.FindControl("lblgvslabId");
            Label lblgvpriceId = (Label)gvrow.FindControl("lblgvpriceId");
            Label lblgvUserMode = (Label)gvrow.FindControl("lblgvUserMode");
            Label lblgvFrom = (Label)gvrow.FindControl("lblgvFrom");
            Label lblgvTo = (Label)gvrow.FindControl("lblgvTo");
            Label lblgvAmount = (Label)gvrow.FindControl("lblgvAmount");
            Label lblgvtax = (Label)gvrow.FindControl("lblgvtax");
            Label lblgvTotalAmount = (Label)gvrow.FindControl("lblgvTotalAmount");
            LinkButton NrmllnkActiveOrInactiveFP = (LinkButton)gvrow.FindControl("NrmllnkActiveOrInactiveFP");
            ddluserMode.SelectedValue = lblgvUserMode.Text;
            txtFromFP.Text = lblgvFrom.Text;
            txtToFP.Text = lblgvTo.Text;
            txttotalamountFP.Text = lblgvTotalAmount.Text;
            txtAmountFP.Text = lblgvAmount.Text;
            txtTaxFP.Text = lblgvtax.Text;
            ViewState["priceIds"] = lblgvpriceId.Text;
            ViewState["slabId"] = lblgvslabId.Text;
            string sActiveStatus = NrmllnkActiveOrInactiveFP.Text.Trim() == "Active" ? "A" : "D";
            ViewState["activeStatusSlab"] = sActiveStatus;
            ddluserMode.Enabled = false;
            BtnAddPrice.Text = "Update Price";
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Normal  Grid View ActiveORInactive  Button
    protected void NrmllnkActiveOrInactiveFP_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sslabId = NrmlPriceDetails.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("NrmllnkActiveOrInactiveFP");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "timeSlabRules?slabId=" + sslabId
                            + "&activeStatus="
                            + sActiveStatus;

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        SlabDetailsClear();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindSlabDetails();
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
    #region Vip  Grid View edit  Button
    protected void ViplnkEditFP_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblgvslabId = (Label)gvrow.FindControl("lblgvslabId");
            Label lblgvpriceId = (Label)gvrow.FindControl("lblgvpriceId");
            Label lblgvUserMode = (Label)gvrow.FindControl("lblgvUserMode");
            Label lblgvFrom = (Label)gvrow.FindControl("lblgvFrom");
            Label lblgvTo = (Label)gvrow.FindControl("lblgvTo");
            Label lblgvAmount = (Label)gvrow.FindControl("lblgvAmount");
            Label lblgvtax = (Label)gvrow.FindControl("lblgvtax");
            Label lblgvTotalAmount = (Label)gvrow.FindControl("lblgvTotalAmount");
            LinkButton ViplnkActiveOrInactiveFP = (LinkButton)gvrow.FindControl("ViplnkActiveOrInactiveFP");
            ddluserMode.SelectedValue = lblgvUserMode.Text;
            txtFromFP.Text = lblgvFrom.Text;
            txtToFP.Text = lblgvTo.Text;
            txttotalamountFP.Text = lblgvTotalAmount.Text;
            txtAmountFP.Text = lblgvAmount.Text;
            txtTaxFP.Text = lblgvtax.Text;
            ddluserMode.Enabled = false;
            BtnAddPrice.Text = "Update Price";
            ViewState["priceIds"] = lblgvpriceId.Text;
            ViewState["slabId"] = lblgvslabId.Text;
            string sActiveStatus = ViplnkActiveOrInactiveFP.Text.Trim() == "Active" ? "A" : "D";
            ViewState["activeStatusSlab"] = sActiveStatus;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Vip  Grid View ActiveORInactive  Button
    protected void ViplnkActiveOrInactiveFP_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sslabId = VipPriceDetails.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("ViplnkActiveOrInactiveFP");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue
                    ("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "timeSlabRules?slabId=" + sslabId
                            + "&activeStatus="
                            + sActiveStatus;

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)
                        ["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        SlabDetailsClear();
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindSlabDetails();
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

    #region SlabDetailsClear
    public void SlabDetailsClear()
    {
        ddluserMode.Enabled = true;
        BtnAddPrice.Text = "Add Price";
        ddluserMode.SelectedValue = "N";
        txtFromFP.Text = string.Empty;
        txtToFP.Text = string.Empty;
        txttotalamountFP.Text = string.Empty;
        txtAmountFP.Text = string.Empty;
        txtTaxFP.Text = string.Empty;
        NrmlPriceDetails.DataSource = null;
        NrmlPriceDetails.DataBind();
        divNormalFP.Visible = false;
        divVipFP.Visible = false;
        VipPriceDetails.DataSource = null;
        VipPriceDetails.DataBind();
    }
    #endregion

    #region Price  Master Class
    public class PriceMasterClass
    {
        public String parkingOwnerId { get; set; }
        public String activeStatus { get; set; }
        public String branchId { get; set; }
        public String floorId { get; set; }
        public String vehicle_accessories { get; set; }
        public String graceTime { get; set; }
        public String taxId { get; set; }
        public String floorName { get; set; }
        public String vehicleAccessoriesName { get; set; }
        public String vehicleaccessories { get; set; }
        public String priceId { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }
        public String idType { get; set; }
        public String totalAmount { get; set; }
        public List<priceDetails> priceDetails { get; set; }
    }

    public class priceDetails
    {
        public String priceId { get; set; }
        public String timeType { get; set; }
        public String taxId { get; set; }
        public String userMode { get; set; }
        public String activeStatus { get; set; }
        public String remarks { get; set; }
        public List<timeSlabDetails> timeSlabDetails { get; set; }

    }

    public class timeSlabDetails
    {
        public String userMode { get; set; }
        public String totalAmount { get; set; }
        public String fromDate { get; set; }
        public String toDate { get; set; }
        public String activeStatus { get; set; }
    }

    public class GetPriceMasterClass
    {
        public String parkingOwnerId { get; set; }
        public String activeStatus { get; set; }
        public String branchId { get; set; }
        public String floorId { get; set; }
        public String vehicle_accessories { get; set; }
        public String graceTime { get; set; }
        public String taxId { get; set; }
        public String floorName { get; set; }
        public String vehicleAccessoriesName { get; set; }
        public String vehicleaccessories { get; set; }
        public String priceId { get; set; }
        public String createdBy { get; set; }
        public String updatedBy { get; set; }
        public String idType { get; set; }
        public List<GetpriceDetailss> priceDetails { get; set; }
    }

    public class GetpriceDetailss
    {
        public String priceId { get; set; }
        public String timeType { get; set; }
        public String taxId { get; set; }
        public String userMode { get; set; }
        public String activeStatus { get; set; }
        public String remarks { get; set; }
        public List<GettimeSlabDetailss> timeSlabDetails { get; set; }

    }

    public class GettimeSlabDetailss
    {
        public String userMode { get; set; }
        public String slabId { get; set; }
        public String taxId { get; set; }
        public String priceId { get; set; }
        public String totalAmount { get; set; }
        public String amount { get; set; }
        public String tax { get; set; }
        public String fromDate { get; set; }
        public String toDate { get; set; }
        public String from { get; set; }
        public String to { get; set; }
        public String activeStatus { get; set; }
        public String updatedBy { get; set; }
        public String createdBy { get; set; }

    }

    #endregion
    #endregion

    #region Floor Master Class
    public class FloorMaster
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockId { get; set; }
        public int floorId { get; set; }
        public string floorName { get; set; }
        public string floorType { get; set; }
        public string squareFeet { get; set; }
        public string typeOfParking { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
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
                        var Option = lst1.Where(x => x.optionName == "floorSetup" && x.MenuOptionAccessActiveStatus == "A")
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
                                btnAddFV.Visible = true;
                                btnAddFP.Visible = true;
                            }
                            else
                            {
                                btnAddFV.Visible = false;
                                btnAddFP.Visible = false;
                            }
                            if (View[0] == "True")
                            {
                                if (Edit[0] == "True")
                                {
                                    gvfloorvehiclemaster.Columns[11].Visible = true;
                                    // gvFloorFeatures.Columns[14].Visible = true;
                                    gvPricemaster.Columns[11].Visible = true;
                                    gvAccessoriesFP.Columns[17].Visible = true;
                                }
                                else
                                {
                                    gvfloorvehiclemaster.Columns[11].Visible = false;
                                    //  gvFloorFeatures.Columns[14].Visible = false;
                                    // gvPricemaster.Columns[11].Visible = false;
                                    gvAccessoriesFP.Columns[17].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvfloorvehiclemaster.Columns[12].Visible = true;
                                    // gvFloorFeatures.Columns[15].Visible = true;
                                    // gvPricemaster.Columns[14].Visible = true;
                                    gvAccessoriesFP.Columns[18].Visible = true;
                                }
                                else
                                {
                                    gvfloorvehiclemaster.Columns[12].Visible = false;
                                    //  gvFloorFeatures.Columns[15].Visible = false;
                                    gvAccessoriesFP.Columns[18].Visible = false;
                                }
                            }
                            else
                            {
                                divForm.Visible = false;

                            }

                        }
                        else
                        {
                            divForm.Visible = false;

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
