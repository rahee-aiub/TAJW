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
    public partial class ZakatSadkahDonation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                lblID.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_ID));

                string PFlag = (string)Session["ProgFlag"];
                CtrlProgFlag.Text = PFlag;

                if (CtrlProgFlag.Text != "1")
                {

                    A2ZCSPARAMETERDTO dto = A2ZCSPARAMETERDTO.GetParameterValue();
                    DateTime dt = Converter.GetDateTime(dto.ProcessDate);
                    string date = dt.ToString("dd/MM/yyyy");
                    CtrlProcDate.Text = date;
                    txtDate.Text = date;

                    LastVoucherNo.Text = Converter.GetString(dto.LastVoucherNo);
                    LastConfirmVchNo.Text = Converter.GetString(dto.LastConfirmVoucherNo);
                   
                }
                else
                {
                    string RtxtDate = (string)Session["StxtDate"];
                    

                    txtDate.Text = RtxtDate;
                  

                }

            }
        }

        protected void RemoveSession()
        {

            Session["ProgFlag"] = string.Empty;
            Session["StxtDate"] = string.Empty;
            
        }

        protected void BtnExit_Click(object sender, EventArgs e)
        {
            RemoveSession();
            Response.Redirect("ERPModule.aspx");
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {

                int result = Converter.GetInteger(CommonManager.Instance.GetScalarValueBySp("Sp_DonateTransaction", "A2ZACGMS"));

                if (result == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "window.alert('Donation Succesfully done')", true);
                    return;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        protected void UpdateDifferRepStat()
        {
            try
            {

                int lVch = Converter.GetInteger(LastVoucherNo.Text);

                int roweffect = A2ZCSPARAMETERDTO.UpdateConfirmVchNo(lVch);
                if (roweffect > 0)
                {

                }

                

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "scriptkey", "<script>alert('System Error.UpdateDifferRepStat Problem');</script>");
                //throw ex;
            }

        }

    
    }
}
