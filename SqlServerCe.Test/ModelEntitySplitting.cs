using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlServerCe.Test.EntitySplitting
{
    public class ModelEntitySplittingContext : DbContext
    {
        protected override bool ShouldValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry)
        {
            // Required to prevent bug - http://stackoverflow.com/questions/5737733
            // Item.Photo
            if (entityEntry.Entity is Article)
            {
                return false;
            }

            return base.ShouldValidateEntity(entityEntry);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Entity Splitting (Entität über mehrere Tabellen speichern)
            // Data Annotations scheint keine Lösung für Entity Splitting zu haben, daher wird Fluent API genutzt
            // Kann Data Annotations und Fluent API gemischt werden?
            modelBuilder.Entity<Article>()
                .Map(m =>
                {
                    m.Properties(p => new { p.ArticleNo });
                    m.ToTable("ARTIKEL");
                    m.Properties(p => new { p.ShortDescription });
                    m.ToTable("ARTIKEL");
                })
                .Map(m =>
                {
                    m.Properties(p => new { p.Photo });
                    m.ToTable("ARTIKELFOTO");
                })
                .Map(m =>
                {
                    m.Properties(p => new { p.Text });
                    m.ToTable("ARTIKELTEXT");
                    m.Properties(p => new { p.TextFormatted });
                    m.ToTable("ARTIKELTEXT");
                });
        }

        public DbSet<Article> Articles { get; set; }
    }

    public class EntityBase
    {
        [Column("OBJECTID")]
        public int Id { get; set; }
    }

    public class Article : EntityBase
    {
        [Column("ARTNR")]
        public string ArticleNo { get; set; }

        [Column("KURZBEZ")]
        public string ShortDescription { get; set; }

        // Required to force Code First to create a ntext column, not a nvarchar(n)
        [MaxLength]
        public string Text { get; set; }

        // Required to force Code First to create a ntext column, not a nvarchar(n)
        [MaxLength]
        public string TextFormatted { get; set; }

        // Required to force Code First to create an image column, not a binary(n)
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }
    }
}
