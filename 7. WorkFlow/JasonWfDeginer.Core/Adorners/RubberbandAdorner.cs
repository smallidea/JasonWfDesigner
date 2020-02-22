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
// ** Desc：RubberbandAdorner.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using JasonWfDesigner.Core.Controls;

namespace JasonWfDesigner.Core
{
    public class RubberbandAdorner : Adorner
    {
        private readonly DesignerCanvas designerCanvas;
        private Point? endPoint;
        private readonly Pen rubberbandPen;
        private Point? startPoint;

        public RubberbandAdorner(DesignerCanvas designerCanvas, Point? dragStartPoint)
            : base(designerCanvas)
        {
            this.designerCanvas = designerCanvas;
            startPoint = dragStartPoint;
            rubberbandPen = new Pen(Brushes.LightSlateGray, 1);
            rubberbandPen.DashStyle = new DashStyle(new double[] {2}, 1);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();

                endPoint = e.GetPosition(this);
                UpdateSelection();
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            // release mouse capture
            if (IsMouseCaptured) ReleaseMouseCapture();

            // remove this adorner from adorner layer
            var adornerLayer = AdornerLayer.GetAdornerLayer(designerCanvas);
            if (adornerLayer != null)
                adornerLayer.Remove(this);

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired !
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (startPoint.HasValue && endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, rubberbandPen,
                    new Rect(startPoint.Value, endPoint.Value));
        }


        private T GetParent<T>(Type parentType, DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent.GetType() == parentType)
                return (T) parent;

            return GetParent<T>(parentType, parent);
        }


        private void UpdateSelection()
        {
            var vm = designerCanvas.DataContext as IDiagramViewModel;
            var rubberBand = new Rect(startPoint.Value, endPoint.Value);
            var itemsControl = GetParent<ItemsControl>(typeof(ItemsControl), designerCanvas);

            foreach (var item in vm.Items)
                if (item is SelectableDesignerItemViewModelBase)
                {
                    var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item);

                    var itemRect = VisualTreeHelper.GetDescendantBounds((Visual) container);
                    var itemBounds = ((Visual) container).TransformToAncestor(designerCanvas)
                        .TransformBounds(itemRect);

                    if (rubberBand.Contains(itemBounds))
                    {
                        item.IsSelected = true;
                    }
                    else
                    {
                        if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                            item.IsSelected = false;
                    }
                }
        }
    }
}