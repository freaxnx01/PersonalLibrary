using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Metadata.Edm;

namespace EFLibrary
{
    public class MigrationHistory
    {
        public string MigrationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte[] Model { get; set; }
        public string ProductVersion { get; set; }

        public string Edmx
        {
            get
            {
                return Encoding.UTF8.GetString(EdmxAsByteArray);
            }
        }

        public byte[] EdmxAsByteArray
        {
            get
            {
                return PersonalLibrary.Misc.Compression.Decompress(Model);
            }
        }


        public MetadataWorkspace MetadataWorkspace
        {
            get
            {
                return Metadata.FromEdmx(EdmxAsByteArray);
            }
        }

        public static List<MigrationHistory> GetMigrationHistory(DbContext dbContext)
        {
            return dbContext.Database.SqlQuery<MigrationHistory>("SELECT * FROM __MigrationHistory").ToList();
        }
    }
}
