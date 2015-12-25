using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteORM.Demo.Model
{
    [Serializable]
    public abstract class TableBase<T>
        where T : class, new()
    {
        public virtual void Save()
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                adapter.CreateUpdate(this);
            }
        }

        public static void Do(Action<TableAdapter<T>> action)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                action(adapter);
            }
        }

        public static void Delete(params object[] args)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                adapter.Delete(args);
            }
        }

        public static T Read(params object[] args)
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return adapter.Read(args);
            }
        }

        public static TReturn Get<TReturn>(Func<TableAdapter<T>, TReturn> action)
            where TReturn : class
        {
            using (TableAdapter<T> adapter = TableAdapter<T>.Open())
            {
                return action(adapter);
            }
        }



    }
}
