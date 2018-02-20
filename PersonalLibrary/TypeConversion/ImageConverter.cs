using System;
using System.ComponentModel;
using System.Drawing;

namespace TypeConversion
{
    [TargetTypeConverter(typeof(Image))]
    public class ImageConverter : TypeConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string filePath = value.ToString();
                Image image = Bitmap.FromFile(filePath);
                return image;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
