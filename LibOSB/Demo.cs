using LibOsb.Model.Constants;
using LibOsb.Tool;
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
            FolderManager fm = new FolderManager(@"C:\Users\acer\Downloads\Compressed\OsuStoryBroadPlayer_2\591442 S3RL feat Harri Rush - Nostalgic (Nightcore Mix)");
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
            var obj2 = Element.Parse(ok.ToString());
            obj2.SetPrivateDiff(fm.Difficulty["yf's Insane"]);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var text = System.IO.File.ReadAllText(@"D:\Program Files (x86)\osu!\Songs\591442 S3RL feat Harri Rush - Nostalgic (Nightcore Mix) (1)\S3RL feat Harri Rush - Nostalgic (Nightcore Mix) (nold_1702).bak");
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
            em.Save(@"D:\Program Files (x86)\osu!\Songs\591442 S3RL feat Harri Rush - Nostalgic (Nightcore Mix) (1)\parsed.osb");

            Console.Read();
            return;
        }
    }
}
