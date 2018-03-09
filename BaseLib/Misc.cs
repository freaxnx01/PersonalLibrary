using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace Library
{
    public static class Misc
    {
        #region GetCustomStackTrace
        public static string GetCustomStackTrace(Exception e)
        {
            StringBuilder sb = new StringBuilder();

            StackTrace st = new StackTrace(e, true);

            foreach (StackFrame frame in st.GetFrames())
            {
                if (!string.IsNullOrEmpty(frame.GetFileName()))
                {
                    sb.AppendLine(string.Format("   at {0}({1}) in {2}:line {3}", frame.GetMethod().DeclaringType.FullName + '.' + frame.GetMethod().Name, GetParameterString(frame.GetMethod()), Path.GetFileName(frame.GetFileName()), frame.GetFileLineNumber().ToString()));
                }
                else
                {
                    sb.AppendLine(string.Format("   at {0}({1})", frame.GetMethod().DeclaringType.FullName + '.' + frame.GetMethod().Name, GetParameterString(frame.GetMethod())));
                }
            }

            return sb.ToString();
        }

        private static string GetParameterString(MethodBase method)
        {
            if (method == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            ParameterInfo[] paramInfos = method.GetParameters();

            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo paramInfo = paramInfos[i];

                string data = paramInfo.ToString();
                if (paramInfo.ParameterType.FullName != null)
                {
                    data = data.Replace(paramInfo.ParameterType.FullName, paramInfo.ParameterType.Name);
                }

                sb.Append(data);

                if (i < paramInfos.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}
