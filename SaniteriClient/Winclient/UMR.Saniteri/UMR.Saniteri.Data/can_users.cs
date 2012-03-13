using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace UMR.Saniteri.Data
{
    public partial class can_users : IDataErrorInfo
    {
        public delegate void IsDirtyHandler(object sender, IsDirtyEventsArgs e);
        public event IsDirtyHandler OnIsDirty;
        public void OnIsDirtyHandler(bool _isDirty)
        {
            if (OnIsDirty != null)
            {
                OnIsDirty(this, new IsDirtyEventsArgs(_isDirty));
            }
        }

        #region CanExecute
        private Dictionary<string, string> _errorCollection = new Dictionary<string, string>();
        public bool CanSaveConfig
        {
            get
            {
                return CanSave && _errorCollection.Count == 0;
            }
        }
        #endregion

        #region CanExecute

        bool canAdd = true;

        public bool CanAdd
        {
            get { return canAdd; }
            set
            {
                canAdd = value;
            }
        }
        bool canDelete = true;

        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; }
        }
        bool canSave = false;

        public bool CanSave
        {
            get { return canSave; }
            set { canSave = value; }
        }
        bool canCancel = false;

        public bool CanCancel
        {
            get { return canCancel; }
            set { canCancel = value; }
        }

        bool _isNew;
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }

        public void SetButtonState(bool state)
        {
            canAdd = state;
            canDelete = state;
            //IsEnable = state;
            canSave = !state;
            canCancel = !state;
            OnIsDirtyHandler(!state);
        }
        #endregion

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "user_id")
                {
                    if (String.IsNullOrEmpty(user_id))
                    {
                        result = "User ID has to be set!";
                        _errorCollection["user_id"] = "User ID has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("user_id"))
                        _errorCollection.Remove("user_id");
                }
                else if (columnName == "first_name")
                {
                    if (String.IsNullOrEmpty(user_id))
                    {
                        result = "User ID has to be set!";
                        _errorCollection["user_id"] = "User ID has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("user_id"))
                        _errorCollection.Remove("user_id");
                }
                else if (columnName == "last_name")
                {
                    if (String.IsNullOrEmpty(last_name))
                    {
                        result = "Last Name has to be set!";
                        _errorCollection["last_name"] = "Last Name has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("last_name"))
                        _errorCollection.Remove("last_name");
                }
                else if (columnName == "title")
                {
                    if (String.IsNullOrEmpty(title))
                    {
                        result = "Title has to be set!";
                        _errorCollection["title"] = "Title has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("title"))
                        _errorCollection.Remove("title");
                }
                return result;
            }
        }
    }
}
