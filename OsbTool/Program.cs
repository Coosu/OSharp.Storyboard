using Milkitic.OsbLib;
using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Extension;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
            var path =
                @"D:\Program Files (x86)\osu!\Songs\240675 P-Light - FUNKY SUMMER BEACH\PLight - FUNKY SUMMER BEACH (Chordzi).osb";
            var sb = File.ReadAllText(path);
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            ElementGroup eg = ElementGroup.Parse(sb, 0);
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Restart();
            ElementGroup eg2 = ElementGroup.Parse(path);
            Console.WriteLine(sw.ElapsedMilliseconds);
            sw.Stop();

            foreach (var item in eg.ElementList)
                item.EventList = item.EventList.Where(e => e.EventType != EventEnum.Color).ToList();
            File.WriteAllText(@"D:\Program Files (x86)\osu!\Songs\470977 Mili - worldexecute(me);\output.osb",
                eg.ToString());
            eg.Expand();
            return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
