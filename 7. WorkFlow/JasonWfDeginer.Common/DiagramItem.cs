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
// ** Desc：DiagramItem.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;

namespace JasonWfDesigner.Common
{
    public class DiagramItem : PersistableItemBase
    {
        public DiagramItem()
        {
            DesignerItems = new List<DiagramItemData>();
            ConnectionIds = new List<int>();
        }

        public List<DiagramItemData> DesignerItems { get; set; }
        public List<int> ConnectionIds { get; set; }

        /// <summary>
        ///     文件名
        /// </summary>
        public string FileName { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss.json");
    }


    public class DiagramItemData
    {
        public DiagramItemData(int itemId, Type itemType)
        {
            ItemId = itemId;
            ItemType = itemType;
        }

        public int ItemId { get; set; }
        public Type ItemType { get; set; }
    }
}