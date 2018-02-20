using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	public class StringUtil
	{
		private StringUtil()
		{
		}
		public static bool IsEmpty(string sString)
		{
			return sString == null || sString.Length == 0;
		}
		[Conditional("DEBUG")]
		public static void AssertNotEmpty(string sString)
		{
			Debug.Assert(!StringUtil.IsEmpty(sString));
		}
		public static string[] CopyStringArray(string[] asArrayToCopy)
		{
			Debug.Assert(asArrayToCopy != null);
			int num = asArrayToCopy.Length;
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = string.Copy(asArrayToCopy[i]);
			}
			return array;
		}
		public static string[] CreateArrayOfEmptyStrings(int iEmptyStrings)
		{
			Debug.Assert(iEmptyStrings >= 0);
			string[] array = new string[iEmptyStrings];
			for (int i = 0; i < iEmptyStrings; i++)
			{
				array[i] = string.Empty;
			}
			return array;
		}
		public static string BytesToPrintableAscii(byte[] abtBytes, char cReplacementCharacter)
		{
			Debug.Assert(abtBytes != null);
			Debug.Assert(cReplacementCharacter < '\u0080');
			byte[] array = (byte[])abtBytes.Clone();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] > 127)
				{
					array[i] = (byte)cReplacementCharacter;
				}
			}
			string @string = Encoding.ASCII.GetString(array);
			return StringUtil.ReplaceNonPrintableAsciiCharacters(@string, cReplacementCharacter);
		}
		public static string ReplaceNonPrintableAsciiCharacters(string sString, char cReplacementCharacter)
		{
			Debug.Assert(sString != null);
			Regex regex = new Regex("[^\\x09\\x0A\\x0D\\x20-\\x7E]");
			return regex.Replace(sString, new string(cReplacementCharacter, 1));
		}
	}
}
