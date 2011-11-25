using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UMR.Saniteri.Data;
using UMR.Saniteri.Command;
using System.Windows.Input;
using UMR.Saniteri.Common;
using UMR.Saniteri.DataFactory;

namespace UMR.Saniteri.ViewModel
{
    public class CanConfigViewModel : INotifyPropertyChanged
    {
        public CanConfigViewModel()
        {
            CanConfig = new can_inventory();
            CanConfig.in_service_date = DateTime.Now;
            LoadInDetails();
            _canConfig.SetButtonState(true);
        }

        private void AssignedPropChangedEventHandler()
        {
            CanConfig.PropertyChanged -= new PropertyChangedEventHandler(CanConfig_PropertyChanged);
            CanConfig.PropertyChanged += new PropertyChangedEventHandler(CanConfig_PropertyChanged);
        }

        private void CanConfig_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsEnable = false;
            CanConfig.SetButtonState(false);
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
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    CanConfigList = context.can_inventory.ToList();
                    if (CanConfigList.Count > 0 && CanConfigList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var id = CanConfigList[_selectedIndex].can_id;
                        CanConfig = context.can_inventory.Where(r => r.can_id == id).FirstOrDefault<can_inventory>();
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
        
        private IList<can_inventory> _canConfigList;
        private can_inventory _canConfig;
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
        public IList<can_inventory> CanConfigList
        {
            get { return _canConfigList; }
            set 
            { 
                _canConfigList = value;
                OnPropertyChanged("CanConfigList");
            }
        }

        public can_inventory CanConfig
        {
            get { return _canConfig; }
            set
            {
                _canConfig = value;
                _canConfig.OnIsDirty -=new can_inventory.IsDirtyHandler(_canConfig_OnIsDirty);
                _canConfig.OnIsDirty += new can_inventory.IsDirtyHandler(_canConfig_OnIsDirty);
                OnPropertyChanged("CanConfig");
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
                    this._addCommand = new DelegateCommand(AddNew, () => { return CanConfig.CanAdd; });
                }
                return this._addCommand;
            }
        }
        private void AddNew()
        {
            _selectedIndex = -1;
            CanConfig = new can_inventory();
            CanConfig.in_service_date = DateTime.Now;
            CanConfig.SetButtonState(false);
            IsEnable = false;
        }

        ICommand _deleteCommand;
        public ICommand deleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    this._deleteCommand = new DelegateCommand(DeleteCommand, () => { return CanConfigList != null && CanConfigList.Count > 0 && CanConfig.CanDelete && selectedIndex >= 0; });
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
                        var id = _canConfig.can_id;
                        var tmpCan = context.can_inventory.Where(r => r.can_id == id).FirstOrDefault<can_inventory>();
                        context.DeleteObject(tmpCan);
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            result = selectedIndex;
                            _selectedIndex = -1;
                            LoadInDetails();
                            CanConfig.SetButtonState(true);
                            selectedIndex = result > 1 ? result - 1 : 0;
                            if (CanConfigList.Count <= 0)
                                CanConfig = new can_inventory();
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
                    this._saveCommand = new DelegateCommand(Save, () => { return CanConfig.CanSaveConfig; });
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
                    if (!(CanConfig.can_id > 0))
                    {
                        //CanConfig.can_id = Guid.NewGuid();
                        context.AddTocan_inventory(CanConfig);
                    }
                    else
                    {
                        var id = _canConfig.can_id;
                        var tmpCan = context.can_inventory.Where(r => r.can_id == id).FirstOrDefault<can_inventory>();
                        context.Detach(tmpCan);
                        context.Attach(_canConfig);
                        context.ObjectStateManager.ChangeObjectState(_canConfig, System.Data.EntityState.Modified);
                    }
                    context.SaveChanges();
                    LoadInDetails();
                    CanConfig.SetButtonState(true);
                    if (selectedIndex < 0)
                        selectedIndex = CanConfigList.Count - 1;
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
                    this._cancelCommand = new DelegateCommand(CancelChanges, () => { return CanConfig.CanCancel; });
                }
                return this._cancelCommand;
            }
        }
        private void CancelChanges()
        {
            LoadInDetails();
            selectedIndex = _selectedIndex >= 0 ? _selectedIndex : 0;
            CanConfig.SetButtonState(true);
        }

        ICommand _selectionChangedCommand;
        public ICommand selectionChangedCommand
        {
            get
            {
                if (this._selectionChangedCommand == null)
                {
                    this._selectionChangedCommand = new DelegateCommand<can_inventory>(
                        (param) => { this.selectedCustomer = param; });
                }
                return this._selectionChangedCommand;
            }
        }

        can_inventory _selectedCustomer;
        public can_inventory selectedCustomer
        {
            get { return this._selectedCustomer; }
            set
            {
                if (value != null)
                {
                    _selectedCustomer = value;
                    using (var context = DatabaseManager.server.GetMainEntities())
                    {
                        CanConfig = context.can_inventory.Where(r => r.can_id == value.can_id).FirstOrDefault<can_inventory>();
                        CanConfig.SetButtonState(true);
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
                    CanConfigList = context.can_inventory.Where(r => (r.ip_address.Contains(searchData) 
                        || r.street.Contains(searchData)
                        || r.city.Contains(searchData)
                        || r.state.Contains(searchData)
                        || r.floor.Contains(searchData)
                        || r.room.Contains(searchData)
                        || r.zip.Contains(searchData))).ToList();
                    if (CanConfigList.Count > 0 && CanConfigList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var id = CanConfigList[_selectedIndex].can_id;
                        CanConfig = context.can_inventory.Where(r => r.can_id == id).FirstOrDefault<can_inventory>();
                        return true;
                    }
                    else
                        CanConfig = new can_inventory();
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
