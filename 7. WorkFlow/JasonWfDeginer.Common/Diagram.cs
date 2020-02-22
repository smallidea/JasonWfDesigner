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
// ** Desc：Diagram.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;

namespace JasonWfDesigner.Common
{
    /// <summary>
    /// </summary>
    public class Diagram4Serialize
    {
        public Diagram4Serialize()
        {
            DiagramItems = new List<NodeDesignerItem>();
            Connections = new List<Connection>();
        }

        public List<NodeDesignerItem> DiagramItems { get; set; }
        public List<Connection> Connections { get; set; }
        public string[] BusinessOjbCollection{ get; set; }
        public int Id { get; set; }

        /// <summary>
        ///     文件名
        /// </summary>
        public string FileName { get; set; } = DateTime.Now.ToString("yyMMddHHmm") + ".json";
    }
}