using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UMR.Saniteri.ViewModel;

namespace UMR.Saniteri
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {
        bool _IsDirty;

        public bool IsDirty
        {
            get { return _IsDirty; }
            set { _IsDirty = value; }
        }

        public Shell()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayLoginScreen();
        }

        private void DisplayLoginScreen()
        {
            var frm = new Login();
            frm.Owner = this;
            this.Opacity = 0.75;
            frm.ShowDialog();
            if (!(frm.DialogResult.HasValue && frm.DialogResult.Value))
                this.Close();
            this.Opacity = 1;
            LoadView("CanConfig");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            container.Content = null;
            DisplayLoginScreen();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string _tag = ((MenuItem)sender).Tag.ToString();
            LoadView(_tag);
        }

        private void LoadView(string _viewName)
        {
            if (IsDirty)
            {
                if (MessageBox.Show("Do you want to discard current change ?", "Discard Change", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
                IsDirty = false;
            }
            switch (_viewName)
            {
                case "CanConfig":
                    var hm = new CanConfigViewModel();
                    hm.OnDirty += new EventHandler(hm_OnDirty);
                    container.Content = hm;
                    break;
                case "transactionLog":
                    var logView = new CanLogViewModel();
                    container.Content = logView;
                    break;
                case "statusCode":
                    var sc = new CanStatusCodeViewModel();
                    sc.OnDirty += new EventHandler(hm_OnDirty);
                    container.Content = sc;
                    break;
                case "status":
                    var status = new CanStatusViewModel();
                    container.Content = status;
                    break;
                case "EventCode":
                    break;
                default:
                    container.Content = null;
                    break;
            }
        }

        void hm_OnDirty(object sender, EventArgs e)
        {
            DirtyEventArgs de = e as DirtyEventArgs;
            IsDirty = de.IsDirty;
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (MessageBox.Show("Do you want to discard current changes?", "Discard Change", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
                IsDirty = false;
            }
            this.Close();
        }

        private void btnToolBar_Click(object sender, RoutedEventArgs e)
        {
            string _tag = ((Button)sender).Tag.ToString();
            LoadView(_tag);
        }
    }
}
