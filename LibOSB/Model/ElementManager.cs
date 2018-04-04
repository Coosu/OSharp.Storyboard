using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOsb
{
    public class ElementManager
    {
        internal List<ElementGroup> GroupList { get; set; }

        public void SortByIndex()
        {
            GroupList.Sort(new GroupSort(GroupSortKind.Index));
        }
        public void CreateGroup(int layerIndex)
        {
            GroupList.Add(new ElementGroup(layerIndex));
        }
        public void Add(ElementGroup elementGroup)
        {
            GroupList.Add(elementGroup);
        }
        public static ElementGroup Adjust(ElementGroup elementGroup, double offsetX, double offsetY, int offsetTiming)
        {
            foreach (var obj in elementGroup.ElementList)
            {
                obj._Adjust(offsetX, offsetY, offsetTiming);
            }
            return elementGroup;
        }
        public override string ToString()
        {
            SortByIndex();
            StringBuilder sb = new StringBuilder();

            foreach (var a in GroupList)
            {
                sb.Append(a.ToString());
            }

            return sb.ToString();
        }
    }
    public enum GroupSortKind { Index }
    /// <summary>
    /// SortTestObj2类排序用的比较器，继承IComparer接口，
    /// 实现接口中的Compare()方法。
    /// </summary>
    public class GroupSort : IComparer<ElementGroup>
    {
        #region 类字段定义
        private GroupSortKind sortKind;
        #endregion
        #region 构造器
        public GroupSort(GroupSortKind sk)
        {
            this.sortKind = sk;
        }
        #endregion
        #region IComparer接口比较方法的实现
        public int Compare(ElementGroup obj1, ElementGroup obj2)
        {
            int res = 0;
            if ((obj1 == null) && (obj2 == null))
            {
                return 0;
            }
            else if ((obj1 != null) && (obj2 == null))
            {
                return 1;
            }
            else if ((obj1 == null) && (obj2 != null))
            {
                return -1;
            }

            if (sortKind == GroupSortKind.Index)
            {
                if (obj1.Index > obj2.Index)
                {
                    res = 1;
                }
                else if (obj1.Index < obj2.Index)
                {
                    res = -1;
                }
            }
            //else if (sortKind == GroupSortKind.Name)
            //{
            //    res = obj1.Name.CompareTo(obj2.Name);
            //}
            return res;
        }
        #endregion
    }
}
