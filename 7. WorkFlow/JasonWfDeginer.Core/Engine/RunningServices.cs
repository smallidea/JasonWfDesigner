#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：Ningji.PlcSimulator.WPF
// ** Create Date：2018-06-07 11:03
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnglogs.com
// ** Version：v 1.0
// ** Last Modified: 2018-06-08 23:05
// ** Desc： RunningServices.cs
// ******************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ningji.PlcSimulator.Core;
using Ningji.PlcSimulator.Core.ViewModels;
using Ningji.PlcSimulator.WPF.ViewModels;

namespace Ningji.PlcSimulator.WPF.Engine
{
    public class RunningServices
    {
        private static DiagramViewModel _diagramViewModel;

        //private LockList<RunningProductVo> _runningProducts = new LockList<RunningProductVo>();
        private List<RollerDesignerItemViewModel> _beginRollers = new List<RollerDesignerItemViewModel>();

        private List<ConnectorViewModel> _connectors = new List<ConnectorViewModel>();
        private List<RollerDesignerItemViewModel> _exitRollers = new List<RollerDesignerItemViewModel>();
        private int _maxStayMilliseconds = 5000;
        private List<RollerDesignerItemViewModel> _roller = new List<RollerDesignerItemViewModel>();
        private List<RunningProductVo> _runningProducts = new List<RunningProductVo>();
        private List<RunningRollerVo> _runningRollers = new List<RunningRollerVo>();

        private List<object> testDatas = new List<object>
        {
            new {Id = 1, UPI = "123456789", Status = 110},
            new {Id = 2, UPI = "123456789", Status = 110},
            new {Id = 3, UPI = "123456789", Status = 110},
            new {Id = 4, UPI = "123456789", Status = 110},
            new {Id = 5, UPI = "123456789", Status = 110}
        };

        public RunningServices(DiagramViewModel diagramViewModel)
        {
            init(diagramViewModel);
            Task.Run(() => { roll(); });
        }

        //TODO:大小板速度不一致
        //TODO:逻辑上不允许先走

        private void init(DiagramViewModel diagramViewModel)
        {
            if (diagramViewModel == null) return;
            _diagramViewModel = diagramViewModel;
            _connectors = diagramViewModel.Items.OfType<ConnectorViewModel>().ToList();
            _roller = diagramViewModel.Items.OfType<RollerDesignerItemViewModel>().ToList();
            foreach (var rollerDesignerItemViewModel in _roller)
            {
                var runningRoller = new RunningRollerVo { Roller = rollerDesignerItemViewModel };
                runningRoller.In += runningRollerOnIn;
                runningRoller.Out += RunningRoller_Out;
                _runningRollers.Add(runningRoller);
            }

            var sinkDataItemIdArray = _connectors
                .Where(a => a.SinkConnectorInfo is FullyCreatedConnectorInfo)
                .Select(a =>
                    (((FullyCreatedConnectorInfo)a.SinkConnectorInfo).DataItem as RollerDesignerItemViewModel)?.Key)
                .ToList();
            _beginRollers = diagramViewModel.Items
                .OfType<RollerDesignerItemViewModel>()
                .Where(a => false == sinkDataItemIdArray.Contains(a.Key)).ToList();

            var sourceDataItemIdArray = _connectors
                .Where(a => a.SourceConnectorInfo != null)
                .Select(a => (a.SourceConnectorInfo.DataItem as RollerDesignerItemViewModel)?.Key)
                .ToList();
            _exitRollers = diagramViewModel.Items
                .OfType<RollerDesignerItemViewModel>()
                .Where(a => false == sourceDataItemIdArray.Contains(a.Key)).ToList();
        }

        /// <summary>
        /// 离开滚筒
        /// </summary>
        /// <param name="runningProduct"></param>
        private void RunningRoller_Out(RunningProductVo runningProduct)
        {
            var currentRoller = _runningRollers.FirstOrDefault(a => a.RollerKey == runningProduct.CurrentRollerKey)?.Roller;
            if (currentRoller == null) return;

            string targetNode = currentRoller.InTrigger?.GetTargetNode(runningProduct);
            if (false == string.IsNullOrWhiteSpace(targetNode))
            {
                runningProduct.TargetNode = targetNode;
            }
        }

        /// <summary>
        /// 进入滚筒
        /// </summary>
        /// <param name="runningProduct"></param>
        private void runningRollerOnIn(RunningProductVo runningProduct)
        {
            var currentRoller = _runningRollers.FirstOrDefault(a => a.RollerKey == runningProduct.CurrentRollerKey)?.Roller;
            //communcation(roller, runningProduct);

            if (currentRoller == null) return;

            string targetNode = currentRoller.OutTrigger?.GetTargetNode(runningProduct);
            if (false == string.IsNullOrWhiteSpace(targetNode))
            {
                runningProduct.TargetNode = targetNode;
            }
        }

         

        /// <summary>
        /// 整体开始流动
        /// </summary>
        private void roll()
        {
            while (true)
            {
                // 查找入口是否有板
                var runningRollerKeys = _runningRollers.Where(a => a.HasProduct).Select(a => a.Roller.Key);
                var freeBeginRollers = _beginRollers.Where(a => false == runningRollerKeys.Contains(a.Key));
                foreach (var item in freeBeginRollers)
                {
                    start(item);
                }

                // 查找到时间的板件
                var timeOutRoller = from a in _runningRollers
                                    where a.StayMilliseconds > _maxStayMilliseconds && a.IsEnabled // 超时 && 滚筒可用
                                    select a;
                foreach (var item in timeOutRoller)
                {
                    running(item);
                }
            }
        }

        /// <summary>
        /// 开始入板
        /// </summary>
        /// <param name="currentRoller"></param>
        private void start(RollerDesignerItemViewModel currentRoller)
        {
            var product = getReadyProduct();
            if (product == null) return;
            RunningProductVo runningProduct = new RunningProductVo { Product = product };
            _runningProducts.Add(runningProduct);
            var beginRoller = _runningRollers.FirstOrDefault(a => a.RollerKey == currentRoller.Key);
            if (beginRoller == null) throw new Exception($"起点（{currentRoller.Key}）不在运行时中");
            beginRoller.BandProduct = runningProduct;
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="curRollerVo"></param>
        private void running(RunningRollerVo curRollerVo)
        {
            if (curRollerVo == null) return;

            // 查找下一节滚筒
            string targetRollerKey = curRollerVo.BandProduct.TargetNode;
            var nextRoller = getNextRoller(curRollerVo.Roller, targetRollerKey);
            if (nextRoller == null)
            {
                // 结束
                if (_exitRollers.Exists(a => a.Key == curRollerVo.RollerKey))
                {
                    curRollerVo.BandProduct = null;
                    _runningProducts.Remove(curRollerVo.BandProduct);
                    //TODO: 此时是否应该记录日志
                    return;
                }
                throw new Exception($"（{curRollerVo.RollerKey}）下一节为空");
            }
            var nextRunningRoller = _runningRollers.FirstOrDefault(a => a.RollerKey == nextRoller.Key);
            if (nextRunningRoller == null) throw new Exception($"下一节（{nextRoller.Key}）不在运行时中");
            var hasProduct = _runningRollers.Any(a => a.RollerKey == nextRoller.Key && a.HasProduct);
            if (hasProduct) return; //TODO: 报警
            if (nextRoller.IsEnabled == false) return; //下一节滚筒禁用时不能流转 //TODO: 给出日志

            nextRunningRoller.BandProduct = curRollerVo.BandProduct;
            curRollerVo.BandProduct = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentRollerKey"></param>
        /// <param name="targetRollerKey"></param>
        /// <returns></returns>
        private RollerDesignerItemViewModel getNextRollerByTargetRollerKey(string currentRollerKey, string targetRollerKey)
        {
            if (string.IsNullOrWhiteSpace(targetRollerKey))
            {
                var sourceConnector =
                    _connectors.FirstOrDefault(a => a.SourceConnectorInfo.DataItem != null
                                                    && (a.SourceConnectorInfo.DataItem as RollerDesignerItemViewModel)?.Key == currentRollerKey
                                                    && a.SinkConnectorInfo is FullyCreatedConnectorInfo);
                if (sourceConnector == null) return null;
                return (sourceConnector.SinkConnectorInfo as FullyCreatedConnectorInfo)?.DataItem as
                    RollerDesignerItemViewModel;
            }

            foreach (var connectorViewModel in _connectors)
            {
                var fullyCreatedConnectorInfo =
                    connectorViewModel?.SinkConnectorInfo as FullyCreatedConnectorInfo;
                if (!(fullyCreatedConnectorInfo?.DataItem is RollerDesignerItemViewModel)) continue;
                var rollerDesignerItem = fullyCreatedConnectorInfo.DataItem as RollerDesignerItemViewModel;
                if (rollerDesignerItem.Key != targetRollerKey) continue;
                if (!(connectorViewModel.SourceConnectorInfo.DataItem is RollerDesignerItemViewModel)) continue;

                var sourceKey =
                    ((RollerDesignerItemViewModel)connectorViewModel.SourceConnectorInfo.DataItem)
                    .Key;
                if (sourceKey == currentRollerKey)
                    return rollerDesignerItem;
                else
                    return getNextRollerByTargetRollerKey(currentRollerKey, sourceKey);
            }
            return null;
        }

        /// <summary>
        ///     获取路径规划的下一节滚筒
        /// </summary>
        /// <param name="currentRoller"></param>
        /// <returns></returns>
        private RollerDesignerItemViewModel getNextRoller(RollerDesignerItemViewModel currentRoller, string targetRollerKey)
        {
            return getNextRollerByTargetRollerKey(currentRoller.Key, targetRollerKey);

            /*   // 目标滚筒不为空
               if (false == string.IsNullOrWhiteSpace(targetRollerKey))
               {
                   return getNextRollerByTargetRollerKey(currentRoller.Key, targetRollerKey);
               }

               // 
               var sourceConnector =
                   _connectors.FirstOrDefault(a => a.SourceConnectorInfo.DataItem != null
                                                   && (a.SourceConnectorInfo.DataItem as RollerDesignerItemViewModel)
                                                   .Key == currentRoller.Key
                                                   && a.SinkConnectorInfo is FullyCreatedConnectorInfo);
               if (sourceConnector == null) return null;
               return (sourceConnector.SinkConnectorInfo as FullyCreatedConnectorInfo)?.DataItem as
                   RollerDesignerItemViewModel;*/
        }

        /// <summary>
        ///     获取准备好的板件
        /// </summary>
        /// <returns></returns>
        private object getReadyProduct()
        {
            var item = testDatas.FirstOrDefault();
            testDatas.Remove(item);
            return item;
        }
    }
    
}