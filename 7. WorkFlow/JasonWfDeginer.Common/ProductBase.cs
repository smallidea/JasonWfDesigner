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
// ** Desc：ProductBase.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using NJIS.AppUtility;

namespace JasonWfDesigner.Common
{
    /// <summary>
    /// </summary>
    public abstract class ProductBase
    {
        public abstract int Trigger { get; set; }

        public abstract int Id { get; set; }

        public abstract string Key { get; set; }

        public StatusEnum StatusBase { get; set; }

        public class StatusEnum : HeritableEnum
        {
            public static StatusEnum NG1 = new StatusEnum("401");
            public static StatusEnum NG2 = new StatusEnum("402");
            public static StatusEnum 入库成功 = new StatusEnum("199");
            public static StatusEnum 正常 = new StatusEnum("666");

            protected StatusEnum(string name) : base(name)
            {
            }

            protected StatusEnum(string name, int value) : base(name, value)
            {
            }
        }
    }
}