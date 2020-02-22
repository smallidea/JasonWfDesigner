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
// ** Desc：ConnectorViewModel.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using JasonWfDesigner.Core.Controls;
using JasonWfDesigner.Core.Helpers;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core
{
    /// <summary>
    /// </summary>
    public class ConnectorViewModel : SelectableDesignerItemViewModelBase
    {
        private Rect area;
        private List<Point> connectionPoints;
        private Point endPoint;
        private ConnectorInfoBase sinkConnectorInfo;
        private Point sourceA;
        private Point sourceB;
        private FullyCreatedConnectorInfo sourceConnectorInfo;


        public ConnectorViewModel(int id, IDiagramViewModel parent,
            FullyCreatedConnectorInfo sourceConnectorInfo, FullyCreatedConnectorInfo sinkConnectorInfo) : base(id,
            parent)
        {
            Init(sourceConnectorInfo, sinkConnectorInfo);
        }

        public ConnectorViewModel(FullyCreatedConnectorInfo sourceConnectorInfo, ConnectorInfoBase sinkConnectorInfo)
        {
            Init(sourceConnectorInfo, sinkConnectorInfo);
        }


        public static IPathFinder PathFinder { get; set; }

        public bool IsFullConnection => sinkConnectorInfo is FullyCreatedConnectorInfo;

        public Point SourceA
        {
            get => sourceA;
            set
            {
                if (sourceA != value)
                {
                    sourceA = value;
                    UpdateArea();
                    NotifyChanged("SourceA");
                }
            }
        }

        public Point SourceB
        {
            get => sourceB;
            set
            {
                if (sourceB != value)
                {
                    sourceB = value;
                    UpdateArea();
                    NotifyChanged("SourceB");
                }
            }
        }

        public List<Point> ConnectionPoints
        {
            get => connectionPoints;
            private set
            {
                if (connectionPoints != value)
                {
                    connectionPoints = value;
                    NotifyChanged("ConnectionPoints");
                }
            }
        }

        public Point EndPoint
        {
            get => endPoint;
            private set
            {
                if (endPoint != value)
                {
                    endPoint = value;
                    NotifyChanged("EndPoint");
                }
            }
        }

        public Rect Area
        {
            get => area;
            private set
            {
                if (area != value)
                {
                    area = value;
                    UpdateConnectionPoints();
                    NotifyChanged("Area");
                }
            }
        }

        public FullyCreatedConnectorInfo SourceConnectorInfo
        {
            get => sourceConnectorInfo;
            set
            {
                if (sourceConnectorInfo != value)
                {
                    sourceConnectorInfo = value;
                    SourceA = PointHelper.GetPointForConnector(SourceConnectorInfo);
                    NotifyChanged("SourceConnectorInfo");
                    (sourceConnectorInfo.DataItem as INotifyPropertyChanged).PropertyChanged +=
                        new WeakINPCEventHandler(ConnectorViewModel_PropertyChanged).Handler;
                }
            }
        }

        public ConnectorInfoBase SinkConnectorInfo
        {
            get => sinkConnectorInfo;
            set
            {
                if (sinkConnectorInfo != value)
                {
                    sinkConnectorInfo = value;
                    if (SinkConnectorInfo is FullyCreatedConnectorInfo)
                    {
                        SourceB = PointHelper.GetPointForConnector((FullyCreatedConnectorInfo) SinkConnectorInfo);
                        (((FullyCreatedConnectorInfo) sinkConnectorInfo).DataItem as INotifyPropertyChanged)
                            .PropertyChanged += new WeakINPCEventHandler(ConnectorViewModel_PropertyChanged).Handler;
                    }
                    else
                    {
                        SourceB = ((PartCreatedConnectionInfo) SinkConnectorInfo).CurrentLocation;
                    }

                    NotifyChanged("SinkConnectorInfo");
                }
            }
        }

        public ConnectorInfo ConnectorInfo(ConnectorOrientation orientation, double left, double top, Point position)
        {
            return new ConnectorInfo
            {
                Orientation = orientation,
                DesignerItemSize = new Size(DesignerItemViewModelBase.ItemWidth, DesignerItemViewModelBase.ItemHeight),
                DesignerItemLeft = left,
                DesignerItemTop = top,
                Position = position
            };
        }

        private void UpdateArea()
        {
            Area = new Rect(SourceA, SourceB);
        }

        private void UpdateConnectionPoints()
        {
            ConnectionPoints = new List<Point>
            {
                new Point(SourceA.X < SourceB.X ? 0d : Area.Width, SourceA.Y < SourceB.Y ? 0d : Area.Height),
                new Point(SourceA.X > SourceB.X ? 0d : Area.Width, SourceA.Y > SourceB.Y ? 0d : Area.Height)
            };

            var sourceInfo = ConnectorInfo(SourceConnectorInfo.Orientation,
                ConnectionPoints[0].X,
                ConnectionPoints[0].Y,
                ConnectionPoints[0]);

            if (IsFullConnection)
            {
                EndPoint = ConnectionPoints.Last();
                var sinkInfo = ConnectorInfo(SinkConnectorInfo.Orientation,
                    ConnectionPoints[1].X,
                    ConnectionPoints[1].Y,
                    ConnectionPoints[1]);

                ConnectionPoints = PathFinder.GetConnectionLine(sourceInfo, sinkInfo, true);
            }
            else
            {
                ConnectionPoints =
                    PathFinder.GetConnectionLine(sourceInfo, ConnectionPoints[1], ConnectorOrientation.Left);
                EndPoint = new Point();
            }
        }

        private void ConnectorViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Left":
                case "Top":
                    SourceA = PointHelper.GetPointForConnector(SourceConnectorInfo);
                    if (SinkConnectorInfo is FullyCreatedConnectorInfo)
                        SourceB = PointHelper.GetPointForConnector((FullyCreatedConnectorInfo) SinkConnectorInfo);
                    break;
            }
        }

        private void Init(FullyCreatedConnectorInfo sourceConnectorInfo, ConnectorInfoBase sinkConnectorInfo)
        {
            Parent = sourceConnectorInfo.DataItem.Parent;
            SourceConnectorInfo = sourceConnectorInfo;
            SinkConnectorInfo = sinkConnectorInfo;
            PathFinder = new OrthogonalPathFinder();
        }
    }
}