using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;

using Commanding;
using TypeConversion;
using System.IO;

namespace ObjectGraphML
{
    public class Engine
    {
        private Dictionary<string, NamespaceAssemblyInfo> namespaceAssemblyInfoDictionary = new Dictionary<string, NamespaceAssemblyInfo>();
        private Dictionary<Type, TypeConverter> typeConverterDictionary = new Dictionary<Type, TypeConverter>();
        
        private Dictionary<string, ICommand> commandDictionary;

        private const string XmlNamespaceAttribute = "xmlns";
        private const string PrefixDefaultNamespace = "(default)";

        public Engine() {}

        public Engine(Dictionary<string, ICommand> commandDictionary)
        {
            this.commandDictionary = commandDictionary;
        }

        public object GetInstanceOfRootObject(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            PopulateNamespaceAssemblyInfoDictionary(xmlDoc);
            Type type = ResolveType(xmlDoc.DocumentElement);
            return Activator.CreateInstance(type);
        }

        public void RenderControl(Control targetControl, string definitionXmlFile, object thisInstance)
        {
            RenderControlFromXml(targetControl, File.ReadAllText(definitionXmlFile), thisInstance);
        }

        public void RenderControlFromXml(Control targetControl, string definitionXml, object thisInstance)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(definitionXml);

            PopulateNamespaceAssemblyInfoDictionary(xmlDoc);

            targetControl.SuspendLayout();

            // Control.Controls (System.Windows.Forms.Control.ControlCollection : ArrangedElementCollection, IList, ICollection, IEnumerable, ICloneable)
            // MenuStrip.Items (System.Windows.Forms.ToolStripItemCollection : ArrangedElementCollection, IList, ICollection, IEnumerable)
            // ToolStripDropDownItem.DropDownItems (System.Windows.Forms.ToolStripItemCollection : ArrangedElementCollection, IList, ICollection, IEnumerable)

            // Root node (z.B. menuStrip) besteht bereits und muss nicht instanziert werden. Lediglich die Attribute werden ausgewertet.
            ApplyAttributes(targetControl, xmlDoc.DocumentElement, thisInstance);

            RenderControl(targetControl, xmlDoc.DocumentElement, thisInstance);

            targetControl.ResumeLayout();
            
        }

        private void PopulateNamespaceAssemblyInfoDictionary(XmlDocument xmlDoc)
        {
            if (namespaceAssemblyInfoDictionary.Count > 0)
            {
                return;
            }

            foreach (XmlAttribute xmlAttribute in xmlDoc.DocumentElement.Attributes)
            {
                if (xmlAttribute.LocalName == XmlNamespaceAttribute)
                {
                    NamespaceAssemblyInfo.AddEntry(namespaceAssemblyInfoDictionary, PrefixDefaultNamespace, xmlAttribute.Value);
                }

                if (xmlAttribute.Prefix == XmlNamespaceAttribute)
                {
                    NamespaceAssemblyInfo.AddEntry(namespaceAssemblyInfoDictionary, xmlAttribute.LocalName, xmlAttribute.Value);
                }
            }
        }

        private void RenderControl(object targetObject, XmlNode xmlNode, object thisInstance)
        {
            RenderControl(targetObject, null, xmlNode, thisInstance);
        }

        private void RenderControl(object targetObject, IList list, XmlNode xmlNode, object thisInstance)
        {
            foreach (XmlNode childXmlNode in xmlNode.ChildNodes)
            {
                if (childXmlNode.NodeType != XmlNodeType.Comment)
                {
                    PropertyInfo propInfo = targetObject.GetType().GetProperty(childXmlNode.Name);
                    if (propInfo != null)
                    {
                        list = propInfo.GetValue(targetObject, null) as IList;
                    }
                    else
                    {
                        // <sys:String .../>
                        // Name = sys:String
                        // LocalName = String
                        // Prefix = sys
                        // NamespaceURI = clr-namespace:System;assembly=mscorlib;

                        Type type = ResolveType(childXmlNode.Prefix, childXmlNode.LocalName);

                        //var nsAsmInfo = NamespaceAssemblyInfo.GetNamespaceAssemblyInfo(namespaceAssemblyInfoDictionary, childXmlNode.Prefix, childXmlNode.NamespaceURI);
                        //string typeFullName = string.Concat(nsAsmInfo.ClrNamespace, ".", childXmlNode.LocalName);
                        //Assembly assembly = nsAsmInfo.Assembly;
                        //Type type = assembly != null ? assembly.GetType(typeFullName) : Type.GetType(typeFullName);

                        targetObject = Activator.CreateInstance(type);
                        list.Add(targetObject);
                        ApplyAttributes(targetObject, childXmlNode, thisInstance);
                    }

                    RenderControl(targetObject, list, childXmlNode, thisInstance);
                }
            }
        }

        private void ApplyAttributes(object targetObject, XmlNode xmlNode, object thisInstance)
        {
            if (xmlNode.Attributes.Count == 0)
            {
                return;
            }

            Type controlType = targetObject.GetType();

            var relevantXmlAttributes = xmlNode.Attributes.OfType<XmlAttribute>()
                .Where(a => !a.LocalName.StartsWith(XmlNamespaceAttribute) && !a.Prefix.StartsWith(XmlNamespaceAttribute));

            foreach (XmlAttribute xmlAttribute in relevantXmlAttributes)
            {
                string value = xmlAttribute.Value;

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (value.StartsWith("{") && value.EndsWith("}"))
                {
                    value = ResolveExpression(value);
                }
                
                string memberName = xmlAttribute.Name;
                PropertyInfo propInfo = controlType.GetProperty(memberName);

                EventInfo eventInfo = null;
                if (propInfo == null)
                {
                    eventInfo = controlType.GetEvent(memberName);
                }

                MethodInfo methodInfo = null;
                if (propInfo == null && eventInfo == null)
                {
                    methodInfo = controlType.GetMethod(memberName);
                }

                if (propInfo == null && eventInfo == null && methodInfo == null)
                {
                    throw new ApplicationException(string.Format("{0}.{1} is not an existing property/event/method.", controlType.FullName, memberName));
                }

                if (propInfo != null)
                {
                    object valueToSet = null;

                    if (propInfo.PropertyType.IsPrimitive || propInfo.PropertyType == typeof(string))
                    {
                        valueToSet = Convert.ChangeType(value, propInfo.PropertyType, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        if (propInfo.PropertyType.BaseType == typeof(System.Enum))
                        {
                            //TODO: Enum-Werte verknüpfen |
                            // z.B. Anchor (System.Windows.Forms.AnchorStyles)
                            valueToSet = Enum.Parse(propInfo.PropertyType, value, false);
                        }
                        else
                        {
                            // Greift z.B. bei Location (Point), Size (Size) oder Font
                            valueToSet = CreateInstance(value, propInfo.PropertyType, thisInstance);
                        }
                    }

                    if (propInfo.CanWrite)
                    {
                        propInfo.SetValue(targetObject, valueToSet, null);
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("{0}.{1} is read only.", propInfo.PropertyType.FullName, propInfo.Name));
                    }
                    
                }
                else if (eventInfo != null)
                {
                    if (thisInstance == null)
                    {
                        throw new ArgumentNullException("thisInstance");
                    }

                    //TODO: BindingFlags (BindingFlags.Instance | BindingFlags.NonPublic) fix
                    MethodInfo methodInfoEventHandler = thisInstance.GetType().GetMethod(value, BindingFlags.Instance | BindingFlags.NonPublic);

                    if (methodInfoEventHandler == null)
                    {
                        throw new ApplicationException(string.Format("EventHandler '{0}' not found on '{1}'.", value, thisInstance.GetType().Name));
                    }

                    Delegate theDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, thisInstance, methodInfoEventHandler);
                    eventInfo.AddEventHandler(targetObject, theDelegate);
                }
                else if (methodInfo != null)
                {
                    //TODO: Method call -> support multiple parameters
                    object[] parameters = new object[] { CreateInstance(value, methodInfo.GetParameters()[0].ParameterType, thisInstance) };
                    methodInfo.Invoke(targetObject, parameters);
                }
            }
        }

        private string ResolveExpression(string expression)
        {
            expression = expression.Substring(1, expression.Length - 2);
            string value = string.Empty;

            const string Space = " ";
            const string ExpressionPrefixStatic = "x:Static";

            if (expression.StartsWith(ExpressionPrefixStatic))
            {
                // Sample:
                // <TextBlock Text="{x:Static A:MyConstants.SomeConstantString}" />
                //xmlns:A="clr-namespace:A"

                expression = expression.Replace(string.Concat(ExpressionPrefixStatic, Space), string.Empty);
                object memberValue = GetMemberValueByObjectPath(expression, BindingFlags.Static | BindingFlags.NonPublic);
                value = memberValue != null ? memberValue.ToString() : string.Empty;
            }
            else
            {
                throw new ApplicationException(string.Format("Expression '{0}' could not have been resolved.", expression));
            }

            return value;
        }

        private object GetMemberValueByObjectPath(string objectPath, BindingFlags bindingFlags)
        {
            const char objectPathSeparator = '.';

            string[] splitted = objectPath.Split(objectPathSeparator);

            if (splitted.Length > 2)
            {
                throw new ApplicationException("Only '<Type>.<Member>' is supported.");
            }

            if (splitted.Length == 2)
            {
                string typePart = splitted[0];

                string namespacePrefix = string.Empty;
                string typeName = string.Empty;

                string[] typePartSplitted = typePart.Split(':');
                if (typePartSplitted.Length == 2)
                {
                    namespacePrefix = typePartSplitted[0];
                    typeName = typePartSplitted[1];
                }
                else
                {
                    typeName = typePart;
                }

                Type type = ResolveType(namespacePrefix, typeName);
                string memberName = splitted[1];
                PropertyInfo propertyInfo = type.GetProperty(memberName, bindingFlags);
                return propertyInfo.GetValue(null, null);
            }

            return null;
        }

        private Type ResolveType(XmlNode xmlNode)
        {
            return ResolveType(xmlNode.Prefix, xmlNode.LocalName);
        }

        private Type ResolveType(string namespacePrefix, string typeName)
        {
            if (string.IsNullOrEmpty(namespacePrefix))
            {
                namespacePrefix = PrefixDefaultNamespace;
            }

            var nsAsmInfo = NamespaceAssemblyInfo.GetNamespaceAssemblyInfo(namespaceAssemblyInfoDictionary, namespacePrefix);
            string typeFullName = string.Concat(nsAsmInfo.ClrNamespace, ".", typeName);
            Assembly assembly = nsAsmInfo.Assembly;
            Type type = assembly != null ? assembly.GetType(typeFullName) : Type.GetType(typeFullName);

            if (type == null)
            {
                throw new ApplicationException(string.Format("Type '{0}' not found.", typeFullName));
            }

            return type;
        }

        private object CreateInstance(string value, Type targetType, object thisInstance)
        {
            object instance = null;

            var typeConverter = GetTypeConverter(targetType);
            if (typeConverter != null)
            {
                instance = typeConverter.ConvertFrom(value);
            }
            else
            {
                string[] splittedValues = value.Split(',');

                ConstructorInfo ctorInfo = Reflection.GetMatchingConstructorInfo(targetType, splittedValues);
                if (ctorInfo != null)
                {
                    instance = ctorInfo.Invoke(Reflection.ComposeConstructorParameterArray(ctorInfo, splittedValues, thisInstance));
                }
            }

            return instance;
        }

        private TypeConverter GetTypeConverter(Type targetType)
        {
            if (!typeConverterDictionary.ContainsKey(targetType))
            {
                if (targetType == typeof(ICommand))
                {
                    CommandConverter converter = new CommandConverter(commandDictionary);
                    typeConverterDictionary.Add(targetType, converter);
                }
                else
                {
                    // Geht davon aus, dass sich alle TypeConverter in derselben Assembly befinden
                    List<Type> typeConverterTypeList = (from t in Assembly.GetAssembly(typeof(TypeConverterBase)).GetTypes()
                                                        where t.GetCustomAttributes(typeof(TargetTypeConverterAttribute), false).Length == 1
                                                        select t).ToList();

                    Type typeConverterType = null;

                    foreach (Type type in typeConverterTypeList)
                    {
                        TargetTypeConverterAttribute attribute = (TargetTypeConverterAttribute)(type.GetCustomAttributes(typeof(TargetTypeConverterAttribute), false))[0];
                        if (attribute.TargetType == targetType)
                        {
                            typeConverterType = type;
                            break;
                        }
                    }

                    if (typeConverterType != null)
                    {
                        typeConverterDictionary.Add(targetType, Activator.CreateInstance(typeConverterType) as TypeConverter);
                    }
                    else
                    {
                        typeConverterDictionary.Add(targetType, null);
                    }
                }
            }

            if (typeConverterDictionary.ContainsKey(targetType))
            {
                return typeConverterDictionary[targetType];
            }
            
            return null;
        }
    }
}
