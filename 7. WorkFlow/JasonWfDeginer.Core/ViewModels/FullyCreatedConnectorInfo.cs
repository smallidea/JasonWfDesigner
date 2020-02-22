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
// ** Desc：FullyCreatedConnectorInfo.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using JasonWfDesigner.Core.Controls;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core
{
    public class FullyCreatedConnectorInfo : ConnectorInfoBase
    {
        private bool showConnectors;

        public FullyCreatedConnectorInfo(DesignerItemViewModelBase dataItem, ConnectorOrientation orientation)
            : base(orientation)
        {
            DataItem = dataItem;
        }


        public DesignerItemViewModelBase DataItem { get; }

        public bool ShowConnectors
        {
            get => showConnectors;
            set
            {
                if (showConnectors != value)
                {
                    showConnectors = value;
                    NotifyChanged("ShowConnectors");
                }
            }
        }
    }
}