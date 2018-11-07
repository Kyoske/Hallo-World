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

namespace WpfApp_GitTest.Views
{
    /// <summary>
    /// Tab1.xaml の相互作用ロジック
    /// </summary>
    public partial class Tab1 : UserControl
    {
        private CancellationTokenSource cts = null;
        private MainWindow mainwindow = null;

        public Tab1()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainwindow = Window.GetWindow(this) as MainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cts == null) 開始();
            else キャンセル();
        }

        private async void 開始()
        {
            var input = @"C:\Users\Valencia\Desktop\apptest\ソース";
            var output = @"C:\Users\Valencia\Desktop\apptest\出力";

            //ボタン
            testbutton.Content = "キャンセル";

            // キャンセル用トークンソース生成
            cts = new CancellationTokenSource();

            var result = await CollectPart.CollectAsync(input, output, cts.Token);

            if (result)
            {
                await mainwindow.ShowMessageAsync("完了!", "おわったよ!");
            }
            else
            {
                await mainwindow.ShowMessageAsync("完了!", "キャンセルされたよ！");
            }

            // キャンセル用トークンソース解放
            cts?.Dispose();
            cts = null;

            //ボタン
            testbutton.Content = "実行";
        }

        private void キャンセル()
        {
            // まだキャンセル要求されていない？
            if (cts?.IsCancellationRequested == false)
            {
                // キャンセル要求
                cts.Cancel();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainwindow.Flyouttest.IsOpen = true;
        }
    }
}
