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
// ** Desc：IDiagramViewModel.cs
// ** Log: 每一个坑都源于精心的设计！每段垃圾代码的都是“故意”的！
// ******************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using JasonWfDesigner.Core.ViewModels;

namespace JasonWfDesigner.Core
{
    public interface IDiagramViewModel
    {
        string Key { get; set; }

        SimpleCommand AddItemCommand { get; }
        SimpleCommand RemoveItemCommand { get; }
        SimpleCommand ClearSelectedItemsCommand { get; }
        List<SelectableDesignerItemViewModelBase> SelectedItems { get; }
        ObservableCollection<SelectableDesignerItemViewModelBase> Items { get; }
        ObservableCollection<DiagramViewModel.BusinessObjData> BusinessObjectCollection { get; set; }
    }
}