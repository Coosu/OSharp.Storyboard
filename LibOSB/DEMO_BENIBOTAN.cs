using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOSB.Constants;
namespace LibOSB
{
    static class DEMO_BENIBOTAN
    {
        public static string BGOrange()
        {
            SBGroup BGs = new SBGroup(0);
            int x;
            for (int i = 0; i < 40; i++)
            {
                x = i * 22;
                //var BG = new SBObject(Types.Sprite, Layers.Foreground, Origins.CentreLeft, @"SB\2dx_7.png", 320, 240);
                //BG.Move(125, 5097, x, 240);
                //BGs.Add(BG);
                var BG = BGs.CreateSprite(Origins.CentreLeft, @"SB\2dx_7.png");
                BG.Move(125, 5097, x, 240);
            }

            return BGs.ToString();
        }
        public static string BGpatterns()
        {
            SBGroup BGpatterns = new SBGroup(0);

            double x, y;
            for (int j = -1; j < 9; j++)
            {
                for (int i = 0; i < (j % 2 == 0 ? 10 : 11); i++)
                {
                    x = i * (68 - 3.5);
                    if (j % 2 != 0) x -= 32;
                    y = j * (78 * 0.75 - 5);
                    var Pattern = new SBObject(Types.Sprite, Layers.Foreground, Origins.TopLeft, @"SB\2dx_76.png", 320, 240);
                    Pattern._Move.Add(0, 125, 5097, x, y, x, y);
                    Pattern._Fade.Add(0, 125, 125, 0.3, 0.3);

                    BGpatterns.Add(Pattern);
                }
            }
            return BGpatterns.ToString();
        }
        public static string SceneTransform()
        {
            SBGroup Patterns = new SBGroup(0);

            double x, y;

            for (int i = 0; i < 11; i++)
            {
                x = i * 64;
                y = 240;
                var Pattern = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\w2.png", 320, 240);
                Pattern._Move.Add(0, 4434, 5097, x, y, x, y);
                Pattern.Vector.Add(0, 4434, 5097, 0, 1, 1, 1);
                Pattern._Fade.Add(0, 4434, 5097, 0, 1);
                Pattern._Fade.Add(0, 5097, 6092, 1, 1);

                Patterns.Add(Pattern);
            }

            return Patterns.ToString();
        }
        public static string BGPurple()
        {
            using (SBGroup BGs = new SBGroup(0))
            {
                int x;
                for (int i = 0; i < 29; i++)
                {
                    x = i * 22;
                    var BG = new SBObject(Types.Sprite, Layers.Foreground, Origins.CentreLeft, @"SB\2dx_8.png", 320, 240);
                    BG._Move.Add(0, 5926, 6423, x + 640, 240, x, 240);
                    BG._Fade.Add(0, 11064, 11064, 1, 1);
                    BG._Move.Add(0, 10567, 11064, x, 240, x - 640, 240);
                    BGs.Add(BG);
                }
            }
            using (SBGroup BGpatterns = new SBGroup(1))
            {
                double x, y;
                for (int j = -1; j < 11; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        x = i * (170 - 170 / 5) - 85;
                        y = j * (58 - 29 / 2 - 1);
                        var Pattern = new SBObject(Types.Sprite, Layers.Foreground, Origins.TopLeft, @"SB\2dx_5.png", 320, 240);
                        Pattern._Move.Add(0, 5926, 6423, x + 640, y, x, y);
                        Pattern._Fade.Add(0, 11064, 11064, 0.9, 0.9);
                        Pattern._Move.Add(0, 10567, 11064, x, y, x - 640, y);

                        BGpatterns.Add(Pattern);
                    }
                }
            }
            using (SBGroup Frame = new SBGroup(3))
            {
                int x = 320, y = 240;
                var TL = new SBObject(Types.Sprite, Layers.Foreground, Origins.TopLeft, @"SB\2dx_30.png", 320, 240);
                TL._Move.Add(0, 5926, 6423, x + 640, y - 2, x, y - 2);
                TL._Move.Add(0, 10567, 11064, x, y - 2, x - 640, y - 2);
                TL._Fade.Add(0, 11064, 11064, 1, 1);
                TL.Parameter.Add(0, 5926, 5926, "V");
                TL.Parameter.Add(0, 5926, 5926, "H");

                var TR = new SBObject(Types.Sprite, Layers.Foreground, Origins.TopRight, @"SB\2dx_30.png", 320, 240);
                TR._Move.Add(0, 5926, 6423, x + 640, y - 2, x, y - 2);
                TR._Move.Add(0, 10567, 11064, x, y - 2, x - 640, y - 2);
                TR._Fade.Add(0, 11064, 11064, 1, 1);
                TR.Parameter.Add(0, 5926, 5926, "V");

                var BL = new SBObject(Types.Sprite, Layers.Foreground, Origins.BottomLeft, @"SB\2dx_30.png", 320, 240);
                BL._Move.Add(0, 5926, 6423, x + 640, y + 2, x, y + 2);
                BL._Move.Add(0, 10567, 11064, x, y + 2, x - 640, y + 2);
                BL._Fade.Add(0, 11064, 11064, 1, 1);
                BL.Parameter.Add(0, 5926, 5926, "H");

                var BR = new SBObject(Types.Sprite, Layers.Foreground, Origins.BottomRight, @"SB\2dx_30.png", 320, 240);
                BR._Move.Add(0, 5926, 6423, x + 640, y + 2, x, y + 2);
                BR._Move.Add(0, 10567, 11064, x, y + 2, x - 640, y + 2);
                BR._Fade.Add(0, 11064, 11064, 1, 1);

                Frame.Add(TL, TR, BL, BR);

            }
            using (SBGroup Panel = new SBGroup(2))
            {

                int x = 320, y = 240;

                var Black = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\panel.jpg", 320, 240);
                Black._Move.Add(0, 5926, 6423, x + 640, y, x, y);
                Black._Fade.Add(0, 11064, 11064, 1, 1);
                Black._Move.Add(0, 10567, 11064, x, y, x - 640, y);

                Panel.Add(Black);

            }
            using (SBGroup Title = new SBGroup(4))
            {

                int x = 320, y = 240;
                var rnd = new Random();
                double rndx, rndy;

                var t = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\2dx_97.png", 320, 240);
                t._Move.Add(0, 5926, 6423, x + 640, y, x, y);
                t._Fade.Add(0, 11064, 11064, 1, 1);
                t._Move.Add(0, 10567, 11064, x, y, x - 640, y);
                t.Loop.Add(6423, (int)((10567 - 6423) / 150d));
                for (int i = 0; i < 4; i++)
                {
                    rndx = rnd.NextDouble() * 2;
                    rndy = rnd.NextDouble() * 2;
                    t.Loop[0].Move.Add(0, i * 50, i * 50, x + rndx, y + rndy, x + rndx, y + rndy);
                }
                Title.Add(t);

            }
            return SBContainer.ToString();
        }
        public static string BGGreen()
        {
            using (SBGroup BGs = new SBGroup(0))
            {
                int x;
                for (int i = 0; i < 29; i++)
                {
                    x = i * 22;
                    var BG = new SBObject(Types.Sprite, Layers.Foreground, Origins.CentreLeft, @"SB\2dx_6.png", 320, 240);
                    BG._Move.Add(0, 10567, 11064, x + 640, 240, x, 240);
                    BG._Fade.Add(0, 17031, 17031, 1, 1);
                    BGs.Add(BG);
                }
            }
            using (SBGroup BGpatterns = new SBGroup(1))
            {
                double x, y;
                for (int j = 0; j < 7; j++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        x = i * (78 - 3) - 3;
                        y = j * (78 - 3) - 3;
                        var Pattern = new SBObject(Types.Sprite, Layers.Foreground, Origins.TopLeft, @"SB\2dx_98.png", 320, 240);
                        Pattern._Move.Add(0, 10567, 11064, x + 640, y, x, y);
                        Pattern._Fade.Add(0, 17031, 17031, 0.9, 0.9);

                        BGpatterns.Add(Pattern);
                    }
                }
            }
            using (SBGroup Frame = new SBGroup(3))
            {
                int x = 320, y = 240;
                var TL = new SBObject(Types.Sprite, Layers.Foreground, Origins.TopLeft, @"SB\2dx_30.png", 320, 240);
                TL._Move.Add(0, 10567, 11064, x + 640, y - 2, x, y - 2);
                TL._Move.Add(0, 14048, 14545, x, y - 2, x - 640, y - 2);
                TL.Parameter.Add(0, 5926, 5926, "V");
                TL.Parameter.Add(0, 5926, 5926, "H");

                var TR = new SBObject(Types.Sprite, Layers.Foreground, Origins.TopRight, @"SB\2dx_30.png", 320, 240);
                TR._Move.Add(0, 10567, 11064, x + 640, y - 2, x, y - 2);
                TR._Move.Add(0, 14048, 14545, x, y - 2, x - 640, y - 2);
                TR.Parameter.Add(0, 5926, 5926, "V");

                var BL = new SBObject(Types.Sprite, Layers.Foreground, Origins.BottomLeft, @"SB\2dx_30.png", 320, 240);
                BL._Move.Add(0, 10567, 11064, x + 640, y + 2, x, y + 2);
                BL._Move.Add(0, 14048, 14545, x, y + 2, x - 640, y + 2);
                BL.Parameter.Add(0, 5926, 5926, "H");

                var BR = new SBObject(Types.Sprite, Layers.Foreground, Origins.BottomRight, @"SB\2dx_30.png", 320, 240);
                BR._Move.Add(0, 10567, 11064, x + 640, y + 2, x, y + 2);
                BR._Move.Add(0, 14048, 14545, x, y + 2, x - 640, y + 2);

                Frame.Add(TL, TR, BL, BR);

            }
            using (SBGroup Panel = new SBGroup(2))
            {

                int x = 320, y = 240;

                var Black = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\panel.jpg", 320, 240);
                Black._Move.Add(0, 10567, 11064, x + 640, y, x, y);
                Black._Fade.Add(0, 17031, 17031, 1, 1);
                Black._Move.Add(0, 14048, 14545, x, y, x - 640, y);

                Panel.Add(Black);

            }
            using (SBGroup Title = new SBGroup(4))
            {

                int x = 320, y = 240;
                var rnd = new Random();
                double rndx, rndy;

                var t = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\2dx_60.png", 320, 240);
                t._Move.Add(0, 10567, 11064, x + 640, y, x, y);
                t._Move.Add(0, 14048, 14545, x, y, x - 640, y);
                t.Loop.Add(11064, (int)((14048 - 11064) / 150d));
                for (int i = 0; i < 4; i++)
                {
                    rndx = rnd.NextDouble() * 2;
                    rndy = rnd.NextDouble() * 2;
                    t.Loop[0].Move.Add(0, i * 50, i * 50, x + rndx, y + rndy, x + rndx, y + rndy);
                }
                t.Loop.Add(10567, (int)((14545 - 10567) / 100d));
                t.Loop[1].Fade.Add(0, 0, 50, 1, 0.5);
                t.Loop[1].Fade.Add(0, 50, 100, 0.5, 1);

                Title.Add(t);

            }
            return SBContainer.ToString();
        }

        public static string Animation1()
        {
            var Anis = new SBGroup(0);

            var Tail1 = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\2dx_108.png", 460, 240);
            Tail1._Move.Add(0, 5926, 6175, 460, 410 + 200, 460, 410);
            Tail1._Fade.Add(0, 15374, 15374, 1, 1);
            Tail1.Parameter.Add(0, 5926, 5926, "H");

            Tail1.Loop.Add(5926, 24);
            Tail1.Loop[0].Vector.Add(0, 0, 0, 1, 1.2, 1, 1.2);
            Tail1.Loop[0].Rotate.Add(0, 0, 0, 0, 0);
            Tail1.Loop[0].Move.Add(0, 0, 0, 460, 410, 460, 410);

            Tail1.Loop[0].Vector.Add(0, 80, 80, 1, 1.1, 1, 1.1);
            Tail1.Loop[0].Rotate.Add(0, 80, 80, -0.15, -0.15);
            Tail1.Loop[0].Move.Add(0, 80, 80, 450, 410, 450, 410);

            Tail1.Loop[0].Vector.Add(0, 160, 160, 1, 1, 1, 1);
            Tail1.Loop[0].Rotate.Add(0, 160, 160, -0.3, -0.3);
            Tail1.Loop[0].Move.Add(0, 160, 160, 450, 420, 450, 420);

            Tail1.Loop[0].Vector.Add(0, 240, 240, 1, 1.1, 1, 1.1);
            Tail1.Loop[0].Rotate.Add(0, 240, 240, -0.15, -0.15);
            Tail1.Loop[0].Move.Add(0, 240, 240, 450, 410, 450, 410);

            Tail1.Loop[0].Vector.Add(0, 320, 320, 1, 1.15, 1, 1.15);
            Tail1.Loop[0].Rotate.Add(0, 320, 320, -0.075, -0.075);
            Tail1.Loop[0].Move.Add(0, 320, 320, 455, 410, 455, 410);

            Tail1.Loop[0].Vector.Add(0, 400, 400, 1, 1.2, 1, 1.2);
            Tail1.Loop[0].Rotate.Add(0, 400, 400, 0, 0);
            Tail1.Loop[0].Move.Add(0, 400, 400, 460, 410, 460, 410);

            Anis.Add(Tail1);

            var Tail2 = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\2dx_108.png", 180, 240);
            Tail2._Move.Add(0, 5926, 6175, 180, 410 + 200, 180, 410);
            Tail2._Fade.Add(0, 15374, 15374, 1, 1);

            Tail2.Loop.Add(5926, 24);
            Tail2.Loop[0].Vector.Add(0, 0, 0, 1, 1.2, 1, 1.2);
            Tail2.Loop[0].Rotate.Add(0, 0, 0, 0, 0);
            Tail2.Loop[0].Move.Add(0, 0, 0, 180, 410, 180, 410);

            Tail2.Loop[0].Vector.Add(0, 80, 80, 1, 1.1, 1, 1.1);
            Tail2.Loop[0].Rotate.Add(0, 80, 80, 0.15, 0.15);
            Tail2.Loop[0].Move.Add(0, 80, 80, 190, 410, 190, 410);

            Tail2.Loop[0].Vector.Add(0, 160, 160, 1, 1, 1, 1);
            Tail2.Loop[0].Rotate.Add(0, 160, 160, 0.3, 0.3);
            Tail2.Loop[0].Move.Add(0, 160, 160, 190, 420, 190, 420);

            Tail2.Loop[0].Vector.Add(0, 240, 240, 1, 1.1, 1, 1.1);
            Tail2.Loop[0].Rotate.Add(0, 240, 240, 0.15, 0.15);
            Tail2.Loop[0].Move.Add(0, 240, 240, 190, 410, 190, 410);

            Tail2.Loop[0].Vector.Add(0, 320, 320, 1, 1.15, 1, 1.15);
            Tail2.Loop[0].Rotate.Add(0, 320, 320, 0.075, 0.075);
            Tail2.Loop[0].Move.Add(0, 320, 320, 185, 410, 185, 410);

            Tail2.Loop[0].Vector.Add(0, 400, 400, 1, 1.2, 1, 1.2);
            Tail2.Loop[0].Rotate.Add(0, 400, 400, 0, 0);
            Tail2.Loop[0].Move.Add(0, 400, 400, 180, 410, 180, 410);

            Anis.Add(Tail2);

            Anis.Add(Tail2);

            var Ani1 = new SBObject(Types.Animation, Layers.Foreground, Origins.BottomCentre, @"SB\Ani1\a.png",
                320, 240, 10, 80, LoopType.LoopForever);
            Ani1._Move.Add(0, 5926, 6175, 480, 480 + 200, 480, 480);
            Ani1._Fade.Add(0, 15374, 15374, 1, 1);

            Ani1.Loop.Add(5926, 24);
            Ani1.Loop[0].Vector.Add(0, 0, 0, 1, 1, 1, 1);
            Ani1.Loop[0].Vector.Add(0, 80, 80, 1, 0.95, 1, 0.95);
            Ani1.Loop[0].Vector.Add(0, 160, 160, 1, 0.9, 1, 0.9);
            Ani1.Loop[0].Vector.Add(0, 240, 240, 1, 0.92, 1, 0.92);
            Ani1.Loop[0].Vector.Add(0, 320, 320, 1, 0.95, 1, 0.95);
            Ani1.Loop[0].Vector.Add(0, 400, 400, 1, 1, 1, 1);
            Anis.Add(Ani1);

            #region Bell1
            var Bell1 = new SBObject(Types.Sprite, Layers.Foreground, Origins.CentreLeft, @"SB\2dx_25.png", 480, 240);
            Bell1.MoveY.Add(0, 5926, 6175, 440 + 200, 440);
            Bell1._Fade.Add(0, 15374, 15374, 1, 1);

            Bell1.Loop.Add(5926, 12);
            Bell1.Loop[0].Rotate.Add(0, 0, 0, 2, 2);
            Bell1.Loop[0].Rotate.Add(0, 80, 80, 1.8, 1.8);
            Bell1.Loop[0].Rotate.Add(0, 160, 160, 1.5, 1.5);
            Bell1.Loop[0].Rotate.Add(0, 240, 240, 1, 1);
            Bell1.Loop[0].Rotate.Add(0, 320, 320, 0.7, 0.7);
            Bell1.Loop[0].Rotate.Add(0, 400, 400, 0.3, 0.3);
            Bell1.Loop[0].Rotate.Add(0, 480, 480, 0.7, 0.7);
            Bell1.Loop[0].Rotate.Add(0, 560, 560, 1, 1);
            Bell1.Loop[0].Rotate.Add(0, 640, 640, 1.5, 1.5);
            Bell1.Loop[0].Rotate.Add(0, 720, 720, 1.8, 1.8);
            Bell1.Loop[0].Rotate.Add(0, 800, 800, 2, 2);
            Bell1.Loop.Add(5926, 24);
            Bell1.Loop[1].MoveY.Add(0, 0, 0, 440, 440);
            Bell1.Loop[1].MoveY.Add(0, 80, 80, 443, 443);
            Bell1.Loop[1].MoveY.Add(0, 160, 160, 446, 446);
            Bell1.Loop[1].MoveY.Add(0, 240, 240, 446, 446);
            Bell1.Loop[1].MoveY.Add(0, 320, 320, 443, 443);
            Bell1.Loop[1].MoveY.Add(0, 400, 400, 440, 440);

            Anis.Add(Bell1);
            #endregion

            var Ani2 = new SBObject(Types.Animation, Layers.Foreground, Origins.BottomCentre, @"SB\Ani1\a.png",
              320, 240, 10, 80, LoopType.LoopForever);
            Ani2._Move.Add(0, 5926, 6175, 160, 480 + 200, 160, 480);
            Ani2.Parameter.Add(0, 5926, 5926, "H");
            Ani2._Fade.Add(0, 15374, 15374, 1, 1);

            Ani2.Loop.Add(5926, 24);
            Ani2.Loop[0].Vector.Add(0, 0, 0, 1, 1, 1, 1);
            Ani2.Loop[0].Vector.Add(0, 80, 80, 1, 0.95, 1, 0.95);
            Ani2.Loop[0].Vector.Add(0, 160, 160, 1, 0.9, 1, 0.9);
            Ani2.Loop[0].Vector.Add(0, 240, 240, 1, 0.92, 1, 0.92);
            Ani2.Loop[0].Vector.Add(0, 320, 320, 1, 0.95, 1, 0.95);
            Ani2.Loop[0].Vector.Add(0, 400, 400, 1, 1, 1, 1);
            Anis.Add(Ani2);

            #region Bell2
            var Bell2 = new SBObject(Types.Sprite, Layers.Foreground, Origins.CentreLeft, @"SB\2dx_24.png", 160, 240);
            Bell2.MoveY.Add(0, 5926, 6175, 440 + 200, 440);
            Bell2._Fade.Add(0, 15374, 15374, 1, 1);

            Bell2.Loop.Add(5926, 12);
            Bell2.Loop[0].Rotate.Add(0, 0, 0, 0.3, 0.3);
            Bell2.Loop[0].Rotate.Add(0, 80, 80, 0.7, 0.7);
            Bell2.Loop[0].Rotate.Add(0, 160, 160, 1, 1);
            Bell2.Loop[0].Rotate.Add(0, 240, 240, 1.5, 1.5);
            Bell2.Loop[0].Rotate.Add(0, 320, 320, 1.8, 1.8);
            Bell2.Loop[0].Rotate.Add(0, 400, 400, 2, 2);
            Bell2.Loop[0].Rotate.Add(0, 480, 480, 1.8, 1.8);
            Bell2.Loop[0].Rotate.Add(0, 560, 560, 1.5, 1.5);
            Bell2.Loop[0].Rotate.Add(0, 640, 640, 1, 1);
            Bell2.Loop[0].Rotate.Add(0, 720, 720, 0.7, 0.7);
            Bell2.Loop[0].Rotate.Add(0, 800, 800, 0.3, 0.3);

            Bell2.Loop.Add(5926, 24);
            Bell2.Loop[1].MoveY.Add(0, 0, 0, 440, 440);
            Bell2.Loop[1].MoveY.Add(0, 80, 80, 443, 443);
            Bell2.Loop[1].MoveY.Add(0, 160, 160, 446, 446);
            Bell2.Loop[1].MoveY.Add(0, 240, 240, 446, 446);
            Bell2.Loop[1].MoveY.Add(0, 320, 320, 443, 443);
            Bell2.Loop[1].MoveY.Add(0, 400, 400, 440, 440);

            Anis.Add(Bell2);

            #endregion



            return Anis.ToString();
        }
    }
}
