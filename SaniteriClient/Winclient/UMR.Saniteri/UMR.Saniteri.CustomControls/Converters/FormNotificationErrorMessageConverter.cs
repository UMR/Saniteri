using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace UMR.Saniteri.CustomControls
{
    [ValueConversion(typeof(string), typeof(string), ParameterType = typeof(string))]
    public class FormNotificationErrorMessageConverter : IMultiValueConverter
    {

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);

            foreach (object obj in values)
            {
                if (obj.ToString().Length > 0)
                {
                    sb.AppendLine(obj.ToString());
                }
            }

            return sb.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

