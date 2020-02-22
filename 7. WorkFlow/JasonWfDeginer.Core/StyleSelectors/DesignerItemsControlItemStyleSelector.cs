// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Core
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：DesignerItemsControlItemStyleSelector.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Windows;
using System.Windows.Controls;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core.StyleSelectors
{
    public class DesignerItemsControlItemStyleSelector : StyleSelector
    {
        static DesignerItemsControlItemStyleSelector()
        {
            Instance = new DesignerItemsControlItemStyleSelector();
        }

        public static DesignerItemsControlItemStyleSelector Instance { get; }


        public override Style SelectStyle(object item, DependencyObject container)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            if (itemsControl == null)
                throw new InvalidOperationException(
                    "DesignerItemsControlItemStyleSelector : Could not find ItemsControl");

            if (item is DesignerItemViewModelBase) return (Style) itemsControl.FindResource("designerItemStyle");

            if (item is ConnectorViewModel) return (Style) itemsControl.FindResource("connectorItemStyle");

            return null;
        }
    }
}