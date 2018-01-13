using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOSB.ActionTypes;
using LibOSB.Constants;
namespace LibOSB
{
    public class ElementGroup : IDisposable
    {
        public void Dispose() { }

        public int Index { get; set; }

        public ElementGroup(int layerIndex)
        {
            ElementContainer.SBGroup.Add(this);
            Index = layerIndex;
        }

        private List<Element> ListObj = new List<Element>();
        internal Element this[int index] { get => ListObj[index]; set => ListObj[index] = value; }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="filePath">File path of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(string filePath)
        {
            var sbo = new Element(Types.Sprite, Layers.Foreground, Origins.Centre, filePath, 320, 240);
            Add(sbo);
            return sbo;
        }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="origin">Origin of the image.</param>
        /// <param name="filePath">File path of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(Origins origin, string filePath)
        {
            var sbo = new Element(Types.Sprite, Layers.Foreground, origin, filePath, 320, 240);
            Add(sbo);
            return sbo;
        }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="layer">Layer of the image.</param>
        /// <param name="origin">Origin of the image.</param>
        /// <param name="filePath">File path of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(Layers layer, Origins origin, string filePath)
        {
            var sbo = new Element(Types.Sprite, layer, origin, filePath, 320, 240);
            Add(sbo);
            return sbo;
        }

        /// <summary>
        /// Create a storyboard element by a static image.
        /// </summary>
        /// <param name="layer">Layer of the image.</param>
        /// <param name="origin">Origin of the image.</param>
        /// <param name="filePath">File path of the image.</param>
        /// <param name="defaultLocation">Default location of the image.</param>
        /// <returns></returns>
        public Element CreateSprite(Layers layer, Origins origin, string filePath, System.Drawing.Point defaultLocation)
        {
            var sbo = new Element(Types.Sprite, layer, origin, filePath, defaultLocation.X, defaultLocation.Y);
            Add(sbo);
            return sbo;
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
        public Element CreateSprite(Layers layer, Origins origin, string filePath, int defaultX, int defaultY)
        {
            var sbo = new Element(Types.Sprite, layer, origin, filePath, defaultX, defaultY);
            Add(sbo);
            return sbo;
        }

        public void Add(Element obj)
        {
            ListObj.Add(obj);
        }

        public void Add(params Element[] objs)
        {
            foreach (var obj in objs)
            {
                ListObj.Add(obj);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var a in ListObj)
            {
                sb.Append(a.ToString());
            }

            return sb.ToString();
        }
    }
}
