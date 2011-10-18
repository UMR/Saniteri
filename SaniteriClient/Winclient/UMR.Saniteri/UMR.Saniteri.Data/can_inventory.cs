using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;

namespace UMR.Saniteri.Data
{
    public partial class can_inventory : IDataErrorInfo
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
                if (columnName == "ip_address")
                {
                    IPAddress address;
                    var total = ip_address == null ? 0 : ip_address.Count(c => c == '.');
                    if (ip_address == null || total < 3 || !(IPAddress.TryParse(ip_address, out address)))
                    {
                        result = "IP Address has to be set!";
                        _errorCollection["ip_address"] = "IP Address has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("ip_address"))
                        _errorCollection.Remove("ip_address");
                }
                else if (columnName == "street")
                {
                    if (String.IsNullOrEmpty(street))
                    {
                        result = "Street has to be set!";
                        _errorCollection["street"] = "Street has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("street"))
                        _errorCollection.Remove("street");
                }
                else if (columnName == "city")
                {
                    if (String.IsNullOrEmpty(city))
                    {
                        result = "City has to be set!";
                        _errorCollection["city"] = "City has to be set!";
                    }
                    else if (_errorCollection.ContainsKey("city"))
                        _errorCollection.Remove("city");
                }
                return result;
            }
        }
    }
}
