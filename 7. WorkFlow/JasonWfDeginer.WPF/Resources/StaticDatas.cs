// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：StaticDatas.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using JasonWfDesigner.Common;

namespace JasonWfDesigner.WPF.Resources
{
    /// <summary>
    /// </summary>
    public class StaticDatas
    {
        /// <summary>
        /// </summary>
        public static Dictionary<string, bool> Enables = new Dictionary<string, bool>
        {
            {"不可用", false},
            {"可用", true}
        };

    }
}