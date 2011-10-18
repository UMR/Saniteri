using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace UMR.Saniteri.CustomControls
{

    [ValueConversion(typeof(Nullable<int>), typeof(string))]
    public class NullableIntegerFormatConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            int intValue;

            if (int.TryParse(value.ToString(), out intValue))
            {
                string strFormatString = parameter.ToString();

                if (!string.IsNullOrEmpty(strFormatString))
                {
                    return string.Format(culture, strFormatString, intValue);
                }

                else
                {
                    return intValue.ToString();
                }
            }

            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.ToString().Trim().Length == 0)
            {
                return null;
            }

            int intValue;

            if (int.TryParse(value.ToString(), out intValue))
            {
                return intValue;
            }
            else
            {
                return value;
            }
        }

        #endregion
    }
}
