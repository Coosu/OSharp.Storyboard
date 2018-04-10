using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOsb
{
    static class Demo
    {
        static void Main(string[] args)
        {
            StringBuilder ok = new StringBuilder();
            ok.AppendLine("Sprite,Foreground,Centre,\"11t-fonts\\E1.png\",320,240");

            ok.AppendLine(" F,0,300,1000,0,1");
            ok.AppendLine(" S,0,300,,1");
            ok.AppendLine(" S,0,400,500,1");
            ok.AppendLine(" S,0,0,200,1");
            ok.AppendLine(" S,0,200,300,1");
            ok.AppendLine(" F,0,2000,,0");
            ok.AppendLine(" F,0,2300,2400,0,1");
            ok.AppendLine(" F,0,2500,,0");
            ok.AppendLine(" F,0,3000,,1");
            ok.AppendLine(" F,0,4000,4200,1,0");
            ok.AppendLine(" M,0,4000,4300,320,240");
            ok.AppendLine(" R,0,4000,,0");
            //var obj2 = Element.Parse(ok.ToString());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var text = System.IO.File.ReadAllText(@"D:\Program Files (x86)\osu!\Songs\470977 Mili - worldexecute(me);\Mili - world.execute(me); (Exile-).bak");
            Console.WriteLine("Read: " + sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            var parsed = ElementGroup.Parse(text, 0);
            Console.WriteLine("Parse: " + sw.ElapsedMilliseconds);
            sw.Reset();
            System.IO.File.WriteAllText("before.txt", parsed.ToString());
            sw.Start();
            parsed.Compress();
            Console.WriteLine("Compress: " + sw.ElapsedMilliseconds);
            sw.Reset();
            System.IO.File.WriteAllText("after.txt", parsed.ToString());
            Console.WriteLine(sw.ElapsedMilliseconds);
            parsed.CreateSprite(LayerType.Background, OriginType.Centre, "BG.jpg");
            ElementManager em = new ElementManager();
            em.Add(parsed);
            em.Save(@"D:\Program Files (x86)\osu!\Songs\470977 Mili - worldexecute(me);\parsed.osb");

            Console.Read();
            return;
        }
    }
}
