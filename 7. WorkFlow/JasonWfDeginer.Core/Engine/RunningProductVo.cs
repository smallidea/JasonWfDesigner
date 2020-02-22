#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：Ningji.PlcSimulator.WPF
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

namespace Ningji.PlcSimulator.WPF.Engine
{
    /// <summary>
    ///     在线体上的板件
    /// </summary>
    public class RunningProductVo
    {
        private string _currentRollerKey;

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

        public object Product { get; set; }

        public Dictionary<string, DateTime> RollerLogs { get; private set; }
            = new Dictionary<string, DateTime>();

        /// <summary>
        /// 
        /// </summary>
        public string TargetNode { get; set; }

        public override string ToString()
        {
            return Product.ToString();
        }
    }
}