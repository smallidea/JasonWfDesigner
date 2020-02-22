﻿// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Core
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：PartCreatedConnectionInfo.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Windows;
using JasonWfDesigner.Core.Controls;

namespace JasonWfDesigner.Core
{
    public class PartCreatedConnectionInfo : ConnectorInfoBase
    {
        public PartCreatedConnectionInfo(Point currentLocation) : base(ConnectorOrientation.None)
        {
            CurrentLocation = currentLocation;
        }

        public Point CurrentLocation { get; }
    }
}