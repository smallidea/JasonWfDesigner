#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2018-06-08 22:20
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnglogs.com
// ** Version：v 1.0
// ** Last Modified: 2018-06-08 23:05
// ** Desc： RunningnodeVo.cs
// ******************************************************************/

#endregion

using System;
using JasonWfDesigner.Core.ViewModels;
using JasonWfDesigner.WPF.ViewModels;

namespace JasonWfDesigner.WPF.Engine
{
    /// <summary>
    ///     运行中的滚筒
    /// </summary>
    public class RunningnodeVo : INPCBase
    {
        #region 私有成员
        private string _statusDesc;
        private DateTime? _inTime;
        private NodeDesignerItemViewModel _node;
        private bool _isBusy = false;
        #endregion

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDesc
        {
            get { return _statusDesc; }
            set
            {
                if (value != _statusDesc)
                {
                    _statusDesc = value;
                    NotifyChanged("StatusDesc");
                }
            }
        }

        /// <summary>
        /// 关联线体
        /// </summary>
        public NodeDesignerItemViewModel node
        {
            get { return _node; }
            set
            {
                if (value != _node)
                {
                    _node = value;
                    NotifyChanged("node");
                    NotifyChanged("nodeKey");
                }
            }
        }

        /// <summary>
        /// 线体编号
        /// </summary>
        public string nodeKey
        {
            get { return node.Key; }
        }

        /// <summary>
        ///     进入滚筒的时间
        /// </summary>
        public DateTime? InTime
        {
            get { return _inTime; }
            private set
            {
                lock (_lock)
                {
                    if (value != _inTime)
                    {
                        _inTime = value;
                        NotifyChanged("InTime");
                        NotifyChanged("StayMilliseconds");
                    }
                }
            }
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled => node.IsEnabled;

        /// <summary>
        /// 是否忙碌
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    NotifyChanged("IsBusy");
                }
            }
        }

        /// <summary>
        ///     当前板在当前滚筒的累计停留时间
        /// </summary>
        public int StayMilliseconds
        {
            get
            {
                lock (_lock)
                {
                    if (_inTime != null)
                        return (int)(DateTime.Now - _inTime.Value).TotalMilliseconds;
                    return 0;
                }
            }
        }

        private readonly object _lock = new object();



        public RunningnodeVo()
        {

        }
    }
}