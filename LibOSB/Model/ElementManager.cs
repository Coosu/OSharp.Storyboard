using System;
using System.Collections.Generic;
using System.Text;

namespace LibOsb.Model
{
    public class ElementManager
    {
        internal List<ElementGroup> GroupList { get; set; } = new List<ElementGroup>();

        public void CreateGroup(int layerIndex)
        {
            GroupList.Add(new ElementGroup(layerIndex));
        }

        public void Add(ElementGroup elementGroup)
        {
            GroupList.Add(elementGroup);
        }

        public void SortByIndex()
        {
            GroupList.Sort(new GroupSort(GroupSortKind.Index));
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
                sb.Append(a);
            }

            return sb.ToString();
        }
        public string ToPublicString()
        {
            SortByIndex();
            StringBuilder sb = new StringBuilder();

            foreach (var a in GroupList)
                sb.Append(a.ToPublicString());

            return sb.ToString();
        }
        public void Save(string path)
        {
            System.IO.File.WriteAllText(path, "[Events]" + Environment.NewLine + "//Background and Video events" + Environment.NewLine +
                   "//Storyboard Layer 0 (Background)" + Environment.NewLine + ToPublicString() + "//Storyboard Sound Samples" + Environment.NewLine);
            // todo 还有.osu内单独部分
        }
    }
    public enum GroupSortKind { Index }
    /// <inheritdoc />
    /// <summary>
    /// SortTestObj2类排序用的比较器，继承IComparer接口，
    /// 实现接口中的Compare()方法。
    /// </summary>
    public class GroupSort : IComparer<ElementGroup>
    {
        #region 类字段定义
        private readonly GroupSortKind _sortKind;
        #endregion
        #region 构造器
        public GroupSort(GroupSortKind sk)
        {
            _sortKind = sk;
        }
        #endregion
        #region IComparer接口比较方法的实现
        public int Compare(ElementGroup obj1, ElementGroup obj2)
        {
            int res = 0;
            if ((obj1 == null) && (obj2 == null))
                return 0;

            if ((obj1 != null) && (obj2 == null))
                return 1;

            if ((obj1 == null) && (obj2 != null))
                return -1;

            if (_sortKind != GroupSortKind.Index)
                return res;

            if (obj1.Index > obj2.Index)
                res = 1;
            else if (obj1.Index < obj2.Index)
                res = -1;

            return res;
        }
        #endregion
    }
}
