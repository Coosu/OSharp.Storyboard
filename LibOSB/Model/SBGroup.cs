using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB
{
    public class SBGroup : IDisposable
    {
        public void Dispose() { }

        public int Index { get; set; }

        public SBGroup(int index)
        {
            SBContainer.SBGroup.Add(this);
            Index = index;
        }

        private List<SBObject> ListObj = new List<SBObject>();
        internal SBObject this[int index] { get => ListObj[index]; set => ListObj[index] = value; }

        internal void Add(SBObject obj)
        {
            ListObj.Add(obj);
        }

        internal void Add(params SBObject[] objs)
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
