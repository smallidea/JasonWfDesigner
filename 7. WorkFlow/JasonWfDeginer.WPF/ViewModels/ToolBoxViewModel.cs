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
// ** Desc：ToolBoxViewModel.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using JasonWfDesigner.Core.Helpers;

namespace JasonWfDesigner.WPF.ViewModels
{
    public class ToolBoxViewModel
    {
        public ToolBoxViewModel()
        {
            ToolBoxItems.Add(new ToolBoxData("../Images/Persist.png", typeof(NodeDesignerItemViewModel)));
            //toolBoxItems.Add(new ToolBoxData("../Images/Persist.png", typeof(PersistDesignerItemViewModel)));
        }

        public List<ToolBoxData> ToolBoxItems { get; } = new List<ToolBoxData>();
    }
}