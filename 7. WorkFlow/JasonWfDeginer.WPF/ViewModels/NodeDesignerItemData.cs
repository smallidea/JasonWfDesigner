// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:55
// ** Desc：nodeDesignerItemData.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.WPF.ViewModels
{
    /// <summary>
    /// </summary>
    public class NodeDesignerItemData : INPCBase
    {
        private string _desc;

        // private bool _isEnabled = true;
        private string _key;
        private string _text;
        private string _nodeType = "普通";
        private string _businessObj;
        private int _value;

        public NodeDesignerItemData(string codeNo, int statusValue, string[] businessObjCollection)
        {
            Key = codeNo;
            Value = statusValue;
            BusinessObjCollection = businessObjCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] BusinessObjCollection { get; }

        public Visibility HasBusinessObj => BusinessObjCollection != null && BusinessObjCollection.Any() ? Visibility.Visible : Visibility.Collapsed;

        public string[] NodeTypeCollection { get; } = new[] { "普通", "开始", "结束", "异常" };

        /// <summary>
        /// 节点类型
        /// </summary>
        public string NodeType
        {
            get => _nodeType;
            set
            {
                if (value != _nodeType)
                {
                    _nodeType = value;
                    NotifyChanged("NodeType");
                }
            }
        }

        /// <summary>
        /// </summary>
        public string Key
        {
            get => _key;
            set
            {
                if (value != _key)
                {
                    _key = value;
                    NotifyChanged("Key");
                }
            }
        }

        /// <summary>
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    NotifyChanged("StatusValue");
                }
            }
        }

        /// <summary>
        /// 当前业务对象
        /// </summary>
        public string BusinessObj
        {
            get => _businessObj;
            set
            {
                if (value != _businessObj)
                {
                    _businessObj = value;
                    NotifyChanged("BusinessObj");
                    NotifyChanged("Header");
                }
            }
        }

        /// <summary>
        /// </summary>
        public string Desc
        {
            get => _desc;
            set
            {
                if (value != _desc)
                {
                    _desc = value;
                    NotifyChanged("Desc");
                }
            }
        }

        /// <summary>
        ///     绑定的文本
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    NotifyChanged("Text");
                }
            }
        }



    }
}