using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteORM.Demo.Model
{
    [Table("Param_{0}")]
    public class ParamTable
    {
        [PrimaryKey]
        public long Id { get; set; }
    }
}
