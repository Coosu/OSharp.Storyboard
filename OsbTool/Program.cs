using Milkitic.OsbLib;
using Milkitic.OsbLib.Extension;
using System;
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
            var sb = File.ReadAllText(
                @"D:\Program Files (x86)\osu!\Songs\470977 Mili - worldexecute(me);\Mili - world.execute(me); (Exile-).osb");
            ElementGroup eg = ElementGroup.Parse(sb, 0);
            foreach (var item in eg.ElementList)
                item.EventList = item.EventList.Where(e => e.EventType != OsbLib.Models.EventEnum.Color).ToList();
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
