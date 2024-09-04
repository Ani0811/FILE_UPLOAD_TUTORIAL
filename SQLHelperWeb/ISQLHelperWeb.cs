using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SQLHelperWeb
{
    public interface ISQLHelperWeb
    {
        DataSet GetData(string argSQL, string[] argParam = null, string[] argParamType = null, object[] argParamValue = null);
        MySqlConnection GetSqlConnection();
        int ExecuteProcedure(string argProcName, string[] argParamName = null, string[] argParamType = null, object[] argParamValue = null);
    }
}