using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	public class SerializationUtil
	{
		private SerializationUtil()
		{
		}
		public static void Serialize(object oObject, Stream oStream)
		{
			Debug.Assert(oObject != null);
			Debug.Assert(oStream != null);
			XmlSerializer xmlSerializer = SerializationUtil.CreateXmlSerializer(oObject);
			xmlSerializer.Serialize(oStream, oObject);
		}
		public static void Serialize(object oObject, StreamWriter oStreamWriter)
		{
			Debug.Assert(oObject != null);
			Debug.Assert(oStreamWriter != null);
			XmlSerializer xmlSerializer = SerializationUtil.CreateXmlSerializer(oObject);
			xmlSerializer.Serialize(oStreamWriter, oObject);
		}
		public static object Deserialize(Type oType, Stream oStream)
		{
			Debug.Assert(oType != null);
			Debug.Assert(oStream != null);
			XmlSerializer xmlSerializer = SerializationUtil.CreateXmlSerializer(oType);
			object obj = xmlSerializer.Deserialize(oStream);
			Debug.Assert(obj != null);
			if (obj is IDeserializationCallback)
			{
				((IDeserializationCallback)obj).OnDeserialization(null);
			}
			return obj;
		}
		public static object Deserialize(Type oType, FileInfo oFileInfo)
		{
			Debug.Assert(oType != null);
			Debug.Assert(oFileInfo != null);
			Debug.Assert(oFileInfo.Exists);
			FileStream fileStream = oFileInfo.OpenRead();
			object obj = null;
			try
			{
				obj = SerializationUtil.Deserialize(oType, fileStream);
			}
			catch
			{
				throw;
			}
			finally
			{
				fileStream.Close();
			}
			Debug.Assert(obj != null);
			return obj;
		}
		public static object Clone(object oObject)
		{
			Debug.Assert(oObject != null);
			MemoryStream memoryStream = new MemoryStream();
			SerializationUtil.Serialize(oObject, memoryStream);
			memoryStream.Position = 0L;
			object result = SerializationUtil.Deserialize(oObject.GetType(), memoryStream);
			memoryStream.Close();
			return result;
		}
		public static void SerializeStringAttributes(XmlTextWriter oXmlTextWriter, params string[] asNameValuePairs)
		{
			Debug.Assert(oXmlTextWriter != null);
			Debug.Assert(asNameValuePairs != null);
			int num = asNameValuePairs.Length;
			if (num % 2 != 0)
			{
				throw new ApplicationException("SerializationUtil.SerializeStringAttributes: asNameValuePairs must contain an even number of elements.");
			}
			for (int i = 0; i < num; i += 2)
			{
				string text = asNameValuePairs[i];
				string value = asNameValuePairs[i + 1];
				if (StringUtil.IsEmpty(text))
				{
					throw new ApplicationException(string.Format("SerializationUtil.SerializeStringAttributes: asNameValuePairs[{0}] is empty or null.", i));
				}
				oXmlTextWriter.WriteAttributeString(text, value);
			}
		}
		public static string DeserializeRequiredStringAttribute(XmlTextReader oXmlTextReader, string sElementName, string sAttributeName)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == sElementName);
			StringUtil.AssertNotEmpty(sElementName);
			StringUtil.AssertNotEmpty(sAttributeName);
			string attribute = oXmlTextReader.GetAttribute(sAttributeName);
			if (attribute == null)
			{
				throw new ApplicationException(string.Format("SerializationUtil.DeserializeRequiredStringAttribute: A {0} XML element is missing a required {1} attribute.", sElementName, sAttributeName));
			}
			return attribute;
		}
		public static int DeserializeRequiredInt32Attribute(XmlTextReader oXmlTextReader, string sElementName, string sAttributeName)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == sElementName);
			StringUtil.AssertNotEmpty(sElementName);
			StringUtil.AssertNotEmpty(sAttributeName);
			string text = SerializationUtil.DeserializeRequiredStringAttribute(oXmlTextReader, sElementName, sAttributeName);
			int result = -2147483648;
			try
			{
				result = int.Parse(text);
			}
			catch (FormatException ex)
			{
				throw new ApplicationException(string.Format("SerializationUtil.DeserializeRequiredInt32Attribute: A {0} XML element has a {1} attribute that must be an Int32 but is not in Int32 format.", sElementName, sAttributeName), ex);
			}
			return result;
		}
		public static float DeserializeRequiredSingleAttribute(XmlTextReader oXmlTextReader, string sElementName, string sAttributeName)
		{
			Debug.Assert(oXmlTextReader != null);
			Debug.Assert(oXmlTextReader.NodeType == XmlNodeType.Element);
			Debug.Assert(oXmlTextReader.Name == sElementName);
			StringUtil.AssertNotEmpty(sElementName);
			StringUtil.AssertNotEmpty(sAttributeName);
			string text = SerializationUtil.DeserializeRequiredStringAttribute(oXmlTextReader, sElementName, sAttributeName);
			float result = -3.40282347E+38f;
			try
			{
				result = float.Parse(text);
			}
			catch (FormatException ex)
			{
				throw new ApplicationException(string.Format("SerializationUtil.DeserializeRequiredSingleAttribute: A {0} XML element has a {1} attribute that must be a Single but is not in Single format.", sElementName, sAttributeName), ex);
			}
			return result;
		}
		protected static XmlSerializer CreateXmlSerializer(object oObject)
		{
			Debug.Assert(oObject != null);
			return SerializationUtil.CreateXmlSerializer(oObject.GetType());
		}
		protected static XmlSerializer CreateXmlSerializer(Type oType)
		{
			Debug.Assert(oType != null);
			return new XmlSerializer(oType);
		}
	}
}
