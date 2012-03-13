using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UMR.Saniteri.DataFactory;
using UMR.Saniteri.Command;
using System.Windows.Input;
using System.ComponentModel;
using UMR.Saniteri.Data;
using UMR.Saniteri.Common;

namespace UMR.Saniteri.ViewModel
{
    public class CanUsersViewModel : INotifyPropertyChanged
    {
        public CanUsersViewModel()
        {
            CanUser = new can_users();
            LoadInDetails();
            _canUser.SetButtonState(true);
        }

        private void AssignedPropChangedEventHandler()
        {
            CanUser.PropertyChanged -= new PropertyChangedEventHandler(CanUser_PropertyChanged);
            CanUser.PropertyChanged += new PropertyChangedEventHandler(CanUser_PropertyChanged);
        }

        private void CanUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsEnable = false;
            CanUser.SetButtonState(false);
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
            get { return CanUsersList.Where(can => can.user_id == this.CanUser.user_id).Count() <= 0; }
            set { _isReadOnly = value; OnPropertyChanged("IsReadOnly"); }
        }

        private void LoadInDetails()
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    CanUsersList = context.can_users.ToList();
                    if (CanUsersList.Count > 0 && CanUsersList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var id = CanUsersList[_selectedIndex].user_id;
                        CanUser = context.can_users.Where(r => r.user_id == id).FirstOrDefault<can_users>();
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

        private IList<can_users> _canUsersList;
        private can_users _canUser;
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
        public IList<can_users> CanUsersList
        {
            get { return _canUsersList; }
            set 
            { 
                _canUsersList = value;
                OnPropertyChanged("CanUsersList");
            }
        }

        public can_users CanUser
        {
            get { return _canUser; }
            set
            {
                _canUser = value;
                _canUser.OnIsDirty -=new can_users.IsDirtyHandler(_canUser_OnIsDirty);
                _canUser.OnIsDirty += new can_users.IsDirtyHandler(_canUser_OnIsDirty);
                OnPropertyChanged("CanUser");
                OnPropertyChanged("IsReadOnly");
            }
        }

        void _canUser_OnIsDirty(object sender, IsDirtyEventsArgs e)
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
                    this._addCommand = new DelegateCommand(AddNew, () => { return CanUser.CanAdd; });
                }
                return this._addCommand;
            }
        }
        private void AddNew()
        {
            _selectedIndex = -1;
            CanUser = new can_users();
            CanUser.SetButtonState(false);
            CanUser.IsNew = true;
            IsEnable = false;
        }

        ICommand _deleteCommand;
        public ICommand deleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    this._deleteCommand = new DelegateCommand(DeleteCommand, () => { return CanUsersList != null && CanUsersList.Count > 0 && CanUser.CanDelete && selectedIndex >= 0; });
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
                        var id = _canUser.user_id;
                        var tmpCan = context.can_users.Where(r => r.user_id == id).FirstOrDefault<can_users>();
                        context.DeleteObject(tmpCan);
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            result = selectedIndex;
                            _selectedIndex = -1;
                            LoadInDetails();
                            CanUser.SetButtonState(true);
                            selectedIndex = result > 1 ? result - 1 : 0;
                            if (CanUsersList.Count <= 0)
                                CanUser = new can_users();
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
                    this._saveCommand = new DelegateCommand(Save, () => { return CanUser.CanSaveConfig; });
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
                    var newCan = context.can_users.Where(r => r.user_id == CanUser.user_id).FirstOrDefault();
                    if (CanUser.IsNew && newCan != null) { DialogManager.popup("User already Exists."); return; }
                    if (newCan == null)
                        context.AddTocan_users(CanUser);
                    else
                    {
                        var id = _canUser.user_id;
                        var tmpCan = context.can_users.Where(r => r.user_id == id).FirstOrDefault<can_users>();
                        context.Detach(tmpCan);
                        context.Attach(_canUser);
                        context.ObjectStateManager.ChangeObjectState(_canUser, System.Data.EntityState.Modified);
                    }
                    context.SaveChanges();
                    LoadInDetails();
                    CanUser.SetButtonState(true);
                    if (selectedIndex < 0)
                        selectedIndex = CanUsersList.Count - 1;
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
            CanUser.IsNew = false;
        }

        ICommand _cancelCommand;
        public ICommand cancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    this._cancelCommand = new DelegateCommand(CancelChanges, () => { return CanUser.CanCancel; });
                }
                return this._cancelCommand;
            }
        }
        private void CancelChanges()
        {
            LoadInDetails();
            selectedIndex = _selectedIndex >= 0 ? _selectedIndex : 0;
            CanUser.SetButtonState(true);
            CanUser.IsNew = false;
        }

        ICommand _selectionChangedCommand;
        public ICommand selectionChangedCommand
        {
            get
            {
                if (this._selectionChangedCommand == null)
                {
                    this._selectionChangedCommand = new DelegateCommand<can_users>(
                        (param) => { this.selectedCustomer = param; });
                }
                return this._selectionChangedCommand;
            }
        }

        can_users _selectedCustomer;
        public can_users selectedCustomer
        {
            get { return this._selectedCustomer; }
            set
            {
                if (value != null)
                {
                    _selectedCustomer = value;
                    using (var context = DatabaseManager.server.GetMainEntities())
                    {
                        CanUser = context.can_users.Where(r => r.user_id == value.user_id).FirstOrDefault<can_users>();
                        CanUser.SetButtonState(true);
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
                    CanUsersList = context.can_users.Where(r => (r.user_id.Contains(searchData) 
                        || r.first_name.Contains(searchData)
                        || r.last_name.Contains(searchData)
                        || r.title.Contains(searchData))).ToList();
                    if (CanUsersList.Count > 0 && CanUsersList.Count >= _selectedIndex && _selectedIndex >= 0)
                    {
                        var id = CanUsersList[_selectedIndex].user_id;
                        CanUser = context.can_users.Where(r => r.user_id == id).FirstOrDefault<can_users>();
                        return true;
                    }
                    else
                        CanUser = new can_users();
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
