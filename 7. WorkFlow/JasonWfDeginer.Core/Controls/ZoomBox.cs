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
// ** Desc：ZoomBox.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace JasonWfDesigner.Core.Controls
{
    public class ZoomBox : Control
    {
        private ScaleTransform scaleTransform;
        private Canvas zoomCanvas;
        private Slider zoomSlider;
        private Thumb zoomThumb;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ScrollViewer == null)
                return;

            zoomThumb = Template.FindName("PART_ZoomThumb", this) as Thumb;
            if (zoomThumb == null)
                throw new Exception("PART_ZoomThumb template is missing!");

            zoomCanvas = Template.FindName("PART_ZoomCanvas", this) as Canvas;
            if (zoomCanvas == null)
                throw new Exception("PART_ZoomCanvas template is missing!");

            zoomSlider = Template.FindName("PART_ZoomSlider", this) as Slider;
            if (zoomSlider == null)
                throw new Exception("PART_ZoomSlider template is missing!");

            zoomThumb.DragDelta += Thumb_DragDelta;
            zoomSlider.ValueChanged += ZoomSlider_ValueChanged;
            scaleTransform = new ScaleTransform();
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var scale = e.NewValue / e.OldValue;
            var halfViewportHeight = ScrollViewer.ViewportHeight / 2;
            var newVerticalOffset =
                (ScrollViewer.VerticalOffset + halfViewportHeight) * scale - halfViewportHeight;
            var halfViewportWidth = ScrollViewer.ViewportWidth / 2;
            var newHorizontalOffset =
                (ScrollViewer.HorizontalOffset + halfViewportWidth) * scale - halfViewportWidth;
            scaleTransform.ScaleX *= scale;
            scaleTransform.ScaleY *= scale;
            ScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);
            ScrollViewer.ScrollToVerticalOffset(newVerticalOffset);
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double scale, xOffset, yOffset;
            InvalidateScale(out scale, out xOffset, out yOffset);
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + e.HorizontalChange / scale);
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + e.VerticalChange / scale);
        }

        private void DesignerCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            double scale, xOffset, yOffset;
            InvalidateScale(out scale, out xOffset, out yOffset);
            zoomThumb.Width = ScrollViewer.ViewportWidth * scale;
            zoomThumb.Height = ScrollViewer.ViewportHeight * scale;
            Canvas.SetLeft(zoomThumb, xOffset + ScrollViewer.HorizontalOffset * scale);
            Canvas.SetTop(zoomThumb, yOffset + ScrollViewer.VerticalOffset * scale);
        }

        private void DesignerCanvas_MouseWheel(object sender, EventArgs e)
        {
            var wheel = (MouseWheelEventArgs) e;

            //divide the value by 10 so that it is more smooth
            double value = Math.Max(0, wheel.Delta / 10);
            value = Math.Min(wheel.Delta, 10);
            zoomSlider.Value += value;
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            var w = DesignerCanvas.ActualWidth * scaleTransform.ScaleX;
            var h = DesignerCanvas.ActualHeight * scaleTransform.ScaleY;

            // zoom canvas size
            var x = zoomCanvas.ActualWidth;
            var y = zoomCanvas.ActualHeight;
            var scaleX = x / w;
            var scaleY = y / h;
            scale = scaleX < scaleY ? scaleX : scaleY;
            xOffset = (x - scale * w) / 2;
            yOffset = (y - scale * h) / 2;
        }

        #region DPs

        #region ScrollViewer

        public ScrollViewer ScrollViewer
        {
            get => (ScrollViewer) GetValue(ScrollViewerProperty);
            set => SetValue(ScrollViewerProperty, value);
        }

        public static readonly DependencyProperty ScrollViewerProperty =
            DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(ZoomBox));

        #endregion

        #region DesignerCanvas

        public static readonly DependencyProperty DesignerCanvasProperty =
            DependencyProperty.Register("DesignerCanvas", typeof(DesignerCanvas), typeof(ZoomBox),
                new FrameworkPropertyMetadata(null,
                    OnDesignerCanvasChanged));


        public DesignerCanvas DesignerCanvas
        {
            get => (DesignerCanvas) GetValue(DesignerCanvasProperty);
            set => SetValue(DesignerCanvasProperty, value);
        }


        private static void OnDesignerCanvasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ZoomBox) d;
            var oldDesignerCanvas = (DesignerCanvas) e.OldValue;
            var newDesignerCanvas = target.DesignerCanvas;
            target.OnDesignerCanvasChanged(oldDesignerCanvas, newDesignerCanvas);
        }


        protected virtual void OnDesignerCanvasChanged(DesignerCanvas oldDesignerCanvas,
            DesignerCanvas newDesignerCanvas)
        {
            if (oldDesignerCanvas != null)
            {
                newDesignerCanvas.LayoutUpdated -= DesignerCanvas_LayoutUpdated;
                newDesignerCanvas.MouseWheel -= DesignerCanvas_MouseWheel;
            }

            if (newDesignerCanvas != null)
            {
                newDesignerCanvas.LayoutUpdated += DesignerCanvas_LayoutUpdated;
                newDesignerCanvas.MouseWheel += DesignerCanvas_MouseWheel;
                newDesignerCanvas.LayoutTransform = scaleTransform;
            }
        }

        #endregion

        #endregion
    }
}