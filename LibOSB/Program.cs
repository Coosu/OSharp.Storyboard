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
            ElementGroup eg = new ElementGroup(0);
            var something = eg.CreateSprite("test.png");
            something.Fade(EasingType.Linear, 0, 1000, 0, 1);
            something.Fade(1000, 3000, 1);

            something.StartLoop(3000, 5);
            something.Move(EasingType.Linear, 0, 300, 320, 240, 640, 480);
            something.Move(EasingType.BackInOut, 300, 600, 640, 480, 123, 45);
            something.EndLoop();

            something.StartLoop(56000, 5);
            something.Move(0, 320, 240);
            something.Move(300, 480, 320);
            something.EndLoop();
            
            Console.WriteLine(something.ToString());
            Console.Read();
            return;
        }
    }
}
