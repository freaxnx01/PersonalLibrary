using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlServerCe.Test
{
    public class MyContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
    }

    public class EntityBase
    {
        [Column("OBJECTID")]
        public int Id { get; set; }
    }

    [Table("ARTIKEL")]
    public class Item : EntityBase
    {
        [Column("ARTNR")]
        public string ItemNo { get; set; }

        [Column("KURZBEZ")]
        public string ShortDescription { get; set; }

        [Column("AKTIV")]
        public bool IsActive { get; set; }

        [Column("PRSEINH")]
        public int PreisEinheit { get; set; }

        [Column("MENGE")]
        public double Inventory { get; set; }

        [Column("GEAENDERT")]
        public DateTime ModifiedOn { get; set; }
    }
}
