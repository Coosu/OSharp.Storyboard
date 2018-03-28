using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibOSB.Constants;

namespace LibOSB
{
    class DEMO
    {
        static string Test()
        {
            ElementGroup eg = new ElementGroup(0);
            var something = eg.CreateSprite("test.png");
            something.StartLoop(334, 5);
            something.Move(EasingType.Linear, 0, 300, 320, 240, 640, 480);
            something.Move(EasingType.BackInOut, 300, 600, 640, 480, 123, 45);
            something.EndLoop();
            return something.ToString();
        }
    }
}
