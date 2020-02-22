// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 16:43
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-22 10:43
// ** Desc：NodeDesignerItemViewModel.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Linq;
using System.Web.UI.WebControls.Expressions;
using System.Windows.Input;
using System.Windows.Media;
using JasonWfDesigner.Common;
using JasonWfDesigner.Core;
using JasonWfDesigner.Core.ViewModels;
using JasonWfDesigner.WPF.Services;
using JasonWfDesigner.WPF.Services.Contracts;

namespace JasonWfDesigner.WPF.ViewModels
{
    /// <summary>
    /// </summary>
    public class NodeDesignerItemViewModel : DesignerItemViewModelBase, ISupportDataChanges
    {
        private string _businessObj = "";
        private string _codeNo = "";
        private string _desc;
        private bool _isEnabled = true;
        private bool _isError;
        private string _key;
        private string _mark;
        private string _nodeType = "普通";
        private int _value;
        private IUIVisualizerService _visualiserService;

        public NodeDesignerItemViewModel(int id, IDiagramViewModel parent, double left, double top, string businessObj,
            string nodeType, string key,
            string desc)
            : base(id, parent, left, top)
        {
            BusinessObj = businessObj;
            NodeType = nodeType;
            Key = key;
            Desc = desc;
            init();
        }

        public NodeDesignerItemViewModel(NodeDesignerItem item, IDiagramViewModel parent) : base(item.Id, parent, item.Left, item.Top)
        {
            Value = item.Value;
            //Id = item.Id;
            BusinessObj = item.BusinessObj;

            NodeType = item.NodeType;
            Key = item.Key;
            Desc = item.Desc;

            init();
        }

        public NodeDesignerItemViewModel()
        {
            init();
        }

        private void init()
        {
            _visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;
            ShowDataChangeWindowCommand = new SimpleCommand(executeShowDataChangeWindowCommand);
            ShowConnectors = false;
        }

        #region Command

        /// <summary>
        ///     修改节点绑定数据
        /// </summary>
        public ICommand ShowDataChangeWindowCommand { get; private set; }

        private void executeShowDataChangeWindowCommand(object parameter)
        {
            var data = new NodeDesignerItemData(Key, Value,
                Parent.BusinessObjectCollection.Select(a => a.Name).ToArray())
            {
                BusinessObj = BusinessObj,
                Desc = Desc,
                NodeType = NodeType
            };

            if (_visualiserService.ShowDialog(data, Key) == true)
            {
                Key = data.Key;
                Mark = data.Desc;
                Value = data.Value;

                BusinessObj = data.BusinessObj;
                NodeType = data.NodeType;
                Desc = data.Desc;
            }

            // _visualiserService.ShowDialog(this, Key);
        }

        #endregion

        #region 属性 

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    NotifyChanged("IsEnabled");
                    NotifyChanged("BackgroupColor");
                }
            }
        }

        /// <summary>
        ///     背景色
        /// </summary>
        public Brush BackgroupColor
        {
            get
            {
                if (false == IsEnabled) return new SolidColorBrush(Colors.DimGray); // 禁用时
                if (_isError) return new SolidColorBrush(Colors.IndianRed); // 错误时
                //if (string.IsNullOrWhiteSpace(Text) == false) return new SolidColorBrush(Colors.PaleGreen);
                return new SolidColorBrush(Colors.CadetBlue);
            }
        }

        /// <summary>
        ///     边框颜色
        /// </summary>
        public Brush BroderColor
        {
            get
            {
                if (_isError) return new SolidColorBrush(Colors.Red); // 错误时
                return new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        ///     是否错误
        /// </summary>
        public bool IsError
        {
            get => _isError;
            set
            {
                if (value != _isError)
                {
                    _isError = value;
                    NotifyChanged("IsError");
                    NotifyChanged("BackgroupColor");
                    NotifyChanged("BroderColor");
                }
            }
        }

        /// <summary>
        /// </summary>
        public string Header =>
            $"{(string.IsNullOrWhiteSpace(_businessObj) ? "" : _businessObj+":")}::{_nodeType}.{_desc}({_value})"; 

        /// <summary>
        ///     编号
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
                    NotifyChanged("Header");
                }
            }
        }

        /// <summary>
        ///     编码
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    NotifyChanged("Value");
                    NotifyChanged("Header");
                }
            }
        }

        /// <summary>
        ///     备注
        /// </summary>
        public string Mark 
        {
            get => _mark;
            set
            {
                if (value != _mark)
                {
                    _mark = _mark == _key ? null : value;

                    NotifyChanged("Mark");
                    NotifyChanged("HasMark");
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
                    NotifyChanged("Header");
                }
            }
        }

        /// <summary>
        ///     节点类型
        /// </summary>
        public string NodeType
        {
            get => _nodeType;
            set
            {
                if (value != _nodeType)
                {
                    _nodeType = value;
                    IsError = _nodeType == "异常";
                    NotifyChanged("NodeType");
                    NotifyChanged("Header");
                }
            }
        }

        /// <summary>
        ///     当前业务对象
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
        public bool HasMark => string.IsNullOrWhiteSpace(_mark) == false;

      

        //TODO: 唯一性限制

        #endregion
    }
}