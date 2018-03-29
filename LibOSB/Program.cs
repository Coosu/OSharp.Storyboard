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

            Console.Read();
            return;
        }
    }
}
