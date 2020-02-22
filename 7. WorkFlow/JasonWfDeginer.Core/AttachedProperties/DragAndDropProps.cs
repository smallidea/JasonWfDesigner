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
// ** Desc：DragAndDropProps.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Windows;
using System.Windows.Input;
using JasonWfDesigner.Core.Helpers;

namespace JasonWfDesigner.Core
{
    public static class DragAndDropProps
    {
        private static void Fe_MouseMove(object sender, MouseEventArgs e)
        {
            var dragStartPoint = GetDragStartPoint((DependencyObject) sender);

            if (e.LeftButton != MouseButtonState.Pressed)
                dragStartPoint = null;

            if (dragStartPoint.HasValue)
            {
                var dataObject = new DragObject();
                dataObject.ContentType = (((FrameworkElement) sender).DataContext as ToolBoxData).Type;
                dataObject.DesiredSize = new Size(65, 65);
                DragDrop.DoDragDrop((DependencyObject) sender, dataObject, DragDropEffects.Copy);
                e.Handled = true;
            }
        }

        private static void Fe_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetDragStartPoint((DependencyObject) sender, e.GetPosition((IInputElement) sender));
        }

        #region EnabledForDrag

        public static readonly DependencyProperty EnabledForDragProperty =
            DependencyProperty.RegisterAttached("EnabledForDrag", typeof(bool), typeof(DragAndDropProps),
                new FrameworkPropertyMetadata(false,
                    OnEnabledForDragChanged));

        public static bool GetEnabledForDrag(DependencyObject d)
        {
            return (bool) d.GetValue(EnabledForDragProperty);
        }

        public static void SetEnabledForDrag(DependencyObject d, bool value)
        {
            d.SetValue(EnabledForDragProperty, value);
        }

        private static void OnEnabledForDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement) d;


            if ((bool) e.NewValue)
            {
                fe.PreviewMouseDown += Fe_PreviewMouseDown;
                fe.MouseMove += Fe_MouseMove;
            }
            else
            {
                fe.PreviewMouseDown -= Fe_PreviewMouseDown;
                fe.MouseMove -= Fe_MouseMove;
            }
        }

        #endregion

        #region DragStartPoint

        public static readonly DependencyProperty DragStartPointProperty =
            DependencyProperty.RegisterAttached("DragStartPoint", typeof(Point?), typeof(DragAndDropProps));

        public static Point? GetDragStartPoint(DependencyObject d)
        {
            return (Point?) d.GetValue(DragStartPointProperty);
        }


        public static void SetDragStartPoint(DependencyObject d, Point? value)
        {
            d.SetValue(DragStartPointProperty, value);
        }

        #endregion
    }
}