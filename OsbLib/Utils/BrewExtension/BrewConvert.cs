using System;
using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Models.EventClass;
using Milkitic.OsbLib.Models.EventType;
using OpenTK;
using StorybrewCommon.Storyboarding;

namespace Milkitic.OsbLib.Utils.BrewExtension
{
    public static class BrewConvert
    {
        public static void ExecuteBrew(this Element element, StoryboardLayer layParsed, OsbSprite brewObj = null)
        {
            if (brewObj == null)
            {
                if (element.Type == ElementType.Sprite)
                    brewObj = layParsed.CreateSprite(element.ImagePath, ConvertOrigin(element.Origin),
                        new Vector2((float)element.DefaultX, (float)element.DefaultY));
                else
                    brewObj = layParsed.CreateAnimation(element.ImagePath, (int)element.FrameCount, (int)element.FrameRate,
                        ConvertLoopType(element.LoopType), ConvertOrigin(element.Origin),
                        new Vector2((float)element.DefaultX, (float)element.DefaultY));
            }

            foreach (var mx in element.MoveXList) ExecuteSingle(mx, brewObj);
            foreach (var my in element.MoveYList) ExecuteSingle(my, brewObj);
            foreach (var s in element.ScaleList) ExecuteSingle(s, brewObj);
            foreach (var f in element.FadeList) ExecuteSingle(f, brewObj);
            foreach (var r in element.RotateList) ExecuteSingle(r, brewObj);
            foreach (var v in element.VectorList) ExecuteDouble(v, brewObj);
            foreach (var m in element.MoveList) ExecuteDouble(m, brewObj);
            foreach (var c in element.ColorList) ExecuteTriple(c, brewObj);
            foreach (var p in element.ParameterList) ExecuteP(p, brewObj);
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
        private static void ExecuteSingle(EventSingle e, OsbSprite brewObj)
        {
            switch (e)
            {
                case Scale _:
                    brewObj.Scale(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start, e.End);
                    break;
                case Fade _:
                    brewObj.Fade(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start, e.End);
                    break;
                case Rotate _:
                    brewObj.Rotate(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start, e.End);
                    break;
                case MoveX _:
                    brewObj.MoveX(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start, e.End);
                    break;
                case MoveY _:
                    brewObj.MoveY(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start, e.End);
                    break;
            }
        }

        private static void ExecuteDouble(EventDouble e, OsbSprite brewObj)
        {
            switch (e)
            {
                case Move _:
                    brewObj.Move(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start.x, e.Start.y, e.End.x,
                        e.End.y);
                    break;
                case Vector _:
                    brewObj.ScaleVec(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start.x, e.Start.y, e.End.x,
                        e.End.y);
                    break;
            }
        }

        private static void ExecuteTriple(EventTriple e, OsbSprite brewObj)
        {
            switch (e)
            {
                case Color _:
                    brewObj.Color(ConvertEasing(e.Easing), e.StartTime, e.EndTime, e.Start.x / 255d, e.Start.y / 255d,
                        e.Start.z / 255d, e.End.x / 255d, e.End.y / 255d, e.End.z / 255d);
                    break;
            }
        }

        private static void ExecuteP(Parameter p, OsbSprite brewObj)
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

        private static OsbOrigin ConvertOrigin(OriginType libOrigin)
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

        private static OsbLoopType ConvertLoopType(LoopType libLoop)
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

        private static OsbEasing ConvertEasing(EasingType libEasing)
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
