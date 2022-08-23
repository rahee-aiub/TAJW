using System;
using System.Web;
using System.Web.UI;
using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.DTO;
using DataAccessLayer.Utility;
using DataAccessLayer.BLL;
using DataAccessLayer.DTO.CustomerServices;
using System.Data;

namespace ATOZWEBGMS.Pages
{
    public partial class LoanInterestRateChange : System.Web.UI.Page
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

                ddlAccNo.Visible = false;

                DivReInput.Visible = false;




            }
        }

        private void CurrencyDropdown()
        {
            string sqlquery = "SELECT CurrencyCode,CurrencyName from A2ZCURRENCY WHERE CurrencyCode != 99";
            ddlCurrency = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlCurrency, "A2ZACGMS");
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

        private void LoanAccDropdown(int Atype)
        {
            string sqlquery = "SELECT AccNo,CONCAT(AccNo,'-',A2ZCURRENCY.CurrencyName) AS AccNum from A2ZACCOUNT inner join A2ZCURRENCY on A2ZCURRENCY.CurrencyCode=A2ZACCOUNT.AccCurrency WHERE AccType = " + Atype + " AND AccPartyNo = " + txtPartyCode.Text + " AND AccCurrency = " + ddlCurrency.SelectedValue + " AND AccStatus = 1 AND AccLoanAmt > 0";
            
            ddlAccNo = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlAccNo, "A2ZACGMS");

            string qry = "SELECT AccNo,AccNo FROM A2ZACCOUNT where AccPartyNo='" + txtPartyCode.Text + "' and AccType='" + ddlLoanType.SelectedValue + "' AND AccCurrency = '" + ddlCurrency.SelectedValue + "' AND AccStatus = 1 AND AccLoanAmt > 0";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            int totrec = dt.Rows.Count;
            if (totrec == 1)
            {
                txtAccNo.Text = Converter.GetString(dt.Rows[0]["AccNo"]);

                Int64 accno = Converter.GetLong(txtAccNo.Text);
                A2ZACCOUNTDTO getDTO = (A2ZACCOUNTDTO.GetInfoAccNo(accno));

                if (getDTO.AccNo > 0)
                {
                    txtAccNo.Text = Converter.GetString(getDTO.AccNo);
                    txtCurrentBalance.Text = Converter.GetString(String.Format("{0:0,0.00}", getDTO.AccBalance));
                    txtInterestRate.Text = Converter.GetString(String.Format("{0:0,0.000}", getDTO.AccIntRate));
                    lblCurrencyCode.Text = Converter.GetString(getDTO.AccCurrency);

                    //int code = Converter.GetInteger(lblCurrencyCode.Text);
                    //A2ZCURRENCYDTO get1DTO = (A2ZCURRENCYDTO.GetInformation(code));

                    //if (get1DTO.CurrencyCode > 0)
                    //{
                    //    txtCurrency.Text = Converter.GetString(get1DTO.CurrencyName);
                    //}

                }

                txtAccNo.Visible = true;
                ddlAccNo.Visible = false;
                return;
            }
            else if (totrec > 1)
            {
                txtAccNo.Visible = false;
                ddlAccNo.Visible = true;
                return;
            }
            else
            {

            }
        }




        protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {


            PartyDropdown();


        }



        protected void txtPartyCode_TextChanged(object sender, EventArgs e)
        {
            if (txtPartyCode.Text != string.Empty)
            {
                int PartyCode = Converter.GetInteger(txtPartyCode.Text);
                A2ZPARTYDTO getDTO = (A2ZPARTYDTO.GetPartyInformation(PartyCode));

                if (getDTO.PartyName == null)
                {
                    txtPartyCode.Text = string.Empty;
                    txtPartyCode.Focus();
                }

                else
                {
                   
                    txtInterestRate.Text = string.Empty;
                    txtCurrentBalance.Text = string.Empty;

                    if (ddlLoanType.SelectedIndex != 0)
                    {
                        if (ddlLoanType.SelectedValue == "11")
                        {
                            LoanAccDropdown(11);
                        }
                        if (ddlLoanType.SelectedValue == "12")
                        {
                            LoanAccDropdown(12);
                        }
                    }

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

                    
                    txtInterestRate.Text = string.Empty;
                    txtCurrentBalance.Text = string.Empty;

                    
                }
            }
        }

        protected void ClearRecords()
        {

            txtNewIntRate.Text = string.Empty;

            ddlPartyName.SelectedIndex = 0;
            ddlLoanType.SelectedIndex = 0;

            txtPartyCode.Text = string.Empty;
            txtAccNo.Text = string.Empty;
            ddlAccNo.SelectedIndex = 0;
            txtCurrentBalance.Text = string.Empty;
           
            txtInterestRate.Text = string.Empty;
            ddlCurrency.SelectedIndex = 0;


        }
        protected void UpdatedMSG()
        {
            string Msg = "";
            string a = "";
            string b = "";
            string c = "";
            string d = "";

            a = "Data Update Sucessfully";
            b = string.Format(ddlAccNo.SelectedValue);

            c = "Generated New Voucher No.";
            d = string.Format(CtrlVoucherNo.Text);

            Msg += a;
            //Msg += b;
            //Msg += "\\n";
            //Msg += "\\n";
            //Msg += c;
            //Msg += d;


            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "window.alert('" + Msg + "')", true);
            return;

        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                var prm = new object[2];

                prm[0] = txtAccNo.Text;
                prm[1] = txtNewIntRate.Text;

                int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("Sp_UpdateInterestRateChange", prm, "A2ZACGMS"));

                if (result == 0)
                {
                    UpdatedMSG();

                    ClearRecords();

                    txtPartyCode.Focus();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        protected void ddlAccNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAccNo.SelectedIndex != 0)
            {
                Int64 accno = Converter.GetLong(ddlAccNo.SelectedValue);
                A2ZACCOUNTDTO getDTO = (A2ZACCOUNTDTO.GetInfoAccNo(accno));

                if (getDTO.AccNo > 0)
                {
                    txtAccNo.Text = Converter.GetString(getDTO.AccNo);
                    txtCurrentBalance.Text = Converter.GetString(String.Format("{0:0,0.00}", getDTO.AccBalance));
                    txtInterestRate.Text = Converter.GetString(String.Format("{0:0,0.000}", getDTO.AccIntRate));
                    lblCurrencyCode.Text = Converter.GetString(getDTO.AccCurrency);

                    //int code = Converter.GetInteger(lblCurrencyCode.Text);
                    //A2ZCURRENCYDTO get1DTO = (A2ZCURRENCYDTO.GetInformation(code));

                    //if (get1DTO.CurrencyCode > 0)
                    //{
                    //    txtCurrency.Text = Converter.GetString(get1DTO.CurrencyName);
                    //}

                }

            }
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

            btnUpdate.Enabled = true;
            btnCancel.Enabled = true;
            BtnExit.Enabled = true;

            DivMain.Attributes.CssStyle.Add("opacity", "300");


            if (txtNewIntRate.Text == txtReInput.Text)
            {
                txtReInput.Text = string.Empty;
                
            }
            else
            {
                txtReInput.Text = string.Empty;
                txtNewIntRate.Text = string.Empty;
                txtNewIntRate.Focus();
            }


        }

        protected void txtNewIntRate_TextChanged(object sender, EventArgs e)
        {
            lblReInput.Text = "Please Re-Input New Interest Rate";

           
            ReInputScreen();
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoanType.SelectedIndex != 0)
            {
                if (ddlLoanType.SelectedValue == "11")
                {
                    LoanAccDropdown(11);
                }
                if (ddlLoanType.SelectedValue == "12")
                {
                    LoanAccDropdown(12);
                }
            }
        }


    }
}
