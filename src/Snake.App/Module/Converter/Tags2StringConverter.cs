using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Snake.App.Module.Converter
{
    public class Tags2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var tags = value as IList<string>;
            return string.Join(",", tags.ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string tag = value as string;
            IList<string> list = new List<string>();
            list.Add(tag);
            return list;
        }
    }
}
