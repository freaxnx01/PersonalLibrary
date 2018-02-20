using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlServerCeEFLibrary.Test
{
    public class MyContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Author>().HasMany(a => a.Posts).WithMany(p => p.Authors).Map(m =>
            //{
            //    m.MapLeftKey("AuthorId");
            //    m.MapRightKey("PostId");
            //    m.ToTable("T_POSTAUTHOR");
            //});

            base.OnModelCreating(modelBuilder);
        }
    }

    public class EntityBase
    {
        [Column("OBJECTID")]
        public int Id { get; set; }
    }

    [Table("T_BLOG")]
    public class Blog : EntityBase
    {
        public Blog()
        {
            Posts = new List<Post>();
            CreatedOn = DateTime.Now;
            IsActive = true;
        }

        [Column("BlogName")]
        public string Name { get; set; }

        [Column("Besitzer")]
        public virtual Person Owner { get; set; }

        public virtual List<Post> Posts { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string Content { get; set; }

        [NotMapped]
        public List<string> Strings { get; set; }

        [Column("ErstelltAm")]
        public DateTime CreatedOn { get; set; }

        [Column("ErstelltVon")]
        public virtual Person CreatedBy { get; set; }

        [Column("AktivJN")]
        public bool IsActive { get; set; }
    }

    [Table("T_POST")]
    public class Post : EntityBase
    {
        public Post()
        {
            Authors = new List<Author>();
        }

        [Column("Parent")]
        public virtual Blog Blog { get; set; }

        [Column("Datum")]
        public DateTime EntryDate { get; set; }

        [Column("Beitrag")]
        public string Text { get; set; }

        [Column("Autor")]
        public virtual List<Author> Authors { get; set; }
    }

    [Table("T_AUTHOR")]
    public class Author : EntityBase
    {
        [Column("Vorname")]
        public string FirstName { get; set; }

        [Column("Nachname")]
        public string LastName { get; set; }

        //public virtual List<Post> Posts { get; set; }
    }

    // Discriminator
    //public class Author : Person
    //{
    //    public virtual List<Post> Posts { get; set; }
    //}

    [Table("T_PERSON")]
    public class Person : EntityBase
    {
        [Column("Vorname")]
        public string FirstName { get; set; }

        [Column("Nachname")]
        public string LastName { get; set; }
    }
}
