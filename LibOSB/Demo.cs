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
        private static int[] GetBookmark()
        {
            string fileroot = @"D:\Program Files (x86)\osu!\Songs\demo\demo - demo (yf_bmp) [demo].osu";
            string[] lines = File.ReadAllLines(fileroot);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf("Bookmarks") != -1)
                {
                    string text = lines[i].Replace("Bookmarks: ", "");
                    string[] timings = text.Split(',');
                    return Array.ConvertAll(timings, delegate (string s) { return int.Parse(s); });
                }
            }
            return null;
        }

        public static string Subtitle()
        {
            int[] timings = GetBookmark();

            SBGroup subtitles = new SBGroup(0);
            for (int i = 0; i < timings.Length - 1; i++)
            {
                var subtitle = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, @"SB\L" + (i + 1) + ".png", 320, 240);
                subtitle.Move.Add(0, timings[i], timings[i + 1] - 200, 320, 450, 320, 450);
                subtitle.Scale.Add(0, timings[i], timings[i + 1] - 200, 0.9, 0.9);
                subtitles.Add(subtitle);
            }

            return subtitles.ToString();
        }
    }
}
