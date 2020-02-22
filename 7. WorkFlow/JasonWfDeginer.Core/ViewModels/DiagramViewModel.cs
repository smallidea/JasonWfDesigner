// ******************************************************************
// ** Copyright：Copyright (c) 2020
// ** Project：JasonWfDesigner.Core
// ** Create Date：2020-02-21 14:31
// ** Created by：陈晓平
// ** Blog：http://smallidea.cnblogs.com
// ** Git：http://smallidea.github.com
// ** Email: smallidea@126.com
// ** Version：v 1.0
// ** Last Modified: 2020-02-21 15:54
// ** Desc：DiagramViewModel.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JasonWfDesigner.Core.UserControls;

namespace JasonWfDesigner.Core.ViewModels
{
    /// <summary>
    /// 画布
    /// </summary>
    public class DiagramViewModel : INPCBase, IDiagramViewModel
    {
        public DiagramViewModel(string key)
        {
            Key = key;
            AddItemCommand = new SimpleCommand(executeAddItemCommand);
            RemoveItemCommand = new SimpleCommand(executeRemoveItemCommand);
            ClearSelectedItemsCommand = new SimpleCommand(executeClearSelectedItemsCommand);
            CreateNewDiagramCommand = new SimpleCommand(executeCreateNewDiagramCommand);
            OpenPropertyWinCommand = new SimpleCommand(executeOpenPropertyWinCommand);
            AddNewBusinessObjCommand = new SimpleCommand(executeAddNewBusinessObjCommand);
            
            Mediator.Instance.Register(this); //???
        }

        public string Key { get; set; }
        /// <summary>创建一个新的视图</summary>
        public SimpleCommand CreateNewDiagramCommand { get; }
        /// <summary>
        /// 添加一个节点
        /// </summary>
        public SimpleCommand AddItemCommand { get; }
        /// <summary>
        /// 
        /// </summary>
        public SimpleCommand RemoveItemCommand { get; }
        /// <summary>
        /// 清除选中的
        /// </summary>
        public SimpleCommand ClearSelectedItemsCommand { get; }
        /// <summary>
        /// 打开属性设置窗口
        /// </summary>
        public SimpleCommand OpenPropertyWinCommand { get; }
        /// <summary>
        /// 
        /// </summary>
        public SimpleCommand AddNewBusinessObjCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SelectableDesignerItemViewModelBase> Items { get; } =
            new ObservableCollection<SelectableDesignerItemViewModelBase>();

        /// <summary>
        /// 
        /// </summary>
        public List<SelectableDesignerItemViewModelBase> SelectedItems
        {
            get { return Items.Where(x => x.IsSelected).ToList(); }
        }

        /// <summary>
        /// 业务对象列表
        /// </summary>
        public ObservableCollection<BusinessObjData> BusinessObjectCollection { get; set; } = 
            new ObservableCollection<BusinessObjData>();
        
        public class BusinessObjData
        {
            public string Name { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dummy"></param>
        [MediatorMessageSink("DoneDrawingMessage")]
        public void OnDoneDrawingMessage(bool dummy)
        {
            foreach (var item in Items.OfType<DesignerItemViewModelBase>()) item.ShowConnectors = false;
        }

        private void executeAddItemCommand(object parameter)
        {
            if (parameter is SelectableDesignerItemViewModelBase)
            {
                var item = (SelectableDesignerItemViewModelBase)parameter;
                item.Parent = this;
                item.Id = (Items.Any() ? Items.Max(a => a.Id) : 0) + 1;
                Items.Add(item);
            }
        }

        private void executeRemoveItemCommand(object parameter)
        {
            if (parameter is SelectableDesignerItemViewModelBase)
            {
                var item = (SelectableDesignerItemViewModelBase)parameter;
                Items.Remove(item);
            }
        }

        private void executeClearSelectedItemsCommand(object parameter)
        {
            foreach (var item in Items) item.IsSelected = false;
        }

        private void executeCreateNewDiagramCommand(object parameter)
        {
            Items.Clear();

            showPropertyWindow();
        }

        private void executeOpenPropertyWinCommand(object parameter)
        {
            showPropertyWindow();
        }

        private void executeAddNewBusinessObjCommand(object parameter)
        {
            BusinessObjectCollection.Add(new BusinessObjData());
        }

        private void showPropertyWindow()
        {
            var propertyWin = new SetPropertyWin();
            propertyWin.DataContext = this;
            propertyWin.ShowDialog();
        }
    }
}