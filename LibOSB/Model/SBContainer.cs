using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibOSB
{
    static public class SBContainer
    {
        private static List<SBGroup> sbGroup = new List<SBGroup>();

        public static List<SBGroup> SBGroup { get => sbGroup; set => sbGroup = value; }

        public static new string ToString()
        {
            SortByIndex();
            StringBuilder sb = new StringBuilder();

            foreach (var a in SBGroup)
            {
                sb.Append(a.ToString());
            }

            return sb.ToString();
        }
        private static void SortByIndex()
        {
            SBGroup.Sort(new GroupSort(GroupSortKind.Index));
        }
    }
    public enum GroupSortKind { Index }
    /// <summary>
    /// SortTestObj2类排序用的比较器，继承IComparer<>接口，
    /// 实现接口中的Compare()方法。
    /// </summary>
    public class GroupSort : IComparer<SBGroup>
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
        public int Compare(SBGroup obj1, SBGroup obj2)
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
