using System;
using System.Drawing;
using System.ComponentModel;

namespace TypeConversion
{
    [TargetTypeConverter(typeof(Font))]
    public class FontConverter : TypeConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] splittedValues = value.ToString().Split(',');

                if (splittedValues.Length >= 2)
                {
                    string fontName = splittedValues[0];
                    float fontSize = float.Parse(splittedValues[1]);

                    object returnValue = null;

                    if (splittedValues.Length == 3)
                    {
                        // fontName, fontSize, fontStyle
                        FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), splittedValues[2]);
                        returnValue = new Font(fontName, fontSize, fontStyle);
                    }
                    else if (splittedValues.Length == 2)
                    {
                        // fontName, fontSize
                        returnValue = new Font(fontName, fontSize, FontStyle.Regular);
                    }

                    return returnValue;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
