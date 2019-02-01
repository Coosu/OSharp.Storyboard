using MGLib.Osu.Reader.Osb;
using OSharp.Storyboard.Events;
using OSharp.Storyboard.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using EasingType = OSharp.Storyboard.Events.EasingType;
using MGCommandType = MGLib.Osu.Model.Osb.CommandType;
using MGEasing = MGLib.Osu.Model.Osb.EasingType;
using MGLayer = MGLib.Osu.Model.Osb.Layer;
using MGLoop = MGLib.Osu.Model.Osb.AnimationLoop;
using MGOrigin = MGLib.Osu.Model.Osb.Position;

namespace OSharp.Storyboard.External
{
    public static class MGLibSwap
    {
        public static LayerType ToOSharp(this MGLayer mgLayer)
        {
            switch (mgLayer)
            {
                case MGLayer.Background:
                    return LayerType.Background;
                case MGLayer.Fail:
                    return LayerType.Fail;
                case MGLayer.Pass:
                    return LayerType.Pass;
                case MGLayer.Foreground:
                    return LayerType.Foreground;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mgLayer), mgLayer, null);
            }
        }

        public static OriginType ToOSharp(this MGOrigin mgOrigin)
        {
            switch (mgOrigin)
            {
                case MGOrigin.TopLeft:
                    return OriginType.TopLeft;
                case MGOrigin.TopCentre:
                    return OriginType.TopCentre;
                case MGOrigin.TopRight:
                    return OriginType.TopRight;
                case MGOrigin.CentreLeft:
                    return OriginType.CentreLeft;
                case MGOrigin.Centre:
                    return OriginType.Centre;
                case MGOrigin.CentreRight:
                    return OriginType.CentreRight;
                case MGOrigin.BottomLeft:
                    return OriginType.BottomLeft;
                case MGOrigin.BottomCentre:
                    return OriginType.BottomCentre;
                case MGOrigin.BottomRight:
                    return OriginType.BottomRight;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mgOrigin), mgOrigin, null);
            }
        }

        public static LoopType ToOSharp(this MGLoop mgLoop)
        {
            switch (mgLoop)
            {
                case MGLoop.LoopForever:
                    return LoopType.LoopForever;
                case MGLoop.LoopOnce:
                    return LoopType.LoopOnce;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mgLoop), mgLoop, null);
            }
        }

        public static EasingType ToOSharp(this MGEasing mgEasing)
        {
            switch (mgEasing)
            {
                case MGEasing.Linear:
                    return EasingType.Linear;
                case MGEasing.EasingOut:
                    return EasingType.EasingOut;
                case MGEasing.EasingIn:
                    return EasingType.EasingIn;
                case MGEasing.QuadIn:
                    return EasingType.QuadIn;
                case MGEasing.QuadOut:
                    return EasingType.QuadOut;
                case MGEasing.QuadInOut:
                    return EasingType.QuadInOut;
                case MGEasing.CubicIn:
                    return EasingType.CubicIn;
                case MGEasing.CubicOut:
                    return EasingType.CubicOut;
                case MGEasing.CubicInOut:
                    return EasingType.CubicInOut;
                case MGEasing.QuartIn:
                    return EasingType.QuartIn;
                case MGEasing.QuartOut:
                    return EasingType.QuartOut;
                case MGEasing.QuartInOut:
                    return EasingType.QuartInOut;
                case MGEasing.QuintIn:
                    return EasingType.QuintIn;
                case MGEasing.QuintOut:
                    return EasingType.QuintOut;
                case MGEasing.QuintInOut:
                    return EasingType.QuintInOut;
                case MGEasing.SineIn:
                    return EasingType.SineIn;
                case MGEasing.SineOut:
                    return EasingType.SineOut;
                case MGEasing.SineInOut:
                    return EasingType.SineInOut;
                case MGEasing.ExpoIn:
                    return EasingType.ExpoIn;
                case MGEasing.ExpoOut:
                    return EasingType.ExpoOut;
                case MGEasing.ExpoInOut:
                    return EasingType.ExpoInOut;
                case MGEasing.CircIn:
                    return EasingType.CircIn;
                case MGEasing.CircOut:
                    return EasingType.CircOut;
                case MGEasing.CircInOut:
                    return EasingType.CircInOut;
                case MGEasing.ElasticIn:
                    return EasingType.ElasticIn;
                case MGEasing.ElasticOut:
                    return EasingType.ElasticOut;
                case MGEasing.ElasticHalfOut:
                    return EasingType.ElasticHalfOut;
                case MGEasing.ElasticQuarterOut:
                    return EasingType.ElasticQuarterOut;
                case MGEasing.ElasticInOut:
                    return EasingType.ElasticInOut;
                case MGEasing.BackIn:
                    return EasingType.BackIn;
                case MGEasing.BackOut:
                    return EasingType.BackOut;
                case MGEasing.BackInOut:
                    return EasingType.BackInOut;
                case MGEasing.BounceIn:
                    return EasingType.BounceIn;
                case MGEasing.BounceOut:
                    return EasingType.BounceOut;
                case MGEasing.BounceInOut:
                    return EasingType.BounceInOut;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mgEasing), mgEasing, null);
            }
        }

        public static EventType ToOSharp(this MGCommandType mgCmdType)
        {
            switch (mgCmdType)
            {
                case MGCommandType.Fade:
                    return EventType.Fade;
                case MGCommandType.Move:
                    return EventType.Move;
                case MGCommandType.MoveX:
                    return EventType.MoveX;
                case MGCommandType.MoveY:
                    return EventType.MoveY;
                case MGCommandType.Scale:
                    return EventType.Scale;
                case MGCommandType.VectorScale:
                    return EventType.Vector;
                case MGCommandType.Rotate:
                    return EventType.Rotate;
                case MGCommandType.Color:
                    return EventType.Color;
                case MGCommandType.Parameter:
                    return EventType.Parameter;
                case MGCommandType.Loop:
                    return EventType.Loop;
                case MGCommandType.Trigger:
                    return EventType.Trigger;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mgCmdType), mgCmdType, null);
            }
        }


        public static ElementGroup ParseFromFile(string path)
        {
            ElementGroup eg = new ElementGroup(0);
            using (OsbElementList elements = new OsbElementList(path))
            {
                foreach (var mgElement in elements)
                {
                    Element osElement;
                    switch (mgElement.ElementType)
                    {
                        case 0: // sprite
                            osElement = eg.CreateSprite(
                                mgElement.Layer.ToOSharp(),
                                mgElement.Origin.ToOSharp(),
                                mgElement.TexturePath, mgElement.Position.Item1,
                                mgElement.Position.Item2
                            );
                            break;
                        case 1: // animation
                            osElement = eg.CreateAnimation(
                                mgElement.Layer.ToOSharp(),
                                mgElement.Origin.ToOSharp(),
                                mgElement.TexturePath, mgElement.Position.Item1,
                                mgElement.Position.Item2,
                                mgElement.FrameCount,
                                mgElement.FrameDelay,
                                mgElement.LoopType.ToOSharp()
                            );
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    AddEvent(mgElement.Commands, osElement);
                }
            }

            return eg;
        }

        private static void AddEvent(IEnumerable<Command> mgCommands, Element osElement)
        {
            foreach (var cmd in mgCommands)
            {
                switch (cmd.CommandType)
                {
                    case MGLib.Osu.Model.Osb.CommandType.Loop:
                        osElement.StartLoop(cmd.Time.Item1, cmd.LoopCount);
                        AddEvent(cmd.SubCommands, osElement);
                        osElement.EndLoop();
                        break;
                    case MGLib.Osu.Model.Osb.CommandType.Trigger:
                        osElement.StartTrigger(cmd.Time.Item1, cmd.Time.Item2, cmd.Trigger);
                        AddEvent(cmd.SubCommands, osElement);
                        osElement.EndLoop();
                        break;
                    default:
                        osElement.AddEvent(cmd.CommandType.ToOSharp(), cmd.EasingType.ToOSharp(), cmd.Time.Item1, cmd.Time.Item2, cmd.Params);
                        break;
                }
            }
        }

        public static void AddEvent(
            this Element element,
            EventType e,
            EasingType easing,
            int startTime,
            int endTime,
            IEnumerable<float> @params)
        {
            if (endTime == int.MinValue)
                endTime = startTime;

            float[] fixedParams;
            int maxLength;
            switch (e)
            {
                case EventType.Fade:
                case EventType.MoveX:
                case EventType.MoveY:
                case EventType.Scale:
                case EventType.Rotate:
                    {
                        maxLength = 2;
                        fixedParams = new float[maxLength];
                        FixParameter(fixedParams, maxLength, @params);
                        break;
                    }
                case EventType.Move:
                case EventType.Vector:
                    {
                        maxLength = 4;
                        fixedParams = new float[maxLength];
                        FixParameter(fixedParams, maxLength, @params);
                        break;
                    }
                case EventType.Color:
                    {
                        maxLength = 6;
                        fixedParams = new float[maxLength];
                        FixParameter(fixedParams, maxLength, @params);
                        break;
                    }
                case EventType.Parameter:
                    {
                        maxLength = 2;
                        fixedParams = @params is float[] floats ? floats : @params.ToArray();
                    }
                    break;
                case EventType.Loop:
                //break;
                case EventType.Trigger:
                //break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }

            element.AddEvent(e, easing, startTime, endTime, fixedParams.Take(maxLength / 2).ToArray(),
                fixedParams.Skip(maxLength / 2).ToArray());
        }

        private static void FixParameter(float[] ps, int maxL, IEnumerable<float> par)
        {
            int count = 0;
            foreach (var p in par)
            {
                ps[count] = p;
                count++;
            }

            if (count == maxL / 2)
            {
                for (int i = maxL / 2; i < maxL; i++)
                {
                    ps[i] = ps[i - maxL / 2];
                }
            }
        }
    }
}
