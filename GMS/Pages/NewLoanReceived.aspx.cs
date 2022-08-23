using System;
using System.Web;
using System.Web.UI;
using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.DTO;
using DataAccessLayer.Utility;
using DataAccessLayer.BLL;
using DataAccessLayer.DTO.CustomerServices;
using System.Data;
using DataAccessLayer.DTO.HouseKeeping;

namespace ATOZWEBGMS.Pages
{
    public partial class NewLoanReceived : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                lblID.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_ID));

                A2ZCSPARAMETERDTO dto = A2ZCSPARAMETERDTO.GetParameterValue();
                DateTime dt = Converter.GetDateTime(dto.ProcessDate);
                string date = dt.ToString("dd/MM/yyyy");
                CtrlProcDate.Text = date;


                AccTypeDropdown();

                CurrencyDropdown();



                lblChequeNo.Visible = false;
                txtChequeNo.Visible = false;
                lblAccountName.Visible = false;
                ddlAccountName.Visible = false;

                ddlAccNo.Visible = false;

                DivReInput.Visible = false;
            }
        }


        private void PartyDropdown()
        {
            string sqlquery = "SELECT PartyCode,PartyName from A2ZPARTYCODE WHERE GroupCode='" + ddlLoanType.SelectedValue + "' GROUP BY PartyCode,PartyName";
            ddlPartyName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlPartyName, "A2ZACGMS");
        }

        protected void BtnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ERPModule.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        private void AccTypeDropdown()
        {
            string sqlquery = "SELECT AccTypeCode,AccTypeDescription from A2ZACCTYPE";
            ddlLoanType = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlLoanType, "A2ZACGMS");
        }


        private void CurrencyDropdown()
        {
            string sqlquery = "SELECT CurrencyCode,CurrencyName from A2ZCURRENCY WHERE CurrencyCode != 99";
            ddlCurrency = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlCurrency, "A2ZACGMS");
        }

        private void CashCodeDropdown()
        {
            string sqlquery = "SELECT PartyAccNo,PartyName from A2ZPARTYCODE WHERE GroupCode = 16 AND PartyCurrencyCode = " + ddlCurrency.SelectedValue + "";
            ddlAccountName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlAccountName, "A2ZACGMS");
        }

        private void BankCodeDropdown()
        {
            string sqlquery = "SELECT PartyAccNo,PartyName from A2ZPARTYCODE WHERE GroupCode = 22 AND PartyCurrencyCode = " + ddlCurrency.SelectedValue + "";
            ddlAccountName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlAccountName, "A2ZACGMS");
        }
        protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {

            A2ZCSPARAMETERDTO dto = A2ZCSPARAMETERDTO.GetParameterValue();
            DateTime ProcDate = Converter.GetDateTime(dto.ProcessDate.ToShortDateString());

            txtEffecDate.Text = ProcDate.ToString("dd/MM/yyyy");

            DateTime SLTEndDate = ProcDate.AddMonths(6);
            DateTime LLTEndDate = ProcDate.AddYears(1);


            if (ddlLoanType.SelectedValue == "11")
            {
                txtEndDate.Text = LLTEndDate.ToString("dd/MM/yyyy");
                txtCalculationDays.Text = (LLTEndDate - ProcDate).TotalDays.ToString();
            }

            if (ddlLoanType.SelectedValue == "12")
            {
                txtEndDate.Text = SLTEndDate.ToString("dd/MM/yyyy");
                txtCalculationDays.Text = (SLTEndDate - ProcDate).TotalDays.ToString();

            }


            PartyDropdown();
        }

        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransactionType.SelectedValue == "47")
            {
                lblChequeNo.Visible = true;
                txtChequeNo.Visible = true;
                lblAccountName.Visible = true;
                ddlAccountName.Visible = true;
                BankCodeDropdown();
            }
            else
            {
                lblChequeNo.Visible = false;
                txtChequeNo.Visible = false;
                lblAccountName.Visible = true;
                ddlAccountName.Visible = true;
                CashCodeDropdown();
            }
        }


        protected void txtPartyCode_TextChanged(object sender, EventArgs e)
        {
            if (txtPartyCode.Text != string.Empty)
            {
                int PartyCode = Converter.GetInteger(txtPartyCode.Text);
                A2ZPARTYDTO getDTO = (A2ZPARTYDTO.GetPartyInformation(PartyCode));

                if (getDTO.PartyCode > 0)
                {
                    ChkGroupCode.Text = Converter.GetString(getDTO.GroupCode);
                    if (ddlLoanType.SelectedValue == ChkGroupCode.Text)
                    {
                        ddlPartyName.SelectedValue = Converter.GetString(getDTO.PartyCode);
                    }
                    else
                    {
                        txtPartyCode.Text = string.Empty;
                        txtPartyCode.Focus();
                    }
                }
                else
                {
                    txtPartyCode.Text = string.Empty;
                    txtPartyCode.Focus();
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
        private void LoanAccDropdown()
        {
            lblMsgFlag.Text = "0";



            string sqlquery = "SELECT AccNo,CONCAT(AccNo,'-',A2ZCURRENCY.CurrencyName) AS AccNum from A2ZACCOUNT inner join A2ZCURRENCY on A2ZCURRENCY.CurrencyCode=A2ZACCOUNT.AccCurrency WHERE AccType = '" + ddlLoanType.SelectedValue + "' AND AccPartyNo = '" + txtPartyCode.Text + "' AND AccCurrency = '" + ddlCurrency.SelectedValue + "' AND AccStatus = 1 AND AccLoanAmt = 0";
            ddlAccNo = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlAccNo, "A2ZACGMS");


            string qry = "SELECT AccNo,AccNo,AccCurrency FROM A2ZACCOUNT where AccPartyNo='" + txtPartyCode.Text + "' and AccType='" + ddlLoanType.SelectedValue + "' AND AccCurrency = '" + ddlCurrency.SelectedValue + "' AND AccStatus = 1 AND AccLoanAmt = 0";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            int totrec = dt.Rows.Count;
            if (totrec == 1)
            {
                txtAccNo.Text = Converter.GetString(dt.Rows[0]["AccNo"]);
                ddlCurrency.SelectedValue = Converter.GetString(dt.Rows[0]["AccCurrency"]);


                txtAccNo.Visible = true;
                ddlAccNo.Visible = false;
            }
            else if (totrec > 1)
            {
                txtAccNo.Visible = false;

                ddlAccNo.Visible = true;
            }
            else
            {
                lblMsgFlag.Text = "1";
            }
        }


        protected void GetAccountCount()
        {
            string qry = "SELECT AccNo,AccNo FROM A2ZACCOUNT where AccPartyNo='" + txtPartyCode.Text + "' and AccType='" + ddlLoanType.SelectedValue + "'";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            int totrec = dt.Rows.Count;
            int newaccno = (totrec + 1);
            hdnNewAccNo.Text = Converter.GetString(newaccno);
        }

        protected void GenerateNewAccNo()
        {
            GetAccountCount();

            string input1 = Converter.GetString(hdnNewAccNo.Text).Length.ToString();

            string result1 = "";

            if (input1 == "1")
            {
                result1 = "000";
            }
            if (input1 == "2")
            {
                result1 = "00";
            }
            if (input1 == "3")
            {
                result1 = "0";
            }

            if (input1 == "4")
            {
                ctrlNewAccNo.Text = txtPartyCode.Text + hdnNewAccNo.Text;
            }

            if (input1 != "4")
            {
                ctrlNewAccNo.Text = txtPartyCode.Text + result1 + hdnNewAccNo.Text;
            }

        }


        protected void ClearRecords()
        {
            ddlPartyName.SelectedIndex = 0;
            txtPartyCode.Text = string.Empty;
            txtEffecDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            txtCalculationDays.Text = string.Empty;
            txtChequeNo.Text = string.Empty;
            txtLoanAmount.Text = string.Empty;
            txtInterestRate.Text = string.Empty;
            txtTranDesc.Text = string.Empty;
            ddlLoanType.SelectedIndex = 0;
            ddlCurrency.SelectedIndex = 0;
            ddlTransactionType.SelectedIndex = 0;
            ddlAccountName.SelectedIndex = 0;
            txtAccNo.Text = string.Empty;
            ddlAccNo.SelectedIndex = 0;
        }
        protected void UpdatedMSG()
        {
            string Msg = "";

            string a = "";
            string b = "";
            string c = "";
            string d = "";
            string x = "";
            string y = "";

            x = "Loan Type : ";
            y = ddlLoanType.SelectedItem.Text;


            c = "Generated New Voucher No. : ";
            d = string.Format(CtrlVoucherNo.Text);

            Msg += x;
            Msg += y;
            Msg += "\\n";
            Msg += "\\n";
            Msg += c;
            Msg += d;


            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "window.alert('" + Msg + "')", true);
            return;

        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //GenerateNewAccNo();


            if (txtLoanAmount.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Pleae Input Loan Amount');", true);
                return;
            }

            if (txtInterestRate.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Pleae Input Interest Rate');", true);
                return;
            }
            

            if (txtTranDesc.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Pleae Input Narration');", true);
                return;
            }





            A2ZVCHNOCTRLDTO getDTO = new A2ZVCHNOCTRLDTO();

            getDTO = (A2ZVCHNOCTRLDTO.GetLastDefaultVchNo());

            if (ddlLoanType.SelectedValue == "11")
            {
                CtrlVoucherNo.Text = "LLT" + getDTO.RecLastNo.ToString("000000");
            }
            else 
            {
                CtrlVoucherNo.Text = "SLT" + getDTO.RecLastNo.ToString("000000");
            }

            var prm = new object[19];

            prm[0] = lblID.Text;
            prm[1] = CtrlVoucherNo.Text;
            prm[2] = Converter.GetDateToYYYYMMDD(CtrlProcDate.Text);
            prm[3] = "1";
            prm[4] = "New Loan Received";
            prm[5] = txtPartyCode.Text;
            prm[6] = ddlLoanType.Text;
            prm[7] = txtAccNo.Text;
            prm[8] = ddlCurrency.SelectedValue;
            prm[9] = ddlTransactionType.SelectedValue;
            if (ddlTransactionType.SelectedValue == "47")
            {
                prm[10] = txtChequeNo.Text;
            }
            else
            {
                prm[10] = "0";
            }

            prm[11] = "0";
            prm[12] = ddlAccountName.SelectedValue;
            prm[13] = txtLoanAmount.Text;
            prm[14] = txtInterestRate.Text;
            prm[15] = txtTranDesc.Text;
            prm[16] = Converter.GetDateToYYYYMMDD(txtEffecDate.Text);
            prm[17] = Converter.GetDateToYYYYMMDD(txtEndDate.Text);
            prm[18] = txtCalculationDays.Text;

            int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("Sp_UpdateLoanReceiveTransaction", prm, "A2ZACGMS"));

            if (result == 0)
            {
                PrintVoucher();

                //UpdatedMSG();

                ClearRecords();

                txtPartyCode.Focus();
            }


        }

        private void PrintVoucher()
        {
            var prm = new object[3];

            prm[0] = txtAccNo.Text;          
            prm[1] = Converter.GetDateToYYYYMMDD(CtrlProcDate.Text);
            prm[2] = "0";
                       
            int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("SpM_GenerateSingleAccountBalance", prm, "A2ZACGMS"));

            if (result == 0)
            {
                
            }


            var p = A2ZERPSYSPRMDTO.GetParameterValue();
            string comName = p.PrmUnitName;
            string comAddress1 = p.PrmUnitAdd1;
            SessionStore.SaveToCustomStore(Params.COMPANY_NAME, comName);
            SessionStore.SaveToCustomStore(Params.BRANCH_ADDRESS, comAddress1);


            SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.VOUCHER_NO, CtrlVoucherNo.Text);
            // SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_TDATE, Converter.GetDateToYYYYMMDD(txtToDate.Text));

            SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.NFLAG, 0);

            SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_DATABASE_NAME_KEY, "A2ZACGMS");


            SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_FILE_NAME_KEY, "rptLoanPrincipalRcvVoucher");


            Response.Redirect("ReportServer.aspx", false);
        }


        protected void ddlAccNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAccNo.Text = ddlAccNo.SelectedValue;

            Int64 accno = Converter.GetLong(txtAccNo.Text);
            A2ZACCOUNTDTO getDTO = (A2ZACCOUNTDTO.GetInfoAccNo(accno));

            if (getDTO.AccNo > 0)
            {
                ddlCurrency.SelectedValue = Converter.GetString(getDTO.AccCurrency);

            }
        }


        protected void txtLoanAmount_TextChanged(object sender, EventArgs e)
        {
            lblReInput.Text = "Please Re-Input Loan Amount";

            lblReInputFlag.Text = "1";

            ReInputScreen();
        }

        protected void txtInterestRate_TextChanged(object sender, EventArgs e)
        {
            lblReInput.Text = "Please Re-Input Interest Rate";

            lblReInputFlag.Text = "2";

            ReInputScreen();
        }
        private void ReInputScreen()
        {
            DivReInput.Visible = true;

            btnUpdate.Enabled = false;
            btnCancel.Enabled = false;
            BtnExit.Enabled = false;


            //DivMainHeader.Attributes.CssStyle.Add("opacity", "0.7");
            //DivChkShowZero.Attributes.CssStyle.Add("opacity", "0.7");

            DivReInput.Style.Add("Top", "250px");
            DivReInput.Style.Add("left", "530px");
            DivReInput.Style.Add("position", "fixed");

            DivMain.Attributes.CssStyle.Add("opacity", "0.3");

            DivReInput.Attributes.CssStyle.Add("opacity", "300");
            DivReInput.Attributes.CssStyle.Add("z-index", "300");

            txtReInput.Focus();
        }
        protected void txtReInput_TextChanged(object sender, EventArgs e)
        {
            DivReInput.Visible = false;

            DivMain.Attributes.CssStyle.Add("opacity", "300");

            btnUpdate.Enabled = true;
            btnCancel.Enabled = true;
            BtnExit.Enabled = true;

            if (lblReInputFlag.Text == "1")
            {
                if (txtLoanAmount.Text == txtReInput.Text)
                {
                    txtReInput.Text = string.Empty;
                    txtInterestRate.Focus();
                    return;
                }
                else
                {
                    txtReInput.Text = string.Empty;
                    txtLoanAmount.Text = string.Empty;
                    txtLoanAmount.Focus();
                    return;
                }
            }
            else if (lblReInputFlag.Text == "2")
            {
                if (txtInterestRate.Text == txtReInput.Text)
                {
                    txtReInput.Text = string.Empty;
                    txtTranDesc.Focus();
                    return;
                }
                else
                {
                    txtReInput.Text = string.Empty;
                    txtInterestRate.Text = string.Empty;
                    txtInterestRate.Focus();
                }
            }
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoanAccDropdown();

            if (lblMsgFlag.Text == "1")
            {
                txtPartyCode.Text = string.Empty;
                ddlPartyName.SelectedIndex = 0;
                txtPartyCode.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Loan Acount No. Not Found');", true);
                return;
            }
        }





    }
}
