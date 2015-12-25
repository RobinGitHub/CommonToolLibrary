using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SQLiteORM
{
    public class SQLiteHelper : DbConnection
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }
        /// <summary>  
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。  
        /// </summary>  
        /// <param name="sql">要执行的增删改的SQL语句</param>  
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(Connection))
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        affectedRows = command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="cmdList"></param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(List<CommandInfo> cmdList)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Connection))
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            int count = 0;
                            foreach (CommandInfo com in cmdList)
                            {
                                string cmdText = com.CommandText;
                                SQLiteParameter[] cmdParms = (SQLiteParameter[])com.Parameters;
                                PrepareCommand(command, connection, trans, cmdText, cmdParms);
                                int val = command.ExecuteNonQuery();
                                count += val;
                                //当前影响的行数必须>0
                                if (com.EffentNextType == EffentNextType.ExecuteEffectRows && val == 0)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                command.Parameters.Clear();
                            }
                            trans.Commit();
                            return count;
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>  
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例  
        /// </summary>  
        /// <param name="sql">要执行的查询语句</param>  
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Connection))
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        /// <summary>  
        /// 执行一个查询语句，返回一个包含查询结果的DataTable  
        /// </summary>  
        /// <param name="sql">要执行的查询语句</param>  
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Connection))
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                        DataTable data = new DataTable();
                        adapter.Fill(data);
                        return data;
                    }
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }

        }
        /// <summary>  
        /// 执行一个查询语句，返回查询结果的第一行第一列  
        /// </summary>  
        /// <param name="sql">要执行的查询语句</param>  
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>  
        /// <returns></returns>  
        public Object ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Connection))
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        return command.ExecuteScalar(CommandBehavior.CloseConnection);
                    }
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        /// <summary>  
        /// 查询数据库中的所有数据类型信息  
        /// </summary>  
        /// <returns></returns>  
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(Connection))
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
                try
                {
                    DataTable data = connection.GetSchema("TABLES");
                    connection.Close();
                    //foreach (DataColumn column in data.Columns)  
                    //{  
                    //    Console.WriteLine(column.ColumnName);  
                    //}  
                    return data;
                }
                catch (SQLiteException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        /// <summary>
        /// 分析需要执行的命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParms">参数</param>
        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                {
                    if ((parm.Direction == ParameterDirection.InputOutput || parm.Direction == ParameterDirection.Input) && parm.Value == null)
                    {
                        parm.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}
