using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_Floor_FloorMaster : System.Web.UI.Page
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
            BindGvFloor();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion
    #region Bind GridView
    public void BindGvFloor()
    {
        try
        {
            divForm.Visible = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim() + "floorMaster?branchId=" + Session["BranchId"].ToString() + "";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var vehicleEditresponse = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(vehicleEditresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(vehicleEditresponse)["response"].ToString();

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
                            gvFloorDetails.DataSource = dt;
                            gvFloorDetails.DataBind();
                            divGv.Visible = true;
                        }
                        else
                        {
                            gvFloorDetails.DataBind();
                        }
                    }

                    else
                    {
                        divAddMasters.Visible = false;
                        Action CallFnOnAdd = new Action(divFormVisible);
                        CallFnOnAdd += Clear;
                        CallFnOnAdd.Invoke();

                        Thread T2 = new Thread(BindDdlBlock);
                        Thread T3 = new Thread(BindDdlFloorName);
                        Thread T4 = new Thread(BindDdlFloorType);
                        T2.Start(); T3.Start(); T4.Start();
                        T2.Join(); T3.Join(); T4.Join();
                        spAddorEdit.InnerText = "Add ";
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
    #region Branch

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
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlBlock.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region FloorName
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
                      + "configMaster?configTypename=floorName&activestatus=A";
                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtFloorName = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlFloorName.DataSource = dtFloorName;
                        ddlFloorName.DataValueField = "configId";
                        ddlFloorName.DataTextField = "configName";
                        ddlFloorName.DataBind();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlFloorName.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region FloorType
    public void BindDdlFloorType()
    {
        try
        {
            using (var client = new HttpClient())
            {
                ddlFloorType.Items.Clear();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
              + "configMaster?activestatus=A&configTypename=floorType";

                HttpResponseMessage response = client.GetAsync(sUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();
                    if (StatusCode == 1)
                    {
                        DataTable dtFloorType = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        ddlFloorType.DataSource = dtFloorType;
                        ddlFloorType.DataValueField = "configId";
                        ddlFloorType.DataTextField = "configName";
                        ddlFloorType.DataBind();

                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                    }
                    ddlFloorType.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    #endregion
    #region Insert
    public void Insert()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                FloorMaster Insert = new FloorMaster()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    blockId = ddlBlock.SelectedValue.Trim(),
                    floorName = ddlFloorName.SelectedValue.Trim(),
                    floorType = ddlFloorType.SelectedValue.Trim(),
                    squareFeet = txtSquarefeet.Text.Trim(),
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PostAsJsonAsync("floorMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        Action InsertSuccess = new Action(Clear);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        InsertSuccess.Invoke();
                        BindGvFloor();
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
    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text.Trim() == "Submit")
        {
            Action InsertCall = new Action(Insert);
            InsertCall.Invoke();
        }
        else
        {
            Action UpdateCall = new Action(Update);
            UpdateCall.Invoke();
        }
    }
    #endregion
    #region Clear 
    public void Clear()
    {
        spAddorEdit.InnerText = "";
        btnSubmit.Text = "Submit";
        ddlBlock.Enabled = true;
        ddlBlock.ClearSelection();
        ddlFloorName.Enabled = true;
        ddlFloorName.ClearSelection();
        ddlFloorType.ClearSelection();
        txtSquarefeet.Text = string.Empty;
        ViewState["sFloorId"] = "";
        ViewState["ActiveStatus"] = "";
        divAddMasters.Visible = false;
    }
    #endregion
    #region Add Details
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        divAddMasters.Visible = false;
        Action CallFnOnAdd = new Action(divFormVisible);
        CallFnOnAdd += Clear;
        CallFnOnAdd.Invoke();

        Thread T2 = new Thread(BindDdlBlock);
        Thread T3 = new Thread(BindDdlFloorName);
        Thread T4 = new Thread(BindDdlFloorType);
        T2.Start(); T3.Start(); T4.Start();
        T2.Join(); T3.Join(); T4.Join();
        spAddorEdit.InnerText = "Add ";
    }
    #endregion
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Action CallFnOnCancel = new Action(divGvVisible);
        CallFnOnCancel += Clear;
        CallFnOnCancel.Invoke();
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
    #region Update
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

                FloorMaster Insert = new FloorMaster()
                {
                    floorId = Convert.ToInt32(ViewState["sFloorId"].ToString()),
                    floorName = ddlFloorName.SelectedValue.Trim(),
                    floorType = ddlFloorType.SelectedValue.Trim(),
                    squareFeet = txtSquarefeet.Text.Trim(),
                    activeStatus = ViewState["ActiveStatus"].ToString(),
                    updatedBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("floorMaster", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        Action InsertSuccess = new Action(Clear);
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        InsertSuccess.Invoke();
                        BindGvFloor();
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
    #region Edit
    protected void LnkEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //divAddMasters.Visible = true;
            spAddorEdit.InnerText = "Edit ";
            btnSubmit.Text = "Update";
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblblockId = (Label)gvrow.FindControl("lblblockId");
            BindDdlBlock();
            ddlBlock.SelectedValue = lblblockId.Text;

            Label lblsquareFeet = (Label)gvrow.FindControl("lblsquareFeet");
            txtSquarefeet.Text = lblsquareFeet.Text;
            Label lblfloorNameId = (Label)gvrow.FindControl("lblfloorNameId");
            BindDdlFloorName();
            ddlFloorName.SelectedValue = lblfloorNameId.Text;
            LinkButton lblActiveStatus = (LinkButton)gvrow.FindControl("lnkActiveOrInactive");
            string sFloorId = gvFloorDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["sFloorId"] = sFloorId.ToString().Trim();
            ViewState["ActiveStatus"] = lblActiveStatus.Text.ToString() == "Active" ? "A" : "D";
            Label lblfloorType = (Label)gvrow.FindControl("lblfloorType");
            BindDdlFloorType();
            ddlFloorType.SelectedValue = lblfloorType.Text;
            divForm.Visible = true;
            divGv.Visible = false;
            ddlBlock.Enabled = false;
            ddlFloorName.Enabled = false;
        }
        catch (Exception ex)
        {
            string[] msg = ex.Message.ToString().Split('.');
            string Pins = msg[0].Replace("'", string.Empty).Trim();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "erroralert('" + Pins + "');", true);
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
                string sFloorId = gvFloorDetails.DataKeys[gvrow.RowIndex].Value.ToString();
                LinkButton lblActiveStatus = (LinkButton)lnkbtn.FindControl("lnkActiveOrInactive");

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sActiveStatus = lblActiveStatus.Text.Trim() == "Active" ? "D" : "A";
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "floorMaster?floorId=" + sFloorId
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
                        BindGvFloor();
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

    #region btnFloorFeatures
    protected void btnFloorFeatures_Click(object sender, EventArgs e)
    {
        Response.Redirect("FloorFeatures.aspx?floorid=" + ViewState["sFloorId"].ToString() + "", false);
    }
    #endregion
    #region btnFloorvehicleMaster
    protected void btnFloorvehicleMaster_Click(object sender, EventArgs e)
    {
        Response.Redirect("FloorVehicleMaster.aspx?floorid=" + ViewState["sFloorId"].ToString() + "", false);
    }
    #endregion
    #region btnPriceMaster
    protected void btnPriceMaster_Click(object sender, EventArgs e)
    {
        Response.Redirect("PriceMaster.aspx?floorid=" + ViewState["sFloorId"].ToString() + "", false);
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
                        var Option = lst1.Where(x => x.optionName == "floorMaster" && x.MenuOptionAccessActiveStatus == "A")
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
                                btnAdd.Visible = true;
                            }
                            else
                            {
                                btnAdd.Visible = false;
                            }
                            if (View[0] == "True")
                            {
                                divGridView.Visible = true;
                                if (Edit[0] == "True")
                                {
                                    gvFloorDetails.Columns[5].Visible = true;
                                }
                                else
                                {
                                    gvFloorDetails.Columns[5].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvFloorDetails.Columns[6].Visible = true;
                                }
                                else
                                {
                                    gvFloorDetails.Columns[6].Visible = false;
                                }

                            }
                            else
                            {
                                divGridView.Visible = false;
                            }

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
