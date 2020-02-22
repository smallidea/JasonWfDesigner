#region Header Text

// /******************************************************************
// ** Copyright：广州宁基智能系统有限公司 Copyright (c) 2017
// ** Project：JasonWfDesigner.WPF
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using JasonWfDesigner.Common;
using JasonWfDesigner.Core;
using JasonWfDesigner.Core.ViewModels;
using JasonWfDesigner.WPF.ViewModels;
using NJIS.AppUtility.Collection;
using NJIS.AppUtility.LogHelper;
using NJIS.AppUtility.NjThreadHelper;
using Sogal.Untity;

namespace JasonWfDesigner.WPF.Engine
{
    /// <summary>
    /// 运行时
    /// <remarks>模拟跑板</remarks>
    /// </summary>
    public class RunningServices
    {
        private static DiagramViewModel _diagramViewModel;

        //private LockList<RunningProductVo> _runningProducts = new LockList<RunningProductVo>();
        private List<RollerDesignerItemViewModel> _beginRollers = new List<RollerDesignerItemViewModel>();
        private List<RollerDesignerItemViewModel> _exitRollers = new List<RollerDesignerItemViewModel>();
        private List<ConnectorViewModel> _connectors = new List<ConnectorViewModel>();
        private int _maxStayMilliseconds = 5000;
        private List<RollerDesignerItemViewModel> _roller = new List<RollerDesignerItemViewModel>();
        private MtObservableCollection<RunningProductVo> _runningProducts = new MtObservableCollection<RunningProductVo>();
        private object _lockRunningProducts = new object();
        private ObservableCollection<RunningRollerVo> _runningRollers = new ObservableCollection<RunningRollerVo>();
        //private List<Panel> _panels = new List<Panel>();
        //private List<string[]> _routePaths = new List<string[]>();
        private List<string[]> _routes = new List<string[]>();

        public ObservableCollection<RunningRollerVo> RunningRollers
        {
            get { return _runningRollers; }
        }

        public ObservableCollection<RunningProductVo> RunningProducts
        {
            get { return _runningProducts; }
        }

        public RunningServices(DiagramViewModel diagramViewModel)
        {
            init(diagramViewModel);
            NjThreadManager.Instance.Loop("roll", roll, 200);
        }

        //TODO:大小板速度不一致
        private void init(DiagramViewModel diagramViewModel)
        {
            // 
            if (diagramViewModel == null) return;
            _diagramViewModel = diagramViewModel;
            _connectors = diagramViewModel.Items.OfType<ConnectorViewModel>().ToList();
            _roller = diagramViewModel.Items.OfType<RollerDesignerItemViewModel>().ToList();
            foreach (var rollerDesignerItemViewModel in _roller)
            {
                var runningRoller = new RunningRollerVo { Roller = rollerDesignerItemViewModel };
                /*runningRoller.In += runningRollerOnIn;
                                runningRoller.Out += runningRollerOnOut;
                                runningRoller.BeginOut += runningRollerOnBeginOut;
                                */
                _runningRollers.Add(runningRoller);
            }

            // 路由信息
            _routes = _connectors
                .Where(a => a.SinkConnectorInfo is FullyCreatedConnectorInfo)
                .Where(a => a.SourceConnectorInfo != null)
                .Where(a => a.SinkConnectorInfo != null)
                .Select(a => new string[2]
                {
                    (a.SourceConnectorInfo.DataItem as RollerDesignerItemViewModel)?.Key,
                    (((FullyCreatedConnectorInfo)a.SinkConnectorInfo).DataItem as RollerDesignerItemViewModel)?.Key
                }).ToList();

            // 开始节点
            var sinkDataItemIdArray = _connectors
                .Where(a => a.SinkConnectorInfo is FullyCreatedConnectorInfo)
                .Select(a =>
                    (((FullyCreatedConnectorInfo)a.SinkConnectorInfo).DataItem as RollerDesignerItemViewModel)?.Key)
                .ToList();
            _beginRollers = diagramViewModel.Items
                .OfType<RollerDesignerItemViewModel>()
                .Where(a => false == sinkDataItemIdArray.Contains(a.Key)).ToList();

            // 结束节点
            var sourceDataItemIdArray = _connectors
                .Where(a => a.SourceConnectorInfo != null)
                .Select(a => (a.SourceConnectorInfo.DataItem as RollerDesignerItemViewModel)?.Key)
                .ToList();
            _exitRollers = diagramViewModel.Items
                .OfType<RollerDesignerItemViewModel>()
                .Where(a => false == sourceDataItemIdArray.Contains(a.Key)).ToList();

            // 加载缓存的板件列表
          //  loadRunningPanels4Roller(diagramViewModel.Key);
        }

        private static readonly string _cacheFileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "cache");

        /// <summary>
        /// 加载上次关闭窗体缓存的板件列表
        /// </summary>
        /// <param name="diagramId"></param>
        private void loadRunningPanels4RollerWithCache(string diagramId)
        {
            var filePath = Path.Combine(_cacheFileDirectory, diagramId + ".json");
            if (File.Exists(filePath) == false) return;
            List<JsonConverter> converters = new List<JsonConverter> { new PanelConverter() };
            var result = JsonHelper.GetJsonObjectFromFile<Dictionary<string, List<RunningProductVo>>>(filePath, converters);
            if (result == null || result.Any() == false) return;
            var items = result.Where(a => a.Value != null);
            foreach (var keyValuePair in items)
            {
                var roller = _runningRollers.FirstOrDefault(a => a.RollerKey == keyValuePair.Key);
                if (roller == null)
                {
                    NjEventLog.Instance.WriteWarning($"{keyValuePair.Key} 对应数据为空！");
                    continue;
                }

                roller.BandProduct = keyValuePair.Value.FirstOrDefault();
            }
        }

        internal class PanelConverter : JsonConverter
        {
            /// <summary>
            /// 重载序列化方法
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="value"></param>
            /// <param name="serializer"></param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue((Panel)value);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="objectType"></param>
            /// <param name="existingValue"></param>
            /// <param name="serializer"></param>
            /// <returns></returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;

                /*   Panel panel = null;
                   if (reader.TokenType == JsonToken.StartArray)
                   {
                       JToken token = JToken.Load(reader);
                       List<string> items = token.ToObject<List<string>>();
                       panel = serializer.Deserialize<Panel>(reader);
                       myCustomType = new MyCustomType(items);
                   }*/
                var panel = serializer.Deserialize<Panel>(reader);
                return panel;
            }

            /// <summary>
            /// 是否可以重写
            /// </summary>
            /// <param name="objectType"></param>
            /// <returns></returns>
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ProductBase);
            }
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="beginNodeKey"></param>
        /// <param name="endNodeKey"></param>
        /// <returns></returns>
        private string[] findPath(string beginNodeKey, string endNodeKey)
        {
            List<string> rollerKeys = new List<string>();
            rollerKeys.Add(beginNodeKey);
            var sinks = _routes.Where(a => a[0] == beginNodeKey).Select(a => a[1]);

            var enumerable = sinks as string[] ?? sinks.ToArray();
            if (enumerable.Any() == false) return null;
            if (enumerable.Length == 1)
            {
                var sink = enumerable.First();
                if (endNodeKey.Equals(sink, StringComparison.CurrentCultureIgnoreCase))
                {
                    return new[] { beginNodeKey, endNodeKey };
                }

                var path = findPath(sink, endNodeKey);
                if (path != null)
                    return rollerKeys.Concat(path).ToArray();
            }
            else
            {
                var paths = new List<string[]>();
                foreach (var sink in enumerable)
                {
                    if (sink == endNodeKey)
                        paths.Add(new[] { beginNodeKey, endNodeKey });
                    else
                    {
                        if (_exitRollers.Any(a => a.Key.Equals(sink, StringComparison.CurrentCultureIgnoreCase))
                      && sink.Equals(endNodeKey, StringComparison.CurrentCultureIgnoreCase) == false)
                            continue;
                        var path = findPath(sink, endNodeKey);
                        if (path != null)
                            paths.Add(path);
                    }
                }

                if (paths.Any())
                {
                    var single = paths.OrderBy(a => a.Length).FirstOrDefault(); // 选择最短路径
                    if (single != null)
                    {
                        if (single[0] == beginNodeKey)
                            return single;
                        return rollerKeys.Concat(single).ToArray();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rollerKey"></param>
        /// <returns></returns>
        private Panel runningRollerOnFree(string rollerKey)
        {
            try
            {
                var beginRoller = _beginRollers.FirstOrDefault(a => a.Key == rollerKey);
                if (beginRoller == null)
                    throw new Exception($"{rollerKey} 不是初始节点");
                if (beginRoller.FreeTrigger == null) return null;

                var panel = new Panel(); //TODO: 应该直接使用ProductBase，而不应该出现Panel这个类
                if (beginRoller.FreeTrigger == null) return null;
                var communicationResult = beginRoller.FreeTrigger.GetTargetNode(panel, beginRoller.Key);
                if (communicationResult == null)
                    throw new Exception("通讯结果为空！");
                return string.IsNullOrWhiteSpace(panel.Upi) && panel.Id <= 0 ? null : panel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                NjEventLog.Instance.WriteError(ex);
            }
            return null;
        }

        /// <summary>
        /// 整体开始流动
        /// </summary>
        private void roll()
        {
            // 查找入口是否有板
            var beginRollerKeyArray = _beginRollers.Select(a => a.Key);
            var freeBeginRollers = _runningRollers.Where(a => beginRollerKeyArray.Contains(a.RollerKey) && a.IsBusy == false 
                                                                              && a.HasProduct == false && a.IsEnabled);
            foreach (var beginRoller in freeBeginRollers)
            {
                beginRoller.IsBusy = true;
                Task.Run(() =>
                {
                    try
                    {
                        beginRoller.StatusDesc = "等待上板";
                        var product = runningRollerOnFree(beginRoller.RollerKey);
                        if (product != null)
                        {
                            start(beginRoller.Roller, product);
                            beginRoller.StatusDesc = $"({product.Key}) 入口等待上板";
                        }
                        else
                            beginRoller.StatusDesc = "没有获取到可用板件";
                    }
                    catch (Exception ex)
                    {
                     NjEventLog.Instance.WriteError(ex);
                    }
                    finally
                    {
                        Thread.Sleep(100);
                        beginRoller.IsBusy = false;
                    }
                });
            }

            // 查找到时间的板件
            var timeOutRoller = from a in _runningRollers
                                where a.StayMilliseconds > _maxStayMilliseconds
                                    && a.IsEnabled
                                    && false == a.IsBusy && a.HasProduct // 超时 && 滚筒可用
                                select a;
            foreach (var item in timeOutRoller)
            {
                item.IsBusy = true;

                Task.Run(() =>
                {
                    try
                    {
                        // 如果当前滚筒已经是出口，就设定目标为null
                        if (_exitRollers.Exists(a => a.Key == item.RollerKey))
                            item.BandProduct.TargetNode = null;

                        // 运行
                        item.StatusDesc = "开始运行";
                        running3(item);

                        // 滚筒上的板件
                        var tmpList = _runningRollers.Where(a => a.HasProduct)
                                                                        .ToDictionary(k => k.RollerKey, v => v.BandProduct?.Id ?? 0);
                        PlcLoopDrive.SetProductPosition(tmpList);
                    }
                    catch (Exception ex)
                    {
                        // 异常时板件停止 
                        if (item.BandProduct != null)
                        {
                            item.BandProduct.IsPause = true;
                            item.StatusDesc = $"因错误“{ex.Message}”暂停";
                        }

                        NjEventLog.Instance.WriteError(ex);
                    }
                    finally
                    {
                        Thread.Sleep(100);
                        item.IsBusy = false;
                    }
                });
            }

            //TODO: 更新板件与滚筒的对应关系
            /* JasonWfDesigner.BoardSorting.Communication.Global.RollerWithProductIdDictionary 
                 = _runningRollers.ToDictionary(k => k.RollerKey, v => v.HasProduct ? (int?) v.BandProduct.Id : null);*/

        }

        /// <summary>
        /// 开始入板
        /// </summary>
        /// <param name="currentRoller"></param>
        /// <param name="product"></param>
        private void start(RollerDesignerItemViewModel currentRoller, ProductBase product)
        {
            if (product == null) return;

            RunningProductVo runningProduct = new RunningProductVo { Product = product };
            lock (_lockRunningProducts)
                _runningProducts.Add(runningProduct);

            var beginRoller = _runningRollers.FirstOrDefault(a => a.RollerKey == currentRoller.Key);
            if (beginRoller == null)
                throw new Exception($"起点（{currentRoller.Key}）不在运行时中");
            beginRoller.BandProduct = runningProduct;

            // 获取默认路径
            runningProduct.TargetNode = getAutoTargetNode(currentRoller.Key);
            if (string.IsNullOrWhiteSpace(runningProduct.TargetNode))
                throw new Exception($"{currentRoller.Key}目标路径路由信息为空！");
            runningProduct.TargetPath = findPath(runningProduct.CurrentRollerKey, runningProduct.TargetNode);
        }

        /// <summary>
        /// 获取默认的下一个
        /// </summary>
        /// <param name="currentRollerKey"></param>
        /// <returns></returns>
        private string getNextAutoNode(string currentRollerKey)
        {
            var sourceConnectors =
                _connectors.Where(a => a.SourceConnectorInfo.DataItem != null
                                                && (a.SourceConnectorInfo.DataItem as RollerDesignerItemViewModel)?.Key == currentRollerKey
                                                && a.SinkConnectorInfo is FullyCreatedConnectorInfo)
                                    .ToList();
            if (sourceConnectors.Count() > 1)
            {
                NjEventLog.Instance.WriteInfo($"板件在{currentRollerKey}上 对应多条路径，无法自动规划路线，请等待通讯完成分配路径！");
                return null;
            }
            return ((sourceConnectors.FirstOrDefault()?.SinkConnectorInfo as FullyCreatedConnectorInfo)
                       ?.DataItem as RollerDesignerItemViewModel)
                       ?.Key;
        }

        /// <summary>
        /// 获取默认的目标路径
        /// </summary>
        /// <param name="currentRollerKey"></param>
        /// <returns></returns>
        private string getAutoTargetNode(string currentRollerKey)
        {
            var targetNode = getNextAutoNode(currentRollerKey);
            if (string.IsNullOrWhiteSpace(targetNode))
                return currentRollerKey;
            else return getAutoTargetNode(targetNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curRollerVo"></param>
        private void running3(RunningRollerVo curRollerVo)
        {
            if (curRollerVo == null)
                throw new Exception("参数 curRollerVo == null");
            if (curRollerVo.BandProduct == null)
                throw new Exception("参数 curRollerVo.BandProduct == null");

            // 
            var currentRollerViewModel = curRollerVo.Roller;
            var currentBandingProduct = curRollerVo.BandProduct;

            // 准备离开
            if (currentRollerViewModel?.BeginOutTrigger != null)
            {
                curRollerVo.StatusDesc = $"等待{curRollerVo.BandProduct.Id}【准备离开】通讯";
                var result =
                    currentRollerViewModel.BeginOutTrigger.GetTargetNode(currentBandingProduct.Product,
                        curRollerVo.RollerKey);
                if (result == null)
                    throw new Exception("返回值为空");
                curRollerVo.StatusDesc = $"完成{curRollerVo.BandProduct.Id}【准备离开】通讯";

                running2_sub(result, currentBandingProduct);
            }

            // 进入下一节滚筒判定
            if (currentBandingProduct.IsPause) return; // 暂停
            if (currentBandingProduct.CurrentRollerKey == currentBandingProduct.NextNode) // 暂停
                return;

            // 结束（当前的滚筒为结束）
            if (isExit(currentBandingProduct.CurrentRollerKey))
            {
                if (currentBandingProduct?.NextNode != null)
                    throw new Exception(
                        $"板件（{currentBandingProduct.Product.ToString()}）， 已经到达了出口{currentBandingProduct.CurrentRollerKey}，但是下节滚筒为{currentBandingProduct.NextNode}");

                // 离开
                if (currentRollerViewModel?.OutTrigger != null)
                {
                    curRollerVo.StatusDesc = $"等待{curRollerVo.BandProduct.Id}【离开】通讯";
                    var result =
                        currentRollerViewModel.OutTrigger.GetTargetNode(currentBandingProduct.Product,
                            curRollerVo.RollerKey);
                    if (result == null)
                        throw new Exception($"{curRollerVo.BandProduct.Id}【离开】通讯返回结果为空");
                    curRollerVo.StatusDesc = $"完成{curRollerVo.BandProduct.Id}【离开】通讯";
                }

                // 离开动作
                curRollerVo.BandProduct = null;
                lock (_lockRunningProducts)
                {
                    if (_runningProducts.Contains(currentBandingProduct) == false)
                        throw new Exception($"{currentBandingProduct.ToString()} 剔除失败");
                    _runningProducts.Remove(currentBandingProduct);
                }
                return;
            }
            if (currentBandingProduct.NextNode == null) // 再次验证
                throw new Exception($"板件（{currentBandingProduct.Product.ToString()}）," +
                                    $"当前滚筒（{currentBandingProduct.CurrentRollerKey}），下节滚筒为空！");

            // 下节滚筒
            var nextRolloerVo = _runningRollers.FirstOrDefault(a => a.RollerKey == currentBandingProduct.NextNode);
            if (nextRolloerVo == null)
                throw new Exception($"{curRollerVo.RollerKey} 对应滚筒数据为空！");
            if (nextRolloerVo.HasProduct) // 下一节滚筒有板
                return;
            var nextRollerViewModel = nextRolloerVo.Roller;
            if (currentBandingProduct.IsPause) return;

            lock (nextRollerViewModel)
            {
                if (nextRolloerVo.HasProduct) // 下一节滚筒有板
                    return;

                // 触发“离开”监听 
                if (currentRollerViewModel?.OutTrigger != null)
                {
                    curRollerVo.StatusDesc = $"等待{curRollerVo.BandProduct.Id}【离开】通讯";
                    var result =
                        currentRollerViewModel.OutTrigger.GetTargetNode(currentBandingProduct.Product,
                            curRollerVo.RollerKey);
                    if (result == null)
                        throw new Exception($"{curRollerVo.BandProduct.Id}【离开】通讯返回结果为空");
                    curRollerVo.StatusDesc = $"完成{curRollerVo.BandProduct.Id}【离开】通讯";

                    running2_sub(result, currentBandingProduct);
                }
                Thread.Sleep(500); // 【注意】增加一个延迟，缓解plc轮询数据过快，实际上板件在顶升下降和板件离开某个滚筒都需要一定的时间

                // 流动
                curRollerVo.BandProduct = null; // 离开动作
                nextRolloerVo.BandProduct = currentBandingProduct; // 进入动作
                RunningAccessService.SaveLog(nextRolloerVo.RollerKey, currentBandingProduct.Id, DateTime.Now); // 记录历史

                // 开始进入下节滚筒，触发“入板”监听 
                if (nextRollerViewModel.InTrigger != null)
                {
                    nextRolloerVo.StatusDesc = $"等待{nextRolloerVo.BandProduct?.Id} 【进入】通讯";
                    var result =
                        nextRollerViewModel.InTrigger.GetTargetNode(currentBandingProduct.Product, nextRolloerVo.RollerKey);
                    if (result == null)
                        throw new Exception("返回值为空");
                    nextRolloerVo.StatusDesc = $"完成{nextRolloerVo.BandProduct?.Id} 【进入】通讯";

                    running2_sub(result, currentBandingProduct);
                }
            }
        }

        //  private readonly object _lock = new object();

        // 当前节点是否为出口
        private bool isExit(string rollerKey)
        {
            return _exitRollers.Exists(a => a.Key == rollerKey);
        }

        private void running2_sub(NodeCommunicationResult result, RunningProductVo runningProduct)
        {
            if (result.IsAllowGo == false)
            {
                runningProduct.IsPause = true;
                return;
            }
            runningProduct.IsPause = false;

            // 没有返回一个目标时
            if (string.IsNullOrWhiteSpace(runningProduct.TargetNode))
            {
                if (string.IsNullOrWhiteSpace(runningProduct.NextNode))
                {
                    var nextRollerKey = getAutoTargetNode(runningProduct.CurrentRollerKey);
                    if (false == string.IsNullOrWhiteSpace(nextRollerKey))
                    {
                        runningProduct.TargetNode = nextRollerKey;
                        runningProduct.TargetPath =
                            findPath(runningProduct.CurrentRollerKey, runningProduct.TargetNode);
                    }
                }
            }

            // 
            if (false == string.IsNullOrWhiteSpace(result.TargetNode)
                && result.TargetNode != runningProduct.TargetNode)
            {
                runningProduct.TargetNode = result.TargetNode;
                var path = findPath(runningProduct.CurrentRollerKey, result.TargetNode);
                if (path == null || path.Length <= 0)
                    throw new Exception($"currentNode（{runningProduct.CurrentRollerKey}）to targetNode（{result.TargetNode}）路径查找为空！");
                runningProduct.TargetPath = path;
            }

            // 是否NG
            if (runningProduct.Product.StatusBase == ProductBase.StatusEnum.NG1 ||
                runningProduct.Product.StatusBase == ProductBase.StatusEnum.NG2)
                runningProduct.IsNg = true;
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

            var path = findPath(currentRollerKey, targetRollerKey);
            if (path != null && path.Length >= 2)
            {
                var next = path[1];
                return _roller.FirstOrDefault(a => a.Key == next);
            }

            return null;
        }

        /// <summary>
        ///     获取路径规划的下一节滚筒
        /// </summary>
        /// <param name="currentRoller"></param>
        /// <param name="targetRollerKey"></param>
        /// <returns></returns>
        private RollerDesignerItemViewModel getNextRoller(RollerDesignerItemViewModel currentRoller, string targetRollerKey)
        {
            return getNextRollerByTargetRollerKey(currentRoller.Key, targetRollerKey);
        }

    }

}