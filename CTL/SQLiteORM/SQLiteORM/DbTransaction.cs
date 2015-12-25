using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace SQLiteORM
{
    //public enum SqliteTransactionType { Deferred, Immediate, Exclusive }

    //public class DbTransaction : DbConnection, IDisposable
    //{
    //    private bool _rollback = true;

    //    public static DbTransaction BeginTransaction(SqliteTransactionType type = SqliteTransactionType.Deferred)
    //    {
    //        DbTransaction tran = new DbTransaction();

    //        BroadcastToListeners("Begin Transaction " + type.ToString());

    //        using (SQLiteCommand command = new SQLiteCommand("Begin " + type.ToString(), tran.Connection))
    //            command.ExecuteNonQuery();

    //        return tran;
    //    }

    //    public void Commit()
    //    {
    //        _rollback = false;
    //        BroadcastToListeners("Commit");

    //        using (SQLiteCommand command = new SQLiteCommand("Commit", Connection))
    //            command.ExecuteNonQuery();
    //    }

    //    public new void Dispose()
    //    {
    //        // Failure to commit will result in rollback
    //        if (_rollback)
    //        {
    //            BroadcastToListeners("Rollback");
    //            using (SQLiteCommand command = new SQLiteCommand("Rollback", Connection))
    //                command.ExecuteNonQuery();
    //        }

    //        base.Dispose();
    //    }

    //}
}
