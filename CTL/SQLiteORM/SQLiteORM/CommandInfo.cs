using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace SQLiteORM
{
    /// <summary>
    /// 执行类型
    /// </summary>
    public enum EffentNextType
    { 
        /// <summary>
        /// 对其他语句无任何影响 
        /// </summary>
        None,
        /// <summary>
        /// 当前语句影响到的行数必须大于0，否则回滚事务
        /// </summary>
        ExecuteEffectRows
    }


    public class CommandInfo
    {
        public string CommandText;
        public SQLiteParameter [] Parameters;
        public EffentNextType EffentNextType = EffentNextType.None;
        public CommandInfo()
        { }
        /// <summary>
        /// 执行多条Sql语句组织参数
        /// </summary>
        /// <param name="sqlText">sql语句</param>
        public CommandInfo(string sqlText)
        {
            this.CommandText = sqlText;
        }
        /// <summary>
        /// 执行多条Sql语句组织参数
        /// </summary>
        /// <param name="sqlText">sql语句</param>
        /// <param name="para">参数</param>
        public CommandInfo(string sqlText, SQLiteParameter[] para)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
        }
        /// <summary>
        /// 执行多条Sql语句组织参数
        /// </summary>
        /// <param name="sqlText">sql</param>
        /// <param name="para">参数</param>
        /// <param name="type">执行类型</param>
        public CommandInfo(string sqlText, SQLiteParameter[] para, EffentNextType type)
        {
            this.CommandText = sqlText;
            this.Parameters = para;
            this.EffentNextType = type;
        }


    }
}
