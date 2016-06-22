using SQLiteORM.Dialect;
using SQLiteORM.ExpressionTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SQLiteORM
{
    public class TableAdapter<T> : DbConnection, IDisposable
        where T : class
    {

        #region 私有属性
        protected TableMeta _meta;
        protected string _tableName;
        private bool _isMultiTableJoin = false;

        private SQLiteHelper sqlHelper = new SQLiteHelper();

        #region 查询语句
        private string _insertSql;
        private string _updateSql;
        private string _selectRowSql;
        private string _selectAllSql;
        private string _selectRowCountSql;
        private string _deleteRowSql;
        private string _deleteAllSql;
        
        private string InsertSql { get { return _insertSql ?? (_insertSql = Actions.InsertSql(_meta, _tableName)); } }
        private string UpdateSql { get { return _updateSql ?? (_updateSql = Actions.UpdateSql(_meta, _tableName)); } }
        private string SelectRowSql { get { return _selectRowSql ?? (_selectRowSql = Actions.SelectRowSql(_meta, _tableName)); } }
        private string SelectAllSql { get { return _selectAllSql ?? (_selectAllSql = Actions.SelectAllSql(_meta, _tableName)); } }
        private string SelectRowCountSql { get { return _selectRowCountSql ?? (_selectRowCountSql = Actions.SelectRowCountSql(_meta, _tableName)); } }
        private string DeleteRowSql { get { return _deleteRowSql ?? (_deleteRowSql = Actions.DeleteRowSql(_meta, _tableName)); } }
        private string DeleteAllSql { get { return _deleteAllSql ?? (_deleteAllSql = Actions.DeleteSql(_meta, _tableName)); } }
        #endregion

        #endregion

        #region 公共方法

        #region 创建对象 实例化
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TableAdapter<T> Open(params object[] args)
        {
            TableAdapter<T> adapter = Activator.CreateInstance<TableAdapter<T>>();

            if (typeof(IMash).IsAssignableFrom(typeof(T)))
            {
                adapter._isMultiTableJoin = true;
                return adapter;
            }

            TableMeta meta = TableMeta.Get(typeof(T));
            if (meta is TableMetaInvalid)
                throw new ArgumentException(typeof(T).FullName + " is invalid because " + string.Join(" and ", ((TableMetaInvalid)meta).Reasons.ToArray()));

            if (args.Length != meta.TableParamCount)
                throw new ArgumentException("Please specify " + meta.TableParamCount + " parameters");

            string tableName = meta.CreateTableName(args);

            if (meta.CheckExists)
            {
                adapter.CreateTable(meta, tableName);
                // not a dynamicly created table, so don't need to check it exists now it's created
                meta.CheckExists = (meta.TableParamCount > 0);
            }

            adapter._meta = meta;
            adapter._tableName = tableName;

            return adapter;
        }
        #endregion

        #region 两个对象是否相同
        /// <summary>
        /// 两个对象是否相同
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public bool AreSame(T one, T two)
        {
            foreach (var tableColumn in _meta.Columns)
            {
                if (tableColumn.IsCaseInsensitive && string.Compare((string)tableColumn.GetValue(one), (string)tableColumn.GetValue(two), true) != 0)
                {
                    return false;
                }
                else
                {
                    if (tableColumn.IsFloatingPoint &&
                        Math.Abs(Convert.ToDouble(tableColumn.GetValue(one)) -
                                  Convert.ToDouble(tableColumn.GetValue(two))) > 9E+10)
                        return false;
                    else
                    {
                        if (!tableColumn.GetValue(one).Equals(tableColumn.GetValue(two)))
                            return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region Insert
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(T model)
        {
            return Insert(null, model);
        }

        public bool Insert(SQLiteTransaction trans, T model)
        {
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(InsertSql, Connection))
                {
                    if (trans != null)
                        command.Transaction = trans;
                    QueryParams(command, model);

                    BroadcastToListeners(command);

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
        }

        public bool Insert(IList<T> models)
        {
            return Insert(null, models);
        }

        public bool Insert(SQLiteTransaction trans, IList<T> models)
        {
            bool rlt = false;

            if (trans == null)
                trans = Connection.BeginTransaction();
            using (trans)
            {
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand(InsertSql, Connection))
                    {
                        foreach (T item in models)
                        {
                            QueryParams(command, item);
                            BroadcastToListeners(command);
                            command.ExecuteNonQuery();
                        }
                    }
                    rlt = true;
                    trans.Commit();
                }
                catch (SQLiteException ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return rlt;
        }
        #endregion

        #region Update
        public bool Update(T model)
        {
            return Update(null, model);
        }
        public bool Update(SQLiteTransaction trans, T model)
        {
            using (SQLiteCommand command = new SQLiteCommand(UpdateSql, Connection))
            {
                if (trans != null)
                    command.Transaction = trans;
                QueryParams(command, model);
                BroadcastToListeners(command);
                return command.ExecuteNonQuery() > 0;
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            return Delete(null, id);
        }

        public bool Delete(SQLiteTransaction trans, int id)
        {
            using (SQLiteCommand command = new SQLiteCommand(DeleteRowSql, Connection))
            {
                if (trans != null)
                    command.Transaction = trans;
                QueryParams(command, false, id);
                BroadcastToListeners(command);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(Expression<Func<T, bool>> condition)
        {
            return Delete(null, condition);
        }

        public bool Delete(SQLiteTransaction trans, Expression<Func<T, bool>> condition)
        {
            using (SQLiteCommand command = new SQLiteCommand(Connection))
            {
                if (trans != null)
                    command.Transaction = trans;
                command.CommandText = DeleteAllSql;
                QueryParams(command, condition);
                BroadcastToListeners(command);
                return command.ExecuteNonQuery() > 0;
            }
        }
        #endregion

        #region GetModel
        public T GetModel(int id)
        {
            using (SQLiteCommand command = new SQLiteCommand(SelectRowSql, Connection))
            {
                QueryParams(command, false, id);
                BroadcastToListeners(command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return (T)ReadRow(reader, _meta);
                }
            }
            return null;
        }
        #endregion

        #region GetList
        public IList GetList(Expression<Func<T, bool>> condition)
        {
            List<T> _results = new List<T>();
            using (SQLiteCommand command = new SQLiteCommand(SelectAllSql, Connection))
            {
                QueryParams(command, condition);
                BroadcastToListeners(command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _results.Add((T)ReadRow(reader, _meta));
                }
            }
            return _results;
        }

        public IList GetList<TKey>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            List<T> _results = new List<T>();

            using (SQLiteCommand command = new SQLiteCommand(Connection))
            {
                command.CommandText = SelectRowCountSql;
                QueryParams(command, condition);
                BroadcastToListeners(command);
                total = Convert.ToInt32(command.ExecuteScalar());

                command.CommandText = SelectAllSql + BuildOrderBy(orderBy, isAsc) + BuildLimit(pageIndex, pageSize);
                BroadcastToListeners(command);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _results.Add((T)ReadRow(reader, _meta));
                }
            }
            return _results;
        }

        public DataTable GetDataTable(Expression<Func<T, bool>> condition)
        {
            using (SQLiteCommand command = new SQLiteCommand(SelectAllSql, Connection))
            {
                QueryParams(command, condition);
                BroadcastToListeners(command);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable data = new DataTable();
                data.TableName = "table1";
                adapter.Fill(data);
                return data;
            }
        }

        public DataTable GetDataTable<TKey>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            using (SQLiteCommand command = new SQLiteCommand(Connection))
            {
                command.CommandText = SelectRowCountSql;
                QueryParams(command, condition);
                BroadcastToListeners(command);
                total = Convert.ToInt32(command.ExecuteScalar());

                command.CommandText = SelectAllSql + BuildOrderBy(orderBy, isAsc) + BuildLimit(pageIndex, pageSize);
                BroadcastToListeners(command);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable data = new DataTable();
                data.TableName = "table1";
                adapter.Fill(data);
                return data;
            }
        }
        #endregion

        #endregion

        #region 私有方法

        #region 创建表
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="tableMeta"></param>
        /// <param name="tableName"></param>
        private void CreateTable(TableMeta tableMeta, string tableName)
        {
            sqlHelper.ExecuteNonQuery(Actions.CreateTable(tableMeta, tableName), null);
        }
        #endregion

        #region 组织参数
        /// <summary>
        /// 组织参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="insertOp"></param>
        /// <param name="args"></param>
        private void QueryParams(SQLiteCommand command, T instance)
        {
            for (int i = 0; i < _meta.Columns.Count; i++)
                if (_meta.Columns[i].PrimaryKey && _meta.Columns[i].IsDefaultValue(instance))
                    command.Parameters.Add(_meta.Columns[i].ParamName, _meta.Columns[i].DbType).Value = null;
                else
                    command.Parameters.Add(_meta.Columns[i].ParamName, _meta.Columns[i].DbType).Value = _meta.Columns[i].GetValue(instance);
        }


        private void QueryParams(SQLiteCommand command, bool insertOp, params object[] args)
        {
            if (_isMultiTableJoin)
                throw new NotSupportedException();

            if (args.Length > _meta.Columns.Count)
                throw new Exception("Too many parameters");

            int argIndex = 0;
            for (int paramIndex = 0; paramIndex < _meta.Columns.Count && argIndex < args.Length; paramIndex++)
            {
                if (_meta.Columns[paramIndex].AutoIncrement && insertOp)
                {
                    command.Parameters.Add(_meta.Columns[paramIndex].ParamName, _meta.Columns[paramIndex].DbType).Value = null;
                }
                else
                {
                    command.Parameters.Add(_meta.Columns[paramIndex].ParamName, _meta.Columns[paramIndex].DbType).Value = args[argIndex];
                    argIndex++;
                }
            }
        }

        private void QueryParams(SQLiteCommand command, Expression<Func<T, bool>> condition)
        {
            var t = (new WhereExpressionVisitor<T>()).ConvertToWhere(condition);
            string where = t.Build(command);
            command.CommandText += " " + where;
        }
        #endregion

        #region 转换成对象 ReadRow
        private object ReadRow(IDataRecord reader, TableMeta meta)
        {
            object instance = Activator.CreateInstance<T>();

            for (int i = 0; i < meta.Columns.Count; i++)
                if (Convert.IsDBNull(reader[i]))
                    meta.Columns[i].SetValue(instance, null);
                else if (typeof(Enum).IsAssignableFrom(meta.Columns[i].Type))
                    meta.Columns[i].SetValue(instance, Enum.Parse(meta.Columns[i].Type, (string)reader[i]));
                else
                    meta.Columns[i].SetValue(instance, reader[i]);

            return instance;
        }
        #endregion

        #region 排序
        /// <summary>
        /// 排序
        /// 返回SQL语句
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        private string BuildOrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            if (orderBy == null)
                return string.Empty;
            var ex = ((MemberExpression)orderBy.Body).Member;
            var colmn = _meta.Columns.First(col => col.FieldName == ex.Name);
            StringBuilder orderBySb = new StringBuilder();
            orderBySb.Append(" ORDER BY " + colmn.FieldName + (isAsc ? " ASC" : " DESC"));
            return orderBySb.ToString();
        }

        #endregion

        #region 分页
        /// <summary>
        /// 分页
        /// 返回SQL语句
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private string BuildLimit(int pageIndex, int pageSize)
        {
            return string.Format(" LIMIT {0}, {1}", (pageIndex - 1) * pageSize, pageSize);
        }
        #endregion
        #endregion
    }
}
