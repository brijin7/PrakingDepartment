using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Master_messageTemplates : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null && Session["UserRole"] == null)
        {
            Session.Clear();
              Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
        }
        if (!IsPostBack)
        {
            BindMsgTemplateDetails();

        }
    }
    #endregion

    #region Bind Msg Template Details
    public void BindMsgTemplateDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim() + "messageTemplates";
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
                            gvMsgTemplateDetails.DataSource = dt;
                            gvMsgTemplateDetails.DataBind();
                        }
                        else
                        {
                            gvMsgTemplateDetails.DataBind();
                        }

                    }
                    else
                    {
                        divGv.Visible = false;
                        divForm.Visible = true;
                        rbtnltemplateType.SelectedValue = "M";
                        divtpid.Visible = false;
                        divpeid.Visible = false;
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

    #region ADD Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        divGv.Visible = false;
        divForm.Visible = true;
        rbtnltemplateType.SelectedValue = "M";
        divtpid.Visible = false;
        divpeid.Visible = false;
        spAddorEdit.InnerText = "Add ";
    }
    #endregion

    #region Cancel Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }

    public void Cancel()
    {
        divGv.Visible = true;
        divForm.Visible = false;
        txtHeader.Text = string.Empty;
        txtmessageBody.Text = string.Empty;
        txtpeid.Text = string.Empty;
        txtsubject.Text = string.Empty;
        txttpid.Text = string.Empty;
        rbtnltemplateType.ClearSelection();
        btnSubmit.Text = "Submit";

        if (rbtnltemplateType.SelectedValue == "S")
        {
            divtpid.Visible = true;
            divpeid.Visible = true;
        }
        else
        {
            divtpid.Visible = false;
            divpeid.Visible = false;
        }
        rbtnltemplateType.Enabled = true;
    }
    #endregion

    #region Submit / Insert 
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Submit")
        {
            InsertMsgTemplateDetails();
        }
        else
        {
            UpdateMsgTemplateDetails();
        }
    }
    public void InsertMsgTemplateDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Insert = new MsgTemplate()
                {
                    messageHeader = txtHeader.Text.Trim(),
                    subject = txtsubject.Text.Trim(),
                    messageBody = txtmessageBody.Text.Trim(),
                    templateType = rbtnltemplateType.SelectedValue.Trim(),
                    peid = txtpeid.Text.Trim(),
                    tpid = txttpid.Text.Trim(),
                    createdBy = Session["UserId"].ToString().Trim()
                };

                HttpResponseMessage response = client.PostAsJsonAsync("messageTemplates", Insert).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindMsgTemplateDetails();
                        Cancel();
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

    #region Edit & Update
    protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton lnkbtn = sender as ImageButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            Label lblGvUniqueId = (Label)gvrow.FindControl("lblGvUniqueId");
            Label lblGvTemplateType = (Label)gvrow.FindControl("lblGvTemplateType");
            Label lblGvMessageHeader = (Label)gvrow.FindControl("lblGvMessageHeader");
            Label lblGvSubject = (Label)gvrow.FindControl("lblGvSubject");
            Label lblGvMessageBody = (Label)gvrow.FindControl("lblGvMessageBody");
            Label lblGvPeid = (Label)gvrow.FindControl("lblGvPeid");
            Label lblGvTpid = (Label)gvrow.FindControl("lblGvTpid");

            string sgvMsgTemplateDetails = gvMsgTemplateDetails.DataKeys[gvrow.RowIndex].Value.ToString();
            ViewState["uniqueId"] = sgvMsgTemplateDetails.ToString().Trim();
        
            rbtnltemplateType.SelectedValue = lblGvTemplateType.Text == "Mail" ? "M" : "S";
            txtHeader.Text = lblGvMessageHeader.Text;
            txtsubject.Text = lblGvSubject.Text;
            txtmessageBody.Text = lblGvMessageBody.Text;
            txtpeid.Text = lblGvPeid.Text;
            txttpid.Text = lblGvTpid.Text;

            if (rbtnltemplateType.SelectedValue == "S")
            {
                divtpid.Visible = true;
                divpeid.Visible = true;

            }
            else
            {
                divtpid.Visible = false;
                divpeid.Visible = false;

            }
            if (lblGvPeid.Text.Trim() == "string")
            {
                txtpeid.Text = "";
            }
            if (lblGvTpid.Text.Trim() == "string")
            {
                txttpid.Text = "";
            }

            divGv.Visible = false;
            divForm.Visible = true;
            btnSubmit.Text = "Update";
            spAddorEdit.InnerText = "Edit ";
            rbtnltemplateType.Enabled = false;
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "erroralert('" + ex.ToString().Trim() + "');", true);
        }
    }
    public void UpdateMsgTemplateDetails()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var Update = new MsgTemplate()
                {
                    uniqueId = ViewState["uniqueId"].ToString(),
                    messageHeader = txtHeader.Text.Trim(),
                    subject = txtsubject.Text.Trim(),
                    messageBody = txtmessageBody.Text.Trim(),
                    templateType = rbtnltemplateType.SelectedValue.Trim(),
                    peid = txtpeid.Text.Trim(),
                    tpid = txttpid.Text.Trim(),
                    updatedBy = Session["UserId"].ToString().Trim()
                };
                HttpResponseMessage response = client.PutAsJsonAsync("messageTemplates", Update).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        btnSubmit.Text = "Submit";
                        BindMsgTemplateDetails();
                        Cancel();
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
    protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            using (var client = new HttpClient())
            {
                ImageButton ImgBtn = sender as ImageButton;
                GridViewRow gvrow = ImgBtn.NamingContainer as GridViewRow;
                string suniqueId = gvMsgTemplateDetails.DataKeys[gvrow.RowIndex].Value.ToString();

                client.BaseAddress = new Uri(Session["BaseUrl"].ToString().Trim());
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string sUrl = Session["BaseUrl"].ToString().Trim()
                            + "messageTemplates?uniqueId=" + suniqueId;

                HttpResponseMessage response = client.DeleteAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var SmartParkingList = response.Content.ReadAsStringAsync().Result;
                    int StatusCode = Convert.ToInt32(JObject.Parse(SmartParkingList)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(SmartParkingList)["response"].ToString();

                    if (StatusCode == 1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "successalert('" + ResponseMsg.ToString().Trim() + "');", true);
                        BindMsgTemplateDetails();
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

    #region MsgTemplate Class
    public class MsgTemplate
    {
        public string uniqueId { get; set; }
        public string messageHeader { get; set; }
        public string subject { get; set; }
        public string messageBody { get; set; }
        public string templateType { get; set; }
        public string peid { get; set; }
        public string tpid { get; set; }
        public string activeStatus { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }

    }
    #endregion

    #region SMS Visible True Method
    protected void rbtnltemplateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnltemplateType.SelectedValue == "S")
        {
            divtpid.Visible = true;
            divpeid.Visible = true;
        }
        else
        {
            divtpid.Visible = false;
            divpeid.Visible = false;
        }
    }
    #endregion
}