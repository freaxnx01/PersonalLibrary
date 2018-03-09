using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Collections;


namespace Library
{
    public static class AssemblyScanner
    {
        #region GetCustomAssemblyAttributeValue
        public static string GetCustomAssemblyAttributeValue(Attribute assemblyAttribute)
        {
            return GetCustomAssemblyAttributeValue(Assembly.GetExecutingAssembly(), assemblyAttribute.GetType());
        }

        public static string GetCustomAssemblyAttributeValue(Type typeOfAttribute)
        {
            return GetCustomAssemblyAttributeValue(Assembly.GetExecutingAssembly(), typeOfAttribute);
        }

        public static string GetCustomAssemblyAttributeValue(Assembly assembly, Type typeOfAttribute)
        {
            string assemblyAttributeValue = string.Empty;

            object[] attributes = assembly.GetCustomAttributes(typeOfAttribute, false);
            if (attributes.Length > 0)
            {
                PropertyInfo[] propsInfo = attributes[0].GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                if (propsInfo.Length > 0)
                {
                    assemblyAttributeValue = propsInfo[0].GetValue(attributes[0], null).ToString();
                }
                else
                {
                    FieldInfo[] filedsInfo = attributes[0].GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                    if (filedsInfo.Length > 0)
                    {
                        assemblyAttributeValue = filedsInfo[0].GetValue(attributes[0]).ToString();
                    }
                }
            }

            return assemblyAttributeValue;
        }
        #endregion

        #region GetNamespaces
        public static List<string> GetNamespaces(string assemblyFile)
        {
            Assembly asm = Assembly.LoadFrom(assemblyFile);
            return GetNamespaces(asm);
        }

        public static List<string> GetNamespaces(Assembly assembly)
        {
            List<string> listOfNamespaces = new List<string>();

            foreach (Type type in assembly.GetTypes())
            {
                if (!listOfNamespaces.Contains(type.Namespace))
                {
                    listOfNamespaces.Add(type.Namespace);
                }
            }

            return listOfNamespaces;
        }
        #endregion

        #region ListImportedAssemblies
        public static List<AssemblyInfo> ListImportedAssemblies(string file)
		{
            if (string.IsNullOrEmpty(file))
            {
                file = GetProcessPath();
            }

            AppDomain newAppDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            Type adType = typeof(DependentAssembly);
            ObjectHandle objHandle = newAppDomain.CreateInstance(adType.Module.Assembly.FullName, adType.FullName);
            DependentAssembly depAsm = (DependentAssembly)objHandle.Unwrap();
            newAppDomain.SetupInformation.ApplicationBase = Path.GetDirectoryName(file);
            depAsm.BuildDependentAssemblyList(file, Path.GetDirectoryName(file));
            List<AssemblyInfo> listOfAssemblyInfo = depAsm.Assemblies;
            AppDomain.Unload(newAppDomain);

            return RemoveDuplicates(listOfAssemblyInfo);
		}

        public static List<AssemblyInfo> RemoveDuplicates(List<AssemblyInfo> assemblies)
        {
            Dictionary<string, AssemblyInfo> dictAssemblies = new Dictionary<string, AssemblyInfo>();
            foreach (AssemblyInfo asmInfo in assemblies)
            {
                if (!dictAssemblies.ContainsKey(asmInfo.FullName))
                {
                    dictAssemblies.Add(asmInfo.FullName, asmInfo);
                }
            }

            return new List<AssemblyInfo>(dictAssemblies.Values);
        }

        public static List<string> GetAssemblyPathList(List<AssemblyInfo> assemblies)
        {
            List<string> listOfAsmPath = new List<string>();
            foreach (AssemblyInfo asmInfo in assemblies)
            {
                if (!asmInfo.GlobalAssemblyCache)
                {
                    listOfAsmPath.Add(asmInfo.Location);
                }
            }

            return listOfAsmPath;
        }

        public enum EnrichOption
        {
            None = 0,
            pdb = 1,
            XmlSerializersDll = 2
        }

        public static List<string> EnrichAssemblyPathList(List<string> assemblyPathList, EnrichOption enrichOptions)
        {
            List<string> enrichedAssemblyPathList = new List<string>(assemblyPathList);

            if ((enrichOptions & EnrichOption.XmlSerializersDll) == EnrichOption.XmlSerializersDll)
            {
                foreach (string asmPath in assemblyPathList)
                {
                    string fileToFind = Path.Combine(Path.GetDirectoryName(asmPath), string.Concat(Path.GetFileNameWithoutExtension(asmPath), ".XmlSerializers.dll"));
                    if (File.Exists(fileToFind))
                    {
                        enrichedAssemblyPathList.Add(fileToFind);
                    }
                }
            }

            if ((enrichOptions & EnrichOption.pdb) == EnrichOption.pdb)
            {
                foreach (string asmPath in assemblyPathList)
                {
                    string fileToFind = Path.ChangeExtension(asmPath, "pdb");
                    if (File.Exists(fileToFind))
                    {
                        enrichedAssemblyPathList.Add(fileToFind);
                    }
                }
            }

            return enrichedAssemblyPathList;
        }

        private static string GetProcessPath()
        {
            // fix the path so that if running under the debugger we get the original file
            string processName = Process.GetCurrentProcess().MainModule.FileName;
            int index = processName.IndexOf("vshost");
            if (index != -1)
            {
                string first = processName.Substring(0, index);
                int numChars = processName.Length - (index + 7);
                string second = processName.Substring(index + 7, numChars);

                processName = first + second;
            }
            return processName;
        }
        #endregion
    }
}
