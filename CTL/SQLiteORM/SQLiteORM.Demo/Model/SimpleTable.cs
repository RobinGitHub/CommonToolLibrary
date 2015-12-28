using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteORM.Demo.Model
{
    [Table]
    public class SimpleTable
    {
        [PrimaryKey]
        public long Id { get; set; }

        [Field]
        public string Test { get; set; }

        [Field]
        public DateTime When { get; set; }
    }
}
