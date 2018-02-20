using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;

using SharpSvn;

namespace SharpSvnLib
{
    public class SharpSvnHelper : IDisposable
    {
        private SvnClient _client = null;

        public SharpSvnHelper()
        {
            _client = new SvnClient();
        }

        public SvnClient Client
        {
            get
            {
                return _client;
            }
        }

        public string GetFileContent(string svnUri, long revision)
        {
            string content = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                _client.Write(Uri.EscapeUriString(svnUri), ms, new SvnWriteArgs() { Revision = revision });
                content = Encoding.UTF8.GetString(ms.GetBuffer());
            }

            return content;
        }

        public XmlDocument GetFileContentAsXmlDocument(string svnUri, long revision)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(GetFileContent(svnUri, revision).Trim().Trim(new char[] { '\0' }));
            return xmlDoc;
        }

        public Collection<SvnListEventArgs> GetListRecursive(string svnUri)
        {
            Collection<SvnListEventArgs> list = null;
            bool gotList = _client.GetList(svnUri, new SvnListArgs() { Depth = SvnDepth.Infinity, RetrieveEntries = SvnDirEntryItems.AllFieldsV15 }, out list);
            //bool gotList = _client.GetList(svnUri, new SvnListArgs() { Depth = SvnDepth.Infinity, EntryItems = SvnDirEntryItems.AllFieldsV15 }, out list);
            return list;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _client = null;
        }

        #endregion
    }
}
