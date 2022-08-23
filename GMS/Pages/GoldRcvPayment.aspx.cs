using System;
using System.Web;
using System.Web.UI;
using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.DTO;
using DataAccessLayer.Utility;
using DataAccessLayer.BLL;
using DataAccessLayer.DTO.CustomerServices;
using System.Data;
using System.Web.UI.WebControls;
using DataAccessLayer.DTO.HouseKeeping;
using System.Collections.Generic;


namespace ATOZWEBGMS.Pages
{
    public partial class GoldRcvPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblID.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_ID));

                A2ZCSPARAMETERDTO dto = A2ZCSPARAMETERDTO.GetParameterValue();
                DateTime dt = Converter.GetDateTime(dto.ProcessDate);
                string date = dt.ToString("dd/MM/yyyy");
                lblProcesDate.Text = date;

                PartyDropdown();

                PayPartyDropdown();

                //CurrencyDropdown();
                DivReInput.Visible = false;
            }
        }





        private void PartyDropdown()
        {
            string sqlquery = "SELECT PartyCode,PartyName from A2ZPARTYCODE WHERE GroupCode != 11 and GroupCode != 12 and GroupCode !=16 and GroupCode !=51 and GroupCode !=21 GROUP BY PartyCode,PartyName";
            ddlPartyName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlPartyName, "A2ZACGMS");
        }

        private void PayPartyDropdown()
        {
            string sqlquery = "SELECT PartyCode,PartyName from A2ZPARTYCODE WHERE GroupCode != 11 and GroupCode != 12 and GroupCode !=16 and GroupCode !=51 and GroupCode !=21 GROUP BY PartyCode,PartyName";
            ddlPayPartyName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlPayPartyName, "A2ZACGMS");
        }

        protected void BtnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ERPModule.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvItemDetails.Rows.Count == 0 || gvItemDetails == null)
                {
                    return;
                }

                A2ZVCHNOCTRLDTO getDTO = new A2ZVCHNOCTRLDTO();
                getDTO = (A2ZVCHNOCTRLDTO.GetLastDefaultVchNo());
                CtrlVoucherNo.Text = "GRP" + getDTO.RecLastNo.ToString("000000");

                foreach (GridViewRow gr in gvItemDetails.Rows)
                {
                    //int gvId = Convert.ToInt32(gr.Cells[0].Text);
                    string gvPartyCode = gr.Cells[1].Text;
                    string gvPartyName = gr.Cells[2].Text;
                    string gvPartyAccNo = gr.Cells[3].Text;
                    decimal gvPureWt = Convert.ToDecimal(gr.Cells[4].Text);




                    var prm = new object[14];


                    prm[0] = ddlLocation.SelectedValue;
                    prm[1] = ddlLocation.SelectedItem.Text;
                    prm[2] = CtrlVoucherNo.Text;
                    prm[3] = lblID.Text;
                    prm[4] = txtPartyCode.Text;
                    prm[5] = lblPartyAccType.Text;
                    prm[6] = lblPartyAccno.Text;

                    prm[7] = gvPureWt;
                    prm[8] = gvPartyCode;
                    prm[9] = lblPayPartyAccType.Text;
                    prm[10] = gvPartyAccNo;


                    if (txtPremurm.Text == string.Empty)
                    {
                        prm[11] = "0";
                    }
                    else
                    {
                        prm[11] = txtPremurm.Text;
                    }
                    prm[12] = txttrnDesc.Text;
                    prm[13] = Converter.GetDateToYYYYMMDD(lblProcesDate.Text);

                    int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_GoldReceivePayment]", prm, "A2ZACGMS"));
                    if(result != 0)
                    {
                        throw new Exception("Transaction Not Done ");
                    }
                }


                Response.Redirect(Request.RawUrl);

            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }


        private void PrintVoucher()
        {
            var p = A2ZERPSYSPRMDTO.GetParameterValue();
            string comName = p.PrmUnitName;
            string comAddress1 = p.PrmUnitAdd1;
            SessionStore.SaveToCustomStore(Params.COMPANY_NAME, comName);
            SessionStore.SaveToCustomStore(Params.BRANCH_ADDRESS, comAddress1);


            SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_NAME1, CtrlVoucherNo.Text);
            // SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.COMMON_TDATE, Converter.GetDateToYYYYMMDD(txtToDate.Text));

            //SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.NFLAG, 0);

            SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_DATABASE_NAME_KEY, "A2ZACGMS");


            SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_FILE_NAME_KEY, "rptGmsFixedPurchaseInvoice");


            Response.Redirect("ReportServer.aspx", false);
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
                    //txtPartyAddress.Text = Converter.GetString(getDTO.PartyAddresssLine1) + " " + Converter.GetString(getDTO.PartyAddresssLine2) + " " + Converter.GetString(getDTO.PartyAddresssLine3);

                    string qry = "SELECT AccType,AccNo,AccCurrency FROM A2ZACCOUNT where AccPartyNo='" + ddlPartyName.SelectedValue + "' AND AccCurrency = '99' AND AccStatus = 1";
                    DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                    int totrec = dt.Rows.Count;
                    if (totrec > 0)
                    {
                        lblPartyAccType.Text = Converter.GetString(dt.Rows[0]["AccType"]);
                        lblPartyAccno.Text = Converter.GetString(dt.Rows[0]["AccNo"]);
                    }

                }
            }
        }

        protected void txtPartyCode_TextChanged(object sender, EventArgs e)
        {
            int PartyCode = Converter.GetInteger(txtPartyCode.Text);
            A2ZPARTYDTO getDTO = (A2ZPARTYDTO.GetPartyInformation(PartyCode));

            if (getDTO.PartyName != string.Empty)
            {
                ddlPartyName.SelectedValue = Converter.GetString(getDTO.PartyCode);
                //txtPartyAddress.Text = Converter.GetString(getDTO.PartyAddresssLine1) + " " + Converter.GetString(getDTO.PartyAddresssLine2) + " " + Converter.GetString(getDTO.PartyAddresssLine3);

                string qry = "SELECT AccType,AccNo,AccCurrency FROM A2ZACCOUNT where AccPartyNo='" + ddlPartyName.SelectedValue + "' AND AccCurrency = '99' AND AccStatus = 1";
                DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                int totrec = dt.Rows.Count;
                if (totrec > 0)
                {
                    lblPartyAccType.Text = Converter.GetString(dt.Rows[0]["AccType"]);
                    lblPartyAccno.Text = Converter.GetString(dt.Rows[0]["AccNo"]);
                }

            }
            else
            {
                ddlPartyName.SelectedIndex = 0;
                txtPartyCode.Text = string.Empty;
                txtPartyCode.Focus();
            }
        }
        private void ReInputScreen()
        {
            DivReInput.Visible = true;

            BtnSubmit.Enabled = false;
            btnCancel.Enabled = false;
            BtnExit.Enabled = false;

            DivReInput.Style.Add("Top", "250px");
            DivReInput.Style.Add("left", "530px");
            DivReInput.Style.Add("position", "fixed");

            DivMain.Attributes.CssStyle.Add("opacity", "0.3");

            DivReInput.Attributes.CssStyle.Add("opacity", "300");
            DivReInput.Attributes.CssStyle.Add("z-index", "300");

            txtReInput.Text = string.Empty;
            txtReInput.Focus();
        }

        protected void txtGoldPureWt_TextChanged(object sender, EventArgs e)
        {
            lblReInput.Text = "Please Re-Input Gold Pure Wt";

            ReInputScreen();
        }

        protected void txtReInput_TextChanged(object sender, EventArgs e)
        {
            DivReInput.Visible = false;
            BtnSubmit.Enabled = true;
            btnCancel.Enabled = true;
            BtnExit.Enabled = true;

            DivMain.Attributes.CssStyle.Add("opacity", "300");


            if (txtGoldPureWt.Text == txtReInput.Text)
            {
                txtReInput.Text = string.Empty;
                txtPremurm.Focus();
            }
            else
            {
                txtReInput.Text = string.Empty;
                txtGoldPureWt.Text = string.Empty;
                txtGoldPureWt.Focus();
            }
        }

        protected void txtPayPartyCode_TextChanged(object sender, EventArgs e)
        {
            int PartyCode = Converter.GetInteger(txtPayPartyCode.Text);
            A2ZPARTYDTO getDTO = (A2ZPARTYDTO.GetPartyInformation(PartyCode));

            if (getDTO.PartyName != string.Empty)
            {
                ddlPayPartyName.SelectedValue = Converter.GetString(getDTO.PartyCode);
                //txtPayPartyAddress.Text = Converter.GetString(getDTO.PartyAddresssLine1) + " " + Converter.GetString(getDTO.PartyAddresssLine2) + " " + Converter.GetString(getDTO.PartyAddresssLine3);

                string qry = "SELECT AccType,AccNo,AccCurrency FROM A2ZACCOUNT where AccPartyNo='" + ddlPayPartyName.SelectedValue + "' AND AccCurrency = '99' AND AccStatus = 1";
                DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                int totrec = dt.Rows.Count;
                if (totrec > 0)
                {
                    lblPayPartyAccType.Text = Converter.GetString(dt.Rows[0]["AccType"]);
                    lblPayPartyAccno.Text = Converter.GetString(dt.Rows[0]["AccNo"]);
                }

            }
            else
            {
                ddlPayPartyName.SelectedIndex = 0;
                txtPayPartyCode.Text = string.Empty;
                txtPayPartyCode.Focus();
            }
        }

        protected void ddlPayPartyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPayPartyName.SelectedIndex != 0)
            {
                int PartyCode = Converter.GetInteger(ddlPayPartyName.SelectedValue);
                A2ZPARTYDTO getDTO = (A2ZPARTYDTO.GetPartyInformation(PartyCode));

                if (getDTO.PartyName != string.Empty)
                {
                    txtPayPartyCode.Text = Converter.GetString(getDTO.PartyCode);
                    //txtPayPartyAddress.Text = Converter.GetString(getDTO.PartyAddresssLine1) + " " + Converter.GetString(getDTO.PartyAddresssLine2) + " " + Converter.GetString(getDTO.PartyAddresssLine3);

                    string qry = "SELECT AccType,AccNo,AccCurrency FROM A2ZACCOUNT where AccPartyNo='" + ddlPayPartyName.SelectedValue + "' AND AccCurrency = '99' AND AccStatus = 1";
                    DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                    int totrec = dt.Rows.Count;
                    if (totrec > 0)
                    {
                        lblPayPartyAccType.Text = Converter.GetString(dt.Rows[0]["AccType"]);
                        lblPayPartyAccno.Text = Converter.GetString(dt.Rows[0]["AccNo"]);
                    }

                }
            }
        }
        protected void gvItemDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Label IdNo = (Label)gvItemDetails.Rows[e.RowIndex].Cells[0].FindControl("lblId");
                int Id = Converter.GetInteger(IdNo.Text);

                //string sqlQuery = string.Empty;
                //int rowEffect;
                //sqlQuery = @"DELETE  FROM WFA2ZITEMGOLD WHERE  Id = '" + Id + "'";
                //rowEffect = Converter.GetInteger(DataAccessLayer.BLL.CommonManager.Instance.ExecuteNonQuery(sqlQuery, "A2ZACGMS"));
                //gvItemDetailsInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }




        protected void btnAdd_Click(object sender, EventArgs e)
        {
            List<partyDetails> lst = new List<partyDetails>();

            if (gvItemDetails.Rows.Count > 0)
            {
                foreach (GridViewRow gr in gvItemDetails.Rows)
                {
                    //int gvId = Convert.ToInt32(gr.Cells[0].Text);
                    string gvPartyCode = gr.Cells[1].Text;
                    string gvPartyName = gr.Cells[2].Text;
                    string gvPartyAccNo = gr.Cells[3].Text;
                    decimal gvPureWt = Convert.ToDecimal(gr.Cells[4].Text);

                    lst.Add(new partyDetails { PartyCode = gvPartyCode, PartyName = gvPartyName, PureWt = gvPureWt, PartyAccNo = gvPartyAccNo });
                }
            }


            partyDetails pd = new partyDetails();
            pd.Id = (lst.Count == null ? 0 : lst.Count) + 1;
            pd.PartyCode = txtPayPartyCode.Text;
            pd.PartyName = ddlPayPartyName.SelectedItem.Text;
            pd.PartyAccNo = lblPayPartyAccno.Text;
            pd.PureWt = Convert.ToDecimal(txtGoldPureWt.Text);
            lst.Add(pd);

            gvItemDetails.DataSource = lst;
            gvItemDetails.DataBind();
        }

    }
}
public class partyDetails
{
    public int Id { get; set; }
    public string PartyCode { get; set; }
    public string PartyName { get; set; }
    public string PartyAccNo { get; set; }
    public decimal PureWt { get; set; }
}