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


public partial class Master_ParkingSlotWeb : System.Web.UI.Page
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
            BindBlockName();
            BindParkingType();
            BindSlotType();
            BindParkinglotforDelete();
        }
        if (IsPostBack)
        {
            ParkingSlot();
        }
        if (Session["UserRole"].ToString() == "E")
        {
            BindMenuAccess();
        }
    }
    #endregion
    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ADD();
    }
    #endregion
    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    #endregion
    #region Cancel Click Fucntion
    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        ddlblock.ClearSelection();
        ddlfloor.ClearSelection();
        ddlSlotStatus.ClearSelection();
        ddlvehicletype.ClearSelection();
        txtrow.Text = "";
        txtcolumn.Text = "";
        ddlparkingtype.ClearSelection();
        txtlaneno.Text = "";
        divslot.Visible = false;

    }
    #endregion
    #region Submit Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertParkingSlot();
        }
        else
        {

        }
    }
    #endregion
    #region Add Click Fucntion
    public void ADD()
    {
        divGv.Visible = false;
        divForm.Visible = true;
        spAdd.InnerText = "Add ";
        BindBlockName();
    }
    #endregion
    #region Bind Block Name
    public void BindBlockName()
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
                                ddlblock.SelectedValue = dt.Rows[0]["blockId"].ToString();
                                ddlblock.Enabled = false;
                                BindFloorName();
                            }
                            else
                            {
                                ddlblock.Enabled = true;
                            }
                        }
                        else
                        {
                            ddlblock.DataBind();
                        }
                        ddlblock.Items.Insert(0, new ListItem("Select", "0"));
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
    #region Bind Floor Function
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
                                ddlfloor.SelectedValue = dt.Rows[0]["floorId"].ToString();
                                ddlfloor.Enabled = false;
                                BindVehicle();
                            }
                            else
                            {
                                ddlfloor.Enabled = true;
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
    #region  Bind Floor
    protected void ddlblock_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindFloorName();
    }
    protected void ddlfloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVehicle();
    }
    #endregion
    #region Bind Vehicle Type
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
                          + "floorVehicleMaster?floorId=" + ddlfloor.SelectedValue + "";
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
                            ddlvehicletype.DataSource = dt;
                            ddlvehicletype.DataValueField = "vehicleType";
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
                        ddlvehicletype.Items.Clear();
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
    #region Bind Parking Type
    public void BindParkingType()
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
                          + "configMaster?configTypename=typeOfparking&activestatus=A";
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
                            ddlparkingtype.DataSource = dt;
                            ddlparkingtype.DataValueField = "configId";
                            ddlparkingtype.DataTextField = "configName";
                            ddlparkingtype.DataBind();
                        }
                        else
                        {
                            ddlparkingtype.DataBind();
                        }
                        ddlparkingtype.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ddlparkingtype.Items.Clear();
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
    #region Get Method
    public void BindParkingSlot()
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
                        + "parkingSlot?activeStatus=A&floorId=";
                sUrl += ddlfloor.SelectedValue
                     + "&typeOfVehicle=";
                sUrl += ddlvehicletype.SelectedValue;
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
                            //var firstItem = lst.ElementAt(0);
                            //var lst1 = firstItem.ParkingSlotDetails.ToList();
                            //DataTable ParkingSlotDetails = ConvertToDataTable(lst1);
                            //ViewState["ParkingSlotDetails"] = ParkingSlotDetails;
                            //txtcolumn.Text = dt.Rows[0]["noOfColumns"].ToString();
                            //txtrow.Text = dt.Rows[0]["noOfRows"].ToString();
                            //ViewState["noOfColumns"] = dt.Rows[0]["noOfColumns"].ToString();
                            //ViewState["noOfRows"] = dt.Rows[0]["noOfRows"].ToString();
                            //ViewState["UpdateActiveStatus"] = dt.Rows[0]["activeStatus"].ToString();
                            //ViewState["UpdatetypeOfParking"] = dt.Rows[0]["typeOfParking"].ToString();
                            //ViewState["UpdatetypeOfVehicle"] = dt.Rows[0]["typeOfVehicle"].ToString();
                            //hfslotcheck.Value = "Y";
                            //ParkingSlot();


                            var firstItem = lst.ElementAt(0);
                            ViewState["parkingLotLineIdslot"] = firstItem.parkingLotLineId;
                            ViewState["typeOfVehicle"] = firstItem.typeOfVehicle;
                            ViewState["noOfRows"] = firstItem.noOfRows;
                            ViewState["noOfColumns"] = firstItem.noOfColumns;
                            ViewState["passageLeftAvailable"] = firstItem.passageLeftAvailable;
                            ViewState["passageRightAvailable"] = firstItem.passageRightAvailable;
                            ViewState["typeOfParking"] = firstItem.typeOfParking;
                            ViewState["activeStatuss"] = firstItem.activeStatus;
                            var lst1 = firstItem.ParkingSlotDetails.ToList();
                            DataTable ParkingSlotDetails = ConvertToDataTable(lst1);
                            ViewState["ParkingSlotDetails"] = ParkingSlotDetails;

                            lblBlockName.Text = dt.Rows[0]["blockName"].ToString();
                            lblFloorName.Text = dt.Rows[0]["NameofFloor"].ToString();
                            lblvehicleName.Text = dt.Rows[0]["vehicleTypeName"].ToString();

                            ViewState["noOfColumns"] = dt.Rows[0]["noOfColumns"].ToString();
                            ViewState["noOfRows"] = dt.Rows[0]["noOfRows"].ToString();
                            txtcolumn.Text = ViewState["noOfColumns"].ToString();
                            txtrow.Text = ViewState["noOfRows"].ToString();

                            hfslotcheck.Value = "Y";
                            ParkingSlot();
                        }
                        else
                        {
                            txtcolumn.Text = "";
                            txtrow.Text = "";
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
    #region Insert Function
    public void InsertParkingSlot()

    {

        divslot.Visible = true;
        divForm.Visible = false;
        divGv.Visible = true;
        try
        {
            InsertParkingSlotArray();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new ParkingSlotClass()
                {
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    blockId = ddlblock.SelectedValue,
                    floorId = ddlfloor.SelectedValue,
                    typeOfVehicle = ddlvehicletype.SelectedValue,
                    noOfRows = txtrow.Text,
                    noOfColumns = txtcolumn.Text,
                    typeOfParking = ddlparkingtype.SelectedValue,
                    activeStatus = "A",
                    createdBy = Session["UserId"].ToString(),
                    ParkingSlotDetails = GetParkingSlotDetails(ViewState["IlaneNumber"].ToString().TrimEnd(','), ViewState["IslotNumber"].ToString().TrimEnd(','),
                                        ViewState["IrowId"].ToString().TrimEnd(','), ViewState["IcolumnId"].ToString().TrimEnd(','),
                                       ViewState["IisChargeUnitAvailable"].ToString().TrimEnd(','), ViewState["IchargePinType"].ToString().TrimEnd(','),
                                       ViewState["IactiveStatus"].ToString().TrimEnd(','), ViewState["IcreatedBy"].ToString().TrimEnd(','))
                };
                HttpResponseMessage response = client.PostAsJsonAsync("parkingSlot", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);

                        BindParkingSlot();
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "gentable()", true);
                        divslot.Visible = true;
                        divForm.Visible = false;
                        divGv.Visible = false;
                        divslotback.Visible = true;
                        divSlotUpdate.Visible = true;


                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        divslot.Visible = false;
                        divslotback.Visible = false;
                        divGv.Visible = false;
                        divForm.Visible = true;
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
    #region Slot Array
    public void InsertParkingSlotArray()
    {
        ViewState["IslotNumber"] = "";
        ViewState["IrowId"] = "";
        ViewState["IcolumnId"] = "";
        ViewState["IisChargeUnitAvailable"] = "";
        ViewState["IchargePinType"] = "";
        ViewState["IactiveStatus"] = "";
        ViewState["IcreatedBy"] = "";
        ViewState["IlaneNumber"] = "";

        int iRowscount = Convert.ToInt32(txtrow.Text);
        int iColumnscount = Convert.ToInt32(txtcolumn.Text);
        int irowcol = iRowscount * iColumnscount;

        for (int i = 1; i <= irowcol; i++)
        {
            int slotNumber = i;
            ViewState["IslotNumber"] += slotNumber.ToString().Trim() + ',';
            string isChargeUnitAvailable = "false";
            ViewState["IisChargeUnitAvailable"] += isChargeUnitAvailable.ToString().Trim() + ',';
            int chargePinType = 0;
            ViewState["IchargePinType"] += chargePinType.ToString().Trim() + ',';
            int activeStatus = Convert.ToInt32(ViewState["NormalconfigId"].ToString());
            ViewState["IactiveStatus"] += activeStatus.ToString().Trim() + ',';
            string createdBy = Session["UserId"].ToString();
            ViewState["IcreatedBy"] += createdBy.ToString().Trim() + ',';
            string laneNumber = "0";
            ViewState["IlaneNumber"] += laneNumber.ToString().Trim() + ',';
        }
        for (int j = 0; j < iRowscount; j++)
        {
            for (int k = 0; k < iColumnscount; k++)
            {
                int columnId = k;
                ViewState["IcolumnId"] += columnId.ToString().Trim() + ',';
                int rowId = j;
                ViewState["IrowId"] += rowId.ToString().Trim() + ',';
            }

        }
    }
    #endregion
    #region Insert ParkingSlotDetails Function
    public static List<ParkingSlotDetails> GetParkingSlotDetails(string laneNumber, string slotNumber, string rowId, string columnId,
        string isChargeUnitAvailable, string chargePinType, string activeStatus, string createdBy)
    {
        string[] laneNumbers;
        string[] slotNumbers;
        string[] rowIds;
        string[] columnIds;
        string[] isChargeUnitAvailables;
        string[] chargePinTypes;
        string[] activeStatuss;
        string[] createdBys;
        laneNumbers = laneNumber.Split(',');
        slotNumbers = slotNumber.Split(',');
        rowIds = rowId.Split(',');
        columnIds = columnId.Split(',');
        isChargeUnitAvailables = isChargeUnitAvailable.Split(',');
        chargePinTypes = chargePinType.Split(',');
        activeStatuss = activeStatus.Split(',');
        createdBys = createdBy.Split(',');
        List<ParkingSlotDetails> lst = new List<ParkingSlotDetails>();
        for (int i = 0; i < activeStatuss.Count(); i++)
        {
            lst.AddRange(new List<ParkingSlotDetails>
            {
                new ParkingSlotDetails { laneNumber=laneNumbers[i] , slotNumber=slotNumbers[i] ,rowId=rowIds[i],
                    columnId=columnIds[i],isChargeUnitAvailable=isChargeUnitAvailables[i] ,
                    chargePinType=chargePinTypes[i],activeStatus=activeStatuss[i],createdBy=createdBys[i]}

            });
        }

        return lst;

    }
    #endregion
    #region Slot Create C#
    public void ParkingSlot()
    {
        PlaceHolder1.Controls.Clear();
        int tblRows;
        int tblCols;
        if (txtrow.Text != "")
        {
            if (hfslotcheck.Value == "Y")
            {
                tblRows = Convert.ToInt32(ViewState["noOfRows"].ToString().Trim());
                tblCols = Convert.ToInt32(ViewState["noOfColumns"].ToString().Trim());
            }
            else
            {
                tblRows = 0;
                tblCols = 0;
            }

        }
        else
        {
            tblRows = 0;
            tblCols = 0;
        }
        Table tbl = new Table();
        PlaceHolder1.Controls.Add(tbl);

        for (int i = 0; i < tblRows; i++)
        {
            TableRow tr = new TableRow();
            for (int j = 0; j < tblCols; j++)
            {


                TableCell tc = new TableCell();
                //Button txtBox = new Button();
                CheckBox txtBox = new CheckBox();


                txtBox.Text = i + "-" + j;
                txtBox.EnableViewState = true;

                txtBox.ID = i + "-" + j;
                if (hfslotcheck.Value == "Y")
                {
                    ActiveColor(i, j);
                    txtBox.Attributes.Add("class", "tdsactive");

                    string slot = ViewState["SlotactiveStatus"].ToString();

                    switch (slot)
                    {
                        case "DeActive":
                            txtBox.Style.Add("background", "red");
                            break;
                        case "Path":
                        case "PathV":
                            txtBox.Style.Add("background", "#747474");
                            break;
                        case "Way":
                            txtBox.Style.Add("background", "#cebebe");
                            break;
                        case "Lane No":
                        case "Lane Top":
                            txtBox.Style.Add("background", "#24222238");
                            break;
                        case "In":
                            txtBox.Style.Add("background", "#00d4ff");
                            break;
                        case "Out":
                            txtBox.Style.Add("background", "#1ba2bd");
                            break;
                        case "VIP":
                            txtBox.Style.Add("background", "#ffee2162");
                            break;
                        case "Reserved":
                        case "VIP Reserved":
                            txtBox.Style.Add("background", "#DFF6FF");
                            break;
                        default:
                            txtBox.Style.Add("background", "#40F44336");
                            break;
                    }

                }
                //txtBox.AutoPostBack = true;
                //txtBox.CheckedChanged += new EventHandler(btnLoadButton_Click);
                this.Form.Controls.Add(txtBox);
                tc.Controls.Add(txtBox);
                tr.Cells.Add(tc);
            }
            tbl.Rows.Add(tr);
        }
        Session["slottable"] = tbl;
        ViewState["dynamictable"] = true;
    }
    #endregion
    #region Slot Update 
    protected void btnSlotUpdate_Click(object sender, EventArgs e)
    {
        if (ddlSlotStatus.SelectedValue == "Select")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Select Slot Type ');", true);
            return;
        }
        if (txtlaneno.Text == "")
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('Enter Lane No');", true);
            return;
        }
        PlaceHolder1.Controls.Clear();
        int tblRows;
        string Id = string.Empty;
        string Final = string.Empty;
        int tblCols;
        tblRows = Convert.ToInt32(ViewState["noOfRows"].ToString().Trim());
        tblCols = Convert.ToInt32(ViewState["noOfColumns"].ToString().Trim());

        Table tbl = (Table)Session["slottable"];

        foreach (TableRow trc in tbl.Rows)
        {
            foreach (TableCell tc in trc.Cells)
            {
                foreach (Control htc in tc.Controls)
                {
                    var cb = htc as CheckBox;
                    if (cb.Checked == true)
                    {
                        Id += cb.Text + ',';
                        string[] slotdetails;
                        slotdetails = cb.Text.Split('-');
                        DataTable dt1 = (DataTable)ViewState["ParkingSlotDetails"];
                        List<ParkingSlotDetails> dt = new List<ParkingSlotDetails>();
                        dt = ConvertDataTable1<ParkingSlotDetails>(dt1);

                        var dtfinal = dt.Where(x => x.rowId == slotdetails[0] && x.columnId == slotdetails[1]).Select(x =>
                        new
                        {
                            parkingSlotId = x.parkingSlotId,
                            parkingLotLineId = x.parkingLotLineId,
                            slotNumber = x.slotNumber,
                            rowId = x.rowId,
                            columnId = x.columnId,
                            isChargeUnitAvailable = x.isChargeUnitAvailable,
                            chargePinType = x.chargePinType,
                            activeStatus = x.activeStatus,
                            createdBy = x.createdBy
                        }).ToList();
                        DataTable final = ConvertToDataTable(dtfinal);
                        ViewState["parkingSlotId"] += final.Rows[0]["parkingSlotId"].ToString() + ',';
                        ViewState["parkingLotLineId"] += final.Rows[0]["parkingLotLineId"].ToString() + ',';
                        ViewState["slotNumber"] += final.Rows[0]["slotNumber"].ToString() + ',';
                        ViewState["rowId"] += final.Rows[0]["rowId"].ToString() + ',';
                        ViewState["columnId"] += final.Rows[0]["columnId"].ToString() + ',';
                        ViewState["isChargeUnitAvailable"] += final.Rows[0]["isChargeUnitAvailable"].ToString() + ',';
                        ViewState["chargePinType"] += final.Rows[0]["chargePinType"].ToString() + ',';
                        ViewState["activeStatus"] += ddlSlotStatus.SelectedValue + ',';
                        ViewState["laneNumber"] += txtlaneno.Text + ',';
                        ViewState["updatedBy"] += Session["UserId"].ToString() + ',';
                    }

                }

            }

        }

        string[] slotIds;
        string[] parkingSlotIds;
        parkingSlotIds = ViewState["parkingSlotId"].ToString().TrimEnd(',').Split(',');
        string[] parkingLotLineIds;
        parkingLotLineIds = ViewState["parkingLotLineId"].ToString().TrimEnd(',').Split(',');
        string[] slotNumbers;
        slotNumbers = ViewState["slotNumber"].ToString().TrimEnd(',').Split(',');
        slotIds = Id.TrimEnd(',').Split(',');
        string[] Ischargeuniavailable;
        Ischargeuniavailable = ViewState["isChargeUnitAvailable"].ToString().TrimEnd(',').Split(',');
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new ParkingSlotClassupdate()
                {
                    parkingLotLineId = ViewState["parkingLotLineIdslot"].ToString(),
                    branchId = Session["branchId"].ToString(),
                    blockId = ddlblock.SelectedValue,
                    floorId = ddlfloor.SelectedValue,
                    parkingOwnerId = Session["parkingOwnerId"].ToString(),
                    typeOfVehicle = ViewState["typeOfVehicle"].ToString().Trim(),
                    noOfRows = txtrow.Text,
                    noOfColumns = txtcolumn.Text,
                    activeStatus = ViewState["activeStatuss"].ToString(),
                    updatedBy = Session["UserId"].ToString(),
                    typeOfParking = ViewState["typeOfParking"].ToString(),
                    ParkingSlotDetailsupdate = GetParkingSlotDetails(ViewState["laneNumber"].ToString().TrimEnd(','),
                    ViewState["parkingSlotId"].ToString().TrimEnd(','), ViewState["parkingLotLineId"].ToString().TrimEnd(',')
                    , ViewState["slotNumber"].ToString().TrimEnd(','), ViewState["rowId"].ToString().TrimEnd(','),
                    ViewState["columnId"].ToString().TrimEnd(','), ViewState["isChargeUnitAvailable"].ToString().TrimEnd(','),
                    ViewState["chargePinType"].ToString().TrimEnd(','), ViewState["activeStatus"].ToString().TrimEnd(','),
                    ViewState["updatedBy"].ToString().TrimEnd(','))
                };
                HttpResponseMessage response = client.PutAsJsonAsync("parkingSlot", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        ViewState["parkingSlotId"] = "";
                        ViewState["parkingLotLineId"] = "";
                        ViewState["LotLineId"] = "";
                        ViewState["slotNumber"] = "";
                        ViewState["laneNumber"] = "";
                        ViewState["rowId"] = "";
                        ViewState["columnId"] = "";
                        ViewState["isChargeUnitAvailable"] = "";
                        ViewState["chargePinType"] = "";
                        ViewState["activeStatus"] = "";
                        ViewState["updatedBy"] = "";
                        //Page.ClientScript.RegisterStartu;pScript(this.GetType(), "CallMyFunction", "gentable()", true);


                    }
                    else
                    {
                        ViewState["parkingSlotId"] = "";
                        ViewState["parkingLotLineId"] = "";
                        ViewState["LotLineId"] = "";
                        ViewState["slotNumber"] = "";
                        ViewState["laneNumber"] = "";
                        ViewState["rowId"] = "";
                        ViewState["columnId"] = "";
                        ViewState["isChargeUnitAvailable"] = "";
                        ViewState["chargePinType"] = "";
                        ViewState["activeStatus"] = "";
                        ViewState["updatedBy"] = "";
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "gentable()", true);
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
            BindParkingSlot();
            ParkingSlot();
        }

    }
    public static List<ParkingSlotDetailsUpdate> GetParkingSlotDetails(string laneNumber,
        string parkingSlotId, string parkingLotLineId, string slotNumber, string rowId,
        string columnId, string isChargeUnitAvailable, string chargePinType,
        string activeStatus, string updatedBy)
    {
        string[] laneNumbers;
        string[] parkingSlotIds;
        string[] parkingLotLineIds;
        string[] slotNumbers;
        string[] rowIds;
        string[] columnIds;
        string[] isChargeUnitAvailables;
        string[] chargePinTypes;
        string[] updatedBys;
        string[] activeStatuss;
        laneNumbers = laneNumber.Split(',');
        parkingSlotIds = parkingSlotId.Split(',');
        parkingLotLineIds = parkingLotLineId.Split(',');
        slotNumbers = slotNumber.Split(',');
        rowIds = rowId.Split(',');
        columnIds = columnId.Split(',');
        isChargeUnitAvailables = isChargeUnitAvailable.Split(',');
        chargePinTypes = chargePinType.Split(',');
        activeStatuss = activeStatus.Split(',');
        updatedBys = updatedBy.Split(',');
        List<ParkingSlotDetailsUpdate> lst = new List<ParkingSlotDetailsUpdate>();
        for (int i = 0; i < activeStatuss.Count(); i++)
        {
            lst.AddRange(new List<ParkingSlotDetailsUpdate>
            {
                new ParkingSlotDetailsUpdate { laneNumber=laneNumbers[i] ,parkingSlotId=parkingSlotIds[i],
                    parkingLotLineId =parkingLotLineIds[i],slotNumber =slotNumbers[i],rowId =rowIds[i],
                columnId =columnIds[i],isChargeUnitAvailable =isChargeUnitAvailables[i],chargePinType =chargePinTypes[i],
                  activeStatus =activeStatuss[i],updatedBy =updatedBys[i]}

            });
        }

        return lst;

    }
    #endregion
    #region Slot Click
    private static List<T> ConvertDataTable1<T>(DataTable dt)
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
    #endregion
    #region Bind Slot Types
    public void BindSlotType()
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
                          + "configMaster?activestatus=A&configTypename=slotType";
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
                            string Name = "Normal";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (Name == dt.Rows[i]["configName"].ToString())
                                {
                                    ViewState["NormalconfigId"] = dt.Rows[i]["configId"].ToString();
                                }

                            }

                            ddlSlotStatus.DataSource = dt;
                            ddlSlotStatus.DataValueField = "configId";
                            ddlSlotStatus.DataTextField = "configName";
                            ddlSlotStatus.DataBind();

                        }
                        else
                        {

                            ddlSlotStatus.DataBind();
                        }
                        ddlSlotStatus.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + ResponseMsg.ToString().Trim() + "');", true);

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
    #region Slot Active Color
    public void ActiveColor(int Row, int Col)
    {
        string Rows = Convert.ToString(Row);
        string Cols = Convert.ToString(Col);
        DataTable dt1 = (DataTable)ViewState["ParkingSlotDetails"];
        List<ParkingSlotDetails> dt = new List<ParkingSlotDetails>();
        dt = ConvertDataTable1<ParkingSlotDetails>(dt1);

        var dtfinals = dt.Where(x => x.rowId == Rows && x.columnId == Cols).Select(x =>
        new
        {
            slotNumber = x.slotNumber,
            slotType = x.slotType,
            parkingLotLineId = x.parkingLotLineId
        }).ToList();
        DataTable finals = ConvertToDataTable(dtfinals);
        ViewState["SlotactiveStatus"] = finals.Rows[0]["slotType"].ToString();
        ViewState["LotLineId"] = finals.Rows[0]["parkingLotLineId"].ToString();
        ViewState["SlotNumber"] = finals.Rows[0]["slotNumber"].ToString();
    }
    #endregion
    #region Bind Grid 
    public void BindParkinglotforDelete()
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
                        + "parkingSlot?activeStatus=A&branchId=";
                sUrl += Session["branchId"].ToString();
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
                            gvparkingslot.DataSource = dt;
                            gvparkingslot.DataBind();

                        }
                        else
                        {
                            gvparkingslot.DataSource = null;
                            gvparkingslot.DataBind();
                        }
                    }
                    else
                    {
                        ADD();
                        gvparkingslot.DataBind();
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
    #region Delete click
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                ImageButton lnkbtn = sender as ImageButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string sParkingLotLineId = gvparkingslot.DataKeys[gvrow.RowIndex].Value.ToString();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "parkingSlot?activestatus=D&parkingLotLineId=" + sParkingLotLineId;
                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindParkinglotforDelete();
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
    #region Edit Click
    protected void LnkSlotEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;

            Label lblgvfloorId = (Label)gvrow.FindControl("lblgvfloorId");
            Label lblgvblockId = (Label)gvrow.FindControl("lblgvblockId");
            Label lblgvvehicleTypeName = (Label)gvrow.FindControl("lblgvvehicleTypeName");
            Label lbltypeOfVehicle = (Label)gvrow.FindControl("lbltypeOfVehicle");
            Label lblgvblockName = (Label)gvrow.FindControl("lblgvblockName");
            Label lblgvNameofFloor = (Label)gvrow.FindControl("lblgvNameofFloor");

            lblBlockName.Text = lblgvblockName.Text;
            lblFloorName.Text = lblgvNameofFloor.Text;
            lblvehicleName.Text = lblgvvehicleTypeName.Text;

            ddlblock.SelectedValue = lblgvblockId.Text;
            BindFloorName();
            ddlfloor.SelectedValue = lblgvfloorId.Text;
            //txtlaneno.Text = lblgvlaneNumber.Text;
            BindVehicle();
            ddlvehicletype.SelectedValue = lbltypeOfVehicle.Text;
            string sgvparkingslot = gvparkingslot.DataKeys[gvrow.RowIndex].Value.ToString();
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

                sUrl += ddlvehicletype.SelectedValue;
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
                            ViewState["parkingLotLineIdslot"] = firstItem.parkingLotLineId;
                            ViewState["typeOfVehicle"] = firstItem.typeOfVehicle;
                            ViewState["noOfRows"] = firstItem.noOfRows;
                            ViewState["noOfColumns"] = firstItem.noOfColumns;
                            ViewState["passageLeftAvailable"] = firstItem.passageLeftAvailable;
                            ViewState["passageRightAvailable"] = firstItem.passageRightAvailable;
                            ViewState["typeOfParking"] = firstItem.typeOfParking;
                            ViewState["activeStatuss"] = firstItem.activeStatus;
                            var lst1 = firstItem.ParkingSlotDetails.ToList();
                            DataTable ParkingSlotDetails = ConvertToDataTable(lst1);
                            ViewState["ParkingSlotDetails"] = ParkingSlotDetails;
                            txtcolumn.Text = dt.Rows[0]["noOfColumns"].ToString();
                            txtrow.Text = dt.Rows[0]["noOfRows"].ToString();
                            ViewState["noOfColumns"] = dt.Rows[0]["noOfColumns"].ToString();
                            ViewState["noOfRows"] = dt.Rows[0]["noOfRows"].ToString();
                            hfslotcheck.Value = "Y";
                            ParkingSlot();

                            spAddorEdit.InnerText = "Edit ";
                            divslot.Visible = true;
                            divForm.Visible = false;
                            divGv.Visible = false;
                            divSlotUpdate.Visible = true;
                            divslotback.Visible = true;
                        }
                        else
                        {
                            txtcolumn.Text = "";
                            txtrow.Text = "";
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
            string[] excp = ex.Message.Replace("'", "").Split('.');
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + excp[0].Trim() + "');", true);
        }
    }
    #endregion
    #region Slot Back Click
    protected void btnslotback_Click(object sender, EventArgs e)
    {
        divslot.Visible = false;
        divForm.Visible = false;
        divSlotUpdate.Visible = false;
        divGv.Visible = true;
        divslotback.Visible = false;
        Cancel();
        BindParkinglotforDelete();
    }
    #endregion
    #region Parking Slot Class
    public class ParkingSlotClass
    {
        public string parkingLotLineId { get; set; }
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
        public string updatedBy { get; set; }
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
        public string isChargeUnitAvailable { get; set; }
        public string chargePinType { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
    }
    public class ParkingSlotClassupdate
    {
        public string parkingLotLineId { get; set; }
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
        public string activeStatus { get; set; }
        public string updatedBy { get; set; }
        public List<ParkingSlotDetailsUpdate> ParkingSlotDetailsupdate { get; set; }

    }

    public class ParkingSlotDetailsUpdate
    {
        public string parkingSlotId { get; set; }
        public string parkingLotLineId { get; set; }
        public string laneNumber { get; set; }
        public string slotNumber { get; set; }
        public string rowId { get; set; }
        public string columnId { get; set; }
        public string isChargeUnitAvailable { get; set; }
        public string chargePinType { get; set; }
        public string activeStatus { get; set; }
        public string updatedBy { get; set; }

    }
    #endregion
    #region Single Slot Update
    protected void btnSingleUpdateSlot_Click(object sender, EventArgs e)
    {
        Session["blockSingle"] = ddlblock.SelectedValue;
        Session["floorSingle"] = ddlfloor.SelectedValue;
        Session["blocknameSingle"] = ddlblock.SelectedItem.Text;
        Session["typeOfVehiclenameSingle"] = ddlvehicletype.SelectedItem.Text;
        Session["floornameSingle"] = ddlfloor.SelectedItem.Text;
        Session["typeOfVehicleSingle"] = ddlvehicletype.SelectedValue;
        Response.Redirect("ParkingSlotSingle.aspx", false);
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
                        var Option = lst1.Where(x => x.optionName == "slot" && x.MenuOptionAccessActiveStatus == "A")
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
                                    gvparkingslot.Columns[7].Visible = true;
                                }
                                else
                                {
                                    gvparkingslot.Columns[7].Visible = false;
                                }
                                if (Delete[0] == "True")
                                {
                                    gvparkingslot.Columns[8].Visible = true;
                                }
                                else
                                {
                                    gvparkingslot.Columns[8].Visible = false;
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
    #endregion
}