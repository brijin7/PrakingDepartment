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

public partial class Master_ParkingSlot : System.Web.UI.Page
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
            BindsingleSlot();
            rbtnChargeType.SelectedValue = "false";
        }
        if (IsPostBack)
        {
            ParkingSlot();
        }
    }
    #endregion
    public void BindsingleSlot()
    {
        lblBlockName.Text = Session["blocknameSingle"].ToString();
        lblFloorName.Text = Session["floornameSingle"].ToString();
        lblvehicleName.Text = Session["typeOfVehiclenameSingle"].ToString();
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
                sUrl += Session["floorSingle"].ToString()
                     + "&typeOfVehicle=";
                sUrl += Session["typeOfVehicleSingle"].ToString();
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
                            txtcolumn.Value = dt.Rows[0]["noOfColumns"].ToString();
                            txtrow.Value = dt.Rows[0]["noOfRows"].ToString();
                            ViewState["noOfColumns"] = dt.Rows[0]["noOfColumns"].ToString();
                            ViewState["noOfRows"] = dt.Rows[0]["noOfRows"].ToString();
                            hfslotcheck.Value = "Y";
                            ParkingSlot();

                        }
                        else
                        {
                            txtcolumn.Value = "";
                            txtrow.Value = "";
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

    #region Cancel Click Fucntion
    public void Cancel()
    {
        imgpassageleft.Visible = false;
        imgpassageright.Visible = false;
        imgpassageright.Visible = false;
        divslot.Visible = false;
        divslotlisttitle.Visible = false;

    }
    #endregion

    #region Get Method
    public void BindParkingSlot()
    {
        dydiv.Visible = true;
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
                sUrl += Session["floorSingle"].ToString()
                     + "&typeOfVehicle=";
                sUrl += Session["typeOfVehicleSingle"].ToString();
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
                            txtcolumn.Value = dt.Rows[0]["noOfColumns"].ToString();
                            txtrow.Value = dt.Rows[0]["noOfRows"].ToString();
                            ViewState["noOfColumns"] = dt.Rows[0]["noOfColumns"].ToString();
                            ViewState["noOfRows"] = dt.Rows[0]["noOfRows"].ToString();
                            hfslotcheck.Value = "Y";
                            ParkingSlot();
                        }
                        else
                        {
                            txtcolumn.Value = "";
                            txtrow.Value = "";
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
    #region Slot Create C#
    public void ParkingSlot()
    {

        PlaceHolder1.Controls.Clear();
        int tblRows;
        int tblCols;
        if (txtrow.Value != "")
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
                Button txtBox = new Button();
                txtBox.Text = i + "-" + j;
                txtBox.EnableViewState = true;

                txtBox.ID = i + "-" + j;
                if (hfslotcheck.Value == "Y")
                {
                    ActiveColor(i, j);
                    txtBox.Attributes.Add("class", "tdsactive");
                    string slot = ViewState["SlotactiveStatus"].ToString();
                    txtBox.Text = ViewState["SlotNumber"].ToString();
                  
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

                txtBox.Click += btnLoadButton_Click;
                tc.Controls.Add(txtBox);
                tr.Cells.Add(tc);
            }
            tbl.Rows.Add(tr);
        }
        ViewState["dynamictable"] = true;
    }
    #endregion
    #region Slot Click
    protected void btnLoadButton_Click(object sender, EventArgs e)
    {

        string slot = (sender as Button).ID;
        string[] slotdetails;
        slotdetails = slot.Split('-');
        DataTable dt1 = (DataTable)ViewState["ParkingSlotDetails"];
        List<ParkingSlotDetails> dt = new List<ParkingSlotDetails>();
        dt = ConvertDataTable1<ParkingSlotDetails>(dt1);

        var dtfinal = dt.Where(x => x.rowId == slotdetails[0] && x.columnId == slotdetails[1]).Select(x =>
        new
        {
            parkingSlotId = x.parkingSlotId,
            parkingLotLineId = x.parkingLotLineId,
            laneNumber = x.laneNumber,
            slotNumber = x.slotNumber,
            rowId = x.rowId,
            columnId = x.columnId,
            isChargeUnitAvailable = x.isChargeUnitAvailable,
            chargePinType = x.chargePinType,
            activeStatus = x.activeStatus,
            createdBy = x.createdBy
        }).ToList();
        DataTable final = ConvertToDataTable(dtfinal);
        ViewState["parkingSlotId"] = final.Rows[0]["parkingSlotId"].ToString();
        ViewState["parkingLotLineId"] = final.Rows[0]["parkingLotLineId"].ToString();
        ViewState["laneNumber"] = final.Rows[0]["laneNumber"].ToString();
        ViewState["slotNumber"] = final.Rows[0]["slotNumber"].ToString();
        ViewState["rowId"] = final.Rows[0]["rowId"].ToString();
        ViewState["columnId"] = final.Rows[0]["columnId"].ToString();
        ViewState["isChargeUnitAvailable"] = final.Rows[0]["isChargeUnitAvailable"].ToString();
        ViewState["chargePinType"] = final.Rows[0]["chargePinType"].ToString();
        ViewState["activeStatus"] = final.Rows[0]["activeStatus"].ToString();
        lblslotnumber.Text = ViewState["slotNumber"].ToString();
        ParkingSlot();

        if (ViewState["isChargeUnitAvailable"].ToString().Trim() == "true")
        {
            rbtnChargeType.SelectedValue = "true";
            CharType.Visible = true;
            GetChargePinImage();
            for (int j = 0; j < gvChargePinType.Items.Count; j++)
            {
                Label lblChargePinId = (Label)gvChargePinType.Items[j].FindControl("lblChargePinId");

                if (lblChargePinId.Text == ViewState["chargePinType"].ToString().Trim())
                {

                    Label lnkbtn = (Label)gvChargePinType.Items[j].FindControl("lblChargePinName");
                    lnkbtn.Style.Add("color", "red");

                }
                else
                {
                    Label lnkbtn = (Label)gvChargePinType.Items[j].FindControl("lblChargePinName");
                    lnkbtn.Style.Add("color", "black");

                }
            }
        }
        else
        {
            rbtnChargeType.SelectedValue = "false";
            CharType.Visible = false;
        }

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "show()", true);


    }

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
    #region Update Function
    protected void btnUpdateSlot_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new ParkingSlotDetailsUpdate()
                {
                    parkingSlotId = ViewState["parkingSlotId"].ToString(),
                    parkingLotLineId = ViewState["parkingLotLineId"].ToString(),
                    laneNumber = ViewState["laneNumber"].ToString(),
                    slotNumber = lblslotnumber.Text,
                    rowId = ViewState["rowId"].ToString(),
                    columnId = ViewState["columnId"].ToString(),
                    isChargeUnitAvailable = rbtnChargeType.SelectedValue,
                    chargePinType = txtCharType.Text == "" ? "0" : txtCharType.Text,
                    activeStatus = ViewState["activeStatus"].ToString(),
                    updatedBy = Session["UserId"].ToString()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("postparkingSlot", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindParkingSlot();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "gentable()", true);

                    }
                    else
                    {
                        BindParkingSlot();
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

    }
    #endregion

    #region Slot Clear Click
    protected void btnCanclerule_Click(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "show()", true);
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
            slotType = x.slotType
        }).ToList();
        DataTable finals = ConvertToDataTable(dtfinals);
        if (finals.Rows.Count > 0)
        {
            ViewState["SlotactiveStatus"] = finals.Rows[0]["slotType"].ToString();
            ViewState["SlotNumber"] = finals.Rows[0]["slotNumber"].ToString();
        }
    }
    #endregion
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
        public string isChargeUnitAvailable { get; set; }
        public string chargePinType { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
    }
    public class ParkingSlotDetailsUpdate
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
        public string updatedBy { get; set; }

    }
    #endregion

    #region Get Charge Pin Image 
    public void GetChargePinImage()
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
                      + "chargePinConfig?activeStatus=A";

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
                            gvChargePinType.DataSource = dt;
                            gvChargePinType.DataBind();
                        }
                        else
                        {
                            gvChargePinType.DataBind();
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
    #region Charge Pin  Change Click
    protected void rbtnChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "show()", true);
        if (rbtnChargeType.SelectedValue == "true")
        {
            CharType.Visible = true;
            GetChargePinImage();

        }
        else
        {

            CharType.Visible = false;
        }

    }
    #endregion
    #region Charge Pin Color Change
    protected void gvChargePinType_ItemCommand(object source, DataListCommandEventArgs e)
    {
        for (int j = 0; j < gvChargePinType.Items.Count; j++)
        {
            int i = e.Item.ItemIndex;
            if (i == j)
            {
                int label = e.Item.ItemIndex;
                Label lnkbtn = (Label)gvChargePinType.Items[j].FindControl("lblChargePinName");
                lnkbtn.Style.Add("color", "red");

            }
            else
            {
                Label lnkbtn = (Label)gvChargePinType.Items[j].FindControl("lblChargePinName");
                lnkbtn.Style.Add("color", "black");

            }
        }

    }
    #endregion
    #region Charge Type Click
    protected void lnkBtnChargeType_Click(object sender, ImageClickEventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "show()", true);
        ImageButton lnkbtn = sender as ImageButton;
        DataListItem gvrow = lnkbtn.NamingContainer as DataListItem;

        Label lblChargePinId = (Label)gvrow.FindControl("lblChargePinId");
        txtCharType.Text = lblChargePinId.Text;



    }
    #endregion   
    protected void btnslotback_Click(object sender, EventArgs e)
    {
        Session["blockSingle"] = "";
        Session["floorSingle"] = "";
        Session["typeOfVehicleSingle"] = "";
        Session["blocknameSingle"] = "";
        Session["floornameSingle"] = "";
        Session["typeOfVehiclenameSingle"] = "";
        Response.Redirect("ParkingSlotweb.aspx", false);
    }
}
