// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Common
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：Connection.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;

namespace JasonWfDesigner.Common
{
    public class Connection : PersistableItemBase
    {
        public Connection(int id, int sourceId, Orientation sourceOrientation,
            Type sourceType, int sinkId, Orientation sinkOrientation, Type sinkType) : base(id)
        {
            SourceId = sourceId;
            SourceOrientation = sourceOrientation;
            SourceType = sourceType;
            SinkId = sinkId;
            SinkOrientation = sinkOrientation;
            SinkType = sinkType;
        }

        public int SourceId { get; }
        public Orientation SourceOrientation { get; }
        public Type SourceType { get; }
        public int SinkId { get; }
        public Orientation SinkOrientation { get; }
        public Type SinkType { get; }
    }
}