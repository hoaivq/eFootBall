using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AppDao
    {
        public string UserName { get; set; }
        public long? UserId { get; set; }
        public DataTable GetTable(string TSQL, params SqlParameter[] myParamArr)
        {
            return GetTable(TSQL, myParamArr, null);
        }

        public DataTable GetTable(string TSQL, SqlParameter[] myParamArr = null, SqlConnection myConn = null)
        {
            try
            {
                if (myConn == null)
                {
                    using (SqlConnection conn = new SqlConnection(MyApp.Setting.DBConnStr))
                    {
                        return GetTable(TSQL, myParamArr, conn);
                    }
                }

                if (myConn.State != ConnectionState.Open) { myConn.Open(); }

                using (SqlDataAdapter da = new SqlDataAdapter(TSQL, myConn))
                {
                    using (DataTable dt = new DataTable("DataTable"))
                    {
                        if (myParamArr != null && myParamArr.Length > 0)
                        {
                            da.SelectCommand.Parameters.AddRange(myParamArr);
                        }
                        da.SelectCommand.CommandType = CommandType.Text;
                        da.SelectCommand.CommandTimeout = 600;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("GetTable", ex, TSQL, myParamArr);
                throw ex;
            }
            finally
            {
                if (myConn != null) { myConn.Close(); }
            }
        }

        public DataTable GetTable(string TSQL, string DBConnStr, SqlParameter[] myParamArr = null)
        {
            try
            {
                using (SqlConnection myConn = new SqlConnection(DBConnStr))
                {
                    return GetTable(TSQL, myParamArr, myConn);
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("GetTable", ex, TSQL, myParamArr);
                throw ex;
            }
        }

        public DataTable GetTableSP(string SPName, params SqlParameter[] myParamArr)
        {
            return GetTableSP(SPName, myParamArr, null);
        }

        public DataTable GetTableSP(string SPName, SqlParameter[] myParamArr = null, SqlConnection myConn = null)
        {
            try
            {
                if (myConn == null)
                {
                    using (SqlConnection conn = new SqlConnection(MyApp.Setting.DBConnStr))
                    {
                        return GetTableSP(SPName, myParamArr, conn);
                    }
                }

                if (myConn.State != ConnectionState.Open) { myConn.Open(); }

                using (SqlDataAdapter da = new SqlDataAdapter(SPName, myConn))
                {
                    using (DataTable dt = new DataTable(SPName))
                    {
                        if (myParamArr != null && myParamArr.Length > 0)
                        {
                            da.SelectCommand.Parameters.AddRange(myParamArr);
                        }
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 600;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("GetTableSP", ex, SPName, myParamArr);
                throw ex;
            }
            finally
            {
                if (myConn != null) { myConn.Close(); }
            }
        }

        public DataTable GetTableSP(string SPName, string DBConnStr, SqlParameter[] myParamArr = null)
        {
            try
            {
                using (SqlConnection myConn = new SqlConnection(DBConnStr))
                {
                    return GetTableSP(SPName, myParamArr, myConn);
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("GetTableSP", ex, SPName, myParamArr);
                throw ex;
            }
        }




        public int ExecSQL(string TSQL, SqlParameter[] myParamArr = null, SqlConnection myConn = null, SqlTransaction myTrans = null)
        {
            try
            {
                if (myConn == null)
                {
                    using (SqlConnection conn = new SqlConnection(MyApp.Setting.DBConnStr))
                    {
                        return ExecSQL(TSQL, myParamArr, conn, myTrans);
                    }
                }

                if (myConn.State != ConnectionState.Open) { myConn.Open(); }

                if (MyApp.Setting.IsGhiLog)
                {
                    StringBuilder sbLogAction = new StringBuilder(TSQL + "|");
                    if (myParamArr != null && myParamArr.Length > 0)
                    {
                        foreach (SqlParameter p in myParamArr)
                        {
                            sbLogAction.AppendLine(p.ParameterName + ": " + p.Value.ToString());
                        }
                    }

                    if (UserId.HasValue)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@UserId,GETDATE(),@LogAction)", myConn))
                        {
                            cmd.Parameters.AddWithValue("UserId", UserId);
                            cmd.Parameters.AddWithValue("LogAction", sbLogAction.ToString());
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 600;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@UserName,GETDATE(),@LogAction)", myConn))
                        {
                            cmd.Parameters.AddWithValue("UserName", UserName);
                            cmd.Parameters.AddWithValue("LogAction", sbLogAction.ToString());
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 600;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand(TSQL, myConn))
                {
                    if (myParamArr != null && myParamArr.Length > 0)
                    {
                        cmd.Parameters.AddRange(myParamArr);
                    }
                    if (myTrans != null)
                    {
                        cmd.Transaction = myTrans;
                    }
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 600;
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("ExecSQL", ex, TSQL, myParamArr);
                throw ex;
            }
            finally
            {
                if (myConn != null)
                {
                    if (myTrans == null)
                    {
                        myConn.Close();
                    }
                }
            }
        }

        public int ExecSQL(string TSQL, params SqlParameter[] myParamArr)
        {
            return ExecSQL(TSQL, myParamArr, null, null);
        }



        public int ExecSP(string SPName, SqlParameter[] myParamArr = null, SqlConnection myConn = null, SqlTransaction myTrans = null)
        {
            try
            {
                if (myConn == null)
                {
                    using (SqlConnection conn = new SqlConnection(MyApp.Setting.DBConnStr))
                    {
                        return ExecSP(SPName, myParamArr, conn, myTrans);
                    }
                }

                if (myConn.State != ConnectionState.Open) { myConn.Open(); }

                if (MyApp.Setting.IsGhiLog)
                {
                    StringBuilder sbLogAction = new StringBuilder(SPName + "|");
                    if (myParamArr != null && myParamArr.Length > 0)
                    {
                        foreach (SqlParameter p in myParamArr)
                        {
                            sbLogAction.AppendLine(p.ParameterName + ": " + p.Value.ToString());
                        }
                    }

                    if (UserId.HasValue)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@UserId,GETDATE(),@LogAction)", myConn))
                        {
                            cmd.Parameters.AddWithValue("UserId", UserId);
                            cmd.Parameters.AddWithValue("LogAction", sbLogAction.ToString());
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 600;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@UserName,GETDATE(),@LogAction)", myConn))
                        {
                            cmd.Parameters.AddWithValue("UserName", UserName);
                            cmd.Parameters.AddWithValue("LogAction", sbLogAction.ToString());
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 600;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand(SPName, myConn))
                {
                    if (myParamArr != null && myParamArr.Length > 0)
                    {
                        cmd.Parameters.AddRange(myParamArr);
                    }
                    if (myTrans != null)
                    {
                        cmd.Transaction = myTrans;
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("ExecSP", ex, SPName, myParamArr);
                throw ex;
            }
            finally
            {
                if (myConn != null)
                {
                    if (myTrans == null)
                    {
                        myConn.Close();
                    }
                }
            }
        }

        public int ExecSP(string TSQL, params SqlParameter[] myParamArr)
        {
            return ExecSP(TSQL, myParamArr, null, null);
        }


        public long ExecSQLGetId(string TSQL, SqlParameter[] myParamArr = null, SqlConnection myConn = null, SqlTransaction myTrans = null)
        {
            try
            {
                if (myConn == null)
                {
                    using (SqlConnection conn = new SqlConnection(MyApp.Setting.DBConnStr))
                    {
                        return ExecSQLGetId(TSQL, myParamArr, conn, myTrans);
                    }
                }

                if (myConn.State != ConnectionState.Open) { myConn.Open(); }

                StringBuilder sbLogAction = new StringBuilder(TSQL + "|");
                if (myParamArr != null && myParamArr.Length > 0)
                {
                    foreach (SqlParameter p in myParamArr)
                    {
                        sbLogAction.AppendLine(p.ParameterName + ": " + p.Value.ToString());
                    }
                }

                if (UserId.HasValue)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@UserId,GETDATE(),@LogAction)", myConn))
                    {
                        cmd.Parameters.AddWithValue("UserId", UserId);
                        cmd.Parameters.AddWithValue("LogAction", sbLogAction.ToString());
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600;
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@UserName,GETDATE(),@LogAction)", myConn))
                    {
                        cmd.Parameters.AddWithValue("UserName", UserName);
                        cmd.Parameters.AddWithValue("LogAction", sbLogAction.ToString());
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600;
                        cmd.ExecuteNonQuery();
                    }
                }

                TSQL = TSQL + " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(TSQL, myConn))
                {
                    if (myParamArr != null && myParamArr.Length > 0)
                    {
                        cmd.Parameters.AddRange(myParamArr);
                    }
                    if (myTrans != null)
                    {
                        cmd.Transaction = myTrans;
                    }
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 600;
                    return long.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                MyApp.Log.GhiLog("ExecSQLGetId", ex, TSQL, myParamArr);
                throw ex;
            }
            finally
            {
                if (myConn != null)
                {
                    if (myTrans == null)
                    {
                        myConn.Close();
                    }
                }
            }
        }
        public long ExecSQLGetId(string TSQL, params SqlParameter[] myParamArr)
        {
            return ExecSQLGetId(TSQL, myParamArr, null, null);
        }
    }
}
