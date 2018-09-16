using Milkitic.OsbLib.Enums;
using Milkitic.OsbLib.Extension;
//using StorybrewCommon.Storyboarding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Milkitic.OsbLib
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
                    else if (line.IndexOf("//", StringComparison.Ordinal) == 0 || line.IndexOf("[Events]", StringComparison.Ordinal) == 0)
                    {
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
                throw ex;
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
