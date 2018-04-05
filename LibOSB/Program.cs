using LibOsb.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOsb
{
    static class Program
    {
        static void Main(string[] args)
        {
            StringBuilder ok = new StringBuilder();
            ok.AppendLine("Sprite,Foreground,Centre,\"11t-fonts\\E1.png\",320,240");
            //ok.AppendLine(" R,0,62113,,-0.056");
            //ok.AppendLine(" C,0,62113,,255,255,255");
            //ok.AppendLine(" P,0,62113,,A");
            //ok.AppendLine(" F,0,62113,62261,0,0.3");
            //ok.AppendLine(" S,0,62113,62575,0.5,0");
            //ok.AppendLine(" MX,0,62113,62575,500,270");
            //ok.AppendLine(" MY,0,62113,62575,200");
            //ok.AppendLine(" F,0,62427,62575,0.3,0");
            //ok.AppendLine(" L,62427,2");
            //ok.AppendLine("  V,0,0,100,320,240");

            ok.AppendLine(" F,0,300,1000,0,1");
            ok.AppendLine(" M,0,300,400,320,240");
            ok.AppendLine(" F,0,2000,,0");
            ok.AppendLine(" F,0,2300,2400,0,1");
            ok.AppendLine(" F,0,2500,,0");
            ok.AppendLine(" F,0,3000,,1");
            ok.AppendLine(" F,0,4000,4200,1,0");
            ok.AppendLine(" M,0,4000,4300,320,240");

            var obj2 = Element.Parse(ok.ToString());
            obj2.Examine();
            Console.WriteLine(obj2._FadeoutList);
            Console.Read();
            return;
        }
    }
}
