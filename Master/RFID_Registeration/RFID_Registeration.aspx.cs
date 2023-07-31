using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_RFID_Registeration_RFID_Registeration : System.Web.UI.Page
{
    readonly Helper helper = new Helper();
    readonly string baseUrl;
    readonly string bindGridUrl;
    readonly string insertUrl;
    readonly string updateUrl;
    readonly string deleteUrl;
    public Master_RFID_Registeration_RFID_Registeration()
    {
        baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        bindGridUrl = baseUrl + "tblregistration";
        insertUrl = baseUrl + "tblregistration";
        updateUrl = baseUrl + "tblregistration";
        deleteUrl = baseUrl + "tblregistration";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BindeGridView();
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }

    private void BindeGridView()
    {
        try
        {
            List<BindGrid> outList;
            int statusCode;
            string responseMessage;
            helper.APIGet<List<BindGrid>>(bindGridUrl, out outList, out statusCode, out responseMessage);

            if (statusCode == 1)
            {
                GridVisibleOrNot(true);
            }
            else
            {
                GridVisibleOrNot(false);
            }
            gvRFIDRegisterartion.DataSource = outList;
            gvRFIDRegisterartion.DataBind();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void GridVisibleOrNot(bool gridVisible)
    {
        if (gridVisible)
        {
            divGridView.Visible = true;
            divForm.Visible = false;
        }
        else
        {
            divGridView.Visible = false;
            divForm.Visible = true;
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

    protected void LnkAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
            GridVisibleOrNot(false);
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSubmit.Text.ToLower() == "submit")
            {
                insert();
            }
            else
            {
                update();
            }
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
            GridVisibleOrNot(true);
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }

    private void insert()
    {
        try
        {
            InsertData data = new InsertData();
            data.vehicleNumber = txtVehicleNumber.Text.Trim();
            data.UserId = txtUserTagId.Text.Trim();
            data.CreatedBy = Session["UserId"].ToString();

            int statusCode;
            string response;
            helper.APIpost<InsertData>(insertUrl, data, out statusCode, out response);

            if (statusCode == 1)
            {
                SuccessAlert(response);
                BindeGridView();
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

    private void update()
    {
        try
        {
            UpdateData data = new UpdateData();
            data.RegistrtionId = ViewState["RegistrtionId"].ToString();
            data.vehicleNumber = txtVehicleNumber.Text.Trim();
            data.UserId = txtUserTagId.Text.Trim();
            data.UpdatedBy = Session["UserId"].ToString();

            int statusCode;
            string response;
            helper.APIput<UpdateData>(insertUrl, data, out statusCode, out response);

            if (statusCode == 1)
            {
                SuccessAlert(response);
                BindeGridView();
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

    private class BindGrid
    {
        public string RegistrtionId { get; set; }
        public string VehicleNumber { get; set; }
        public string UserId { get; set; }
        public string ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }

    private class InsertData
    {
        public string vehicleNumber { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
    }

    private class UpdateData
    {
        public string RegistrtionId { get; set; }
        public string vehicleNumber { get; set; }
        public string UserId { get; set; }
        public string UpdatedBy { get; set; }
    }


protected void LnkEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton Imgbtn = sender as ImageButton;
            GridViewRow gvrow = Imgbtn.NamingContainer as GridViewRow;
            string registrtionId = gvRFIDRegisterartion.DataKeys[gvrow.RowIndex]["RegistrtionId"].ToString();
            string userId = gvRFIDRegisterartion.DataKeys[gvrow.RowIndex]["UserId"].ToString();
            string vehicleNumber = gvRFIDRegisterartion.DataKeys[gvrow.RowIndex]["vehicleNumber"].ToString();
            string UserId = gvRFIDRegisterartion.DataKeys[gvrow.RowIndex]["UserId"].ToString();
            ViewState["RegistrtionId"] = registrtionId;

            txtUserTagId.Text = userId;
            txtVehicleNumber.Text = vehicleNumber;
            GridVisibleOrNot(false);

            btnSubmit.Text = "Update";
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }

    private void Clear()
    {
        btnSubmit.Text = "Submit";
        txtUserTagId.Text = string.Empty;
        txtVehicleNumber.Text = string.Empty;
    }

    protected void lnkActiveOrInactive_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string registrtionId = gvRFIDRegisterartion.DataKeys[gvrow.RowIndex]["RegistrtionId"].ToString();
            string activestatus = gvRFIDRegisterartion.DataKeys[gvrow.RowIndex]["Activestatus"].ToString()=="A"?"D":"A";

            string requestUrl = deleteUrl + "?RegistrtionId=" + registrtionId + "&ActiveStatus=" + activestatus;
            int statusCode;
            string response;
            helper.APIDelete(requestUrl, out statusCode, out response);

            if (statusCode == 1)
            {
                SuccessAlert(response);
                BindeGridView();
            }
            else
            {
                InfoAlert(response);
            }
        }
        catch (Exception ex)
        {
            ErrorAlert(ex);
        }
    }
}