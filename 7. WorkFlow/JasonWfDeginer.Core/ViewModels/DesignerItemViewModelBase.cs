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
// ** Desc：DesignerItemViewModelBase.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using JasonWfDesigner.Core.Controls;

namespace JasonWfDesigner.Core.ViewModels
{
    public abstract class DesignerItemViewModelBase : SelectableDesignerItemViewModelBase
    {
        private readonly List<FullyCreatedConnectorInfo> connectors = new List<FullyCreatedConnectorInfo>();
        private double left;
        private bool showConnectors;
        private double top;

        protected DesignerItemViewModelBase(int id, IDiagramViewModel parent, double left, double top) : base(id,
            parent)
        {
            this.left = left;
            this.top = top;
            Init();
        }

        protected DesignerItemViewModelBase()
        {
            Init();
        }


        public FullyCreatedConnectorInfo TopConnector => connectors[0];


        public FullyCreatedConnectorInfo BottomConnector => connectors[1];


        public FullyCreatedConnectorInfo LeftConnector => connectors[2];


        public FullyCreatedConnectorInfo RightConnector => connectors[3];


        public static double ItemWidth { get; } = 65;

        public static double ItemHeight { get; } = 65;

        public bool ShowConnectors
        {
            get => showConnectors;
            set
            {
                if (showConnectors != value)
                {
                    showConnectors = value;
                    TopConnector.ShowConnectors = value;
                    BottomConnector.ShowConnectors = value;
                    RightConnector.ShowConnectors = value;
                    LeftConnector.ShowConnectors = value;
                    NotifyChanged("ShowConnectors");
                }
            }
        }


        public double Left
        {
            get => left;
            set
            {
                if (left != value)
                {
                    left = value;
                    NotifyChanged("Left");
                }
            }
        }

        public double Top
        {
            get => top;
            set
            {
                if (top != value)
                {
                    top = value;
                    NotifyChanged("Top");
                }
            }
        }


        private void Init()
        {
            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Top));
            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Bottom));
            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Left));
            connectors.Add(new FullyCreatedConnectorInfo(this, ConnectorOrientation.Right));
        }
    }
}