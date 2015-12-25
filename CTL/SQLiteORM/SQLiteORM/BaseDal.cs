using SQLiteORM.Dialect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SQLiteORM
{
    public class BaseDal<T> : DbConnection, IDisposable
        where T : class, new()
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
        private string _deleteRowSql;
        private string _deleteAllSql;

        private string InsertSql { get { return _insertSql ?? (_insertSql = Actions.InsertSql(_meta, _tableName)); } }
        private string UpdateSql { get { return _updateSql ?? (_updateSql = Actions.UpdateSql(_meta, _tableName)); } }
        private string SelectRowSql { get { return _selectRowSql ?? (_selectRowSql = Actions.SelectRowSql(_meta, _tableName)); } }
        private string SelectAllSql { get { return _selectAllSql ?? (_selectAllSql = Actions.SelectAllSql(_meta, _tableName)); } }
        private string DeleteRowSql { get { return _deleteRowSql ?? (_deleteRowSql = Actions.DeleteRowSql(_meta, _tableName)); } }
        private string DeleteAllSql { get { return _deleteAllSql ?? (_deleteAllSql = Actions.DeleteSql(_meta, _tableName)); } }
        #endregion

        #endregion

        #region IDisposable
        public void Dispose()
        {

        }
        #endregion

        #region 公共方法

        #region 创建对象 实例化
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static BaseDal<T> Open(params object[] args)
        {
            BaseDal<T> adapter = Activator.CreateInstance<BaseDal<T>>();

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
        public virtual bool Insert(T model)
        {
            try
            {
                using (SQLiteCommand command = new SQLiteCommand(InsertSql, Connection))
                {
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
            bool rlt = false;
            using (SQLiteTransaction trans = Connection.BeginTransaction())
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

        public virtual bool Update(T model)
        {
            using (SQLiteCommand command = new SQLiteCommand(UpdateSql, Connection))
            {
                QueryParams(command, model);
                BroadcastToListeners(command);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(T model)
        {
            return false;
        }

        public bool Delete(Expression<Func<T, bool>> condition)
        {
            return false;
        }

        //public T GetModel(Expression<Func<T, bool>> condition)
        //{
        //    return 0;
        //}
        //public TResult GetModel<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        //{
        //    return T;
        //}

        //public int GetCount(Expression<Func<T, bool>> condition)
        //{
        //    return 0;
        //}

        //public IList GetList(Expression<Func<T, bool>> condition)
        //{
        //    return null;
        //}
        //public IList GetList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        //{
        //    return null;
        //}
        //public IList GetList<TResult>(int pageIndex, int pageSize, out int total, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition)
        //{
        //    return null;
        //}
        //public IList GetList<TKey>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> OrderBy, bool IsAsc = true)
        //{
        //    return null;
        //}
        //public IList GetList<TResult, TKey>(int pageIndex, int pageSize, out int total, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> OrderBy, bool IsAsc = true)
        //{
        //    return null;
        //}


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
        #endregion

        #endregion
    }
}
