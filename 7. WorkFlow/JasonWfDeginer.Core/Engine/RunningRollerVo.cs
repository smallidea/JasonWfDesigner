#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：Ningji.PlcSimulator.WPF
// ** Create Date：2018-06-08 22:20
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnglogs.com
// ** Version：v 1.0
// ** Last Modified: 2018-06-08 23:05
// ** Desc： RunningRollerVo.cs
// ******************************************************************/

#endregion

using System;
using Ningji.PlcSimulator.WPF.ViewModels;

namespace Ningji.PlcSimulator.WPF.Engine
{
    /// <summary>
    ///     运行中的滚筒
    /// </summary>
    public class RunningRollerVo
    {
        private RunningProductVo _bandProduct;

        public RollerDesignerItemViewModel Roller { get; set; }

        public string RollerKey
        {
            get { return Roller.Key; }
        }

        /// <summary>
        ///     进入滚筒的时间
        /// </summary>
        public DateTime? InTime { get; private set; }

        /// <summary>
        ///     有板
        /// </summary>
        public bool HasProduct => BandProduct != null;

        /// <summary>
        /// </summary>
        public bool IsEnabled => Roller.IsEnabled;

        /// <summary>
        ///     当前板在当前滚筒的累计停留时间
        /// </summary>
        public int StayMilliseconds
        {
            get
            {
                if (InTime != null)
                    return (int)(DateTime.Now - InTime.Value).TotalMilliseconds;
                return 0;
            }
        }

        /// <summary>
        ///     绑定的板件
        /// </summary>
        public RunningProductVo BandProduct
        {
            get { return _bandProduct; }
            set
            {
                if (_bandProduct != value)
                {
                    this.Out?.Invoke(_bandProduct); // 离开

                    _bandProduct = value;
                    if (value == null)
                    {
                        InTime = null;
                        Roller.Text = null;
                    }
                    else
                    {
                        InTime = DateTime.Now;
                        Roller.Text = _bandProduct.ToString();
                        _bandProduct.CurrentRollerKey = RollerKey;
                    }

                    this.In?.Invoke(_bandProduct); // 进入
                }
            }
        }

        public delegate void OnMoveHandler(RunningProductVo runningRoller);

        public event OnMoveHandler In;
        public event OnMoveHandler Out;


    }
}