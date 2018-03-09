using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace Library
{
    internal class DependentAssembly : MarshalByRefObject
    {
        private List<AssemblyInfo> listOfAssemblyInfo = new List<AssemblyInfo>();

        public List<AssemblyInfo> Assemblies
        {
            get
            {
                return listOfAssemblyInfo;
            }
        }

        public void BuildDependentAssemblyList(string path, string asmResolvePath)
        {
            // have we already seen this one?
            foreach (AssemblyInfo asmInfo in listOfAssemblyInfo)
            {
                if (asmInfo.FullName == path)
                {
                    return;
                }
            }

            Assembly asm = null;
            // look for common path delimiters in the string 
            // to see if it is a name or a path

            if ((path.IndexOf(Path.DirectorySeparatorChar, 0, path.Length) != -1) ||
                (path.IndexOf(Path.AltDirectorySeparatorChar, 0, path.Length) != -1))
            {
                // load the assembly from a path
                asm = Assembly.ReflectionOnlyLoadFrom(path);
            }
            else
            {
                string asmPath = Path.Combine(asmResolvePath, path.Split(',')[0]);
                asmPath = string.Concat(asmPath, ".dll");
                if (File.Exists(asmPath))
                {
                    asm = Assembly.LoadFrom(asmPath);
                }
                else
                {
                    asmPath = Path.ChangeExtension(asmPath, "exe");
                    if (File.Exists(asmPath))
                    {
                        asm = Assembly.LoadFrom(asmPath);
                    }
                }

                if (asm == null)
                {
                    // try as assembly name
                    try
                    {
                        asm = Assembly.ReflectionOnlyLoad(path);
                    }
                    catch { }
                }

                if (asm == null)
                {
                    throw new FileLoadException(string.Format("Assembly {0} not found.", Path.GetFileNameWithoutExtension(path)));
                }
            }

            // add the assembly to the list
            if (asm != null)
            {
                AssemblyInfo asmInfo = new AssemblyInfo();
                asmInfo.FullName = asm.FullName;
                asmInfo.GlobalAssemblyCache = asm.GlobalAssemblyCache;
                asmInfo.Location = asm.Location;
                asmInfo.CodeBase = asm.CodeBase;
                asmInfo.ImageRuntimeVersion = asm.ImageRuntimeVersion;
                listOfAssemblyInfo.Add(asmInfo);
            }

            // get the referenced assemblies
            AssemblyName[] imports = asm.GetReferencedAssemblies();

            // iterate
            foreach (AssemblyName asmName in imports)
            {
                // now recursively call this assembly to get the new modules 
                // it references
                BuildDependentAssemblyList(asmName.FullName, asmResolvePath);
            }
        }
    }
}
