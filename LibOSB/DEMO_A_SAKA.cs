using LibOSB.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB
{
    class DEMO_A_SAKA
    {
        public static string Sakura()
        {
            var rnd = new Random();
            ElementGroup sakuras = new ElementGroup(0);
            const double degree = 135f / 180 * Math.PI;

            //
            // Saku1
            //
            for (int i = -500; i < 4000; i++)
            {
                int x1, y1;
                double x2, y2;
                double s, r;
                double rot1, rot2;

                int timeA, timeB;

                x1 = rnd.Next(-807, 747);
                y1 = -200;

                //r = Math.Pow(s, 2);
                r = 1100;

                rot1 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;
                rot2 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;

                s = rnd.NextDouble() * 1;

                //timeA = i * 20;
                //timeB = timeA + (int)(4000 * Math.Pow(s, -1));
                timeA = i * 40;
                timeB = timeA + (int)(7000 * Math.Pow(s, -1));

                x2 = x1 + r * Math.Sin(degree);
                y2 = y1 - r * Math.Cos(degree);

                var sakura1 = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                sakura1.Move(0, timeA, timeB, x1, y1, x2, y2);
                sakura1.Fade(timeA, timeB, 0.5);
                sakura1.Scale(timeA, timeB, s);
                sakura1.Rotate(0, timeA, timeB, rot1, rot2);
                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                sakura1._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                sakuras.Add(sakura1);
            }

            //
            // Saku2
            //
            for (int i = 0; i < 250; i++)
            {
                int x1, y1;
                double x2, y2;
                double s, r;
                double rot1, rot2;

                int timeA, timeB;
                x1 = rnd.Next(-807, 747);
                y1 = -200;

                //r = Math.Pow(s, 2);
                r = 1100;

                rot1 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;
                rot2 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;


                x2 = x1 + r * Math.Sin(degree);
                y2 = y1 - r * Math.Cos(degree);

                s = rnd.NextDouble() * 2 + 1;

                timeA = i * 500;
                timeB = timeA + (int)(7000 * Math.Pow(s, -1));

                var sakura2 = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                sakura2.Move(0, timeA, timeB, x1, y1, x2, y2);
                sakura2.Fade(timeA, timeB, 0.5);
                sakura2.Scale(timeA, timeB, s);
                sakura2.Rotate(0, timeA, timeB, rot1, rot2);
                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                sakura2._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                sakuras.Add(sakura2);
            }

            //
            // Saku3
            //
            for (int i = 0; i < 50; i++)
            {
                int x1, y1;
                double x2, y2;
                double s, r;
                double rot1, rot2;

                int timeA, timeB;
                x1 = rnd.Next(-807, 747);
                y1 = -200;

                //r = Math.Pow(s, 2);
                r = 1100;

                rot1 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;
                rot2 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;


                x2 = x1 + r * Math.Sin(degree);
                y2 = y1 - r * Math.Cos(degree);

                s = rnd.NextDouble() * 2 + 3;

                timeA = i * 5000;
                timeB = timeA + (int)(7000 * Math.Pow(s, -1));

                var sakura3 = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                sakura3.Move(0, timeA, timeB, x1, y1, x2, y2);
                sakura3.Fade(timeA, timeB, 0.5);
                sakura3.Scale(timeA, timeB, s);
                sakura3.Rotate(0, timeA, timeB, rot1, rot2);
                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                sakura3._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                sakuras.Add(sakura3);
            }

            //
            // Saku4
            //
            for (int i = 0; i < 20; i++)
            {
                int x1, y1;
                double x2, y2;
                double s, r;
                double rot1, rot2;

                int timeA, timeB;
                x1 = rnd.Next(-807, 747);
                y1 = -200;

                //r = Math.Pow(s, 2);
                r = 1100;

                rot1 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;
                rot2 = rnd.NextDouble() * Math.PI * 4 - Math.PI * 2;


                x2 = x1 + r * Math.Sin(degree);
                y2 = y1 - r * Math.Cos(degree);

                s = rnd.NextDouble() * 5 + 6;

                timeA = i * 10000;
                timeB = timeA + (int)(7000 * Math.Pow(s, -1));

                var sakura4 = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                sakura4.Move(0, timeA, timeB, x1, y1, x2, y2);
                sakura4.Fade(timeA, timeB, 0.5);
                sakura4.Scale(timeA, timeB, s);
                sakura4.Rotate(0, timeA, timeB, rot1, rot2);

                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                sakura4._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                sakuras.Add(sakura4);
            }
            return sakuras.ToString();
        }

    }
}
