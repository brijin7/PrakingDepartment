using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_Configuration_VehicleSizeConfig : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
              Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (!IsPostBack)
        {
            BindVehicle();
            BindVehicleSizeConfig();
        }
    }
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

                            ddlvehicletype.DataSource = dt;
                            ddlvehicletype.DataValueField = "vehicleConfigId";
                            ddlvehicletype.DataTextField = "vehicleName";
                            ddlvehicletype.DataBind();

                        }
                        else
                        {
                            ddlvehicletype.DataBind();
                        }
                        ddlvehicletype.Items.Insert(0, new ListItem("Select", "0"));
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
    #region BindGridView
    public void BindVehicleSizeConfig()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("vehicleSizeConfigMaster").Result;
                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        if (dt.Rows.Count > 0)
                        {
                            gvVehicleSizeConfig.DataSource = dt;
                            gvVehicleSizeConfig.DataBind();
                            divGridView.Visible = true;
                        }
                        else
                        {
                            gvVehicleSizeConfig.DataBind();
                        }
                    }
                    else
                    {
                        spAddorEdit.InnerText = "Add ";
                        Action CallFnOnAdd = new Action(divFormVisible);
                        CallFnOnAdd.Invoke();
                        divForm.Visible = true;
                        divGridView.Visible = false;
                        divGv.Visible = false;

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
    #region Add Details
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        spAddorEdit.InnerText = "Add ";
        Action CallFnOnAdd = new Action(divFormVisible);
        CallFnOnAdd.Invoke();
        divForm.Visible = true;
        divGridView.Visible = false;
        divGv.Visible = false;

    }
    #endregion
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetInputField();
        Action CallFnOnCancel = new Action(divGvVisible);
        CallFnOnCancel.Invoke();
    }
    public void ResetInputField()
    {
        spAddorEdit.InnerText = "";
        divForm.Visible = false;
        divGridView.Visible = true;
        divGv.Visible = true;
        BindVehicleSizeConfig();
        ddlvehicletype.ClearSelection();
        txtModelName.Text = string.Empty;
        txtLength.Text = string.Empty;
        txtHeight.Text = string.Empty;
        btnSubmit.Text = "Submit";
        ddlvehicletype.Enabled = true;
    }

    #endregion
    #region divFormVisible
    public void divFormVisible()
    {
        divForm.Visible = true;
        divGv.Visible = false;
    }
    #endregion
    #region divGvVisible
    public void divGvVisible()
    {
        divForm.Visible = false;
        divGv.Visible = true;
    }
    #endregion

    #region Insert Function
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            Insert();
        }
        else
        {
            Update();
        }
    }
    public void Insert()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new VehicleSizeConfig()
                {
                    vehicleConfigId = ddlvehicletype.SelectedValue,
                    modelName = txtModelName.Text.Trim(),
                    length = txtLength.Text.Trim(),
                    height = txtHeight.Text.Trim(),
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString().Trim()                 
                };
                HttpResponseMessage response = client.PostAsJsonAsync("vehicleSizeConfigMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ResetInputField();
                        btnSubmit.Visible = true;
                        BindVehicleSizeConfig();

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
        finally
        {


        }
    }
    #endregion
    #region Edit
    protected void LnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            spAddorEdit.InnerText = "Edit ";
            divForm.Visible = true;
            divGv.Visible = false;
            divGridView.Visible = false;

            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string sVehSizeConfigId = gvVehicleSizeConfig.DataKeys[gvrow.RowIndex].Value.ToString();

            Label lblvehicleConfigId = (Label)gvrow.FindControl("lblvehicleConfigId");
            Label lblmodelName = (Label)gvrow.FindControl("lblmodelName");
            Label lblvehiclelength = (Label)gvrow.FindControl("lblvehiclelength");
            Label lblvehicleheight = (Label)gvrow.FindControl("lblvehicleheight");
           
            LinkButton lblActiveStatus = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");        

            ddlvehicletype.SelectedValue = lblvehicleConfigId.Text;
            ddlvehicletype.Enabled = false;
            txtModelName.Text = lblmodelName.Text;
            txtLength.Text = lblvehiclelength.Text;
            txtHeight.Text = lblvehicleheight.Text;

            ViewState["vehicleSizeConfigId"] = sVehSizeConfigId.ToString().Trim();
            ViewState["ActiveStatus"] = lblActiveStatus.Text.ToString() == "Active" ? "A" : "D";
            btnSubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Update Function
    public void Update()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new VehicleSizeConfig()
                {
                    vehicleSizeConfigId = ViewState["vehicleSizeConfigId"].ToString(),
                    modelName = txtModelName.Text.Trim(),
                    length = txtLength.Text.Trim(),
                    height = txtHeight.Text.Trim(),
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("vehicleSizeConfigMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ResetInputField();
                        btnSubmit.Visible = true;

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
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sVehSizeConfigId = gvVehicleSizeConfig.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "vehicleSizeConfigMaster?vehicleSizeConfigId=" + sVehSizeConfigId
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
                        BindVehicleSizeConfig();
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

    #region Vehicle Size Config Class
    public class VehicleSizeConfig
    {
        public string vehicleConfigId { get; set; }
        public string vehicleSizeConfigId { get; set; }
        public string modelName { get; set; }
        public string length { get; set; }
        public string height { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
    }
    #endregion
}