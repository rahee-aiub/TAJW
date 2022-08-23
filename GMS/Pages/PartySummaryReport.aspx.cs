using System;
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
    public partial class PartySummaryReport : System.Web.UI.Page
    {

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


                    string cflag = (string)Session["CFlag"];

                    CFlag.Text = cflag;

                    string Module = (string)Session["SModule"];

                    if (lblflag.Text == string.Empty)
                    {
                        lblModule.Text = Request.QueryString["a%b"];
                        MultiAccFlag.Text = "0";

                        chkAllGroup.Checked = true;
                        ddlGroup.Enabled = false;

                    }
                    else
                    {
                        lblModule.Text = Module;
                    }

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
                        
                        lblProcDate.Text = Converter.GetString(date);

                        CtrlOldAccNo.Text = Converter.GetString(dto.OldAccNoFlag);



                        FunctionName();



                        lblGroup.Visible = true;
                        ddlGroup.Visible = true;





                    }
                    else
                    {


                        string RchkAllGroup = (string)Session["SchkAllGroup"];
                       
                        string RddlGroup = (string)Session["SddlGroup"];

                                             
                        string Rtxtfdate = (string)Session["Stxtfdate"];
                      

                      


                        string Cflag = (string)Session["CFlag"];
                        CFlag.Text = Cflag;

                       
                       
                     

                        lblGroup.Visible = true;
                       
                        ddlGroup.Visible = true;


                        if (RchkAllGroup == "1")
                        {
                            chkAllGroup.Checked = true;
                           
                            ddlGroup.SelectedValue = "-Select-";
                           
                            ddlGroup.Enabled = false;
                        }
                        else
                        {
                            chkAllGroup.Checked = false;
                           
                            ddlGroup.SelectedValue = RddlGroup;
                                   
                        }



                        txtfdate.Text = Rtxtfdate;
                       

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

        
        protected void FunctionName()
        {

            lblStatementFunc.Text = "Party Summary Report";

        }


        protected void StoreRecordsSession()
        {
            Session["ProgFlag"] = "1";
 
            
            if (chkAllGroup.Checked == true)
            {
                Session["SchkAllGroup"] = "1";
            }
            else
            {
                Session["SchkAllGroup"] = "0";
            }

           
            Session["SddlGroup"] = ddlGroup.SelectedValue;

            Session["Stxtfdate"] = txtfdate.Text;
                       
            Session["SlblProcDate"] = lblProcDate.Text;



            Session["SModule"] = lblModule.Text;

            Session["flag"] = "1";



            Session["CFlag"] = CFlag.Text;

            
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlGroup.SelectedIndex == 0 && chkAllGroup.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Select Group');", true);
                    return;
                }

                StoreRecordsSession();

                var p = A2ZERPSYSPRMDTO.GetParameterValue();
                string comName = p.PrmUnitName;
                string comAddress1 = p.PrmUnitAdd1;

                SessionStore.SaveToCustomStore(Params.COMPANY_NAME, comName);
                SessionStore.SaveToCustomStore(Params.BRANCH_ADDRESS, comAddress1);

                DateTime fdate = DateTime.ParseExact(txtfdate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
               
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
               
                //  For Account Type and Name Desc. 

                int gcode = 0;

                if (chkAllGroup.Checked)
                {
                    SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.GROUPCODE, gcode);
                }
                else 
                {
                    gcode = Converter.GetInteger(ddlGroup.SelectedValue);
                    SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.GROUPCODE, gcode);
                }

               

                //============== End For Multi User Parameter =========================
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_DATABASE_NAME_KEY, "A2ZACGMS");

                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.NFLAG, 0);
                SessionStore.SaveToCustomStore(DataAccessLayer.Utility.Params.REPORT_FILE_NAME_KEY, "rptGmsPartySummaryReport");

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

      

            Session["SchkAllGroup"] = string.Empty;
            Session["SddlGroup"] = string.Empty;





            Session["Stxtfdate"] = string.Empty;
            



        }
        protected void BtnExit_Click(object sender, EventArgs e)
        {
            RemoveSession();
            Response.Redirect("ERPModule.aspx");
        }


        

       
        protected void chkAllGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllGroup.Checked)
            {
               
                ddlGroup.Enabled = false;   
                ddlGroup.SelectedIndex = 0;               
                
            }
            else
            {
                ddlGroup.Enabled = true;
            }
        }


              
    }

}
