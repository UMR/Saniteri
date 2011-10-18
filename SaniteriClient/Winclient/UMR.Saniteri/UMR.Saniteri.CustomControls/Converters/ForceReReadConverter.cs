using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace UMR.Saniteri.CustomControls
{

    //This forum post discusses why this converter is required.
    //http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=1829709&SiteID=1

    [ValueConversion(typeof(object), typeof(object))]
    public class ForceReReadConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
