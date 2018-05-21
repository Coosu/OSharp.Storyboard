using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOsb.Model.Constants;
using LibOsb.Model.EventType;

namespace LibOsb.BrewHelper
{
    internal class BrewConvert
    {
        public static void ExeM(Move m, OsbSprite brewObj) =>
            brewObj.Move(CvtEasing(m.Easing), m.StartTime, m.EndTime, m.P11, m.P12, m.P21, m.P22);
        public static void ExeS(Scale s, OsbSprite brewObj) =>
            brewObj.Scale(CvtEasing(s.Easing), s.StartTime, s.EndTime, s.P11, s.P21);
        public static void ExeF(Fade f, OsbSprite brewObj) =>
            brewObj.Fade(CvtEasing(f.Easing), f.StartTime, f.EndTime, f.P11, f.P21);
        public static void ExeR(Rotate r, OsbSprite brewObj) =>
            brewObj.Rotate(CvtEasing(r.Easing), r.StartTime, r.EndTime, r.P11, r.P21);
        public static void ExeV(Vector v, OsbSprite brewObj) =>
            brewObj.ScaleVec(CvtEasing(v.Easing), v.StartTime, v.EndTime, v.P11, v.P12, v.P21, v.P22);
        public static void ExeC(Color c, OsbSprite brewObj) =>
            brewObj.Color(CvtEasing(c.Easing), c.StartTime, c.EndTime, c.P11 / 255d, c.P12 / 255d, c.P13 / 255d,
                c.P21 / 255d, c.P22 / 255d, c.P23 / 255d);
        public static void ExeMx(MoveX mx, OsbSprite brewObj) =>
            brewObj.MoveX(CvtEasing(mx.Easing), mx.StartTime, mx.EndTime, mx.P11, mx.P21);
        public static void ExeMy(MoveY my, OsbSprite brewObj) =>
            brewObj.MoveY(CvtEasing(my.Easing), my.StartTime, my.EndTime, my.P11, my.P21);
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
                default:
                    throw new ArgumentOutOfRangeException();
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
                    throw new ArgumentOutOfRangeException(libOrigin + "未提供转换");
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
                    throw new ArgumentOutOfRangeException(libLoop + "未提供转换");
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
                    throw new ArgumentOutOfRangeException(libEasing + "未提供转换");
            }

        }
    }
}
