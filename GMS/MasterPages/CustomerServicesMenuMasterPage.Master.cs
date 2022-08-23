﻿using System;
using System.Data;
using System.Web.UI.WebControls;
using ATOZWEBGMS.WebSessionStore;
using DataAccessLayer.DTO.CustomerServices;
using DataAccessLayer.Utility;
using DataAccessLayer.DTO.HouseKeeping;



namespace ATOZWEBGMS.MasterPages
{
    public partial class CustomerServicesMenuMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblUserName.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_NAME));
                    lblUserBranchNo.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_BRNO));
                    lblUserLabel.Text = DataAccessLayer.Utility.Converter.GetString(SessionStore.GetValue(Params.SYS_USER_LEVEL));

                    string UnitAddress1 = string.Empty;
                    string UnitAddress2 = string.Empty;
                    string UnitAddress3 = string.Empty;

                    string UnitNameB = string.Empty;
                    string UnitAddress1B = string.Empty;


                    var p = A2ZERPSYSPRMDTO.GetParameterValue();

                    lblUnitFlag.Text = Converter.GetString(p.PrmUnitFlag);

                    if (p.PrmUnitFlag == 1 || lblUserLabel.Text == "40")
                    {
                        lblUserBranchNo.Text = Converter.GetString(p.PrmUnitNo);
                        lblCompanyName.Text = p.PrmUnitName;
                        UnitAddress1 = p.PrmUnitAdd1;

                        UnitNameB = p.PrmUnitNameB;
                        UnitAddress1B = p.PrmUnitAdd1B;
                    }
                    else
                    {
                        A2ZERPBRANCHDTO getDTO = new A2ZERPBRANCHDTO();
                        int userbranch = Converter.GetInteger(lblUserBranchNo.Text);
                        getDTO = (A2ZERPBRANCHDTO.GetInformation(userbranch));

                        if (getDTO.BranchNo > 0)
                        {
                            lblCompanyName.Text = Converter.GetString(getDTO.BranchName);
                            UnitAddress1 = Converter.GetString(getDTO.BranchAdd1);

                            UnitNameB = Converter.GetString(getDTO.BranchNameB);
                            UnitAddress1B = Converter.GetString(getDTO.BranchAdd1B);
                        }
                    }

                    SessionStore.SaveToCustomStore(Params.BRNO, lblUserBranchNo.Text);
                    SessionStore.SaveToCustomStore(Params.COMPANY_NAME, lblCompanyName.Text);
                    SessionStore.SaveToCustomStore(Params.BRANCH_ADDRESS, UnitAddress1);
                    SessionStore.SaveToCustomStore(Params.SYS_UNIT_FLAG, lblUnitFlag.Text);

                    SessionStore.SaveToCustomStore(Params.COMPANY_NAME_B, UnitNameB);
                    SessionStore.SaveToCustomStore(Params.BRANCH_ADDRESS_B, UnitAddress1B);
                   
                    GetMenuData();

                    A2ZCSPARAMETERDTO dto = A2ZCSPARAMETERDTO.GetParameterValue();
                    lblProcessDate.Text = Converter.GetString(dto.ProcessDate.ToLongDateString());



                }
                else
                {
                    this.Page.MaintainScrollPositionOnPostBack = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetMenuData()
        {
            string strQuery = "SELECT MenuNo,MenuName,MenuParentNo,MenuUrl FROM A2ZERPMENU WHERE UserId = " + DataAccessLayer.Utility.Converter.GetSmallInteger(SessionStore.GetValue(Params.SYS_USER_ID));
            DataTable table = DataAccessLayer.BLL.CommonManager.Instance.GetDataTableByQuery(strQuery, "A2ZACGMS");


            if (table.Rows.Count == 0)
            {
                string notifyMsg = "?txtOne=" + lblUserName.Text + "&txtTwo=" + "You Don't Have Permission" +
                                                           "&txtThree=" + "Contact Your Super User" + "&PreviousMenu=ERP.aspx";
                Server.Transfer("Notify.aspx" + notifyMsg);
            }


            DataView view = new DataView(table);
            view.RowFilter = "MenuParentNo IS NULL";
            foreach (DataRowView row in view)
            {
                MenuItem menuItem = new MenuItem(row["MenuName"].ToString(), row["MenuNo"].ToString());
                menuItem.NavigateUrl = row["MenuUrl"].ToString();
                menuBar.Items.Add(menuItem);
                AddChildItems(table, menuItem);
            }
        }
        private void AddChildItems(DataTable table, MenuItem menuItem)
        {
            DataView viewItem = new DataView(table);
            viewItem.RowFilter = "MenuParentNo = " + menuItem.Value;
            foreach (DataRowView childView in viewItem)
            {
                MenuItem childItem = new MenuItem(childView["MenuName"].ToString(), childView["MenuNo"].ToString());
                childItem.NavigateUrl = childView["MenuUrl"].ToString();
                menuItem.ChildItems.Add(childItem);
                AddChildItems(table, childItem);
            }
        }
    }
}
