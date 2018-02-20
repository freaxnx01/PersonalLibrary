using System;
using System.Drawing;
using System.ComponentModel;

namespace TypeConversion
{
    [TargetTypeConverter(typeof(Size))]
    public class SizeConverter : TypeConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] splitted = value.ToString().Split(',');
                if (splitted.Length == 2)
                {
                    int width = 0;
                    if (!int.TryParse(splitted[0], out width))
                    {
                        throw new ArgumentException(string.Format("Value '{0}' is not numeric.", splitted[0]));
                    }

                    int height = 0;
                    if (!int.TryParse(splitted[1], out height))
                    {
                        throw new ArgumentException(string.Format("Value '{0}' is not numeric.", splitted[1]));
                    }

                    return new Size(width, height);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
