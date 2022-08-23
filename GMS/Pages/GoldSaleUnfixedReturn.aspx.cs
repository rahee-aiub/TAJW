﻿using System;
using System.Web;
using System.Web.UI;
using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.DTO;
using DataAccessLayer.Utility;
using DataAccessLayer.BLL;
using DataAccessLayer.DTO.CustomerServices;
using System.Data;
using System.Web.UI.WebControls;


namespace ATOZWEBGMS.Pages
{
    public partial class GoldSaleUnFixedReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                divPost.Visible = false;
                divLastSaleInfo.Visible = false;

                lblID.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_ID));

                A2ZCSPARAMETERDTO dto = A2ZCSPARAMETERDTO.GetParameterValue();
                DateTime dt = Converter.GetDateTime(dto.ProcessDate);
                string date = dt.ToString("dd/MM/yyyy");
                lblProcesDate.Text = date;
                txtDueDate.Text = date;

                ddlLocation.SelectedValue = "1";
                ddlLocation.Enabled = false;

                CodeDropdown();
                PartyDropdown();
                CurrencyDropdown();

                ddlCurrency.SelectedIndex = 1;

                ConvertCurrencyDropdown();

                TruncateWF();

                CalculateAvgRate();



            }
        }

        protected void TruncateWF()
        {
            string depositQry = "DELETE dbo.WFA2ZITEMGOLD WHERE UserId='" + lblID.Text + "'";
            int rowEffect1 = Converter.GetInteger(DataAccessLayer.BLL.CommonManager.Instance.ExecuteNonQuery(depositQry, "A2ZACGMS"));
        }

        private void CalculateAvgRate()
        {
            int result1 = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_GenerateTodaysMetalAvgRate]", "A2ZACGMS"));

            int result2 = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_GenerateTodaysMakingAvgRate]", "A2ZACGMS"));

            int result3 = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_GenerateTodaysStoneMakingAvgRate]", "A2ZACGMS"));

            int result4 = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_GenerateTodaysCarryingAvgRate]", "A2ZACGMS"));

            int result5 = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_CalculateItemGoldBalance]", "A2ZACGMS"));
        }

        private void ImportAEDRate()
        {
            string qry = "SELECT ExchangeRate FROM A2ZCURRENCY WHERE CurrencyCode = " + ddlCurrency.SelectedValue + "";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            int totrec = dt.Rows.Count;
            if (totrec > 0)
            {
                lblAEDRate.Text = Converter.GetString(dt.Rows[0]["ExchangeRate"]);
            }


        }
        private void PartyDropdown()
        {
            string sqlquery = "SELECT PartyCode,PartyName from A2ZPARTYCODE WHERE GroupCode != 11 and GroupCode != 12 and GroupCode !=16 and GroupCode !=51 and GroupCode !=21 GROUP BY PartyCode,PartyName";
            ddlPartyName = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlPartyName, "A2ZACGMS");
        }



        private void CodeDropdown()
        {
            string sqlquery = "SELECT GroupCode,CONCAT(GroupName,'  (',MakingRangeFrom, '  to ',MakingRangeTo,')') AS GroupName FROM A2ZITEMGROUP";
            ddlCode = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlCode, "A2ZACGMS");
        }

        private void ConvertCurrencyDropdown()
        {
            string sqlquery = "SELECT CurrencyCode,CurrencyName from A2ZCURRENCY WHERE CurrencyCode != 99";
            ddlConvCurrency = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlConvCurrency, "A2ZACGMS");

        }
        private void CurrencyDropdown()
        {
            string sqlquery = "SELECT CurrencyCode,CurrencyName from A2ZCURRENCY WHERE CurrencyCode > 1 AND CurrencyCode != 99";
            ddlCurrency = DataAccessLayer.BLL.CommonManager.Instance.FillDropDownList(sqlquery, ddlCurrency, "A2ZACGMS");

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
            if (lblTotalValue.Text == string.Empty || Converter.GetDecimal(lblTotalValue.Text) < 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Insert At least One item');", true);
                return;
            }

            txtTotalGross.Text = lblGrossWt.Text;
            txtTotalNet.Text = lblNetWt.Text;

            txtMaking.Text = lblMaking.Text;
            txtStone.Text = lblStoneValue.Text;
            txtTotalValueView.Text = lblTotalValue.Text;
            txtDiscount.Text = "0.00";




            ddlConvCurrency.SelectedValue = ddlCurrency.SelectedValue;
            ConvertCalculate(1);
            CalculateConvertTotal();

            if (ddlCurrency.SelectedValue == "2")
            {
                txtFinalCarringRate.ReadOnly = true;
                txtConvCarrying.ReadOnly = true;

                txtFinalCarringRate.Text = "0.00";
                txtConvCarrying.Text = "0.00";
            }
            txtConvRate.ReadOnly = true;

            divPost.Visible = true;

            divPost.Style.Add("Top", "170px");
            divPost.Style.Add("left", "350px");
            divPost.Style.Add("position", "fixed");

            divMain.Attributes.CssStyle.Add("opacity", "0.5");

            divPost.Attributes.CssStyle.Add("opacity", "400");
            divPost.Attributes.CssStyle.Add("z-index", "400");

        }

        private void gvItemDetailsInfo()
        {
            EmptyTotalInfo();
            string sqlquery = "(SELECT Id,ItemGroupName,Karat,ItemName,Purity,GrossWt,StoneWt,NetWt,PureWt,MakingRate,StoneMakingRate,CarringRate,MakingValue,StoneMakingValue,TotalMetalValue,CarringValue,TotalValue,StockPureWt,StockMakingValue,StockStoneMakingValue,StockCarringValue FROM WFA2ZITEMGOLD WHERE UserId = " + lblID.Text + ")";
            gvItemDetails = DataAccessLayer.BLL.CommonManager.Instance.FillGridViewList(sqlquery, gvItemDetails, "A2ZACGMS");
        }

        private void EmptyTotalInfo()
        {
            lblGrossWt.Text = string.Empty;
            lblStoneWt.Text = string.Empty;
            lblNetWt.Text = string.Empty;
            lblPureWt.Text = string.Empty;
            lblMaking.Text = string.Empty;
            lblStoneValue.Text = string.Empty;
            lblCarryingValue.Text = string.Empty;
            lblTotalValue.Text = string.Empty;
        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            if (ddlKarat.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Karat');", true);
                return;
            }
            try
            {
                txtPartyCode.ReadOnly = true;
                ddlPartyName.Enabled = false;

                var prm = new object[39];
                prm[0] = Converter.GetDateToYYYYMMDD(lblProcesDate.Text);
                prm[1] = ddlCode.SelectedValue;
                prm[2] = ddlCode.SelectedItem.Text.Substring(0, 4);
                prm[3] = ddlCurrency.SelectedValue;
                prm[4] = ddlKarat.SelectedValue;
                prm[5] = txtItemName.Text;
                prm[6] = txtPurity.Text;
                prm[7] = txtGrossWt.Text;
                prm[8] = txtStoneWt.Text;
                prm[9] = txtNetWt.Text;
                prm[10] = txtPureWt.Text;
                prm[11] = txtMakingRate.Text;
                prm[12] = txtStoneMakingRate.Text;
                prm[13] = txtMakingValue.Text;
                prm[14] = txtStoneMakingValue.Text;
                prm[15] = "0"; // TotalMetalValue.Text;
                prm[16] = txtTotalValue.Text;
                prm[17] = "24"; //Record Type
                prm[18] = "UnFixed Sale Return"; //Sale Type
                prm[19] = "2"; //FuxedUnfixed
                prm[20] = ddlLocation.SelectedValue;
                prm[21] = ddlLocation.SelectedItem.Text;
                prm[22] = ddlPartyName.SelectedValue;
                prm[23] = lblPartyAccno.Text;
                prm[24] = txtRefName.Text;
                prm[25] = Converter.GetDateToYYYYMMDD(txtDueDate.Text);
                prm[26] = "0";
                prm[27] = txtCarryingRate.Text; //Carrying Rate
                prm[28] = txtCarryingValue.Text; //Carrying Value
                prm[29] = "0";
                prm[30] = lblID.Text;
                prm[31] = lblStockPurity.Text;
                prm[32] = lblStockPureWt.Text;
                prm[33] = lblStockMakingRate.Text;
                prm[34] = lblStockMakingValue.Text;
                prm[35] = lblStockStoneMakingRate.Text;
                prm[36] = lblStockStoneMakingValue.Text;
                prm[37] = lblStockCarringRate.Text;
                prm[38] = lblStockCarringValue.Text;



                int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_InsertWfSaleItemGoldReturn]", prm, "A2ZACGMS"));


                if (result == 0)
                {
                    ClearInfo();
                    gvItemDetailsInfo();
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }



        protected void gvItemDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Label IdNo = (Label)gvItemDetails.Rows[e.RowIndex].Cells[0].FindControl("lblId");
                int Id = Converter.GetInteger(IdNo.Text);

                string sqlQuery = string.Empty;
                int rowEffect;
                sqlQuery = @"DELETE  FROM WFA2ZITEMGOLD WHERE  Id = '" + Id + "'";
                rowEffect = Converter.GetInteger(DataAccessLayer.BLL.CommonManager.Instance.ExecuteNonQuery(sqlQuery, "A2ZACGMS"));
                gvItemDetailsInfo();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializedInfo()
        {

            ddlKarat.SelectedIndex = 0;
            txtPurity.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtGrossWt.Text = string.Empty;
            txtStoneWt.Text = string.Empty;
            txtNetWt.Text = string.Empty;
            txtPureWt.Text = string.Empty;
            txtMakingRate.Text = string.Empty;
            txtStoneMakingRate.Text = string.Empty;
            txtMakingValue.Text = string.Empty;
            txtStoneMakingValue.Text = string.Empty;

            txtTotalValue.Text = string.Empty;

        }

        private void ClearInfo()
        {
            ddlCode.SelectedIndex = 0;
            ddlKarat.SelectedIndex = 0;
            txtPurity.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtGrossWt.Text = string.Empty;
            txtStoneWt.Text = string.Empty;
            txtNetWt.Text = string.Empty;
            txtPureWt.Text = string.Empty;
            txtMakingRate.Text = string.Empty;
            txtStoneMakingRate.Text = string.Empty;
            txtMakingValue.Text = string.Empty;
            txtStoneMakingValue.Text = string.Empty;

            txtTotalValue.Text = string.Empty;
            txtCarryingRate.Text = string.Empty;
            txtCarryingValue.Text = string.Empty;

        }
        private void CalculateNetWtPureWt()
        {

            if (txtStoneWt.Text == string.Empty)
            {
                txtStoneWt.Text = "0";
                txtStoneMakingRate.Text = "0";
                txtStoneMakingValue.Text = "0";
            }



            decimal NetWt = Converter.GetDecimal(txtGrossWt.Text) - Converter.GetDecimal(txtStoneWt.Text);
            decimal PureWt = NetWt * Converter.GetDecimal(txtPurity.Text);

            txtNetWt.Text = NetWt.ToString("0,0.00");
            txtPureWt.Text = PureWt.ToString("0,0.00");


        }

        private void CalculateMakingValue()
        {

            decimal MakingRate = Converter.GetDecimal(txtNetWt.Text) * Converter.GetDecimal(txtMakingRate.Text);
            txtMakingValue.Text = MakingRate.ToString("0,0.00");


        }

        private void CalculateStoneMakingValue()
        {
            if (txtStoneWt.Text != string.Empty && txtStoneMakingRate.Text != string.Empty)
            {
                decimal StoneValue = Converter.GetDecimal(txtStoneWt.Text) * Converter.GetDecimal(txtStoneMakingRate.Text);
                txtStoneMakingValue.Text = StoneValue.ToString("0,0.00");
            }

        }


        private void CalculateCarryingValue()
        {
            if (txtCarryingRate.Text != string.Empty && txtGrossWt.Text != string.Empty)
            {
                decimal GrossWt = Converter.GetDecimal(txtGrossWt.Text);
                decimal CarryingRate = Converter.GetDecimal(txtCarryingRate.Text);
                decimal CarryingValue = GrossWt * CarryingRate;

                txtCarryingValue.Text = CarryingValue.ToString("0,0.00");
            }

        }
        private void CalculateTotalValue()
        {

            decimal MakingValue = 0;
            decimal StoneMakingValue = 0;

            if (txtMakingValue.Text != string.Empty)
            {
                MakingValue = Converter.GetDecimal(txtMakingValue.Text);
            }
            if (txtStoneMakingValue.Text != string.Empty)
            {
                StoneMakingValue = Converter.GetDecimal(txtStoneMakingValue.Text);
            }


            decimal TotalValue = StoneMakingValue + MakingValue;
            txtTotalValue.Text = TotalValue.ToString("0,0.00");


        }

        private void CalculateStockValue()
        {
            lblStockPureWt.Text = (Converter.GetDecimal(txtNetWt.Text) * Converter.GetDecimal(lblStockPurity.Text)).ToString("0,0.00");
            lblStockMakingValue.Text = (Converter.GetDecimal(txtNetWt.Text) * Converter.GetDecimal(lblStockMakingRate.Text)).ToString("0,0.00");
            lblStockStoneMakingValue.Text = (Converter.GetDecimal(txtStoneWt.Text) * Converter.GetDecimal(lblStockStoneMakingRate.Text)).ToString("0,0.00");
            lblStockCarringValue.Text = (Converter.GetDecimal(txtGrossWt.Text) * Converter.GetDecimal(lblStockCarringRate.Text)).ToString("0,0.00"); ;
        }


        private void ValidCheckStoneWt()
        {
            lblMsgFlag.Text = "0";

            decimal grosswt = 0;
            decimal stonewt = 0;
            decimal netwt = 0;

            grosswt = Converter.GetDecimal(txtGrossWt.Text);
            stonewt = Converter.GetDecimal(txtStoneWt.Text);
            netwt = (grosswt - stonewt);

            if (netwt <= 0)
            {
                lblMsgFlag.Text = "1";
            }
        }

        protected void txtStoneWt_TextChanged(object sender, EventArgs e)
        {
            ValidCheckStoneWt();
            if (lblMsgFlag.Text == "1")
            {
                txtStoneWt.Text = string.Empty;
                txtStoneWt.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Input Excess Stone Weight');", true);
                return;
            }


            CalculateNetWtPureWt();

            CalculateMakingValue();
            CalculateCarryingValue();
            CalculateStoneMakingValue();
            txtMakingRate.Focus();
            CalculateStockValue();
            CalculateTotalValue();
        }

        protected void txtGrossWt_TextChanged(object sender, EventArgs e)
        {
            CalculateNetWtPureWt();

            CalculateMakingValue();
            CalculateCarryingValue();
            CalculateStoneMakingValue();
            txtStoneWt.Focus();
            CalculateStockValue();
            CalculateTotalValue();
        }

        protected void txtMakingRate_TextChanged(object sender, EventArgs e)
        {
            Decimal StockMakingRate = Converter.GetDecimal(lblStockMakingRate.Text);
            Decimal InputMakingRate = Converter.GetDecimal(txtMakingRate.Text);
            if (StockMakingRate > InputMakingRate)
            {
                txtMakingRate.Text = lblStockMakingRate.Text;
                txtMakingRate.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Making Rate');", true);
                return;
            }
            else
            {
                CalculateMakingValue();
                CalculateTotalValue();
                txtStoneMakingRate.Focus();
            }
        }

        protected void txtStoneMaking_TextChanged(object sender, EventArgs e)
        {
            Decimal StockStoneMakingRate = Converter.GetDecimal(lblStockStoneMakingRate.Text);
            Decimal InputStoneMakingRate = Converter.GetDecimal(txtStoneMakingRate.Text);
            if (StockStoneMakingRate > InputStoneMakingRate)
            {
                txtStoneMakingRate.Text = lblStockStoneMakingRate.Text;
                txtStoneMakingRate.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Stone Making Rate');", true);
                return;
            }
            else
            {
                CalculateStoneMakingValue();
                CalculateTotalValue();
                txtCarryingRate.Focus();
            }
        }

        protected void txtCarryingRate_TextChanged(object sender, EventArgs e)
        {
            Decimal StockCarryingRate = Converter.GetDecimal(lblStockCarringRate.Text);
            Decimal InputCarryingRate = Converter.GetDecimal(txtCarryingRate.Text);
            if (StockCarryingRate > InputCarryingRate)
            {
                txtCarryingRate.Text = lblStockCarringRate.Text;
                txtCarryingRate.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Carrying Rate');", true);
                return;
            }
            else
            {
                CalculateCarryingValue();
            }
        }

        protected void gvItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Decimal GrossWt = 0;
            Decimal StoneWt = 0;
            Decimal NetWt = 0;
            Decimal PureWt = 0;
            Decimal MValue = 0;
            Decimal SValue = 0;

            Decimal TotalValue = 0;
            Decimal Carrying = 0;




            for (int i = 0; i < gvItemDetails.Rows.Count; ++i)
            {
                String Gw = gvItemDetails.Rows[i].Cells[5].Text.ToString();
                String Sw = gvItemDetails.Rows[i].Cells[6].Text.ToString();
                String Nw = gvItemDetails.Rows[i].Cells[7].Text.ToString();
                String Pw = gvItemDetails.Rows[i].Cells[8].Text.ToString();
                String Mv = gvItemDetails.Rows[i].Cells[12].Text.ToString();
                String Sv = gvItemDetails.Rows[i].Cells[13].Text.ToString();

                String Cv = gvItemDetails.Rows[i].Cells[14].Text.ToString();
                String Tv = gvItemDetails.Rows[i].Cells[15].Text.ToString();



                GrossWt += Converter.GetDecimal(Gw);
                StoneWt += Converter.GetDecimal(Sw);
                NetWt += Converter.GetDecimal(Nw);
                PureWt += Converter.GetDecimal(Pw);
                MValue += Converter.GetDecimal(Mv);
                SValue += Converter.GetDecimal(Sv);
                Carrying += Converter.GetDecimal(Cv);
                TotalValue += Converter.GetDecimal(Tv);



                lblGrossWt.Text = Converter.GetString(GrossWt.ToString(("0,0.00")));
                lblStoneWt.Text = Converter.GetString(StoneWt.ToString(("0,0.00")));
                lblNetWt.Text = Converter.GetString(NetWt.ToString(("0,0.00")));
                lblPureWt.Text = Converter.GetString(PureWt.ToString(("0,0.00")));
                lblMaking.Text = Converter.GetString(MValue.ToString(("0,0.00")));
                lblStoneValue.Text = Converter.GetString(SValue.ToString(("0,0.00")));
                lblCarryingValue.Text = Converter.GetString(Carrying.ToString(("0,0.00")));
                lblTotalValue.Text = Converter.GetString(TotalValue.ToString(("0,0.00")));


            }


        }

        protected void ddlKarat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string qry = "SELECT ItemGroupCode  FROM WFA2ZITEMGOLD WHERE ItemGroupCode = " + ddlCode.SelectedValue + " AND Karat = " + ddlKarat.SelectedValue + " AND UserId=" + lblID.Text + "";
            //DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            //if (dt.Rows.Count > 0)
            //{
            //    ddlKarat.SelectedIndex = 0;
            //    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Item Already Added');", true);
            //    return;
            //}


            MakingAvgRate();
            StoneMakingAvgRate();
            CarryingAvgRate();


            LoadPurityDropdown(Converter.GetInteger(ddlKarat.SelectedValue));
            LastTransactionInfo();
            txtItemName.Focus();
        }


        private void MakingAvgRate()
        {
            string qry = "SELECT RecCode,AvgRate FROM A2ZPURCHASE WHERE Location = " + ddlLocation.SelectedValue + " AND RecCode = 2  AND ItemGroupCode = " + ddlCode.SelectedValue + " AND Karat = " + ddlKarat.SelectedValue + "";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            int totrec = dt.Rows.Count;
            if (totrec > 0)
            {
                lblStockMakingRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["AvgRate"]));
                txtMakingRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["AvgRate"]));
            }
        }


        private void StoneMakingAvgRate()
        {
            string qry = "SELECT RecCode,AvgRate FROM A2ZPURCHASE WHERE Location = " + ddlLocation.SelectedValue + " AND RecCode = 3 AND ItemGroupCode = " + ddlCode.SelectedValue + " AND Karat = " + ddlKarat.SelectedValue + "";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            int totrec = dt.Rows.Count;
            if (totrec > 0)
            {
                lblStockStoneMakingRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["AvgRate"]));
                txtStoneMakingRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["AvgRate"]));
            }
        }

        private void CarryingAvgRate()
        {
            string qry = "SELECT RecCode,AvgRate FROM A2ZPURCHASE WHERE RecCode = 4";
            DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
            int totrec = dt.Rows.Count;
            if (totrec > 0)
            {
                lblStockCarringRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["AvgRate"]));
                txtCarryingRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["AvgRate"]));
                //txtFinalCarringRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["AvgRate"]));
            }
        }

        private void LoadPurityDropdown(int karat)
        {
            if (karat == 22)
            {
                string qry = "SELECT MakingRangeFrom,MakingRangeTo,Purity22 FROM A2ZITEMGROUP WHERE GroupCode = " + ddlCode.SelectedValue + "";
                DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                if (dt.Rows.Count > 0)
                {
                    txtPurity.Text = Converter.GetString(dt.Rows[0]["Purity22"]);
                    string range = Converter.GetString(dt.Rows[0]["MakingRangeFrom"]) + " To " + Converter.GetString(dt.Rows[0]["MakingRangeTo"]);
                    txtItemName.Attributes.Add("placeholder", "" + range + "");
                }
            }
            if (karat == 21)
            {
                string qry = "SELECT MakingRangeFrom,MakingRangeTo,Purity21 FROM A2ZITEMGROUP WHERE GroupCode = " + ddlCode.SelectedValue + "";
                DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                if (dt.Rows.Count > 0)
                {
                    txtPurity.Text = Converter.GetString(dt.Rows[0]["Purity21"]);
                    string range = Converter.GetString(dt.Rows[0]["MakingRangeFrom"]) + " To " + Converter.GetString(dt.Rows[0]["MakingRangeTo"]);
                    txtItemName.Attributes.Add("placeholder", "" + range + "");
                }
            }
            if (karat == 18)
            {
                string qry = "SELECT MakingRangeFrom,MakingRangeTo,Purity18 FROM A2ZITEMGROUP WHERE GroupCode = " + ddlCode.SelectedValue + "";
                DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                if (dt.Rows.Count > 0)
                {
                    txtPurity.Text = Converter.GetString(dt.Rows[0]["Purity18"]);
                    string range = Converter.GetString(dt.Rows[0]["MakingRangeFrom"]) + " To " + Converter.GetString(dt.Rows[0]["MakingRangeTo"]);
                    txtItemName.Attributes.Add("placeholder", "" + range + "");
                }
            }

            lblStockPurity.Text = txtPurity.Text;
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
                    txtPartyAddress.Text = Converter.GetString(getDTO.PartyAddresssLine1) + " " + Converter.GetString(getDTO.PartyAddresssLine2) + " " + Converter.GetString(getDTO.PartyAddresssLine3);

                    ddlCurrency_SelectedIndexChanged(this, e);
                }
            }
        }

        protected void ddlCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializedInfo();





        }


        protected void txtItemName_TextChanged(object sender, EventArgs e)
        {
            txtPurity.Focus();
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCurrency.SelectedIndex != 0)
            {
                string qry = "SELECT AccType,AccNo,AccCurrency FROM A2ZACCOUNT where AccPartyNo='" + ddlPartyName.SelectedValue + "' AND AccCurrency = '" + ddlCurrency.SelectedValue + "' AND AccStatus = 1";
                DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                int totrec = dt.Rows.Count;
                if (totrec > 0)
                {
                    lblPartyAccType.Text = Converter.GetString(dt.Rows[0]["AccType"]);
                    lblPartyAccno.Text = Converter.GetString(dt.Rows[0]["AccNo"]);

                    ImportAEDRate();

                }
            }
        }

        protected void txtPurity_TextChanged(object sender, EventArgs e)
        {

            Decimal StockPurity = Converter.GetDecimal(lblStockPurity.Text);
            Decimal InputPurity = Converter.GetDecimal(txtPurity.Text);
            if (StockPurity > InputPurity || InputPurity > 1)
            {
                txtPurity.Text = lblStockPurity.Text;
                txtPurity.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid Purity');", true);
                return;
            }
            else
            {
                txtGrossWt.Focus();
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            A2ZVCHNOCTRLDTO getDTO = new A2ZVCHNOCTRLDTO();

            getDTO = (A2ZVCHNOCTRLDTO.GetLastDefaultVchNo());
            CtrlVoucherNo.Text = "FS" + getDTO.RecLastNo.ToString("000000");

            lblDescription.Text = "Gw:" + lblGrossWt.Text + ",St:" + lblStoneWt.Text + ",Nw:" + lblNetWt.Text + ",Pt:" + lblPureWt.Text + ",Sv:" + lblStoneValue.Text + ",Mc:" + lblMaking.Text;

            try
            {

                var prm = new object[17];

                prm[0] = ddlLocation.SelectedValue;
                prm[1] = ddlCurrency.SelectedValue;
                prm[2] = CtrlVoucherNo.Text;
                prm[3] = lblID.Text;
                prm[4] = txtPartyCode.Text;
                prm[5] = lblPartyAccType.Text;
                prm[6] = "94";
                prm[7] = "Un Fixed Sale Return";
                prm[8] = "1";
                prm[9] = "1";
                prm[10] = lblPureWt.Text;
                prm[11] = txtConvNetAmt.Text;
                prm[12] = lblDescription.Text;
                prm[13] = ddlConvCurrency.SelectedValue;
                prm[14] = txtConvRate.Text;
                prm[15] = txtFinalCarringRate.Text;
                prm[16] = txtConvDiscount.Text;


                int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("[Sp_InsertSaleItemGoldReturn]", prm, "A2ZACGMS"));


                if (result == 0)
                {
                    Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            divPost.Visible = false;
            divMain.Attributes.CssStyle.Add("opacity", "300");
        }



        protected void txtConvDiscount_TextChanged(object sender, EventArgs e)
        {
            txtConvNetAmt.Text = (Converter.GetDecimal(txtConvMaking.Text) + Converter.GetDecimal(txtConvStone.Text) + Converter.GetDecimal(txtConvCarrying.Text) - Converter.GetDecimal(txtConvDiscount.Text)).ToString("0,0.00");
        }



        protected void ddlConvCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlConvCurrency.SelectedValue == "2")
            {
                ConvertCalculate(1);
                txtConvRate.ReadOnly = true;
                txtFinalCarringRate.Focus();
                txtFinalCarringRate.Text = "0.00";
                txtConvCarrying.Text = "0.00";
                txtFinalCarringRate.ReadOnly = true;
                txtConvCarrying.ReadOnly = true;

            }
            else
            {
                ConvertCalculate(Converter.GetDecimal(lblAEDRate.Text));

                if (Converter.GetDecimal(txtConvRate.Text) != 1)
                {
                    txtConvCarrying.Text = (Converter.GetDecimal(lblCarryingValue.Text)).ToString("0,0.00");
                }

                txtConvRate.ReadOnly = false;
                txtConvRate.Focus();

                txtFinalCarringRate.ReadOnly = false;
                txtConvCarrying.ReadOnly = false;
                txtFinalCarringRate.Text = lblStockCarringRate.Text;
            }

            txtConvDiscount.Text = "00.00";
            txtDiscount.Text = "00.00";
            CalculateConvertTotal();
        }

        private void LastTransactionInfo()
        {
            if (ddlKarat.SelectedIndex != 0 && ddlCode.SelectedIndex != 0)
            {
                string qry = "SELECT TOP 1 ItemName,Purity,MakingRate,StoneMakingRate,CarringRate FROM A2ZITEMGOLD WHERE RecordType = 22 AND ItemGroupCode = " + ddlCode.SelectedValue + " AND Karat = " + ddlKarat.SelectedValue + " ORDER BY Id DESC";
                DataTable dt = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(qry, "A2ZACGMS");
                int totrec = dt.Rows.Count;
                if (totrec > 0)
                {
                    divLastSaleInfo.Visible = true;

                    lblLastSaleItemName.Text = Converter.GetString(dt.Rows[0]["ItemName"]);
                    lblLastSalePurity.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["Purity"]));
                    lblLastSaleMakingRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["MakingRate"]));
                    lblLastSaleStoneMakingRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["StoneMakingRate"]));
                    lblLastSaleCarringRate.Text = Converter.GetString(String.Format("{0:0,0.00}", dt.Rows[0]["CarringRate"]));
                }
                else
                {
                    divLastSaleInfo.Visible = false;

                    lblLastSaleItemName.Text = string.Empty;
                    lblLastSalePurity.Text = string.Empty;
                    lblLastSaleMakingRate.Text = string.Empty;
                    lblLastSaleStoneMakingRate.Text = string.Empty;
                    lblLastSaleCarringRate.Text = string.Empty;
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
                txtPartyAddress.Text = Converter.GetString(getDTO.PartyAddresssLine1) + " " + Converter.GetString(getDTO.PartyAddresssLine2) + " " + Converter.GetString(getDTO.PartyAddresssLine3);

                ddlCurrency_SelectedIndexChanged(this, e);
            }
            else
            {
                ddlPartyName.SelectedIndex = 0;
                txtPartyCode.Text = string.Empty;
                txtPartyCode.Focus();
            }
        }

        protected void txtConvRate_TextChanged(object sender, EventArgs e)
        {
            ConvertCalculate(Converter.GetDecimal(txtConvRate.Text));
            txtConvDiscount.Text = "00.00";
            txtDiscount.Text = "00.00";
            CalculateConvertTotal();
            txtFinalCarringRate.Focus();
        }

        private void CalculateConvertTotal()
        {

            if (Converter.GetDecimal(txtConvRate.Text) != 1)
            {
                txtConvNetAmt.Text = (Converter.GetDecimal(txtConvMaking.Text) + Converter.GetDecimal(txtConvStone.Text) + Converter.GetDecimal(txtConvCarrying.Text) - Converter.GetDecimal(txtConvDiscount.Text)).ToString("0,0.00");

            }
            else
            {
                txtConvNetAmt.Text = (Converter.GetDecimal(txtConvMaking.Text) + Converter.GetDecimal(txtConvStone.Text) - Converter.GetDecimal(txtConvDiscount.Text)).ToString("0,0.00");
            }
        }

        private void ConvertCalculate(Decimal ConvRate)
        {
            txtConvRate.Text = ConvRate.ToString();
            txtConvMaking.Text = (ConvRate * Converter.GetDecimal(lblMaking.Text)).ToString("0,0.00");
            txtConvStone.Text = (ConvRate * Converter.GetDecimal(lblStoneValue.Text)).ToString("0,0.00");
            txtConvDiscount.Text = (ConvRate * Converter.GetDecimal(txtDiscount.Text)).ToString("0,0.00");




        }

        protected void txtFinalCarringRate_TextChanged(object sender, EventArgs e)
        {
            if (txtFinalCarringRate.Text != string.Empty)
            {
                txtConvCarrying.Text = (Converter.GetDecimal(txtFinalCarringRate.Text) * Converter.GetDecimal(txtTotalGross.Text)).ToString("0,0.00");
                CalculateConvertTotal();
            }
        }
    }
}
