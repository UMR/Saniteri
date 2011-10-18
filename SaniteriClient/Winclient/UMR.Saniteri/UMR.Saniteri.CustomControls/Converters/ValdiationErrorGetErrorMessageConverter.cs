using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;

namespace UMR.Saniteri.CustomControls
{

    /// <summary>
    /// Converter that returns the ValidationError.ErrorContent or the first inner exception message.
    /// This converter is handly because it will return the message from an InnerException or the ErrorContent.
    /// Some data binding exceptions are package in a System.Reflection.TargetInvocationException and this converter will return the message from the actual exception rather than the useless TargetInvocationException message.
    /// This converter usefullness is demonstrated when IDataErrorInfo property validation routines throw exceptions. Those exceptions will bubble throught the WPF data binding pipeline and the InnerException will be displayed using this converter.
    /// </summary>
    [ValueConversion(typeof(ValidationError), typeof(string))]
    public class ValdiationErrorGetErrorMessageConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);
            foreach (ValidationError objVB in (System.Collections.ObjectModel.ReadOnlyObservableCollection<ValidationError>)value)
            {
                if (objVB.Exception == null || objVB.Exception.InnerException == null)
                {
                    sb.AppendLine(objVB.ErrorContent.ToString());
                }
                else
                {
                    sb.AppendLine(objVB.Exception.InnerException.Message);
                }
            }

            //remove the last line feed
            if (sb.Length > 2)
            {
                sb.Length -= 2;
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
