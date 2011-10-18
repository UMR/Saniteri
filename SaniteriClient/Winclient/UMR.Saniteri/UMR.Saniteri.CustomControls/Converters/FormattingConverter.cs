using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace UMR.Saniteri.CustomControls
{

    [ValueConversion(typeof(object), typeof(string))]
    public class FormattingConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {

                string strFormatString = parameter.ToString();

                if (!string.IsNullOrEmpty(strFormatString))
                {
                    return string.Format(culture, strFormatString, value);
                }
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.ComponentModel.TypeConverter objTypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(targetType);
            object objReturnValue = null;

            if (objTypeConverter.CanConvertFrom(value.GetType()))
            {

                try
                {
                    objReturnValue = objTypeConverter.ConvertFrom(value);
                }

                catch (Exception ex)
                {
                    //HACK - developers you have two options here.
                    //1. Return nothing which in effect wipes out the offending text in the binding control
                    //
                    //objReturnValue = Nothing
                    //
                    //
                    //2. Return the value. Then allow the data binding to fail further down the chain, ie. when it attempts to bind to the entity object. This failure will be handled by the data binding pipeline.
                    //
                    objReturnValue = value;
                }

            }

            return objReturnValue;

        }

        #endregion
    }
}

