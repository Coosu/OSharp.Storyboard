using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOsb.BrewHelper;
using LibOsb.Model.Constants;
using OpenTK;
using StorybrewCommon.Storyboarding;

namespace LibOsb.Util
{
    internal static class ElementExtension
    {
        public static void ExecuteBrew(this Element element, StoryboardLayer layParsed, OsbSprite brewObj = null)
        {
            if (brewObj == null)
            {
                if (element.Type == ElementType.Sprite)
                    brewObj = layParsed.CreateSprite(element.ImagePath, BrewConvert.CvtOrigin(element.Origin),
                        new Vector2((float) element.DefaultX, (float) element.DefaultY));
                else
                    brewObj = layParsed.CreateAnimation(element.ImagePath, (int)element.FrameCount, (int)element.FrameRate,
                        BrewConvert.CvtLoopType(element.LoopType), BrewConvert.CvtOrigin(element.Origin),
                        new Vector2((float) element.DefaultX, (float) element.DefaultY));
            }

            foreach (var m in element.MoveList)
                BrewConvert.ExeM(m, brewObj);
            foreach (var s in element.ScaleList)
                BrewConvert.ExeS(s, brewObj);
            foreach (var f in element.FadeList)
                BrewConvert.ExeF(f, brewObj);
            foreach (var r in element.RotateList)
                BrewConvert.ExeR(r, brewObj);
            foreach (var v in element.VectorList)
                BrewConvert.ExeV(v, brewObj);
            foreach (var c in element.ColorList)
                BrewConvert.ExeC(c, brewObj);
            foreach (var mx in element.MoveXList)
                BrewConvert.ExeMx(mx, brewObj);
            foreach (var my in element.MoveYList)
                BrewConvert.ExeMy(my, brewObj);
            foreach (var p in element.ParameterList)
                BrewConvert.ExeP(p, brewObj);
            foreach (var l in element.LoopList)
            {
                brewObj.StartLoopGroup(l.StartTime, l.LoopCount);
                l.ExecuteBrew(layParsed, brewObj);
                brewObj.EndGroup();
            }
            foreach (var t in element.TriggerList)
            {
                brewObj.StartTriggerGroup(t.TriggerType, t.StartTime, t.EndTime);
                t.ExecuteBrew(layParsed, brewObj);
                brewObj.EndGroup();
            }
        }

    }
}
