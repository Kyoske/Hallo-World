using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Threading;

namespace WpfApp_GitTest
{
    public static class CollectPart
    {
        /* test */
        public static async Task<bool> CollectAsync(string Input, string Output, CancellationToken token)
        {
            var リスト = new Dictionary<string, string>();

            var ファイルリスト = Directory.EnumerateFiles(Input, "*.*", SearchOption.AllDirectories)
                           .AsParallel()
                           .Where(c => 拡張子判定(c) || Path.GetExtension(c) == ".zip")
                           .Select(c => c);

            //非同期
            var result = await Task.Run(() =>
            {
                foreach (var path in ファイルリスト)
                {
                    //キャンセルされたら途中で終了
                    if (token.IsCancellationRequested)
                    {
                        return false;
                    }

                    switch (Path.GetExtension(path))
                    {
                        case ".c":
                        case ".h":
                            OpenFileCopy(path, Output);
                            break;
                        case ".zip": //zipファイルは特別
                                OpenZipCopy(path, Output);
                            break;
                        default:
                            break;
                    }
                }
                return true;
            }, token);

            return result; 
        }

        private static string 部品No取得(string 加工前) => 加工前?.Replace("/*", "")?.Replace("*/", "")?.Trim() ?? "";

        private static void OpenFileCopy(string path, string outputPath)
        {
            var ヘッダ = File.ReadLines(path).FirstOrDefault();
            var 部品No = 部品No取得(ヘッダ);
            var ファイル名 = Path.GetFileName(path);

            if (string.IsNullOrEmpty(部品No)) return;

            var 出力パス = Path.Combine(outputPath, 部品No);
            if (!Directory.Exists(出力パス)) Directory.CreateDirectory(出力パス);

            var 出力ファイル名 = Path.Combine(出力パス,ファイル名);
            File.Copy(path, 出力ファイル名, true);
        }

        private static void OpenZipCopy(string path, string outputPath)
        {          
            //ZIP書庫を開く
            using (var a = ZipFile.OpenRead(path))
            {
                foreach (var e in a.Entries)
                {
                    if (拡張子判定(e.Name))
                    {
                        var 部品No = "";
                        using (var sr = new StreamReader(e.Open(), Encoding.GetEncoding("shift_jis")))
                        {
                            //1行目のみ。空白の場合は中断
                            部品No = 部品No取得(sr.ReadLine());
                            if (string.IsNullOrEmpty(部品No)) continue;
                        }

                        var 出力パス = Path.Combine(outputPath, 部品No);
                        if (!Directory.Exists(出力パス)) Directory.CreateDirectory(出力パス);

                        var 出力ファイル名 = Path.Combine(出力パス, e.Name);
                        e.ExtractToFile(出力ファイル名, true);
                    }
                }
            }
        }

        private static bool 拡張子判定(string path)
        {
            if (Path.GetExtension(path) == ".c" || Path.GetExtension(path) == ".h") return true;
            else return false;                
        }
    }
}
