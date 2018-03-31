using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB
{
    static class Program
    {
        static void Main(string[] args)
        {
            ElementGroup scene2 = new ElementGroup(1);
            var tree = scene2.CreateSprite("tree.png");
            tree.Fade(EasingType.Linear, 0, 1000, 0, 1);
            tree.Fade(1000, 3000, 1);

            tree.StartLoop(3000, 5);
            tree.Move(EasingType.Linear, 0, 300, 320, 240, 640, 480);
            tree.Move(EasingType.BackInOut, 300, 600, 640, 480, 123, 45);
            tree.EndLoop();

            tree.StartLoop(56000, 5);
            tree.Move(0, 320, 240);
            tree.Move(300, 480, 320);
            tree.EndLoop();

            ElementGroup scene1 = new ElementGroup(0);
            var sky = scene1.CreateSprite("sky.png");
            sky.Fade(EasingType.Linear, 0, 1000, 0, 1);
            sky.Fade(3000, 3000, 1);

            // ElementManager.SortByIndex();

            //Console.WriteLine(ElementManager.ToString());
            StringBuilder ok = new StringBuilder();
            ok.AppendLine("Sprite,Foreground,Centre,\"11t-fonts\\E1.png\",320,240");
            ok.AppendLine(" R,0,62113,,-0.056");
            ok.AppendLine(" C,0,62113,,255,255,255");
            ok.AppendLine(" P,0,62113,,A");
            ok.AppendLine(" F,0,62113,62261,0,0.3");
            ok.AppendLine(" S,0,62113,62575,0.5,0");
            ok.AppendLine(" MX,0,62113,62575,500,270");
            ok.AppendLine(" MY,0,62113,62575,200");
            ok.AppendLine(" F,0,62427,62575,0.3,0");

            ok.AppendLine("Sprite,Foreground,Centre,\"11t-fonts\\y.png\",320,240");
            ok.AppendLine(" R,0,62113,,-0.05");
            ok.AppendLine(" C,0,62113,,255,255,255");
            ok.AppendLine(" P,0,62113,,A");
            ok.AppendLine(" F,0,62113,62261,0,0.3");
            ok.AppendLine(" S,0,62113,62575,0.5,0");
            ok.AppendLine(" MX,0,62113,62575,533,270");
            ok.AppendLine(" MY,0,62113,62575,198.35,200");
            ok.AppendLine(" F,0,62466,62614,0.3,0");
            // var text = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\test.txt");

            try
            {
                var sb = ElementGroup.Parse(ok.ToString(), 1);
                //Console.WriteLine(sb);
                sb = ElementManager.Adjust(sb, 1000000, 1000000, 1000000);
                Console.WriteLine(sb);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
            }
            Console.Read();
            return;
        }
    }
}
