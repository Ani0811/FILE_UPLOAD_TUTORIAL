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

namespace SQLHelperWeb
{
    public class SQLHelperWeb : ISQLHelperWeb
    {
        public MySql.Data.MySqlClient.MySqlConnection GetSqlConnection()
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

        public DataSet GetData(string argSQL, string[] argParam = null, string[] argParamType = null, Object[] argParamValue = null)
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
        public int ExecuteProcedure(string argProcName, string[] argParamName = null, string[] argParamType = null, object[] argParamValue = null)
        {
            MySql.Data.MySqlClient.MySqlConnection objConnection = null;
            MySql.Data.MySqlClient.MySqlCommand objSQLCmd = null;
            MySql.Data.MySqlClient.MySqlTransaction objSQLTrans = null;

            int intReturnValue = 0;

            try
            {
                objConnection = new MySqlConnection();
                if (objConnection == null || objConnection.State == 0)
                {
                    objConnection = GetSqlConnection();
                }
                objSQLCmd = SetCommandParameters(objConnection, argProcName, argParamName, argParamType, argParamValue);
                objSQLTrans = objConnection.BeginTransaction();
                objSQLCmd.Transaction = objSQLTrans;

                intReturnValue = objSQLCmd.ExecuteNonQuery();
                objSQLTrans.Commit();
                objSQLCmd.Parameters.Clear();
            }
            catch (Exception exp) { objSQLTrans.Rollback(); }
            finally { objSQLTrans.Dispose(); objSQLTrans = null; objSQLCmd.Dispose(); objSQLCmd = null; objConnection.Close(); objConnection.Dispose(); objConnection = null; }
            return intReturnValue;
        }
        private MySqlCommand SetCommandParameters(MySqlConnection argConnection, string argProcName, string[] argParamName, string[] argParamType, object[] argParamValue)
        {
            MySqlCommand objSQLCmd = null;
            try
            {
                objSQLCmd = new MySqlCommand();
                objSQLCmd.Connection = argConnection;
                objSQLCmd.CommandTimeout = 0;
                objSQLCmd.CommandText = argProcName;
                objSQLCmd.CommandType = CommandType.StoredProcedure;

                if ((argParamName.Length > 0) && (argParamValue.Length > 0) && (argParamType.Length > 0))
                {
                    for (int iCount = 0; iCount < argParamValue.Length; iCount++)
                    {
                        if ((argParamType[iCount].ToUpper() == "STRING") || (argParamType[iCount].ToUpper() == "VARCHAR"))
                        {
                            objSQLCmd.Parameters.Add(argParamName[iCount], MySqlDbType.VarChar);
                            if ((argParamValue[iCount] != null))
                                objSQLCmd.Parameters[argParamName[iCount]].Value = argParamValue[iCount];
                            else
                                objSQLCmd.Parameters[argParamName[iCount]].Value = DBNull.Value;
                        }
                        else if (argParamType[iCount].ToUpper() == "LONGTEXT")
                        {
                            objSQLCmd.Parameters.Add(argParamName[iCount], MySqlDbType.LongText);
                            if ((argParamValue[iCount] != null))
                                objSQLCmd.Parameters[argParamName[iCount]].Value = argParamValue[iCount];
                            else
                                objSQLCmd.Parameters[argParamName[iCount]].Value = DBNull.Value;
                        }
                        else if (argParamType[iCount].ToUpper() == "DATETIME")
                        {
                            objSQLCmd.Parameters.Add(argParamName[iCount], MySqlDbType.DateTime);
                            if (argParamValue[iCount] != null)
                                objSQLCmd.Parameters[argParamName[iCount]].Value = Convert.ToDateTime(argParamValue[iCount]);
                            else
                                objSQLCmd.Parameters[argParamName[iCount]].Value = DBNull.Value;
                        }
                        else if ((argParamType[iCount].ToString().ToUpper().Trim() == "INT") || (argParamType[iCount].ToString().ToUpper().Trim() == "BIGINT"))
                        {
                            objSQLCmd.Parameters.Add(argParamName[iCount], MySqlDbType.Int64);
                            if (argParamValue[iCount] != null)
                                objSQLCmd.Parameters[argParamName[iCount]].Value = Convert.ToInt32(argParamValue[iCount]);
                            else
                                objSQLCmd.Parameters[argParamName[iCount]].Value = DBNull.Value;
                        }
                        else if ((argParamType[iCount].ToString().ToUpper().Trim() == "DECIMAL"))
                        {
                            objSQLCmd.Parameters.Add(argParamName[iCount], MySqlDbType.Decimal);
                            if (argParamValue[iCount] != null)
                                objSQLCmd.Parameters[argParamName[iCount]].Value = Convert.ToDecimal(argParamValue[iCount]);
                            else
                                objSQLCmd.Parameters[argParamName[iCount]].Value = DBNull.Value;
                        }
                        else if ((argParamType[iCount].ToUpper() == "BIT") || (argParamType[iCount].ToUpper() == "BOOL") || (argParamType[iCount].ToUpper() == "BOOLEAN"))
                        {
                            objSQLCmd.Parameters.Add(argParamName[iCount], MySqlDbType.Bit);
                            if (argParamValue[iCount] != null)
                                objSQLCmd.Parameters[argParamName[iCount]].Value = Convert.ToBoolean(argParamValue[iCount]);
                            else
                                objSQLCmd.Parameters[argParamName[iCount]].Value = DBNull.Value;
                        }
                    }
                }
            }
            catch (Exception ex) {; }
            finally {; }
            return objSQLCmd;
        }
    }
}
