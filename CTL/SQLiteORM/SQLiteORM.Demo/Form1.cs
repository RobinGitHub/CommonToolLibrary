using SQLiteORM.Demo.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SQLiteORM.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            //using (DbConnection conn = new DbConnection())
            //{
            //    //// CREATE
            //    //SimpleTable simple = new SimpleTable() { Id = 5, Test = "Hello", When = DateTime.UtcNow };
            //    //simple.Save();
            //    //Console.WriteLine("Simple[5]: Test='" + SimpleTable.Read(5).Test + "'");

            //    //// UPDATE
            //    //simple.Test = "Updated";
            //    //simple.Save();
            //    //Console.WriteLine("Simple[5]: Test='" + SimpleTable.Read(5).Test + "'");

            //    //// DELETE
            //    //SimpleTable.Delete(5);
            //    //Console.WriteLine("Simple[5]: " + (SimpleTable.Read(5) == null ? "null" : "not null"));


            //    ////using (DbTransaction tran = DbTransaction.BeginTransaction())
            //    ////{
            //    ////    simple = new SimpleTable() { Id = 9, Test = "Hello" };
            //    ////    simple.Save();
            //    ////    tran.Commit();
            //    ////}
            //    //Console.WriteLine("Simple[9]: " + (SimpleTable.Read(9) == null ? "null" : "not null"));

            //    List<SimpleTable> simList = new List<SimpleTable>();
            //    for (int i = 2; i < 4; i++)
            //    {
            //        var item = new SimpleTable() { Id = i, Test = "Hello", When = DateTime.UtcNow };
            //        simList.Add(item);
            //    }
            //    //SimpleTable simple = new SimpleTable() { Id = 2, Test = "Hello", When = DateTime.UtcNow };
            //    //using (BaseDal<SimpleTable> adapter = BaseDal<SimpleTable>.Open())
            //    //{
            //    //    adapter.Insert(simList);
            //    //}
            //    //SimpleTable simple = new SimpleTable() { Id = 2, Test = "SSS", When = DateTime.UtcNow };
            //    using (BaseDal<SimpleTable> adapter = BaseDal<SimpleTable>.Open())
            //    {
            //        //adapter.Update(simple);
            //        //adapter.Delete(t => t.Id == 2);
            //        //int total = 0;
            //        //var model = adapter.GetList(1, 10, out total, t => t.Id == 3, t => t.Id, true);

            //        //string sql = "select * from SimpleTable";
            //        //SQLiteHelper sqlHelper = new SQLiteHelper();
            //        //DataTable dt = sqlHelper.ExecuteDataTable(sql, null);

            //    }

            //    ////--
            //    //SimpleEntityDemo();
            //    //BigListRowSelect();
            //    //ParameterizedTableNameDemo();
            //    ////     AnonymousTableDemo();
            //    //ExpressionTreeDemo();
            //    //RolesPermissionsDemo();
            //}

            //using (BaseDal<NumberTable> adapter = BaseDal<NumberTable>.Open())
            //{
            //    int total = 0;
            //    var model = adapter.GetList(1, 10, out total, t => t.Id == 3, t => t.Id, true);
            //}

            //NumberTable numberTable = new NumberTable();
            //Random rnd = new Random();
            //numberTable.Insert(new NumberTable() { Id = 2, Random = rnd.Next(200) });

            //var t = numberTable.GetModel(2);

            SimpleTable simple = new SimpleTable() { Test = "SSS", When = DateTime.UtcNow };
            Class1 c = new Class1();
            c.Insert(simple);

        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            using (TableAdapter<SimpleTable> adapter = TableAdapter<SimpleTable>.Open())
            { 

            }
        }



        //private static void BigListRowSelect()
        //{

        //    Random rnd = new Random();

        //    using (TableAdapter<NumberTable> adapter = TableAdapter<NumberTable>.Open())
        //    {

        //        Console.WriteLine("Writing 200 rows");
        //        for (int i = 0; i < 200; i++)
        //            adapter.CreateUpdate(new NumberTable() { Random = i });

        //        for (int i = 0; i < 20; i++)
        //        {
        //            Console.Write(string.Format("{0:00}: ", i));
        //            foreach (var numtab in adapter.Select().OrderBy(t => t.Random).TakePage(i, 10))
        //                Console.Write(string.Format("{0:000} ", ((NumberTable)numtab).Random));
        //            Console.WriteLine();
        //        }
        //    }
        //    Console.WriteLine();
        //}

        //private static void ExpressionTreeDemo()
        //{
        //    Console.WriteLine("Lambda to Sql (expression tree) demo.");

        //    Random rnd = new Random();
        //    using (TableAdapter<NumberTable> adapter = TableAdapter<NumberTable>.Open())
        //    {
        //        adapter.DeleteAll();

        //        Console.WriteLine("Writing 100 rows... (wait)");
        //        for (int i = 0; i < 100; i++)
        //            adapter.CreateUpdate(new NumberTable() { Random = rnd.Next(200) });

        //        Console.WriteLine("Select from RandomNumbers table, where between 10 and 30 then Order");
        //        foreach (var numtab in adapter.Select().Where(t => t.Random > 10 && t.Random < 30).OrderBy(t => t.Random))
        //            Console.WriteLine(((NumberTable)numtab).Random);
        //    }
        //    Console.WriteLine();

        //    using (TableAdapter<SimpleTable> adapter = TableAdapter<SimpleTable>.Open())
        //    {
        //        adapter.CreateUpdate(23, "this is a test", DateTime.Now);
        //        adapter.CreateUpdate(new SimpleTable() { Id = 22, Test = "my lovely test", When = DateTime.Now.AddDays(1) });
        //        adapter.CreateUpdate(24, null, DateTime.Now);

        //        Console.WriteLine("Select from SimpleTable where Id >= (some expression)");
        //        foreach (SimpleTable item in adapter.Select().Where(t => t.Id >= 2 / 30))
        //            Console.WriteLine("{0} {1} {2}", item.Id, item.Test, item.When);
        //    }
        //    Console.WriteLine();
        //}

        //private static void AnonymousTableDemo()
        //{
        //    using (AnonymousAdapter adapter = AnonymousAdapter.Open("SimpleTable"))
        //    {
        //        Console.WriteLine("Anonymous table. Read from 'SimpleTable'");
        //        foreach (var x in adapter.Select())
        //        {
        //            foreach (var column in adapter.Meta.Columns)
        //                Console.Write(column.Name + " ='" + column.GetValue(x) + "' ");
        //            Console.WriteLine("");
        //        }
        //        Console.WriteLine("");
        //    }
        //}

        //private static void ParameterizedTableNameDemo()
        //{
        //    using (TableAdapter<ParamTable> adapter = TableAdapter<ParamTable>.Open(2))
        //    {
        //        adapter.CreateUpdate(23);
        //        adapter.CreateUpdate(17);
        //        adapter.CreateUpdate(99);
        //        adapter.CreateUpdate(100);

        //        Console.WriteLine("ParamTable 2");
        //        foreach (ParamTable item in adapter.Select().Where(Where.GreaterOrEqual("Id", 99)))
        //            Console.WriteLine("{0}", item.Id);
        //    }
        //}

        //private static void SimpleEntityDemo()
        //{
        //    using (TableAdapter<SimpleTable> adapter = TableAdapter<SimpleTable>.Open())
        //    {
        //        adapter.CreateUpdate(23, "this is a test", DateTime.Now);
        //        adapter.CreateUpdate(new SimpleTable() { Id = 22, Test = "my lovely test", When = DateTime.Now.AddDays(1) });

        //        SimpleTable result = adapter.Read(23);
        //        result.Test = "updated";

        //        adapter.CreateUpdate(result);
        //        adapter.Delete(2);

        //        Console.WriteLine("Simple table");
        //        foreach (SimpleTable item in adapter.Select().Where(
        //                Where.And(
        //                    Where.LessOrEqual("[When]", DateTime.Now.AddHours(1)),
        //                    Where.LessOrEqual("Id", 23))
        //                    ))

        //            Console.WriteLine("{0} {1} {2}", item.Id, item.Test, item.When);
        //    }

        //    Console.WriteLine();
        //}

        //private static void RolesPermissionsDemo()
        //{
        //    Role admin = new Role() { Name = "Admin" };
        //    Role group = new Role() { Name = "Group" };

        //    Permission fire = new Permission() { Name = "Fire" };
        //    Permission cook = new Permission() { Name = "Cook" };
        //    Permission lick = new Permission() { Name = "Lick" };

        //    using (TableAdapter<Role> adapter = TableAdapter<Role>.Open())
        //    {
        //        adapter.DeleteAll();
        //        adapter.CreateUpdate(admin);
        //        adapter.CreateUpdate(group);
        //    }

        //    using (TableAdapter<Permission> adapter = TableAdapter<Permission>.Open())
        //    {
        //        adapter.DeleteAll();
        //        adapter.CreateUpdate(fire);
        //        adapter.CreateUpdate(cook);
        //        adapter.CreateUpdate(lick);
        //    }

        //    using (TableAdapter<RolePermissions> adapter = TableAdapter<RolePermissions>.Open())
        //    {
        //        adapter.DeleteAll();
        //        adapter.CreateUpdate(admin.Id, fire.Id);
        //        adapter.CreateUpdate(admin.Id, cook.Id);
        //        adapter.CreateUpdate(admin.Id, lick.Id);

        //        adapter.CreateUpdate(group.Id, cook.Id);
        //        adapter.CreateUpdate(group.Id, lick.Id);
        //    }

        //    using (TableAdapter<Mash<Role, RolePermissions, Permission>> adapater = TableAdapter<Mash<Role, RolePermissions, Permission>>.Open())
        //    {
        //        Console.WriteLine("Role \tPermission (many-to-many)");
        //        foreach (var mashObj in adapater.Select())
        //        {
        //            var mash = (Mash<Role, RolePermissions, Permission>)mashObj;
        //            Console.WriteLine("{0}\t{1}", mash.Table1.Name, mash.Table3.Name);
        //        }
        //        Console.WriteLine();

        //        Console.WriteLine("Roles with 'Cook' permission");
        //        foreach (var mashObj in adapater.Select().Where(Where.Equal("t3.Name", "Cook")))
        //        {
        //            var mash = (Mash<Role, RolePermissions, Permission>)mashObj;
        //            Console.WriteLine("{0}\t{1}", mash.Table1.Name, mash.Table3.Name);
        //        }
        //    }
        //}

    }
}
