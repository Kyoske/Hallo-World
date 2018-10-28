using System;
using System.IO;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace github_hello
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new int[] { 4, 2, 3, 1 };

            Console.WriteLine(Min(a).ToString());

            var path = @"C:\Users\Valencia\Desktop\TEST.xlsx";
            using (var sr = new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
            {
                var book = WorkbookFactory.Create(path);
                System.Windows.Forms.MessageBox.Show(book.GetSheetName(0));
            }
        }

        private static int Min(int[] a)
        {
            ref var min = ref a[0];

            for (int i = 0; i < a.Length; i++)
            {
                if (min > a[i]) min = ref a[i];
            }

            return min;
        }
    }
}
