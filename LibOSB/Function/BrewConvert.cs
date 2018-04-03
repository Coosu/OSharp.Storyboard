using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOSB.Model.Constants;
using LibOSB.Model.EventType;

namespace LibOSB.Function
{
    class BrewConvert
    {
        public static void ExeM(Move m, OsbSprite brewObj) =>
            brewObj.Move(CvtEasing(m.Easing), m.StartTime, m.EndTime, m.P1_1, m.P1_2, m.P2_1, m.P2_2);
        public static void ExeS(Scale s, OsbSprite brewObj) =>
            brewObj.Scale(CvtEasing(s.Easing), s.StartTime, s.EndTime, s.P1_1, s.P2_1);
        public static void ExeF(Fade f, OsbSprite brewObj) =>
            brewObj.Fade(CvtEasing(f.Easing), f.StartTime, f.EndTime, f.P1_1, f.P2_1);
        public static void ExeR(Rotate r, OsbSprite brewObj) =>
            brewObj.Rotate(CvtEasing(r.Easing), r.StartTime, r.EndTime, r.P1_1, r.P2_1);
        public static void ExeV(Vector v, OsbSprite brewObj) =>
            brewObj.ScaleVec(CvtEasing(v.Easing), v.StartTime, v.EndTime, v.P1_1, v.P1_2, v.P2_1, v.P2_2);
        public static void ExeC(Color c, OsbSprite brewObj) =>
            brewObj.Color(CvtEasing(c.Easing), c.StartTime, c.EndTime, c.P1_1 / 255d, c.P1_2 / 255d, c.P1_3 / 255d,
                c.P2_1 / 255d, c.P2_2 / 255d, c.P2_3 / 255d);
        public static void ExeMx(MoveX mx, OsbSprite brewObj) =>
            brewObj.MoveX(CvtEasing(mx.Easing), mx.StartTime, mx.EndTime, mx.P1_1, mx.P2_1);
        public static void ExeMy(MoveY my, OsbSprite brewObj) =>
            brewObj.MoveY(CvtEasing(my.Easing), my.StartTime, my.EndTime, my.P1_1, my.P2_1);
        public static void ExeP(Parameter p, OsbSprite brewObj)
        {
            switch (p.PType)
            {
                case "A":
                    brewObj.Additive(p.StartTime, p.EndTime);
                    break;
                case "H":
                    brewObj.FlipH(p.StartTime, p.EndTime);
                    break;
                case "V":
                    brewObj.FlipV(p.StartTime, p.EndTime);
                    break;
            }
        }

        public static OsbOrigin CvtOrigin(OriginType libOrigin)
        {
            switch (libOrigin)
            {
                case OriginType.BottomCentre:
                    return OsbOrigin.BottomCentre;
                case OriginType.BottomLeft:
                    return OsbOrigin.BottomLeft;
                case OriginType.BottomRight:
                    return OsbOrigin.BottomRight;
                case OriginType.Centre:
                    return OsbOrigin.Centre;
                case OriginType.CentreLeft:
                    return OsbOrigin.CentreLeft;
                case OriginType.CentreRight:
                    return OsbOrigin.CentreRight;
                case OriginType.TopCentre:
                    return OsbOrigin.TopCentre;
                case OriginType.TopLeft:
                    return OsbOrigin.TopLeft;
                case OriginType.TopRight:
                    return OsbOrigin.TopRight;
                default:
                    throw new Exception(libOrigin.ToString() + "未提供转换");
            }

        }
        public static OsbLoopType CvtLoopType(LoopType libLoop)
        {
            switch (libLoop)
            {
                case LoopType.LoopForever:
                    return OsbLoopType.LoopForever;
                case LoopType.LoopOnce:
                    return OsbLoopType.LoopOnce;
                default:
                    throw new Exception(libLoop.ToString() + "未提供转换");
            }

        }
        public static OsbEasing CvtEasing(EasingType libEasing)
        {
            switch (libEasing)
            {
                case EasingType.Linear:
                    return OsbEasing.None;
                case EasingType.EasingOut:
                    return OsbEasing.Out;
                case EasingType.EasingIn:
                    return OsbEasing.In;

                case EasingType.QuadIn:
                    return OsbEasing.InQuad;
                case EasingType.QuadOut:
                    return OsbEasing.OutQuad;
                case EasingType.QuadInOut:
                    return OsbEasing.InOutQuad;

                case EasingType.CubicIn:
                    return OsbEasing.InCubic;
                case EasingType.CubicOut:
                    return OsbEasing.OutCubic;
                case EasingType.CubicInOut:
                    return OsbEasing.InOutCubic;

                case EasingType.QuartIn:
                    return OsbEasing.InQuart;
                case EasingType.QuartOut:
                    return OsbEasing.OutQuart;
                case EasingType.QuartInOut:
                    return OsbEasing.InOutQuart;

                case EasingType.QuintIn:
                    return OsbEasing.InQuint;
                case EasingType.QuintOut:
                    return OsbEasing.OutQuint;
                case EasingType.QuintInOut:
                    return OsbEasing.InOutQuart;

                case EasingType.SineIn:
                    return OsbEasing.InSine;
                case EasingType.SineOut:
                    return OsbEasing.OutSine;
                case EasingType.SineInOut:
                    return OsbEasing.InOutSine;

                case EasingType.ExpoIn:
                    return OsbEasing.InExpo;
                case EasingType.ExpoOut:
                    return OsbEasing.OutExpo;
                case EasingType.ExpoInOut:
                    return OsbEasing.InOutExpo;

                case EasingType.CircIn:
                    return OsbEasing.InCirc;
                case EasingType.CircOut:
                    return OsbEasing.OutCirc;
                case EasingType.CircInOut:
                    return OsbEasing.InOutCirc;

                case EasingType.ElasticIn:
                    return OsbEasing.InElastic;
                case EasingType.ElasticOut:
                    return OsbEasing.OutElastic;
                case EasingType.ElasticHalfOut:
                    return OsbEasing.OutElasticHalf;
                case EasingType.ElasticQuarterOut:
                    return OsbEasing.OutElasticQuarter;
                case EasingType.ElasticInOut:
                    return OsbEasing.InOutElastic;

                case EasingType.BackIn:
                    return OsbEasing.InBack;
                case EasingType.BackOut:
                    return OsbEasing.OutBack;
                case EasingType.BackInOut:
                    return OsbEasing.InOutBack;

                case EasingType.BounceIn:
                    return OsbEasing.InBounce;
                case EasingType.BounceOut:
                    return OsbEasing.OutBounce;
                case EasingType.BounceInOut:
                    return OsbEasing.InOutBounce;

                default:
                    throw new Exception(libEasing.ToString() + "未提供转换");
            }

        }
    }
}
