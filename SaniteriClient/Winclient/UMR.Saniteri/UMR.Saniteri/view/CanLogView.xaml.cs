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

namespace UMR.Saniteri.view
{
    /// <summary>
    /// Interaction logic for CanLogView.xaml
    /// </summary>
    public partial class CanLogView : UserControl
    {
        public CanLogView()
        {
            InitializeComponent();
        }

        private void mainBdr_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var height = ((Border)sender).ActualHeight;
            dataGrd.MaxHeight = height - 40;
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var height = ((Border)sender).ActualHeight;
            grdTransaction.MaxHeight = height - 50;
            grdMaintenance.MaxHeight = height - 50;
        }
    }
}
