using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace UMR.Saniteri.CustomControls
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class ImageSourceConverter : IMultiValueConverter
    {


        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strImage = string.Empty;

            if ((bool)values[0])
            {
                strImage = values[1].ToString();
            }

            else
            {
                strImage = values[2].ToString();
            }

            if (!string.IsNullOrEmpty(strImage))
            {

                Uri objURI = new Uri(strImage, UriKind.Relative);
                return new BitmapImage(objURI);
            }

            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
