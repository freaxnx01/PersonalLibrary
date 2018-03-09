using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Library.CodeGen
{
    public class CodeGenClass
    {
        public CodeGenClass()
        {
            BaseTypes = new List<string>();
        }

        public static CodeGenClass DeserializeFromXml(string xml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CodeGenClass));
            MemoryStream mem = new MemoryStream(Encoding.Default.GetBytes(xml));
            return (CodeGenClass)ser.Deserialize(mem);
        }

        public static CodeGenClass DeserializeFromFile(string file)
        {
            return File.Exists(file) ? DeserializeFromXml(File.ReadAllText(file)) : null;
        }

        public void Generate(CodeCompileUnit ccu)
        {
            // namespace
            CodeNamespace cns = new CodeNamespace(Namespace);
            ccu.Namespaces.Add(cns);

            // imports
            foreach (string import in Imports)
            {
                cns.Imports.Add(new CodeNamespaceImport(import));
            }

            CodeTypeDeclaration ctd = new CodeTypeDeclaration(Name);
            ctd.IsClass = true;
            ctd.IsPartial = IsPartialClass;
            ctd.Attributes = MemberAttributes.Public;

            if (IsStatic)
            {
                ctd.Attributes = ctd.Attributes | MemberAttributes.Static;
            }

            foreach (string baseType in BaseTypes)
            {
                ctd.BaseTypes.Add(baseType);
            }

            // class to namespace
            cns.Types.Add(ctd);

            CodeSnippetTypeMember cstm = new CodeSnippetTypeMember();
            cstm.Text = Code;
            ctd.Members.Add(cstm);
        }

        public string Namespace { get; set; }
        public string Name { get; set; }
        public bool IsPartialClass { get; set; }
        public bool IsStatic { get; set; }
        public List<string> BaseTypes { get; set; }
        public List<string> Imports { get; set; }
        public string Code { get; set; }
    }
}
