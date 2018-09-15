using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models;
using Milkitic.OsbLib.Models.EventType;
using OpenTK;
using StorybrewCommon.Storyboarding;
using System;
using System.Linq;

namespace Milkitic.OsbLib.Extension
{
    public static class BrewConvert
    {
        public static void ExecuteBrew(this EventContainer ele, StoryboardLayer layParsed, OsbSprite brewObj = null)
        {
            if (brewObj == null)
            {
                switch (ele)
                {
                    case AnimatedElement ani:
                        brewObj = layParsed.CreateAnimation(ani.ImagePath, (int)ani.FrameCount, (int)ani.FrameDelay,
                            ToBrewType(ani.LoopType), ToBrewType(ani.Origin), new Vector2(ani.DefaultX, ani.DefaultY));
                        foreach (var l in ani.LoopList)
                        {
                            brewObj.StartLoopGroup(l.StartTime, l.LoopCount);
                            l.ExecuteBrew(layParsed, brewObj);
                            brewObj.EndGroup();
                        }
                        foreach (var t in ani.TriggerList)
                        {
                            brewObj.StartTriggerGroup(t.TriggerType, t.StartTime, t.EndTime);
                            t.ExecuteBrew(layParsed, brewObj);
                            brewObj.EndGroup();
                        }
                        break;
                    case Element sta:
                        brewObj = layParsed.CreateSprite(sta.ImagePath, ToBrewType(sta.Origin),
                            new Vector2(sta.DefaultX, sta.DefaultY));
                        foreach (var l in sta.LoopList)
                        {
                            brewObj.StartLoopGroup(l.StartTime, l.LoopCount);
                            l.ExecuteBrew(layParsed, brewObj);
                            brewObj.EndGroup();
                        }
                        foreach (var t in sta.TriggerList)
                        {
                            brewObj.StartTriggerGroup(t.TriggerType, t.StartTime, t.EndTime);
                            t.ExecuteBrew(layParsed, brewObj);
                            brewObj.EndGroup();
                        }
                        break;
                    default:
                        throw new NullReferenceException("Can't be Loop/Trigger");
                }
            }

            foreach (var e in ele.EventList)
            {
                switch (e)
                {
                    case Scale s:
                        brewObj.Scale(ToBrewType(s.Easing), s.StartTime, s.EndTime, s.S1, s.S2);
                        break;
                    case Fade f:
                        brewObj.Fade(ToBrewType(f.Easing), f.StartTime, f.EndTime, f.F1, f.F2);
                        break;
                    case Rotate r:
                        brewObj.Rotate(ToBrewType(r.Easing), r.StartTime, r.EndTime, r.R1, r.R2);
                        break;
                    case MoveX mx:
                        brewObj.MoveX(ToBrewType(mx.Easing), mx.StartTime, mx.EndTime, mx.X1, mx.X2);
                        break;
                    case MoveY my:
                        brewObj.MoveY(ToBrewType(my.Easing), my.StartTime, my.EndTime, my.Y1, my.Y2);
                        break;
                    case Move m:
                        brewObj.Move(ToBrewType(m.Easing), m.StartTime, m.EndTime, m.X1, m.Y1, m.X2, m.Y2);
                        break;
                    case Vector v:
                        brewObj.ScaleVec(ToBrewType(v.Easing), v.StartTime, v.EndTime, v.Vx1, v.Vx1, v.Vx2, v.Vx2);
                        break;
                    case Color c:
                        brewObj.Color(ToBrewType(c.Easing), c.StartTime, c.EndTime,
                            c.R1 / 255d, c.G1 / 255d, c.B1 / 255d, c.R2 / 255d, c.G2 / 255d, c.B2 / 255d);
                        break;
                    case Parameter p:
                        switch (p.Type)
                        {
                            case ParameterEnum.Additive:
                                brewObj.Additive(p.StartTime, p.EndTime);
                                break;
                            case ParameterEnum.Horizontal:
                                brewObj.FlipH(p.StartTime, p.EndTime);
                                break;
                            case ParameterEnum.Vertical:
                                brewObj.FlipV(p.StartTime, p.EndTime);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                }
            }

        }

        private static OsbOrigin ToBrewType(this OriginType libOrigin)
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

        private static OsbLoopType ToBrewType(this LoopType libLoop)
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

        private static OsbEasing ToBrewType(this EasingType libEasing)
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
