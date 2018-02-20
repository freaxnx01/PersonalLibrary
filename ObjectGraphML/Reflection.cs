using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace ObjectGraphML
{
    public class Reflection
    {
        public static ConstructorInfo GetMatchingConstructorInfo(Type type, string[] values)
        {
            //TODO: ctor mit übereinstimmender Anzahl Param. Kein Typenabgleich!
            ConstructorInfo ctorInfo = (from c in type.GetConstructors()
                                        where c.GetParameters().Length == values.Length
                                        select c).SingleOrDefault();

            if (ctorInfo == null)
            {
                throw new ApplicationException(string.Format("No matching ctor found. Type: {0}, Values: {1}", type.Name, values ));
            }

            return ctorInfo;
        }

        public static object[] ComposeConstructorParameterArray(ConstructorInfo ctorInfo, string[] values, object thisInstance)
        {
            List<object> parameters = new List<object>();
            int paramCounter = 0;

            foreach (ParameterInfo paramInfo in ctorInfo.GetParameters())
            {
                if (paramCounter < values.Length)
                {
                    //TODO: String handling '', wie wird ' im String behandelt?
                    string paramStringValue = values[paramCounter].Trim().Trim('\'');

                    object paramValue = paramStringValue;

                    //TODO: this.
                    if (paramStringValue.StartsWith("this."))
                    {
                        string reference = paramStringValue.Substring("this.".Length);

                        //TODO: GetField und BindingFlags fix
                        FieldInfo fieldInfo = thisInstance.GetType().GetField(reference, BindingFlags.Instance | BindingFlags.NonPublic);

                        paramValue = fieldInfo.GetValue(thisInstance);
                    }

                    if (paramInfo.ParameterType != typeof(System.Object))
                    {
                        parameters.Add(Convert.ChangeType(paramValue, paramInfo.ParameterType, CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        parameters.Add(paramValue);
                    }
                }

                paramCounter++;
            }

            return parameters.ToArray();
        }
    }
}
