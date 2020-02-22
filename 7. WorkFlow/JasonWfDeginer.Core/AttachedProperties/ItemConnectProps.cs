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
// ** Desc：ItemConnectProps.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Windows;
using System.Windows.Input;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core.AttachedProperties
{
    public static class ItemConnectProps
    {
        private static void Fe_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((FrameworkElement) sender).DataContext is DesignerItemViewModelBase)
            {
                var designerItem =
                    (DesignerItemViewModelBase) ((FrameworkElement) sender).DataContext;
                designerItem.ShowConnectors = true;
            }
        }

        #region EnabledForConnection

        public static readonly DependencyProperty EnabledForConnectionProperty =
            DependencyProperty.RegisterAttached("EnabledForConnection", typeof(bool), typeof(ItemConnectProps),
                new FrameworkPropertyMetadata(false,
                    OnEnabledForConnectionChanged));

        public static bool GetEnabledForConnection(DependencyObject d)
        {
            return (bool) d.GetValue(EnabledForConnectionProperty);
        }

        public static void SetEnabledForConnection(DependencyObject d, bool value)
        {
            d.SetValue(EnabledForConnectionProperty, value);
        }

        private static void OnEnabledForConnectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement) d;


            if ((bool) e.NewValue)
                fe.MouseEnter += Fe_MouseEnter;
            else
                fe.MouseEnter -= Fe_MouseEnter;
        }

        #endregion
    }
}