﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PersonalLibrary.Misc
{
    public class XsltUtility
    {
        public static string GetOutputMethodFromXslt(string xsltPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xsltPath);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

            XmlNode xmlNode = xmlDoc.SelectSingleNode("xsl:stylesheet/xsl:output", nsmgr);
            return xmlNode.Attributes["method"].Value;
        }
    }
}
