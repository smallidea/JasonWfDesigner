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
// ** Desc：IGlobal.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;

namespace JasonWfDesigner.Common
{
    /// <summary>
    ///     总
    /// </summary>
    public abstract class GlobalBase
    {
        /// <summary>
        /// </summary>
        public static Dictionary<string, ProductAndFlow> nodeWithProductIdDictionary { get; set; } =
            new Dictionary<string, ProductAndFlow>();

        /// <summary>
        ///     清空数据
        /// </summary>
        public abstract void CleanData();

        public class ProductAndFlow
        {
            public int ProductId { get; set; }
            public int FlowDirection { get; set; }
        }
    }
}