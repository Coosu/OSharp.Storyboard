using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibOSB.ActionTypes;
using LibOSB.Constants;
namespace LibOSB
{
    public class SBGroup : IDisposable
    {
        public void Dispose() { }

        public int Index { get; set; }

        public SBGroup(int layerIndex)
        {
            SBContainer.SBGroup.Add(this);
            Index = layerIndex;
        }

        private List<SBObject> ListObj = new List<SBObject>();
        internal SBObject this[int index] { get => ListObj[index]; set => ListObj[index] = value; }

        public SBObject CreateSprite(string filePath)
        {
            var sbo = new SBObject(Types.Sprite, Layers.Foreground, Origins.Centre, filePath, 320, 240);
            Add(sbo);
            return sbo;
        }
        public SBObject CreateSprite(Origins origin, string filePath)
        {
            var sbo = new SBObject(Types.Sprite, Layers.Foreground, origin, filePath, 320, 240);
            Add(sbo);
            return sbo;
        }
        public SBObject CreateSprite(Layers layer, Origins origin, string filePath)
        {
            var sbo = new SBObject(Types.Sprite, layer, origin, filePath, 320, 240);
            Add(sbo);
            return sbo;
        }
        public SBObject CreateSprite(Layers layer, Origins origin, string filePath, System.Drawing.Point defaultLocation)
        {
            var sbo = new SBObject(Types.Sprite, layer, origin, filePath, defaultLocation.X, defaultLocation.Y);
            Add(sbo);
            return sbo;
        }

        public SBObject CreateSprite(Layers layer, Origins origin, string filePath, double defaultX, double defaultY)
        {
            var sbo = new SBObject(Types.Sprite, layer, origin, filePath, defaultX, defaultY);
            Add(sbo);
            return sbo;
        }

        public void Add(SBObject obj)
        {
            ListObj.Add(obj);
        }

        public void Add(params SBObject[] objs)
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
