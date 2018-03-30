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

            ElementContainer.SortByIndex();

            Console.WriteLine(ElementContainer.ToString());

            string ok = "Sprite,Background,Centre,\"BG/OCHIBA1.jpg\",320,240\r\n F,0,26053,26099,0,0.06\r\n M,0,26053,38492,320,240,331,252\r\n S,1,26053,38492,0.64,0.67\r\n R,1,26053,38492,0,0.014\r\n F,0,26099,26145,0.122,0";
            var sb = Element.Parse(ok);

            Console.Read();
            return;
        }
    }
}
