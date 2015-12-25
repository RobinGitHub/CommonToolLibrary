using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace SQLiteORM
{
    public class DirectSql : DbConnection
    {
        public void Execute(string sql, Func<object[], bool> perRow)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, Connection))
            {
                SQLiteDataReader reader = command.ExecuteReader();
                object[] vals = new object[reader.VisibleFieldCount];

                while (reader.Read())
                {
                    reader.GetValues(vals);
                    if (!perRow(vals))
                        break; 
                }

                reader.Dispose();
            }
        }      
    }
}
