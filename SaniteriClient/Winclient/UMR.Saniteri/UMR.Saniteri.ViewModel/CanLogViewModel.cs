﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using UMR.Saniteri.Command;
using UMR.Saniteri.Data;
using UMR.Saniteri.Common;

namespace UMR.Saniteri.ViewModel
{
    public class CanLogViewModel : INotifyPropertyChanged
    {
        public CanLogViewModel()
        {
            LoadInDetails();
        }
       
        private IList<can_inventory> _canConfigList;
        private IList<can_transaction_log> _canEventsList;
        private IList<can_maintenance> _canMaintenanceList;

        private can_inventory _canConfig;
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
        public IList<can_inventory> CanConfigList
        {
            get { return _canConfigList; }
            set { _canConfigList = value; OnPropertyChanged("CanConfigList"); }
        }

        public IList<can_transaction_log> CanEventsList
        {
            get { return _canEventsList; }
            set { _canEventsList = value; OnPropertyChanged("CanEventsList"); }
        }

        public IList<can_maintenance> CanMaintenanceList
        {
            get { return _canMaintenanceList; }
            set { _canMaintenanceList = value; OnPropertyChanged("CanMaintenanceList"); }
        }

        public can_inventory CanConfig
        {
            get { return _canConfig; }
            set
            {
                _canConfig = value;
                OnPropertyChanged("CanConfig");
            }
        }

        private void LoadInDetails()
        {
            try
            {
                using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                {
                    CanConfigList = context.can_inventory.ToList();
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
                    this._selectionChangedCommand = new DelegateCommand<can_inventory>(
                        (param) => { this.selectedCan = param; });
                }
                return this._selectionChangedCommand;
            }
        }

        can_inventory _selectedCan;
        public can_inventory selectedCan
        {
            get { return this._selectedCan; }
            set
            {
                if (value != null)
                {
                    _selectedCan = value;
                    using (var context = UMR.Saniteri.Data.DBManager.GetMainEntities())
                    {
                        CanConfig = context.can_inventory.Where(r => r.can_id == value.can_id).FirstOrDefault<can_inventory>();
                        CanEventsList = context.can_transaction_log.Where(r => r.can_id == value.can_id).ToList<can_transaction_log>();
                        CanMaintenanceList = context.can_maintenance.Where(r => r.can_id == value.can_id).ToList<can_maintenance>();
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
        }
        #endregion
    }
}
