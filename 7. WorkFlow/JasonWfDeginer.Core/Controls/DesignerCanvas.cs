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
// ** Desc：DesignerCanvas.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core.Controls
{
    public class DesignerCanvas : Canvas
    {
        private List<Connector> _connectorsHit = new List<Connector>();

        private ConnectorViewModel _partialConnection;
        private Point? _rubberbandSelectionStartPoint;
        private Connector _sourceConnector;

        public DesignerCanvas()
        {
            AllowDrop = true;
            Mediator.Instance.Register(this);
        }


        public Connector SourceConnector
        {
            get => _sourceConnector;
            set
            {
                if (_sourceConnector != value)
                {
                    _sourceConnector = value;
                    _connectorsHit.Add(_sourceConnector);
                    var sourceDataItem = _sourceConnector.DataContext as FullyCreatedConnectorInfo;

                    var rectangleBounds = _sourceConnector.TransformToVisual(this)
                        .TransformBounds(new Rect(_sourceConnector.RenderSize));
                    var point = new Point(rectangleBounds.Left + rectangleBounds.Width / 2,
                        rectangleBounds.Bottom + rectangleBounds.Height / 2);
                    _partialConnection = new ConnectorViewModel(sourceDataItem, new PartCreatedConnectionInfo(point));
                    sourceDataItem?.DataItem.Parent.AddItemCommand.Execute(_partialConnection);
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //if we are source of event, we are rubberband selecting
                if (!Equals(e.Source, this)) return;

                // in case that this click is the start for a 
                // drag operation we cache the start point
                _rubberbandSelectionStartPoint = e.GetPosition(this);

                var vm = DataContext as IDiagramViewModel;
                if (vm == null) return;
                if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    vm.ClearSelectedItemsCommand.Execute(null);
                e.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            Mediator.Instance.NotifyColleagues("DoneDrawingMessage", true);

            if (_sourceConnector != null)
            {
                var sourceDataItem = _sourceConnector.DataContext as FullyCreatedConnectorInfo;
                if (_connectorsHit.Count() == 2)
                {
                    var sinkConnector = _connectorsHit.Last();

                    if (sinkConnector.DataContext is FullyCreatedConnectorInfo sinkDataItem)
                    {
                        var indexOfLastTempConnection = sinkDataItem.DataItem.Parent.Items.Count - 1;
                        sinkDataItem.DataItem.Parent.RemoveItemCommand.Execute(
                            sinkDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);

                        sinkDataItem.DataItem.Parent.AddItemCommand.Execute(new ConnectorViewModel(sourceDataItem,
                            sinkDataItem));
                    }
                }
                else
                {
                    //Need to remove last item as we did not finish drawing the path
                    if (sourceDataItem != null)
                    {
                        var indexOfLastTempConnection = sourceDataItem.DataItem.Parent.Items.Count - 1;
                        sourceDataItem.DataItem.Parent.RemoveItemCommand.Execute(
                            sourceDataItem.DataItem.Parent.Items[indexOfLastTempConnection]);
                    }
                }
            }

            _connectorsHit = new List<Connector>();
            _sourceConnector = null;
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (SourceConnector != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    var currentPoint = e.GetPosition(this);
                    _partialConnection.SinkConnectorInfo = new PartCreatedConnectionInfo(currentPoint);
                    HitTesting(currentPoint);
                }
            }
            else
            {
                // if mouse button is not pressed we have no drag operation, ...
                if (e.LeftButton != MouseButtonState.Pressed)
                    _rubberbandSelectionStartPoint = null;

                // ... but if mouse button is pressed and start
                // point value is set we do have one
                if (_rubberbandSelectionStartPoint.HasValue)
                {
                    // create rubberband adorner
                    var adorerLayer = AdornerLayer.GetAdornerLayer(this);
                    if (adorerLayer != null)
                    {
                        var adorer = new RubberbandAdorner(this, _rubberbandSelectionStartPoint);
                        adorerLayer.Add(adorer);
                    }
                }
            }

            e.Handled = true;
        }


        protected override Size MeasureOverride(Size constraint)
        {
            var size = new Size();

            foreach (UIElement element in InternalChildren)
            {
                var left = GetLeft(element);
                var top = GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                var desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }

            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        private void HitTesting(Point hitPoint)
        {
            var hitObject = InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                   hitObject.GetType() != typeof(DesignerCanvas))
            {
                if (hitObject is Connector)
                    if (!_connectorsHit.Contains(hitObject as Connector))
                        _connectorsHit.Add(hitObject as Connector);
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }
        }


        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (e.Data.GetData(typeof(DragObject)) is DragObject dragObject)
            {
                (DataContext as IDiagramViewModel)?.ClearSelectedItemsCommand.Execute(null);
                var position = e.GetPosition(this);
                var itemBase =
                    (DesignerItemViewModelBase) Activator.CreateInstance(dragObject.ContentType);
                itemBase.Left = Math.Max(0, position.X - DesignerItemViewModelBase.ItemWidth / 2);
                itemBase.Top = Math.Max(0, position.Y - DesignerItemViewModelBase.ItemHeight / 2);
                itemBase.IsSelected = true;
                (DataContext as IDiagramViewModel)?.AddItemCommand.Execute(itemBase);
            }

            e.Handled = true;
        }
    }
}