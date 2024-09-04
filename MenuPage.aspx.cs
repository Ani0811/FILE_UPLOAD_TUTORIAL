using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using SQLHelperWeb;

namespace FileUploadWeb
{
    public partial class MenuPage : System.Web.UI.Page
    {
        SQLHelperWeb.ISQLHelperWeb objSQLHelperWeb = new SQLHelperWeb.SQLHelperWeb();
        StringBuilder sb = null;
        DataTable oDt = null;
        protected void Page_Load(object sender, EventArgs e) 
        {
            //GetMenu();
            //GetTreeView();
            //GetTreeView_D();
            BuildMenu();
            BuildTreeView();
        }
        private void GetMenu_1()
        {
            Menu oMenu = new Menu(); oMenu.ID = "nMenu";
            oMenu.Orientation = Orientation.Horizontal;

            MenuItem homeItem = new MenuItem("Home");
            homeItem.ChildItems.Add(new MenuItem("About"));

            MenuItem helpItem = new MenuItem("Help");
            helpItem.ChildItems.Add(new MenuItem("Update"));
            homeItem.ChildItems.Add(helpItem);

            oMenu.Items.Add(homeItem);

            MenuItem manageFilesItem = new MenuItem("Manage Files");
            manageFilesItem.ChildItems.Add(new MenuItem("Upload File", "", "", "~/FileUpload.aspx"));
            manageFilesItem.ChildItems.Add(new MenuItem("Retrieve File", "", "", "~/FileRetrieve.aspx"));
            oMenu.Items.Add(manageFilesItem);

            MenuItem viewDataItem = new MenuItem("View Data");
            viewDataItem.ChildItems.Add(new MenuItem("Load Data"));
            viewDataItem.ChildItems.Add(new MenuItem("Grid View", "", "", "~/GridViewData.aspx"));
            oMenu.Items.Add(viewDataItem);
            

            oMenu.CssClass = "MenuBackground";
            oMenu.DynamicMenuStyle.CssClass = "MenuLevel";
            oMenu.DynamicMenuItemStyle.CssClass = "textarea";
            oMenu.DynamicHoverStyle.BackColor = Color.Beige;

            Control Con = Page.FindControl("MenuControl");
            Con.Controls.Add(oMenu);
        }
        private void GetMenu()
        {
            DataSet oDs = null;
            DataTable oDt_Top = null;
            DataTable oDt_Lv1 = null;

            string s_Menu_P = null;
            string s_Menu_L = null;
            int i_MenuIndex = 0;

            try 
            {
                Menu oMenu = new Menu(); oMenu.ID = "nMenu";
                oMenu.Orientation = Orientation.Horizontal;

                oDs = objSQLHelperWeb.GetData("SP_USERMENU");
                if(oDs != null && oDs.Tables[0].Rows.Count > 0)
                {
                    for (int iRows = 0; iRows < oDs.Tables.Count; iRows++)
                    {
                        oDt_Top = oDs.Tables[iRows];

                        foreach (DataRow Top_R in oDt_Top.Rows)
                        {
                            if (Top_R["MENU_PARENTID"].ToString() == string.Empty)
                            {
                                s_Menu_P = Top_R["MENU_ID"].ToString();
                                MenuItem TopMenu = new MenuItem(Top_R["MENU_DESC"].ToString());

                                oDt_Lv1 = oDs.Tables[iRows + 1]; // Level 1 menu
                                foreach (DataRow Lvl1_R in oDt_Lv1.Rows)
                                {
                                    if (Lvl1_R["MENU_PARENTID"].ToString() == s_Menu_P) // Level - 1
                                    {
                                        s_Menu_L = Lvl1_R["MENU_ID"].ToString();
                                        TopMenu.ChildItems.Add(new MenuItem(Lvl1_R["MENU_DESC"].ToString()));
                                    }
                                    else
                                    {
                                        if (Lvl1_R["MENU_PARENTID"].ToString() == s_Menu_L) // Level - 2
                                        {
                                            i_MenuIndex++;
                                            TopMenu.ChildItems[i_MenuIndex].ChildItems.Add(new MenuItem(Lvl1_R["MENU_DESC"].ToString()));
                                            i_MenuIndex = 0;
                                        }
                                    }
                                }
                                oDt_Lv1 = null; iRows++;
                                oMenu.Items.Add(TopMenu); TopMenu = null;
                            }
                        }
                    }
                    
                    oMenu.CssClass = "MenuBackground";
                    oMenu.DynamicMenuStyle.CssClass = "MenuLevel";
                    oMenu.DynamicMenuItemStyle.CssClass = "textarea";
                    oMenu.DynamicHoverStyle.BackColor = Color.Beige;

                    Control Con = Page.FindControl("MenuControl");
                    Con.Controls.Add(oMenu);
                }
            }
            catch(Exception ex) {; }
            finally { oDs = null; }
        }
        private void BuildMenu()
        {
            DataSet oDs = null;
            Menu oMenu = null;
            try 
            {
                oDs = objSQLHelperWeb.GetData("SP_USERMENU");

                if (oDs != null && oDs.Tables[0].Rows.Count > 0)
                {
                    oMenu = new Menu();
                    oMenu.Orientation = Orientation.Horizontal;
                    DataTable oDt_Menu = oDs.Tables[0];

                    oMenu.CssClass = "MenuBackground";
                    oMenu.DynamicMenuStyle.CssClass = "MenuLevel";
                    oMenu.DynamicMenuItemStyle.CssClass = "textarea";
                    oMenu.DynamicHoverStyle.BackColor = Color.Beige;

                    foreach (DataRow Top_R in oDs.Tables[0].Rows)
                    {
                        if (Top_R["MENU_PARENTID"].ToString() == string.Empty)
                        {
                            MenuItem topMenu = new MenuItem(Top_R["MENU_DESC"].ToString());
                            oMenu.Items.Add(topMenu);
                            GetMenuItems(topMenu.ChildItems, oDt_Menu, Top_R["MENU_ID"].ToString());
                        }
                    }
                    Control Con = Page.FindControl("MenuControl");
                    Con.Controls.Add(oMenu);
                }
            }
            catch(Exception ex) {; }
            finally { oDs = null; }
        }
        private void GetMenuItems(MenuItemCollection items, DataTable oDt, string parentID)
        {
            foreach (DataRow row in oDt.Rows)
            {
                if (row["MENU_PARENTID"].ToString().ToUpper().Trim() == parentID.ToUpper().Trim())
                {
                    MenuItem newMenuItem = new MenuItem(row["MENU_DESC"].ToString())
                    {
                        Value = row["MENU_ID"].ToString()
                    };
                    items.Add(newMenuItem);

                    GetMenuItems(newMenuItem.ChildItems, oDt, newMenuItem.Value);
                }
            }
        }
        private void GetTreeView()
        {
            TreeView tView = new TreeView();
            tView.HoverNodeStyle.Font.Bold = true;
            tView.LeafNodeStyle.BackColor = System.Drawing.Color.Pink;
            tView.RootNodeStyle.BackColor = System.Drawing.Color.Pink;

            TreeNode homeNode = new TreeNode("Home");
            homeNode.Target = "_blank";

            TreeNode employeeNode = new TreeNode("Employee");
            employeeNode.Target = "_blank";
            employeeNode.ChildNodes.Add(new TreeNode("Upload Resume", "", "", "", "_blank"));
            employeeNode.ChildNodes.Add(new TreeNode("Edit Resume", "", "", "", "_blank"));
            employeeNode.ChildNodes.Add(new TreeNode("View Resume", "", "", "", "_blank"));

            TreeNode employerNode = new TreeNode("Employer");
            employerNode.Target = "_blank";
            employerNode.ChildNodes.Add(new TreeNode("Upload Job", "", "", "", "_blank"));
            employerNode.ChildNodes.Add(new TreeNode("Edit Job", "", "", "", "_blank"));
            employerNode.ChildNodes.Add(new TreeNode("View Job", "", "", "", "_blank"));

            TreeNode adminNode = new TreeNode("Admin");
            adminNode.Target = "_blank";
            adminNode.ChildNodes.Add(new TreeNode("Add User", "", "", "", "_blank"));
            adminNode.ChildNodes.Add(new TreeNode("Edit User", "", "", "", "_blank"));
            adminNode.ChildNodes.Add(new TreeNode("View User", "", "", "", "_blank"));

            tView.Nodes.Add(homeNode);
            tView.Nodes.Add(employeeNode);
            tView.Nodes.Add(employerNode);
            tView.Nodes.Add(adminNode);

            TreeViewControl.Controls.Add(tView);
        }
        private void GetTreeView_D()
        {
            DataSet oDs = null;
            DataTable oDt = null;
            DataRow[] T_Nodes_Rows = null;

            string s_TreeView_Root = null;
            int iRows = 0;
            try
            {
                TreeView tView = new TreeView();
                TreeNode rootNode = null;

                oDs = objSQLHelperWeb.GetData("SP_USERTREEVIEW");
                if(oDs != null && oDs.Tables[0].Rows.Count > 0)
                {
                    DataTable oDt_TreeView = oDs.Tables[0];
                    rootNode = new TreeNode("Home");
                    //GetChildNodes();
                    /*foreach (DataRow o_Rows in oDs.Tables[0].Rows)
                    {
                        if (o_Rows["TREEVIEW_PARENTID"].ToString() == string.Empty)
                        {
                            s_TreeView_Root = o_Rows["TREEVIEW_ID"].ToString();
                            rootNode = new TreeNode(o_Rows["TREEVIEW_DESC"].ToString());
                        }
                        else
                        {
                            if ((o_Rows["TREEVIEW_PARENTID"].ToString() != string.Empty))
                            {
                                T_Nodes_Rows = oDs.Tables[0].Select("TREEVIEW_PARENTID = '" + s_TreeView_Root + "'");
                                if (T_Nodes_Rows != null)
                                {
                                    foreach (DataRow N_Row in T_Nodes_Rows)
                                    {
                                        rootNode.ChildNodes.Add(new TreeNode(N_Row["TREEVIEW_DESC"].ToString(), "", "", "", "_blank"));
                                    }
                                }
                            }
                        }
                    }*/
                    tView.Nodes.Add(rootNode); rootNode = null;
                }
                Control Con = Page.FindControl("TreeViewControl");
                Con.Controls.Add(tView);
            }
            catch (Exception ex){; }
            finally {oDs = null; }
        }
        private void BuildTreeView()
        {
            DataSet oDs = null;
            TreeView tView = null;
            TreeNode rootNode = null;
            try
            {
                //DataSet oDs = objSQLHelperWeb.GetData("SP_USERMENU");
                oDs = objSQLHelperWeb.GetData("SP_USERTREEVIEW");
                if (oDs != null && oDs.Tables[0].Rows.Count > 0)
                {
                    tView = new TreeView();
                    DataTable oDt_TreeView = oDs.Tables[0];

                    foreach (DataRow o_Rows in oDs.Tables[0].Rows)
                    {
                        if (o_Rows["TREEVIEW_PARENTID"].ToString() == string.Empty)
                        {
                            //s_TreeView_Root = o_Rows["TREEVIEW_ID"].ToString();
                            rootNode = new TreeNode(o_Rows["TREEVIEW_DESC"].ToString());
                            tView.Nodes.Add(rootNode);
                            tView.CollapseAll();
                            BuildTreeNodes(rootNode.ChildNodes, oDt_TreeView, o_Rows["TREEVIEW_ID"].ToString());
                        }
                    }
                    //tView.ExpandAll();
                    Control Con = Page.FindControl("TreeViewControl");
                    Con.Controls.Add(tView);
                }
            }
            catch (Exception ex)
            {
                ;// Handle exception
            }
        }
        private void BuildTreeNodes(TreeNodeCollection nodes, DataTable oDt, string parentId)
        {
            foreach (DataRow row in oDt.Rows)
            {
                if (row["TREEVIEW_PARENTID"].ToString().ToUpper().Trim() == parentId.ToUpper().Trim())
                {
                    TreeNode treeNode = new TreeNode(row["TREEVIEW_DESC"].ToString())
                    {
                        Value = row["TREEVIEW_ID"].ToString()
                    };
                    nodes.Add(treeNode);

                    BuildTreeNodes(treeNode.ChildNodes, oDt, treeNode.Value);
                }
            }
        }
    }
}