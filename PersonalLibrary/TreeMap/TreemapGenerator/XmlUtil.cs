using System;
using System.Diagnostics;
using System.Xml;
namespace Microsoft.Research.CommunityTechnologies.XmlLib
{
	internal class XmlUtil
	{
		private XmlUtil()
		{
		}
		public static XmlNode AppendNewNode(XmlNode oParentNode, string sChildName)
		{
			Debug.Assert(oParentNode != null);
			Debug.Assert(sChildName != "");
			XmlDocument xmlDocument = oParentNode.OwnerDocument;
			if (xmlDocument == null)
			{
				xmlDocument = (XmlDocument)oParentNode;
			}
			return oParentNode.AppendChild(xmlDocument.CreateElement(sChildName));
		}
		public static XmlNode AppendNewNode(XmlNode oParentNode, string sChildName, string sInnerText)
		{
			Debug.Assert(oParentNode != null);
			Debug.Assert(sChildName != "");
			Debug.Assert(sInnerText != null);
			XmlNode xmlNode = XmlUtil.AppendNewNode(oParentNode, sChildName);
			xmlNode.InnerText = sInnerText;
			return xmlNode;
		}
		public static XmlNode SelectRequiredSingleNode(XmlNode oNode, string sXPath)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sXPath != "");
			XmlNode xmlNode = oNode.SelectSingleNode(sXPath);
			if (xmlNode == null)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"XmlUtil.SelectRequiredSingleNode: A ", 
					oNode.Name, 
					" node is missing a required descendent node.  The XPath is \"", 
					sXPath, 
					"\"."
				}));
			}
			return xmlNode;
		}
		[Conditional("DEBUG")]
		public static void CheckNodeName(XmlNode oNode, string sExpectedName)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(oNode.Name == sExpectedName);
		}
		public static bool GetInnerText(XmlNode oNode, bool bRequired, out string sInnerText)
		{
			Debug.Assert(oNode != null);
			sInnerText = oNode.InnerText;
			bool result;
			if (sInnerText == null || sInnerText.Trim().Length == 0)
			{
				if (bRequired)
				{
					throw new InvalidOperationException("A " + oNode.Name + " node is missing required inner text.");
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
		public static bool GetAttribute(XmlNode oNode, string sName, bool bRequired, out string sValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			sValue = ((XmlElement)oNode).GetAttribute(sName);
			bool result;
			if (sValue == "")
			{
				if (bRequired)
				{
					throw new InvalidOperationException(string.Concat(new string[]
					{
						"A ", 
						oNode.Name, 
						" node is missing a required ", 
						sName, 
						" attribute."
					}));
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
		public static bool GetInt32Attribute(XmlNode oNode, string sName, bool bRequired, out int iValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			string text;
			bool result;
			if (!XmlUtil.GetAttribute(oNode, sName, bRequired, out text))
			{
				iValue = -2147483648;
				result = false;
			}
			else
			{
				try
				{
					iValue = int.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("XmlUtil.GetInt32Attribute: Can't convert " + text + " from String to Int32.", ex);
				}
				result = true;
			}
			return result;
		}
		public static bool GetInt64Attribute(XmlNode oNode, string sName, bool bRequired, out long i64Value)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			string text;
			bool result;
			if (!XmlUtil.GetAttribute(oNode, sName, bRequired, out text))
			{
				i64Value = -9223372036854775808L;
				result = false;
			}
			else
			{
				try
				{
					i64Value = long.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("XmlUtil.GetInt64Attribute: Can't convert " + text + " from String to Int64.", ex);
				}
				result = true;
			}
			return result;
		}
		public static bool GetSingleAttribute(XmlNode oNode, string sName, bool bRequired, out float fValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			string text;
			bool result;
			if (!XmlUtil.GetAttribute(oNode, sName, bRequired, out text))
			{
				fValue = -3.40282347E+38f;
				result = false;
			}
			else
			{
				try
				{
					fValue = float.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("XmlUtil.GetSingleAttribute: Can't convert " + text + " from String to Single.", ex);
				}
				result = true;
			}
			return result;
		}
		public static bool GetBooleanAttribute(XmlNode oNode, string sName, bool bRequired, out bool bValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			string text;
			bool result;
			if (XmlUtil.GetAttribute(oNode, sName, bRequired, out text))
			{
				string text2;
				if ((text2 = text) != null)
				{
					text2 = string.IsInterned(text2);
					if (text2 != "0")
					{
						if (text2 != "1")
						{
							goto IL_6F;
						}
						bValue = true;
					}
					else
					{
						bValue = false;
					}
					result = true;
					return result;
				}
				IL_6F:
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"XmlUtil.GetBooleanAttribute: A ", 
					oNode.Name, 
					" node has a ", 
					sName, 
					" attribute that is not 0 or 1."
				}));
			}
			bValue = false;
			result = false;
			return result;
		}
		public static bool GetDateTimeAttribute(XmlNode oNode, string sName, bool bRequired, out DateTime oValue)
		{
			Debug.Assert(oNode != null);
			Debug.Assert(sName != null);
			Debug.Assert(sName != "");
			string text;
			bool result;
			if (!XmlUtil.GetAttribute(oNode, sName, bRequired, out text))
			{
				oValue = DateTime.MinValue;
				result = false;
			}
			else
			{
				try
				{
					oValue = DateTime.Parse(text);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("XmlUtil.GetDateTimeAttribute: Can't convert " + text + " from String to DateTime.", ex);
				}
				result = true;
			}
			return result;
		}
		public static void SetAttributes(XmlNode oNode, params string[] asNameValuePairs)
		{
			int num = asNameValuePairs.Length;
			if (num % 2 != 0)
			{
				throw new ArgumentException("XmlUtil.SetAttributes: asNameValuePairs must contain an even number of strings.");
			}
			XmlElement xmlElement = (XmlElement)oNode;
			for (int i = 0; i < num; i += 2)
			{
				string name = asNameValuePairs[i];
				string value = asNameValuePairs[i + 1];
				xmlElement.SetAttribute(name, value);
			}
		}
	}
}
