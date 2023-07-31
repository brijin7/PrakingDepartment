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

public partial class PreParking : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Session["UserId"] != null && Session["UserRole"] != null)
                {
                    lblUserName.Text = Session["UserName"].ToString().Trim();
                    if (Session["UserRole"].ToString() == "SA")
                    {
                        lblUserRole.Text = "Sadmin";
                    }
                    else if (Session["UserRole"].ToString() == "A")
                    {
                        lblUserRole.Text = "Admin";
                    }
                    else
                    {
                        lblUserRole.Text = "User";

                    }

                    if (Session["UserRole"].ToString().Trim() == "SA")
                    {
                        Session["approvalStatus"] = "A";
                    }
                    else
                    {
                        Session["approvalStatus"] = "W";
                    }

                    //lblLoginUser.Text = Session["FirstName"].ToString().Trim() + " " + Session["LastName"].ToString().Trim();

                    //Session["PrintUserName"] = lblLoginUser.Text.Trim();    

                    if (Session["UserRole"].ToString().Trim() != "SA") ///SA(superAdmin) , A(admin), E(employee), C(customer)
                    {
                        if (Session["UserRole"].ToString().Trim() == "E")
                        {
                            BindMenuAccessRights();
                            lblParkingName.Text = Session["parkingName"].ToString().Trim();
                            lblBranchName.Text = Session["branchName"].ToString().Trim();
                        }
                        else if (Session["UserRole"].ToString().Trim() == "A" || Session["UserRole"].ToString().Trim() == "Admin")
                        {
                            lblParkingName.Text = Session["parkingName"].ToString().Trim();

                            lstBranchLogin.Visible = true;
                            lstbtnMaster.Visible = false;
                            lstReports.Visible = false;
                            lstOwnerlogin.Visible = false;
                            lstOwnerBack.Visible = false;
                            lnkBtnPrintingInsSA.Visible = false;
                            lstPoPreference.Visible = false;
                            if (Session["ShowPage"].ToString().Trim() == "1")
                            {
                                lblBranchName.Text = Session["branchName"].ToString().Trim();
                                lstbtnMaster.Visible = true;
                                lstBranch.Visible = true;
                                liBranchMaster.Visible = true;
                                liOnlineBookingMaster.Visible = true;
                                lishiftMaster.Visible = true;
                                lnkBlockMaster.Visible = true;
                                lstFloorMaster.Visible = true;
                                lstUserMaster.Visible = true;
                                lstEmployeeMaster.Visible = true;
                                lstMenuAccessRights.Visible = true;
                                lnkBtnPaymentSetting.Visible = true;
                                lnkBtnOfferMapping.Visible = true;
                                lnkBtnServiceWise.Visible = true;
                                lnkBtnPassTransList.Visible = true;
                                lnkBtnPrintingInfo.Visible = true;
                                lnkBtnParkingPass.Visible = true;
                                lnkBtnParkingSlot.Visible = true;
                                lstOfferMaster.Visible = true;
                                lstCheckInCheckOut.Visible = true;
                                LstScanCheckInCheckOut.Visible = true;
                                LstAddOnServiceBooking.Visible = true;
                                lstParkingPassTransaction.Visible = true;
                                lstBranchLogin.Visible = false;
                                lstReports.Visible = true;
                                lstPoPreference.Visible = true;
                                if (Session["OneBranch"].ToString() == "true")
                                {
                                    lstBranchBack.Visible = false;
                                }
                                else
                                {
                                    lstBranchBack.Visible = true;
                                }
                                if (Session["slotExist"].ToString().Trim() == "Y")
                                {
                                    lstBooking.Visible = true;
                                    lstBookingWithoutSlot.Visible = false;
                                }
                                else
                                {
                                    lstBooking.Visible = false;
                                    lstBookingWithoutSlot.Visible = true;
                                }

                                if (Session["branchOptions"].ToString().Trim() == "Y")
                                {
                                    if (Session["blockOption"].ToString().Trim() == "N")
                                    {
                                        lnkBlockMaster.Visible = false;
                                    }
                                    if (Session["floorOption"].ToString().Trim() == "N")
                                    {
                                        lifloorMaster.Visible = false;
                                    }
                                    if (Session["slotsOption"].ToString().Trim() == "N")
                                    {
                                        lnkBtnParkingSlot.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lstConfiguration.Visible = true;
                        lstFAQMaster.Visible = true;
                        lstBranch.Visible = true;
                        liBranchMaster.Visible = true;
                        liOnlineBookingMaster.Visible = true;
                        lishiftMaster.Visible = false;
                        lstOwnerMaster.Visible = true;
                        //lnkBtnBranchMstr.Visible = true;
                        lstMessageTemplate.Visible = true;
                        lnkBtnOfferMaster.Visible = true;
                        lnkBtnParkingList.Visible = true;

                        //lnkBtnOfferRules.Visible = true;

                        lstCancellationRules.Visible = true;
                        lstOfferMaster.Visible = true;
                        lstbtnMaster.Visible = true;
                        lstSubscription.Visible = true;
                        lnkBtnPrintingInsSA.Visible = true;
                        lstOwnerlogin.Visible = true;

                        // lstBranchLogin.Visible = true;

                        lstReports.Visible = true;
                        lstBranchBack.Visible = false;
                        lstBranchLogin.Visible = false;

                        lstDeviceMaster.Visible = true;
                        lstRFIDRegistration.Visible = true;

                        if (Session["ShowPage"].ToString().Trim() == "1")
                        {
                            lblParkingName.Text = Session["parkingName"].ToString().Trim();
                            lblBranchName.Text = Session["branchName"].ToString().Trim();

                            lstbtnMaster.Visible = true;
                            lstBranch.Visible = true;
                            liBranchMaster.Visible = true;
                            liOnlineBookingMaster.Visible = true;
                            lishiftMaster.Visible = true;
                            lnkBlockMaster.Visible = true;
                            lstFloorMaster.Visible = true;
                            lstUserMaster.Visible = true;
                            lstEmployeeMaster.Visible = true;
                            lstMenuAccessRights.Visible = true;
                            lnkBtnOfferMapping.Visible = true;
                            lnkBtnServiceWise.Visible = true;
                            lnkBtnPassTransList.Visible = true;
                            lnkBtnPrintingInsSA.Visible = false;
                            lnkBtnPrintingInfo.Visible = true;
                            lnkBtnParkingPass.Visible = true;
                            lnkBtnParkingSlot.Visible = true;

                            lstOwnerlogin.Visible = false;
                            lstOwnerBack.Visible = true;
                            lstReports.Visible = true;
                            lstOfferMaster.Visible = true;
                            lstConfiguration.Visible = false;
                            lstSubscription.Visible = false;
                            lstFAQMaster.Visible = false;
                            lnkBtnPaymentSetting.Visible = true;
                            lstOwnerMaster.Visible = false;
                            //lnkBtnBranchMstr.Visible = false;
                            lstMessageTemplate.Visible = false;
                            lnkBtnOfferMaster.Visible = false;
                            lnkBtnOfferRules.Visible = false;
                            lstCancellationRules.Visible = false;
                            lnkBtnParkingList.Visible = false;
                            lstPoPreference.Visible = true;
                            if (Session["slotExist"].ToString().Trim() == "Y")
                            {
                                lstBooking.Visible = true;
                                lstBookingWithoutSlot.Visible = false;
                            }
                            else
                            {
                                lstBooking.Visible = false;
                                lstBookingWithoutSlot.Visible = true;
                            }
                            LstAddOnServiceBooking.Visible = true;
                            lstParkingPassTransaction.Visible = true;
                            lstCheckInCheckOut.Visible = true;
                            LstScanCheckInCheckOut.Visible = true;
                            if (Session["branchOptions"].ToString().Trim() == "Y")
                            {
                                if (Session["blockOption"].ToString().Trim() == "N")
                                {
                                    lnkBlockMaster.Visible = false;
                                }
                                if (Session["floorOption"].ToString().Trim() == "N")
                                {
                                    lifloorMaster.Visible = false;
                                }
                                if (Session["slotsOption"].ToString().Trim() == "N")
                                {
                                    lnkBtnParkingSlot.Visible = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Session.Clear();
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('" + ex.ToString().Trim() + "');", true);
            }
            finally
            {
                if (hfHasClassActive.Value == "Show")
                {
                    img.CssClass = img.CssClass.Replace("active", "ImgStyle");
                    content.Attributes.Add("class", "content");
                    header1.Attributes.Add("class", "header1");
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Show');", true);
                }
                else
                {
                    img.CssClass = img.CssClass.Replace("ImgStyle", "active");
                    content.Attributes.Add("class", "contentoptimize");
                    header1.Attributes.Add("class", "header1 optimize");
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('Hide');", true);
                }

                if (Session["Bookingnav"] != null)
                {
                    if (Session["Bookingnav"].ToString() == "Hide")
                    {
                        img.CssClass = img.CssClass.Replace("ImgStyle", "active");
                        content.Attributes.Add("class", "contentoptimize");
                        header1.Attributes.Add("class", "header1 optimize");
                        hfHasClassActive.Value = "Hide";
                        Session["Bookingnav"] = "Show";
                    }
                }
                else
                {
                    Session.Clear();
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
                }

            }
        }
    }
    protected void lbtnSadminBack_Click(object sender, EventArgs e)
    {
        Session["ShowPage"] = "0";
        Session["parkingOwnerId"] = "0";
        Session["branchId"] = "";
        Session["parkingName"] = "";
        Session["branchName"] = "";
        Session["slotExist"] = "";
        Session["multiBook"] = "";
        Session["isBookCheckInAvailable"] = "";
        Session["Branch"] = "0";
        Session["MobNo"] = null;
        Response.Redirect("~/Login/OwnerLogin.aspx", false);
    }
    protected void lbtnBranchBack_Click(object sender, EventArgs e)
    {
        if (Session["UserRole"].ToString().Trim() == "SA")
        {
            Session["ShowPage"] = "0";
            Session["parkingOwnerId"] = "0";
            Session["branchId"] = "";
            Session["parkingName"] = "";
            Session["branchName"] = "";
            Session["slotExist"] = "";
            Session["multiBook"] = "";
            Session["isBookCheckInAvailable"] = "";
        }
        else
        {
            Session["ShowPage"] = "0";
            Session["branchId"] = "";
            Session["branchName"] = "";
            Session["slotExist"] = "";
            Session["multiBook"] = "";
            Session["isBookCheckInAvailable"] = "";
        }
        Session["MobNo"] = null;

        Response.Redirect("~/Login/BranchLogin.aspx", false);
    }
    protected void lbtnlogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogoutUrl"].Trim(), true);
    }

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
    public void BindMenuAccessRights()
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
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        Session["parkingName"] = dt.Rows[0]["parkingName"].ToString();
                        Session["parkingOwnerId"] = dt.Rows[0]["parkingOwnerId"].ToString();
                        Session["branchName"] = dt.Rows[0]["branchName"].ToString();
                        Session["branchId"] = dt.Rows[0]["branchId"].ToString();
                        GetOwnerId(Session["branchId"].ToString());
                        GetSlotExits(Session["branchId"].ToString());
                        int Count = 0;
                        int Branch = 0;
                        int Floor = 0;
                        List<menuOptionAccess> lst = JsonConvert.DeserializeObject<List<menuOptionAccess>>(ResponseMsg);
                        var firstItem = lst.ElementAt(0);
                        var lst1 = firstItem.optionDetails.ToList();
                        DataTable optionDetails = ConvertToDataTable(lst1);

                        // BranchMaster
                        var BranchMaster = lst1.Where(x => x.optionName == "branchMaster" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (BranchMaster.Count > 0)
                        {
                            Count = 1;
                            lstBranch.Visible = true;
                            liBranchMaster.Visible = true;
                            liOnlineBookingMaster.Visible = true;
                        }
                        else
                        {
                            if (Branch == 1)
                            {
                                lstBranch.Visible = true;
                            }
                            else
                            {
                                lstBranch.Visible = false;
                            }
                            liBranchMaster.Visible = false;
                            liOnlineBookingMaster.Visible = false;

                        }

                        // ShiftMaster
                        var shiftMaster = lst1.Where(x => x.optionName == "shiftMaster" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (shiftMaster.Count > 0)
                        {
                            Count = 1;
                            lstBranch.Visible = true;
                            lishiftMaster.Visible = true;
                        }
                        else
                        {
                            if (Branch == 1)
                            {
                                lstBranch.Visible = true;
                            }
                            else
                            {
                                lstBranch.Visible = false;
                            }
                            lishiftMaster.Visible = false;

                        }

                        // BlockMaster
                        var blockMaster = lst1.Where(x => x.optionName == "blockMaster" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (blockMaster.Count > 0)
                        {
                            Count = 1;
                            lnkBlockMaster.Visible = true;
                        }
                        else
                        {
                            lnkBlockMaster.Visible = false;
                        }

                        // Offer Mapping
                        var offerMapping = lst1.Where(x => x.optionName == "offerMapping" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (offerMapping.Count > 0)
                        {
                            Count = 1;
                            lnkBtnOfferMaster.Visible = true;
                            lnkBtnOfferMapping.Visible = true;
                        }
                        else
                        {
                            lnkBtnOfferMaster.Visible = false;
                            lnkBtnOfferMapping.Visible = false;

                        }

                        // Employee Master
                        var employeeMaster = lst1.Where(x => x.optionName == "employeeMaster" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (employeeMaster.Count > 0)
                        {
                            Count = 1;
                            lstEmployeeMaster.Visible = true;
                        }
                        else
                        {
                            lstEmployeeMaster.Visible = false;
                        }

                        // FloorSetup
                        var floorSetup = lst1.Where(x => x.optionName == "floorSetup" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (floorSetup.Count > 0)
                        {
                            Count = 1;
                            Floor = 1;
                            lstFloorMaster.Visible = true;
                            lifloorSetup.Visible = true;
                        }
                        else
                        {
                            lstFloorMaster.Visible = false;
                            lifloorSetup.Visible = false;

                        }

                        // Floor Master
                        var floorMaster = lst1.Where(x => x.optionName == "floorMaster" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (floorMaster.Count > 0)
                        {
                            Count = 1;
                            lstFloorMaster.Visible = true;
                            lifloorMaster.Visible = true;
                        }
                        else
                        {
                            if (Floor == 1)
                            {
                                lstFloorMaster.Visible = true;
                            }
                            else
                            {
                                lstFloorMaster.Visible = false;

                            }
                            lifloorMaster.Visible = false;

                        }

                        // User Master
                        var userMaster = lst1.Where(x => x.optionName == "userMaster" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (userMaster.Count > 0)
                        {
                            Count = 1;
                            lstUserMaster.Visible = true;
                        }
                        else
                        {
                            lstUserMaster.Visible = false;

                        }
                        // menuAccessRights
                        var menuAccessRights = lst1.Where(x => x.optionName == "menuAccessRights" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (menuAccessRights.Count > 0)
                        {
                            Count = 1;
                            lstMenuAccessRights.Visible = true;
                        }
                        else
                        {
                            lstMenuAccessRights.Visible = false;

                        }
                        //printingInstructions
                        var printingInstructions = lst1.Where(x => x.optionName == "printingInstructions" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (printingInstructions.Count > 0)
                        {
                            Count = 1;
                            lnkBtnPrintingInfo.Visible = true;
                        }
                        else
                        {
                            lnkBtnPrintingInfo.Visible = false;
                        }
                        // passConfig
                        var passConfig = lst1.Where(x => x.optionName == "passConfig" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (passConfig.Count > 0)
                        {
                            Count = 1;
                            lnkBtnParkingPass.Visible = true;
                        }
                        else
                        {
                            lnkBtnParkingPass.Visible = false;

                        }
                        // Slot
                        var slot = lst1.Where(x => x.optionName == "slot" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (slot.Count > 0)
                        {
                            Count = 1;
                            lnkBtnParkingSlot.Visible = true;
                        }
                        else
                        {
                            lnkBtnParkingSlot.Visible = false;

                        }
                        // checkInCheckOut
                        int checkinCount = 0;
                        var checkIn = lst1.Where(x => x.optionName == "checkIn" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();
                        if (checkIn.Count > 0)
                        {
                            checkinCount = 1;
                            lstCheckInCheckOut.Visible = true;

                        }
                        else
                        {
                            lstCheckInCheckOut.Visible = false;

                        }
                        var CheckOut = lst1.Where(x => x.optionName == "checkOut" && x.MenuOptionAccessActiveStatus == "A")
                       .Select(x => new
                       {
                           optionName = x.optionName
                       }).ToList();
                        if (CheckOut.Count > 0)
                        {
                            lstCheckInCheckOut.Visible = true;
                        }
                        else
                        {
                            if (checkinCount == 1)
                            {
                                lstCheckInCheckOut.Visible = true;
                            }
                            else
                            {
                                lstCheckInCheckOut.Visible = false;

                            }


                        }
                        // booking
                        var booking = lst1.Where(x => x.optionName == "booking" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();

                        if (booking.Count > 0)
                        {
                            if (Session["slotExist"].ToString().Trim() == "Y")
                            {
                                lstBooking.Visible = true;
                                lstBookingWithoutSlot.Visible = false;
                            }
                            else
                            {
                                lstBooking.Visible = false;
                                lstBookingWithoutSlot.Visible = true;
                            }
                        }
                        else
                        {
                            lstBooking.Visible = false;
                            lstBookingWithoutSlot.Visible = false;
                        }
                        // passTransaction
                        var passTransaction = lst1.Where(x => x.optionName == "passTransaction" && x.MenuOptionAccessActiveStatus == "A")
                       .Select(x => new
                       {
                           optionName = x.optionName
                       }).ToList();

                        if (passTransaction.Count > 0)
                        {
                            lstParkingPassTransaction.Visible = true;
                        }
                        else
                        {
                            lstParkingPassTransaction.Visible = false;
                        }
                        //payment Integration
                        var PaymentIntegration = lst1.Where(x => x.optionName == "PaymentIntegration" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();

                        if (PaymentIntegration.Count > 0)
                        {
                            lnkBtnPaymentSetting.Visible = true;
                        }
                        else
                        {
                            lnkBtnPaymentSetting.Visible = false;
                        }

                        //servicebaseReport
                        var servicebaseReport = lst1.Where(x => x.optionName == "servicebaseReport" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();

                        if (servicebaseReport.Count > 0)
                        {
                            lstReports.Visible = true;
                            lnkBtnServiceWise.Visible = true;
                        }
                        else
                        {
                            lstReports.Visible = false;
                            lnkBtnServiceWise.Visible = false;
                        }
                        //addOnService
                        var addOnService = lst1.Where(x => x.optionName == "addOnService" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();

                        if (addOnService.Count > 0)
                        {
                            LstAddOnServiceBooking.Visible = true;
                        }
                        else
                        {
                            LstAddOnServiceBooking.Visible = false;
                        }

                        //smartCheckInCheckOut
                        var smartCheckInCheckOut = lst1.Where(x => x.optionName == "smartCheckInCheckOut" && x.MenuOptionAccessActiveStatus == "A")
                        .Select(x => new
                        {
                            optionName = x.optionName
                        }).ToList();

                        if (smartCheckInCheckOut.Count > 0)
                        {
                            LstScanCheckInCheckOut.Visible = true;
                        }
                        else
                        {
                            LstScanCheckInCheckOut.Visible = false;
                        }
                        if (Count == 0)
                        {
                            lstbtnMaster.Visible = false;
                        }


                    }

                }
            }
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "alert('" + ex.ToString().Trim() + "');", true);
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
    public void GetOwnerId(string branchId)
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
                          + "branchMaster?activeStatus=A&approvalStatus=A&branchId=";
                sUrl += branchId;
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);

                        Session["parkingName"] = dt.Rows[0]["parkingName"].ToString();
                        Session["parkingOwnerId"] = dt.Rows[0]["parkingOwnerId"].ToString();
                        Session["branchName"] = dt.Rows[0]["branchName"].ToString();
                        Session["branchId"] = dt.Rows[0]["branchId"].ToString();
                        Session["slotExist"] = dt.Rows[0]["slotExist"].ToString();
                        Session["multiBook"] = dt.Rows[0]["multiBook"].ToString();
                        Session["isBookCheckInAvailable"] = dt.Rows[0]["isBookCheckInAvailable"].ToString();
                        Session["branchOptions"] = dt.Rows[0]["branchOptions"].ToString();
                        Session["ShowPage"] = "1";
                        if (Session["branchOptions"].ToString() == "Y")
                        {
                            List<BranchMasterClass> Branch = JsonConvert.DeserializeObject<List<BranchMasterClass>>(ResponseMsg);
                            var firstItem = Branch.ElementAt(0);
                            var lst1 = firstItem.branchOptionDetails.ToList();
                            DataTable branchOptionDetails = ConvertToDataTable(lst1);
                            Session["blockOption"] = branchOptionDetails.Rows[0]["blockOption"].ToString();
                            Session["floorOption"] = branchOptionDetails.Rows[0]["floorOption"].ToString();
                            Session["slotsOption"] = branchOptionDetails.Rows[0]["slotsOption"].ToString();

                        }
                        else
                        {
                            //Response.Redirect("~/PO_Preference.aspx", false);

                        }


                    }
                    else
                    {

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    #endregion
    #region Get Slot Exits
    public void GetSlotExits(string branchId)
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
                          + "branchMaster?activeStatus=A&approvalStatus=A&branchId=";
                sUrl += branchId;
                HttpResponseMessage response = client.GetAsync(sUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var Locresponse = response.Content.ReadAsStringAsync().Result;
                    int statusCode = Convert.ToInt32(JObject.Parse(Locresponse)["statusCode"].ToString());
                    string ResponseMsg = JObject.Parse(Locresponse)["response"].ToString();
                    if (statusCode == 1)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(ResponseMsg);
                        Session["slotExist"] = dt.Rows[0]["slotExist"].ToString();
                        Session["multiBook"] = dt.Rows[0]["multiBook"].ToString();
                        Session["isBookCheckInAvailable"] = dt.Rows[0]["isBookCheckInAvailable"].ToString();
                    }
                    else
                    {

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
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "erroralert('" + ex + "');", true);
        }
    }
    #endregion

    #region Branch  Master Class 
    public class BranchMasterClass
    {
        public String branchId { get; set; }
        public string branchName { get; set; }
        public string shortName { get; set; }
        public String parkingOwnerId { get; set; }
        public string parkingName { get; set; }
        public string blockOption { get; set; }
        public string slotExist { get; set; }
        public string multiBook { get; set; }
        public string isBookCheckInAvailable { get; set; }
        public string slotsOption { get; set; }
        public string branchOptions { get; set; }
        public string createdDate { get; set; }
        public List<branchOptionDetails> branchOptionDetails { get; set; }
    }
    public class branchOptionDetails
    {
        public string parkingOwnerConfigId { get; set; }
        public string parkingOwnerId { get; set; }
        public string branchId { get; set; }
        public string blockOption { get; set; }
        public string employeeOption { get; set; }
        public string floorOption { get; set; }
        public string slotsOption { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
    }

    #endregion
}

