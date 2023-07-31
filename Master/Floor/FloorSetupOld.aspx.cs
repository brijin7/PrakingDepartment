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
        FVBindVehicleSize();
        BindFloorVehicle();
        divFloorSetup.Visible = true;
        divFloorFeatures.Visible = false;
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
        divFloorFeatures.Visible = false;
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
    public void FVBindVehicleSize()
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
                          + "vehicleSizeConfigMaster?activestatus=A";
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
                    vehicleType = ddlvehicletypeFV.SelectedValue,
                    capacity = txtCapacityFV.Text,
                    vehicleSizeConfigId = ddlvehicleSizeFV.SelectedValue,                 
                    rules = txtRulesFV.Text,
                    floorVehicleId = hffloorVehicleIdFV.Value,
                    floorId = hfflooridFV.Value,
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
        FPCancel();
        FPADD();
        if (rbtnGVTypeFP.SelectedItem.Text == "Vehicle")
        {
            divddlVehicle.Visible = true;
            ////divddlaccessories.Visible = false;
            divddlaccessoriesType.Visible = false;
            divtimetype.Visible = true;
            //DivFPVNDetails.Visible = true;
            DivFPVNAccDetails.Visible = false;
            chkVIP.Visible = true;
        }
        else
        {

            chkVIP.Visible = false;
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
            divtimetype.Visible = true;
            //DivFPVNDetails.Visible = true;
            DivFPVNAccDetails.Visible = false;
            chkVIP.Visible = true;
        }
        else
        {
            rbtnTypeFP.SelectedValue = "A";
            chkVIP.Visible = false;
            //divddlaccessories.Visible = true;
            divddlaccessoriesType.Visible = true;
            DivFPVNDetails.Visible = false;
            DivFPVNAccDetails.Visible = true;
            divtimetype.Visible = false;
            divddlVehicle.Visible = false;
        }
    }
    #endregion   

    #region Bind Vehicle
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

        if (btnSubmitFP.Text == "Submit")
        {
            InsertPrice();
        }
        else
        {
            UpdatePrice();
        }
    }
    #endregion

    #region Insert 
    public void InsertPrice()
    {
        try
        {
            string timeType = string.Empty;
            string userMode = string.Empty;
            string TotalAmount = string.Empty;

            if (rbtnTypeFP.SelectedValue == "V")
            {
                timeType = "H,EH,D,ED,H,EH,D,ED";
                userMode = "N,N,N,N,V,V,V,V";

                if (chkVIP.Checked == true)
                {
                    TotalAmount = txtNHtotalamountFP.Text + "," + txtEHTotalAmount.Text + "," + txtNDtotal.Text + ","
                        + txtNEDTotal.Text + ","
                   + txtVHtotalAmt.Text + "," + txtVEHtotalamt.Text + "," + txtVDTotal.Text + ","
                   + txtVEDTotal.Text;
                }
                else
                {
                    TotalAmount = txtNHtotalamountFP.Text + "," + txtEHTotalAmount.Text + "," + txtNDtotal.Text + ","
                       + txtNEDTotal.Text + ",0,0,0,0";
                }
            }
            else
            {
                userMode = "";
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
                    floorId = ViewState["floorId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    idType = rbtnTypeFP.SelectedValue,
                    vehicle_accessories = lblvcvalueFP.Text,
                    graceTime = txtGraceTime.Text == "" ? "0" : txtGraceTime.Text,
                    createdBy = Session["UserId"].ToString(),
                    priceDetails = GetpriceDetails(TotalAmount, timeType, userMode, ddltaxFP.SelectedValue, "A")
                };
                HttpResponseMessage response = client.PostAsJsonAsync("priceMaster1", Insert).Result;
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

    public static List<priceDetails> GetpriceDetails(string totalAmount, string timeType, string userMode,
        string taxId, string activeStatus)
    {
        string[] totalAmounts;
        string[] timeTypes;
        string[] userModes;
        totalAmounts = totalAmount.Split(',');
        timeTypes = timeType.Split(',');
        userModes = userMode.Split(',');
        userModes = userMode.Split(',');
        List<priceDetails> lst = new List<priceDetails>();
        for (int i = 0; i < totalAmounts.Count(); i++)
        {
            lst.AddRange(new List<priceDetails>
            {
                new priceDetails { totalAmount=totalAmounts[i] , timeType=timeTypes[i] ,userMode=userModes[i] ,
                    taxId=taxId, activeStatus=activeStatus}

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
            string timeType = string.Empty;
            string userMode = string.Empty;
            string TotalAmount = string.Empty;
            if (rbtnTypeFP.SelectedValue == "V")
            {
                timeType = "H,EH,D,ED,H,EH,D,ED";
                userMode = "N,N,N,N,V,V,V,V";

                if (chkVIP.Checked == true)
                {
                    TotalAmount = txtNHtotalamountFP.Text + "," + txtEHTotalAmount.Text + "," + txtNDtotal.Text + ","
                        + txtNEDTotal.Text + "," + txtVHtotalAmt.Text + "," + txtVEHtotalamt.Text + "," + txtVDTotal.Text + ","
                   + txtVEDTotal.Text;
                }
                else
                {
                    TotalAmount = txtNHtotalamountFP.Text + "," + txtEHTotalAmount.Text + "," + txtNDtotal.Text + ","
                       + txtNEDTotal.Text + ",0,0,0,0";
                }
            }
            else
            {
                userMode = "";
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
                var Update = new PriceMasterClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    floorId = ViewState["floorId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    idType = rbtnTypeFP.SelectedValue,
                    vehicle_accessories = lblvcvalueFP.Text,
                    graceTime = txtGraceTime.Text == "" ? "0" : txtGraceTime.Text,
                    updatedBy = Session["UserId"].ToString()

                };
                if (rbtnTypeFP.SelectedValue == "V")
                {
                    Update.priceDetails = PutpriceDetails(TotalAmount, timeType, userMode, ddltaxFP.SelectedValue, "A",
                    Session["hfpriceId"].ToString());
                }
                if (rbtnTypeFP.SelectedValue == "A")
                {
                    Update.priceDetails = PutpriceDetails(TotalAmount, timeType, userMode, ddltaxFP.SelectedValue, "A",
                      HfpriceId.Value);
                }
                HttpResponseMessage response = client.PutAsJsonAsync("priceMaster1", Update).Result;
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
    public static List<priceDetails> PutpriceDetails(string totalAmount, string timeType, string userMode,
       string taxId, string activeStatus, string priceId
   )
    {
        string[] totalAmounts;
        string[] timeTypes;
        string[] userModes;
        string[] priceIds;
        totalAmounts = totalAmount.Split(',');
        timeTypes = timeType.Split(',');
        userModes = userMode.Split(',');
        userModes = userMode.Split(',');
        priceIds = priceId.Split(',');
        List<priceDetails> lst = new List<priceDetails>();
        for (int i = 0; i < totalAmounts.Count(); i++)
        {
            lst.AddRange(new List<priceDetails>
            {
                new priceDetails { totalAmount=totalAmounts[i] , timeType=timeTypes[i] ,userMode=userModes[i] ,
                    taxId=taxId, activeStatus=activeStatus , priceId=priceIds[i]}

            });
        }

        return lst;

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
        if(rbtnTypeFP.SelectedValue == "V")
        {
            DivFPVNDetails.Visible = true;
        }       
        NormalAmountCalcFP();
        NormalExtAmountCalcFP();
        VIPAmountCalcFP();
        VIPExtAmountCalcFP();
        NormalAccAmountCalcFP();
    }

    protected void txtNHtotalamountFP_TextChanged(object sender, EventArgs e)
    {
        NormalAmountCalcFP();       
    }
    protected void txtEHTotalAmount_TextChanged(object sender, EventArgs e)
    {
        NormalExtAmountCalcFP();
    }
    protected void txtVHtotalAmt_TextChanged(object sender, EventArgs e)
    {
        VIPAmountCalcFP();
    }
    protected void txtVEHtotalamt_TextChanged(object sender, EventArgs e)
    {
        VIPExtAmountCalcFP();
    }
    protected void txtNAcctotalamt_TextChanged(object sender, EventArgs e)
    {
        NormalAccAmountCalcFP();
    }
    public void NormalAmountCalcFP()
    {
        try
        {
            if (txtNHtotalamountFP.Text != null && txtNHtotalamountFP.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtNHtotalamountFP.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtNHAmountFP.Text = totalAmount;
                txtNHTaxFP.Text = Tax.ToString("0.0");
                txtEHTotalAmount.Focus();
            }
            if (txtNDtotal.Text != null && txtNDtotal.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtNDtotal.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtNDAmt.Text = totalAmount;
                txtNDTax.Text = Tax.ToString("0.0");
                txtNEDTotal.Focus();               
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void NormalExtAmountCalcFP()
    {
        try
        {
            if (txtEHTotalAmount.Text != null && txtEHTotalAmount.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtEHTotalAmount.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtNEHamount.Text = totalAmount;
                txtNEHtaxamt.Text = Tax.ToString("0.0");
                txtNDtotal.Focus();
            }
            if (txtNEDTotal.Text != null && txtNEDTotal.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtNEDTotal.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtNEDAmt.Text = totalAmount;
                txtNEDTax.Text = Tax.ToString("0.0");
                txtVHtotalAmt.Focus();
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void VIPAmountCalcFP()
    {
        try
        {
            if (txtVHtotalAmt.Text != null && txtVHtotalAmt.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtVHtotalAmt.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtVHAmt.Text = totalAmount;
                txtVHTaxamt.Text = Tax.ToString("0.0");
                txtVEHtotalamt.Focus();
            }
            if (txtVDTotal.Text != null && txtVDTotal.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtVDTotal.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtVDAmt.Text = totalAmount;
                txtVDTax.Text = Tax.ToString("0.0");
                txtVEDTotal.Focus();
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void VIPExtAmountCalcFP()
    {
        try
        {
            if (txtVEHtotalamt.Text != null && txtVEHtotalamt.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtVEHtotalamt.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtVEHAmt.Text = totalAmount;
                txtVEHtaxAmt.Text = Tax.ToString("0.0");
                txtVDTotal.Focus();
            }
            if (txtVEDTotal.Text != null && txtVEDTotal.Text != "" && Convert.ToInt32(ddltaxFP.SelectedValue) != 0)
            {
                string[] tax = ddltaxFP.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                decimal Amount = Convert.ToDecimal(txtVEDTotal.Text);
                decimal TaxPercent = Convert.ToDecimal(taxPt.Trim());

                decimal Tax = (Math.Round(Amount * TaxPercent / 100));
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString("0.0");
                txtVEDAmt.Text = totalAmount;
                txtVEDTax.Text = Tax.ToString("0.0");
                btnSubmitFP.Focus();
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
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
    }
    public void FPCancel()
    {
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
        txtNHtotalamountFP.Text = "";
        txtNEHamount.Text = "";
        txtNHTaxFP.Text = "";
        txtNEHtaxamt.Text = "";
        txtNHAmountFP.Text = "";
        txtEHTotalAmount.Text = "";
        txtVHAmt.Text = "";
        txtVHTaxamt.Text = "";
        txtGraceTime.Text = "";
        txtVHtotalAmt.Text = "";
        txtVEHtaxAmt.Text = "";
        txtVEHtotalamt.Text = "";
        txtVEHAmt.Text = "";
        txtNAcctotalamt.Text = "";
        txtNAcctaxamt.Text = "";
        txtNAccAmt.Text = "";
        txtNDAmt.Text = "";
        txtNDtotal.Text = "";
        txtNDTax.Text = "";
        txtVDAmt.Text = "";
        txtVDTax.Text = "";
        txtVDTotal.Text = "";
        txtNEDAmt.Text = "";
        txtNEDTax.Text = "";
        txtNEDTotal.Text = "";
        txtVEDAmt.Text = "";
        txtVEDTax.Text = "";
        txtVEDTotal.Text = "";
        DivFPVNAccDetails.Visible = false;
        DivFPVNDetails.Visible = false;
    }
    #endregion

    #region Check VIP Visible
    protected void chkVIP_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVIP.Checked == false)
        {
            DivVIP.Visible = false;
        }
        else
        {
            DivVIP.Visible = true;
        }
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
            divddlVehicle.Visible = true;
            //divddlaccessories.Visible = false;
            divddlaccessoriesType.Visible = false;
            divtimetype.Visible = true;
            //DivFPVNDetails.Visible = true;
            DivFPVNAccDetails.Visible = false;
            chkVIP.Visible = true;
        }
        else
        {
            chkVIP.Visible = false;
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
            spAddorEditFP.InnerText = "Edit ";
            FPADD();
            if (lblgvidType.Text == "V")
            {
                divddlVehicle.Visible = true;
                ddlVehicleFP.SelectedValue = lblgvvehicleAccessories.Text;
            }
            else
            {
                //divddlaccessories.Visible = true;
                divddlaccessoriesType.Visible = true;

                ddlaccessoriesFP.SelectedValue = lblgvvehicleAccessories.Text;
            }

            var dataList = gvrow.FindControl("dlVPriceDetails") as DataList;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataList.Items.Count; i++)
            {
                string hfid = string.Empty;
                Label lbllblgvpriceIdFPV = dataList.Items[i].FindControl("lblgvpriceIdFPV") as Label;
                Label lblgvtotalAmountFPV = dataList.Items[i].FindControl("lblgvtotalAmountFPV") as Label;
                Label lblgvuserModeFPV = dataList.Items[i].FindControl("lblgvuserModeFPV") as Label;
                Label lblgvtaxFPV = dataList.Items[i].FindControl("lblgvtaxFPV") as Label;
                Label lblgvtimeTypeFPV = dataList.Items[i].FindControl("lblgvtimeTypeFPV") as Label;
                Label lblgvamountFPV = dataList.Items[i].FindControl("lblgvamountFPV") as Label;

                if (lblgvuserModeFPV.Text.Trim() == "N" & lblgvtimeTypeFPV.Text.Trim() == "H")
                {
                    txtNHtotalamountFP.Text = lblgvtotalAmountFPV.Text;
                    txtNHTaxFP.Text = lblgvtaxFPV.Text;
                    txtNHAmountFP.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }

                if (lblgvuserModeFPV.Text.Trim() == "N" & lblgvtimeTypeFPV.Text.Trim() == "EH")
                {
                    txtEHTotalAmount.Text = lblgvtotalAmountFPV.Text;
                    txtNEHtaxamt.Text = lblgvtaxFPV.Text;
                    txtNEHamount.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }

                if (lblgvuserModeFPV.Text.Trim() == "N" & lblgvtimeTypeFPV.Text.Trim() == "D")
                {
                    txtNDtotal.Text = lblgvtotalAmountFPV.Text;
                    txtNDTax.Text = lblgvtaxFPV.Text;
                    txtNDAmt.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }

                if (lblgvuserModeFPV.Text.Trim() == "N" & lblgvtimeTypeFPV.Text.Trim() == "ED")
                {
                    txtNEDTotal.Text = lblgvtotalAmountFPV.Text;
                    txtNEDTax.Text = lblgvtaxFPV.Text;
                    txtNEDAmt.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }

                if (lblgvuserModeFPV.Text.Trim() == "V" & lblgvtimeTypeFPV.Text.Trim() == "H")
                {
                    txtVHtotalAmt.Text = lblgvtotalAmountFPV.Text;
                    txtVHTaxamt.Text = lblgvtaxFPV.Text;
                    txtVHAmt.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }

                if (lblgvuserModeFPV.Text.Trim() == "V" & lblgvtimeTypeFPV.Text.Trim() == "EH")
                {
                    txtVEHtotalamt.Text = lblgvtotalAmountFPV.Text;
                    txtVEHtaxAmt.Text = lblgvtaxFPV.Text;
                    txtVEHAmt.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }

                if (lblgvuserModeFPV.Text.Trim() == "V" & lblgvtimeTypeFPV.Text.Trim() == "D")
                {
                    txtVDTotal.Text = lblgvtotalAmountFPV.Text;
                    txtVDTax.Text = lblgvtaxFPV.Text;
                    txtVDAmt.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }
                if (lblgvuserModeFPV.Text.Trim() == "V" & lblgvtimeTypeFPV.Text.Trim() == "ED")
                {
                    txtVEDTotal.Text = lblgvtotalAmountFPV.Text;
                    txtVEDTax.Text = lblgvtaxFPV.Text;
                    txtVEDAmt.Text = lblgvamountFPV.Text;
                    hfid = lbllblgvpriceIdFPV.Text + ",";
                }
                Session["hfpriceId"] = sb.Append(hfid);
            }
            if (txtVHtotalAmt.Text == "0.0" & txtVEHtotalamt.Text == "0.0" & txtVDTotal.Text == "0.0" & txtVEDTotal.Text == "0.0")
            {
                chkVIP.Checked = false;
                DivVIP.Visible = false;
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
            Label lblgvusermode = (Label)gvrow.FindControl("lblgvusermodeFP");
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
                chkVIP.Visible = false;
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
                        List<PriceMasterClass> Price = JsonConvert.DeserializeObject<List<PriceMasterClass>>(ResponseMsg);

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
    #region  GV Vehicle RowDateBound 
    protected void gvPricemaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {
            var PriceH = e.Row.DataItem as PriceMasterClass;
            var dataList = e.Row.FindControl("dlVPriceDetails") as DataList;
            dataList.DataSource = PriceH.priceDetails;
            dataList.DataBind();

        }
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
        public List<priceDetails> priceDetails { get; set; }
    }
    public class priceDetails
    {
        public String taxId { get; set; }
        public String priceId { get; set; }
        public String tax { get; set; }
        public String amount { get; set; }
        public String timeType { get; set; }
        public String userMode { get; set; }
        public String totalAmount { get; set; }
        public String activeStatus { get; set; }
        public String remarks { get; set; }

    }
    #endregion
    #endregion

    #region FF      
    #region btnFloorFeatures
    protected void btnFloorFeatures_Click(object sender, EventArgs e)
    {
        divGvFF.Visible = true;
        divFormFF.Visible = false;
        BindFloorFeatures();
        Bindtax();
        divFloorSetup.Visible = true;
        divFloorFeatures.Visible = true;
        divFloorVehicle.Visible = false;
        divFloorPrice.Visible = false;
        divFloorSetup.Style.Add("border-style", "groove");
        divFloorSetup.Style.Add("border-width", "1px");
        divFloorSetup.Style.Add("border-color", "#d6e5b3");
        divFloorSetup.Style.Add("box-shadow", "rgba(17, 17, 26, 0.1) " +
            "0px 4px 16px, rgba(17, 17, 26, 0.1) " +
            "0px 8px 24px, rgba(17, 17, 26, 0.1) " +
            "0px 16px 56px");

    }

    #endregion     
    #region ADD Click
    protected void btnAddFF_Click(object sender, EventArgs e)
    {
        divGvFF.Visible = false;
        divFormFF.Visible = true;
        Bindtax();
        spAddorEditFF.InnerText = "Add ";
    }
    #endregion
    #region Cancel Click
    protected void btnCancelFF_Click(object sender, EventArgs e)
    {
        FFCancel();
    }
    public void FFCancel()
    {
        divGvFF.Visible = true;
        divFormFF.Visible = false;
        txtFeaturenameFF.Text = string.Empty;
        txtdescriptionFF.Text = string.Empty;
        txtamountFF.Text = string.Empty;
        ddltaxFF.ClearSelection();
        btnSubmitFF.Text = "Submit";
        txtTamountFF.Text = "";
        txttaxamountFF.Text = "";
    }
    #endregion
    #region Submit / Insert
    protected void btnSubmitFF_Click(object sender, EventArgs e)
    {
        if (btnSubmitFF.Text == "Submit")
        {
            InsFloorFeatures();
        }
        else
        {
            UpdateFloorFeatures();
        }
    }
    public void InsFloorFeatures()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FloorFeatures Insert = new FloorFeatures()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString().Trim(),
                    branchId = Session["branchId"].ToString().Trim(),
                    floorId = ViewState["floorId"].ToString().Trim(),
                    featureName = txtFeaturenameFF.Text.Trim(),
                    description = txtdescriptionFF.Text.Trim(),
                    totalAmount = txtamountFF.Text.Trim(),
                    taxId = ddltaxFF.SelectedValue.Trim(),
                    createdBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("floorFeatures", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindFloorFeatures();
                        FFCancel();
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
    #region Bind ddl Tax 
    public void Bindtax()
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
                        dt.Columns.Add(new DataColumn("tax", System.Type.GetType("System.String"), "taxName + ' ~ ' + taxPercentage"));

                        if (dt.Rows.Count > 0)
                        {
                            ddltaxFF.DataSource = dt;
                            ddltaxFF.DataValueField = "taxId";
                            ddltaxFF.DataTextField = "tax";
                            ddltaxFF.DataBind();
                        }
                        else
                        {
                            ddltaxFF.DataBind();
                        }
                        ddltaxFF.Items.Insert(0, new ListItem("Select", "0"));
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
    #region Amount Cal
    protected void ddltaxFF_SelectedIndexChanged(object sender, EventArgs e)
    {
        AmountCalcFF();
    }

    public void AmountCalcFF()
    {
        try
        {
            if (txtamountFF.Text != null && txtamountFF.Text != "" && Convert.ToInt32(ddltaxFF.SelectedValue) != 0)
            {
                string[] tax = ddltaxFF.SelectedItem.Text.Split('~');
                string taxPt = tax[1].ToString();
                int Amount = Convert.ToInt32(txtamountFF.Text);
                double TaxPercent = Convert.ToDouble(taxPt.Trim());

                string Tax = (Math.Round(Amount * TaxPercent / 100)).ToString();
                string totalAmount = (Math.Round(Amount - Convert.ToDecimal(Tax))).ToString();
                txtTamountFF.Text = totalAmount;
                txttaxamountFF.Text = Tax;

            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }

    #endregion
    #region Bind GV FloorFeatures
    public void BindFloorFeatures()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim() + "floorFeatures?&floorId=" + ViewState["floorId"].ToString() + "";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        gvFloorFeatures.DataSource = dt;
                        gvFloorFeatures.DataBind();
                        divGvFF.Visible = true;
                    }
                    else
                    {
                        gvFloorFeatures.DataBind();
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
    #region Edit & Update
    protected void imgBtnEditFF_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            Label lblGvBlockId = (Label)gvrow.FindControl("lblGvBlockIdFF");
            Label lblGvFloorId = (Label)gvrow.FindControl("lblGvFloorIdFF");
            Label lblGvFeaturesId = (Label)gvrow.FindControl("lblGvFeaturesIdFF");
            Label lblGvFeatureName = (Label)gvrow.FindControl("lblGvFeatureNameFF");
            Label lblGvDescription = (Label)gvrow.FindControl("lblGvDescriptionFF");
            Label lblGvAmount = (Label)gvrow.FindControl("lblGvAmountFF");
            Label lblGvTaxPercentage = (Label)gvrow.FindControl("lblGvTaxPercentageFF");
            Label lblGvTaxId = (Label)gvrow.FindControl("lblGvTaxIdFF");
            Label lblGvTaxAmount = (Label)gvrow.FindControl("lblGvTaxAmountFF");
            Label lblGvTotalAmount = (Label)gvrow.FindControl("lblGvTotalAmountFF");
            ViewState["floorId"] = lblGvFloorId.Text;
            LinkButton lnkActiveOrInactive = (LinkButton)gvrow.FindControl("lnkActiveOrInactiveFF");
            string sgvFeaturesId = gvFloorFeatures.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["ActiveStatus"] = lnkActiveOrInactive.ToString() == "Active" ? "A" : "D";
            ViewState["FeaturesId"] = sgvFeaturesId.ToString().Trim();
            txtFeaturenameFF.Text = lblGvFeatureName.Text;
            txtdescriptionFF.Text = lblGvDescription.Text;
            txtTamountFF.Text = lblGvAmount.Text;
            txttaxamountFF.Text = lblGvTaxAmount.Text;
            txtamountFF.Text = lblGvTotalAmount.Text;
            Bindtax();
            ddltaxFF.SelectedValue = lblGvTaxId.Text;
            //ViewState["Tax"] = lblGvTaxAmount.Text;
            // txttotalAmountFF.Text = lblGvTotalAmount.Text;
            divGvFF.Visible = false;
            divFormFF.Visible = true;
            btnSubmitFF.Text = "Update";
            spAddorEditFF.InnerText = "Edit ";
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string excep = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + excep + "');", true);
        }
    }
    public void UpdateFloorFeatures()
    {
        try
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Update = new FloorFeatures()
                {
                    floorId = ViewState["floorId"].ToString().Trim(),
                    featuresId = ViewState["FeaturesId"].ToString(),
                    featureName = txtFeaturenameFF.Text.Trim(),
                    description = txtdescriptionFF.Text.Trim(),
                    totalAmount = txtamountFF.Text.Trim(),
                    taxId = ddltaxFF.SelectedValue.Trim(),
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("floorFeatures", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmitFF.Text = "Submit";
                        BindFloorFeatures();
                        FFCancel();
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
    protected void lnkActiveOrInactiveFF_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sFeaturesId = gvFloorFeatures.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactiveFF");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "floorFeatures?featuresId=" + sFeaturesId
                            + "&activeStatus=" + sActiveStatus;

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindFloorFeatures();
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
    #region FloorFeatures Class
    public class FloorFeatures
    {
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string floorId { get; set; }
        public string featuresId { get; set; }
        public string featureName { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
        public string taxId { get; set; }
        public string tax { get; set; }
        public string totalAmount { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
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
                                btnAddFF.Visible = true;
                            }
                            else
                            {
                                btnAddFV.Visible = false;
                                btnAddFP.Visible = false;
                                btnAddFF.Visible = false;
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
