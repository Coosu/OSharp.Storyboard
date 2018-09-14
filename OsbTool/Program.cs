using Milkitic.OsbLib;
using System;
using System.IO;
using System.Windows.Forms;
using Milkitic.OsbLib.Extension;

namespace Milkitic.OsbTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var sb = File.ReadAllText(
                @"D:\Program Files (x86)\osu!\Songs\396495 yuiko - GLORIOUS_DAYS(Short)\yuiko - GLORIOUS_DAYS(Short) (Karen).osb");
            ElementGroup eg = ElementGroup.Parse(sb, 0);
            eg.Expand();
            File.WriteAllText(@"D:\Program Files (x86)\osu!\Songs\396495 yuiko - GLORIOUS_DAYS(Short)\output.osb",
                eg.ToString());
            return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
