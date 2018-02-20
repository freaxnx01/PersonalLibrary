using System.Collections.Generic;
using System.Reflection;

namespace ObjectGraphML
{
    public class NamespaceAssemblyInfo
    {
        public string ClrNamespace { get; private set; }
        public string AssemblyString { get; private set; }

        public NamespaceAssemblyInfo(string namespaceURI)
        {
            // TODO: Validierung, Err handling
            string[] splitted = namespaceURI.Split(';');
            string[] values = splitted[0].Split(':');
            ClrNamespace = values[1];
            if (splitted.Length == 2)
            {
                values = splitted[1].Split('=');
                AssemblyString = values[1];
            }
        }

        public Assembly Assembly
        {
            get
            {
                if (!string.IsNullOrEmpty(AssemblyString))
                {
                    return Assembly.LoadWithPartialName(AssemblyString);
                }

                return null;
            }
        }

        public static void AddEntry(Dictionary<string, NamespaceAssemblyInfo> namespaceAssemblyInfoDictionary,
            string prefix, string namespaceURI)
        {
            if (!namespaceAssemblyInfoDictionary.ContainsKey(prefix))
            {
                namespaceAssemblyInfoDictionary.Add(prefix, new NamespaceAssemblyInfo(namespaceURI));
            }
        }

        public static NamespaceAssemblyInfo GetNamespaceAssemblyInfo(Dictionary<string, NamespaceAssemblyInfo> namespaceAssemblyInfoDictionary, string prefix)
        {
            if (namespaceAssemblyInfoDictionary.ContainsKey(prefix))
            {
                return namespaceAssemblyInfoDictionary[prefix];
            }

            return null;
        }
    }
}
