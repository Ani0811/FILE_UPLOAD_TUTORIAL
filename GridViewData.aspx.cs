using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SQLHelperWeb;
using System.Drawing;
using Antlr.Runtime.Misc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Reflection;

namespace FileUploadWeb
{
    public partial class GridViewData : System.Web.UI.Page
    {
        SQLHelperWeb.ISQLHelperWeb objSQLHelperWeb = new SQLHelperWeb.SQLHelperWeb();
        public string strGridView = string.Empty;
        DataTable oDt = null;
        StringBuilder sb = null;
        int iTotal_RowsCount = 0;
        int iPg = 1;
        int iDivider = 5;

        string[] sPgID = null;
        string sQryStr = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack)
            //{
            //    iPg = Convert.ToInt32(Request["PG"]);
            //}
            if (Request["ACTION"] != null && Request["ACTION"] == "DEL")
            {
                if (Request["PG"] != null && Request["PG"].ToString().Contains("|") == true)
                {
                    /**********************************
                     * Loop through from Code Behind, store proc called from C# function
                     * ********************************
                    sQryStr = Request["PG"].ToString();
                    sPgID = sQryStr.Split('|');

                    foreach (var lt in sPgID) 
                    {
                        if (lt != "") { Get_DEL_Row(Convert.ToInt32(lt)); }
                    }
                    ***********************************/
                    /**********************************
                     *Loop at store procedure
                     **********************************/
                    sQryStr = Request["PG"].ToString();
                    Get_DEL_Row(sQryStr);
                    /**********************************/

                    /**********************************
                     * QueryString XML
                     * ********************************/
                    
                }
                Response.ClearContent();

                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                isreadonly.SetValue(HttpContext.Current.Request.QueryString, false, null);

                HttpContext.Current.Request.QueryString.Clear();
                isreadonly.SetValue(HttpContext.Current.Request.QueryString, true, null);
                //isreadonly.SetValue(HttpContext.Current.Request.QueryString, true, null);
            }
            else if (Request["PG"] != null)
            {
                iPg = Convert.ToInt32(Request["PG"]);
            }
            /*else if (Request.Form["pgRowCount"] != null)
            {
                iDivider = Convert.ToInt32(Request.Form["pgRowCount"]);
            }*/
            else if (Request["PAGECOUNT"] != null)
            {
                iDivider = Convert.ToInt32(Request["PAGECOUNT"]);
            }
            strGridView = Get_GridViewData();
        }
        private void Get_DEL_Row(int argImgID)
        {
            string[] str_arg1 = null;
            string[] str_arg2 = null;
            object[] objVal = null;
            int iRowsAffected = 0;

            try 
            {
                str_arg1 = new string[] { "@argImgID" };
                str_arg2 = new string[] { "INT" };
                objVal = new object[] { argImgID };
                
                iRowsAffected = objSQLHelperWeb.ExecuteProcedure("SP_DELETE_RECORD", str_arg1, str_arg2, objVal);
                
            }
            catch (Exception ex) {; }
            finally { str_arg1 = null; str_arg2 = null; objVal = null; }
        }
        private void Get_DEL_Row(string argImgID)
        {
            string[] str_arg1 = null;
            string[] str_arg2 = null;
            object[] objVal = null;
            int iRowsAffected = 0;

            try
            {
                str_arg1 = new string[] { "@argImgID" };
                str_arg2 = new string[] { "LONGTEXT" };
                objVal = new object[] { argImgID };

                iRowsAffected = objSQLHelperWeb.ExecuteProcedure("SP_DELETE_RECORD3", str_arg1, str_arg2, objVal);

            }
            catch (Exception ex) {; }
            finally { str_arg1 = null; str_arg2 = null; objVal = null; }
        }
        /*private void Get_DEL_Row(string argImgID_XML)
        {
            string[] str_arg1 = null;
            string[] str_arg2 = null;
            object[] objVal = null;
            int iRowsAffected = 0;

            try
            {
                str_arg1 = new string[] { "@argImgID_XML" };
                str_arg2 = new string[] { "LONGTEXT" };
                objVal = new object[] { argImgID_XML };

                iRowsAffected = objSQLHelperWeb.ExecuteProcedure("SP_DELETE_RECORD_XML", str_arg1, str_arg2, objVal);

            }
            catch (Exception ex) {; }
            finally { str_arg1 = null; str_arg2 = null; objVal = null; }
        }*/
        private string Get_GridViewData()
        {
            sb = new StringBuilder();
            sb.Append("<table id=\"T1\" align=\"center\" border=\"1\">");
            Get_Tbl_Column_Headers();
            sb.Append("</table>");

            //sb.Append("<iframe align=\"center\" border=\"1\">");
            //sb.Append("</iframe>");
            
            return sb.ToString();

        }
        private void Get_Tbl_Column_Headers() 
        {
            DataSet oDs = null;

            string[] str_arg1 = null;
            string[] str_arg2 = null;
            object[] objVal = null;

            try
            {
                str_arg1 = new string[] { "@PageIndex", "@PageSize" };
                str_arg2 = new string[] { "INT", "INT" };
                objVal = new object[] { iPg, iDivider };

                oDs = objSQLHelperWeb.GetData("SP_GET_GALLERY_LIST", str_arg1, str_arg2, objVal);
                if(oDs != null && oDs.Tables[1].Rows.Count > 0)
                {
                    iTotal_RowsCount = Convert.ToInt32(oDs.Tables[0].Rows[0][0]);
                    oDt = oDs.Tables[1];

                    sb.Append("<thead id=\"Header\" style=\"background-color:forestgreen\" border-style=\"solid\">");
                    sb.Append("<tr>");
                    foreach(DataColumn dc in oDt.Columns)
                    {
                        sb.Append("<td>");
                        sb.Append(dc.ColumnName == "IMGID" ? string.Empty : dc.ColumnName);
                        if(dc.ColumnName == "IMGID") 
                        {
                            sb.Append("<input id=\"chb_All\" type=\"checkbox\" onclick=\"getChbAll()\"/>"); 
                        }

                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");
                    sb.Append("</thead>");
                    Get_Tbl_Rows(oDt);
                }
            }
            catch(Exception ex) { ; }
            finally { oDs = null; str_arg1 = null; str_arg2 = null; objVal = null; }
        }
        private void Get_Tbl_Rows(DataTable oDt)
        {
            int i = 1;
            try
            {
                sb.Append("<tbody>");
                foreach (DataRow row in oDt.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn col in oDt.Columns)
                    {
                        sb.Append("<td class=\"auto-style8\">");

                        if(col.ColumnName == "IMGID")
                        {
                            sb.Append("<input id=\"chb_" + i.ToString() + "\" type=\"checkbox\" title=\"chb_" + row[col.ColumnName].ToString() + "\" onclick=\"getChb(this)\" />");
                            i++;
                        }
                        else if (col.ColumnName == "IMGNAME")
                        {
                            //sb.Append("<a id=" + row[col.ColumnName].ToString() + " href=\"GridViewData.aspx?PG=" + row[col.ColumnName].ToString() + "\" >" + row[col.ColumnName].ToString() + "</a>");
                            sb.Append("<a id=" + row[col.ColumnName].ToString() + " href=\"#\" onclick=\"Display_Image('" + row["IMGID"].ToString() + "','" + row["FILETYPE"].ToString() + "')\">" + row[col.ColumnName].ToString() + "</a>");
                        }
                        else
                        {
                            sb.Append(row[col.ColumnName].ToString());
                        }
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                Get_Tbl_Footer(oDt.Rows.Count);
            }
            catch(Exception ex) { ; }
            finally { ; }
        }
        private void Get_Tbl_Footer(int iRowsCount)
        {
            try
            {
                sb.Append("<tfoot id =\"Footer1\" style=\"background-color:forestgreen\" border-style=\"solid\">");
                sb.Append("<tr><td></td>");
                Get_Pagination();
                sb.Append("<td></td><td></td></tr>");
                
                sb.Append("<tr>");
                sb.Append("<td class=\"auto-style6\"><input id=\"del\" type=\"submit\" value=\"Delete\" onclick=\"Delete_Click()\"/>");
                sb.Append("<td class=\"auto-style6\">No. Of Records : " + iRowsCount.ToString() + "</td>");
                sb.Append("<td class=\"auto-style6\"></td>");
                sb.Append("<td class=\"auto-style6\">");

                if (iRowsCount < 5)
                {
                    sb.Append("<select id =\"pgRowCount\" name=\"pgRowCount\" disabled title=\"pgRowCount\">" +
                            "<option value=\"5\" selected>5</option>" +
                            "<option value = \"10\">10</option>" +
                            "<option value=\"15\">15</option></select>");
                    sb.Append("<input id=\"go\" type=\"submit\" disabled value=\"Go\">");
                }
                else
                {
                    sb.Append("<select id =\"pgRowCount\" name=\"pgRowCount\" title=\"pgRowCount\">");
                    if (iDivider == 5) 
                    { 
                        sb.Append("<option value=\"5\" selected>5</option>");
                        sb.Append("<option value = \"10\">10</option>");
                        sb.Append("<option value=\"15\">15</option>");
                    }
                    else if (iDivider == 10) 
                    {
                        sb.Append("<option value=\"5\">5</option>");
                        sb.Append("<option value=\"10\" selected>10</option>");
                        sb.Append("<option value=\"15\">15</option>");
                    }
                    else if (iDivider == 15) 
                    {
                        sb.Append("<option value=\"5\">5</option>");
                        sb.Append("<option value=\"10\">10</option>");
                        sb.Append("<option value=\"15\" selected>15</option>");
                    }
                    sb.Append("</select>");
                    sb.Append("<input id=\"go\" type=\"submit\" value=\"Go\" onclick=\"Go_Submit()\"/>");
                }
                sb.Append("</td></tr></tfoot>");
            }
            catch(Exception ex) { ; }
            finally {; }  
        }
        private void Get_Pagination()
        {
            try
            {
                double iPageCount = Math.Ceiling((double)iTotal_RowsCount / (double)iDivider);
                sb.Append("<td>");
                for (int i = 1; i < (iPageCount + 1); i++)
                {
                    sb.Append("&nbsp;");
                    sb.Append("<a id=" + i.ToString() + "\" href=\"GridViewData.aspx?PG=" + i.ToString() + "\" >" + i.ToString() + "</a>");
                    sb.Append("&nbsp;");
                }
                sb.Append("</td>");
            }
            catch (Exception ex) {; }
            finally {; }
        }
    }
}