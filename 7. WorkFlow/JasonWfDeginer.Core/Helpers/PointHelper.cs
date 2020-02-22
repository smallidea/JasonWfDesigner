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
// ** Desc：PointHelper.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Windows;
using JasonWfDesigner.Core.Controls;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core
{
    public class PointHelper
    {
        public static Point GetPointForConnector(FullyCreatedConnectorInfo connector)
        {
            var point = new Point();

            switch (connector.Orientation)
            {
                case ConnectorOrientation.Top:
                    point = new Point(connector.DataItem.Left + DesignerItemViewModelBase.ItemWidth / 2,
                        connector.DataItem.Top - ConnectorInfoBase.ConnectorHeight);
                    break;
                case ConnectorOrientation.Bottom:
                    point = new Point(connector.DataItem.Left + DesignerItemViewModelBase.ItemWidth / 2,
                        connector.DataItem.Top + DesignerItemViewModelBase.ItemHeight +
                        ConnectorInfoBase.ConnectorHeight / 2);
                    break;
                case ConnectorOrientation.Right:
                    point = new Point(
                        connector.DataItem.Left + DesignerItemViewModelBase.ItemWidth +
                        ConnectorInfoBase.ConnectorWidth,
                        connector.DataItem.Top + DesignerItemViewModelBase.ItemHeight / 2);
                    break;
                case ConnectorOrientation.Left:
                    point = new Point(connector.DataItem.Left - ConnectorInfoBase.ConnectorWidth,
                        connector.DataItem.Top + DesignerItemViewModelBase.ItemHeight / 2);
                    break;
            }

            return point;
        }
    }
}