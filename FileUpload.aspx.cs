using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using SQLHelperWeb;

namespace FileUploadWeb
{
    public partial class FileUpload : System.Web.UI.Page
    {
        
        SQLHelperWeb.ISQLHelperWeb objSQLHelperWeb = new SQLHelperWeb.SQLHelperWeb();
        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
            if (!IsPostBack)
            {
                i = Convert.ToInt32(Request["ID"]);

                if (i != 0) { DisplayImage(i); }
                else { GetFileUpload(); }
            }
        }

        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            HttpPostedFile postedFile = FileUpload1.PostedFile;
            if (postedFile.ContentLength != 0)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string fileExtension = Path.GetExtension(fileName);
                int fileSize = postedFile.ContentLength;

                Stream oStream = postedFile.InputStream;
                BinaryReader oBinaryReader = new BinaryReader(oStream);
                Byte[] bytes = oBinaryReader.ReadBytes((int)oStream.Length);

                using (MySqlConnection conn = objSQLHelperWeb.GetSqlConnection())
                {
                    MySqlCommand cmd = new MySqlCommand("SP_UPLOAD_IMAGE", conn);
                    MySql.Data.MySqlClient.MySqlTransaction objTrans = null;
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlParameter paramName = new MySqlParameter()
                    {
                        ParameterName = "@argImgName",
                        Value = fileName
                    };
                    cmd.Parameters.Add(paramName);

                    MySqlParameter paramSize = new MySqlParameter()
                    {
                        ParameterName = "@argImgSize",
                        Value = fileSize
                    };
                    cmd.Parameters.Add(paramSize);

                    MySqlParameter paramImageData = new MySqlParameter()
                    {
                        ParameterName = "@argImgData",
                        Value = bytes
                    };
                    cmd.Parameters.Add(paramImageData);

                    MySqlParameter paramFileType = new MySqlParameter()
                    {
                        ParameterName = "@argFileType",
                        Value = fileExtension.ToUpper(),
                    };
                    cmd.Parameters.Add(paramFileType);

                    objTrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = objTrans;
                    int iRowsAffected = cmd.ExecuteNonQuery();
                    objTrans.Commit();
                }
            }
            else { Response.Write("<script language='javascript'>alert('Please choose file...before uploading !');</script>"); }
            GetFileUpload();
        }
        private void GetFileUpload()
        {
            string fileName = null;
            string[] str_arg1 = null;
            string[] str_arg2 = null;
            object[] objVal = null;

            DataTable oDt = null;
            try
            {
                str_arg1 = new string[] { "@argImgID" };
                str_arg2 = new string[] { "INT" };
                objVal = new object[] { 0 };

                oDt = objSQLHelperWeb.GetData("SP_RETRIEVE_IMAGE", str_arg1, str_arg2, objVal).Tables[0];
                GD_View.PageIndex = (Session["PageIndex"] != null) ? Convert.ToInt32(Session["PageIndex"].ToString()) : 0;
                GD_View.DataSource = oDt;
                GD_View.DataBind();

                foreach (GridViewRow gr in GD_View.Rows)
                {
                    fileName = gr.Cells[1].Text.ToString().Trim().ToLower();

                    if ((fileName.EndsWith(".jpg") == true) || (fileName.EndsWith(".jpeg") == true))
                    {
                        LinkButton lb = new LinkButton() { Text = gr.Cells[1].Text, ID = gr.Cells[0].Text };
                        //lb.OnClientClick = "~/FileUpload.aspx?ID=" + lb.ID;
                        lb.PostBackUrl = "~/FileUpload.aspx?ID=" + lb.ID;
                        lb.CommandName = "Page";
                        lb.CommandArgument = lb.ID;
                        gr.Cells[1].Controls.Add(lb);
                    }
                    else
                    {
                        HyperLink hp = new HyperLink() { Text = gr.Cells[1].Text, ID = gr.Cells[0].Text };
                        hp.Target = "_Blank";
                        hp.NavigateUrl = "~/FileRetrieve.aspx?ID=" + hp.ID;
                        gr.Cells[1].Controls.Add(hp);
                    }
                }
            }
            catch (Exception ex) {; }
            finally { fileName = null; str_arg1 = null; str_arg2 = null; objVal = null; objSQLHelperWeb = null; oDt = null; }
        }
        private void DisplayImage(int argFileID)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.CssClass = "imageDiv";
            img.ImageUrl = "~/FileRetrieve.aspx?ID=" + argFileID;
            
            Img_Panel.Controls.Add(img);
            GetFileUpload();
            GD_View.PageIndex = Convert.ToInt32(Session["PageIndex"]);
        }

        protected void GD_View_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["PageIndex"] = e.NewPageIndex.ToString();
            GD_View.PageIndex = e.NewPageIndex;
            GetFileUpload();
        }

        protected void GD_View_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                GD_View.PageIndex = (Session["PageIndex"] != null) ? Convert.ToInt32(Session["PageIndex"].ToString()) : 0;
            }
        }
    }
}