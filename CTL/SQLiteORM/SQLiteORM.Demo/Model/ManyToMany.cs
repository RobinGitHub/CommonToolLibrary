using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteORM.Demo.Model
{
    [Table]
    public class Role
    {
        [PrimaryKey(true)]
        public long Id { get; set; }
        [Field]
        public string Name { get; set; }
    }

    [Table]
    public class RolePermissions
    {
        [ForeignKey(typeof(Role))]
        public long RoleId { get; set; }

        [ForeignKey(typeof(Permission))]
        public long PermissionId { get; set; }
    }

    [Table]
    public class Permission
    {
        [PrimaryKey(true)]
        public long Id { get; set; }

        [Field]
        public string Name { get; set; }
    }

}
