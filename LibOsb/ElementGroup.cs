using System;
using System.Collections.Generic;
using System.Text;
using LibOsb.Compress;
using LibOsb.Enums;
using LibOsb.Utils;
using LibOsb.Utils.BrewExtension;
using StorybrewCommon.Storyboarding;

namespace LibOsb
{
    public class ElementGroup : IDisposable
    {
        public void Dispose() { }

        public int Index { get; set; }
        public ElementGroup(int layerIndex) => Index = layerIndex;

        internal List<Element> ElementList { get; set; } = new List<Element>();

        internal Element this[int index] { get => ElementList[index]; set => ElementList[index] = value; }

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

        public void ExecuteBrew(StoryboardLayer layParsed)
        {
            foreach (var lib in ElementList)
            {
                lib.ExecuteBrew(layParsed);
            }
        }

        public void Compress()
        {
            foreach (var obj in ElementList)
            {
                obj.Compress();
            }
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
        public static ElementGroup Parse(string osbString, int layerIndex)
        {
            StringBuilder sb = new StringBuilder();
            ElementGroup obj = null;
            int currentLine = 1;

            try
            {
                var lines = osbString.Replace("\r", "").Split('\n');
                bool isFirst = true, isOpen = false;
                int lineCount = 0;
                foreach (var line in lines)
                {
                    var pars = line.Split(',');
                    if (pars[0] == "Sprite" || pars[0] == "Animation")
                    {
                        isOpen = true;
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            if (obj == null)
                                obj = new ElementGroup(layerIndex);
                            obj.Add(Element.Parse(sb.ToString(), currentLine - lineCount));
                            sb.Clear();
                            lineCount = 0;
                        }
                    }
                    else if (line.IndexOf("//", StringComparison.Ordinal) == 0 || line.IndexOf("[Events]", StringComparison.Ordinal) == 0)
                    {
                        lineCount++;
                        currentLine++;
                        continue;
                    }
                    else if (isFirst)
                    {
                        throw new Exception($"Unknown script: \"{pars[0]}\" at line: {currentLine}");
                    }
                    lineCount++;
                    sb.AppendLine(line);
                    currentLine++;
                }
                if (isOpen)
                {
                    if (obj == null)
                        obj = new ElementGroup(layerIndex);
                    obj.Add(Element.Parse(sb.ToString(), currentLine - lineCount));
                    sb.Clear();
                    isOpen = false;
                }
            }
            catch
            {
                // ignored
            }

            return obj;
        }
    }
}
