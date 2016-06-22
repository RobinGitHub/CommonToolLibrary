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
    [Serializable]
    public abstract class TableBase<T>
        where T : class
    {
        #region Insert
        public virtual bool Insert(T model)
        {
            return Insert(null, model);
        }

        public bool Insert(SQLiteTransaction trans, T model)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.Insert(trans, model);
            }
        }

        public bool Insert(IList<T> models)
        {
            return Insert(null, models);
        }

        public bool Insert(SQLiteTransaction trans, IList<T> models)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.Insert(trans, models);
            }
        }
        #endregion

        #region Update
        public virtual bool Update(T model)
        {
            return Update(null, model);
        }
        public bool Update(SQLiteTransaction trans, T model)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.Update(trans, model);
            }
        } 
        #endregion

        #region Delete
        public virtual bool Delete(int id)
        {
            return Delete(null, id);
        }

        public bool Delete(SQLiteTransaction trans, int id)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.Delete(trans, id);
            }
        }

        public bool Delete(Expression<Func<T, bool>> condition)
        {
            return Delete(null, condition);
        }

        public bool Delete(SQLiteTransaction trans, Expression<Func<T, bool>> condition)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.Delete(trans, condition);
            }
        } 
        #endregion

        #region GetModel
        public T GetModel(int id)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.GetModel(id);
            }
        } 
        #endregion

        #region GetList
        public IList GetList(Expression<Func<T, bool>> condition)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.GetList(condition);
            }
        }

        public IList GetList<TKey>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.GetList(pageIndex, pageSize, out total, condition, orderBy, isAsc);
            }
        }

        public DataTable GetDataTable(Expression<Func<T, bool>> condition)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.GetDataTable(condition);
            }
        }

        public DataTable GetDataTable<TKey>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.GetDataTable(pageIndex, pageSize, out total, condition, orderBy, isAsc);
            }
        }
        #endregion
        
        #region SQLiteHelper
        SQLiteHelper sqlHelper = new SQLiteHelper();

        public int ExecuteNonQuery(string sql)
        {
            return sqlHelper.ExecuteNonQuery(sql);
        }
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            return sqlHelper.ExecuteNonQuery(sql, parameters);
        }

        public int ExecuteNonQuery(List<CommandInfo> cmdList)
        {
            return sqlHelper.ExecuteNonQuery(cmdList);
        }

        public SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
        {
            return sqlHelper.ExecuteReader(sql, parameters);
        }

        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            return sqlHelper.ExecuteDataTable(sql, parameters);
        }

        public Object ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            return sqlHelper.ExecuteScalar(sql, parameters);
        } 
        #endregion
    }
}
