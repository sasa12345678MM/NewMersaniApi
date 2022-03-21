using System;
using Dapper;
using System.IO;
using System.Linq;
using System.Data;
using Mersani.Utility;
using Newtonsoft.Json;
using Mersani.models.Auth;
using Mersani.Utility.Exceptions;
using System.Collections.Generic;
using Mersani.models.Administrator;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using Mersani.models.Users;
using Mersani.models.website;

namespace Mersani.Oracle
{
    public class OracleDQ
    {
        public static string getConnectionString()
        {
            return Util.getConnectionString(Directory.GetCurrentDirectory() + "\\", "OrcleStr");
        }

        public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TValue> values = Enumerable.ToList(dict.Values);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }

        public static IEnumerable<TKey> RandomKeys<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TKey> keys = Enumerable.ToList(dict.Keys);
            int size = dict.Count;
            while (true)
            {
                yield return keys[rand.Next(size)];
            }
        }

        internal static Task<DataSet> ExcuteXmlProcAsync(string v, List<dynamic> dynamics, object authParms)
        {
            throw new NotImplementedException();
        }

        public static List<T> GetData<T>(string _query, string secParm, object param = null, CommandType? commandType = null, bool public_ = false)
        {
            var result = new List<T>();
            try
            {
                bool isAuthorized = checkAuthenticatedUser(secParm);
                if (isAuthorized || public_)
                {
                    var connectionString = getConnectionString();
                    var conn = new OracleConnection(connectionString);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        if (commandType == CommandType.StoredProcedure)
                            result = SqlMapper.Query<T>(conn, _query, param, commandType: CommandType.StoredProcedure).ToList();
                        else
                            result = SqlMapper.Query<T>(conn, _query, param).ToList();//.Select(x=> SerializeEntity.NormalizeKeys(JObject.FromObject(x));

                        conn.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                {
                    result = null;
                }
            }
            catch (Exception ex)
            {
                throw new NotFoundException(ex.Message);
            }

            return result;
        }

        public static bool PostData(string _query, string secParm, object param = null, CommandType? commandType = null)
        {
            var result = false;
            object data = null;
            try
            {
                bool isAuthorized = checkAuthenticatedUser(secParm);
                if (isAuthorized)
                {
                    var connectionString = getConnectionString();
                    var conn = new OracleConnection(connectionString);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        if (commandType == CommandType.StoredProcedure)
                            data = SqlMapper.Query(conn, _query, param, commandType: CommandType.StoredProcedure);
                        else
                            data = SqlMapper.Query(conn, _query, param, commandType: CommandType.Text);

                        if (data != null) result = true;
                        conn.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                    result = false;
            }
            catch (Exception ex)
            {
                throw new NotFoundException(ex.Message);
            }

            return result;
        }

        public static async Task<List<T>> GetDataAsync<T>(string _query, string authParam, object param = null, CommandType? commandType = null, bool public_ = false)
        {
            var result = new List<T>();
            IEnumerable<T> data = null;
            try
            {
                bool isAuthorized = checkAuthenticatedUser(authParam);
                if (isAuthorized || public_)
                {
                    var connectionString = getConnectionString();
                    var conn = new OracleConnection(connectionString);
                    if (conn.State == ConnectionState.Closed)
                    {
                        await conn.OpenAsync();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        if (commandType == CommandType.StoredProcedure)
                            data = await SqlMapper.QueryAsync<T>(conn, _query, param, commandType: CommandType.StoredProcedure);
                        else
                            data = await SqlMapper.QueryAsync<T>(conn, _query, param);//.Select(x=> SerializeEntity.NormalizeKeys(JObject.FromObject(x));

                        result = data.ToList();
                        conn.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                {
                    result = null;
                }
            }
            catch (Exception ex)
            {
                throw new NotFoundException(ex.Message);
            }

            return result;
        }
        public static async Task<bool> PostDataAsync(string _query, string authParam, object param = null, CommandType? commandType = null)
        {
            var result = false;
            object data;
            try
            {
                bool isAuthorized = checkAuthenticatedUser(authParam);
                if (isAuthorized)
                {
                    var connectionString = getConnectionString();
                    var conn = new OracleConnection(connectionString);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    if (conn.State == ConnectionState.Open)
                    {
                        if (commandType == CommandType.StoredProcedure)
                            data = await SqlMapper.QueryAsync(conn, _query, param, commandType: CommandType.StoredProcedure);
                        else
                            data = await SqlMapper.QueryAsync(conn, _query, param, commandType: CommandType.Text);

                        if (data != null) result = true;
                        conn.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                    result = false;
            }
            catch (Exception ex)
            {
                throw new NotFoundException(ex.Message);
            }

            return result;
        }

        // Get
        public static async Task<DataSet> ExcuteXmlProcAsync(string procName, List<dynamic> entities, string authParam, bool puplic = false)
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            bool isAuthorized = checkAuthenticatedUser(authParam);
            if (isAuthorized || puplic)
            {
                // here we will encode entity as xml type
                string xmlParms = SerializeEntity.Encode(entities);
                if (xmlParms.Contains("&")) xmlParms = xmlParms.Replace("&", "&amp;");

                // send to database and return dataset
                OracleConnection oraCon = new OracleConnection(connectionString);
                if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
                OracleCommand oraCommand = new OracleCommand();
                oraCommand.CommandType = CommandType.StoredProcedure;
                oraCommand.CommandText = procName;
                oraCommand.Connection = oraCon;
                oraCommand.Parameters.Add("xml_document", OracleDbType.XmlType, xmlParms, ParameterDirection.Input);

                // error code
                OracleParameter errorCode = new OracleParameter("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
                errorCode.Size = 256;
                oraCommand.Parameters.Add(errorCode);

                // error msg
                OracleParameter errorMsg = new OracleParameter("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
                errorMsg.Size = 256;
                oraCommand.Parameters.Add(errorMsg);

                OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);

                try
                {
                    adapt.Fill(ds, rootName);
                }
                catch (Exception ex)
                {
                    return handleExceptionInDataSet(ds, new { Id = "-1", Message = ex.Message });
                }
                finally
                {
                    int tcount = ds.Tables.Count;
                    if (tcount == 1)
                    {
                        ds.Tables[tcount - 1].TableName = "result";
                        ds.Tables.Add("message");
                    }
                    if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";

                    if (tcount == 0)
                    {
                        ds.Tables.Add("result");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                    }
                    // set error msg

                    // close connection
                    adapt.Dispose();
                    oraCon.Close();
                    oraCon.Dispose();
                }
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            else
            {
                return handleExceptionInDataSet(ds, null);
            }
            return ds;
        }

        // Get data as dataset
        public static async Task<DataSet> ExcuteGetQueryAsync(string _query, List<OracleParameter> oracleParameters, string authParam, CommandType commandType, string rootName = "result", bool _public = false)
        {
            DataSet ds = new DataSet();
            string connection = getConnectionString();
            bool isAuthorized = checkAuthenticatedUser(authParam);
            if (isAuthorized || _public)
            {
                OracleConnection conn = new OracleConnection(connection);
                await conn.OpenAsync();
                OracleCommand cmnd = new OracleCommand(_query, conn);
                OracleDataAdapter adapt = new OracleDataAdapter(cmnd);

                adapt.SelectCommand.CommandType = commandType;
                if (oracleParameters != null)
                    adapt.SelectCommand.Parameters.AddRange(oracleParameters.ToArray());

                /***********************************
                 * +********************************************/
                try
                {
                    adapt.Fill(ds, rootName);
                }
                catch (Exception exc)
                {
                    return handleExceptionInDataSet(ds, exc);
                }
                finally
                {
                    int tcount = ds.Tables.Count;
                    if (tcount == 0)
                    {
                        ds.Tables.Add("result");
                        DataTable errorTable = ds.Tables.Add("message");
                        if (!errorTable.Columns.Contains("msgHead"))
                        {
                            errorTable.Columns.Add("msgHead", typeof(string));
                            errorTable.Columns.Add("msgBody", typeof(string));
                            errorTable.Rows.Add(new Object[] { "1", "Done" });
                        }
                    }
                    if (tcount == 1)
                    {
                        ds.Tables[tcount - 1].TableName = "result";

                        DataTable errorTable = ds.Tables.Add("message");
                        if (!errorTable.Columns.Contains("msgHead"))
                        {
                            errorTable.Columns.Add("msgHead", typeof(string));
                            errorTable.Columns.Add("msgBody", typeof(string));
                            errorTable.Rows.Add(new Object[] { "1", "Done" });
                        }
                    }
                    if (tcount > 1)
                    {
                        ds.Tables[tcount - 1].TableName = "message";
                        DataTable errorTable = ds.Tables[tcount - 1];
                        if (!errorTable.Columns.Contains("msgHead"))
                        {
                            errorTable.Columns.Add("msgHead", typeof(string));
                            errorTable.Columns.Add("msgBody", typeof(string));
                            errorTable.Rows.Add(new Object[] { "1", "Done" });
                        }
                    }
                    //--------------------------------------------------
                    adapt.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
                adapt.Dispose();
                conn.Close();
                conn.Dispose();
            }
            else
            {
                return handleExceptionInDataSet(ds, null);
            }

            return ds;
        }

        // return datatset with nor result and exception error
        public static DataSet handleExceptionInDataSet(DataSet ds, dynamic ex)
        {
            if (ds.Tables[0].TableName != "result") ds.Tables.Add("result");
            DataTable errorTable = ds.Tables.Add("message");

            errorTable.Columns.Add("msgHead", typeof(string));
            errorTable.Columns.Add("msgBody", typeof(string));

            if (ex == null) errorTable.Rows.Add(new Object[] { "-1", "User Not Found!, Please Login First!" });
            else errorTable.Rows.Add(new Object[] { "-1", ex.Message });

            return ds;
        }

        public static Task<DataSet> handleOnlineOfflineDataSet(DataSet ds, dynamic data)
        {
            DataTable resTable = ds.Tables.Add("result");
            resTable.Columns.Add("ON_OFF_Y_N", typeof(char));

            DataTable errorTable = ds.Tables.Add("message");

            errorTable.Columns.Add("msgHead", typeof(int));
            errorTable.Columns.Add("msgBody", typeof(bool));

            bool online = data.GetType().GetProperty("message").GetValue(data, null);
            if (data == null || !online)
            {
                resTable.Rows.Add(new Object[] { 'N' });
                errorTable.Rows.Add(new Object[] { 1, false });
            }
            else
            {
                resTable.Rows.Add(new Object[] { 'Y' });
                errorTable.Rows.Add(new Object[] { 1, data.message });
            }

            return Task.Run(() => { return ds; });
        }

        // Get Data For Selectize
        public static async Task<DataSet> ExcuteSelectizeProcAsync(string procName, List<dynamic> entities, string authParam, bool _public = false, char encodeType = 'N')
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            bool isAuthorized = checkAuthenticatedUser(authParam);
            if (isAuthorized || _public)
            {
                // send to database and return dataset
                OracleConnection oraCon = new OracleConnection(connectionString);
                if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
                OracleCommand oraCommand = new OracleCommand();
                oraCommand.CommandType = CommandType.StoredProcedure;
                oraCommand.CommandText = procName;
                oraCommand.Connection = oraCon;

                string xmlParms = encodeType == 'N' ? SerializeEntity.Encode(entities): SerializeEntity.QueryStringToXML(entities);
                if (xmlParms.Contains("&")) xmlParms = xmlParms.Replace("&", "&amp;");

                oraCommand.Parameters.Add("P_GENERAL_XML", OracleDbType.XmlType, xmlParms, ParameterDirection.Input);

                OracleParameter pResult = new OracleParameter("P_GENERAL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                oraCommand.Parameters.Add(pResult);
                // error code
                OracleParameter errorCode = new OracleParameter("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
                errorCode.Size = 256;
                oraCommand.Parameters.Add(errorCode);

                // error msg
                OracleParameter errorMsg = new OracleParameter("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
                errorMsg.Size = 256;
                oraCommand.Parameters.Add(errorMsg);
                OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);
                try
                {
                    adapt.Fill(ds, rootName);
                }
                catch (Exception exc)
                {
                    return handleExceptionInDataSet(ds, new { Id = "-1", Message = exc.Message });
                }
                finally
                {
                    int tcount = ds.Tables.Count;
                    if (tcount == 1)
                    {
                        ds.Tables[tcount - 1].TableName = "result";
                        //ds.Tables.Add("message");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                    }
                    if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";
                    if (tcount == 0)
                    {
                        ds.Tables.Add("result");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                    }
                    // set error msg

                    // close connection
                    adapt.Dispose();
                    oraCon.Close();
                    oraCon.Dispose();
                }
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            else
            {
                return handleExceptionInDataSet(ds, null);
            }
            return ds;
        }

        // Post Data as Master Object with details list
        public static async Task<DataSet> ExcuteMasterDetailsXMLAsync(string procName, dynamic parameters, string authParam)
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            bool isAuthorized = checkAuthenticatedUser(authParam);
            if (isAuthorized)
            {
                OracleConnection oraCon = new OracleConnection(connectionString);
                if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
                OracleCommand oraCommand = new OracleCommand();
                oraCommand.CommandType = CommandType.StoredProcedure;
                oraCommand.CommandText = procName;
                oraCommand.Connection = oraCon;
                foreach (var param in parameters)
                {
                    string xmlParms = SerializeEntity.Encode(param.Value);
                    if (xmlParms.Contains("&")) xmlParms = xmlParms.Replace("&", "&amp;");
                    oraCommand.Parameters.Add(param.Key, OracleDbType.XmlType, xmlParms, ParameterDirection.Input);
                }

                OracleParameter errorCode = new OracleParameter();
                errorCode.ParameterName = "VERRORCODE";
                errorCode.Direction = ParameterDirection.Output;
                errorCode.OracleDbType = OracleDbType.Varchar2;
                errorCode.Size = 256;
                oraCommand.Parameters.Add(errorCode);

                // error msg
                OracleParameter errorMsg = new OracleParameter();
                errorMsg.ParameterName = "VERRORMSG";
                errorMsg.Direction = ParameterDirection.Output;
                errorMsg.OracleDbType = OracleDbType.Varchar2;
                errorMsg.Size = 256;
                oraCommand.Parameters.Add(errorMsg);

                OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);

                try
                {
                    adapt.Fill(ds, rootName);
                }
                catch (Exception exc)
                {
                    return handleExceptionInDataSet(ds, new { Id = "-1", Message = exc.Message });
                }
                finally
                {
                    int tcount = ds.Tables.Count;
                    if (tcount == 1)
                    {
                        ds.Tables[tcount - 1].TableName = "result";
                        ds.Tables.Add("error");
                    }
                    if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";

                    if (tcount == 0)
                    {
                        ds.Tables.Add("result");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                    }
                    // set error msg

                    // close connection
                    adapt.Dispose();
                    oraCon.Close();
                    oraCon.Dispose();
                }
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            else
            {
                return handleExceptionInDataSet(ds, null);
            }
            return ds;
        }

        public static async Task<DataSet> ExcuteMasterDetailsXMLAsyncWithOutPut(string procName, dynamic parameters, string authParam)
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            bool isAuthorized = checkAuthenticatedUser(authParam);
            if (isAuthorized)
            {
                OracleConnection oraCon = new OracleConnection(connectionString);
                if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
                OracleCommand oraCommand = new OracleCommand();
                oraCommand.CommandType = CommandType.StoredProcedure;
                oraCommand.CommandText = procName;
                oraCommand.Connection = oraCon;
                foreach (var param in parameters)
                {
                    string xmlParms = SerializeEntity.Encode(param.Value);
                    if (xmlParms.Contains("&")) xmlParms = xmlParms.Replace("&", "&amp;");
                    oraCommand.Parameters.Add(param.Key, OracleDbType.XmlType, xmlParms, ParameterDirection.Input);
                    //oraCommand.Parameters.Add(param.Key, OracleDbType.XmlType, SerializeEntity.Encode(param.Value), ParameterDirection.Input);
                }
                oraCommand.Parameters.Add("P_GENERAL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                OracleParameter errorCode = new OracleParameter();
                errorCode.ParameterName = "VERRORCODE";
                errorCode.Direction = ParameterDirection.Output;
                errorCode.OracleDbType = OracleDbType.Varchar2;
                errorCode.Size = 256;
                oraCommand.Parameters.Add(errorCode);

                // error msg
                OracleParameter errorMsg = new OracleParameter();
                errorMsg.ParameterName = "VERRORMSG";
                errorMsg.Direction = ParameterDirection.Output;
                errorMsg.OracleDbType = OracleDbType.Varchar2;
                errorMsg.Size = 256;
                oraCommand.Parameters.Add(errorMsg);

                OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);

                try
                {
                    adapt.Fill(ds, rootName);
                }
                catch (Exception exc)
                {
                    return handleExceptionInDataSet(ds, new { Id = "-1", Message = exc.Message });
                }
                finally
                {
                    int tcount = ds.Tables.Count;
                    if (tcount == 1)
                    {
                        ds.Tables[tcount - 1].TableName = "result";
                        //ds.Tables.Add("error");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                    }
                    if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";

                    if (tcount == 0)
                    {
                        ds.Tables.Add("result");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                    }
                    // set error msg

                    // close connection
                    adapt.Dispose();
                    oraCon.Close();
                    oraCon.Dispose();
                }
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            else
            {
                return handleExceptionInDataSet(ds, null);
            }
            return ds;
        }

        public static async Task<DataSet> LoginAuthCheck(string _query, UserLoginModal userLogin)
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            // send to database and return dataset
            OracleConnection oraCon = new OracleConnection(connectionString);
            if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
            OracleCommand oraCommand = new OracleCommand();
            oraCommand.CommandType = CommandType.StoredProcedure;
            oraCommand.CommandText = _query;//"GetActionType";
            oraCommand.Connection = oraCon;
            oraCommand.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, userLogin.UserName, ParameterDirection.Input);
            oraCommand.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, userLogin.Password, ParameterDirection.Input);

            OracleParameter pResult = new OracleParameter("P_USER_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
            oraCommand.Parameters.Add(pResult);
            // error code
            OracleParameter errorCode = new OracleParameter("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            errorCode.Size = 256;
            oraCommand.Parameters.Add(errorCode);

            // error msg
            OracleParameter errorMsg = new OracleParameter("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
            errorMsg.Size = 256;
            oraCommand.Parameters.Add(errorMsg);



            OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);

            try
            {
                adapt.Fill(ds, rootName);
            }
            catch (Exception exc)
            {
                return handleExceptionInDataSet(ds, new { Id = "-1", Message = exc.Message });
            }
            finally
            {
                int tcount = ds.Tables.Count;
                if (tcount == 1)
                {
                    ds.Tables[tcount - 1].TableName = "result";
                    DataTable errorTable = ds.Tables.Add("message");
                    errorTable.Columns.Add("msgHead", typeof(string));
                    errorTable.Columns.Add("msgBody", typeof(string));
                    errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                }
                if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";

                if (tcount == 0)
                {
                    ds.Tables.Add("result");
                    DataTable errorTable = ds.Tables.Add("message");
                    errorTable.Columns.Add("msgHead", typeof(string));
                    errorTable.Columns.Add("msgBody", typeof(string));
                    errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                }
                // set error msg

                // close connection
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            adapt.Dispose();
            oraCon.Close();
            oraCon.Dispose();
            return ds;
        }
        public static async Task<DataSet> WebLoginAuthCheck(string _query, WebLoginModel userLogin)
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            // send to database and return dataset
            OracleConnection oraCon = new OracleConnection(connectionString);
            if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
            OracleCommand oraCommand = new OracleCommand();
            oraCommand.CommandType = CommandType.StoredProcedure;
            oraCommand.CommandText = _query;//"GetActionType";
            oraCommand.Connection = oraCon;
            oraCommand.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, userLogin.email, ParameterDirection.Input);
            oraCommand.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, userLogin.Password, ParameterDirection.Input);

            OracleParameter pResult = new OracleParameter("P_USER_DATA", OracleDbType.RefCursor, ParameterDirection.Output);
            oraCommand.Parameters.Add(pResult);
            // error code
            OracleParameter errorCode = new OracleParameter("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            errorCode.Size = 256;
            oraCommand.Parameters.Add(errorCode);

            // error msg
            OracleParameter errorMsg = new OracleParameter("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
            errorMsg.Size = 256;
            oraCommand.Parameters.Add(errorMsg);

            OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);

            try
            {
                adapt.Fill(ds, rootName);
            }
            catch (Exception exc)
            {
                return handleExceptionInDataSet(ds, new { Id = "-1", Message = exc.Message });
            }
            finally
            {
                int tcount = ds.Tables.Count;
                if (tcount == 1)
                {
                    ds.Tables[tcount - 1].TableName = "result";
                    DataTable errorTable = ds.Tables.Add("message");
                    errorTable.Columns.Add("msgHead", typeof(string));
                    errorTable.Columns.Add("msgBody", typeof(string));
                    errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                }
                if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";

                if (tcount == 0)
                {
                    ds.Tables.Add("result");
                    DataTable errorTable = ds.Tables.Add("message");
                    errorTable.Columns.Add("msgHead", typeof(string));
                    errorTable.Columns.Add("msgBody", typeof(string));
                    errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                }
                // set error msg

                // close connection
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            adapt.Dispose();
            oraCon.Close();
            oraCon.Dispose();
            return ds;
        }

        private static bool checkAuthenticatedUser(string secParm)
        {
            if ((secParm != null && secParm.Length > 0))
            {
                string[] arr = secParm.Split(',');
                int UserCode = Convert.ToInt16(arr[0].Split('/')[1]);
                int UserGroup = Convert.ToInt16(arr[1].Split('/')[1]);
                if (UserCode > 0 || UserGroup > 0)
                    return true;
            }
            return false;
        }

        public static AuthParams GetAuthenticatedUserObject(string secParm)
        {
            if (secParm != null && secParm.Length > 0)
            {
                string[] arr = secParm.Split(',');
                int UserCode = Convert.ToInt16(arr[0].Split('/')[1]);
                int UserGroup = Convert.ToInt16(arr[1].Split('/')[1]);
                int ForDebug = arr[2].Split('/')[1] != "undefined" ? Convert.ToInt16(arr[2].Split('/')[1]) : 0;
                string UserLogin = arr[3].Split('/')[1];
                string UserVCODE = arr[4].Split('/')[1];
                int UserVCurrency = arr[5].Split('/')[1] != "undefined" ? Convert.ToInt16(arr[5].Split('/')[1]) : 0;
                string UserType = arr[6].Split('/')[1];
                string UserLanguage = arr[7].Split('/')[1];
                string UserParentVCode = arr[8].Split('/')[1];
                return new AuthParams()
                {
                    UserCode = UserCode,
                    UserGroup = UserGroup,
                    ForDebug = ForDebug,
                    UserLogin = UserLogin,
                    User_Act_PH = UserVCODE,
                    UserCurrency = UserVCurrency,
                    UserType = UserType,
                    UserLanguage = UserLanguage,
                    User_Parent_V_Code = UserParentVCode
                };
            }
            return new AuthParams();
        }

        public static object WriteTranslation(List<Translation> translations, int langId)
        {
            int doneFlag = 1;

            string path = Environment.CurrentDirectory + "\\wwwroot" + "\\assets" + "\\i18n";

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (langId == 1)
                    path = path + "\\ar" + ".json";
                else
                    path = path + "\\en" + ".json";

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
                File.SetAttributes(path, FileAttributes.Normal);
                using (var writer = new StreamWriter(path, false))
                {
                    //Dictionary<String, String> dic = new Dictionary<string, String>();
                    //translations.ForEach(t =>
                    //{
                    //    dic[t.Keyword] = t.Value;
                    //});
                    Dictionary<string, Dictionary<string, String>> dic = new Dictionary<string, Dictionary<string, String>>();
                    translations.ForEach(t =>
                    {
                        Dictionary<string, String> dic2 = new Dictionary<string, String>();
                        translations.ForEach(tc =>
                        {
                            if (tc.ParentCode == t.ParentCode)
                            {
                                dic2[tc.Keyword] = tc.Value;
                            }
                        });
                        dic[t.ParentCode] = dic2;

                    });
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dic, Newtonsoft.Json.Formatting.Indented);
                    writer.Write(JSONString);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                doneFlag = 0;
            }

            var xx = new { doneFlag = doneFlag, path = path };

            return xx;
        }
        public static string GetEncryptedFileName(string authParms)
        {
            string fileName = "";
            var query = $"select  FUN_GEN_ATTNAME as FileName from dual";
            List<UpladFile> UpladFile = OracleDQ.GetData<UpladFile>(query, authParms, null);
            fileName = UpladFile[0].FILENAME.Length > 0 ? UpladFile[0].FILENAME : "";
            return fileName;
        }

        public static async Task<DataSet> ExcuteDeleteProcAsync(string procName, dynamic entity, string authParam)
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            bool isAuthorized = checkAuthenticatedUser(authParam);
            if (isAuthorized)
            {
                // send to database and return dataset
                OracleConnection oraCon = new OracleConnection(connectionString);
                if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
                OracleCommand oraCommand = new OracleCommand();
                oraCommand.CommandType = CommandType.StoredProcedure;
                oraCommand.CommandText = procName;
                oraCommand.Connection = oraCon;
                oraCommand.Parameters.Add("P_CODE", OracleDbType.Int32, entity.code, ParameterDirection.Input);
                oraCommand.Parameters.Add("P_TYPE", OracleDbType.Int32, entity.type, ParameterDirection.Input);

                // error code
                OracleParameter errorCode = new OracleParameter("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
                errorCode.Size = 256;
                oraCommand.Parameters.Add(errorCode);

                // error msg
                OracleParameter errorMsg = new OracleParameter("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
                errorMsg.Size = 256;
                oraCommand.Parameters.Add(errorMsg);

                OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);

                try
                {
                    adapt.Fill(ds, rootName);
                }
                catch (Exception ex)
                {
                    return handleExceptionInDataSet(ds, new { Id = "-1", Message = ex.Message });
                }
                finally
                {
                    int tcount = ds.Tables.Count;
                    if (tcount == 1)
                    {
                        ds.Tables[tcount - 1].TableName = "result";
                        ds.Tables.Add("message");
                    }
                    if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";

                    if (tcount == 0)
                    {
                        ds.Tables.Add("result");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { errorCode.Value.ToString(), errorMsg.Value.ToString() });
                    }
                    // set error msg

                    // close connection
                    adapt.Dispose();
                    oraCon.Close();
                    oraCon.Dispose();
                }
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            else
            {
                return handleExceptionInDataSet(ds, null);
            }
            return ds;
        }


        public static async Task<DataSet> ExcuteSyncLiveWithLocalProc(string procName, string authParam)
        {
            DataSet ds = new DataSet();
            string connectionString = getConnectionString();
            string rootName = "result";
            bool isAuthorized = checkAuthenticatedUser(authParam);
            if (isAuthorized)
            {
                // send to database and return dataset
                OracleConnection oraCon = new OracleConnection(connectionString);
                if (oraCon.State == ConnectionState.Closed) await oraCon.OpenAsync();
                OracleCommand oraCommand = new OracleCommand();
                oraCommand.CommandType = CommandType.StoredProcedure;
                oraCommand.CommandText = procName;
                oraCommand.Connection = oraCon;

                OracleDataAdapter adapt = new OracleDataAdapter(oraCommand);

                try
                {
                    adapt.Fill(ds, rootName);
                }
                catch (Exception exc)
                {
                    return handleExceptionInDataSet(ds, new { Id = "-1", Message = exc.Message });
                }
                finally
                {
                    int tcount = ds.Tables.Count;
                    if (tcount == 1)
                    {
                        ds.Tables[tcount - 1].TableName = "result";
                        //ds.Tables.Add("message");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { "1", "Done" });
                    }
                    if (tcount > 1) ds.Tables[tcount - 1].TableName = "message";

                    if (tcount == 0)
                    {
                        ds.Tables.Add("result");
                        DataTable errorTable = ds.Tables.Add("message");
                        errorTable.Columns.Add("msgHead", typeof(string));
                        errorTable.Columns.Add("msgBody", typeof(string));
                        errorTable.Rows.Add(new Object[] { "1", "Done" });
                    }
                    // set error msg

                    // close connection
                    adapt.Dispose();
                    oraCon.Close();
                    oraCon.Dispose();
                }
                adapt.Dispose();
                oraCon.Close();
                oraCon.Dispose();
            }
            else
            {
                return handleExceptionInDataSet(ds, null);
            }
            return ds;
        }
    }
}
