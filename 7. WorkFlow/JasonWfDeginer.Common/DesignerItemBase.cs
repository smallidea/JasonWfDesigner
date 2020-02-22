// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Common
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：DesignerItemBase.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

namespace JasonWfDesigner.Common
{
    public abstract class DesignerItemBase : PersistableItemBase
    {
        public DesignerItemBase(int id, double left, double top) : base(id)
        {
            Left = left;
            Top = top;
        }

        public double Left { get; }
        public double Top { get; }
    }
}