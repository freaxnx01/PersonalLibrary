using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Collections;
using System.Data.Entity;
using System.Reflection;
using System.Data;
using System.Windows.Forms;

using SqlServerCeEFLibrary;
using SqlServerCe.Test.EntitySplitting;

namespace SqlServerCe.Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new MainForm());
            return;

            CreateDatabase<ModelEntitySplittingContext>();
            using (ModelEntitySplittingContext ctx = new ModelEntitySplittingContext())
            {
                Article article = new Article();

                article.ArticleNo = "9240";

                Image img = Image.FromFile("waschmaschine.jpg");
                article.Photo = img.ToByteArray();
                //article.Photo = ToLibrary.ConvertImageToByteArray("waschmaschine.jpg");

                article.Text = File.ReadAllText("text.txt");

                //RichTextBox rtfBox = new RichTextBox();
                //rtfBox.Text = File.ReadAllText("text.txt");
                //rtfBox.SelectAll();
                //rtfBox.SelectionColor = Color.OrangeRed;
                //article.TextFormatted = rtfBox.Rtf;
                article.TextFormatted = File.ReadAllText("RtfMitBild.rtf");

                ctx.Articles.Add(article);

                ctx.SaveChanges();
            }



            Stopwatch stopwatch = Stopwatch.StartNew();

            //ContextTest();
            //SqlCeBulkTest();
            //EFCompactBulkTest();
            
            Console.WriteLine(stopwatch.Elapsed.TotalSeconds.ToString());
            Console.ReadLine();
        }

        private static void EFCompactBulkTest()
        {
            CreateDatabase();
            using (MyContext ctx = new MyContext())
            {
                new Bulk().BulkInsert(ctx, GetTestData());
            }
        }

        private static void CreateDatabase()
        {
            CreateDatabase<MyContext>();
        }

        private static void CreateDatabase<T>() where T : DbContext, new()
        {
            T ctx = new T();
            if (ctx.Database.Exists())
            {
                ctx.Database.Delete();
            }
            ctx.Database.CreateIfNotExists();
        }

        private static List<Item> GetTestData()
        {
            List<Item> list = new List<Item>();

            for (int i = 0; i < 10000; i++)
            {
                Item item = new Item();
                item.ItemNo = Guid.NewGuid().ToString();
                item.IsActive = true;
                item.Inventory = 200 * i;
                item.PreisEinheit = i + 1;
                item.ShortDescription = "Short Description " + i.ToString();
                item.ModifiedOn = DateTime.Today;
                list.Add(item);
            }

            return list;
        }

        //private static void ContextTest()
        //{
        //    CreateDatabase();
        //    MyContext ctx = new MyContext();

        //    int i = 0;
        //    foreach (Item item in GetTestData())
        //    {
        //        ctx.Items.Add(item);

        //        if (i % 200 == 0)
        //        {
        //            Console.WriteLine(i.ToString());
        //            ctx.SaveChanges();
        //            ctx = new MyContext();
        //        }
        //        i++;
        //    }

        //    ctx.SaveChanges();
        //}

        //private static void SqlCeBulkTest()
        //{
        //    CreateDatabase();

        //    SqlCeConnection conn = new SqlCeConnection("Data Source=ConsoleApplication1.MyContext.sdf");
        //    conn.Open();

        //    SqlCeCommand cmd = conn.CreateCommand();
        //    cmd.CommandType = System.Data.CommandType.TableDirect;
        //    cmd.CommandText = "ARTIKEL";

        //    SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Scrollable);

        //    foreach (Item item in GetTestData())
        //    {
        //        SqlCeUpdatableRecord rec = rs.CreateRecord();
        //        rec.SetValue(1, item.ItemNo);
        //        rec.SetValue(3, false);
        //        rec.SetValue(4, 0);
        //        rec.SetValue(5, 0);
        //        rec.SetValue(6, DateTime.Today);
        //        rs.Insert(rec);
        //    }
        //}

        //private static void SqlCeBulkTestMsdn()
        //{
        //    SqlCeConnection conn = null;

        //    try
        //    {
        //        File.Delete("Test.sdf");

        //        SqlCeEngine engine = new SqlCeEngine("Data Source=Test.sdf");
        //        engine.CreateDatabase();

        //        conn = new SqlCeConnection("Data Source=Test.sdf");
        //        conn.Open();

        //        SqlCeCommand cmd = conn.CreateCommand();
        //        cmd.CommandText = "CREATE TABLE myTable (col1 INT, col2 MONEY, col3 NVARCHAR(200))";
        //        cmd.ExecuteNonQuery();

        //        cmd.CommandText = "SELECT * FROM myTable";

        //        SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable |
        //            ResultSetOptions.Scrollable);

        //        for (int i = 0; i < 10000; i++)
        //        {
        //            SqlCeUpdatableRecord rec = rs.CreateRecord();
        //            rec.SetInt32(0, 34);
        //            rec.SetDecimal(1, (decimal)44.66);
        //            rec.SetString(2, "Sample text" + i.ToString());
        //            rs.Insert(rec);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
    }
}
