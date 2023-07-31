using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Activities.Statements;
using System.Xml.Linq;
using CrystalDecisions.Shared.Json;

public partial class Master_DeviceManager_DeviceMaster : System.Web.UI.Page
{
    readonly Helper helper = new Helper();
    readonly string baseUrl;
    readonly string ddlParkingUrl;
    readonly string ddlBranchUrl;
    readonly string gvAddedDevicesUrl;
    readonly string insertUrl;
    readonly string updateUrl;
    readonly string activeOrInactiveUrl;
    public Master_DeviceManager_DeviceMaster()
    {
        baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        ddlParkingUrl = baseUrl + "DeviceInformation?activeStatus=A";
        ddlBranchUrl = baseUrl + "DeviceInformation?activeStatus=A";
        gvAddedDevicesUrl = baseUrl + "DeviceInformation";
        insertUrl = baseUrl + "DeviceInformation";
        updateUrl = baseUrl + "DeviceInformation";
        activeOrInactiveUrl = baseUrl + "DeviceInformation";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Bind_InsertedGridView();
            }
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }

    private void BindParkingOwnerDropDown()
    {
        try
        {
            ddlParking.Items.Clear();
            ddlBranch.Items.Clear();

            int statusCode = 0;
            string response = null;
            JArray JArrayResult = null;

            helper.APIGet<JArray>(ddlParkingUrl, out JArrayResult, out statusCode, out response);

            if (statusCode == 1)
            {
                JArray filteredJarray = new JArray
                    (
                        JArrayResult.Select(obj => new JObject()
                        {
                             new JProperty("parkingOwnerId", obj["parkingOwnerId"]),
                             new JProperty("parkingName", obj["parkingName"]),
                        })
                    );

                List<ParkingOwnerDdl> ddlSrc = filteredJarray.ToObject<List<ParkingOwnerDdl>>();
                ddlParking.DataValueField = "ParkingOwnerId";
                ddlParking.DataTextField = "ParkingName";
                ddlParking.DataSource = ddlSrc;
                ddlParking.DataBind();
            }
            ddlParking.Items.Insert(0, new ListItem() { Text = "Select", Value = "0" });
            ddlBranch.Items.Insert(0, new ListItem() { Text = "Select", Value = "0" });
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void BindBranchDdl()
    {
        try
        {
            int statusCode = 0;
            string response = null;
            JArray JArrayResult = null;

            string requestUrl = ddlBranchUrl + "&parkingOwnerId=" + ddlParking.SelectedValue.Trim();
            helper.APIGet<JArray>(requestUrl, out JArrayResult, out statusCode, out response);

            if (statusCode == 1)
            {
                JArray branchDetails = (JArray)JArrayResult[0]["branchDetails"];
                JArray filteredJarray = new JArray
                    (
                        branchDetails.Select(obj => new JObject()
                        {
                             new JProperty("branchId", obj["branchId"]),
                             new JProperty("branchName", obj["branchName"]),
                        })
                    );

                List<BranchDdl> ddlSrc = filteredJarray.ToObject<List<BranchDdl>>();
                ddlBranch.DataValueField = "BranchId";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataSource = ddlSrc;
                ddlBranch.DataBind();
            }
            ddlBranch.Items.Insert(0, new ListItem() { Text = "Select", Value = "0" });
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void DdlParking_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBranchDdl();
    }
    private void Insert()
    {
        try
        {
            List<InsertDetails> insertDetails = new List<InsertDetails>();

            int count = 0;
            foreach (GridViewRow row in gvAddIPAndMAC.Rows)
            {
                CheckBox gvBtnCheckBox = (CheckBox)row.FindControl("gvBtnCheckBox");
                DropDownList gvDdlInOut = (DropDownList)row.FindControl("gvDdlInOut");
                TextBox gvTxtDeviceDescription = (TextBox)row.FindControl("gvTxtDeviceDescription");
                TextBox gvTxtMachineId = (TextBox)row.FindControl("gvTxtMachineId");
                Label lblgvIPAddress = (Label)row.FindControl("lblgvIPAddress");
                Label lblgvMACAddress = (Label)row.FindControl("lblgvMACAddress");

                if (gvBtnCheckBox.Checked == true)
                {
                    count++;
                    insertDetails.Add(new InsertDetails()
                    {
                        OwnerId = ddlParking.SelectedValue.Trim(),
                        BranchId = ddlBranch.SelectedValue.Trim(),
                        Ipaddress = lblgvIPAddress.Text.Trim(),
                        Macaddress = lblgvMACAddress.Text.Trim(),
                        CreatedBy = Session["UserId"].ToString(),
                        DeviceType = gvTxtDeviceDescription.Text.Trim(),
                        DeviceDirection = gvDdlInOut.SelectedValue.Trim(),
                        MachineId = gvTxtMachineId.Text.Trim(),
                    });
                }
            }
            if (count == 0)
            {
                InfoAlert("Select atleast one device.");
                return;
            }

            int statusCode = 0;
            string response = string.Empty;

            var jArray = JArray.FromObject(insertDetails);

            JObject jObjectInsertDetails = new JObject();
            jObjectInsertDetails.Add("DeviceInformationDetails", jArray);

            helper.APIpost<JObject>(insertUrl, jObjectInsertDetails, out statusCode, out response);

            if (statusCode == 1)
            {
                SuccessAlert(response);
                Bind_InsertedGridView();
            }
            else
            {
                InfoAlert(response);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void Update()
    {
        try
        {
            UpdateDetails updateData = new UpdateDetails();
            foreach (GridViewRow row in gvAddIPAndMAC.Rows)
            {
                DropDownList gvDdlInOut = (DropDownList)row.FindControl("gvDdlInOut");
                TextBox gvTxtDeviceDescription = (TextBox)row.FindControl("gvTxtDeviceDescription");
                TextBox gvTxtMachineId = (TextBox)row.FindControl("gvTxtMachineId");
                updateData = new UpdateDetails()
                {
                    DeviceDirection = gvDdlInOut.SelectedValue.Trim(),
                    DeviceType = gvTxtDeviceDescription.Text.Trim(),
                    UniqueId = ViewState["uniqueId"].ToString(),
                    UpdatedBy = Session["UserId"].ToString(),
                    MachineId = gvTxtMachineId.Text.Trim()
                };
            }

            int statusCode = 0;
            string response = string.Empty;
            helper.APIput<UpdateDetails>(updateUrl, updateData, out statusCode, out response);

            if (statusCode == 1)
            {
                SuccessAlert(response);
                Bind_InsertedGridView();
            }
            else
            {
                InfoAlert(response);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void GetInsertedData(out DataTable dt, out int sCode)
    {
        try
        {
            int statusCode = 0;
            string response = null;
            JArray JArrayResult = null;

            helper.APIGet<JArray>(gvAddedDevicesUrl, out JArrayResult, out statusCode, out response);

            if (JArrayResult == null)
            {
                dt = null;
            }
            else
            {
                dt = JArrayResult.ToObject<DataTable>();
            }
            sCode = statusCode;
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void Bind_InsertedGridView()
    {
        try
        {

            int statusCode = 0;
            DataTable dt = null;

            GetInsertedData(out dt, out statusCode);

            if (statusCode == 1)
            {
                divForm.Visible = false;
                divGridView.Visible = true;
                gvShowIPAndMAC.DataSource = dt;
                gvShowIPAndMAC.DataBind();
            }
            else
            {
                divForm.Visible = true;
                divGridView.Visible = false;
                BindParkingOwnerDropDown();
                //Bind_LAN_IP_Grid();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void Bind_LAN_IP_Grid()
    {
        try
        {
            DataTable dt_LAN_Details = new DataTable();
            dt_LAN_Details = GetLAN_IP_And_MAC_Address();

            int statusCode = 0;
            DataTable dtInserted_LAN = null;

            GetInsertedData(out dtInserted_LAN, out statusCode);

            DataTable gvDsrc;
            if (dtInserted_LAN != null && dtInserted_LAN.Rows.Count > 0)
            {
                var filteredInsertedData = dtInserted_LAN.AsEnumerable().Where
                                                               (
                                                               x => x.Field<Int64>("BranchId") == Convert.ToInt64(ddlBranch.SelectedValue.Trim()) &&
                                                               x.Field<Int64>("OwnerId") == Convert.ToInt64(ddlParking.SelectedValue.Trim())
                                                               );

                IEnumerable<DataRow> filteredData;
                if (filteredInsertedData.Count() > 0)
                {
                    filteredData = dt_LAN_Details.AsEnumerable()
                                              .Where(row => !dtInserted_LAN.AsEnumerable().Where
                                                  (
                                                  x => x.Field<Int64>("BranchId") == Convert.ToInt64(ddlBranch.SelectedValue.Trim()) &&
                                                  x.Field<Int64>("OwnerId") == Convert.ToInt64(ddlParking.SelectedValue.Trim())
                                                  )
                                              .Select(x => x.Field<string>("Ipaddress"))
                                              .Contains(row.Field<string>("Ipaddress")));
                }
                else
                {
                    filteredData = dt_LAN_Details.AsEnumerable().Select(x => x);
                }



                if (filteredData.Count() > 0)
                {
                    gvDsrc = filteredData.CopyToDataTable();
                }
                else
                {
                    gvDsrc = null;
                }
            }
            else
            {
                gvDsrc = dt_LAN_Details;
            }

            gvAddIPAndMAC.DataSource = gvDsrc;
            gvAddIPAndMAC.DataBind();

            if (gvDsrc != null && gvDsrc.Rows.Count > 0)
            {
                divForm.Visible = true;
                divGridView.Visible = false;
            }
            else
            {
                InfoAlert("Already added all the devices connected to the LAN.");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private DataTable GetLAN_IP_And_MAC_Address()
    {
        try
        {
            IPAddress localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                                 .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            // Get the ARP table
            var arpTable = new DataTable("ARP");
            arpTable.Columns.Add("Ipaddress", typeof(string));
            arpTable.Columns.Add("Macaddress", typeof(string));
            arpTable.Columns.Add("DeviceType", typeof(string));
            arpTable.Columns.Add("MachineId", typeof(string));

            using (var process = new Process())
            {
                process.StartInfo.FileName = "arp";
                process.StartInfo.Arguments = "-a";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line != null && line.Contains(localIp.ToString())) continue;

                    var elements = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (elements.Length == 3)
                    {
                        var row = arpTable.NewRow();
                        row["Ipaddress"] = IPAddress.Parse(elements[0]);
                        row["Macaddress"] = elements[1];
                        row["DeviceType"] = "";
                        row["MachineId"] = "";
                        arpTable.Rows.Add(row);
                    }
                }
            }
            return arpTable;
        }
        catch (Exception)
        {
            throw;
        }
    }
    #region Alerts
    private void SuccessAlert(string responseMsg)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + responseMsg.ToString().Trim() + "');", true);
    }
    private void InfoAlert(string responseMsg)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "infoalert('" + responseMsg.ToString().Trim() + "');", true);
    }
    private void ErrorAlert(Exception ex)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.Message.ToString().Trim() + "');", true);
    }
    #endregion
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
    protected void LnkAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            BindParkingOwnerDropDown();
            gvAddIPAndMAC.DataSource = null;
            gvAddIPAndMAC.DataBind();
            divForm.Visible = true;
            divGridView.Visible = false;

            ddlBranch.ClearSelection();
            ddlParking.ClearSelection();
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Bind_InsertedGridView();
        btnSubmit.Text = "Submit";
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Bind_LAN_IP_Grid();
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }
    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string uniqueId = gvShowIPAndMAC.DataKeys[gvrow.RowIndex]["UniqueId"].ToString();
            string ActiveStatus = gvShowIPAndMAC.DataKeys[gvrow.RowIndex]["ActiveStatus"].ToString();
            ActiveStatus = ActiveStatus == "A" ? "D" : "A";
            string requestUri = activeOrInactiveUrl + "?ActiveStatus=" + ActiveStatus + "&UniqueId=" + uniqueId;

            int statusCode = 0;
            string response = string.Empty;

            helper.APIDelete(requestUri, out statusCode, out response);

            if (statusCode == 0)
            {
                InfoAlert(response);
            }
            else
            {
                SuccessAlert(response);
                Bind_InsertedGridView();
            }
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }
    protected void LnkEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton Imgbtn = sender as ImageButton;
            GridViewRow gvrow = Imgbtn.NamingContainer as GridViewRow;
            string uniqueId = gvShowIPAndMAC.DataKeys[gvrow.RowIndex]["UniqueId"].ToString();
            ViewState["uniqueId"] = uniqueId;
            string ActiveStatus = gvShowIPAndMAC.DataKeys[gvrow.RowIndex]["ActiveStatus"].ToString();
            string OwnerID = gvShowIPAndMAC.DataKeys[gvrow.RowIndex]["OwnerId"].ToString();
            string BranchID = gvShowIPAndMAC.DataKeys[gvrow.RowIndex]["BranchId"].ToString();


            BindParkingOwnerDropDown();
            ddlParking.SelectedValue = OwnerID;
            BindBranchDdl();
            ddlBranch.SelectedValue = BranchID;

            divGridView.Visible = false;
            divForm.Visible = true;

            ddlParking.Enabled = false;
            ddlBranch.Enabled = false;

            DataTable insertedDt = new DataTable();
            int statusCode = 0;
            GetInsertedData(out insertedDt, out statusCode);

            DataTable editDatatable = insertedDt.AsEnumerable().Where(row => row.Field<Int64>("uniqueId") == Convert.ToInt64(uniqueId)).CopyToDataTable();
            gvAddIPAndMAC.DataSource = editDatatable;
            gvAddIPAndMAC.DataBind();

            foreach (GridViewRow row in gvAddIPAndMAC.Rows)
            {
                CheckBox chkBox = (CheckBox)row.FindControl("gvBtnCheckBox");
                chkBox.Visible = false;
            }

            btnSubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }

    private class ParkingOwnerDdl
    {
        public string ParkingOwnerId { get; set; }
        public string ParkingName { get; set; }
    }
    private class BranchDdl
    {
        public string BranchId { get; set; }
        public string BranchName { get; set; }
    }
    public class UpdateDetails
    {
        public string DeviceType { get; set; }
        public string DeviceDirection { get; set; }
        public string UniqueId { get; set; }
        public string UpdatedBy { get; set; }
        public string MachineId { get; set; }
    }
    public class InsertDetails
    {
        public string Macaddress { get; set; }
        public string Ipaddress { get; set; }
        public string MachineId { get; set; }
        public string DeviceType { get; set; }
        public string DeviceDirection { get; set; }
        public string OwnerId { get; set; }
        public string BranchId { get; set; }
        public string CreatedBy { get; set; }
    }
    private class DeviceInformationDetails
    {
        public List<InsertDetails> DeviceInfo { get; set; }
    }
}