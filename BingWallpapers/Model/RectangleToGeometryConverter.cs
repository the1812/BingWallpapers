using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BingWallpapers.Model
{
    class RectangleToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Rectangle rect)
            {
                return new RectangleGeometry(new Rect(new Size(rect.Width, rect.Height)))
                {
                    RadiusX = rect.RadiusX,
                    RadiusY = rect.RadiusY
                };
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
