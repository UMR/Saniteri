using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;

namespace CustomControls.Saniteri.CustomControls
{
  public class BaseUserControl : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        #region " Declarations "
            private Dictionary<string, System.Windows.Controls.Control> _objControlsWithValiationExceptions = new Dictionary<string, System.Windows.Controls.Control>();
        #endregion
        #region " Events "
            public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        
        #region " Properties "

        public string ValidationExceptionErrors {
            get { return GetExceptionValidationErrors(); }
        }

        public bool HasValidationExceptionErrors {
            get { return _objControlsWithValiationExceptions.Keys.Count > 0; }
        }
        #endregion
        #region " Methods "

        private void ExceptionValidationErrorHandler(object sender, RoutedEventArgs e)
        {

            System.Windows.Controls.ValidationErrorEventArgs args = (System.Windows.Controls.ValidationErrorEventArgs)e;

            if (args.Error.RuleInError is System.Windows.Controls.ExceptionValidationRule) {
                //only work with validation errors that are Exceptions because the business object has already recorded the business rule violations.

                string strDataItemName = ((System.Windows.Data.BindingExpression)(System.Object)((System.Windows.Controls.ValidationError)args.Error).BindingInError).DataItem.ToString();
                string strPropertyName = ((System.Windows.PropertyPath)((System.Windows.Data.Binding)((System.Windows.Data.BindingExpression)(System.Object)((System.Windows.Controls.ValidationError)args.Error).BindingInError).ParentBinding).Path).Path;

                string strKey = string.Concat(strDataItemName, ":", strPropertyName);

                if (args.Action == ValidationErrorEventAction.Added) {

                    _objControlsWithValiationExceptions.Add(strKey, (System.Windows.Controls.Control)e.OriginalSource);
                    OnRaisePropertyChanged("ValidationExceptionErrors");
                }

                else if (args.Action == ValidationErrorEventAction.Removed) {

                    _objControlsWithValiationExceptions.Remove(strKey);
                    OnRaisePropertyChanged("ValidationExceptionErrors");
                }
            }
        }

        private void // ERROR: Handles clauses are not supported in C#
UserControlBase_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //this adds a form level handler and will listen for each control that has the NotifyOnValidationError=True and a ValidationError occurs.
            this.AddHandler(System.Windows.Controls.Validation.ErrorEvent, new RoutedEventHandler(ExceptionValidationErrorHandler), true);

        }

        private void // ERROR: Handles clauses are not supported in C#
UserControlBase_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RemoveHandler(System.Windows.Controls.Validation.ErrorEvent, new RoutedEventHandler(ExceptionValidationErrorHandler));

        }

        protected void ClearValidationExceptionErrors()
        {
            _objControlsWithValiationExceptions.Clear();
            OnRaisePropertyChanged("ValidationExceptionErrors");

        }

        /// <summary>
        /// Checks each of the controls that have raised the Validation.ErrorEvent to see if the control still has an exception validation rule in the control's Validation.Errors collection
        /// </summary>
        /// <remarks>This is the SUPER fast way to check the Validation Exception errors. This works only if the controls on the form all have the property, NotifyOnValidationError=True</remarks>
        private string GetExceptionValidationErrors()
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);

            foreach (KeyValuePair<string, System.Windows.Controls.Control> obj in _objControlsWithValiationExceptions) {

                if (System.Windows.Controls.Validation.GetHasError(obj.Value)) {

                    foreach (ValidationError objVE in System.Windows.Controls.Validation.GetErrors((DependencyObject)obj.Value)) {

                        if (objVE.RuleInError is System.Windows.Controls.ExceptionValidationRule) {
                            sb.Append(StringFormatting.CamelCaseString.GetWords(((System.Windows.PropertyPath)((System.Windows.Data.Binding)((System.Windows.Data.BindingExpression)(System.Object)objVE.BindingInError).ParentBinding).Path).Path));
                            sb.Append(" has error ");

                            if (objVE.Exception == null || objVE.Exception.InnerException == null) {
                                sb.AppendLine(objVE.ErrorContent.ToString());
                            }
                            else {
                                sb.AppendLine(objVE.Exception.InnerException.Message);
                            }

                        }

                    }

                }

            }

            return sb.ToString();

        }

        protected void OnRaisePropertyChanged(string strPropertyName)
        {

            if (this.PropertyChanged != null)
            {
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
                }
            }

        }

        protected void UpdateFocusedField()
        {
            FrameworkElement fwE = Keyboard.FocusedElement as FrameworkElement;

            if (fwE != null) {

                BindingExpression expression = null;

                if (fwE is TextBox) {
                    expression = fwE.GetBindingExpression(TextBox.TextProperty);
                    //
                    //TODO developers add more controls types here. Won't be that many.
                    //this would include custom TextBox controls or 3rd party TextBox controls
                }

                if (expression != null) {
                    expression.UpdateSource();
                }
            }
        }
#endregion

    }
}
