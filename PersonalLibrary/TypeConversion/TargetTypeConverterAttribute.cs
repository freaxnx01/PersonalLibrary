using System;

namespace TypeConversion
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TargetTypeConverterAttribute : Attribute
    {
        public Type TargetType { get; set; }

        public TargetTypeConverterAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
