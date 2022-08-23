﻿using System;
using ATOZWEBGMS.WebSessionStore;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer.DTO.CustomerServices;
using DataAccessLayer.DTO.HouseKeeping;
using DataAccessLayer.Utility;
using System.Data;
using DataAccessLayer.BLL;
using DataAccessLayer.DTO.SystemControl;

namespace ATOZWEBGMS.Pages
{
    public partial class PartyAccountStatement : System.Web.UI.Page
    {

        //public string TmpOpenDate;
        //public string TmpAccMatureDate;
        //public string TmpAccPrevRenwlDate;
        //public Int16 TmpAccPeriod;
        //public Decimal TmpAccOrgAmt;
        //public Decimal TmpAccPrincipal;
        //public Decimal TmpAccIntRate;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    string NewAccNo = (string)Session["NewAccNo"];
                    string flag = (string)Session["flag"];
                    lblflag.Text = flag;


                    lblID.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_ID));
                    lblIDName.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_NAME));


                    //lblCashCode.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_GLCASHCODE));

                    //lblBranchNo.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.BRNO));
                    //lblUnitFlag.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_UNIT_FLAG));
                    //lblUserLabel.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_LEVEL));


                    string cflag = (string)Session["CFlag"];

                    CFlag.Text = cflag;

                    string Module = (string)Session["SModule"];

                    if (lblflag.Text == string.Empty)
                    {
                        lblModule.Text = Request.QueryString["a%b"];
                        MultiAccFlag.Text = "0";

                    }
                    else
                    {
                        lblModule.Text = Module;
                    }


                    PartyDropdown();
                    
                  
                    string PFlag = (string)Session["ProgFlag"];
                    CtrlProgFlag.Text = PFlag;

                    if (CtrlProgFlag.Text != "1")
                    {


                        var p = A2ZERPSYSPRMDTO.GetParameterValue();
                        lblCompanyName.Text = Converter.GetString(p.PrmUnitName);
                        lblBranchName.Text = Converter.GetString(p.PrmUnitName);
                        
                        
                        //var p = A2ZERPSYSPRMDTO.GetParameterValue();
                        //lblBranchNo.Text = Converter.GetString(p.PrmUnitNo);

                        A2ZCSPARAMETERDTO dto = A2ZCSPARAMETERDTO.GetParameterValue();
                        DateTime dt = Converter.GetDateTime(dto.ProcessDate);
                        string date = dt.ToString("dd/MM/yyyy");
                        txtfdate.Text = Converter.GetString(date);
                        txttdate.Text = Converter.GetString(date);
                        lblProcDate.Text = Converter.GetString(date);

                        CtrlOldAccNo.Text = Converter.GetString(dto.OldAccNoFlag);



                        FunctionName();
                    }
                    else
                    {
                        string RtxtPartyCode = (string)Session["StxtPartyCode"];
                        string RddlPartyName = (string)Session["SddlPartyName"];
                                               
                        string Rtxtfdate = (string)Session["Stxtfdate"];
                        string Rtxttdate = (string)Session["Stxttdate"];

                        txtPartyCode.Text = RtxtPartyCode;
                        ddlPartyName.SelectedValue = RddlPartyName;

                        txtfdate.Text = Rtxtfdate;
                        txttdate.Text = Rtxttdate;     
                    }


                    A2ZERPSYSPRMDTO dto1 = A2ZERPSYSPRMDTO.GetParameterValue();
                    lblBegFinYear.Text = Converter.GetString(dto1.PrmBegFinYear);

                    FunctionName();

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.Page_Load Problem');</script>");


                //throw ex;
            }
        }



        private void PartyDropdown()
        {
            string sqlquery = "SELECT PartyCode,PartyName from A2ZPARTYCODE WHERE GroupCode != 11 and GroupCode != 12 and GroupCode !=16 and GroupCode !=51 and GroupCode !=22 GROUP BY PartyCode,PartyName";
            ddlPartyName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlPartyName, "A2ZACGMS");
        }



        protected void FunctionName()
        {

            lblStatementFunc.Text = "Party Account Statement";

        }


        protected void StoreRecordsSession()
        {
            Session["ProgFlag"] = "1";


            Session["StxtPartyCode"] = txtPartyCode.Text;
            Session["SddlPartyName"] = ddlPartyName.SelectedValue;

            Session["Stxtfdate"] = txtfdate.Text;
            Session["Stxttdate"] = txttdate.Text;

            




            Session["SlblProcDate"] = lblProcDate.Text;



            Session["SModule"] = lblModule.Text;

            Session["flag"] = "1";



            Session["CFlag"] = CFlag.Text;

            

        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtPartyCode.Text == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Party Code Not Selected');", true);
                    return;
                }

                StoreRecordsSession();

                var p = A2ZERPSYSPRMDTO.GetParameterValue();
                string comName = p.PrmUnitName;
                string comAddress1 = p.PrmUnitAdd1;

                SessionStore.SaveToCustomStore(Params.COMPANY_NAME, comName);
                SessionStore.SaveToCustomStore(Params.BRANCH_ADDRESS, comAddress1);

                DateTime fdate = DateTime.ParseExact(txtfdate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime tdate = DateTime.ParseExact(txttdate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                int fYear = fdate.Year;
                int bYear = Converter.GetInteger(lblBegFinYear.Text);

                if (fYear < bYear)
                {
                    txtfdate.Text = lblProcDate.Text;
                    txtfdate.Focus();

                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid From Date Input');", true);
                    return;
                }


                // FOR From Date and To Date
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_FDATE, Converter.GetDateToYYYYMMDD(txtfdate.Text));
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_TDATE, Converter.GetDateToYYYYMMDD(txttdate.Text));




                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.KEYCODE, txtPartyCode.Text);

                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_NAME1, ddlPartyName.SelectedItem.Text);
                
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.ACCNO, 0);
               



                

                int id = Converter.GetInteger(lblID.Text);
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.USERNO, id);
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.USERNAME, lblIDName.Text);
               

                //============== End For Multi User Parameter =========================
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_DATABASE_NAME_KEY, "A2ZACGMS");

                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.NFLAG, 0);
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_FILE_NAME_KEY, "rptMPartyAccountStatement");

                Response.Redirect("ReportServer.aspx", false);

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.BtnView_Click Problem');</script>");
                //throw ex;
            }
        }


        protected void RemoveSession()
        {
            Session["flag"] = string.Empty;
            Session["NewAccNo"] = string.Empty;
            Session["RTranDate"] = string.Empty;
            Session["SFuncOpt"] = string.Empty;
            Session["SModule"] = string.Empty;
            Session["SControlFlag"] = string.Empty;

            Session["ProgFlag"] = string.Empty;

            Session["CFlag"] = string.Empty;

            Session["SddlBranchNo"] = string.Empty;

            Session["StxtAccNo"] = string.Empty;
            Session["SlblAccTitle"] = string.Empty;

            Session["SlblBranchNo"] = string.Empty;

            Session["SCtrlAccType"] = string.Empty;

            Session["StxtAccType"] = string.Empty;
            Session["SddlAccType"] = string.Empty;

            Session["StxtAccType1"] = string.Empty;
            Session["SddlAccType1"] = string.Empty;

            Session["StxtOldAccNo1"] = string.Empty;


            Session["StxtCULBMemNo"] = string.Empty;
            Session["SlblMemName"] = string.Empty;


            Session["StxtMemberNo"] = string.Empty;
            Session["SlblMemType"] = string.Empty;
            Session["SlblMemNo"] = string.Empty;


            Session["StxtAccountNo"] = string.Empty;
            Session["Stxtstat"] = string.Empty;


            Session["Stxtfdate"] = string.Empty;
            Session["Stxttdate"] = string.Empty;


            Session["SChkInterest"] = string.Empty;

            Session["SChkAccStatus"] = string.Empty;

            Session["SlblProcDate"] = string.Empty;


            Session["StClass"] = string.Empty;
            Session["SlblTrnCode"] = string.Empty;
            Session["StOpenDt"] = string.Empty;
            Session["StMaturityDt"] = string.Empty;
            Session["StRenewalDt"] = string.Empty;
            Session["StAccLoanSancDate"] = string.Empty;
            Session["StAccDisbDate"] = string.Empty;
            Session["StAccPeriod"] = string.Empty;
            Session["StOrgAmt"] = string.Empty;
            Session["StPrincipleAmt"] = string.Empty;
            Session["StIntRate"] = string.Empty;
            Session["StAccLoanSancAmt"] = string.Empty;
            Session["StAccDisbAmt"] = string.Empty;
            Session["StAccNoInstl"] = string.Empty;
            Session["StAccLoanInstlAmt"] = string.Empty;
            Session["StAccLoanLastInstlAmt"] = string.Empty;

            Session["SlblPreAddressLine1"] = string.Empty;
            Session["SlblPreTelephone"] = string.Empty;
            Session["SlblPreMobile"] = string.Empty;

            Session["SlblAutoRenewal"] = string.Empty;
            Session["SlblDepositAmount"] = string.Empty;
            Session["SlblTotDepositAmount"] = string.Empty;
            Session["StAccFixedMthInt"] = string.Empty;

            Session["SMultiAccFlag"] = string.Empty;



        }
        protected void BtnExit_Click(object sender, EventArgs e)
        {
            RemoveSession();
            Response.Redirect("ERPModule.aspx");
        }


        protected void InvalidCreditUnionMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Credit Union');", true);
            return;
        }

        protected void InvalidMemberMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Member No.');", true);
            return;
        }
        protected void InvalidAccountMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Account No.');", true);
            return;
        }


        protected void InvalidAccountNoMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Account Does Not Exist');", true);

            return;
        }

        protected void InvalidCuNoMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Credit Union');", true);
            return;
        }

        protected void AccessDeniedMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Not Allowed Credit Union');", true);
            return;
        }


        private void InvalidAccountNotMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Account Number');", true);
            return;
        }
        protected void AccTransferedMSG()
        {
            //String csname1 = "PopupScript";
            //Type cstype = GetType();
            //ClientScriptManager cs = Page.ClientScript;

            //if (!cs.IsStartupScriptRegistered(cstype, csname1))
            //{
            //    String cstext1 = "alert('Account not Active');";
            //    cs.RegisterStartupScript(cstype, csname1, cstext1, true);
            //}

            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Account Transfered');", true);
            return;
        }


        

        public void clearInfo()
        {

            tOpenDt.Text = string.Empty;
            tMaturityDt.Text = string.Empty;
            tRenewalDt.Text = string.Empty;
            tAccPeriod.Text = string.Empty;
            tOrgAmt.Text = string.Empty;
            tPrincipleAmt.Text = string.Empty;
            tIntRate.Text = string.Empty;
        }


        private void InvalidMemMSG()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Member No.');", true);
            return;
        }


        

        private void InvalidAcc()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Account Not Found');", true);
            return;
        }


       



        protected void txtPartyCode_TextChanged(object sender, EventArgs e)
        {
            if (txtPartyCode.Text != string.Empty)
            {
                int PartyCode = Converter.GetInteger(txtPartyCode.Text);
                A2ZPARTYDTO getDTO = (A2ZPARTYDTO.GetPartyInformation(PartyCode));

                if (getDTO.PartyName == null)
                {
                    //txtPartyName.Text = string.Empty;
                    txtPartyCode.Text = string.Empty;
                    txtPartyCode.Focus();
                }

                else
                {
                    ddlPartyName.SelectedValue = txtPartyCode.Text;

                   
                }
            }
        }

        protected void ddlPartyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPartyName.SelectedIndex != 0)
            {
                int PartyCode = Converter.GetInteger(ddlPartyName.SelectedValue);
                A2ZPARTYDTO getDTO = (A2ZPARTYDTO.GetPartyInformation(PartyCode));

                if (getDTO.PartyName != string.Empty)
                {
                    txtPartyCode.Text = Converter.GetString(getDTO.PartyCode);
                    
                }
            }
        }

    }

}
