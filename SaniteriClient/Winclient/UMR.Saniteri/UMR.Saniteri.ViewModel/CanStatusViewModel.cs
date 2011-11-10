using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UMR.Saniteri.Data;
using UMR.Saniteri.Common;
using System.Windows.Input;
using UMR.Saniteri.Command;

namespace UMR.Saniteri.ViewModel
{
    public class CanStatusViewModel : INotifyPropertyChanged
    {
        public CanStatusViewModel()
        {
            LoadInDetails();
        }

        private IList<can_status> _canStatusList;

        private can_status _canStatus;
        private int _selectedIndex = 0;
        public string searchData { get; protected set; }

        public int selectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value >= 0)
                {
                    _selectedIndex = value;
                    OnPropertyChanged("selectedIndex");
                }
            }
        }
        public IList<can_status> CanStatusList
        {
            get { return _canStatusList; }
            set { _canStatusList = value; OnPropertyChanged("CanConfigList"); }
        }

        public can_status CanStatus
        {
            get { return _canStatus; }
            set
            {
                _canStatus = value;
                OnPropertyChanged("CanConfig");
            }
        }

        private void LoadInDetails()
        {
            try
            {
                using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                {
                    CanStatusList = context.can_status.ToList();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg = ex.InnerException.Message;
                DialogManager.popup(msg);
            }
        }

        #region Command
        ICommand _selectionChangedCommand;
        public ICommand selectionChangedCommand
        {
            get
            {
                if (this._selectionChangedCommand == null)
                {
                    this._selectionChangedCommand = new DelegateCommand<can_status>(
                        (param) => { this.selectedCan = param; });
                }
                return this._selectionChangedCommand;
            }
        }

        can_status _selectedCan;
        public can_status selectedCan
        {
            get { return this._selectedCan; }
            set
            {
                if (value != null)
                {
                    _selectedCan = value;
                    using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                    {
                        CanStatus = context.can_status.Where(r => r.seqno == value.seqno).FirstOrDefault<can_status>();
                    }
                }
            }
        }

        public ICommand searchCommand
        {
            get { return new DelegateCommand<string>((e) => { this.searchData = e; this.Search(); }); }
        }

        protected bool Search()
        {
            using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
            {
                try
                {
                    CanStatusList = context.can_status.Where(r => (r.status_description.Contains(searchData))).ToList();
                    if (CanStatusList.Count > 0 && CanStatusList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var seqno = CanStatusList[_selectedIndex].seqno;
                        CanStatus = context.can_status.Where(r => r.seqno == seqno).FirstOrDefault<can_status>();
                        return true;
                    }
                    else
                        CanStatus = new can_status();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
