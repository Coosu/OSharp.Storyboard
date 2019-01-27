using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Storyboard.Enums;
using OSharp.Storyboard.Models;
//using StorybrewCommon.Storyboarding;

namespace OSharp.Storyboard
{
    public class ElementGroup : IDisposable
    {
        public void Dispose() { }

        public int Index { get; set; }
        public ElementGroup(int layerIndex) => Index = layerIndex;

        public List<Element> ElementList { get; set; } = new List<Element>();

        public Element this[int index] { get => ElementList[index]; set => ElementList[index] = value; }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="filePath">File path of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(string filePath)
        {
            var obj = new Element(ElementType.Sprite, LayerType.Foreground, OriginType.Centre, filePath, 320, 240);
            Add(obj);
            return obj;
        }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="origin">Origin of the image.</param>
        /// <param name="filePath">File path of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(OriginType origin, string filePath)
        {
            var obj = new Element(ElementType.Sprite, LayerType.Foreground, origin, filePath, 320, 240);
            Add(obj);
            return obj;
        }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="layer">Layer of the image.</param>
        /// <param name="origin">Origin of the image.</param>
        /// <param name="filePath">File path of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(LayerType layer, OriginType origin, string filePath)
        {
            var obj = new Element(ElementType.Sprite, layer, origin, filePath, 320, 240);
            Add(obj);
            return obj;
        }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="layer">Layer of the image.</param>
        /// <param name="origin">Origin of the image.</param>
        /// <param name="filePath">File path of the image.</param>
        /// <param name="defaultLocation">Default location of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(LayerType layer, OriginType origin, string filePath, System.Drawing.Point defaultLocation)
        {
            var obj = new Element(ElementType.Sprite, layer, origin, filePath, defaultLocation.X, defaultLocation.Y);
            Add(obj);
            return obj;
        }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="layer">Layer of the image.</param>
        /// <param name="origin">Origin of the image.</param>
        /// <param name="filePath">File path of the image.</param>
        /// <param name="defaultX">Default x-coordinate of the image.</param>
        /// <param name="defaultY">Default y-coordinate of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(LayerType layer, OriginType origin, string filePath, int defaultX, int defaultY)
        {
            var obj = new Element(ElementType.Sprite, layer, origin, filePath, defaultX, defaultY);
            Add(obj);
            return obj;
        }

        public void Add(Element obj)
        {
            ElementList.Add(obj);
        }

        public void Add(params Element[] objs)
        {
            foreach (var obj in objs)
            {
                ElementList.Add(obj);
            }
        }

        //public void ExecuteBrew(StoryboardLayer layParsed)
        //{
        //    throw new NotImplementedException();
        //    foreach (var lib in ElementList)
        //    {
        //        lib.ExecuteBrew(layParsed);
        //    }
        //}

        public void Compress()
        {
            throw new NotImplementedException();
            //foreach (var obj in ElementList)
            //{
            //    obj.Compress();
            //}
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var obj in ElementList)
            {
                sb.Append(obj);
            }

            return sb.ToString();
        }

        public static async Task<ElementGroup> ParseAsync(string path)
        {
            return await Task.Run(() => Parse(path));
        }

        public static ElementGroup Parse(string path)
        {
            ElementGroup elementGroup = new ElementGroup(0);
            Element obj = null;
            //int currentLine = 1;
            bool isLooping = false, isTriggring = false, isBlank = false;
            using (StreamReader sr = new StreamReader(path))
            {
                int rowIndex = 0;
                do
                {
                    string line = sr.ReadLine();
                    rowIndex++;
                    if (line.StartsWith("//") || line.StartsWith("[Events]"))
                    {
                        if (line.Contains("Sound Samples"))
                            break;
                    }
                    else
                    {
                        try
                        {
                            ParseElement(line);
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Line: {rowIndex}: {e.Message}");
                        }
                    }

                    //currentLine++;
                } while (!sr.EndOfStream);
            }

            if (obj != null)
                elementGroup.Add(obj);

            return elementGroup;

            void ParseElement(string line)
            {
                var pars = line.Split(',');

                if (pars[0] == "Sprite" || pars[0] == "Animation" ||
                    pars[0] == "4" || pars[0] == "6")
                {
                    if (obj != null)
                    {
                        obj.SafeMode = true;
                        obj.TryEndLoop();
                        elementGroup.Add(obj);
                        obj = null;
                    }

                    if (pars.Length == 6)
                    {
                        obj = new Element(pars[0], pars[1], pars[2], pars[3].Trim('\"'), float.Parse(pars[4]), float.Parse(pars[5]));
                        //obj.SafeMode = false;
                        isLooping = false;
                        isTriggring = false;
                        isBlank = false;
                    }
                    else if (pars.Length == 9)
                        obj = new AnimatedElement(pars[0], pars[1], pars[2], pars[3].Trim('\"'), float.Parse(pars[4]), float.Parse(pars[5]),
                            int.Parse(pars[6]), float.Parse(pars[7]), pars[8]);
                    else
                        throw new Exception("Sprite declared wrongly");
                }
                else if (line.Trim() == "")
                {
                    isBlank = true;
                }
                else
                {
                    if (obj == null)
                        throw new Exception("Sprite need to be declared before using");
                    if (isBlank)
                        throw new Exception("Events shouldn't be declared after blank line");

                    // 验证层次是否合法
                    if (pars[0].Length - pars[0].TrimStart(' ').Length > 2 ||
                        pars[0].Length - pars[0].TrimStart('_').Length > 2)
                    {
                        throw new Exception("Unknown relation of the event");
                    }
                    else if (pars[0].StartsWith("  ") || pars[0].StartsWith("__"))
                    {
                        if (!isLooping && !isTriggring)
                            throw new Exception("The event should be looping or triggering");
                    }
                    else if (pars[0].StartsWith(" ") || pars[0].StartsWith("_"))
                    {
                        if (isLooping || isTriggring)
                        {
                            obj.EndLoop();
                            isLooping = false;
                            isTriggring = false;
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown relation of the event");
                    }

                    // 开始验证event类别
                    pars[0] = pars[0].Trim().Trim('_');

                    //if (pars.Length < 5 || pars.Length > 10)
                    //    throw new Exception("Line :" + currentLine + " (Wrong parameter for all events)");

                    string _event = pars[0];
                    int easing = -1, startTime = -1, endTime = -1;
                    if (_event != "T" && _event != "L")
                    {
                        easing = int.Parse(pars[1]);
                        if (easing > 34 || easing < 0) throw new FormatException("Unknown easing");
                        startTime = int.Parse(pars[2]);
                        endTime = pars[3] == "" ? startTime : int.Parse(pars[3]);
                    }

                    ParseCommonCommand(obj, ref isLooping, ref isTriggring, pars, _event, easing, startTime, endTime);
                }
            }
        }

        private static void ParseCommonCommand(Element obj, ref bool isLooping, ref bool isTriggring, string[] pars,
            string _event, int easing, int startTime, int endTime)
        {
            switch (pars[0])
            {
                // EventSingle
                case "F":
                case "S":
                case "R":
                case "MX":
                case "MY":
                    float p1, p2;

                    // 验证是否存在缺省
                    if (pars.Length == 5)
                        p1 = p2 = float.Parse(pars[4]);
                    else if (pars.Length == 6)
                    {
                        p1 = float.Parse(pars[4]);
                        p2 = float.Parse(pars[5]);
                    }
                    else if (pars.Length > 6)
                    {
                        var @params = pars.Skip(4).ToArray();
                        var duration = endTime - startTime;
                        for (var i = 0; i < @params.Length - 1; i++)
                        {
                            var param1 = @params[i];
                            var param2 = @params[i + 1];
                            ParseCommonCommand(obj, ref isLooping, ref isTriggring,
                                new[] { pars[0], pars[1], pars[2], pars[3], param1, param2 }, _event, easing,
                                startTime + duration * i, endTime + duration * i);
                        }

                        return;
                    }
                    else
                    {
                        throw new Exception($"Wrong parameter for event: \"{_event}\"");
                    }

                    // 开始添加成员
                    switch (_event)
                    {
                        case "F":
                            obj.Fade((EasingType)easing, startTime, endTime, p1, p2);
                            break;
                        case "S":
                            obj.Scale((EasingType)easing, startTime, endTime, p1, p2);
                            break;
                        case "R":
                            obj.Rotate((EasingType)easing, startTime, endTime, p1, p2);
                            break;
                        case "MX":
                            obj.MoveX((EasingType)easing, startTime, endTime, p1, p2);
                            break;
                        case "MY":
                            obj.MoveY((EasingType)easing, startTime, endTime, p1, p2);
                            break;
                    }
                    break;

                // EventDouble
                case "M":
                case "V":
                    float p11, p12, p21, p22;

                    // 验证是否存在缺省
                    if (pars.Length == 6)
                    {
                        p11 = p21 = float.Parse(pars[4]);
                        p12 = p22 = float.Parse(pars[5]);
                    }
                    else if (pars.Length == 8)
                    {
                        p11 = float.Parse(pars[4]);
                        p12 = float.Parse(pars[5]);
                        p21 = float.Parse(pars[6]);
                        p22 = float.Parse(pars[7]);
                    }
                    else if (pars.Length > 8 && (pars.Length - 4) % 2 == 0)
                    {
                        var @params = pars.Skip(4).ToArray();
                        var duration = endTime - startTime;
                        for (int i = 0, j = 0; i < @params.Length - 2; i += 2, j++)
                        {
                            var param11 = @params[i];
                            var param12 = @params[i + 1];
                            var param21 = @params[i + 2];
                            var param22 = @params[i + 3];
                            ParseCommonCommand(obj, ref isLooping, ref isTriggring,
                                new[] { pars[0], pars[1], pars[2], pars[3], param11, param12, param21, param22 }, _event, easing,
                                startTime + duration * j, endTime + duration * j);
                        }

                        return;
                    }
                    else
                    {
                        throw new Exception($"Wrong parameter for event: \"{_event}\"");
                    }
                    // 开始添加成员
                    switch (_event)
                    {
                        case "M":
                            obj.Move((EasingType)easing, startTime, endTime, p11, p12, p21, p22);
                            break;
                        case "V":
                            obj.Vector((EasingType)easing, startTime, endTime, p11, p12, p21, p22);
                            break;
                    }
                    break;

                // EventTriple
                case "C":
                    int c11, c12, c13, c21, c22, c23;

                    // 验证是否存在缺省
                    if (pars.Length == 7)
                    {
                        c11 = c21 = int.Parse(pars[4]);
                        c12 = c22 = int.Parse(pars[5]);
                        c13 = c23 = int.Parse(pars[6]);
                    }
                    else if (pars.Length == 10)
                    {
                        c11 = int.Parse(pars[4]);
                        c12 = int.Parse(pars[5]);
                        c13 = int.Parse(pars[6]);
                        c21 = int.Parse(pars[7]);
                        c22 = int.Parse(pars[8]);
                        c23 = int.Parse(pars[9]);
                    }
                    else if (pars.Length > 10 && (pars.Length - 4) % 3 == 0)
                    {
                        var @params = pars.Skip(4).ToArray();
                        var duration = endTime - startTime;
                        for (int i = 0, j = 0; i < @params.Length - 3; i += 3, j++)
                        {
                            var param11 = @params[i];
                            var param12 = @params[i + 1];
                            var param13 = @params[i + 2];
                            var param21 = @params[i + 3];
                            var param22 = @params[i + 4];
                            var param23 = @params[i + 5];
                            ParseCommonCommand(obj, ref isLooping, ref isTriggring,
                                new[] { pars[0], pars[1], pars[2], pars[3], param11, param12, param13, param21, param22, param23 }, _event, easing,
                                startTime + duration * j, endTime + duration * j);
                        }

                        return;
                    }
                    else
                    {
                        throw new Exception($"Wrong parameter for event: \"{_event}\"");
                    }
                    // 开始添加成员
                    switch (_event)
                    {
                        case "C":
                            obj.Color((EasingType)easing, startTime, endTime, c11, c12, c13, c21, c22, c23);
                            break;
                    }
                    break;

                case "P":
                    if (pars.Length == 5)
                    {
                        string type = pars[4];
                        obj.Parameter((EasingType)easing, startTime, endTime, type.ToEnum());
                    }
                    else
                    {
                        throw new Exception($"Wrong parameter for event: \"{_event}\"");
                    }
                    break;

                case "L":
                    if (pars.Length == 3)
                    {
                        startTime = int.Parse(pars[1]);
                        int loopCount = int.Parse(pars[2]);
                        obj.StartLoop(startTime, loopCount);
                        isLooping = true;
                    }
                    else
                    {
                        throw new Exception($"Wrong parameter for event: \"{_event}\"");
                    }
                    break;

                case "T":
                    if (pars.Length == 4)
                    {
                        string triggerType = pars[1];
                        startTime = int.Parse(pars[2]);
                        endTime = int.Parse(pars[3]);
                        obj.StartTrigger(startTime, endTime, triggerType);
                        isTriggring = true;
                    }
                    else
                    {
                        throw new Exception($"Wrong parameter for event: \"{_event}\"");
                    }
                    break;
                default:
                    throw new Exception($"Unknown event: \"{_event}\"");
            }
        }

        public static ElementGroup Parse(string osbString, int layerIndex)
        {
            StringBuilder sb = new StringBuilder();
            ElementGroup elementGroup = new ElementGroup(layerIndex);
            int currentLine = 1;
            int elmentLines = 0;

            try
            {
                var lines = osbString.Replace("\r", "").Split('\n');
                bool isFirst = true, startReading = false;

                foreach (var line in lines)
                {
                    var pars = line.Split(',');
                    if (pars[0] == "Sprite" || pars[0] == "Animation")
                    {
                        startReading = true;
                        if (isFirst) isFirst = false;
                        else ParseElement();
                    }
                    else if (line.StartsWith("//") || line.StartsWith("[Events]"))
                    {
                        if (line.Contains("Sound Samples"))
                            break;
                        elmentLines++;
                        currentLine++;
                        continue;
                    }
                    else if (isFirst)
                    {
                        throw new Exception($"Unknown script: \"{pars[0]}\" at line: {currentLine}");
                    }

                    elmentLines++;
                    sb.AppendLine(line);
                    currentLine++;
                }

                if (startReading) ParseElement();
            }
            catch (Exception ex)
            {
                throw;
            }

            return elementGroup;

            void ParseElement()
            {
                elementGroup.Add(Element.Parse(sb.ToString(), currentLine - elmentLines));
                sb.Clear();
                elmentLines = 0;
            }
        }
    }
}
