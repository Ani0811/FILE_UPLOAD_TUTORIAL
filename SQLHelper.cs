using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SQLHelper
{
	public class SQLHelper
	{
        private MySql.Data.MySqlClient.MySqlConnection GetSqlConnection()
        {
            string strServer = null;
            string strPort = null;
            string strDatabase = null;
            string strPWD = null;
            string strUserID = null;
            string strSystem = null;

            MySql.Data.MySqlClient.MySqlConnection objConn = null;

            try
            {
                //var ExecAppPath = this.GetType().Assembly.Location;
                //ConfigurationManager.OpenExeConfiguration(ExecAppPath).AppSettings.Settings;


                strSystem = Convert.ToString(ConfigurationManager.AppSettings["SERVERIMPLEMENTATION"]);

                strServer = Convert.ToString(ConfigurationManager.AppSettings["SERVER"]);
                strPort = Convert.ToString(ConfigurationManager.AppSettings["PORT"]);
                strDatabase = Convert.ToString(ConfigurationManager.AppSettings["DATABASE"]);
                strUserID = Convert.ToString(ConfigurationManager.AppSettings["UID"]);
                strPWD = Convert.ToString(ConfigurationManager.AppSettings["PWD"]);

                if (objConn != null) { objConn.Dispose(); }
                else { objConn = new MySql.Data.MySqlClient.MySqlConnection(@"SERVER=" + strServer + "; DATABASE=" + strDatabase + "; USERID=" + strUserID + "; PWD=" + strPWD + "; SSL Mode=None; persistsecurityinfo=True; AllowZeroDateTime=True; UseAffectedRows=True;"); }

                try
                {
                    if (objConn.State == ConnectionState.Open) { objConn.Close(); objConn.Dispose(); }
                    else { objConn.Open(); }
                    if (objConn == null)
                    {
                        objConn = new MySql.Data.MySqlClient.MySqlConnection();
                        objConn.ConnectionString = @"SERVER=" + strServer + ":" + strPort + "; DATABASE=" + strDatabase + "; USERID=" + strUserID + "; PWD=" + strPWD + "; SSL Mode=None; persistsecurityinfo=True; AllowZeroDateTime=True; UseAffectedRows=True;";
                        objConn.Open();
                    }
                }
                catch (Exception ex) {; }
            }
            catch (Exception ex) {; }
            finally {; }//objConn.Close();}
            return objConn;
        }

        private DataSet GetData(string argSQL, string[] argParam = null, string[] argParamType = null, Object[] argParamValue = null)
        {
            int iRowsFetched = 0;
            DataSet objResultDataSet = null;

            MySql.Data.MySqlClient.MySqlDataAdapter objDataAdapter = null;

            try
            {
                objDataAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter();
                objDataAdapter.SelectCommand = new MySql.Data.MySqlClient.MySqlCommand(argSQL, GetSqlConnection());
                objDataAdapter.SelectCommand.CommandTimeout = 0;
                objDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                if (argParam != null && argParamType != null && argParamValue != null)
                {
                    for (int iParamCount = 0; iParamCount < argParam.Length; iParamCount++)
                    {
                        if (argParamType[iParamCount].ToString().ToUpper().Trim() == "VARCHAR")
                        {
                            if (argParamValue[iParamCount] != null)
                            {
                                objDataAdapter.SelectCommand.Parameters.Add(Convert.ToString(argParam[iParamCount]), MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToString(argParamValue[iParamCount]);
                            }
                            else
                            {
                                objDataAdapter.SelectCommand.Parameters.Add(Convert.ToString(argParam[iParamCount]), MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = DBNull.Value;
                            }
                        }
                        else if (argParamType[iParamCount].ToString().ToUpper().Trim() == "INT")
                        {
                            objDataAdapter.SelectCommand.Parameters.Add(Convert.ToString(argParam[iParamCount]), MySql.Data.MySqlClient.MySqlDbType.Int64).Value = Convert.ToInt64(argParamValue[iParamCount]);
                        }
                        else if (argParamType[iParamCount].ToString().ToUpper().Trim() == "DOUBLE")
                        {
                            objDataAdapter.SelectCommand.Parameters.Add(Convert.ToString(argParam[iParamCount]), MySql.Data.MySqlClient.MySqlDbType.Double).Value = Convert.ToDouble(argParamValue[iParamCount]);
                        }
                        else if (argParamType[iParamCount].ToString().ToUpper().Trim() == "DATETIME")
                        {
                            objDataAdapter.SelectCommand.Parameters.Add(Convert.ToString(argParam[iParamCount]), MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = (DateTime)argParamValue[iParamCount];
                        }
                        else if (argParamType[iParamCount].ToString().ToUpper().Trim() == "BIT")
                        {
                            objDataAdapter.SelectCommand.Parameters.Add(Convert.ToString(argParam[iParamCount]), MySql.Data.MySqlClient.MySqlDbType.Bit).Value = (bool)argParamValue[iParamCount];
                        }
                        /*else if (argParamType[iParamCount].ToString().ToUpper().Trim() == "LONGBLOB")
                        {
                            objDataAdapter.SelectCommand.Parameters.Add(Convert.ToString(argParam[iParamCount]), MySql.Data.MySqlClient.MySqlDbType.LongBlob).Value = (SqlBinary)argParamValue[iParamCount];
                        }*/
                    }
                }

                objResultDataSet = new DataSet();
                objResultDataSet.Clear();
                iRowsFetched = objDataAdapter.Fill(objResultDataSet, "DataTable");

                if (objResultDataSet != null && objResultDataSet.Tables.Count > 0) {; }
                objDataAdapter.SelectCommand.Connection = null;
                if (objDataAdapter != null) { objDataAdapter.Dispose(); }
            }
            catch (MySql.Data.MySqlClient.MySqlException MySqlEx) {; }
            finally { objDataAdapter = null; }
            return objResultDataSet;
        }
    }
}
