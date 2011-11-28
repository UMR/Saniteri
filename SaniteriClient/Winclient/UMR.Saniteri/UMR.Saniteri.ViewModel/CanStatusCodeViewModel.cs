using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UMR.Saniteri.Data;
using UMR.Saniteri.Common;
using System.Windows.Input;
using UMR.Saniteri.Command;
using UMR.Saniteri.DataFactory;

namespace UMR.Saniteri.ViewModel
{
    public class CanStatusCodeViewModel : INotifyPropertyChanged
    {
        public CanStatusCodeViewModel()
        {
            CanStatusConfig = new can_statuscode();
            LoadInDetails();
            _canStatusConfig.SetButtonState(true);
        }

        private void AssignedPropChangedEventHandler()
        {
            CanStatusConfig.PropertyChanged -= new PropertyChangedEventHandler(CanConfig_PropertyChanged);
            CanStatusConfig.PropertyChanged += new PropertyChangedEventHandler(CanConfig_PropertyChanged);
        }

        private void CanConfig_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsEnable = false;
            CanStatusConfig.SetButtonState(false);
        }

        bool _IsEnable = true;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                _IsEnable = value;
                OnPropertyChanged("IsEnable");
            }
        }

        bool _isReadOnly;

        public bool IsReadOnly
        {
            get { return CanStatusConfigList.Where(can => can.status_type == this.CanStatusConfig.status_type).Count() <= 0; }
            set { _isReadOnly = value; OnPropertyChanged("IsReadOnly"); }
        }

        private void LoadInDetails()
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    CanStatusConfigList = context.can_statuscode.ToList();
                    if (CanStatusConfigList.Count > 0 && CanStatusConfigList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var _type = CanStatusConfigList[_selectedIndex].status_type;
                        CanStatusConfig = context.can_statuscode.Where(r => r.status_type == _type).FirstOrDefault<can_statuscode>();
                    }
                }
                IsEnable = true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg = ex.InnerException.Message;
                DialogManager.popup(msg);
            }
        }

        private IList<can_statuscode> _canStatusConfigList;
        private can_statuscode _canStatusConfig;
        private int _selectedIndex = 0;
        public string searchData { get; protected set; }

        public event EventHandler OnDirty;
        private void Dirty(bool _dirty)
        {
            if (OnDirty != null)
                OnDirty(this, new DirtyEventArgs(_dirty));
        }

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
        public IList<can_statuscode> CanStatusConfigList
        {
            get { return _canStatusConfigList; }
            set 
            { 
                _canStatusConfigList = value;
                OnPropertyChanged("CanStatusConfigList");
            }
        }

        public can_statuscode CanStatusConfig
        {
            get { return _canStatusConfig; }
            set
            {
                _canStatusConfig = value;
                _canStatusConfig.OnIsDirty -= new can_statuscode.IsDirtyHandler(_canConfig_OnIsDirty);
                _canStatusConfig.OnIsDirty += new can_statuscode.IsDirtyHandler(_canConfig_OnIsDirty);
                OnPropertyChanged("CanStatusConfig");
                OnPropertyChanged("IsReadOnly");
            }
        }

        void _canConfig_OnIsDirty(object sender, IsDirtyEventsArgs e)
        {
            Dirty(e.IsDirty);
        }

        #region Command

        ICommand _addCommand;
        public ICommand addCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    this._addCommand = new DelegateCommand(AddNew, () => { return CanStatusConfig.CanAdd; });
                }
                return this._addCommand;
            }
        }
        private void AddNew()
        {
            _selectedIndex = -1;
            CanStatusConfig = new can_statuscode();
            CanStatusConfig.SetButtonState(false);
            IsEnable = false;
        }

        ICommand _deleteCommand;
        public ICommand deleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    this._deleteCommand = new DelegateCommand(DeleteCommand, () => { return CanStatusConfigList != null && CanStatusConfigList.Count > 0 && CanStatusConfig.CanDelete && selectedIndex >= 0; });
                }
                return this._deleteCommand;
            }
        }

        private void DeleteCommand()
        {
            if (DialogManager.confirm("Do you really want to delete?", "Delete Confirmation"))
            {
                try
                {
                    using (var context = DatabaseManager.server.GetMainEntities())
                    {
                        var _type = _canStatusConfig.status_type;
                        var tmpCan = context.can_statuscode.Where(r => r.status_type == _type).FirstOrDefault<can_statuscode>();
                        context.DeleteObject(tmpCan);
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            result = selectedIndex;
                            _selectedIndex = -1;
                            LoadInDetails();
                            CanStatusConfig.SetButtonState(true);
                            selectedIndex = result > 1 ? result - 1 : 0;
                            if (CanStatusConfigList.Count <= 0)
                                CanStatusConfig = new can_statuscode();
                        }
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
        }

        ICommand _saveCommand;
        public ICommand saveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    this._saveCommand = new DelegateCommand(Save, () => { return CanStatusConfig.CanSaveConfig; });
                }
                return this._saveCommand;
            }
        }
        private void Save()
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    var newCan = context.can_statuscode.Where(r => r.status_type == CanStatusConfig.status_type).FirstOrDefault();
                    if (newCan == null) context.AddTocan_statuscode(CanStatusConfig);
                    else
                    {
                        var _type = _canStatusConfig.status_type;
                        var tmpCan = context.can_statuscode.Where(r => r.status_type == _type).FirstOrDefault<can_statuscode>();
                        context.Detach(tmpCan);
                        context.Attach(_canStatusConfig);
                        context.ObjectStateManager.ChangeObjectState(_canStatusConfig, System.Data.EntityState.Modified);
                    }
                    context.SaveChanges();
                    LoadInDetails();
                    CanStatusConfig.SetButtonState(true);
                    if (selectedIndex < 0)
                        selectedIndex = CanStatusConfigList.Count - 1;
                    else
                        OnPropertyChanged("selectedIndex");
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

        ICommand _cancelCommand;
        public ICommand cancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    this._cancelCommand = new DelegateCommand(CancelChanges, () => { return CanStatusConfig.CanCancel; });
                }
                return this._cancelCommand;
            }
        }
        private void CancelChanges()
        {
            LoadInDetails();
            selectedIndex = _selectedIndex >= 0 ? _selectedIndex : 0;
            CanStatusConfig.SetButtonState(true);
        }

        ICommand _selectionChangedCommand;
        public ICommand selectionChangedCommand
        {
            get
            {
                if (this._selectionChangedCommand == null)
                {
                    this._selectionChangedCommand = new DelegateCommand<can_statuscode>(
                        (param) => { this.selectedCustomer = param; });
                }
                return this._selectionChangedCommand;
            }
        }

        can_statuscode _selectedCustomer;
        public can_statuscode selectedCustomer
        {
            get { return this._selectedCustomer; }
            set
            {
                if (value != null)
                {
                    _selectedCustomer = value;
                    using (var context = DatabaseManager.server.GetMainEntities())
                    {
                        CanStatusConfig = context.can_statuscode.Where(r => r.status_type == value.status_type).FirstOrDefault<can_statuscode>();
                        CanStatusConfig.SetButtonState(true);
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
            using (var context = DatabaseManager.server.GetMainEntities())
            {
                try
                {
                    CanStatusConfigList = context.can_statuscode.Where(r => r.description.Contains(searchData)).ToList();
                    if (CanStatusConfigList.Count > 0 && CanStatusConfigList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var _type = CanStatusConfigList[_selectedIndex].status_type;
                        CanStatusConfig = context.can_statuscode.Where(r => r.status_type == _type).FirstOrDefault<can_statuscode>();
                        return true;
                    }
                    else
                        CanStatusConfig = new can_statuscode();
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
            AssignedPropChangedEventHandler();
        }
        #endregion
    }
}
