using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOsb.Enums;
using LibOsb.Utils;

namespace LibOsb
{
    static class Demo
    {
        static void Main(string[] args)
        {
            var ok = new StringBuilder();
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

            int g = 3;
            int startT = 0, endT = 1000, splitT = 10;
            double startX = 0, endX = 5000;

            double x = endX - startX;

            double sb = EasingType.EasingIn.Ease(g);
            Console.WriteLine(sb);
            Console.Read();
        }
    }
}
