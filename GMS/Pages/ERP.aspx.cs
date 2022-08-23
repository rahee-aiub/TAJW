﻿using System;
using System.Web;
using System.Web.UI;
using DataAccessLayer.DTO;
using System.Drawing;
using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.Utility;
using DataAccessLayer.DTO.HouseKeeping;
using System.Data;
using DataAccessLayer.BLL;
using DataAccessLayer.DTO.CustomerServices;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace ATOZWEBGMS.Pages
{
    /// <summary>
    /// ONI 13/02/2016
    /// </summary>
    public partial class ERP : Page
    {
        SqlConnection con = new SqlConnection(DataAccessLayer.Constants.DBConstants.GetConnectionString("master"));
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                linkSMSReload.Visible = false;

                DivFromCashCode.Visible = false;

                linkSMSReload.Text = "SMS Reload";

                lblLastProcDate.ForeColor = Color.Black;
                lblNewProcDt.ForeColor = Color.Black;
                lblSOD.ForeColor = Color.Black;

                DivChangePassword.Visible = false;
                txtIdNo.Focus();
                btChangePassword.Visible = false;
                DivLogin.Visible = false;

                DivDetails.Visible = false;

                if (IsPostBack)
                {

                    lblPassword.Text = string.Empty;
                    lblold.Text = string.Empty;
                }
                else
                {

                    A2ZERPSYSPRMDTO dto1 = A2ZERPSYSPRMDTO.GetParameterValue();
                    hdndatapath.Text = Converter.GetString(dto1.PrmDataPath);

                    lblUserId.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_ID));


                    txtsmsMsg.ReadOnly = true;

                    DivGridViewSMS.Visible = false;
                    DivTextMsg.Visible = false;

                    linkSMSMessage.Visible = false;
                    lblNo.Visible = false;

                    GetSMSMsg();


                    DivLogin.Visible = false;
                    DivChangePassword.Visible = false;
                    btnLogin.Enabled = true;
                    DivLogin.Visible = false;
                    Session["dummy"] = "dummy";

                    AtoZUtility a2zUtility = new AtoZUtility();
                    string notifyMsg = string.Empty;
                   

                    //btnCustomerService2.Visible = false;

                 

                    btnHk.Visible = false;

                    btnHr.Visible = false;

                    btnInv.Visible = false;

                    btnBooth.Visible = false;

                 



                    A2ZERPSYSPRMDTO dto = A2ZERPSYSPRMDTO.GetParameterValue();
                 

                    string a = (string)Session["IdsNo"];

                    txtIdNo.Text = a;
                    CheckUserId();

                    string sqlQuery = @"SELECT * FROM dbo.A2ZSYSMODULECTRL  WHERE IDSNO='" + txtIdNo.Text + "'";

                    DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(sqlQuery, "A2ZHKGMS");

                    if (dt.Rows.Count > 0)
                    {

                        lblNoRec.Text = Converter.GetString(dt.Rows.Count);


                        DataView view = new DataView(dt);

                        foreach (DataRowView row in view)
                        {
                            int moduleNo = Converter.GetInteger(row["ModuleNo"].ToString());

                            if (dt.Rows.Count == 1)
                            {
                                string LogOut = (string)Session["LogOutFlag"];
                                if (LogOut == "1")
                                {
                                    Session["LogOutFlag"] = "";
                                    Response.Redirect("Login.aspx", false);
                                    return;
                                }
                                else
                                {
                                    hdnModule.Value = Converter.GetString(moduleNo);
                                    SODProcessFunction();
                                    if (lblMsgFlag.Text == "1")
                                    {
                                        ShowMessage();
                                    }
                                    else if (lblMsgFlag.Text == "2")
                                    {
                                        return;
                                    }

                                    if (moduleNo == 1)
                                    {
                                        lblSwFlag.Text = "0";

                                        if (lblSwFlag.Text == "0")
                                        {
                                           
                                        }
                                        else
                                        {
                                            CSFunc();
                                            return;
                                        }
                                    }
                                    else
                                        if (moduleNo == 2)
                                        {
                                            HKFunc();
                                            return;
                                        }
                                        else
                                            if (moduleNo == 3)
                                            {
                                                lblSwFlag.Text = "0";

                                                if (lblSwFlag.Text == "0")
                                                {
                                                   
                                                }
                                                else
                                                {
                                                    BoothFunc();
                                                    return;
                                                }

                                            }
                                }
                            }

                            switch (moduleNo)
                            {
                                case 1:
                                  //  btnCustomerService2.Visible = false;
                                    break;
                                case 2:
                                    btnHk.Visible = true;
                                    break;
                                case 3:
                                    btnBooth.Visible = true;
                                    break;

                          
                                default:

                                    break;
                            }

                        }


                    }
                    else
                    {

                        String csname1 = "PopupScript";
                        Type cstype = GetType();
                        ClientScriptManager cs = Page.ClientScript;

                        if (!cs.IsStartupScriptRegistered(cstype, csname1))
                        {
                            String cstext1 = "alert('This User Have No Permission.');";
                            cs.RegisterStartupScript(cstype, csname1, cstext1, true);

                            txtIdNo.Text = string.Empty;
                            txtPassword.Text = string.Empty;
                        }
                        return;

                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetSMSMsg()
        {
            string qry4 = "SELECT SMSNo,SMSDate FROM A2ZUSERSMS WHERE  SMSToIdsNo='" + lblUserId.Text + "' AND (SMSStatus=3)";
            DataTable dt4 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry4, "A2ZHKGMS");
            int totrec = dt4.Rows.Count;
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr2 in dt4.Rows)
                {
                    linkSMSMessage.Text = "New SMS Messages";
                    lblNo.Text = Converter.GetString(totrec);
                    DivSMS.Visible = true;
                    linkSMSMessage.Visible = true;
                    lblNo.Visible = true;

                }

            }

        }

        private void gvSMS()
        {
            string sqlquery3 = "SELECT SMSDate,SMSNo,SMSFromIdsNo,SMSNote,SMSStatus from A2ZUSERSMS WHERE  SMSToIdsNo='" + lblUserId.Text + "' AND (SMSStatus=3)";
            gvSMSInfo = DataAccessLayer.BLL.CommonManager.Instance.FillGridViewList(sqlquery3, gvSMSInfo, "A2ZHKGMS");
        }

        protected void linkSMSMessage_Click(object sender, EventArgs e)
        {
            if (DivGridViewSMS.Visible == false)
            {

                gvSMS();
                gvSMSInfo.Visible = true;

                DivGridViewSMS.Visible = true;
            }

        }

        protected void BtnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (DivTextMsg.Visible == false)
                {
                    Button b = (Button)sender;
                    GridViewRow r = (GridViewRow)b.NamingContainer;
                    Label smsNo = (Label)gvSMSInfo.Rows[r.RowIndex].Cells[2].FindControl("lblSMSNo");
                    Label smsNote = (Label)gvSMSInfo.Rows[r.RowIndex].Cells[4].FindControl("lblSMSNote");
                    Label smsStatus = (Label)gvSMSInfo.Rows[r.RowIndex].Cells[5].FindControl("lblSMSStatus");

                    int a = Converter.GetInteger(smsNo.Text);
                    lblsmsNo.Text = Converter.GetString(a);

                    DivTextMsg.Visible = true;
                    txtsmsMsg.Text = smsNote.Text;
                    txtsmsMsg.Visible = true;


                    lblStatus.Text = "4";



                    string CheckUp = "UPDATE A2ZUSERSMS SET SMSStatus='" + lblStatus.Text + "' WHERE SMSNo='" + lblsmsNo.Text + "'";
                    int rowEffect = Converter.GetInteger(DataAccessLayer.BLL.CommonManager.Instance.ExecuteNonQuery(CheckUp, "A2ZHKGMS"));
                    if (rowEffect > 0)
                    {
                        string qry4 = "SELECT SMSNo,SMSDate FROM A2ZUSERSMS WHERE  SMSToIdsNo='" + lblUserId.Text + "' AND (SMSStatus=3)";
                        DataTable dt4 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry4, "A2ZHKGMS");
                        int totrec = dt4.Rows.Count;
                        lblNo.Text = Converter.GetString(totrec);
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.BtnEdit_Click Problem');</script>");
                //throw ex;
            }

        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            string qry4 = "SELECT SMSNo,SMSDate FROM A2ZUSERSMS WHERE  SMSToIdsNo='" + lblUserId.Text + "' AND (SMSStatus=3)";
            DataTable dt4 = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry4, "A2ZHKGMS");
            if (dt4.Rows.Count <= 0)
            {
                DivGridViewSMS.Visible = false;

                DivGridViewSMS.Visible = false;
                DivTextMsg.Visible = false;

                DivSMS.Visible = false;
                linkSMSMessage.Visible = false;
                lblNo.Visible = false;

            }
            else
            {
                DivTextMsg.Visible = false;
                txtsmsMsg.Text = string.Empty;

                gvSMS();
                gvSMSInfo.Visible = true;
            }
        }

        protected void SODProcessFunction()
        {
            lblMsgFlag.Text = "0";

            A2ZERPSYSPRMDTO prmobj = A2ZERPSYSPRMDTO.GetParameterValue();

            if (prmobj.PrmEODStat == 0)
            {
                return;
            }
            else
            {
                A2ZSYSIDSDTO sysobj = A2ZSYSIDSDTO.GetUserInformation(Converter.GetInteger(txtIdNo.Text), "A2ZHKGMS");

                if (sysobj.SODflag == false)
                {
                    lblMsgFlag.Text = "2";
                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "alert('Access Denied START OF DAY NOT DONE');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);
                    }
                    return;
                }
                else
                {
                    lblMsgFlag.Text = "1";
                }
            }
        }

        protected void ShowMessage()
        {

            int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("Sp_DUMMYProcessDt", "A2ZACGMS"));

            if (result == 0)
            {
                var dt = A2ZCSPARAMETERDTO.GetParameterValue();
                DateTime processDate = dt.ProcessDate;
                DateTime NewprocessDate = dt.DummyProcessDate;


                string date = NewprocessDate.ToString("dd/MM/yyyy");
                lblNewProcDate.Text = date;

                txtLastProcDt.Text = Converter.GetString(String.Format("{0:D}", processDate));
                txtNewProcDt.Text = Converter.GetString(String.Format("{0:D}", NewprocessDate));

                lblLastYear.Text = Converter.GetString(processDate.Year);
                lblNewYear.Text = Converter.GetString(NewprocessDate.Year);

                txtStDay.Focus();
            }


            DivDetails.Visible = true;
            DivMain.Attributes.CssStyle.Add("opacity", "0.1");
            DivDetails.Style.Add("Top", "220px");
            DivDetails.Style.Add("Right", "140px");
            DivDetails.Style.Add("position", "fixed");
            DivDetails.Attributes.CssStyle.Add("opacity", "200");
            DivDetails.Attributes.CssStyle.Add("z-index", "200");

            lockbutton();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtStDay.Text == string.Empty)
                {
                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "alert('Please Input START OF DAY');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);
                    }

                    DivMain.Attributes.CssStyle.Add("opacity", "100");

                    DivDetails.Visible = false;
                    DivMain.Visible = true;

                    unlockbutton();
                    txtStDay.Text = string.Empty;

                    return;

                }

                if (txtStDay.Text == "START OF DAY")
                {
                    lblYearFlag.Text = "0";

                    lblProessing.Text = "Please Wait... Auto SOD Processing";

                    if (lblLastYear.Text != lblNewYear.Text)
                    {
                        lblYearFlag.Text = "1";
                        CreateNewYearDatabase();
                    }

                    
                    ProcessSOD();

                    return;

                }
                else
                {

                    DivMain.Attributes.CssStyle.Add("opacity", "100");

                    DivDetails.Visible = false;
                    DivMain.Visible = true;

                    unlockbutton();
                    txtStDay.Text = string.Empty;
                    txtStDay.Focus();

                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "alert('Please Input START OF DAY');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void ProcessSOD()
        {
            var prm = new object[3];
            prm[0] = txtIdNo.Text;

            DateTime fdate = DateTime.ParseExact(lblNewProcDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            prm[1] = fdate;
            prm[2] = lblYearFlag.Text;
            


            int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("Sp_SODProcess", prm, "A2ZACGMS"));

            if (result == 0)
            {
                UpdateEODStat();
                DivDetails.Visible = false;
                DivMain.Visible = true;

             

                ModuleFunction();
                return;
            }

        }

        protected void UpdateEODStat()
        {
            try
            {
                Int16 BStat = 0;

                int roweffect = A2ZERPSYSPRMDTO.UpdateEODStat(BStat);
                if (roweffect > 0)
                {

                }

            }
            catch (Exception)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.UpdateEODStat Problem');</script>");
                
            }

        }

        protected void btnHideMessageDiv_Click(object sender, EventArgs e)
        {
           
        }

        private void unlockbutton()
        {
            btnInv.Enabled = true;
            btnHr.Enabled = true;
            btnHk.Enabled = true;
            btnBooth.Enabled = true;
            //btnGl.Enabled = true;
            btnHome.Enabled = true;
            //btnCustomerService2.Enabled = false;
        }

        private void lockbutton()
        {
            btnInv.Enabled = false;
            btnHr.Enabled = false;
            btnHk.Enabled = false;
            btnBooth.Enabled = false;
            //btnGl.Enabled = false;
            btnHome.Enabled = false;
           // btnCustomerService2.Enabled = false;
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
        

        }

        protected void btChangePassword_Click(object sender, EventArgs e)
        {
            DivChangePassword.Visible = true;
            DivLogin.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            DivChangePassword.Visible = false;
            DivLogin.Visible = true;

            txtIdNo.Text = string.Empty;
            lblWelcome.Text = string.Empty;
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
         
        }

        protected void txtIdNo_TextChanged(object sender, EventArgs e)
        {

            try
            {
                int checkUser = CheckUserId();

                if (checkUser == 0)
                {
                    txtPassword.Focus();
                    btnLogin.Enabled = true;
                }
                else
                {
                    btnLogin.Enabled = false;

                    string msg = string.Empty;

                    switch (checkUser)
                    {
                        case 1:
                            msg = "User Id Not Available";
                            break;
                        case 2:
                            msg = "Change Password - New Id was Created";
                            break;
                        //case 3:
                        //    msg = "User Id is using by other client or Abnormal Logout";

                        //    break;
                        default:
                            msg = "Check User Id Information";
                            break;
                    }

                    String csname1 = "PopupScript";
                    Type cstype = GetType();
                    ClientScriptManager cs = Page.ClientScript;

                    if (!cs.IsStartupScriptRegistered(cstype, csname1))
                    {
                        String cstext1 = "alert('" + msg + "');";
                        cs.RegisterStartupScript(cstype, csname1, cstext1, true);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

      

        private void ModuleFunction()
        {
            if (hdnModule.Value == "1")
            {
                CSFunc();
            }
            else
                if (hdnModule.Value == "2")
                {
                    HKFunc();
                }
                else
                    if (hdnModule.Value == "3")
                    {
                        BoothFunc();
                    }
         
        }

        protected void btnCustomerService_Click(object sender, EventArgs e)
        {
            hdnModule.Value = "1";
            SODProcessFunction();
            if (lblMsgFlag.Text == "1")
            {
                ShowMessage();
            }
            else if (lblMsgFlag.Text == "2")
            {
                return;
            }
            else
            {

                CSFunc();
             

            }
        }

        protected void CSFunc()
        {
            try
            {
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_DBNAME, "A2ZACGMS");
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_MODULE, Enums.ModuleConstant.AccountsService);//Session["LoadSelectedModule"] = "Inventory";

                hdnModule.Value = "1";
              
                A2ZSYSIDSDTO.UpdateUserCSLoginFlag(Converter.GetInteger(txtIdNo.Text), 1);

                Login();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      
        protected void btnHk_Click(object sender, EventArgs e)
        {
            HKFunc();
        }

        protected void HKFunc()
        {
            try
            {
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_DBNAME, "A2ZHKGMS");
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_MODULE, Enums.ModuleConstant.HouseKeeping);//Session["LoadSelectedModule"] = "Inventory";

                hdnModule.Value = "2";
                //CheckUserId();

                int checkUser = CheckUserId();
                switch (checkUser)
                {
                    //case 3:
                    //    AbnormalMSG();
                    //    return;

                }

                A2ZSYSIDSDTO.UpdateUserHKLoginFlag(Converter.GetInteger(txtIdNo.Text), 1);


                Login();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void btnBooth_Click(object sender, EventArgs e)
        {
            hdnModule.Value = "3";
            SODProcessFunction();
            if (lblMsgFlag.Text == "1")
            {
                ShowMessage();
            }
            else if (lblMsgFlag.Text == "2")
            {
                return;
            }
            else
            {
                lblSwFlag.Text = "0";

                if (lblSwFlag.Text == "0")
                {
                    // SelectFromCashCodeSw();
                }
                else
                {
                    BoothFunc();
                }
            }

        }

        protected void BoothFunc()
        {
            try
            {
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_DBNAME, "A2ZBTCUBS");
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_MODULE, Enums.ModuleConstant.Booth);//Session["LoadSelectedModule"] = "Inventory";

                hdnModule.Value = "3";
               

                int checkUser = CheckUserId();
                switch (checkUser)
                {
          
                }


                lblCashCode.Text = txtGLCashCode.Text; //DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_GLCASHCODE));


                A2ZCSPARAMETERDTO dto2 = A2ZCSPARAMETERDTO.GetParameterValue();
                DateTime dt2 = Converter.GetDateTime(dto2.ProcessDate);
                //string date1 = dt2.ToString("dd/MM/yyyy");
                txtTranDate.Text = Converter.GetString(dt2);





                Login();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void btnInv_Click(object sender, EventArgs e)
        {

        }

        protected void btnHr_Click(object sender, EventArgs e)
        {
            hdnModule.Value = "3";
            SODProcessFunction();
            if (lblMsgFlag.Text == "1")
            {
                ShowMessage();
            }
            else if (lblMsgFlag.Text == "2")
            {
                return;
            }
            else
            {
                HRFunc();
            }

        }

        protected void HRFunc()
        {
            try
            {
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_DBNAME, "A2ZHRCUBS");
                SessionStore.SaveToCustomStore(Params.SYS_SELECT_MODULE, Enums.ModuleConstant.HumanResource);//Session["LoadSelectedModule"] = "Inventory";

                hdnModule.Value = "3";
               

                int checkUser = CheckUserId();
                switch (checkUser)
                {


                }


                Login();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            DivMain.Attributes.CssStyle.Add("opacity", "100");
            DivLogin.Visible = false;
            txtIdNo.Text = string.Empty;
            lblWelcome.Text = string.Empty;
            Response.Redirect("ERP.aspx", false);
        }

       
        protected int CheckUserId()
        {
            // For Return Value of CheckUserId()
            //---------------------------------
            // 0 = Id is Available 
            // 1 = ID not in Table
            // 2 = Please Change Password - New Id was created
            // 3 = Id was not Initialize - Abnormal Logout
            //---------------------------------
            // End of For Return Value of CheckUserId()

            try
            {
                lblWelcome.Text = null;
                txtIdsLevel.Text = null;
                txtIdsFlag.Text = null; // Using for Permission
                txtOrgPass.Text = null;
                A2ZSYSIDSDTO idsDto = new A2ZSYSIDSDTO();

                if (hdnModule.Value == "1")
                {
                    string dbName = Converter.GetString("A2ZACGMS");
                    idsDto = A2ZSYSIDSDTO.GetUserInformation(Converter.GetInteger(txtIdNo.Text), dbName);
                }

            

                if (hdnModule.Value == "2")
                {
                    string dbName = Converter.GetString("A2ZHKGMS");
                    idsDto = A2ZSYSIDSDTO.GetUserInformation(Converter.GetInteger(txtIdNo.Text), dbName);
                }

                if (hdnModule.Value == "3")
                {
                    string dbName = Converter.GetString("A2ZBTCUBS");
                    idsDto = A2ZSYSIDSDTO.GetUserInformation(Converter.GetInteger(txtIdNo.Text), dbName);
                }

             

                if (idsDto.IdsNo == 0)
                {
                    lblWelcome.Text = "ID Not Found";
                    lblWelcome.ForeColor = Color.Red;
                    DivLogin.Visible = true;
                    btChangePassword.Visible = false;
                    return 1;
                }

                //IdsLogInFlag 
                lblWelcome.Text = idsDto.IdsName;

                Session["UserName"] = lblWelcome.Text;

                txtIdsLevel.Text = Converter.GetString(idsDto.IdsLevel);

                Session["USERLAVEL"] = txtIdsLevel.Text;

                txtIdsFlag.Text = Converter.GetString(idsDto.IdsFlag); // Using for Permission
                txtOrgPass.Text = idsDto.IdsPass;

                txtGLCashCode.Text = Converter.GetString(idsDto.GLCashCode);
                txtUserBranchNo.Text = Converter.GetString(idsDto.UserBranchNo);

                txtUserEmpCode.Text = Converter.GetString(idsDto.EmpCode);
                txtServerIP.Text = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                txtServerName.Text = System.Net.Dns.GetHostName();
                txtUserIP.Text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                lblWelcome.ForeColor = Color.Green;
                DivLogin.Visible = true;
                btChangePassword.Visible = true;
                btChangePassword.Visible = true;


                if (idsDto.IdsPass == "XXXXXXXX")
                {
                    lblWelcome.ForeColor = Color.Red;
                    DivLogin.Visible = true;
                    return 2;
                }

                if (idsDto.IdsLogInFlag == 1)
                {
                    return 3;
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void AbnormalMSG()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('User Id is using by other client or Abnormal Logout');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }

        protected void ProcessDoneMSG()
        {
            String csname1 = "PopupScript";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                String cstext1 = "alert('Current Date Process Done');";
                cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            }

            return;

        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserRegistry.aspx", true);
        }

        protected void AddAuditInformation()
        {
            Int16 moduleNo = 0;

            Enums.ModuleConstant masterPage = Enums.ModuleConstant.HouseKeeping;
            if (SessionStore.ContainsKey(Params.SYS_SELECT_MODULE))//if (Session["LoadSelectedModule"] != null)
            {
                masterPage = (Enums.ModuleConstant)SessionStore.GetValue(Params.SYS_SELECT_MODULE);//Session["LoadSelectedModule"].ToString();
            }

            switch (masterPage)
            {
                case Enums.ModuleConstant.AccountsService:
                    moduleNo = 1;
                    break;

                case Enums.ModuleConstant.GeneralLedger:
                    moduleNo = 2;
                    break;
                case Enums.ModuleConstant.HouseKeeping:
                    moduleNo = 2;
                    break;
                case Enums.ModuleConstant.Booth:
                    moduleNo = 3;
                    break;
 
                default:
                    moduleNo = 1;
                    break;
            }

            var dto = new A2ZAUDITDTO();

            dto.UserId = Converter.GetInteger(SessionStore.GetValue(Params.SYS_USER_ID));
            dto.EmpCode = Converter.GetSmallInteger(SessionStore.GetValue(Params.SYS_USER_EMP_CODE));
            dto.UserIP = Converter.GetString(SessionStore.GetValue(Params.SYS_USER_IP));
            dto.UserServerIP = Converter.GetString(SessionStore.GetValue(Params.SYS_USER_SERVER_IP));
            dto.UserServerName = Converter.GetString(SessionStore.GetValue(Params.SYS_USER_SERVER_NAME));

       
        }

        protected void Login()
        {
            try
            {
                CheckUserId();
                string dbName = Converter.GetString(SessionStore.GetValue(Params.SYS_SELECT_DBNAME));
                if (hdnModule.Value == "1")
                {
                    A2ZSYSIDSDTO.UpdateUserCSLoginFlag(Converter.GetInteger(txtIdNo.Text), 1);
                }

              

                if (hdnModule.Value == "2")
                {
                    A2ZSYSIDSDTO.UpdateUserHKLoginFlag(Converter.GetInteger(txtIdNo.Text), 1);
                }
              
                SessionStore.SaveToCustomStore(Params.SYS_USER_ID, txtIdNo.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_NAME, lblWelcome.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_LEVEL, txtIdsLevel.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_PERMISSION, txtIdsFlag.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_EMP_CODE, txtUserEmpCode.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_SERVER_IP, txtServerIP.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_SERVER_NAME, txtServerName.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_IP, txtUserIP.Text);
                SessionStore.SaveToCustomStore(Params.SYS_USER_GLCASHCODE, ddlFCashCode.SelectedValue);
                SessionStore.SaveToCustomStore(Params.SYS_USER_BRNO, txtUserBranchNo.Text);

                AddAuditInformation();

                Response.Redirect("ERPModule.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            DataAccessLayer.DTO.HouseKeeping.A2ZERPSYSPRMDTO.UpdateNoOfUser(-1);

            Session.Clear();

            Response.Redirect("Login.aspx", false);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            DivMain.Attributes.CssStyle.Add("opacity", "100");

            DivDetails.Visible = false;
            DivMain.Visible = true;

            unlockbutton();
        }
        protected void linkSMSReload_Click(object sender, EventArgs e)
        {
            GetSMSMsg();
        }
        protected void BtnProceed_Click(object sender, EventArgs e)
        {
         

            if (ddlFCashCode.SelectedValue != "-Select-")
            {
                SessionStore.SaveToCustomStore(Params.SYS_USER_GLCASHCODE, ddlFCashCode.SelectedValue);

                ModuleFunction();
                return;
            }
            else
            {
                DivFromCashCode.Visible = true;
                ddlFCashCode.Focus();

            }
        }
        protected void BtnBack1_Click(object sender, EventArgs e)
        {
            if (lblNoRec.Text == "1")
            {
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                DivMain.Attributes.CssStyle.Add("opacity", "100");

                DivFromCashCode.Visible = false;
                DivMain.Visible = true;

                unlockbutton();
            }

        }
        protected void SelectFromCashCodeSw()
        {
            if (hdnModule.Value == "1")
            {
                lblModuleFunc.Text = "Customer Service Module";
            }
            else
             
                if (hdnModule.Value == "3")
                {
                    lblModuleFunc.Text = "Booth Module";
                }


            DivFromCashCode.Visible = true;
            CashCodeDropdown();

            string sqlQuery = "SELECT FromCashCode FROM dbo.A2ZUSERCASHCODE  WHERE IDSNO='" + txtIdNo.Text + "'";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(sqlQuery, "A2ZACGMS");
            if (dt.Rows.Count > 1)
            {
                DivMain.Attributes.CssStyle.Add("opacity", "0.0");
                DivFromCashCode.Style.Add("Top", "200px");
                DivFromCashCode.Style.Add("Right", "300px");
                DivFromCashCode.Style.Add("position", "fixed");
                DivFromCashCode.Attributes.CssStyle.Add("opacity", "200");
                DivFromCashCode.Attributes.CssStyle.Add("z-index", "200");

                lockbutton();
            }
            else
            {
                ddlFCashCode.SelectedValue = Converter.GetString(dt.Rows[0]["FromCashCode"]);
                SessionStore.SaveToCustomStore(Params.SYS_USER_GLCASHCODE, ddlFCashCode.SelectedValue);

                ModuleFunction();
                return;
            }

        }
        protected void CashCodeDropdown()
        {
            string sqlquery = @"SELECT FromCashCode,+ CAST (FromCashCode AS VARCHAR(100))+ '-' + LTRIM(RTRIM(FromCashCodeDesc)) from A2ZUSERCASHCODE WHERE IdsNo = '" + txtIdNo.Text + "'";
            //string sqlquery = "SELECT FromCashCode,FromCashCodeDesc from A2ZUSERCASHCODE WHERE IdsNo = '" + txtIdNo.Text + "'";
            ddlFCashCode = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlFCashCode, "A2ZACGMS");

        }


        public void CreateNewYearDatabase()
       
        {
            try
            {
                con.Open();
                string DatabaseName = "";
                int year = Converter.GetInteger(lblLastYear.Text);
                int yearr = year + 1;
                string financialYr = Converter.GetString(yearr);
                DatabaseName = "A2ZACGMST" + financialYr;
                hdndbname.Text = DatabaseName;
                
                string str = "CREATE DATABASE " + DatabaseName + " ON PRIMARY " +
                         "(NAME = MyDataBaseName, " +
                         "FILENAME = '" + hdndatapath.Text + "" + DatabaseName + ".mdf', " +
                         "SIZE = 3MB, MAXSIZE = UNLIMITED, FILEGROWTH = 10%) " +
                         "LOG ON (NAME = MyDataBaseName_log, " +
                        "FILENAME = '" + hdndatapath.Text + "" + DatabaseName + "_log.ldf', " +
                        "SIZE = 1MB, " +
                        "MAXSIZE = UNLIMITED, " +
                        "FILEGROWTH = 10%)";

              
                SqlCommand cmd = new SqlCommand(str, con);
                cmd.ExecuteNonQuery();
                SqlConnection.ClearAllPools();


                con.Dispose();
                con.Close();
                string CSOpBaltblename = "A2ZACOPBALANCE";
                ACOPCreate(CSOpBaltblename);

                string Trntblename = "A2ZTRANSACTION";
                TrnCreate(Trntblename);

                UpdateDatabseFile();


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.Process End Problem');</script>");
               
            }



        }

        public void ACOPCreate(string TName)
        {
            SqlConnection conn = new SqlConnection(DataAccessLayer.Constants.DBConstants.GetConnectionString(hdndbname.Text));

            try
            {
                using (SqlCommand cmd = new SqlCommand("CREATE TABLE [dbo].[" + TName + "]("
                                + "[Id] [int] IDENTITY(1,1) NOT NULL,"
                                + "[TrnDate] [smalldatetime] NOT NULL,"
                                + "[KeyNo] [int] NULL,"
                                + "[AccType] [int] NULL,"
                                + "[AccNo] [bigint] NULL,"
                                + "[TrnPrincAmount] [money] NULL,"
                                + "[TrnIntAmount] [money] NULL,"
                                + "[TrnAmount] [money] NULL,"
                                + "CONSTRAINT [PK_" + TName + "] PRIMARY KEY CLUSTERED "
                                + "("
                                + "[ID] ASC"
                                + ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]"
                                + ") ON [PRIMARY]", conn))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        public void TrnCreate(string TName)
        {
            SqlConnection conn = new SqlConnection(DataAccessLayer.Constants.DBConstants.GetConnectionString(hdndbname.Text));

            try
            {
                using (SqlCommand cmd = new SqlCommand("CREATE TABLE [dbo].[" + TName + "]("
                                + "[Id] [int] IDENTITY(1,1) NOT NULL,"
                                + "[TrnDate] [smalldatetime] NULL,"
                                + "[VchNo] [nvarchar](20) NULL,"
                                + "[VoucherNo]  [nvarchar](20) NULL,"
                                + "[TrnKeyNo][int] NULL,"
                                + "[CurrencyCode][int] NULL,"
                                + "[AccType] [int] NULL,"
                                + "[AccNo] [bigint] NULL,"
                                + "[FuncOpt]  [smallint] NULL,"
                                + "[FuncOptDesc] [nvarchar](100) NULL,"
                                + "[PayType] [int] NULL,"
                                + "[TrnType] [tinyint] NULL,"
                                + "[TrnDrCr] [tinyint] NULL,"
                                + "[TrnPrincDebit] [money] NULL ,"
                                + "[TrnPrincCredit] [money] NULL ,"
                                + "[TrnIntDebit] [money] NULL ,"
                                + "[TrnIntCredit] [money] NULL ,"
                                + "[TrnDebitAmt] [money] NULL ,"
                                + "[TrnCreditAmt] [money] NULL,"
                                + "[TrnDebitWeight] [money] NULL,"
                                + "[TrnCreditWeight] [money] NULL,"
                                + "[TrnIntRate] [money] NULL,"
                                + "[ExchRate] [decimal](18, 8) NULL,"
                                + "[TrnDesc] [nvarchar](100) NULL,"
                                + "[TrnVchType] [nvarchar](1) NULL,"
                                + "[TrnChqPrx] [nvarchar](7) NULL,"
                                + "[TrnChqNo] [nvarchar](15) NULL,"
                                + "[TrnBankChqNo] [nvarchar](50) NULL,"
                                + "[TrnCSGL] [tinyint] NULL,"
                                + "[AccKeyNo] [int] NULL,"
                                + "[AccDebitAmt] [money] NULL,"
                                + "[AccCreditAmt] [money] NULL,"
                                + "[AccDebitWeight] [money] NULL,"
                                + "[AccCreditWeight] [money] NULL,"
                                + "[TrnDiscountAmt] [money] NULL,"
                                + "[TrnFlag] [tinyint] NULL,"
                                + "[TrnProcStat] [tinyint] NULL,"
                                + "[TrnSysUser] [tinyint] NULL,"
                                + "[UserID] [int] NULL,"
                                + "[VerifyUserID] [int] NULL,"
                                + "[CreateDate] [datetime] NULL,"
                                + "[TrnSwitch] [tinyint] NULL,"
                                + "CONSTRAINT [PK_" + TName + "] PRIMARY KEY CLUSTERED "
                                + "("
                                + "[ID] ASC"
                                + ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]"
                                + ") ON [PRIMARY]", conn))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void UpdateDatabseFile()
        {
            try
            {
                A2ZDATABASEDTO objDTO = new A2ZDATABASEDTO();

                objDTO.DatabaseName = Converter.GetString(hdndbname.Text);

                int roweffect = A2ZDATABASEDTO.InsertInformation(objDTO);
                if (roweffect > 0)
                {

                }



            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.UpdateEODStat Problem');</script>");
               
            }

        }

        protected void btnCustomerService_Click(object sender, ImageClickEventArgs e)
        {
            hdnModule.Value = "1";
            SODProcessFunction();
            if (lblMsgFlag.Text == "1")
            {
                ShowMessage();
            }
            else if (lblMsgFlag.Text == "2")
            {
                return;
            }
            else
            {
                CSFunc();
            }
        }

        protected void btnHk_Click1(object sender, ImageClickEventArgs e)
        {
            HKFunc();
        }

        protected void btnExitProgram_Click(object sender, ImageClickEventArgs e)
        {
            DataAccessLayer.DTO.HouseKeeping.A2ZERPSYSPRMDTO.UpdateNoOfUser(-1);

            Session.Clear();

            Response.Redirect("Login.aspx", false);
        }


    }
}
