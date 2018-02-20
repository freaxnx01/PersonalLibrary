using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data.Entity;
using Xml.Schema.Linq;
using LinqToEdmx;
using LinqToEdmx.Designer;
using LinqToEdmx.Model.Storage;

namespace EFLibrary
{
    public class EdmxHelper : LinqToEdmx.Edmx
    {
        public static Edmx Parse(DbContext dbContext)
        {
            return XTypedServices.Parse<Edmx, TEdmx>(dbContext.GetEdmx(), LinqToXsdTypeManager.Instance);
        }

        public static string[] SplitEdmxToFiles(string edmx)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(edmx);

            List<string> files = new List<string>();

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("edmx", "http://schemas.microsoft.com/ado/2008/10/edmx");
            nsmgr.AddNamespace("mapping", "http://schemas.microsoft.com/ado/2008/09/mapping/cs");
            nsmgr.AddNamespace("ssdl", "http://schemas.microsoft.com/ado/2009/02/edm/ssdl");
            nsmgr.AddNamespace("edm", "http://schemas.microsoft.com/ado/2008/09/edm");

            files.Add(SplitEdmxFile(xmlDoc, nsmgr, "/edmx:Edmx/edmx:Runtime/edmx:ConceptualModels/edm:Schema", "csdl"));
            files.Add(SplitEdmxFile(xmlDoc, nsmgr, "/edmx:Edmx/edmx:Runtime/edmx:Mappings/mapping:Mapping", "msl"));
            files.Add(SplitEdmxFile(xmlDoc, nsmgr, "/edmx:Edmx/edmx:Runtime/edmx:StorageModels/ssdl:Schema", "ssdl"));

            return files.ToArray();
        }

        private static string SplitEdmxFile(XmlDocument edmxXmlDoc, XmlNamespaceManager nsmgr, string xpath, string extension)
        {
            XmlNode mslXmlNode = edmxXmlDoc.SelectSingleNode(xpath, nsmgr);
            XmlDocument newXmlDoc = new XmlDocument();
            XmlNode importedXmlNode = newXmlDoc.ImportNode(mslXmlNode, true);
            newXmlDoc.AppendChild(importedXmlNode);
            string file = Path.ChangeExtension(Path.GetTempFileName(), extension);
            newXmlDoc.Save(file);
            return file;
        }
    }
}
