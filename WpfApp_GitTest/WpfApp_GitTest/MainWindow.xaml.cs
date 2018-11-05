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
        private CancellationTokenSource cts = null;

        public MainWindow()
        {
            InitializeComponent();
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
                await this.ShowMessageAsync("完了!", "おわったよ!");
            }
            else
            {
                await this.ShowMessageAsync("完了!", "キャンセルされたよ！");
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
            Flyouttest.IsOpen = true;
        }
    }
}
