// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.WPF
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 16:34
// ** Desc：WindowViewModel.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using JasonWfDesigner.Common;
using JasonWfDesigner.Common.Lib;
using JasonWfDesigner.Core;
using JasonWfDesigner.Core.Controls;
using JasonWfDesigner.Core.ViewModels;
using JasonWfDesigner.WPF.Services;
using JasonWfDesigner.WPF.Services.Contracts;

namespace JasonWfDesigner.WPF.ViewModels
{
    /// <summary>
    ///     主界面绑定对象
    /// </summary>
    public class MainWindowViewModel : INPCBase
    {
        #region ctor

        public MainWindowViewModel()
        {
            _messageBoxService = ApplicationServicesProvider.Instance.Provider.MessageBoxService;
            _databaseAccessService = ApplicationServicesProvider.Instance.Provider.DatabaseAccessService;
            _savedDiagrams = _databaseAccessService.FetchAllDiagram().ToList();

            ToolBoxViewModel = new ToolBoxViewModel();

            DeleteSelectedItemsCommand = new SimpleCommand(executeDeleteSelectedItemsCommand);
            CreateNewDiagramCommand = new SimpleCommand(executeCreateNewDiagramCommand);
            SaveDiagramCommand = new SimpleCommand(executeSaveDiagramCommand);
            LoadDiagramCommand = new SimpleCommand(executeLoadDiagramCommand);
            CloseWindowCommand = new SimpleCommand(executeCloseWindowCommand);

            //OrthogonalPathFinder is a pretty bad attempt at finding path points, it just shows you, you can swap this out with relative
            //ease if you wish just create a new IPathFinder class and pass it in right here
            ConnectorViewModel.PathFinder = new OrthogonalPathFinder();
        }

        #endregion

        #region 私有成员

        private readonly IDatabaseAccessService _databaseAccessService;
        private readonly IMessageBoxService _messageBoxService;
        private DiagramViewModel _diagramViewModel;
        private bool _isBusy;
        private List<SelectableDesignerItemViewModelBase> _itemsToRemove;

        private int? _savedDiagramId;
        private string _saveDiagramFileName;

        private List<DiagramItem> _savedDiagrams;

        private static readonly string _cacheFileDirectory =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "cache\\");

        #endregion

        #region command

        /// <summary>
        ///     删除一个流程图
        /// </summary>
        public SimpleCommand DeleteSelectedItemsCommand { get; }

        /// <summary>
        ///     创建一个流程图
        /// </summary>
        public SimpleCommand CreateNewDiagramCommand { get; }

        /// <summary>
        ///     保存流程图
        /// </summary>
        public SimpleCommand SaveDiagramCommand { get; }

        /// <summary>
        ///     加载数据
        /// </summary>
        public SimpleCommand LoadDiagramCommand { get; }

        /// <summary>
        ///     关闭窗口时
        ///     <remarks>关闭窗口时提示是否保存当前板件的在线信息</remarks>
        /// </summary>
        public SimpleCommand CloseWindowCommand { get; }

        #endregion

        #region 绑定属性
        /// <summary>
        /// 
        /// </summary>
        public ToolBoxViewModel ToolBoxViewModel { get; }

        /// <summary>
        /// 图形
        /// </summary>
        public DiagramViewModel DiagramViewModel
        {
            get => _diagramViewModel;
            set
            {
                if (_diagramViewModel != value)
                {
                    _diagramViewModel = value;
                    NotifyChanged("DiagramViewModel");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
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
        /// 
        /// </summary>
        public List<DiagramItem> SavedDiagrams
        {
            get => _savedDiagrams;
            set
            {
                if (_savedDiagrams != value)
                {
                    _savedDiagrams = value;
                    NotifyChanged("SavedDiagrams");
                    NotifyChanged("SavedDiagramFileNameCollection");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> SavedDiagramFileNameCollection
        {
            get { return _savedDiagrams.Select(a => a.FileName).ToList(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? SavedDiagramId
        {
            get => _savedDiagramId;
            set
            {
                if (_savedDiagramId != value)
                {
                    _savedDiagramId = value;
                    SavedDiagramFileName = _savedDiagrams.FirstOrDefault(a => a.Id == _savedDiagramId)?.FileName;
                    NotifyChanged("SavedDiagramId");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SavedDiagramFileName
        {
            get { return _savedDiagrams.FirstOrDefault(a => a.Id == _savedDiagramId)?.FileName; }
            set
            {
                if (_saveDiagramFileName != value)
                {
                    _saveDiagramFileName = value;
                    _savedDiagramId = _savedDiagrams.FirstOrDefault(a => a.FileName == _saveDiagramFileName)?.Id;
                    NotifyChanged("SavedDiagramFileName");
                }
            }
        }

        #endregion

        #region Command 执行

        //TODO: ctrl + z 撤销上一步的操作

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="parameter"></param>
        private void executeDeleteSelectedItemsCommand(object parameter)
        {
            _itemsToRemove = DiagramViewModel.SelectedItems;
            var connectionsToAlsoRemove =
                new List<SelectableDesignerItemViewModelBase>();

            foreach (var connector in DiagramViewModel.Items.OfType<ConnectorViewModel>())
            {
                if (ItemsToDeleteHasConnector(_itemsToRemove, connector.SourceConnectorInfo))
                    connectionsToAlsoRemove.Add(connector);

                if (ItemsToDeleteHasConnector(_itemsToRemove, (FullyCreatedConnectorInfo)connector.SinkConnectorInfo))
                    connectionsToAlsoRemove.Add(connector);
            }

            _itemsToRemove.AddRange(connectionsToAlsoRemove);
            foreach (var selectedItem in _itemsToRemove)
                DiagramViewModel.RemoveItemCommand.Execute(selectedItem);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="parameter"></param>
        private void executeCreateNewDiagramCommand(object parameter)
        {
            _itemsToRemove = new List<SelectableDesignerItemViewModelBase>();
            SavedDiagramId = null;



            DiagramViewModel = new DiagramViewModel("test");
            DiagramViewModel.CreateNewDiagramCommand.Execute(null);
        }

        /// <summary>
        /// 保存流程图
        /// </summary>
        /// <param name="parameter"></param>
        private void executeSaveDiagramCommand(object parameter)
        {
            if (!DiagramViewModel.Items.Any())
            {
                _messageBoxService.ShowError("There must be at least one item in order save a diagram");
                return;
            }

            IsBusy = true;
            var task0 = Task.Factory.StartNew(() =>
            {
                //0. 数据准备
                var diagram = new Diagram4Serialize();
                if (SavedDiagramId != null)
                {
                    diagram.Id = SavedDiagramId.Value;
                    diagram.FileName = SavedDiagramFileName + ".json";
                }

                var nodes = _diagramViewModel.Items.OfType<NodeDesignerItemViewModel>().ToList();
                var connections = _diagramViewModel.Items.OfType<ConnectorViewModel>().ToList();
                var routes = connections
                    .Where(a => a.SinkConnectorInfo is FullyCreatedConnectorInfo)
                    .Where(a => a.SourceConnectorInfo != null)
                    .Where(a => a.SinkConnectorInfo != null)
                    .Select(a => new string[2]
                    {
                        (a.SourceConnectorInfo.DataItem as NodeDesignerItemViewModel)?.Key,
                        (((FullyCreatedConnectorInfo) a.SinkConnectorInfo).DataItem as NodeDesignerItemViewModel)?.Key
                    }).ToList();

                //1. 检查
                //1.1. 不允许没有编号
                /*                if (nodes.Any(a => string.IsNullOrWhiteSpace(a.Value)))
                                {
                                    MessageBox.Show("节点必须有编号！");
                                    return -1;
                                }*/

                //1.2. 编号不能重复
                var group = nodes.GroupBy(a => a.BusinessObj + "." + a.Value);
                var sameKeys = group.Where(a => a.Count() > 1).ToList();
                if (sameKeys.Any())
                {
                    var keys = JsonHelper.ConvertToStr(sameKeys.Select(a => a.Key));
                    MessageBox.Show($"{keys} 重复");
                    return -1;
                }

                //1.3. 节点之间必须有连接
                if (nodes.Count > 1)
                {
                    var noteRoutNodes = (from a in nodes
                                         where routes.Any(b => b.Contains(a.Key)) == false
                                               && a.NodeType != "异常"
                                         select a).ToList();
                    if (noteRoutNodes.Any())
                    {
                        MessageBox.Show($"{JsonHelper.ConvertToStr(noteRoutNodes.Select(a => a.Header))} 节点之间必须连接！");
                        return -1;
                    }
                }

                //2. 节点
                foreach (var nodeDesignerItemViewModel in nodes)
                {
                    var diagramItem = new NodeDesignerItem(nodeDesignerItemViewModel.Id,
                        nodeDesignerItemViewModel.Left, nodeDesignerItemViewModel.Top,
                        nodeDesignerItemViewModel.Key, nodeDesignerItemViewModel.Value)
                    {
                        NodeType = nodeDesignerItemViewModel.NodeType,
                        BusinessObj = nodeDesignerItemViewModel.BusinessObj,
                        Desc = nodeDesignerItemViewModel.Desc
                    };
                    diagram.DiagramItems.Add(diagramItem);
                }

                //3. 路由信息
                foreach (var connectorViewModel in connections)
                {
                    if (connectorViewModel.SinkConnectorInfo is FullyCreatedConnectorInfo == false)
                        throw new Exception("connectorViewModel.SinkConnectorInfo type not FullyCreatedConnectorInfo");
                    var sinkConnector = (FullyCreatedConnectorInfo)connectorViewModel.SinkConnectorInfo;
                    var connection = new Connection(
                        connectorViewModel.Id,
                        connectorViewModel.SourceConnectorInfo.DataItem.Id,
                        getOrientationFromConnector(connectorViewModel.SourceConnectorInfo.Orientation),
                        getTypeOfDiagramItem(connectorViewModel.SourceConnectorInfo.DataItem),
                        sinkConnector.DataItem.Id,
                        getOrientationFromConnector(sinkConnector.Orientation),
                        getTypeOfDiagramItem(sinkConnector.DataItem));
                    diagram.Connections.Add(connection);
                }

                //4. 业务对象列表
                diagram.BusinessOjbCollection =
                    _diagramViewModel.BusinessObjectCollection.Select(a => a.Name).ToArray();

                //5. 保存
                _databaseAccessService.SaveDiagramView(diagram);
                if (_savedDiagrams.All(a => a.Id != diagram.Id))
                    SavedDiagrams = new List<DiagramItem>
                    {
                        new DiagramItem {Id = diagram.Id, FileName = diagram.FileName}
                    };
                IsBusy = false;
                _messageBoxService.ShowInformation($"Finished saving Diagram Id : {diagram.FileName}");
                return 1;
            });
        }

        /// <summary>
        /// 加载流程图
        /// </summary>
        /// <param name="parameter"></param>
        private void executeLoadDiagramCommand(object parameter)
        {
            IsBusy = true;
            if (SavedDiagramId == null)
            {
                _messageBoxService.ShowError("You need to select a diagram to load");
                return;
            }

            var task0 = Task.Factory.StartNew(() =>
            {
                var diagramView = _databaseAccessService.LoadDiagramView(SavedDiagramFileName);
                if (diagramView == null) return null;
                var diagramViewModel = new DiagramViewModel(diagramView.Id.ToString());

                //load diagram items
                foreach (var designerItemBase in diagramView.DiagramItems)
                {
                    var diagramItem = designerItemBase;
                    var settingsDesignerItemViewModel = new NodeDesignerItemViewModel(diagramItem, diagramViewModel);
                    diagramViewModel.Items.Add(settingsDesignerItemViewModel);
                }

                //load connection items
                foreach (var connection in diagramView.Connections)
                {
                    var sourceItem =
                        getConnectorDataItem(diagramViewModel, connection.SourceId, connection.SourceType);
                    var sourceConnectorOrientation =
                        getOrientationForConnector(connection.SourceOrientation);
                    var sourceConnectorInfo =
                        getFullConnectorInfo(connection.Id, sourceItem, sourceConnectorOrientation);

                    var sinkItem =
                        getConnectorDataItem(diagramViewModel, connection.SinkId, connection.SinkType);
                    var sinkConnectorOrientation =
                        getOrientationForConnector(connection.SinkOrientation);
                    var sinkConnectorInfo =
                        getFullConnectorInfo(connection.Id, sinkItem, sinkConnectorOrientation);

                    var connectionVm = new ConnectorViewModel(connection.Id, diagramViewModel,
                        sourceConnectorInfo, sinkConnectorInfo);
                    diagramViewModel.Items.Add(connectionVm);
                }

                // load business obj
                if (diagramView.BusinessOjbCollection != null)
                    foreach (var s in diagramView.BusinessOjbCollection)
                    {
                        diagramViewModel.BusinessObjectCollection.Add(new DiagramViewModel.BusinessObjData { Name = s });
                    }

                return diagramViewModel;
            });
            task0.ContinueWith(ant =>
            {
                DiagramViewModel = ant.Result;
                IsBusy = false;
                if (SavedDiagramId != null)
                    _messageBoxService.ShowInformation($"Finished loading Diagram Id : {SavedDiagramId.Value}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

        }

        // 关闭窗口
        private void executeCloseWindowCommand(object parameter)
        {
            createDirectory(_cacheFileDirectory);
        }

        #endregion

        #region 私有方法

        private void createDirectory(string directoryPath)
        {
            if (false == Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath); //新建文件夹   
        }

        private static FullyCreatedConnectorInfo getFullConnectorInfo(int connectorId,
            DesignerItemViewModelBase dataItem,
            ConnectorOrientation connectorOrientation)
        {
            switch (connectorOrientation)
            {
                case ConnectorOrientation.Top:
                    return dataItem.TopConnector;
                case ConnectorOrientation.Left:
                    return dataItem.LeftConnector;
                case ConnectorOrientation.Right:
                    return dataItem.RightConnector;
                case ConnectorOrientation.Bottom:
                    return dataItem.BottomConnector;

                default:
                    throw new InvalidOperationException(
                        $"Found invalid persisted Connector Orientation for Connector Id: {connectorId}");
            }
        }

        private static Type getTypeOfDiagramItem(DesignerItemViewModelBase vmType)
        {
            if (vmType is NodeDesignerItemViewModel)
                return typeof(NodeDesignerItem);

            throw new InvalidOperationException(string.Format(
                "Unknown diagram type. Currently only {0} and {1} are supported",
                typeof(NodeDesignerItem).AssemblyQualifiedName,
                typeof(NodeDesignerItemViewModel).AssemblyQualifiedName
            ));
        }

        private static DesignerItemViewModelBase getConnectorDataItem(DiagramViewModel diagramViewModel,
            int conectorDataItemId, Type connectorDataItemType)
        {
            DesignerItemViewModelBase dataItem = null;

            if (connectorDataItemType == typeof(NodeDesignerItem))
                dataItem = diagramViewModel.Items.OfType<NodeDesignerItemViewModel>()
                    .Single(x => x.Id == conectorDataItemId);

            return dataItem;
        }

        private static Orientation getOrientationFromConnector(ConnectorOrientation connectorOrientation)
        {
            var result = Orientation.None;
            switch (connectorOrientation)
            {
                case ConnectorOrientation.Bottom:
                    result = Orientation.Bottom;
                    break;
                case ConnectorOrientation.Left:
                    result = Orientation.Left;
                    break;
                case ConnectorOrientation.Top:
                    result = Orientation.Top;
                    break;
                case ConnectorOrientation.Right:
                    result = Orientation.Right;
                    break;
            }

            return result;
        }

        private static ConnectorOrientation getOrientationForConnector(Orientation persistedOrientation)
        {
            var result = ConnectorOrientation.None;
            switch (persistedOrientation)
            {
                case Orientation.Bottom:
                    result = ConnectorOrientation.Bottom;
                    break;
                case Orientation.Left:
                    result = ConnectorOrientation.Left;
                    break;
                case Orientation.Top:
                    result = ConnectorOrientation.Top;
                    break;
                case Orientation.Right:
                    result = ConnectorOrientation.Right;
                    break;
            }

            return result;
        }

        private bool ItemsToDeleteHasConnector(List<SelectableDesignerItemViewModelBase> itemsToRemove,
            FullyCreatedConnectorInfo connector)
        {
            return itemsToRemove.Contains(connector.DataItem);
        }


        private void deleteFromDatabase(DiagramItem wholeDiagramToAdjust,
            SelectableDesignerItemViewModelBase itemToDelete)
        {
            //make sure the item is removes from Diagram as well as removing them as individual items from database
            if (itemToDelete is NodeDesignerItemViewModel)
            {
                var diagramItemToRemoveFromParent = wholeDiagramToAdjust.DesignerItems
                    .Single(x => x.ItemId == itemToDelete.Id && x.ItemType == typeof(NodeDesignerItem));
                wholeDiagramToAdjust.DesignerItems.Remove(diagramItemToRemoveFromParent);
                _databaseAccessService.DeleteDesignerItem<NodeDesignerItem>(itemToDelete.Id);
            }

            if (itemToDelete is ConnectorViewModel)
            {
                wholeDiagramToAdjust.ConnectionIds.Remove(itemToDelete.Id);
                _databaseAccessService.DeleteConnection(itemToDelete.Id);
            }

            _databaseAccessService.SaveDiagram(wholeDiagramToAdjust);
        }

        #endregion
    }
}