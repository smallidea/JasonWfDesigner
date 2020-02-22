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
// ** Desc：SelectionProps.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Windows;
using System.Windows.Input;

namespace JasonWfDesigner.Core
{
    public static class SelectionProps
    {
        private static void Fe_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectableDesignerItemViewModelBase =
                (SelectableDesignerItemViewModelBase) ((FrameworkElement) sender).DataContext;

            if (selectableDesignerItemViewModelBase != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
                        selectableDesignerItemViewModelBase.IsSelected =
                            !selectableDesignerItemViewModelBase.IsSelected;

                    if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                        selectableDesignerItemViewModelBase.IsSelected =
                            !selectableDesignerItemViewModelBase.IsSelected;
                }
                else if (!selectableDesignerItemViewModelBase.IsSelected)
                {
                    foreach (var item in selectableDesignerItemViewModelBase.Parent
                        .SelectedItems)
                        item.IsSelected = false;

                    selectableDesignerItemViewModelBase.Parent.SelectedItems.Clear();
                    selectableDesignerItemViewModelBase.IsSelected = true;
                }
            }
        }

        #region EnabledForSelection

        public static readonly DependencyProperty EnabledForSelectionProperty =
            DependencyProperty.RegisterAttached("EnabledForSelection", typeof(bool), typeof(SelectionProps),
                new FrameworkPropertyMetadata(false,
                    OnEnabledForSelectionChanged));

        public static bool GetEnabledForSelection(DependencyObject d)
        {
            return (bool) d.GetValue(EnabledForSelectionProperty);
        }

        public static void SetEnabledForSelection(DependencyObject d, bool value)
        {
            d.SetValue(EnabledForSelectionProperty, value);
        }

        private static void OnEnabledForSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement) d;
            if ((bool) e.NewValue)
                fe.PreviewMouseDown += Fe_PreviewMouseDown;
            else
                fe.PreviewMouseDown -= Fe_PreviewMouseDown;
        }

        #endregion
    }
}