using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace UMR.Saniteri.Data
{
    public partial class can_eventcodes : IDataErrorInfo
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
                if (columnName == "event_type")
                {
                    int eType;
                    if ((!int.TryParse(event_type.ToString(), out eType)) || eType < 0)
                    {
                        result = "Event Type has to be set!";
                        _errorCollection["event_type"] = "Event Type has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("event_type"))
                        _errorCollection.Remove("event_type");
                } 
                else if (columnName == "description")
                {
                    if (String.IsNullOrEmpty(description))
                    {
                        result = "Description has to be set!";
                        _errorCollection["description"] = "Description has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("description"))
                        _errorCollection.Remove("description");
                }
                return result;
            }
        }
    }
}
