using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteORM.Demo.Model
{
    [Table]
    public class NumberTable
    {
        [PrimaryKey(true)]
        public long Id { get; set; }

        [Field(Name = "SomeRandom")]
        public int Random { get; set; }
    }
}
