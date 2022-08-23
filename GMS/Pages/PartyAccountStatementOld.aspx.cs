using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.BLL;
using DataAccessLayer.Utility;
using DataAccessLayer.DTO.CustomerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer.DTO.HouseKeeping;

namespace ATOZWEBGMS.Pages
{
    public partial class PartyAccountStatementOld : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PartyDropdown();
                lblID.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_ID));
                lblUsername.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_NAME));
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlPartyName.SelectedIndex == 0)
                {
                    return;
                }
                if (txtfdate.Text == string.Empty || txttdate.Text == string.Empty)
                {
                    return;
                }

               
                var prm = new object[5];
                prm[0] = Convert.ToInt32(lblPartyNo.Text);
                prm[1] = "0";
                prm[2] = DataAccessLayer.Utility.Converter.GetDateToYYYYMMDD(txtfdate.Text);
                prm[3] = DataAccessLayer.Utility.Converter.GetDateToYYYYMMDD(txttdate.Text);
                prm[4] = 0;

                int result = Convert.ToInt32(CommonManager.Instance.GetScalarValueBySp("[SpM_PartyAccountStatement1]", prm, "A2ZACGMS"));
                
                var p = A2ZERPSYSPRMDTO.GetParameterValue();
                string comName = p.PrmUnitName;
                string comAddress1 = p.PrmUnitAdd1;
                SessionStore.SaveToCustomStore(Params.COMPANY_NAME, comName);
                SessionStore.SaveToCustomStore(Params.BRANCH_ADDRESS, comAddress1);
                SessionStore.SaveToCustomStore(Params.USERNO, lblID.Text);
                SessionStore.SaveToCustomStore(Params.USERNAME, lblUsername.Text);
                SessionStore.SaveToCustomStore(Params.COMMON_NAME1, ddlPartyName.SelectedItem.Text);

                SessionStore.SaveToCustomStore(Params.NFLAG, 0);
                SessionStore.SaveToCustomStore(Params.KEYCODE, 0);
                SessionStore.SaveToCustomStore(Params.ACCNO, 0);                

                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_FDATE, DataAccessLayer.Utility.Converter.GetDateToYYYYMMDD(txtfdate.Text));
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_TDATE, DataAccessLayer.Utility.Converter.GetDateToYYYYMMDD(txttdate.Text));
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_DATABASE_NAME_KEY, "A2ZACGMS");
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_FILE_NAME_KEY, "rptMPartyAccountStatement1");

                Response.Redirect("ReportServer.aspx", false);


                //if (result == 0)
                //{
                //    SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_FILE_NAME_KEY, "rptMPartyAccountStatement1");
                //}
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }

        protected void BtnExit_Click(object sender, EventArgs e)
        {
            RemoveSession();
            Response.Redirect("ERPModule.aspx");
        }

        protected void txtPartyCode_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPartyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPartyNo.Text = ddlPartyName.SelectedValue;
            txtPartyCode.Text = lblPartyNo.Text;
            FindAccountNo();
        }

        private void FindAccountNo()
        {
            MSGFlag.Text = "0";

            string qry = string.Empty;

            //if (ChkAccStatus.Checked == true)
            //{
            qry = "SELECT AccType,AccNo FROM A2ZACCOUNT WHERE AccPartyNo='" + lblPartyNo.Text.Trim() + "'  AND AccStatus < 98";
            //}
            //else
            //{
            //    qry = "SELECT AccType,AccNo FROM A2ZACCOUNT WHERE AccPartyNo='" + txtPartyCode.Text + "' AND AccStatus = 99";
            //}


            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count == 1)
                {

                    foreach (DataRow dr in dt.Rows)
                    {

                        lblAccNo.Text = dr["AccNo"].ToString();
                        GetInfo();
                    }
                }
                else
                {
                    MultiAccFlag.Text = "1";

                    //gvGroupAccInfo.Visible = true;

                    //string sqlquery3 = "SELECT distinct AccType,A2ZACCTYPE.AccTypeDescription,AccNo,AccStatus,AccStatusDescription FROM A2ZACCOUNT inner join A2ZACCTYPE on A2ZACCTYPE.AccTypeCode=A2ZACCOUNT.AccType inner join A2ZACCSTATUS on A2ZACCSTATUS.AccStatusCode=A2ZACCOUNT.AccStatus WHERE AccPartyNo='" + txtPartyCode.Text + "' AND AccType='" + txtAccType.Text + "' AND AccStatus < 98";
                    //gvGroupAccInfo = DataAccessLayer.BLL.CommonManager.Instance.FillGridViewList(sqlquery3, gvGroupAccInfo, "A2ZACGMS");

                }
            }
            else
            {
                MSGFlag.Text = "1";
            }


        }
       

        protected void GetInfo()
        {
            try
            {
                Int64 AccNumber = Convert.ToInt64(lblAccNo.Text);
                A2ZACCOUNTDTO getDTO = (A2ZACCOUNTDTO.GetInfoAccNo(AccNumber));

                if (getDTO.a > 0)
                {
                    CtrlAccStatus.Text = getDTO.AccStatus.ToString();
                    CtrlAccType.Text = getDTO.AccType.ToString();


                    if (CtrlAccStatus.Text == "98")
                    {

                        return;
                    }

                    Int16 AccStat = Convert.ToInt16(CtrlAccStatus.Text);
                    A2ZACCSTATUSDTO get3DTO = (A2ZACCSTATUSDTO.GetInformation(AccStat));
                    if (get3DTO.AccStatusCode > 0)
                    {
                        stat.Text = get3DTO.AccStatusDescription.ToString();
                    }

                    txtPartyCode.Text = getDTO.AccPartyNo.ToString();

                    //txtAccType.Text = Converter.GetString(CtrlAccType.Text);
                    //ddlAccType.SelectedValue = Converter.GetString(CtrlAccType.Text);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.GetInfo Problem');</script>");
            }
        }

        private void PartyDropdown()
        {
            string sqlquery = "SELECT DISTINCT PartyCode,PartyName from A2ZPARTYCODE";
            ddlPartyName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlPartyName, "A2ZACGMS");
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
    }
}