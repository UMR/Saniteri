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
    public class CanEventCodeViewModel  : INotifyPropertyChanged
    {
        public CanEventCodeViewModel()
        {
            CanEventConfig = new can_eventcodes();
            LoadInDetails();
            _canEventConfig.SetButtonState(true);
        }

        private void AssignedPropChangedEventHandler()
        {
            CanEventConfig.PropertyChanged -= new PropertyChangedEventHandler(CanConfig_PropertyChanged);
            CanEventConfig.PropertyChanged += new PropertyChangedEventHandler(CanConfig_PropertyChanged);
        }

        private void CanConfig_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsEnable = false;
            CanEventConfig.SetButtonState(false);
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

        private void LoadInDetails()
        {
            try
            {
                using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                {
                    CanStatusConfigList = context.can_eventcodes.ToList();
                    if (CanStatusConfigList.Count > 0 && CanStatusConfigList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var _type = CanStatusConfigList[_selectedIndex].event_type;
                        CanEventConfig = context.can_eventcodes.Where(r => r.event_type == _type).FirstOrDefault<can_eventcodes>();
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

        private IList<can_eventcodes> _canEventConfigList;
        private can_eventcodes _canEventConfig;
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
        public IList<can_eventcodes> CanStatusConfigList
        {
            get { return _canEventConfigList; }
            set 
            { 
                _canEventConfigList = value;
                OnPropertyChanged("CanStatusConfigList");
            }
        }

        public can_eventcodes CanEventConfig
        {
            get { return _canEventConfig; }
            set
            {
                _canEventConfig = value;
                _canEventConfig.OnIsDirty -= new can_eventcodes.IsDirtyHandler(_canConfig_OnIsDirty);
                _canEventConfig.OnIsDirty += new can_eventcodes.IsDirtyHandler(_canConfig_OnIsDirty);
                OnPropertyChanged("CanStatusConfig");
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
                    this._addCommand = new DelegateCommand(AddNew, () => { return CanEventConfig.CanAdd; });
                }
                return this._addCommand;
            }
        }
        private void AddNew()
        {
            _selectedIndex = -1;
            CanEventConfig = new can_eventcodes();
            CanEventConfig.SetButtonState(false);
            IsEnable = false;
        }

        ICommand _deleteCommand;
        public ICommand deleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    this._deleteCommand = new DelegateCommand(DeleteCommand, () => { return CanStatusConfigList != null && CanStatusConfigList.Count > 0 && CanEventConfig.CanDelete && selectedIndex >= 0; });
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
                    using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                    {
                        var _type = _canEventConfig.event_type;
                        var tmpCan = context.can_eventcodes.Where(r => r.event_type == _type).FirstOrDefault<can_eventcodes>();
                        context.DeleteObject(tmpCan);
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            result = selectedIndex;
                            _selectedIndex = -1;
                            LoadInDetails();
                            CanEventConfig.SetButtonState(true);
                            selectedIndex = result > 1 ? result - 1 : 0;
                            if (CanStatusConfigList.Count <= 0)
                                CanEventConfig = new can_eventcodes();
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
                    this._saveCommand = new DelegateCommand(Save, () => { return CanEventConfig.CanSaveConfig; });
                }
                return this._saveCommand;
            }
        }
        private void Save()
        {
            try
            {
                using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                {
                    if (!(CanEventConfig.event_type > 0))
                        context.AddTocan_eventcodes(CanEventConfig);
                    else
                    {
                        var _type = _canEventConfig.event_type;
                        var tmpCan = context.can_eventcodes.Where(r => r.event_type == _type).FirstOrDefault<can_eventcodes>();
                        context.Detach(tmpCan);
                        context.Attach(_canEventConfig);
                        context.ObjectStateManager.ChangeObjectState(_canEventConfig, System.Data.EntityState.Modified);
                    }
                    context.SaveChanges();
                    LoadInDetails();
                    CanEventConfig.SetButtonState(true);
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
                    this._cancelCommand = new DelegateCommand(CancelChanges, () => { return CanEventConfig.CanCancel; });
                }
                return this._cancelCommand;
            }
        }
        private void CancelChanges()
        {
            LoadInDetails();
            selectedIndex = _selectedIndex >= 0 ? _selectedIndex : 0;
            CanEventConfig.SetButtonState(true);
        }

        ICommand _selectionChangedCommand;
        public ICommand selectionChangedCommand
        {
            get
            {
                if (this._selectionChangedCommand == null)
                {
                    this._selectionChangedCommand = new DelegateCommand<can_eventcodes>(
                        (param) => { this.selectedCustomer = param; });
                }
                return this._selectionChangedCommand;
            }
        }

        can_eventcodes _selectedCustomer;
        public can_eventcodes selectedCustomer
        {
            get { return this._selectedCustomer; }
            set
            {
                if (value != null)
                {
                    _selectedCustomer = value;
                    using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                    {
                        CanEventConfig = context.can_eventcodes.Where(r => r.event_type == value.event_type).FirstOrDefault<can_eventcodes>();
                        CanEventConfig.SetButtonState(true);
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
                    CanStatusConfigList = context.can_eventcodes.Where(r => r.description.Contains(searchData)).ToList();
                    if (CanStatusConfigList.Count > 0 && CanStatusConfigList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var _type = CanStatusConfigList[_selectedIndex].event_type;
                        CanEventConfig = context.can_eventcodes.Where(r => r.event_type == _type).FirstOrDefault<can_eventcodes>();
                        return true;
                    }
                    else
                        CanEventConfig = new can_eventcodes();
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
