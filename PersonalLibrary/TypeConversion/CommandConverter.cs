using System.ComponentModel;
using System.Collections.Generic;

using Commanding;

namespace TypeConversion
{
    [TargetTypeConverter(typeof(ICommand))]
    public class CommandConverter : TypeConverterBase
    {
        private Dictionary<string, ICommand> commandDictionary;

        public CommandConverter(Dictionary<string, ICommand> commandDictionary)
        {
            this.commandDictionary = commandDictionary;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (commandDictionary == null)
            {
                return null;
            }

            if (value is string)
            {
                string valueString = value.ToString();
                if (commandDictionary.ContainsKey(valueString))
                {
                    return commandDictionary[valueString];
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
