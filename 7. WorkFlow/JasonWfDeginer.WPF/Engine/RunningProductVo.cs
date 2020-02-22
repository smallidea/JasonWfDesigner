#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2018-06-08 22:20
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnglogs.com
// ** Version：v 1.0
// ** Last Modified: 2018-06-08 23:05
// ** Desc： RunningProductVo.cs
// ******************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonWfDesigner.Common;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.WPF.Engine
{
    /// <summary>
    ///     在线体上的板件
    /// </summary>
    public class RunningProductVo : INPCBase
    {
        private string _currentRollerKey;
        private static readonly object _lock = new object();
        private ProductBase _product;
        private bool _isNg = false;
        private string _targetNode;
        private string[] _targetPath;
        private bool _isPause = false;
        private string _lastNode;

        public RunningProductVo()
        {
        }

        /// <summary>
        /// 当前滚筒编号
        /// </summary>
        public string CurrentRollerKey
        {
            get { return _currentRollerKey; }
            set
            {
                if (_currentRollerKey != value)
                {
                    _currentRollerKey = value;
                    if (string.IsNullOrWhiteSpace(_currentRollerKey) == false)
                        RollerLogs.Add(_currentRollerKey, DateTime.Now);
                }
            }
        }

        public int Id => Product?.Id ?? -1;

        public string Key => Product?.Key;


        /// <summary>
        /// 业务对象，用于扩展
        /// </summary>
        public ProductBase Product
        {
            get { return _product; }
            set
            {
                lock (_lock)
                {
                    _product = value;
                }
            }
        }

        /// <summary>
        /// 是否NG
        /// </summary>
        public bool IsNg
        {
            get
            {
                return _isNg;
            }
            set
            {
                if (Product.StatusBase == ProductBase.StatusEnum.NG1 && TargetNode != "NG1")
                    throw new Exception("TargetNode 应为 NG1");

                if (value != _isNg)
                {
                    _isNg = value;
                    NotifyChanged("IsNg");
                }
            }
        }

        /// <summary>
        /// 已走过路径历史
        /// </summary>
        public Dictionary<string, DateTime> RollerLogs { get; private set; }
            = new Dictionary<string, DateTime>();

        /// <summary>
        /// 
        /// </summary>
        public string RollerLogsText
        {
            get { return string.Join(", ", RollerLogs.OrderBy(a => a.Value).Select(a => a.Key)); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TargetPathText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (TargetPath == null) return sb.ToString();

                for (int i = 0; i < TargetPath.Length; i++)
                {
                    var node = TargetPath[i];
                    if (node == CurrentRollerKey)
                        sb.Append(node + "(当前)");
                    else
                        sb.Append(node);

                    if (i + 1 < TargetPath.Length)
                        sb.Append(",");
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 下一节滚筒编号
        /// </summary>
        public string NextNode
        {
            get
            {
                if (TargetPath == null || TargetPath.Length <= 0) return null; // 路径不能为空

                if (IsPause) return CurrentRollerKey; // 暂停

                var currentIndex = TargetPath.ToList().IndexOf(CurrentRollerKey);
                if ((currentIndex + 1) == TargetPath.Length)
                    return null;

                var nextIndex = (currentIndex + 1) == TargetPath.Length ? currentIndex : currentIndex + 1;
                return TargetPath[nextIndex];
            }
        }

        /// <summary>
        /// 上一节滚筒编号
        /// </summary>
        public string LastNode
        {
            get { return _lastNode; }
            set
            {
                if (_lastNode != value)
                {
                    _lastNode = value;
                    NotifyChanged("LastNode");
                }
            }
        }

        /// <summary>
        /// 目标
        /// </summary>
        public string TargetNode
        {
            get { return _targetNode; }
            set
            {
                if (_targetNode != value)
                {
                    _targetNode = value;
                    NotifyChanged("TargetNode");
                }
            }
        }

        /// <summary>
        /// 目标路径
        /// </summary>
        public string[] TargetPath
        {
            get { return _targetPath; }
            set
            {
                if (value != null && _targetPath != value)
                {
                    _targetPath = value;
                    NotifyChanged("NextNode");
                    NotifyChanged("TargetPathText");
                    NotifyChanged("TargetPath");
                }
            }
        }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPause
        {
            get { return _isPause; }
            set
            {
                if (_isPause != value)
                {
                    _isPause = value;
                    NotifyChanged("IsPause");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Product.ToString();
        }
    }
}