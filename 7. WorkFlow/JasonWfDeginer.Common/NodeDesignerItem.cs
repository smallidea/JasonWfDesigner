// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Common
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：nodeDesignerItem.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

namespace JasonWfDesigner.Common
{
    public class NodeDesignerItem : DesignerItemBase
    {
        public NodeDesignerItem(int id, double left, double top, string key, int value) : base(id, left, top)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Desc { get; set; }

        public string NodeType { get; set; }

        public string BusinessObj { get; set; }

        public int Value { get; set; }

    }

    //TODO: 好繁琐，要修改
}