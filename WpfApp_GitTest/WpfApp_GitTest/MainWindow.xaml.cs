using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WpfApp_GitTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private dynamic VM
        {
            get { return this.DataContext; }
        }

        protected void link_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((Hyperlink)sender).NavigateUri.ToString());
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            this.VM.RemoveError("InputString");
        }
    }
}
