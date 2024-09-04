using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace FileUploadWeb
{
    public partial class FileRetrieve : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(Request["ID"]);
            DisplayFile(i);
        }
        private void DisplayFile(int argFileID)
        {
            string sFileName = null;
            string sFileType = null;
            string[] str_arg1 = null;
            string[] str_arg2 = null;

            object[] objVal = null;
            byte[] bData = new byte[0];
            int iBytes = 0;

            SQLHelperWeb.ISQLHelperWeb objSQLHelperWeb = new SQLHelperWeb.SQLHelperWeb();

            try
            {
                if (argFileID != 0)
                {
                    str_arg1 = new string[] { "@argImgID" };
                    str_arg2 = new string[] { "INT" };

                    objVal = new object[] { argFileID };

                    DataTable dt = objSQLHelperWeb.GetData("SP_RETRIEVE_IMAGE", str_arg1, str_arg2, objVal).Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        BufferedStream bwBuffer = null;
                        bData = (byte[])dt.Rows[0]["IMAGE"];
                        sFileName = dt.Rows[0]["FILENAME"].ToString();
                        sFileType = dt.Rows[0]["FILETYPE"].ToString();
                        if (bData.Length > 0)
                        {
                            switch (sFileType)
                            {
                                case ".JPG":
                                    this.Context.Response.AppendHeader("Content-Type", "image/jpg");
                                    break;
                                case ".JPEG":
                                    this.Context.Response.AppendHeader("Content-Type", "image/jpeg");
                                    break;
                                case ".TXT":
                                    this.Context.Response.AppendHeader("Content-Type", "text/plain");
                                    break;
                                case ".PDF":
                                    this.Context.Response.AppendHeader("Content-Type", "application/pdf");
                                    break;
                                case ".GIF":
                                    this.Context.Response.AppendHeader("Content-Type", "image/gif");
                                    break;
                                case ".MP3":
                                    this.Context.Response.AppendHeader("Content-Type", "audio/mpeg");
                                    break;
                                case ".MP4":
                                    this.Context.Response.AppendHeader("Content-Type", "video/mp4");
                                    break;
                                default:
                                    break;
                            }
                            this.Context.Response.AppendHeader("Content-Disposition", "inline; filename=" + Path.GetFileName(sFileName));
                            //this.Context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(sFileName));

                            bwBuffer = new BufferedStream(this.Response.OutputStream);
                            iBytes = bData.Length;
                            bwBuffer.Write(bData, 0, iBytes);
                            bwBuffer.Close();
                        }
                    }
                }
            }
            catch (Exception ex) {; }
            finally { sFileName = null; sFileName = null; str_arg1 = null; str_arg2 = null; objVal = null; objSQLHelperWeb = null; }
        }
    }
}