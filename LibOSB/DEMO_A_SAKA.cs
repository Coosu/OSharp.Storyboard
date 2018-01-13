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
            ElementGroup Sakuras = new ElementGroup(0);
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

                var Sakura = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                Sakura.Move(0, timeA, timeB, x1, y1, x2, y2);
                Sakura.Fade(timeA, timeB, 0.5);
                Sakura.Scale.Add(0, timeA, timeB, s, s);
                Sakura.Rotate.Add(0, timeA, timeB, rot1, rot2);
                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                Sakura._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                Sakuras.Add(Sakura);
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

                var Sakura2 = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                Sakura2.Move(0, timeA, timeB, x1, y1, x2, y2);
                Sakura2.Fade(timeA, timeB, 0.5);
                Sakura2.Scale.Add(0, timeA, timeB, s, s);
                Sakura2.Rotate.Add(0, timeA, timeB, rot1, rot2);
                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                Sakura2._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                Sakuras.Add(Sakura2);
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

                var Sakura3 = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                Sakura3.Move(0, timeA, timeB, x1, y1, x2, y2);
                Sakura3.Fade(timeA, timeB, 0.5);
                Sakura3.Scale.Add(0, timeA, timeB, s, s);
                Sakura3.Rotate.Add(0, timeA, timeB, rot1, rot2);
                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                Sakura3._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                Sakuras.Add(Sakura3);
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

                var Sakura4 = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, @"sakura.png", 320, 240);
                Sakura4.Move(0, timeA, timeB, x1, y1, x2, y2);
                Sakura4.Fade(timeA, timeB, 0.5);
                Sakura4.Scale.Add(0, timeA, timeB, s, s);
                Sakura4.Rotate.Add(0, timeA, timeB, rot1, rot2);

                byte Cr = (byte)rnd.Next(253, 255);
                byte Cg = (byte)rnd.Next(216, 255);
                byte Cb = (byte)rnd.Next(238, 255);
                Sakura4._Color.Add(0, timeA, timeB, Cr, Cg, Cb, Cr, Cg, Cb);

                Sakuras.Add(Sakura4);
            }
            return Sakuras.ToString();
        }

    }
}
